function ModulePermissionViewModel(item) {
    var self = this;
    item = item || {};
    self.RoleId = ko.observable(item.RoleId || 0);
    self.RoleName = ko.observable(item.RoleName || '');
    self.Modules = ko.observableArray(Riddha.ko.global.arrayMap(item.Modules, ModuleViewModel), []);
   
}
function ModuleViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.Checked = ko.observable(item.Checked || false);
}
function DropDownViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
}
function ControllerViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.Checked = ko.observable(item.Checked || false);
}
function ControllerPermissionViewModel(item) {
    var self = this;
    item = item || {};
    self.RoleId = ko.observable(item.RoleId || 0);
    self.Controllers = ko.observableArray(Riddha.ko.global.arrayMap(item.Controllers, ControllerViewModel), []);
}

function ActionViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.Checked = ko.observable(item.Checked || false);
}

function ActionPermissionViewModel(item)
{
    var self = this;
    item = item || {};
    self.RoleId = ko.observable(item.RoleId || 0);
    self.Actions = ko.observableArray(Riddha.ko.global.arrayMap(item.Actions, ActionViewModel), []);
}
function OwnerPermissionViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.RoleId = ko.observable(item.RoleId || 0);
    self.RoleName = ko.observable(item.RoleName || '');
    self.Modules = ko.observableArray(Riddha.ko.global.arrayMap(item.Modules, ModuleViewModel), []);
}

