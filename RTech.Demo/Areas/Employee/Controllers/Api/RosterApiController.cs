using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Areas.Office.Controllers.Api;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class RosterApiController : ApiController
    {
        SRoster rosterServices = null;
        LocalizedString loc = null;
        SSection sectionServices = null;
        public RosterApiController()
        {
            rosterServices = new SRoster();
            loc = new LocalizedString();
            sectionServices = new SSection();
        }
        [ActionFilter("2037")]
        public ServiceResult<IQueryable<ERoster>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var roster = branchId != null ? rosterServices.List().Data.Where(x => x.Employee.BranchId == branchId) : rosterServices.List().Data;
            return new ServiceResult<IQueryable<ERoster>>
            {
                Data = roster,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ERoster> Get(int id)
        {
            ERoster roster = rosterServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ERoster>()
            {
                Data = roster,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("2018")]
        public ServiceResult<RosterVM> Post(RosterVM model)
        {

            SRoster sservice = new SRoster();
            SDateTable service = new SDateTable();
            var totalDays = 0;
            var dateList = new List<EDateTable>();
            switch (RiddhaSession.OperationDate)
            {
                case "ne":
                    dateList = service.GetDaysInNepaliMonth(model.Year, model.MonthId);
                    totalDays = dateList.Count;
                    break;
                case "en":
                    dateList = service.GetDaysInEnglishMonth(model.Year, model.MonthId);
                    totalDays = dateList.Count;
                    break;
                default:
                    break;
            }
            int i = 0;
            foreach (var row in model.RosterRows)
            {
                List<ERoster> rosterList = new List<ERoster>();
                //sservice.RemoveByEmployeeId(row.EmployeeId, model.Year, model.MonthId);
                i = 0;
                foreach (var column in row.Columns)
                {

                    //if (column.ShiftId != 0)
                    //{
                    rosterList.Add(new ERoster()
                    {
                        Date = dateList[i].EngDate,
                        EmployeeId = row.EmployeeId,
                        RosterCreationDate = System.DateTime.Now,
                        ShiftId = column.ShiftId

                    });
                    //}
                    i++;
                }
                if (rosterList.Count() > 0)
                {
                    sservice.AddRange(rosterList, row.EmployeeId);
                    Common.AddAuditTrail("2002", "2018", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 1, "Dynamic Roster Added Sucessfully");
                }

            }
            return new ServiceResult<RosterVM>()
            {

                Status = ResultStatus.Ok,
                Message = loc.Localize("AddedSuccess")
            };
        }
        public ServiceResult<ERoster> Put(ERoster model)
        {
            model.Employee.BranchId = RiddhaSession.CurrentUser.BranchId;
            var result = rosterServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2002", "2018", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ERoster>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status,
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var roster = rosterServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = rosterServices.Remove(roster);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet, ActionFilter("2037")]
        public ServiceResult<RosterVM> RefreshRoster(int year, int monthId, string SectionIds)
        {
            string language = RiddhaSession.Language;
            RosterVM vm = new RosterVM();
            SDateTable service = new SDateTable();
            var totalDays = 0;
            var dateList = new List<EDateTable>();
            switch (RiddhaSession.OperationDate)
            {
                case "ne":
                    dateList = service.GetDaysInNepaliMonth(year, monthId);
                    totalDays = dateList.Count;
                    break;
                case "en":
                    dateList = service.GetDaysInEnglishMonth(year, monthId);
                    totalDays = dateList.Count;
                    break;
                default:
                    break;
            }

            List<DateTableViewModel> days = new List<DateTableViewModel>();
            for (int i = 1; i <= totalDays; i++)
            {
                DateTableViewModel dt = new DateTableViewModel();
                dt.Id = i;
                dt.NepDate = dateList[i - 1].NepDate;
                dt.EngDate = dateList[i - 1].EngDate;
                dt.DayName = Enum.GetName(typeof(DayOfWeek), dt.EngDate.DayOfWeek);
                days.Add(dt);
            }
            vm.Year = year;
            vm.MonthId = monthId;
            DateTime minDate = dateList.First().EngDate;
            DateTime maxDate = dateList.Last().EngDate;
            var RosterRawData = (from c in rosterServices.List().Data
                                 where c.Date >= minDate && c.Date <= maxDate
                                 select c
                                     ).ToList();
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
                             where c.ShiftTypeId == 2
                             select c
                               ).ToList();
            //prepare rosterSheet

            foreach (var employee in Employees)
            {
                vm.RosterRows.Add(new RosterRow()
                {
                    EmployeeId = employee.Id.ToInt(),
                    //EmployeeName = employee.Name,
                    EmployeeName = employee.Code + "-" + (language == "ne" && employee.NameNp != null ? employee.NameNp : employee.Name),
                    Columns = (from c in days
                               join d in RosterRawData.Where(x => x.EmployeeId == employee.Id).ToList()
                               on c.EngDate equals d.Date into joined
                               from j in joined.DefaultIfEmpty()
                               select new ColumnsModel()
                               {
                                   Day = c.Id,
                                   DayName = c.DayName,
                                   ShiftId = j == null ? 0 : j.ShiftId
                               }).ToList()
                });
            }

            return new ServiceResult<RosterVM>() { Data = vm, Status = ResultStatus.Ok };
        }

        [HttpGet, ActionFilter("2037")]
        public ServiceResult<RosterVM> RefreshRoster(int year, int monthId, string SectionIds, string empIds)
        {
            string language = RiddhaSession.Language;
            RosterVM vm = new RosterVM();
            SDateTable service = new SDateTable();
            var totalDays = 0;
            var dateList = new List<EDateTable>();
            switch (RiddhaSession.OperationDate)
            {
                case "ne":
                    dateList = service.GetDaysInNepaliMonth(year, monthId);
                    totalDays = dateList.Count;
                    break;
                case "en":
                    dateList = service.GetDaysInEnglishMonth(year, monthId);
                    totalDays = dateList.Count;
                    break;
                default:
                    break;
            }

            List<DateTableViewModel> days = new List<DateTableViewModel>();
            for (int i = 1; i <= totalDays; i++)
            {
                DateTableViewModel dt = new DateTableViewModel();
                dt.Id = i;
                dt.NepDate = dateList[i - 1].NepDate;
                dt.EngDate = dateList[i - 1].EngDate;
                dt.DayName = Enum.GetName(typeof(DayOfWeek), dt.EngDate.DayOfWeek);
                days.Add(dt);
            }
            vm.Year = year;
            vm.MonthId = monthId;
            DateTime minDate = dateList.First().EngDate;
            DateTime maxDate = dateList.Last().EngDate;
            var RosterRawData = (from c in rosterServices.List().Data
                                 where c.Date >= minDate && c.Date <= maxDate
                                 select c
                                     ).ToList();
            //foreach (var employeeId in (from c in RosterRawData select c.EmployeeId).Distinct().ToList())
            SEmployee employeeServices = new SEmployee();
            //prepare section
            var sections = (from c in SectionIds.Split(',')
                            select c.ToInt()
                               ).Distinct().ToArray();
            
            //list employee
            List<EEmployee> Employees = new List<EEmployee>();
            if (empIds== null)
            {
                
                    Employees = (from c in Common.GetEmployees().Data.Where(x => x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated)
                                 join d in sections
                                 on c.SectionId equals d
                                 where c.ShiftTypeId == 2
                                 select c
                                   ).ToList();
            }
            else
            {
                var emps = (from c in empIds.Split(',')
                            select c.ToInt()
                               ).Distinct().ToArray();
                var data = Common.GetEmployees().Data.Where(x => x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated);
                Employees = (from c in data
                             join d in emps
                             on c.Id equals d
                             where c.ShiftTypeId == 2
                             select c
                                   ).ToList();
            }
            //prepare rosterSheet

            foreach (var employee in Employees)
            {
                vm.RosterRows.Add(new RosterRow()
                {
                    EmployeeId = employee.Id.ToInt(),
                    //EmployeeName = employee.Name,
                    EmployeeName = employee.Code + "-" + (language == "ne" && employee.NameNp != null ? employee.NameNp : employee.Name),
                    Columns = (from c in days
                               join d in RosterRawData.Where(x => x.EmployeeId == employee.Id).ToList()
                               on c.EngDate equals d.Date into joined
                               from j in joined.DefaultIfEmpty()
                               select new ColumnsModel()
                               {
                                   Day = c.Id,
                                   DayName = c.DayName,
                                   ShiftId = j == null ? 0 : j.ShiftId
                               }).ToList()
                });
            }

            return new ServiceResult<RosterVM>() { Data = vm, Status = ResultStatus.Ok };
        }
        [HttpPost]
        public ServiceResult<object> Upload(int Year, int MonthId)
        {
            var branchId = RiddhaSession.BranchId ?? 0;
            var request = HttpContext.Current.Request;
            var empQuery = new SEmployee().List().Data.Where(x => x.BranchId == branchId);
            var shiftQuery = new SShift().List().Data.Where(x => x.BranchId == branchId);
            using (var package = new OfficeOpenXml.ExcelPackage(request.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                int empId = 0;
                for (int rowIterator = 4; rowIterator <= noOfRow; rowIterator++)
                {
                    List<ERoster> rosterLst = new List<ERoster>();
                    empId = getEmpIdFromCodeAndName(empQuery, (workSheet.Cells[rowIterator, 1].Value ?? string.Empty).ToString(), (workSheet.Cells[rowIterator, 2].Value ?? string.Empty).ToString());
                    if (empId == 0)
                    {
                        continue;
                    }
                    SRoster sservice = new SRoster();
                    SDateTable service = new SDateTable();
                    var totalDays = 0;
                    var dateList = new List<EDateTable>();
                    switch (RiddhaSession.OperationDate)
                    {
                        case "ne":
                            dateList = service.GetDaysInNepaliMonth(Year, MonthId);
                            totalDays = dateList.Count;
                            break;
                        case "en":
                            dateList = service.GetDaysInEnglishMonth(Year, MonthId);
                            totalDays = dateList.Count;
                            break;
                        default:
                            break;
                    }
                    //sservice.RemoveByEmployeeId(empId, Year, MonthId);
                    for (int i = 0; i < noOfCol - 2; i++)
                    {
                        int shiftId = 0;
                        shiftId = getshiftIdFromCode(shiftQuery, (workSheet.Cells[rowIterator, i + 3].Value ?? string.Empty).ToString());
                        int day = (workSheet.Cells[2, i + 3].Value ?? string.Empty).ToString().ToInt();
                        if (day != 0 && totalDays >= day)
                        {
                            rosterLst.Add(new ERoster()
                            {
                                Date = dateList[day - 1].EngDate,
                                EmployeeId = empId,
                                RosterCreationDate = System.DateTime.Now,
                                ShiftId = shiftId
                            });
                        }
                    }
                    if (rosterLst.Count() > 0)
                    {
                        sservice.AddRange(rosterLst, empId);
                    }
                }
            }
            return new ServiceResult<object>()
            {
                Status = ResultStatus.Ok,
                Message = "Uploaded Successfully"
            };
        }

        private int getshiftIdFromCode(IQueryable<EShift> shiftQuery, string code)
        {
            var shift = shiftQuery.Where(x => x.ShiftCode.ToLower() == code.ToLower()).FirstOrDefault() ?? new EShift();
            return shift.Id;
        }

        private int getEmpIdFromCodeAndName(IQueryable<EEmployee> empQuery, string code, string name)
        {
            var emp = empQuery.Where(x => x.Code.ToLower() == code.ToLower() && x.Name.ToLower() == name.Trim().ToLower()).FirstOrDefault() ?? new EEmployee();
            return emp.Id;
        }
        [HttpGet]
        public ServiceResult<List<ShiftDropdownVm>> GetShifts()
        {
            string language = RiddhaSession.Language;
            var branchId = RiddhaSession.BranchId;
            var sService = new SShift();

            var shifts = (from c in sService.List().Data
                          where c.BranchId == branchId
                          select new ShiftDropdownVm
                          {
                              Id = c.Id,
                              //ShiftName = c.ShiftName,
                              ShiftName = language == "ne" && c.NameNp != null ? c.NameNp : c.ShiftName,
                              ShiftCode = c.ShiftCode
                          }).ToList();
            //shifts.ForEach(x => x.Branch = null);
            //return new ServiceResult<List<EShift>>() { Data = new SShift().List().Data.Where(x => x.BranchId == branchId).ToList(), Status = ResultStatus.Ok };
            return new ServiceResult<List<ShiftDropdownVm>>()
            {
                Data = shifts,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<SectionGridVm>> GetSectionsByDepartment(string id)
        {
            SSection sectionServices = new SSection();
            string[] departments = id.Split(',');
            List<SectionGridVm> sections = new List<SectionGridVm>();
            string language = RiddhaSession.Language.ToString();
            if (RiddhaSession.RoleId == 0)
            {
                sections = (from c in sectionServices.List().Data.ToList()
                            join d in departments on c.DepartmentId equals int.Parse(d)
                            select new SectionGridVm
                            {
                                Id = c.Id,
                                Code = c.Code,
                                //Name = c.Name,
                                //NameNp = c.NameNp,
                                Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                BranchId = c.BranchId,
                                DepartmentId = c.DepartmentId,
                            }).ToList();
            }
            else
            {
                DataVisibilityLevel dataVisibilityLevel = (DataVisibilityLevel)RiddhaSession.DataVisibilityLevel;
                switch (dataVisibilityLevel)
                {
                    case DataVisibilityLevel.Self:
                    case DataVisibilityLevel.Section:
                        sections = (from c in sectionServices.List().Data.ToList()
                                    where c.BranchId == RiddhaSession.BranchId && c.Id == RiddhaSession.SectionId
                                    select new SectionGridVm
                                    {
                                        Id = c.Id,
                                        Code = c.Code,
                                        //Name = c.Name,
                                        //NameNp = c.NameNp,
                                        Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                        BranchId = c.BranchId,
                                        DepartmentId = c.DepartmentId,

                                    }).ToList();
                        break;
                    case DataVisibilityLevel.Department:
                    case DataVisibilityLevel.Branch:
                    case DataVisibilityLevel.ReportingHierarchy:
                    case DataVisibilityLevel.All:
                        sections = (from c in sectionServices.List().Data.ToList()
                                    where c.BranchId == RiddhaSession.BranchId
                                    join d in departments on c.DepartmentId equals int.Parse(d)
                                    select new SectionGridVm
                                    {
                                        Id = c.Id,
                                        Code = c.Code,
                                        //Name = c.Name,
                                        //NameNp = c.NameNp,
                                        Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                        BranchId = c.BranchId,
                                        DepartmentId = c.DepartmentId,
                                    }).ToList();
                        break;
                    default:
                        break;
                }
            }


            return new ServiceResult<List<SectionGridVm>>()
            {
                Data = sections,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<EmployeeGridVm>> GetEmployeeBySection(string id, int activeInactiveMode)
        {
            int? branchId = RiddhaSession.BranchId;
            string language = RiddhaSession.Language.ToString();
            SEmployee employeeServices = new SEmployee();
            List<EmployeeGridVm> employee = new List<EmployeeGridVm>();
            List<EEmployee> empLst = new List<EEmployee>();


            //Changes For Report Search Parameter..
            string[] sections = id.Split(',');
            if (RiddhaSession.RoleId == 0)
            {
                empLst = employeeServices.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).ToList();
                if (RiddhaSession.PackageId > 0 && activeInactiveMode == 0)
                {
                    empLst = empLst.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
                }
                employee = (from c in empLst
                            join d in sections on c.SectionId equals int.Parse(d)
                            select new EmployeeGridVm
                            {
                                Id = c.Id,
                                //Name = c.Name
                                Name = c.Code + "-" + (language == "ne" && c.NameNp != null ? c.NameNp : c.Name)
                            }).ToList();
            }
            else
            {
                empLst = employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
                DataVisibilityLevel dataVisibilityLevel = (DataVisibilityLevel)RiddhaSession.DataVisibilityLevel;
                switch (dataVisibilityLevel)
                {
                    case DataVisibilityLevel.Self:
                        employee = (from c in empLst.Where(x => x.Id == RiddhaSession.EmployeeId).ToList()
                                    select new EmployeeGridVm
                                    {
                                        Id = c.Id,
                                        //Name = c.Name
                                        Name = c.Code + "-" + (language == "ne" && c.NameNp != null ? c.NameNp : c.Name)
                                    }).ToList();
                        break;
                    case DataVisibilityLevel.Section:
                    case DataVisibilityLevel.Department:
                    case DataVisibilityLevel.Branch:
                    case DataVisibilityLevel.All:
                        employee = (from c in empLst
                                    join d in sections on c.SectionId equals int.Parse(d)
                                    select new EmployeeGridVm
                                    {
                                        Id = c.Id,
                                        //Name = c.Name
                                        Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                    }).ToList();
                        break;
                    case DataVisibilityLevel.ReportingHierarchy:
                        employee = (from c in Common.GetEmployees().Data
                                    select new EmployeeGridVm()
                                    {
                                        Id = c.Id,
                                        Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                    }).ToList();
                        break;
                    default:
                        break;
                }
            }
            return new ServiceResult<List<EmployeeGridVm>>()
            {
                Data = employee,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<List<DepartmentGridVm>> GetDepartments(int branchId)
        {
            SDepartment services = new SDepartment();
            //int? branchId = RiddhaSession.BranchId;
            int roleId = RiddhaSession.RoleId;
            string language = RiddhaSession.Language.ToString();

            var departmentLst = new List<DepartmentGridVm>();


            if (roleId > 0)
            {
                DataVisibilityLevel dataVisibilityLevel = (DataVisibilityLevel)RiddhaSession.DataVisibilityLevel;
                switch (dataVisibilityLevel)
                {
                    case DataVisibilityLevel.Self:
                    case DataVisibilityLevel.Section:
                    case DataVisibilityLevel.Department:
                        departmentLst = (from c in services.List().Data.Where(x => x.BranchId == branchId && x.Id == RiddhaSession.DepartmentId)
                                         select new DepartmentGridVm()
                                         {
                                             Id = c.Id,
                                             BranchId = c.BranchId,
                                             Code = c.Code,
                                             //Name = c.Name,
                                             //NameNp = c.NameNp,
                                             Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                             NumberOfStaff = c.NumberOfStaff
                                         }).ToList();
                        break;
                    case DataVisibilityLevel.Branch:
                    case DataVisibilityLevel.ReportingHierarchy:
                    case DataVisibilityLevel.All:
                        departmentLst = (from c in services.List().Data.Where(x => x.BranchId == branchId)
                                         select new DepartmentGridVm()
                                         {
                                             Id = c.Id,
                                             BranchId = c.BranchId,
                                             Code = c.Code,
                                             //Name = c.Name,
                                             //NameNp = c.NameNp,
                                             Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                             NumberOfStaff = c.NumberOfStaff
                                         }).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                departmentLst = (from c in services.List().Data.Where(x => x.BranchId == branchId)
                                 select new DepartmentGridVm()
                                 {
                                     Id = c.Id,
                                     BranchId = c.BranchId,
                                     Code = c.Code,
                                     //Name = c.Name,
                                     //NameNp = c.NameNp,
                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                     NumberOfStaff = c.NumberOfStaff
                                 }).ToList();
            }

            return new ServiceResult<List<DepartmentGridVm>>()
            {
                Data = departmentLst,
                Status = ResultStatus.Ok
            };
        }

    }
}
public class RosterViewModel
{
    public List<DateTableViewModel> Days { get; set; }
    public List<EShift> Shifts { get; set; }
}
public class CheckboxModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Checked { get; set; }
}
public class DateTableViewModel
{
    public int Id { get; set; }
    public string NepDate { get; set; }
    public DateTime EngDate { get; set; }
    public string DayName { get; set; }
}
public class ExcelRosterVm
{
    public int MonthId { get; set; }
    public int Year { get; set; }
    public int[] Sections { get; set; }
}
#region viewmodel new

public class RosterVM
{
    public RosterVM()
    {
        RosterRows = new List<RosterRow>();
    }
    public int Year { get; set; }
    public int MonthId { get; set; }
    public List<RosterRow> RosterRows { get; set; }



}
public class RosterRow
{
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeName { get; set; }
    public List<ColumnsModel> Columns { get; set; }
}
public class ColumnsModel
{
    public int Day { get; set; }
    public int ShiftId { get; set; }
    public string ShiftCode { get; set; }
    public string DayName { get; set; }
}

#endregion

public class ShiftDropdownVm
{
    public int Id { get; set; }
    public string ShiftName { get; set; }
    public string ShiftCode { get; set; }
}


