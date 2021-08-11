using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class EHolidayEmployee
    {
        [Key]
        public int Id { get; set; }
        public int HolidayId { get; set; }
        public int EmployeeId { get; set; }
        public virtual EHoliday Holiday { get; set; }
        public virtual EEmployee Employee { get; set; }
    }
}
