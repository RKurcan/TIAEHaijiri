using System;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EDesktopProductKey
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime SystemDate { get; set; }
        public int DeviceCount { get; set; }
        public string MAC { get; set; }
        public string Key { get; set; }
        public bool IsPaid { get; set; }
        public virtual ECompany Company { get; set; }

    }
}
