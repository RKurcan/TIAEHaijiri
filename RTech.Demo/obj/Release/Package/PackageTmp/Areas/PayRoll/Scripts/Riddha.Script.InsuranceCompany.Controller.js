/// <reference path="../../Scripts/bootstrap-dialog.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="riddha.script.insurancecompany.model.js" />


function insuranceCompanyController() {
    var self = this;
    var lang = Riddha.config().CurrentLanguage;
    self.Insurance = ko.observable(new InsuranceCompanyModel());
    self.Insurances = ko.observableArray([]);
    self.SelectedInsurance = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    var url = "/Api/InsuranceCompanyApi";

    self.CreateUpdate = function () {
        if (self.Insurance().Code() == "") {
            return Riddha.UI.Toast("Code is required..", 0);

        }
        if (self.Insurance().Name() == "") {
            return Riddha.UI.Toast("Name is required..", 0);
        }
        if (self.Insurance().Address() == "") {
            return Riddha.UI.Toast("Address is required..", 0);
        }
        if (self.Insurance().ContactNo() == "") {
            return Riddha.UI.Toast("ContactNo is required..", 0);
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Insurance()))
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                    self.Reset();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Insurance()))
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                    self.Reset();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };

    self.Reset = function () {
        self.Insurance(new InsuranceCompanyModel({ Id: self.Insurance().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        if (self.SelectedInsurance() == undefined || self.SelectedInsurance().length > 1 || self.SelectedInsurance().Id() == 0) {
            Riddha.UI.Toast("Please select row to edit..", 0);
            return;
        }
        self.Insurance(new InsuranceCompanyModel(ko.toJS(self.SelectedInsurance())));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (insurance) {
        if (self.SelectedInsurance() == undefined || self.SelectedInsurance().length > 1 || self.SelectedInsurance().Id() == 0) {
            Riddha.UI.Toast("Please select row to delete..", 0);
            return;
        }
        Riddha.UI.Confirm("Delete Confirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedInsurance().Id(), null)
            .done(function (result) {
                if (result.Status == 4) {
                    self.Reset();
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }, '', "Delete " + self.SelectedInsurance().Name())
    };

    self.ShowModal = function () {
        $("#insuranceCreationModal").modal('show');
    };

    $("#insuranceCreationModal").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#insuranceCreationModal").modal('hide');
        self.Reset();
    };

    self.KendoGridOptions = {
        title: "Insurance",
        target: "#insuranceKendoGrid",
        url: "/Api/InsuranceCompanyApi/GetInsuranceKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        multiselect: false,
        selectable: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: lang == "ne" ? "कोड" : "Code", width: 170 },
            { field: 'Name', title: lang == "ne" ? "नाम" : "Name", width: 270 },
            { field: 'Address', title: lang == "ne" ? "ठेगाना" : "Address", width: 270 },
            { field: 'ContactNo', title: lang == "ne" ? "सम्पर्क" : "ContactNo", width: 270},
        ],
        SelectedItem: function (item) {
            self.SelectedInsurance(new InsuranceCompanyModel(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedInsurance(new InsuranceCompanyModel());
        $("#insuranceKendoGrid").getKendoGrid().dataSource.read();
        $("#insuranceKendoGrid").getKendoGrid().dataSource.filter({});
    };
}
