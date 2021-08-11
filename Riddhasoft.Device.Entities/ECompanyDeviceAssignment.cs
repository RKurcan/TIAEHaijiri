using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Device.Entities
{
    public class ECompanyDeviceAssignment
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int DeviceId { get; set; }
        public int AssignedById { get; set; }
        public DateTime AssignedOn { get; set; }
        public virtual EUser AssignedBy { get; set; }
        public virtual ECompany Company { get; set; }
        public virtual EDevice Device { get; set; }
    }
}
