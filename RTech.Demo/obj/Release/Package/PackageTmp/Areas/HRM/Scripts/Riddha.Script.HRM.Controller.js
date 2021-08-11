/// <reference path="Riddha.Script.HRM.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />


//Employee Status
function employeeStatusController() {
    var self = this;
    var contractUrl = "/Api/ContractApi";
    var resignationUrl = "/Api/ResignationApi";
    var terminationUrl = "/Api/TerminationApi";
    var educationurl = "/Api/EducationApi";
    var skillurl = "/Api/SkillsApi";
    var licenseurl = "/Api/LicenseApi";
    var languageurl = "/Api/LanguageApi";
    var experienceurl = "/Api/ExperienceApi";
    var membershipurl = "/Api/MembershipApi";
    var empDocUrl = "/Api/EmployeeDocumentApi";
    var empOtherDocUrl = "/Api/EmployeeOtherDocumentApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ContractModeOfButton = ko.observable('Create');
    self.ResignationModeOfButton = ko.observable('Create');
    self.TerminationModeOfButton = ko.observable('Create');
    self.EmployeeEducationModeOfButton = ko.observable('Create');
    self.EmployeeSkillModeOfButton = ko.observable('Create');
    self.EmployeeLicenseModeOfButton = ko.observable('Create');
    self.EmployeeLanguageModeOfButton = ko.observable('Create');
    self.EmployeeExperienceModeOfButton = ko.observable('Create');
    self.EmployeeMembershipModeOfButton = ko.observable('Create');
    self.EmployeeOtherDocumentModeOfButton = ko.observable('Create');
    //self.SelectedStatus = ko.observable('all');
    //self.InitialStatus = ko.observable('all');
    self.Filter = ko.observable('all');


    self.RefreshGridFilterWise = function (filterBy) {
        if (self.checkCallBack == "") {
            return;
        }
        self.Filter(filterBy);
        self.checkCallBack();
    }

    //Contract & Employee Grid Model
    self.Contract = ko.observable(new ContractModel());
    self.Contracts = ko.observableArray([]);
    self.SelectedContract = ko.observable();
    self.SelectedEmployee = ko.observable(new EmployeeGridVm());
    self.EmploymentStatus = ko.observableArray([]);

    //Resignation Model
    self.Resignation = ko.observable(new ResignationModel);
    self.Resignations = ko.observableArray([]);
    self.SelectedResignation = ko.observable();
    self.ResignationEmployees = ko.observableArray([]);

    //Termination Model
    self.Termination = ko.observable(new TerminationModel);
    self.Terminations = ko.observableArray([]);
    self.SelectedTermination = ko.observable();
    self.TerminationEmployees = ko.observableArray([]);


    //Qualification
    //1st Education Model
    self.EmployeeEducation = ko.observable(new EmployeeEducationModel());
    self.EmployeeEducations = ko.observableArray([]);
    self.SelectedEducation = ko.observable();
    self.Education = ko.observable(new EducationSearchViewModel());
    self.EducationId = ko.observable(self.Education().Id || 0);

    //2nd Skilss Model
    self.EmployeeSkill = ko.observable(new EmployeeSkillModel());
    self.EmployeeSkills = ko.observableArray([]);
    self.SelectedEmployeeSkill = ko.observable();
    self.Skill = ko.observable(new EmployeeSkillSearchViewModel());
    self.SkillsId = ko.observable(self.Skill().Id || 0);

    //3rd License Model
    self.EmployeeLicense = ko.observable(new EmployeeLicenseModel());
    self.EmployeeLicenses = ko.observableArray([]);
    self.SelectedEmployeeLicense = ko.observable();
    self.License = ko.observable(new EmployeeLicenseSearchViewModel());
    self.LicenseId = ko.observable(self.License().Id || 0);

    //4th Language Model
    self.EmployeeLanguage = ko.observable(new EmployeeLanguageModel());
    self.EmployeeLanguages = ko.observableArray([]);
    self.SelectedEmployeeLanguage = ko.observable();
    self.Language = ko.observable(new EmployeeLanguageSearchViewModel());
    self.LanguageId = ko.observable(self.Language().Id || 0);


    //5th Experience Model
    self.EmployeeExperience = ko.observable(new ExperienceModel());
    self.EmployeeExperiences = ko.observableArray([]);
    self.SelectedEmployeeExperience = ko.observable();

    //6th Membership Model
    self.EmployeeMembership = ko.observable(new MembershipModel());
    self.EmployeeMemberships = ko.observableArray([]);
    self.SelectedEmployeeMembership = ko.observable();

    //7th Employee Documents Model
    self.EmployeeDoc = ko.observable(new EmpDocModel());


    //8th EmployeeOther Document
    self.EmployeeOtherDocument = ko.observable(new EmployeeOtherDocumentModel());
    self.EmployeeOtherDocuments = ko.observableArray([]);
    self.SelectedEmployeeOtherDocument = ko.observable();

    //Employee Grid List & Contract Crud operation Starts from Here
    getEmploymentStatus();
    function getEmploymentStatus() {
        Riddha.ajax.get(contractUrl + "/GetEmploymentStatusForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.EmploymentStatus(data);
            });
    }
    self.checkCallBack = "";
    self.KendoGridOptions = {
        title: "Employee",
        target: "#empKendoGrid",
        url: contractUrl + "/GetEmpKendoGrid",
        height: 490,
        paramData: function () { return { Type: self.Filter() } },
        multiSelect: true,
        group: true,
        selectable: true,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'IdCardNo', title: lang == "ne" ? "कर्मचारी कोड" : "Employee Code", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(IdCardNo)#' : '#:IdCardNo#' },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 225 },
            { field: 'DepartmentName', title: lang == "ne" ? "विभाग" : "Department" },
            { field: 'Section', title: lang == "ne" ? "फाँट " : "Section" },
            { field: 'DesignationName', title: lang == "ne" ? "फाँट " : "Designation" },
            { field: 'EmploymentStatusName', title: lang == "ne" ? "" : "Employment Status" },
        ],
        SelectedItem: function (item) {
            self.SelectedEmployee(new EmployeeGridVm(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callback) {
            self.checkCallBack = callback;
        },
        autoOpen: true
    };
    self.RefreshKendoGrid = function () {
        $("#empKendoGrid").getKendoGrid().dataSource.read();
    }


    self.ContractApprovecount = ko.observable(0);
    self.ResignationApproveCount = ko.observable(0);
    self.TerminationApproveCount = ko.observable(0);
    self.EducationApproveCount = ko.observable(0);
    self.ResignationApproveCount = ko.observable(0);



    function getEmpContractByEmpId() {
        Riddha.ajax.get(contractUrl + "/GetContractsByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, ContractModel);
                self.Contracts(data);
            });
    };

    self.CreateUpdate = function () {
        self.Contract().EmployeeId(self.SelectedEmployee().Id());
        if (self.Contract().EmploymentStatusId() == undefined) {
            Riddha.util.localize.Required("EmploymentStatus");
            return;
        };
        if (self.Contract().BeganOn() == "NaN/aN/aN") {
            Riddha.util.localize.Required("BeganOn");
            return;
        }
        if (self.Contract().EndedOn() == "NaN/aN/aN") {
            Riddha.util.localize.Required("EndedOn");
            return;
        }
        if (self.ContractModeOfButton() == 'Create') {
            Riddha.ajax.post(contractUrl, ko.toJS(self.Contract()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpContractByEmpId();
                        self.Reset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ContractModeOfButton() == 'Update') {
            Riddha.ajax.put(contractUrl, ko.toJS(self.Contract()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpContractByEmpId();
                        self.Reset();
                        self.ContractModeOfButton("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                    ;
                });
        }
    };

    self.Reset = function () {
        self.Contract(new ContractModel({ Id: self.Contract().Id() }));
    };

    self.SelectContract = function (model) {
        self.SelectedContract(new ContractModel(ko.toJS(model)));
        self.Contract(new ContractModel(ko.toJS(model)));
        self.ContractModeOfButton('Update');
    };

    self.Delete = function (contract) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(contractUrl + "/" + contract.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Contracts.remove(contract)
                    };
                    self.ContractModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.CreateEmpContract = function () {
        if (self.SelectedEmployee().Id() == 0) {
            Riddha.UI.Toast("Please select employee to Contract", 0);
            return;
        }
        getEmpContractByEmpId();
        $("#contractCreationModel").modal('show');
    };

    $("#contractCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#contractCreationModel").modal('hide');
        self.ContractModeOfButton("Create");
    };

    self.ActionVisibility = function (actionCode, approvedById) {
        if (Riddha.global.permission.validateAction(actionCode) && approvedById() == null)
            return true;
        else
            return false;
    }


    //Resignation Crud operation Starts from Here
    function getResignationForwardToEmployees() {
        Riddha.ajax.get(resignationUrl + "/GetEmployeesForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.ResignationEmployees(data);
            });
    };

    function getEmpResignationByEmpId() {
        Riddha.ajax.get(resignationUrl + "/GetResignationByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, ResignationModel);
                //self.Resignations(data);
                if (result.Data.length > 0) {
                    self.SelectedResignation(new ResignationModel(ko.toJS(result.Data[0])));
                    self.Resignation(new ResignationModel(ko.toJS(result.Data[0])));
                    self.ResignationModeOfButton('Update');
                }
            });
    };

    self.CreateUpdateResignation = function () {
        self.Resignation().EmployeeId(self.SelectedEmployee().Id());
        if (self.Resignation().Code() == "") {
            return Riddha.util.localize.Required("Code");
        };
        if (self.Resignation().NoticeDate() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("NoticeDate");
        };
        if (self.Resignation().ForwardToId() == undefined) {
            return Riddha.util.localize.Required("ForwardTo");
        };
        if (self.Resignation().DesiredResignDate() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("DesiredResignDate");
        }
        if (self.Resignation().Reason() == "") {
            return Riddha.util.localize.Required("Reason");
        };
        if (self.ResignationModeOfButton() == 'Create') {
            Riddha.ajax.post(resignationUrl, ko.toJS(self.Resignation()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpResignationByEmpId();
                        self.ResetResignation();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ResignationModeOfButton() == 'Update') {
            Riddha.ajax.put(resignationUrl, ko.toJS(self.Resignation()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpResignationByEmpId();
                        self.ResetResignation();
                        self.ResignationModeOfButton("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                    ;
                });
        }
    };

    self.ResetResignation = function () {
        self.Resignation(new ResignationModel({ Id: self.Resignation().Id() }));
    };

    self.SelectResignation = function (model) {
        self.SelectedResignation(new ResignationModel(ko.toJS(model)));
        self.Resignation(new ResignationModel(ko.toJS(model)));
        self.ResignationModeOfButton('Update');
    };

    self.DeleteResignation = function (resignation) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(contractUrl + "/" + resignation.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Resignations.remove(resignation)
                    };
                    self.ResignationModeOfButton("Create");
                    self.ResetResignation();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };
    getResignationForwardToEmployees();
    self.CreateEmpResignation = function () {
        if (self.SelectedEmployee().Id() == 0) {
            Riddha.UI.Toast("Please select employee to resignation", 0);
            return;
        }
        getEmpResignationByEmpId();

        $("#resignationCreationModel").modal('show');

    };

    $("#resignationCreationModel").on('hidden.bs.modal', function () {
        self.ResetResignation();
        self.ResignationModeOfButton("Create");
    });

    self.ResignationCloseModal = function () {
        $("#resignationCreationModel").modal('hide');
        self.ResignationModeOfButton("Create");
    };

    //Termination Crud operation Starts from Here
    self.ChangeStatus = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "कर्मचारी" : "Employee" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "प्रशिक्षार्थी" : "Intern" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "सम्झौता आधार" : "ContractBasis" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "स्थायी" : "Permanent" },
    ]);

    function getTerminationForwardToEmployees() {
        Riddha.ajax.get(terminationUrl + "/GetEmployeesForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.TerminationEmployees(data);
            });
    };

    function getEmpTerminationByEmpId() {
        Riddha.ajax.get(terminationUrl + "/GetTerminationByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                //self.Terminations(data);
                if (result.Data.length > 0) {
                    self.SelectedTermination(new TerminationModel(ko.toJS(result.Data[0])));
                    self.Termination(new TerminationModel(ko.toJS(result.Data[0])));
                    self.TerminationModeOfButton('Update');
                }
            });
    };

    self.CreateUpdateTermination = function () {
        self.Termination().EmployeeId(self.SelectedEmployee().Id());
        if (self.Termination().NoticeDate() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("Notice Date");
        };
        if (self.Termination().ForwardToId() == undefined) {
            return Riddha.util.localize.Required("Forward To");
        };
        if (self.Termination().ServiceEndDate() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("Service End Date");
        }
        if (self.Termination().Reason() == "") {
            return Riddha.util.localize.Required("Reason");
        };
        if (self.TerminationModeOfButton() == 'Create') {
            Riddha.ajax.post(terminationUrl, ko.toJS(self.Termination()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpTerminationByEmpId();
                        self.ResetTermination();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.TerminationModeOfButton() == 'Update') {
            Riddha.ajax.put(terminationUrl, ko.toJS(self.Termination()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpTerminationByEmpId();
                        self.ResetTermination();
                        self.TerminationModeOfButton("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                    ;
                });
        }
    };

    self.ResetTermination = function () {
        self.Termination(new TerminationModel({ Id: self.Termination().Id() }));
    };

    self.SelectTermination = function (model) {
        self.SelectedTermination(new TerminationModel(ko.toJS(model)));
        self.Termination(new TerminationModel(ko.toJS(model)));
        self.TerminationModeOfButton('Update');
    };

    self.DeleteTermination = function (termination) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(terminationUrl + "/" + termination.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Terminations.remove(termination)
                    };
                    self.TerminationModeOfButton("Create");
                    self.ResetTermination();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.CreateEmpTermination = function () {
        if (self.SelectedEmployee().Id() == 0) {
            Riddha.UI.Toast("Please select employee to termination", 0);
            return;
        }
        getEmpTerminationByEmpId();
        getTerminationForwardToEmployees();
        $("#terminationCreationModel").modal('show');

    };

    $("#terminationCreationModel").on('hidden.bs.modal', function () {
        self.ResetTermination();
        self.TerminationModeOfButton("Create");
    });

    self.TerminationCloseModal = function () {
        $("#terminationCreationModel").modal('hide');
        self.TerminationModeOfButton("Create");
    };

    //Qualification 
    //1st Education

    self.GetEducation = function () {
        if (self.Education().Code() != '' || self.Education().Name() != '') {
            Riddha.ajax.get("/Api/EducationApi/SearchEducation/?eduCode=" + self.Education().Code() + "&eduName=" + self.Education().Name(), null)
                .done(function (result) {
                    self.Education(new EducationSearchViewModel(result.Data));
                    self.EmployeeEducation().EducationId(result.Data.Id);
                });
        } else
            return Riddha.UI.Toast("Please Enter Education Code To Search", 2);
    };

    self.EducationAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/EducationApi/GetEduLstForAutoComplete",
        select: function (item) {
            self.Education(new EducationSearchViewModel(item));
            self.EmployeeEducation().EducationId(item.Id);
        },
        placeholder: lang == "ne" ? "शिक्षा छान्नुहोस" : "Search Education"
    };

    function getEmpEducationByEmpId() {
        Riddha.ajax.get(educationurl + "/GetEducationByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmployeeEducationGridVm);
                self.EmployeeEducations(data);
            });
    };

    self.EmployeeEducationCreateUpdate = function () {
        self.EmployeeEducation().EmployeeId(self.SelectedEmployee().Id());
        if (self.EmployeeEducation().EducationId() == 0) {
            Riddha.UI.Toast("Please select Education", 0);
            return;
        }
        if (self.EmployeeEducationModeOfButton() == 'Create') {
            Riddha.ajax.post(educationurl + "/CreateEmpEducation", ko.toJS(self.EmployeeEducation()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpEducationByEmpId();
                        self.ResetEmployeeEducation();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.EmployeeEducationModeOfButton() == 'Update') {
            Riddha.ajax.put(educationurl + "/UpdateEmpEducation", ko.toJS(self.EmployeeEducation()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpEducationByEmpId();
                        self.ResetEmployeeEducation();
                        self.EmployeeEducationModeOfButton("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                    ;
                });
        }
    };

    self.EmployeeEducationSelect = function (model) {
        self.SelectedEducation(model);
        self.EmployeeEducation(new EmployeeEducationModel(ko.toJS(model)));
        self.Education(new EducationSearchViewModel({ Code: self.SelectedEducation().EducationCode(), Name: self.SelectedEducation().EducationName() }));
        self.GetEducation();
        self.EmployeeEducationModeOfButton('Update');
    };

    self.ResetEmployeeEducation = function () {
        self.EmployeeEducation(new EmployeeEducationModel({ Id: self.EmployeeEducation().Id() }));
        self.Education(new EducationSearchViewModel());
    };

    self.DeleteEmployeeEducation = function (employeeEducation) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.get(educationurl + "/DeleteEmpEducation?id=" + employeeEducation.Id(), null)
                .done(function (result) {
                    self.EmployeeEducations.remove(employeeEducation)
                    self.EmployeeEducationModeOfButton("Create");
                    self.ResetEmployeeEducation();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    //2nd Skills
    self.GetSkill = function () {
        if (self.Skill().Code() != '' || self.Skill().Name() != '') {
            Riddha.ajax.get("/Api/SkillsApi/SearchSkills/?skillCode=" + self.Skill().Code() + "&skillName=" + self.Skill().Name(), null)
                .done(function (result) {
                    self.Skill(new EmployeeSkillSearchViewModel(result.Data));
                    self.EmployeeSkill().SkillsId(result.Data.Id);
                });
        } else
            return Riddha.UI.Toast("Please Enter Skill Code To Search", 2);
    };

    self.SkillAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/SkillsApi/GetSkillLstForAutoComplete",
        select: function (item) {
            self.Skill(new EmployeeSkillSearchViewModel(item));
            self.EmployeeSkill().SkillsId(item.Id);
        },
        placeholder: lang == "ne" ? "कौशल छान्नुहोस" : "Search Skill"
    };

    function getEmpSkillByEmpId() {
        Riddha.ajax.get(skillurl + "/GetSkillByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmployeeSkillGridVm);
                self.EmployeeSkills(data);
            });
    };

    self.EmployeeSkillCreateUpdate = function () {
        self.EmployeeSkill().EmployeeId(self.SelectedEmployee().Id());
        if (self.EmployeeSkill().SkillsId() == 0) {
            Riddha.UI.Toast("Please select Skill", 0);
            return;
        };
        if (self.EmployeeSkillModeOfButton() == 'Create') {
            Riddha.ajax.post(skillurl + "/CreateEmpSkill", ko.toJS(self.EmployeeSkill()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpSkillByEmpId();
                        self.ResetEmployeeSkill();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.EmployeeSkillModeOfButton() == 'Update') {
            Riddha.ajax.put(skillurl + "/UpdateEmpSkill", ko.toJS(self.EmployeeSkill()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpSkillByEmpId();
                        self.ResetEmployeeSkill();
                        self.EmployeeSkillModeOfButton("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                    ;
                });
        }
    };

    self.EmployeeSkillSelect = function (model) {
        self.SelectedEmployeeSkill(model);
        self.EmployeeSkill(new EmployeeSkillModel(ko.toJS(model)));
        self.Skill(new EmployeeSkillSearchViewModel({ Code: self.SelectedEmployeeSkill().SkillCode(), Name: self.SelectedEmployeeSkill().SkillName() }));
        self.GetSkill();
        self.EmployeeSkillModeOfButton('Update');
    };

    self.ResetEmployeeSkill = function () {
        self.EmployeeSkill(new EmployeeSkillModel({ Id: self.EmployeeSkill().Id() }));
        self.Skill(new EmployeeSkillSearchViewModel());
    };

    self.DeleteEmployeeSkill = function (employeeSkill) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.get(skillurl + "/DeleteEmpSkill?id=" + employeeSkill.Id(), null)
                .done(function (result) {
                    self.EmployeeSkills.remove(employeeSkill)
                    self.EmployeeSkillModeOfButton("Create");
                    self.ResetEmployeeSkill();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    //3rd License
    self.GetLicense = function () {
        if (self.License().Code() != '' || self.License().Name() != '') {
            Riddha.ajax.get("/Api/LicenseApi/SearchLicense/?licenseCode=" + self.License().Code() + "&licenseName=" + self.License().Name(), null)
                .done(function (result) {
                    self.License(new EmployeeLicenseSearchViewModel(result.Data));
                    self.EmployeeLicense().LicenseId(result.Data.Id);
                });
        } else
            return Riddha.UI.Toast("Please Enter License Code To Search", 2);
    };

    self.LicenseAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/LicenseApi/GetLicenseLstForAutoComplete",
        select: function (item) {
            self.License(new EmployeeLicenseSearchViewModel(item));
            self.EmployeeLicense().LicenseId(item.Id);
        },
        placeholder: lang == "ne" ? "लाइसेन्स छान्नुहोस" : "Search License"
    };

    function getEmpLicenseByEmpId() {
        Riddha.ajax.get(licenseurl + "/GetLicenseByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmployeeLicenseGridVm);
                self.EmployeeLicenses(data);
            });
    };

    self.EmployeeLicenseCreateUpdate = function () {
        self.EmployeeLicense().EmployeeId(self.SelectedEmployee().Id());
        if (self.EmployeeLicense().LicenseId() == 0) {
            Riddha.UI.Toast("Please select License", 0);
            return;
        }
        if (self.EmployeeLicenseModeOfButton() == 'Create') {
            Riddha.ajax.post(licenseurl + "/CreateEmpLicense", ko.toJS(self.EmployeeLicense()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpLicenseByEmpId();
                        self.ResetEmployeeLicense();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.EmployeeLicenseModeOfButton() == 'Update') {
            Riddha.ajax.put(licenseurl + "/UpdateEmpLicense", ko.toJS(self.EmployeeLicense()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpLicenseByEmpId();
                        self.ResetEmployeeLicense();
                        self.EmployeeLicenseModeOfButton("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                    ;
                });
        }
    };

    self.EmployeeLicenseSelect = function (model) {
        self.SelectedEmployeeLicense(model);
        self.EmployeeLicense(new EmployeeLicenseModel(ko.toJS(model)));
        self.License(new EmployeeLicenseSearchViewModel({ Code: self.SelectedEmployeeLicense().LicenseCode(), Name: self.SelectedEmployeeLicense().LicenseName() }));
        self.GetLicense();
        self.EmployeeLicenseModeOfButton('Update');
    };

    self.ResetEmployeeLicense = function () {
        self.EmployeeLicense(new EmployeeLicenseModel({ Id: self.EmployeeLicense().Id() }));
        self.License(new EmployeeLicenseSearchViewModel());
    };

    self.DeleteEmployeeLicense = function (employeeLicense) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.get(licenseurl + "/DeleteEmpLicense?id=" + employeeLicense.Id(), null)
                .done(function (result) {
                    self.EmployeeLicenses.remove(employeeLicense)
                    self.EmployeeLicenseModeOfButton("Create");
                    self.ResetEmployeeLicense();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };


    //4th Language
    self.GeLanguage = function () {
        if (self.Language().Code() != '' || self.Language().Name() != '') {
            Riddha.ajax.get("/Api/LanguageApi/SearchLanguage/?languageCode=" + self.Language().Code() + "&languageName=" + self.Language().Name(), null)
                .done(function (result) {
                    self.Language(new EmployeeLanguageSearchViewModel(result.Data));
                    self.EmployeeLanguage().LanguageId(result.Data.Id);
                });
        } else
            return Riddha.UI.Toast("Please Enter Language Code To Search", 2);
    };

    self.LanguageAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/LanguageApi/GetLanguageLstForAutoComplete",
        select: function (item) {
            self.Language(new EmployeeLanguageSearchViewModel(item));
            self.EmployeeLanguage().LanguageId(item.Id);
        },
        placeholder: lang == "ne" ? "भाषा छान्नुहोस" : "Search Language"
    };

    function getEmpLanguageByEmpId() {
        Riddha.ajax.get(languageurl + "/GetLanguageByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmployeeLanguageGridVm);
                self.EmployeeLanguages(data);
            });
    };

    self.EmployeeLanguageCreateUpdate = function () {
        self.EmployeeLanguage().EmployeeId(self.SelectedEmployee().Id());
        if (self.EmployeeLanguage().LanguageId() == 0) {
            Riddha.UI.Toast("Please select Language", 0);
            return;
        }
        if (self.EmployeeLanguageModeOfButton() == 'Create') {
            Riddha.ajax.post(languageurl + "/CreateEmpLanguage", ko.toJS(self.EmployeeLanguage()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpLanguageByEmpId();
                        self.ResetEmployeeLanguage();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.EmployeeLanguageModeOfButton() == 'Update') {
            Riddha.ajax.put(languageurl + "/UpdateEmpLanguage", ko.toJS(self.EmployeeLanguage()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmpLanguageByEmpId();
                        self.ResetEmployeeLanguage();
                        self.EmployeeLanguageModeOfButton("Create");
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                    ;
                });
        }
    };

    self.EmployeeLanguageSelect = function (model) {
        self.SelectedEmployeeLanguage(model);
        self.EmployeeLanguage(new EmployeeLanguageModel(ko.toJS(model)));
        self.Language(new EmployeeLanguageSearchViewModel({ Code: self.SelectedEmployeeLanguage().LanguageCode(), Name: self.SelectedEmployeeLanguage().LanguageName() }));
        self.GeLanguage();
        self.EmployeeLanguageModeOfButton('Update');
    };

    self.ResetEmployeeLanguage = function () {
        self.EmployeeLanguage(new EmployeeLanguageModel({ Id: self.EmployeeLanguage().Id() }));
        self.Language(new EmployeeLanguageSearchViewModel());
    };

    self.DeleteEmployeeLanguage = function (employeeLanguage) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.get(languageurl + "/DeleteEmpLanguage?id=" + employeeLanguage.Id(), null)
                .done(function (result) {
                    self.EmployeeLanguages.remove(employeeLanguage)
                    self.EmployeeLanguageModeOfButton("Create");
                    self.ResetEmployeeLanguage();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    //5th Experience

    function getEmpExperienceByEmpId() {
        Riddha.ajax.get(experienceurl + "/GetExperienceByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, ExperienceModel);
                self.EmployeeExperiences(data);
            });
    };

    self.EmployeeExperienceCreateUpdate = function () {
        if (self.EmployeeExperience().Title() == "") {
            Riddha.util.localize.Required("Title");
            return;
        }
        if (self.EmployeeExperience().OrganizationName() == "") {
            Riddha.util.localize.Required("OrganizationName");
            return;
        }
        if (self.EmployeeExperience().BeganOn() == "NaN/aN/aN") {
            Riddha.util.localize.Required("BeganOn");
            return;
        }
        if (self.EmployeeExperience().EndedOn() == "NaN/aN/aN") {
            Riddha.util.localize.Required("EndedOn");
            return;
        }
        self.EmployeeExperience().EmployeeId(self.SelectedEmployee().Id());
        if (self.EmployeeExperienceModeOfButton() == 'Create') {
            Riddha.ajax.post(experienceurl, ko.toJS(self.EmployeeExperience()))
                .done(function (result) {
                    self.EmployeeExperiences.push(new ExperienceModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ResetEmployeeExperience();
                });
        }
        else if (self.EmployeeExperienceModeOfButton() == 'Update') {
            Riddha.ajax.put(experienceurl, ko.toJS(self.EmployeeExperience()))
                .done(function (result) {
                    self.EmployeeExperiences.replace(self.SelectedEmployeeExperience(), new ExperienceModel(ko.toJS(self.EmployeeExperience())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.EmployeeExperienceModeOfButton("Create");
                    self.ResetEmployeeExperience();
                });
        };
    };

    self.ResetEmployeeExperience = function () {
        self.EmployeeExperience(new ExperienceModel({ Id: self.EmployeeExperience().Id() }));
        self.EmployeeExperienceModeOfButton("Create");
    };

    self.SelectEmployeeExperience = function (model) {
        self.SelectedEmployeeExperience(model);
        self.EmployeeExperience(new ExperienceModel(ko.toJS(model)));
        self.EmployeeExperienceModeOfButton('Update');
    };

    self.DeleteEmployeeExperience = function (employeeExperience) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(experienceurl + "/" + employeeExperience.Id(), null)
                .done(function (result) {
                    self.EmployeeExperiences.remove(employeeExperience)
                    self.EmployeeExperienceModeOfButton("Create");
                    self.ResetEmployeeExperience();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    //6th Membership 

    function getEmpMembershipByEmpId() {
        Riddha.ajax.get(membershipurl + "/GetMembershipByEmpId?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, MembershipModel);
                self.EmployeeMemberships(data);
            });
    };

    self.EmployeeMembershipCreateUpdate = function () {
        if (self.EmployeeMembership().Name() == "") {
            Riddha.util.localize.Required("Name");
            return;
        }
        if (self.EmployeeMembership().Description() == "") {
            Riddha.util.localize.Required("Description");
            return;
        }
        self.EmployeeMembership().EmployeeId(self.SelectedEmployee().Id());
        if (self.EmployeeMembershipModeOfButton() == 'Create') {
            Riddha.ajax.post(membershipurl, ko.toJS(self.EmployeeMembership()))
                .done(function (result) {
                    self.EmployeeMemberships.push(new MembershipModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ResetEmployeeMembership();
                });
        }
        else if (self.EmployeeMembershipModeOfButton() == 'Update') {
            Riddha.ajax.put(membershipurl, ko.toJS(self.EmployeeMembership()))
                .done(function (result) {
                    self.EmployeeMemberships.replace(self.SelectedEmployeeMembership(), new MembershipModel(ko.toJS(self.EmployeeMembership())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.EmployeeMembershipModeOfButton("Create");
                    self.ResetEmployeeMembership();
                });
        };
    };

    self.ResetEmployeeMembership = function () {
        self.EmployeeMembership(new MembershipModel({ Id: self.EmployeeMembership().Id() }));
        self.EmployeeMembershipModeOfButton("Create");
    };

    self.SelectEmployeeMembership = function (model) {
        self.SelectedEmployeeMembership(model);
        self.EmployeeMembership(new MembershipModel(ko.toJS(model)));
        self.EmployeeMembershipModeOfButton('Update');
    };

    self.DeleteEmployeeMembership = function (employeeMembership) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(membershipurl + "/" + employeeMembership.Id(), null)
                .done(function (result) {
                    self.EmployeeMemberships.remove(employeeMembership)
                    self.EmployeeMembershipModeOfButton("Create");
                    self.ResetEmployeeMembership();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    //Qualification Modal show & hide Section

    self.CreateEmpQualification = function () {
        if (self.SelectedEmployee().Id() == 0) {
            Riddha.UI.Toast("Please select employee..", 0);
            return;
        }
        getEmpEducationByEmpId();
        getEmpSkillByEmpId();
        getEmpLicenseByEmpId();
        getEmpLanguageByEmpId();
        getEmpExperienceByEmpId();
        getEmpMembershipByEmpId();
        $("#qualificationCreationModel").modal('show');
    };

    self.CreateEmpOtherInfo = function () {
        if (self.SelectedEmployee().Id() == 0) {
            Riddha.UI.Toast("Please select employee..", 0);
            return;
        }
        getEmpSkillByEmpId();
        getEmpLicenseByEmpId();
        getEmpLanguageByEmpId();
        getEmpExperienceByEmpId();
        getEmpMembershipByEmpId();
        $("#empOtherInfoCreationModel").modal('show');
    };

    self.empOtherInfoCloseModal = function () {
        $("#empOtherInfoCreationModel").modal('hide');
    };

    $("#qualificationCreationModel").on('hidden.bs.modal', function () {
        //self.ResetTermination();
        //self.TerminationModeOfButton("Create");
    });

    self.TerminationCloseModal = function () {
        $("#qualificationCreationModel").modal('hide');
    };
    self.NextTab = function myfunction() {
        $('#tabList > .active').next('li').find('a').trigger('click');
    };
    self.ShowEmpDocModal = function () {
        if (self.SelectedEmployee().Id() == 0) {
            Riddha.UI.Toast("Please select employee for document", 0);
            return;
        }

        Riddha.ajax.get(empDocUrl + "?empId=" + self.SelectedEmployee().Id())
            .done(function (response) {
                if (response.Status == 4) {
                    self.EmployeeDoc(new EmpDocModel(response.Data));
                    getEmployeeOtherDocumentByEmployeeId();
                    $("#empDocModel").modal('show');
                }
            })
    }
    $("#empDocModel").on('hidden.bs.modal', function () {
        self.ResetEmpDoc();
    });
    self.CloseEmpDocModal = function () {
        $("#empDocModel").modal('hide');
    }
    self.CreateUpdateEmpDoc = function () {
        if (self.EmployeeDoc().Id() == 0) {
            self.EmployeeDoc().EmployeeId(self.SelectedEmployee().Id());
            Riddha.ajax.post(empDocUrl, self.EmployeeDoc())
                .done(function (response) {
                    if (response.Status == 4) {
                        self.ResetEmpDoc();
                        self.CloseEmpDocModal();
                    }
                    Riddha.UI.Toast(response.Message, response.Status);
                })
        }
        else {
            Riddha.ajax.put(empDocUrl, self.EmployeeDoc())
                .done(function (response) {
                    if (response.Status == 4) {
                        self.ResetEmpDoc();
                        self.CloseEmpDocModal();
                    }
                    Riddha.UI.Toast(response.Message, response.Status);
                })
        }
    }
    self.ResetEmpDoc = function () {
        self.EmployeeDoc(new EmpDocModel({ Id: self.EmployeeDoc().Id(), EmployeeId: self.EmployeeDoc().EmployeeId() }));
    }
    self.ExportEmpExcel = function () {
        window.open("/HRM/Contract/ExportEmpExcel?filter=" + self.Filter());
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
        Riddha.UI.Confirm("ExcelUploadConfirm", function () {
            xhr.open("POST", "/api/EmployeeDocumentApi/Upload");
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


    //Employee Other Document Added on 2019-02-17 by Raz

    function getEmployeeOtherDocumentByEmployeeId() {
        Riddha.ajax.get(empOtherDocUrl + "/GetEmployeeOtherDocument?empId=" + self.SelectedEmployee().Id())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(result.Data, EmployeeOtherDocumentModel);
                self.EmployeeOtherDocuments(data);
            });
    };

    self.CreateUpdateEmployeeOtherDocument = function () {
        if (self.EmployeeOtherDocument().FileName() == "") {
            Riddha.util.localize.Required("Name");
            return;
        }
        if (self.EmployeeOtherDocument().FileUrl() == "") {
            Riddha.util.localize.Required("File");
            return;
        }
        self.EmployeeOtherDocument().EmployeeId(self.SelectedEmployee().Id());
        if (self.EmployeeOtherDocumentModeOfButton() == 'Create') {
            Riddha.ajax.post(empOtherDocUrl, ko.toJS(self.EmployeeOtherDocument()))
                .done(function (result) {
                    self.EmployeeOtherDocuments.push(new EmployeeOtherDocumentModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ResetEmployeeOtherDocument();
                });
        }
        else if (self.EmployeeOtherDocumentModeOfButton() == 'Update') {
            Riddha.ajax.put(empOtherDocUrl, ko.toJS(self.EmployeeOtherDocument()))
                .done(function (result) {
                    self.EmployeeOtherDocuments.replace(self.SelectedEmployeeOtherDocument(), new EmployeeOtherDocumentModel(ko.toJS(self.EmployeeOtherDocument())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ResetEmployeeOtherDocument();
                });
        };
    };

    self.ResetEmployeeOtherDocument = function () {
        self.EmployeeOtherDocument(new EmployeeOtherDocumentModel({ Id: self.EmployeeOtherDocument().Id() }));
        self.EmployeeOtherDocumentModeOfButton("Create");
    };

    self.SelectEmployeeOtherDocument = function (model) {
        self.SelectedEmployeeOtherDocument(model);
        self.EmployeeOtherDocument(new EmployeeOtherDocumentModel(ko.toJS(model)));
        self.EmployeeOtherDocumentModeOfButton('Update');
    };

    self.DeleteEmployeeOtherDocument = function (employeeOtherDocument) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(empOtherDocUrl + "/" + employeeOtherDocument.Id(), null)
                .done(function (result) {
                    self.EmployeeOtherDocuments.remove(employeeOtherDocument)
                    self.EmployeeOtherDocumentModeOfButton("Create");
                    self.ResetEmployeeOtherDocument();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

}

//Employment Status
function employmentStatusController() {
    var self = this;
    var url = "/Api/EmploymentStatusApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.EmploymentStatus = ko.observable(new EmploymentStatusModel());
    self.EmploymentStatuses = ko.observableArray([]);
    self.SelectedEmploymentStatus = ko.observable();
    self.EmploymentStatusWiseLeaveLst = ko.observableArray([]);

    self.EmploymentStatusEnum = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "समान्य रोजगार" : "NormalEmployment" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "मृतक" : "Deceased" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "पूर्वनिर्धारित" : "Defaulter" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "समाप्त" : "Terminated" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "राजीना" : "Resigned" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "प्रारम्भिक अवकाश" : "EarlyRetirement" },
        { Id: 6, Name: config.CurrentLanguage == 'ne' ? "समान्य अवकाश" : "NormalRetirement" },
        { Id: 7, Name: config.CurrentLanguage == 'ne' ? "सम्झौता अवधि समाप्त" : "ContractPeriodOver" },
        { Id: 8, Name: config.CurrentLanguage == 'ne' ? "सम्झौतामा" : "OnContract" },
        { Id: 9, Name: config.CurrentLanguage == 'ne' ? "स्थायी काम" : "PermanentJob" },
        { Id: 10, Name: config.CurrentLanguage == 'ne' ? "रिटायरइङ" : "Retiring" },
    ]);

    self.ApplicableGender = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सबै" : "All" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "पुरुष" : "Male" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "महिला" : "Female" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "अन्य" : "Others" },

    ]);

    self.LeaveIncreamentPeriods = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "बार्सिक" : "Yearly" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "मासिक" : "Monthly" },
    ]);

    self.GetApplicableGenderName = function (id) {
        var mapped = ko.utils.arrayFirst(self.ApplicableGender(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    getEmploymentStatus();

    function getEmploymentStatus() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmploymentStatusModel);
                self.EmploymentStatuses(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.EmploymentStatus().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.EmploymentStatus()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmploymentStatus();
                        self.Reset();
                        self.CloseModal();
                    };
                    Riddha.UI.Toast(result.Message, result.Status);

                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.EmploymentStatus()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getEmploymentStatus();
                        self.ModeOfButton("Create");
                        self.Reset();
                        self.CloseModal();
                    };
                    Riddha.UI.Toast(result.Message, result.Status);

                });
        };
    };

    self.Reset = function () {
        self.EmploymentStatus(new EmploymentStatusModel({ Id: self.EmploymentStatus().Id() }));
    };

    self.Select = function (model) {
        self.SelectedEmploymentStatus(model);
        self.EmploymentStatus(new EmploymentStatusModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (employmentStatus) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + employmentStatus.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.EmploymentStatuses.remove(employmentStatus)
                    };
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.LeaveQuataFor = ko.observable('');
    self.LeaveQuata = function (model) {
        self.LeaveQuataFor("For " + model.Name());
        GetEmploymentStatusWiseLeaveLst(model.Id());
        self.ShowLeaveQuataModal();
    };

    function GetEmploymentStatusWiseLeaveLst(employmentStatusId) {
        Riddha.ajax.get(url + "/GetEmploymentStatusWiseLeaveQuota?employmentStatusId=" + employmentStatusId)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmploymentStatusWiseLeavedBalanceModel);
                self.EmploymentStatusWiseLeaveLst(data);
            });
    };

    self.ApplyLeaveQuota = function () {
        var data = { LeaveQuota: self.EmploymentStatusWiseLeaveLst() };
        Riddha.ajax.post(url + "/ApplyLeaveQuota", ko.toJS(data))
            .done(function (result) {
                if (result.Status == 4) {
                    self.CloseLeaveQuotaModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }

    self.ShowLeaveQuataModal = function () {
        $("#LeaveQuataCreationModal").modal('show');
    };

    $("#LeaveQuataCreationModal").on('hidden.bs.modal', function () {
        self.ResetLeaveQuota();
    });

    self.CloseLeaveQuotaModal = function () {
        $("#LeaveQuataCreationModal").modal('hide');
        self.ResetLeaveQuota();
    };

    self.ResetLeaveQuota = function () {
        self.EmploymentStatusWiseLeaveLst([]);
    }

    self.ShowModal = function () {
        $("#employmentStatusCreationModel").modal('show');
    };

    $("#employmentStatusCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#employmentStatusCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
}

//Employee Verification
function verificationController() {
    var self = this;
    var contractUrl = "/Api/ContractApi";
    var resignationUrl = "/Api/ResignationApi";
    var terminationUrl = "/Api/TerminationApi";
    var educationUrl = "/Api/EducationApi";
    var skillUrl = "/Api/SkillsApi";
    var experienceUrl = "/Api/experienceApi";
    var licenseUrl = "/Api/LicenseApi";
    var languageUrl = "/Api/LanguageApi";
    var membershipUrl = "/Api/MembershipApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;

    //Contract Model
    self.Contract = ko.observable(new ContractVerificationGridVm());
    self.Contracts = ko.observableArray([]);
    self.SelectedContract = ko.observable(new ContractVerificationGridVm());
    self.UnapprovedCount = ko.observable(new UnapprovedCountModel());

    //Resignation Model
    self.Resignation = ko.observable(new ResignationVerificationGridVm());
    self.Resignations = ko.observableArray([]);
    self.SelectedResignation = ko.observable(new ResignationVerificationGridVm());

    //Termination Model
    self.Termination = ko.observable(new TerminationVerificationGridVm);
    self.Terminations = ko.observableArray([]);
    self.SelectedTermination = ko.observable(new TerminationVerificationGridVm());

    //Qualification Model
    //1st Education
    self.Education = ko.observable(new QualificationModel());
    self.Educations = ko.observableArray([]);
    self.SelectedEducation = ko.observable(new QualificationModel());

    //2nd Skill
    self.Skill = ko.observable(new QualificationModel());
    self.Skills = ko.observableArray([]);
    self.SelectedSkill = ko.observable(new QualificationModel());

    //3rd License
    self.License = ko.observable(new QualificationModel());
    self.Licenses = ko.observableArray([]);
    self.SelectedLicense = ko.observable(new QualificationModel());

    //4th Language
    self.Language = ko.observable(new QualificationModel());
    self.Languages = ko.observableArray([]);
    self.SelectedLanguage = ko.observable(new QualificationModel());

    //5th Membership
    self.Membership = ko.observable(new MembershipVm());
    self.Memberships = ko.observableArray([]);
    self.SelectedMembership = ko.observable(new MembershipVm());

    //6th Experience
    self.Experience = ko.observable(new ExperienceVm());
    self.Experiences = ko.observableArray([]);
    self.SelectedExperience = ko.observable(new ExperienceVm());

    getUnapprovedCount();
    function getUnapprovedCount() {
        Riddha.ajax.get(contractUrl + "/GetUnapprovedCount")
            .done(function (result) {
                self.UnapprovedCount(new UnapprovedCountModel(result.Data));
            });
    }
    //Contract verification
    self.KendoGridOptionForContracts = {
        title: "Contract",
        target: "#contractKendoGrid",
        url: contractUrl + "/GetContractKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: lang == "ne" ? "कर्मचारीको नाम" : "Code" },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 225 },
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारी कोड" : "Employee Code", template: lang == "ne" ? '#=GetNepaliUnicodeNumber(EmployeeCode)#' : '#:EmployeeCode#' },
            { field: 'EmploymentStatusName', title: lang == "ne" ? "विभाग" : "Employment Status" },
            { field: 'Period', title: lang == "ne" ? "समय" : "Period", width: 150 },
            { field: 'ApprovedOn', title: lang == "ne" ? "समय" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedContract(new ContractVerificationGridVm(item));
        },
        SelectedItems: function (items) {
        }
    };

    function getEmpContractByEmpId() {
        Riddha.ajax.get(contractUrl + "/GetVerificationContractsByEmpId?empId=" + self.SelectedContract().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedContract(new ContractVerificationGridVm(ko.toJS(result.Data[0])));
                    self.Contract(new ContractVerificationGridVm(ko.toJS(result.Data[0])));
                }
            });
    };


    self.Approve = function (item) {
        if (self.SelectedContract().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedContract().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        self.Contract().Id(self.SelectedContract().Id());
        Riddha.ajax.get("/Api/ContractApi/Approve?id=" + self.SelectedContract().Id() + "&empId=" + self.SelectedContract().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().Contract(parseInt(self.UnapprovedCount().Contract()) - 1);
                    self.RefreshContractKendoGrid();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleRevert = ko.computed(function () {
        if (self.SelectedContract().ApprovedById() == null) {
            return false;
        }
        else {
            return true;
        }
    }, this);

    self.Revert = function (item) {
        if (self.SelectedContract().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedContract().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to Revert ??", function () {
            self.Contract().Id(self.SelectedContract().Id());
            Riddha.ajax.get("/Api/ContractApi/Revert?id=" + self.SelectedContract().Id() + "&empId=" + self.SelectedContract().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Contract(parseInt(self.UnapprovedCount().Contract()) + 1);
                        self.RefreshContractKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    }

    self.RefreshContractKendoGrid = function () {
        $("#contractKendoGrid").getKendoGrid().dataSource.read();
    };

    self.ShowModal = function () {
        if (self.SelectedContract().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getEmpContractByEmpId();
        $("#contractViewModel").modal('show');
    };

    self.CloseModal = function () {
        $("#contractViewModel").modal('hide');
    };
    self.GetOnDemandKendoGrid = {
        Resignation: "",
        Termination: "",
        Education: "",
        Skill: "",
        Experience: "",
        License: "",
        Membership: "",
        Language: ""
    };
    self.TabClickLock = {
        Resignation: false,
        Termination: false,
        Education: false,
        Skill: false,
        Experience: false,
        License: false,
        Membership: false,
        Language: false
    };
    //Resignation verification
    self.KendoGridOptionForResignations = {
        title: "Contract",
        target: "#resignationKendoGrid",
        url: resignationUrl + "/GetResignationKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'Code', title: lang == "ne" ? "कर्मचारीको" : "Code", width: 150 },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 225 },
            { field: 'NoticeDate', title: lang == "ne" ? "विभाग" : "Notice Date", template: "#=SuitableDate(NoticeDate)#" },
            { field: 'DesiredResignDate', title: lang == "ne" ? "समय" : "Desired Resign Date", template: "#=SuitableDate(DesiredResignDate)#" },
            { field: 'Reason', title: lang == "ne" ? "कर्मचारीको" : "Reason", width: 225 },
            { field: 'ApprovedOn', title: lang == "ne" ? "समय" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedResignation(new ResignationVerificationGridVm(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Resignation = callBack;
        }
    };

    function getEmpResignationByEmpId() {
        Riddha.ajax.get(resignationUrl + "/GetVerificationResignationsByEmpId?empId=" + self.SelectedResignation().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedResignation(new ResignationVerificationGridVm(ko.toJS(result.Data[0])));
                    self.Resignation(new ResignationVerificationGridVm(ko.toJS(result.Data[0])));
                }
            });
    };

    self.ResignationApprove = function (item) {
        if (self.SelectedResignation().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedResignation().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };

        self.Resignation().Id(self.SelectedResignation().Id());
        Riddha.ajax.get("/Api/ResignationApi/Approve?id=" + self.SelectedResignation().Id() + "&empId=" + self.SelectedResignation().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().Resignation(parseInt(self.UnapprovedCount().Resignation()) - 1);
                    self.RefreshResignationKendoGrid();
                    self.ResignationCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleResignationRevert = ko.computed(function () {
        if (self.SelectedResignation().ApprovedById() == null) {
            return false;
        }
        else {
            return true;
        }
    }, this);

    self.ResignationRevert = function (item) {
        if (self.SelectedResignation().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedResignation().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };
        Riddha.UI.Confirm("Are u sure u want to revert ?", function () {
            self.Resignation().Id(self.SelectedResignation().Id());
            Riddha.ajax.get("/Api/ResignationApi/Revert?id=" + self.SelectedResignation().Id() + "&empId=" + self.SelectedResignation().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Resignation(parseInt(self.UnapprovedCount().Resignation()) + 1);
                        self.RefreshResignationKendoGrid();
                        self.ResignationCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });

    };

    self.RefreshResignationKendoGrid = function () {
        $("#resignationKendoGrid").getKendoGrid().dataSource.read();
    };

    self.ResignationShowModal = function () {
        if (self.SelectedResignation().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getEmpResignationByEmpId();
        $("#resignationViewModel").modal('show');
    };

    self.ResignationCloseModal = function () {
        $("#resignationViewModel").modal('hide');
    };



    //Termination verification
    self.KendoGridOptionForTerminations = {
        title: "Termination",
        target: "#terminationKendoGrid",
        url: terminationUrl + "/GetTerminationKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'Code', title: lang == "ne" ? "कर्मचारीको" : "Code", width: 150 },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 200 },
            { field: 'NoticeDate', title: lang == "ne" ? "विभाग" : "Notice Date", template: "#=SuitableDate(NoticeDate)#" },
            { field: 'ServiceEndDate', title: lang == "ne" ? "समय" : "Service End Date", template: "#=SuitableDate(ServiceEndDate)#" },
            { field: 'Reason', title: lang == "ne" ? "कर्मचारीको" : "Reason", width: 150 },
            { field: 'ChangeStatusName', title: lang == "ne" ? "कर्मचारीको" : "Change Status" },
            { field: 'ApprovedOn', title: lang == "ne" ? "समय" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedTermination(new TerminationVerificationGridVm(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Termination = callBack;
        }
    };

    function getEmpTerminationByEmpId() {
        Riddha.ajax.get(terminationUrl + "/GetVerificationTerminationsByEmpId?empId=" + self.SelectedTermination().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedTermination(new TerminationVerificationGridVm(ko.toJS(result.Data[0])));
                    self.Termination(new TerminationVerificationGridVm(ko.toJS(result.Data[0])));
                }
            });
    };


    self.TerminationApprove = function (item) {
        if (self.SelectedTermination().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedTermination().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        self.Termination().Id(self.SelectedTermination().Id());
        Riddha.ajax.get("/Api/TerminationApi/Approve?id=" + self.SelectedTermination().Id() + "&empId=" + self.SelectedTermination().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().Termination(parseInt(self.UnapprovedCount().Termination()) - 1);
                    self.RefreshTerminationKendoGrid();
                    self.TerminationCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleTerminationRevert = ko.computed(function () {
        if (self.SelectedTermination().ApprovedById() == null) {
            return false;
        }
        else {
            return true;
        }
    }, this);

    self.TerminationRevert = function (item) {
        if (self.SelectedTermination().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedTermination().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };

        Riddha.UI.Confirm("Are you sure you want to revert ?", function () {
            self.Termination().Id(self.SelectedTermination().Id());
            Riddha.ajax.get("/Api/TerminationApi/Revert?id=" + self.SelectedTermination().Id() + "&empId=" + self.SelectedTermination().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().Termination(parseInt(self.UnapprovedCount().Termination()) + 1);
                        self.RefreshTerminationKendoGrid();
                        self.TerminationCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    };

    self.RefreshTerminationKendoGrid = function () {
        $("#terminationKendoGrid").getKendoGrid().dataSource.read();
    };

    self.TerminationShowModal = function () {
        if (self.SelectedTermination().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getEmpTerminationByEmpId();
        $("#terminationViewModal").modal('show');
    };

    self.TerminationCloseModal = function () {
        $("#terminationViewModal").modal('hide');
    };

    //Qualification
    //1st Education
    self.KendoGridOptionForEducations = {
        title: "Education",
        target: "#educationKendoGrid",
        url: educationUrl + "/GetEmpEducationKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारीको" : "Emp.Code", width: 150, columnMenu: true },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 200 },
            { field: 'Code', title: lang == "ne" ? "विभाग" : "Edu.Code" },
            { field: 'Name', title: lang == "ne" ? "समय" : "Name" },
            { field: 'ApprovedOn', title: lang == "ne" ? "कर्मचारीको" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedEducation(new QualificationModel(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Education = callBack;
        }
    };

    function getEmpeducationByEmpId() {
        Riddha.ajax.get(educationUrl + "/GetVerificationEducationByEmpId?empId=" + self.SelectedEducation().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedEducation(new QualificationModel(ko.toJS(result.Data[0])));
                    self.Education(new QualificationModel(ko.toJS(result.Data[0])));
                }
            });
    };

    self.EducationApprove = function (item) {
        if (self.SelectedEducation().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedEducation().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        self.Education().Id(self.SelectedEducation().Id());
        Riddha.ajax.get("/Api/EducationApi/Approve?id=" + self.SelectedEducation().Id() + "&empId=" + self.SelectedEducation().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().EmployeeEducation(parseInt(self.UnapprovedCount().EmployeeEducation()) - 1);
                    self.RefreshEducationKendoGrid();
                    self.EducationCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleEducationRevert = ko.computed(function () {
        if (self.SelectedEducation().ApprovedById() == null) {
            return false;
        } else {
            return true;
        }
    });

    self.EducationRevert = function (item) {
        
        if (self.SelectedEducation().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedEducation().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };

        Riddha.UI.Confirm("Are u sure u want to revert ?", function () {
            self.Education().Id(self.SelectedEducation().Id());
            Riddha.ajax.get("/Api/EducationApi/Revert?id=" + self.SelectedEducation().Id() + "&empId=" + self.SelectedEducation().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().EmployeeEducation(parseInt(self.UnapprovedCount().EmployeeEducation()) + 1);
                        self.RefreshEducationKendoGrid();
                        self.EducationCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    };

    self.RefreshEducationKendoGrid = function () {
        $("#educationKendoGrid").getKendoGrid().dataSource.read();
    };

    self.EducationShowModal = function () {
        if (self.SelectedEducation().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getEmpeducationByEmpId();
        $("#educationViewModal").modal('show');
    };

    self.EducationCloseModal = function () {
        $("#educationViewModal").modal('hide');
    };

    //2nd Skill
    self.KendoGridOptionForSkills = {
        title: "Skills",
        target: "#skillKendoGrid",
        url: skillUrl + "/GetEmpSkillKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारीको" : "Emp.Code", width: 150, columnMenu: true },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 200 },
            { field: 'Code', title: lang == "ne" ? "विभाग" : "Skill Code" },
            { field: 'Name', title: lang == "ne" ? "समय" : "Name" },
            { field: 'ApprovedOn', title: lang == "ne" ? "कर्मचारीको" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedSkill(new QualificationModel(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Skill = callBack;
        }
    };

    function getSkillByEmpId() {
        Riddha.ajax.get(skillUrl + "/GetVerificationSkillByEmpId?empId=" + self.SelectedSkill().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedSkill(new QualificationModel(ko.toJS(result.Data[0])));
                    self.Skill(new QualificationModel(ko.toJS(result.Data[0])));
                }
            });
    };

    self.SkillApprove = function (item) {
        if (self.SelectedSkill().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedSkill().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        self.Skill().Id(self.SelectedSkill().Id());
        Riddha.ajax.get("/Api/SkillsApi/Approve?id=" + self.SelectedSkill().Id() + "&empId=" + self.SelectedSkill().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().EmployeeSkill(parseInt(self.UnapprovedCount().EmployeeSkill()) - 1);
                    self.RefreshSkillKendoGrid();
                    self.SkillCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleSkillRevert = ko.computed(function () {
        if (self.SelectedSkill().ApprovedById() == null) {
            return false;
        } else {
            return true;
        }
    });

    self.SkillRevert = function (item) {
        if (self.SelectedSkill().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedSkill().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };

        Riddha.UI.Confirm("Are u sure u want to revert ?", function () {
            self.Skill().Id(self.SelectedSkill().Id());
            Riddha.ajax.get("/Api/SkillsApi/Revert?id=" + self.SelectedSkill().Id() + "&empId=" + self.SelectedSkill().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().EmployeeSkill(parseInt(self.UnapprovedCount().EmployeeSkill()) + 1);
                        self.RefreshSkillKendoGrid();
                        self.SkillCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    };

    self.RefreshSkillKendoGrid = function () {
        $("#skillKendoGrid").getKendoGrid().dataSource.read();
    };

    self.SkillShowModal = function () {
        if (self.SelectedSkill().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getSkillByEmpId();
        $("#skillViewModal").modal('show');
    };

    self.SkillCloseModal = function () {
        $("#skillViewModal").modal('hide');
    };

    //3rd License
    self.KendoGridOptionForLicense = {
        title: "License",
        target: "#licenseKendoGrid",
        url: licenseUrl + "/GetEmpLicenseKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारीको" : "Emp.Code", width: 150, columnMenu: true },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 200 },
            { field: 'Code', title: lang == "ne" ? "विभाग" : "License Code" },
            { field: 'Name', title: lang == "ne" ? "समय" : "Name" },
            { field: 'ApprovedOn', title: lang == "ne" ? "कर्मचारीको" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedLicense(new QualificationModel(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.License = callBack;
        }
    };

    function getLicenseByEmpId() {
        Riddha.ajax.get(licenseUrl + "/GetVerificationLicenseByEmpId?empId=" + self.SelectedLicense().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedLicense(new QualificationModel(ko.toJS(result.Data[0])));
                    self.License(new QualificationModel(ko.toJS(result.Data[0])));
                }
            });
    };

    self.LicenseApprove = function (item) {
        if (self.SelectedLicense().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedLicense().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        self.License().Id(self.SelectedLicense().Id());
        Riddha.ajax.get("/Api/LicenseApi/Approve?id=" + self.SelectedLicense().Id() + "&empId=" + self.SelectedLicense().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().EmployeeLicense(parseInt(self.UnapprovedCount().EmployeeLicense()) - 1);
                    self.RefreshLicenseKendoGrid();
                    self.LicenseCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleLicenseRevert = ko.computed(function () {
        if (self.SelectedLicense().ApprovedById() == null) {
            return false;
        } else {
            return true;
        }
    }, this);

    self.LicenseRevert = function (item) {
        if (self.SelectedLicense().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedLicense().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };

        Riddha.UI.Confirm("Are u sure u want to revert ?", function () {
            self.License().Id(self.SelectedLicense().Id());
            Riddha.ajax.get("/Api/LicenseApi/Revert?id=" + self.SelectedLicense().Id() + "&empId=" + self.SelectedLicense().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().EmployeeLicense(parseInt(self.UnapprovedCount().EmployeeLicense()) + 1);
                        self.RefreshLicenseKendoGrid();
                        self.LicenseCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    };

    self.RefreshLicenseKendoGrid = function () {
        $("#licenseKendoGrid").getKendoGrid().dataSource.read();
    };

    self.LicenseShowModal = function () {
        if (self.SelectedLicense().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getLicenseByEmpId();
        $("#licenseViewModal").modal('show');
    };

    self.LicenseCloseModal = function () {
        $("#licenseViewModal").modal('hide');
    };

    //4th Language

    self.KendoGridOptionForLanguage = {
        title: "Language",
        target: "#languageKendoGrid",
        url: languageUrl + "/GetEmpLanguageKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारीको" : "Emp.Code", width: 150, columnMenu: true },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 200 },
            { field: 'Code', title: lang == "ne" ? "विभाग" : "Language Code" },
            { field: 'Name', title: lang == "ne" ? "समय" : "Name" },
            { field: 'ApprovedOn', title: lang == "ne" ? "कर्मचारीको" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedLanguage(new QualificationModel(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Language = callBack;
        }
    };

    function getLanguageByEmpId() {
        Riddha.ajax.get(languageUrl + "/GetVerificationLanguageByEmpId?empId=" + self.SelectedLanguage().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedLanguage(new QualificationModel(ko.toJS(result.Data[0])));
                    self.Language(new QualificationModel(ko.toJS(result.Data[0])));
                }
            });
    };

    self.LanguageApprove = function (item) {
        if (self.SelectedLanguage().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedLanguage().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        self.Language().Id(self.SelectedLanguage().Id());
        Riddha.ajax.get("/Api/LanguageApi/Approve?id=" + self.SelectedLanguage().Id() + "&empId=" + self.SelectedLanguage().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().EmployeeLanguage(parseInt(self.UnapprovedCount().EmployeeLanguage()) - 1);
                    self.RefreshLanguageKendoGrid();
                    self.LicenseCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleLanguageRevert = ko.computed(function () {
        if (self.SelectedLanguage().ApprovedById() == null) {
            return false;
        } else {
            return true;
        }
    }, this);

    self.LanguageRevert = function (item) {
        
        if (self.SelectedLanguage().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedLanguage().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };

        Riddha.UI.Confirm("Are you sure you want to revert?", function () {
            self.Language().Id(self.SelectedLanguage().Id());
            Riddha.ajax.get("/Api/LanguageApi/Revert?id=" + self.SelectedLanguage().Id() + "&empId=" + self.SelectedLanguage().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().EmployeeLanguage(parseInt(self.UnapprovedCount().EmployeeLanguage()) + 1);
                        self.RefreshLanguageKendoGrid();
                        self.LicenseCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });

    };

    self.RefreshLanguageKendoGrid = function () {
        $("#languageKendoGrid").getKendoGrid().dataSource.read();
    };

    self.LanguageShowModal = function () {
        if (self.SelectedLanguage().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getLanguageByEmpId();
        $("#languageViewModal").modal('show');
    };

    self.LanguageCloseModal = function () {
        $("#languageViewModal").modal('hide');
    };

    //5th Membership

    self.KendoGridOptionForMembership = {
        title: "Membership",
        target: "#membershipKendoGrid",
        url: membershipUrl + "/GetEmpMembershipKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारीको" : "Emp.Code", width: 150, columnMenu: true },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 200 },
            { field: 'Code', title: lang == "ne" ? "विभाग" : "Membership Code" },
            { field: 'Name', title: lang == "ne" ? "समय" : "Name" },
            { field: 'ApprovedOn', title: lang == "ne" ? "कर्मचारीको" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedMembership(new MembershipVm(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Membership = callBack;
        }
    };

    function getMembershipByEmpId() {
        Riddha.ajax.get(membershipUrl + "/GetVerificationMembershipByEmpId?empId=" + self.SelectedMembership().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedMembership(new MembershipVm(ko.toJS(result.Data[0])));
                    self.Membership(new MembershipVm(ko.toJS(result.Data[0])));
                }
            });
    };

    self.MembershipApprove = function (item) {
        if (self.SelectedMembership().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedMembership().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        self.Membership().Id(self.SelectedMembership().Id());
        Riddha.ajax.get("/Api/MembershipApi/Approve?id=" + self.SelectedMembership().Id() + "&empId=" + self.SelectedMembership().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().EmployeeMembership(parseInt(self.UnapprovedCount().EmployeeMembership()) - 1);
                    self.RefreshMembershipKendoGrid();
                    self.MembershipCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleMembershipRevert = ko.computed(function () {
        if (self.SelectedMembership().ApprovedById() == null) {
            return false;
        } else {
            return true;
        }
    }, this);

    self.MembershipRevert = function (item) {
        if (self.SelectedMembership().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedMembership().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };

        Riddha.UI.Confirm("Are u sure u want to revert ??", function () {
            self.Membership().Id(self.SelectedMembership().Id());
            Riddha.ajax.get("/Api/MembershipApi/Revert?id=" + self.SelectedMembership().Id() + "&empId=" + self.SelectedMembership().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().EmployeeMembership(parseInt(self.UnapprovedCount().EmployeeMembership()) + 1);
                        self.RefreshMembershipKendoGrid();
                        self.MembershipCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    };

    self.RefreshMembershipKendoGrid = function () {
        $("#membershipKendoGrid").getKendoGrid().dataSource.read();
    };

    self.MembershipShowModal = function () {
        if (self.SelectedMembership().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getMembershipByEmpId();
        $("#membershipViewModal").modal('show');
    };

    self.MembershipCloseModal = function () {
        $("#membershipViewModal").modal('hide');
    };

    //6th Experience

    self.KendoGridOptionForExperience = {
        title: "Experience",
        target: "#experienceKendoGrid",
        url: experienceUrl + "/GetEmpExperienceKendoGrid",
        height: 350,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: false,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: 'EmployeeCode', title: lang == "ne" ? "कर्मचारीको" : "Emp.Code", width: 150, columnMenu: true },
            { field: 'EmployeeName', title: lang == "ne" ? "कर्मचारीको नाम" : "Employee Name", width: 200 },
            { field: 'Title', title: lang == "ne" ? "विभाग" : "Title" },
            { field: 'OrganizationName', title: lang == "ne" ? "समय" : "OrganizationName" },
            { field: 'BeganOn', title: lang == "ne" ? "समय" : "BeganOn", template: "#=SuitableDate(BeganOn)#" },
            { field: 'EndedOn', title: lang == "ne" ? "समय" : "EndedOn", template: "#=SuitableDate(EndedOn)#" },
            { field: 'ApprovedOn', title: lang == "ne" ? "कर्मचारीको" : "Approved On", template: "#=SuitableDate(ApprovedOn)#" },
            { field: 'ApprovedBy', title: lang == "ne" ? "समय" : "Approved By" },
        ],
        SelectedItem: function (item) {
            self.SelectedExperience(new ExperienceVm(item));
        },
        SelectedItems: function (items) {
        },
        open: function (callBack) {
            self.GetOnDemandKendoGrid.Experience = callBack;
        }
    };

    function getExperienceByEmpId() {
        Riddha.ajax.get(experienceUrl + "/GetVerificationExperienceByEmpId?empId=" + self.SelectedExperience().EmployeeId())
            .done(function (result) {
                if (result.Data.length > 0) {
                    self.SelectedExperience(new ExperienceVm(ko.toJS(result.Data[0])));
                    self.Experience(new ExperienceVm(ko.toJS(result.Data[0])));
                }
            });
    };

    self.ExperienceApprove = function (item) {
        if (self.SelectedExperience().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedExperience().ApprovedById() != null) {
            Riddha.UI.Toast("Already Approved", 0);
            return;
        };
        self.Experience().Id(self.SelectedExperience().Id());
        Riddha.ajax.get("/Api/ExperienceApi/Approve?id=" + self.SelectedExperience().Id() + "&empId=" + self.SelectedExperience().EmployeeId())
            .done(function (result) {
                if (result.Status == 4) {
                    self.UnapprovedCount().EmployeeExperience(parseInt(self.UnapprovedCount().EmployeeExperience()) - 1);
                    self.RefreshExperienceKendoGrid();
                    self.ExperienceCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VisibleExperienceRevert = ko.computed(function () {
        if (self.SelectedExperience().ApprovedById() == null) {
            return false;
        } else {
            return true;
        }
    }, this);

    self.ExperienceRevert = function (item) {
        if (self.SelectedExperience().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        };
        if (self.SelectedExperience().ApprovedById() == null) {
            Riddha.UI.Toast("Already Reverted", 0);
            return;
        };

        Riddha.UI.Confirm("Are u sure u want to revert ??", function () {
            self.Experience().Id(self.SelectedExperience().Id());
            Riddha.ajax.get("/Api/ExperienceApi/Revert?id=" + self.SelectedExperience().Id() + "&empId=" + self.SelectedExperience().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.UnapprovedCount().EmployeeExperience(parseInt(self.UnapprovedCount().EmployeeExperience()) + 1);
                        self.RefreshExperienceKendoGrid();
                        self.ExperienceCloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    };

    self.RefreshExperienceKendoGrid = function () {
        $("#experienceKendoGrid").getKendoGrid().dataSource.read();
    };

    self.ExperienceShowModal = function () {
        if (self.SelectedExperience().Id() == 0) {
            Riddha.UI.Toast("Please select employee", 0);
            return;
        }
        getExperienceByEmpId();
        $("#experienceViewModal").modal('show');
    };

    self.ExperienceCloseModal = function () {
        $("#experienceViewModal").modal('hide');
    };
}

//Education
function educationController() {
    var self = this;
    var url = "/Api/EducationApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.Education = ko.observable(new EducationModel());
    self.Educations = ko.observableArray([]);
    self.SelectedEducation = ko.observable();

    getEducations();
    function getEducations() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EducationModel);
                self.Educations(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.Education().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Education()))
                .done(function (result) {
                    self.Educations.push(new EducationModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Education()))
                .done(function (result) {
                    self.Educations.replace(self.SelectedEducation(), new EducationModel(ko.toJS(self.Education())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                });
        };
    }

    self.Reset = function () {
        self.Education(new EducationModel({ Id: self.Education().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        self.SelectedEducation(model);
        self.Education(new EducationModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (education) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + education.Id(), null)
                .done(function (result) {
                    self.Educations.remove(education)
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#educationCreationModel").modal('show');
    };

    $("#educationCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#educationCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
}

//Experience 
//Operationally completed but not in use 
function experienceController() {
    var self = this;
    var url = "/Api/ExperienceApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.Experience = ko.observable(new ExperienceModel());
    self.Experiences = ko.observableArray([]);
    self.SelectedExperience = ko.observable();

    getExperiences();
    function getExperiences() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ExperienceModel);
                self.Experiences(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.Experience().Title() == "") {
            Riddha.util.localize.Required("Title");
            return
        }
        if (self.Experience().OrganizationName() == "") {
            Riddha.util.localize.Required("OrganizationName");
            return
        }
        if (self.Experience().BeganOn() == "NaN/aN/aN") {
            Riddha.util.localize.Required("BeganOn");
            return
        }
        if (self.Experience().EndedOn() == "NaN/aN/aN") {
            Riddha.util.localize.Required("EndedOn");
            return
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Experience()))
                .done(function (result) {
                    self.Experiences.push(new ExperienceModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Experience()))
                .done(function (result) {
                    self.Experiences.replace(self.SelectedExperience(), new ExperienceModel(ko.toJS(self.Experience())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                });
        };
    }

    self.Reset = function () {
        self.Experience(new ExperienceModel({ Id: self.Experience().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        self.SelectedExperience(model);
        self.Experience(new ExperienceModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (experience) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + experience.Id(), null)
                .done(function (result) {
                    self.Experiences.remove(experience)
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };


    self.ShowModal = function () {
        $("#experienceCreationModel").modal('show');
    };

    $("#experienceCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#experienceCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
}

//Language
function languageController() {
    var self = this;
    var url = "/Api/LanguageApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.Language = ko.observable(new LanguageModel());
    self.Languages = ko.observableArray([]);
    self.SelectedLanguage = ko.observable();

    getLanguages();
    function getLanguages() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), LanguageModel);
                self.Languages(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.Language().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Language()))
                .done(function (result) {
                    self.Languages.push(new LanguageModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Language()))
                .done(function (result) {
                    self.Languages.replace(self.SelectedLanguage(), new LanguageModel(ko.toJS(self.Language())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                });
        };
    }

    self.Reset = function () {
        self.Language(new LanguageModel({ Id: self.Language().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        self.SelectedLanguage(model);
        self.Language(new LanguageModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (language) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + language.Id(), null)
                .done(function (result) {
                    self.Languages.remove(language)
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#languageCreationModel").modal('show');
    };

    $("#languageCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#languageCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
}

//License
function licenseController() {
    var self = this;
    var url = "/Api/LicenseApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.License = ko.observable(new LicenseModel());
    self.Licenses = ko.observableArray([]);
    self.SelectedLicense = ko.observable();

    getLicenses();
    function getLicenses() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), LicenseModel);
                self.Licenses(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.License().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.License()))
                .done(function (result) {
                    self.Licenses.push(new LicenseModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.License()))
                .done(function (result) {
                    self.Licenses.replace(self.SelectedLicense(), new LicenseModel(ko.toJS(self.License())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                });
        };
    }

    self.Reset = function () {
        self.License(new LicenseModel({ Id: self.License().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        self.SelectedLicense(model);
        self.License(new LicenseModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (license) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + license.Id(), null)
                .done(function (result) {
                    self.Licenses.remove(license)
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#licenseCreationModel").modal('show');
    };

    $("#licenseCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#licenseCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
}

//Membership
//Operationally completed but not in use 
function membershipController() {
    var self = this;
    var url = "/Api/MembershipApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.Membership = ko.observable(new MembershipModel());
    self.Memberships = ko.observableArray([]);
    self.SelectedMembership = ko.observable();

    getMemberships();
    function getMemberships() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MembershipModel);
                self.Memberships(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.Membership().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Membership()))
                .done(function (result) {
                    self.Memberships.push(new MembershipModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Membership()))
                .done(function (result) {
                    self.Memberships.replace(self.SelectedMembership(), new MembershipModel(ko.toJS(self.Membership())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        };
    }

    self.Reset = function () {
        self.Membership(new MembershipModel({ Id: self.Membership().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        self.SelectedMembership(model);
        self.Membership(new MembershipModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (membership) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + membership.Id(), null)
                .done(function (result) {
                    self.Memberships.remove(membership)
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#membershipCreationModel").modal('show');
    };

    $("#membershipCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#membershipCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
}

//Skill
function skillController() {
    var self = this;
    var url = "/Api/SkillsApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.Skill = ko.observable(new SkillModel());
    self.Skills = ko.observableArray([]);
    self.SelectedSkill = ko.observable();

    getSkills();
    function getSkills() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), SkillModel);
                self.Skills(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.Skill().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Skill()))
                .done(function (result) {
                    self.Skills.push(new SkillModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Skill()))
                .done(function (result) {
                    self.Skills.replace(self.SelectedSkill(), new SkillModel(ko.toJS(self.Skill())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                });
        };
    }

    self.Reset = function () {
        self.Skill(new SkillModel({ Id: self.Skill().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        self.SelectedSkill(model);
        self.Skill(new SkillModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (skill) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + skill.Id(), null)
                .done(function (result) {
                    self.Skills.remove(skill)
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#skillCreationModel").modal('show');
    };

    $("#skillCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#skillCreationModel").modal('hide');
        self.ModeOfButton("Create");
    }
}

//Course
function courseController() {
    var self = this;
    var url = "/Api/CourseApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.Course = ko.observable(new CourseModel());
    self.Courses = ko.observableArray([]);
    self.SelectedCourse = ko.observable();
    self.Departments = ko.observable();
    self.Coordinator = ko.observable(new EmployeeSearchViewModel());

    getDepartment();
    function getDepartment() {
        Riddha.ajax.get(url + "/GetDepartmentsForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DepartmentDropdownModel);
                self.Departments(data);
            });
    }

    self.EmployeeAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/CourseApi/GetEmployeeLstForAutoComplete",
        select: function (item) {
            self.Coordinator(new EmployeeSearchViewModel(item));
            self.Course().CoordinatorId(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Search Coordinator"
    };

    self.Currency = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "नेपाली रुपैयाँ" : "NepaliRupees" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "भारतीय रुपैयाँ" : "IndianRupees" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "अमेरिकी डलर" : "AmericanDollar" },
    ]);


    GetCourse();
    function GetCourse() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), CourseGridVm);
                self.Courses(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.Course().Title() == "") {
            return Riddha.util.localize.Required("Title");
        }
        if (self.Course().DepartmentId() == undefined) {
            return Riddha.util.localize.Required("Department");
        }
        if (self.Course().CoordinatorId() == 0) {
            return Riddha.util.localize.Required("Coordinator");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Course()))
                .done(function (result) {
                    if (result.Status == 4) {
                        GetCourse();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Course()))
                .done(function (result) {
                    if (result.Status == 4) {
                        GetCourse();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Select = function (model) {
        self.SelectedCourse(model);
        self.Course(new CourseModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Reset = function () {
        self.Course(new CourseModel({ Id: self.Course().Id() }));
        self.ModeOfButton("Create");
    };

    self.Delete = function (course) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + course.Id(), null)
                .done(function (result) {
                    self.Courses.remove(course)
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#courseCreationModel").modal('show');

    };

    $("#courseCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#courseCreationModel").modal('hide');
        // self.ModeOfButton("Create");
    }
}

//Session
function sessionsController() {
    var self = this;
    var url = "/Api/SessionApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.Session = ko.observable(new SessionModel());
    self.Sessions = ko.observableArray([]);
    self.SelectedSession = ko.observable();
    self.Course = ko.observable(new CourseSearchViewModel());

    self.Method = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "कक्षा" : "Classroom" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "आत्म अध्ययन" : "SelfStudy" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "वेबएक्स" : "WebEx" },
    ]);

    self.CourseAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/SessionApi/GetCourseLstForAutoComplete",
        select: function (item) {
            self.Course(new CourseSearchViewModel(item));
            self.Session().CourseId(item.Id);
        },
        placeholder: lang == "ne" ? "पाठ्यक्रम छान्नुहोस" : "Search Course"
    };

    GetSession();
    function GetSession() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), SessionModel);
                self.Sessions(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.Session().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.Session().CourseId() == 0) {
            return Riddha.util.localize.Required("Course");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Session()))
                .done(function (result) {
                    if (result.Status == 4) {
                        GetSession();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Session()))
                .done(function (result) {
                    if (result.Status == 4) {
                        GetSession();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Reset = function () {
        self.Session(new SessionModel({ Id: self.Session().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        self.SelectedSession(model);
        self.Session(new SessionModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (session) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + session.Id(), null)
                .done(function (result) {
                    self.Sessions.remove(session)
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#sessionCreationModel").modal('show');

    };

    $("#sessionCreationModel").on('hidden.bs.modal', function () {
        // self.Reset();
    });

    self.CloseModal = function () {
        $("#sessionCreationModel").modal('hide');
    }
}

//Participant
function participantController() {
    var self = this;
    var url = "/Api/ParticipantApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.Participant = ko.observable(new ParticipantModel());
    self.Participants = ko.observableArray([]);
    self.SelectedParticipant = ko.observable();
    self.Session = ko.observable(new SessionSearchViewModel());
    self.Employees = ko.observableArray([]);
    self.EmpIds = ko.observableArray([]);
    GetParticipants();

    self.ParticipantStatus = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "योजना" : "Planned" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "प्रक्रिया" : "InProcessing" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "पूरा" : "Completed" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "रद्द" : "Canceled" },
    ]);

    self.SessionAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/ParticipantApi/GetSessionLstForAutoComplete",
        select: function (item) {
            self.Session(new SessionSearchViewModel(item));
            self.Participant().SessionId(item.Id);
        },
        placeholder: lang == "ne" ? "सत्र छान्नुहोस" : "Search Session"
    };

    self.EmpMultiOptions = {
        dataTextField: "Name",
        url: "/Api/ParticipantApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.EmpIds.push(item.Id);
        },
        deselect: function (item) {
            self.EmpIds.remove(item.Id);
        },
        value: function () {
            var emps = ko.utils.arrayMap(self.Employees(), function (item) {
                return { Id: item.Id(), Name: item.Name() }
            });
            return emps;
        },
        multiSelect: undefined
    }

    function GetParticipants() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ParticipantModel);
                self.Participants(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.Participant().StartDate() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("StartDate");
        }
        else if (self.Participant().EndDate() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("EndDate");
        }
        if (self.Participant().StartDate() > self.Participant().EndDate()) {
            return Riddha.UI.Toast("The End Date must be greater than the From Date");
        }
        var data = { Participant: self.Participant(), EmpIds: self.EmpIds() };
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        GetParticipants();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        GetParticipants();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        };
    }

    self.Reset = function () {
        self.Participant(new ParticipantModel({ Id: self.Participant().Id() }));
        self.EmpIds([]);
        self.EmpMultiOptions.multiSelect.value([]);
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        Riddha.ajax.get(url + "?id=" + model.Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.Participant(new ParticipantModel(result.Data.Participant));
                    self.Participant().SessionName(result.Data.Participant.Session.Name);
                    var data = Riddha.ko.global.arrayMap(result.Data.EmpLst, GlobalDropdownModel);
                    self.Employees(data);
                    self.EmpIds([]);
                    ko.utils.arrayMap(result.Data.EmpLst, function (item) {
                        self.EmpIds.push(item.Id);
                    });
                    self.ModeOfButton('Update');
                    self.ShowModal();
                }
            })
    };

    self.Delete = function (Participant) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + Participant.Id(), null)
                .done(function (result) {
                    self.Participants.remove(Participant)
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#participantCreationModel").modal('show');

    };

    $("#participantCreationModel").on('hidden.bs.modal', function () {
        // self.Reset();
    });

    self.CloseModal = function () {
        $("#participantCreationModel").modal('hide');
    }
}

//CustomField
function customFieldController() {
    var self = this;
    var url = "/Api/CustomFieldApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.CustomField = ko.observable(new CustomFieldModel());
    self.CustomFields = ko.observableArray([]);
    self.SelectedCustomField = ko.observable();

    self.FieldType = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "पाठ" : "Text" },
    ]);

    getCustomFields();
    function getCustomFields() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), CustomFieldModel);
                self.CustomFields(data);
            });
    };

    self.CreateUpdate = function () {
        if (self.CustomField().FieldName() == "") {
            return Riddha.util.localize.Required("FieldName");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.CustomField()))
                .done(function (result) {
                    self.CustomFields.push(new CustomFieldModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.CustomField()))
                .done(function (result) {
                    self.CustomFields.replace(self.SelectedCustomField(), new CustomFieldModel(ko.toJS(self.CustomField())));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        };
    };

    self.Reset = function () {
        self.CustomField(new CustomFieldModel({ Id: self.CustomField().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        self.SelectedCustomField(model);
        self.CustomField(new CustomFieldModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (customField) {
        Riddha.UI.Confirm("Delete Confirm", function () {
            Riddha.ajax.delete(url + "/" + customField.Id(), null)
                .done(function (result) {
                    self.CustomFields.remove(customField)
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }, '', "Delete " + customField.FieldName())
    };

    self.ShowModal = function () {
        $("#customFieldCreationModal").modal('show');
    };

    $("#customFieldCreationModal").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#customFieldCreationModal").modal('hide');
        self.ModeOfButton("Create");
    };
}

//Disciplinary Cases
function disciplinaryCasesController() {
    var self = this;
    var url = "/Api/DisciplinaryCasesApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Create');
    self.DisciplinaryCase = ko.observable(new DisciplinaryCaseModel());
    self.DisciplinaryCases = ko.observableArray([]);
    self.SelectedDisciplinaryCase = ko.observable();
    self.FordwardTo = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.EmpIds = ko.observableArray([]);

    self.DisciplinaryStatus = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "प्रक्रिया" : "InProcessing" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "बन्द" : "Close" },

    ]);

    self.DisciplinaryActions = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "मौखिक चेतावनी दिनु" : "Give Verbal Warning" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "लिखित चेतावनी दिनु" : "Give Written Warning" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "सुनेपछि शास्त्रीय छ" : "Have Disciplinary Hearing" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "परामर्श दिनु" : "Provide Counselling" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "परिवीक्षाधीन मा राख्नु" : "Put On Probation" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "प्रशासनिक मा पठाउनु" : "Send On Administrative" },
        { Id: 6, Name: config.CurrentLanguage == 'ne' ? "निलम्बन" : "Suspend" },
        { Id: 7, Name: config.CurrentLanguage == 'ne' ? "समाप्त" : "Terminate" },

    ]);

    getFordwardToEmployee();
    function getFordwardToEmployee() {
        Riddha.ajax.get(url + "/GetFordwardToEmployee", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.FordwardTo(data);
            });
    };
    getDisciplinaryCase();
    function getDisciplinaryCase() {
        Riddha.ajax.get(url)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DisciplinaryCaseModel);
                self.DisciplinaryCases(data);
            });
    };
    self.GetFordwardToEmployeeName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.FordwardTo()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.EmpMultiOptions = {
        dataTextField: "Name",
        url: "/Api/DisciplinaryCasesApi/GetEmpLstForMultiSelect",
        select: function (item) {
            self.EmpIds.push(item.Id);
        },
        deselect: function (item) {
            self.EmpIds.remove(item.Id);
        },
        value: function () {
            var emps = ko.utils.arrayMap(self.Employees(), function (item) {
                return { Id: item.Id(), Name: item.Name() }
            });
            return emps;
        },
        multiSelect: undefined
    };



    self.CreateUpdate = function () {
        var data = { DisciplinaryCases: self.DisciplinaryCase(), EmpIds: self.EmpIds() };
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        getDisciplinaryCase();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        getDisciplinaryCase();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        };
    };

    self.Reset = function () {
        self.DisciplinaryCase(new DisciplinaryCaseModel({ Id: self.DisciplinaryCase().Id() }));
        self.EmpIds([]);
        self.EmpMultiOptions.multiSelect.value([]);
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        Riddha.ajax.get(url + "?id=" + model.Id())
            .done(function (result) {
                if (result.Status == 4) {
                    
                    self.DisciplinaryCase(new DisciplinaryCaseModel(result.Data.DisciplinaryCases));
                    var data = Riddha.ko.global.arrayMap(result.Data.EmpLst, GlobalDropdownModel);
                    self.Employees(data);
                    self.EmpIds([]);
                    ko.utils.arrayMap(result.Data.EmpLst, function (item) {
                        self.EmpIds.push(item.Id);
                    });
                    self.ModeOfButton('Update');
                    self.ShowModal();
                }
            })
    };

    self.Delete = function (disciplinaryCase) {
        Riddha.UI.Confirm("Delete Confirm", function () {
            Riddha.ajax.delete(url + "/" + disciplinaryCase.Id(), null)
                .done(function (result) {
                    self.DisciplinaryCases.remove(disciplinaryCase)
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }, '', "Delete " + disciplinaryCase.CaseName())
    };

    self.ShowModal = function () {
        $("#disciplinaryCasesModal").modal('show');
    };

    $("#disciplinaryCasesModal").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#disciplinaryCasesModal").modal('hide');
        self.ModeOfButton("Create");
    };

}