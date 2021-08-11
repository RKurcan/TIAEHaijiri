/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
function payrollReportController() {
    var self = this;
    var config = new Riddha.config();
    var language = config.CurrentLanguage;
    var curDate = config.CurDate;    
    self.PayrollReports = ko.observableArray([]);
    self.Report = ko.observable();
    self.ReportView = ko.observable();
    self.ReportVisible = ko.observable(false);
    self.ReportId = ko.observable(0);
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.OnDate = ko.observable('').extend({ Date: '' });
    self.EndDate = ko.observable('').extend({ Date: '' });
    self.Daywise = ko.observable(0);
    self.VisibleEndDate = ko.observable(false);
    self.Months = ko.observableArray([
    { Id: 1, Name: "January" },
    { Id: 2, Name: "February" },
    { Id: 3, Name: "March" },
    { Id: 4, Name: "April" },
    { Id: 5, Name: "May" },
    { Id: 6, Name: "June" },
    { Id: 7, Name: "July" },
    { Id: 8, Name: "August" },
    { Id: 9, Name: "September" },
    { Id: 10, Name: "October" },
    { Id: 11, Name: "November" },
    { Id: 12, Name: "December" }
    ]);
    self.MonthId = ko.observable();
    self.Year = ko.observable(new Date().getFullYear());
    getReportItems();
    function getReportItems() {
        Riddha.ajax.get("/Api/PayRollReportApi")
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ReportItemModel);
            self.PayrollReports(data);
        });
    }
    self.ShowModal = function (item) {
        self.Report(item.Report());
        self.ReportId(item.ReportId());
        if (item.ReportId() == 16 || item.ReportId() == 10) {
            self.VisibleEndDate(true);
        }
        $("#reportModel").modal('show');
    }

    $("#reportModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#reportModel").modal('hide');
    }
    self.Reset = function () {
    }
    self.GenerateReport = function () {
        var employees = "";
        ko.utils.arrayForEach(self.Employees(), function (data) {
            if (data.Checked() == true) {
                if (employees.length != 0)
                    employees += "," + data.Id();
                else
                    employees = data.Id() + '';
            }
        });
        if (employees.length > 0) {
            var url = "";

            if (self.ReportId() == 20) {
                url = "/PayRollReport/GenerateReport/?id=" + employees + "&onDate=" + self.Year() + "&month=" + self.MonthId()
            }
            else if (self.ReportId() == 21) {
                url = "/MonthlyWiseAttendanceRpt/GenerateReport/?id=" + employees + "&startDate=" + self.OnDate() + "&endDate=" + self.EndDate()
            }

            Riddha.ajax.get(url)
            .done(function (result) {
                $("#reportModel").modal('hide');
                self.ReportVisible(true);
                self.ReportView(result);
            });
        }
    }
    self.BackToReportGrid = function () {
        self.ReportVisible(false);
    }

    getDepartments();
    function getDepartments() {
        Riddha.ajax.get("/Api/DepartmentApi", null)
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
            self.CheckAllEmployees(false);
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
            self.CheckAllEmployees(false);
        }
    });

    self.CheckAllEmployees.subscribe(function (newValue, e) {
        ko.utils.arrayForEach(self.Employees(), function (item) {
            item.Checked(newValue);
        });
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
            Riddha.ajax.get("/Api/SectionApi/GetSectionsByDepartment/" + departments)
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
            Riddha.ajax.get("/Api/EmployeeApi/GetEmployeeBySection/" + sections)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Employees(data);
            });
        }
    };

    self.ReportViewModel = {

        MonthId: ko.observable(),
        Year: ko.observable(),
        EmployeeIds: ko.observableArray([]),
        RosterRows: ko.observableArray([
        ])
    }
}