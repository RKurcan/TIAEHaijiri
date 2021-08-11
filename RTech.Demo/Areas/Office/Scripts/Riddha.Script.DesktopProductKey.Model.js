
function DesktopProductKeyCheckBoxModel(item) {
    var self = this;
    item = item || {};
    self.DeviceId = ko.observable(item.DeviceId || 0);
    self.DeviceSerialNo = ko.observable(item.DeviceSerialNo || '');
    self.Model = ko.observable(item.Model || '');
    self.Checked = ko.observable(item.Checked || false);
};

function DesktopProductKeyModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
    self.IssueDate = ko.observable(item.IssueDate || '');
    self.SystemDate = ko.observable(item.SystemDate || '');
    self.BackDate = ko.observable(item.BackDate || '');
    self.DeviceCount = ko.observable(item.DeviceCount || 0);
    self.MAC = ko.observable(item.MAC || '');
    self.Key = ko.observable(item.Key || '');
}

function DesktopProductKeyCompanyVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.Address = ko.observable(item.Address || '');
    self.AddressNp = ko.observable(item.AddressNp || '');
    self.ContactNo = ko.observable(item.ContactNo || '');
    self.ContactPerson = ko.observable(item.ContactPerson || '');
    self.ContactPersonNp = ko.observable(item.ContactPersonNp || '');
    self.Email = ko.observable(item.Email || '');
    self.WebUrl = ko.observable(item.WebUrl || '');
    self.PAN = ko.observable(item.PAN || '');
    self.LogoUrl = ko.observable(item.LogoUrl || '');
    self.Status = ko.observable(item.Status || false);
    self.EnableMobile = ko.observable(item.EnableMobile || false);
    self.SoftwarePackageType = ko.observable(item.SoftwarePackageType || 0);
    self.SoftwareType = ko.observable(item.SoftwareType || 0);
    self.Price = ko.observable(item.Price || 0);
    self.AllowDepartmentwiseAttendance = ko.observable(item.AllowDepartmentwiseAttendance || false);
    self.DeviceCount = ko.observable(item.DeviceCount || 0);
    self.Key = ko.observable(item.Key || false);
}