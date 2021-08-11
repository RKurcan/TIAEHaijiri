using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using RTech.Demo.Areas.Report.Controllers.Api;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Controllers.MobileApi
{
    public class MReportApiController : MRootController
    {
        #region Reporting Block
        [HttpGet]
        public MobileResult<SummaryDaywiseReport> GetMonthlyAttendanceSummary(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    //var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, monthId);
                    //DateTime fromDate = new DateTime(today.Year, today.Month, 1);
                    //DateTime toDate = new DateTime(today.Year, today.Month, daysInMonth);

                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(now.Year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    //int totalDays = (toDate - fromDate).Days + 1;
                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId, false).Data;

                    var selfResult = result.Where(x => x.EmployeeId == emp.Id).ToList();
                    List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();
                    reportData = selfResult;
                    var summaryLst = (reportData.GroupBy(i => i.EmployeeId)
                    .Select(i => new SummaryDaywiseReport()
                    {
                        //EmployeeCode = i.FirstOrDefault().EmployeeCode,
                        //EmployeeName = i.FirstOrDefault().EmployeeName,
                        //DepartmentName = i.FirstOrDefault().DepartmentName,
                        //DepartmentNamee = i.FirstOrDefault().DepartmentNamee,
                        TotalDays = daysInMonth.ToString(),
                        Absent = i.Where(j => j.Remark.ToLower() == "absent").Count().ToString(),
                        OfficeOut = i.Where(j => j.OfficeVisit.ToLower() == "yes").Count().ToString(),
                        Present = i.Where(j => j.Remark.ToLower() == "present").Count().ToString(),
                        PresentInHoliday = i.Where(j => j.Remark.ToLower() == "present" && j.Holiday.ToLower() == "yes").Count().ToString(),
                        PresentInDayOff = i.Where(j => j.Remark.ToLower() == "present" && j.Weekend.ToLower() == "yes").Count().ToString(),
                        Misc = i.Where(j => j.Remark.ToLower() == "misc").Count().ToString(),
                        Weekend = i.Where(j => j.Weekend.ToLower() == "yes").Count().ToString(),
                        //DutyDay = (daysInMonth - (i.Where(j => j.Remark.ToLower() == "weekend").Count() + i.Where(j => j.Holiday.ToLower() == "yes").Count())).ToString(),
                        EarlyIn = i.Where(j => j.EarlyIn != "").Count().ToString(),
                        EarlyOut = i.Where(j => j.EarlyOut != "").Count().ToString(),
                        LateIn = i.Where(j => j.LateIn != "").Count().ToString(),
                        LateOut = i.Where(j => j.LateOut != "").Count().ToString(),
                        Holiday = i.Where(j => j.Holiday.ToLower() == "yes").Count().ToString(),
                        Leave = i.Where(j => j.OnLeave.ToLower() == "yes").Count().ToString(),
                        Worked = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Actual.ToTimeSpan())).TotalHours.ToString("#00.00"),
                        Ot = GetOt(i),
                        Remarks = getRemarks(i.Where(j => j.OnLeave.ToLower() == "yes").ToList(), i.Where(j => j.Remark.ToLower() == "absent").ToList())
                    })).ToList();
                    return new MobileResult<SummaryDaywiseReport>()
                    {
                        Data = summaryLst.FirstOrDefault(),
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<SummaryDaywiseReport>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<SummaryDaywiseReport>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<SummaryDaywiseReport> GetMonthlyAttendanceSummary(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    //var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    //DateTime fromDate = new DateTime(today.Year, today.Month, 1);
                    //DateTime toDate = new DateTime(today.Year, today.Month, daysInMonth);

                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    //int totalDays = (toDate - fromDate).Days + 1;
                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId, false).Data;

                    var selfResult = result.Where(x => x.EmployeeId == emp.Id).ToList();
                    List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();
                    reportData = selfResult;
                    var summaryLst = (reportData.GroupBy(i => i.EmployeeId)
                    .Select(i => new SummaryDaywiseReport()
                    {
                        TotalDays = daysInMonth.ToString(),
                        Absent = i.Where(j => j.Remark.ToLower() == "absent").Count().ToString(),
                        OfficeOut = i.Where(j => j.OfficeVisit.ToLower() == "yes").Count().ToString(),
                        Present = i.Where(j => j.Remark.ToLower() == "present").Count().ToString(),
                        PresentInHoliday = i.Where(j => j.Remark.ToLower() == "present" && j.Holiday.ToLower() == "yes").Count().ToString(),
                        PresentInDayOff = i.Where(j => j.Remark.ToLower() == "present" && j.Weekend.ToLower() == "yes").Count().ToString(),
                        Misc = i.Where(j => j.Remark.ToLower() == "misc").Count().ToString(),
                        Weekend = i.Where(j => j.Weekend.ToLower() == "yes").Count().ToString(),
                        //DutyDay = (daysInMonth - (i.Where(j => j.Remark.ToLower() == "weekend").Count() + i.Where(j => j.Holiday.ToLower() == "yes").Count())).ToString(),
                        EarlyIn = i.Where(j => j.EarlyIn != "").Count().ToString(),
                        EarlyOut = i.Where(j => j.EarlyOut != "").Count().ToString(),
                        LateIn = i.Where(j => j.LateIn != "").Count().ToString(),
                        LateOut = i.Where(j => j.LateOut != "").Count().ToString(),
                        Holiday = i.Where(j => j.Holiday.ToLower() == "yes").Count().ToString(),
                        Leave = i.Where(j => j.OnLeave.ToLower() == "yes").Count().ToString(),
                        Worked = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Actual.ToTimeSpan())).TotalHours.ToString("#00.00"),
                        Ot = GetOt(i),
                        Remarks = getRemarks(i.Where(j => j.OnLeave.ToLower() == "yes").ToList(), i.Where(j => j.Remark.ToLower() == "absent").ToList())
                    })).ToList();
                    return new MobileResult<SummaryDaywiseReport>()
                    {
                        Data = summaryLst.FirstOrDefault(),
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<SummaryDaywiseReport>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<SummaryDaywiseReport>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyDetailReport>> GetMonthlyDetailReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);

                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(now.Year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId, false).Data;

                    List<EMMonthlyDetailReport> list = new List<EMMonthlyDetailReport>();
                    list = (from c in result.Where(x => x.EmployeeId == emp.Id).ToList()
                            select new EMMonthlyDetailReport()
                            {
                                Actual = c.Actual,
                                DayName = c.DayName,
                                Ot = c.Ot,
                                PunchTime = c.ActualTimeIn + "-" + c.ActualTimeOut,
                                ShiftTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                Remark = c.Remark,
                                WorkDate = c.WorkDate,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyDetailReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyDetailReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }

            }
            return new MobileResult<List<EMMonthlyDetailReport>>()
            {
                Data = null,
                Status = MobileResultStatus.InvalidToken,
                Message = "Invalid Token."
            };
        }
        [HttpGet]
        public MobileResult<List<EMMonthlyDetailReport>> GetMonthlyDetailReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);

                    DateTime startDate = new DateTime(year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId, false).Data;

                    List<EMMonthlyDetailReport> list = new List<EMMonthlyDetailReport>();
                    list = (from c in result.Where(x => x.EmployeeId == emp.Id).ToList()
                            select new EMMonthlyDetailReport()
                            {
                                Actual = c.Actual,
                                DayName = c.DayName,
                                Ot = c.Ot,
                                PunchTime = c.ActualTimeIn + "-" + c.ActualTimeOut,
                                ShiftTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                Remark = c.Remark,
                                WorkDate = c.WorkDate,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyDetailReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyDetailReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }

            }
            return new MobileResult<List<EMMonthlyDetailReport>>()
            {
                Data = null,
                Status = MobileResultStatus.InvalidToken,
                Message = "Invalid Token."
            };
        }

        [HttpGet]
        public MobileResult<List<EMTakenLeave>> GetMonthlyTakenLeaveReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(now.Year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId).Data;
                    List<EMTakenLeave> list = new List<EMTakenLeave>();
                    list = (from c in result.Where(x => x.EmployeeId == emp.Id && x.OnLeave == "Yes")
                            select new EMTakenLeave()
                            {
                                Date = c.WorkDate,
                                Day = c.DayName,
                                Reamrk = c.Remark,
                            }).ToList();
                    return new MobileResult<List<EMTakenLeave>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMTakenLeave>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }

            return new MobileResult<List<EMTakenLeave>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMTakenLeave>> GetMonthlyTakenLeaveReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime startDate = new DateTime(year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId).Data;
                    List<EMTakenLeave> list = new List<EMTakenLeave>();
                    list = (from c in result.Where(x => x.EmployeeId == emp.Id && x.OnLeave == "Yes")
                            select new EMTakenLeave()
                            {
                                Date = c.WorkDate,
                                Day = c.DayName,
                                Reamrk = c.Remark,
                            }).ToList();
                    return new MobileResult<List<EMTakenLeave>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMTakenLeave>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }

            return new MobileResult<List<EMTakenLeave>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMRemaningLeave>> GetRemaningLeaveReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                SFiscalYear fiscalYearServices = new SFiscalYear();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                int currentFiscalYearId = fiscalYearServices.List().Data.Where(x => x.BranchId == emp.BranchId && x.CurrentFiscalYear).FirstOrDefault().Id;
                if (emp != null)
                {
                    SLeaveReport reportService = new SLeaveReport("en");
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(now.Year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    var result = reportService.GetLeaveReportFromSp(startDate, endDate, (int)emp.BranchId, currentFiscalYearId).Data;
                    List<EMRemaningLeave> list = new List<EMRemaningLeave>();
                    list = (from c in result.Where(x => x.EmployeeId == emp.Id)
                            select new EMRemaningLeave()
                            {
                                Leave = c.LeaveName,
                                OpeningBalance = c.Balance,
                                Remaning = c.RemLeave,
                                Taken = c.TakenLeave,
                            }).ToList();
                    return new MobileResult<List<EMRemaningLeave>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMRemaningLeave>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }

            return new MobileResult<List<EMRemaningLeave>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }
        public MobileResult<List<EMRemaningLeave>> GetRemaningLeaveReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                SFiscalYear fiscalYearServices = new SFiscalYear();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                int currentFiscalYearId = fiscalYearServices.List().Data.Where(x => x.BranchId == emp.BranchId && x.CurrentFiscalYear).FirstOrDefault().Id;
                if (emp != null)
                {
                    SLeaveReport reportService = new SLeaveReport("en");
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime startDate = new DateTime(year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    var result = reportService.GetLeaveReportFromSp(startDate, endDate, (int)emp.BranchId, currentFiscalYearId).Data;
                    List<EMRemaningLeave> list = new List<EMRemaningLeave>();
                    list = (from c in result.Where(x => x.EmployeeId == emp.Id)
                            select new EMRemaningLeave()
                            {
                                Leave = c.LeaveName,
                                OpeningBalance = c.Balance,
                                Remaning = c.RemLeave,
                                Taken = c.TakenLeave,
                            }).ToList();
                    return new MobileResult<List<EMRemaningLeave>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMRemaningLeave>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }

            return new MobileResult<List<EMRemaningLeave>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyOTReport>> GetMonthlyOtReport(int empId, int monthId)
        {

            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(now.Year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId, false).Data;

                    List<EMMonthlyOTReport> list = new List<EMMonthlyOTReport>();
                    list = (from c in result
                            where c.Ot != "00:00" && c.Ot != ""
                            select new EMMonthlyOTReport()
                            {
                                Actual = c.ActualTimeIn + "-" + c.ActualTimeOut,
                                ActualTime = c.Actual,
                                Date = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                WorkTime = c.Standard,
                                DayName = c.DayName,
                                OT = c.Ot,
                            }).ToList();

                    TimeSpan otsum = new TimeSpan();
                    foreach (var item in list)
                    {
                        otsum = otsum + item.OT.ToTimeSpan();
                    }
                    foreach (var item in list)
                    {
                        item.OTSum = otsum.ToString(@"hh\:mm");
                        break;
                    }
                    return new MobileResult<List<EMMonthlyOTReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                        Token = null,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyOTReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyOTReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyOTReport>> GetMonthlyOtReport(int empId, int monthId, int year)
        {

            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime startDate = new DateTime(year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId, false).Data;

                    List<EMMonthlyOTReport> list = new List<EMMonthlyOTReport>();
                    list = (from c in result
                            where c.Ot != "00:00" && c.Ot != ""
                            select new EMMonthlyOTReport()
                            {
                                Actual = c.ActualTimeIn + "-" + c.ActualTimeOut,
                                ActualTime = c.Actual,
                                Date = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                WorkTime = c.Standard,
                                DayName = c.DayName,
                                OT = c.Ot,
                            }).ToList();

                    TimeSpan otsum = new TimeSpan();
                    foreach (var item in list)
                    {
                        otsum = otsum + item.OT.ToTimeSpan();
                    }
                    foreach (var item in list)
                    {
                        item.OTSum = otsum.ToString(@"hh\:mm");
                        break;
                    }
                    return new MobileResult<List<EMMonthlyOTReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                        Token = null,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyOTReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyOTReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMManualPunchRequest>> GetMonthlyManualPunchReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(now.Year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    SManualPunch manualPunchServices = new SManualPunch();
                    var manualPunches = manualPunchServices.List().Data.Where(x => x.EmployeeId == empId && DbFunctions.TruncateTime(x.DateTime) >= startDate && DbFunctions.TruncateTime(x.DateTime) <= endDate).ToList();
                    var result = (from c in manualPunches
                                  select new EMManualPunchRequest()
                                  {
                                      Date = c.DateTime.ToString("yyyy/MM/dd"),
                                      Time = c.DateTime.ToString(@"hh\:mm"),
                                      Remark = c.Remark,
                                  }).ToList();
                    return new MobileResult<List<EMManualPunchRequest>>()
                    {
                        Data = result,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMManualPunchRequest>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMManualPunchRequest>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }
        [HttpGet]
        public MobileResult<List<EMManualPunchRequest>> GetMonthlyManualPunchReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {

                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    SManualPunch manualPunchServices = new SManualPunch();
                    var manualPunches = manualPunchServices.List().Data.Where(x => x.EmployeeId == empId && DbFunctions.TruncateTime(x.DateTime) >= startDate && DbFunctions.TruncateTime(x.DateTime) <= endDate).ToList();
                    var result = (from c in manualPunches
                                  select new EMManualPunchRequest()
                                  {
                                      Date = c.DateTime.ToString("yyyy/MM/dd"),
                                      Time = c.DateTime.ToString(@"hh\:mm"),
                                      Remark = c.Remark,
                                  }).ToList();
                    return new MobileResult<List<EMManualPunchRequest>>()
                    {
                        Data = result,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMManualPunchRequest>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMManualPunchRequest>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMissingPunchReport>> GetMonthlyMissingPunchReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(now.Year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId, false).Data;
                    List<EMMissingPunchReport> list = new List<EMMissingPunchReport>();
                    list = (from c in result
                            where c.Remark == "Misc"
                            select new EMMissingPunchReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                ActualTimeOut = c.ActualTimeOut,
                                Remark = c.Remark
                            }).ToList();
                    return new MobileResult<List<EMMissingPunchReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMissingPunchReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMissingPunchReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }
        [HttpGet]
        public MobileResult<List<EMMissingPunchReport>> GetMonthlyMissingPunchReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime startDate = new DateTime(year, monthId, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(startDate, endDate, (int)emp.BranchId, false).Data;
                    List<EMMissingPunchReport> list = new List<EMMissingPunchReport>();
                    list = (from c in result
                            where c.Remark == "Misc"
                            select new EMMissingPunchReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                ActualTimeOut = c.ActualTimeOut,
                                Remark = c.Remark
                            }).ToList();
                    return new MobileResult<List<EMMissingPunchReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMissingPunchReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMissingPunchReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMOfficeVisitReport>> GetMonthlyOfficeVisitReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime fromDate = new DateTime(now.Year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    SOfficeVisit officeVisitServices = new SOfficeVisit();
                    var officeVisit = officeVisitServices.ListDetail().Data.Where(x => x.EmployeeId == emp.Id && DbFunctions.TruncateTime(x.OfficeVisit.From) >= fromDate && DbFunctions.TruncateTime(x.OfficeVisit.To) <= toDate).ToList();
                    var result = (from c in officeVisit
                                  select new EMOfficeVisitReport()
                                  {
                                      FromDate = c.OfficeVisit.From.ToString("yyyy/MM/dd"),
                                      Remark = c.OfficeVisit.Remark,
                                      ToDate = c.OfficeVisit.To.ToString("yyyy/MM/dd"),
                                  }).ToList();
                    return new MobileResult<List<EMOfficeVisitReport>>()
                    {
                        Data = result,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMOfficeVisitReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMOfficeVisitReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMOfficeVisitReport>> GetMonthlyOfficeVisitReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime fromDate = new DateTime(year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    SOfficeVisit officeVisitServices = new SOfficeVisit();
                    var officeVisit = officeVisitServices.ListDetail().Data.Where(x => x.EmployeeId == emp.Id && DbFunctions.TruncateTime(x.OfficeVisit.From) >= fromDate && DbFunctions.TruncateTime(x.OfficeVisit.To) <= toDate).ToList();
                    var result = (from c in officeVisit
                                  select new EMOfficeVisitReport()
                                  {
                                      FromDate = c.OfficeVisit.From.ToString("yyyy/MM/dd"),
                                      Remark = c.OfficeVisit.Remark,
                                      ToDate = c.OfficeVisit.To.ToString("yyyy/MM/dd"),
                                  }).ToList();
                    return new MobileResult<List<EMOfficeVisitReport>>()
                    {
                        Data = result,
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMOfficeVisitReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMOfficeVisitReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyLateReport>> GetMonthlyLateInReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime fromDate = new DateTime(now.Year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyLateReport> list = new List<EMMonthlyLateReport>();
                    list = (from c in result
                            where c.LateIn != "00:00" && c.LateIn != ""
                            select new EMMonthlyLateReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                LateIn = c.LateIn,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyLateReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyLateReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyLateReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyLateReport>> GetMonthlyLateInReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime fromDate = new DateTime(year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyLateReport> list = new List<EMMonthlyLateReport>();
                    list = (from c in result
                            where c.LateIn != "00:00" && c.LateIn != ""
                            select new EMMonthlyLateReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                LateIn = c.LateIn,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyLateReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyLateReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyLateReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyLateReport>> GetMonthlyLateOutReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime fromDate = new DateTime(now.Year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyLateReport> list = new List<EMMonthlyLateReport>();
                    list = (from c in result
                            where c.LateOut != "00:00" && c.LateOut != ""
                            select new EMMonthlyLateReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                LateOut = c.LateOut,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyLateReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyLateReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyLateReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyLateReport>> GetMonthlyLateOutReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime fromDate = new DateTime(year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyLateReport> list = new List<EMMonthlyLateReport>();
                    list = (from c in result
                            where c.LateOut != "00:00" && c.LateOut != ""
                            select new EMMonthlyLateReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                LateOut = c.LateOut,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyLateReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyLateReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyLateReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyEarlyReport>> GetMonthlyEarlyInReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime fromDate = new DateTime(now.Year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyEarlyReport> list = new List<EMMonthlyEarlyReport>();
                    list = (from c in result
                            where c.EarlyIn != "00:00" && c.EarlyIn != ""
                            select new EMMonthlyEarlyReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                EarlyIn = c.EarlyIn,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyEarlyReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyEarlyReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyEarlyReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyEarlyReport>> GetMonthlyEarlyInReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime fromDate = new DateTime(year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyEarlyReport> list = new List<EMMonthlyEarlyReport>();
                    list = (from c in result
                            where c.EarlyIn != "00:00" && c.EarlyIn != ""
                            select new EMMonthlyEarlyReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                EarlyIn = c.EarlyIn,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyEarlyReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyEarlyReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyEarlyReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyEarlyReport>> GetMonthlyEarlyOutReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime fromDate = new DateTime(now.Year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyEarlyReport> list = new List<EMMonthlyEarlyReport>();
                    list = (from c in result
                            where c.EarlyOut != "00:00" && c.EarlyOut != ""
                            select new EMMonthlyEarlyReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                EarlyOut = c.EarlyOut,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyEarlyReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyEarlyReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyEarlyReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyEarlyReport>> GetMonthlyEarlyOutReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime fromDate = new DateTime(year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyEarlyReport> list = new List<EMMonthlyEarlyReport>();
                    list = (from c in result
                            where c.EarlyOut != "00:00" && c.EarlyOut != ""
                            select new EMMonthlyEarlyReport()
                            {
                                WorkDate = c.WorkDate,
                                PlanedTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                ActualTimeIn = c.ActualTimeIn,
                                EarlyOut = c.EarlyOut,
                            }).ToList();
                    return new MobileResult<List<EMMonthlyEarlyReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyEarlyReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyEarlyReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyAbsentReport>> GetMonthlyAbsentReport(int empId, int monthId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    var today = System.DateTime.Now.Date;
                    int daysInMonth = DateTime.DaysInMonth(today.Year, monthId);
                    DateTime now = DateTime.Now;
                    DateTime fromDate = new DateTime(now.Year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyAbsentReport> list = new List<EMMonthlyAbsentReport>();
                    list = (from c in result
                            where c.Remark == "Absent"
                            select new EMMonthlyAbsentReport()
                            {
                                WorkDate = c.WorkDate,
                                ShiftTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                PunchTime = c.ActualTimeIn + "-" + c.ActualTimeOut,
                                Actual = c.Actual,
                                Remark = c.Remark
                            }).ToList();
                    return new MobileResult<List<EMMonthlyAbsentReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyAbsentReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyAbsentReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

        [HttpGet]
        public MobileResult<List<EMMonthlyAbsentReport>> GetMonthlyAbsentReport(int empId, int monthId, int year)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    SMonthlyWiseReport reportService = new SMonthlyWiseReport("en");
                    reportService.FilteredEmployeeIDs = Common.intToArray(empId);
                    int daysInMonth = DateTime.DaysInMonth(year, monthId);
                    DateTime fromDate = new DateTime(year, monthId, 1);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                    var result = reportService.GetAttendanceReportFromSp(fromDate, toDate, (int)emp.BranchId, false).Data;
                    List<EMMonthlyAbsentReport> list = new List<EMMonthlyAbsentReport>();
                    list = (from c in result
                            where c.Remark == "Absent"
                            select new EMMonthlyAbsentReport()
                            {
                                WorkDate = c.WorkDate,
                                ShiftTime = c.PlannedTimeIn + "-" + c.PlannedTimeOut,
                                DayName = c.DayName,
                                PunchTime = c.ActualTimeIn + "-" + c.ActualTimeOut,
                                Actual = c.Actual,
                                Remark = c.Remark
                            }).ToList();
                    return new MobileResult<List<EMMonthlyAbsentReport>>()
                    {
                        Data = list,
                        Message = "",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<List<EMMonthlyAbsentReport>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMMonthlyAbsentReport>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }
        #endregion

        #region Functions Block
        private string GetOt(IGrouping<int, MonthlyWiseReport> i)
        {
            TimeSpan sumTillNowTimeSpan = TimeSpan.Zero;
            foreach (var item in i)
            {
                sumTillNowTimeSpan += item.Ot.ToTimeSpan();
            }
            double hours = sumTillNowTimeSpan.Hours + (sumTillNowTimeSpan.Days * 24);
            string time = hours + ":" + sumTillNowTimeSpan.Minutes;
            return time;
        }
        private string getRemarks(List<MonthlyWiseReport> leaveLst, List<MonthlyWiseReport> absentLst)
        {
            return joinStringForRemarks(leaveLst);

        }
        private string joinStringForRemarks(List<MonthlyWiseReport> list)
        {
            if (list.Count() < 1)
            {
                return "";
            }
            string opDate = RiddhaSession.OperationDate;
            var dates = (from c in list
                         select c.WorkDate.ToDateTime()).ToList();
            List<DateRange> dateRanges = GetDateRanges(dates);
            IList<string> result = new List<string> { };
            foreach (var item in dateRanges)
            {
                if (item.Start == item.End)
                {
                    result.Add(item.Start.Day.ToString());
                }
                else
                {
                    result.Add(item.Start.Day.ToString() + "-" + item.End.Day.ToString());
                }
            }
            return string.Join(",", result);
        }
        private List<DateRange> GetDateRanges(List<DateTime> dates)
        {
            if (dates == null || !dates.Any()) return null;
            dates = dates.OrderBy(x => x.Date).ToList();
            var dateRangeList = new List<DateRange>();
            DateRange dateRange = null;
            for (var i = 0; i < dates.Count; i++)
            {
                if (dateRange == null)
                {
                    dateRange = new DateRange { Start = dates[i] };
                }
                if (i == dates.Count - 1 || dates[i].Date.AddDays(1) != dates[i + 1].Date)
                {
                    dateRange.End = dates[i].Date;
                    dateRangeList.Add(dateRange);
                    dateRange = null;
                }
            }
            return dateRangeList;
        }
        #endregion
    }

}
