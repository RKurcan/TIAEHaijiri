using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.Common
{
    public class EKendoAutoComplete
    {
        public EKendoAutoComplete()
        {
            Filter = new KendoAutoCompleteFilter();
        }
        public KendoAutoCompleteFilter Filter { get; set; }
        public int BranchId { get; set; }
    }
    public class KendoAutoCompleteFilter
    {
        public KendoAutoCompleteFilter()
        {
            Filters = new List<AutoCompleteFilterObject>();
        }
        public List<AutoCompleteFilterObject> Filters { get; set; }
    }
    public class AutoCompleteFilterObject
    {
        public string Value { get; set; }
        public string Operator { get; set; }
        public string Field { get; set; }
        public bool IgnoreCase { get; set; }
        public string Logic { get; set; }
    }
}
