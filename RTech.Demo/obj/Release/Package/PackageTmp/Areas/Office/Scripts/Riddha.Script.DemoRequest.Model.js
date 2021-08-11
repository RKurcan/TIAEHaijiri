function DemoRequestModel(item) {
    var self = this;
    var item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.Address = ko.observable(item.Address || '');
    self.ContactNo = ko.observable(item.ContactNo || '');
    self.ContactPerson = ko.observable(item.ContactPerson || '');
    self.Email = ko.observable(item.Email || '');
    self.WebUrl = ko.observable(item.WebUrl || '');
}