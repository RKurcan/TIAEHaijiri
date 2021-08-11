/// <reference path="Riddha.Script.Device.Model.js" />
/// <reference path="Riddha.Script.Device.Controller.js" />

function ModelModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '').extend({ required: 'Device Name is Required' });
    self.ImageURL = ko.observable(item.ImageURL || '');
    self.Manufacture = ko.observable(item.Manufacture || 0);
    self.Brand = ko.observable(item.Brand || 0);
    self.IsAccessDevice = ko.observable(item.IsAccessDevice || false);
    self.IsFaceDevice = ko.observable(item.IsFaceDevice || false);
}

function DeviceModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.ModelId = ko.observable(item.ModelId || 0);
    self.ModelName = ko.observable(item.ModelName || '');
    self.SerialNumber = ko.observable(item.SerialNumber || '').extend({ required: 'Device Serial Number is Required' });
    self.Status = ko.observable(item.Status || '');
    self.DeviceType = ko.observable(item.DeviceType || '');
    self.Type = ko.observable(item.Type || '');
    self.IpAddress = ko.observable(item.IpAddress || '');
    self.DevFuns = ko.observable(item.DevFuns || '');
    self.FaceCount = ko.observable(item.FaceCount || 0);
    self.FPCount = ko.observable(item.FPCount || 0);
    self.FwVersion = ko.observable(item.FwVersion || '');
    self.TransCount = ko.observable(item.TransCount || 0);
    self.UserCount = ko.observable(item.UserCount || 0);
    self.LastActivity = ko.observable(item.LastActivity || '');
    self.DeviceImage = ko.observable(item.DeviceImage || '');
    self.IsAccessDevice = ko.observable(item.IsAccessDevice || false);
    self.IsFaceDevice = ko.observable(item.IsFaceDevice || false);
}

function DeviceGridViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ModelId = ko.observable(item.ModelId || 0);
    self.Model = ko.observable(item.Model || '');
    self.SerialNumber = ko.observable(item.SerialNumber || '');
    self.DeviceType = ko.observable(item.DeviceType || 0);
    self.DeviceTypeName = ko.observable(item.DeviceTypeName || '');
    self.Status = ko.observable(item.Status || 0);
    self.StatusName = ko.observable(item.StatusName || '');
    self.TotalCount = ko.observable(item.TotalCount || 0);
}
function CompanyDeviceGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.IpAddress = ko.observable(item.IpAddress || '');
    self.SerialNo = ko.observable(item.SerialNo || '');
    self.Type = ko.observable(item.Type || '');
    self.IsOnline = ko.observable(item.IsOnline || false);
    self.ModelName = ko.observable(item.ModelName || '');
}