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
  public  class EEvent
    {
      [Key]
      public int Id { get; set; }
      [StringLength(100)]
      public string Title { get; set; }
      public string Description { get; set; }
      public DateTime From { get; set; }
      public DateTime To { get; set; }
      public DateTime CreatedOn { get; set; }
      public NotificationLevel EventLevel { get; set; }
      public int? BranchId { get; set; }
      public int FiscalYearId { get; set; }
      public int CreatedById { get; set; }
      public virtual EBranch Branch { get; set; }
      public virtual EUser CreatedBy { get; set; }
      public virtual EFiscalYear FiscalYear { get; set; }
    }

    public class EEventDetails
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int TargetId { get; set; }
        public virtual EEvent Event { get; set; }
    }
    
}
