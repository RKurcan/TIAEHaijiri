/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.DeviceAssignment.Model.js" />

//DeviceAssignment 
function deviceAssignmentController() {
    var self = this;
    self.DeviceAssignment = ko.observable(new DeviceAssignmentTransactionViewModel());
    self.DeviceAssignmentGridLst = ko.observableArray([]);
    self.DeviceAssignments = ko.observableArray([]);
    self.SelectedDeviceAssignment = ko.observable();
    self.DeviceStatusLst = ko.observableArray([
        { Id: 0, Name: 'New' },
        { Id: 1, Name: 'Reseller' },
        { Id: 2, Name: 'Customer' },
        { Id: 3, Name: 'Damage' },
    ]);
    self.ModeOfButton = ko.observable('Create');
    self.Resellers = ko.observableArray([]);
    self.Models = ko.observableArray([]);
    self.Devices = ko.observableArray([]);
    self.AllDevices = ko.observableArray([]);
    self.ModelId = ko.observable(0);
    var url = "/Api/DeviceAssignmentApi";
    getModels();
    getDevices();
    getResellers();
   // getDeviceAssignments();
    function getDeviceAssignments() {
        self.gridOptions.refresh();
        //debugger;
        //self.DeviceAssignmentGridLst([]);
        //Riddha.ajax.get(url)
        //.done(function (result) {
        //    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DeviceAssignmentModel);
        //    self.DeviceAssignmentGridLst(ko.toJS( data));
        //});
    };

    function getResellers() {
        Riddha.ajax.get("/Api/ResellerApi", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ResellerModel);
            self.Resellers(data);
        });
    };

    self.GetResellersName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Resellers()), function (data) {
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
        Riddha.ajax.get("/Api/DeviceApi/GetModelForResellerAssign", null)
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

    self.DeviceAssignmentDetailModeOfButton = ko.observable('Add');
    self.EditDeleteDeviceAssignment = function (device) {
        //adding devices to grid 
      
        for (var i = 1; i <= self.DeviceAssignments().length; i++) {
            if (self.DeviceAssignment().DeviceId() == self.DeviceAssignments()[i - 1].DeviceId()) {
                Riddha.UI.Toast("This Serial Number is already existed!!!", Riddha.CRM.Global.ResultStatus.processError);
                return self.DeviceAssignment().DeviceId("");
            }
        }
        if (self.DeviceAssignment().ResellerId() == undefined) {
            return Riddha.UI.Toast("Select Partner to assign device", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.ModelId() == undefined) {
            return Riddha.UI.Toast("Select Model to assign device", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (device.DeviceIds().length == 0) {
            return Riddha.UI.Toast("Select Serial Number to assign", Riddha.CRM.Global.ResultStatus.processError);
        }


        if (self.DeviceAssignmentDetailModeOfButton() == 'Add') {

            self.DeviceAssignments.push(new DeviceAssignmentTransactionViewModel(ko.toJS(self.DeviceAssignment())));
            self.DeviceAssignment(new DeviceAssignmentTransactionViewModel({
                ResellerId: self.DeviceAssignment().ResellerId(),
                IsPrivate: self.DeviceAssignment().IsPrivate()
            }));
        }
        else if (self.DeviceAssignmentDetailModeOfButton() == 'Update') {
            self.DeviceAssignments.replace(new DeviceAssignmentTransactionViewModel(ko.toJS(self.DeviceAssignment())));
            self.DeviceAssignment(new DeviceAssignmentTransactionViewModel({
                ResellerId: self.DeviceAssignment().ResellerId(),
                IsPrivate: self.DeviceAssignment().IsPrivate()
            }));
        }
    };

    self.SelectDeviceAssignment = function (model) {
        self.DeviceAssignment(model);
        self.DeviceAssignmentDetailModeOfButton('Update');
    };

    self.DeleteDeviceAssignment = function (model) {
        self.DeviceAssignments.remove(model);
    };

    self.CreateUpdate = function () {
        if (self.DeviceAssignment().ResellerId() == undefined) {
            return Riddha.UI.Toast("Select Partner to assign device", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.ModelId() == undefined) {
            return Riddha.UI.Toast("Select Model to assign device", Riddha.CRM.Global.ResultStatus.processError);
        }
        var data = self.DeviceAssignment();
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(data))
            .done(function (result) {
                //getDeviceAssignments();
                Riddha.UI.Toast(result.Message, result.Status);
                self.Reset();
                self.CloseModal();
                getDeviceAssignments();
                getDevices();
            });
        }
    };

    self.SelectedSerialNos = ko.observableArray([]);
    self.CheckedAllSerialNos = ko.observable(false);
    self.CheckedAllSerialNos.subscribe(function (newValue) {
        self.SelectedSerialNos([]);
        ko.utils.arrayForEach(self.Devices(), function (item) {
            if (newValue) {
                self.SelectedSerialNos.push(item.Id());
            }
            else {
                self.SelectedSerialNos([]);
            }
        });
    })
    self.Delete=function()
    {
        if (self.SelectedDeviceAssignment() == undefined) {
            return Riddha.UI.Toast("Please Select Row to delete", Riddha.CRM.Global.ResultStatus.processError);
        }
        Riddha.UI.Confirm("Confirm to delete this message?", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedDeviceAssignment().Id, null)
            .done(function (result) {
                self.DeviceAssignments.remove(self.SelectedDeviceAssignment())
                self.ModeOfButton("Create");
                self.Reset();
                Riddha.UI.Toast(result.Message, result.Status);
                getDevices();
                getDeviceAssignments();
                getResellers();
                getModels();
                self.GetStatusName();

            });
        })

    }


    self.Reset = function () {
        self.DeviceAssignment(new DeviceAssignmentTransactionViewModel({ Id: self.DeviceAssignment().Id() }));
        self.DeviceAssignments([]);
        self.Devices([]);
        self.ModelId(undefined);
        self.ModeOfButton('Create');
    };

    self.Return = function (item) {
        if (self.SelectedDeviceAssignment() == undefined) {
            return Riddha.UI.Toast("Please Select Row to return", Riddha.CRM.Global.ResultStatus.processError);
        }
        //var device = Riddha.ko.global.find(self.AllDevices, self.SelectDeviceAssignment().DeviceId);
        //if (device.Status() == 3) {
        //    return Riddha.UI.Toast("Can not return damage device", Riddha.CRM.Global.ResultStatus.processError);
        //}
        Riddha.ajax.get(url + "/Return" + "/?id=" + self.SelectedDeviceAssignment().DeviceId)
        .done(function (result) {
            if (result.Status == 4) {
                return Riddha.UI.Toast(result.Message, result.Status);
                getDeviceAssignments();
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
        $("#deviceAssignCreationModel").modal('hide'); 0
    };


    self.gridOptions = riddhaKoGrid({
        data: self.DeviceAssignmentGridLst,
        columnDefs: [{ field: 'Id', visible: false },
            { field: 'Reseller', displayName: 'Partner' },
            { field: 'AssignOn', displayName: 'AssignOn', cellFilter: function (data) { return moment(data).format('YYYY/MM/DD') } },
            { field: 'Model', displayName: 'Model' },
            { field: 'DeviceSerialNo', displayName: 'DeviceSerialNo' },
            { field: 'IsPrivate', displayName: 'IsPrivate' },
        { field: 'Status', displayName: 'Status' }],
        filterText: self.filterText,
        pageSize: 10,
        enableServerPaging: true,
        jsonUrl: url,
        getSelectedItem: function (data) {
            self.SelectedDeviceAssignment(data);
        },
        getSelectedItems: function (data) {
            //self.selectedManagementCommittees(data);
        }

    });




}