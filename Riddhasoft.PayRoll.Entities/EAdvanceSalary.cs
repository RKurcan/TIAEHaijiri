using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EAdvanceSalary
    {

        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public decimal PreviousDue { get; set; }
        public decimal RequestAmount { get; set; }

        public decimal Interest { get; set; }

        public decimal Installment { get; set; }
        public decimal InstallmentAmount { get; set; }

        public int BranchId { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual EEmployee Employee { get; set; }
    }
}
