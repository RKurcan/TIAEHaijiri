using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities.Travel
{
    public class ETravelInformation
    {
        public int Id { get; set; }
        public string MainDestination { get; set; }
        public DateTime TravelPeriodFrom { get; set; }
        public DateTime TravelPeriodTo { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string DestinationAddress { get; set; }
        public int BranchId { get; set; }
        public int TravelRequestId { get; set; }

        public virtual ETravelRequest TravelRequest { get; set; }
    }
}
