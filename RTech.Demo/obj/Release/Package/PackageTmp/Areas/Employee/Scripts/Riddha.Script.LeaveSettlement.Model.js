function LeaveSettlementModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.LeaveMasterId = ko.observable(item.LeaveMasterId || 0);
    self.FiscalYearId = ko.observable(item.FiscalYearId || 0);
    self.Balance = ko.observable(item.Balance || 0);
    self.SettlementType = ko.observable(item.SettlementType || 0);
    self.BranchId = ko.observable(item.BranchId || null);
}
function LeaveSettlementViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || '');
    self.LeaveMasterId = ko.observable(item.LeaveMasterId || 0);
    self.FiscalYearId = ko.observable(item.FiscalYearId || 0);
    self.Paid = ko.observable(item.Paid || 0);
    self.CarrytoNext = ko.observable(item.CarrytoNext || 0);
    self.FiscalYearName = ko.observable(item.FiscalYearName || '');
    self.LeaveMasterName = ko.observable(item.LeaveMasterName || '');
    self.Balance = ko.observable(item.Balance || '');
    self.SettlementType = ko.observable(item.SettlementType || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
}
function EmployeeDropdownModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.DesignationId = ko.observable(item.DesignationId || 0);
}
function LeaveMasterDropdownModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.IsPaidLeave = ko.observable(item.IsPaidLeave || false);
    self.IsLeaveCarryable = ko.observable(item.IsLeaveCarryable || false);
}