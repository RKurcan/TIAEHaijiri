using Riddhasoft.Device.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Areas.Device.Models
{
    public class DeviceAssignmentViewModel
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string Reseller { get; set; }
        public DateTime AssignOn { get; set; }
        public string Model { get; set; }
        public string DeviceSerialNo { get; set; }
        public string Status { get; set; }
        public string Company { get; set; }
        public int TotalCount { get; set; }
        public bool IsPrivate { get; set; }
    }
}