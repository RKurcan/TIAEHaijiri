function EmployeeUserSetupModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.UserName = ko.observable(item.UserName || '').extend({ required: 'User Name is Required' });
    self.Password = ko.observable(item.Password || '').extend({ required: 'User Password is Required' });
    self.RoleId = ko.observable(item.RoleId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.IsActivated = ko.observable(item.IsActivated || false);
}
function EmployeeUserSetupGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || '');
    self.BranchName = ko.observable(item.BranchName || '');
    self.BranchId = ko.observable(item.BranchId || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || '');
    self.UserName = ko.observable(item.UserName || '');
    self.RoleName = ko.observable(item.RoleName || '');
    self.RoleId = ko.observable(item.RoleId || 0);
    self.Password = ko.observable(item.Password || '');
}

function DropdownViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
}