using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities
{
    public class EFOCTicket
    {
        [Key]
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AppliedDate { get; set; }
        public RequestType RequestType { get; set; }
        [StringLength(200)]
        public string SectorAFrom { get; set; }
        [StringLength(200)]
        public string SectorATo { get; set; }
        [StringLength(200)]
        public string SectorBFrom { get; set; }
        [StringLength(200)]
        public string SectorBTo { get; set; }
        public DateTime SectorADateOfFlight { get; set; }
        public DateTime? SectorBDateOfFlight { get; set; }
        [StringLength(100)]
        public string SectorAFlightNo { get; set; }
        [StringLength(100)]
        public string SectorBFlightNo { get; set; }
        public int Rebate { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int RecommendedBy { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
        public int BranchId { get; set; }

        public virtual EEmployee Employee { get; set; }
        public virtual EBranch Branch { get; set; }

    }
    public class EFOCTicketDetail
    {
        [Key]
        public int Id { get; set; }
        public int FOCTicketId { get; set; }
        public string Name { get; set; }
        public Relation Relation { get; set; }

        public virtual EFOCTicket FOCTicket { get; set; }
    }
    public enum RequestType
    {
        Oneway,
        Twoway
    }
    public enum Relation
    {
        Self,
        Spouse,
        Children,
        Parent,
        GrandParent,
        InLaw
    }
}
