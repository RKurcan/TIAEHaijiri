using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using static RTech.Demo.Utilities.WDMS;

namespace RTech.Demo.Areas.Device.Controllers.Api
{
    public class DeviceApiController : ApiController
    {
        SDevice deviceServices = null;
        LocalizedString loc = null;
        public DeviceApiController()
        {
            loc = new LocalizedString();
            deviceServices = new SDevice();
        }

        //[HttpGet]
        //public bool CheckDuplicateSerialNo(int modelId, string serialNo)
        //{
        //    return deviceServices.List().Data.Where(x => x.ModelId == modelId && x.SerialNumber == serialNo).Any();
        //}
        public ServiceResult<List<DeviceGridViewModel>> Get(int pageSize = 10, int page = 1, string searchText = "")
        {
            int totalRowNum = deviceServices.List().Data.Count();
            var devices = deviceServices.List().Data.Where(x => x.SerialNumber.Contains(searchText)).OrderBy(x => x.SerialNumber).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            if (devices.Count > 0)
            {
                var resultData = (from m in devices
                                  select new DeviceGridViewModel()
                                  {
                                      Id = m.Id,
                                      Model = m.Model == null ? "" : m.Model.Name,
                                      SerialNumber = m.SerialNumber,
                                      DeviceTypeName = Enum.GetName(typeof(DeviceType), m.DeviceType),
                                      StatusName = Enum.GetName(typeof(Status), m.Status)
                                  }).ToList();
                resultData.ForEach(x => x.TotalCount = totalRowNum);

                return new ServiceResult<List<DeviceGridViewModel>>()
                {
                    Data = resultData,
                    Status = ResultStatus.Ok
                };
            }
            else
            {
                return new ServiceResult<List<DeviceGridViewModel>>()
                {
                    Data = new List<DeviceGridViewModel>(),
                    Status = ResultStatus.Ok
                };
            }
        }

        [HttpPost]
        public KendoGridResult<List<DeviceGridViewModel>> GetOwnerDeviceKendoGrid(KendoPageListArguments arg)
        {
            SDevice deviceServices = new SDevice();
            IQueryable<EDevice> deviceQuery;
            deviceQuery = deviceServices.List().Data;
            int totalRowNum = deviceQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EDevice> paginatedQuery;
            switch (searchField)
            {
                case "Model":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = deviceQuery.Where(x => x.Model.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id);
                    }
                    else
                    {
                        paginatedQuery = deviceQuery.Where(x => x.Model.Name == searchValue.Trim()).OrderByDescending(x => x.Id);
                    }
                    break;
                case "SerialNumber":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = deviceQuery.Where(x => x.SerialNumber.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id);
                    }
                    else
                    {
                        paginatedQuery = deviceQuery.Where(x => x.SerialNumber == searchValue.Trim()).OrderByDescending(x => x.Id);
                    }
                    break;
                default:
                    paginatedQuery = deviceQuery.OrderByDescending(x => x.Id);
                    break;
            }
            var list = (from m in paginatedQuery.ToList()
                        select new DeviceGridViewModel()
                        {
                            Id = m.Id,
                            ModelId = m.ModelId,
                            Model = m.Model == null ? "" : m.Model.Name,
                            SerialNumber = m.SerialNumber,
                            DeviceTypeName = Enum.GetName(typeof(DeviceType), m.DeviceType),
                            StatusName = Enum.GetName(typeof(Status), m.Status),
                            DeviceType = m.DeviceType,
                            Status = m.Status,
                        }).ToList();
            return new KendoGridResult<List<DeviceGridViewModel>>()
            {
                Data = list.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = list.Count()
            };
        }

        //get devices by each company to update name and ip address
        [HttpGet, ActionFilter("1039")]
        public ServiceResult<List<CompanyDeviceGridVm>> GetCompanyDevice()
        {
            List<CompanyDeviceGridVm> resultLst = new List<CompanyDeviceGridVm>();
            SCompanyDeviceAssignment companyDeviceAssignServices = new SCompanyDeviceAssignment();
            int companyId = RiddhaSession.CompanyId;
            var deviceAssignedToCompanyLst = companyDeviceAssignServices.List().Data.Where(x => x.CompanyId == companyId && x.Device.Status == Status.Customer).ToList();

            WdmsData.WdmsEntities wdmsdb = new WdmsData.WdmsEntities();
            // var devices = wdmsdb.iclock.Where(x => x.company_id == null).ToList();

            foreach (var item in deviceAssignedToCompanyLst)
            {
                CompanyDeviceGridVm vm = new CompanyDeviceGridVm();
                #region WDMS DB Region
                if (item.Device.DeviceType == DeviceType.ADMS)
                {
                    var device = (from c in wdmsdb.iclock
                                  select new
                                  {
                                      SN = c.SN,
                                      IPAddress = c.IPAddress,
                                      LastActivity = c.LastActivity
                                  }
                               ).Where(x => x.SN == item.Device.SerialNumber).FirstOrDefault();
                    if (device != null)
                    {
                        //vm.IsOnline = (System.DateTime.Now - DateTime.Parse(device.LastActivity.ToString())).TotalMinutes < 30;
                        vm.IpAddress = device.IPAddress;
                    }
                    else
                    {
                        vm.IpAddress = item.Device.IpAddress;
                    }
                }
                else
                {
                    vm.IpAddress = item.Device.IpAddress;
                }
                #endregion

                vm.IpAddress = item.Device.IpAddress;
                vm.Id = item.DeviceId;
                vm.Name = item.Device.Name;
                vm.SerialNumber = item.Device.SerialNumber;
                vm.Type = item.Device.DeviceType.ToString();
                //vm.IsOnline = getOnlineStatus(item.DeviceId);
                vm.ModelName = item.Device.Model.Name;
                resultLst.Add(vm);
            }
            return new ServiceResult<List<CompanyDeviceGridVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost, ActionFilter("1039")]
        public KendoGridResult<List<CompanyDeviceGridVm>> GetKendoGrid(KendoPageListArguments arg)
        {
            List<CompanyDeviceGridVm> resultLst = new List<CompanyDeviceGridVm>();
            SCompanyDeviceAssignment companyDeviceAssignServices = new SCompanyDeviceAssignment();
            int companyId = RiddhaSession.CompanyId;
            var deviceAssignedToCompanyLst = companyDeviceAssignServices.List().Data.Where(x => x.CompanyId == companyId && x.Device.Status == Status.Customer).ToList();
            List<DeviceGridVm> list = new List<DeviceGridVm>();
            WebrequestService webrequest = new WebrequestService(RiddhaSession.ADMSUrl);
            try
            {
                list = webrequest.Get<List<RTech.Demo.Models.DeviceGridVm>>("/api/homeapi/getdevicebybranch?code=" + RiddhaSession.BranchCode).Result;
            }
            catch (Exception ex)
            {

                list = new List<DeviceGridVm>();
            }
            
            //var 
            //list = list ?? new 
            foreach (var item in deviceAssignedToCompanyLst)
            {
                CompanyDeviceGridVm vm = new CompanyDeviceGridVm();
                #region WDMS DB Region
                SDevice sdevice = new SDevice();
                item.Device = sdevice.Find(item.DeviceId);
                Log.SytemLog(""+(item.Device==null).ToString());
                vm.IpAddress = item.Device.IpAddress;
                vm.Id = item.DeviceId;
                vm.Name = item.Device.Name;
                vm.SerialNumber = item.Device.SerialNumber;
                vm.Type = item.Device.DeviceType.ToString();
                vm.DeviceImage = item.Device.Model.ImageURL;
                if (item.Device.DeviceType == DeviceType.ADMS)
                {
                    RTech.Demo.Models.DeviceGridVm device = (from c in list
                                  select c
                               ).Where(x => x.SN == item.Device.SerialNumber).FirstOrDefault();
                    if (device != null)
                    {
                        vm.Status = device.DeviceStatus;
                        vm.DevFuns = device.DevFuns;
                        vm.FaceCount = device.FaceCount;
                        vm.FPCount = device.FPCount;
                        vm.FwVersion = device.FirmwareVersion;
                        vm.TransCount = device.TransCount;
                        vm.UserCount = device.UserCount;
                        vm.IpAddress = device.IP;
                        vm.LastActivity = device.LastActivity;
                        vm.ModelName = device.DeviceModel;
                    }
                    else
                    {
                        vm.IpAddress = item.Device.IpAddress;
                        vm.Status = "Not Authorized";
                    }
                }
                else
                {
                    vm.IpAddress = item.Device.IpAddress;
                    vm.Id = item.DeviceId;
                    vm.Name = item.Device.Name;
                    vm.SerialNumber = item.Device.SerialNumber;
                    vm.Type = item.Device.DeviceType.ToString();
                    //vm.IsOnline = getOnlineStatus(item.DeviceId);
                    vm.Status = "Non Adms Devices";
                    vm.ModelName = item.Device.Model.Name;
                }
                #endregion
                resultLst.Add(vm);
            }
            return new KendoGridResult<List<CompanyDeviceGridVm>>()
            {
                Data = resultLst.Skip(arg.Skip).Take(arg.Take).ToList(),
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = resultLst.Count,
            };
        }

        private bool getOnlineStatus(int deviceId)
        {
            return true;
        }

        [HttpGet]
        public ServiceResult<List<EDevice>> GetModelForResellerAssign()
        {
            deviceServices = new SDevice();

            var data = (from c in deviceServices.List().Data.Where(x => x.Status == Status.New).ToList()
                        select new EDevice()
                        {
                            CheckInOutIndex = c.CheckInOutIndex,
                            Status = c.Status,
                            Company_Id = c.Company_Id,
                            DeviceType = c.DeviceType,
                            Id = c.Id,
                            IpAddress = c.IpAddress,
                            LastActivity = c.LastActivity,
                            ModelId = c.ModelId,
                            Name = c.Name,
                            SerialNumber = c.SerialNumber,
                            IsAccessDevice = c.IsAccessDevice,
                            IsFaceDevice = c.IsFaceDevice,
                        }).ToList();
            return new ServiceResult<List<EDevice>>()
            {
                Data = data,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<IQueryable<EDevice>> GetModelForCustomerAssign()
        {
            return new ServiceResult<IQueryable<EDevice>>()
            {
                Data = deviceServices.List().Data.Where(x => x.Status == Status.Reseller),
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EDevice> Get(int id)
        {
            EDevice devices = deviceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EDevice>()
            {
                Data = devices,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public bool CheckDuplicateSNo(string serialNumber)
        {
            return deviceServices.List().Data.Where(x => x.SerialNumber == serialNumber).Any();
        }
        //[HttpGet]
        //public bool CheckDuplicateSerialNo(int modelId, string serialNo)
        //{
        //    return deviceServices.List().Data.Where(x => x.ModelId == modelId && x.SerialNumber == serialNo).Any();
        //}

        public ServiceResult<List<EDevice>> Post(DeviceViewmodel vm)
        {
            if (vm.Devices.Count() != 0)
            {
                var device = vm.Devices.FirstOrDefault();
                var model = new SModel().List().Data.Where(x => x.Id == device.ModelId).FirstOrDefault();
                foreach (var item in vm.Devices)
                {
                    if (model.IsFaceDevice)
                    {
                        item.IsFaceDevice = true;
                    };
                    if (model.IsAccessDevice)
                    {
                        item.IsAccessDevice = true;
                    }
                }
            }
            var result = deviceServices.AddRange(vm.Devices);
            return new ServiceResult<List<EDevice>>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EDevice> Put(EDevice model)
        {

            var result = deviceServices.Update(model);

            return new ServiceResult<EDevice>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = ResultStatus.Ok
            };
        }
        [HttpPut, ActionFilter("1014")]
        public ServiceResult<bool> UpdateCompanyDevice(DeviceViewmodel vm)
        {
            LocalizedString loc = new LocalizedString();
            var result = deviceServices.UpdateCompanyDevice(vm.Devices);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1002", "1014", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, RiddhaSession.UserId, loc.Localize(result.Message));
            }
            return new ServiceResult<bool>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }

        [HttpPut, ActionFilter("1014")]
        public ServiceResult<bool> UpdateDevice(CompanyDeviceGridVm vm)
        {
            var device = deviceServices.List().Data.Where(x => x.Id == vm.Id).FirstOrDefault();
            if (device != null)
            {
                device.Name = vm.Name;
                var result = deviceServices.UpdateDevice(device);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("1002", "1014", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, RiddhaSession.UserId, loc.Localize(result.Message));
                }
                return new ServiceResult<bool>()
                {
                    Data = result.Data,
                    Status = result.Status,
                    Message = loc.Localize(result.Message)
                };
            }
            return new ServiceResult<bool>()
            {
                Data = false,
                Message = "Process error.",
                Status = ResultStatus.processError,
            };

        }

        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var device = deviceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (device.Status != 0)
            {
                return new ServiceResult<int>()
                {
                    Data = 0,
                    Message = "Only new device can be deleted.",
                    Status = ResultStatus.processError,
                };
            }
            return deviceServices.Remove(device);
        }
        [HttpPost]
        public ServiceResult<List<EDevice>> Upload(int modelId, int quantity)
        {
            var request = HttpContext.Current.Request;
            List<EDevice> deviceLst = new List<EDevice>();
            using (var package = new OfficeOpenXml.ExcelPackage(request.InputStream))
            {

                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                var range = workSheet.Cells[1, 1, 1, noOfCol];
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    EDevice device = new EDevice();
                    device.Company_Id = null;
                    device.ModelId = modelId;
                    device.Status = Status.New;
                    device.DeviceType = (DeviceType)workSheet.Cells[rowIterator, 1].Value.ToInt();
                    device.SerialNumber = (workSheet.Cells[rowIterator, 2].Value ?? string.Empty).ToString();
                    deviceLst.Add(device);
                }

                if (deviceLst.Count() != quantity)
                {
                    return new ServiceResult<List<EDevice>>()
                    {
                        Data = null,
                        Status = ResultStatus.processError,
                        Message = "Device count does not match the quantity"
                    };
                }
                var duplicateLst = deviceLst.GroupBy(x => x.SerialNumber)
              .Where(g => g.Count() > 1)
              .Select(y => new { Element = y.Key, Counter = y.Count() })
              .ToList();
                if (duplicateLst.Count > 0)
                {
                    return new ServiceResult<List<EDevice>>()
                    {
                        Data = new List<EDevice>(),
                        Status = ResultStatus.processError,
                        Message = "There are devices with duplicate serial number please check and upload again"
                    };
                }
            }
            return new ServiceResult<List<EDevice>>()
            {
                Data = deviceLst,
                Status = ResultStatus.Ok,
                Message = "Devices are imported successfully!!verify and save"
            };
        }

    }

    public class DeviceViewmodel
    {
        public List<EDevice> Devices { get; set; }
    }
    public class DeviceGridViewModel
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string DeviceTypeName { get; set; }
        public DeviceType DeviceType { get; set; }
        public Status Status { get; set; }
        public string StatusName { get; set; }
        public int TotalCount { get; set; }
    }
    public class CompanyDeviceGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string ModelName { get; set; }
        public string DevFuns { get; set; }
        public int UserCount { get; set; }
        public int FaceCount { get; set; }
        public int TransCount { get; set; }
        public int FPCount { get; set; }
        public string FwVersion { get; set; }
        public string LastActivity { get; set; }
        public string DeviceImage { get; set; }
    }

}
