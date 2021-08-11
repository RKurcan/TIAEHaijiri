using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EEmployeeSalaryAndTaxPayable
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public int MonthId { get; set; }
        public int Year { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string JoinedMonth { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }

        #region Salary Payable
        public decimal TotalOTAmount { get; set; }
        public decimal TotalAllowancesAmount { get; set; }

        public decimal TotalAdditionAmount { get; set; }
        public decimal TotalDeductionAmount { get; set; }
        public decimal InsuranceAmount { get; set; }

        public decimal InsurancePaidbyOffice { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal NetSalary { get; set; }

        public bool IsPaidSalary { get; set; }

        #endregion

        #region Tax Payable
        /// <summary>
        /// PF Employee 
        /// </summary>
        public decimal PFEE { get; set; }

        /// <summary>
        /// PF Employer 
        /// </summary>
        public decimal PFER { get; set; }

        public decimal Gratituity { get; set; }

        /// <summary>
        /// Social Security Employee
        /// </summary>
        public decimal SSEE { get; set; }
        /// <summary>
        /// Social Security Employer
        /// </summary>
        public decimal SSER { get; set; }

        public decimal PensionFundER { get; set; }

        public decimal PensionFundEE { get; set; }

        
        public decimal CITAmount { get; set; }

        

        public decimal SocialSecurityTax { get; set; }
        public decimal RenumerationTax { get; set; }

        public bool IsPaidPF { get; set; }
        public bool IsPaidGratituity { get; set; }

        public bool IsPaidSS { get; set; }


        public bool IsPaidSSF { get; set; }
        public bool IsPaidCIT { get; set; }

        public decimal RebateAmount { get; set; }

        #endregion
        public int CreatedByUserId { get; set; }
        public DateTime CreationDateTime { get; set; }

        public int PaymentMadeById { get; set; }
        public DateTime? PaymentMadeDateTime { get; set; }

        public bool PaySlipGenerated { get; set; }

        public int PaySlipGeneratedById { get; set; }
        public DateTime? PaySlipGeneratedDateTime { get; set; }

        public decimal AbsentDeductionAmount { get; set; }
        public decimal LateDeductionAmount { get; set; }
        public decimal EarlyOutDeductionAmount { get; set; }




    }
}
