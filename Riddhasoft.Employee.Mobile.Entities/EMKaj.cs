using System;

namespace Riddhasoft.Employee.Mobile.Entities
{
    public class EMKaj
    {
        public int EmployeeId { get; set; }
        public string Remark { get; set; }
        public DateTime FromDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}
