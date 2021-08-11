using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class ENotice
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100), Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int CreatedById { get; set; }
        public int? BranchId { get; set; }
        public bool IsUrgent { get; set; }
        public NotificationLevel NoticeLevel { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        public int FiscalYearId { get; set; }
        public virtual EBranch Branch { get; set; }
        public virtual EUser CreatedBy { get; set; }
        public virtual EFiscalYear FiscalYear { get; set; }

    }
    public class ENoticeDetails
    {
        public int Id { get; set; }
        public int NoticeId { get; set; }
        public int TargetId { get; set; }
        public virtual ENotice Notice { get; set; }
    }
}
