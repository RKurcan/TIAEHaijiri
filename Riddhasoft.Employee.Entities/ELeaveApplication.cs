using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class ELeaveApplication
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int LeaveMasterId { get; set; }
        public int CreatedById { get; set; }
        public DateTime TransactionDate { get; set; }
        //public bool IsApproved { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int? BranchId { get; set; }
        public LeaveDay LeaveDay { get; set; }
        [StringLength(200), Required]
        public string Description { get; set; }
        public string AdminRemark { get; set; }

        public virtual EBranch Branch { get; set; }
        public virtual EUser CreatedBy { get; set; }
        public virtual ELeaveMaster LeaveMaster { get; set; }
        public virtual EEmployee Employee { get; set; }

    }
    public enum LeaveStatus
    {
        New,
        Approve,
        Reject,
        Revert
    }
    public class EleaveApplicationLog
    {
        public int Id { get; set; }
        public int LeaveApplicationId { get; set; }
        public decimal LeaveCount { get; set; }
        public int FiscalYearId { get; set; }
        public virtual ELeaveApplication LeaveApplication { get; set; }
        public virtual EFiscalYear FiscalYear { get; set; }
    }

    public enum LeaveDay
    {
        FullDay = 0,
        EarlyLeave = 1,
        LateLeave = 2
    }
}
