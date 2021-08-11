/// <reference path="riddha.script.officevisitrequest.model.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="../../../webcamjs-master/webcam.js" />
/// <reference path="../../../webcamjs-master/webcam.min.js" />

function officeVisitRequestController() {
    var self = this;
    self.OfficeVisitRequest = ko.observable(new OffiveVisitRequestVm());
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);

    self.SelectedOfficeVisitRequest = ko.observable();
    var url = "/Api/OfficeVisitRequestApi";
    self.ModeOfButton = ko.observable('Create');
    self.RoleId = ko.observable('1');
    self.MapURL = ko.observable('');
    self.AuthorizedGeoLocation = ko.observable(false);

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
        self.OfficeVisitRequest().Longitude(position.coords.longitude);
        self.OfficeVisitRequest().Latitude(position.coords.latitude);
       
        self.OfficeVisitRequest().Altitude(position.coords.altitude == undefined ? "" : position.coords.altitude);
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
        title: "Office Visit Request",
        target: "#officeVisitRequestKendoGrid",
        url: "/Api/OfficeVisitRequestApi/GetKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        selectable: true,
        group: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmployeeCode', title: lang == "ne" ? "Employee Code" : "Code", filterable: true },
            { field: 'EmployeeName', title: lang == "ne" ? "Employee Name" : "Name", filterable: true },
            { field: 'Department', title: lang == "ne" ? "Department" : "Department", filterable: false },
            { field: 'FromDateAndTime', title: lang == "ne" ? "From" : "From", filterable: false },
            { field: 'ToDateAndTime', title: lang == "ne" ? "To" : "To", filterable: false },
            { field: 'Remark', title: lang == "ne" ? "Remark" : "Remark", filterable: false },
            { field: 'IsApprove', title: lang == "ne" ? "Is Approved" : "Is Approved", filterable: false, template: "#=getApproveStatus(IsApprove)#" },
            { field: 'ApprovedOn', title: lang == "ne" ? "Approve Date" : "Approve Date", filterable: false },
            { field: 'ApproveByName', title: lang == "ne" ? "Approve By" : "Approve By", filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedOfficeVisitRequest(new OffiveVisitRequestVm(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        $("#officeVisitRequestKendoGrid").getKendoGrid().dataSource.read();
    };

    self.Reset = function () {
        self.OfficeVisitRequest(new OffiveVisitRequestVm({ Id: self.OfficeVisitRequest().Id() }));
        self.Employee(new EmpSearchViewModel({ Id: self.Employee().Id() }));
    };

    self.View = function (model) {
        if (self.SelectedOfficeVisitRequest() == undefined || self.SelectedOfficeVisitRequest().length > 1 || self.SelectedOfficeVisitRequest().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        Riddha.ajax.get(url + "/Get?id=" + self.SelectedOfficeVisitRequest().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.OfficeVisitRequest(new OffiveVisitRequestVm(ko.toJS(result.Data)));
                    ShowMap();
                    self.ShowModal();
                    return;
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        //self.OfficeVisitRequest(new OffiveVisitRequestVm(ko.toJS(self.SelectedOfficeVisitRequest())));
        //ShowMap();
        //self.ShowModal();
    };

    self.Delete = function (section) {
        if (self.SelectedOfficeVisitRequest() == undefined || self.SelectedOfficeVisitRequest().length > 1 || self.SelectedOfficeVisitRequest().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        if (self.SelectedOfficeVisitRequest().IsApprove()) {
            Riddha.UI.Toast("Approve data cannot be deleted.");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedOfficeVisitRequest().Id(), null)
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
        if (self.SelectedOfficeVisitRequest().IsApprove()) {
            Riddha.UI.Toast("Already Appoved.", 0);
            return;
        };
        Riddha.ajax.get("/Api/OfficeVisitRequestApi/Approve?id=" + self.SelectedOfficeVisitRequest().Id() + "&adminRemark=" + self.OfficeVisitRequest().AdminRemark())
            .done(function (result) {
                if (result.Status == 4) {
                    self.Reset();
                    self.CloseModal();
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.ShowModal = function () {
        $("#officevisitRequestModal").modal('show');
    };

    $("#officevisitRequestModal").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#officevisitRequestModal").modal('hide');
        self.Reset();
    };

    function ShowMap() {
        self.MapURL('https://maps.google.com/maps?q=' + self.SelectedOfficeVisitRequest().Latitude() + ',' + self.SelectedOfficeVisitRequest().Longitude() + '&z=15&output=embed')
    }

    //region Manual Munch Request Creation on web portal
    //Added by Ganesh 2020/12/14
    //get employee for auto complete
    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.Employee(new EmpSearchViewModel(item));
            self.OfficeVisitRequest().EmployeeId(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    }

    self.ShowCreationModal = function () {
        getLocation();
        $("#officeVisitRequestCreatopnModal").modal('show');
    };
    self.CloseCreationModal = function () {
        self.Reset();
        $("#officeVisitRequestCreatopnModal").modal('hide');
    };

    self.CreateUpdate = function () {
        if (self.OfficeVisitRequest().EmployeeId() == 0) {
            Riddha.UI.Toast("Please select employee.", 0);
            return;
        }
        if (self.OfficeVisitRequest().Remark() == "") {
            Riddha.UI.Toast("Remark is Required.", 0);
            return;
        }
        if (self.OfficeVisitRequest().FromDate() == "") {
            Riddha.UI.Toast("From Date is Required.", 0);
            return;
        }
        if (self.OfficeVisitRequest().FromTime() == "") {
            Riddha.UI.Toast("From Time is Required", 0);
            return;
        }

        if (self.OfficeVisitRequest().ToDate() == "") {
            Riddha.UI.Toast("To Date is Required.", 0);
            return;
        }
        if (self.OfficeVisitRequest().ToTime() == "") {
            Riddha.UI.Toast("To Time is Required", 0);
            return;
        }
        if (self.OfficeVisitRequest().Image() == "") {
            Riddha.UI.Toast("Image is Required", 0);
            return;
        }
        if (self.AuthorizedGeoLocation() == false) {
            getLocation();
            Riddha.UI.Toast("Location is Required, Please allow location on browser.", 0);
            return;
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.OfficeVisitRequest()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseCreationModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.OfficeVisitRequest()))
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