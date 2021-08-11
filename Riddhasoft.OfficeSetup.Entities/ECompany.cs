using System;
using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class ECompany
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100), Required]
        public string Name { get; set; }
        [StringLength(100)]
        public string NameNp { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(150), Required]
        public string Address { get; set; }
        [StringLength(150)]
        public string AddressNp { get; set; }
        [StringLength(50), Required]
        public string ContactNo { get; set; }
        [StringLength(100), Required]
        public string ContactPerson { get; set; }
        [StringLength(100)]
        public string ContactPersonNp { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [StringLength(250)]
        public string WebUrl { get; set; }
        [StringLength(25)]
        public string PAN { get; set; }
        public bool IsSuspended { get; set; }
        public bool EnableMobile { get; set; }
        public string LogoUrl { get; set; }
        public int ResellerId { get; set; }
        public SoftwarePackageType SoftwarePackageType { get; set; }
        public decimal Price { get; set; }
        public bool AllowDepartmentwiseAttendance { get; set; }
        public SoftwareType SoftwareType { get; set; }
        public bool EmploymentStatusWiseLeave { get; set; }
        public bool AutoLeaveApproved { get; set; }
        public TimeSpan MinimumOTHour { get; set; }
        public OrganizationType OrganizationType { get; set; }
        public virtual EReseller Reseller { get; set; }
    }
    public class ECompanyLicense
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int LicensePeriod { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual ECompany Company { get; set; }
    }
    public class ECompanyLicenseLog
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime IssueDate { get; set; }
        public int LicensePeriod { get; set; }
        public bool IsPaid { get; set; }
        public decimal Rate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string FileAttachment { get; set; }
        public string Remark { get; set; }
        public virtual ECompany Company { get; set; }
    }
    public enum SoftwarePackageType
    {
        Silver,
        Gold,
        Platinum,
        Corporate
    }
    public enum SoftwareType
    {
        Web,
        Desktop
    }
    public enum OrganizationType
    {
        NonGovernment,
        Government
    }

    public enum PaymentMethod
    {
        Credit,
        Cash,
        Bank,
        OnlineTransfer
    }
}
