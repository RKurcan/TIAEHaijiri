using System;

namespace Riddhasoft.Employee.Entities
{
    public class EEmployeeLateInAndEarlyOutRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Remark { get; set; }
        public int? ApproveById { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime SystemDate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime RequestedDate { get; set; }
        public TimeSpan? ActualInTime { get; set; }
        public TimeSpan? ActualOutTime { get; set; }
        public TimeSpan? PlannedInTime { get; set; }
        public TimeSpan? PlannedOutTime { get; set; }
        public TimeSpan? LateInTime { get; set; }
        public TimeSpan? EarlyOutTime { get; set; }
        public LateInEarlyOutRequestType LateInEarlyOutRequestType { get; set; }

        public virtual EEmployee Employee { get; set; }
    }
    public enum LateInEarlyOutRequestType
    {
        LateIn,
        EarlyOut,
    }
}
