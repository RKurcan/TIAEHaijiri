using Riddhasoft.HRM.Entities;
using Riddhasoft.OfficeSetup.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RTech.Demo.Models
{
    public class TestModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class DeviceUpdateVm
    {
        public string SN { get; set; }
        public string BranchCode { get; set; }
        public string DeptNo { get; set; }
        public bool IsFace { get; set; }
        public bool IsAccess { get; set; }
    }
    public class DeviceGridVm
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public string Name { get; set; }
        public string DeviceStatusName { get; set; }
        public string DeviceStatus { get; set; }
        public string LastActivity { get; set; }
        public string SN { get; set; }
        public string FirmwareVersion { get; set; }
        public int UserCount { get; set; }
        public int FPCount { get; set; }
        public int FaceCount { get; set; }
        public int TransCount { get; set; }
        public string DevFuns { get; set; }
        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public bool IsFaceDevice { get; set; }
        public string DeviceModel { get; set; }
        public bool IsAccessDevice { get; set; }
    }

    public class ADMSUserInfoVm
    {
        public int Id { get; set; }
        public string UserPin { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Card { get; set; }
        public int Group { get; set; }
        public string TimeZone { get; set; }
        public string DeviceSn { get; set; }
        public string BranchCode { get; set; }
        public int Privilege { get; set; }
        public int Category { get; set; }
        public string PhotoURL { get; set; }
    }

    public class EmploymentStatusWiseLeaveQuotaViewModel
    {
        public int Id { get; set; }
        public int LeaveId { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int EmploymentStatusId { get; set; }
        public decimal Balance { get; set; }
        public decimal MaxLimit { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public bool IsMapped { get; set; }
        public bool IsReplacementLeave { get; set; }
    }
    public class EmploymentStatusWiseLeaveQuotaApplyVm
    {
        public List<EEmploymentStatusWiseLeavedBalance> LeaveQuota { get; set; }
    }
}