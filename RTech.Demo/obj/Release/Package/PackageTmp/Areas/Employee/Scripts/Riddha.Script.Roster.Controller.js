/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Roster.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
function rosterController() {
    var self = this;
    self.Roster = ko.observable(new RosterModel);
    var url = "/Api/RosterApi";
    var config = new Riddha.config();
    var curDate = config.CurDate;
    self.Departments = ko.observableArray([]);
    self.SearchDepartmentText = ko.observable('');
    self.FilteredDepartment = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.SearchSectionText = ko.observable('');
    self.FilteredSection = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.SearchEmployeeText = ko.observable('');
    self.FilteredEmployee = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.CheckedSections = ko.observableArray([]);
    self.Months = ko.observableArray([]);
    self.Branches = ko.observableArray([]);
    self.BranchId = ko.observable(0);

    getBranches();
    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Branches(data);
            });
    };


    if (config.CurrentOperationDate == "en") {
        self.Months([
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
    }
    else {
        self.Months([
            { Id: 1, Name: config.CurrentLanguage == 'ne' ? "बैसाख" : "Baishakh" },
            { Id: 2, Name: config.CurrentLanguage == 'ne' ? "जेठ" : "Jestha" },
            { Id: 3, Name: config.CurrentLanguage == 'ne' ? "असार" : "Asar" },
            { Id: 4, Name: config.CurrentLanguage == 'ne' ? "साउन" : "Shrawan" },
            { Id: 5, Name: config.CurrentLanguage == 'ne' ? "भदौ" : "Bhadau" },
            { Id: 6, Name: config.CurrentLanguage == 'ne' ? "असोज" : "Aswin" },
            { Id: 7, Name: config.CurrentLanguage == 'ne' ? "कार्तिक" : "Kartik" },
            { Id: 8, Name: config.CurrentLanguage == 'ne' ? "मंसिर" : "Mansir" },
            { Id: 9, Name: config.CurrentLanguage == 'ne' ? "पुष" : "Poush" },
            { Id: 10, Name: config.CurrentLanguage == 'ne' ? "माघ" : "Magh" },
            { Id: 11, Name: config.CurrentLanguage == 'ne' ? "फाल्गुन" : "Falgun" },
            { Id: 12, Name: config.CurrentLanguage == 'ne' ? "चैत्र" : "Chaitra" },
        ]);
    }

    if (config.CurrentLanguage == "ne" && config.CurrentOperationDate == "en") {
        self.Months([
            { Id: 1, Name: "जनवरी" },
            { Id: 2, Name: "फेब्रुअरी" },
            { Id: 3, Name: "मार्च" },
            { Id: 4, Name: "अप्रिल" },
            { Id: 5, Name: "मे" },
            { Id: 6, Name: "जून" },
            { Id: 7, Name: "जुलाई" },
            { Id: 8, Name: "अगस्ट" },
            { Id: 9, Name: "सेप्टेम्बर" },
            { Id: 10, Name: "अक्टोबर" },
            { Id: 11, Name: "नोभेम्बर" },
            { Id: 12, Name: "डिसेम्बर" }
        ]);
    }


    self.MonthId = ko.observable(Riddha.util.getMonth(curDate));
    self.Year = ko.observable(Riddha.util.getYear(curDate)); //ko.observable(config.CurrentOperationDate == "en" ? new Date().getFullYear() : '2074');
    self.Days = ko.observableArray([]);
    self.Shifts = ko.observableArray([]);


    self.Search = function () {
        if (self.BranchId() == undefined) {
            return;
        }
        getDepartments();
    };

    function getDepartments() {
        Riddha.ajax.get(url + "/GetDepartments?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Departments(data);
                self.FilteredDepartment(data);
            });
    };

    self.SearchDepartmentText.subscribe(function (newValue) {
        if (newValue == '') {
            self.FilteredDepartment(self.Departments());
        } else {
            self.FilteredDepartment(Riddha.ko.global.filter(self.Departments, newValue));
        }
    });


    //Checkall Dep
    self.CheckAllDepartments.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Departments(), function (item) {
            item.Checked(newValue);
        });
        if (newValue) {
            self.GetSections();
        }
        else {
            self.Sections([]);
            self.FilteredSection([]);
            self.CheckAllSections(false);
        }
    });

    self.CheckAllSections.subscribe(function (newValue) {
        $.each(ko.toJS(self.Sections()), function (i, item) { self.CheckedSections.push(item.Id) })
        if (newValue) {
            self.GetEmployee();
        }
        else {
            self.CheckedSections([]);
            self.Employees([]);
            self.FilteredEmployee([]);
            //self.CheckAllEmployees(false);
        }
    });

    self.CheckAllEmployees.subscribe(function (newValue, e) {
        //ko.utils.arrayForEach(self.Employees(), function (item) {
        //    item.Checked(newValue);
        //});
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
                self.FilteredSection([]);
            }
        });
        if (departments.length > 0) {
            Riddha.ajax.get("/Api/RosterApi/GetSectionsByDepartment/" + departments)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Sections(data);
                    self.FilteredSection(data);
                });
        }
    };

    self.SearchSectionText.subscribe(function (newValue) {
        if (newValue == '') {
            self.FilteredSection(self.Sections());
        } else {
            self.FilteredSection(Riddha.ko.global.filter(self.Sections, newValue));
        }
    });

    self.GetEmployee = function () {
        if (self.CheckedSections().length > 0) {
            Riddha.ajax.get("/Api/RosterApi/GetEmployeeBySection?id=" + self.CheckedSections().toString() + "&activeInactiveMode=" + 0)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Employees(data);
                    self.FilteredEmployee(data);
                });
        }
        else {
            self.FilteredEmployee([]);
        }
    };

    self.SearchEmployeeText.subscribe(function (newValue) {
        if (newValue == '') {
            self.FilteredEmployee(self.Employees());
        } else {
            self.FilteredEmployee(Riddha.ko.global.filter(self.Employees, newValue));
        }
    });



    self.ChangeHeadSelection = function (data) {
        ko.utils.arrayForEach(self.RosterViewModel.RosterRows(), function (item) {
            item.Columns()[data.Day - 1].ShiftId(data.ShiftId())
        });
    };


    self.RosterDetails = ko.observableArray([]);
    self.getValue = function (engDate, ShiftId) {
        return new RosterModel({ Id: 0, Date: engDate, ShiftId: ShiftId });
    };

    self.RosterViewModel = {
        MonthId: ko.observable(),
        Year: ko.observable(),
        Headers: ko.observableArray([]),
        RosterRows: ko.observableArray([
        ])
    }

    self.ShiftWiseSummary = ko.observableArray([]);
    self.CalculateTotal = function (day, e) {
        console.log(e);

        var day = $(e.target).data('day');
        sumShiftTotal(day);
    }

    function sumShiftTotal(day) {
        var data = new Array();
        $('[data-day="' + day + '"]').each(function () {
            data.push({ day: day, shift: $(this).val() });
        });
        ko.utils.arrayForEach(self.Shifts(), function (item) {
            var filtered = ko.utils.arrayFilter(data, function (d) {
                return item.Id() + "" == d.shift;
            });
            $('[data-shift="' + item.Id() + '"] [data-day="' + day + '"]').html(filtered.length);

        })
    }

    var getShift = function () {
        Riddha.ajax.get("/Api/RosterApi/getshifts")
            .done(function (result) {
                self.Shifts(Riddha.ko.global.arrayMap(result.Data, ShiftModel));
                self.ShiftWiseSummary(new Array(result.Data.length));


            });
    }
    self.HeaderShifts = ko.observableArray([]);
    getShift();
    self.RefreshRoster = function () {
        //var SectionIds = '';
        var departments = getSelectedDepartments();
        var sections = self.CheckedSections();
        var employees = getSelectedEmps();

        if (departments.length < 1) {
            Riddha.UI.Toast("Select Departments to refresh roster", 0);
            return;
        }
        if (self.CheckedSections().length < 1) {
            Riddha.UI.Toast("Select Sections to refresh roster", 0);
            return;
        }
        if (self.Year() == "") {
            Riddha.UI.Toast("Please enter year", 0);
            return;
        }
        if (self.MonthId() == undefined) {
            Riddha.UI.Toast("Please select month", 0);
            return;
        }
        var data = { Year: self.Year(), MonthId: self.MonthId(), DepartmentIds: departments, SectionIds: sections, EmpIds: employees };
        // Riddha.ajax.get("/Api/RosterApi/RefreshRoster?year=" + self.Year() + "&monthId=" + self.MonthId() + "&departmentIds=" + departments + "&sectionIds=" + sections + "&empIds=" + employees)

        self.HeaderShifts([]);
        Riddha.ajax.get(url + "/RefreshRoster?year=" + self.Year() + "&monthId=" + self.MonthId() + "&departmentIds=" + departments + "&sectionIds=" + sections.toString() + "&empIds=" + employees, data)
            .done(function (result) {

                self.RosterViewModel.RosterRows([]);
                self.RosterViewModel.Year = self.Year();
                self.RosterViewModel.MonthId = self.MonthId();
                self.RosterViewModel.RosterRows(Riddha.ko.global.arrayMap(result.Data.RosterRows, RosterRow));

                if (result.Data.RosterRows.length > 0) {
                    self.RosterViewModel.Headers(result.Data.RosterRows[0].Columns);
                    for (var i = 0; i < result.Data.RosterRows[0].Columns.length; i++) {
                        self.HeaderShifts.push(ko.observable({ ShiftId: ko.observable(), Day: (i + 1) }));
                        sumShiftTotal(i + 1);
                    }
                }
                window.scrollTo(0, $("#rosterWindowtest").offset().top);
                if (!$("#layout-body").hasClass('sidebar-collapse')) {
                    $(".sidebar-toggle").click();
                }
            });
    }

    //latest script for filtering department,section and employee
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
        ko.utils.arrayForEach(self.FilteredSection(), function (data) {

            if (sections.length != 0)
                sections += "," + data.Id();
            else
                sections = data.Id() + '';
        });
        return sections;
    }


    self.ExportRosterExcel = function () {
        debugger;
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();
        if (departments.length < 1) {
            Riddha.UI.Toast("Select Departments to refresh roster", 0);
            return;
        }
        if (self.Year() == "") {
            Riddha.UI.Toast("Please enter year", 0);
            return;
        }
        if (self.MonthId() == undefined) {
            Riddha.UI.Toast("Please select month", 0);
            return;
        }
        //window.open("/Employee/Roster/ExportRosterExcel?year=" + self.Year() + "&monthId=" + self.MonthId() + "&departmentIds=" + departments + "&sectionIds=" + sections + "&empIds=" + employees);
        window.open("/Employee/Roster/ExportRosterExcel?year=" + self.Year() + "&monthId=" + self.MonthId() + "&SectionIds=" + sections);

    }
    self.trigerFileBrowse = function () {
        $("#UploadedFile").click();
    }
    self.UploadClick = function () {
        var xhr = new XMLHttpRequest();
        var file = document.getElementById('UploadedFile').files[0];
        if (file == undefined) {
            return Riddha.UI.Toast("Please Select Excel File to Upload", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Year() == "") {
            Riddha.UI.Toast("Please enter year", 0);
            return;
        }
        if (self.MonthId() == undefined) {
            Riddha.UI.Toast("Please select month", 0);
            return;
        }
        Riddha.UI.Confirm("ExcelUploadConfirm", function () {
            xhr.open("POST", "/api/RosterApi/Upload?Year=" + self.Year() + "&MonthId=" + self.MonthId());
            xhr.setRequestHeader("filename", file.name);
            xhr.onreadystatechange = function (data) {
                if (xhr.readyState == 4) {
                    var response = JSON.parse(xhr.responseText);
                    if (response["Status"] == 4) {
                    }
                    return Riddha.UI.Toast(response["Message"], response["Status"]);
                }
            };
            xhr.send(file);
        });
    };
    self.kendoWindowParam = {
        title: 'Roster Management',
        open: self.RefreshRoster,
        maximize: true,
        //width: 900,
        //height: 300,
        position: 'center',
        //actions:['close'],
        target: '#rosterWindow'
    };
    self.CreateUpdate = function () {
        Riddha.ajax.post("/Api/RosterApi", ko.toJS(self.RosterViewModel))
            .done(function (result) {
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }

    self.newRosterViewModel = {

        Rows: ko.observableArray([])
    };
    self.newRosterColumns = {

    }
}
function RosterRowsViewModelUpdated(item) {
    var self = this;
    item = item || {};
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || 0);
    self.ShiftId = ko.observable(item.ShiftId);
    self.Date = ko.observable(item.ShiftId);
}

function RosterRow(item) {
    var self = this;
    item = item || {};
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || 0);
    self.Columns = ko.observableArray([]);
    if (item.Columns.length > 0) {
        self.Columns(Riddha.ko.global.arrayMap(item.Columns, ColumnsModel));
    }

}
function ColumnsModel(item) {
    var self = this;
    item = item || {};
    self.Day = ko.observable(item.Day || 0);
    self.DayName = ko.observable(item.DayName || '');
    self.ShiftId = ko.observable(item.ShiftId || 0);
}
function KeyValue(item) {

}