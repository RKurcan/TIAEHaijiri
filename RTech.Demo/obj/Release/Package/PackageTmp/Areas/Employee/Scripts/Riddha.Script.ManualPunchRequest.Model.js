/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
function ManualPunchRequestVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Remark = ko.observable(item.Remark || '');
    self.AdminRemark = ko.observable(item.AdminRemark || '');
    self.Date = ko.observable(item.Date || '');
    self.DateTime = ko.observable(item.DateTime || '');
    self.Time = ko.observable(item.Time || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Designation = ko.observable(item.Designation || '');
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.BranchId = ko.observable(item.BranchId || 0);
    self.SystemDate = ko.observable(item.SystemDate || '');
    self.ApproveBy = ko.observable(item.ApproveBy || 0);
    self.ApproveDate = ko.observable(item.ApproveDate || '');
    self.IsApproved = ko.observable(item.IsApproved || 0);
    self.Image = ko.observable(item.Image || '');
    self.Longitude = ko.observable(item.Longitude || '');
    self.Latitude = ko.observable(item.Latitude || '');
    self.Altitude = ko.observable(item.Altitude || '');
}