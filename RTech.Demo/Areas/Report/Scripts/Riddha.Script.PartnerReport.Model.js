

function CustomerExpiryReportModel(item) {
    var self = this;
    item = item || {};
    self.CompanyAddress = ko.observable(item.CompanyAddress || '');
    self.CompanyCode = ko.observable(item.CompanyCode || '');
    self.CompanyContactNo = ko.observable(item.CompanyContactNo || '');
    self.CompanyContactPerson = ko.observable(item.CompanyContactPerson || '');
    self.CompanyExpiryDate = ko.observable(item.CompanyExpiryDate || '');
    self.CompanyName = ko.observable(item.CompanyName || '');
    self.CompanyId = ko.observable(item.CompanyId || 0);
    self.ResellerAddress = ko.observable(item.ResellerAddress || '');
    self.ResellerContactNo = ko.observable(item.ResellerContactNo || '');
    self.ResellerContactPerson = ko.observable(item.ResellerContactPerson || '');
    self.ResellerId = ko.observable(item.ResellerId || 0);
    self.ResellerName = ko.observable(item.ResellerName || '');
    self.ExpiryDate = ko.observable(item.ExpiryDate || '');
}

function CustomerDesktopreportVm(item) {
    var self = this;
    item = item || {};
    self.CompanyCode = ko.observable(item.CompanyCode || '');
    self.CompanyName = ko.observable(item.CompanyName || '');
    self.CompanyAddress = ko.observable(item.CompanyAddress || '');
    self.CompanyContactNo = ko.observable(item.CompanyContactNo || '');
    self.CompanyContactPerson = ko.observable(item.CompanyContactPerson || '');
    self.IssueDate = ko.observable(item.IssueDate || '');
    self.MAC = ko.observable(item.MAC || '');
    self.Key = ko.observable(item.Key || '');
    self.ResellerName = ko.observable(item.ResellerName || '');
    self.ResellerAddress = ko.observable(item.ResellerAddress || '');
    self.ResellerContactPerson = ko.observable(item.ResellerContactPerson || '');
    self.ResellerContactNo = ko.observable(item.ResellerContactNo || '');
}

function MonthWiseNewCustomerReportModel(item) {
    var self = this;
    item = item || {};
    self.Month = ko.observable(item.Month || '');
    self.MonthNo = ko.observable(item.MonthNo || 0);
    self.Count = ko.observable(item.Count || 0);
    self.Diffrence = ko.observable(item.Diffrence || 0);
}