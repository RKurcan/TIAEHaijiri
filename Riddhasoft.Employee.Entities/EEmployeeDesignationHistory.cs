using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class EEmployeeDesignationHistory
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int DesignationId { get; set; }
        public int BranchId { get; set; }
        public DateTime DateTime { get; set; }
        public virtual EEmployee Employee { get; set; }
    }
}
