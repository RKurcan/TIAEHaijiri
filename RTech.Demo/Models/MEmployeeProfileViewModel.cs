using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Models
{
    public class MEmployeeProfileViewModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string DateOfBirth { get; set; }
        public string DeviceCode { get; set; }
    }
}