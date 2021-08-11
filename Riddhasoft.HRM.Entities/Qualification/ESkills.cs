using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities.Qualification
{
    public class ESkills
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(200), Required]
        public string Name { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
        public int? BranchId { get; set; }

        public virtual EBranch Branch { get; set; }
    }

    public class EEmployeeSkills
    {
        public int Id { get; set; }
        public int SkillsId { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public virtual ESkills Skills { get; set; }
        public virtual EEmployee Employee { get; set; }
    }

}
