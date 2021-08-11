using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class EKaj
    {
        [Key]
        public int Id { get; set; }
        public string Remark { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int BranchId { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApprove { get; set; }
        public KajStatus KajStatus { get; set; }
        public virtual EBranch Branch { get; set; }
    }
    public class EKajDetail
    {
        public int Id { get; set; }
        public int KajId { get; set; }
        public int EmployeeId { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual EKaj Kaj { get; set; }
    }
    public enum KajStatus
    {
        New,
        Approve,
        Reject
    }
}
