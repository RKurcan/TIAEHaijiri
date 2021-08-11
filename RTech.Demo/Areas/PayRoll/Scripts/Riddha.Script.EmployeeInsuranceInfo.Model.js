

function EmployeeInsuranceInfoModel(item) {

    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.InsuranceCompanyId = ko.observable(item.InsuranceCompanyId || 0);
    self.PolicyNo = ko.observable(item.PolicyNo || "");
    self.PolicyAmount = ko.observable(item.PolicyAmount || 0);
    self.PremiumAmount = ko.observable(item.PremiumAmount || 0);
    self.InsuraneDocument = ko.observable(item.InsuraneDocument || "");
    self.IssueDate = ko.observable(item.IssueDate || "").extend({ date: "yyyy/MM/dd" });
    self.ExpiryDate = ko.observable(item.ExpiryDate || "").extend({ date: "yyyy/MM/dd" });
    self.BranchId = ko.observable(item.BranchId || 0);
}