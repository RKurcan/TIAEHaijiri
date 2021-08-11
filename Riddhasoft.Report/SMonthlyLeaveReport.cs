using Riddhasoft.Employee.Entities;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HumanResource.Management.Report
{
    public class SMonthlyLeaveReport
    {
        Riddhasoft.DB.RiddhaDBContext db;
        public SMonthlyLeaveReport()
        {
            db = new DB.RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<List<MonthlyWiseReport>> Get(DateTime FromDate, DateTime ToDate, List<EEmployee> empList)
        {
            List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();
            var employeeList = empList;
            var employeeWithFixedShift = employeeList.Where(x => x.ShiftTypeId == 0).ToList();
            var EmployeeWithDynamicShift = employeeList.Where(x => x.ShiftTypeId != 0);
            var EmployeFixedShiftData = (from c in employeeWithFixedShift
                                         join d in db.EmployeeShitList.ToList()
                                         on c.Id equals d.EmployeeId
                                         into joined
                                         //join e in db.GLogData.ToList() on c.code into aaaa
                                         from j in joined.DefaultIfEmpty(new EEmployeeShitList())
                                         select new MonthlyWiseReport()
                                         {
                                             EmployeeId = c.Id,
                                             EmployeeDeviceCode = c.DeviceCode,
                                             EmployeeCode = c.Code,
                                             EmployeeName = c.Name,
                                             NoPunch = c.NoPunch,
                                             SinglePunch = c.SinglePunch,
                                             TwoPunch = c.TwoPunch,
                                             MultiplePunch = c.MultiplePunch,
                                             FourPunch = c.FourPunch,
                                             DepartmentCode = c.Section == null ? "" : c.Section.Department.Code,
                                             DepartmentName = c.Section == null ? "" : c.Section.Department.Name,
                                             PlannedTimeIn = j.Shift == null ? "00:00" : j.Shift.ShiftStartTime.ToString(@"hh\:mm"),
                                             PlannedTimeOut = j.Shift == null ? "00:00" : j.Shift.ShiftEndTime.ToString(@"hh\:mm"),

                                         }
                        ).ToList();

            List<DateTime> workdays = new List<DateTime>();
            while (FromDate <= ToDate)
            {
                foreach (var item in EmployeFixedShiftData)
                {
                    var eachRow = new MonthlyWiseReport()
                    {
                        WorkDate = FromDate.ToString("yyyy/MM/dd"),
                        EmployeeId = item.EmployeeId,
                        EmployeeDeviceCode = item.EmployeeDeviceCode,
                        EmployeeCode = item.EmployeeCode,
                        EmployeeName = item.EmployeeName,
                        DepartmentCode = item.DepartmentCode,
                        DepartmentName = item.DepartmentName,
                        PlannedTimeIn = item.PlannedTimeIn,
                        PlannedTimeOut = item.PlannedTimeOut,
                        NoPunch = item.NoPunch,
                        SinglePunch = item.SinglePunch,
                        TwoPunch = item.TwoPunch,
                        MultiplePunch = item.MultiplePunch,
                        FourPunch = item.FourPunch,
                    };

                    calculateRemaining(eachRow, FromDate);
                    reportData.Add(eachRow);
                }
                foreach (var item in EmployeeWithDynamicShift)
                {
                    var roster = db.Roster.Where(x => x.EmployeeId == item.Id && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(FromDate)).FirstOrDefault();
                    var eachRow = new MonthlyWiseReport()
                    {
                        WorkDate = FromDate.ToString("yyyy/MM/dd"),
                        EmployeeId = item.Id,
                        EmployeeCode = item.Code,
                        EmployeeName = item.Name,
                        EmployeeDeviceCode = item.DeviceCode,
                        DepartmentCode = item.Section == null ? "" : item.Section.Department.Code,
                        DepartmentName = item.Section == null ? "" : item.Section.Department.Name,
                        ShiftTypeId = item.ShiftTypeId ?? 1,
                        PlannedTimeIn = roster == null ? "00:00" : roster.Shift.ShiftStartTime.ToString(@"hh\:mm"),
                        PlannedTimeOut = roster == null ? "00:00" : roster.Shift.ShiftEndTime.ToString(@"hh\:mm"),
                        NoPunch = item.NoPunch,
                        SinglePunch = item.SinglePunch,
                        TwoPunch = item.TwoPunch,
                        MultiplePunch = item.MultiplePunch,
                        FourPunch = item.FourPunch,
                    };
                    calculateRemaining(eachRow, FromDate);
                    reportData.Add(eachRow);
                }
                FromDate = FromDate.AddDays(1);
            }
            return new Riddhasoft.Services.Common.ServiceResult<List<MonthlyWiseReport>>()
            {
                Data = reportData,
                Status = ResultStatus.Ok
            };
        }
        private void calculateRemaining(MonthlyWiseReport eachRow, DateTime FromDate)
        {
            eachRow.ActualTimeIn = new TimeSpan(0).ToString(@"hh\:mm");
            eachRow.ActualTimeOut = new TimeSpan(0).ToString(@"hh\:mm");
            if (!eachRow.NoPunch)
            {
                //var log = db.GLogData.Where(x => x.EnrollId == eachRow.EmployeeDeviceCode && DbFunctions.TruncateTime(x.DateTime) == DbFunctions.TruncateTime(FromDate) && x.IsDelete == false).ToList();
                var log = db.AttendanceLog.Where(x => x.EmployeeId == eachRow.EmployeeId && DbFunctions.TruncateTime(x.DateTime) == DbFunctions.TruncateTime(FromDate)).ToList();
                //TimeSpan breakTime = TimeSpan.Parse(eachRow.PlannedBreakOut);
                if (log != null && log.Count() > 0)
                {
                    eachRow.ActualTimeIn = log.OrderBy(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");
                    if (eachRow.NoPunch == false && eachRow.SinglePunch == false && log.Count > 1)
                        eachRow.ActualTimeOut = log.OrderByDescending(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");
                }
                //if (eachRow.FourPunch)
                //{//todo import shift condition
                //    FromDate = FromDate.AddTicks(breakTime.Ticks);
                //    log = log.Where(x => x.DateTime >= FromDate).ToList();
                //    if (log.Count > 0)
                //        eachRow.BreakOut = log.OrderBy(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");
                //    if (log.Count > 1)
                //        eachRow.BreakIn = log.OrderBy(x => x.DateTime).Skip(1).Take(1).First().DateTime.ToString(@"HH\:mm");
                //}

            }

            #region leave Calculation Block

            if (eachRow.Absent == "Yes")
            {
                //
                calculateLeaveHoliday(eachRow, FromDate);
            }
            else
            {
                if (eachRow.ActualTimeIn != "00:00" && eachRow.ActualTimeOut != "00:00")
                {
                    calculateLeaveHoliday(eachRow, FromDate);
                }
            }
            #endregion
        }
        private void calculateLeaveHoliday(AttendanceReportDetailViewModel eachRow, DateTime FromDate)
        {
            var curdate = DateTime.Parse(eachRow.WorkDate);
            // var officeOuts = db.OfficeInOut.Where(x => x.EmployeeId == eachRow.EmployeeId && DbFunctions.TruncateTime(x.From) <= DbFunctions.TruncateTime(curdate) && x.To >= DbFunctions.TruncateTime(curdate)).FirstOrDefault();
            //if (officeOuts != null)
            //{
            //    eachRow.OnOfficeOut = "Yes";
            //    eachRow.LeaveName = "[OO] " + officeOuts.Remark;
            //    eachRow.ActualTimeIn = eachRow.PlannedTimeIn;
            //    eachRow.ActualTimeOut = eachRow.PlannedTimeOut;
            //}

            var leaves = db.LeaveApplication.Where(x => x.EmployeeId == eachRow.EmployeeId && DbFunctions.TruncateTime(x.From) <= DbFunctions.TruncateTime(curdate) && x.To >= DbFunctions.TruncateTime(curdate)).FirstOrDefault();
            if (leaves != null)
            {
                eachRow.OnLeave = "Yes";
                eachRow.LeaveName = leaves.LeaveMaster.Name;

            }
            else
            {
                var holiday = db.HolidayDetails.Where(x => DbFunctions.TruncateTime(x.BeginDate) <= DbFunctions.TruncateTime(curdate) && x.EndDate >= DbFunctions.TruncateTime(curdate)).FirstOrDefault();
                if (holiday != null)
                {
                    var employeeHoliday = db.HolidayEmployee.Where(x => x.EmployeeId == eachRow.EmployeeId && x.HolidayId == holiday.Id).FirstOrDefault();
                    if (employeeHoliday != null)
                    {
                        eachRow.Holiday = "Yes";
                        eachRow.LeaveName = employeeHoliday.Holiday.Name;
                        if (eachRow.ShiftTypeId == 0)
                        {
                            eachRow.PlannedTimeIn = "00:00";
                            eachRow.PlannedTimeOut = "00:00";
                        }
                    }
                }
                else
                {
                    var dayOfWeek = (int)curdate.DayOfWeek;
                    var wolist = db.EmployeeWOList.Where(x => x.EmployeeId == eachRow.EmployeeId && x.OffDayId == dayOfWeek).FirstOrDefault();
                    if (wolist != null)
                    {
                        eachRow.Weekend = "Yes";
                        if (eachRow.ShiftTypeId == 0)
                        {
                            eachRow.PlannedTimeIn = "00:00";
                            eachRow.PlannedTimeOut = "00:00";
                        }
                    }
                }
            }


        }
    }
}
