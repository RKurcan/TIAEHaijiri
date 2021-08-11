/// <reference path="Riddha.Script.PayRoll.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="riddha.script.employeeinsuranceinfo.model.js" />

function payRollController() {
    var self = this;
    var url = "/Api/PayRollApi";
    var allowanceUrl = "/Api/AllowanceApi";
    var deductionUrl = "/Api/DeductionApi";
    self.PayRoll = ko.observable(new PayRollModel());
    self.PayRolls = ko.observableArray([]);
    self.Banks = ko.observableArray([]);
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);
    self.SelectedPayRoll = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.ModeOfButtonAllowance = ko.observable('Create');
    self.ModeOfButtonGradeGroup = ko.observable('Create');
    self.ModeOfButtonDeduction = ko.observable('Create');
    self.Allowance = ko.observable(new EmpAllowanceModel());
    self.Allowances = ko.observableArray([]);
    self.EmpAllowances = ko.observableArray([]);
    //employee grade upgrade models
    self.EmpGradeUpgrade = ko.observable(new EmpGradeUpgradeModel());
    self.EmpGrades = ko.observableArray([]);
    self.GradeGroups = ko.observableArray([]);
    self.LateDeductionBy = ko.observableArray(
        [
            { Id: 0, Name: "Days" },
            { Id: 1, Name: "HalfDay " },
            { Id: 2, Name: "Hour " },
            { Id: 3, Name: "SingleDay " }
        ]);

    self.EarlyDeductionBy = ko.observableArray(
        [
            { Id: 0, Name: "Days" },
            { Id: 1, Name: "HalfDay " },
            { Id: 2, Name: "Hour " },
            { Id: 3, Name: "SingleDay " }
        ]);

    self.AllowanceCalculatedByArr = ko.observableArray([{ Id: 0, Name: "Value" }, { Id: 1, Name: "Percentage" }]);
    self.GetAllowanceCalculatedName = function (id, PaidPer) {
        var maped = ko.utils.arrayFirst(self.AllowanceCalculatedByArr(), function (item) {
            return item.Id == id();
        });
        if (maped.Id == 0) {
            return "";
        }
        else {
            if (PaidPer() == 0) {
                return " % of Net Salary";
            } else {
                return " % of Basic Salary";
            }
        }
    }
    self.AllowancePaidPerArr = ko.observableArray([{ Id: 0, Name: "Net Salary" }, { Id: 1, Name: "Basic Salary" }]);
    self.AllownacePeriodArr = ko.observableArray([{ Id: 0, Name: "Hourly" }, { Id: 1, Name: "Daily" }, { Id: 2, Name: "Weekly" },
    { Id: 3, Name: "Monthly" }, { Id: 4, Name: "Yearly" }]);
    self.GetAllowancePaidPerName = function (id) {
        var maped = ko.utils.arrayFirst(self.AllowancePaidPerArr(), function (item) {
            return item.Id == id();
        });
        return maped.Name || "";
    }

    self.GetAllowancePeriod = function (id) {
        var mapped = ko.utils.arrayFirst(self.AllownacePeriodArr(), function (item) {
            return item.Id == id();
        });
        return mapped.Name;
    }
    self.GetMinWorkHour = function (minWorkHour) {

    }

    self.Deduction = ko.observable(new EmpDeductionModel());
    self.Deductions = ko.observableArray([]);
    self.EmpDeductions = ko.observableArray([]);
    self.DeductionCalculatedByArr = ko.observableArray([{ Id: 0, Name: "Value" }, { Id: 1, Name: "Percentage" }]);
    self.GetDeductionCalculatedName = function (id) {
        var maped = ko.utils.arrayFirst(self.DeductionCalculatedByArr(), function (item) {
            return item.Id == id();
        });
        if (maped.Id == 0) {
            return "";
        }
        else {
            return "%";
        }
    }
    self.DeductionPaidPerArr = ko.observableArray([{ Id: 0, Name: "Net Salary" }, { Id: 1, Name: "Basic Salary" }]);
    self.GetDeductionPaidPerName = function (id) {
        var maped = ko.utils.arrayFirst(self.DeductionPaidPerArr(), function (item) {
            return item.Id == id();
        });
        return maped.Name || "";
    }
    GetPayRolls();
    function GetPayRolls() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), PayRollModel);
                self.PayRolls(data);
            });
    };
    // getBank();
    getAllowances();

    function getAllowances() {
        Riddha.ajax.get(url + "/GetAllowanceForDropdown")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, AllowanceModel);
                self.Allowances(data);
            })
    }
    self.GetAllowanceName = function (id) {
        var allowance = Riddha.ko.global.find(self.Allowances, id);
        return allowance.Name || "";
    }
    self.GetGradeName = function (GradeGroupId) {
        var grade = Riddha.ko.global.find(self.GradeGroups, GradeGroupId);
        return grade.Name || "";
    }
    getDeductions();
    function getDeductions() {
        Riddha.ajax.get(url + "/GetDeductionForDropdown")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, DeductionModel);
                self.Deductions(data);
            })
    }
    self.GetDeductionName = function (id) {
        var deduction = Riddha.ko.global.find(self.Deductions, id);
        return deduction.Name || "";
    }
    self.SalaryPaidPer = ko.observableArray([
        { Id: 0, Name: "month" },
        { Id: 1, Name: "hour" },
    ]);

    self.PaymentBy = ko.observableArray([
        { Id: 0, Name: "Cash" },
        { Id: 1, Name: "Cheque" },
    ]);

    self.HRAPayPer = ko.observableArray([
        { Id: 0, Name: "Days" },
        { Id: 1, Name: "Hour" },
    ]);

    self.ConveyancePayPer = ko.observableArray([
        { Id: 0, Name: "Days" },
        { Id: 1, Name: "Hour" },
    ]);

    self.DApaidBy = ko.observableArray([
        { Id: 0, Name: "None" },
    ]);

    self.TdsPaidBy = ko.observableArray([
        { Id: 0, Name: "NetSalary" },
        { Id: 1, Name: "BasicSalary" },
    ]);

    self.TypeOfEmployee = ko.observableArray([
        { Id: 0, Name: "Permanent" },
        { Id: 1, Name: "Temprory" },
    ]);

    function getBank() {
        Riddha.ajax.get("/Api/BankApi", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), BankModel);
                self.Banks(data);
            });
    };

    self.GetBankName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Banks()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.GetPaymentByName = function (id) {
        var mapped = ko.utils.arrayFirst(self.PaymentBy(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };
    self.GetEmployee = function () {
        Riddha.ajax.get("/Api/PayRollApi/SearchEmployee/?empCode=" + self.Employee().Code() + "&empName=" + self.Employee().Name(), null)
            .done(function (result) {
                self.Employee(new EmpSearchViewModel(result.Data));
                self.PayRoll().EmployeeId(result.Data.Id);
            });
    };
    //get gradegroups in dropdown
    getGradeGroups();
    function getGradeGroups() {
        Riddha.ajax.get(url + "/GetGradeGroupsForDropdown")
            .done(function (response) {
                if (response.Status == 4) {
                    var data = Riddha.ko.global.arrayMap(response.Data, GlobalDropdownModel);
                    self.GradeGroups(data);
                }
            })
    }
    self.CreateUpdate = function () {
        if (self.PayRoll().BasicSalary.hasError()) {
            return Riddha.UI.Toast(self.PayRoll().BasicSalary.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        self.PayRoll().EmployeeId(self.SelectedEmployee().Id());
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post("/Api/PayRollApi", ko.toJS(self.PayRoll()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.PayRolls([]);
                        var data = Riddha.ko.global.arrayMap(result.Data, PayRollModel);
                        self.PayRolls(data);
                        self.Reset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put("/Api/PayRollApi", ko.toJS(self.PayRoll()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.PayRolls([]);
                        var data = Riddha.ko.global.arrayMap(result.Data, PayRollModel);
                        self.PayRolls(data);
                        self.Reset();
                        self.ModeOfButton("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        };
    };

    function CheckAllowanceIsDuplicate(AllowanceId) {

        var IsDuplicate = ko.utils.arrayFirst(self.EmpAllowances(), function (item) {
            if (AllowanceId == item.AllowanceId()) {
                return true;
            } else {
                return false;
            }
        });
        return IsDuplicate;
    }
    //allowance operations
    self.CreateUpdateAllowance = function () {
        if (self.Allowance().AllowanceId() == "") {
            return Riddha.util.localize.Required("Allowance");
        }
        if (self.Allowance().Value() == 0) {
            return Riddha.util.localize.Required("Value");
        }
        self.Allowance().EmployeeId(self.SelectedEmployee().Id());
        if (self.ModeOfButtonAllowance() == 'Create') {
            if (CheckAllowanceIsDuplicate(self.Allowance().AllowanceId())) {
                Riddha.UI.Toast("Allowance has already been added")
                return;
            }
            Riddha.ajax.post(allowanceUrl + "/AddEmpAllowance", self.Allowance())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmpAllowances.push(new EmpAllowanceModel(result.Data));
                        self.ResetAllowance();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else {
            Riddha.ajax.put(allowanceUrl + "/UpdateEmpAllowance", self.Allowance())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.GetEmpAllowance();
                        //self.EmpAllowances.replace(self.SelectedAllowance(), new AllowanceModel(result.Data));
                        self.ResetAllowance();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    }
    self.ResetAllowance = function () {
        self.Allowance(new EmpAllowanceModel({ Id: self.Allowance().Id() }));
        self.Allowance(new EmpAllowanceModel());
        self.ModeOfButtonAllowance('Create');
    }
    self.SelectAllowance = function (model) {
        self.Allowance(new EmpAllowanceModel(ko.toJS(model)));
        self.ModeOfButtonAllowance('Update');
    }
    self.DeleteAllowance = function (model) {
        Riddha.UI.Confirm("Confirm to delete this?", function () {
            Riddha.ajax.get(allowanceUrl + "/DeleteEmpAllowance?id=" + model.Id())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmpAllowances.remove(model);
                        self.ResetAllowance();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                })
        });
    }
    //deduction operaions
    self.CreateUpdateDeduction = function () {
        if (self.Deduction().DeductionId() == "") {
            return Riddha.util.localize.Required("Deduction");
        }
        if (self.Deduction().Value() == 0) {
            return Riddha.util.localize.Required("Value");
        }
        self.Deduction().EmployeeId(self.SelectedEmployee().Id());
        if (self.ModeOfButtonDeduction() == 'Create') {
            Riddha.ajax.post(deductionUrl + "/AddEmpDeduction", self.Deduction())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmpDeductions.push(new EmpDeductionModel(result.Data));
                        self.ResetDeduction();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else {
            Riddha.ajax.put(deductionUrl + "/UpdateEmpDeduction", self.Deduction())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.GetEmpDeduction();
                        //self.EmpDeductions.replace(self.SelectedDeduction(), new DeductionModel(result.Data));
                        self.ResetDeduction();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    }
    self.ResetDeduction = function () {
        self.Deduction(new EmpDeductionModel({ Id: self.Deduction().Id() }));
        self.Deduction(new EmpDeductionModel());
        self.ModeOfButtonDeduction('Create');
    }
    self.SelectDeduction = function (model) {
        self.Deduction(new EmpDeductionModel(ko.toJS(model)));
        self.ModeOfButtonDeduction('Update');
    }
    self.DeleteDeduction = function (model) {
        Riddha.UI.Confirm("Confirm to delete this?", function () {
            Riddha.ajax.get(deductionUrl + "/DeleteEmpDeduction?id=" + model.Id())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmpDeductions.remove(model);
                        self.ResetDeduction();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                })
        });
    }
    self.Select = function (model) {
        self.SelectedPayRoll(model);
        self.PayRoll(new PayRollModel(ko.toJS(model)));
        self.ModeOfButton('Update');
    };

    self.Delete = function (payRoll) {
        Riddha.UI.Confirm("Confirm to delete this?", function () {
            Riddha.ajax.delete(url + "/" + payRoll.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.PayRolls([]);
                        var data = Riddha.ko.global.arrayMap(result.Data, PayRollModel);
                        self.PayRolls(data);
                        self.ModeOfButton("Create");
                        self.Reset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.Reset = function () {
        self.PayRoll(new PayRollModel({ Id: self.PayRoll().Id(), EmployeeID: self.PayRoll().EmployeeId() }));
    }

    self.ShowModal = function () {
        if (self.SelectedEmployee() == undefined) {
            return Riddha.UI.Toast("Please Select employee for payroll", 0);
        }
        self.PayRolls([]);
        Riddha.ajax.get(url + "/GetPayrollHist?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    var data = Riddha.ko.global.arrayMap(result.Data, PayRollModel);
                    self.PayRolls(data);
                    $("#payRollCreationModel").modal('show');
                }
            });

        GetEmployeeInsuranceInfo();
    };

    $("#payRollCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
        self.ResetAllowance();
        $('a[href="#payroll"]').click();
    });

    self.CloseModal = function () {
        $("#payRollCreationModel").modal('hide');
        self.ResetAllowance();
        $('a[href="#payroll"]').click();
    };
    self.SelectedEmployee = ko.observable();
    self.SelectedEmployees = ko.observableArray([]);
    self.KendoGridOptions = {
        title: "Employee",
        target: "#empKendoGrid",
        url: url + "/GetEmpKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'IdCardNo', title: lang == "ne" ? "कर्मचारी कोड" : "Employee Code", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(IdCardNo)#' : '#:IdCardNo#' },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 225 },
            { field: 'DesignationName', title: lang == "ne" ? "पद" : "Designation", filterable: false },
            { field: 'DepartmentName', title: lang == "ne" ? "विभाग" : "Department", filterable: false },
            { field: 'Section', title: lang == "ne" ? "फाँट " : "Section", filterable: false },
            { field: 'GradeGroupName', title: lang == "ne" ? "फाँट " : "Grade Group", filterable: false }
        ],
        SelectedItem: function (item) {
            self.SelectedEmployee(new EmployeeGridVm(item));
        },
        SelectedItems: function (items) {
            var data = Riddha.ko.global.arrayMap(items, EmployeeGridVm);
            self.SelectedEmployees(data);
        }
    }
    self.GetEmpAllowance = function () {
        self.EmpAllowances([]);
        Riddha.ajax.get(allowanceUrl + "/GetEmpAllowance?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmpAllowanceModel);
                self.EmpAllowances(data);
            })
    }
    self.GetAllowanceDetail = function () {
        var allowanceId = self.Allowance().AllowanceId;
        var allowance = Riddha.ko.global.find(self.Allowances, allowanceId);
        self.Allowance(new EmpAllowanceModel(ko.toJS(allowance)));
        self.Allowance().AllowanceId(allowanceId());
    }

    self.GetEmpGradeUpgrade = function () {
        self.EmpGradeUpgrade(new EmpGradeUpgradeModel());
        self.EmpGrades([]);
        Riddha.ajax.get(url + "/GetEmpGradeUpgrades?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmpGradeUpgradeModel);
                self.EmpGrades(data);
            });
    }
    GetGradeUpgrades();
    function GetGradeUpgrades() {
        Riddha.ajax.get(url + "/GetGradeGroupsForDropdown")
            .done(function (response) {
                if (response.Status == 4) {
                    var data = Riddha.ko.global.arrayMap(response.Data, GlobalDropdownModel);
                    self.GradeGroups(data);
                }
            })
    }

    self.GetEmpDeduction = function () {
        self.EmpDeductions([]);
        Riddha.ajax.get(deductionUrl + "/GetEmpDeduction?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmpDeductionModel);
                self.EmpDeductions(data);
            })
    }
    self.GetDeductionDetail = function () {
        var DeductionId = self.Deduction().DeductionId;
        var Deduction = Riddha.ko.global.find(self.Deductions, DeductionId);
        self.Deduction(new EmpDeductionModel(ko.toJS(Deduction)));
        self.Deduction().DeductionId(DeductionId());
    }
    //region for employee grade upgrade
    self.ShowGradeUpgradeModel = function () {
        if (self.SelectedEmployees().length <= 0) {
            Riddha.UI.Toast("Select Employee for grade upgrade", 0);
            return;
        }
        self.EmpGradeUpgrade(new EmpGradeUpgradeModel());
        $("#gradeUpgradeModel").modal('show');

    }

    self.GradeUpgradeEmployee = function () {
        if (self.EmpGradeUpgrade().GradeGroupId() == undefined) {
            return Riddha.UI.Toast("Please select group", 0);
        }
        if (self.EmpGradeUpgrade().EffectedFrom() == "") {
            return Riddha.UI.Toast("Please enter From date", 0);
        }
        if (self.EmpGradeUpgrade().EffectedTo() == "") {
            return Riddha.UI.Toast("Please enter To date", 0);
        }
        var empIds = [];
        ko.utils.arrayForEach(self.SelectedEmployees(), function (data) {
            empIds.push(data.Id());
        });
        var data = { EmpIds: empIds, EmployeeGrade: self.EmpGradeUpgrade() };
        Riddha.ajax.post(url + "/GradeUpgradeEmployee", data)
            .done(function (response) {
                if (response.Status == 4) {
                    $("#gradeUpgradeModel").modal('hide');
                }
                Riddha.UI.Toast(response.Message, response.Status);
            });
    }
    self.ResetGradeGroups = function () {
        self.EmpGradeUpgrade(new EmpGradeUpgradeModel());
        self.ModeOfButtonGradeGroup('Create');
    }
    self.CreateUpdateGradeGroup = function () {
        if (self.EmpGradeUpgrade().GradeGroupId() == 0) {
            return Riddha.UI.Toast("Please select group", 0);
        }
        if (self.EmpGradeUpgrade().EffectedFrom() == "") {
            return Riddha.UI.Toast("Please enter From date", 0);
        }
        if (self.EmpGradeUpgrade().EffectedTo() == "") {
            return Riddha.UI.Toast("Please enter To date", 0);
        }
        self.EmpGradeUpgrade().EmployeeId(self.SelectedEmployee().Id());
        if (self.ModeOfButtonGradeGroup() == 'Create') {
            var empIds = [];
            ko.utils.arrayForEach(self.SelectedEmployees(), function (data) {
                empIds.push(data.Id());
            });
            var data = { EmpIds: empIds, EmployeeGrade: self.EmpGradeUpgrade() };
            Riddha.ajax.post(url + "/GradeUpgradeEmployee", data)
                .done(function (response) {
                    if (response.Status == 4) {
                        self.GetEmpGradeUpgrade();
                        self.ResetGradeGroups();
                    }
                    Riddha.UI.Toast(response.Message, response.Status);
                });
        }
        else {
            Riddha.ajax.put(url + "/UpdateEmpGradeGroup", self.EmpGradeUpgrade())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.GetEmpGradeUpgrade();
                        //self.EmpDeductions.replace(self.SelectedDeduction(), new DeductionModel(result.Data));
                        self.ResetGradeGroups();
                        self.ModeOfButtonGradeGroup("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    }

    self.SelectGradeGroup = function (empGradeGroup) {
        self.EmpGradeUpgrade(new EmpGradeUpgradeModel(ko.toJS(empGradeGroup)));
        self.ModeOfButtonGradeGroup('Update');
    }

    self.DeleteGradeGroup = function (empGradeGroup) {
        Riddha.UI.Confirm("Confirm to delete grade group ?", function () {
            Riddha.ajax.get(url + "/DeleteEmpGradeGroup/" + empGradeGroup.Id())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmpGrades([]);
                        var data = Riddha.ko.global.arrayMap(result.Data, EmpGradeUpgradeModel);
                        self.EmpGrades(data);
                        self.ModeOfButtonGradeGroup("Create");
                        self.Reset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    }

    // Region Insurance 

    self.EmployeeInsuranceInfo = ko.observable(new EmployeeInsuranceInfoModel());
    self.EmployeeInsuranceInfos = ko.observableArray([]);
    self.EmpInsuranceInfoSelectedEmployeeInsuranceInfo = ko.observable();
    self.EmpInsuranceInfoModeOfButton = ko.observable('Create');
    var EmployeeInsuranceInfoUrl = "/Api/EmployeeInsuranceInformationApi";
    self.Employees = ko.observableArray([]);
    self.InsuranceCompanies = ko.observableArray([]);
    GetEmployees();
    function GetEmployees() {

        Riddha.ajax.get(EmployeeInsuranceInfoUrl + "/GetEmployees").done(function (result) {

            if (result.Status == 4) {

                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Employees(data);
            }
        })
    }

    GetInsuranceCompanies();

    function GetInsuranceCompanies() {

        Riddha.ajax.get(EmployeeInsuranceInfoUrl + "/GetInsuranceCompanies").done(function (result) {

            if (result.Status == 4) {

                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.InsuranceCompanies(data);
            }
        })
    }

    self.GetInsuranceCompanyName = function (Id) {

        var val = ko.utils.arrayFirst(self.InsuranceCompanies(), function (data) {

            return data.Id() == Id;
        });
        return val.Name();
    }

    function GetEmployeeInsuranceInfo() {

        Riddha.ajax.get(EmployeeInsuranceInfoUrl + "/GetEmpInsuranceInfo?EmpId=" + self.SelectedEmployee().Id()).done(function (result) {

            if (result.Status == 4) {

                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmployeeInsuranceInfoModel);
                self.EmployeeInsuranceInfos(data);
            }
        })
    }

    self.EmpInsuranceInfoCreateUpdate = function () {
        self.EmployeeInsuranceInfo().EmployeeId(self.SelectedEmployee().Id());

        if (self.EmployeeInsuranceInfo().InsuranceCompanyId() == 0 || self.EmployeeInsuranceInfo().InsuranceCompanyId() == undefined) {
            Riddha.UI.Toast("Please Select Insurance Company..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().PolicyNo() == "") {
            Riddha.UI.Toast("Policy is required..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().PolicyAmount() == 0) {
            Riddha.UI.Toast("Policy Amount is required..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().PremiumAmount() == 0) {
            Riddha.UI.Toast("Premium Amount is required..", 0);
            return;
        }

        if (self.EmployeeInsuranceInfo().IssueDate() == "NaN/aN/aN") {
            Riddha.UI.Toast("IssueDate is required..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().ExpiryDate() == "NaN/aN/aN") {
            Riddha.UI.Toast("ExpiryDate is required..", 0);
            return;
        }

        if (self.EmployeeInsuranceInfo().IssueDate() > self.EmployeeInsuranceInfo().ExpiryDate()) {

            Riddha.UI.Toast("IssueDate cannot be greater than ExpiryDate ..", 0);
            return;
        }
        if (self.EmployeeInsuranceInfo().InsuraneDocument() == "") {
            Riddha.UI.Toast("Insurance Document is required..", 0);
            return;
        }

        if (self.EmpInsuranceInfoModeOfButton() == 'Create') {
            Riddha.ajax.post(EmployeeInsuranceInfoUrl, ko.toJS(self.EmployeeInsuranceInfo()))
                .done(function (result) {
                    if (result.Status == 4) {

                        self.EmpInsuranceInfoReset();

                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.EmpInsuranceInfoModeOfButton() == 'Update') {
            Riddha.ajax.put(EmployeeInsuranceInfoUrl, ko.toJS(self.EmployeeInsuranceInfo()))
                .done(function (result) {
                    if (result.Status == 4) {

                        self.EmpInsuranceInfoReset();

                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.EmpInsuranceInfoReset = function () {
        self.EmployeeInsuranceInfo(new EmployeeInsuranceInfoModel({ Id: self.EmployeeInsuranceInfo().Id() }));
        self.EmployeeInsuranceInfo().EmployeeId(self.SelectedEmployee().Id());
        GetEmployeeInsuranceInfo();
        self.EmpInsuranceInfoModeOfButton("Create");
    };

    self.EmpInsuranceInfoSelect = function (model) {



        self.EmployeeInsuranceInfo(new EmployeeInsuranceInfoModel(ko.toJS(model)));
        self.EmpInsuranceInfoModeOfButton('Update');

    };

    self.EmpInsuranceInfoDelete = function (model) {

        Riddha.UI.Confirm("Delete Confirm", function () {
            Riddha.ajax.delete(EmployeeInsuranceInfoUrl + "/" + model.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmpInsuranceInfoReset();
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });

    };
    // End Region 

    // region  Advance Salary 


    self.EmpAdvanceSalary = ko.observable(new EmpAdvanceSalaryModel());
    self.EmpAdvanceSalarys = ko.observableArray([]);
    self.SelectedEmpAdvanceSalary = ko.observable();
    self.EmpAdvanceSalaryModeOfButton = ko.observable('Create');
    var EmpAdvanceSalaryUrl = "/Api/AdvanceSalaryApi";
    
    function GetEmpAdvanceSalary() {

        Riddha.ajax.get(EmpAdvanceSalaryUrl + "?EmpId=" + self.SelectedEmployee().Id()).done(function (result) {

            if (result.Status == 4) {

                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmpAdvanceSalaryModel);
                self.EmpAdvanceSalarys(data);
            }
        })
    }
    function GetPreviousDue() {

        Riddha.ajax.get(EmpAdvanceSalaryUrl + "GetPreviousDue?EmpId=" + self.SelectedEmployee().Id()).done(function (result) {

            if (result.Status == 4) {

                var PreviousDue = result.Data;
                self.EmpAdvanceSalary().PreviousDue(PreviousDue);

            }
        })
    }
    
    self.EmpAdvanceSalaryCreateUpdate = function () {

        self.EmpAdvanceSalary().EmployeeId(self.SelectedEmployee().Id());

        
        if (self.EmpAdvanceSalary().RequestAmount() == 0) {
            Riddha.UI.Toast("Policy is required..", 0);
            return;
        }
        if (self.EmpAdvanceSalary().Interest() == 0) {
            Riddha.UI.Toast("Policy is required..", 0);
            return;
        }
        if (self.EmpAdvanceSalary().Installment() == 0) {
            Riddha.UI.Toast("Policy Amount is required..", 0);
            return;
        }
        if (self.EmpAdvanceSalary().InstallmentAmount() == 0) {
            Riddha.UI.Toast("Premium Amount is required..", 0);
            return;
        }

        if (self.EmpAdvanceSalary().RequestedDate() == "NaN/aN/aN") {
            Riddha.UI.Toast("IssueDate is required..", 0);
            return;

        }
        if (self.EmpAdvanceSalaryModeOfButton() == 'Create') {
            Riddha.ajax.post(EmpAdvanceSalaryUrl, ko.toJS(self.EmpAdvanceSalary()))
                .done(function (result) {
                    if (result.Status == 4) {

                        self.EmpAdvanceSalaryReset();
                        
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.EmpAdvanceSalaryModeOfButton() == 'Update') {
            Riddha.ajax.put(EmpAdvanceSalaryUrl, ko.toJS(self.EmpAdvanceSalary()))
                .done(function (result) {
                    if (result.Status == 4) {

                        self.EmpAdvanceSalaryReset();

                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };                                 

    self.EmpAdvanceSalaryReset = function () {
        self.EmpAdvanceSalary(new EmpAdvanceSalaryModel({ Id: self.EmpAdvanceSalary().Id() }));
        self.EmpAdvanceSalary().EmployeeId(self.SelectedEmployee().Id());
        GetEmpAdvanceSalary();
        self.EmpAdvanceSalaryModeOfButton("Create");
    };

    self.EmpAdvanceSalarySelect = function (model) {



        self.EmpAdvanceSalary(new EmpAdvanceSalaryModel(ko.toJS(model)));
        self.EmpAdvanceSalaryModeOfButton('Update');

    };

    self.EmpAdvanceSalaryDelete = function (model) {

        Riddha.UI.Confirm("Delete Confirm", function () {
            Riddha.ajax.delete(EmpAdvanceSalaryUrl + "/" + model.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmpAdvanceSalaryReset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });

    };


    self.EmpAdvanceSalaryCloseModal = function () {
        $("#AdvanceSalaryCreationModal").modal('hide');

    }


    self.EmpAdvanceSalaryShowModal = function () {
        if (self.SelectedEmployee() == undefined || self.SelectedEmployee().Id() == 0) {

            Riddha.UI.Toast("Please select the employee", 0);
            return;
        }
        $("#AdvanceSalaryCreationModal").modal('show');
        GetEmpAdvanceSalary();
        GetPreviousDue();
    }

    // End region 


    //Region Tax Info 

    //function GetTaxInfoOfCurrentFiscalYear() {

    //    Riddha.ajax.get(url + "/GetTaxInfoOfCurrentFiscalYear").done(function (result) {

    //        if (result.Status == 4) {
               
    //        };
    //    });
    //}
    // End region 

}

function allowanceController() {
    var self = this;
    var url = "/Api/AllowanceApi";
    self.Allowance = ko.observable(new AllowanceModel());
    self.Allowances = ko.observableArray([]);
    self.ModeOfButton = ko.observable('Create');
    self.SelectedAllowance = ko.observable();
    //self.VisiblePaidPer = ko.observable(false);
    self.MinWorkHour = ko.observable();
    self.ModalTitle = ko.observable();
    self.AllowancePaidPer = ko.observable();
    self.AllowanceCalculatedByArr = ko.observableArray([{ Id: 0, Name: "Value" }, { Id: 1, Name: "Percentage" }]);
    self.AllowancePeriodArr = ko.observableArray([{ Id: 0, Name: lang == "ne" ? "घण्टा" : "Hourly" },
    { Id: 1, Name: lang == "ne" ? "दैनिक" : "Daily" },
    { Id: 2, Name: lang == "ne" ? "साप्ताहिक" : "Weekly" },
    { Id: 3, Name: lang == "ne" ? "मासिक" : "Monthly" },
    { Id: 4, Name: lang == "ne" ? "वार्षिक" : "Annually" }]);

    self.AllowancePaidPerArr = ko.observableArray([{ Id: 0, Name: "Net Salary" }, { Id: 1, Name: "Basic Salary" }]);
    self.GetAllowanceCalculatedName = function (id) {
        var maped = ko.utils.arrayFirst(self.AllowanceCalculatedByArr(), function (item) {
            return item.Id == id();
        });
        if (maped.Id == 0) {
            return "";
        }
        else {
            return "%";
        }
    }

    self.GetAllowancePaidPerName = function (id) {
        if (id() == "") {
            return "";
        }
        var maped = ko.utils.arrayFirst(self.AllowancePaidPerArr(), function (item) {
            return item.Id == id();
        });
        return maped.Name || "";
    }

    getAllowanceGridList();
    function getAllowanceGridList() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, AllowanceModel);
                self.Allowances(data);
            });
    }
    self.KendoGridOptions = {
        title: "Allowance",
        target: "#allowanceKendoGrid",
        url: url + "/GetAllowanceKendoGrid",
        height: 500,
        paramData: {},
        selectable: true,
        multiSelect: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: lang == "ne" ? "कोड" : "Code" },
            { field: 'Name', title: lang == "ne" ? "नाम" : "Name" },
            { field: 'NameNp', title: lang == "ne" ? "नाम" : "NameNp" },
            { field: 'ValueName', title: lang == "ne" ? "मूल्य" : "Value" },
            { field: 'AllowanceCalculatedByName', title: lang == "ne" ? "गणना गरिएको" : "Calculated By" },
            { field: 'AllowancePaidPerName', title: lang == "ne" ? "प्रति भुक्तानी" : "Paid Per" },
            { field: 'AllowancePeriodName', title: lang == "ne" ? "अवधि" : "Period" },
            { field: 'MinimumWorkingHour', title: lang == "ne" ? "न्यूनतम कार्य घण्टा" : "Minimum Work Hour" },
        ],
        SelectedItem: function (item) {
            debugger;
            self.SelectedAllowance(new AllowanceGridVm(item));
            self.Allowance(new AllowanceModel(ko.toJS(item)));
            self.ModeOfButton('Update');
        },
        SelectedItems: function (items) {
        },
        //open: function (callBack) {
        //    self.GetOnDemandKendoGrid.Resignation = callBack;
        //}
    }
    self.CreateUpdate = function () {
        if (self.Allowance().Code() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Allowance().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.Allowance().Value() == 0) {
            return Riddha.util.localize.Required("Value");
        }
        if (self.ModeOfButton() == 'Create') {
            debugger;
            Riddha.ajax.post(url, self.Allowance())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Allowances.push(new AllowanceModel(result.Data));
                        self.RefreshKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else {
            if (self)
                Riddha.ajax.put(url, self.Allowance())
                    .done(function (result) {
                        if (result.Status == 4) {
                            self.Allowances.replace(self.SelectedAllowance(), new AllowanceModel(result.Data));
                            //getAllowanceGridList();
                            self.RefreshKendoGrid();
                            self.CloseModal();
                        }
                        Riddha.UI.Toast(result.Message, result.Status);
                    });
        }

    }

    self.Delete = function (item) {
        if (self.SelectedAllowance() == undefined) {
            return Riddha.UI.Toast("Please select allowance to delete", 0);
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + ko.toJS(item).Allowance.Id)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Allowances.remove(item);
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                })
        });
    }
    self.ShowModal = function (id, title) {
        if (id == 1) {
            self.Allowance(new AllowanceModel());
            self.ModeOfButton('Create');
        } else if (self.SelectedAllowance() == undefined) {
            return Riddha.UI.Toast("Please select allowance to edit", 0);
        }
        self.ModalTitle(title);
        $("#allowanceCreationModel").modal('show');
    }
    $("#allowanceCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });
    self.CloseModal = function () {
        $("#allowanceCreationModel").modal('hide');
        self.Reset();
    }
    self.Reset = function () {
        self.Allowance(new AllowanceModel({ Id: self.Allowance().Id() }));
        self.ModeOfButton('Create');
    }

    self.RefreshKendoGrid = function () {
        $("#allowanceKendoGrid").getKendoGrid().dataSource.read();
    };

}

function deductionController() {
    var self = this;
    var url = "/Api/DeductionApi";
    self.Deduction = ko.observable(new DeductionModel());
    self.Deductions = ko.observableArray([]);
    self.DeductionCalculatedByArr = ko.observableArray([{ Id: 0, Name: "Value" }, { Id: 1, Name: "Percentage" }]);
    self.GetDeductionCalculatedName = function (id) {
        var maped = ko.utils.arrayFirst(self.DeductionCalculatedByArr(), function (item) {
            return item.Id == id();
        });
        if (maped.Id == 0) {
            return "";
        }
        else {
            return "%";
        }
    }
    self.DeductionPaidPerArr = ko.observableArray([{ Id: 0, Name: "Net Salary" }, { Id: 1, Name: "Basic Salary" }]);
    self.GetDeductionPaidPerName = function (id) {
        var maped = ko.utils.arrayFirst(self.DeductionPaidPerArr(), function (item) {
            return item.Id == id();
        });
        return maped.Name || "";
    }
    self.ModeOfButton = ko.observable('Create');
    self.SelectedDeduction = ko.observable();
    getDeductionGridList();
    function getDeductionGridList() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, DeductionModel);
                self.Deductions(data);
            })
    }
    self.CreateUpdate = function () {
        if (self.Deduction().Code() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Deduction().Code() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.Deduction().Value() == 0) {
            return Riddha.util.localize.Required("Value");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, self.Deduction())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Deductions.push(new DeductionModel(result.Data));
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else {
            Riddha.ajax.put(url, self.Deduction())
                .done(function (result) {
                    if (result.Status == 4) {
                        //self.Deductions.replace(self.SelectedDeduction(), new DeductionModel(result.Data));
                        getDeductionGridList();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    }
    self.Select = function (model) {
        self.SelectedDeduction(ko.toJS(model));
        self.Deduction(new DeductionModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    }
    self.Delete = function (model) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + model.Id())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Deductions.remove(model);
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                })
        });
    }
    self.ShowModal = function () {
        $("#DeductionCreationModel").modal('show');
    }
    $("#DeductionCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });
    self.CloseModal = function () {
        $("#DeductionCreationModel").modal('hide');
        self.Reset();
    }
    self.Reset = function () {
        self.Deduction(new DeductionModel({ Id: self.Deduction().Id() }));
        self.ModeOfButton('Create');
    }
}

function payRollVerificaitonController() {
    var self = this;
    var payrollUrl = "/Api/PayrollApi";
    var allowanceUrl = "/Api/AllowanceApi";
    var deductionUrl = "/Api/DeductionApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.UnapprovedCount = ko.observable(new UnapprovedCountModel());

    //Payroll Model
    self.Payroll = ko.observable(new PayrollVerificationGridVm());
    self.Payrolls = ko.observableArray([]);
    self.SelectedPayroll = ko.observable(new PayrollVerificationGridVm());

    //Allowance Model
    self.Allowance = ko.observable(new AllowanceVerificationGridVm());
    self.Allowances = ko.observableArray([]);
    self.SelectedAllowance = ko.observable(new AllowanceVerificationGridVm());

    //Deduction Model
    self.Deduction = ko.observable(new DeductionVerificationGridVm());
    self.Deductions = ko.observableArray([]);
    self.SelectedDeduction = ko.observable(new DeductionVerificationGridVm());

    //GradeGroup Model
    self.GradeGroup = ko.observable(new EmpGradeUpgradeModel());
    self.GradeGroups = ko.observableArray([]);
    self.SelectedGradeGroup = ko.observable(new EmpGradeUpgradeModel());

    self.GetOnDemandKendoGrid = {
        Allowance: "",
        Deduction: "",
        GradeGroup: "",
    };

    self.TabClickLock = {
        Allowance: false,
        Deduction: false,
        GradeGroup: false,
    };

    getUnapprovedCount();
    function getUnapprovedCount() {
        Riddha.ajax.get(payrollUrl + "/GetUnapprovedCount")
            .done(function (result) {
                self.UnapprovedCount(new UnapprovedCountModel(result.Data));
            });
    }

    //salary verification
    self.KendoGridOptionForSalary = {
        title: "Salary",
        target: "#salaryKendoGrid",
        url: payrollUrl + "/GetPayrollKendoGrid",
        height: 350,
        paramData: {},
        selectable :true,
        multiSelect: true,
        group: false,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 225 },
            { field: 'Designation', title: lang == "ne" ? "कर्मचारीको नाम" : "Designation" },
            { field: 'Section', title: lang == "ne" ? "कर्मचारीको नाम" : "Section" },
            { field: 'EffectedFrom', title: lang == "ne" ? "कर्मचारी कोड" : "Effected From", template: "#=SuitableDate(EffectedFrom)#" },
            { field: 'BasicSalary', title: lang == "ne" ? "विभाग" : "Basic Salary" },
            { field: 'ApprovedOn', title: lang == "ne" ? "स्वीकृत गरिएको " : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "स्वीकृत गरेको" : "Approved By" }
        ],
        SelectedItem: function (item) {
            self.SelectedPayroll(new PayrollVerificationGridVm(item));
        },
        SelectedItems: function (items) {
        }
    };

    function getEmpPayrollByEmpId() {
        Riddha.ajax.get(payrollUrl + "/GetVerificationPayrollByEmpId?empId=" + self.SelectedPayroll().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedPayroll(new PayrollVerificationGridVm(ko.toJS(result.Data[0])));
                    self.Payroll(new PayrollVerificationGridVm(ko.toJS(result.Data[0])));
                }
            });
    };

    self.PayrollApprove = function (item) {
        //Riddha.UI.Confirm("DeleteConfirm", function () {

        if (self.SelectedPayroll().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedPayroll().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to Approve ??", function () {
            self.Payroll().Id(self.SelectedPayroll().Id());
            Riddha.ajax.get("/Api/PayrollApi/Approve?id=" + self.SelectedPayroll().Id() + "&empId=" + self.SelectedPayroll().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Payroll(parseInt(self.UnapprovedCount().Payroll()) - 1);
                        self.RefreshPayrollKendoGrid();
                        self.PayrollCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })

    };

    self.PayrollRevert = function (item) {
        if (self.SelectedPayroll().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedPayroll().ApprovedById() == null) {
            Riddha.UI.Toast("Allowance has not been approved yet. Can't revert an unapproved allowance", 0);
            return;
        };

        Riddha.UI.Confirm("Are u sure u want to Revert ??", function () {
            self.Payroll().Id(self.SelectedPayroll().Id());
            Riddha.ajax.get("/Api/PayrollApi/Revert?id=" + self.SelectedPayroll().Id() + "&empId=" + self.SelectedPayroll().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Payroll(parseInt(self.UnapprovedCount().Payroll()) + 1);
                        self.RefreshPayrollKendoGrid();
                        self.PayrollCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });

    }

    self.RefreshPayrollKendoGrid = function () {
        $("#salaryKendoGrid").getKendoGrid().dataSource.read();
    };

    self.PayrollShowModal = function () {
        if (self.SelectedPayroll().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getEmpPayrollByEmpId();
        $("#payrollViewModel").modal('show');
    };

    self.PayrollCloseModal = function () {
        $("#payrollViewModel").modal('hide');
    };

    //Allowance verification
    self.KendoGridOptionForAllowance = {
        title: "Allowance",
        target: "#allowanceKendoGrid",
        url: allowanceUrl + "/GetEmpAllowanceKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Employee', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 225 },
            { field: 'Designation', title: lang == "ne" ? "पद" : "Designation" },
            { field: 'Section', title: lang == "ne" ? "कर्मचारीको नाम" : "Section" },
            { field: 'Allowance', title: lang == "ne" ? "भत्ता" : "Allowance" },
            { field: 'Value', title: lang == "ne" ? "मूल्य" : "Value" },
            { field: 'AllowancePaidPer', title: lang == "ne" ? "प्रति भुक्तानी" : "Paid Per" },
            //{ field: 'AllowancePeriod', title: lang == "ne" ? "विभाग" : "Period" },
            { field: 'ApprovedOn', title: lang == "ne" ? "स्वीकृत गरिएको" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "स्वीकृत गरेको" : "Approved By" }
        ],
        SelectedItem: function (item) {
            self.SelectedAllowance(new AllowanceVerificationGridVm(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Allowance = callBack;
        }
    };

    function getEmpAllowanceByEmpId() {
        Riddha.ajax.get(allowanceUrl + "/GetVerificationAllowanceByEmpId?empId=" + self.SelectedAllowance().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    debugger;
                    self.SelectedAllowance(new AllowanceVerificationGridVm(ko.toJS(result.Data[0])));
                    self.Allowance(new AllowanceVerificationGridVm(ko.toJS(result.Data[0])));
                }
            });
    };


    self.AllowanceApprove = function (item) {
        if (self.SelectedAllowance().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedAllowance().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to Approve ??", function () {
            self.Allowance().Id(self.SelectedAllowance().Id());
            Riddha.ajax.get("/Api/AllowanceApi/Approve?id=" + self.SelectedAllowance().Id() + "&empId=" + self.SelectedAllowance().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Allowance(parseInt(self.UnapprovedCount().Allowance()) - 1);
                        self.RefreshAllowanceKendoGrid();
                        self.PayrollCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })

    };

    self.AllowanceRevert = function (item) {
        if (self.SelectedAllowance().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedAllowance().ApprovedById() == null) {
            Riddha.UI.Toast("Allownce has not been approved yet.", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to Revert ??", function () {
            self.Allowance().Id(self.SelectedAllowance().Id());
            Riddha.ajax.get("/Api/AllowanceApi/Revert?id=" + self.SelectedAllowance().Id() + "&empId=" + self.SelectedAllowance().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Allowance(parseInt(self.UnapprovedCount().Allowance()) + 1);
                        self.RefreshAllowanceKendoGrid();
                        self.PayrollCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })

    };

    self.RefreshAllowanceKendoGrid = function () {
        $("#allowanceKendoGrid").getKendoGrid().dataSource.read();
    };

    self.AllowanceShowModal = function () {
        if (self.SelectedAllowance().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getEmpAllowanceByEmpId();
        $("#allowanceViewModel").modal('show');
    };

    self.AllowanceCloseModal = function () {
        $("#allowanceViewModel").modal('hide');
    };

    //Deduction verification
    self.KendoGridOptionForDeduction = {
        title: "Deduction",
        target: "#deductionKendoGrid",
        url: deductionUrl + "/GetDeductionKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Employee', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 225 },
            { field: 'Designation', title: lang == "ne" ? "कर्मचारीको नाम" : "Designation" },
            { field: 'Section', title: lang == "ne" ? "कर्मचारीको नाम" : "Section" },
            { field: 'Deduction', title: lang == "ne" ? "कर्मचारी कोड" : "Deduction" },
            { field: 'Value', title: lang == "ne" ? "विभाग" : "Value" },
            { field: 'DeductionPaidPer', title: lang == "ne" ? "विभाग" : "Paid Per" },
            { field: 'ApprovedOn', title: lang == "ne" ? "स्वीकृत गरिएको " : "Approved On" },
            { field: 'ApprovedBy', title: lang == "ne" ? "स्वीकृत गरेको" : "Approved By" }
        ],
        SelectedItem: function (item) {
            self.SelectedDeduction(new DeductionVerificationGridVm(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Deduction = callBack;
        }
    };

    function getEmpDeductionByEmpId() {
        Riddha.ajax.get(deductionUrl + "/GetVerificationDeductionByEmpId?empId=" + self.SelectedDeduction().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedDeduction(new DeductionVerificationGridVm(ko.toJS(result.Data[0])));
                    self.Deduction(new DeductionVerificationGridVm(ko.toJS(result.Data[0])));
                }
            });
    };

    self.DeductionApprove = function (item) {
        if (self.SelectedDeduction().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedDeduction().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to Approve ??", function () {
            self.Deduction().Id(self.SelectedDeduction().Id());
            Riddha.ajax.get("/Api/DeductionApi/Approve?id=" + self.SelectedDeduction().Id() + "&empId=" + self.SelectedDeduction().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Deduction(parseInt(self.UnapprovedCount().Deduction()) - 1);
                        self.RefreshDeductionKendoGrid();
                        self.DeductionCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.DeductionRevert = function (item) {
        if (self.SelectedDeduction().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedDeduction().ApprovedById() == null) {
            Riddha.UI.Toast("Deduction not approved yet", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to Approve ??", function () {
            self.Deduction().Id(self.SelectedDeduction().Id());
            Riddha.ajax.get("/Api/DeductionApi/Revert?id=" + self.SelectedDeduction().Id() + "&empId=" + self.SelectedDeduction().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Deduction(parseInt(self.UnapprovedCount().Deduction()) + 1);
                        self.RefreshDeductionKendoGrid();
                        self.DeductionCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.RefreshDeductionKendoGrid = function () {
        $("#deductionKendoGrid").getKendoGrid().dataSource.read();
    };

    self.DeductionShowModal = function () {
        if (self.SelectedDeduction().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getEmpDeductionByEmpId();
        $("#deductionViewModel").modal('show');
    };

    self.DeductionCloseModal = function () {
        $("#deductionViewModel").modal('hide');
    };


    //Grade Group Verification

    self.KendoGridOptionForGradeGroup = {
        title: "Grade Group",
        target: "#gradeGroupKendoGrid",
        url: payrollUrl + "/GetGradeGroupKendoGrid",
        height: 350,
        paramData: {},
        selectable: true,
        multiSelect: true,
        selectable: true,
        group: false,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 225 },
            { field: 'GradeGroupName', title: lang == "ne" ? "कर्मचारी कोड" : "Grade Group", },
            { field: 'EffectedFrom', title: lang == "ne" ? "विभाग" : "EffectiveFrom", template: "#=SuitableDate(EffectedFrom)#" },
            { field: 'EffectedTo', title: lang == "ne" ? "विभाग" : "EffectiveTo", template: "#=SuitableDate(EffectedTo)#" },
            { field: 'ApprovedOn', title: lang == "ne" ? "स्वीकृत गरिएको " : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "स्वीकृत गरेको" : "Approved By" }
        ],
        SelectedItem: function (item) {
            self.SelectedGradeGroup(new EmpGradeUpgradeModel(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.GradeGroup = callBack;
        }
    };

    //function getEmpGradeGroupByEmpId() {
    //    Riddha.ajax.get(deductionUrl + "/GetVerificationGradeGroupByEmpId?empId=" + self.SelectedGradeGroup().EmployeeId())
    //    .done(function (result) {
    //        if (result.Data.length > 0) {
    //            self.SelectedGradeGroup(new GradeGroupVerificationGridVm(ko.toJS(result.Data[0])));
    //            self.Deduction(new DeductionVerificationGridVm(ko.toJS(result.Data[0])));
    //        }
    //    });
    //};

    self.GradeGroupApprove = function (item) {
        if (self.SelectedGradeGroup().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedGradeGroup().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to Approve ??", function () {
            self.GradeGroup().Id(self.SelectedGradeGroup().Id());
            Riddha.ajax.get(payrollUrl + "/ApproveGradeGroup?id=" + self.SelectedGradeGroup().Id())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().GradeGroup(parseInt(self.UnapprovedCount().GradeGroup()) - 1);
                        self.RefreshGradeGroupKendoGrid();
                        self.GradeGroupCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.GradeGroupRevert = function (item) {
        if (self.SelectedGradeGroup().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedGradeGroup().ApprovedById() == null) {
            Riddha.UI.Toast("Grade Group not approved yet", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to Revert ??", function () {
            self.Deduction().Id(self.SelectedDeduction().Id());
            Riddha.ajax.get(payrollUrl + "/RevertGradeGroup?id=" + self.SelectedGradeGroup().Id())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().GradeGroup(parseInt(self.UnapprovedCount().GradeGroup()) + 1);
                        self.RefreshGradeGroupKendoGrid();
                        self.GradeGroupCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.RefreshGradeGroupKendoGrid = function () {
        $("#gradeGroupKendoGrid").getKendoGrid().dataSource.read();
    };

    self.GradeGroupShowModal = function () {
        if (self.SelectedGradeGroup().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getEmpDeductionByEmpId();
        $("#gradeGroupViewModal").modal('show');
    };

    self.GradeGroupCloseModal = function () {
        $("#gradeGroupViewModal").modal('hide');
    };





    // End Region Insurance 

}