
function PayrollConfigurationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.PresentInHoliday = ko.observable(item.PresentInHoliday || "0");
    self.PresentInDayOff = ko.observable(item.PresentInDayOff || "0");
    self.InsuranceContributionByEmpyr = ko.observable(item.InsuranceContributionByEmpyr || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
}