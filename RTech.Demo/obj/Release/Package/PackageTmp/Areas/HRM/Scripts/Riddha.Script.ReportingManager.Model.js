function EmployeeSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.Designation = ko.observable(item.Designation || '');
    self.DesignationId = ko.observable(item.DesignationId || 0);
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
}

function ReportingLineupGridVm(item) {
    var self = this;
    item = item || {};
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeNameNp = ko.observable(item.EmployeeNameNp || '');
    self.IdCardNo = ko.observable(item.IdCardNo || '');
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.Mobile = ko.observable(item.Mobile || '');
    self.Email = ko.observable(item.Email || '');
    self.ReportingManagerId = ko.observable(item.ReportingManagerId || 0);
    self.ReportingManager = ko.observable(item.ReportingManager || '');
}

function ReportingLineupVm(item) {
    var self = this;
    item = item || {};
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Designation = ko.observable(item.Designation || '');
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
    self.ReportingManagerId = ko.observable(item.ReportingManagerId || 0);
    self.ReportingManagerName = ko.observable(item.ReportingManagerName || '');
    self.ReportingManagerDesignation = ko.observable(item.ReportingManagerDesignation || '');
    self.ReportingManagerDepartment = ko.observable(item.ReportingManagerDepartment || '');
    self.ReportingManagerSection = ko.observable(item.ReportingManagerSection || '');
    self.ReportingManagerPhoto = ko.observable(item.ReportingManagerPhoto || '');
}