using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riddhasoft.Globals.Conversion;
using System.Data;
using System.Data.SqlClient;
namespace Riddhasoft.HumanResource.Management.Report
{
    public class SPayrollCalculationReport
    {
        Riddhasoft.DB.RiddhaDBContext db;
        private string currentLanguage = "";
        public SPayrollCalculationReport(string currentLanguage = "")
        {
            this.currentLanguage = currentLanguage;
            db = new DB.RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<List<PayrollCalculationReportViewModel>> Get(DateTime FromDate, DateTime ToDate, List<EEmployee> employeeList)
        {
            List<PayrollCalculationReportViewModel> reportData = new List<PayrollCalculationReportViewModel>();
            List<MonthlyEmployeeSummaryReport> roughData = new List<MonthlyEmployeeSummaryReport>();
            SDateTable DateService = new SDateTable();
            int daysInMonth = (ToDate - FromDate).Days + 1;
            var employeeWithFixedShift = employeeList.Where(x => x.ShiftTypeId == 0).ToList();
            var EmployeeWithDynamicShift = employeeList.Where(x => x.ShiftTypeId != 0);
            var EmployeFixedShiftData = (from c in employeeWithFixedShift
                                         join d in db.EmployeeShitList.ToList()
                                         on c.Id equals d.EmployeeId
                                         into joined
                                         //join e in db.GLogData.ToList() on c.code into aaaa
                                         from j in joined.DefaultIfEmpty(new EEmployeeShitList())
                                         select new MonthlyEmployeeSummaryReport()
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
                                             SectionCode = c.Section == null ? "" : c.Section.Code,
                                             SectionName = c.Section == null ? "" : c.Section.Name,
                                             PlannedTimeIn = j.Shift.ShiftStartTime.ToString(@"hh\:mm"),
                                             PlannedTimeOut = j.Shift.ShiftEndTime.ToString(@"hh\:mm")
                                         }
                        ).ToList();

            List<DateTime> workdays = new List<DateTime>();
            while (FromDate <= ToDate)
            {
                foreach (var item in EmployeFixedShiftData)
                {
                    var eachRow = new MonthlyEmployeeSummaryReport()
                    {
                        WorkDate = FromDate.ToString("yyyy/MM/dd"),
                        EmployeeId = item.EmployeeId,
                        EmployeeDeviceCode = item.EmployeeDeviceCode,
                        EmployeeCode = item.EmployeeCode,
                        EmployeeName = item.EmployeeName,
                        DepartmentCode = item.DepartmentCode,
                        DepartmentName = item.DepartmentName,
                        SectionCode = item.SectionCode,
                        SectionName = item.SectionName,
                        PlannedTimeIn = item.PlannedTimeIn,
                        PlannedTimeOut = item.PlannedTimeOut,
                        NoPunch = item.NoPunch,
                        SinglePunch = item.SinglePunch,
                        TwoPunch = item.TwoPunch,
                        MultiplePunch = item.MultiplePunch,
                        FourPunch = item.FourPunch,
                    };

                    calculateRemaining(eachRow, FromDate);
                    roughData.Add(eachRow);
                }
                foreach (var item in EmployeeWithDynamicShift)
                {
                    var roster = db.Roster.Where(x => x.EmployeeId == item.Id && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(FromDate)).FirstOrDefault();
                    var eachRow = new MonthlyEmployeeSummaryReport()
                    {
                        WorkDate = FromDate.ToString("yyyy/MM/dd"),
                        EmployeeId = item.Id,
                        EmployeeCode = item.Code,
                        EmployeeName = item.Name,
                        EmployeeDeviceCode = item.DeviceCode,
                        DepartmentCode = item.Section == null ? "" : item.Section.Department.Code,
                        DepartmentName = item.Section == null ? "" : item.Section.Department.Name,
                        SectionCode = item.Section == null ? "" : item.Section.Code,
                        SectionName = item.Section == null ? "" : item.Section.Name,
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
                    roughData.Add(eachRow);
                }
                FromDate = FromDate.AddDays(1);
            }

            List<PayrollCalculationReportViewModel> PayrollList = new List<PayrollCalculationReportViewModel>();
            var payrollData = db.PayRollSetup.ToList();

            foreach (var item in employeeList)
            {
                EPayRollSetup payroll = payrollData.Where(x => x.EmployeeId == item.Id).FirstOrDefault();
                if (payroll == null)
                    continue;
                payroll = payroll ?? new EPayRollSetup();
                decimal BasicSalary = (payroll).BasicSalary;
                int DaysWorked = roughData.Where(x => x.EmployeeId == item.Id).Count(x => x.Remark == "Present");
                TimeSpan plannedWork = calculatePlannedHours(roughData, item.Id);
                int holidayCount = roughData.Where(x => x.EmployeeId == item.Id).Count(x => x.Remark == "Holiday" && DateTime.Parse(x.WorkDate) > payroll.EffectedFrom);
                int WeekendCount = roughData.Where(x => x.EmployeeId == item.Id).Count(x => x.Remark == "Weekend" && DateTime.Parse(x.WorkDate) > payroll.EffectedFrom);
                TimeSpan overTime = new TimeSpan(0);
                decimal grossSalary = 0;
                decimal WeekendAmount = 0;

                decimal unitsalary = 0;
                if (BasicSalary != 0)
                {
                    switch (payroll.SalaryPaidPer)
                    {
                        case SalaryPaidPer.month:
                            unitsalary = BasicSalary / daysInMonth;
                            grossSalary = unitsalary * (DaysWorked + holidayCount);
                            overTime = calculateOverTime(roughData, item.Id);
                            WeekendAmount = unitsalary * WeekendCount;

                            break;
                        case SalaryPaidPer.hour:
                            TimeSpan workedHours = calculateWorkedHours(roughData, item.Id);
                            grossSalary = ((decimal)workedHours.TotalHours * BasicSalary);
                            WeekendAmount = 0;
                            holidayCount = 0;
                            WeekendCount = 0;
                            break;
                        default:
                            unitsalary = BasicSalary / daysInMonth;
                            grossSalary = unitsalary * (DaysWorked + holidayCount);
                            overTime = calculateOverTime(roughData, item.Id);
                            WeekendAmount = unitsalary * WeekendCount;
                            break;
                    }
                }
                else
                {
                    grossSalary = 0;
                }
                //grosssalary=unitsalary()

                var payrollReportData = new PayrollCalculationReportViewModel()
                {
                    EmployeeCode = item.Code,
                    EmployeeName = item.Name,
                    Designation = item.DesignationId.ToString(),
                    DepartmentCode = item.Section.Department.Code,
                    DepartmentName = item.Section.Department.Name,
                    DaysWorked = DaysWorked,
                    HolidayCount = holidayCount,
                    WeekendCount = WeekendCount,
                    LeavesCount = roughData.Where(x => x.EmployeeId == item.Id).Count(x => x.Remark == "Leave"),
                    Overtime = overTime,
                    OvertimeAmount = ((decimal)(overTime.TotalHours / plannedWork.TotalHours)) * payroll.OtRatePerHour * unitsalary,
                    BasicSalary = BasicSalary,
                    GrossSalary = grossSalary,
                    //Conveyance = (payroll.ConveyancePayPer == PayRatePer.Days ? DaysWorked : (decimal)calculateWorkedHours(roughData, item.Id).TotalHours) * payroll.Conveyance,
                    // HRA = (payroll.HRAPayPer == PayRatePer.Days ? DaysWorked : (decimal)calculateWorkedHours(roughData, item.Id).TotalHours) * payroll.HRA,
                    CIT = (grossSalary * payroll.CITRate) / 100,
                    //ProvidendFund = BasicSalary * payroll.Medical / 100m,
                    ProvidendFund = BasicSalary * payroll.PFRate / 100m,
                    WeekendAmount = WeekendAmount,
                    Advance = 0,
                    Loan = 0
                };

                #region Addition allowance
                var additionalAllowance = 0m;
                var allowances = db.PayRollAdditionalAllowance.Where(x => x.PayrollId == payroll.Id).ToList();
                if (allowances != null && allowances.Count() > 0)
                {

                    for (int i = 0; i < allowances.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                payrollReportData.AdditionalText1 = allowances[i].AllowanceName;
                                payrollReportData.AdditionalValue1 = allowances[i].AllowanceValue;
                                break;
                            case 1:
                                payrollReportData.AdditionalText2 = allowances[i].AllowanceName;
                                payrollReportData.AdditionalValue2 = allowances[i].AllowanceValue;
                                break;
                            default:
                                break;
                        }
                        additionalAllowance += allowances[i].AllowanceValue;
                    }

                }

                #endregion

                switch (payroll.TdsPaidBy)
                {
                    case TdsPaidBy.NetSalary:
                        var Earnings = grossSalary + payrollReportData.HRA + payrollReportData.Conveyance + payrollReportData.OvertimeAmount;
                        var Deduction = payrollReportData.ProvidendFund + payrollReportData.CIT + payrollReportData.Advance + payrollReportData.Loan;

                        payrollReportData.TDS = ((Earnings + additionalAllowance) - Deduction) * payroll.TDS / 100;
                        break;
                    case TdsPaidBy.BasicSalary:
                        payrollReportData.TDS = BasicSalary * payroll.TDS / 100;
                        break;
                    default:
                        break;
                }
                reportData.Add(payrollReportData);
            }
            return new Riddhasoft.Services.Common.ServiceResult<List<PayrollCalculationReportViewModel>>()
            {
                Data = reportData,
                Status = ResultStatus.Ok
            };
        }

        private TimeSpan calculateOverTime(List<MonthlyEmployeeSummaryReport> roughData, int p)
        {
            try
            {
                var data = roughData.Where(x => x.EmployeeId == p && x.Remark == "Present");
                if (data.Count() > 0)
                {
                    return data.Select(c => TimeSpan.Parse(c.Ot))
                                                     .Aggregate((working, next) => working.Add(next));
                }
                else
                {
                    return new TimeSpan(0);
                }
            }
            catch
            {

                return new TimeSpan(0);
            }
        }
        private TimeSpan calculateWorkedHours(List<MonthlyEmployeeSummaryReport> roughData, int p)
        {
            var data = roughData.Where(x => x.EmployeeId == p && x.Remark == "Present");
            if (data.Count() > 0)
            {
                return roughData.Where(x => x.EmployeeId == p && x.Remark == "Present").Select(c => TimeSpan.Parse(c.Actual))
                                             .Aggregate((working, next) => working.Add(next));
            }
            else
            {
                return new TimeSpan(0);
            }
        }
        private TimeSpan calculateOverTime(List<MonthlyWiseReport> roughData, int p)
        {
            try
            {
                var data = roughData.Where(x => x.EmployeeId == p && x.Remark == "Present");
                if (data.Count() > 0)
                {
                    return data.Select(c => TimeSpan.Parse(c.Ot))
                                                     .Aggregate((working, next) => working.Add(next));
                }
                else
                {
                    return new TimeSpan(0);
                }
            }
            catch
            {

                return new TimeSpan(0);
            }
        }
        private TimeSpan calculateWorkedHours(List<MonthlyWiseReport> roughData, int p)
        {
            var data = roughData.Where(x => x.EmployeeId == p && x.Remark == "Present");
            if (data.Count() > 0)
            {
                return roughData.Where(x => x.EmployeeId == p && x.Remark == "Present").Select(c => TimeSpan.Parse(c.Actual))
                                             .Aggregate((working, next) => working.Add(next));
            }
            else
            {
                return new TimeSpan(0);
            }
        }
        private TimeSpan calculatePlannedHours(List<MonthlyWiseReport> roughData, int p)
        {
            var data = roughData.Where(x => x.EmployeeId == p);
            if (data.Count() > 0)
            {
                return roughData.Where(x => x.EmployeeId == p).Select(c => TimeSpan.Parse(c.Standard))

                                             .Aggregate((working, next) => working.Add(next));
            }
            else
            {
                return new TimeSpan();
            }
        }
        private TimeSpan calculatePlannedHours(List<MonthlyEmployeeSummaryReport> roughData, int p)
        {
            var data = roughData.Where(x => x.EmployeeId == p);
            if (data.Count() > 0)
            {
                return roughData.Where(x => x.EmployeeId == p).Select(c => TimeSpan.Parse(c.Standard))

                                             .Aggregate((working, next) => working.Add(next));
            }
            else
            {
                return new TimeSpan();
            }
        }
        private void calculateRemaining(MonthlyEmployeeSummaryReport eachRow, DateTime FromDate)
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

        public ServiceResult<List<PayrollCalculationReportViewModel>> GetAttendanceReportFromSp(DateTime FromDate, DateTime ToDate, int branchId, int[] emps)
        {
            List<PayrollCalculationReportViewModel> reportData = new List<PayrollCalculationReportViewModel>();
            List<DB.AttendanceReportResult> result=new List<DB.AttendanceReportResult>();
            try
            {
                string emp = string.Join(",", emps);
                result = db.SP_GET_ATTENDACE_REPORT(FromDate, ToDate, branchId, currentLanguage, emp);
            }
            catch (SqlException ex)
            {
                switch (db.Database.Connection.State)
                {
                    case ConnectionState.Broken:
                        break;
                    case ConnectionState.Closed:
                        break;
                    case ConnectionState.Connecting:
                    case ConnectionState.Executing:
                    case ConnectionState.Fetching:
                    case ConnectionState.Open:
                        db.Database.Connection.Close();
                        break;
                    default:
                        break;
                }
                db.Database.Connection.Close();
            }
            
            List<MonthlyWiseReport> rptList = new List<MonthlyWiseReport>();
            int daysInMonth = (ToDate - FromDate).Days + 1;
            var roughData = (from c in result
                             join d in emps
                                on c.EmployeeId equals d
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
                                 ShiftStartGrace = c.EarlyGrace == null ? "00:00" : c.EarlyGrace.ToString(@"hh\:mm"),
                                 ShiftEndGrace = c.LateGrace == null ? "00:00" : c.LateGrace.ToString(@"hh\:mm"),
                                 ShiftName = "",//to get from db
                                 ShiftTypeId = c.ShiftTypeId,
                                 SinglePunch = c.SinglePunch,
                                 TwoPunch = c.TwoPunch,
                                 Weekend = c.WEEKEND,
                                 WorkDate = c.EngDate.ToString("yyyy/MM/dd"),
                                 NepDate = c.NepDate,
                                 HolidayName = c.HOLIDAYNAME,
                                 DepartmentCode = c.DepartmentCode,
                                 DepartmentName = c.DepartmentCode + " - " + c.DepartmentName,
                                 SectionCode = c.SectionCode,
                                 SectionName = c.SectionName,
                                 DayName = c.EngDate.DayOfWeek.ToString(),

                             }
                         ).ToList();
            var employeeList = (from c in db.Employee.Where(x => x.BranchId == branchId)
                                join d in emps
                                on c.Id equals d
                                select c
                                   ).ToList();

            List<PayrollCalculationReportViewModel> PayrollList = new List<PayrollCalculationReportViewModel>();
            var payrollData = (from c in db.PayRollSetup.Where(x => x.BranchId == branchId)
                               join d in emps
                               on c.EmployeeId equals d


                               select c
                                  ).ToList();
            var gradeGroups = (from c in db.EmployeeGrade.Where(x => x.BranchId == branchId && DbFunctions.TruncateTime(x.EffectedFrom) <= ToDate && DbFunctions.TruncateTime(x.EffectedTo) >= FromDate).ToList()
                               join d in emps
                               on c.EmployeeId equals d
                               join e in db.GradeGroup.Where(x => x.BranchId == branchId)
                               on c.GradeGroupId equals e.Id
                               select new
                               {
                                   GradeGoupId = c.GradeGroupId,
                                   EmployeeId = c.EmployeeId,
                                   Amount = e.Value,
                                   EffectedFrom = c.EffectedFrom
                               }
                                  ).ToList();
            foreach (var item in employeeList)
            {
                EPayRollSetup payroll = payrollData.Where(x => x.EmployeeId == item.Id).FirstOrDefault();
                payroll = payroll ?? new EPayRollSetup();
                decimal BasicSalary = (payroll).BasicSalary;
                int DaysWorked = roughData.Where(x => x.EmployeeId == item.Id).
                    Count(x => x.Remark == "Present" && x.Holiday != "Yes" && x.Weekend != "Yes");
                TimeSpan plannedWork = calculatePlannedHours(roughData, item.Id);
                int holidayCount = roughData.Where(x => x.EmployeeId == item.Id).Count(x => x.Holiday == "Yes");
                int WeekendCount = roughData.Where(x => x.EmployeeId == item.Id).Count(x => x.Remark.ToLower() == "weekend");
                TimeSpan overTime = new TimeSpan(0);
                decimal grossSalary = 0;
                decimal WeekendAmount = 0;

                decimal unitsalary = 0;
                var eGradeGroups=gradeGroups.Where(x => x.EmployeeId == item.Id).OrderByDescending(x => x.EffectedFrom).FirstOrDefault();
                decimal gradeAmount = 0;
                if (eGradeGroups != null)
                {
                    gradeAmount = eGradeGroups.Amount;
                }
                decimal lateDeduction = 0M;
                decimal earlyDeduction = 0M;
                if (BasicSalary != 0)
                {
                    switch (payroll.SalaryPaidPer)
                    {
                        case SalaryPaidPer.month:
                            unitsalary = BasicSalary / daysInMonth;
                            WeekendAmount = unitsalary * WeekendCount;
                            grossSalary = unitsalary * (DaysWorked + holidayCount) + WeekendAmount+gradeAmount;
                            overTime = calculateOverTime(roughData, item.Id);

                            lateDeduction = getLateDuductionAmount(roughData, payroll, unitsalary);
                            earlyDeduction = getEarlyDuductionAmount(roughData, payroll, unitsalary);
                            break;
                        case SalaryPaidPer.hour:
                            TimeSpan workedHours = calculateWorkedHours(roughData, item.Id);
                            grossSalary = ((decimal)workedHours.TotalHours * BasicSalary)+gradeAmount;
                            WeekendAmount = 0;
                            holidayCount = 0;
                            WeekendCount = 0;

                            break;
                        default:
                            unitsalary = BasicSalary / daysInMonth;
                            WeekendAmount = unitsalary * WeekendCount;
                            grossSalary = unitsalary * (DaysWorked + holidayCount) + WeekendAmount+gradeAmount;
                            overTime = calculateOverTime(roughData, item.Id);

                            break;
                    }
                }
                else
                {
                    grossSalary = 0;
                }
                //grosssalary=unitsalary()

                var payrollReportData = new PayrollCalculationReportViewModel()
                {
                    EmployeeCode = item.Code,
                    EmployeeName = item.Name,
                    //Designation = item.DesignationId,
                    DepartmentCode = item.Section.Department.Code,
                    DepartmentName = item.Section.Department.Name,
                    DaysWorked = DaysWorked,
                    HolidayCount = holidayCount,
                    WeekendCount = WeekendCount,
                    LeavesCount = roughData.Where(x => x.EmployeeId == item.Id).Count(x => x.Remark == "Leave"),
                    Overtime = overTime,
                    OvertimeAmount = Math.Round(((decimal)(overTime.TotalHours)) * payroll.OtRatePerHour, 2),
                    BasicSalary = BasicSalary,
                    GrossSalary = Math.Round(grossSalary, 2),
                    // Conveyance = (payroll.ConveyancePayPer == PayRatePer.Days ? DaysWorked : (decimal)calculateWorkedHours(roughData, item.Id).TotalHours) * payroll.Conveyance,
                    //HRA = (payroll.HRAPayPer == PayRatePer.Days ? DaysWorked : (decimal)calculateWorkedHours(roughData, item.Id).TotalHours) * payroll.HRA,
                    CIT = Math.Round(((grossSalary * payroll.CITRate) / 100m), 2),
                    ProvidendFund = Math.Round((BasicSalary * payroll.PFRate / 100m), 2),
                    WeekendAmount = Math.Round(WeekendAmount, 2),
                    Advance = 0,
                    Loan = 0,
                    LateDeduction = Math.Round(lateDeduction, 2),
                    EarlyDeduction = Math.Round(earlyDeduction, 2),
                    GradeAmount=gradeAmount
                };

                #region Addition allowance
                //var additionalAllowance = 0m;
                //var allowances = db.PayRollAdditionalAllowance.Where(x => x.PayrollId == payroll.Id).ToList();
                //if (allowances != null && allowances.Count() > 0)
                //{

                //    for (int i = 0; i < allowances.Count; i++)
                //    {
                //        switch (i)
                //        {
                //            case 0:
                //                payrollReportData.AdditionalText1 = allowances[i].AllowanceName;
                //                payrollReportData.AdditionalValue1 = allowances[i].AllowanceValue;
                //                break;
                //            case 1:
                //                payrollReportData.AdditionalText2 = allowances[i].AllowanceName;
                //                payrollReportData.AdditionalValue2 = allowances[i].AllowanceValue;
                //                break;
                //            default:
                //                break;
                //        }
                //        additionalAllowance += allowances[i].AllowanceValue;
                //    }

                //}


                #endregion

                var allowances = 0m;

                switch (payroll.TdsPaidBy)
                {
                    case TdsPaidBy.NetSalary:
                        var Earnings = grossSalary + payrollReportData.HRA + payrollReportData.Conveyance + payrollReportData.OvertimeAmount;
                        var Deduction = payrollReportData.ProvidendFund + payrollReportData.CIT + payrollReportData.Advance + payrollReportData.Loan;

                        payrollReportData.TDS = Math.Round((((Earnings + allowances) - Deduction) * payroll.TDS / 100), 2);
                        break;
                    case TdsPaidBy.BasicSalary:
                        payrollReportData.TDS = Math.Round(BasicSalary * payroll.TDS / 100, 2);
                        break;
                    default:
                        break;
                }
                reportData.Add(payrollReportData);
            }
            return new ServiceResult<List<PayrollCalculationReportViewModel>>() { Data = reportData };
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
                                   select c.Id).OrderBy(c => c).ToArray();
                    return result;
                }
                else
                {
                    int[] depIds = Array.ConvertAll(deps.Split(','), s => int.Parse(s));
                    result.Data = (from c in empQuery
                                   join d in depIds on c.Section.DepartmentId equals d
                                   select c.Id).OrderBy(c => c).ToArray();
                    return result;
                }
            }
        }
        private decimal getLateDuductionAmount(List<MonthlyWiseReport> roughData, EPayRollSetup payroll, decimal unitSalary)
        {
            if (payroll.EnableLateDeduction == false)
                return 0m;
            var totalLateDays = roughData.Where(x => x.EmployeeId == payroll.EmployeeId && x.LateIn != "00:00").ToList();
            if (totalLateDays.Count > payroll.LateGraceDay)
            {
                var totalLateHours = totalLateDays.Select(c => TimeSpan.Parse(c.LateIn ?? "00:00"))
                                            .Aggregate((working, next) => working.Add(next));
                switch (payroll.LateDeductionBy)
                {
                    case LateDeductionBy.Days:


                        return unitSalary * totalLateDays.Count;
                    case LateDeductionBy.HalfDay:
                        return (unitSalary * totalLateDays.Count) / 2;
                    case LateDeductionBy.Hour:

                        return (decimal)totalLateHours.TotalHours * payroll.LateDeductionRate;
                    case LateDeductionBy.SingleDay:

                        if (totalLateHours.TotalHours > 0)
                            return unitSalary;
                        else
                            return 0M;
                    default:
                        return 0M;
                }
            }
            return 0M;

        }
        private decimal getEarlyDuductionAmount(List<MonthlyWiseReport> roughData, EPayRollSetup payroll, decimal unitSalary)
        {
            if (payroll.EnableEarlyDeduction == false)
                return 0m;
            var totalEarlyDays = roughData.Where(x => x.EmployeeId == payroll.EmployeeId && x.EarlyOut != "00:00").ToList();
            if (totalEarlyDays.Count > payroll.EarlyGraceDay)
            {
                var totalEarlyHours = totalEarlyDays.Select(c => TimeSpan.Parse(c.EarlyOut ?? "00:00" ))
                                            .Aggregate((working, next) => working.Add(next));
                switch (payroll.EarlyDeductionBy)
                {
                    case LateDeductionBy.Days:
                        return unitSalary * totalEarlyDays.Count;
                    case LateDeductionBy.HalfDay:
                        return (unitSalary * totalEarlyDays.Count) / 2;
                    case LateDeductionBy.Hour:

                        return (decimal)totalEarlyHours.TotalHours * payroll.EarlyDeductionRate;
                    case LateDeductionBy.SingleDay:

                        if (totalEarlyHours.TotalHours > 0)
                            return unitSalary;
                        else
                            return 0M;
                    default:
                        return 0M;
                }
            }
            return 0M;

        }
    }
}
