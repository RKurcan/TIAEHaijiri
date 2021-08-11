using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.User.Entity
{
    public class EUserRole
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Name Of the role like <b>Admin,Super Admin ,manager, account manager ,Inventory Manager,etc</b>
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string NameNp { get; set; }
        /// <summary>
        /// priority is set for the role 
        /// this comment is for informative purpose
        /// roles priority dont have direct use but it can be used in future for reporting or any other priority wise order
        /// Default zero.
        /// </summary>
        /// 
        public int Priority { get; set; }

        public int? BranchId { get; set; }
        public virtual EBranch Branch { get; set; }
    }
}
