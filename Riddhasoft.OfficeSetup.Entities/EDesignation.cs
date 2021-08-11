using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EDesignation
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10), Required]
        public string Code { get; set; }
        [StringLength(100), Required]
        public string Name { get; set; }
        [StringLength(200)]
        public string NameNp { get; set; }
        public int DesignationLevel { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
        public decimal? MaxSalary { get; set; }
        public decimal? MinSalary { get; set; }
        public virtual EBranch Branch { get; set; }

    }
}
