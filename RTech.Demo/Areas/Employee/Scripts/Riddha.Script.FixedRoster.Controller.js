/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.FixedRoster.Model.js" />

function fixedRosterController() {
    var self = this;
    self.FixedRoster = ko.observable(new FixedRosterModel);
    var url = "/Api/FixedRosterApi";
    var config = Riddha.config();
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
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

    //getDepartments();
    //function getDepartments() {
    //    Riddha.ajax.get(url + "/GetDepartment", null)
    //    .done(function (result) {
    //        var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
    //        self.Departments(data);
    //    });
    //};
    self.Search = function () {
        if (self.BranchId() == undefined) {
            return;
        }
        getDepartments();
    };

    function getDepartments() {
        Riddha.ajax.get("/Api/RosterApi/GetDepartments?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Sections([]);
                self.Departments([]);
                self.CheckAllDepartments(false);
                self.CheckAllSections(false);
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
            Riddha.ajax.get("/Api/RosterApi/GetSectionsByDepartment/" + departments)
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
        ])
    }

    self.ShiftWiseSummary = ko.observableArray([]);
    self.CalculateTotal = function () {
        var data = new Array();
        $('[data-shift="0"]').each(function () {
            data.push({ day: 0, shift: $(this).val() });
        });
        ko.utils.arrayForEach(self.Shifts(), function (item) {
            var filtered = ko.utils.arrayFilter(data, function (d) {
                return item.Id() + "" == d.shift;
            });
            $('[data-result="' + item.Id() + '"]').html(filtered.length);
        })
    }

    function sumShiftTotal(day) {
       
    };

    var getShift = function () {
        Riddha.ajax.get("/Api/FixedRosterApi/GetShifts")
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ShiftModel);
            self.Shifts(data);
        });
    };


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
        Riddha.ajax.get("/Api/FixedRosterApi/RefreshRoster?SectionIds=" + SectionIds)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmployeeFixedShiftModel);
            self.Employees(data);
            self.CalculateTotal();
        })
    }
    getShift();
    self.CreateUpdate = function () {
        Riddha.ajax.post("/Api/FixedRosterApi/",{ EmployeeFixedShifts:ko.toJS(self.Employees)})
       .done(function (result) {
           Riddha.UI.Toast(result.Message, result.Status);
       });
    }
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