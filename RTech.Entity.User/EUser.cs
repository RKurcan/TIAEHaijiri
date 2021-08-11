using Riddhasoft.OfficeSetup.Entities;
using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.User.Entity
{
    public class EUser
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public UserType UserType { get; set; }
        public int? BranchId { get; set; }
        public string FullName { get; set; }
        public string PhotoURL { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
        public virtual EUserRole Role { get; set; }
        public virtual EBranch Branch { get; set; }
    }
    public class ECompanyLogin
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }

        public int UserId { get; set; }
        public virtual EUser User { get; set; }
        public virtual ECompany Company { get; set; }
    }

    public class EResellerLogin
    {
        public int Id { get; set; }
        public int ResellerId { get; set; }
        public int UserId { get; set; }
        public bool IsActivated { get; set; }
        public string ActivationCode { get; set; }
        public virtual EUser User { get; set; }
        public virtual EReseller Reseller { get; set; }
    }
    public enum UserType
    {
        Admin,
        Reseller,
        User,
        Owner,
        Employee
    }
}
