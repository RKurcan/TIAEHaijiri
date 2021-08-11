using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Entity.User
{
    public class EOwnerPermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ControllerId { get; set; }
        public virtual EUserRole Role { get; set; }
    }
    public static class OwnerPermissionData
    {
        public static List<OwnerActionPermission> GetOwnerPermissionList()
        {
            return new List<OwnerActionPermission>(){
                new OwnerActionPermission(){Id=1,Module="Office",Controller="Reseller"},
                new OwnerActionPermission(){Id=2,Module="Office",Controller="Model"},
                new OwnerActionPermission(){Id=3,Module="Office",Controller="Device"},
                new OwnerActionPermission(){Id=4,Module="Office",Controller="DeviceAssignment"},
                new OwnerActionPermission(){Id=5,Module="Office",Controller="UserRole"},
                new OwnerActionPermission(){Id=6,Module="Office",Controller="User"},
                new OwnerActionPermission(){Id=7,Module="Office",Controller="OwnerPermission"},
                new OwnerActionPermission(){Id=8,Module="Office",Controller="DemoRequest"},
            };
        }
    }
    public class OwnerActionPermission
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public string Controller { get; set; }
    }
}
