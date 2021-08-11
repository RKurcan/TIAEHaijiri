
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class ManualPunchApiController : ApiController
    {
        SManualPunch manualPunchServices = null;
        SEmployee employeeServices = null;
        LocalizedString loc = null;
        public ManualPunchApiController()
        {
            manualPunchServices = new SManualPunch();
            employeeServices = new SEmployee();
            loc = new LocalizedString();
        }
        [ActionFilter("2040")]
        public ServiceResult<List<ManualPunchGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SManualPunch service = new SManualPunch();
            var manualPunchLst = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                                  select new ManualPunchGridVm()
                                  {
                                      Id = c.Id,
                                      EmployeeName = c.Employee.Name,
                                      EmployeeCode = c.Employee.Code,
                                      EmployeeId = c.EmployeeId,
                                      DateTime = c.DateTime.ToString("yyyy/MM/dd"),
                                      //Time = c.DateTime.ToString("H:mm"),
                                      Time = c.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                                      Remark = c.Remark
                                  }).ToList();

            return new ServiceResult<List<ManualPunchGridVm>>()
            {
                Data = manualPunchLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<IQueryable<EManualPunch>> GetManualPunchesFromEmpId(int empId = 0)
        {
            int? branchId = RiddhaSession.BranchId;
            var manualPunch = manualPunchServices.List().Data.Where(x => x.EmployeeId == empId);
            return new ServiceResult<IQueryable<EManualPunch>>()
            {
                Data = manualPunch,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EmpSearchViewModel> SearchEmployee(string empCode = "", int empId = 0)
        {
            EmpSearchViewModel vm = new EmpSearchViewModel();
            var empList = employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated).ToList();
            EEmployee employee = new EEmployee();
            if (!string.IsNullOrEmpty(empCode))
            {
                employee = empList.Where(x => x.Code == empCode).FirstOrDefault();
            }
            else if (empId != 0)
            {
                employee = empList.Where(x => x.Id == empId).FirstOrDefault();
            }
            if (employee != null)
            {

                vm.Id = employee.Id;
                vm.Code = employee.Code;
                vm.Name = employee.Name;
                vm.Designation = employee.Designation == null ? "" : employee.Designation.Name;
                vm.Section = employee.Section == null ? "" : employee.Section.Name;
                vm.Department = employee.Section.Department == null ? "" : employee.Section.Department.Name;
                vm.Photo = employee.ImageUrl;
            }
            return new ServiceResult<EmpSearchViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EManualPunch> Get(int id)
        {
            var manualPunch = manualPunchServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EManualPunch>()
            {
                Data = manualPunch,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("2024")]
        public ServiceResult<EManualPunch> Post(ManualPunchModel vm)
        {
            EManualPunch punchModel = new EManualPunch();
            punchModel.EmployeeId = vm.EmployeeId;
            punchModel.BranchId = RiddhaSession.BranchId;
            punchModel.DateTime = vm.DateTime.AddTicks(vm.Time.ToTimeSpan().Ticks);
            punchModel.Remark = vm.Remark;
            var result = manualPunchServices.Add(punchModel);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2005", "2024", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EManualPunch>()
            {
                Data = punchModel,
                Status = ResultStatus.Ok,
                Message = loc.Localize("AddedSuccess")
            };
        }
        [ActionFilter("2025")]
        public ServiceResult<EManualPunch> Put(ManualPunchModel vm)
        {
            EManualPunch punchModel = new EManualPunch();
            punchModel.Id = vm.Id;
            punchModel.EmployeeId = vm.EmployeeId;
            punchModel.BranchId = RiddhaSession.BranchId;
            punchModel.DateTime = vm.DateTime.AddTicks(vm.Time.ToTimeSpan().Ticks);
            punchModel.Remark = vm.Remark;
            var result = manualPunchServices.Update(punchModel);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2005", "2025", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EManualPunch>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status

            };
        }
        [ActionFilter("2026")]
        public ServiceResult<int> Delete(int id)
        {
            var manualPunch = manualPunchServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = manualPunchServices.Remove(manualPunch);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2005", "2026", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #region Kendo Grid
        [HttpPost, ActionFilter("2040")]
        public KendoGridResult<List<ManualPunchGridVm>> GetManualPunchKendoGrid(KendoPageListArguments arg)
        {
            var branchId = RiddhaSession.BranchId;
            SManualPunch service = new SManualPunch();
            IQueryable<EManualPunch> manualPunchQuery = null;
            var empQuery = Common.GetEmployees().Data;
            if (RiddhaSession.IsHeadOffice || RiddhaSession.DataVisibilityLevel == 4)
            {
                manualPunchQuery = service.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId);
            }
            else
            {
                manualPunchQuery = service.List().Data.Where(x => x.BranchId == branchId);
            }
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EManualPunch> paginatedQuery;
            switch (searchField)
            {
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = manualPunchQuery.Where(x => x.Employee.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id);
                    }
                    else
                    {
                        paginatedQuery = manualPunchQuery.Where(x => x.Employee.Name == searchValue.Trim()).OrderByDescending(x => x.Id);
                    }
                    break;
                case "Remark":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = manualPunchQuery.Where(x => x.Remark.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id);
                    }
                    else
                    {
                        paginatedQuery = manualPunchQuery.Where(x => x.Remark == searchValue.Trim()).OrderByDescending(x => x.Id);
                    }
                    break;
                default:
                    paginatedQuery = manualPunchQuery.OrderByDescending(x => x.Id);
                    break;
            }
            var manualPunchlist = (from c in manualPunchQuery.ToList()
                                   join d in empQuery on c.EmployeeId equals d.Id
                                   select new ManualPunchGridVm()
                                   {
                                       Id = c.Id,
                                       EmployeeName = c.Employee.Code + "-" + c.Employee.Name,
                                       EmployeeNameNp = c.Employee.Code + "-" + c.Employee.NameNp,
                                       EmployeeCode = c.Employee.Code,
                                       EmployeeId = c.EmployeeId,
                                       DateTime = c.DateTime.ToString("yyyy/MM/dd"),
                                       Time = c.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                                       Remark = c.Remark
                                   }).ToList();
            return new KendoGridResult<List<ManualPunchGridVm>>()
            {
                Data = manualPunchlist.OrderByDescending(x => x.Id).Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = manualPunchlist.Count(),
            };
        }
        #endregion
    }

    public class ManualPunchModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
        public String Time { get; set; }
        public string Remark { get; set; }
    }
    public class ManualPunchGridVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameNp { get; set; }
        public string EmployeeCode { get; set; }
        public string DateTime { get; set; }
        public string Time { get; set; }
        public string Remark { get; set; }
    }

}
