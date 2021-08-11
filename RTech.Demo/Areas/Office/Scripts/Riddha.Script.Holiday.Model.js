function HolidayModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '').extend({ required: "Holiday Name is Required" });
    self.NameNp = ko.observable(item.NameNp || '');
    self.Description = ko.observable(item.Description || '');
    self.HolidayType = ko.observable(item.HolidayType || 0);
    self.ApplicableGender = ko.observable(item.ApplicableGender || 0);
    self.ApplicableReligion = ko.observable(item.ApplicableReligion || 0);
    self.IsOccuredInSameDate = ko.observable(item.IsOccuredInSameDate || false);
    self.BranchId = ko.observable(item.BranchId || null);
    self.Date = ko.observable(item.Date || '').extend({ date: '' });
}

function HolidayDetailModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BeginDate = ko.observable(item.BeginDate || '').extend({ date: '' });
    self.EndDate = ko.observable(item.EndDate || '').extend({ date: '' });
    self.NumberOfDays = ko.observable(item.NumberOfDays || 0);
    self.HolidayId = ko.observable(item.HolidayId || 0);
    self.FiscalYearId = ko.observable(item.FiscalYearId || 0);
}
function HolidayGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.HolidayId = ko.observable(item.HolidayId || 0);
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.NumberOfDays = ko.observable(item.NumberOfDays || 0);
    self.BeginDate = ko.observable(item.BeginDate || '').extend({ date: '' });
    self.EndDate = ko.observable(item.EndDate || '').extend({ date: '' });
    self.ApplicableGender = ko.observable(item.ApplicableGender || '');
    self.ApplicableReligion = ko.observable(item.ApplicableReligion || '');
    self.HolidayType = ko.observable(item.HolidayType || '');
    self.Year= ko.observable(item.Year|| '');
    self.FiscalYearId = ko.observable(item.FiscalYearId || 0);
}

function DropDownModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
}