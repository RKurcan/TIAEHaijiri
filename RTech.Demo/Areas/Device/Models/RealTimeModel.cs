using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Areas.Device.Models
{
    public class RealTimeModel
    {
        public string CompanyCode { get; set; }
        public int EnrollId { get; set; }
        public DateTime Date { get; set; }
        public string DeviceSN { get; set; }
        public int VerifyMode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}