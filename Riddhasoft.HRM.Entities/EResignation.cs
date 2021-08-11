using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riddhasoft.Employee.Entities;

namespace Riddhasoft.HRM.Entities
{
    public class EResignation
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        public DateTime NoticeDate { get; set; }
        public DateTime DesiredResignDate { get; set; }
        [StringLength(300), Required]
        public string Reason { get; set; }
        [StringLength(300)]
        public string Details { get; set; }
        public int EmployeeId { get; set; }
        public int ForwardToId { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int BranchId { get; set; }
        public bool IsApproved { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FileUrl { get; set; }

        public virtual EEmployee Employee { get; set; }
        public virtual EEmployee ForwardTo { get; set; }
    }
}
