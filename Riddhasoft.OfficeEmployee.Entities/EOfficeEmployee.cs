using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeEmployee.Entities
{
    public class EOfficeEmployee
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public virtual EEmployee Employee { get; set; }
    }
}
