using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class EWeeklyRoster
    {

        [Key]
        public int Id { get; set; }

        public Day Day { get; set; }

        public int ShiftId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime RosterCreationDate { get; set; }
        public DateTime? RosterExpireDate { get; set; }
        public bool IsActive { get; set; }

        public virtual EEmployee Employee { get; set; }
        public virtual EShift Shift { get; set; }

    }
    public enum Day
    {
        sunday = 0,
        monday = 1,
        tuesday = 2,
        wednesday = 3,
        thursday = 4,
        friday = 5,
        saturday = 6,
        none = 7
    }
}
