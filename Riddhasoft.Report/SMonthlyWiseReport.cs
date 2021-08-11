using Riddhasoft.Attendance.Entities;
using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Globals.Conversion;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.HumanResource.Management.Report
{
    public class SMonthlyWiseReport
    {
        RiddhaDBContext db = null;
        private string currentLanguage = "";
        public string CurrentOperationDate = "";
        public int[] FilteredEmployeeIDs { get; set; }
        public TimeSpan minimumOTHour { get; set; }


        public SMonthlyWiseReport(string currentLanguage = "")
        {
            db = new DB.RiddhaDBContext();

            this.currentLanguage = currentLanguage;
        }
        private void calculateRemaining(MonthlyWiseReport eachRow, DateTime FromDate, List<EAttendanceLog> attlog, MonthlyWiseReport lastRow = null)
        {
            var a = (lastRow ?? new MonthlyWiseReport()).Standard;
            var b = eachRow.Standard;
            if (eachRow.FourPunch && eachRow.RoundTheClock == false)
            {//todo import shift condition
                var log = attlog.Where(x => x.EmployeeId == eachRow.EmployeeId && (x.DateTime.Date) == (FromDate) && x.IsDelete == false).ToList();
                TimeSpan breakTime = TimeSpan.Parse(eachRow.PlannedLunchIn);
                FromDate = FromDate.AddTicks(breakTime.Ticks);
                log = log.Where(x => x.DateTime >= FromDate).ToList();
                if (FromDate.ToString(@"HH\:mm") == "00:00")
                {
                    if (log.Count > 1)
                    {
                        eachRow.ActualLunchOut = log.OrderBy(x => x.DateTime).Skip(1).Take(1).First().DateTime.ToString(@"HH\:mm");
                    }
                    if (log.Count > 2)
                    {
                        eachRow.ActualLunchIn = log.OrderBy(x => x.DateTime).Skip(2).Take(2).First().DateTime.ToString(@"HH\:mm");
                    }
                }
                else
                {
                    if (log.Count > 0)
                        eachRow.ActualLunchOut = log.OrderBy(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");

                    if (log.Count > 1)
                        eachRow.ActualLunchIn = log.OrderBy(x => x.DateTime).Skip(1).Take(1).First().DateTime.ToString(@"HH\:mm");
                }

            }
            if (eachRow.ActualTimeIn == "00:00")
            {
                eachRow.ActualTimeOut = "00:00";
            }
            if (eachRow.RoundTheClock)
            {
                //time in same day last punch
                var log = attlog.Where(x => x.EmployeeId == eachRow.EmployeeId && (x.DateTime.Date) == (FromDate) && x.IsDelete == false).ToList();
                TimeSpan breakTime = TimeSpan.Parse(eachRow.PlannedLunchIn);
                FromDate = FromDate.AddTicks(breakTime.Ticks);
                log = log.Where(x => x.DateTime >= FromDate).ToList();
                if (log.Count > 0)
                {

                    eachRow.ActualTimeIn = log.OrderByDescending(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");

                    //time out next day first punch

                    log = attlog.Where(x => x.EmployeeId == eachRow.EmployeeId && (x.DateTime.Date) == (FromDate.AddDays(1)) && x.IsDelete == false).ToList();


                    log = log.Where(x => x.DateTime >= FromDate).ToList();
                    if (log.Count > 0)
                    {
                        eachRow.ActualTimeOut = log.OrderBy(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");

                    }
                    else
                    {
                        eachRow.ActualTimeOut = "00:00";
                    }
                }
                else
                {
                    eachRow.ActualTimeIn = "00:00";
                }
            }
            if (lastRow != null && lastRow.RoundTheClock)
            {
                if (eachRow.ActualTimeIn == lastRow.ActualTimeOut)
                {
                    var log = attlog.Where(x => x.EmployeeId == eachRow.EmployeeId && (x.DateTime.Date) == (FromDate) && x.IsDelete == false).ToList();
                    TimeSpan breakTime = TimeSpan.Parse(eachRow.PlannedLunchIn);
                    FromDate = FromDate.AddTicks(breakTime.Ticks);
                    log = log.Where(x => x.DateTime >= FromDate).ToList();
                    if (log.Count > 1)
                    {
                        eachRow.ActualTimeIn = log.OrderBy(x => x.DateTime).Skip(1).Take(1).First().DateTime.ToString(@"HH\:mm");
                        eachRow.ActualTimeOut = log.OrderByDescending(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");
                    }
                    else
                    {
                        eachRow.ActualTimeIn = "00:00";
                    }
                }
            }
        }

        public ServiceResult<List<MonthlyWiseReport>> GetAttendanceReportFromSp(DateTime FromDate, DateTime ToDate, int branchId, bool OTV2 = false, string branches = "")
        {
            string emp = string.Join(",", FilteredEmployeeIDs);
            var result = db.SP_GET_ATTENDACE_REPORT(FromDate, ToDate, branchId, currentLanguage, emp, branches);
            if (FilteredEmployeeIDs.Length > 0)
            {
                result = (from c in result
                          join d in FilteredEmployeeIDs
                          on c.EmployeeId equals d
                          select c
                                      ).ToList();
            }
            List<MonthlyWiseReport> rptList = new List<MonthlyWiseReport>();
            rptList = (from c in result
                       select new MonthlyWiseReport(OTV2,minimumOTHour)
                       {
                           ActualTimeIn = c.PUNCHIN == null ? "00:00" : c.PUNCHIN.ToDateTime().ToString(@"HH\:mm"),
                           ActualTimeOut = c.PUNCHOUT == null ? "00:00" : (c.PUNCHIN == c.PUNCHOUT ? "00:00" : c.PUNCHOUT.ToDateTime().ToString(@"HH\:mm")),
                           ActualLunchIn = c.BREAKIN == null ? "00:00" : (c.BREAKIN == c.BREAKIN ? "00:00" : c.BREAKIN.ToDateTime().ToString(@"HH\:mm")),
                           ActualLunchOut = c.BREAKOUT == null ? "00:00" : (c.BREAKOUT == c.BREAKOUT ? "00:00" : c.BREAKOUT.ToDateTime().ToString(@"HH\:mm")),
                           EmployeeCode = c.EmployeeCode,
                           EmployeeDeviceCode = c.EmployeeDeviceCode,
                           EmployeeId = c.EmployeeId,
                           EmployeeName = currentLanguage == "ne" && c.EmployeeNameNp != null ? c.EmployeeNameNp : c.EmployeeName,
                           FourPunch = c.FourPunch,
                           Holiday = string.IsNullOrEmpty(c.HOLIDAYNAME) ? "No" : "Yes",
                           LeaveName = c.LEAVENAME,
                           OfficeVisit = c.OFFICEVISIT,
                           MobileNo = c.MobileNo,
                           MultiplePunch = c.MultiplePunch,
                           NoPunch = c.NoPunch,
                           OnLeave = string.IsNullOrEmpty(c.LEAVENAME) ? "No" : "Yes",
                           PlannedTimeIn = c.PlannedTimeIn == null ? "00:00" : c.PlannedTimeIn.ToString(@"hh\:mm"),
                           PlannedTimeOut = c.PlannedTimeOut == null ? "00:000" : c.PlannedTimeOut.ToString(@"hh\:mm"),
                           PlannedLunchIn = c.PLANNEDLUNCHSTART == null ? "00:000" : c.PLANNEDLUNCHSTART.ToString(@"hh\:mm"),
                           PlannedLunchOut = c.PLANNEDLUNCHEND == null ? "00:000" : c.PLANNEDLUNCHEND.ToString(@"hh\:mm"),
                           ShiftStartGrace = c.EarlyGrace == null ? "00:00" : c.EarlyGrace.ToString(@"hh\:mm"),
                           ShiftEndGrace = c.LateGrace == null ? "00:00" : c.LateGrace.ToString(@"hh\:mm"),
                           ShiftName = "",//to get from db
                           ShiftTypeId = c.ShiftTypeId,
                           ShiftType = c.ShiftType,
                           SinglePunch = c.SinglePunch,
                           TwoPunch = c.TwoPunch,
                           Weekend = c.WEEKEND,
                           WorkDate = c.EngDate.ToString("yyyy/MM/dd"),
                           NepDate = c.NepDate,
                           HolidayName = c.HOLIDAYNAME,
                           DepartmentCode = c.DepartmentCode,
                           //DepartmentName =c.DepartmentCode +" - "+ c.DepartmentName,
                           DepartmentName = c.DepartmentCode + " - " + (currentLanguage == "ne" && c.DepartmentNameNp != null ? c.DepartmentNameNp : c.DepartmentName),
                           DepartmentNamee = c.DepartmentName,
                           SectionCode = c.SectionCode,
                           SectionName = c.SectionName,
                           //DayName = c.EngDate.DayOfWeek.ToString(),
                           DayName = currentLanguage == "ne" ? Extension.ConvertEngDayNameToNep(c.EngDate.DayOfWeek.ToString()) : c.EngDate.DayOfWeek.ToString(),
                           Kaj = c.KAJ,
                           LeaveDescription = c.LeaveDescription,
                       }).ToList();


            //filter employee for fourpunch
            //var attlog = db.AttendanceLog.Where(x => DbFunctions.TruncateTime(x.DateTime) >= DbFunctions.TruncateTime(FromDate) && DbFunctions.TruncateTime(x.DateTime) <= DbFunctions.TruncateTime(ToDate)).ToList();
            var attlog = db.SP_GET_ATTENDACE_LOG_RAW_DATA(branchId, FromDate, ToDate);
            var fourPunchValidRptList = rptList.Where(x => x.FourPunch).ToList();
            var fourPunchValidEmployee = fourPunchValidRptList.Select(x => x.EmployeeId).Distinct().ToArray();
            //attlog = (from c in attlog
            //          join d in fourPunchValidEmployee on c.EmployeeId equals d

            //          select c
            //).ToList();

            int rowIndex = 0;
            foreach (var item in rptList)
            {
                if (rowIndex == 0)
                {
                    calculateRemaining(item, item.WorkDate.ToDateTime(), attlog);
                }
                else
                {
                    calculateRemaining(item, item.WorkDate.ToDateTime(), attlog, rptList[rowIndex - 1]);
                }
                rowIndex++;
            }
            ///get attlog for particulardays
            ///
            return new ServiceResult<List<MonthlyWiseReport>>() { Data = rptList };
        }

        public ServiceResult<List<MonthlyWiseReport>> GetAttendanceReportFromSpForRDLC(DateTime FromDate, DateTime ToDate, int branchId, bool OTV2 = false, string branches = "")
        {
            string emp = string.Join(",", FilteredEmployeeIDs);
            var result = db.SP_GET_ATTENDACE_REPORT(FromDate, ToDate, branchId, currentLanguage, emp, branches);
            if (FilteredEmployeeIDs.Length > 0)
            {
                result = (from c in result
                          join d in FilteredEmployeeIDs
                          on c.EmployeeId equals d
                          select c
                                      ).ToList();
            }
            List<MonthlyWiseReport> rptList = new List<MonthlyWiseReport>();

            rptList = (from c in result
                       select new MonthlyWiseReport(OTV2,minimumOTHour)
                       {
                           ActualTimeIn = c.PUNCHIN == null ? "00:00" : c.PUNCHIN.ToDateTime().ToString(@"HH\:mm"),
                           ActualTimeOut = c.PUNCHOUT == null ? "00:00" : (c.PUNCHIN == c.PUNCHOUT ? "00:00" : c.PUNCHOUT.ToDateTime().ToString(@"HH\:mm")),
                           ActualLunchIn = c.BREAKIN == null ? "00:00" : (c.BREAKIN == c.BREAKIN ? "00:00" : c.BREAKIN.ToDateTime().ToString(@"HH\:mm")),
                           ActualLunchOut = c.BREAKOUT == null ? "00:00" : (c.BREAKOUT == c.BREAKOUT ? "00:00" : c.BREAKOUT.ToDateTime().ToString(@"HH\:mm")),
                           EmployeeCode = c.EmployeeCode,
                           EmployeeDeviceCode = c.EmployeeDeviceCode,
                           EmployeeId = c.EmployeeId,
                           //EmployeeName = c.EmployeeName,
                           EmployeeName = currentLanguage == "ne" && c.EmployeeNameNp != null ? c.EmployeeNameNp : c.EmployeeName,
                           FourPunch = c.FourPunch,
                           Holiday = string.IsNullOrEmpty(c.HOLIDAYNAME) ? "No" : "Yes",
                           LeaveName = c.LEAVENAME,
                           OfficeVisit = c.OFFICEVISIT,
                           MobileNo = c.MobileNo,
                           MultiplePunch = c.MultiplePunch,
                           NoPunch = c.NoPunch,
                           OnLeave = string.IsNullOrEmpty(c.LEAVENAME) ? "No" : "Yes",
                           PlannedTimeIn = c.PlannedTimeIn == null ? "00:00" : c.PlannedTimeIn.ToString(@"hh\:mm"),
                           PlannedTimeOut = c.PlannedTimeOut == null ? "00:000" : c.PlannedTimeOut.ToString(@"hh\:mm"),
                           PlannedLunchIn = c.PLANNEDLUNCHSTART == null ? "00:000" : c.PLANNEDLUNCHSTART.ToString(@"hh\:mm"),
                           PlannedLunchOut = c.PLANNEDLUNCHEND == null ? "00:000" : c.PLANNEDLUNCHEND.ToString(@"hh\:mm"),
                           ShiftStartGrace = c.EarlyGrace == null ? "00:00" : c.EarlyGrace.ToString(@"hh\:mm"),
                           ShiftEndGrace = c.LateGrace == null ? "00:00" : c.LateGrace.ToString(@"hh\:mm"),
                           ShiftName = "",//to get from db
                           ShiftTypeId = c.ShiftTypeId,
                           SinglePunch = c.SinglePunch,
                           TwoPunch = c.TwoPunch,
                           Weekend = c.WEEKEND,
                           //WorkDate = c.EngDate.ToString("yyyy/MM/dd"),
                           WorkDate = CurrentOperationDate == "ne" ? c.NepDate : c.EngDate.ToString("yyyy/MM/dd"),
                           NepDate = c.NepDate,
                           HolidayName = c.HOLIDAYNAME,
                           DepartmentCode = c.DepartmentCode,
                           //DepartmentName =c.DepartmentCode +" - "+ c.DepartmentName,
                           DepartmentName = c.DepartmentCode + " - " + (currentLanguage == "ne" && c.DepartmentNameNp != null ? c.DepartmentNameNp : c.DepartmentName),
                           DepartmentNamee = c.DepartmentName,
                           SectionCode = c.SectionCode,
                           SectionName = c.SectionName,
                           //DayName = c.EngDate.DayOfWeek.ToString(),
                           DayName = currentLanguage == "ne" ? Extension.ConvertEngDayNameToNep(c.EngDate.DayOfWeek.ToString()) : c.EngDate.DayOfWeek.ToString(),
                           Kaj = c.KAJ,

                       }).ToList();


            //filter employee for fourpunch
            //var attlog = db.AttendanceLog.Where(x => DbFunctions.TruncateTime(x.DateTime) >= DbFunctions.TruncateTime(FromDate) && DbFunctions.TruncateTime(x.DateTime) <= DbFunctions.TruncateTime(ToDate)).ToList();
            var attlog = db.SP_GET_ATTENDACE_LOG_RAW_DATA(branchId, FromDate, ToDate);
            var fourPunchValidRptList = rptList.Where(x => x.FourPunch).ToList();
            var fourPunchValidEmployee = fourPunchValidRptList.Select(x => x.EmployeeId).Distinct().ToArray();
            //attlog = (from c in attlog
            //          join d in fourPunchValidEmployee on c.EmployeeId equals d

            //          select c
            //).ToList();

            int rowIndex = 0;
            foreach (var item in rptList)
            {
                if (rowIndex == 0)
                {
                    calculateRemaining(item, item.WorkDate.ToDateTime(), attlog);
                }
                else
                {
                    calculateRemaining(item, item.WorkDate.ToDateTime(), attlog, rptList[rowIndex - 1]);
                }
                rowIndex++;

            }

            ///get attlog for particulardays
            ///
            return new ServiceResult<List<MonthlyWiseReport>>() { Data = rptList };
        }

        public ServiceResult<List<MonthlyRosterReport>> GetMonthlyRosterReportFromSp(DateTime FromDate, DateTime ToDate, int branchId, string empIds, string sectionIds, string deptIds)
        {
            var result = db.SP_GET_EMPLOYEEWISE_ROSTER_REPORT(FromDate, ToDate, branchId, currentLanguage, empIds, sectionIds, deptIds);
            List<MonthlyRosterReport> rptList = new List<MonthlyRosterReport>();

            rptList = (from c in result
                       select new MonthlyRosterReport()
                       {
                           EmployeeId = c.EmployeeId,
                           EmployeeName = c.EmployeeName,
                           ShiftName = c.ShiftName,
                           ShiftCode = c.ShiftCode,
                           NepDate = c.NepDate,
                           DepartmentName = c.DepartmentCode + " - " + c.DepartmentName,
                           SectionName = c.SectionCode + " - " + c.SectionName,
                           EngDate = c.EngDate.ToString("yyyy/MM/dd")
                       }
                         ).ToList();

            return new ServiceResult<List<MonthlyRosterReport>>() { Data = rptList };
        }
        public ServiceResult<int[]> GetEmpIdsForReportParam(string deps, string secs, string emps)
        {
            ServiceResult<int[]> result = new ServiceResult<int[]>();
            if (emps != null)
            {
                result.Data = Array.ConvertAll(emps.Split(','), s => int.Parse(s));
                return result;
            }
            var empQuery = db.Employee;
            if (deps == null)
            {
                result.Data = new int[] { };
                return result;
            }
            else
            {
                if (secs != null)
                {
                    int[] secIds = Array.ConvertAll(secs.Split(','), s => int.Parse(s));
                    result.Data = (from c in empQuery
                                   join d in secIds on c.SectionId equals d
                                   select c.Id).ToArray();
                    return result;
                }
                else
                {
                    int[] depIds = Array.ConvertAll(deps.Split(','), s => int.Parse(s));
                    result.Data = (from c in empQuery
                                   join d in depIds on c.Section.DepartmentId equals d
                                   select c.Id).ToArray();
                    return result;
                }
            }
        }

        public ServiceResult<List<MultipunchReportViewModel>> GetMultiPunch(DateTime FromDate, DateTime ToDate, int branchId)
        {
            string emp = string.Join(",", FilteredEmployeeIDs);
            List<MultipunchReportViewModel> reportData = new List<MultipunchReportViewModel>();
            SEmployee empServices = new SEmployee();
            var employeeList = (from c in empServices.List().Data.Where(x => x.BranchId == branchId)
                                join d in FilteredEmployeeIDs
                                        on c.Id equals d
                                select c
                                );
            var employeeWithFixedShift = employeeList.Where(x => x.ShiftTypeId == 0).ToList();
            var EmployeeWithDynamicShift = employeeList.Where(x => x.ShiftTypeId != 0);
            var EmployeFixedShiftData = (from c in employeeWithFixedShift
                                         join d in db.EmployeeShitList.ToList()
                                         on c.Id equals d.EmployeeId
                                         into joined
                                         //join e in db.GLogData.ToList() on c.code into aaaa
                                         from j in joined.DefaultIfEmpty(new EEmployeeShitList())
                                         select new MultipunchReportViewModel()
                                         {
                                             EmployeeId = c.Id,
                                             EmployeeDeviceCode = c.DeviceCode,
                                             EmployeeCode = c.Code,
                                             EmployeeName = c.Name,
                                             DepartmentCode = c.Section == null ? "" : c.Section.Department.Code,
                                             DepartmentName = c.Section == null ? "" : c.Section.Department.Name,
                                         }
                        ).ToList();

            List<DateTime> workdays = new List<DateTime>();
            while (FromDate <= ToDate)
            {
                foreach (var item in EmployeFixedShiftData)
                {
                    var eachRow = new MultipunchReportViewModel()
                    {
                        WorkDate = FromDate.ToString("yyyy/MM/dd"),
                        EmployeeId = item.EmployeeId,
                        EmployeeDeviceCode = item.EmployeeDeviceCode,
                        EmployeeCode = item.EmployeeCode,
                        EmployeeName = item.EmployeeName,
                        DepartmentCode = item.DepartmentCode,
                        DepartmentName = item.DepartmentName,
                    };

                    calculateRemaining(eachRow, FromDate, ToDate, branchId);
                    reportData.Add(eachRow);
                }
                foreach (var item in EmployeeWithDynamicShift)
                {
                    var roster = db.Roster.Where(x => x.EmployeeId == item.Id && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(FromDate)).FirstOrDefault();
                    var eachRow = new MultipunchReportViewModel()
                    {
                        WorkDate = FromDate.ToString("yyyy/MM/dd"),
                        EmployeeId = item.Id,
                        EmployeeCode = item.Code,
                        EmployeeName = item.Name,
                        EmployeeDeviceCode = item.DeviceCode,
                        DepartmentCode = item.Section == null ? "" : item.Section.Department.Code,
                        DepartmentName = item.Section == null ? "" : item.Section.Department.Name,
                    };
                    calculateRemaining(eachRow, FromDate, ToDate, branchId);
                    reportData.Add(eachRow);
                }
                FromDate = FromDate.AddDays(1);
            }
            return new Riddhasoft.Services.Common.ServiceResult<List<MultipunchReportViewModel>>()
            {
                Data = reportData,
                Status = ResultStatus.Ok
            };
        }
        private void calculateRemaining(MultipunchReportViewModel eachRow, DateTime FromDate, DateTime ToDate, int branchId)
        {
            //var log = db.GLogData.Where(x => x.EnrollId == eachRow.EmployeeId && DbFunctions.TruncateTime(x.DateTime) == DbFunctions.TruncateTime(FromDate) && x.IsDelete == false).ToList();
            var attlog = db.SP_GET_ATTENDACE_LOG_RAW_DATA(branchId, FromDate, ToDate);
            var log = attlog.Where(x => x.EmployeeId == eachRow.EmployeeId).ToList();
            foreach (var item in log)
            {
                eachRow.PunchTime += "," + item.DateTime.ToString(@"HH\:mm");
            }
        }
    }
}
