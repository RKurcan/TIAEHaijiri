using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.HRM.Services.Training;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class CourseApiController : ApiController
    {
        SCourse _courseServices = null;
        SEmployee _employeeServices = null;
        SDepartment _departmentServices = null;
        LocalizedString _loc = null;

        public CourseApiController()
        {
            _courseServices = new SCourse();
            _departmentServices = new SDepartment();
            _employeeServices = new SEmployee();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<CourseGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var courseLst = (from c in _courseServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                             select new CourseGridVm()
                             {
                                 Id = c.Id,
                                 CoordinatorId = c.CoordinatorId,
                                 CoordinatorName = c.Coordinator.Name,
                                 Cost = c.Cost,
                                 Currency = Enum.GetName(typeof(Currency), c.Currency),
                                 DepartmentId = c.DepartmentId,
                                 DepartmentName = c.Coordinator.Section.Department.Name,
                                 Description = c.Description,
                                 SubVersion = c.SubVersion,
                                 Title = c.Title,
                                 Version = c.Version,
                                 BranchId = c.BranchId,
                             }).ToList();
            return new ServiceResult<List<CourseGridVm>>()
            {
                Data = courseLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ECourse> Get(int id)
        {
            ECourse course = _courseServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ECourse>()
            {
                Data = course,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ECourse> Post(ECourse model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            var result = _courseServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8006", "7126", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ECourse>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ECourse> Put(ECourse model)
        {
            var result = _courseServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8006", "7127", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ECourse>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var course = _courseServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _courseServices.Remove(course);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8006", "7128", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPost]
        public ServiceResult<List<EmployeeAutoCompleteVm>> GetEmployeeLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<EmployeeAutoCompleteVm> resultLst = new List<EmployeeAutoCompleteVm>();
            if (model != null)
            {
                var employeeLst = _employeeServices.List().Data.Where(x => x.BranchId == branchId);
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                if (searchText == "___")
                {
                    employeeLst = employeeLst.OrderBy(x => x.Name).Take(20);
                }
                else
                {
                    employeeLst = employeeLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower()));
                }
                if (employeeLst != null)
                {
                    resultLst = (from c in employeeLst
                                 select new EmployeeAutoCompleteVm()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     Name = c.Code + " - " + c.Name + " - " + (c.Mobile ?? ""),
                                 }).OrderBy(x => x.Name).ToList();
                }
            }
            return new ServiceResult<List<EmployeeAutoCompleteVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<List<DropdownViewModel>> GetDepartmentsForDropdown()
        {
            string language = RiddhaSession.Language.ToString();
            int? branchId = RiddhaSession.BranchId;
            List<DropdownViewModel> resultLst = (from c in _departmentServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = language == "ne" &&
                                                     c.NameNp != null ? c.NameNp : c.Name
                                                 }
                                                   ).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
    }
    public class CourseGridVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Version { get; set; }
        public int SubVersion { get; set; }
        public string Currency { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public int CoordinatorId { get; set; }
        public string CoordinatorName { get; set; }
        public int BranchId { get; set; }
    }
    public class EmployeeAutoCompleteVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

}
