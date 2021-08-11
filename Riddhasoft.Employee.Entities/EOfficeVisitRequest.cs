using Riddhasoft.OfficeSetup.Entities;
using System;

namespace Riddhasoft.Employee.Entities
{
    public class EOfficeVisitRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
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
        public string AdminRemark { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual EBranch Branch { get; set; }
    }
}
