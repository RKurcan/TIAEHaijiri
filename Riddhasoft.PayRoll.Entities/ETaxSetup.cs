using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class ETaxSetup
    {

        public int Id { get; set; }
        public int FiscalYearId { get; set; }

        public string FiscalYear { get; set; }
        
        public DateTime CreationDate { get; set; }
        public int BranchId { get; set; }

        #region Tax Slab for tax value above final value 

        public decimal TaxPercAboveFinalValue { get; set; }

        #endregion
        #region Maximum Deduction Limit 

        /// <summary>
        /// Example :  IN + PF + CIT = Rs 300000 
        /// </summary>
        public decimal DeductionLimitAmount { get; set; }
        /// <summary>
        /// Example : 1 / 3 of gross salary
        /// </summary>
        public string DeductionLimitRatio { get; set; }

        /// <summary>
        /// Rebate perc of tax calculated 
        /// </summary>
        public decimal RebatePercForFemaleUnmarried { get; set; }

        #endregion


        #region SSF Information 

        /// <summary>
        /// Provident Fund  By Employer
        /// </summary>
        public decimal PFPercByEmployer { get; set; }



        /// <summary>
        /// Provident Fund  By Employee
        /// </summary>
        public decimal PFPercByEmployee { get; set; }


        /// <summary>
        /// Gratituity  By Employee
        /// </summary>
        public decimal GratituityPercByEmployer { get; set; }


        /// <summary>
        /// Social Security  By Employer
        /// </summary>
        public decimal SSPercByEmployer { get; set; }

        /// <summary>
        /// Social Security  By Employee

        /// </summary>
        public decimal SSPercByEmployee { get; set; }



        /// <summary>
        /// Pension Fund  By Employer
        /// </summary>
        public decimal PensionFundPercByEmployer { get; set; }

        /// <summary>
        /// Pension Fund By Employee
        /// </summary>
        public decimal PensionFundPercByEmployee { get; set; }




        #endregion
    }

    public class ETaxSlabDetails {


        public int Id { get; set; }

        public int TaxSetupId { get; set; }

        public int SN { get; set; }
        public decimal TaxPerc { get; set; }
        public decimal IndividualAmount { get; set; }
        public decimal CoupleAmount { get; set; }

        public virtual ETaxSetup TaxSetup { get; set; }

    }

}
