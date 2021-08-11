using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.HRM.Entities
{
    public class EEmploymentStatus
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool IsContract { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
        public int? BranchId { get; set; }
        public virtual EBranch Branch { get; set; }
    }

    public class EEmploymentStatusWiseLeavedBalance
    {
        public int Id { get; set; }
        public int LeaveId { get; set; }
        public int EmploymentStatusId { get; set; }
        public decimal Balance { get; set; }
        public decimal MaxLimit { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public bool IsMapped { get; set; }
        public bool IsReplacementLeave { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ELeaveMaster Leave { get; set; }
        public virtual EEmploymentStatus EmploymentStatus { get; set; }
    }
    public class EEmploymentStatusWiseLeavedBalanceHist
    {
        public int Id { get; set; }
        public int LeaveId { get; set; }
        public int EmploymentStatusId { get; set; }
        public decimal Balance { get; set; }
        public decimal MaxLimit { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public bool IsMapped { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ELeaveMaster Leave { get; set; }
        public virtual EEmploymentStatus EmploymentStatus { get; set; }
    }

}
