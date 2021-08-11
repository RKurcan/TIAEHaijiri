using Riddhasoft.Employee.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.HRM.Entities
{
    public class EContract
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        public DateTime BeganOn { get; set; }
        public DateTime EndedOn { get; set; }
        public int EmploymentStatusId { get; set; }
        public int EmployeeId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public int BranchId { get; set; }
        public string FileUrl { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual EEmploymentStatus EmploymentStatus { get; set; }
    }


}
