function DesignationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '').extend({ required: 'Designation Code is Required' });
    self.Name = ko.observable(item.Name || '').extend({ required: 'Designation Name is Required' });
    self.NameNp = ko.observable(item.NameNp || '');
    self.DesignationLevel = ko.observable(item.DesignationLevel || 0);
    self.BranchId = ko.observable(item.BranchId || null);
    self.MaxSalary = ko.observable(item.MaxSalary || 0);
    self.MinSalary = ko.observable(item.MinSalary || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
}

function DesignationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.DesignationLevel = ko.observable(item.DesignationLevel || 0);
    self.BranchId = ko.observable(item.BranchId || null);
    self.MaxSalary = ko.observable(item.MaxSalary || 0);
    self.MinSalary = ko.observable(item.MinSalary || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
}

function DesignationWiseLeavedBalanceModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.LeaveId = ko.observable(item.LeaveId || 0);
    self.DesignationId = ko.observable(item.DesignationId || 0);
    self.Balance = ko.observable(item.Balance || 0);
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.MaxLimit = ko.observable(item.MaxLimit || 0);
    self.IsPaidLeave = ko.observable(item.IsPaidLeave || false);
    self.IsLeaveCarryable = ko.observable(item.IsLeaveCarryable || false);
    self.ApplicableGender = ko.observable(item.ApplicableGender || 0);
    self.IsMapped = ko.observable(item.IsMapped || false);
    self.IsReplacementLeave = ko.observable(item.IsReplacementLeave || false);
    self.LeaveIncreamentPeriod = ko.observable(item.LeaveIncreamentPeriod || 0);

}