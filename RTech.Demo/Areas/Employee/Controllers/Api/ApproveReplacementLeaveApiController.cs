using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using RTech.Demo.Areas.Office.Controllers.Api;
using RTech.Demo.Areas.Report.Controllers.Api;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class ApproveReplacementLeaveApiController : ApiController
    {
        SDateTable _dateTableServices = null;
        SLeaveMaster _leaveServices = null;
        SReplacementLeaveBalance _replacementLeaveBalance = null;
        LocalizedString _loc = null;
        string language = RiddhaSession.Language.ToString();
        public ApproveReplacementLeaveApiController()
        {
            _dateTableServices = new SDateTable();
            _replacementLeaveBalance = new SReplacementLeaveBalance();
            _leaveServices = new SLeaveMaster();
            _loc = new LocalizedString();
        }

        [HttpGet]
        public ServiceResult<List<string>> GetNepaliMonths()
        {
            string lang = RiddhaSession.Language;
            return new ServiceResult<List<string>>()
            {
                Data = _dateTableServices.GetNepaliMonths(lang),
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<string>> GetEnglishMonths()
        {
            string lang = RiddhaSession.Language;
            return new ServiceResult<List<string>>()
            {
                Data = _dateTableServices.GetEnglishMonths(lang),
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
                sections = (from c in sectionServices.List().Data.ToList()
                            join d in departments on c.DepartmentId equals int.Parse(d)
                            select new SectionGridVm
                            {
                                Id = c.Id,
                                Code = c.Code,
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
            SEmployee employeeServices = new SEmployee();
            List<EmployeeGridVm> employee = new List<EmployeeGridVm>();
            var empLst = Common.GetEmployees().Data;
            //var empLst = employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList();
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
                                Name = c.Code + "-" + (language == "ne" && c.NameNp != null ? c.NameNp : c.Name)
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
                                        Name = c.Code+"-"+(language == "ne" && c.NameNp != null ? c.NameNp : c.Name)
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
                                        Name = c.Code+"-"+ (language == "ne" && c.NameNp != null ? c.NameNp : c.Name)
                                    }).ToList();
                        break;
                    case DataVisibilityLevel.ReportingHierarchy:
                        employee = (from c in Common.GetEmployees().Data
                                    select new EmployeeGridVm()
                                    {
                                        Id = c.Id,
                                        Name =c.Code+"-"+ (language == "ne" && c.NameNp != null ? c.NameNp : c.Name)
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
        [HttpPost]
        public KendoGridResult<object> GetReplacementLeaveForApproval(KendoReportViewModel vm)
        {
            int totalDays = (vm.ToDate.ToDateTime() - vm.OnDate.ToDateTime()).Days + 1;
            SMonthlyWiseReport reportService = new SMonthlyWiseReport();
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            reportService.FilteredEmployeeIDs = employees;
            //var result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt()).Data;
            var result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), vm.BranchId).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            int maxCount = 0;
            List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();
            if (employees.Count() > 0)
            {
                maxCount = employees.Count();
                employees = employees.Skip(vm.Skip).Take(vm.Take).ToArray();
                reportData = (from c in result
                              join d in employees
                              on c.EmployeeId equals d
                              select c
                                 ).Where(x => (x.Remark.ToLower() == "present" && x.Holiday.ToLower() == "yes") || (x.Remark.ToLower() == "present" && x.Weekend.ToLower() == "yes")).ToList();
            }
            else
            {
                reportData = result;
            }

            List<ReplacementLeaveApprovalGridModel> resultLst = new List<ReplacementLeaveApprovalGridModel>();
            foreach (var item in reportData)
            {
                ReplacementLeaveApprovalGridModel model = new ReplacementLeaveApprovalGridModel();
                model.EmployeeCode = item.EmployeeCode;
                model.EmployeeName = item.EmployeeName;
                model.Date = item.WorkDate;
                model.EmployeeId = item.EmployeeId;
                model.NepDate = item.NepDate;
                model.PresentOnHoliday = (item.Remark.ToLower() == "present" && item.Holiday.ToLower() == "yes") ? 1 : 0;
                model.PresentOnDayOff = (item.Remark.ToLower() == "present" && item.Weekend.ToLower() == "yes") ? 1 : 0;
                resultLst.Add(model);
            }

            return new KendoGridResult<object>()
            {
                Data = resultLst.OrderBy(x => x.Date.ToDateTime()).ThenBy(n => n.EmployeeName),
                Status = ResultStatus.Ok,
                TotalCount = resultLst.Count
            };
        }

        [HttpPost]
        public ServiceResult<int> Post(ReplacementLeavePostVm model)
        {
            int fyId = RiddhaSession.FYId;
            List<EEmployeePresentInOffHist> histLst = new List<EEmployeePresentInOffHist>();
            var repLeaveBalLst = (from c in model.List
                                  select new EReplacementLeaveBalance()
                                  {
                                      EmployeeId = c.EmployeeId,
                                      FyId = fyId,
                                      OpeningBalance = 0,
                                      RemainingBalance = (c.PresentOnHoliday == 1 && c.PresentOnDayOff == 1) ? 2 : 1
                                  }).ToList();
            foreach (var item in model.List)
            {
                EEmployeePresentInOffHist hist = new EEmployeePresentInOffHist();
                hist.Date = item.Date.ToDateTime();
                hist.EmployeeId = item.EmployeeId;
                if (item.PresentOnHoliday == 1 && item.PresentOnDayOff == 1)
                {
                    hist.PresentInHolidayOrDayOff = PresentInHolidayOrDayOff.Both;
                }
                else if (item.PresentOnHoliday == 1)
                {
                    hist.PresentInHolidayOrDayOff = PresentInHolidayOrDayOff.Holiday;
                }
                else if (item.PresentOnDayOff == 1)
                {
                    hist.PresentInHolidayOrDayOff = PresentInHolidayOrDayOff.DayOff;
                }
                _replacementLeaveBalance.Add(hist, repLeaveBalLst);
                Common.AddAuditTrail("8016", "7198", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 1, "Approve");
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                //Message = "ApproveSuccess",
                Message = _loc.Localize("ApproveSuccess"),
                Status = ResultStatus.Ok
            };
        }
        [HttpPost, ActionFilter("7201")]
        public KendoGridResult<List<ReplacementLeaveApprovalGridModel>> GetApprovalListKendoGrid(KendoPageListArguments vm)
        {
            var branchId = RiddhaSession.BranchId;
            List<EEmployeePresentInOffHist> histList = new List<EEmployeePresentInOffHist>();
            if (RiddhaSession.IsHeadOffice || RiddhaSession.DataVisibilityLevel == (int)DataVisibilityLevel.All)
            {
                histList = _replacementLeaveBalance.HistList().Data.Where(x => x.Employee.Branch.CompanyId == RiddhaSession.CompanyId).ToList();
            }
            else
            {
                histList = _replacementLeaveBalance.HistList().Data.Where(x => x.Employee.BranchId == branchId).ToList();
            }
            var Histlist = (from c in histList.ToList()
                            select new ReplacementLeaveApprovalGridModel()
                            {
                                EmployeeCode = c.Employee.Code,
                                EmployeeName = c.Employee.Name,
                                Date = c.Date.ToString("yyyy/MM/dd"),
                                PresentOnDayOff = c.PresentInHolidayOrDayOff == PresentInHolidayOrDayOff.Holiday ? 0 : 1,
                                PresentOnHoliday = c.PresentInHolidayOrDayOff == PresentInHolidayOrDayOff.DayOff ? 0 : 1,
                            }).ToList();
            return new KendoGridResult<List<ReplacementLeaveApprovalGridModel>>()
            {
                Data = Histlist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = Histlist.Count()
            };
        }

    }

    public class ReplacementLeaveApprovalGridModel
    {
        public int EmployeeId { get; set; }
        public string Date { get; set; }
        public string NepDate { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int PresentOnHoliday { get; set; }
        public int PresentOnDayOff { get; set; }
    }
    public class ReplacementLeavePostVm
    {
        public List<ReplacementLeaveApprovalGridModel> List { get; set; }
    }

}
