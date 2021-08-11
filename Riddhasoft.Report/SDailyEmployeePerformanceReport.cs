using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riddhasoft.Globals.Conversion;

namespace Riddhasoft.HumanResource.Management.Report
{
    public class SDailyEmployeePerformanceReport
    {
        RiddhaDBContext db = null;
        private int branchId = 0;

        private int fiscalYearId = 0;
        private string currentLanguage = "";
        public int[] FilteredEmployeeIDs { get; set; }
        private SDailyEmployeePerformanceReport()
        {

        }
        public SDailyEmployeePerformanceReport(int branchId, int fiscalYearId, string currentLanguage = "")
        {
            db = new DB.RiddhaDBContext();
            this.branchId = branchId;
            this.fiscalYearId = fiscalYearId;
            this.currentLanguage = currentLanguage;
        }
        public Riddhasoft.Services.Common.ServiceResult<List<AttendanceReportDetailViewModel>> Get(DateTime FromDate, int CompanyId)
        {
            List<AttendanceReportDetailViewModel> reportData = new List<AttendanceReportDetailViewModel>();

            var employeeList = db.Employee.Where(x => x.Branch.CompanyId == CompanyId).ToList();
            var shiftList = db.EmployeeShitList.ToList();
            var employeeWithFixedShift = employeeList.Where(x => x.ShiftTypeId == 0).ToList();
            var EmployeeWithDynamicShift = employeeList.Where(x => x.ShiftTypeId != 0);
            var EmployeFixedShiftData = (from c in employeeWithFixedShift
                                         join d in shiftList
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
                                             MobileNo = c.Mobile,
                                             NoPunch = c.NoPunch,
                                             SinglePunch = c.SinglePunch,
                                             TwoPunch = c.TwoPunch,
                                             MultiplePunch = c.MultiplePunch,
                                             FourPunch = c.FourPunch,
                                             PlannedTimeIn = (j.Shift ?? new EShift()).ShiftStartTime.ToString(@"hh\:mm"),
                                             PlannedTimeOut = (j.Shift ?? new EShift()).ShiftEndTime.ToString(@"hh\:mm")
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
                    MobileNo = item.MobileNo,
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
                    MobileNo = item.Mobile,
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
        public Riddhasoft.Services.Common.ServiceResult<List<AttendanceReportDetailViewModel>> Get(DateTime FromDate, List<EEmployee> employeeList)
        {
            List<AttendanceReportDetailViewModel> reportData = new List<AttendanceReportDetailViewModel>();

            var shiftList = db.EmployeeShitList.ToList();
            var employeeWithFixedShift = employeeList.Where(x => x.ShiftTypeId == 0).ToList();
            var EmployeeWithDynamicShift = employeeList.Where(x => x.ShiftTypeId != 0);
            var EmployeFixedShiftData = (from c in employeeWithFixedShift
                                         join d in shiftList
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
                                             MobileNo = c.Mobile,
                                             NoPunch = c.NoPunch,
                                             SinglePunch = c.SinglePunch,
                                             TwoPunch = c.TwoPunch,
                                             MultiplePunch = c.MultiplePunch,
                                             FourPunch = c.FourPunch,
                                             PlannedTimeIn = (j.Shift ?? new EShift()).ShiftStartTime.ToString(@"hh\:mm"),
                                             PlannedTimeOut = (j.Shift ?? new EShift()).ShiftEndTime.ToString(@"hh\:mm")
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
                    MobileNo = item.MobileNo,
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
                    MobileNo = item.Mobile,
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

        public ServiceResult<List<AttendanceReportDetailViewModel>> Get(string p)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<List<AttendanceReportDetailViewModel>> Get(DateTime FromDate, List<EShift> list)
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
                                             MobileNo = c.Mobile,
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
                    MobileNo = item.MobileNo,
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
                    MobileNo = item.Mobile,
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
                //var log = db.AttendanceLog.Where(x => x.EmployeeId == eachRow.EmployeeDeviceCode && DbFunctions.TruncateTime(x.DateTime) == DbFunctions.TruncateTime(FromDate)).ToList();
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
                    //TODO: report contineu
                    var holiday = db.HolidayDetails.Where(x => x.Holiday.BranchId == this.branchId && x.FiscalYearId == fiscalYearId && DbFunctions.TruncateTime(x.BeginDate) <= DbFunctions.TruncateTime(curdate) && x.EndDate >= DbFunctions.TruncateTime(curdate)).FirstOrDefault();
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

        public ServiceResult<List<AttendanceReportDetailViewModel>> GetAttendanceReportFromSp(DateTime FromDate)
        {
            string emp = "";
            if (FilteredEmployeeIDs!=null)
            {
                emp = string.Join(",", FilteredEmployeeIDs);
            }
            var result = db.SP_GET_ATTENDACE_REPORT(FromDate, FromDate, branchId, currentLanguage, emp);
            List<AttendanceReportDetailViewModel> rptList = new List<AttendanceReportDetailViewModel>();
            rptList = (from c in result
                       select new AttendanceReportDetailViewModel()
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
                           MobileNo = c.MobileNo,
                           MultiplePunch = c.MultiplePunch,
                           NoPunch = c.NoPunch,
                           OnLeave = string.IsNullOrEmpty(c.LEAVENAME) ? "No" : "Yes",
                           PlannedTimeIn = c.PlannedTimeIn == null ? "00:00" : c.PlannedTimeIn.ToString(@"hh\:mm"),
                           PlannedTimeOut = c.PlannedTimeOut == null ? "00:000" : c.PlannedTimeOut.ToString(@"hh\:mm"),
                           ShiftStartGrace = c.EarlyGrace == null ? "00:00" : c.EarlyGrace.ToString(@"hh\:mm"),
                           ShiftEndGrace = c.LateGrace == null ? "00:00" : c.LateGrace.ToString(@"hh\:mm"),
                           ShiftName = "",//to get from db
                           ShiftTypeId = c.ShiftTypeId,
                           SinglePunch = c.SinglePunch,
                           TwoPunch = c.TwoPunch,
                           Weekend = c.WEEKEND,
                           WorkDate = c.EngDate.ToString("yyyy/MM/dd"),
                           HolidayName = c.HOLIDAYNAME,
                           DepartmentName = currentLanguage == "ne" && c.DepartmentNameNp != null ? c.DepartmentNameNp : c.DepartmentName,
                           EmploymentStatus = c.EmploymentStatus,
                           OfficeVisit = c.OFFICEVISIT,
                           Kaj = c.KAJ
                       }).ToList();

            return new ServiceResult<List<AttendanceReportDetailViewModel>>() { Data = rptList };
        }
    }
}
