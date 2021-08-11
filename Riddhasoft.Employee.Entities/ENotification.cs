using System;

namespace Riddhasoft.Employee.Entities
{
    public class ENotification
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public int TypeId { get; set; }
        public NotificationLevel NotificationLevel { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime TranDate { get; set; }
        public int CompanyId { get; set; }
    }

    public class ENotificationDetail
    {
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int TargetId { get; set; }
        public virtual ENotification Notification { get; set; }
    }

    public enum NotificationType
    {
        Holiday,
        Event,
        Notice,
        Leave,
        LeaveRequest,
        OfficeVisit,
        Kaj,
        ManualPunch,
        LateInEarlyOut
    }
    public enum NotificationLevel
    {
        All,
        Branch,
        Department,
        Section,
        Employee
    }
}
