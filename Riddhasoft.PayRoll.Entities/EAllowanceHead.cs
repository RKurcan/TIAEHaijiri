using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Entities
{
    public class EAllowanceHead
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
