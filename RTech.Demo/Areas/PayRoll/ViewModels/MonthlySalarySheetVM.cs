using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Areas.PayRoll.ViewModels
{
    public class MonthlySalarySheetVM
    {


        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public string JoinedMonth { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }


        public string DepartmentName { get; set; }

        public decimal Grade { get; set; }

        public string GradeName { get; set; }
        public decimal BasicSalary { get; set; }

        public decimal GrossSalary { get; set; }


        #region Allowances 
        public List<Allowance> Allowances { get; set; }
        #endregion


        #region Pay Deductions 
        public List<PayDeduction> PayDeductions { get; set; }
        #endregion


        #region SSF Information 


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
        public decimal PensionFundEE { get; set; }
        public decimal PensionFundER { get; set; }



        #endregion
        #region Tax Info 


        public decimal TaxableAmount { get; set; }

        public decimal SocialSecurityTax { get; set; }
        public decimal RenumerationTax { get; set; }
        public MaritialStatus MaritalStatusEnum { get; set; }
        public string MaritalStatus { get; set; }
        public Gender GenderEnum { get; set; }

        public string Gender { get; set; }

        #endregion

        #region Insurance Info 


        public decimal InsurancePremiumAmount { get; set; }

        public decimal InsurancePaidbyOffice { get; set; }

        #endregion
        #region CIT 

        public decimal CITAmount { get; set; }

        public decimal CITRate { get; set; }



        #endregion

        public string Absent { get; set; }
        public decimal AbsentDeductionAmount { get; set; }
        public string Leave { get; set; }
        public string Late { get; set; }
        public string Latehours { get; set; }
        public decimal LateDeductionAmount { get; set; }
        public string EarlyOut { get; set; }
        public string EarlyOutHours { get; set; }
        public  decimal EarlyOutDeductionAmount { get; set; }
        public decimal AdditionAmount { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal RebateAmount { get; set; }
        public string OTHours { get; set; }
        public decimal OTAmount { get; set; }


        public decimal NetSalary { get; set; }

        public DateTime? SheetOfFromDate { get; set; }

        public DateTime? SheetOfToDate { get; set; }


        public bool IsApproved { get; set; }

        public bool salarySheethasBeenCreated { get; set; }

        public OrganizationType OrganizationType { get; set; }


        public EmploymentStatus EmploymentStatus { get; set; }
        public DateTime? SSFEffectiveFromDate { get; set; }
        public int DaysInMonth{ get; set; }
        public string Present { get;  set; }
        public int DaysWorked { get; set; }
        public int Weekend { get;  set; }
        public int HolidayCount { get;  set; }
    }


    public class SSFInformationVM {

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
    }
    public class Allowance {

        public int AllowanceId { get; set; }
        public string AllowanceHead { get; set; }
        public decimal AllowanceAmount { get; set; }

    }

    public class PayDeduction {

        public string DeductionHead { get; set; }
        public decimal DeductionAmount { get; set; }
    }



    public class MonthlySalaryPostingVM {

        public int MonthId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<MonthlySalarySheetVM> MonthlySalarySheet { get; set; }

    }

}