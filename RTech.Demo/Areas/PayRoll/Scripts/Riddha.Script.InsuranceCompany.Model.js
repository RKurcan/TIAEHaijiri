
function InsuranceCompanyModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.Address = ko.observable(item.Address || '');
    self.ContactNo = ko.observable(item.ContactNo || '');
    self.BranchId = ko.observable(item.BranchId || 0);
}