using System;
using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class ELeaveMaster
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(50), Required]
        public string Name { get; set; }
        [StringLength(100)]
        public string NameNp { get; set; }
        public decimal Balance { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public bool IsReplacementLeave { get; set; }
        public decimal MaximumLeaveBalance { get; set; }
        public virtual EBranch Branch { get; set; }
    }

    public class EDesignationWiseLeavedBalance
    {
        public int Id { get; set; }
        public int LeaveId { get; set; }
        public int DesignationId { get; set; }
        public decimal Balance { get; set; }
        public decimal MaxLimit { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public bool IsMapped { get; set; }
        public bool IsReplacementLeave { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ELeaveMaster Leave { get; set; }
        public virtual EDesignation Designation { get; set; }
    }
    public class EDesignationWiseLeavedBalanceHist
    {
        public int Id { get; set; }
        public int LeaveId { get; set; }
        public int DesignationId { get; set; }
        public decimal Balance { get; set; }
        public decimal MaxLimit { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public bool IsMapped { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ELeaveMaster Leave { get; set; }
        public virtual EDesignation Designation { get; set; }
    }


}
