using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riddhasoft.Globals.Conversion;
using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;

namespace Riddhasoft.Report.ReportViewModel
{
    public class AttendanceReportDetailViewModel
    {
        private bool oTV2;
        private TimeSpan minimumOTHour;

        public AttendanceReportDetailViewModel()
        {
            this.minimumOTHour = new TimeSpan();
            this.oTV2 = false;

        }
        public AttendanceReportDetailViewModel(bool oTV2, TimeSpan minimumOTHour)
        {
            this.oTV2 = oTV2;
            this.minimumOTHour = minimumOTHour;
        }

        public int EmployeeId { get; set; }
        public int EmployeeDeviceCode { get; set; }
        public string MobileNo { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public string EmploymentStatusString { get; set; }
        public string WorkDate { get; set; }
        public string NepDate { get; set; }
        public string DayName { get; set; }
        public string ShiftName { get; set; }
        public string PlannedTimeIn { get; set; }
        public string PlannedTimeOut { get; set; }
        public string PlannedLunchIn { get; set; }
        public string PlannedLunchOut { get; set; }
        public string ShiftStartGrace { get; set; }

        public string ShiftEndGrace { get; set; }
        public string ActualTimeIn { get; set; }

        public string ActualTimeOut { get; set; }
        public string ActualLunchIn { get; set; }
        public string ActualLunchOut { get; set; }
        public int ShiftTypeId { get; set; }
        /// <summary>
        /// worktime 
        /// </summary>
        public string Standard
        {
            get
            {
                var intime = PlannedTimeIn.ToTimeSpan();
                var outtime = PlannedTimeOut.ToTimeSpan();

                return getShiftHours(intime, outtime).ToString(@"hh\:mm");

            }
        }
        public string Normal { get; set; }
        public string Ot
        {
            get
            {
                if (Absent == "Yes" || SinglePunch || NoPunch)
                    return "";//"00:00";
                if (!oTV2)
                {
                    if (Absent == "Yes" || SinglePunch || NoPunch)
                        return "";//"00:00";
                    var standard = Standard.ToTimeSpan();
                    var actual = Actual.ToTimeSpan();
                    if (standard < actual)
                    {
                        string result = (standard - actual).ToString(@"hh\:mm");
                        TimeSpan OTHour = TimeSpan.Parse(result);
                        if (OTHour > minimumOTHour)
                        {
                            return (standard - actual).ToString(@"hh\:mm");
                        }
                        return "";
                        //return (standard - actual).ToString(@"hh\:mm");
                    }
                    else
                    {
                        return ""; //"00:00";
                    }
                }
                else
                {
                    TimeSpan beforeShiftOt = new TimeSpan(0), afterShiftOt = new TimeSpan(0);
                    TimeSpan shiftIn = PlannedTimeIn.ToTimeSpan();
                    TimeSpan shiftOut = PlannedTimeOut.ToTimeSpan();
                    TimeSpan actualIn = ActualTimeIn.ToTimeSpan();
                    TimeSpan actualOut = ActualTimeOut.ToTimeSpan();
                    if (shiftIn == shiftOut)
                    {
                        if (Actual.ToTimeSpan() > minimumOTHour)
                        {
                            return Actual;
                        }
                        return "";
                    }
                    if (actualIn < shiftIn)
                    {
                        beforeShiftOt = shiftIn - actualIn;
                    }
                    if (shiftOut < actualOut)
                    {
                        afterShiftOt = actualOut - shiftOut;
                    }
                    TimeSpan TotalOt = (beforeShiftOt + afterShiftOt);

                    if (TotalOt > minimumOTHour)
                    {
                        string result = TotalOt.ToString(@"hh\:mm");
                        TimeSpan OTHour = TimeSpan.Parse(result);

                        return TotalOt.ToString(@"hh\:mm");

                        //return TotalOt.ToString(@"hh\:mm");
                    }
                    else
                    {
                        return ""; //"00:00";
                    }
                }

            }
        }
        public string Actual
        {
            get
            {
                var intime = ActualTimeIn.ToTimeSpan();
                var outtime = ActualTimeOut.ToTimeSpan();

                TimeSpan lunchDeduction = new TimeSpan(0);
                if (FourPunch)
                {
                    var actualBreak = TimeSpan.Parse(ActualLunchIn ?? "00:00") - TimeSpan.Parse(ActualLunchOut ?? "00:00");
                    var standardBreak = TimeSpan.Parse(PlannedLunchIn ?? "00:00") - TimeSpan.Parse(PlannedLunchOut ?? "00:00");
                    if (actualBreak > standardBreak && standardBreak != new TimeSpan(0))
                    {
                        lunchDeduction = actualBreak - standardBreak;
                    }
                }

                if (SinglePunch || NoPunch || ActualTimeOut == "00:00")
                {
                    return "00:00";
                }

                if (outtime > intime)
                {
                    TimeSpan shiftHour = new TimeSpan(outtime.Ticks - intime.Ticks);
                    return (shiftHour - lunchDeduction).ToString(@"hh\:mm");
                }

                else if (outtime == intime)
                {
                    TimeSpan idleTime = new TimeSpan(0, 0, 0);
                    TimeSpan shiftHour = new TimeSpan();
                    shiftHour = idleTime;
                    return (shiftHour - lunchDeduction).ToString(@"hh\:mm");
                }

                else
                {
                    TimeSpan idleTime = new TimeSpan(24, 0, 0);
                    TimeSpan shiftHour = new TimeSpan();
                    shiftHour = idleTime - intime + outtime;
                    return (shiftHour - lunchDeduction).ToString(@"hh\:mm");
                }

            }
        }
        public string OnLeave { get; set; }
        public string Holiday { get; set; }
        public string Weekend { get; set; }
        public string Absent
        {
            get
            {
                return Actual.ToTimeSpan().Hours == 0 ? Actual.ToTimeSpan().Minutes == 0 ? "Yes" : "No" : "No";
            }
        }
        public string LateIn
        {
            get
            {
                //changes made for late in in single punch 
                if ((Absent == "Yes" && ActualTimeIn == "00:00") || SinglePunch || NoPunch)
                    return "";//"00:00";
                if ((ActualTimeIn.ToTimeSpan() > PlannedTimeIn.ToTimeSpan()))
                {
                    var diff = (ActualTimeIn.ToTimeSpan() - PlannedTimeIn.ToTimeSpan());
                    if ((diff <= ShiftStartGrace.ToTimeSpan()))
                    {
                        return "";// "00:00";
                    }
                    return diff.ToString(@"hh\:mm");
                }
                else
                {
                    return "";// "00:00";
                }


            }
        }
        public string EarlyIn
        {
            get
            {
                //if (Absent == "Yes" || SinglePunch || NoPunch)
                if ((Absent == "Yes" && ActualTimeIn == "00:00") || SinglePunch || NoPunch)
                    return "";//00:00";
                if ((PlannedTimeIn.ToTimeSpan() > ActualTimeIn.ToTimeSpan()))
                {
                    return (ActualTimeIn.ToTimeSpan() - PlannedTimeIn.ToTimeSpan()).ToString(@"hh\:mm");
                }
                else
                {
                    return "";// "00:00";
                }
            }
        }
        public string EarlyOut
        {
            get
            {
                if (Absent == "Yes" || SinglePunch || NoPunch)
                    return "";// "00:00";
                if ((PlannedTimeOut.ToTimeSpan() > ActualTimeOut.ToTimeSpan()))
                {
                    return (PlannedTimeOut.ToTimeSpan() - ActualTimeOut.ToTimeSpan()).ToString(@"hh\:mm");
                }
                else
                {
                    return "";// "00:00";
                }
                ;
            }
        }
        public string LateOut
        {
            get
            {
                if (Absent == "Yes" || SinglePunch || NoPunch)
                    return "";// "00:00";
                if ((PlannedTimeOut.ToTimeSpan() < ActualTimeOut.ToTimeSpan()))
                {
                    return (ActualTimeOut.ToTimeSpan() - PlannedTimeOut.ToTimeSpan()).ToString(@"hh\:mm");
                }
                else
                {
                    return "";// "00:00";
                }

            }
        }
        public string Remark
        {
            get
            {
                if (OfficeVisit == "YES")
                {
                    return "Office Visit";
                }
                if (Kaj == "YES")
                {
                    return "Kaj";
                }

                if (NoPunch)
                {
                    //return OnLeave == "Yes" ? "Leave" : Holiday == "Yes" ? "Holiday" : Weekend == "Yes" ? "Weekend" : "Present";
                    return OnLeave == "Yes" ? LeaveName : Holiday == "Yes" ? HolidayName : Weekend == "Yes" ? "Weekend" : "Present";


                }
                if (SinglePunch)
                {
                    if (ActualTimeIn != "00:00")
                        return "Present";
                    else
                    {
                        //return OnLeave == "Yes" ? "Leave" : Holiday == "Yes" ? "Holiday" : Weekend == "Yes" ? "Weekend" : "Absent";
                        return OnLeave == "Yes" ? LeaveName : Holiday == "Yes" ? HolidayName : Weekend == "Yes" ? "Weekend" : "Absent";
                    }
                }
                if (ActualTimeOut == "00:00" && ActualTimeIn != "00:00")
                {
                    return "Misc";
                }
                switch (ShiftType)
                {
                    case ShiftType.Morning:
                        break;
                    case ShiftType.Day:
                        break;
                    case ShiftType.Evening:
                        break;
                    case ShiftType.Night:
                        break;
                    case ShiftType.Dynamic:
                        break;
                    case ShiftType.DayOff:
                        return "Day Off";

                    case ShiftType.NightOff:
                        return "Night Off";

                    default:
                        break;
                }

                // This code block is only for bir hospital
                // remark should be absent if roster is not define
                if (ShiftTypeId == 2 && Weekend == "Yes" && Absent == "Yes" && OnLeave != "Yes" && Holiday != "Yes")
                {
                    return "Absent";
                }
                //return Actual.ToTimeSpan().Hours == 0 ? OnLeave == "Yes" ? "Leave" : Holiday == "Yes" ? "Holiday" : Weekend == "Yes" ? "Weekend" : "Absent" : "Present";
                return Actual.ToTimeSpan().TotalMinutes == 0 ? OnLeave == "Yes" ? LeaveName + " ( " + LeaveDescription + " )" : Holiday == "Yes" ? HolidayName : Weekend == "Yes" ? "Weekend" : "Absent" : "Present";
            }
        }
        public string LeaveName { get; set; }
        public string LeaveDescription { get; set; }
        public string OfficeVisit { get; set; }
        public string Kaj { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameNp { get; set; }

        public bool SinglePunch { get; set; }
        public bool NoPunch { get; set; }
        public bool TwoPunch { get; set; }
        public bool FourPunch { get; set; }
        public bool MultiplePunch { get; set; }
        public bool RoundTheClock { get; set; }
        public ShiftType ShiftType { get; set; }
        private TimeSpan getShiftHours(TimeSpan shiftStartHour, TimeSpan shiftEndHour)
        {

            if (shiftEndHour > shiftStartHour)
            {
                TimeSpan shiftHour = new TimeSpan(shiftEndHour.Ticks - shiftStartHour.Ticks);
                return shiftHour;
            }

            else if (shiftEndHour == shiftStartHour)
            {
                TimeSpan idleTime = new TimeSpan(0, 0, 0);
                TimeSpan shiftHour = new TimeSpan();
                shiftHour = idleTime;
                return shiftHour;
            }

            else
            {
                RoundTheClock = true;
                TimeSpan idleTime = new TimeSpan(24, 0, 0);
                TimeSpan shiftHour = new TimeSpan();
                shiftHour = idleTime - shiftStartHour + shiftEndHour;
                return shiftHour;
            }
        }

        public string HolidayName { get; set; }
        public string DepartmentName { get; set; }
        public string PlannedTimeOut12Format
        {
            get
            {
                return ConvertTimeSpan(this.PlannedTimeOut);

            }
        }
        public string PlannedTimeIn12Format
        {
            get
            {
                return ConvertTimeSpan(this.PlannedTimeIn);

            }
        }
        public string ActualTimeIn12Format
        {
            get
            {
                return ConvertTimeSpan(this.ActualTimeIn);
            }
        }
        public string ActualTimeOut12Format
        {
            get
            {
                return ConvertTimeSpan(this.ActualTimeOut);
            }
        }

        public string ActualLunchIn12Format
        {
            get
            {
                return ConvertTimeSpan(this.ActualLunchIn);
            }
        }
        public string ActualLunchOut12Format
        {
            get
            {
                return ConvertTimeSpan(this.ActualLunchOut);
            }
        }
        private string ConvertTimeSpan(string timeSpan)
        {

            try
            {
                var hours = int.Parse(timeSpan.Split(':')[0]);
                var minutes = int.Parse(timeSpan.Split(':')[1]);
                var amPmDesignator = "AM";
                if (hours == 0)
                    hours = 12;
                else if (hours == 12)
                    amPmDesignator = "PM";
                else if (hours > 12)
                {
                    hours -= 12;
                    amPmDesignator = "PM";
                }
                var formattedTime =
                  String.Format("{0}:{1:00} {2}", hours, minutes, amPmDesignator);

                return formattedTime=="12:00 AM"?"00:00": formattedTime;

            }
            catch (Exception)
            {

                return "00:00";

            }

        }
    }
    public class MultipunchReportViewModel
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string WorkDate { get; set; }
        public string PunchTime { get; set; }
        public string DayName { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeDeviceCode { get; set; }
    }


    public class MonthlyWiseReport : AttendanceReportDetailViewModel
    {
        public MonthlyWiseReport()
        {

        }
        public MonthlyWiseReport(bool OTV2, TimeSpan minimumOTHour)
            : base(OTV2, minimumOTHour)
        {

        }

        public int SN { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNamee { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }

    }

    public class MonthlyEmployeeSummaryReport : AttendanceReportDetailViewModel
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
    }
    public class PayrollCalculationReportViewModel
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }

        public string Designation { get; set; }

        public int DaysWorked { get; set; }
        public int HolidayCount { get; set; }
        public int WeekendCount { get; set; }
        public int LeavesCount { get; set; }
        public TimeSpan Overtime { get; set; }
        public decimal OvertimeAmount { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal Conveyance { get; set; }
        public decimal ProvidendFund { get; set; }
        public decimal CIT { get; set; }
        public decimal TDS { get; set; }
        public decimal Advance { get; set; }
        public decimal Loan { get; set; }
        public decimal LateDeduction { get; set; }
        public decimal EarlyDeduction { get; set; }

        public decimal WeekendAmount { get; set; }
        public string AdditionalText1 { get; set; }
        public string AdditionalText2 { get; set; }
        public decimal AdditionalValue1 { get; set; }
        public decimal AdditionalValue2 { get; set; }
        public decimal NetSalary
        {
            get
            {
                var earnings = GrossSalary + OvertimeAmount;
                var deduction = TDS + CIT + ProvidendFund + LateDeduction + EarlyDeduction;
                return earnings - deduction;

            }
        }


        public decimal GradeAmount { get; set; }
    }

    public class MonthlyRosterReport
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
        public string EngDate { get; set; }
        public string NepDate { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
    }
}
