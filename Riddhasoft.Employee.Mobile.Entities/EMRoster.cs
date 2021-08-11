using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Mobile.Entities
{
    public class EMRoster
    {
        public string RosterType { get; set; }
        public string Title { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string WorkCode { get; set; }
        public bool IsCurrentDay { get; set; }
    }
    public enum RosterType
    {
        Fixed,
        Weekly,
        Monthly
    }
}
