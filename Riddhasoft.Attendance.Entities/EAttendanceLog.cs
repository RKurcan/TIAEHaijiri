using Riddhasoft.Device.Entities;
using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Riddhasoft.Attendance.Entities
{
    public class EAttendanceLog
    {
        [Key]
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int EmployeeId { get; set; }
        public int VerifyMode { get; set; }
        public DateTime DateTime { get; set; }
        /// <summary>
        /// from device or manully punched
        /// </summary>
        public string Remark { get; set; }
        public bool IsDelete { get; set; }
        public string CompanyCode { get; set; }
        public decimal Temperature { get; set; }

        public virtual EEmployee Employee { get; set; }
    }

    public class FingerPrint
    {

        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int UserPassword { get; set; }
        public string Name { get; set; }
        public string FingerPrintId { get; set; }
        public bool Select { get; set; }
        public byte[] FingerTemplate = new byte[10800];
        public int Previledge { get; set; }
        public string sTmpDate { get; set; }
    }
}
