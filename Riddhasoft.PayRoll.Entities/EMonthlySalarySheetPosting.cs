using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EMonthlySalarySheetPosting
    {

        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }

        public int Year { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }


        public string DepartmentName { get; set; }

        public decimal Grade { get; set; }
        public string GradeName { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal GrossSalary { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal SocialSecurityTax { get; set; }
        public decimal RenumerationTax { get; set; }
        public MaritialStatus MaritalStatusEnum { get; set; }
        public Gender GenderEnum { get; set; }

        #region SSFInfo

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
        /// <summary>
        /// Pension Fund Employee
        /// </summary>
        public decimal PensionFundEE { get; set; }
        /// <summary>
        /// Pension Fund Employer
        /// </summary>
        public decimal PensionFundER { get; set; }



        #endregion


        #region Insurance Info 
        public decimal InsurancePremiumAmount { get; set; }


        public decimal InsurancePaidbyOffice { get; set; }

        #endregion
        #region CIT 
        public decimal CITAmount { get; set; }

        #endregion


        public string Absent { get; set; }
        public string Leave { get; set; }
        public string LateIn { get; set; }
        public string EarlyOut { get; set; }

        // Pay Deduction Property 
        //item.AbsentDeductionAmount   
        //item.LateDeductionAmount     
        //item.EarlyOutDeductionAmount 
        public decimal AbsentDeductionAmount { get; set; }
        public decimal LateDeductionAmount { get; set; }
        public decimal EarlyOutDeductionAmount { get; set; }


        public string OtHours { get; set; }
        public decimal OtAmount { get; set; }
        public decimal AdditionAmount { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal RebateAmount { get; set; }

        public decimal NetSalary { get; set; }

        public int BranchId { get; set; }

        public int CreatedByUserId { get; set; }
        
        public DateTime CreationDateTime { get; set; }
        public bool IsApproved { get; set; }
        public int ApprorvedByUserId { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public SalaryStatus SalaryStatus { get; set; }



        public virtual EEmployee Employee { get; set; }


    }
    public class EMonthlySalarySheetAllowances {

        public int Id { get; set; }
        public int MonthlySalarySheetPostingId { get; set; }
        public int AllowanceId { get; set; }
        public string AllowanceHead { get; set; }
        public decimal AllowanceAmount { get; set; }

        public virtual EMonthlySalarySheetPosting MonthlySalarySheetPosting { get; set; }
        }

    public class EMonthlySalarySheetDeductions {
        public int Id { get; set; }
        
        public int MonthlySalarySheetPostingId{ get; set; }
        public string DeductionHead { get; set; }
        public decimal DeductionAmount { get; set; }

        public virtual EMonthlySalarySheetPosting MonthlySalarySheetPosting { get; set; }




    }

    public enum SalaryStatus {

        New, 
        Payable,
        Paid
    }

}
