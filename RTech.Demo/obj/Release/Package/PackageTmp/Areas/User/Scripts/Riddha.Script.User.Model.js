function UserModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '').extend({ required: 'User Name is Required' });
    self.Password = ko.observable(item.Password || '').extend({ required: 'User Password is Required' });
    self.FullName = ko.observable(item.FullName || '');
    self.PhotoURL = ko.observable(item.PhotoURL || '');
    self.RoleId = ko.observable(item.RoleId || 0);
    self.BranchId = ko.observable(item.BranchId || null);
    self.EmpId = ko.observable(item.EmpId || 0);
    self.EmpName = ko.observable(item.EmpName || "");
    self.Email = ko.observable(item.Email || '');
}
function UserRoleModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.Name = ko.observable(item.Name || '').extend({ required: 'UserRole Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.Priority = ko.observable(item.Priority || 0);
}

function UserRoleGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.Priority = ko.observable(item.Priority || 0);
}

function DropdownViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
}
function TreeViewModel(item) {
    var self = this;
    item = item || {};
    self.id = ko.observable(item.id || "");
    self.text = ko.observable(item.text || '');
    self.checked = ko.observable(item.checked || false);
    self.hasChildren = ko.observable(item.hasChildren || false);
    self.parentId = ko.observable(item.parentId || '');
}


