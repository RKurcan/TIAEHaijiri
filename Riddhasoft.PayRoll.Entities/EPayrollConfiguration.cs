using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EPayrollConfiguration
    {
        [Key]
        public int Id { get; set; }
        public PresentInHoliday PresentInHoliday { get; set; }
        public PresentInDayOff PresentInDayOff { get; set; }
        public decimal InsuranceContributionByEmpyr { get; set; }

        public int BranchId { get; set; }
    }

    public enum PresentInHoliday
    {
        OTAllow,
        ReplacementLeave,
        None
    }
    public enum PresentInDayOff
    {
        OTAllow,
        ReplacementLeave,
        None
    }
}
