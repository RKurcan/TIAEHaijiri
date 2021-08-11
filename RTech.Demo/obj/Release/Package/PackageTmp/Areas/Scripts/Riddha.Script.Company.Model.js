function CompanyModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '').extend({ required: 'Customer Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.Address = ko.observable(item.Address || '').extend({ required: 'Customer Address is Required' });
    self.AddressNp = ko.observable(item.AddressNp || '');
    self.ContactNo = ko.observable(item.ContactNo || '').extend({ required: 'Customer ContactNo is Required' });
    self.ContactPerson = ko.observable(item.ContactPerson || '').extend({ required: 'Contact Person is Required' });
    self.ContactPersonNp = ko.observable(item.ContactPersonNp || '');
    self.Email = ko.observable(item.Email || '');
    self.WebUrl = ko.observable(item.WebUrl || '');
    self.PAN = ko.observable(item.PAN || '');
    self.LogoUrl = ko.observable(item.LogoUrl || '');
    self.Status = ko.observable(item.Status || false);
    self.EnableMobile = ko.observable(item.EnableMobile || false);
    self.SoftwarePackageType = ko.observable(item.SoftwarePackageType || 0);
    self.SoftwareType = ko.observable(item.SoftwareType || 0);
    self.OrganizationType = ko.observable(item.OrganizationType || 0);
    self.Price = ko.observable(item.Price || 0);
    self.AllowDepartmentwiseAttendance = ko.observable(item.AllowDepartmentwiseAttendance || false);
    self.EmploymentStatusWiseLeave = ko.observable(item.EmploymentStatusWiseLeave || false);
    self.AutoLeaveApproved = ko.observable(item.AutoLeaveApproved || false);
}

function CompanyLicenseModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
    self.LicensePeriod = ko.observable(item.LicensePeriod || 1);
    self.IssueDate = ko.observable(item.IssueDate || '');
    self.ExpiryDate = ko.observable(item.ExpiryDate || '');
}
function CompanyLicenseLogModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
    self.LicensePeriod = ko.observable(item.LicensePeriod || '');
    self.IssueDate = ko.observable(item.IssueDate || '').extend({ date: '' });
}
function CompanyLicenseViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
    self.LicenseStatus = ko.observable(item.LicenseStatus || '');
    self.LicensePeriod = ko.observable(item.LicensePeriod || '');
    self.IssueDate = ko.observable(item.IssueDate || '').extend({ date: '' });
    self.ExpiryDate = ko.observable(item.ExpiryDate || '').extend({ date: '' });
}

function CompanyGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.Address = ko.observable(item.Address || '');
    self.AddressNp = ko.observable(item.AddressNp || '');
    self.ContactNo = ko.observable(item.ContactNo || '');
    self.ContactPerson = ko.observable(item.ContactPerson || '');
    self.ContactPersonNp = ko.observable(item.ContactPersonNp || '');
    self.Email = ko.observable(item.Email || '');
    self.WebUrl = ko.observable(item.WebUrl || '');
    self.PAN = ko.observable(item.PAN || '');
    self.LogoUrl = ko.observable(item.LogoUrl || '');
    self.Status = ko.observable(item.Status || false);
    self.SoftwarePackageType = ko.observable(item.SoftwarePackageType || '');
    self.SoftwareType = ko.observable(item.SoftwareType || '');
    self.AutoLeaveApproved = ko.observable(item.AutoLeaveApproved || false);
}

function ShiftModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ShiftCode = ko.observable(item.ShiftCode || '').extend({ required: 'Shift Code is Required' });
    self.ShiftName = ko.observable(item.ShiftName || '').extend({ required: 'Shift Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.ShiftStartTime = ko.observable(item.ShiftStartTime || '').extend({ required: 'Shift Start Time is Required' });
    self.ShiftEndTime = ko.observable(item.ShiftEndTime || '').extend({ required: 'Shift End Time is Required' });
    self.LunchStartTime = ko.observable(item.LunchStartTime || '');
    self.LunchEndTime = ko.observable(item.LunchEndTime || '');
    self.ShiftType = ko.observable(item.ShiftType || 0);
    self.NumberOfStaff = ko.observable(item.NumberOfStaff || 0);
    self.BranchId = ko.observable(item.BranchId || null);
    self.LateGrace = ko.observable(item.LateGrace || '');
    self.EarlyGrace = ko.observable(item.EarlyGrace || '');
    self.ShortDayWorkingEnable = ko.observable(item.ShortDayWorkingEnable || false);
    self.ShiftStartGrace = ko.observable(item.ShiftStartGrace || '');
    self.StartMonth = ko.observable(item.StartMonth || 0);
    self.StartDays = ko.observable(item.StartDays || 0);
    self.ShiftEndGrace = ko.observable(item.ShiftEndGrace || '');
    self.EndMonth = ko.observable(item.EndMonth || 0);
    self.EndDays = ko.observable(item.EndDays || 0);
    self.ShiftHours = ko.observable(item.ShiftHours || '');
    self.HalfDayWorkingHour = ko.observable(item.HalfDayWorkingHour || '');
    self.DeclareAbsentForLateIn = ko.observable(item.DeclareAbsentForLateIn || false);
    self.DeclareAbsentForEarlyOut = ko.observable(item.DeclareAbsentForEarlyOut || false);
}

function ShiftGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ShiftCode = ko.observable(item.ShiftCode || '');
    self.ShiftName = ko.observable(item.ShiftName || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.ShiftStartTime = ko.observable(item.ShiftStartTime || '');
    self.ShiftEndTime = ko.observable(item.ShiftEndTime || '');
    self.LunchStartTime = ko.observable(item.LunchStartTime || '');
    self.LunchEndTime = ko.observable(item.LunchEndTime || '');
    self.ShiftType = ko.observable(item.ShiftType || 0);
    self.NumberOfStaff = ko.observable(item.NumberOfStaff || 0);
    self.BranchId = ko.observable(item.BranchId || null);
    self.LateGrace = ko.observable(item.LateGrace || '');
    self.EarlyGrace = ko.observable(item.EarlyGrace || '');
    self.ShortDayWorkingEnable = ko.observable(item.ShortDayWorkingEnable || false);
    self.ShiftStartGrace = ko.observable(item.ShiftStartGrace || '');
    self.StartMonth = ko.observable(item.StartMonth || 0);
    self.StartDays = ko.observable(item.StartDays || 0);
    self.ShiftEndGrace = ko.observable(item.ShiftEndGrace || '');
    self.EndMonth = ko.observable(item.EndMonth || 0);
    self.EndDays = ko.observable(item.EndDays || 0);
    self.ShiftHours = ko.observable(item.ShiftHours || '');
    self.HalfDayWorkingHour = ko.observable(item.HalfDayWorkingHour || '');
    self.DeclareAbsentForLateIn = ko.observable(item.DeclareAbsentForLateIn || false);
    self.DeclareAbsentForEarlyOut = ko.observable(item.DeclareAbsentForEarlyOut || false);
}



function LeaveMasterModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '').extend({ required: 'Leave Code is Required' });
    self.Name = ko.observable(item.Name || '').extend({ required: 'Leave Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.IsPaidLeave = ko.observable(item.IsPaidLeave || false);
    self.IsLeaveCarryable = ko.observable(item.IsLeaveCarryable || false);
    self.ApplicableGender = ko.observable(item.ApplicableGender || 0);
    self.BranchId = ko.observable(item.BranchId || null);
    self.Balance = ko.observable(item.Balance || 0).extend({ required: 'Leave Balance is Required' });
    self.Description = ko.observable(item.Description || '').extend({ required: 'Leave Description is Required' });
    self.IsReplacementLeave = ko.observable(item.IsReplacementLeave || false);
    self.LeaveIncreamentPeriod = ko.observable(item.LeaveIncreamentPeriod || 0);
    self.MaximumLeaveBalance = ko.observable(item.MaximumLeaveBalance || 0);
}

function LeaveMasterGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.IsPaidLeave = ko.observable(item.IsPaidLeave || false);
    self.IsLeaveCarryable = ko.observable(item.IsLeaveCarryable || false);
    self.ApplicableGender = ko.observable(item.ApplicableGender || 0);
    self.BranchId = ko.observable(item.BranchId || null);
    self.Balance = ko.observable(item.Balance || 0);
    self.Description = ko.observable(item.Description || '');
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function SectionModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '').extend({ required: 'Section Code is Required' });
    self.Name = ko.observable(item.Name || '').extend({ required: 'Section Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.DepartmentId = ko.observable(item.DepartmentId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.BranchName = ko.observable(item.BranchName || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
}
function SectionGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.DepartmentId = ko.observable(item.DepartmentId || 0);
    self.BranchId = ko.observable(item.BranchId || null);
}
