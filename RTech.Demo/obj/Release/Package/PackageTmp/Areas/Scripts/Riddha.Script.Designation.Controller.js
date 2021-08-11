/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Designation.Model.js" />

function designationController() {
    var self = this;
    var config = new Riddha.config();
    var url = "/Api/DesignationApi";
    self.Designation = ko.observable(new DesignationModel());
    self.Designations = ko.observableArray([]);
    self.DesigWiseLeaveLst = ko.observableArray([]);
    self.SelectedDesignation = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.LeaveIncreamentPeriods = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "बार्सिक" : "Yearly" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "मासिक" : "Monthly" },
    ]);

    self.KendoGridOptions = {
        title: "Designation",
        target: "#designationKendoGrid",
        url: "/Api/DesignationApi/GetKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        selectable: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: lang == "ne" ? "कोड" : "Code", width: 100, filterable: true },
            { field: 'Name', title: lang == "ne" ? "नाम" : "Name", width: 200, filterable: true },
            { field: 'DesignationLevel', title: lang == "ne" ? "पदको तह" : "Designation Level", width: 200, filterable: false },
            { field: 'MaxSalary', title: lang == "ne" ? "अधिक्तम तलब" : "Max Salary", width: 200, filterable: false },
            { field: 'MinSalary', title: lang == "ne" ? "न्युनतम तलब" : "Min Salary", width: 200, filterable: false },

        ],
        SelectedItem: function (item) {
            self.SelectedDesignation(new DesignationGridVm(item));
        },
        SelectedItems: function (items) {
        }
    };
    self.RefreshKendoGrid = function () {
        self.SelectedDesignation(new DesignationGridVm());
        $("#designationKendoGrid").getKendoGrid().dataSource.read();
    };


    function GetDesigWiseLeaveLst(desigId) {
        Riddha.ajax.get(url + "/GetDesigWiseLeaveQuota?desigId=" + desigId)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DesignationWiseLeavedBalanceModel);
                self.DesigWiseLeaveLst(data);
            });
    };

    self.ApplicableGender = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सबै" : "All" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "पुरुष" : "Male" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "महिला" : "Female" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "अन्य" : "Others" },

    ]);

    self.GetApplicableGenderName = function (id) {
        var mapped = ko.utils.arrayFirst(self.ApplicableGender(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.CreateUpdate = function () {
        if (self.Designation().Code.hasError()) {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Designation().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Designation()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Designation()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Select = function (model) {
        if (self.SelectedDesignation() == undefined || self.SelectedDesignation().length > 1 || self.SelectedDesignation().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        self.Designation(new DesignationModel(ko.toJS(self.SelectedDesignation())));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.LeaveQuataFor = ko.observable('');

    self.LeaveQuata = function (desig) {
        if (self.SelectedDesignation() == undefined || self.SelectedDesignation().length > 1 || self.SelectedDesignation().Id() == 0) {
            Riddha.UI.Toast("Please Select Row.");
            return;
        }
        GetDesigWiseLeaveLst(self.SelectedDesignation().Id());
        self.LeaveQuataFor(config.CurrentLanguage == "ne" && self.SelectedDesignation().NameNp() != '' ? self.SelectedDesignation().NameNp() + "को " : "For " + self.SelectedDesignation().Name());
        self.LeaveQuataModal();
    };

    self.Reset = function () {
        self.Designation(new DesignationModel({ Id: self.Designation().Id() }));
        self.ModeOfButton("Create");
    };

    self.Delete = function (model) {
        if (self.SelectedDesignation() == undefined || self.SelectedDesignation().length > 1 || self.SelectedDesignation().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedDesignation().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Reset();
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#designationCreationModel").modal('show');
    };

    $("#designationCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    $("#LeaveQuataCreationModel").on('hidden.bs.modal', function () {
        self.ResetLeaveQuota();
    });

    self.CloseModal = function () {
        $("#designationCreationModel").modal('hide');
        self.Reset();
    };

    self.CloseLeaveQuotaModel = function () {
        $("#LeaveQuataCreationModel").modal('hide');
        self.ResetLeaveQuota();
    };

    self.LeaveQuataModal = function () {
        $("#LeaveQuataCreationModel").modal('show');
    };

    self.ResetLeaveQuota = function () {
        self.DesigWiseLeaveLst([]);
    }

    self.ApplyLeaveQuota = function () {
        var data = { LeaveQuota: self.DesigWiseLeaveLst() };
        Riddha.ajax.post(url + "/ApplyLeaveQuota", ko.toJS(data))
            .done(function (result) {
                if (result.Status == 4) {
                    self.CloseLeaveQuotaModel();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }
}
