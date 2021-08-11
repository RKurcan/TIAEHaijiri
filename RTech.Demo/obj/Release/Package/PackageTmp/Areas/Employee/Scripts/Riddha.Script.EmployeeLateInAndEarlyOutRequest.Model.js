

function EmployeeLateInAndEarlyOutRequestModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.Remark = ko.observable(item.Remark || '');
    self.ApproveById = ko.observable(item.ApproveById || 0);
    self.ApproveByName = ko.observable(item.ApproveByName || '');
    self.ApproveDate = ko.observable(item.ApproveDate || '');
    self.RequestDate = ko.observable(item.RequestDate || '');
    self.SystemDate = ko.observable(item.SystemDate || '');
    self.IsApproved = ko.observable(item.IsApproved || '');
    self.PunchInTime = ko.observable(item.PunchInTime || '');
    self.PunchOutTime = ko.observable(item.PunchOutTime || '');
    self.PlannedInTime = ko.observable(item.PlannedInTime || '');
    self.PlannedOutTime = ko.observable(item.PlannedOutTime || '');
    self.LateInEarlyOutRequestType = ko.observable(item.LateInEarlyOutRequestType || 0);
    self.LateInEarlyOutRequestTypeName = ko.observable(item.LateInEarlyOutRequestTypeName || '');
    self.WorkTime = ko.observable(item.WorkTime || '');
    self.EmployeePhoto = ko.observable(item.EmployeePhoto || '');
    self.EmployeeDesignation = ko.observable(item.EmployeeDesignation || '');
}