using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.DB.SP_Result
{
    public class Leave_Application_SP_Result
    {

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveMaster { get; set; }
        public int LeaveMasterId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string LeaveDayName { get; set; }
        public string LeaveDay { get; set; }
        public string Description { get; set; }
        public int ApprovedById { get; set; }
        public string LeaveStatusName { get; set; }
        public string LeaveStatus { get; set; }
        public int Days { get; set; }
        public string ApprovedByUser { get; set; }
    }
}
