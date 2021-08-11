/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
function hrReportController(date, CompanyName) {
    var self = this;
    var config = new Riddha.config();
    var language = config.CurrentLanguage;
    var curDate = config.CurDate;
    self.Report = ko.observable();
    self.ReportId = ko.observable(0);
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.Courses = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.CheckAllCourses = ko.observable(false);
    self.OnDate = ko.observable(date).extend({ date: 'yyyy/MM/dd' });
    self.EndDate = ko.observable(date).extend({ date: 'yyyy/MM/dd' });
    self.Months = ko.observableArray([]);
    self.MonthId = ko.observable(0);
    self.Year = ko.observable(Riddha.util.getYear(curDate));
    self.VisibleEndDate = ko.observable(false);
    self.VisibleOnDate = ko.observable(false);
    self.VisibleNormal = ko.observable(false);
    self.VisibleDepartmentsOnly = ko.observable(false);
    self.VisibleCourses = ko.observable(false);

    var docHeight = $(document).height();

    getMonths();

    getDepartments();

    self.KendoGridOptionsForEmployeeContract = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoEmployeeContractReportWindow",
            url: "/Api/EmployeeContractReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            pageSize: 50,
            columns: self.columnSpecForEmployeeContractReport

        }
    }

    self.KendoGridOptionsForEmployeeResignation = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoEmployeeResignationReportWindow",
            url: "/Api/EmployeeResignationReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            //groupParam: [
            //    {
            //        field: "EmployeeName",

            //    }],
            //sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columnSpecForEmployeeResignationReport
        }
    }

    self.KendoGridOptionsForEmployeeTermination = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoEmployeeTerminationReportWindow",
            url: "/Api/EmployeeTerminationReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            //groupParam: [
            //    {
            //        field: "EmployeeName",

            //    }],
            //sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columnSpecForEmployeeTerminationReport
        }
    }

    self.KendoGridOptionsForEmployeePersonalInformation = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: '#kendoEmployeePersonalInformationWindow',
            url: "/Api/EmployeePersonalInformationApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            //groupParam: [
            //    {
            //        field: "EmployeeName",

            //    }],
            //sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columnSpecForEmployeePersonalInformation
        }
    }

    self.KendoGridOptionsForEmployeeExperience = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoEmployeeExperienceReportWindow",
            url: "/Api/EmployeeExperienceReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            groupParam: [
                {
                    field: "EmployeeName",

                }],
            pageSize: 50,
            columns: self.columnSpecForEmployeeExperienceReport
        }
    }

    self.KendoGridOptionsForEmployeeQualification = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoEmployeeQualificationReportWindow",
            url: "/Api/EmployeeQualificationReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, onDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            groupParam: [
                { field: "EmployeeName" },
                {
                    field: "Type",

                }],
            pageSize: 50,
            columns: self.columnSpecForEmployeeQualificationReport
        }
    }

    self.KendoGridOptionsForTrainingCourse = function (title) {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoTrainingCourseReportWindow",
            url: "/Api/TrainingCourseReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            groupParam: [
                { field: "DepartmentName" }
            ],
            pageSize: 50,
            columns: self.columnSpecForTrainingCoursesReport
        }
    }

    self.KendoGridOptionsForTrainingSession = function (title) {
        var departments = getSelectedDepartments();
        var courses = getSelectedCourses();
        return {
            title: title,
            target: "#kendoTrainingSessionReportWindow",
            url: "/Api/TrainingSessionReportApi/GenerateReport",
            paramData: { DeptIds: departments, CourseIds: courses },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            groupParam: [
                { field: "CourseName" }
            ],
            pageSize: 50,
            columns: self.columnSpecForTrainingSessionsReport
        }
    }

    self.KendoGridOptionsForTrainingParticipation = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoTrainingParticipationReportWindow",
            url: "/Api/TrainingParticipationReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            groupParam: [
                { field: "CourseName" }
            ],
            pageSize: 50,
            columns: self.columnSpecForTrainingParticipationReport
        }
    }

    self.KendoGridOptionsForDisciplinaryCaseCaseWise = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoCasewiseDisciplinaryCaseReportWindow",
            url: "/Api/CasewiseDisciplinaryCaseReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, OnDate: self.OnDate(), EndDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            groupParam: [
                { field: "CaseName" }
            ],
            pageSize: 50,
            columns: self.columnSpecForCasewiseDisciplinaryCaseReport
        }
    }

    self.KendoGridOptionsForDisciplinaryCaseEmployeewise = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoEmployeewiseDisciplinaryCaseReportWindow",
            url: "/Api/EmployeewiseDisciplinaryCaseReportApi/GenerateReport",
            paramData: { DeptIds: departments, SectionIds: sections, EmpIds: employees, OnDate: self.OnDate(), ToDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            groupParam: [
                { field: "EmployeeName" }
            ],
            pageSize: 50,
            columns: self.columnSpecForEmployeewiseDisciplinaryCaseReport
        }
    }

    self.KendoGridOptionsForPromotionHistory = function (title) {
        var employees = getSelectedEmps();
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        return {
            title: title,
            target: "#kendoEmployeePromotionHistoryReportWindow",
            url: "/Api/EmployeePromotionHistoryReportApi/GenerateReport",
            paramData: { EmployeeIds: employees },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            groupParam: [
                { field: "EmployeeName" }
            ],
            pageSize: 50,
            columns: self.columnSpecForEmployeePromotionHistReport
        }
    }

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
            Riddha.ajax.get("/Api/AttendanceReportApi/GetEmployeeBySection?id=" + sections + "&activeInactiveMode=0")
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Employees(data);
                });
        }
    };

    self.GetCourses = function () {
        var departments = "";
        ko.utils.arrayForEach(self.Departments(), function (data) {
            if (data.Checked() == true) {
                if (departments.length != 0)
                    departments += "," + data.Id();
                else
                    departments = data.Id() + '';
            }
            else {
                self.Courses([]);
            }
        });
        if (departments.length > 0) {
            Riddha.ajax.get("/Api/TrainingSessionReportApi/GetCoursesByDepartment?id=" + departments)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Courses(data);
                });

        }
    }

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

    function getDepartments() {
        Riddha.ajax.get("/Api/AttendanceReportApi/GetDepartments", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Departments(data);
            });
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

    function getSelectedCourses() {
        var courses = "";
        ko.utils.arrayForEach(self.Courses(), function (data) {
            if (data.Checked() == true) {
                if (courses.length != 0)
                    courses += "," + data.Id();
                else
                    courses = data.Id() + '';
            }
        });
        return courses;
    }

    self.CheckAllDepartments.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Departments(), function (item) {
            item.Checked(newValue);
        });
        if (newValue) {
            self.GetSections();
            self.GetCourses();
        }
        else {
            self.Sections([]);
            self.Courses([]);
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
        //ko.utils.arrayForEach(self.Employees(), function (item) {
        //    item.Checked(newValue);
        //});
    });

    self.CheckAllCourses.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Courses(), function (item) {
            item.Checked(newValue);
        });
    });

    self.ShowModal = function (id, title) {
        self.VisibleEndDate(false);
        self.VisibleOnDate(false);
        self.Report(title);
        self.ReportId(id);
        if (id == 46) {
            self.VisibleDepartmentsOnly(true);
            self.VisibleNormal(false);
            self.VisibleCourses(false);
            $("#HrReportModal").modal('show');
        }
        else if (id == 47) {
            self.VisibleCourses(true);
            self.VisibleNormal(false);
            self.VisibleDepartmentsOnly(false);
            $("#HrReportModal").modal('show');
        }
        else {
            self.VisibleNormal(true);
            self.VisibleDepartmentsOnly(false);
            self.VisibleCourses(false);
            $("#HrReportModal").modal('show');
        }
    }

    self.CloseModal = function () {
        self.Reset();
        $("#HrReportModal").modal('hide');
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

    self.columnSpecForEmployeeContractReport = [
        {
            field: "EmployeeName", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" },

        },
        {
            field: "ContractCode", title: language == "ne" ? "अनुबंध कोड" : "Contract Code", headerAttributes: { style: "text-align:center;" },

        },
        {
            field: "BeganOn", title: language == "ne" ? "सुरु गरिएको" : "Began On", template: "#=SuitableDate(BeganOn)#", filterable: false, headerAttributes: { style: "text-align:center;" },

        },
        {
            field: "EndedOn", title: language == "ne" ? "अन्त्य गरिएको" : "Ended On", template: "#=SuitableDate(EndedOn)#", filterable: false, headerAttributes: { style: "text-align:center;" },

        },
        {
            field: "EmploymentStatus", title: language == "ne" ? "कर्मचारी स्थिति" : "Employee Status", headerAttributes: { style: "text-align:center;" },

        },
        {
            field: "ApprovedBy", title: language == "ne" ? "स्विकृत गर्ने " : "Approved By", headerAttributes: { style: "text-align:center;" },

        }
    ];

    self.columnSpecForEmployeeResignationReport = [
        {
            field: "EmployeeName", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "ResignCode", title: language == "ne" ? "राजिनामा कोड" : "Resign Code", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "NoticeDate", title: language == "ne" ? "सूचना मिति" : "Notice Date", template: "#=SuitableDate(NoticeDate)#", filterable: false, headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "DesiredResignDate", title: language == "ne" ? "इच्छित राजिनामा" : "Desired Resign Date", template: "#=SuitableDate(DesiredResignDate)#", filterable: false, headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Reason", title: language == "ne" ? "कारण" : "Reason", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Details", title: language == "ne" ? "विवरणहरू" : "Details", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "ApprovedBy", title: language == "ne" ? "स्विकृत गर्ने " : "Approved By", headerAttributes: { style: "text-align:center;" },

        }
    ];
    self.columnSpecForEmployeeTerminationReport = [
        {
            field: "EmployeeName", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "TerminationCode", title: language == "ne" ? "बर्खास्त कोड" : "Termination Code", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "NoticeDate", title: language == "ne" ? "सूचना मिति" : "Notice Date", template: "#=SuitableDate(NoticeDate)#", filterable: false, headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "EndDate", title: language == "ne" ? "सेवा अन्त्य मिति" : "Service End Date", template: "#=SuitableDate(EndDate)#", filterable: false, headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Reason", title: language == "ne" ? "कारण" : "Reason", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Details", title: language == "ne" ? "विवरणहरू" : "Details", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "ChangeStatus", title: language == "ne" ? "स्थिति परिवर्तन" : "Change Status", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "ApprovedBy", title: language == "ne" ? "स्विकृत गर्ने " : "Approved By", headerAttributes: { style: "text-align:center;" },

        }
    ];

    self.columnSpecForEmployeePersonalInformation = [
        {
            field: "EmployeeName", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Designation", title: language == "ne" ? "पद" : "Designation", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Department", title: language == "ne" ? "विभाग" : "Department", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Section", title: language == "ne" ? "फाँट" : "Section", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "GradeGroup", title: language == "ne" ? "ग्रेड समूह" : "Grade Group", headerAttributes: { style: "text-align:center;" },
        },
        //{
        //    field: "EmployeeDeviceCode", title: language == "ne" ? "कर्मचारी उपकरण कोड" : "Employee Device Code", headerAttributes: { style: "text-align:center;" },
        //},
        {
            field: "JoinDate", title: language == "ne" ? "सुरु गरेको  मिति" : "Join Date", template: "#=SuitableDate(JoinDate)#", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "PhoneNo", title: language == "ne" ? "कर्मचारी उपकरण कोड" : "Phone No.", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Address", title: language == "ne" ? "कर्मचारी उपकरण कोड" : "Address", headerAttributes: { style: "text-align:center;" },
        },
    ];

    self.columnSpecForEmployeeExperienceReport = [
        {
            field: "EmployeeName", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Department", title: language == "ne" ? "विभाग" : "Department", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Code", title: language == "ne" ? "अनुभव कोड" : "Experience Code", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Title", title: language == "ne" ? "शीर्षक" : "Title", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Organization", title: language == "ne" ? "संगठन नाम" : "Organization Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "BeganOn", title: language == "ne" ? "सुरु गरिएको" : "Began On", template: "#=SuitableDate(BeganOn)#", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "EndedOn", title: language == "ne" ? "अन्त्य गरिएको" : "Ended On", template: "#=SuitableDate(EndedOn)#", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Description", title: language == "ne" ? "वर्णन" : "Description", headerAttributes: { style: "text-align:center;" },
        },
        //{
        //    field: "ApprovedBy", title: language == "ne" ? "स्विकृत गर्ने" : "Approved By", headerAttributes: { style: "text-align:center;" },
        //},
    ];

    self.columnSpecForEmployeeQualificationReport = [
        {
            field: "EmployeeName", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Name", title: language == "ne" ? "नाम" : "Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Description", title: language == "ne" ? "वर्णन" : "Description", headerAttributes: { style: "text-align:center;" },
        },
        //{
        //    field: "ApprovedBy", title: language == "ne" ? "स्विकृत गर्ने" : "Approved By", headerAttributes: { style: "text-align:center;" },
        //},
    ];

    self.columnSpecForTrainingCoursesReport = [
        {
            field: "CourseTitle", title: language == "ne" ? "कर्मचारी नाम" : "Course Title", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "DepartmentName", title: language == "ne" ? "विभाग" : "Department", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Coordinator", title: language == "ne" ? "संयोजक" : "Coordinator", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Version", title: language == "ne" ? "संस्करण" : "Version", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Cost", title: language == "ne" ? "लागत" : "Cost", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Description", title: language == "ne" ? "वर्णन" : "Description", headerAttributes: { style: "text-align:center;" },
        },
    ];

    self.columnSpecForTrainingSessionsReport = [
        {
            field: "SessionName", title: language == "ne" ? "सत्र" : "Session", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "DepartmentName", title: language == "ne" ? "विभाग" : "Department", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Duration", title: language == "ne" ? "विधि" : "Duration", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Method", title: language == "ne" ? "अवधि" : "Method", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Location", title: language == "ne" ? "स्थान" : "Location", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Description", title: language == "ne" ? "वर्णन" : "Description", headerAttributes: { style: "text-align:center;" },
        },
    ];

    self.columnSpecForTrainingParticipationReport = [
        {
            field: "EmployeeName", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "CourseName", title: language == "ne" ? "पाठ्यक्रम" : "Course", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "SessionName", title: language == "ne" ? "सत्र" : "Session", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "StartDate", title: language == "ne" ? "सुरू मिति" : "Start Date", template: "#=SuitableDate(StartDate)#", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "EndDate", title: language == "ne" ? "अन्त्य मिति" : "End Date", template: "#=SuitableDate(EndDate)#", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "ParticipantStatus", title: language == "ne" ? "सहभागी स्थिति" : "Participant Status", headerAttributes: { style: "text-align:center;" },
        },
    ];

    self.columnSpecForCasewiseDisciplinaryCaseReport = [
        {
            field: "EmployeeName", title: language == "ne" ? "कर्मचारी नाम" : "Employee Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Description", title: language == "ne" ? "वर्णन" : "Description", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Actions", title: language == "ne" ? "कार्य" : "Actions", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Status", title: language == "ne" ? "स्थिति" : "Status", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "ForwardTo", title: language == "ne" ? "फर्वार्ड गरिएको" : "Forwarded To", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "CreatedOn", title: language == "ne" ? "बनाएको मिति" : "Created On", template: "#=SuitableDate(CreatedOn)#", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "CreatedBy", title: language == "ne" ? "बनाउने" : "Created By", headerAttributes: { style: "text-align:center;" },
        },
    ];

    self.columnSpecForEmployeewiseDisciplinaryCaseReport = [
        {
            field: "CaseName", title: language == "ne" ? "मुद्दा नाम" : "Case Name", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Description", title: language == "ne" ? "वर्णन" : "Description", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Actions", title: language == "ne" ? "कार्य" : "Action", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Status", title: language == "ne" ? "स्थिति" : "Status", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "CreatedOn", title: language == "ne" ? "बनाएको मिति" : "Created On", template: "#=SuitableDate(CreatedOn)#", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "CreatedBy", title: language == "ne" ? "बनाउने" : "Created By", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "ForwardTo", title: language == "ne" ? "फर्वार्ड गरिएको" : "Forwarded To", headerAttributes: { style: "text-align:center;" },
        },
    ];

    self.columnSpecForEmployeePromotionHistReport = [
        {
            field: "EmployeeName", title: language == "ne" ? "Employee" : "Employee", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Department", title: language == "ne" ? "Department" : "Department", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "Section", title: language == "ne" ? "कार्य" : "Section", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "DesignationName", title: language == "ne" ? "Designation" : "Designation", headerAttributes: { style: "text-align:center;" },
        },
        {
            field: "AddedDate", title: language == "ne" ? "बनाएको मिति" : "Created On", template: "#=SuitableDate(AddedDate)#", headerAttributes: { style: "text-align:center;" },
        },
    ];

}
