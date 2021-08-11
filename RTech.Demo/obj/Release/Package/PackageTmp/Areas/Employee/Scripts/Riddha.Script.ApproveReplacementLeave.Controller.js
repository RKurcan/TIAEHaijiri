/// <reference path="Riddha.Script.ApproveReplacementLeave.Model.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
function ApproveReplacementLeaveController() {
    var self = this;
    var url = "/Api/ApproveReplacementLeaveApi";
    var config = new Riddha.config();
    var language = config.CurrentLanguage;
    var opDate = config.CurrentOperationDate;
    var curDate = config.CurDate;
    self.Report = ko.observable();
    self.ReportId = ko.observable(0);
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.CheckAllCourses = ko.observable(false);
    self.Months = ko.observableArray([]);
    self.MonthId = ko.observable(0);
    self.Year = ko.observable(Riddha.util.getYear(curDate));
    self.VisibleEndDate = ko.observable(false);
    self.VisibleOnDate = ko.observable(false);
    self.OnDate = ko.observable('');
    self.ToDate = ko.observable('');
    self.SelectedReplacementLeave = ko.observable();
    self.ReplacementLeaves = ko.observableArray([]);
    self.Branches = ko.observableArray([]);
    self.BranchId = ko.observable(0);

    getBranches();
    getMonths();

    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Branches(data);
            });
    };
    self.Search = function () {
        if (self.BranchId() == undefined) {
            return;
        }
        self.CheckAllDepartments(false);
        self.CheckAllSections(false);
        self.Employees([]);
        getDepartments();
    };


    function getMonths() {
        var monthUrl = "";
        if (config.CurrentOperationDate == "ne") {
            monthUrl = "/Api/ApproveReplacementLeaveApi/GetNepaliMonths";
        }
        else {
            monthUrl = "/Api/ApproveReplacementLeaveApi/GetEnglishMonths"
        }
        Riddha.ajax.get(monthUrl)
            .done(function (result) {
                if (result.Status == 4) {
                    for (var i = 0; i < result.Data.length; i++) {
                        self.Months.push(new GlobalDropdownModel({ Id: i + 1, Name: result.Data[i] }));
                    }
                    self.MonthId(Riddha.util.getMonth(curDate));
                }
            })
    }


    function getDepartments() {
        Riddha.ajax.get("/Api/ApproveReplacementLeaveApi/GetDepartments?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Departments(data);
            });
    };

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
            Riddha.ajax.get("/Api/ApproveReplacementLeaveApi/GetSectionsByDepartment/" + departments)
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
            Riddha.ajax.get("/Api/ApproveReplacementLeaveApi/GetEmployeeBySection?id=" + sections + "&activeInactiveMode=0")
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
        if (employees == '' && self.Employees().length != 0) {
            ko.utils.arrayForEach(self.Employees(), function (data) {

                if (employees.length != 0)
                    employees += "," + data.Id();
                else
                    employees = data.Id() + '';

            });
        }
        return employees;
    }

    function getSelectedDepartments() {
        var deps = "";
        ko.utils.arrayForEach(self.Departments(), function (data) {
            if (data.Checked() == true) {
                if (deps.length != 0)
                    deps += "," + data.Id();
                else
                    deps = data.Id() + '';
            }
        });
        return deps;
    }

    function getSelectedSections() {
        var sections = "";
        ko.utils.arrayForEach(self.Sections(), function (data) {
            if (data.Checked() == true) {
                if (sections.length != 0)
                    sections += "," + data.Id();
                else
                    sections = data.Id() + '';
            }
        });
        return sections;
    }

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
            self.CheckAllCourses(false);
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

    self.Reset = function () {
        self.CheckAllSections(false);
        self.CheckAllDepartments(false);
        ko.utils.arrayForEach(self.Departments(), function (data) {
            data.Checked(false);
            self.Sections([]);
            self.Employees([]);
        });
    }
    self.checkCallBack = "";
    //Kendo Grid
    self.KendoGridOptions = {
        title: "Replacement Leave",
        target: "#approveReplacementLeaveKendoGrid",
        url: "/Api/ApproveReplacementLeaveApi/GetReplacementLeaveForApproval",
        height: 400,
        paramData: function () { return { BranchId: self.BranchId(), DeptIds: getSelectedDepartments(), SectionIds: getSelectedSections(), EmpIds: getSelectedEmps(), onDate: self.OnDate(), ToDate: self.ToDate() } },
        multiSelect: false,
        group: true,
        selectable: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'NepDate', hidden: opDate == "ne" ? false : true, title: lang == "ne" ? "मिति" : "Date", width: 100, filterable: false },
            { field: 'Date', hidden: opDate == "en" ? false : true, title: lang == "ne" ? "मिति" : "Date", width: 100, filterable: false },
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारीको नाम" : "Emp.Code", width: 150, filterable: false },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको कोड" : "Emp.Name", width: 300 },
            { field: 'PresentOnHoliday', title: lang == "ne" ? "छुट्टीको दिन प्रस्तुत" : "Present In Holiday", filterable: false },
            { field: 'PresentOnDayOff', title: lang == "ne" ? "बन्दको दिन प्रस्तुत" : "Present In Day Off", filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedReplacementLeave(new ReplacementLeaveApprovalGridModel(item));
        },
        open: function (callback) {
            self.checkCallBack = callback;
        },
        //autoOpen: false
    }
    self.RefreshKendoGrid = function () {
        self.SelectedReplacementLeave(new ReplacementLeaveApprovalGridModel());
        $("#approveReplacementLeaveKendoGrid").getKendoGrid().dataSource.read();
    }
    self.dateLength = ko.observable(28);
    self.ComputeMaxDate = ko.computed(function () {
        Riddha.global.getMaxDaysInMonth(self.Year(), self.MonthId())
            .then(function (maxDays) {
                self.dateLength(maxDays);

                //for ondate and end date 
                setDefDate()
            });
    });
    function setDefDate() {
        if (Riddha.config().CurrentOperationDate == 'en') {
            self.OnDate(setDateToTextBox(self.Year(), self.MonthId(), 1));
            self.ToDate(setDateToTextBox(self.Year(), self.MonthId(), self.dateLength()));
        }
        if (Riddha.config().CurrentOperationDate == 'ne') {
            var onDateNe = setDateToTextBox(self.Year(), self.MonthId(), 1);
            self.OnDate(BS2AD(onDateNe));
            var toDateNe = setDateToTextBox(self.Year(), self.MonthId(), self.dateLength());
            self.ToDate(BS2AD(toDateNe));
        }
        function setDateToTextBox(year, month, id) {
            return '' + year + '/' + Riddha.util.padLeft(month, 2) + '/' + Riddha.util.padLeft(id, 2);
        }
    }

    self.RefreshApprovalGrid = function () {

        self.checkCallBack();
    }

    self.Approve = function () {
        var grid = $("#approveReplacementLeaveKendoGrid").getKendoGrid();
        if (grid == undefined) {
            Riddha.UI.Toast("Please Refresh or select Employee", 0);
            return;
        }
        Riddha.UI.Confirm("Confirm to approve", function () {

            var datas = grid.dataSource.data();
            self.ReplacementLeaves(Riddha.ko.global.arrayMap(datas, ReplacementLeaveApprovalGridModel));
            Riddha.ajax.post(url, { List: self.ReplacementLeaves() })
                .done(function (result) {
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    }

    //Leave Replacement Approval List Kendo grid
    self.KendoGridOptionForApprovalList = {
        title: "Approval List",
        target: "#approvalListKendoGrid",
        url: "/Api/ApproveReplacementLeaveApi/GetApprovalListKendoGrid",
        height: 400,
        paramData: {},
        selectable: true,
        group: true,
        groupParam: { field: "EmployeeName" },
        columns: [
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारीको नाम" : "Emp.Code", width: 150, filterable: false },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको कोड" : "Emp.Name", width: 300 },
            { field: 'Date', title: lang == "ne" ? "मिति" : "Date", width: 100, filterable: false, template: "#=SuitableDate(Date)#" },
            { field: 'PresentOnHoliday', title: lang == "ne" ? "छुट्टीको दिन प्रस्तुत" : "Present In Holiday", filterable: false },
            { field: 'PresentOnDayOff', title: lang == "ne" ? "बन्दको दिन प्रस्तुत" : "Present In Day Off", filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedSection(new SectionModel(item));
        },
        SelectedItems: function (items) {

        }
    };
}