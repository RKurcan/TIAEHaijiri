using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Device.Entities
{
    public class EDevice
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string IpAddress { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int? Company_Id { get; set; }
        public int? ModelId { get; set; }
        [StringLength(50), Required]
        public string SerialNumber { get; set; }
        public DateTime? LastActivity { get; set; }
        public Status Status { get; set; }
        public DeviceType DeviceType { get; set; }
        public int CheckInOutIndex { get; set; }
        public bool IsFaceDevice { get; set; }
        public bool IsAccessDevice { get; set; }
        public virtual EModel Model { get; set; }
    }
    public enum Status
    {
        New = 0,
        Reseller = 1,
        Customer = 2,
        Damage = 3
    }
    public enum DeviceType
    {
        Normal = 0,
        ADMS = 1
    }

    public class EWdmsConfig
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
    public class EDevicewiseDepartment
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int DeviceId { get; set; }
        public virtual EDepartment Department { get; set; }
        public virtual EDevice Device { get; set; }

    }
}
