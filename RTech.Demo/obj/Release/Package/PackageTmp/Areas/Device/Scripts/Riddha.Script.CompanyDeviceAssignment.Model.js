function CompanyDeviceAssignmentModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
    self.DeviceId = ko.observable(item.DeviceId || 0);
    self.AssignedById = ko.observable(item.AssignedById || 0);
    self.AssignedOn = ko.observable(item.AssignedOn || '').extend({ date: "yyyy/MM/dd" });
}