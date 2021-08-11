using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Globals.Conversion;
using Riddhasoft.Report.ReportViewModel;
using System;
using System.Linq;
namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMHome
    {
        DB.RiddhaDBContext db = null;
        public SMHome()
        {
            db = new DB.RiddhaDBContext();
        }

        public EMHomeAttendanceInfo GetHomeAttendanceInfo(int empId)
        {
            var attendanceInfo = (from c in db.SP_GET_ATTENDACE_REPORT_BY_EMPLOYEE_ID(System.DateTime.Now, System.DateTime.Now, empId)

                                  select new EMHomeAttendanceInfo()
                                  {
                                      PlannedTime = c.PlannedTimeIn == null ? getShiftInfo(c) : ((c.PlannedTimeIn.ToString(@"hh\:mm") + " - " + c.PlannedTimeOut.ToString(@"hh\:mm"))),
                                      Attendance = parseDateToTime(c),
                                      Kaj = c.KAJ,
                                      Leave = c.LEAVENAME,
                                      OfficeVisit = c.OFFICEVISIT,
                                  }).FirstOrDefault();
            return attendanceInfo;

        }

        private string getShiftInfo(DB.AttendanceReportResult c)
        {
            if (c.WEEKEND == "Yes")
                return "Weekend";
            else if (string.IsNullOrEmpty(c.HOLIDAYNAME) == false)
            {
                return "Holiday";
            }
            else if (string.IsNullOrEmpty(c.LEAVENAME) == false)
                return "Leave";
            return "";
        }

        private string parseDateToTime(DB.AttendanceReportResult c)
        {
            if (c.PUNCHIN == null)
            {
                if (c.WEEKEND == "Yes")
                {
                    return "Weekend";
                }
                else if (string.IsNullOrEmpty(c.HOLIDAYNAME) == false)
                {
                    return c.HOLIDAYNAME;
                }
                else if (string.IsNullOrEmpty(c.LEAVENAME) == false)
                {
                    return c.LEAVENAME;
                }
                else if (string.IsNullOrEmpty(c.KAJ) == false)
                {
                    return c.KAJ;
                }
                else if (string.IsNullOrEmpty(c.OFFICEVISIT) == false)
                {
                    return c.OFFICEVISIT;
                }
                return "";
            }
            else if (c.PUNCHIN == c.PUNCHOUT)
            {
                return c.PUNCHIN.ToDateTime().ToString(@"HH\:mm") + " - ";
            }
            else
            {
                return c.PUNCHIN.ToDateTime().ToString(@"HH\:mm") + " - " + c.PUNCHOUT.ToDateTime().ToString(@"HH\:mm");
            }
        }

        public EMonthlyAttendanceSummary GetAttendanceSummary(int empId)
        {
            var today = System.DateTime.Now.Date;

            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var fromDate = new DateTime(today.Year, today.Month, 1);
            var toDate = new DateTime(today.Year, today.Month, daysInMonth);
            var attendances = (from c in db.SP_GET_ATTENDACE_REPORT_BY_EMPLOYEE_ID(fromDate, toDate, empId)

                               select new MonthlyWiseReport()
                               {
                                   ActualTimeIn = c.PUNCHIN == null ? "00:00" : c.PUNCHIN.ToDateTime().ToString(@"HH\:mm"),
                                   ActualTimeOut = c.PUNCHOUT == null ? "00:00" : (c.PUNCHIN == c.PUNCHOUT ? "00:00" : c.PUNCHOUT.ToDateTime().ToString(@"HH\:mm")),
                                   EmployeeCode = c.EmployeeCode,
                                   EmployeeDeviceCode = c.EmployeeDeviceCode,
                                   EmployeeId = c.EmployeeId,
                                   EmployeeName = c.EmployeeName,
                                   FourPunch = c.FourPunch,
                                   Holiday = string.IsNullOrEmpty(c.HOLIDAYNAME) ? "No" : "Yes",
                                   LeaveName = c.LEAVENAME,
                                   MobileNo = c.MobileNo,
                                   MultiplePunch = c.MultiplePunch,
                                   NoPunch = c.NoPunch,
                                   OnLeave = string.IsNullOrEmpty(c.LEAVENAME) ? "No" : "Yes",
                                   PlannedTimeIn = c.PlannedTimeIn == null ? "00:00" : c.PlannedTimeIn.ToString(@"hh\:mm"),
                                   PlannedTimeOut = c.PlannedTimeOut == null ? "00:000" : c.PlannedTimeOut.ToString(@"hh\:mm"),
                                   ShiftName = "",//to get from db
                                   ShiftTypeId = c.ShiftTypeId,
                                   SinglePunch = c.SinglePunch,
                                   TwoPunch = c.TwoPunch,
                                   Weekend = c.WEEKEND,
                                   WorkDate = c.EngDate.ToString("yyyy/MM/dd"),
                                   HolidayName = c.HOLIDAYNAME,
                                   DepartmentCode = c.DepartmentCode,
                                   DepartmentName = c.DepartmentName,
                                   SectionCode = c.SectionCode,
                                   SectionName = c.SectionName,
                                   DayName = c.EngDate.DayOfWeek.ToString(),
                                   Kaj = c.KAJ,
                                   OfficeVisit = c.OFFICEVISIT,
                                   
                               }
                                  ).ToList();

            return (attendances.GroupBy(i => i.EmployeeId)
             .Select(i => new EMonthlyAttendanceSummary()
             {
                 AbsentCount = i.Where(j => j.Remark.ToLower() == "absent" && j.WorkDate.ToDateTime() <= today).Count(),
                 PresentCount = i.Where(j => j.Remark.ToLower() == "present").Count(),
                 WeekendCount = i.Where(j => j.Remark.ToLower() == "weekend").Count(),
                 //DutyDayCount = (totalDays - (i.Where(j => j.Remark.ToLower() == "weekend").Count())),
                 //EarlyInCount = i.Where(j => j.EarlyIn != "00:00").Count().ToString(),
                 //EarlyOutCount = i.Where(j => j.EarlyOut != "00:00").Count().ToString(),
                 //LateInCount = i.Where(j => j.LateIn != "00:00").Count().ToString(),
                 //LateOutCount = i.Where(j => j.LateOut != "00:00").Count().ToString(),
                 HolidayCount = i.Where(j => j.Holiday.ToLower() == "yes").Count(),
                 LeaveDayCount = i.Where(j => j.OnLeave.ToLower() == "yes").Count(),
                 Ot = string.Format("{0}:{1}", i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Ot.ToTimeSpan())).TotalHours.ToString(), "00"),
             })).FirstOrDefault();


        }
    }
}
