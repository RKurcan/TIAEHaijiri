/// <reference path="../../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Employee.Model.js" />
/// <reference path="../../Scripts/Riddha.Script.Company.Model.js" />

function manualPunchController() {
    var self = this;
    var url = "/Api/ManualPunchApi";
    var lang = new Riddha.config().CurrentLanguage;
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);
    self.ManualPunch = ko.observable(new ManualPunchModel());
    self.ManualPunches = ko.observableArray([]);

    self.SelectedPunch = ko.observable();
    self.ModeOfButton = ko.observable('Create');

    // GetManualPunch();
    function GetManualPunch() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ManualPunchGridVm);
            self.ManualPunches(data);
        });
    };


    self.GetEmployee = function () {
        if (self.Employee().Code() != '' || self.Employee().Name() != '') {
            Riddha.ajax.get("/Api/ManualPunchApi/SearchEmployee/?empCode=" + self.Employee().Code() + "&empName=" + self.Employee().Name(), null)
           .done(function (result) {
               self.Employee(new EmpSearchViewModel(result.Data));
               self.ManualPunch().EmployeeId(result.Data.Id);
           });
        } else
            return Riddha.UI.Toast("Please Enter Employee Code Or Name To Search", 2);
    }

    self.CreateUpdate = function () {
        if (self.ManualPunch().EmployeeId() == 0) {
            return Riddha.util.localize.Required("Employee");
        }
        if (self.ManualPunch().DateTime() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("DateTime");
        }
        if (self.ManualPunch().Time() == 0) {
            return Riddha.util.localize.Required("Time");
        }
        if (self.ManualPunch().Remark() == "") {
            return Riddha.util.localize.Required("Remark");
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.ManualPunch()))
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
            Riddha.ajax.put(url, ko.toJS(self.ManualPunch()))
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                    self.ModeOfButton("Create");
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };

    self.Reset = function () {
        self.ManualPunch(new ManualPunchModel({ Id: self.ManualPunch().Id() }));
        self.Employee(new EmpSearchViewModel());
    };

    self.Select = function (model) {
        if (self.SelectedPunch() == undefined || self.SelectedPunch().length > 1 || self.SelectedPunch().Id() == 0) {
            Riddha.UI.Toast("Please select row to edit..", 0);
            return;
        }
        self.ManualPunch(new ManualPunchModel(ko.toJS(self.SelectedPunch())));
        self.Employee(new EmpSearchViewModel({ Code: self.SelectedPunch().EmployeeCode(), Name: self.SelectedPunch().EmployeeName() }));
        self.GetEmployee();
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (manualPunch) {
        if (self.SelectedPunch() == undefined || self.SelectedPunch().length > 1 || self.SelectedPunch().Id() == 0) {
            Riddha.UI.Toast("Please select row to delete..", 0);
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedPunch().Id(), null)
            .done(function (result) {
                self.ManualPunches.remove(manualPunch)
                self.ModeOfButton("Create");
                self.Reset();
                self.RefreshKendoGrid();
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    self.ShowModal = function () {
        $("#ManualPunchCreationModel").modal('show');
    };

    $("#ManualPunchCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
        self.RefreshKendoGrid();
        self.ModeOfButton("Create");
    });

    self.CloseModal = function () {
        $("#ManualPunchCreationModel").modal('hide');
        self.Reset();
        self.RefreshKendoGrid();
        self.ModeOfButton("Create");
    };
    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.Employee(new EmpSearchViewModel(item));
            self.ManualPunch().EmployeeId(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    }

    //Kendo Grid
    self.KendoGridOptions = {
        title: "ManualPunch",
        target: "#manualPunchKendoGrid",
        url: "/Api/ManualPunchApi/GetManualPunchKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: true,
        //groupParam: { field: "DepartmentName" },

        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmployeeName', title: "Employee", width: 150, hidden: lang == "en" ? false : true },
            { field: 'EmployeeNameNp', title: "कर्मचारी", width: 150, hidden: lang == "ne" ? false : true, template: "#=EmployeeNameNp==null?EmployeeName:EmployeeNameNp#" },
            { field: 'DateTime', title: lang == "ne" ? "मिति र समय" : "Date Time", template: "#=SuitableDate(DateTime)+'-'+Time#", width: 80, filterable: false },
            { field: 'Remark', title: lang == "ne" ? "टिप्पणी" : "Remark", width: 200, filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedPunch(new ManualPunchGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedPunch(new ManualPunchGridVm());
        $("#manualPunchKendoGrid").getKendoGrid().dataSource.read();
    }
}