using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EEmployeeGrade
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int GradeGroupId { get; set; }
        public DateTime EffectedFrom { get; set; }
        public DateTime EffectedTo { get; set; }
        public int BranchId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public virtual EEmployee Employee { get; set; }
    }
}
