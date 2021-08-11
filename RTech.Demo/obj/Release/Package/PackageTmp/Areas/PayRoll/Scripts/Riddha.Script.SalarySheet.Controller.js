
/// <reference path="../../../scripts/knockout-2.3.0.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="riddha.script.salarysheet.model.js" />


function MonthlySalarySheetController() {
    var self = this;
    var url = "/Api/SalarySheetApi";
    self.OrganizationTypes = ko.observableArray([

        { Id: 0, Name: 'Non-Government' },
        { Id: 1, Name: 'Government' }
    ]);
    self.OrganizationType = ko.observable(1);
    self.MonthlySalarySheet = ko.observable(new MonthlySalarySheetModel());
    self.MonthlySalarySheets = ko.observableArray([]);
    self.AllowanceHeads = ko.observable([]);
    self.Departments = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.DepartmentId = ko.observable(0);
    self.EmployeeId = ko.observable(0);
    self.FilteredEmployees = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.Sections = ko.observableArray([]);
    var config = new Riddha.config();
    var curDate = config.CurDate;
    self.Months = ko.observableArray([]);

    self.FiscalYear = ko.observable("");
    self.OnDate = ko.observable('').extend({ Date: 'yyyy/MM/dd' });
    self.EndDate = ko.observable('').extend({ Date: 'yyyy/MM/dd' });
    self.MonthId = ko.observable(Riddha.util.getMonth(curDate));
    self.Year = ko.observable(Riddha.util.getYear(curDate));

    self.HeaderHeight = function () {

        //debugger;
        //try {

        //    var element = $("#salarySheetTbl th");
        //    return element[0].offsetHeight;
        //} catch (e) {

        //    return 103;
        //}
        var height = $("#GradeTitle")[0].offsetHeight;
        return height;
    } 

        
    
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
        //self.PageSizeByDate(tDay)
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
    }



    function getSelectedEmps() {
        var employees = "";
        ko.utils.arrayForEach(self.FilteredEmployees(), function (data) {
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
            { Id: 5, Name: config.CurrentLanguage == 'ne' ? "भदौ" : "Bhadra" },
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


    //GetMonthlySalarySheet();
    function GetMonthlySalarySheet() {


        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();
        debugger;
        Riddha.ajax.get(url + "/GetMonthlySalarySheets?OnDate=" + ko.toJS(self.OnDate()) + "&ToDate=" + ko.toJS(self.EndDate())
            + "&DepartmentIds=" + departments + "&SectionIds=" + sections + "&EmpIds=" + employees + "&MonthId=" + self.MonthId())
            .done(function (result) {

                if (result.Status == 4) {

                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthlySalarySheetModel);
                    self.MonthlySalarySheets(data);
                    var allowanceHeads = Riddha.ko.global.arrayMap(ko.toJS(result.Data[0].Allowances), AllowanceModel);
                    self.AllowanceHeads(allowanceHeads);
                    if (result.Message != "") {
                        Riddha.UI.Toast(result.Message);
                    }
                }
                else {

                    $("#ErrorMsg").text(result.Message);
                    $("#ValidationModal").modal('show');
                }
             
            });
    }


    getDepartments();
    function getDepartments() {
        Riddha.ajax.get(url + "/GetDepartments", null)
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
            Riddha.ajax.get("/Api/SectionApi/GetSectionsByDepartment/" + departments)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Sections(data);
                });
        }
    };
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
    self.SearchItemText = ko.observable('');
    self.SearchItemText.subscribe(function (newValue) {
        if (newValue == '') {
            self.FilteredEmployees(self.Employees());
        } else {
            self.FilteredEmployees(Riddha.ko.global.filter(self.Employees, newValue));
        }
    })


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
                self.FilteredEmployees([]);
            }
        });
        if (sections.length > 0) {
            Riddha.ajax.get("/Api/AttendanceReportApi/GetEmployeeBySection?id=" + sections + "&activeInactiveMode=" + 0)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Employees(data);
                    self.FilteredEmployees(data);
                });
        }
    };





    self.Refresh = function () {
        GetMonthlySalarySheet();

    }

    self.MonthlySalaryPost = function () {

        Riddha.UI.Confirm("Are you sure you want to post this Salary?", function () {
            Riddha.ajax.post(url, {
                MonthId: self.MonthId(),
                MonthlySalarySheet: ko.toJS(self.MonthlySalarySheets()),
                FromDate :  self.OnDate(),
                EndDate: self.EndDate()

            }).done(function (result) {

                if (result.Status == 4) {
                    self.Refresh();
                }
                Riddha.UI.Toast(result.Message, result.Status)
            });

        });

    }

    GetFiscalYear();
    function GetFiscalYear() {
        Riddha.ajax.get(url + "/GetCurrentFiscalYear").done(function (result) {
            if (result.Status == 4) {
                self.FiscalYear(result.Data);
            }
        });

    }

    GetHeaderHeight();
    function GetHeaderHeight() {

        var element = $("#salarySheetTbl th");
        self.HeaderHeight(element[0].offsetHeight);
    }


}