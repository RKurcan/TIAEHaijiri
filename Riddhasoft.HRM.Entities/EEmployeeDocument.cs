using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities
{
    public class EEmployeeDocument
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [StringLength(100)]
        public string PFNo { get; set; }
        [StringLength(100)]
        public string CITNo { get; set; }
        [StringLength(100)]
        public string BankACNo { get; set; }
        [StringLength(100)]
        public string PanNo { get; set; }
        public string PFFileUrl { get; set; }
        public string CITFileUrl { get; set; }
        public string AppointmentFileUrl { get; set; }
        public string ContractFileUrl { get; set; }
        public EEmployee Employee { get; set; }
    }
    public class EEmployeeOtherDocument
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200), Required]
        public string FileName { get; set; }
        [StringLength(500)]
        public string FileUrl { get; set; }
        public int EmployeeId { get; set; }

        public virtual EEmployee Employee { get; set; }
    }
}
