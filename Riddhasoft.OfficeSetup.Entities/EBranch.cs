using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EBranch
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [StringLength(10), Required]
        public string Code { get; set; }
        [StringLength(100), Required]
        public string Name { get; set; }
        [StringLength(200)]
        public string NameNp { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(200)]
        public string AddressNp { get; set; }
        [StringLength(100)]
        public string ContactNo { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        public bool IsHeadOffice { get; set; }
        public virtual ECompany Company { get; set; }
    }
}
