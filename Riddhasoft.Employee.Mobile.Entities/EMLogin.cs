using System;

namespace Riddhasoft.Employee.Mobile.Entities
{
    public class EMLogin
    {
        public string CompanyCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class EMEmployeeProfile
    {
        public int EmployeeId { get; set; }
        public string IdCardNo { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Shift { get; set; }
        public string PunchTime { get; set; }
        public string Remark { get; set; }
        public string PhotoUrl { get; set; }

    }

    public class EMHomeAttendanceInfo
    {
        public string PlannedTime { get; set; }
        public string Attendance { get; set; }
        public WorkCode WorkCode { get; set; }
        public string Leave { get; set; }
        public string Kaj { get; set; }
        public string OfficeVisit { get; set; }
    }
    public class EMonthlyAttendanceSummary
    {
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int HolidayCount { get; set; }
        public int LeaveDayCount { get; set; }

        public string Ot { get; set; }

        public int WeekendCount { get; set; }
    }
    public class EMUpcomming
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public int TargetId { get; set; }
        public int RemDays { get; set; }
    }
    public class EMNotification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Type { get; set; }
        public int TargetId { get; set; }
        public int RemDays { get; set; }
    }
    public enum WorkCode
    {
        Shift,
        Holiday,
        Leave,
        Weekend
    }
}
