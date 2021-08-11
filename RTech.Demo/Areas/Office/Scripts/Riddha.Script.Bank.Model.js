function BankModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '').extend({ required: 'Bank Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.AddressNp = ko.observable(item.AddressNp || '');
    self.Address = ko.observable(item.Address || '');
    self.BranchId = ko.observable(item.BranchId || null);
}

function BankGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.AddressNp = ko.observable(item.AddressNp || '');
    self.Address = ko.observable(item.Address || '');
    self.BranchId = ko.observable(item.BranchId || null);
}