function ResellerModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '').extend({ required: 'Reseller Name is Required' });
    self.Address = ko.observable(item.Address || '').extend({ required: 'Reseller Address is Required' });
    self.ContactNo = ko.observable(item.ContactNo || '').extend({ required: 'Reseller Contact Number is Required' });
    self.ContactPerson = ko.observable(item.ContactPerson || '').extend({ required: 'Contact Person is Required' });
    self.Email = ko.observable(item.Email || '');
    self.WebUrl = ko.observable(item.WebUrl || '');
    self.PAN = ko.observable(item.PAN || '');
    self.LogoUrl = ko.observable(item.LogoUrl || '');
    self.CompanyRegistrationNo = ko.observable(item.CompanyRegistrationNo || '');
    self.CRDUrl = ko.observable(item.CRDUrl || '');
    self.PANVATUrl = ko.observable(item.PANVATUrl || '');
}
function ResellerGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Address = ko.observable(item.Address || '');
    self.ContactNo = ko.observable(item.ContactNo || '');
    self.Email = ko.observable(item.Email || '');
    self.PAN = ko.observable(item.PAN || '');
    self.WebUrl = ko.observable(item.WebUrl || '');
    self.LogoUrl = ko.observable(item.LogoUrl || '');
    self.Status = ko.observable(item.Status || '');
    self.ContactPerson = ko.observable(item.ContactPerson || '');
}

function DemoRequestModel(item) {
    var self = this;
    var item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.Address = ko.observable(item.Address || '');
    self.ContactNo = ko.observable(item.ContactNo || '');
    self.ContactPerson = ko.observable(item.ContactPerson || '');
    self.Email = ko.observable(item.Email || '');
    self.WebUrl = ko.observable(item.WebUrl || '');
}

function UserPasswordReset(item)
{
    var self = this;
    var item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CompanyCode = ko.observable(item.Companycode || '');
    self.Username = ko.observable(item.Name || '');
    self.Email = ko.observable(item.Email || '');
}