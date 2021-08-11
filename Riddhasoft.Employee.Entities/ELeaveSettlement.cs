using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class ELeaveSettlement
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveMasterId { get; set; }
        public int FiscalYearId { get; set; }
        public decimal Balance { get; set; }
        public SettlementType SettlementType { get; set; }
        public int? BranchId { get; set; }
        public virtual EBranch Branch { get; set; }
        public virtual ELeaveMaster LeaveMaster { get; set; }
        public virtual EFiscalYear FiscalYear { get; set; }
        public virtual EEmployee Employee { get; set; }
    }
    public class ELeaveCarryForwardBalance
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveMasterId { get; set; }
        public decimal Balance { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual ELeaveMaster LeaveMaster { get; set; }
        public virtual EFiscalYear FiscalYear { get; set; }
    }
    public enum SettlementType
    {
        Paid = 0,
        CarrytoNext = 1
    }
}
