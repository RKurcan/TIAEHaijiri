using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Areas.Office.Controllers.Api;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class WeeklyRosterApiController : ApiController
    {
        SWeeklyRoster rosterServices = null;
        LocalizedString loc = null;

        public WeeklyRosterApiController()
        {
            rosterServices = new SWeeklyRoster();
            loc = new LocalizedString();
        }
        [ActionFilter("2019")]
        public ServiceResult<RosterVM> Post(RosterVM model)
        {
            List<ERoster> rosters = new List<ERoster>();
            SDateTable service = new SDateTable();
            List<EWeeklyRoster> datatosave = new List<EWeeklyRoster>();
            int i = 0;
            foreach (var row in model.RosterRows)
            {
                rosterServices.RemoveByEmployeeId(row.EmployeeId);
                i = 0;
                foreach (var column in row.Columns)
                {

                    if (column.ShiftId != 0)
                    {
                        datatosave.Add(new EWeeklyRoster()
                        {
                            Day = (Day)column.Day,
                            EmployeeId = row.EmployeeId,
                            RosterCreationDate = System.DateTime.Now,
                            ShiftId = column.ShiftId
                        });
                    }
                    i++;
                }
            }
            if (datatosave.Count > 0)
            {
                rosterServices.AddWeeklyRosterRange(datatosave);
                Common.AddAuditTrail("2003", "2019", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 1, "Added Sucessfully");
            }
            return new ServiceResult<RosterVM>()
            {
                Status = ResultStatus.Ok,
                Message = loc.Localize("AddedSuccess")
            };
        }

        [HttpGet, ActionFilter("2038")]
        public ServiceResult<RosterVM> RefreshRoster(string SectionIds)
        {
            string language = RiddhaSession.Language;
            RosterVM vm = new RosterVM();
            SDateTable service = new SDateTable();
            var days = service.GetDaysInWeek();
            var RosterRawData = (from c in rosterServices.List().Data

                                 select c
                                     );
            //foreach (var employeeId in (from c in RosterRawData select c.EmployeeId).Distinct().ToList())
            SEmployee employeeServices = new SEmployee();
            //prepare section
            var sections = (from c in SectionIds.Split(',')
                            select c.ToInt()
                               ).Distinct().ToArray();
            //list employee
            var Employees = (from c in Common.GetEmployees().Data.Where(x => x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated)
                             join d in sections
                             on c.SectionId equals d
                             where c.ShiftTypeId == 1
                             select c
                               ).ToList();
            //prepare rosterSheet

            foreach (var employee in Employees)
            {
                vm.RosterRows.Add(new RosterRow()
                {
                    EmployeeId = employee.Id.ToInt(),
                    EmployeeName = employee.Code + "-" + (language == "ne" && employee.NameNp != null ? employee.NameNp : employee.Name),
                    Columns = (from c in days
                               join d in RosterRawData.Where(x => x.EmployeeId == employee.Id).ToList()
                               on (Day)days.IndexOf(c) equals d.Day into joined
                               from j in joined.DefaultIfEmpty()
                               select new ColumnsModel()
                               {
                                   Day = days.IndexOf(c),
                                   DayName = c,
                                   ShiftId = j == null ? 0 : j.ShiftId
                               }).ToList()
                });
            }

            return new ServiceResult<RosterVM>() { Data = vm, Status = ResultStatus.Ok };
        }
        public ServiceResult<List<EShift>> GetShifts()
        {
            var branchId = RiddhaSession.BranchId;
            return new ServiceResult<List<EShift>>() { Data = new SShift().List().Data.Where(x => x.BranchId == branchId).ToList(), Status = ResultStatus.Ok };
        }
        [HttpGet]
        public ServiceResult<List<DepartmentGridVm>> GetDepartment()
        {
            SDepartment services = new SDepartment();
            int? branchId = RiddhaSession.BranchId;
            var departmentLst = (from c in services.List().Data.Where(x => x.BranchId == branchId)
                                 select new DepartmentGridVm()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     NameNp = c.NameNp,
                                 }).ToList();

            return new ServiceResult<List<DepartmentGridVm>>()
            {
                Data = departmentLst,
                Status = ResultStatus.Ok
            };
        }
    }
}
