using Riddhasoft.OfficeSetup.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.Employee.Entities
{
    public class EManualPunchRequest
    {
        [Key]
        public int Id { get; set; }
        public string Remark { get; set; }
        public string AdminRemark { get; set; }
        public DateTime DateTime { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public DateTime SystemDate { get; set; }
        public int? ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
        public bool IsApproved { get; set; }
        public string Image { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Altitude { get; set; }
         

        public virtual EEmployee Employee { get; set; }
        public virtual EBranch Branch { get; set; }
    }
}
