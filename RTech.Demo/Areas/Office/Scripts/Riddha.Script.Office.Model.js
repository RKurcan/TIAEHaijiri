function DepartmentModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.BranchName = ko.observable(item.BranchName || '');
    self.BranchId = ko.observable(item.BranchId || null);
    self.NumberOfStaff = ko.observable(item.NumberOfStaff || 0);
};

function DepartmentGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.BranchId = ko.observable(item.BranchId || null);
    self.NumberOfStaff = ko.observable(item.NumberOfStaff || 0);
}

function FiscalYearModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.FiscalYear = ko.observable(item.FiscalYear || '').extend({ required: 'Fiscal Year is Required' });
    self.StartDate = ko.observable(item.StartDate || '').extend({ date: '' });
    self.EndDate = ko.observable(item.EndDate || '').extend({ date: '' });
    self.CurrentFiscalYear = ko.observable(item.CurrentFiscalYear || false);
    self.BranchId = ko.observable(item.BranchId || null);
}
function FiscalYearGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.FiscalYear = ko.observable(item.FiscalYear || '');
    self.StartDate = ko.observable(item.StartDate || '').extend({ date: '' });
    self.EndDate = ko.observable(item.EndDate || '').extend({ date: '' });
    self.CurrentFiscalYear = ko.observable(item.CurrentFiscalYear || false);
    self.BranchId = ko.observable(item.BranchId || null);
}

function NoticeModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Title = ko.observable(item.Title || '').extend({ required: 'Title is Required' });
    self.Description = ko.observable(item.Description || '');
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || 0).extend({ date: '' });
    self.PublishedOn = ko.observable(item.PublishedOn || new Date()).extend({ date: '' });
    self.ExpiredOn = ko.observable(item.ExpiredOn || new Date()).extend({ date: '' });
    self.PublishedById = ko.observable(item.PublishedById || 0);
    self.IsUrgent = ko.observable(item.IsUrgent || false);
    self.NoticeLevel = ko.observable(item.NoticeLevel || 0);
}
function NoticeGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Title = ko.observable(item.Title || '').extend({ required: 'Title is Required' });
    self.Description = ko.observable(item.Description || '');
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || 0).extend({ date: '' });
    self.PublishedOn = ko.observable(item.PublishedOn || new Date()).extend({ date: '' });
    self.ExpiredOn = ko.observable(item.ExpiredOn || new Date()).extend({ date: '' });
    self.PublishedById = ko.observable(item.PublishedById || 0);
    self.IsUrgent = ko.observable(item.IsUrgent || false);
    self.NoticeLevel = ko.observable(item.NoticeLevel || 0);
}

function NoticeDetail(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.NoticeId = ko.observable(item.NoticeId || 0);
    self.DepartmentId = ko.observable(item.DepartmentId || 0);

}
