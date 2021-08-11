using Riddhasoft.Employee.Entities;
using Riddhasoft.HRM.Entities.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities.Travel
{
    public class ETravelRequest
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }


        #region Travel Summary
        public string Purpose { get; set; }
        public decimal SumOfEstimateExpense { get; set; }
        public bool ApplyForCashAdvance { get; set; }
        /// <summary>
        /// Enable if apply for cash advance is true.
        /// </summary>
        public decimal AdvanceAmount { get; set; }
        public string Comment { get; set; }
        #endregion
        public virtual EEmployee Employee { get; set; }
    }
}
