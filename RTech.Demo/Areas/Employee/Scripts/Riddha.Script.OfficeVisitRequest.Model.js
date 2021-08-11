function OffiveVisitRequestVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.FromDateAndTime = ko.observable(item.FromDateAndTime || '');
    self.FromDate = ko.observable(item.FromDate || '');
    self.ToDate = ko.observable(item.ToDate || '');
    self.ToDateAndTime = ko.observable(item.ToDateAndTime || '');
    self.ApprovedById = ko.observable(item.ApprovedById || 0);
    self.ApproveByName = ko.observable(item.ApproveByName || '');
    self.IsApprove = ko.observable(item.IsApprove || false);
    self.ApprovedOn = ko.observable(item.ApprovedOn || '');
    self.Department = ko.observable(item.Department || '');
    self.Designation = ko.observable(item.Designation || '');
    self.Section = ko.observable(item.Section || '');
    self.SystemDate = ko.observable(item.SystemDate || '');
    self.FromTime = ko.observable(item.FromTime || '');
    self.ToTime = ko.observable(item.ToTime || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Remark = ko.observable(item.Remark || '');
    self.Altitude = ko.observable(item.Altitude || ''); 
    self.Image = ko.observable(item.Image || '');
 
    //added for creating
    self.Latitude = ko.observable(item.Latitude || '');
    self.Longitude = ko.observable(item.Longitude || '');

    self.AdminRemark = ko.observable(item.AdminRemark || '');
 
}