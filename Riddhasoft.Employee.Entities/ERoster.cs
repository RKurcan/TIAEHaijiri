
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class ERoster
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int ShiftId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime RosterCreationDate { get; set; }

        public virtual EEmployee Employee { get; set; }
        public virtual EShift Shift { get; set; }
    }
}
