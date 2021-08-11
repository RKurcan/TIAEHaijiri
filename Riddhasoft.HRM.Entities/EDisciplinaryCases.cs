using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities
{
    public class EDisciplinaryCases
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200), Required]
        public string CaseName { get; set; }
        [StringLength(500), Required]
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DisciplinaryStatus DisciplinaryStatus { get; set; }
        public DisciplinaryActions DisciplinaryActions { get; set; }
        public int BranchId { get; set; }
        public int ForwardToId { get; set; }

        public virtual EEmployee ForwardTo { get; set; }

    }
    public class EDisciplinaryCasesDetail
    {
        public int Id { get; set; }
        public int DisciplinaryCasesId { get; set; }
        public int EmployeeId { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual EDisciplinaryCases DisciplinaryCases { get; set; }
    }
    public enum DisciplinaryStatus
    {
        InProgress,
        Close
    }
    public enum DisciplinaryActions
    {
        GiveVerbalWarning,
        GiveWrittenWarning,
        HaveDisciplinaryHearing,
        ProvideCounselling,
        PutOnProbation,
        SendOnAdministrativeLeave,
        Suspend,
        Terminate
    }
  
    
}
