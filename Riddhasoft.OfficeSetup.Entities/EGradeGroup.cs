using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EGradeGroup
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public string NameNp { get; set; }
        public int? BranchId { get; set; }
        public decimal Value { get; set; }
        public int CompanyId { get; set; }
        public virtual EBranch Branch { get; set; }
    }
}
