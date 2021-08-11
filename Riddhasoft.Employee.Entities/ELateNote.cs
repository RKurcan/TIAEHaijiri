using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class ELateNote
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedById { get; set; }
        public int BranchId { get; set; }
    }
}
