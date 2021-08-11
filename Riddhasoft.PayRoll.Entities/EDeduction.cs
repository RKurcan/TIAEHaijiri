using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EDeduction
    {
        public int Id { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DeductionCalculatedBy DeductionCalculatedBy { get; set; }
        public DeductionPaidPer DeductionPaidPer { get; set; }
        public int BranchId { get; set; }
        public virtual EBranch Branch { get; set; }
    }
    public class EEmployeeDeduction
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int DeductionId { get; set; }
        public decimal Value { get; set; }
        public DeductionCalculatedBy DeductionCalculatedBy { get; set; }
        public DeductionPaidPer DeductionPaidPer { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public int BranchId { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual EDeduction Deduction { get; set; }
    }
    public enum DeductionCalculatedBy
    {
        Value,
        Percentage
    }
    public enum DeductionPaidPer
    {
        NetSalary = 0,
        BasicSalary = 1,
    }
}
