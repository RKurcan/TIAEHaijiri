using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Mobile.Entities
{
    public class EMLeaveInfo
    {
        public int LeaveId { get; set; }
        public string LeaveName { get; set; }
        public decimal RemLeave { get; set; }
        public decimal TakenLeave { get; set; }
        public decimal UnapprovedLeave { get; set; }
    }

    
}
