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
    public class EAllowance
    {
        public int Id { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string NameNp { get; set; }
        public decimal Value { get; set; }
        public TimeSpan MinimumWorkingHour { get; set; }
        public AllowancePeriod AllowancePeriod { get; set; }
        public AllowanceCalculatedBy AllowanceCalculatedBy { get; set; }
        public AllowancePaidPer AllowancePaidPer { get; set; }        
        public int BranchId { get; set; }
        public virtual EBranch Branch { get; set; }
    }
    public class EEmployeeAlowance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int AllowanceId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal Value { get; set; }
        public TimeSpan MinimumWorkingHour { get; set; }
        public AllowancePeriod AllowancePeriod { get; set; }
        public AllowanceCalculatedBy AllowanceCalculatedBy { get; set; }
        public AllowancePaidPer AllowancePaidPer { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public int BranchId { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual EAllowance Allowance { get; set; }
    }
    public enum AllowanceCalculatedBy
    {
        Value,
        Percentage
    }
    public enum AllowancePaidPer
    {
        NetSalary = 0,
        BasicSalary = 1,
    }

    public enum AllowancePeriod
    {
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Annually
    }
}
