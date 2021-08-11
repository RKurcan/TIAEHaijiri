function BranchModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '').extend({ required: 'Branch Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.AddressNp = ko.observable(item.AddressNp || '');
    self.Address = ko.observable(item.Address || '').extend({ required: 'Branch Address is Required' });
    self.ContactNo = ko.observable(item.ContactNo || '').extend({ required: 'Branch ContactNo is Required' });
    self.Email = ko.observable(item.Email || '');
    self.CompanyName = ko.observable(item.CompanyName || '');
}
function DropdownViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
}