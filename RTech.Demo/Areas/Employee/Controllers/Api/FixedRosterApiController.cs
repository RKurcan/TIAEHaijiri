using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class FixedRosterApiController : ApiController
    {
        SRoster _rosterServices = null;
        LocalizedString _loc = null;
        SDepartment _departmentServices = null;
        SSection _sectionServices = null;
        SShift _shiftServices = null;
        SWeeklyRoster _weeklyRosterServices = null;
        SEmployee _employeeServices = null;
        string language = RiddhaSession.Language;

        public FixedRosterApiController()
        {
            _rosterServices = new SRoster();
            _loc = new LocalizedString();
            _departmentServices = new SDepartment();
            _sectionServices = new SSection();
            _shiftServices = new SShift();
            _weeklyRosterServices = new SWeeklyRoster();
            _employeeServices = new SEmployee();
        }
        [HttpGet]
        public ServiceResult<List<DepartmentVm>> GetDepartment()
        {
            int? branchId = RiddhaSession.BranchId;
            var departmentLst = (from c in _departmentServices.List().Data.Where(x => x.BranchId == branchId)
                                 select new DepartmentVm()
                                 {
                                     Id = c.Id,
                                     //Name = c.Name,
                                     //NameNp = c.NameNp,
                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                 }).ToList();

            return new ServiceResult<List<DepartmentVm>>()
            {
                Data = departmentLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<SectionVm>> GetSectionsByDepartment(string id)
        {
            string[] departments = id.Split(',');
            List<SectionVm> sections = (from c in _sectionServices.List().Data.ToList()
                                        where c.BranchId == RiddhaSession.BranchId
                                        join d in departments on c.DepartmentId equals int.Parse(d)
                                        select new SectionVm
                                            {
                                                Id = c.Id,
                                                Code = c.Code,
                                                //Name = c.Name,
                                                //NameNp = c.NameNp,
                                                Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                                BranchId = c.BranchId,
                                                DepartmentId = c.DepartmentId,

                                            }).ToList();
            return new ServiceResult<List<SectionVm>>()
            {
                Data = sections,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<FixedRosterShiftVm>> GetShifts()
        {
            var branchId = RiddhaSession.BranchId;
            var shifts = (from c in _shiftServices.List().Data.Where(x => x.BranchId == branchId)
                          select new FixedRosterShiftVm()
                          {
                              Id = c.Id,
                              //ShiftName = c.ShiftName,
                              ShiftName = language == "ne" && c.NameNp != null ? c.NameNp : c.ShiftName,
                              ShiftCode = c.ShiftCode
                          }
                          ).ToList();
            return new ServiceResult<List<FixedRosterShiftVm>>()
            {
                Data = shifts,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]

        public ServiceResult<RosterVM> Post(FixedRosterEmployeeVm model)
        {
            List<EEmployeeShitList> datatosave = new List<EEmployeeShitList>();
            int i = 0;
            foreach (var row in model.EmployeeFixedShifts)
            {

                _employeeServices.UpdateShift(row.Id, 0, row.ShiftId);

            }
            if (datatosave.Count > 0)
            {
                _employeeServices.AddEmployeeShitList(datatosave);
                Common.AddAuditTrail("8004", "7123", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 1, "Added Sucessfully");
            }
            return new ServiceResult<RosterVM>()
            {
                Status = ResultStatus.Ok,
                Message = _loc.Localize("AddedSuccess")
            };
        }

        [HttpGet]
        public ServiceResult<List<EmployeeFixedShiftModel>> RefreshRoster(string SectionIds)
        {
           string language = RiddhaSession.Language;
            var branchId = RiddhaSession.BranchId;
            var sections = (from c in SectionIds.Split(',')
                            select c.ToInt()
                               ).Distinct().ToArray();
            //var employees = _employeeServices.GetFixedRoster(sections, language).Data;
            var employees = Common.GetEmployees().Data.Where(x => x.ShiftTypeId == 0 && x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated);
            var result = (from c in _employeeServices.ListEmpShift().ToList()
                          join e in  employees on c.EmployeeId equals e.Id 
                          join d in sections on e.SectionId equals d
                          select new EmployeeFixedShiftModel()
                          {
                              Id = e.Id,
                              Code = e.Code,
                              Name = language == "ne" && e.NameNp != null ? e.NameNp : e.Name,
                              ShiftId = c.ShiftId
                          }).ToList();
            return new ServiceResult<List<EmployeeFixedShiftModel>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
    }
    public class DepartmentVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
    }

    public class SectionVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int? BranchId { get; set; }
        public int DepartmentId { get; set; }
    }

    public class FixedRosterEmployeeVm
    {
        public FixedRosterEmployeeVm()
        {
            EmployeeFixedShifts = new List<EmployeeFixedShiftModel>();
        }
        public List<EmployeeFixedShiftModel> EmployeeFixedShifts { get; set; }
    }
    public class FixedRosterShiftVm
    {
        public int Id { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }

    }
}
