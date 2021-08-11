using Riddhasoft.Device.Entities;
using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.EHajiriPro.Desktop.ViewModel
{
    public class SectionGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public int? BranchId { get; set; }
        public int DepartmentId { get; set; }
    }

    public class DesignationGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DesignationLevel { get; set; }
        public int? BranchId { get; set; }
        public decimal? MaxSalary { get; set; }
        public decimal? MinSalary { get; set; }
    }
    public class ShiftGridVm
    {
        public int Id { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string NameNp { get; set; }
        public TimeSpan? EarlyGrace { get; set; }
        public TimeSpan? LateGrace { get; set; }
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public TimeSpan LunchStartTime { get; set; }
        public TimeSpan LunchEndTime { get; set; }
        public ShiftType ShiftType { get; set; }
        public int NumberOfStaff { get; set; }
        public int? BranchId { get; set; }

        public bool ShortDayWorkingEnable { get; set; }
        public TimeSpan ShiftStartGrace { get; set; }
        public TimeSpan ShiftEndGrace { get; set; }
        public NepaliMonth StartMonth { get; set; }
        public int StartDays { get; set; }
        public NepaliMonth EndMonth { get; set; }
        public int EndDays { get; set; }
        public TimeSpan? HalfDayWorkingHour { get; set; }
        public bool DeclareAbsentForLateIn { get; set; }
        public bool DeclareAbsentForEarlyOut { get; set; }
        public string ShiftTypeName { get; set; }
    }
    public class DeviceGridVm
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public int? Company_Id { get; set; }
        public int? ModelId { get; set; }
        public string ModelName { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? LastActivity { get; set; }
        public Status Status { get; set; }
        public DeviceType DeviceType { get; set; }
        public int CheckInOutIndex { get; set; }
    }
    public class LeaveMasterGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public string ApplicableGenderName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? BranchId { get; set; }
        public bool IsReplacementLeave { get; set; }
    }

    public class EmployeeSearchVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IDCardNo { get; set; }
        public string Department { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
    }

    public class LeaveQuataGridVm
    {
        public int Id { get; set; }
        public int LeaveMasterId { get; set; }
        public string LeaveName { get; set; }
        public decimal Balance { get; set; }
        public int ApplicableGender { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public bool IsMapped { get; set; }

    }

    public class LeaveApplicationGridVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int LeaveMasterId { get; set; }
        public string LeaveMasterName { get; set; }
        public int CreatedById { get; set; }
        public DateTime TransactionDate { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public string LeaveStatusName { get; set; }
        public int? ApprovedById { get; set; }
        public string ApproveByUser { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int? BranchId { get; set; }
        public LeaveDay LeaveDay { get; set; }
        public string LeaveDayName { get; set; }
        public string Description { get; set; }
        public decimal TotalLeaveDay { get; set; }
    }

    public class HolidayDetailsVm
    {
        public int Id { get; set; }
        public int HolidayId { get; set; }
        public int NumberOfDays { get; set; }
        public string FiscalYear { get; set; }
        public int FiscalYearId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }

    public class HolidayGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string Description { get; set; }
        public HolidayType HolidayType { get; set; }
        public bool IsOccuredInSameDate { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public string ApplicableGenderName { get; set; }
        public ApplicableReligion ApplicableReligion { get; set; }
        public string ApplicableReligionName { get; set; }
        public int? BranchId { get; set; }
        public string Date { get; set; }
    }

    public class ManualPunchGridVm
    {
        public int Id { get; set; }
        public int SerialNo { get; set; }
        public string Remark { get; set; }
        public string DateTime { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? ReportingManagerId { get; set; }
    }

    public class OfficeVisitGridVm
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int? BranchId { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApprove { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Status { get; set; }
    }

    public class DashboardVm
    {
        public int DeviceCount { get; set; }
        public int EmployeeCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int DepartmentCount { get; set; }
        public int LateInCount { get; set; }
        public int OnLeaveCount { get; set; }
        public int OffiveVisitCount { get; set; }
        public List<DashboarCountGridVm> AbsentList { get; set; }
        public List<DashboarCountGridVm> OnLeaveList { get; set; }
        public List<DashboarCountGridVm> OfficeVisitList { get; set; }
        public List<DashboarCountGridVm> EmployeeList { get; set; }

    }
    public class DashboarCountGridVm
    {
        public string Employee { get; set; }
        public string Department { get; set; }
    }

    public class FiscalYearGrdiVm
    {
        public int Id { get; set; }
        public string FiscalYear { get; set; }
        public bool CurrentFiscalYear { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class BranchGridVm
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string Address { get; set; }
        public string AddressNp { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public bool IsHeadOffice { get; set; }
        public string CompanyName { get; set; }
    }

    public class EmployeeGridVm
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string ShiftType { get; set; }
        public string LoginStatus { get; set; }
        public int? UserId { get; set; }
    }

    public class UserGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordChar { get; set; }
        public int? RoleId { get; set; }
        public UserType UserType { get; set; }
        public int? BranchId { get; set; }
        public string FullName { get; set; }
        public string PhotoURL { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsDeleted { get; set; }
    }

    


}
