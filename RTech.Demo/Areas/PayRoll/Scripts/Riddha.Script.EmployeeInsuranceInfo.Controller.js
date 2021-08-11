
/// <reference path="../../Scripts/bootstrap-dialog.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="riddha.script.employeeinsuranceinfo.model.js" />

function EmployeeInsuranceInfoController() {
    var self = this;
    var lang = Riddha.config().CurrentLanguage;
    self.EmployeeInsuranceInfo = ko.observable(new EmployeeInsuranceInfoModel());
    self.EmployeeInsuranceInfos = ko.observableArray([]);
    self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo = ko.observable();
    self.EmpInsuranceInfoModeOfButton = ko.observable('Create');
    var url = "/Api/EmployeeInsuranceInformationApi";
    self.Employees = ko.observableArray([]);
    self.InsuranceCompanies = ko.observableArray([]);
    GetEmployees();
    function GetEmployees() {

        Riddha.ajax.get(url + "/GetEmployees").done(function (result) {

            if (result.Status == 4) {

                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Employees(data);
            }
        })
    }

    GetInsuranceCompanies();

    function GetInsuranceCompanies() {

        Riddha.ajax.get(url + "/GetInsuranceCompanies").done(function (result) {

            if (result.Status == 4) {

                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.InsuranceCompanies(data);
            }
        })
    }
    self.EmpInsuranceInfoCreateUpdate = function () {
        if (self.EmployeeInsuranceInfo().EmployeeId() == 0  || self.EmployeeInsuranceInfo().InsuranceCompanyId() == undefined) {
            Riddha.UI.Toast("Please EmpInsuranceInfoSelect employee..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().InsuranceCompanyId() == 0 || self.EmployeeInsuranceInfo().InsuranceCompanyId() == undefined) {
            Riddha.UI.Toast("Please EmpInsuranceInfoSelect Insurance Company..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().PolicyNo() == "") {
            Riddha.UI.Toast("Policy is required..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().PolicyAmount() == 0) {
            Riddha.UI.Toast("Policy Amount is required..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().PremiumAmount() == 0) {
            Riddha.UI.Toast("Premium Amount is required..", 0);
            return;
        }
 
        if (self.EmployeeInsuranceInfo().IssueDate() == "NaN/aN/aN") {
            Riddha.UI.Toast("IssueDate is required..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().ExpiryDate() == "NaN/aN/aN") {
            Riddha.UI.Toast("ExpiryDate is required..", 0);
            return;
        }

        if (self.EmployeeInsuranceInfo().IssueDate() > self.EmployeeInsuranceInfo().ExpiryDate()) {

            Riddha.UI.Toast("IssueDate cannot be greater than ExpiryDate ..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().InsuraneDocument() == "") {
            Riddha.UI.Toast("Insurance Document is required..", 0);
            return;
        }

        if (self.EmpInsuranceInfoModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.EmployeeInsuranceInfo()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.EmpInsuranceInfoReset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.EmpInsuranceInfoModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.EmployeeInsuranceInfo()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.EmpInsuranceInfoReset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.EmpInsuranceInfoReset = function () {
        self.EmployeeInsuranceInfo(new EmployeeInsuranceInfoModel({ Id: self.EmployeeInsuranceInfo().Id() }));
        self.EmpInsuranceInfoModeOfButton("Create");
    };

    self.EmpInsuranceInfoSelect = function (model) {

        if (self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo() == undefined || self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo().length > 1 || self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo().Id() == 0) {
            Riddha.UI.Toast("Please EmpInsuranceInfoSelect row to edit..", 0);
            return;
        }

        Riddha.ajax.get(url + "?Id=" + self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo().Id()).done(function (result) {
            if (result.Status == 4) {

                self.EmployeeInsuranceInfo(new EmployeeInsuranceInfoModel(ko.toJS(result.Data)));
                self.EmpInsuranceInfoModeOfButton('Update');
                self.ShowModal();
            }


        });
    };

    self.EmpInsuranceInfoDelete = function (insurance) {
        if (self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo() == undefined || self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo().length > 1 || self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo().Id() == 0) {
            Riddha.UI.Toast("Please EmpInsuranceInfoSelect row to delete..", 0);
            return;
        }
        Riddha.UI.Confirm("Delete Confirm", function () {
            Riddha.ajax.delete(url + "/" + self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmpInsuranceInfoReset();
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });

    };

    self.ShowModal = function () {
        $("#EmployeeInsuranceInfoCreationModal").modal('show');
    };

    $("#EmployeeInsuranceInfoCreationModal").on('hidden.bs.modal', function () {
        self.EmpInsuranceInfoReset();
    });

    self.CloseModal = function () {
        $("#EmployeeInsuranceInfoCreationModal").modal('hide');
        self.EmpInsuranceInfoReset();
    };

    self.KendoGridOptions = {
        title: "Employee Insurance Info",
        target: "#EmpInsuranceInfoKendoGrid",
        url: "/Api/EmployeeInsuranceInformationApi/GetEmpInsuranceInfoKendoGrid",
        height: 490,
        paramData: {},
        multiEmpInsuranceInfoSelect: true,
        group: true,
        multiEmpInsuranceInfoSelect: false,
        selectable: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmployeeName', title: lang == "ne" ? "Employee Name" : "Employee Name", width: 170 },
            { field: 'InsuranceCompanyName', title: lang == "ne" ? "Insurance Company" : "Insurance Company", width: 270 },
            { field: 'PolicyNo', title: lang == "ne" ? "PolicyNo" : "PolicyNo", width: 170 },
            { field: 'PolicyAmount', title: lang == "ne" ? "Policy Amount" : "PolicyAmount", width: 170, filterable: false },
            { field: 'PremiumAmount', title: lang == "ne" ? "Premium Amount" : "Premium Amount", width: 170, filterable: false },
            { field: 'IssueDate', title: lang == "ne" ? "Issue Date" : "Issue Date", width: 270, filterable: false, template: "#=SuitableDate(IssueDate)#" },
            { field: 'ExpiryDate', title: lang == "ne" ? "Expiry Date" : "Expiry Date", width: 270, filterable: false, template: "#=SuitableDate(ExpiryDate)#" },

        ],
        EmpInsuranceInfoSelectedItem: function (item) {
            self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo(new EmployeeInsuranceInfoModel(item));
        },
        EmpInsuranceInfoSelectedItems: function (items) {
        }
    };

    self.RefreshKendoGrid = function () {
        self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo(new EmployeeInsuranceInfoModel());
        $("#EmpInsuranceInfoKendoGrid").getKendoGrid().dataSource.read();
        $("#EmpInsuranceInfoKendoGrid").getKendoGrid().dataSource.filter({});
    };
}
