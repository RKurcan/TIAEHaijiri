using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class ELeaveBalance
    {

        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveMasterId { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal RemainingBalance { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual ELeaveMaster LeaveMaster { get; set; }
    }
    public class EReplacementLeaveBalance
    {
        [Key]
        public int Id { get; set; }
        public int FyId { get; set; }
        public int EmployeeId { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal RemainingBalance { get; set; }
        public virtual EEmployee Employee { get; set; }
    }
    public class EEmployeePresentInOffHist
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public PresentInHolidayOrDayOff PresentInHolidayOrDayOff { get; set; }
        public virtual EEmployee Employee { get; set; }

    }

    public enum PresentInHolidayOrDayOff
    {
        Holiday,
        DayOff,
        Both
    }
}
