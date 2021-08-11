/// <reference path="riddha.script.officevisitrequest.model.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />


function officeVisitRequestController() {
    var self = this;
    self.OfficeVisitRequest = ko.observable(new OffiveVisitRequestVm());
    self.SelectedOfficeVisitRequest = ko.observable();
    var url = "/Api/OfficeVisitRequestApi";
    self.MapURL = ko.observable('');
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
        self.OfficeVisitRequest(new OffiveVisitRequestVm());
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
        Riddha.ajax.get("/Api/OfficeVisitRequestApi/Approve?id=" + self.SelectedOfficeVisitRequest().Id())
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
}