function ReplacementLeaveApprovalGridModel(item) {
    var self = this;
    item = item || {};
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.Date = ko.observable(item.Date || '').extend({ date: '' });
    self.PresentOnHoliday = ko.observable(item.PresentOnHoliday || 0);
    self.PresentOnDayOff = ko.observable(item.PresentOnDayOff || 0);
}

