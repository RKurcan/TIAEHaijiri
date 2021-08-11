/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.ReportingManager.Model.js" />

function reportingLineupController() {
    var self = this;
    var url = "/Api/ReportingLineupApi";
    self.Employee = ko.observable(new EmployeeSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.ActiveInactiveMode = ko.observable("0");
    self.SelectedReportingLineup = ko.observable();
    self.ReportingLineup = ko.observable(new ReportingLineupVm());

    //self.KendoGridOptions = {
    //    title: "Reporting Lineup",
    //    target: "#reportingLineupKendoGrid",
    //    url: "/Api/ReportingLineupApi/GetEmpKendoGrid",
    //    height: 490,
    //    paramData: {},
    //    multiSelect: false,
    //    parentId: 'ReportingManagerId',
    //    Id: "EmployeeId",
    //    //group: true,
    //    //groupParam: { field: "ReportingManager" },
    //    columns: [
    //        { field: 'IdCardNo', title: lang == "ne" ? "कर्मचारी कोड" : "Employee Code", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(IdCardNo)#' : '#:IdCardNo#', width: 100 },
    //        { field: 'EmployeeName', title: "Employee Name", width: 225, hidden: lang == "en" ? false : true },
    //        { field: 'EmployeeNameNp', title: "कर्मचारीको नाम", width: 225, hidden: lang == "ne" ? false : true, template: "#=EmployeeNameNp==null?EmployeeName:EmployeeNameNp#" },
    //        { field: 'Designation', title: lang == "ne" ? "पद" : "Designation" },
    //        { field: 'Department', title: lang == "ne" ? "विभाग" : "Department" },
    //        { field: 'Section', title: lang == "ne" ? "फाँट " : "Section" },
    //        { field: 'Mobile', title: lang == "ne" ? "मोबाईल." : "Mobile", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(Mobile)#' : '#:Mobile#' },
    //        { field: 'Email', title: lang == "ne" ? "ईमेल " : "Email" },
    //        { field: 'ReportingManager', title: lang == "ne" ? "ईमेल " : "Reporting Manager" },
    //    ],
    //    SelectedItem: function (item) {
    //        self.SelectedReportingLineup(new ReportingLineupGridVm(item));
    //    },
    //    SelectedItems: function (items) {
    //    }
    //}

    //self.RefreshKendoGrid = function () {
    //    var tree = $("#reportingLineupKendoGrid").data("kendoTreeList");
    //    tree.dataSource.read();
    //}

    self.KendoGridOptions = {
        title: "Reporting Lineup",
        target: "#reportingLineupKendoGrid",
        url: "/Api/ReportingLineupApi/GetEmpKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        selectable: true,
        groupParam: { field: "ReportingManager" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'IdCardNo', title: lang == "ne" ? "कर्मचारी कोड" : "Employee Code", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(IdCardNo)#' : '#:IdCardNo#', width: 100 },
            { field: 'EmployeeName', title: "Employee Name", width: 225, hidden: lang == "en" ? false : true },
            { field: 'EmployeeNameNp', title: "कर्मचारीको नाम", width: 225, hidden: lang == "ne" ? false : true, template: "#=EmployeeNameNp==null?EmployeeName:EmployeeNameNp#" },
            { field: 'Designation', title: lang == "ne" ? "पद" : "Designation" },
            { field: 'Department', title: lang == "ne" ? "विभाग" : "Department" },
            { field: 'Section', title: lang == "ne" ? "फाँट " : "Section" },
            { field: 'Mobile', title: lang == "ne" ? "मोबाईल." : "Mobile", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(Mobile)#' : '#:Mobile#' },
            { field: 'Email', title: lang == "ne" ? "ईमेल " : "Email" },
            { field: 'ReportingManager', title: lang == "ne" ? "ईमेल " : "Reporting Manager" },
        ],
        SelectedItem: function (item) {
            self.SelectedReportingLineup(new ReportingLineupGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };
    self.RefreshKendoGrid = function () {
        self.SelectedReportingLineup(new ReportingLineupGridVm());
        $("#reportingLineupKendoGrid").getKendoGrid().dataSource.read();
    }

    getDepartments();
    function getDepartments() {
        Riddha.ajax.get("/Api/AttendanceReportApi/GetDepartments", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Departments(data);
            });
    };

    //Checkall Section
    self.CheckAllDepartments.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Departments(), function (item) {
            item.Checked(newValue);
        });
        if (newValue) {
            self.GetSections();
        }
        else {
            self.Sections([]);
            self.CheckAllSections(false);
        }
    });

    self.CheckAllSections.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Sections(), function (item) {
            item.Checked(newValue);
        });
        if (newValue) {
            self.GetEmployee();
        }
        else {
            self.Employees([]);
        }
    });

    self.GetSections = function () {
        var departments = "";
        ko.utils.arrayForEach(self.Departments(), function (data) {
            if (data.Checked() == true) {
                if (departments.length != 0)
                    departments += "," + data.Id();
                else
                    departments = data.Id() + '';
            }
            else {
                self.Sections([]);
            }
        });
        if (departments.length > 0) {
            Riddha.ajax.get("/Api/AttendanceReportApi/GetSectionsByDepartment/" + departments)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Sections(data);
                });
        }
    };

    self.GetEmployee = function () {
        var sections = "";
        ko.utils.arrayForEach(self.Sections(), function (data) {
            if (data.Checked() == true) {
                if (sections.length != 0)
                    sections += "," + data.Id();
                else
                    sections = data.Id() + '';
            }
            else {
                self.Employees([]);
            }
        });
        if (sections.length > 0) {
            Riddha.ajax.get("/Api/AttendanceReportApi/GetEmployeeBySection?id=" + sections + "&activeInactiveMode=" + self.ActiveInactiveMode())
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Employees(data);
                });
        }
    };

    function getSelectedEmps() {
        var employees = "";
        ko.utils.arrayForEach(self.Employees(), function (data) {
            if (data.Checked() == true) {
                if (employees.length != 0)
                    employees += "," + data.Id();
                else
                    employees = data.Id() + '';
            }
        });
        return employees;
    }

    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.Employee(new EmployeeSearchViewModel(item));
            //self.LeaveApplication().EmployeeId(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    };


    self.Save = function () {
        var employees = getSelectedEmps();
        var managerId = self.Employee().Id;
        var data = { EmpIds: employees, ManagerId: managerId };
        Riddha.ajax.post("/Api/ReportingLineupApi/Save", ko.toJS(data))
            .done(function (result) {
                Riddha.UI.Toast("Added Successfully", 4);
                self.CloseModal();
                self.RefreshKendoGrid();
            });
    };

    self.Update = function () {
        var managerId = self.Employee().Id;
        var employeeId = self.ReportingLineup().EmployeeId();
        var data = { EmpIds: [employeeId], ManagerId: managerId };
        Riddha.ajax.post("/Api/ReportingLineupApi/Save", ko.toJS(data))
            .done(function (result) {
                Riddha.UI.Toast("Updated Successfully", 4);
                self.EditCloseModal();
                self.RefreshKendoGrid();
            });
    };
    self.ShowModal = function () {
        $("#reportingLineupModal").modal('show');
    };

    $("#reportingLineupModal").on('hidden.bs.modal', function () {
        //self.RefreshKendoGrid();
        //self.ModeOfButton("Create");
        //self.Reset();
    });

    self.CloseModal = function () {
        $("#reportingLineupModal").modal('hide');
        self.RefreshKendoGrid();
        self.Reset();
    };

    self.Select = function (model) {
        if (self.SelectedReportingLineup() == undefined) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        Riddha.ajax.get(url + "/Get/" + self.SelectedReportingLineup().EmployeeId(), null)
            .done(function (result) {
                self.ReportingLineup(new ReportingLineupVm(result.Data));
                self.Employee(new EmployeeSearchViewModel({ Name: self.ReportingLineup().ReportingManagerName(), Designation: self.ReportingLineup().ReportingManagerDesignation(), Department: self.ReportingLineup().ReportingManagerDepartment(), Section: self.ReportingLineup().ReportingManagerSection(), Photo: self.ReportingLineup().ReportingManagerPhoto() }));
                self.EditShowModal();
            });
    }

    self.EditShowModal = function () {
        $("#reportingLineupUpdateModal").modal('show');
    };

    self.EditCloseModal = function () {
        $("#reportingLineupUpdateModal").modal('hide');
        self.RefreshKendoGrid();
        self.Reset();
    };


    self.Reset = function () {
        self.Employee(new EmployeeSearchViewModel());
        self.ReportingLineup(new ReportingLineupVm());
        self.Sections([]);
        self.Employees([]);
        self.CheckAllSections(false);
        self.CheckAllDepartments(false);
    };
}