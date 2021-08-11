/// <reference path="Riddha.Script.AllowanceHead.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />


function allowanceHeadController() {
    var self = this;
    var url = "/Api/AllowanceHeadApi";
    self.AllowanceHead = ko.observable(new AllowanceHeadModel());
    self.AllowanceHeads = ko.observableArray([]);
    self.ModeOfButton = ko.observable('Create');
    self.SelectedAllowanceHead = ko.observable();
    var lang = Riddha.config().CurrentLanguage;

    self.KendoGridOptions = {
        title: "Allowance Head",
        target: "#allowanceHeadKendoGrid",
        url: url + "/GetAllowanceHeadKendoGrid",
        height: 500,
        paramData: {},
        multiSelect: false,
        selectable: true,
        group: true,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'Code', title: lang == "ne" ? "कोड" : "Code" },
            { field: 'Name', title: lang == "ne" ? "नाम" : "Name" },
        ],
        SelectedItem: function (item) {
            self.SelectedAllowanceHead(new AllowanceHeadModel(item));
        },
        SelectedItems: function (items) {
        },
    }


    self.CreateUpdate = function () {
        if (self.AllowanceHead().Code() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.AllowanceHead().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, self.AllowanceHead())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else {

            Riddha.ajax.put(url, self.AllowanceHead())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }

    }

    self.Select = function () {
        debugger;
        if (self.SelectedAllowanceHead() == undefined || self.SelectedAllowanceHead().length > 1 || self.SelectedAllowanceHead().Id() == 0) {
            if (lang == "ne") {
                Riddha.UI.Toast("कृपया सम्पादन गर्न पक्ति चयन गर्नुहोस्", 0);
                return;
            }
            else {
                Riddha.UI.Toast("Please select row to edit.", 0);
                return;
            }
        }
        Riddha.ajax.get(url + "/GetAllowanceHeadById?id=" + self.SelectedAllowanceHead().Id(), null)
            .done(function (result) {
                if (result.Status == 4) {
                    self.ModeOfButton('Update');
                    self.AllowanceHead(new AllowanceHeadModel(result.Data));
                    self.ShowModal();
                }
            });

    };

    self.Delete = function (item) {
        if (self.SelectedAllowanceHead() == undefined) {
            return Riddha.UI.Toast("Please select allowance head to delete", 0);
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedAllowanceHead().Id())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                })
        });
    }
    self.ShowModal = function () {
        $("#allowanceHeadCreationModel").modal('show');
    }
    $("#allowanceHeadCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });
    self.CloseModal = function () {
        $("#allowanceHeadCreationModel").modal('hide');
        self.Reset();
    }
    self.Reset = function () {
        self.AllowanceHead(new AllowanceHeadModel());
        self.ModeOfButton('Create');
    }

    self.RefreshKendoGrid = function () {
        $("#allowanceHeadKendoGrid").getKendoGrid().dataSource.read();
        self.Reset();
    };

}