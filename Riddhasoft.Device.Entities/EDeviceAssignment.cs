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
    public class EDeviceAssignment
    {
        [Key]
        public int Id { get; set; }
        public int ResellerId { get; set; }
        public bool IsPrivate { get; set; }
        public int DeviceId { get; set; }
        public int AssignedById { get; set; }
        public DateTime AssignedOn { get; set; }
        public virtual EUser AssignedBy { get; set; }
        public virtual EReseller Reseller { get; set; }
        public virtual EDevice Device { get; set; }
    }
}
