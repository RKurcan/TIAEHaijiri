using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.User.Entity
{
    public class ERoleOnModule
    {
        [Key]
        public int Id { get; set; }
        public Module Module { get; set; }
        public int RoleId { get; set; }
        public virtual EUserRole Role { get; set; }
    }

    public enum Module
    {
        Office,
        Employee,
        Payroll,
        Permission,
        UserSetup,
        Report,
        Device
    }
}
