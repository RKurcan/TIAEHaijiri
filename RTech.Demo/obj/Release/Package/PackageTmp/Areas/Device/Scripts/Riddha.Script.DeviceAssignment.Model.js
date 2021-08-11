function DeviceAssignmentModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ResellerId = ko.observable(item.ResellerId || 0);
    self.IsPrivate = ko.observable(item.IsPrivate || false);
    self.DeviceId = ko.observable(item.DeviceId || 0);
    self.AssignedById = ko.observable(item.AssignedById || 0);
    self.AssignedOn = ko.observable(item.AssignedOn || '').extend({ date: "yyyy/MM/dd" });
}
function DeviceAssignmentTransactionViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ResellerId = ko.observable(item.ResellerId || 0);
    self.IsPrivate = ko.observable(item.IsPrivate || false);
    self.DeviceIds = ko.observableArray(item.DeviceIds || []);
    self.AssignedById = ko.observable(item.AssignedById || 0);
    self.AssignedOn = ko.observable(item.AssignedOn || '').extend({ date: "yyyy/MM/dd" });
}
function DeviceAssignmentViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.DeviceId = ko.observable(item.DeviceId || 0);
    self.Reseller = ko.observable(item.Reseller || '');
    self.AssignOn = ko.observable(item.AssignOn || '').extend({ date: "yyyy/MM/dd" });
    self.Model = ko.observable(item.Model || '');
    self.DeviceSerialNo = ko.observableArray(item.DeviceSerialNo || '');
    self.Company = ko.observable(item.Company || '');
    self.Status = ko.observable(item.Status || 0);
    self.TotalCount = ko.observable(item.TotalCount || 0);
    self.IsPrivate = ko.observable(item.IsPrivate || false);
}