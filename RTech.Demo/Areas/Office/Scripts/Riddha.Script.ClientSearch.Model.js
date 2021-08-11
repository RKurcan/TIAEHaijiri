
function CompanyLicenseLogVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.IsseDate = ko.observable(item.IsseDate || '');
    self.LicensePeriod = ko.observable(item.LicensePeriod || 0);
    //self.DueAmount = ko.observable(item.DueAmount || 0);
    self.FileAttachment = ko.observable(item.FileAttachment || '');
    self.IsPaid = ko.observable(item.IsPaid || false);
    self.PaidAmount = ko.observable(item.PaidAmount || 0);
    self.PaymentMethod = ko.observable(item.PaymentMethod || 0);
    self.Rate = ko.observable(item.Rate || 0);
    self.PaymentMethodName = ko.observable(item.PaymentMethodName || '');
    self.Remark = ko.observable(item.Remark || '');

    self.DueAmount = ko.computed(function () {
        var due = Number(self.Rate()) - Number(self.PaidAmount());
        return due.toFixed(2);
    });
}

function CompanyLoginVm(item) {
    var self = this;
    item = item || {};
    self.UserType = ko.observable(item.UserType || '');
    self.FullName = ko.observable(item.FullName || '');
    self.Name = ko.observable(item.Name || '');
    self.Password = ko.observable(item.Password || '');
}

function CompanyResellerInfoVm(item) {
    var self = this;
    item = item || {};
    self.Address = ko.observable(item.Address || '');
    self.ContactNo = ko.observable(item.ContactNo || '');
    self.ContactPerson = ko.observable(item.ContactPerson || '');
    self.Email = ko.observable(item.Email || '');
    self.Name = ko.observable(item.Name || '');
    self.Pan = ko.observable(item.Pan || '');
    self.Web = ko.observable(item.Web || '');
    self.UserName = ko.observable(item.UserName || '');
    self.Password = ko.observable(item.Password || '');
}

function ClientGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.ContactNo = ko.observable(item.ContactNo || '');
    self.ContactPerson = ko.observable(item.ContactPerson || '');
    self.Name = ko.observable(item.Name || '');
    self.SoftwarePackage = ko.observable(item.SoftwarePackage || '');
}