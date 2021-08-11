
function GradeGroupModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.BranchId = ko.observable(item.BranchId || null);
    self.Value = ko.observable(item.Value || 0);
};

function GardeGroupGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.BranchId = ko.observable(item.BranchId || null);
    self.Value = ko.observable(item.Value || 0);
}