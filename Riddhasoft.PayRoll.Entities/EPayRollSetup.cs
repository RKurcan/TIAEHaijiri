using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;

namespace Riddhasoft.PayRoll.Entities
{
    //public class EPayRollSetup
    //{
    //    [Key]
    //    public int Id { get; set; }
    //    public int EmployeeId { get; set; }
    //    public DateTime EffectedFrom { get; set; }
    //    public decimal BasicSaliry { get; set; }
    //    public SalaryPaidPer SalaryPaidPer { get; set; }
    //    public string PFNo { get; set; }
    //    public string ESINo { get; set; }
    //    public string PANNo { get; set; }
    //    public Paymentby PaymentBy { get; set; }
    //    public TypeOfEmployee TypeOfEmployee { get; set; }
    //    public decimal GrossAmount { get; set; }
    //    public string AccountNo { get; set; }
    //    public decimal OtRatePerHour { get; set; }
    //    public PayRatePer OTPayPer { get; set; }
    //    public decimal Conveyance { get; set; }
    //    public PayRatePer ConveyancePayPer { get; set; }
    //    public decimal Medical { get; set; }
    //    public PayRatePer MedicalPayPer { get; set; }
    //    public decimal HRA { get; set; }
    //    public PayRatePer HRAPayPer { get; set; }
    //    public decimal TDS { get; set; }
    //    public TdsPaidBy TdsPaidBy { get; set; }
    //    public decimal DA { get; set; }
    //    public DAPaidBy DApaidBy { get; set; }
    //    public decimal? CITRate { get; set; }
    //    public int BankId { get; set; }


    //    public virtual EBank Bank { get; set; }
    //    public virtual EEmployee Employee { get; set; }

    //}
    public class EPayRollSetup
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime EffectedFrom { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal BasicSalary { get; set; }
        public SalaryPaidPer SalaryPaidPer { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal OtRatePerHour { get; set; }
        public PayRatePer OTPayPer { get; set; }
        public decimal TDS { get; set; }
        public TdsPaidBy TdsPaidBy { get; set; }
        public decimal CITRate { get; set; }
        public decimal PFRate { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public int BranchId { get; set; }
        public bool EnableLateDeduction { get; set; }
        public int LateGraceDay { get; set; }
        public LateDeductionBy LateDeductionBy { get; set; }
        public decimal LateDeductionRate { get; set; }
        public bool EnableEarlyDeduction { get; set; }
        public int EarlyGraceDay { get; set; }
        public LateDeductionBy EarlyDeductionBy { get; set; }
        public decimal EarlyDeductionRate { get; set; }
        public DateTime? SSFEffectedFromDate { get; set; }

        public virtual EEmployee Employee { get; set; }
    }
    public class EPayRollAdditionalAllowance
    {
        [Key]
        public int Id { get; set; }
        public int PayrollId { get; set; }
        public string AllowanceName { get; set; }
        public decimal AllowanceValue { get; set; }
        public virtual EPayRollSetup Payroll { get; set; }

    }
    public enum TypeOfEmployee
    {
        Permanent = 0,
        Temprory = 1,
    }
    public enum Paymentby
    {
        Cash = 0,
        Cheque = 1,
    }
    public enum PayRatePer
    {
        Days = 0,
        Hour = 1
    }
    public enum SalaryPaidPer
    {
        month = 0,
        hour = 1
    }
    public enum TdsPaidBy
    {
        NetSalary = 0,
        BasicSalary = 1,
    }
    public enum DAPaidBy
    {
        None = 0
    }
    public enum LateDeductionBy
    {
        Days = 0,
        HalfDay = 1,
        Hour = 2,
        SingleDay = 3
    }
}
