using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EInsuranceCompany
    {

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }

        public string Address { get; set; }
        public string ContactNo { get; set; }

        public int BranchId { get; set; }


    }
}
