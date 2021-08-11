using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities.Training
{
    public class EParticipant
    {
        [Key]
        public int Id { get; set; }
        public int SessionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ParticipantStatus ParticipantStatus { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public int BranchId { get; set; }

        public virtual ESession Session { get; set; }


    }

    public class EParticipantDetail
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public int EmployeeId { get; set; }

        public virtual EParticipant Participant { get; set; }
        public virtual EEmployee Employee { get; set; }
    }

    public enum ParticipantStatus
    {
        Planned,
        InProcessing,
        Completed,
        Canceled
    }
}
