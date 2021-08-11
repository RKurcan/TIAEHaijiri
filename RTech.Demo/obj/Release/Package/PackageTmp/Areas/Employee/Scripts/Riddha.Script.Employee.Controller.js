/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Employee.Model.js" />

//Employee
function employeeController() {
    var self = this;
    var config = new Riddha.config();
    var url = "/Api/EmployeeApi";
    self.Employee = ko.observable(new EmployeeModel());
    self.Employees = ko.observableArray([]);
    self.SelectedEmployee = ko.observable();
    self.Sections = ko.observableArray([]);
    self.Roles = ko.observableArray([]);
    self.AllSections = ko.observableArray([]);
    self.SelectedShift = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.Designations = ko.observableArray([]);
    self.GradeGroups = ko.observableArray([]);
    self.Departments = ko.observableArray([]);
    self.Shifts = ko.observableArray([]);
    self.Banks = ko.observableArray([]);
    self.DepartmentId = ko.observable();
    self.EmployeeInfo = ko.observable(new EmployeeInfoModel());

    getRoles();
    function getRoles() {
        Riddha.ajax.get("/Api/UserRoleApi")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, GlobalDropdownModel);
                self.Roles(data);
            });
    }

    getBanks();
    function getBanks() {
        Riddha.ajax.get("/Api/EmployeeApi/GetBanks")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, GlobalDropdownModel);
                self.Banks(data);
            });
    }
    //punch selection code



    function bindSingleCheckInCheckBox() {
        $('[data-check]').off('change').on('change', function (e) {
            var $context = {};
            var key = $(this).data('check');
            $context = ko.contextFor(this);
            resetPunch($context.$data);
            $context.$data[key](true);

            function resetPunch($data) {
                $data.NoPunch(false);
                $data.SinglePunch(false);
                $data.TwoPunch(false);
                $data.FourPunch(false);
                $data.MultiplePunch(false);
            }
        })
    }


    getShift();
    self.Gender = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "पुरुष" : "Male" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "महिला" : "Female" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "अन्य" : "Others" },
    ]);

    self.Religion = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "हिन्दू" : "Hinduism" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "बौद्ध " : "Buddhism" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "मुस्लिम " : "Islam" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "ईसाइ" : "Christianity" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "जुदाइजम " : "Judaism" },
    ]);

    self.Shift = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "निश्चित " : "Fixed" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "साप्ताहिक" : "Weekly" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "मासिक" : "Monthly" },

    ]);

    self.WeeklyOfType = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "निश्चित " : "Fixed" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "गतिशिल" : "Daynamic" },
    ]);
    //weekly off section start
    self.SelectedWeeklyOff = ko.observableArray([]);
    self.WeeklyOff = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "आइतबार" : "Sunday" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "सोमबार" : "Monday" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "मंगलवार" : "Tuesday" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "बुधवार" : "Wednesday" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "बिहिबार" : "Thursday" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "शुक्रबार" : "Friday" },
        { Id: 6, Name: config.CurrentLanguage == 'ne' ? "सनिबार" : "Saturday" },
    ]);
    //weekly off seciton end

    self.BloodGroups = ko.observableArray([

        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "ए+" : "A+" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "ए-" : "A-" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "बि+" : "B+" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "बि-" : "B-" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "एबि+" : "AB+" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "एबि-" : "AB-" },
        { Id: 6, Name: config.CurrentLanguage == 'ne' ? "व+" : "O+" },
        { Id: 7, Name: config.CurrentLanguage == 'ne' ? "व-" : "O-" }
    ]);
    self.MaritalStatusList = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "बिबाहित" : "Married" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "अबिबाहित" : "Unmarried" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "सम्बन्ध बिच्छेद" : "Divorced" }
    ]);
    //getEmployees();
    self.filterText = ko.observable('');

    self.syncDeviceEmployee = function () {
        Riddha.ajax.get(url + "/PullDataFromWdms", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmployeeGridVm);
                self.Employees(data);
            });
    }
    getDesignation();
    getDepartments();
    getGradeGroup();
    function getDesignation() {
        Riddha.ajax.get(url + "/GetDesignationForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Designations(data);
            });
    };

    function getGradeGroup() {
        Riddha.ajax.get(url + "/GetGradeGroupForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.GradeGroups(data);
            });
    };
    function getShift() {
        Riddha.ajax.get(url + "/GetShiftsForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Shifts(data);
            });
    };

    self.GetGenderName = function (id) {
        var mapped = ko.utils.arrayFirst(self.Gender(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetShiftTypeName = function (id) {
        var mapped = ko.utils.arrayFirst(self.Shift(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetBloodGroupName = function (id) {
        var mapped = ko.utils.arrayFirst(self.BloodGroups(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetDesignationName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Designations()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    function getDepartments() {
        Riddha.ajax.get(url + "/GetDepartmentsForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Departments(data);
                getSections();
            });
    };

    self.GetDepartmentName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Departments()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    function getSections() {
        Riddha.ajax.get(url + "/GetAllSections", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), SectionModel);
                self.AllSections(data);
            });
    };

    self.CreateUpdate = function (model) {
        if (self.Employee().Code.hasError()) {
            return Riddha.util.localize.Required("EmployeeCode");

        }
        if (self.Employee().Name.hasError()) {
            return Riddha.util.localize.Required("Name");

        }
        if (self.Employee().DeviceCode() <= 0) {
            return Riddha.util.localize.Required("EmployeeDeviceCode");
        }
        if (self.Employee().DesignationId() == undefined) {
            return Riddha.util.localize.Required("Designation");
        }
        if (self.Employee().SectionId() == undefined) {
            return Riddha.util.localize.Required("Section");
        }
        if (self.Employee().ShiftId() == undefined) {
            return Riddha.util.localize.Required("Shift");
        }

        if (self.ModeOfButton() == 'Create') {
            var model = ko.toJS(self.Employee());
            model.WeeklyOffIds = self.SelectedWeeklyOff();
            Riddha.ajax.post(url, model)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.gridOptions.refresh();
                        getRoles();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            var model = ko.toJS(self.Employee());
            model.WeeklyOffIds = self.SelectedWeeklyOff();
            model.UserId = self.SelectedEmployee().UserId;
            Riddha.ajax.put(url, model)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.gridOptions.refresh();
                        self.ModeOfButton("Create");
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };
    self.Reset = function () {
        self.Employee(new EmployeeModel({ Id: self.Employee().Id() }));
        //self.Religion([]);
        self.ModeOfButton('Create');
        self.SelectedWeeklyOff([]);
    };

    self.Select = function (model) {
        if (self.SelectedEmployee() == undefined) {
            //return Riddha.UI.Toast("Please Select Row to Edit", Riddha.CRM.Global.ResultStatus.processError);
            return Riddha.util.localize.Required("PleaseSelectRowToEdit");
        }
        self.DepartmentId(self.SelectedEmployee().DepartmentId);
        Riddha.ajax.get(url + "/get/" + self.SelectedEmployee().Id).done(function (data) {
            if (data.Status == 4) {
                self.Employee(new EmployeeModel(ko.toJS(data.Data)));
                self.SelectedWeeklyOff(data.Data.WeeklyOffIds);
                self.ModeOfButton('Update');
                self.ShowModal();
            }
        });
    };

    self.Delete = function (employee) {
        if (self.SelectedEmployee() == undefined) {
            return Riddha.UI.Toast("Please Select Row to Delete", Riddha.CRM.Global.ResultStatus.processError);
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedEmployee().Id, null)
                .done(function (result) {
                    obj.gridOptions.refresh();
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        if (self.ModeOfButton() == 'Create') {
            self.Employee().TwoPunch(true);
            self.SelectedWeeklyOff([6]);
            self.Employee().MaritialStatus(1);
        }
        $("#employeeCreationModel").modal('show');
        $("#tabList li").removeClass('active'); $("#tabList li:first").addClass('active');
        bindSingleCheckInCheckBox();
    };

    self.ActivateDeactivateEmp = function (item) {
        if (self.SelectedEmployee() == undefined) {
            return Riddha.UI.Toast("Please Select Row to Login", Riddha.CRM.Global.ResultStatus.processError);
        }
        //if (self.SelectedEmployee().Status == "On") {
        //    Riddha.UI.Confirm("DeactivateConfirm", function () {
        //        Riddha.ajax.get(url + "/ActivateDeactivateEmp?empId=" + self.SelectedEmployee().Id + "&userName=" + self.SelectedEmployee().UserName + "&password=" + self.SelectedEmployee().Password + "&roleId=" + self.SelectedEmployee().RoleId)
        //    .done(function (result) {
        //        if (result.Status == 4) {
        //            obj.gridOptions.refresh();
        //        }
        //        Riddha.UI.Toast(result.Message, result.Status);
        //    });
        //    });
        //}
        //else {
        Riddha.ajax.get(url + "/GetEmpLogin?empId=" + self.SelectedEmployee().Id)
            .done(function (result) {
                if (result.Status == 4) {
                    if (result.Data.Id == 0) {
                        self.Employee().UserName(self.SelectedEmployee().Mobile);
                        self.Employee().Password(self.SelectedEmployee().Mobile);
                    }
                    else {
                        self.Employee().UserName(result.Data.Name);
                        self.Employee().Password(result.Data.Password);
                        self.Employee().RoleId(result.Data.RoleId);
                    }
                    //self.SelectedEmployee(item);
                    $("#loginModal").modal('show');
                }
            });

        //}
    }
    self.ActivateEmployee = function (emp) {
        if (self.Employee().RoleId() == undefined) {
            return Riddha.util.localize.Required("Role");
        }
        if (self.Employee().UserName() == undefined) {
            return Riddha.util.localize.Required("UserName");
        }
        if (self.Employee().Password() == undefined) {
            return Riddha.util.localize.Required("Password");
        }
        Riddha.ajax.get(url + "/ActivateDeactivateEmp?empId=" + self.SelectedEmployee().Id + "&userName=" + self.Employee().UserName() + "&password=" + self.Employee().Password() + "&roleId=" + self.Employee().RoleId())
            .done(function (result) {
                if (result.Status == 4) {
                    $("#loginModal").modal('hide');
                    obj.gridOptions.refresh();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }

    $("#employeeCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#employeeCreationModel").modal('hide');
        self.Reset();
    };

    self.IsNullableName = function (nameNp) {
        var result = false;
        ko.utils.arrayForEach(self.Sections(), function (item) {
            if (item.NameNp() == null) {
                result = true;
            }
        })
        return result;
    }



    self.InfoShowModal = function (model) {
        if (self.SelectedEmployee() == undefined) {
            return Riddha.UI.Toast("Please Select Row to View Details", Riddha.CRM.Global.ResultStatus.processError);
        }
        $("#employeeInfo").modal('show');
        getEmpInfo(self.SelectedEmployee().Id);
    };

    function getEmpInfo(id) {
        Riddha.ajax.get(url + "/GetEmpInfo?id=" + id)
            .done(function (result) {
                self.EmployeeInfo(new EmployeeInfoModel(result.Data));
            })
    };


    self.trigerFileBrowse = function () {
        $("#UploadedFile").click();
    }
    self.UploadClick = function () {
        var xhr = new XMLHttpRequest();
        var file = document.getElementById('UploadedFile').files[0];
        if (file == undefined) {
            return Riddha.UI.Toast("Please Select Excel File to Upload", Riddha.CRM.Global.ResultStatus.processError);
        }

        Riddha.UI.Confirm("ExcelUploadConfirm", function () {
            xhr.open("POST", "/api/EmployeeApi/Upload");
            xhr.setRequestHeader("filename", file.name);
            xhr.onreadystatechange = function (data) {
                if (xhr.readyState == 4) {
                    var response = JSON.parse(xhr.responseText);
                    if (response["Status"] == 4) {
                        obj.gridOptions.refresh();
                    }
                    return Riddha.UI.Toast(response["Message"], response["Status"]);
                }
            };
            xhr.send(file);
        });
    };


    self.ExportClick = function () {
        window.location = "/Employee/Employee/ExportEmployee";
    };

    self.EmployeeExcelExportModel = {
        DeviceId: ko.observable(),
        BadgeNUmber: ko.observable(),
        Name: ko.observable(''),
        NameNepali: ko.observable(''),
        Gender: ko.observable(''),
        Address: ko.observable(),
        Mobile: ko.observable(''),
        Designation: ko.observable(''),
        Section: ko.observable(),

    }
    self.gridOptions =
        riddhaKoGrid(
            {
                data: self.Employees,
                columnDefs: [{ field: 'Id', visible: false }, { field: 'IdCardNo', displayName: 'Employee Code', width: '10%' }, { field: 'EmployeeName', displayName: 'Employee Name', width: '20%' }, { field: 'NameNp', displayName: 'Employee Name(Nepali)', width: '20%' }, { field: 'DesignationName', displayName: 'Designation', width: '15%' }, { field: 'DepartmentName', displayName: 'Department', width: '15%' }, { field: 'Mobile', displayName: 'Mobile', width: '10%' }, { field: 'Status', displayName: 'Login Status', width: '10%' }],
                filterText: self.filterText,
                //pageSize: 50,
                enableServerPaging: true,
                jsonUrl: url,
                getSelectedItem: function (data) {
                    self.SelectedEmployee(data);
                },
                getSelectedItems: function (data) {
                }
            });
    self.KendoGridOptions = {
        title: "Employee",
        target: "#empKendoGrid",
        url: "/Api/EmployeeApi/GetEmpKendoGrid",
        paramData: {},
        multiSelect: true,
        columns: [
            { field: 'IdCardNo', title: "Code" },
            { field: 'EmployeeName', title: "Name" },
            { field: 'NameNp', title: "Name Np" },
            { field: 'DepartmentName', title: "Department" },
            { field: 'Mobile', title: "Mobile" }
        ],
        SelectedItem: function (item) {
        },
        SelectedItems: function (items) {
        }
    }


}

//Leave Balance
function leaveBalanceController() {
    var self = this;
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.ModeOfButton = ko.observable('Save');
    self.LeaveMasters = ko.observableArray([]);
    self.EmployeeId = ko.observable(self.Employee().Id || 0);


    self.GetEmployee = function () {
        if (self.Employee().Code() != '' || self.Employee().Name() != '') {
            Riddha.ajax.get("/Api/LeaveBalanceApi/SearchEmployee/?empCode=" + self.Employee().Code() + "&empName=" + self.Employee().Name(), null)
                .done(function (result) {
                    self.Employee(new EmpSearchViewModel(result.Data.Employee));
                    if (result.Data.LeaveMasters.length <= 0 && self.Employee().Id() != 0) {
                        getLeaveMaster();
                    }
                    else {
                        var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data.LeaveMasters), LeaveMasterViewModel);
                        self.LeaveMasters(data);
                        self.ModeOfButton('Update');
                    }
                });
        }
        else {
            return Riddha.UI.Toast("Please Enter Employee Code Or Name To Search", 2);
        }
    };
    function getLeaveMaster() {
        Riddha.ajax.get("/Api/LeaveMasterApi", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), LeaveMasterViewModel);
                self.LeaveMasters(data);
            });
    }
    self.Create = function () {
        if (self.Employee().Id() > 0) {
            var data = { Employee: self.Employee(), LeaveMasters: self.LeaveMasters() };
            Riddha.ajax.post("/Api/LeaveBalanceApi", ko.toJS(data))
                .done(function (result) {
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else {
            return Riddha.UI.Toast("Please Choose Valid Employee!!", 1);
        }
    };
    self.Reset = function () {
        self.Employee(new EmpSearchViewModel());
        self.LeaveMasters([]);
        self.ModeOfButton('Save');
    }
}

//Leave Application
function leaveApplicationController() {
    var self = this;
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    var url = "/Api/LeaveApplicationApi";
    self.LeaveApplication = ko.observable(new LeaveApplicationViewModel());
    self.LeaveApplications = ko.observableArray([]);
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);
    self.LeaveMasters = ko.observableArray([]);
    self.LeaveMasterId = ko.observable(0);
    self.ApproveBy = ko.observableArray([]);
    self.ModeOfButton = ko.observable('Create');
    self.SelectedLeaveApplication = ko.observable();

    self.LeaveDay = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "पुरा दिन" : "Full Day" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "चाँडो बिदा" : "Early Leave" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "ढिलो बिदा " : "Late Leave" },
    ]);

    self.GetLeaveDayName = function (id) {
        var mapped = ko.utils.arrayFirst(self.LeaveDay(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    getApproveUser();
    //function getLeaveMaster() {
    //    var emp = Riddha.ko.global.find(self.Employee, self.LeaveApplication().EmployeeId);
    //    Riddha.ajax.get(url + "/GetDesigWiseLeave?desigId=" + self.Employee().DesignationId())
    //        .done(function (result) {
    //            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), LeaveMasterGridVm);
    //            self.LeaveMasters(data);
    //        });
    //};

    function getLeaveMaster() {
        var emp = Riddha.ko.global.find(self.Employee, self.LeaveApplication().EmployeeId);
        Riddha.ajax.get(url + "/GetDesigAndEmploymentWiseLeave?empId=" + self.Employee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), LeaveMasterGridVm);
                self.LeaveMasters(data);
            });
    };

    function getApproveUser() {
        Riddha.ajax.get(url + "/GetEmployeesForDropdown/?empId=" + self.LeaveApplication().EmployeeId())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.ApproveBy(data);
            });
    };
    self.GetApproveUserName = function (id) {
        var mapped = ko.utils.arrayFirst(self.ApproveBy(), function (data) {
            return data.Id() == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetEmployee = function () {
        if (self.Employee().Code() != '' || self.Employee().Name() != '') {
            Riddha.ajax.get("/Api/LeaveApplicationApi/SearchEmployee/?empCode=" + self.Employee().Code() + "&empName=" + self.Employee().Name(), null)
                .done(function (result) {
                    self.Employee(new EmpSearchViewModel(result.Data));
                    self.LeaveApplication().EmployeeId(result.Data.Id);
                    getLeaveMaster();
                    self.LeaveMasters([]);
                    self.LeaveApplication().LeaveCount(0);
                });
        }
        else {
            return Riddha.UI.Toast("Please Enter Employee Code Or Name To Search", 2);
        }

    };
    self.GetRemLeave = function () {
        if (self.LeaveApplication().LeaveMasterId() == undefined || self.LeaveApplication().EmployeeId() == 0) {
            return false;
        }
        Riddha.ajax.get("/Api/LeaveApplicationApi/GetRemLeave/?leaveMastId=" + self.LeaveApplication().LeaveMasterId() + "&empId=" + self.LeaveApplication().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.LeaveApplication().LeaveCount(result.Data);
                }
                else {
                    Riddha.UI.Toast(result.Message, result.Status);
                }
            });
    }
    self.ShowModal = function () {
        $("#leaveApplicationCreationModel").modal('show');
    };

    $("#leaveApplicationCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
        self.RefreshKendoGrid();
        self.ModeOfButton("Create");
    });

    self.CloseModal = function () {

        self.Reset();
        self.RefreshKendoGrid();
        self.ModeOfButton("Create");
        $("#leaveApplicationCreationModel").modal('hide');
    };

    self.CreateUpdate = function () {

        if (self.LeaveApplication().EmployeeId() == 0) {
            return Riddha.util.localize.Required("Employee");
        }
        else if (self.LeaveApplication().From() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("From");
        }
        else if (self.LeaveApplication().LeaveDay() == 0) {
            if (self.LeaveApplication().To() == "NaN/aN/aN") {
                return Riddha.util.localize.Required("To");
            }
        }
        if (self.LeaveApplication().From() > self.LeaveApplication().To()) {
            return Riddha.UI.Toast("Invalid To date");
        }
        else if (self.LeaveApplication().Description() == "") {
            return Riddha.util.localize.Required("Description");
        }
        else if (self.LeaveApplication().LeaveMasterId() == undefined) {
            return Riddha.util.localize.Required("LeaveMaster");
        }
        else if (self.LeaveApplication().ApprovedById() == undefined) {
            return Riddha.util.localize.Required("ApprovedBy");
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post("/Api/LeaveApplicationApi", ko.toJS(self.LeaveApplication()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.LeaveApplication()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.ModeOfButton("Create");
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Reset = function () {
        self.LeaveApplication(new LeaveApplicationViewModel());
        self.Employee(new EmpSearchViewModel());
    };

    self.Select = function (model) {
        if (self.SelectedLeaveApplication() == undefined || self.SelectedLeaveApplication().length > 1 || self.SelectedLeaveApplication().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        self.LeaveApplication(new LeaveApplicationViewModel(ko.toJS(self.SelectedLeaveApplication())));
        self.Employee(new EmpSearchViewModel({ Code: self.SelectedLeaveApplication().EmployeeCode(), Name: self.SelectedLeaveApplication().EmployeeName() }));
        self.GetEmployee();
        Riddha.util.delayExecute(function () {
            self.LeaveApplication().LeaveMasterId(self.SelectedLeaveApplication().LeaveMasterId());
            self.GetRemLeave();
            self.ModeOfButton('Update');
            self.ShowModal();
        }, 500);
    };

    self.Delete = function (leaveapplication) {
        if (self.SelectedLeaveApplication() == undefined || self.SelectedLeaveApplication().length > 1 || self.SelectedLeaveApplication().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        if (self.SelectedLeaveApplication().LeaveStatus() == 1) {
            Riddha.UI.Toast("Approved leave application cannot be deleted..", 0);
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedLeaveApplication().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.ModeOfButton("Create");
                        self.Reset();
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.Employee(new EmpSearchViewModel(item));
            self.LeaveApplication().EmployeeId(item.Id);
            getLeaveMaster();
            self.LeaveMasters([]);
            self.LeaveApplication().LeaveCount(0);
            getApproveUser();
            self.LeaveApplication().ApprovedById(item.ReportingManagerId);

        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    }
    self.KendoAutoCompleteOptions = {
        title: "Search Employee",
        target: "#empSearch",
        width: 500,
        url: "/Api/EmployeeApi/GetEmpKendoGrid",
        paramData: {},
        multiSelect: true,
        columns: [
            { field: 'IdCardNo', title: "Code" },
            { field: 'EmployeeName', title: "Name" },
            { field: 'NameNp', title: "Name Np" },
            { field: 'DepartmentName', title: "Department" },
            { field: 'Mobile', title: "Mobile" }
        ],
        SelectedItem: function (item) {

        },
        SelectedItems: function (items) {

        }
    }
    //Kendo grid

    self.KendoGridOptions = {
        title: "LeaveApplication",
        target: "#leaveApplicationKendoGrid",
        url: "/Api/LeaveApplicationApi/GetLeaveApplicationKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        selectable: true,
        //groupParam: { field: "DepartmentName" },

        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारी" : "Employee", width: 180 },
            { field: 'LeaveMaster', title: lang == "ne" ? "बिदाको प्रकार" : "Leave Type", width: 220 },
            { field: 'From', title: lang == "ne" ? "मिति देखी" : "From", template: "#=SuitableDate(From)#", filterable: false },
            { field: 'To', title: lang == "ne" ? "मिति सम्म" : "	To", template: "#=SuitableDate(To)#", filterable: false },
            { field: 'Days', title: lang == "ne" ? "दिन" : "Days", template: "#=SuitableNumber(Days)#", filterable: false },
            { field: 'LeaveDay', title: lang == "ne" ? "बिदाको दिन" : "Leave Day", template: "#=getLeavedayTypeName(LeaveDay)#", filterable: false },
            { field: 'ApprovedByUser', title: lang == "ne" ? "स्विक्रित द्वारा" : "Approved By", filterable: false },
            {
                field: 'LeaveStatusName', title: lang == "ne" ? "स्थिति" : "Status", template: "#=getLeaveStatusTemp(LeaveStatus,LeaveStatusName)#", filterable: false
            },
        ],
        SelectedItem: function (item) {
            self.SelectedLeaveApplication(new leaveApplicationGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedLeaveApplication(new leaveApplicationGridVm());
        $("#leaveApplicationKendoGrid").getKendoGrid().dataSource.read();
    }
}

function leaveApprovalController() {
    var self = this;
    var url = "/Api/LeaveApprovalApi";
    self.LeaveApproval = ko.observable(new LeaveApprovalViewModel());
    self.LeaveApprovals = ko.observableArray([]);
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);
    self.LeaveAppId = ko.observable(0);

    self.LeaveStatusList = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "नयाँ" : "New" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "स्विकृती" : "Approve" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "अस्विकार" : "Reject" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "उल्टाइयो" : "Reverted" },

    ]);
    self.GetLeaveStatusName = function (id) {
        var mapped = ko.utils.arrayFirst(self.LeaveStatusList(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };
    self.LeaveDay = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "पुरा दिन" : "Full Day" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "चाँडो बिदा" : "Early Leave" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "ढिलो बिदा" : "Late Leave" },
    ]);
    self.GetLeaveDayName = function (id) {
        var mapped = ko.utils.arrayFirst(self.LeaveDay(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };
    //GetLeaveApprovals();
    function GetLeaveApprovals() {
        Riddha.ajax.get(url, ko.toJS(self.LeaveApproval()))
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), LeaveApprovalViewModel);
                self.LeaveApprovals(data);
            });
    }

    self.GetRemLeave = function () {
        if (self.LeaveApproval().LeaveMasterId() == undefined) {
            return false;
        }
        Riddha.ajax.get("/Api/LeaveApprovalApi/GetRemLeave/?leaveMastId=" + self.LeaveApproval().LeaveMasterId() + "&empId=" + self.LeaveApproval().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.LeaveApproval().LeaveCount(result.Data);
                }
                else {
                    Riddha.UI.Toast(result.Message, result.Status);
                }
            });
    }
    self.ViewDetails = function (item) {
        if (self.LeaveAppId() == 0) {
            Riddha.UI.Toast("Please select row to view details", 0);
            return;
        }
        Riddha.ajax.get(url + "/GetLeaveApprovalDetails?leaveAppId=" + self.LeaveAppId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.LeaveApproval(new LeaveApprovalViewModel(result.Data));
                    self.ShowModal();
                }
            })
    };

    self.Reset = function () {
        self.LeaveApproval(new LeaveApprovalViewModel());
        self.LeaveAppId(0);
    };

    

    self.Approve = function (item) {
        if (self.LeaveApproval().LeaveStatus() == 1) {
            Riddha.UI.Toast("Already Appoved", 0);
            return;
        }
        Riddha.ajax.get("/Api/LeaveApprovalApi/Approve?id=" + self.LeaveApproval().Id() + "&remarks=" + self.LeaveApproval().AdminRemark())
            .done(function (result) {
                if (result.Status == 4) {
                    //GetLeaveApprovals();
                    self.CloseModal();
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }


    self.Reject = function (item) {
        if (self.LeaveApproval().LeaveStatus() == 2) {
            Riddha.UI.Toast("Already Rejected", 2);
            return;
        }
        if (self.LeaveApproval().LeaveStatus() == 1) {
            Riddha.UI.Toast("Leave already approved. Cannot Reject approved leave", 0);
            return;
        }
        Riddha.ajax.get(url + "/Reject?id=" + self.LeaveApproval().Id() + "&remarks=" + self.LeaveApproval().AdminRemark())
            .done(function (result) {
                if (result.Status == 4) {
                    //GetLeaveApprovals();
                    self.RefreshKendoGrid();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }

    self.Revert = function (item) {
        if (self.LeaveApproval().LeaveStatus() == 3) {
            Riddha.UI.Toast("Already Reverted", 2);
            return;
        }
        Riddha.ajax.get(url + "/Revert?id=" + self.LeaveApproval().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }


    //Kendo Grid
    self.KendoGridOptions = {
        title: "Leave Approval",
        target: "#leaveApprovalKendoGrid",
        url: "/Api/LeaveApprovalApi/GetLeaveApprovalKendoGrid",
        height: 490,
        paramData: {},
        //multiSelect: true,
        group: true,
        //selectable: false,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmpName', title: lang == "ne" ? "कर्मचारी" : "Employee", width: 250 },
            { field: 'Leave', title: lang == "ne" ? "बिदाको प्रकार" : "LeaveType", width: 200 },
            { field: 'From', title: lang == "ne" ? "मिति देखी" : "From", filterable: false, template: "#=SuitableDate(From)#" },
            { field: 'To', title: lang == "ne" ? "मिति सम्म" : "To", filterable: false, template: "#=SuitableDate(To)#" },
            { field: 'LeaveCount', title: lang == "ne" ? "दिन " : "Days", filterable: false, template: "#=SuitableNumber(LeaveCount)#" },

            {
                field: 'LeaveStatusName', title: lang == "ne" ? "स्थिति" : "Status", template: "#=getLeaveStatusTemp(LeaveStatus,LeaveStatusName)#", filterable: false
            },
            {
                command: [
                    { name: "view", template: '<a class="k-grid-view k-button" ><span class="fa fa-eye text-green"  ></span></a>', click: View },
                    { name: "approve", template: '<a class="k-grid-approve k-button" ><span class="fa fa-check text-blue"  ></span></a>', click: Approve }
                ],
                title: lang == "ne" ? "कार्य" : "Action",
                width: "180px"
            }
        ],
        SelectedItem: function (item) {
            self.LeaveAppId(item.Id);
        },
        SelectedItems: function (items) {

        },
    };

    $("#leaveApprovalKendoGrid").kendoTooltip({
        filter: ".k-grid-view",
        position: "bottom",
        content: function (e) {
            return lang == "ne" ? "विवरण हेर्नुहोस्" : "View Details";
        }
    });
    $("#leaveApprovalKendoGrid").kendoTooltip({
        filter: ".k-grid-approve",
        position: "bottom",
        content: function (e) {
            return lang == "ne" ? "अनुमोदन गर्नुहोस्" : "Approve";
        }
    });

    function View(e) {
        var grid = $("#leaveApprovalKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        Riddha.ajax.get(url + "/GetLeaveApprovalDetails?leaveAppId=" + item.Id)
            .done(function (result) {
                if (result.Status == 4) {
                    self.LeaveApproval(new LeaveApprovalViewModel(result.Data));
                    $("#leaveinfoModal").modal('show');
                }
            })
    }

    function Approve(e) {
        var grid = $("#leaveApprovalKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        Riddha.ajax.get("/Api/LeaveApprovalApi/Approve?id=" + item.Id + "&remarks=" + self.LeaveApproval().AdminRemark())
            .done(function (result) {
                if (result.Status == 4) {
                    //GetLeaveApprovals();
                    self.CloseModal();
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }

    self.RefreshKendoGrid = function () {
        self.LeaveAppId(0);
        $("#leaveApprovalKendoGrid").getKendoGrid().dataSource.read();
    }

    self.ShowModal = function () {
        $("#leaveinfoModal").modal('show');
    }

    $('#leaveinfoModal').on('hide.bs.modal', function (e) {
        self.Reset();
    });

    self.CloseModal = function () {
        self.Reset();
        $("#leaveinfoModal").modal('hide');
    }
}

function employeeControllerUsingKendoGrid() {
    var self = this;
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    var url = "/Api/EmployeeApi";
    var admsUrl = "/Api/ADMSApi";
    self.Employee = ko.observable(new EmployeeModel());
    self.Employees = ko.observableArray([]);
    self.SelectedEmployee = ko.observable();
    self.Sections = ko.observableArray([]);
    self.Roles = ko.observableArray([]);
    self.AllSections = ko.observableArray([]);
    self.SelectedShift = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.Designations = ko.observableArray([]);
    self.GradeGroups = ko.observableArray([]);
    self.Departments = ko.observableArray([]);
    self.Shifts = ko.observableArray([]);
    self.Banks = ko.observableArray([]);
    self.DepartmentId = ko.observable();
    self.EmployeeInfo = ko.observable(new EmployeeInfoModel());
    self.ShiftTypeId = ko.observable(0);
    self.ModeOfLoginActivation = ko.observable(true);
    self.OTManagement = ko.observable(new OTManagementModel());
    self.ShiftManagementId = ko.observable(0);
    self.Devices = ko.observableArray([]);
    self.DeviceSN = ko.observable();
    self.Branches = ko.observableArray([]);
    self.BranchId = ko.observable(0);
    self.GridIsOpen = ko.observable(false);
    self.IsRequestComplete = ko.observable(true);
    self.PromotionDesignationId = ko.observable(0);

    getBranches();
    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Branches(data);
            });
    };


    getRoles();
    function getRoles() {
        Riddha.ajax.get(url + "/GetRolesForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, GlobalDropdownModel);
                self.Roles(data);
            });
    }

    getBanks();
    function getBanks() {
        Riddha.ajax.get("/Api/EmployeeApi/GetBanks")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, GlobalDropdownModel);
                self.Banks(data);
            });
    }

    getCompanyDevices();
    function getCompanyDevices() {
        Riddha.ajax.get("/Api/EmployeeApi/GetCompanyDevices")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, GlobalDropdownModel);
                self.Devices(data);
            });
    }


    function bindSingleCheckInCheckBox() {
        $('[data-check]').off('change').on('change', function (e) {
            var $context = {};
            var key = $(this).data('check');
            $context = ko.contextFor(this);
            resetPunch($context.$data);
            $context.$data[key](true);

            function resetPunch($data) {
                $data.NoPunch(false);
                $data.SinglePunch(false);
                $data.TwoPunch(false);
                $data.FourPunch(false);
                $data.MultiplePunch(false);

            }

        })
    }


    getShift();
    self.Gender = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "पुरुष" : "Male" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "महिला" : "Female" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "अन्य" : "Others" },
    ]);

    self.Religion = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "हिन्दू" : "Hinduism" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "बौद्ध " : "Buddhism" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "मुस्लिम " : "Islam" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "ईसाइ" : "Christianity" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "जुदाइजम " : "Judaism" },
    ]);

    self.Shift = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "निश्चित " : "Fixed" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "साप्ताहिक" : "Weekly" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "मासिक" : "Monthly" },

    ]);

    self.WeeklyOfType = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "निश्चित " : "Fixed" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "गतिशिल" : "Daynamic" },
    ]);
    //weekly off section start
    self.SelectedWeeklyOff = ko.observableArray([]);
    self.WeeklyOff = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "आइतबार" : "Sunday" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "सोमबार" : "Monday" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "मंगलवार" : "Tuesday" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "बुधवार" : "Wednesday" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "बिहिबार" : "Thursday" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "शुक्रबार" : "Friday" },
        { Id: 6, Name: config.CurrentLanguage == 'ne' ? "सनिबार" : "Saturday" },
    ]);
    //weekly off seciton end

    self.BloodGroups = ko.observableArray([

        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "ए+" : "A+" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "ए-" : "A-" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "बि+" : "B+" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "बि-" : "B-" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "एबि+" : "AB+" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "एबि-" : "AB-" },
        { Id: 6, Name: config.CurrentLanguage == 'ne' ? "व+" : "O+" },
        { Id: 7, Name: config.CurrentLanguage == 'ne' ? "व-" : "O-" }
    ]);

    self.MaritalStatusList = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "बिबाहित" : "Married" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "अबिबाहित" : "Unmarried" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "सम्बन्ध बिच्छेद" : "Divorced" }
    ]);

    self.filterText = ko.observable('');

    self.syncDeviceEmployee = function () {
        Riddha.UI.Confirm("PullDevicEemployeeConfirm", function () {
            Riddha.ajax.get(url + "/PullDataFromWdms", null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                    }

                });
        });
    }
    getDesignation();

    getGradeGroup();
    function getDesignation() {
        Riddha.ajax.get(url + "/GetDesignationForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Designations(data);
            });
    };

    self.DesignationData = function () {
        return ko.toJS(self.Designations());
    };

    self.designationDropDownParam = { url: url + "/GetDesignationForDropdown" };

    function getGradeGroup() {
        Riddha.ajax.get(url + "/GetGradeGroupForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.GradeGroups(data);
            });
    };

    function getShift() {
        Riddha.ajax.get(url + "/GetShiftsForDropdown?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Shifts(data);
            });
    };

    self.GetGenderName = function (id) {
        var mapped = ko.utils.arrayFirst(self.Gender(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetShiftTypeName = function (id) {
        var mapped = ko.utils.arrayFirst(self.Shift(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetBloodGroupName = function (id) {
        var mapped = ko.utils.arrayFirst(self.BloodGroups(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetDesignationName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Designations()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    function getDepartments() {
        Riddha.ajax.get(url + "/GetDepartmentsForDropdown?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Departments(data);
                getSections();
            });
    };

    self.GetDepartmentName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Departments()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    function getSections() {
        //Riddha.ajax.get(url + "/GetAllSections", null)
        Riddha.ajax.get(url + "/GetAllSections?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), SectionModel);
                self.AllSections(data);
            });
    };

    self.CreateUpdate = function (model) {
        if (self.Employee().Code.hasError()) {
            return Riddha.util.localize.Required("EmployeeCode");
        }
        if (self.Employee().Name.hasError()) {
            return Riddha.util.localize.Required("Name");

        }
        if (self.Employee().DeviceCode() <= 0) {
            return Riddha.util.localize.Required("EmployeeDeviceCode");
        }
        if (self.Employee().DesignationId() == undefined) {
            return Riddha.util.localize.Required("Designation");
        }
        if (self.Employee().SectionId() == undefined) {
            return Riddha.util.localize.Required("Section");
        }
        if (self.Employee().ShiftId() == undefined) {
            return Riddha.util.localize.Required("Shift");
        }
        if (self.BranchId() != undefined) {
            self.Employee().BranchId(self.BranchId());
        }
        if (self.Employee().BranchId() == undefined) {
            Riddha.UI.Toast("Please select branch..")
            return;
        }
        self.IsRequestComplete(false);

        if (self.ModeOfButton() == 'Create') {
            var model = ko.toJS(self.Employee());
            model.WeeklyOffIds = self.SelectedWeeklyOff();
            Riddha.ajax.post(url, model)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        getRoles();
                        self.Reset();
                        self.CloseModal();
                    }
                    self.IsRequestComplete(true);
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            var model = ko.toJS(self.Employee());
            model.WeeklyOffIds = self.SelectedWeeklyOff();
            model.UserId = self.SelectedEmployee().UserId;
            Riddha.ajax.put(url, model)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.ModeOfButton("Create");
                        self.Reset();
                        self.CloseModal();
                    }
                    self.IsRequestComplete(true);
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };
    self.Reset = function () {
        self.Employee(new EmployeeModel({ Id: self.Employee().Id() }));
        //self.Religion([]);
        self.ModeOfButton('Create');
        self.SelectedWeeklyOff([]);
    };

    self.Select = function (model) {
        if (self.SelectedEmployee() == undefined || self.SelectedEmployees().length > 1) {
            //return Riddha.UI.Toast("Please Select Row to Edit", Riddha.CRM.Global.ResultStatus.processError);
            return Riddha.util.localize.Required("PleaseSelectRowToEdit");
        }
        self.DepartmentId(self.SelectedEmployee().DepartmentId());
        Riddha.ajax.get(url + "/get/" + self.SelectedEmployee().Id()).done(function (data) {
            if (data.Status == 4) {
                self.Employee(new EmployeeModel(ko.toJS(data.Data)));
                //self.designationDropDownParam.value(self.Employee().DesignationId());
                self.SelectedWeeklyOff(data.Data.WeeklyOffIds);
                self.ModeOfButton('Update');
                self.ShowModal();
            }
        });
    };

    self.Delete = function (employee) {
        if (self.SelectedEmployees().length < 1) {
            // return Riddha.UI.Toast("Please Select Row to Delete", Riddha.CRM.Global.ResultStatus.processError);
            return Riddha.util.localize.Required("PleaseSelectRowToDelete");
        }
        var ids = [];
        ko.utils.arrayForEach(self.SelectedEmployees(), function (item) {
            ids.push(item.Id());
        });
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.post(url + "/DeleteKendoGridEmployee", { Ids: ids })
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.ModeOfButton("Create");
                        self.Reset();
                        Riddha.UI.Toast(result.Message, result.Status);
                    }
                });
        })
    };
    $('#employeeCreationModel').on('hide.bs.modal', function (e) {
        self.Reset();
    });

    self.ShowModal = function () {
        getShift();
        getDepartments();
        if (self.ModeOfButton() == 'Create') {
            self.Employee().TwoPunch(true);
            self.SelectedWeeklyOff([6]);
            self.Employee().MaritialStatus(1);
        }
        self.Employee().BranchId(self.BranchId());
        $("#employeeCreationModel").modal('show');
        $("#tabList li").removeClass('active'); $("#tabList li:first").addClass('active');
        bindSingleCheckInCheckBox();
    };

    self.ShowActivateDeactivateModal = function (item) {
        if (self.SelectedEmployee() == undefined || self.SelectedEmployees().length > 1) {
            //return Riddha.UI.Toast("Please Select Row to Login", Riddha.CRM.Global.ResultStatus.processError);
            return Riddha.util.localize.Required("PleaseSelectRowToLogin");
        }
        self.ModeOfLoginActivation(self.SelectedEmployee().Status() == "On" ? false : true);
        //if (self.SelectedEmployee().Status() == "On") {
        //    Riddha.UI.Confirm("DeactivateConfirm", function () {
        //        Riddha.ajax.get(url + "/ActivateDeactivateEmp?empId=" + self.SelectedEmployee().Id() + "&userName=''" + "&password=''" + "&roleId=0")
        //    .done(function (result) {
        //        if (result.Status == 4) {
        //            self.RefreshKendoGrid();
        //        }
        //        Riddha.UI.Toast(result.Message, result.Status);
        //    });
        //    });
        //}
        //else {
        Riddha.ajax.get(url + "/GetEmpLogin?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    if (result.Data.Id == 0) {
                        self.Employee().UserName(self.SelectedEmployee().Mobile());
                        self.Employee().Password(self.SelectedEmployee().Mobile());
                    }
                    else {
                        self.Employee().UserName(result.Data.Name);
                        self.Employee().Password(result.Data.Password);
                        self.Employee().RoleId(result.Data.RoleId);
                    }
                    //self.SelectedEmployee(item);
                    $("#loginModal").modal('show');
                }
            });

        //}
    }
    self.ActivateDeactivateEmp = function (emp) {
        if (self.Employee().RoleId() == undefined) {
            return Riddha.util.localize.Required("Role");
        }
        if (self.Employee().UserName() == undefined) {
            return Riddha.util.localize.Required("UserName");
        }
        if (self.Employee().Password() == undefined) {
            return Riddha.util.localize.Required("Password");
        }
        if (self.ModeOfLoginActivation() == false) {
            Riddha.ajax.get(url + "/ActivateDeactivateEmp?empId=" + self.SelectedEmployee().Id() + "&userName=''" + "&password=''" + "&roleId=0")
                .done(function (result) {
                    if (result.Status == 4) {
                        $("#loginModal").modal('hide');
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else {
            Riddha.ajax.get(url + "/ActivateDeactivateEmp?empId=" + self.SelectedEmployee().Id() + "&userName=" + self.Employee().UserName() + "&password=" + self.Employee().Password() + "&roleId=" + self.Employee().RoleId())
                .done(function (result) {
                    if (result.Status == 4) {
                        $("#loginModal").modal('hide');
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }

    }
    self.CloseModal = function () {
        $("#employeeCreationModel").modal('hide');
        self.Reset();
    };

    self.IsNullableName = function (nameNp) {
        var result = false;
        ko.utils.arrayForEach(self.Sections(), function (item) {
            if (item.NameNp() == null) {
                result = true;
            }
        })
        return result;
    }
    self.InfoShowModal = function (model) {
        if (self.SelectedEmployee() == undefined || self.SelectedEmployees().length > 1) {
            return Riddha.util.localize.Required("PleaseSelectRowToViewDetails");
        }
        getEmpInfo(self.SelectedEmployee().Id());
    };

    function getEmpInfo(id) {
        Riddha.ajax.get(url + "/GetEmpInfo?id=" + id)
            .done(function (result) {
                self.EmployeeInfo(new EmployeeInfoModel(result.Data));
                $("#employeeInfo").modal('show');
            })
    };


    self.trigerFileBrowse = function () {
        $("#UploadedFile").click();
    }
    self.UploadClick = function () {
        var xhr = new XMLHttpRequest();
        var file = document.getElementById('UploadedFile').files[0];
        if (file == undefined) {
            return Riddha.UI.Toast("Please Select Excel File to Upload", Riddha.CRM.Global.ResultStatus.processError);
        }

        Riddha.UI.Confirm("ExcelUploadConfirm", function () {
            xhr.open("POST", "/api/EmployeeApi/Upload");
            xhr.setRequestHeader("filename", file.name);
            xhr.onreadystatechange = function (data) {
                if (xhr.readyState == 4) {
                    var response = JSON.parse(xhr.responseText);
                    if (response["Status"] == 4) {
                        self.RefreshKendoGrid();
                    }
                    return Riddha.UI.Toast(response["Message"], response["Status"]);
                }
            };
            xhr.send(file);
        });
    };


    self.ExportClick = function () {
        window.location = "/Employee/Employee/ExportEmployee";
    };

    self.ExportFormatClick = function () {
        window.location = "/Employee/Employee/ExportEmployeeFormatToSave";
    };

    self.EmployeeExcelExportModel = {
        DeviceId: ko.observable(),
        BadgeNUmber: ko.observable(),
        Name: ko.observable(''),
        NameNepali: ko.observable(''),
        Gender: ko.observable(''),
        Address: ko.observable(),
        Mobile: ko.observable(''),
        Designation: ko.observable(''),
        Section: ko.observable(),

    }

    self.SelectedEmployees = ko.observableArray([]);

    self.Search = function () {
        if (self.GridIsOpen())
            self.DestroyGrid();
        self.GridIsOpen(true);
        if (self.checkCallBack == "") {
            return;
        }
        self.checkCallBack();
    }

    self.checkCallBack = "";
    self.KendoGridOptions = {
        title: "Employee",
        target: "#empKendoGrid",
        url: "/Api/EmployeeApi/GetEmpKendoGrid",
        height: 490,
        multiselect: true,
        selectable: false,
        paramData: function () { return { BranchId: self.BranchId() } },
        group: true,
        //groupParam:{},// { field: "DesignationName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'DeviceCode', title: lang == "ne" ? "कर्मचारीको उपकरण कोड" : "Device Code", template: lang == "ne" ? '#=SuitableNumber(DeviceCode)#' : '#:DeviceCode#', width: 120 },
            { field: 'IdCardNo', title: lang == "ne" ? "कर्मचारीको कोड" : "Employee Code", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(IdCardNo)#' : '#:IdCardNo#', width: 120 },
            { field: 'EmployeeName', title: "Employee Name", width: 200, hidden: lang == "en" ? false : true },
            { field: 'EmployeeNameNp', title: "कर्मचारीको नाम", width: 200, hidden: lang == "ne" ? false : true, template: "#=EmployeeNameNp==null?EmployeeName:EmployeeNameNp#" },
            { field: 'DesignationName', title: lang == "ne" ? "पद" : "Designation" },
            { field: 'DepartmentName', title: lang == "ne" ? "विभाग" : "Department" },
            { field: 'Section', title: lang == "ne" ? "फाँट " : "Section" },
            { field: 'ShiftType', title: lang == "ne" ? "सिफ्ट प्रकार" : "ShiftType" },
            { field: 'Mobile', title: lang == "ne" ? "मोबाईल." : "Mobile", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(Mobile)#' : '#:Mobile#' },
            { field: 'Status', title: lang == "ne" ? "लागिन" : "Login", filterable: false },
            //{ field: 'IsOTAllowed', title: lang == "ne" ? "ओ टी प्रदान" : "OT Allow", filterable: false, template: '#=lang=="ne"?(IsOTAllowed==true?"छ":"छैन"):IsOTAllowed==true?"YES":"NO"#' }
        ],
        SelectedItem: function (item) {
            self.SelectedEmployee(new EmployeeGridVm(item));
        },
        SelectedItems: function (items) {
            var data = Riddha.ko.global.arrayMap(items, EmployeeGridVm);
            self.SelectedEmployees(data);
        },
        open: function (callback) {
            self.checkCallBack = callback;
        },
    }

    self.RefreshKendoGrid = function () {
        $("#empKendoGrid").getKendoGrid().dataSource.read();
    }

    self.DestroyGrid = function () {
        var grid = $("#empKendoGrid").getKendoGrid()
        grid.destroy();
    }

    self.NextTab = function myfunction() {
        $('#tabList > .active').next('li').find('a').trigger('click');
    }

    self.ShowShiftTypeModal = function () {
        if (self.SelectedEmployees().length < 1) {
            Riddha.UI.Toast("Select Employee to apply shift type", 0);
            return;
        }
        getShift();
        $("#shiftTypeModal").modal('show');
    }

    self.ShowPromotionModal = function () {
        if (self.SelectedEmployees().length < 1) {
            Riddha.UI.Toast("Please select employee.", 0);
            return;
        }
        $("#promotionModal").modal('show');
    }

    self.ApplyShiftType = function () {
        var ids = [];
        ko.utils.arrayForEach(self.SelectedEmployees(), function (item) {
            ids.push(item.Id());
        });
        var data = { ShiftTypeId: self.ShiftTypeId(), EmpIds: ids, ShiftId: self.ShiftManagementId() }
        Riddha.ajax.post(url + "/ApplyShifttype", data)
            .done(function (response) {
                if (response.Status == 4) {
                    $("#shiftTypeModal").modal('hide');
                    self.ShiftTypeId(0);
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(response.Message, response.Status);
            })
    }

    self.ApplyDesignationPromotion = function () {
        var ids = [];
        ko.utils.arrayForEach(self.SelectedEmployees(), function (item) {
            ids.push(item.Id());
        });
        var data = { PromotionDesignationId: self.PromotionDesignationId(), EmpIds: ids }
        Riddha.ajax.post(url + "/ApplyDesignationPromotion", data)
            .done(function (response) {
                if (response.Status == 4) {
                    $("#promotionModal").modal('hide');
                    self.PromotionDesignationId(0);
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(response.Message, response.Status);
            })
    }

    self.ShowOTManagementModal = function () {
        if (self.SelectedEmployees().length < 1) {
            Riddha.UI.Toast("Select Employee to manage OT", 0);
            return;
        }
        $("#oTManagementModal").modal('show');
    }

    self.CloseOTManagementModal = function () {
        self.OTManagement(new OTManagementModel());
        $("#oTManagementModal").modal('hide');
    }

    self.SaveOTConfig = function () {
        var ids = [];
        ko.utils.arrayForEach(self.SelectedEmployees(), function (item) {
            ids.push(item.Id());
        });
        Riddha.ajax.post(url + "/ManageOT", { OTModel: ko.toJS(self.OTManagement()), EmpIds: ids })
            .done(function (result) {
                if (result.Status == 4) {
                    self.CloseOTManagementModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            })
    }


    //Region ADMS
    self.deleteUserDev = function () {
        if (self.SelectedEmployee() == undefined) {
            Riddha.UI.Toast("Please select employee.");
            return;
        }
        Riddha.UI.Confirm("Confirm to delete user ?", function () {
            Riddha.ajax.get(admsUrl + "/deleteUserDev?empId=" + self.SelectedEmployee().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.deleteUserFaceDev = function () {
        if (self.SelectedEmployee() == undefined) {
            Riddha.UI.Toast("Please select employee.");
            return;
        }
        Riddha.UI.Confirm("Confirm to delete user face template ?", function () {
            Riddha.ajax.get(admsUrl + "/deleteUserFaceDev?empId=" + self.SelectedEmployee().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.deleteUserFpDev = function () {
        if (self.SelectedEmployee() == undefined) {
            Riddha.UI.Toast("Please select employee.");
            return;
        }
        Riddha.UI.Confirm("Confirm to delete user finger print ?", function () {
            Riddha.ajax.get(admsUrl + "/deleteUserFpDev?empId=" + self.SelectedEmployee().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.deleteUserPicDev = function () {
        if (self.SelectedEmployee() == undefined) {
            Riddha.UI.Toast("Please select employee.");
            return;
        }
        Riddha.UI.Confirm("Confirm to delete server data ?", function () {
            Riddha.ajax.get(admsUrl + "/deleteUserPicDev?empId=" + self.SelectedEmployee().Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };

    self.toNewDevice = function () {
        if (self.SelectedEmployee() == undefined) {
            Riddha.UI.Toast("Please select employee.", 0);
            return;
        }
        if (self.DeviceSN() == undefined) {
            //Riddha.UI.Toast("Please select destination device.",0);
            return;
        }
        Riddha.UI.Confirm("Confirm to send user to new device ?", function () {
            Riddha.ajax.get(admsUrl + "/toNewDevice?empId=" + self.SelectedEmployee().Id() + "&deviceSN=" + self.DeviceSN(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast("command executed. ", 4);
                        return;
                    } else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }
                });
        });
    };
    //End Region
}

//this function called from Leave Application
function getLeaveStatusTemp(id, name) {
    if (id == 2) {
        return "<span class='badge bg-red'>" + name + "</span>";
    }
    else if (id == 1) {
        return "<span class='badge bg-green'>" + name + "</span>";
    }
    else if (id == 3) {
        return "<span class='badge bg-red'>" + name + "</span>";
    }
    else {
        return "<span class='badge bg-orange'>" + name + "</span>";
    }
};

// yo function Leave Application kendo grid ma enum ko language anusar name lerauna use garey ko xa.. 
function getLeavedayTypeName(id) {
    var self = this;
    self.LeaveDay = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "पुरा दिन" : "Full Day" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "चाँडो बिदा" : "Early Leave" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "ढिलो बिदा " : "Late Leave" },
    ]);

    var mapped = ko.utils.arrayFirst(self.LeaveDay(), function (data) {
        return data.Id == id;
    });
    return mapped = (mapped || { Name: '' }).Name;
}

function getLeaveStatusTempForApproval(id, name) {
    if (id == 0) {
        return "<span class='badge bg-orange'>" + name + "</span>";
    }

}

function empLeaveOpeningBalController() {
    var self = this;
    var config = new Riddha.config();
    var url = "/Api/EmpLeaveOpeningBalApi";
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.EmpLeaveBalRows = ko.observable([]);
    self.OpBalViewModel = {
        Headers: ko.observableArray([]),
        EmpLeaveBalRows: ko.observableArray([])
    }

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
        self.CheckAllDepartments(false);
        self.CheckAllSections(false);
        self.Employees([]);
        getDepartments();
    };


    function getDepartments() {
        Riddha.ajax.get("/Api/ApproveReplacementLeaveApi/GetDepartments?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Departments(data);
            });
    };

    //getDepartments();
    //function getDepartments() {
    //    Riddha.ajax.get("/Api/AttendanceReportApi/GetDepartments", null)
    //        .done(function (result) {
    //            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
    //            self.Departments(data);
    //        });
    //};

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
            Riddha.ajax.get("/Api/ApproveReplacementLeaveApi/GetEmployeeBySection?id=" + sections + "&activeInactiveMode=" + 0)
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
    self.RefreshGrid = function () {
        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();
        if (departments.length < 1) {
            return Riddha.UI.Toast("Please Select Department to refresh table", 0);
        }
        Riddha.ajax.post(url + "/RefreshGrid", { DeptIds: departments, SectionIds: sections, EmpIds: employees })
            .done(function (result) {
                if (result.Status == 4) {
                    if (result.Data.EmpLeaveBalRows.length > 0) {
                        self.OpBalViewModel.Headers(result.Data.EmpLeaveBalRows[0].EmpLeaveBalColumns);
                    }
                    var data = Riddha.ko.global.arrayMap(result.Data.EmpLeaveBalRows, EmpLeaveBalRowModel);
                    self.OpBalViewModel.EmpLeaveBalRows(data);
                    $("#collapse").click();
                }
                else {
                    Riddha.UI.Toast(result.Message, result.Status);
                }
            })
    }

    self.Save = function () {
        Riddha.ajax.post(url, ko.toJS(self.OpBalViewModel))
            .done(function (response) {
                if (response.Status == 4) {

                }
                Riddha.UI.Toast(response.Message, response.Status);
            });
    }
}