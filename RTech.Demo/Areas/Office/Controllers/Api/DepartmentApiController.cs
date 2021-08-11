using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class DepartmentApiController : ApiController
    {
        SDepartment departmentServices = null;
        LocalizedString loc = null;
        SDevicewiseDepartment _devicewiseDepartmentServices = null;
        SBranch branchServices = null;
        public DepartmentApiController()
        {
            departmentServices = new SDepartment();
            _devicewiseDepartmentServices = new SDevicewiseDepartment();
            loc = new LocalizedString();
            branchServices = new SBranch();
        }
        [ActionFilter("1042")]
        public ServiceResult<List<DepartmentGridVm>> Get()
        {
            SDepartment services = new SDepartment();
            int? branchId = RiddhaSession.BranchId;
            var departmentLst = (from c in services.List().Data.Where(x => x.BranchId == branchId)
                                 select new DepartmentGridVm()
                                 {
                                     Id = c.Id,
                                     BranchId = c.BranchId,
                                     Code = c.Code,
                                     Name = c.Name,
                                     NameNp = c.NameNp,
                                     NumberOfStaff = c.NumberOfStaff
                                 }).ToList();

            return new ServiceResult<List<DepartmentGridVm>>()
            {
                Data = departmentLst,
                Status = ResultStatus.Ok
            };
        }

        // GET api/departmentapi/5
        public ServiceResult<EDepartment> Get(int id)
        {
            var department = departmentServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EDepartment>()
            {
                Data = department,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        // POST api/departmentapi
        [ActionFilter("1019")]
        public ServiceResult<DepartmentVm> Post(DepartmentVm vm)
        {
            //vm.Department.BranchId = RiddhaSession.BranchId;
            var result = departmentServices.Add(vm.Department);
            if (result.Status == ResultStatus.Ok)
            {
                if (vm.Devices!=null)
                {
                    EDevicewiseDepartment model = new EDevicewiseDepartment();
                    foreach (var item in vm.Devices)
                    {
                        if (item.Checked)
                        {
                            model.DepartmentId = vm.Department.Id;
                            model.DeviceId = item.Id;
                            _devicewiseDepartmentServices.Add(model);
                        }
                    }
                    #region Update to ADMS
                    //api call
                    //string[] sns = sn.Split(',');
                    int[] deviceIDs = vm.Devices.Where(x => x.Checked).Select(x => x.Id).ToArray();
                    SCompanyDeviceAssignment sDevice = new SCompanyDeviceAssignment();
                    var devices = (from c in sDevice.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId).ToList()
                                   join d in deviceIDs on c.DeviceId equals d
                                   select c.Device
                                   ).ToList();
                    foreach (var device in devices)
                    {
                        new Thread(() =>
                        {
                            DeviceGridVm admsVm = new DeviceGridVm()
                            {
                                SN = device.SerialNumber,
                                BranchCode = RiddhaSession.BranchCode,
                                DepartmentCode = vm.Department.Code,

                            };
                            WebrequestService webrequest = new WebrequestService(RiddhaSession.ADMSUrl);
                            var ADMSResult = webrequest.Post<string>("/Api/HomeApi/UpdateDevice", webrequest.SerializeObject<DeviceGridVm>(admsVm)).Result;

                        }).Start();
                    }
                    #endregion
                    Common.AddAuditTrail("1005", "1019", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Department.Id, loc.Localize(result.Message));
                }
            }
            return new ServiceResult<DepartmentVm>()
            {
                Data = vm,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }
        // PUT api/departmentapi/5
        [ActionFilter("1020")]
        public ServiceResult<DepartmentVm> Put(DepartmentVm vm)
        {
            //vm.Department.BranchId = RiddhaSession.BranchId;
            var result = departmentServices.Update(vm.Department);
            if (result.Status == ResultStatus.Ok)
            {
                var existingDevicewiseDepartment = _devicewiseDepartmentServices.List().Data.Where(x => x.DepartmentId == result.Data.Id).ToList();
                if (existingDevicewiseDepartment.Count > 0)
                {
                    var deleteResult = _devicewiseDepartmentServices.Remove(existingDevicewiseDepartment);
                }

                EDevicewiseDepartment model = new EDevicewiseDepartment();
                foreach (var item in vm.Devices)
                {
                    if (item.Checked)
                    {
                        model.DepartmentId = vm.Department.Id;
                        model.DeviceId = item.Id;
                        _devicewiseDepartmentServices.Add(model);
                    }
                }
                Common.AddAuditTrail("1005", "1020", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Department.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<DepartmentVm>()
            {
                Data = vm,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }

        // DELETE api/departmentapi/5
        [ActionFilter("1021")]
        public ServiceResult<int> Delete(int id)
        {
            var department = departmentServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = departmentServices.Remove(department);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1005", "1021", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }
        private bool checkValidString(string value)
        {
            return !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
        }

        [HttpPost]
        public ServiceResult<List<EDepartment>> Upload()
        {
            var branch = RiddhaSession.CurrentUser.Branch;
            var request = HttpContext.Current.Request;
            List<EDepartment> DepLst = new List<EDepartment>();
            using (var package = new OfficeOpenXml.ExcelPackage(request.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    EDepartment model = new EDepartment();
                    model.BranchId = branch.Id;

                    model.Code = (workSheet.Cells[rowIterator, 1].Value ?? string.Empty).ToString();
                    if (checkValidString(model.Code) == false)
                    {
                        continue;
                    }
                    model.Name = (workSheet.Cells[rowIterator, 2].Value ?? string.Empty).ToString();
                    if (checkValidString(model.Name) == false)
                    {
                        continue;
                    }
                    model.NameNp = (workSheet.Cells[rowIterator, 3].Value ?? string.Empty).ToString();
                    model.NumberOfStaff = (workSheet.Cells[rowIterator, 4].Value ?? string.Empty).ToInt();
                    DepLst.Add(model);
                }
                var uniqueLst = DepLst.GroupBy(x => x.Code)
              .Where(g => g.Count() == 1)
              .Select(y => new { Element = y.Key, Counter = y.Count() })
              .ToList();
                var listToSave = (from c in DepLst
                                  join d in uniqueLst on c.Code equals d.Element
                                  select c).ToList();
                var result = new ServiceResult<List<EDepartment>>();
                if (listToSave.Count() > 0)
                {
                    result = departmentServices.UploadExcel(listToSave, branch.Id);
                }
                if (uniqueLst.Count() != DepLst.Count())
                {
                    return new ServiceResult<List<EDepartment>>()
                    {
                        Data = listToSave,
                        Message = listToSave.Count().ToString() + " out of " + DepLst.Count().ToString() + " Saved Successfully",
                        Status = ResultStatus.Ok
                    };
                }
                return new ServiceResult<List<EDepartment>>()
                {
                    Data = result.Data,
                    Status = result.Status,
                    Message = loc.Localize(result.Message)
                };
            }
        }

        [HttpPost]
        public KendoGridResult<List<DepartmentGridVm>> GetDepKendoGrid(KendoPageListArguments arg)
        {

            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            SDepartment departmentServices = new SDepartment();
            IQueryable<EDepartment> depQuery;
            var branch = branchServices.List().Data.Where(x => x.Id == RiddhaSession.BranchId).FirstOrDefault();
            if (branch.IsHeadOffice)
            {
                depQuery = departmentServices.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId);
            }
            else
            {
                depQuery = departmentServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId);
            }
            int totalRowNum = depQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EDepartment> paginatedQuery;
            switch (searchField)
            {
                case "BranchName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = depQuery.Where(x => x.Branch.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = depQuery.Where(x => x.Branch.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = depQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = depQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = depQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = depQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                default:
                    paginatedQuery = depQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    break;
            }
            var deplist = (from c in paginatedQuery
                           select new DepartmentGridVm()
                           {
                               Id = c.Id,
                               Code = c.Code,
                               Name = c.Name,
                               NameNp = c.NameNp,
                               NumberOfStaff = c.NumberOfStaff,
                               BranchId = c.BranchId,
                               BranchName = c.Branch.Name,
                           }).ToList();
            return new KendoGridResult<List<DepartmentGridVm>>()
            {
                Data = deplist.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = deplist.Count()
            };
        }

        [HttpGet]
        public ServiceResult<List<DeviceCheckBoxVM>> GetDevices()
        {
            List<DeviceCheckBoxVM> resultList = new List<DeviceCheckBoxVM>();
            int companyId = RiddhaSession.CompanyId;
            SCompanyDeviceAssignment companyDeviceAssignServices = new SCompanyDeviceAssignment();
            var deviceAssignedToCompanyLst = companyDeviceAssignServices.List().Data.Where(x => x.CompanyId == companyId && x.Device.Status == Status.Customer).ToList();

            foreach (var item in deviceAssignedToCompanyLst)
            {
                DeviceCheckBoxVM vm = new DeviceCheckBoxVM();
                vm.Id = item.Device.Id;
                vm.Name = item.Device.Name;
                resultList.Add(vm);
            }
            return new ServiceResult<List<DeviceCheckBoxVM>>()
            {
                Data = resultList,
                Message = "",
                Status = ResultStatus.Ok
            };
        }


        [HttpGet]
        public ServiceResult<DepartmentVm> GetDepartment(int id)
        {
            DepartmentVm vm = new DepartmentVm();
            vm.Department = departmentServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var deviceWiseDepartment = _devicewiseDepartmentServices.List().Data.Where(x => x.DepartmentId == id).ToList();

            vm.Devices = (from c in deviceWiseDepartment
                          select new DeviceCheckBoxVM()
                          {
                              Id = c.DeviceId,
                              Name = c.Device.Name,
                              Checked = true,
                          }).ToList();
            return new ServiceResult<DepartmentVm>()
            {
                Data = vm,
                Message = "",
                Status = ResultStatus.Ok
            };

        }

    }
    public class DepartmentGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public int NumberOfStaff { get; set; }
    }
    public class DepDeleteVm
    {
        public int[] Ids { get; set; }
    }

    public class DeviceCheckBoxVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
    public class DepartmentVm
    {
        public EDepartment Department { get; set; }
        public List<DeviceCheckBoxVM> Devices { get; set; }
    }
}
