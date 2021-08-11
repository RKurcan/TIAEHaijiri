using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Mobile.Entities
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
        Leave
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
