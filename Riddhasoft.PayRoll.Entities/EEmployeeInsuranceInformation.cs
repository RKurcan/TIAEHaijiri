using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EEmployeeInsuranceInformation
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int InsuranceCompanyId { get; set; }
        public string PolicyNo { get; set; }
        public string PolicyAmount { get; set; }
        public decimal PremiumAmount { get; set; }
        public string InsuraneDocument { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int BranchId { get; set; }
        public virtual EInsuranceCompany InsuranceCompany { get; set; }
        public virtual EEmployee Employee { get; set; }

    }
}
