/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Report.Model.js" />

function attendanceReportController(date, companyName) {
    var self = this;
    var config = new Riddha.config();
    var language = config.CurrentLanguage;
    var curDate = config.CurDate;
    var opDate = config.CurrentOperationDate;
    self.PackageId = ko.observable(config.PackageId);
    self.AttendanceReports = ko.observableArray([]);
    self.PayrollReports = ko.observableArray([]);
    self.Report = ko.observable();
    self.ReportView = ko.observable();
    self.ReportVisible = ko.observable(false);
    self.ReportId = ko.observable(0);
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
    self.CheckAllBranches = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.OnDate = ko.observable(date).extend({ date: 'yyyy/MM/dd' });
    self.EndDate = ko.observable(date).extend({ date: 'yyyy/MM/dd' });
    self.OTV2 = ko.observable(false);
    self.Daywise = ko.observable("1");
    self.VisibleEndDate = ko.observable(false);
    self.CompanyName = ko.observable(companyName);
    self.Months = ko.observableArray([]);
    self.MonthName = ko.observable('');
    self.ToMonthName = ko.observable('');
    self.MonthId = ko.observable(0);
    self.ToMonthId = ko.observable(0);
    self.FiscalYearId = ko.observable(0);
    self.FiscalYears = ko.observableArray([]);
    self.Year = ko.observable(Riddha.util.getYear(curDate));
    self.ActiveInactiveMode = ko.observable("0");
    self.IncludePunchTime = ko.observable(false);
    self.IsLeaveWiseSummary = ko.observable(false);
    getReportItems();
    //getPayrollreportItems();
    self.PageSizeByDate = ko.observable(0);
    self.showLoading = ko.observable(false);

    self.AllEmployees = ko.observableArray([]);
    self.EmployeeSummaryData = ko.observableArray([]);
    self.EmployeeSummaryDataGroupBy = ko.observableArray([]);
    self.EmployeeSummaryHourWiseData = ko.observableArray([]);
    self.EmployeeSummaryHourWiseDataGroupBy = ko.observableArray([]);
    self.MonthlyAttendanceReportData = ko.observableArray([]);
    self.MonthlyMultiPunchReportData = ko.observableArray([]);
    self.MonthlyMultiPunchReportDataGroupBy = ko.observableArray([]);
    self.MonthlyEarlyInData = ko.observableArray([]);
    self.MonthlyEarlyOutData = ko.observableArray([]);
    self.MonthlyLateInData = ko.observableArray([]);
    self.MonthlyLateOutData = ko.observableArray([]);
    self.MonthlyAbsentData = ko.observableArray([]);
    self.MonthlyLeaveData = ko.observableArray([]);
    self.MonthlyOTData = ko.observableArray([]);
    self.MonthlyManualPunchData = ko.observableArray([]);
    self.MonthlyMissingPunchData = ko.observableArray([]);
    self.MonthlyEarlyInDataGroupBy = ko.observableArray([]);
    self.MonthlyEarlyOutDataGroupBy = ko.observableArray([]);
    self.MonthlyLateInDataGroupBy = ko.observableArray([]);
    self.MonthlyLateOutDataGroupBy = ko.observableArray([]);
    self.MonthlyAbsentDataGroupBy = ko.observableArray([]);
    self.MonthlyLeaveDataGroupBy = ko.observableArray([]);
    self.MonthlyMissingPunchDataGroupBy = ko.observableArray([]);
    self.MonthlyOTDataGroupBy = ko.observableArray([]);
    self.MonthlyManualPunchDataGroupBy = ko.observableArray([]);
    self.Branches = ko.observableArray([]);
    self.BranchId = ko.observable(0);
    self.CheckedSections = ko.observableArray([]);
    self.CheckedDepartments = ko.observableArray([]);
    self.CurrentFiscalYearName = ko.observable('');

    var companyName = localStorage.getItem('companyName');
    var companyPhone = SuitableNumber(localStorage.getItem('companyPhone'));
    var companyEmail = localStorage.getItem('companyEmail');
    var companyLogo = localStorage.getItem('companyLogo');
    var companyAddress = localStorage.getItem('companyAddress');
    var base_url = window.location.origin;
    self.VisibleRemark = function () {

        return localStorage.getItem('remark')
    };
    $('.companyName').text(companyName);
    $('.companyPhone').text(companyPhone);
    $('.companyEmail').text(companyEmail);
    $(".companyAddress").text(companyAddress);
    $(".companyImage").attr("src", base_url + companyLogo);
    $(".companyAddress").attr("src", companyAddress);




    getBranches();
    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Branches(data);
            });
    };

    function getReportItems() {
        Riddha.ajax.get("/Api/AttendanceReportApi")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ReportItemModel);
                self.AttendanceReports(data);
            });
    }

    function getPayrollreportItems() {
        Riddha.ajax.get("/Api/PayRollReportApi")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ReportItemModel);
                self.PayrollReports(data);
            });
    };

    function getMonths() {
        var monthArray = new Array();
        if (config.CurrentOperationDate == "ne") {
            if (config.CurrentLanguage == 'ne') {
                monthArray = ["बैशाख", "जेठ ", "असार", "श्रावण", "भदौ", "असोज", "कार्तिक", "मंसिर", "पुष", "माघ", "फाल्गुन", "चैत्र"];
            }
            else {
                monthArray = ["Baisakh", "Jestha", "Ashad", "Shrawan", "Bhadra", "Asoj", "Kartik", "Mangshir", "Poush", "Magh", "Falgun", "Chaitra"];
            }

        }
        else {
            if (config.CurrentLanguage == 'ne') {
                monthArray = ["जनवरी", "फेब्रुअरी", "मार्च", "अप्रिल", "मे", "जून", "जुलाई", "अगस्ट", "सेप्टेम्बर", "अक्टोबर", "नोभेम्बर", "डिसेम्बर"];
            }
            else {
                monthArray = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            }
        }
        for (var i = 0; i < 12; i++) {
            self.Months.push(new GlobalDropdownModel({ Id: i + 1, Name: monthArray[i] }));
        }
        self.MonthId(Riddha.util.getMonth(curDate));
        self.ToMonthId(Riddha.util.getMonth(curDate));
    }

    getMonths();

    self.ShowModal = function (id, title) {
        self.VisibleEndDate(false);
        self.OnDate(curDate);
        self.Report(title);
        self.ReportId(id);
        if (id == 16 || id == 10 || id == 21 || id == 22 || id == 36 || id == 37 || id == 38 || id == 39 || id == 40 ||
            id == 41 || id == 42 || id == 43 || id == 52 || id == 53 || id == 55 || id == 56 || id == 57 || id == 58 || id == 5012) {
            setDefDate();
            self.VisibleEndDate(true);
        }
        if (id == 13) {
            setDefDate();
            $("#MASModel").modal('show');
        }
        else {

            $("#reportModel").modal('show');
        }
    }

    self.AuditTrialShowModal = function (id, title) {
        self.VisibleEndDate(false);
        self.OnDate(curDate);
        self.Report(title);
        self.ReportId(id);
        if (id == 60) {
            setDefDate();
            self.VisibleEndDate(true);
        }
        $("#auditTrialReportModal").modal('show');
    };

    self.ShowMonthlyRosterModal = function (id, title) {
        self.VisibleEndDate(false);
        self.OnDate(curDate);
        self.Report(title);
        setDefDate();
        self.VisibleEndDate(true);
        $("#monthlyRosterModal").modal('show');
    }

    self.ShowMonthlyOtModal = function (id, title) {
        self.VisibleEndDate(false);
        self.OnDate(curDate);
        self.Report(title);
        setDefDate();
        self.VisibleEndDate(true);
        $("#monthlyOtModal").modal('show');
    }

    $("#reportModel").on('hidden.bs.modal', function () {
        //  location.href = "/report/attendancereport";
    });

    $("#MASModel").on('hidden.bs.modal', function () {
        //  location.href = "/report/attendancereport";
    });

    $("#monthlyRosterModal").on('hidden.bs.modal', function () {
        // location.href = "/report/attendancereport";
    });

    self.CloseModal = function () {
        $("#reportModel").modal('hide');
        //getReportItems();
        //attendanceReportInit();
    }

    self.AuditTrialCloseModal = function () {
        $("#auditTrialReportModal").modal('hide');
        //getReportItems();
        //attendanceReportInit();
    }

    self.RosterCloseModal = function () {
        $("#monthlyRosterModal").modal('hide');
    }

    self.MasCloseModal = function () {
        $("#MASModel").modal('hide');
    }

    //This function use for monthly attendace report single page per person 
    self.DateDiffrence = function () {

        var dateFirst = new Date(self.OnDate());
        var dateSecond = new Date(self.EndDate());
        // time difference
        var timeDiff = Math.abs(dateSecond.getTime() - dateFirst.getTime());
        // days difference
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        var one = 1;
        var tDay = diffDays + one;
        // difference
        self.PageSizeByDate(tDay)
    }
    var docHeight = $(window).height() - 0;

    self.KendoGridOptionsForDailyEmployeeAttendance = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var options = localStorage["column-options"];
        if (options) {
            options = JSON.parse(options);
        }
        return {

            title: getSuitableTitle(title, 0),
            target: "#kendoDailyEmpAttendanceReportWindow",
            url: "/Api/DailyEmployeeAttendanceReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            groupParam: { field: "DepartmentName" },
            group: false,
            pageSize: 50,
            columns: options || self.columSpec
        }
    }

    self.KendoGridOptionsForDailyEarlyIn = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoEarlyInReportWindow",
            url: "/Api/DailyEarlyInReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            //groupParam: language == "ne" ? { field: "DepartmentNameNp" } : { field: "DepartmentName" },
            groupParam: { field: "DepartmentName" },
            pageSize: 50,
            columns: self.columSpecForEarlyIn
        }
    }

    self.KendoGridOptionsForDailyEarlyOut = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoEarlyOutReportWindow",
            url: "/Api/DailyEarlyOutReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: { field: "DepartmentName" },
            pageSize: 50,
            columns: self.columSpec
        }
    }

    self.KendoGridOptionsForEmployeeAbsent = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoEmployeeAbesntReportWindow",
            url: "/Api/DailyEmployeeAbsentReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: { field: "DepartmentName" },
            pageSize: 50,
            columns: self.columSpecForAbsent
        }
    }

    self.KendoGridOptionsForDailyLateIn = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoLateInReportWindow",
            url: "/Api/DailyLateInReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: { field: "DepartmentName" },
            pageSize: 50,
            columns: self.columSpecForLateIn
        }
    }

    self.KendoGridOptionsForDailyLateOut = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoLateOutReportWindow",
            url: "/Api/DailyEmployeeLateOutReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: { field: "DepartmentName" },
            pageSize: 50,
            columns: self.columSpecForLateOut
        }
    }

    self.KendoGridOptionsForDailyMissingPunches = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoMissingPunchesReportWindow",
            url: "/Api/DailyMissingPunchesReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            pageSize: 50,
            columns: self.columSpec
        }
    }

    self.KendoGridOptionsForDailyLeave = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoDailyLeaveReportWindow",
            url: "/Api/DailyLeaveReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: { field: "DepartmentName" },
            pageSize: 50,
            columns: self.columSpec
        }
    }

    self.KendoGridOptionsForDailyOt = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoDailyOtReportWindow",
            url: "/Api/DailyOtReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: { field: "DepartmentName" },
            pageSize: 50,
            columns: self.columSpecForDailtOt
        }
    }



    self.KendoGridOptionsForMonthlyAttendance = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlyAttendanceReportWindow",
            url: "/Api/MonthlyAttendanceReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            groupParam: [
                {
                    field: "DepartmentName"
                },
                {
                    field: "EmployeeName",

                }],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: self.PageSizeByDate(),
            columns: self.columSpecForMonthly
        }
    };

    function getHeader(code, name) {
        return code + '-' + name;

    }

    self.KendoGridOptionsForMonthlySummaryDaywise = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var daywiseSummaryUrl = "/Api/MonthlyEmployeeSummaryReportApi/GenerateReport";
        var leavewiseSummaryUrl = "/Api/MonthlyEmployeeSummaryReportApi/GenerateReportLeaveWise";
        var options = self.IsLeaveWiseSummary() ? localStorage["summary-leave-column-options"] : localStorage["summary-column-options"];
        if (options) {
            options = JSON.parse(options);
        }
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlySummaryReportWindowDaywise",
            url: self.IsLeaveWiseSummary() ? leavewiseSummaryUrl : daywiseSummaryUrl,
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), Daywise: self.Daywise(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [{ field: "DepartmentName" }],
            pageSize: 50,
            columns: options || (self.IsLeaveWiseSummary() ? self.columSpecForMonthlySummaryWithLeaveDayWise() : self.columSpecForMonthlySummaryDayWise)
            //columns: self.IsLeaveWiseSummary() ? self.columSpecForMonthlySummaryWithLeaveDayWise() : self.columSpecForMonthlySummaryDayWise
            //columns: options || self.columSpecForMonthlySummaryDayWise
        }
    }



    self.KendoGridOptionsForMonthlySummaryHourwise = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlySummaryReportWindowHourwise",
            url: "/Api/MonthlyEmployeeSummaryReportApi/GenerateReportHourWise",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), Daywise: self.Daywise(), ActiveInactiveMode: self.ActiveInactiveMode(), OTV2: self.OTV2() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [{ field: "DepartmentName" }],
            pageSize: 50,
            columns: self.columSpecForMonthlySummaryHourWise
        }
    }



    self.KendoGridOptionsForMonthlyAttendanceStat = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            width: '100%',
            target: "#kendoMonthlyAttendanceStatisticWindow",
            url: "/Api/MonthlyAttendanceStatisticApi/GenerateReport",
            paramData: { Year: self.Year(), MonthId: self.MonthId(), DeptIds: departments, SectionIds: sections, EmpIds: employees, ActiveInactiveMode: self.ActiveInactiveMode(), IncludePunchTime: self.IncludePunchTime },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            //groupParam: [{ field: "DepartmentName" }],
            pageSize: 50,
            sortable: false,
            columns: getDynamicColumnspec
        }
    }

    self.KendoGridOptionsForMonthlyRosterRpt = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            width: '100%',
            target: "#kendoMonthlyRosterReportWindow",
            url: "/Api/MonthlyRosterReportApi/GenerateReport",
            paramData: { Year: self.Year(), MonthId: self.MonthId(), DeptIds: departments, SectionIds: sections, EmpIds: employees, ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            //groupParam: [{ field: "DepartmentName" }],
            pageSize: 50,
            sortable: false,
            columns: getMonthlyRosterColumnspec
        }
    }

    self.KendoGridOptionsForPayroll = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoPayrollReportWindow",
            url: "/Api/PayRollReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [{ field: "DepartmentName" }],
            pageSize: 50,
            columns: self.columSpecForPayroll
        }
    }

    self.KendoGridOptionsForMonthlyEarlyIn = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlyEarlyInReportWindow",
            url: "/Api/MonthlyEarlyInReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "DepartmentName" }],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForMonthlyEarlyIn
        }
    }

    self.KendoGridOptionsForMonthlyEarlyOut = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlyEarlyOutReportWindow",
            url: "/Api/MonthlyEarlyOutReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "DepartmentName" }],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForMonthlyEarlyOut
        }
    }

    self.KendoGridOptionsForMonthlyLateIn = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlylateInReportWindow",
            url: "/Api/MonthlyLateInReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "DepartmentName" }],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForMonthlyLateIn
        }
    }

    self.KendoGridOptionsForMonthlyLateOut = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlylateOutReportWindow",
            url: "/Api/MonthlyLateOutReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "DepartmentName" }],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForMonthlyLateOut
        }
    }

    self.KendoGridOptionsForMonthlyAbsent = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlyAbsentReportWindow",
            url: "/Api/MonthlyAbsentReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "DepartmentName" },
                {
                    field: "EmployeeName",

                }],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForMonthlyAbsent
        }
    }

    self.KendoGridOptionsForMonthlyMissingPunches = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlyMissingPunchesReportWindow",
            url: "/Api/MonthlyMissingPunchesReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "DepartmentName" },
                {
                    field: "EmployeeName",

                }],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForMonthlyMissingPunches
        }
    }

    self.KendoGridOptionsForMonthlyLeave = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlyLeaveReportWindow",
            url: "/Api/MonthlyLeaveReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "DepartmentName" },
            ],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForMonthlyLeave
        }
    }

    self.KendoGridOptionsForMonthlyOt = function (title) {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();

        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlyOtReportWindow",
            url: "/Api/MonthlyOtReportApi/GenerateReport",

            paramData: {
                DeptIds: departments, SectionIds: sections,
                EmpIds: employees, onDate: self.OnDate(),
                ToDate: self.EndDate(),
                ActiveInactiveMode: self.ActiveInactiveMode(),
                OTV2: self.OTV2()
            },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "DepartmentName" },
                { field: "EmployeeName" },
            ],
            sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForMonthlyOt
        }
    }

    self.KendoGridOptionsForMonthlyOtRpt = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            width: '100%',
            target: "#kendoMonthlyOtClaimReportWindow",
            url: "/Api/MonthlyOtReportApi/GenerateMonthlyOtReport",
            paramData: { Year: self.Year(), MonthId: self.MonthId(), DeptIds: departments, SectionIds: sections, EmpIds: employees, ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            //groupParam: [{ field: "DepartmentName" }],
            pageSize: 50,
            sortable: false,
            columns: getMonthlyOtColumnspec
        }
    }

    self.KendoGridOptionsForEmployeeAllowance = function (title) {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();

        return {
            title: getSuitableTitle(title),
            target: "#kendoAllowanceReportWindow",
            targetPivotGrid: "#kendoAllowanceReportWindow",
            url: "/Api/EmployeeAllowanceReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            modelFields: {
                EmployeeName: { type: "string" },
                Allowance: { type: "string" },
                AllowanceValue: { type: "number" },
                EngDate: { type: "string" }
            },
            cubeDimensions: {
                Allowance: { caption: "All allowance" },
                EmployeeName: { caption: "All Employee" }
            },
            measureField: "AllowanceValue",
            columns: [{ name: "Allowance", expand: true }],
            rows: [{ name: "EmployeeName", expand: true }, { name: "EngDate", expand: true }],
        }
    }

    self.KendoGridOptionsForDailyManualPunch = function (title) {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();

        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoManualPunchReportWindow",
            url: "/Api/DailyManualPunchReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: { field: "EmployeeName" },
            pageSize: 50,
            columns: self.columSpecForDailyManualPunch
        }
    }

    self.KendoGridOptionsForMonthlyManualPunch = function (title) {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();

        return {
            title: getSuitableTitle(title),
            target: "#kendoManualPunchReportWindow",
            url: "/Api/MonthlyManualPunchReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), toDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: { field: "EmployeeName" },
            pageSize: 50,
            columns: self.columSpecForMonthlyManualPunch
        }
    }

    self.KendoGridOptionsForMonthlyOfficeVisit = function (title) {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();

        return {
            title: getSuitableTitle(title),
            target: "#kendoOfficeVisitReportWindow",
            url: "/Api/MonthlyOfficeVisitReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), toDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            //groupParam: { field: "EmployeeName" },
            pageSize: 50,
            columns: self.columSpecForMonthlyOfficeVisit
        }
    }

    self.KendoGridOptionsForDailyOfficeVisit = function (title) {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();

        return {
            title: getSuitableTitle(title, 0),
            target: "#kendoOfficeVisitReportWindow",
            url: "/Api/MonthlyOfficeVisitReportApi/GenerateDailyReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            //groupParam: { field: "EmployeeName" },
            pageSize: 50,
            columns: self.columSpecForDailyOfficeVisit
        }
    }

    self.KendoGridOptionsForMonthlyMultiPunch = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoMonthlyAttendanceReportWindow",
            url: "/Api/MonthlyMultiPunchReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            //groupParam: [
            //    {
            //        field: "DepartmentName"
            //    },
            //    {
            //        field: "EmployeeName",

            //    }],
            pageSize: 50,
            columns: self.columSpecForMonthly
        }
    }

    self.KendoGridOptionsForAuditTrial = function (title) {
        return {
            title: getSuitableTitle(title),
            target: "#kendoAuditTrialReportWindow",
            url: "/Api/AuditTrialReportApi/GenerateReport",
            paramData: { OnDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            //groupParam: [
            //    {
            //        field: "DepartmentName"
            //    },
            //    {
            //        field: "EmployeeName",
            //    }],
            pageSize: 50,
            columns: self.columSpecForAuditTrial
        }
    }

    self.columSpecForAuditTrial = [
        { field: "Menu", title: lang == "ne" ? "Menu" : "Menu", filterable: false, sortable: false },
        { field: "Action", title: lang == "ne" ? "Action" : "Action", filterable: false, sortable: false },
        { field: "UserName", title: lang == "ne" ? "User" : "User", filterable: false, sortable: false },
        { field: "Date", title: lang == "ne" ? "मिति" : "Date", filterable: false, sortable: false, template: "#=SuitableDate(Date)#" },
        { field: "Message", title: lang == "ne" ? "Message" : "Message", filterable: false, sortable: false },
    ];

    self.columSpecForMonthlyMissingPunches = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "दिन" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false, template: alertTemplate("ActualTimeIn"), }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForMonthlyLeave = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "दिन" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForMonthlyAbsent = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "दिन" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForMonthlyLateOut = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "दिन" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false, template: alertTemplate("LateOut") }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForMonthlyLateIn = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "दिन" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateIn", title: language == "ne" ? "इन" : "In", template: alertTemplate("LateIn"), filterable: false, sortable: false }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForMonthlyEarlyOut = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "दिन" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false, template: alertTemplate("EarlyOut") }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForMonthlyEarlyIn = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "दिन" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false, template: alertTemplate("EarlyIn") }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpec = [
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "र्कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        //{ title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "ActualLunchIn", title: language == "ne" ? "ब्रेक इन" : "Break In", filterable: false, sortable: false }, { field: "ActualLunchOut", title: language == "ne" ? "ब्रेक आउट" : "Break Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false, template: hideZero("Actual") }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "ActualLunchOut", title: language == "ne" ? "ब्रेक आउट" : "Break Out", filterable: false, sortable: false }, { field: "ActualLunchIn", title: language == "ne" ? "ब्रेक इन" : "Break In", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false, template: hideZero("Actual") }] },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", template: alertTemplate("Ot"), filterable: false, sortable: false },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateIn", title: language == "ne" ? "इन" : "In", template: alertTemplate("LateIn"), filterable: false, sortable: false }, { field: "LateOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "EarlyOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false, template: alertTemplate("EarlyOut") }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];
    self.columSpecForEarlyIn = [
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "र्कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "ActualLunchIn", title: language == "ne" ? "ब्रेक इन" : "Break In", filterable: false, sortable: false }, { field: "ActualLunchOut", title: language == "ne" ? "ब्रेक आउट" : "Break Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false, template: hideZero("Actual") }] },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", template: alertTemplate("Ot"), filterable: false, sortable: false },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateIn", title: language == "ne" ? "इन" : "In", template: alertTemplate("LateIn"), filterable: false, sortable: false }, { field: "LateOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false, template: alertTemplate("EarlyIn") }, { field: "EarlyOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForLateIn = [
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "र्कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "ActualLunchIn", title: language == "ne" ? "ब्रेक इन" : "Break In", filterable: false, sortable: false }, { field: "ActualLunchOut", title: language == "ne" ? "ब्रेक आउट" : "Break Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false, template: hideZero("Actual") }] },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", filterable: false, sortable: false },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateIn", title: language == "ne" ? "इन" : "In", template: alertTemplate("LateIn"), filterable: false, sortable: false }, { field: "LateOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "EarlyOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false, template: alertTemplate("EarlyOut") }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForLateOut = [
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "र्कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "ActualLunchIn", title: language == "ne" ? "ब्रेक इन" : "Break In", filterable: false, sortable: false }, { field: "ActualLunchOut", title: language == "ne" ? "ब्रेक आउट" : "Break Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false, template: hideZero("Actual") }] },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", filterable: false, sortable: false },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "LateOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false, template: alertTemplate("LateOut") }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "EarlyOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForAbsent = [
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "र्कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "ActualLunchIn", title: language == "ne" ? "ब्रेक इन" : "Break In", filterable: false, sortable: false }, { field: "ActualLunchOut", title: language == "ne" ? "ब्रेक आउट" : "Break Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false, template: hideZero("Actual") }] },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", filterable: false, sortable: false },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "LateOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "EarlyOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false, template: alertTemplate("Remark") },
    ];

    function alertTemplate(field) {
        return " #if(" + field + "===''){#                            " +
            "    <span class='col-lg-12'>#= " + field + " #</span>       " +
            "#}else{#                            " +
            "    <span class='col-lg-12 bg-gray'>#= " + field + " #</span>       " +
            "#}# ";
    }

    function hideZero(field) {
        return " #if(" + field + "==='00:00'){#                            " +
            "    <span class='col-lg-12'></span>       " +
            "#}else{#                            " +
            "    <span class='col-lg-12'>#= " + field + " #</span>       " +
            "#}# ";
    }

    self.columSpecForMonthly = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "दिन" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "ActualLunchOut", title: language == "ne" ? "ब्रेक आउट" : "Break Out", filterable: false, sortable: false }, { field: "ActualLunchIn", title: language == "ne" ? "ब्रेक इन" : "Break In", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false, template: hideZero("Actual") }] },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", filterable: false, sortable: false, template: alertTemplate("Ot") },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false, template: alertTemplate("LateIn") }, { field: "LateOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "EarlyOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false, template: alertTemplate("EarlyOut") }] },
        { field: "Remark", title: language == "ne" ? "कैफियत" : "Remark", template: "#=SuitableRemarks(Remark)#", filterable: false, sortable: false },
    ];

    self.columSpecForMonthlySummaryHourWise = [
        {
            title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" },
            columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 },
            { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }]
        },
        { field: "ShiftWorkedTime", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false },
        { field: "Worked", title: language == "ne" ? "सही समय" : "Actual Time", filterable: false, sortable: false },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", filterable: false, sortable: false },
        { title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "LateIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "LateOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
        { title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "EarlyIn", title: language == "ne" ? "इन" : "In", filterable: false, sortable: false }, { field: "EarlyOut", title: language == "ne" ? "आउट" : "Out", filterable: false, sortable: false }] },
    ];

    self.columSpecForMonthlySummaryDayWise = [
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150, menu: false }] },
        { field: "TotalDays", title: language == "ne" ? "जम्मा दिन" : "Total Days", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "DutyDay", title: language == "ne" ? "कार्य दिन" : "Duty Days", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "Weekend", title: language == "ne" ? "सप्ताह दिन" : "Weekend (Days)", filterable: false, sortable: false, width: 75, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "Holiday", title: language == "ne" ? "सार्वजनिक बिदा(दिन)" : "Holiday (Days)", filterable: false, sortable: false, width: 80, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        //{ title: language == "ne" ? "" : "Present", headerAttributes: { style: "text-align:center;" }, columns: [
        { field: "Present", title: language == "ne" ? "उपस्थित(दिन)" : "Present (Days)", filterable: false, sortable: false, width: 60, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "PresentInHoliday", title: language == "ne" ? "उपस्थित(दिन)" : "Present On Holiday", filterable: false, sortable: false, width: 70, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "PresentInDayOff", title: language == "ne" ? "उपस्थित(दिन)" : "Present On DayOff", filterable: false, sortable: false, width: 70, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        //]},
        { field: "Absent", title: language == "ne" ? "अनुपस्थित(दिन)" : "Absent (Days)", filterable: false, sortable: false, width: 70, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "Misc", title: language == "ne" ? "विविध(दिन)" : "Misc (Days)", filterable: false, sortable: false, width: 60, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "Leave", title: language == "ne" ? "बिदा(दिन)" : "Leave (Days)", filterable: false, sortable: false, width: 70, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "Worked", title: language == "ne" ? "कार्य(घण्टा)" : "Worked (Hours)", filterable: false, sortable: false, width: 80, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT (Hours)", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        //{title: language == "ne" ? "ढिला" : "Late", headerAttributes: { style: "text-align:center;" }, columns: [
        { field: "LateIn", title: language == "ne" ? "इन" : "Late  In", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "LateOut", title: language == "ne" ? "आउट" : "Late Out", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        //]},
        //{title: language == "ne" ? "चाडो" : "Early", headerAttributes: { style: "text-align:center;" }, columns: [
        { field: "EarlyIn", title: language == "ne" ? "इन" : "Early  In", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "EarlyOut", title: language == "ne" ? "आउट" : "Early Out", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        //]},
        { field: "OfficeOut", title: language == "ne" ? "काजमा गएको(दिन)" : "Office Visit", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" } },
        { field: "Remarks", title: language == "ne" ? "टिप्पणी" : "Remarks", filterable: false, sortable: false },
    ];

    self.columSpecForDailyManualPunch = [
        {
            title: language == "ne" ? "कर्मचारी" : "Employee",
            columns: [
                { field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Employee Code", width: 200 },
                { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Employee Name", width: 300 }
            ]
        },
        {
            title: language == "ne" ? "म्यानुअल हाजिरी " : "Manual Punch",
            columns: [
                //{ field: "Date", title: language == "ne" ? "मिति" : "Date", template: "#=SuitableDate(Date)#", filterable: false, sortable: false },
                { field: "Time", title: language == "ne" ? "समय" : "Time", filterable: false, sortable: false, width: 200 },
                { field: "Remarks", title: language == "ne" ? "टिप्पणी" : "Remark", filterable: false, sortable: false },
            ]
        }
    ];

    self.columSpecForMonthlyManualPunch = [
        {
            title: language == "ne" ? "कर्मचारी" : "Employee",
            columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Employee Code", width: 200 },
            { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Employee Name", width: 300 }]
        },
        {
            title: language == "ne" ? "म्यानुअल हाजिरी" : "Manual Punch",
            columns: [
                { field: "Date", title: language == "ne" ? "मिति" : "Date", template: "#=SuitableDate(Date)#", filterable: false, sortable: false, width: 200 },
                { field: "Time", title: language == "ne" ? "समय" : "Time", filterable: false, sortable: false, width: 200 },
                { field: "Remarks", title: language == "ne" ? "टिप्पणी" : "Remark", filterable: false, sortable: false },
            ]
        }
    ]

    self.columSpecForMonthlyOfficeVisit = [
        {
            title: language == "ne" ? "कर्मचारी" : "Employee",
            columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Employee Code", width: 200 },
            { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Employee Name", width: 300 }]
        },
        {
            title: language == "ne" ? "काज" : "Office Visit",
            columns: [
                { field: "From", title: language == "ne" ? "मिति देखी" : "From", template: "#=SuitableDate(From)#", filterable: false, sortable: false, width: 200 },
                { field: "To", title: language == "ne" ? "मिति सम्म" : "To", template: "#=SuitableDate(To)#", filterable: false, sortable: false, width: 200 },
                { field: "Remark", title: language == "ne" ? "टिप्पणी" : "Remark", filterable: false, sortable: false },
            ]
        }
    ]

    self.columSpecForDailyOfficeVisit = [
        {
            title: language == "ne" ? "कर्मचारी" : "Employee",
            columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Employee Code", width: 200 },
            { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Employee Name", width: 300 }]
        },
        {
            title: language == "ne" ? "काज" : "Office Visit",
            columns: [
                { field: "From", title: language == "ne" ? "मिति देखी" : "From", template: "#=SuitableDate(From)#", filterable: false, sortable: false, width: 200 },
                { field: "To", title: language == "ne" ? "मिति सम्म" : "To", template: "#=SuitableDate(To)#", filterable: false, sortable: false, width: 200 },
                { field: "Remark", title: language == "ne" ? "टिप्पणी" : "Remark", filterable: false, sortable: false },
            ]
        }
    ]

    /* for summary report with leave wise count*/
    var leaveCodes = [];
    self.LeaveCodes = ko.observableArray([]);
    getValidLeaveCode();
    self.columSpecForMonthlySummaryWithLeaveDayWise = function () {
        var colums =
            [
                { field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 90 },
                { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150, menu: false },
                { field: "TotalDays", title: language == "ne" ? "जम्मा दिन" : "Total Days", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 40 },
                { field: "DutyDay", title: language == "ne" ? "कार्य दिन" : "Duty Days", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 40 },
                { field: "Weekend", title: language == "ne" ? "सप्ताह दिन" : "Weekend (Days)", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 70 },
                { field: "Holiday", title: language == "ne" ? "सार्वजनिक बिदा(दिन)" : "Holiday (Days)", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 70 },
                { field: "Present", title: language == "ne" ? "उपस्थित(दिन)" : "Present (Days)", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 60 },
                { field: "PresentInHoliday", title: language == "ne" ? "उपस्थित(दिन)" : "Present On Holiday", filterable: false, sortable: false, width: 70, headerAttributes: { style: "white-space: normal;text-align:center;" } },
                { field: "PresentInDayOff", title: language == "ne" ? "उपस्थित(दिन)" : "Present On DayOff", filterable: false, sortable: false, width: 70, headerAttributes: { style: "white-space: normal;text-align:center;" } },
                { field: "Absent", title: language == "ne" ? "अनुपस्थित(दिन)" : "Absent (Days)", filterable: false, sortable: false, width: 70, headerAttributes: { style: "white-space: normal;text-align:center;" } },
                { field: "Misc", title: language == "ne" ? "विविध(दिन)" : "Misc (Days)", filterable: false, sortable: false, width: 60, headerAttributes: { style: "white-space: normal;text-align:center;" } },
            ];
        for (var i = 0; i < leaveCodes.length; i++) {
            colums.push({ field: leaveCodes[i].Code, title: leaveCodes[i].Code, filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 45 });
        }
        var lastColumns = [{ field: "Worked", title: language == "ne" ? "कार्य(घण्टा)" : "Worked (Hours)", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 80 },
        { field: "OfficeOut", title: language == "ne" ? "काजमा गएको(दिन)" : "Office Visit", filterable: false, sortable: false, headerAttributes: { style: "white-space: normal;text-align:center;" }, width: 50 },
        { field: "Remarks", title: language == "ne" ? "टिप्पणी" : "Remarks", filterable: false, sortable: false, width: 80 }
        ];
        for (var i = 0; i < lastColumns.length; i++) {
            colums.push(lastColumns[i]);
        }
        return colums;
    };

    function getValidLeaveCode() {
        Riddha.ajax.get("/api/LeaveMasterApi")
            .done(function (result) {
                leaveCodes = result.Data;
                if (self.IsLeaveWiseSummary() == true) {
                    self.LeaveCodes(result.Data);
                }
            });
    }

    self.columSpecForPayroll = [
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "EmployeeCode", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "EmployeeName", width: 150 }] },
        {
            title: language == "ne" ? "Days" : "Days", headerAttributes: { style: "text-align:center;" },
            columns: [{ field: 'DaysWorked', title: language == "ne" ? "र्कोड" : "Worked", filterable: false, sortable: false },
            { field: 'HolidayCount', title: language == "ne" ? "र्कोड" : "Holiday", filterable: false, sortable: false },
            { field: 'WeekendCount', title: language == "ne" ? "र्कोड" : "Weekend", filterable: false, sortable: false },
            { field: 'LeavesCount', title: language == "ne" ? "र्कोड" : "Leaves", filterable: false, sortable: false },
            ]
        },
        {
            title: language == "ne" ? "ओ.टी" : "Earning", headerAttributes: { style: "text-align:center;" },
            columns: [{ field: 'BasicSalary', title: language == "ne" ? "र्कोड" : "Basic", filterable: false, sortable: false },
            { field: 'GrossSalary', title: language == "ne" ? "र्कोड" : "Gross", filterable: false, sortable: false },
            { field: 'GradeAmount', title: language == "ne" ? "र्कोड" : "Grade", filterable: false, sortable: false },

            ]
        },
        {
            title: language == "ne" ? "ओ.टी" : "OT", headerAttributes: { style: "text-align:center;" },
            columns: [{ field: 'Overtime', title: language == "ne" ? "र्कोड" : "Time(H)", filterable: false, sortable: false },
            { field: 'OvertimeAmount', title: language == "ne" ? "र्कोड" : "Amount", filterable: false, sortable: false },
            ]
        },


        {
            title: language == "ne" ? "ओ.टी" : "Deduction", headerAttributes: { style: "text-align:center;" },
            columns: [{ field: "ProvidendFund", title: language == "ne" ? "कार्य(घण्टा)" : "P F", filterable: false, sortable: false },
            { field: 'CIT', title: language == "ne" ? "र्कोड" : "CIT", filterable: false, sortable: false },
            { field: "TDS", title: language == "ne" ? "ओ.टी" : "TDS", filterable: false, sortable: false },
            { field: "LateDeduction", title: language == "ne" ? "ओ.टी" : "Late In", filterable: false, sortable: false },
            { field: "EarlyDeduction", title: language == "ne" ? "ओ.टी" : "Early Out", filterable: false, sortable: false },
            ]
        },
        { field: "Advance", title: language == "ne" ? "ओ.टी" : "Advance", filterable: false, sortable: false },
        { field: "Loan", title: language == "ne" ? "ओ.टी" : "OTLoan", filterable: false, sortable: false },
        { field: "NetSalary", title: language == "ne" ? "ओ.टी" : "NetSalary", filterable: false, sortable: false },


    ];

    self.columSpecForMonthlyOt = [
        { field: opDate == 'ne' ? 'NepDate' : 'WorkDate', title: language == "ne" ? "मिती" : "Date", format: "{0:yyyy/MM/dd}", filterable: false, sortable: true, template: "#=SuitableDate(WorkDate)#" },
        { field: 'DayName', title: language == "ne" ? "बार" : "Day", filterable: false, sortable: false },
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", template: alertTemplate("Ot"), filterable: false, sortable: false },
    ];

    self.columSpecForDailtOt = [
        { title: language == "ne" ? "कर्मचारी" : "Employee", headerAttributes: { style: "text-align:center;" }, columns: [{ field: 'EmployeeCode', title: language == "ne" ? "कोड" : "Code", width: 100 }, { field: 'EmployeeName', title: language == "ne" ? "नाम" : "Name", width: 150 }] },
        { title: language == "ne" ? "तय समय" : "Planned Time", headerAttributes: { style: "text-align:center;" }, columns: [{ field: "PlannedTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "PlannedTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Standard", title: language == "ne" ? "कार्य समय" : "Work Time", filterable: false, sortable: false, width: 100 }] },
        { title: language == "ne" ? "सही  समय" : "Actual Time", headerAttributes: { style: "text-align:center" }, columns: [{ field: "ActualTimeIn", title: language == "ne" ? "आउने" : "In", filterable: false, sortable: false }, { field: "ActualTimeOut", title: language == "ne" ? "जाने" : "Out", filterable: false, sortable: false }, { field: "Actual", title: language == "ne" ? "सही समय" : "Actual", filterable: false, sortable: false }] },
        { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", template: alertTemplate("Ot"), filterable: false, sortable: false },
    ];


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

        ko.utils.arrayForEach(self.Months(), function (item) {
            if (item.Id() == self.MonthId()) {
                self.MonthName(item.Name());
            }
            if (item.Id() == self.ToMonthId()) {
                self.ToMonthName(item.Name());
            }
        });

    }

    function getDynamicColumnspec() {
        var data = [];
        var title = "";
        var columns = [{
            field: "EmployeeName",
            locked: true,
            filterable: false,
            lockable: false,
            //title: "Emp Name",
            title: language == "ne" ? "कर्मचारीको नाम" : "Employee Name",
            width: 150
            //कर्मचारी नाम


        }];
        var fromMonth = self.OnDate();
        for (var i = 1; i <= self.dateLength(); i++) {
            var columnField = "Day" + Riddha.util.padLeft(i, 2);
            var template = "" +
                "#if(" + columnField + "==='A'){#                      " +
                "    <span class='badge bg-orange'>#= " + columnField + " #</span>     " +
                "#}else if(" + columnField + "==='P'){#                            " +
                "    <span class='badge bg-green'>#= " + columnField + " #</span>       " +
                "#}else if(" + columnField + "==='L'){#                            " +
                "    <span class='badge bg-blue'>#= " + columnField + " #</span>       " +
                "#}else if(" + columnField + "==='M'){#                            " +
                "    <span class='badge bg-aqua'>#= " + columnField + " #</span>       " +
                "#}else{#                            " +
                "    <span class='text-'><b>#= " + columnField + " #</b></span>       " +
                "#}#                               ";

            columns.push({
                title: language == "ne" ? GetNepaliUnicodeNumber(title + i) : (title + i), filterable: false, width: self.IncludePunchTime() ? 85 : 45,
                columns: [{
                    field: columnField, title: getDayName(fromMonth, i), filterable: false, width: self.IncludePunchTime() ? 85 : 45,
                    template: template
                }]
            });

        }
        return columns;
    }

    function getMonthlyOtColumnspec() {
        var data = [];
        var title = "";
        var columns = [
            {
                field: "EmployeeName",
                locked: true,
                filterable: true,
                lockable: false,
                title: language == "ne" ? "कर्मचारीको नाम" : "Employee Name",
                width: 200,
            }
        ];
        var fromMonth = self.OnDate();
        for (var i = 1; i <= self.dateLength(); i++) {
            var columnField = "Day" + Riddha.util.padLeft(i, 2);
            columns.push({
                title: title + i, filterable: false, width: 45,
                //columns: [
                //    { field: "Ot", title: language == "ne" ? "ओ.टी" : "OT", template: alertTemplate("Ot"), filterable: false, sortable: false },
                //]
                columns: [{
                    field: columnField, title: getDayName(fromMonth, i).substr(0, 3), filterable: false, width: 45,
                    template:
                        "<span class='#if(" + columnField + " !== \'-\') {# bg-gray #}#'>#=" + columnField + "#</span>"
                }]
            });
        }
        return columns;
    }

    function getMonthlyRosterColumnspec() {
        var data = [];
        var title = "";
        var columns = [
            {
                field: "EmployeeName",
                locked: true,
                filterable: true,
                lockable: false,
                title: language == "ne" ? "कर्मचारीको नाम" : "Employee Name",
                width: 200,

            }
        ];
        var fromMonth = self.OnDate();
        for (var i = 1; i <= self.dateLength(); i++) {
            var columnField = "Day" + Riddha.util.padLeft(i, 2);
            columns.push({
                title: language == "ne" ? GetNepaliUnicodeNumber(title + i) : (title + i), filterable: false, width: 45,
                columns: [{
                    field: columnField, title: getDayName(fromMonth, i).substr(0, 3), filterable: false, width: 45,
                    template:
                        "<span class='#if(" + columnField + " === \'OFF\' || " + columnField + " === \'PH\'|| " + columnField + " === \'L\' ) {# text-red #}#'>#=" + columnField + "#</span>"
                }]
            });
        }
        return columns;
    }

    function getDayName(dateString, i) {
        var days = language == "ne" ? ['आइत', 'सोम', 'मंगल', 'बुध', 'बिही', 'शुक्र', 'शनि'] : ['Sun', 'Mon', 'Tue', 'Wed', 'Thur', 'Fri', 'Sat'];
        var d = new Date(addDays(dateString, (i - 1)));
        var dayName = days[d.getDay()];
        return dayName;
    }

    function addDays(date, days) {
        var result = new Date(date);
        result.setDate(result.getDate() + days);
        return result;
    }

    self.BackToReportGrid = function () {
        self.ReportVisible(false);
    }

    //getDepartments();
    //function getDepartments() {
    //    Riddha.ajax.get("/Api/AttendanceReportApi/GetDepartments", null)
    //        .done(function (result) {
    //            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
    //            self.Departments(data);
    //            self.FilteredDepartment(data);
    //        });
    //};
    self.GetDepartments = function () {
        var branches = "";
        ko.utils.arrayForEach(self.Branches(), function (data) {
            if (data.Checked() == true) {
                if (branches.length != 0)
                    branches += "," + data.Id();
                else
                    branches = data.Id() + '';
            }
            else {
                self.Departments([]);
                self.FilteredDepartment([]);
            }
        });
        if (branches.length > 0) {
            Riddha.ajax.get("/Api/AttendanceReportApi/GetDepartmentsByBranch?branchIds=" + branches)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Departments(data);
                    self.FilteredDepartment(data);
                });
        }
    };

    self.SearchDepartmentText.subscribe(function (newValue) {
        if (newValue == '') {
            self.FilteredDepartment(self.Departments());
        } else {
            self.FilteredDepartment(Riddha.ko.global.filter(self.Departments, newValue));
        }
    });


    //Checkall Department
    self.CheckAllBranches.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Branches(), function (item) {
            item.Checked(newValue);
        });
        if (newValue) {
            self.GetDepartments();
        }
        else {
            self.Departments([]);
            self.FilteredDepartment([]);
            self.CheckAllDepartments(false);
        }
    });

    //self.CheckAllDepartments.subscribe(function (newValue) {
    //    ko.utils.arrayForEach(self.Departments(), function (item) {
    //        item.Checked(newValue);
    //    });
    //    if (newValue) {
    //        self.GetSections();
    //    }
    //    else {
    //        self.Sections([]);
    //        self.FilteredSection([]);
    //        self.CheckAllSections(false);
    //    }
    //});

    self.CheckAllDepartments.subscribe(function (newValue) {
        $.each(ko.toJS(self.Departments()), function (i, item) { self.CheckedDepartments.push(item.Id) })
        if (newValue) {
            self.GetSections();
        }
        else {
            self.CheckedDepartments([]);
            self.Sections([]);
            self.FilteredSection([]);
        }
        ko.utils.arrayForEach(self.Departments(), function (item) {
            item.Checked(newValue);

        });

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
        }
        ko.utils.arrayForEach(self.Sections(), function (item) {
            item.Checked(newValue);

        });

    });

    self.CheckAllEmployees.subscribe(function (newValue, e) {
        //ko.utils.arrayForEach(self.Employees(), function (item) {
        //    item.Checked(newValue);
        //});
    });


    //self.GetSections = function () {
    //    var departments = "";
    //    ko.utils.arrayForEach(self.Departments(), function (data) {
    //        if (data.Checked() == true) {
    //            if (departments.length != 0)
    //                departments += "," + data.Id();
    //            else
    //                departments = data.Id() + '';
    //        }
    //        else {
    //            self.Sections([]);
    //            self.FilteredSection([]);
    //        }
    //    });
    //    if (departments.length > 0) {
    //        Riddha.ajax.get("/Api/AttendanceReportApi/GetSectionsByDepartment/" + departments)
    //            .done(function (result) {
    //                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
    //                self.Sections(data);
    //                self.FilteredSection(data);
    //            });
    //    }
    //};

    self.GetSections = function () {

        if (self.CheckedDepartments().length > 0) {
            Riddha.ajax.get("/Api/AttendanceReportApi/GetSectionsByDepartment/" + self.CheckedDepartments().toString())
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Sections(data);
                    self.FilteredSection(data);
                });
        }
        else {
            self.FilteredSection([]);
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
            Riddha.ajax.get("/Api/AttendanceReportApi/GetEmployeeBySection?id=" + self.CheckedSections().toString() + "&activeInactiveMode=" + 0)
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



    self.ReportViewModel = {

        MonthId: ko.observable(),
        Year: ko.observable(),
        EmployeeIds: ko.observableArray([]),
        RosterRows: ko.observableArray([
        ])
    }

    self.Reset = function () {
        self.CheckAllSections(false);
        self.CheckAllDepartments(false);
        ko.utils.arrayForEach(self.Departments(), function (data) {
            data.Checked(false);
            self.Sections([]);
            self.Employees([]);
        });
    }

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
        if (employees.length == 0) {
            ko.utils.arrayForEach(self.Employees(), function (data) {
                if (employees.length != 0)
                    employees += "," + data.Id();
                else
                    employees = data.Id() + '';
            });
        }
        return employees;
    }

    //function getSelectedDepartments() {
    //    var deps = "";
    //    ko.utils.arrayForEach(self.Departments(), function (data) {
    //        if (data.Checked() == true) {
    //            if (deps.length != 0)
    //                deps += "," + data.Id();
    //            else
    //                deps = data.Id() + '';
    //        }
    //    });
    //    return deps;
    //}

    function getSelectedDepartments() {
        return self.CheckedDepartments().toString();
    }

    //function getSelectedSections() {
    //    var sections = "";
    //    ko.utils.arrayForEach(self.Sections(), function (data) {
    //        if (data.Checked() == true) {
    //            if (sections.length != 0)
    //                sections += "," + data.Id();
    //            else
    //                sections = data.Id() + '';
    //        }
    //    });
    //    return sections;
    //}

    function getSelectedSections() {
        return self.CheckedSections().toString();
    }

    function getSelectedBranches() {
        var branches = "";
        ko.utils.arrayForEach(self.Branches(), function (data) {
            if (data.Checked() == true) {
                if (branches.length != 0)
                    branches += "," + data.Id();
                else
                    branches = data.Id() + '';
            }
        });
        return branches;
    }

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
                return title + " from " + SuitableDate(self.OnDate()) + " to " + SuitableDate(self.EndDate()) + " " + self.MonthName();
        }
    };
    var togleDept = {};
    self.toggle = function (id) {

        switch (togleDept[id.DepartmentName()]) {
            case 0:
                $('[toggle-id="' + id.DepartmentName() + '"]').not(this).show();
                togleDept[id.DepartmentName()] = 1;
                break;

            default:
                $('[toggle-id="' + id.DepartmentName() + '"]').not(this).hide();
                togleDept[id.DepartmentName()] = 0;
                break;
        }

        //$('[toggle-id="RSC - Restaurant Support Centre"]').not(this).show();
    };
    self.Print = function (id) {
        //$("#orderConfirmationPrint").modal('show');
        var mywindow = window.open('', 'PRINT', '');

        mywindow.document.write('<html><head>');
        mywindow.document.write('<link rel="stylesheet" href="/bootstrap/css/bootstrap.min.css">');
        mywindow.document.write('<link rel="stylesheet" href="/dist/css/AdminLTE.min.css">');
        mywindow.document.write('<link href="~/Content/Site.css" rel="stylesheet" />');
        mywindow.document.write('');
        mywindow.document.write('</head><body >');

        mywindow.document.write($(id).html());
        mywindow.document.write('</body></html>');

        mywindow.document.close(); // necessary for IE >= 10
        mywindow.focus(); // necessary for IE >= 10*/

        setTimeout(function () {
            mywindow.print();
            mywindow.close();
            return true;
        }, 1000);

    }

    self.ExportExcel = function (id) {
        debugger;
        var tab_text = "<table border='2px'>";
        var textRange; var j = 0;
        tab = document.getElementById(id); // id of table
        var headerRowCount = $("#" + id + " thead tr").length;
        for (j = 0; j < tab.rows.length; j++) {
            if (j < headerRowCount) {
                tab_text += "<tr bgcolor='#87AFC6'>";
            }
            else {
                tab_text += "<tr>";
            }
            tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
            //tab_text=tab_text+"</tr>";
        }

        tab_text = tab_text + "</table>";
        tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
        /*tab_text = tab_text.replace(/<img[^>]*>/gi, "");*/ // remove if u want images in your table
        tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params


        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
        {
            txtArea1.document.open("txt/html", "replace");
            txtArea1.document.write(tab_text);
            txtArea1.document.close();
            txtArea1.focus();
            sa = txtArea1.document.execCommand("SaveAs", true, "Report.xls");
        }
        else                 //other browser not tested on IE 11
            sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

        return (sa);
    }

    self.Generate = function (title, type) {
        var branches = getSelectedBranches();
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var rptUrl = '';
        switch (title) {
            case 'MonthlyAttendancereport':
                rptUrl = '/Report/MonthlyWiseAttendanceRpt/GenerateReport';
                break;
            default:
                return;
        }

        window.open(rptUrl + '?' +
            'BranchIds=' + branches +
            '&DeptIds=' + departments +
            '&SectionIds=' + sections +
            '&EmpIds=' + employees +
            '&onDate=' + self.OnDate() +
            '&ToDate=' + self.EndDate() +
            '&OTV2=' + self.OTV2() +
            '&ActiveInactiveMode=' + self.ActiveInactiveMode() +
            '&type=' + type);

    };
    self.GenerateMultiPunch = function (title, type) {
        var branches = getSelectedBranches();
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var rptUrl = '';
        switch (title) {
            case 'MonthlyAttendancereport':
                rptUrl = '/Report/MonthlyWiseAttendanceRpt/GenerateMultiPunchReport';
                break;
            default:
                return;
        }
        window.open(rptUrl + '?' +
            'BranchIds=' + branches +
            '&DeptIds=' + departments +
            '&SectionIds=' + sections +
            '&EmpIds=' + employees +
            '&onDate=' + self.OnDate() +
            '&ToDate=' + self.EndDate() +
            '&OTV2=' + self.OTV2() +
            '&ActiveInactiveMode=' + self.ActiveInactiveMode() +
            '&type=' + type);

    };



    //HTML Table Report Functions Added Later
    self.HtmlReportForMonthlyAttendance = function (title) {
        var branches = getSelectedBranches();
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var reportTitle = getSuitableTitle(title);
        self.showLoading(true);
        Riddha.ajax.post("/Api/MonthlyAttendanceReportApi/GenerateReport", {
            BranchIds: branches, DeptIds: departments, SectionIds: sections, EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            OTV2: self.OTV2(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MOnthWiseEmployeeGroupReportModel);
            self.MonthlyAttendanceReportData(data);
            self.showLoading(false);
            function prepareSummary() {
                $('[data-result-col-ot]').each(function () {

                    var resultCol = $(this).data().resultColOt;
                    var sum = "00:00";
                    var tDays = "";
                    var Tsum = "00:00";
                    $('[data-sum-col-ot="' + resultCol + '"]').each(function () {
                        var d = $(this).text();
                        if (d == "") {
                            d = "00:00";
                        }
                        let t = sum; // hh:mm
                        let ms = Number(t.split(':')[0]) * 60 * 60 * 1000 + Number(t.split(':')[1]) * 60 * 1000;
                        let dms = Number(d.split(':')[0]) * 60 * 60 * 1000 + Number(d.split(':')[1]) * 60 * 1000;
                        var diff = ms + dms;

                        var days = Math.floor(diff / (1000 * 60 * 60 * 24));
                        diff -= days * (1000 * 60 * 60 * 24);

                        if (days != 0) {
                            tDays += days;
                        }

                        var hours = Math.floor(diff / (1000 * 60 * 60));
                        diff -= hours * (1000 * 60 * 60);

                        var mins = Math.floor(diff / (1000 * 60));
                        diff -= mins * (1000 * 60);

                        var seconds = Math.floor(diff / (1000));
                        diff -= seconds * (1000);
                        sum = hours + ":" + mins;
                        if (tDays != "") {
                            var daysInHours = tDays * 24;
                            var hInMinutes = daysInHours * 60;
                            var hoursIn = hInMinutes / 60;
                            Tsum = hoursIn + hours + ":" + mins;
                        }
                    });
                    if (Tsum != "" && Tsum != "00:00") {
                        $(this).text(Tsum);
                    }
                    else {
                        $(this).text(sum);
                    }

                });

                $('[data-result-col-LateIn]').each(function () {
                    var resultCol = $(this).data().resultColLatein;
                    var sum = "00:00";
                    var tDays = "";
                    var Tsum = "00:00";
                    $('[data-sum-col-LateIn="' + resultCol + '"]').each(function () {
                        var d = $(this).text();
                        if (d == "") {
                            d = "00:00";
                        }

                        let t = sum;
                        let ms = Number(t.split(':')[0]) * 60 * 60 * 1000 + Number(t.split(':')[1]) * 60 * 1000;
                        let dms = Number(d.split(':')[0]) * 60 * 60 * 1000 + Number(d.split(':')[1]) * 60 * 1000;
                        var diff = ms + dms;

                        var days = Math.floor(diff / (1000 * 60 * 60 * 24));
                        diff -= days * (1000 * 60 * 60 * 24);

                        if (days != 0) {
                            tDays += days;
                        }

                        var hours = Math.floor(diff / (1000 * 60 * 60));
                        diff -= hours * (1000 * 60 * 60);

                        var mins = Math.floor(diff / (1000 * 60));
                        diff -= mins * (1000 * 60);

                        var seconds = Math.floor(diff / (1000));
                        diff -= seconds * (1000);
                        sum = hours + ":" + mins;
                    });
                    if (Tsum != "" && Tsum != "00:00") {
                        $(this).text(Tsum);
                    }
                    else {
                        $(this).text(sum);
                    }
                });

                $('[data-result-col-LateOut]').each(function () {
                    var resultCol = $(this).data().resultColLateout;
                    var sum = "00:00";
                    var tDays = "";
                    var Tsum = "00:00";
                    $('[data-sum-col-Lateout="' + resultCol + '"]').each(function () {
                        var d = $(this).text();
                        if (d == "") {
                            d = "00:00";
                        }

                        let t = sum;
                        let ms = Number(t.split(':')[0]) * 60 * 60 * 1000 + Number(t.split(':')[1]) * 60 * 1000;
                        let dms = Number(d.split(':')[0]) * 60 * 60 * 1000 + Number(d.split(':')[1]) * 60 * 1000;
                        var diff = ms + dms;

                        var days = Math.floor(diff / (1000 * 60 * 60 * 24));
                        diff -= days * (1000 * 60 * 60 * 24);

                        if (days != 0) {
                            tDays += days;
                        }

                        var hours = Math.floor(diff / (1000 * 60 * 60));
                        diff -= hours * (1000 * 60 * 60);

                        var mins = Math.floor(diff / (1000 * 60));
                        diff -= mins * (1000 * 60);

                        var seconds = Math.floor(diff / (1000));
                        diff -= seconds * (1000);
                        sum = hours + ":" + mins;
                    });
                    if (Tsum != "" && Tsum != "00:00") {
                        $(this).text(Tsum);
                    }
                    else {
                        $(this).text(sum);
                    }
                });

                $('[data-result-col-EarlyIn]').each(function () {
                    var resultCol = $(this).data().resultColEarlyin;
                    var sum = "00:00";
                    var tDays = "";
                    var Tsum = "00:00";
                    $('[data-sum-col-EarlyIn="' + resultCol + '"]').each(function () {
                        var d = $(this).text();
                        if (d == "") {
                            d = "00:00";
                        }

                        let t = sum;
                        let ms = Number(t.split(':')[0]) * 60 * 60 * 1000 + Number(t.split(':')[1]) * 60 * 1000;
                        let dms = Number(d.split(':')[0]) * 60 * 60 * 1000 + Number(d.split(':')[1]) * 60 * 1000;
                        var diff = ms + dms;

                        var days = Math.floor(diff / (1000 * 60 * 60 * 24));
                        diff -= days * (1000 * 60 * 60 * 24);

                        if (days != 0) {
                            tDays += days;
                        }

                        var hours = Math.floor(diff / (1000 * 60 * 60));
                        diff -= hours * (1000 * 60 * 60);

                        var mins = Math.floor(diff / (1000 * 60));
                        diff -= mins * (1000 * 60);

                        var seconds = Math.floor(diff / (1000));
                        diff -= seconds * (1000);
                        sum = hours + ":" + mins;
                    });
                    if (Tsum != "" && Tsum != "00:00") {
                        $(this).text(Tsum);
                    }
                    else {
                        $(this).text(sum);
                    }
                });

                $('[data-result-col-EarlyOut]').each(function () {
                    var resultCol = $(this).data().resultColEarlyout;
                    var sum = "00:00";
                    var tDays = "";
                    var Tsum = "00:00";
                    $('[data-sum-col-EarlyOut="' + resultCol + '"]').each(function () {
                        var d = $(this).text();
                        if (d == "") {
                            d = "00:00";
                        }

                        let t = sum;
                        let ms = Number(t.split(':')[0]) * 60 * 60 * 1000 + Number(t.split(':')[1]) * 60 * 1000;
                        let dms = Number(d.split(':')[0]) * 60 * 60 * 1000 + Number(d.split(':')[1]) * 60 * 1000;
                        var diff = ms + dms;

                        var days = Math.floor(diff / (1000 * 60 * 60 * 24));
                        diff -= days * (1000 * 60 * 60 * 24);

                        if (days != 0) {
                            tDays += days;
                        }

                        var hours = Math.floor(diff / (1000 * 60 * 60));
                        diff -= hours * (1000 * 60 * 60);

                        var mins = Math.floor(diff / (1000 * 60));
                        diff -= mins * (1000 * 60);

                        var seconds = Math.floor(diff / (1000));
                        diff -= seconds * (1000);
                        sum = hours + ":" + mins;
                    });
                    if (Tsum != "" && Tsum != "00:00") {
                        $(this).text(Tsum);
                    }
                    else {
                        $(this).text(sum);
                    }
                });
            }
            prepareSummary();
        });
        $("#monthlyAttendanceReportTitle").text(reportTitle);
        $("#monthlyAttendanceReportModal").modal('show');

    }

    self.HtmlReportForMonthlyMultiPunchReport = function (title) {
        var branches = getSelectedBranches();
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var reportTitle = getSuitableTitle(title);
        self.showLoading(true);
        Riddha.ajax.post("/Api/EmployeeMultiPunchReportApi/GenerateReport", {
            BranchIds: branches,
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            OTV2: self.OTV2(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyMultiPunchReportModel);


            self.MonthlyMultiPunchReportData(data);
            Riddha.UI.groupArray(self.MonthlyMultiPunchReportDataGroupBy, { key: 'Name', list: self.Employees }, { key: 'Name', list: self.MonthlyMultiPunchReportData }, MonthlyMultiPunchReportModel, [])
            $("#multiPunchReportTitle").text(reportTitle);
            $("#multiPunchReportModal").modal('show');
        });

    };

    self.reportVisibility = ko.observable(getStorageValue);
    function getStorageValue() {
        return JSON.parse(localStorage.reportVisibility);
    };
    self.GenerateEmployeeSummaryReport = function () {
        $("#viewEmployeeSummaryReport").modal('show');
        self.showLoading(true);
        self.LeaveCodes([]);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var daywiseSummaryUrl = "/Api/MonthlyEmployeeSummaryReportApi/GenerateReport";
        var leavewiseSummaryUrl = "/Api/MonthlyEmployeeSummaryReportApi/GenerateReportLeaveWise";
        var options = self.IsLeaveWiseSummary() ? localStorage["summary-leave-column-options"] : localStorage["summary-column-options"];
        if (options) {
            options = JSON.parse(options);
        }
        var targetUrl = self.IsLeaveWiseSummary() ? leavewiseSummaryUrl : daywiseSummaryUrl;
        if (self.IsLeaveWiseSummary()) {
            Riddha.ajax.post(leavewiseSummaryUrl, {
                DeptIds: departments,
                SectionIds: sections,
                EmpIds: employees,
                onDate: self.OnDate(),
                ToDate: self.EndDate(),
                Daywise: self.Daywise(),
                OTV2: self.OTV2(),
                ActiveInactiveMode: self.ActiveInactiveMode()
            }).done(function (result) {
                self.showLoading(false);
                self.EmployeeSummaryData(result.Data);
                getValidLeaveCode();
                Riddha.UI.groupArrayDynimic(self.EmployeeSummaryDataGroupBy, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.EmployeeSummaryData }, EmployeeSummaryModel, ['LateIn', 'EarlyOut', 'EarlyIn', 'LateOut'])
                $('#ReportTitle').text("Employee Summary Report Day Wise from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
                $("#viewEmployeeSummaryReport").modal('show');
            });
        }
        else {
            Riddha.ajax.post(daywiseSummaryUrl, {
                DeptIds: departments,
                SectionIds: sections,
                EmpIds: employees,
                onDate: self.OnDate(),
                ToDate: self.EndDate(),
                Daywise: self.Daywise(),
                OTV2: self.OTV2(),
                ActiveInactiveMode: self.ActiveInactiveMode()
            }).done(function (result) {
                self.showLoading(false);
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmployeeSummaryModel);
                self.EmployeeSummaryData(data);
                Riddha.UI.groupArray(self.EmployeeSummaryDataGroupBy, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.EmployeeSummaryData }, EmployeeSummaryModel, ['LateIn', 'EarlyOut', 'EarlyIn', 'LateOut'])
                $('#dynCol').attr('colspan', 18);
                $('#ReportTitle').text("Employee Summary Report Day Wise from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
                $("#viewEmployeeSummaryReport").modal('show');
            });
        }
    }

    self.GenerateEmployeesSummaryHourwiseReport = function () {
        $("#viewEmployeeHourWiseSummaryReport").modal('show');
        self.showLoading(true);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        Riddha.ajax.post("/Api/MonthlyEmployeeSummaryReportApi/GenerateReportHourWise", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            Daywise: self.Daywise(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmployeeSummaryModel);
            self.EmployeeSummaryHourWiseData(data);
            Riddha.UI.groupArray(self.EmployeeSummaryHourWiseDataGroupBy, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.EmployeeSummaryHourWiseData }, EmployeeSummaryModel, [])
            $('.ReportTitle').text("Employee Summary Report Hour Wise from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $('#viewEmployeeHourWiseSummaryReport').modal('show');
        });
    }

    self.GenerateMonthlyEarlyInReport = function (title) {
        $("#viewMonthlyInReport").modal('show');
        self.showLoading(true);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var reportTitle = getSuitableTitle(title);
        Riddha.ajax.post("/Api/MonthlyEarlyInReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyEarlyInModel);
            self.MonthlyEarlyInData(data);
            Riddha.UI.groupArray(self.MonthlyEarlyInDataGroupBy, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.MonthlyEarlyInData }, MonthlyEarlyInModel, [])

            $('.ReportTitle').text("Monthly Early In Report From " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $("#viewMonthlyInReport").modal('show');
        });
    }

    self.GenerateMonthlyEarlyOutReport = function (title) {
        $("#viewMonthlyOutReport").modal('show');
        self.showLoading(true);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        Riddha.ajax.post("/Api/MonthlyEarlyOutReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyEarlyInModel);
            self.MonthlyEarlyOutData(data);
            Riddha.UI.groupArray(self.MonthlyEarlyOutDataGroupBy, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.MonthlyEarlyOutData }, MonthlyEarlyInModel, [])
            $('.ReportTitle').text("Monthly Early Out Report from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $("#viewMonthlyOutReport").modal('show');
        });
    }

    self.GenerateMonthlyLateInReport = function () {
        $("#viewMonthlyLateInReport").modal('show');
        self.showLoading(true);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        Riddha.ajax.post("/Api/MonthlyLateInReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyEarlyInModel);
            self.MonthlyLateInData(data);
            Riddha.UI.groupArray(self.MonthlyLateInDataGroupBy, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.MonthlyLateInData }, MonthlyEarlyInModel, []);
            $('.ReportTitle').text("Monthly Late In Report from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $("#viewMonthlyLateInReport").modal('show');

        })
    }

    self.GenerateMonthlyLateOutReport = function () {
        $("#viewMonthlyLateOutReport").modal('show');
        self.showLoading(true);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        Riddha.ajax.post("/Api/MonthlyLateOutReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyEarlyInModel);
            self.MonthlyLateOutData(data);
            Riddha.UI.groupArray(self.MonthlyLateOutDataGroupBy, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.MonthlyLateOutData }, MonthlyEarlyInModel, []);
            $('.ReportTitle').text("Monthly Late Out Report from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $("#viewMonthlyLateOutReport").modal('show');
        })
    }

    self.MonthlyAbsentDataGroupByNew = ko.observableArray([]);

    self.GenerateMonthlyAbsentReport = function () {
        $("#viewMonthlyAbsentReport").modal('show');
        self.showLoading(true);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        getAllEmployees();
        Riddha.ajax.post("/Api/MonthlyAbsentReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyEarlyInModel);
            self.MonthlyAbsentData(data);
            var RootArray = ko.observableArray([]);

            ///first level grouping 
            Riddha.UI.groupArray(RootArray, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.MonthlyAbsentData }, MonthlyEarlyInModel, []);
            //next level grouping detail wise
            // out of first level would be like {header: {},detail :[]}//observables
            //loop for each details from first level out put

            ko.utils.arrayForEach(RootArray(), function (item, i) {
                var outArray = ko.observableArray([]);
                // same logic to group and sum for next levels
                Riddha.UI.groupArray(outArray, { key: 'Name', list: self.AllEmployees }, { key: 'EmployeeName', list: item.detail }, MonthlyEarlyInModel, [])
                item.detail(outArray());
            });
            self.MonthlyAbsentDataGroupBy(RootArray());
            $('.ReportTitle').text("Monthly Absent Report from " + self.OnDate() + " To " + self.EndDate());
            $("#viewMonthlyAbsentReport").modal('show');
        })
    }

    self.GenerateMonthlyMissingPunchesReport = function () {
        $("#viewMonthlyMissingPunchReport").modal('show');
        self.showLoading(true);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        getAllEmployees();
        Riddha.ajax.post("/Api/MonthlyMissingPunchesReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyEarlyInModel);
            self.MonthlyMissingPunchData(data);
            var RootArray = ko.observableArray([]);
            Riddha.UI.groupArray(RootArray, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.MonthlyMissingPunchData }, MonthlyEarlyInModel, []);
            ko.utils.arrayForEach(RootArray(), function (item, i) {
                var outArray = ko.observableArray([]);
                Riddha.UI.groupArray(outArray, { key: 'Name', list: self.AllEmployees }, { key: 'EmployeeName', list: item.detail }, MonthlyEarlyInModel, []);
                item.detail(outArray());
            });
            self.MonthlyMissingPunchDataGroupBy(RootArray());
            $('.ReportTitle').text("Monthly Missing Punches Report from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $("#viewMonthlyMissingPunchReport").modal('show');
        })
    }

    self.GenerateMonthlyOTReport = function () {
        $("#viewMonthlyOTReport").modal('show');
        self.showLoading(true);
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();
        getAllEmployees();
        Riddha.ajax.post("/Api/MonthlyOtReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode(),
            OTV2: self.OTV2()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyEarlyInModel);
            self.MonthlyOTData(data);
            var RootArray = ko.observableArray([]);
            Riddha.UI.groupArray(RootArray, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.MonthlyOTData }, MonthlyEarlyInModel, []);
            ko.utils.arrayForEach(RootArray(), function (item, i) {
                var outArray = ko.observableArray([]);
                Riddha.UI.groupArray(outArray, { key: 'Name', list: self.AllEmployees }, { key: 'EmployeeName', list: item.detail }, MonthlyEarlyInModel, []);
                item.detail(outArray());
            });
            self.MonthlyOTDataGroupBy(RootArray());
            $('.ReportTitle').text("Monthly OT Report from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $("#viewMonthlyOTReport").modal('show');
        })
    }
    function getAllEmployees() {
        Riddha.ajax.get("/Api/AttendanceReportApi/GetAllEmployees").done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
            self.AllEmployees(data);
        })
    };

    self.GenerateMonthlyLeaveReport = function () {
        $("#viewMonthlyLeaveReport").modal('show');
        self.showLoading(true);
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        Riddha.ajax.post("/Api/MonthlyLeaveReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            ToDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            self.showLoading(false);
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlyEarlyInModel);
            self.MonthlyLeaveData(data);
            Riddha.UI.groupArray(self.MonthlyLeaveDataGroupBy, { key: 'Name', list: self.Departments }, { key: 'DepartmentNamee', list: self.MonthlyLeaveData }, MonthlyEarlyInModel, []);
            $('.ReportTitle').text("Monthly Leave Report from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $("#viewMonthlyLeaveReport").modal('show');
        })
    }

    self.GenerateMonthlyManualPunchReport = function () {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();
        getAllEmployees();
        Riddha.ajax.post("/Api/MonthlyManualPunchReportApi/GenerateReport", {
            DeptIds: departments,
            SectionIds: sections,
            EmpIds: employees,
            onDate: self.OnDate(),
            toDate: self.EndDate(),
            ActiveInactiveMode: self.ActiveInactiveMode()
        }).done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ManualPunchReportModel);
            self.MonthlyManualPunchData(data);
            Riddha.UI.groupArray(self.MonthlyManualPunchDataGroupBy, { key: 'Name', list: self.AllEmployees }, { key: 'EmployeeName', list: self.MonthlyManualPunchData }, ManualPunchReportModel, []);
            $('.ReportTitle').text("Monthly Manual Punch Report from " + SuitableDate(self.OnDate()) + " To " + SuitableDate(self.EndDate()));
            $("#viewMonthlyManualPunchReport").modal('show');
        })
    }


    /* Region Employee Self-Service Report*/

    self.KendoGridOptionsForEmployeeLateInEarlyOut = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoEmployeeLateInEarlyOutReportWindow",
            url: "/Api/EmployeeLateInEarlyOutReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            groupParam: [
                {
                    field: "Department"
                },
                {
                    field: "Employee",

                }],
            pageSize: self.PageSizeByDate(),
            columns: self.columSpecForLateInEarlyOut
        }
    };

    self.columSpecForLateInEarlyOut = [
        { field: "RequestedDate", title: lang == "ne" ? "मिति" : "Request Date", filterable: false, sortable: false, template: "#=SuitableDate(RequestedDate)#" },
        { field: "LateInEarlyOutRequestType", title: lang == "ne" ? "RequestType" : "RequestType", filterable: false, sortable: false, template: "#=GetLateInEarlyOutRequestType(LateInEarlyOutRequestType)#" },
        { field: "PlannedInTime", title: lang == "ne" ? "PlannedIn" : "PlannedIn", filterable: false, sortable: false },
        { field: "ActualInTime", title: lang == "ne" ? "ActualIn" : "ActualIn", filterable: false, sortable: false },
        { field: "LateInTime", title: lang == "ne" ? "LateIn" : "LateIn", filterable: false, sortable: false },
        { field: "PlannedOutTime", title: lang == "ne" ? "PlannedOut" : "PlannedOut", filterable: false, sortable: false },
        { field: "ActualOutTime", title: lang == "ne" ? "ActualOut" : "ActualOut", filterable: false, sortable: false },
        { field: "EarlyOutTime", title: lang == "ne" ? "EarlyOut" : "EarlyOut", filterable: false, sortable: false },
        { field: "Remark", title: lang == "ne" ? "Remark" : "Remark", filterable: false, sortable: false },
        { field: "ApproveDate", title: lang == "ne" ? "मिति" : "Approve Date", filterable: false, sortable: false, template: "#=SuitableDate(ApproveDate)#" },
        { field: "ApproveBy", title: lang == "ne" ? "ApproveBy" : "ApproveBy", filterable: false, sortable: false },
    ];

    self.KendoGridOptionsForEmployeePunchRequest = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoEmployeePunchRequestReportWindow",
            url: "/Api/EmployeePunchRequestReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            groupParam: [
                {
                    field: "Department"
                },
                {
                    field: "Employee",

                }],
            pageSize: self.PageSizeByDate(),
            columns: self.columSpecForManualPunchRequest
        }
    }

    self.columSpecForManualPunchRequest = [
        { field: "RequestDate", title: lang == "ne" ? "मिति" : "Request Date", filterable: false, sortable: false, template: "#=SuitableDate(RequestDate)#" },
        { field: "Remark", title: lang == "ne" ? "Remark" : "Remark", filterable: false, sortable: false },
        { field: "ApproveDate", title: lang == "ne" ? "मिति" : "Approve Date", filterable: false, sortable: false, template: "#=SuitableDate(ApproveDate)#" },
        { field: "ApproveBy", title: lang == "ne" ? "ApproveBy" : "Approve By", filterable: false, sortable: false },
    ];



    self.KendoGridOptionsForEmployeeOfficeVisitRequest = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: getSuitableTitle(title),
            target: "#kendoEmployeeOfficeVisitRequestReportWindow",
            url: "/Api/EmployeeOfficeVisitRequestReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate(), ActiveInactiveMode: self.ActiveInactiveMode() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            groupParam: [
                {
                    field: "Department"
                },
                {
                    field: "Employee",

                }],
            pageSize: self.PageSizeByDate(),
            columns: self.columSpecForOfficeVisitRequest
        }
    }

    self.columSpecForOfficeVisitRequest = [
        { field: "FromDate", title: lang == "ne" ? "मिति देखि" : "From Date", filterable: false, sortable: false, template: "#=SuitableDate(FromDate)#" },
        { field: "ToDate", title: lang == "ne" ? "मिति सम्म" : "To Date", filterable: false, sortable: false, template: "#=SuitableDate(ToDate)#" },
        { field: "Remark", title: lang == "ne" ? "Remark" : "Remark", filterable: false, sortable: false },
        { field: "ApproveDate", title: lang == "ne" ? "मिति" : "Approved Date", filterable: false, sortable: false, template: "#=SuitableDate(ApproveDate)#" },
        { field: "Approveby", title: lang == "ne" ? "द्वारा स्वीकृत" : "Approved By", filterable: false, sortable: false },
    ];

    /*
     * Payroll Report Section 
     * Insurance Report , Provident Fund Report , CIT Report
     */
    self.PayrollReportTitle = ko.observable("");
    self.PayrollInsuranceList = ko.observableArray([]);
    self.PayrollProvidentFundList = ko.observableArray([]);
    self.PayrollCITList = ko.observableArray([]);

    var url_Payroll = "/Api/PayRollReportApi";
    GetFiscalYears();
    function GetFiscalYears() {
        Riddha.ajax.get(url_Payroll + "/GetFiscalYears").done(function (result) {
            if (result.Status == 4) {
                var data = Riddha.ko.global.arrayMap(result.Data, FiscalYearDropdownVm);
                self.FiscalYears(data);
                GetCurrentFiscalYear();
            }
        });
    }


    function GetCurrentFiscalYear() {
        ko.utils.arrayForEach(self.FiscalYears(), function (item) {
            if (item.CurrentFiscalYear() == true) {
                self.CurrentFiscalYearName(item.Name());
            }
        });
    }

    self.ShowPayrollReportModal = function (id, title) {
        self.PayrollReportTitle(title);
        self.ReportId(id);
        $("#ReportParamModal").modal('show');
    }

    self.ShowSalaryHistoryReportModal = function (id, title) {
        self.PayrollReportTitle(title);
        self.ReportId(id);
        $("#salaryHistoryReportParamModal").modal('show');
    }

    self.KendoGridOptionsForSalaryHistoryReport = function (title) {

        var employees = getSelectedEmps();
        return {
            title: getSuitableTitle(title),
            target: "#kendoEmployeeSalaryHistoryReportWindow",
            url: "/Api/PayRollReportApi/GenerateSalaryHistory",
            paramData: { FiscalYearId: self.FiscalYearId(), EmpIds: employees },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            hidePdf: true,
            group: false,
            groupParam: [
                {
                    field: "Employee",

                }],
            pageSize: self.PageSizeByDate(),
            columns: self.columSpecForSalaryHistory
        }
    }

    self.columSpecForSalaryHistory = [
        { field: "Employee", title: lang == "ne" ? "Employee" : "Employee", filterable: false, sortable: false },
        { field: "Department", title: lang == "ne" ? "Department" : "Department", filterable: false, sortable: false },
        { field: "Section", title: lang == "ne" ? "Section" : "Section", filterable: false, sortable: false },
        { field: "Designation", title: lang == "ne" ? "Designation" : "Designation", filterable: false, sortable: false },
        { field: "GradeName", title: lang == "ne" ? "Grade Name" : "Grade Name", filterable: false, sortable: false },
        { field: "GradeValue", title: lang == "ne" ? "Grade Value" : "Grade Value", filterable: false, sortable: false },
        { field: "BasicSalary", title: lang == "ne" ? "BasicSalary" : "BasicSalary", filterable: false, sortable: false },
        { field: "GrossSalary", title: lang == "ne" ? "GrossSalary" : "GrossSalary", filterable: false, sortable: false },
        { field: "TaxableAmont", title: lang == "ne" ? "TaxableAmont" : "TaxableAmont", filterable: false, sortable: false },
        { field: "SocialSecurityTax", title: lang == "ne" ? "SocialSecurityTax" : "SocialSecurityTax", filterable: false, sortable: false },
        { field: "RenumerationTax", title: lang == "ne" ? "RenumerationTax" : "RenumerationTax", filterable: false, sortable: false },
        { field: "PFEmployee", title: lang == "ne" ? "PFEmployee" : "PFEmployee", filterable: false, sortable: false },
        { field: "PFEmployeer", title: lang == "ne" ? "PFEmployeer" : "PFEmployeer", filterable: false, sortable: false },
        { field: "Gratituity", title: lang == "ne" ? "Gratituity" : "Gratituity", filterable: false, sortable: false },
        { field: "SSEmployee", title: lang == "ne" ? "SSEmployee" : "SSEmployee", filterable: false, sortable: false },
        { field: "SSEmployeer", title: lang == "ne" ? "SSEmployeer" : "SSEmployeer", filterable: false, sortable: false },
        { field: "InsurancePremiumAmount", title: lang == "ne" ? "InsurancePremiumAmount" : "InsurancePremiumAmount", filterable: false, sortable: false },
        { field: "CITAmount", title: lang == "ne" ? "CITAmount" : "CITAmount", filterable: false, sortable: false },
        { field: "Absent", title: lang == "ne" ? "Absent" : "Absent", filterable: false, sortable: false },
        { field: "Leave", title: lang == "ne" ? "Leave" : "Leave", filterable: false, sortable: false },
        { field: "LateIn", title: lang == "ne" ? "LateIn" : "LateIn", filterable: false, sortable: false },
        { field: "EarlyOut", title: lang == "ne" ? "EarlyOut" : "EarlyOut", filterable: false, sortable: false },
        { field: "DeductionAmount", title: lang == "ne" ? "DeductionAmount" : "DeductionAmount", filterable: false, sortable: false },
        { field: "RebateAmount", title: lang == "ne" ? "RebateAmount" : "RebateAmount", filterable: false, sortable: false },
        { field: "NetSalary", title: lang == "ne" ? "NetSalary" : "NetSalary", filterable: false, sortable: false },
        { field: "PensionFundEmployee", title: lang == "ne" ? "PensionFundEmployee" : "PensionFundEmployee", filterable: false, sortable: false },
        { field: "PensionFundEmployeer", title: lang == "ne" ? "PensionFundEmployeer" : "PensionFundEmployeer", filterable: false, sortable: false },
        { field: "InsurancePaidbyOffice", title: lang == "ne" ? "InsurancePaidbyOffice" : "InsurancePaidbyOffice", filterable: false, sortable: false },
        { field: "AdditionAmount", title: lang == "ne" ? "AdditionAmount" : "AdditionAmount", filterable: false, sortable: false },
    ];


    self.TotalEmployeeContributionAmount = ko.observable(0);
    self.TotalEmployerContributionAmount = ko.observable(0);
    self.TotalDeductionAmount = ko.observable(0);
    self.GenerateInsuranceReport = function () {
        if (language == "ne") {
            self.Report("सावधिक जीवन विमा कोष कट्टी फारम");
        }
        else {
            self.Report("Employee Insurance Report from " + self.MonthName());
        }

        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();

        var data = {
            EmpIds: employees,
            DeptIds: departments,
            SectionIds: sections,
            OnDate: self.OnDate(),
            ToDate: self.EndDate(),
            MonthId: self.MonthId(),
            ToMonthId: self.ToMonthId(),
            FiscalYearId: self.FiscalYearId()
        };
        Riddha.ajax.post(url_Payroll + "/GenerateInsuranceReport", data).done(function (result) {
            if (result.Status == 4) {
                debugger;
                var data = Riddha.ko.global.arrayMap(result.Data, InsuanceReportModel);
                self.PayrollInsuranceList(data);

                var totalEmployeeContributionAmount = 0;
                var totalEmployerContributionAmount = 0;
                var totalDeductionAmount = 0;
                ko.utils.arrayForEach(self.PayrollInsuranceList(), function (item) {
                    totalEmployeeContributionAmount += Number(item.EmployeeContributionAmount());
                    totalEmployerContributionAmount += Number(item.EmployerContributionAmount());
                    totalDeductionAmount += Number(item.TotalDeduction());
                });

                self.TotalEmployeeContributionAmount(totalEmployeeContributionAmount);
                self.TotalEmployerContributionAmount(totalEmployerContributionAmount);
                self.TotalDeductionAmount(totalDeductionAmount);
                $("#InsuranceReportModal").modal('show');
            }
        });

    }

    self.TotalEmployeeContributionAmount = function () {
        var total = 0;
        ko.utils.arrayForEach(self.PayrollInsuranceList(), function (item) {
            total += Number(item.EmployeeContributionAmount());
        });
        return total;
    }


    self.GenerateProvidentFundReport = function () {

        self.ResetPFParam();
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();

        var data = {
            EmpIds: employees,
            DeptIds: departments,
            SectionIds: sections,
            OnDate: self.OnDate(),
            ToDate: self.EndDate(),
            MonthId: self.MonthId(),
            ToMonthId: self.ToMonthId(),
            FiscalYearId: self.FiscalYearId()
        };
        Riddha.ajax.post(url_Payroll + "/GenerateProvidentFundReport", data).done(function (result) {
            if (result.Status == 4) {
                var data = Riddha.ko.global.arrayMap(result.Data, ProvidentFundReportModel);
                self.PayrollProvidentFundList(data);
                CalculateTotalPFDeduction();

                $("#ProvidentFundReportModal").modal('show');
            }
        });
    }

    self.TotalPFDeduction = ko.observable(0);
    self.TotalPFEmployee = ko.observable(0);
    self.TotalPFEmployer = ko.observable(0);

    self.CompanyCodeNo = ko.observable('');
    self.IssuedToBankName = ko.observable('');
    self.FundDeduction = ko.observable('');
    self.IssuedDate = ko.observable(curDate);
    self.IssuedAmount = ko.observable(0);

    function CalculateTotalPFDeduction() {

        var TotalPFDeduction = 0;
        var TotalPFEmployee = 0;
        var TotalPFEmployer = 0;

        ko.utils.arrayForEach(self.PayrollProvidentFundList(), function (data) {

            TotalPFDeduction = TotalPFDeduction + data.TotalDeduction();
            TotalPFEmployee = TotalPFEmployee + data.EmployeeContributionAmount();
            TotalPFEmployer = TotalPFEmployer + data.EmployerContributionAmount();
        });
        self.TotalPFDeduction(TotalPFDeduction);
        self.TotalPFEmployee(TotalPFEmployee);
        self.TotalPFEmployer(TotalPFEmployer);
        self.IssuedAmount(self.TotalPFDeduction());
    }



    self.ResetPFParam = function () {
        self.CompanyCodeNo('');
        self.IssuedToBankName('');
        self.FundDeduction('');
        self.IssuedDate('');
        self.IssuedAmount(0);
    }

    self.ShowPFReportParamModal = function () {
        $("#PFReportParamModal").modal('show');
    }

    self.HidePFReportParamModal = function () {
        $("#PFReportParamModal").modal('hide');
    }




    self.CITTotalAmount = ko.observable(0);
    self.GenerateCITReport = function () {
        if (language == "ne") {
            self.Report("नागरिक लगानी कोष कट्टी फारम");
        }
        else {
            self.Report("Employee CIT Report from " + self.MonthName());
        }
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();

        var data = {
            EmpIds: employees,
            DeptIds: departments,
            SectionIds: sections,
            OnDate: self.OnDate(),
            ToDate: self.EndDate(),
            MonthId: self.MonthId(),
            ToMonthId: self.ToMonthId(),
            FiscalYearId: self.FiscalYearId()
        };
        Riddha.ajax.post(url_Payroll + "/GenerateCITReport", data).done(function (result) {
            if (result.Status == 4) {
                var data = Riddha.ko.global.arrayMap(result.Data, CITReportModel);
                var CITTotalAmount = 0;
                self.PayrollCITList(data);
                ko.utils.arrayForEach(self.PayrollCITList(), function (item) {
                    CITTotalAmount = CITTotalAmount + item.TotalDeduction();
                });
                self.CITTotalAmount(CITTotalAmount);
                $("#CITReportModal").modal('show');

            }
        });

    }

}

function GetLateInEarlyOutRequestType(name) {
    if (name == 'LateIn') {
        return "<span class='badge bg-aqua'>" + name + "</span>";
    }
    else {
        return "<span class='badge bg-orange'>" + name + "</span>";
    }
};