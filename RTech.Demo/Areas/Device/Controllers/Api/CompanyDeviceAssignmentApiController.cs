using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.User.Entity;
using RTech.Demo.Areas.Device.Models;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace RTech.Demo.Areas.Device.Controllers.Api
{
    public class CompanyDeviceAssignmentApiController : ApiController
    {
        SCompanyDeviceAssignment companyDeviceAssignmentServices = null;
        SDevice deviceServices = null;
        SReseller resellerServices = null;
        SDeviceAssignment assignmentServices = null;
        public CompanyDeviceAssignmentApiController()
        {
            companyDeviceAssignmentServices = new SCompanyDeviceAssignment();
            deviceServices = new SDevice();
            resellerServices = new SReseller();
            assignmentServices = new SDeviceAssignment();
        }
        public ServiceResult<List<DeviceAssignmentViewModel>> Get(int pageSize = 10, int page = 1, string searchText = "")
        {
            int totalRowNum = companyDeviceAssignmentServices.List().Data.Count();
            EUser curuser = RTech.Demo.Utilities.RiddhaSession.CurrentUser ?? new Riddhasoft.User.Entity.EUser();
            var companyDeviceAssignment = companyDeviceAssignmentServices.List().Data.Where(x => x.Device.SerialNumber.Contains(searchText) && x.AssignedById == curuser.Id).OrderBy(x => x.Device.SerialNumber).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            // var eachCompanyDeviceAssignment = companyDeviceAssignment.Where(x => x.AssignedById == curuser.Id);
            List<DeviceAssignmentViewModel> resultData = (from c in companyDeviceAssignment
                                                          select new DeviceAssignmentViewModel
                                                          {
                                                              AssignOn = c.AssignedOn,
                                                              DeviceSerialNo = c.Device.SerialNumber,
                                                              Id = c.Id,
                                                              DeviceId = c.DeviceId,
                                                              Model = c.Device.Model.Name,
                                                              Company = c.Company.Name,
                                                              Status = Enum.GetName(typeof(Status), c.Device.Status)
                                                          }).ToList();
            resultData.ForEach(x => x.TotalCount = totalRowNum);
            return new ServiceResult<List<DeviceAssignmentViewModel>>()
            {
                Data = resultData,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ECompanyDeviceAssignment> Get(int id)
        {
            ECompanyDeviceAssignment companydevice = companyDeviceAssignmentServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ECompanyDeviceAssignment>()
            {
                Data = companydevice,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<ECompanyDeviceAssignment>> Post(CompanyDeviceAssignmentVm vm)
        {
            vm.CompanyDeviceMgmtLst.ForEach(x => x.AssignedOn = DateTime.Now);
            vm.CompanyDeviceMgmtLst.ForEach(x => x.AssignedById = RiddhaSession.CurrentUser.Id);
            try
            {
                int companyId = vm.CompanyDeviceMgmtLst.FirstOrDefault().CompanyId;
                var department = new SDepartment().List().Data.Where(x => x.Branch.CompanyId == companyId).FirstOrDefault();
                string CompanyCode = new SCompany().List().Data.Where(x => x.Id == companyId).ToList().FirstOrDefault().Code;
                int[] deviceIDs = vm.CompanyDeviceMgmtLst.Select(x => x.DeviceId).ToArray();
                SDevice sDevice = new SDevice();
                var devices = (from c in sDevice.List().Data.ToList()
                               join d in deviceIDs on c.Id equals d
                               select c
                               ).ToList();

                foreach (var device in devices)
                {
                    new Thread(() =>
                    {
                        DeviceGridVm admsVm = new DeviceGridVm()
                        {
                            SN = device.SerialNumber,
                            BranchCode = CompanyCode,
                            //DepartmentCode = "" ,
                            DepartmentCode = department == null ? "" : department.Code,
                            IsFaceDevice = device.IsFaceDevice,
                            IsAccessDevice = device.IsAccessDevice
                        };
                        WebrequestService webrequest = new WebrequestService(RiddhaSession.ADMSUrl);
                        var ADMSResult = webrequest.Post<string>("/Api/HomeApi/UpdateDevice", webrequest.SerializeObject<DeviceGridVm>(admsVm)).Result;

                    }).Start();
                }
            }
            catch (Exception)
            {

            }
            return companyDeviceAssignmentServices.Add(vm.CompanyDeviceMgmtLst);
        }
        public ServiceResult<ECompanyDeviceAssignment> Put([FromBody]ECompanyDeviceAssignment model)
        {
            return companyDeviceAssignmentServices.Update(model);
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var companyDevice = companyDeviceAssignmentServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return companyDeviceAssignmentServices.Remove(companyDevice);
        }

        [HttpGet]
        public ServiceResult<int> Return(int id)
        {
            EDevice device = deviceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (device != null)
            {
                device.Status = Status.Damage;
                deviceServices.Update(device);
            }
            return new ServiceResult<int>
            {
                Data = 1,
                Status = ResultStatus.Ok,
                Message = "Return Completed"
            };
        }

        [HttpGet]
        public ServiceResult<int> ReturnNew(int id)
        {
            EDevice device = deviceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (device != null)
            {
                device.Status = Status.Reseller;
                deviceServices.Update(device);
                var deleteResult = companyDeviceAssignmentServices.List().Data.Where(x => x.DeviceId == id).FirstOrDefault();
                companyDeviceAssignmentServices.Remove(deleteResult);
            }

            return new ServiceResult<int>
            {
                Data = 1,
                Status = ResultStatus.Ok,
                Message = "Return Completed"
            };
        }

        [HttpGet]
        public ServiceResult<List<EDevice>> GetDevices()
        {
            var user = RiddhaSession.CurrentUser;
            var resellerLogin = resellerServices.ListResellerLogin().Data.Where(x => x.UserId == user.Id).FirstOrDefault();
            var deviceAssignment = assignmentServices.List().Data.Where(x => x.ResellerId == resellerLogin.ResellerId).ToList();
            var alreadyAssignedDevice = companyDeviceAssignmentServices.List().Data.Where(x => x.AssignedById == resellerLogin.UserId).ToList();

            List<EDevice> resultData = (from c in deviceAssignment
                                        where !(from o in alreadyAssignedDevice
                                                select o.DeviceId)
                                               .Contains(c.DeviceId)
                                        select new EDevice()
                                        {
                                            Id = c.DeviceId,
                                            SerialNumber = c.Device.SerialNumber,
                                            ModelId = c.Device.ModelId
                                        }).ToList();
            return new ServiceResult<List<EDevice>>()
            {
                Data = resultData,
                Status = ResultStatus.Ok
            };
        }
    }
    public class CompanyDeviceAssignmentVm
    {
        public List<ECompanyDeviceAssignment> CompanyDeviceMgmtLst { get; set; }
    }
}
