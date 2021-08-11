/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Roster.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
function weeklyRosterController() {
    var self = this;
    self.Roster = ko.observable(new RosterModel);
    var url = "/Api/RosterApi";
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.Months = ko.observableArray([]);
    self.HeaderShifts = ko.observableArray([]);
    var config = Riddha.config();

    self.MonthId = ko.observable();
    self.Year = ko.observable(config.CurrentOperationDate == "en" ? new Date().getFullYear() : '2074');
    self.Days = ko.observableArray([]);
    self.Shifts = ko.observableArray([]);
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


    self.Search = function () {
        if (self.BranchId() == undefined) {
            return;
        }
        getDepartments();
    };
    
    function getDepartments() {
        //Riddha.ajax.get(url + "/GetDepartment", null)
        Riddha.ajax.get("/Api/RosterApi/GetDepartments?branchId=" + self.BranchId(), null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
            self.Departments(data);
        });
    };

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

    });

    self.ChangeHeadSelection = function (data) {
        ko.utils.arrayForEach(self.RosterViewModel.RosterRows(), function (item) {
            item.Columns()[data.Day - 1].ShiftId(data.ShiftId())
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
            Riddha.ajax.get("/Api/AttendanceReportApi/GetSectionsByDepartment/" + departments)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Sections(data);
            });
        }
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
            //{
           // EngDate: "01/01/2017", NepDate: '', DayName: 'Sunday', Shifts: ko.observableArray([])

//        }
        ])



    }
    self.ShiftWiseSummary = ko.observableArray([]);
    self.CalculateTotal = function (day, e) {
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
    getShift();
    self.RefreshRoster = function () {
        var SectionIds = '';
        ko.utils.arrayForEach(self.Sections(), function (item) {
            SectionIds += ',' + item.Id();
        });
        if (SectionIds.length > 0) {
            SectionIds = SectionIds.substr(1, SectionIds.length);
        }
        else {
            Riddha.UI.Toast("Please Choose Section");
            return;
        }
        self.HeaderShifts([]);
        Riddha.ajax.get("/Api/WeeklyRosterApi/RefreshRoster?SectionIds=" + SectionIds)
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
        });
    }
    self.kendoWindowParam = {
        title: 'Weekly Roster Management',
        open: self.RefreshRoster,
        maximize: false,
        width: '90%',
        height: '600',
        position: 'center',
        //actions:['close'],
        target: '#rosterWindow'
    };
    self.CreateUpdate = function () {
        Riddha.ajax.post("/Api/weeklyRosterApi/", ko.toJS(self.RosterViewModel))
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