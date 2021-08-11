function leaveReportController(date, companyName) {
    var self = this;
    var config = new Riddha.config();
    var language = config.CurrentLanguage;
    var curDate = config.CurDate;
    self.Report = ko.observable();
    self.ReportId = ko.observable(0);
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.OnDate = ko.observable(date).extend({ date: 'yyyy/MM/dd' });
    self.EndDate = ko.observable(date).extend({ date: 'yyyy/MM/dd' });
    self.Months = ko.observableArray([]);
    self.MonthId = ko.observable(0);
    self.Year = ko.observable(Riddha.util.getYear(curDate));
    self.ActiveInactiveMode = ko.observable("0");
    self.VisibleEndDate = ko.observable(false);
    self.FiscalYears = ko.observableArray([]);
    self.LeaveMasters = ko.observableArray([]);


    getDepartments();
    function getDepartments() {
        Riddha.ajax.get("/Api/AttendanceReportApi/GetDepartments", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Departments(data);
            });
    };



    getFiscalYears();
    function getFiscalYears() {
        Riddha.ajax.get("/Api/LeaveReportApi/GetFiscalYear", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.FiscalYears(data);
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
            //self.CheckAllEmployees(false);
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

    function getMonths() {
        var monthUrl = "";
        if (config.CurrentOperationDate == "ne") {
            monthUrl = "/Api/AttendanceReportApi/GetNepaliMonths";
        }
        else {
            monthUrl = "/Api/AttendanceReportApi/GetEnglishMonths"
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
    getMonths();

    self.ShowModal = function (id, title) {
        self.VisibleEndDate(false);
        self.OnDate(curDate);
        self.Report(title);
        self.ReportId(id);
        if (id == 30 || id == 31) {
            setDefDate();
            //self.VisibleEndDate(true);
        }
        if (id == 32) {
            setDefDate();
            //self.VisibleEndDate(true);
        }
        if (id == 30 || id == 31 || id == 32) {

            $("#leaveReportModel").modal('show');
        }
    };

    self.leaveMasterWiseReportShowModal = function (id, title) {
        $("#leaveMasterWiseReportModal").modal('show');
    }

    self.LeaveMasterWiseCloseModal = function () {
        $("#leaveMasterWiseReportModal").modal('hide');
    };

    self.CloseModal = function () {
        $("#leaveReportModel").modal('hide');
    };

    self.Reset = function () {
        self.CheckAllSections(false);
        self.CheckAllDepartments(false);
        ko.utils.arrayForEach(self.Departments(), function (data) {
            data.Checked(false);
            self.Sections([]);
            self.Employees([]);
        });
    };

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
            self.EndDate(setDateToTextBox(self.Year(), self.MonthId(), self.dateLength()));
        }
        if (Riddha.config().CurrentOperationDate == 'ne') {
            var onDateNe = setDateToTextBox(self.Year(), self.MonthId(), 1);
            self.OnDate(BS2AD(onDateNe));
            var toDateNe = setDateToTextBox(self.Year(), self.MonthId(), self.dateLength());
            self.EndDate(BS2AD(toDateNe));
        }
        function setDateToTextBox(year, month, id) {
            return '' + year + '/' + Riddha.util.padLeft(month, 2) + '/' + Riddha.util.padLeft(id, 2);
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

    function getSelectedFiscalYear() {
        var fiscalYear = "";
        ko.utils.arrayForEach(self.FiscalYears(), function (data) {
            if (data.Checked() == true) {
                if (fiscalYear.length != 0)
                    fiscalYear += "," + data.Id();
                else
                    fiscalYear = data.Id() + '';
            }
        });
        return fiscalYear;
    }


    self.KendoGridOptionsForLeaveWiseSummary = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();


        return {
            title: title,
            target: "#kendoLeaveWiseSummaryReportWindow",
            url: "/Api/LeaveReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            //height: docHeight,
            width: '70%',
            height: docHeight - 20,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                {
                    field: "LeaveName",

                }],
            //sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForLeaveSummary
        }
    }
    var docHeight = $(document).height();

    self.KendoGridOptionsForLeaveSummary = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        //var fiscalYear = getSelectedFiscalYear();
        return {
            title: title,
            target: "#kendoLeaveSummaryReportWindow",
            url: "/Api/LeaveReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            //height: docHeight,
            width: '70%',
            height: docHeight - 20,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                {
                    field: "Name",

                }],
            //sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForLeaveSummary
        }
    }

    self.KendoGridOptionsForLeaveReport = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#pivotgrid",
            targetPivotGrid: "#pivotgrid",
            url: "/Api/LeaveSummaryReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight - 20,
            multiSelect: false,
            maximize: true,
            pageSize: 10000,
            modelFields: {
                EmployeeName: { type: "string" },
                LeaveName: { type: "string" },
                TakenLeave: { type: "number" }
            },
            cubeDimensions: {
                EmployeeName: {
                    caption: language == "ne" ? "सबै कर्मचारीहरु" : "All Employees"
                },
                LeaveName: {
                    caption: language == "ne" ? "सबै बिदाहरु" : "All Leaves"
                },
            },
            measureField: "TakenLeave",
            columns: [{
                name: "LeaveName",
                //name: language == "ne" ? "बिदाको नाम " : "LeaveName",
                headerAttribures: { style: "white-space:normal" },
                expand: true
            }],
            rows: [{ name: "EmployeeName", expand: true }],
        }
    }

    self.columSpecForLeaveSummary = [
        {
            field: "Name", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" }, filterable: true,

        },
        {
            field: "LeaveName", title: language == "ne" ? "बिदा" : "Leave", headerAttributes: { style: "text-align:center;" }, filterable: true,

        },
        {
            field: "Balance", title: language == "ne" ? "उद्घाटन ब्यालेन्स" : "Opening Balance", headerAttributes: { style: "text-align:center;" }, filterable: true,

        },
        {
            field: "TakenLeave", title: language == "ne" ? "लिएको" : "Taken", headerAttributes: { style: "text-align:center;" }, filterable: true,

        },
        {
            field: "RemLeave", title: language == "ne" ? "बाँकी" : "Remaining", headerAttributes: { style: "text-align:center;" }, filterable: true,

        },
    ];

    function getSuitableTitle(title, date) {
        if (date == false) {
            if (language == "ne")
                return title + " " + SuitableDate(self.OnDate());
            else
                return title + " on " + SuitableDate(self.OnDate());
        }
        else {
            if (language == "ne")
                return title + " " + SuitableDate(self.OnDate()) + " बाट " + SuitableDate(self.EndDate()) + " सम्म ";
            else
                return title + " from " + SuitableDate(self.OnDate()) + " to " + SuitableDate(self.EndDate());
        }
    };


    function getSelectedLeaveMasters() {
        debugger;
        var leavemasters = "";
        ko.utils.arrayForEach(self.LeaveMasters(), function (data) {
            if (data.Checked() == true) {
                if (leavemasters.length != 0)
                    leavemasters += "," + data.Id();
                else
                    leavemasters = data.Id() + '';
            }
        });
        if (leavemasters == "" || leavemasters.length == 0) {
            ko.utils.arrayForEach(self.LeaveMasters(), function (data) {
                if (leavemasters.length != 0)
                    leavemasters += "," + data.Id();
                else
                    leavemasters = data.Id() + '';
            });
        }
        return leavemasters;
    }

    getLeaveMasterForCheckBoxList();
    function getLeaveMasterForCheckBoxList() {
        Riddha.ajax.get("/Api/LeaveMasterApi/GetLeaveMaster", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.LeaveMasters(data);
            });
    }

    self.KendoGridOptionsForLeaveWiseSummary = function (title) {
        var leaveMasters = getSelectedLeaveMasters();
        return {
            title: title,
            target: "#kendoLeaveWiseSummaryReportWindow",
            url: "/Api/LeaveReportApi/GenerateLeaveMasterWiseLeaveReport",
            paramData: { LeaveMasterIds: leaveMasters, ActiveInactiveMode: self.ActiveInactiveMode() },
            width: '70%',
            height: docHeight - 20,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                {
                    field: "Section",

                },
                {
                    field: "Employee",

                }],
            //sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForLeaveMasterWieReport
        }
    }
    self.columSpecForLeaveMasterWieReport = [
        {
            field: "Employee", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" }, filterable: false,

        },
        {
            field: "LeaveName", title: language == "ne" ? "बिदा" : "Leave", headerAttributes: { style: "text-align:center;" }, filterable: false,

        },
        {
            field: "FromDate", title: language == "ne" ? "FromDate " : "From Date", headerAttributes: { style: "text-align:center;" }, filterable: false, template: "#=SuitableDate(FromDate)#",

        },
        {
            field: "ToDate", title: language == "ne" ? "लिएको" : "To Date", headerAttributes: { style: "text-align:center;" }, filterable: false, template: "#=SuitableDate(ToDate)#",

        },
        {
            field: "LeaveDays", title: language == "ne" ? "बाँकी" : "Leave Days", headerAttributes: { style: "text-align:center;" }, filterable: false,

        },
        {
            field: "Description", title: language == "ne" ? "बाँकी" : "Description", headerAttributes: { style: "text-align:center;" }, filterable: false,

        },
    ];
}