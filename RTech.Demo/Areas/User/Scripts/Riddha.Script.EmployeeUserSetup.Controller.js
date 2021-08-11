/// <reference path="Riddha.Script.EmployeeUserSetup.Model.js" />
/// <reference path="../../../Scripts/knockout-3.4.2.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />

function employeeUserSetupController() {
    var self = this;
    self.ModeOfButton = ko.observable('Create');
    self.EmployeeUserLogin = ko.observable(new EmployeeUserSetupModel());
    self.EmployeeUserLogins = ko.observableArray([]);
    self.SelectedEmployeeUserLogin = ko.observable();
    self.Branches = ko.observableArray([]);
    self.Roles = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    var url = "/Api/EmployeeUserSetupApi";

    getBranches();
    getEmployees();
    getRoles();
    getEmployeesUserSetup();


    //populate Data to grid
    function getEmployeesUserSetup() {
        Riddha.ajax.get(url, null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmployeeUserSetupGridVm);
            self.EmployeeUserLogins(data);
        });
    };

    //Populate Branch in dropdown
    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DropdownViewModel);
            self.Branches(data);
        });
    };

    //Populate Role in dropdown
    function getRoles() {
        Riddha.ajax.get(url + "/GetRolesForDropdown", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
            self.Roles(data);
        });
    };

    //Populate Employee in dropdown
    function getEmployees() {
        Riddha.ajax.get(url + "/GetEmployeesForDropdown", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
            self.Employees(data);
        });
    };


    self.CreateUpdate = function () {

        if (self.EmployeeUserLogin().BranchId() == undefined) {
            return Riddha.UI.Toast("Please select Branch!!", 2);
        }
        if (self.EmployeeUserLogin().EmployeeId() == undefined) {
            return Riddha.UI.Toast("Please select Employee!!", 2);
        }
        if (self.EmployeeUserLogin().RoleId() == undefined) {
            return Riddha.UI.Toast("Please select Role!!", 2);
        }
        if (self.EmployeeUserLogin().UserName() == "") {
            return Riddha.UI.Toast("User Name is Requierd!!", 2);
        }
        if (self.EmployeeUserLogin().Password() == "") {
            return Riddha.UI.Toast("Password is Requierd!!", 2);
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.EmployeeUserLogin()))
            .done(function (result) {
                if (result.Status == 4) {
                    getEmployeesUserSetup();
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.EmployeeUserLogin()))
            .done(function (result) {
                if (result.Status == 4) {
                    getEmployeesUserSetup();
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };


    self.Reset = function () {
        self.EmployeeUserLogin(new EmployeeUserSetupModel({ Id: self.EmployeeUserLogin().Id() }));
    };

    self.Select = function (model) {
        self.SelectedEmployeeUserLogin(model);
        self.EmployeeUserLogin(new EmployeeUserSetupModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (employeeUserSetup) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + employeeUserSetup.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmployeeUserLogins.remove(employeeUserSetup)
                    };
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#employeeUserCreationModel").modal('show');
    };

    $("#employeeUserCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#employeeUserCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };

}