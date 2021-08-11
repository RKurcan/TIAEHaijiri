using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.Common
{
    public class KendoPageListArguments
    {
        public KendoPageListArguments()
        {
            Sort = new List<KendoGridSort>();
            Filter = new KendoGridFilter();
        }
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int BranchId { get; set; }
        public List<KendoGridSort> Sort { get; set; }
        public  KendoGridFilter Filter { get; set; }

        
    }
    public class KendoGridSort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
    public class KendoGridFilter
    {
        public KendoGridFilter()
        {
            Filters = new List<FilterObject>();
        }
        public List<FilterObject> Filters { get; set; }
    }
    public class FilterObject
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    
    }
}
