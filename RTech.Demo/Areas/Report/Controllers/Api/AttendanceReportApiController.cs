using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Areas.Office.Controllers.Api;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class AttendanceReportApiController : ApiController
    {
        SDateTable dateTableServices = null;
        public AttendanceReportApiController()
        {
            dateTableServices = new SDateTable();
        }
        // GET api/attendancereportapi
        public ServiceResult<List<ReportItem>> Get()
        {
            int roleId = RiddhaSession.RoleId;
            SUserRole userRoleServices = new SUserRole();
            Common common = new Common();
            var allReportItems = common.populateList();
            if (roleId != 0)
            {
                allReportItems = (from c in allReportItems
                                  join d in userRoleServices.ListActionRights().Data.Where(x => x.RoleId == roleId) on c.ActionCode equals d.MenuAction.ActionCode
                                  select c).ToList();
            }
            return new ServiceResult<List<ReportItem>>()
            {
                Data = allReportItems,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<List<DepartmentGridVm>> GetDepartments()
        {
            SDepartment services = new SDepartment();
            int? branchId = RiddhaSession.BranchId;
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
        public ServiceResult<List<DepartmentGridVm>> GetDepartmentsByBranch(string branchIds)
        {
            SDepartment services = new SDepartment();
            int roleId = RiddhaSession.RoleId;
            string[] branches = branchIds.Split(',');
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
                        //departmentLst = (from c in services.List().Data.Where(x => x.BranchId == branchId && x.Id == RiddhaSession.DepartmentId)
                        departmentLst = (from c in services.List().Data.ToList()
                                         join d in branches on c.BranchId equals int.Parse(d)
                                         where c.Id == RiddhaSession.DepartmentId
                                         select new DepartmentGridVm()
                                         {
                                             Id = c.Id,
                                             BranchId = c.BranchId,
                                             Code = c.Code,
                                             Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                             NumberOfStaff = c.NumberOfStaff
                                         }).ToList();
                        break;
                    case DataVisibilityLevel.Branch:
                        departmentLst = (from c in services.List().Data.ToList()
                                         join d in branches on c.BranchId equals int.Parse(d)
                                         select new DepartmentGridVm()
                                         {
                                             Id = c.Id,
                                             BranchId = c.BranchId,
                                             Code = c.Code,
                                             Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                             NumberOfStaff = c.NumberOfStaff
                                         }).ToList();
                        break;
                    case DataVisibilityLevel.All:
                        departmentLst = (from c in services.List().Data.ToList()
                                         join d in branches on c.BranchId equals int.Parse(d)
                                         select new DepartmentGridVm()
                                         {
                                             Id = c.Id,
                                             BranchId = c.BranchId,
                                             Code = c.Code,
                                             Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                             NumberOfStaff = c.NumberOfStaff
                                         }).ToList();
                        break;
                    case DataVisibilityLevel.ReportingHierarchy:
                        departmentLst = (from c in services.List().Data.ToList()
                                         join d in branches on c.BranchId equals int.Parse(d)
                                         select new DepartmentGridVm()
                                         {
                                             Id = c.Id,
                                             BranchId = c.BranchId,
                                             Code = c.Code,
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
                departmentLst = (from c in services.List().Data.ToList()
                                 join d in branches on c.BranchId equals int.Parse(d)
                                 select new DepartmentGridVm()
                                 {
                                     Id = c.Id,
                                     BranchId = c.BranchId,
                                     Code = c.Code,
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
        [HttpGet]
        public ServiceResult<List<SectionGridVm>> GetSectionsByDepartment(string id)
        {
            SSection sectionServices = new SSection();
            string[] departments = id.Split(',');
            List<SectionGridVm> sections = new List<SectionGridVm>();
            string language = RiddhaSession.Language.ToString();
            if (RiddhaSession.RoleId == 0)
            {
                sections = (from c in sectionServices.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).ToList()
                                //where c.BranchId == RiddhaSession.BranchId
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
        public ServiceResult<List<EmployeeGridVm>> GetAllEmployees()
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            SEmployee employeeServices = new SEmployee();
            var employee = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId)
                            select new EmployeeGridVm()
                            {
                                Id = c.Id,
                                Name = c.Name
                            }).ToList();
            return new ServiceResult<List<EmployeeGridVm>>()
            {
                Data = employee.OrderBy(x => x.Name).ToList(),
                Message = "",
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
            var empLst = Common.GetEmployees().Data;
            if (RiddhaSession.PackageId > 0 && activeInactiveMode == 0)
            {
                empLst = empLst.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            //Changes For Report Search Parameter..
            string[] sections = id.Split(',');
            if (RiddhaSession.RoleId == 0)
            {
                employee = (from c in empLst
                            join d in sections on c.SectionId equals int.Parse(d)
                            select new EmployeeGridVm
                            {
                                Id = c.Id,
                                //Name = c.Name
                                Name =c.Code+"-"+( language == "ne" && string.IsNullOrEmpty(c.NameNp) == false ? c.NameNp : c.Name),
                                EmployeeName=c.Name,
                            }).ToList();
            }
            else
            {
                DataVisibilityLevel dataVisibilityLevel = (DataVisibilityLevel)RiddhaSession.DataVisibilityLevel;
                switch (dataVisibilityLevel)
                {
                    case DataVisibilityLevel.Self:
                        employee = (from c in empLst.Where(x => x.Id == RiddhaSession.EmployeeId).ToList()
                                    select new EmployeeGridVm
                                    {
                                        Id = c.Id,
                                        //Name = c.Name
                                        Name = c.Code + "-" + (language == "ne" && string.IsNullOrEmpty(c.NameNp) == false ? c.NameNp : c.Name),
                                        EmployeeName = c.Name,
                                    }).ToList();
                        break;
                    case DataVisibilityLevel.Section:
                    case DataVisibilityLevel.Department:
                    case DataVisibilityLevel.Branch:
                    case DataVisibilityLevel.ReportingHierarchy:
                    case DataVisibilityLevel.All:
                        employee = (from c in empLst
                                    join d in sections on c.SectionId equals int.Parse(d)
                                    select new EmployeeGridVm
                                    {
                                        Id = c.Id,
                                        //Name = c.Name
                                        Name = c.Code + "-" + (language == "ne" && string.IsNullOrEmpty(c.NameNp) == false ? c.NameNp : c.Name),
                                        EmployeeName = c.Name,
                                    }).ToList();
                        break;
                    default:
                        break;
                }
            }
            return new ServiceResult<List<EmployeeGridVm>>()
            {
                Data = employee.OrderBy(x=>x.EmployeeName).ToList(),
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<string>> GetEnglishMonths()
        {
            string lang = RiddhaSession.Language;
            return new ServiceResult<List<string>>()
            {
                Data = dateTableServices.GetEnglishMonths(lang),
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<string>> GetNepaliMonths()
        {
            string lang = RiddhaSession.Language;
            return new ServiceResult<List<string>>()
            {
                Data = dateTableServices.GetNepaliMonths(lang),
                Status = ResultStatus.Ok
            };
        }
    }
    public class KendoReportViewModel : KendoPageListArguments
    {
        public string FISCALYEAR { get; set; }
        public string BranchIds { get; set; }
        public string EmpIds { get; set; }
        public string DeptIds { get; set; }
        public string SectionIds { get; set; }
        public string CourseIds { get; set; }
        public string LeaveMasterIds { get; set; }
        public string OnDate { get; set; }
        public string ToDate { get; set; }
        public int DayWise { get; set; }
        public int Year { get; set; }
        public int MonthId { get; set; }
        public int ToMonthId { get; set; }
        public int FiscalYearId { get; set; }
        public int ActiveInactiveMode { get; set; }
        public bool IncludePunchTime { get; set; }
        public bool OTV2 { get; set; }
        public int Type { get; set; }
    }


}
