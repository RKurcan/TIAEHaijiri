using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities.Training
{
    public class ECourse
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150), Required]
        public string Title { get; set; }
        public int DepartmentId { get; set; }
        public int Version { get; set; }
        public int SubVersion { get; set; }
        public Currency Currency { get; set; }
        public decimal Cost { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int CoordinatorId { get; set; }
        public int BranchId { get; set; }

        public virtual EEmployee Coordinator { get; set; }
        public virtual EDepartment Department { get; set; }

        public virtual EBranch Branch { get; set; }
    }

    public enum Currency
    {
        NepaliRupees = 0,
        IndianRupees = 1,
        AmericanDollar = 2
    }
}
