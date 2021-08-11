using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EBank
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(50),Required]
        public string Name { get; set; }
        [StringLength(100)]
        public string NameNp { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(300)]
        public string AddressNp { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }

        public virtual EBranch Branch { get; set; }
    }
}
