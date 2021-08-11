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
   public class SDailyLateInReport
    {
       
        DB.RiddhaDBContext db;
        public SDailyLateInReport()
        {
            db = new DB.RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<List<AttendanceReportDetailViewModel>> Get(DateTime FromDate)
        {
            List<AttendanceReportDetailViewModel> reportData = new List<AttendanceReportDetailViewModel>();

            var employeeList = db.Employee.ToList();
            var employeeWithFixedShift = employeeList.Where(x => x.ShiftTypeId == 0).ToList();
            var EmployeeWithDynamicShift = employeeList.Where(x => x.ShiftTypeId != 0);
            var EmployeFixedShiftData = (from c in employeeWithFixedShift
                                         join d in db.EmployeeShitList.ToList()
                                         on c.Id equals d.EmployeeId
                                         into joined
                                         //join e in db.GLogData.ToList() on c.code into aaaa
                                         from j in joined.DefaultIfEmpty(new EEmployeeShitList())
                                         select new AttendanceReportDetailViewModel()
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
                                             PlannedTimeIn = j.Shift.ShiftStartTime.ToString(@"hh\:mm"),
                                             PlannedTimeOut = j.Shift.ShiftEndTime.ToString(@"hh\:mm")
                                         }
                        ).ToList();

            List<DateTime> workdays = new List<DateTime>();

            foreach (var item in EmployeFixedShiftData)
            {
                var eachRow = new AttendanceReportDetailViewModel()
                {
                    WorkDate = FromDate.ToString("yyyy/MM/dd"),
                    EmployeeId = item.EmployeeId,
                    EmployeeDeviceCode = item.EmployeeDeviceCode,
                    EmployeeCode = item.EmployeeCode,
                    EmployeeName = item.EmployeeName,
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
                var eachRow = new AttendanceReportDetailViewModel()
                {
                    WorkDate = FromDate.ToString("yyyy/MM/dd"),
                    EmployeeId = item.Id,
                    EmployeeCode = item.Code,
                    EmployeeName = item.Name,
                    EmployeeDeviceCode = item.DeviceCode,

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






            return new Riddhasoft.Services.Common.ServiceResult<List<AttendanceReportDetailViewModel>>()
            {
                Data = reportData,
                Status = ResultStatus.Ok
            };
        }

        private void calculateRemaining(AttendanceReportDetailViewModel eachRow, DateTime FromDate)
        {
            eachRow.ActualTimeIn = new TimeSpan(0).ToString(@"hh\:mm");
            eachRow.ActualTimeOut = new TimeSpan(0).ToString(@"hh\:mm");
            if (!eachRow.NoPunch)
            {
                //var log = db.GLogData.Where(x => x.EnrollId == eachRow.EmployeeDeviceCode && DbFunctions.TruncateTime(x.DateTime) == DbFunctions.TruncateTime(FromDate)).ToList();
                var log = db.AttendanceLog.Where(x => x.EmployeeId == eachRow.EmployeeId && DbFunctions.TruncateTime(x.DateTime) == DbFunctions.TruncateTime(FromDate)).ToList();
                if (log != null && log.Count() > 0)
                {
                    eachRow.ActualTimeIn = log.OrderBy(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");
                    if (eachRow.TwoPunch && log.Count > 1)
                        eachRow.ActualTimeOut = log.OrderByDescending(x => x.DateTime).First().DateTime.ToString(@"HH\:mm");
                }

            }

            #region leave Calculation Block

            if (eachRow.Absent == "Yes")
            {
                //
                var curdate = DateTime.Parse(eachRow.WorkDate);
                var leaves = db.LeaveApplication.Where(x => x.EmployeeId == eachRow.EmployeeId && DbFunctions.TruncateTime(x.From) <= DbFunctions.TruncateTime(curdate) && x.To >= DbFunctions.TruncateTime(curdate)).FirstOrDefault();
                if (leaves != null)
                {
                    eachRow.OnLeave = "Yes";

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
            #endregion
        }
    }
}
