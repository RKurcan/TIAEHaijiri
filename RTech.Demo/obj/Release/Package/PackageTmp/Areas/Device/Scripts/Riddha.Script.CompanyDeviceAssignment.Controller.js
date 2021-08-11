/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.CompanyDeviceAssignment.Model.js" />
function CompanyDeviceAssignmentController() {
    var self = this;
    self.CompanyDeviceAssignment = ko.observable(new CompanyDeviceAssignmentModel());
    self.CompanyDeviceAssignmentGridLst = ko.observableArray([]);
    self.CompanyDeviceAssignments = ko.observableArray([]);
    self.SelectedCompanyDeviceAssignment = ko.observable();
    self.DeviceStatusLst = ko.observableArray([
        { Id: 0, Name: 'New' },
        { Id: 1, Name: 'Reseller' },
        { Id: 2, Name: 'Customer' },
        { Id: 3, Name: 'Damage' },
    ]);
    self.ModeOfButton = ko.observable('Create');
    self.Companies = ko.observableArray([]);
    self.Models = ko.observableArray([]);
    self.Devices = ko.observableArray([]);
    self.AllDevices = ko.observableArray([]);
    self.ModelId = ko.observable(0);
    var url = "/Api/CompanyDeviceAssignmentApi";
    getModels();
    getDevices();
    getCompanies();


    function getCompanyDeviceAssignments() {
        self.CompanyDeviceAssignmentGridLst([]);
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DeviceAssignmentViewModel);
                self.CompanyDeviceAssignmentGridLst(data);
            });
    };

    function getCompanies() {
        Riddha.ajax.get("/Api/CompanyApi", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), CompanyModel);
                self.Companies(data);
            });
    };

    self.GetCompaniesName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Companies()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    function getModels() {
        Riddha.ajax.get("/Api/ModelApi", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ModelModel);
                self.Models(data);
            });
    };
    self.filterText = ko.observable("");

    self.GetModelsName = function (id) {
        var device = Riddha.ko.global.find(self.AllDevices, id);
        return Riddha.ko.global.find(self.Models, device.ModelId).Name;
    };

    self.GetStatusName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.DeviceStatusLst()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    function getDevices() {
        Riddha.ajax.get(url + "/GetDevices", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DeviceModel);
                self.AllDevices(data);
            });
    };

    self.GetDevicesName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.AllDevices()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { SerialNumber: '' }).SerialNumber;
    };

    self.CompanyDeviceAssignmentDetailModeOfButton = ko.observable('Add');

    self.EditDeleteCompanyDeviceAssignment = function (device) {

        for (var i = 1; i <= self.CompanyDeviceAssignments().length; i++) {
            if (self.CompanyDeviceAssignment().DeviceId() == self.CompanyDeviceAssignments()[i - 1].DeviceId()) {
                Riddha.UI.Toast("This Serial Number is already existed!!!", Riddha.CRM.Global.ResultStatus.processError);
                return self.CompanyDeviceAssignment().DeviceId("");
            }
        }
        if (self.CompanyDeviceAssignment().CompanyId() == undefined) {
            return Riddha.UI.Toast("Select Company to assign device", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.ModelId() == undefined) {
            return Riddha.UI.Toast("Select Model to assign device", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (device.DeviceId() == undefined) {
            return Riddha.UI.Toast("Select Serial Number to assign", Riddha.CRM.Global.ResultStatus.processError);
        }

        if (self.CompanyDeviceAssignmentDetailModeOfButton() == 'Add') {
            self.CompanyDeviceAssignments.push(new CompanyDeviceAssignmentModel(ko.toJS(self.CompanyDeviceAssignment())));
            self.CompanyDeviceAssignment(new CompanyDeviceAssignmentModel({
                CompanyId: self.CompanyDeviceAssignment().CompanyId()
            }));
        }
        else if (self.CompanyDeviceAssignmentDetailModeOfButton() == 'Update') {
            self.CompanyDeviceAssignments.replace(new CompanyDeviceAssignmentModel(ko.toJS(self.CompanyDeviceAssignment())));
            self.CompanyDeviceAssignment(new CompanyDeviceAssignmentModel({
                CompanyId: self.CompanyDeviceAssignment().CompanyId()
            }));
        }

    };

    self.SelectCompanyDeviceAssignment = function (model) {
        self.CompanyDeviceAssignment(model);
        self.CompanyDeviceAssignmentDetailModeOfButton('Update');
    };

    self.DeleteCompanyDeviceAssignment = function (model) {
        self.CompanyDeviceAssignments.remove(model);
    };

    self.CreateUpdate = function () {
        if (self.CompanyDeviceAssignments().length <= 0) {
            return Riddha.UI.Toast("Add Device to assign", Riddha.CRM.Global.ResultStatus.processError);
        }
        var data = { CompanyDeviceMgmtLst: self.CompanyDeviceAssignments() };
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        refreshGrid();
                        self.Reset();
                        self.AllDevices([]);
                        getDevices();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            //ajax Call
        };
    };
    function refreshGrid() {
        self.gridOptions.refresh();
    }
    self.Reset = function () {
        self.CompanyDeviceAssignment(new CompanyDeviceAssignmentModel({ Id: self.CompanyDeviceAssignment().Id() }));
        self.CompanyDeviceAssignments([]);
        self.ModeOfButton('Create');
    };
    self.ChangeStatus = ko.observable();

    self.Return = function (item) {
        if (self.SelectedCompanyDeviceAssignment() == undefined) {
            return Riddha.UI.Toast("Please Select Row to return", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.SelectedCompanyDeviceAssignment().Status == "Damage") {
            return Riddha.UI.Toast("This device is already returned", Riddha.CRM.Global.ResultStatus.processError);
        }
        Riddha.ajax.get(url + "/Return" + "/?id=" + self.SelectedCompanyDeviceAssignment().DeviceId)
            .done(function (result) {
                if (result.Status == 4) {
                    self.ChangeStatus(self.SelectedCompanyDeviceAssignment());
                    self.ChangeStatus().Status = "Damage";
                    self.CompanyDeviceAssignmentGridLst.replace(self.SelectedCompanyDeviceAssignment(), self.ChangeStatus());
                    return Riddha.UI.Toast(result.Message, result.Status);
                }
            })
    };

    self.ReturnNew = function (item) {
        if (self.SelectedCompanyDeviceAssignment() == undefined) {
            return Riddha.UI.Toast("Please Select Row to return", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.SelectedCompanyDeviceAssignment().Status == "Damage") {
            return Riddha.UI.Toast("This device is already returned", Riddha.CRM.Global.ResultStatus.processError);
        }
        Riddha.ajax.get(url + "/ReturnNew" + "/?id=" + self.SelectedCompanyDeviceAssignment().DeviceId)
            .done(function (result) {
                if (result.Status == 4) {
                    getDevices();
                    refreshGrid();
                    return Riddha.UI.Toast(result.Message, result.Status);
                }
            })
    };

    self.ShowModal = function () {
        $("#deviceAssignCreationModel").modal('show');
    };

    $("#deviceAssignCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#deviceAssignCreationModel").modal('hide');
    };
    self.gridOptions = riddhaKoGrid(
        {
            data: self.CompanyDeviceAssignmentGridLst,
            columnDefs: [{ field: 'Id', visible: false },
            { field: 'Company', displayName: 'Company' },
            { field: 'AssignOn', displayName: 'AssignOn', cellFilter: function (data) { return moment(data).format('YYYY/MM/DD') } },
            { field: 'Model', displayName: 'Model' },
            { field: 'DeviceSerialNo', displayName: 'DeviceSerialNo' },
            { field: 'Status', displayName: 'Status' }],
            filterText: self.filterText,
            pageSize: 10,
            enableServerPaging: true,
            jsonUrl: url,
            getSelectedItem: function (data) {
                self.SelectedCompanyDeviceAssignment(data);
            },
            getSelectedItems: function (data) {
                //self.selectedManagementCommittees(data);
            }
        });


    //self.CheckedAllPrograms = ko.observable(false);
    //self.CheckedAllPrograms.subscribe(function (newValue) {
    //    self.SelectedPrograms([]);
    //    ko.utils.arrayForEach(self.Programs(), function (item) {
    //        if (newValue) {
    //            self.SelectedPrograms.push(item.Id());
    //        }
    //        else {
    //            self.SelectedPrograms([]);
    //        }
    //    });
    //})



}