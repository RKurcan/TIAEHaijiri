using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EDemoRequest
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50), Required]
        public string Name { get; set; }
        [StringLength(150), Required]
        public string Address { get; set; }
        [StringLength(50), Required]
        public string ContactNo { get; set; }
        [StringLength(100), Required]
        public string ContactPerson { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [StringLength(250)]
        public string WebUrl { get; set; }
    }
}
