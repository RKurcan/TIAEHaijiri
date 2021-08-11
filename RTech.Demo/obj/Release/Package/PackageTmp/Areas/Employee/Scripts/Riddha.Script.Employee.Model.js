function EmployeeModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '').extend({ required: 'Employee Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.Code = ko.observable(item.Code || '').extend({ required: 'Employee UserID is Required' });
    self.SectionId = ko.observable(item.SectionId || 0);
    self.DesignationId = ko.observable(item.DesignationId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.DateOfJoin = ko.observable(item.DateOfJoin || '').extend({ date: '' });
    self.DeviceCode = ko.observable(item.DeviceCode || 0);
    self.DateOfBirth = ko.observable(item.DateOfBirth || '').extend({ date: '' });
    self.MaritialStatus = ko.observable(item.MaritialStatus || false);
    self.BloodGroup = ko.observable(item.BloodGroup || false);
    self.Gender = ko.observable(item.Gender || false);
    self.Mobile = ko.observable(item.Mobile || '');
    self.Email = ko.observable(item.Email || '');
    self.PermanentAddress = ko.observable(item.PermanentAddress || '');
    self.PermanentAddressNp = ko.observable(item.PermanentAddressNp || '');
    self.TemporaryAddress = ko.observable(item.TemporaryAddress || '');
    self.TemporaryAddressNp = ko.observable(item.TemporaryAddressNp || '');
    self.ImageUrl = ko.observable(item.ImageUrl || '');
    self.ShiftTypeId = ko.observable(item.ShiftTypeId || 0);
    self.ShiftId = ko.observable(item.ShiftId || 0);
    self.WOTypeId = ko.observable(item.WOTypeId || 0);
    self.PrimaryWeeklyOfType = ko.observable(item.PrimaryWeeklyOfType || 0);
    self.Section = ko.observable(item.Section || new SectionModel());
    self.MaxWorkingHour = ko.observable(item.MaxWorkingHour || '00:00');
    self.AllowedLateIn = ko.observable(item.AllowedLateIn || '00:00');
    self.AllowedEarlyOut = ko.observable(item.AllowedEarlyOut || '00:00');
    self.HalfdayWorkingHour = ko.observable(item.HalfdayWorkingHour || '00:00');
    self.ShortDayWorkingHour = ko.observable(item.ShortDayWorkingHour || '00:00');
    self.PresentMarkingDuration = ko.observable(item.PresentMarkingDuration || '00:00');
    self.MaxOTHour = ko.observable(item.MaxOTHour || "00:00");
    self.MinOTHour = ko.observable(item.MinOTHour || "00:00");
    self.IsOTAllowed = ko.observable(item.IsOTAllowed || false);
    self.NoPunch = ko.observable(item.NoPunch || false);
    self.SinglePunch = ko.observable(item.SinglePunch || false);
    self.MultiplePunch = ko.observable(item.MultiplePunch || false);
    self.TwoPunch = ko.observable(item.TwoPunch || false);
    self.FourPunch = ko.observable(item.FourPunch || false);
    self.ConsiderTimeLoss = ko.observable(item.ConsiderTimeLoss || false);
    self.HalfDayMarking = ko.observable(item.HalfDayMarking || false);
    self.WeeklyOffIds = ko.observableArray(item.WeeklyOffIds || []);
    self.PassportNo = ko.observable(item.PassportNo || '');
    self.CitizenNo = ko.observable(item.CitizenNo || '');
    self.IssueDate = ko.observable(item.IssueDate || '').extend({ date: '' });
    self.IssueDistict = ko.observable(item.IssueDistict || '');
    self.Religion = ko.observable(item.Religion || false);
    self.GradeGroupId = ko.observable(item.GradeGroupId || 0);
    self.RoleId = ko.observable(item.RoleId || 0);
    self.UserName = ko.observable(item.UserName || '');
    self.Password = ko.observable(item.Password || '');
    self.IsManager = ko.observable(item.IsManager || false);
    self.EmploymentStatus = ko.observable(item.EmploymentStatus || 0);
    self.ReportingManagerId = ko.observable(item.ReportingManagerId || 0);
    self.PANNo = ko.observable(item.PANNo || '');
    self.SSNNo = ko.observable(item.SSNNo || '');
    self.EnableSSN = ko.observable(item.EnableSSN || false);
    self.BankId = ko.observable(item.BankId || 0);
    self.BankAccountNo = ko.observable(item.BankAccountNo || '');
}
function OTManagementModel(item) {
    var self = this;
    item = item || {};
    self.IsOTAllowed = ko.observable(item.IsOTAllowed || false);
    self.MaxOTHour = ko.observable(item.MaxOTHour || "00:00");
    self.MinOTHour = ko.observable(item.MinOTHour || "00:00");
}
function EmployeeGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.IdCardNo = ko.observable(item.IdCardNo || '');
    self.DeviceCode = ko.observable(item.DeviceCode || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.DateOfJoin = ko.observable(item.DateOfJoin || '').extend({ date: '' });
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.Mobile = ko.observable(item.Mobile || '');
    self.Email = ko.observable(item.Email || '');
    self.PhotoURL = ko.observable(item.PhotoURL || '');
    self.DepartmentId = ko.observable(item.DepartmentId || 0);
    self.SectionId = ko.observable(item.SectionId || 0);
    self.DesignationName = ko.observable(item.DesignationName || '');
    self.GradeGroupId = ko.observable(item.GradeGroupId || '');
    self.GradeGroupName = ko.observable(item.GradeGroupName);
    self.IsActivated = ko.observable(item.IsActivated || false);
    self.UserId = ko.observable(item.UserId || 0);
    self.Status = ko.observable(item.Status || 0);
}

function EmployeeInfoModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Photo = ko.observable(item.Photo || '');
    self.IdCardNo = ko.observable(item.IdCardNo || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeNameNp = ko.observable(item.EmployeeNameNp || '');
    self.BranchName = ko.observable(item.BranchName || '');
    self.BranchNameNp = ko.observable(item.BranchNameNp || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.DepartmentNameNp = ko.observable(item.DepartmentNameNp || '');
    self.SectionName = ko.observable(item.SectionName || '');
    self.SectionNameNp = ko.observable(item.SectionNameNp || '');
    self.DesignationName = ko.observable(item.DesignationName || '');
    self.DesignationNameNp = ko.observable(item.DesignationNameNp || '');
    self.DateOfBirth = ko.observable(item.DateOfBirth || '').extend({ date: '' });
    self.DateOfJoin = ko.observable(item.DateOfJoin || '').extend({ date: '' });
    self.Mobile = ko.observable(item.Mobile || '');
    self.Address = ko.observable(item.Address || '');
    self.BloodGroup = ko.observable(item.BloodGroup || 0);
    self.PunchType = ko.observable(item.PunchType || '');
    self.MaxWorkingHours = ko.observable(item.MaxWorkingHours || '');
    self.ShiftType = ko.observable(item.ShiftType || '');
    self.ShiftName = ko.observable(item.ShiftName || '');
    self.ShiftNameNp = ko.observable(item.ShiftNameNp || '');
}

function DropdownViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
}
function ShiftModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ShiftName = ko.observable(item.ShiftName || '');
    self.NameNp = ko.observable(item.NameNp || '');
}
function SectionModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.DepartmentId = ko.observable(item.DepartmentId || 0);
    self.BranchId = ko.observable(item.BranchId || null);
}
function EmpSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Designation = ko.observable(item.Designation || '');
    self.DesignationId = ko.observable(item.DesignationId || 0);
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
}

function LeaveMasterViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.OpeningBal = ko.observable(item.OpeningBal || 0);
}

function LeaveApplicationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.LeaveMasterId = ko.observable(item.LeaveMasterId || 0);
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.ApprovedById = ko.observable(item.ApprovedById || 0);
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.IsApproved = ko.observable(item.IsApproved || false);
    self.ApprovedOn = ko.observable(item.ApprovedOn || '');
    self.LeaveDay = ko.observable(item.LeaveDay || 0);
    self.Description = ko.observable(item.Description || '').extend({ required: 'Description is Required' });
}

function LeaveApplicationViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.LeaveMasterId = ko.observable(item.LeaveMasterId || 0);
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.LeaveCount = ko.observable(item.LeaveCount || 0);
    self.Days = ko.observable(item.Days || 0);
    self.LeaveDay = ko.observable(item.LeaveDay || 0);
    self.Description = ko.observable(item.Description || '').extend({ required: 'Description is Required' });
    self.ApprovedById = ko.observable(item.ApprovedById || 0);
    self.LeaveStatus = ko.observable(item.LeaveStatus || 0);
    self.LeaveMaster = ko.observable(item.LeaveMaster || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
}

function leaveApplicationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.LeaveMaster = ko.observable(item.LeaveMaster || '');
    self.LeaveMasterId = ko.observable(item.LeaveMasterId || 0);
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.Days = ko.observable(item.Days || '');
    self.LeaveDay = ko.observable(item.LeaveDay || 0);
    self.Description = ko.observable(item.Description || '');
    self.ApprovedById = ko.observable(item.ApprovedById || 0);
    self.LeaveStatus = ko.observable(item.LeaveStatus || 0);
    self.LeaveCount = ko.observable(item.LeaveCount || 0);

}

function ManualPunchModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Employee = ko.observable((item.Employee || {}).Name || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.DateTime = ko.observable(item.DateTime || '').extend({ date: '' });
    self.Time = ko.observable(item.Time || '');
    self.CompanyId = ko.observable(item.CompanyId || 0);
    self.Remark = ko.observable(item.Remark || '');
}

function ManualPunchGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeNameNp = ko.observable(item.EmployeeNameNp || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.DateTime = ko.observable(item.DateTime || '').extend({ date: '' });
    self.Time = ko.observable(item.Time || '');
    self.Remark = ko.observable(item.Remark || '');
}



function OfficeVisitModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Remark = ko.observable(item.Remark || '').extend({ required: 'Remark  is Required' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.BranchId = ko.observable(item.BranchId || 0);
    self.ToTime = ko.observable(item.ToTime || 0);
    self.FromTime = ko.observable(item.FromTime || 0);
    self.IsApprove = ko.observable(item.IsApprove || false);
}
function OfficeVisitDetailModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.OfficeVisitId = ko.observable(item.OfficeVisitId || '');
    self.EmployeeId = ko.observable(item.EmployeeId || '');
}
function EmpSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.Designation = ko.observable(item.Designation || '');
    self.DesignationId = ko.observable(item.DesignationId || 0);
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
}
function OfficeVisitGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.FromTime = ko.observable(item.FromTime || '');
    self.ToTime = ko.observable(item.ToTime || '');
    self.Remark = ko.observable(item.Remark || '');
    self.OfficeVisitStatus = ko.observable(item.OfficeVisitStatus || '');
}

function OfficeVisitApprovalGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.FromTime = ko.observable(item.FromTime || '');
    self.ToTime = ko.observable(item.ToTime || '');
    self.Remark = ko.observable(item.Remark || '');
    self.OfficeVisitStatus = ko.observable(item.OfficeVisitStatus || '');
}

function EventModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Title = ko.observable(item.Title || '').extend({ required: 'Title is Required' });
    self.Description = ko.observable(item.Description || '');
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || 0).extend({ date: '' });
    self.EventLevel = ko.observable(item.EventLevel || 0);
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
}

function EventGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Title = ko.observable(item.Title || '');
    self.Description = ko.observable(item.Description || '');
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || 0).extend({ date: '' });
    self.EventLevel = ko.observable(item.EventLevel || 0);
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
}

function EventDetailModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EventId = ko.observable(item.EventId || 0);
    self.TargetId = ko.observable(item.TargetId || 0);
}

function LeaveApprovalViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.Leave = ko.observable(item.Leave || '');
    self.LeaveDay = ko.observable(item.LeaveDay || 0);
    self.LeaveStatus = ko.observable(item.LeaveStatus || 0);
    self.RemLeave = ko.observable(item.RemLeave || 0);
    self.LeaveCount = ko.observable(item.LeaveCount || 0);
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.EmpCode = ko.observable(item.EmpCode || '');
    self.EmpName = ko.observable(item.EmpName || '');
    self.Designation = ko.observable(item.Designation || '');
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
    self.Description = ko.observable(item.Description || '');
    self.AdminRemark = ko.observable(item.AdminRemark || '');
}

function KajModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Remark = ko.observable(item.Remark || '').extend({ required: 'Remark  is Required' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.BranchId = ko.observable(item.BranchId || 0);
    self.ToTime = ko.observable(item.ToTime || 0);
    self.FromTime = ko.observable(item.FromTime || 0);
    self.IsApprove = ko.observable(item.IsApprove || false);
    self.KajStatus = ko.observable(item.KajStatus || 0);
    self.KajStatusName = ko.observable(item.KajStatusName || '');
}

function KajDetailModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.KajId = ko.observable(item.KajId || '');
    self.EmployeeId = ko.observable(item.EmployeeId || '');
}

function KajGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.From = ko.observable(item.From || '').extend({ date: '' });
    self.To = ko.observable(item.To || '').extend({ date: '' });
    self.FromTime = ko.observable(item.FromTime || '');
    self.ToTime = ko.observable(item.ToTime || '');
    self.Remark = ko.observable(item.Remark || '');
    self.KajStatus = ko.observable(item.KajStatus || 0);
    self.KajStatusName = ko.observable(item.KajStatusName || '');
    self.IsApprove = ko.observable(item.IsApprove || false);
}

function EmpLeaveBalRowModel(item) {
    var self = this;
    item = item || {};
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmpLeaveBalColumns = ko.observableArray(Riddha.ko.global.arrayMap(item.EmpLeaveBalColumns, EmpLeaveBalColumnModel) || []);
}
function EmpLeaveBalColumnModel(item) {
    var self = this;
    item = item || {};
    self.LeaveId = ko.observable(item.LeaveId || 0);
    self.LeaveCode = ko.observable(item.LeaveCode || '');
    self.LeaveName = ko.observable(item.LeaveName || '');
    self.OpBal = ko.observable(item.OpBal || 0);
    self.IsMapped = ko.observable(item.IsMapped || false);
}
function EmpLeaveBalParamModel(item) {
    var self = this;
    item = item || {};
    self.EmpIds = ko.observable(item.EmpIds || '');
    self.DeptIds = ko.observable(item.DeptIds || '');
    self.SectionIds = ko.observable(item.SectionIds || '');
}




