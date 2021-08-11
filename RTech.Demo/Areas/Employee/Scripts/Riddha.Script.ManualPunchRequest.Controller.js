/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="riddha.script.manualpunchrequest.model.js" />
/// <reference path="../../../webcamjs-master/webcam.js" />
/// <reference path="../../../webcamjs-master/webcam.min.js" />

function manualPunchRequestController() {
    var self = this;
    self.ManualPunchRequest = ko.observable(new ManualPunchRequestVm());
    self.ModeOfButton = ko.observable('Create');
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);
    self.SelectedManualPunchRequest = ko.observable();
    self.RoleId = ko.observable('1');
    var url = "/Api/ManualPunchRequestApi";
    self.MapURL = ko.observable('');
    self.AuthorizedGeoLocation = ko.observable(false);
    //Region Geo-Location
    //Added by Raz 2020/12/11
    //This function use for get client longitude,latitude from browser using navigator.geolocation
    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(setLocation);

        } else {
            Riddha.UI.Toast("Geolocation is not supported by this browser. you cannot entry manul punch  from this browser", 0);
            return;
        }
    }

    function setLocation(position) {
        if (position.coords.longitude != "") {
            self.AuthorizedGeoLocation(true);
        }
        self.ManualPunchRequest().Longitude(position.coords.longitude);
        self.ManualPunchRequest().Latitude(position.coords.latitude);
        self.ManualPunchRequest().Altitude(position.coords.altitude == undefined ? "" : position.coords.altitude);
        //alert("lat : " + position.coords.latitude + "long : " + position.coords.longitude + "Alt : " + position.coords.Altitude);
    }


    //endregion


    //region webcam
    self.InitWebCam = {};
    self.webCamCallBack = function (initCallBack) {
        self.InitWebCam = initCallBack;
    };

    self.OpenWebCam = function () {
        self.InitWebCam();
    };

    self.OffCam = function () {
        Webcam.reset();
        var element = document.getElementById('my_camera');
        element.style.width = null;
        element.style.height = null;
    };

    //endregion


    self.KendoGridOptions = {
        title: "Manual Punch Request",
        target: "#manualPunchRequestKendoGrid",
        url: "/Api/ManualPunchRequestApi/GetKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmployeeCode', title: lang == "ne" ? "Employee Code" : "Employee Code" },
            { field: 'EmployeeName', title: lang == "ne" ? "Employee Name" : "Employee Name", filterable: false },
            { field: 'Date', title: lang == "ne" ? "Date" : "Date", filterable: false },
            { field: 'IsApproved', title: lang == "ne" ? "Is Approved" : "Is Approved", filterable: false, template: "#=getApproveStatus(IsApproved)#" },
            { field: 'ApproveDate', title: lang == "ne" ? "Approve Date" : "Approve Date", filterable: false },
            { field: 'ApproveByName', title: lang == "ne" ? "Approve By" : "Approve By", filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedManualPunchRequest(new ManualPunchRequestVm(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        $("#manualPunchRequestKendoGrid").getKendoGrid().dataSource.read();
    }

    self.Reset = function () {
        self.ManualPunchRequest(new ManualPunchRequestVm({ Id: self.ManualPunchRequest().Id() }));
    };

    self.View = function (model) {
        if (self.SelectedManualPunchRequest() == undefined || self.SelectedManualPunchRequest().length > 1 || self.SelectedManualPunchRequest().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        Riddha.ajax.get(url + "/Get?id=" + self.SelectedManualPunchRequest().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.ManualPunchRequest(new ManualPunchRequestVm(ko.toJS(result.Data)));
                    ShowMap();
                    self.ShowModal();
                    return;
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.ShowModal = function () {

        $("#manualPunchRequestModal").modal('show');
    };



    $("#manualPunchRequestModal").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#manualPunchRequestModal").modal('hide');
        self.Reset();
    };

    self.Delete = function (section) {
        if (self.SelectedManualPunchRequest() == undefined || self.SelectedManualPunchRequest().length > 1 || self.SelectedManualPunchRequest().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        if (self.SelectedManualPunchRequest().IsApproved()) {
            Riddha.UI.Toast("Approve data cannot be deleted.");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedManualPunchRequest().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Reset();
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.Approve = function (item) {

        if (self.SelectedManualPunchRequest().IsApproved()) {
            Riddha.UI.Toast("Already Appoved.", 0);
            return;
        };
        Riddha.ajax.get("/Api/ManualPunchRequestApi/Approve?id=" + self.ManualPunchRequest().Id() + "&adminRemark=" + self.ManualPunchRequest().AdminRemark())
            .done(function (result) {
                if (result.Status == 4) {
                    self.CloseModal();
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    function ShowMap() {
        self.MapURL('https://maps.google.com/maps?q=' + self.SelectedManualPunchRequest().Latitude() + ',' + self.SelectedManualPunchRequest().Longitude() + '&z=15&output=embed')
    };


    //region Manual Munch Request Creation on web portal
    //Added by Raz 2020/11/30


    //show manual punch creation popup
    self.ShowCreationModal = function () {
        getLocation();
        $("#manualPunchRequestCreatopnModal").modal('show');
    };

    self.CloseCreationModal = function () {
        self.Reset();
        $("#manualPunchRequestCreatopnModal").modal('hide');
    };


    //get employee for auto complete
    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.Employee(new EmpSearchViewModel(item));
            self.ManualPunchRequest().EmployeeId(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    }

    self.CreateUpdate = function () {
        if (self.ManualPunchRequest().EmployeeId() == 0) {
            Riddha.UI.Toast("Please select employee.", 0);
            return;
        }
        if (self.ManualPunchRequest().Remark() == "") {
            Riddha.UI.Toast("Remark is Required.", 0);
            return;
        }
        if (self.ManualPunchRequest().Date() == "") {
            Riddha.UI.Toast("Date is Required.", 0);
            return;
        }
        if (self.ManualPunchRequest().Time() == "") {
            Riddha.UI.Toast("Time is Required", 0);
            return;
        }
        if (self.ManualPunchRequest().Image() == "") {
            Riddha.UI.Toast("Image is Required", 0);
            return;
        }
        if (self.AuthorizedGeoLocation() == false) {
            getLocation();
            Riddha.UI.Toast("Location is Required, Please allow location on browser.", 0);
            return;
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.ManualPunchRequest()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseCreationModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Section()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseCreationModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

}


function getApproveStatus(id) {
    var self = this;
    self.value = ko.observable([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "होइन" : "No" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "हो" : "Yes" },
    ]);
    var mapped = ko.utils.arrayFirst(self.value(), function (data) {
        return data.Id == id;
    });
    return mapped = (mapped || { Name: '' }).Name;
}