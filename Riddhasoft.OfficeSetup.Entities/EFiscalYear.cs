using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EFiscalYear
    {
        [Key]
        public int Id { get; set; }
        [StringLength(15), Required]
        public string FiscalYear { get; set; }
        [DisplayName("Current Fiscal Year")]
        public bool CurrentFiscalYear { get; set; }
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public virtual EBranch Branch { get; set; }
    }
}
