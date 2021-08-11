

function AllowanceHeadModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.BranchId = ko.observable(item.BranchId || 0);
    self.CreationDate = ko.observable(item.CreationDate || '');
}