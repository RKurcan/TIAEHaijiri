using System;

namespace Riddhasoft.Employee.Mobile.Entities
{
    public class EMOfficeVisitRequest
    {
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan ToTime { get; set; }
        public string Remark { get; set; }
        public int BranchId { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApprove { get; set; }
        public string Image { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Altitude { get; set; }
        public DateTime SystemDate { get; set; }
    }

}
