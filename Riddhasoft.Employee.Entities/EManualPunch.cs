using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
   public class EManualPunch
    {
        [Key]
        public int Id { get; set; }
        public string Remark { get; set; }
        public DateTime DateTime { get; set; }
        public int EmployeeId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public virtual EBranch Branch { get; set; }
        public virtual ECompany Company { get; set; }
        public virtual EEmployee Employee { get; set; }



    }
}
