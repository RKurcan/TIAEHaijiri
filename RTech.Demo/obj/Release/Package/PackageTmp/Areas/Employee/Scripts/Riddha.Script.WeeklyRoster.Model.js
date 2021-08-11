function RosterModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Date = ko.observable(item.Date || '');
    self.RosterCreationDate = ko.observable(item.RosterCreationDate || '');
    self.ShiftId = ko.observable(item.ShiftId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
}
function DateTableModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.NepDate = ko.observable(item.NepDate || '');
    self.EngDate = ko.observable(item.EngDate || '').extend({ date: '' });
    self.DayName = ko.observable(item.DayName || '');
}
function ShiftModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.ShiftName || '');
    self.ShiftCode = ko.observable(item.ShiftCode || '');
}

