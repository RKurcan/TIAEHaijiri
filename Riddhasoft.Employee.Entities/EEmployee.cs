using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.User.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.Employee.Entities
{
    public class EEmployee
    {
        [Key]

        #region Official record
        public int Id { get; set; }
        [StringLength(10), Required]
        public string Code { get; set; }
        [StringLength(100), Required]
        public string Name { get; set; }
        [StringLength(200)]
        public string NameNp { get; set; }
        public int? SectionId { get; set; }
        public int? DesignationId { get; set; }
        public int? GradeGroupId { get; set; }
        public int? BranchId { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public int DeviceCode { get; set; }
        public bool IsManager { get; set; }
        public virtual ESection Section { get; set; }
        public virtual EGradeGroup GradeGroup { get; set; }
        public virtual EDesignation Designation { get; set; }
        public virtual EBranch Branch { get; set; }
        #endregion

        #region BAsic Information
        public DateTime? DateOfBirth { get; set; }
        public MaritialStatus MaritialStatus { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public Gender Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentAddressNp { get; set; }
        public string TemporaryAddress { get; set; }
        public string TemporaryAddressNp { get; set; }
        public string PassportNo { get; set; }
        public string CitizenNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssueDistict { get; set; }
        public Religious Religion { get; set; }
        public string ImageUrl { get; set; }
        public string PANNo { get; set; }
        public string SSNNo { get; set; }
        public bool EnableSSN { get; set; }
        #endregion

        #region Shift Policy /w0 management
        /// <summary>
        /// fixed ->0
        /// rotation -> 1
        /// </summary>
        public int? ShiftTypeId { get; set; }
        /// <summary>
        /// fixed->0
        /// weekly->1
        /// Monthly->2
        /// </summary>
        public int? WOTypeId { get; set; }

        #endregion

        #region Time/Punch Managment
        public TimeSpan MaxWorkingHour { get; set; }
        public TimeSpan AllowedLateIn { get; set; }
        public TimeSpan AllowedEarlyOut { get; set; }
        public TimeSpan HalfdayWorkingHour { get; set; }
        public TimeSpan ShortDayWorkingHour { get; set; }
        public TimeSpan PresentMarkingDuration { get; set; }
        public bool NoPunch { get; set; }
        public bool SinglePunch { get; set; }
        public bool MultiplePunch { get; set; }
        public bool TwoPunch { get; set; }
        public bool FourPunch { get; set; }
        public bool ConsiderTimeLoss { get; set; }
        public bool HalfDayMarking { get; set; }
        #endregion

        public int? UserId { get; set; }
        public int? ReportingManagerId { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public bool IsOTAllowed { get; set; }
        public TimeSpan? MaxOTHour { get; set; }
        public TimeSpan? MinOTHour { get; set; }
        public int BankId { get; set; }
        public string BankAccountNo { get; set; }

        public string PFNo { get; set; }
        public string CITNo { get; set; }
        public virtual EUser User { get; set; }

    }
    public class EEmployeeLogin
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string UserName { get; set; }
        [Required, StringLength(50)]
        public string Password { get; set; }
        public int BranchId { get; set; }
        public int RoleId { get; set; }
        public bool IsActivated { get; set; }
        public virtual EEmployee Employee { get; set; }
        public virtual EBranch Branch { get; set; }
        public virtual EUserRole Role { get; set; }

    }
    public enum MaritialStatus
    {
        Married = 0,
        UnMarried = 1,
        Divorced = 2,
        NotSpecified = 3
    }
    public enum BloodGroup
    {
        APositive = 0,
        ANeagtive = 1,
        BPositive = 3,
        BNeagtive = 4,
        ABPositive = 5,
        ABNeagtive = 6,
        OPositive = 7,
        ONeagtive = 8,
    }
    public enum Religious
    {
        Hinduism = 0,
        Buddhism = 1,
        Islam = 2,
        Christianity = 3,
        Judaism = 4,

    }
    public enum Gender
    {
        Male = 0,
        Female = 1,
        Others = 2,
    }
    public enum WeeklyOfType
    {
        Fixed = 0,
        Daynamic = 1,
    }
    public enum PrimaryWeeklyOfType
    {
        None = 0,
        Sunday = 1,
        Modnday = 2,
        Tuesday = 3,
        Wednesday4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7,
    }

    public class EEmployeeShitList
    {
        [Key]
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public int AfterDays { get; set; }
        public int EmployeeId { get; set; }
        public virtual EShift Shift { get; set; }
        public virtual EEmployee Employee { get; set; }
    }
    public class EEmployeeWOList
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 0->sunday
        /// 1->monday
        /// 2->tuesday
        /// .
        /// .
        /// .
        /// 6->saturday
        /// </summary>
        public int OffDayId { get; set; }
        public int AfterDays { get; set; }
        public int EmployeeId { get; set; }

        public virtual EEmployee Employee { get; set; }
    }
    public enum EmploymentStatus
    {
        NormalEmployment = 0,
        Deceased,
        Defaulter,
        Terminated,
        Resigned,
        EarlyRetirement,
        NormalRetirement,
        ContractPeriodOver,
        OnContract,
        PermanentJob,
        Retiring
    }

}
