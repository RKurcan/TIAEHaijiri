/// <reference path="riddha.script.desktopproductkey.model.js" />


function desktopProductKeyController() {
    var self = this;
    var config = new Riddha.config();
    var url = "/Api/DesktopProductKeyApi";
    self.DesktopProductKey = ko.observable(new DesktopProductKeyModel());
    self.SelectedCompany = ko.observable();
    self.Devices = ko.observableArray([]);
    self.Keys = ko.observableArray([]);
    self.DeviceCount = ko.observableArray([]);
    self.CheckAllDevices = ko.observable(false);
    self.ModeOfButton = ko.observable('Create');
    self.SelectedCompanyName = ko.observable('');

    self.KendoGridOptions = {
        title: "Company",
        target: "#companyKendoGrid",
        url: "/Api/DesktopProductKeyApi/GetKendoGrid",
        height: '500',
        paramData: {},
        multiSelect: false,
        selectable: false,
        group: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: "Code", filterable: true, width: 70 },
            { field: 'Name', title: "Name", filterable: true, width: 150 },
            { field: 'Address', title: "Address", filterable: false, width: 150 },
            { field: 'ContactNo', title: "Contact", filterable: false, width: 100 },
            { field: 'Status', title: "Status", filterable: false, width: 70, template: "#=getCustomerStatusTemp(Status)#" },
            { field: 'DeviceCount', title: "Device", filterable: false, width: 70 },
            { field: 'Key', title: "Generated Key", filterable: false, width: 70, template: "#=getKeyStatusTemp(Key)#" },
            {
                command: [
                    { name: "license", template: '<a class="k-grid-license k-button" ><span class="fa fa-key text-blue"  ></span></a>', click: ShowModal }
                ],
                title: lang == "ne" ? "कार्य" : "Action",
                width: "80px"
            }
        ],
        SelectedItem: function (item) {
            self.SelectedCompany(new DesktopProductKeyCompanyVm(item));
        },

        SelectedItems: function (items) {
        },
    };

    self.RefreshKendoGrid = function () {
        self.SelectedCompany(new DesktopProductKeyCompanyVm());
        $("#companyKendoGrid").getKendoGrid().dataSource.read();
    };

    self.GetCompaniesDevice = function (customerId) {
        Riddha.ajax.get(url + "/GetCustomerDevice/?companyId=" + customerId)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DesktopProductKeyCheckBoxModel);
                self.Devices(data);
            });
    };

    self.GetCompanySavedProductKey = function (customerId) {
        Riddha.ajax.get(url + "/GetProductKeyByCompanyId/?companyId=" + customerId)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DesktopProductKeyModel);
                self.Keys(data);
            });
    };

    self.CheckAllDevices.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Devices(), function (item) {
            item.Checked(newValue);
        });
    });

    self.CreateUpdate = function () {
        if (self.DesktopProductKey().MAC() == '') {
            Riddha.UI.Toast("Please enter mac address to generate key.", 0)
            return;
        };
        var devices = getSelectedDevices();
        if (devices.length == 0) {
            Riddha.UI.Toast("Please check device to generate key.", 0)
            return;
        };
        self.DeviceCount = devices.split(',');
        self.DesktopProductKey().DeviceCount(self.DeviceCount.length);
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.DesktopProductKey()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.DesktopProductKey().Key(result.Data.Key);
                        self.GetCompanySavedProductKey(result.Data.CompanyId);
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }

    };

    function getSelectedDevices() {
        var device = "";
        ko.utils.arrayForEach(self.Devices(), function (data) {
            if (data.Checked() == true) {
                if (device.length != 0)
                    device += "," + data.DeviceId();
                else
                    device = data.DeviceId() + '';
            }
        });
        return device;
    }

    self.SendKeyToMail = function (model) {
        Riddha.ajax.get(url + "/SendKeyToMail/?productKeyId=" + model.Id())
            .done(function (result) {
                Riddha.UI.Toast(result.Message, result.Status);
            })
    };

    function ShowModal(e) {
        var grid = $("#companyKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        self.SelectedCompanyName(item.Name);
        self.DesktopProductKey().CompanyId(item.Id);
        if (item.DeviceCount == 0) {
            Riddha.UI.Toast("Please assign device first.", 0);
            return;
        }
        self.GetCompaniesDevice(item.Id);
        self.GetCompanySavedProductKey(item.Id);
        $("#productKeyCreationModel").modal('show');
    }

    self.Reset = function () {
        self.DesktopProductKey(new DesktopProductKeyModel());
        self.Devices([]);
        self.Keys([]);
        self.CheckAllDevices(false);
        self.SelectedCompanyName('');
        self.ModeOfButton("Create");
    };

    $("#productKeyCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#productKeyCreationModel").modal('hide');
        self.Reset();
    };
}

function getKeyStatusTemp(name) {
    if (name == '') {
        name = 'NO'
        return "<span class='badge bg-orange'>" + name + "</span>";
    }
    else {
        name = 'YES'
        return "<span class='badge bg-green'>" + name + "</span>";
    }
};