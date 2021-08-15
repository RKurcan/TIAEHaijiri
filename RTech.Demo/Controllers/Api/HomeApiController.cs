using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Mobile.Services;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Areas.Office.Controllers.Api;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using static RTech.Demo.Utilities.WDMS;

namespace RTech.Demo.Controllers.Api
{
    public class HomeApiController : ApiController
    {
        LocalizedString loc = null;
        SNotification notificationServices = null;
        SUser userServices = null;
        SEmployee empservices = null;
        int _packageId = 0;
        SCompany _companyServices = null;
        SBranch _branchServices = null;
        SCompanyDeviceAssignment _compDeviceServices = null;
        SDeviceAssignment _deviceAssignment = null;
        SEmployee _empServices = null;
        SDepartment _departmentServices = null;
        SReseller _resellerServices = null;
        SDevice _deviceServices = null;
        string lang = RiddhaSession.Language;

        public HomeApiController()
        {
            loc = new LocalizedString();
            notificationServices = new SNotification();
            userServices = new SUser();
            empservices = new SEmployee();
            _packageId = RiddhaSession.PackageId;
            _companyServices = new SCompany();
            _branchServices = new SBranch();
            _compDeviceServices = new SCompanyDeviceAssignment();
            _empServices = new SEmployee();
            _departmentServices = new SDepartment();
            _resellerServices = new SReseller();
            _deviceAssignment = new SDeviceAssignment();
            _deviceServices = new SDevice();

        }

        [HttpGet]
        public ServiceResult<DashboardUserViewModel> GetAttendanceInfo()
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            List<AttendanceReportDetailViewModel> reportList = new List<AttendanceReportDetailViewModel>();
            SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(branchId, RiddhaSession.FYId);
            var DepartmentCount = _departmentServices.List().Data.Count(x => x.BranchId == RiddhaSession.BranchId);
            int[] Emp = Common.GetEmployees().Data.Select(x => x.Id).ToArray();
            var result = reportService.GetAttendanceReportFromSp(System.DateTime.Now);
            reportList = result.Data;
            reportList = (from c in result.Data
                          join d in Emp on c.EmployeeId equals d
                          select c
                              ).ToList();
            if (_packageId > 0)
            {
                reportList = reportList.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            DashboardUserViewModel vm = new DashboardUserViewModel()
            {
                PresentCount = reportList.Count(x => x.ActualTimeIn != "00:00"),
                LateInCount = reportList.Count(x => x.LateIn != "00:00" && x.LateIn != ""),
                AbsentCount = reportList.Count(x => x.ActualTimeIn == "00:00" && x.OnLeave == "No" && x.OfficeVisit == "NO" && x.Kaj == "NO"),
                OnLeaveCount = reportList.Count(x => x.LeaveName != null),
                HolidayCount = reportList.Count(x => x.Remark == "Holiday"),
                OfficeVisitCount = reportList.Count(x => x.OfficeVisit == "YES"),
                KajCount = reportList.Count(x => x.Kaj == "YES"),
                EmployeeCount = reportList.Count(),
                //EmployeeCount = empCount,
                DepartmentCount = DepartmentCount,
            };
            vm.Devices = GetDevices();
            vm.EmpList = GetDashboardEmpList(result);
            return new ServiceResult<DashboardUserViewModel>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<WeekAttendanceModel>> GetWeekAttendance(int userId)
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            var endDate = DateTime.Now;
            var startDate = DateTime.Now.AddDays(-6);
            SMonthlyWiseReport _reportService = new SMonthlyWiseReport();
            SEmployee _empServices = new SEmployee();
            var emp = _empServices.List().Data.Where(x => x.UserId == userId).FirstOrDefault();
            var empId = emp == null ? 0 : emp.Id;
            _reportService.FilteredEmployeeIDs = new int[0] { };
            var attendance = _reportService.GetAttendanceReportFromSp(startDate, endDate, branchId);
            var result = (from c in attendance.Data.Where(x => x.EmployeeId == empId).ToList()
                          select new WeekAttendanceModel()
                          {
                              Date = c.WorkDate,
                              InTime = c.ActualTimeIn,
                              OutTime = c.ActualTimeOut
                          }).ToList();
            return new ServiceResult<List<WeekAttendanceModel>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<DeviceGridVm>> GetActiveDeviceKendoGrid(KendoPageListArguments arg)
        {
            WebrequestService webrequest = new WebrequestService(RiddhaSession.ADMSUrl);
            string EnableADMS = ConfigurationManager.AppSettings["EnableADMS"];
            List<DeviceGridVm> list = new List<DeviceGridVm>();
            if (EnableADMS != "0")
            {
                try
                {
                    list = webrequest.Get<List<RTech.Demo.Models.DeviceGridVm>>("/api/homeapi/getdevicebybranch?code=" + RiddhaSession.BranchCode).Result;
                }
                catch (Exception ex)
                {
                    Log.SytemLog(ex.Message);
                    list = new List<DeviceGridVm>();
                }
            }
            else
            {
                list = new List<DeviceGridVm>();
            }
            var device = new SDevice().List().Data;
            var result = (from c in list
                          join d in device on c.SN equals d.SerialNumber into joinedT
                          from d in joinedT.DefaultIfEmpty()
                          select new DeviceGridVm()
                          {
                              BranchCode = c.BranchCode,
                              DepartmentCode = c.DepartmentCode,
                              DevFuns = c.DevFuns,
                              DeviceModel = c.DeviceModel,
                              DeviceStatus = c.DeviceStatus,
                              DeviceStatusName = c.DeviceStatusName,
                              FaceCount = c.FaceCount,
                              FirmwareVersion = c.FirmwareVersion,
                              FPCount = c.FPCount,
                              Id = c.Id,
                              IP = c.IP,
                              IsAccessDevice = c.IsAccessDevice,
                              IsFaceDevice = c.IsFaceDevice,
                              LastActivity = c.LastActivity,
                              Name = d.Name,
                              SN = c.SN,
                              TransCount = c.TransCount,
                              UserCount = c.UserCount,
                          }).ToList();
            return new KendoGridResult<List<DeviceGridVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<AttendacneInfoDetails>> GetAttendanceInfoDetails(KendoHomeRequest model)
        {
            string currentLanguage = RiddhaSession.Language;
            int? BranchId = RiddhaSession.BranchId;
            SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(BranchId.ToInt(), RiddhaSession.FYId, RiddhaSession.Language);
            List<AttendanceReportDetailViewModel> result = reportService.GetAttendanceReportFromSp(System.DateTime.Now).Data;
            int[] Emp = Common.GetEmployees().Data.Select(x => x.Id).ToArray();
            result = (from c in result
                      join d in Emp on c.EmployeeId equals d
                      select c
                            ).ToList();
            List<AttendacneInfoDetails> reportList = new List<AttendacneInfoDetails>();
            if (_packageId > 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            if (model.type == "present")
            {
                reportList = (from c in result.Where(x => x.ActualTimeIn != "00:00")
                              select new AttendacneInfoDetails()
                              {
                                  EmployeeName = c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  EmployeeDeviceCode = c.EmployeeDeviceCode,
                                  DepartmentName = c.DepartmentName,
                                  ShiftName = c.ShiftName,
                                  //PlannedTimeIn = c.PlannedTimeIn,
                                  //PlannedTimeOut = c.PlannedTimeOut,
                                  //ActualTimeIn = c.ActualTimeIn,
                                  //ActualTimeOut = c.ActualTimeOut,
                                  Remark = c.Remark,
                                  PlannedTimeIn12Format = c.PlannedTimeIn12Format,
                                  ActualTimeIn12Format = c.ActualTimeIn12Format,
                                  ActualTimeOut12Format = c.ActualTimeOut12Format,
                                  PlannedTimeOut12Format = c.PlannedTimeOut12Format

                              }).ToList();

            }
            if (model.type == "latein")
            {
                reportList = (from c in result.Where(x => x.LateIn != "00:00" && x.LateIn != "")
                              select new AttendacneInfoDetails()
                              {
                                  EmployeeName = c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  EmployeeDeviceCode = c.EmployeeDeviceCode,
                                  ShiftName = c.ShiftName,
                                  //PlannedTimeIn = c.PlannedTimeIn,
                                  //PlannedTimeOut = c.PlannedTimeOut,
                                  //ActualTimeIn = c.ActualTimeIn,
                                  //ActualTimeOut = c.ActualTimeOut,
                                  LateTime = GetLateTime(c.ActualTimeIn, c.PlannedTimeIn),
                                  DepartmentName = c.DepartmentName,
                                  Remark = c.Remark,
                                  PlannedTimeIn12Format = c.PlannedTimeIn12Format,
                                  ActualTimeIn12Format = c.ActualTimeIn12Format,
                                  ActualTimeOut12Format = c.ActualTimeOut12Format,
                                  PlannedTimeOut12Format = c.PlannedTimeOut12Format

                              }).ToList();

            }
            if (model.type == "absent")
            {
                reportList = (from c in result.Where(x => x.ActualTimeIn == "00:00" && x.OnLeave == "No" && x.OfficeVisit == "NO" && x.Kaj == "NO")
                              select new AttendacneInfoDetails()
                              {
                                  EmployeeName = c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  EmployeeDeviceCode = c.EmployeeDeviceCode,
                                  ShiftName = c.ShiftName,
                                  //PlannedTimeIn = c.PlannedTimeIn,
                                  //PlannedTimeOut = c.PlannedTimeOut,
                                  //ActualTimeIn = c.ActualTimeIn,
                                  //ActualTimeOut = c.ActualTimeOut,
                                  DepartmentName = c.DepartmentName,
                                  Remark = c.Remark,
                                  PlannedTimeIn12Format = c.PlannedTimeIn12Format,
                                  ActualTimeIn12Format = c.ActualTimeIn12Format,
                                  ActualTimeOut12Format = c.ActualTimeOut12Format,
                                  PlannedTimeOut12Format = c.PlannedTimeOut12Format
                              }).ToList();

            }
            if (model.type == "leave")
            {
                reportList = (from c in result.Where(x => x.LeaveName != null)
                              select new AttendacneInfoDetails()
                              {
                                  EmployeeName = c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  EmployeeDeviceCode = c.EmployeeDeviceCode,
                                  DepartmentName = c.DepartmentName,
                                  ShiftName = c.ShiftName,
                                  //PlannedTimeIn = c.PlannedTimeIn,
                                  //PlannedTimeOut = c.PlannedTimeOut,
                                  //ActualTimeIn = c.ActualTimeIn,
                                  //ActualTimeOut = c.ActualTimeOut,
                                  PlannedTimeIn12Format = c.PlannedTimeIn12Format,
                                  ActualTimeIn12Format = c.ActualTimeIn12Format,
                                  ActualTimeOut12Format = c.ActualTimeOut12Format,
                                  PlannedTimeOut12Format = c.PlannedTimeOut12Format,
                                  Remark = c.Remark,
                              }).ToList();

            }
            if (model.type == "employee")
            {
                reportList = (from c in result
                              select new AttendacneInfoDetails()
                              {
                                  EmployeeName = c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  EmployeeDeviceCode = c.EmployeeDeviceCode,
                                  DepartmentName = c.DepartmentName,
                                  ShiftName = c.ShiftName,
                                  //PlannedTimeIn = c.PlannedTimeIn,
                                  //PlannedTimeOut = c.PlannedTimeOut,
                                  //ActualTimeIn = c.ActualTimeIn,
                                  //ActualTimeOut = c.ActualTimeOut,
                                  Remark = c.Remark,
                                  PlannedTimeIn12Format = c.PlannedTimeIn12Format,
                                  ActualTimeIn12Format = c.ActualTimeIn12Format,
                                  ActualTimeOut12Format = c.ActualTimeOut12Format,
                                  PlannedTimeOut12Format = c.PlannedTimeOut12Format
                              }).ToList();
            }
            if (model.type == "officeVisit")
            {
                reportList = (from c in result.Where(x => x.OfficeVisit == "YES" || x.Kaj == "YES")
                              select new AttendacneInfoDetails()
                              {
                                  EmployeeName = c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  DepartmentName = c.DepartmentName,
                                  EmployeeDeviceCode = c.EmployeeDeviceCode,
                                  ShiftName = c.ShiftName,
                                  //PlannedTimeIn = c.PlannedTimeIn,
                                  //PlannedTimeOut = c.PlannedTimeOut,
                                  //ActualTimeIn = c.ActualTimeIn,
                                  //ActualTimeOut = c.ActualTimeOut,
                                  Remark = c.Remark,
                                  PlannedTimeIn12Format = c.PlannedTimeIn12Format,
                                  ActualTimeIn12Format = c.ActualTimeIn12Format,
                                  ActualTimeOut12Format = c.ActualTimeOut12Format,
                                  PlannedTimeOut12Format = c.PlannedTimeOut12Format
                              }).ToList();


            }

            if (model.Filter.Filters.Count() > 0)
            {
                switch (model.Filter.Filters[0].Operator.ToLower())
                {
                    case "startswith":
                        reportList = (from c in reportList
                                      where (c.EmployeeName.ToLower().StartsWith(model.Filter.Filters[0].Value.Trim().ToLower()) || c.EmployeeCode.Trim().ToLower().StartsWith(model.Filter.Filters[0].Value.ToLower()))
                                      select c).ToList();
                        break;
                    case "eq":
                        reportList = (from c in reportList
                                      where (c.EmployeeName.ToLower() == (model.Filter.Filters[0].Value.Trim().ToLower()) || c.EmployeeCode.Trim().ToLower() == (model.Filter.Filters[0].Value.ToLower()))
                                      select c).ToList();
                        break;
                }
            }
            if (model.Sort.Count() > 0)
            {
                switch (model.Sort[0].Field.ToLower())
                {
                    case "employeename":
                        if (model.Sort[0].Dir.ToLower() == "asc")
                        {
                            reportList = reportList.OrderBy(x => x.EmployeeName).ToList();
                        }
                        else
                        {
                            reportList = reportList.OrderByDescending(x => x.EmployeeName).ToList();
                        }
                        break;
                    case "employeecode":
                        if (model.Sort[0].Dir.ToLower() == "asc")
                        {
                            reportList = reportList.OrderBy(x => x.EmployeeName).ToList();
                        }
                        else
                        {
                            reportList = reportList.OrderByDescending(x => x.EmployeeName).ToList();
                        }
                        break;
                    default:
                        reportList = (from c in reportList
                                      select c).OrderBy(x => x.EmployeeName).ToList();
                        break;
                }
            }
            return new KendoGridResult<List<AttendacneInfoDetails>>()
            {
                Data = reportList.Skip(model.Skip).Take(model.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = reportList.Count
            };
        }

        private string GetLateTime(string ActualTimeIn, string PlannedTimeIn)
        {
            DateTime d1 = new DateTime();
            d1 = Convert.ToDateTime(ActualTimeIn); DateTime d2 = new DateTime();
            d2 = Convert.ToDateTime(PlannedTimeIn);
            TimeSpan ts = d1.Subtract(d2);
            return ts.ToString(@"hh\:mm");
        }

        [HttpGet]
        public ServiceResult<List<HomeUpcomming>> GetUpcommingNews()
        {
            int? branchId = RiddhaSession.BranchId;
            DateTime todaysDate = DateTime.Today;
            DateTime lastDate = DateTime.Now.AddMonths(1);
            var notificationLst = notificationServices.List().Data.Where(x => x.CompanyId == branchId && x.NotificationType != NotificationType.Leave && x.EffectiveDate >= todaysDate && x.EffectiveDate <= lastDate).ToList();
            List<HomeUpcomming> upcommingsLst = (from c in notificationLst
                                                 select new HomeUpcomming()
                                                 {
                                                     Title = c.Title,
                                                     Type = c.NotificationType,
                                                     Desc = c.Message,
                                                     Date = c.EffectiveDate.ToString("yyyy/MM/dd")

                                                 }).OrderBy(x => x.Date).ToList();
            return new ServiceResult<List<HomeUpcomming>>()
            {
                Data = upcommingsLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<DashboardVm> GetResellerDashboardDeviceInfo()
        {
            DashboardVm vm = new DashboardVm();
            EUser curuser = RTech.Demo.Utilities.RiddhaSession.CurrentUser ?? new Riddhasoft.User.Entity.EUser();
            SDeviceAssignment AssignedDevices = new SDeviceAssignment();
            List<EDeviceAssignment> AssignedDevicesList = AssignedDevices.List().Data.Where(x => x.Device.Status == Status.Reseller).ToList();
            SCompanyDeviceAssignment companyAssignedDevices = new SCompanyDeviceAssignment();
            List<ECompanyDeviceAssignment> companyAssignedDevicesList = companyAssignedDevices.List().Data.Where(x => x.AssignedById == curuser.Id).ToList();
            int resellerId = userServices.GetResellerLoginLst().Where(x => x.UserId == curuser.Id).FirstOrDefault().ResellerId;
            vm.ResellerVm = new DashboardResellerVm()
            {
                ResellerDeviceCount = AssignedDevicesList.Count(x => (x.ResellerId == resellerId)),
                CustomerDeviceCount = companyAssignedDevicesList.Count(x => x.Device.Status == Status.Customer),
                DamageDeviceCount = companyAssignedDevicesList.Count(x => x.Device.Status == Status.Damage),
            };
            //vm.CompanyList = (from b in companyAssignedDevicesList.Where(x => x.Device.Status == Status.Customer)
            //                  group b by b.CompanyId into g
            //                  select new ResellerDetailVm
            //                  {
            //                      Address = g.First().Company.Address,
            //                      Name = g.First().Company.Name,
            //                      Quantity = g.Count()
            //                  }).ToList();
            return new ServiceResult<DashboardVm>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };

        }

        [HttpGet]
        public ServiceResult<DashboardVm> GetResellersCustomerInfo()
        {
            DashboardVm vm = new DashboardVm();
            SDevice deviceDevices = new SDevice();
            List<EDevice> deviceLst = deviceDevices.List().Data.ToList();
            SDeviceAssignment AssignedDevices = new SDeviceAssignment();
            List<EDeviceAssignment> AssignedDevicesList = AssignedDevices.List().Data.Where(x => x.Device.Status == Status.Reseller).ToList();
            vm.DashBoardAdminVm = new DashBoardAdminVm()
            {
                NewDeviceCount = deviceLst.Count(x => x.Status == Status.New),
                ResellerDeviceCount = _resellerServices.List().Data.Count(),
                //CustomerDeviceCount = deviceLst.Count(x => x.Status == Status.Customer),
                CustomerDeviceCount = _companyServices.List().Data.Count(),
                DamageDeviceCount = deviceLst.Count(x => x.Status == Status.Damage),
            };

            var reselllerList = _resellerServices.List().Data.ToList();
            var deviceAssingment = _deviceAssignment.List().Data.ToList();


            vm.ResellerList = (from d in reselllerList
                               select new ResellerDetailVm()
                               {
                                   Address = d.Address,
                                   ContactNo = d.ContactNo,
                                   ContactPerson = d.ContactPerson,
                                   Name = d.Name,
                                   ResellerStockDevice = getResellerStockDevice(d.Id),
                                   ResellerDamageDevice = GetResellerDamageDeviceList(d.Id),
                                   ReselletCustomerDevice = getResellerCustomerDevice(d.Id)
                               }).ToList();
            return new ServiceResult<DashboardVm>()
            {
                Data = vm,
                Status = ResultStatus.Ok,

            };

        }

        private int getResellerCustomerDevice(int id)
        {
            return _deviceAssignment.List().Data.Where(x => x.ResellerId == id && x.Device.Status == Status.Customer).Count();
        }

        private int GetResellerDamageDeviceList(int id)
        {
            return _deviceAssignment.List().Data.Where(x => x.ResellerId == id && x.Device.Status == Status.Damage).Count();
        }

        private int getResellerStockDevice(int resellerId)
        {
            return _deviceAssignment.List().Data.Where(x => x.ResellerId == resellerId && x.Device.Status == Status.Reseller).Count();
        }

        [HttpGet]
        public ServiceResult<EMHomeAttendanceInfo> GetCurrentUserAttendanceInfo(int userId)
        {
            var emp = empservices.List().Data.Where(x => x.UserId == userId).FirstOrDefault() ?? new EEmployee();
            SMHome homeService = new SMHome();
            EMHomeAttendanceInfo homeAttendanceInfo = homeService.GetHomeAttendanceInfo(emp.Id);
            return new ServiceResult<EMHomeAttendanceInfo>()
            {
                Data = homeAttendanceInfo,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EMonthlyAttendanceSummary> GetCurrentUserAttendanceSummary(int userId)
        {
            var emp = empservices.List().Data.Where(x => x.UserId == userId).FirstOrDefault() ?? new EEmployee();
            SMHome homeService = new SMHome();
            CurrentUserAttendanceSummary vm = new CurrentUserAttendanceSummary();
            EMonthlyAttendanceSummary homeattendanceSummary = homeService.GetAttendanceSummary(emp.Id);
            return new ServiceResult<EMonthlyAttendanceSummary>()
            {
                Data = homeattendanceSummary,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<CurrentUserAttendanceSummary> GetCurrentUserAttendanceSummaryAndInfo(int userId)
        {
            var emp = empservices.List().Data.Where(x => x.UserId == userId).FirstOrDefault() ?? new EEmployee();
            SMHome homeService = new SMHome();
            CurrentUserAttendanceSummary vm = new CurrentUserAttendanceSummary();
            vm.AttendanceInfo = homeService.GetHomeAttendanceInfo(emp.Id);
            vm.MonthlySummary = homeService.GetAttendanceSummary(emp.Id);
            return new ServiceResult<CurrentUserAttendanceSummary>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<PartnerKendoGridModel>> GetCustomerKendoGrid(KendoPageListArguments vm)
        {
            int userId = RiddhaSession.UserId;
            var reseller = userServices.GetResellerLoginLst().Where(x => x.UserId == userId).FirstOrDefault();
            var companyQuery = _companyServices.List().Data.Where(x => x.ResellerId == reseller.ResellerId);
            var branchQuery = _branchServices.List().Data;
            var companyLicenseQuery = _companyServices.ListCompanyLicense().Data;
            List<ECompany> companyLst = new List<ECompany>();
            if (vm.Filter.Filters.Count() > 0)
            {
                string searchText = vm.Filter.Filters[0].Value.ToLower();
                switch (vm.Filter.Filters[0].Operator.ToLower())
                {
                    case "startswith":
                        companyLst = (from c in companyQuery
                                      where (c.Name.ToLower().StartsWith(searchText))
                                      select c).OrderBy(x => x.Name).Skip(vm.Skip).Take(vm.Take).ToList();
                        break;
                    case "eq":
                        companyLst = (from c in companyQuery
                                      where (c.Name.ToLower().StartsWith(searchText))
                                      select c).OrderBy(x => x.Name).Skip(vm.Skip).Take(vm.Take).ToList();
                        break;
                    default:
                        companyLst = companyQuery.ToList();
                        break;
                }
            }
            else
            {
                companyLst = (from c in companyQuery
                              select c).OrderBy(x => x.Name).Skip(vm.Skip).Take(vm.Take).ToList();
            }

            List<PartnerKendoGridModel> resultLst = (from c in companyLst
                                                     select new PartnerKendoGridModel()
                                                     {
                                                         NoOfBranches = branchQuery.Where(x => x.CompanyId == c.Id).Count().ToString(),
                                                         NoOfUsers = userServices.List().Data.Where(x => x.Branch.CompanyId == c.Id).Count().ToString(),
                                                         NoOfDevice = _compDeviceServices.List().Data.Where(x => x.CompanyId == c.Id).Count().ToString(),
                                                         NoOfEmployee = _empServices.List().Data.Where(x => x.Branch.CompanyId == c.Id).Count().ToString(),
                                                         ServiceStartedFrom = (companyLicenseQuery.Where(x => x.CompanyId == c.Id).FirstOrDefault() ?? new ECompanyLicense()).IssueDate.ToString("yyyy/MM/dd"),
                                                         LicenseExpiredDate = (companyLicenseQuery.Where(x => x.CompanyId == c.Id).FirstOrDefault() ?? new ECompanyLicense()).ExpiryDate.ToString("yyyy/MM/dd"),
                                                         SoftwarePackage = c.SoftwarePackageType.ToString(),
                                                         CompanyAddress = c.Address,
                                                         CompanyCode = c.Code,
                                                         CompanyName = c.Name,
                                                         ContactNo = c.ContactNo,
                                                         ContactPerson = c.ContactPerson
                                                     }).ToList();

            return new KendoGridResult<List<PartnerKendoGridModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok,
                TotalCount = companyQuery.Count()
            };
        }

        public List<DashboardDivDataModel> GetDashboardEmpList(ServiceResult<List<AttendanceReportDetailViewModel>> result)
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            List<AttendanceReportDetailViewModel> reportList = new List<AttendanceReportDetailViewModel>();
            SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(branchId, RiddhaSession.FYId, RiddhaSession.Language);
            //var result = reportService.GetAttendanceReportFromSp(System.DateTime.Now);
            //reportList = result.Data;
            int[] Emp = Common.GetEmployees().Data.Select(x => x.Id).ToArray();
            reportList = (from c in result.Data
                          join d in Emp on c.EmployeeId equals d
                          select c
                          ).ToList();
            if (_packageId > 0)
            {
                reportList = reportList.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            var dashData = new List<DashboardDivDataModel>();

            dashData.Add(new DashboardDivDataModel()
            {
                Type = "Absent",
                Employees = (from c in reportList.Where(x => x.Absent.ToLower() == "yes") select new EmployeeInfo() { Code = c.EmployeeCode, Name = c.EmployeeName, Designation = c.DepartmentName }).ToList()
            });
            dashData.Add(new DashboardDivDataModel()
            {
                Type = "Leave",
                Employees = (from c in reportList.Where(x => x.OnLeave.ToLower() == "yes") select new EmployeeInfo() { Code = c.EmployeeCode, Name = c.EmployeeName, Designation = c.DepartmentName }).ToList()
            });

            dashData.Add(new DashboardDivDataModel()
            {
                Type = "Office Visit",
                Employees = (from c in reportList.Where(x => x.OfficeVisit.ToLower() == "yes") select new EmployeeInfo() { Code = c.EmployeeCode, Name = c.EmployeeName, Designation = c.DepartmentName }).ToList()
            });
            dashData.Add(new DashboardDivDataModel()
            {
                Type = "Kaj",
                Employees = (from c in reportList.Where(x => x.Kaj.ToLower() == "yes") select new EmployeeInfo() { Code = c.EmployeeCode, Name = c.EmployeeName, Designation = c.DepartmentName }).ToList()
            });

            return dashData;

            //return new ServiceResult<List<DashboardDivDataModel>>()
            //{
            //    Data = dashData,
            //    Status = ResultStatus.Ok
            //};
        }

        public List<DashBoardDeviceVm> GetDevices()
        {
            List<DashBoardDeviceVm> resultLst = new List<DashBoardDeviceVm>();
            SCompanyDeviceAssignment companyDeviceAssignServices = new SCompanyDeviceAssignment();
            int companyId = RiddhaSession.CompanyId;
            var deviceAssignedToCompanyLst = companyDeviceAssignServices.List().Data.Where(x => x.CompanyId == companyId && x.Device.Status == Status.Customer).ToList();

            // var devices = wdmsdb.iclock.Where(x => x.company_id == null).ToList();
            WebrequestService webrequest = new WebrequestService(RiddhaSession.ADMSUrl);
            string EnableADMS = ConfigurationManager.AppSettings["EnableADMS"];
            List<DeviceGridVm> list = new List<DeviceGridVm>();
            if (EnableADMS != "0")
            {
                try
                {
                    list = webrequest.Get<List<RTech.Demo.Models.DeviceGridVm>>("/api/homeapi/getdevicebybranch?code=" + RiddhaSession.BranchCode).Result;
                }
                catch (Exception ex)
                {
                    Log.SytemLog(ex.Message);
                    list = new List<DeviceGridVm>();
                }
            }
            else
            {
                list = new List<DeviceGridVm>();
            }
            foreach (var item in deviceAssignedToCompanyLst)
            {
                DashBoardDeviceVm vm = new DashBoardDeviceVm();
                if (item.Device.DeviceType == DeviceType.ADMS)
                {
                    try
                    {
                        var device = (from c in list
                                      select c
                                ).Where(x => x.SN == item.Device.SerialNumber).FirstOrDefault();
                        if (device != null)
                        {
                            vm.IsOnline = (device.DeviceStatus == "Online" || device.DeviceStatus.StartsWith("Synchronizing"));
                        }
                    }
                    catch
                    {

                        Log.SytemLog("error on device listing dashboard");
                    }
                }
                vm.Id = item.DeviceId;
                vm.Name = item.Device.Name;
                resultLst.Add(vm);
            }

            return resultLst;

        }

        [HttpPost]
        public KendoGridResult<List<DepartmentGridVm>> GetDepartments(KendoPageListArguments vm)
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            IQueryable<EDepartment> depQuery = _departmentServices.List().Data.Where(x => x.BranchId == branchId);
            var employees = _empServices.List().Data.Where(x => x.BranchId == branchId && x.Section != null && (x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring)).ToList();
            int totalRowNum = depQuery.Count();
            var deplist = (from c in depQuery.ToList()
                           join d in employees
                                on c.Id equals d.Section.DepartmentId into temp
                           select new DepartmentGridVm()
                           {
                               Id = c.Id,
                               Code = c.Code,
                               Name = c.Name,
                               NameNp = c.NameNp,
                               NumberOfStaff = temp.Where(x => x.Section.DepartmentId == c.Id).Count(),
                               BranchId = c.BranchId,
                           }).ToList();
            return new KendoGridResult<List<DepartmentGridVm>>()
            {
                Data = deplist,
                TotalCount = totalRowNum,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<ResellerInfoVm>> GetResellerDetails(KendoPageListArguments vm)
        {
            var reseller = _resellerServices.List().Data.ToList();
            var resellerList = (from c in reseller
                                select new ResellerInfoVm()
                                {
                                    Address = c.Address,
                                    Code = c.Code,
                                    Contact = c.ContactNo,
                                    ContactPerson = c.ContactPerson,
                                    Email = c.Email,
                                    Name = c.Name,
                                    PanNo = c.PAN,
                                    Website = c.WebUrl
                                }).ToList();
            return new KendoGridResult<List<ResellerInfoVm>>()
            {
                Data = resellerList,
                TotalCount = resellerList.Count,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public KendoGridResult<List<CompanyInfoVm>> GetCompanyDetails(KendoPageListArguments vm)
        {
            var company = _companyServices.List().Data.ToList();
            var device = _compDeviceServices.List().Data.ToList();
            var companyList = (from c in company
                                   //join d in device on c.Id equals d.CompanyId
                               select new CompanyInfoVm()
                               {
                                   Address = c.Address,
                                   Code = c.Code,
                                   Contact = c.ContactNo,
                                   ContactPerson = c.ContactPerson,
                                   Email = c.Email,
                                   Name = c.Name,
                                   Pan = c.PAN,
                                   Website = c.WebUrl,
                                   ResellerName = c.Reseller.Name,
                                   DeviceCount = device.Count(x => x.CompanyId == c.Id)
                               }).OrderBy(x => x.DeviceCount).ToList();
            return new KendoGridResult<List<CompanyInfoVm>>()
            {
                Data = companyList,
                TotalCount = companyList.Count,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<ResellerDashboarStockDeviceGridVm>> GetResellerStockDeviceList(KendoPageListArguments vm)
        {
            EUser curuser = RTech.Demo.Utilities.RiddhaSession.CurrentUser ?? new Riddhasoft.User.Entity.EUser();
            SDeviceAssignment AssignedDevices = new SDeviceAssignment();
            List<EDeviceAssignment> AssignedDevicesList = AssignedDevices.List().Data.Where(x => x.Device.Status == Status.Reseller).ToList();
            int resellerId = userServices.GetResellerLoginLst().Where(x => x.UserId == curuser.Id).FirstOrDefault().ResellerId;
            var stockdevice = AssignedDevicesList.Where(x => x.ResellerId == resellerId);
            var stockdeviceList = (from c in stockdevice
                                   select new ResellerDashboarStockDeviceGridVm()
                                   {
                                       DeviceType = Enum.GetName(typeof(DeviceType), c.Device.DeviceType),
                                       ModelName = c.Device.Model.Name,
                                       SerialNo = c.Device.SerialNumber,
                                   }).ToList();
            return new KendoGridResult<List<ResellerDashboarStockDeviceGridVm>>()
            {
                Data = stockdeviceList,
                TotalCount = stockdeviceList.Count,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<ResellerDashboarStockDeviceGridVm>> GetResellerDamageDeviceList(KendoPageListArguments vm)
        {
            EUser curuser = RTech.Demo.Utilities.RiddhaSession.CurrentUser ?? new Riddhasoft.User.Entity.EUser();
            SCompanyDeviceAssignment companyAssignedDevices = new SCompanyDeviceAssignment();
            List<ECompanyDeviceAssignment> companyAssignedDevicesList = companyAssignedDevices.List().Data.Where(x => x.AssignedById == curuser.Id).ToList();
            var damageDevice = companyAssignedDevicesList.Where(x => x.Device.Status == Status.Damage);
            var damagedeviceList = (from c in damageDevice
                                    select new ResellerDashboarStockDeviceGridVm()
                                    {
                                        DeviceType = Enum.GetName(typeof(DeviceType), c.Device.DeviceType),
                                        ModelName = c.Device.Model.Name,
                                        SerialNo = c.Device.SerialNumber,
                                    }).ToList();
            return new KendoGridResult<List<ResellerDashboarStockDeviceGridVm>>()
            {
                Data = damagedeviceList,
                TotalCount = damagedeviceList.Count,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<EmpDOBAndJoinVM>> GetEmpDOBAndJoin()
        {
            int BranchId = (int)RiddhaSession.BranchId;
            var empdata = _empServices.List().Data.Where(x => x.BranchId == BranchId).ToList();
            var employeesBirthdaylist = (from c in empdata.Where(x => x.DateOfBirth.GetValueOrDefault().Day == DateTime.Now.Day
                                && x.DateOfBirth.GetValueOrDefault().Month == DateTime.Now.Month)
                                         select new EmpDOBAndJoinVM()
                                         {
                                             Name = lang == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                             //Section = c.Section == null ? "" : lang == "ne" && c.Section.NameNp != null ? c.Section.NameNp : c.Section.Name,
                                             //Department = c.Section == null ? "" : lang == "ne" && c.Section.Department.NameNp != null ? c.Section.Department.NameNp : c.Section.Department.Name,
                                             Description = lang == "en" ? "birthday is today" : "जन्मदिन आजरहे को छ | ",
                                         }).ToList();
            var employeesJoinedlist = (from c in empdata.Where(x => x.DateOfJoin.GetValueOrDefault().Day == DateTime.Now.Day
                                && x.DateOfJoin.GetValueOrDefault().Month == DateTime.Now.Month)
                                       select new EmpDOBAndJoinVM()
                                       {
                                           Name = lang == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                           //Section = c.Section == null ? "" : lang == "ne" && c.Section.NameNp != null ? c.Section.NameNp : c.Section.Name,
                                           //Department = c.Section == null ? "" : lang == "ne" && c.Section.Department.NameNp != null ? c.Section.Department.NameNp : c.Section.Department.Name,
                                           Description = lang == "en" ? "Anniversary is today" : "वार्षिकोत्सव आजरहे को छ | ",
                                       }).ToList();
            return new ServiceResult<List<EmpDOBAndJoinVM>>()
            {
                Data = employeesBirthdaylist.Concat(employeesJoinedlist).ToList(),
                Status = ResultStatus.Ok
            };



        }



    }

    public class HomeUpcomming
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Date { get; set; }
        public NotificationType Type { get; set; }
        public int TargetId { get; set; }
    }

    public class DashboardVm
    {
        public DashBoardAdminVm DashBoardAdminVm { get; set; }
        public DashboardResellerVm ResellerVm { get; set; }
        public List<ResellerDetailVm> ResellerList { get; set; }

    }
    public class DashboardUserViewModel
    {
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LateInCount { get; set; }
        public int OnLeaveCount { get; set; }
        public int HolidayCount { get; set; }
        public int EmployeeCount { get; set; }
        public int OfficeVisitCount { get; set; }
        public int KajCount { get; set; }
        public int DepartmentCount { get; set; }
        public List<ReportItem> ReportItems { get; set; }
        public List<DashBoardDeviceVm> Devices { get; set; }
        public List<DashboardDivDataModel> EmpList { get; set; }
    }
    public class DashboardResellerVm
    {
        public int NewDeviceCount { get; set; }
        public int ResellerDeviceCount { get; set; }
        public int CustomerDeviceCount { get; set; }
        public int DamageDeviceCount { get; set; }
        public string ResellerName { get; set; }
    }

    public class ResellerDetailVm
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public int ResellerStockDevice { get; set; }
        public int ReselletCustomerDevice { get; set; }
        public int ResellerDamageDevice { get; set; }
    }

    public class DashBoardAdminVm
    {
        public int NewDeviceCount { get; set; }
        public int ResellerDeviceCount { get; set; }
        public int CustomerDeviceCount { get; set; }
        public int DamageDeviceCount { get; set; }
        public int TotalResellerDeviceCount { get; set; }
        public Array EachResellerDeviceCount { get; set; }
        public string ResellerName { get; set; }
    }

    public class AttendacneInfoDetails
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int EmployeeDeviceCode { get; set; }
        public string ShiftName { get; set; }
        public string PlannedTimeIn { get; set; }
        public string PlannedTimeOut { get; set; }
        public string ActualTimeIn { get; set; }
        public string ActualTimeOut { get; set; }
        public string PlannedTimeIn12Format { get; set; }
        public string PlannedTimeOut12Format { get; set; }
        public string ActualTimeIn12Format { get; set; }
        public string ActualTimeOut12Format { get; set; }
        public int TotalCount { get; set; }
        public string LateTime { get; set; }
        public string DepartmentName { get; set; }
        public string Remark { get; set; }
    }


    public class KendoHomeRequest : KendoPageListArguments
    {
        public string type { get; set; }
        public int count { get; set; }
    }

    public class EmployeeInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
    }
    public class DashboardDivDataModel
    {
        public DashboardDivDataModel()
        {
            Employees = new List<EmployeeInfo>();
        }
        public List<EmployeeInfo> Employees { get; set; }
        public string Type { get; set; }
    }

    public class WeekAttendanceModel
    {
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Date { get; set; }
    }

    public class DashBoardDeviceVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
    }



    public class CurrentUserAttendanceSummary
    {
        public EMHomeAttendanceInfo AttendanceInfo { get; set; }
        public EMonthlyAttendanceSummary MonthlySummary { get; set; }
    }

    public class ResellerInfoVm
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string PanNo { get; set; }
    }

    public class CompanyInfoVm
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Pan { get; set; }
        public string ResellerName { get; set; }
        public int DeviceCount { get; set; }
    }

    public class ResellerDashboarStockDeviceGridVm
    {
        public string ModelName { get; set; }
        public string SerialNo { get; set; }
        public string DeviceType { get; set; }
    }

    public class EmpDOBAndJoinVM
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Description { get; set; }
    }



}
