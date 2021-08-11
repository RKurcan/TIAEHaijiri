/// <reference path="../../Scripts/bootstrap-dialog.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Company.Model.js" />

//Company
function companyController(date, defaultExpiryDate) {
    var self = this;
    var config = new Riddha.config();
    var url = "/Api/CompanyApi";
    self.Company = ko.observable(new CompanyModel());
    self.Companies = ko.observableArray([]);
    self.SelectedCompany = ko.observable();
    self.UserName = ko.observable('');
    self.Password = ko.observable('');
    self.ModeOfButton = ko.observable('Create');
    self.CompanyLicense = ko.observable(new CompanyLicenseModel());
    self.LicenseStatus = ko.observable('0');

    //Region Password Validation
    self.IsValid = ko.observable(false);
    self.PasswordInfo = ko.observable('');
    self.PasswordInfoStyle = ko.observable('');
    self.PasswordInfo = ko.computed(function () {
        var language = Riddha.config().CurrentLanguage;
        var password = self.Password();
        self.PasswordInfoStyle('form-control text-center bg-red-gradient');
        if (/^([0|\+[0-9]{1,5})?([7-9][0-9]{9})$/.test(password) == false) {
            if (password == "") {
                self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
                return language == "ne" ? "उदाहरण: Hamro@hajiri123 " : "eg:(valid password):Hamro@hajiri123";
            }
            else if (password.length < 6) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डको लम्मबाइ ६ भन्दा बढी चाहिन्छ " : "Password should be of more than 6 chars";
            } else if (password.search(/\d/) == -1) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा नम्बर अनिवार्य छ" : "Password should contain a number";
            } else if (password.search(/[a-zA-Z]/) == -1) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा अल्फाबेट अनिवार्य छ" : "Password should contain a alphabet";
            } else if (/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/.test(password) == false) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा बिषेस स्ंकेत अनिवार्य छ" : "Password should contain a special symbol";
            } else if ((/[A-Z]/.test(password) && /[a-z]/.test(password)) == false) {
                self.IsValid(false);
                return language == "ne" ? "पास्वर्डमा " : "Password should contain a upper & lower case";
            }
            else {
                self.IsValid(true);
                self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
                return language == "ne" ? "पास्वर्ड ठीक छ" : "Valid Password";
            }
        }
        else {
            self.IsValid(true);
            self.PasswordInfoStyle('form-control text-center bg-aqua-gradient');
            return language == "ne" ? "पास्वर्ड ठीक छ" : "Valid Password";
        }

    }, self);
    //EndRegion


    self.GetExpiryDate = function () {
        if (self.CompanyLicense().LicensePeriod() > 5 || self.CompanyLicense().LicensePeriod() < 1) {
            self.CompanyLicense().LicensePeriod('');
            return false;
        }
        if (self.CompanyLicense().IssueDate() == "") {
            return false;
        }
        Riddha.ajax.get(url + "/GetLicenseExpiryDate?period=" + self.CompanyLicense().LicensePeriod() + "&issueDate=" + self.CompanyLicense().IssueDate())
            .done(function (result) {
                if (result.Status == 4) {
                    self.CompanyLicense().ExpiryDate(result.Data);
                }
            })
    };

    self.LicenseStatusList = ko.observableArray([
        { Id: '0', Name: 'New' },
        { Id: '1', Name: 'ReNew' }
    ]);

    self.SoftwarePackageType = ko.observableArray([
        { Id: '0', Name: 'Silver' },
        { Id: '1', Name: 'Gold' },
        { Id: '2', Name: 'Platinum' },
    ]);

    self.SoftwareTypes = ko.observableArray([
        { Id: '0', Name: 'Web' },
        { Id: '1', Name: 'Desktop' },
    ]);

    self.OrganizationTypes = ko.observableArray([
        { Id: '0', Name: 'NonGovernment' },
        { Id: '1', Name: 'Government' },
    ]);

    //GetCompanies();
    //function GetCompanies() {
    //    Riddha.ajax.get(url)
    //        .done(function (result) {
    //            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), CompanyGridVm);
    //            self.Companies(data);
    //        });
    //};

    var record = 0;
    self.KendoGridOptions = {
        title: "Company",
        target: "#companyKendoGrid",
        url: url + "/GetKendoGrid",
        height: '500',
        paramData: {},
        multiSelect: false,
        selectable: false,
        group: true,
        columns: [
            { field: 'Code', title: "Code", filterable: true, width: 60 },
            { field: 'Name', title: "Name", filterable: true, width: 130 },
            { field: 'Address', title: "Address", filterable: false, width: 130 },
            { field: 'ContactNo', title: "Contact", filterable: false, width: 90 },
            { field: 'SoftwarePackageType', title: "Package", filterable: false, width: 60, template: "#= getSoftwarePackageTypeTemp(SoftwarePackageType)#" },
            { field: 'SoftwareType', title: "Type", filterable: false, width: 70, template: "#=getSoftwareTypeTemp(SoftwareType)#" },
            { field: 'Status', title: "Status", filterable: false, width: 70, template: "#=getCustomerStatusTemp(Status)#" },
            {
                command: [
                    { name: "edit", template: '<a class="k-grid-edit k-button" ><span class="fa fa-pencil text-green"  ></span></a>', click: Edit },
                    { name: "delete", template: '<a class="k-grid-delete k-button" ><span class="fa fa-trash text-red"  ></span></a>', click: Delete },
                    { name: "approve", template: '<a class="k-grid-approve k-button" ><span class="fa fa-check text-blue"  ></span></a>', click: ApproveSuspend },
                    { name: "license", template: '<a class="k-grid-license k-button" ><span class="fa fa-key text-blue"  ></span></a>', click: ShowCompLicenseModal }
                ],
                title: lang == "ne" ? "कार्य" : "Action",
                width: "250px"
            }

        ],
        SelectedItem: function (item) {
            self.SelectedCompany(new CompanyModel(item));
        },

        SelectedItems: function (items) {
        },
    };

    self.RefreshKendoGrid = function () {
        $("#companyKendoGrid").getKendoGrid().dataSource.read();
    };

    $("#companyKendoGrid").kendoTooltip({
        filter: ".k-grid-delete",
        position: "bottom",
        content: function (e) {
            return lang == "ne" ? "डीलीट गर्नुहोस्" : "Delete";
        }
    });

    $("#companyKendoGrid").kendoTooltip({
        filter: ".k-grid-edit",
        position: "bottom",
        content: function (e) {
            return lang == "ne" ? "काट्छाट गर्नुहोस्" : "Edit";
        }
    });

    $("#companyKendoGrid").kendoTooltip({
        filter: ".k-grid-approve",
        position: "bottom",
        content: function (e) {
            return lang == "ne" ? "रुजु गर्नुहोस्" : "Approve/Suspend";
        }
    });

    $("#companyKendoGrid").kendoTooltip({
        filter: ".k-grid-license",
        position: "bottom",
        content: function (e) {
            return lang == "ne" ? "रुजु गर्नुहोस्" : "License";
        }
    });

    self.CompanyLicenseFor = ko.observable('');
    self.CheckDuplicateSNo = function (item, event) {
        Riddha.ajax.get(url + "/CheckDuplicateSNo/?Code=" + item.Code())
            .done(function (result) {
                if (result == true) {
                    Riddha.UI.Toast("Code already exist!!!", Riddha.CRM.Global.ResultStatus.processError);
                    return self.Company().Code("");
                }
            })
    }
    self.CreateUpdate = function () {

        if (self.Company().Code() == "") {
            return Riddha.UI.Toast("Please enter code!!!", Riddha.CRM.Global.ResultStatus.processError);
        }

        if (self.Company().Name.hasError()) {
            return Riddha.UI.Toast(self.Company().Name.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Company().Address.hasError()) {
            return Riddha.UI.Toast(self.Company().Address.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Company().ContactNo.hasError()) {
            return Riddha.UI.Toast(self.Company().ContactNo.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Company().ContactPerson.hasError()) {
            return Riddha.UI.Toast(self.Company().ContactPerson.validationMessage(), Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Company().Email() == "") {
            return Riddha.UI.Toast("Please Email Address!!!", Riddha.CRM.Global.ResultStatus.processError);
        }
        if (self.Company().SoftwareType() == "0") {
            if (self.IsValid() == false) {
                return Riddha.UI.Toast("Invalid Password, Please Check u'r Password Valid or Not!!", 2);
            }
        }
        if (self.ModeOfButton() == 'Create') {
            var data = { Company: self.Company(), UserName: self.UserName(), Password: self.Password() };
            Riddha.ajax.post(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        //GetCompanies();
                        self.RefreshKendoGrid();
                        self.Reset();
                        self.CloseModal();
                    };
                    Riddha.UI.Toast(result.Message, result.Status);

                });
        }
        else if (self.ModeOfButton() == 'Update') {
            var data = { Company: self.Company(), UserName: self.UserName(), Password: self.Password() };
            Riddha.ajax.put(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        //GetCompanies();
                        self.RefreshKendoGrid();
                        self.ModeOfButton("Create");
                        self.Reset();
                        self.CloseModal();
                    };
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Reset = function () {
        self.Company(new CompanyModel({ Id: self.Company().Id() }));
        self.UserName([]);
        self.Password([]);
        //self.ModeOfButton('Create');
    };

    function Edit(e) {
        var grid = $("#companyKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));

        Riddha.ajax.get(url + "/?id=" + item.Id)
            .done(function (result) {
                self.Company(new CompanyModel(result.Data.Company));
                if (result.Data.Company.SoftwareType == 0) {
                    self.UserName(result.Data.UserName);
                    self.Password(result.Data.Password);
                }
                self.ModeOfButton('Update');
                self.ShowModal();
            });
    }

    function Delete(e) {
        var grid = $("#companyKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        if (item.SoftwareType == "Desktop") {
            Riddha.UI.Toast("Only web customer can be deleted.", 0);
            return;
        }
        Riddha.UI.Confirm("Confirm to Delete this Model?", function () {
            Riddha.ajax.delete(url + "/" + item.Id, null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        //Riddha.UI.Toast(result.Message, result.Status);
                        //self.Companies.remove(company);
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    }

    function ApproveSuspend(e) {
        var grid = $("#companyKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        if (item.SoftwareType == "Desktop") {
            Riddha.UI.Toast("Only web customer can be approved or suspend", 0);
            return;
        }
        if (item.Status == 'New') {
            return Riddha.UI.Toast("The login account is not registered for this Company", Riddha.CRM.Global.ResultStatus.processError);
        }
        Riddha.ajax.get(url + "/Suspend" + "/?id=" + item.Id)
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            })
    }

    function ShowCompLicenseModal(e) {
        var grid = $("#companyKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        if (item.SoftwareType == 'Desktop') {
            Riddha.UI.Toast("Only web customer can set License", 0);
            return;
        }
        Riddha.ajax.get(url + "/GetCompanyLicense?companyId=" + item.Id)
            .done(function (result) {
                if (result.Status == 4) {
                    self.CompanyLicense(new CompanyLicenseModel(result.Data));
                    if (result.Data.CompanyId == 0) {
                        self.LicenseStatus('New');
                        self.CompanyLicense().CompanyId(item.Id);
                        self.CompanyLicense().IssueDate(date);
                        self.CompanyLicense().ExpiryDate(defaultExpiryDate);
                    }
                    else {
                        self.LicenseStatus('Renew');
                    }
                    $("#companyLicenseCreationModel").modal('show');
                }
            });
    }

    self.ShowModal = function () {
        $("#companyCreationModel").modal('show');
    };

    //self.ShowCompLicenseModal = function (item) {
    //    Riddha.ajax.get(url + "/GetCompanyLicense?companyId=" + item.Id())
    //        .done(function (result) {
    //            if (result.Status == 4) {
    //                self.CompanyLicense(new CompanyLicenseModel(result.Data));
    //                if (result.Data.CompanyId == 0) {
    //                    self.LicenseStatus('New');
    //                    self.CompanyLicense().CompanyId(item.Id());
    //                    self.CompanyLicense().IssueDate(date);
    //                    self.CompanyLicense().ExpiryDate(defaultExpiryDate);
    //                }
    //                else {
    //                    self.LicenseStatus('Renew');
    //                }
    //                $("#companyLicenseCreationModel").modal('show');
    //            }
    //        });

    //};

    $("#companyLicenseCreationModel").on('hidden.bs.modal', function () {
        self.ResetCompanyLicense();
    });

    $("#companyCreationModel").on('hidden.bs.modal', function () {
        self.ModeOfButton("Create");
        self.Reset();
    });

    self.CloseModal = function () {
        $("#companyCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
    self.CloseCompanyLicenseModel = function () {
        $("#companyLicenseCreationModel").modal('hide');
    };
    self.ResetCompanyLicense = function () {
        self.CompanyLicense(new CompanyLicenseModel());
    };

    self.IsRequestComplete = ko.observable(true);
    self.SaveCompanyLicense = function () {
        self.IsRequestComplete(false);
        Riddha.ajax.post(url + "/SaveCompanyLicense", self.CompanyLicense())
            .done(function (result) {
                if (result.Status == 4) {
                    self.IsRequestComplete(true);
                    self.CloseCompanyLicenseModel();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.CheckDuplicateEmail = function (item, event) {
        var resx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!resx.test(item.Email())) {
            Riddha.UI.Toast("Invalid Emial Address!!!", Riddha.CRM.Global.ResultStatus.processError);
            return self.Company().Email("");
        }
        Riddha.ajax.get(url + "/CheckDuplicateEmail/?Email=" + item.Email())
            .done(function (result) {
                if (result == true) {
                    Riddha.UI.Toast("This Email already exist!!!", Riddha.CRM.Global.ResultStatus.processError);
                    return self.Company().Email("");
                }
            })
    }


}

function companyProfileController() {
    var self = this;
    var config = new Riddha.config();
    var url = "/Api/CompanyApi";
    self.Company = ko.observable(new CompanyModel());
    self.SoftwarePackage = ko.observable(config.PackageId);

    self.OrganizationTypes = ko.observableArray([
        { Id: '0', Name: 'NonGovernment' },
        { Id: '1', Name: 'Government' },
    ]);

    getCompanyProfile();
    function getCompanyProfile() {
        Riddha.ajax.get(url + "/GetComapnyProfile")
            .done(function (result) {
                self.Company(new CompanyModel(result.Data));
            })
    }
    self.CheckDuplicateSNo = function (item, event) {
        Riddha.ajax.get(url + "/CheckDuplicateSNo/?Code=" + item.Code())
            .done(function (result) {
                if (result == true) {
                    //user toast using process   
                    Riddha.UI.Toast("Code already exist!!!", Riddha.CRM.Global.ResultStatus.processError);
                    return self.Company().Code("");
                }
            })
    };

    self.UpdateCompanyProfile = function () {
        if (self.Company().Code() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Company().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        }
        if (self.Company().Address.hasError()) {
            return Riddha.util.localize.Required("Address");
        }
        if (self.Company().ContactNo.hasError()) {
            return Riddha.util.localize.Required("ContactNo");
        }
        if (self.Company().ContactPerson.hasError()) {
            return Riddha.util.localize.Required("ContactPerson");
        }
        if (self.Company().Email() == "") {
            return Riddha.util.localize.Required("Email");
        }
        Riddha.ajax.put(url + "/UpdateCompanyProfile", ko.toJS(self.Company()))
            .done(function (result) {
                Riddha.UI.Toast(result.Message, result.Status);
                if (result.Status == 4) {
                    localStorage.companyLogo = self.Company().LogoUrl();
                    setLogoUrl();
                }
            })
    }

    self.Reset = function (model) {
        self.Company(new CompanyModel({ Id: model.Id() }));
    };
}

//Shift
function shiftController() {
    var self = this;
    var url = "/Api/ShiftApi";
    var config = new Riddha.config();
    self.Shift = ko.observable(new ShiftModel());
    self.Shifts = ko.observableArray([]);
    self.SelectedShift = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.BranchId = ko.observable(0);
    self.Branches = ko.observableArray([]);

    getBranches();
    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Branches(data);
            });
    };

    self.ShiftType = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "बिहान" : "Morning" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "दिन" : "Day" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "साँझ्" : "Evening" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "रात्री" : "Night" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "डाइनामिक" : "Dynamic" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "दिन-अफ" : "Day-Off" },
        { Id: 6, Name: config.CurrentLanguage == 'ne' ? "रात-अफ" : "Night-Off" },
    ]);

    self.StartMonth = ko.observableArray([
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

    self.EndMonth = ko.observableArray([
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

    //GetShifts();
    //function GetShifts() {
    //    Riddha.ajax.get(url)
    //        .done(function (result) {
    //            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ShiftGridVm);
    //            self.Shifts(data);
    //        });
    //};
    self.Search = function () {
        self.GetShifts();
    };

    self.GetShifts = function myfunction() {
        Riddha.ajax.get(url + "/GetShift?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), ShiftGridVm);
                self.Shifts(data);
            });
    }

    self.CreateUpdate = function () {
        if (self.Shift().ShiftCode() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Shift().ShiftName() == "") {
            return Riddha.util.localize.Required("Name");
        }
        if (self.Shift().ShiftType() == 4) {
            if (self.ShiftHours() == "") {
                return Riddha.util.localize.Required("ShiftHours");
            }
        }
        else {
            if (self.Shift().ShiftStartTime() == "") {
                return Riddha.util.localize.Required("ShiftStartTime");
            }
            if (self.Shift().ShiftEndTime() == "") {
                return Riddha.util.localize.Required("ShiftEndTime");
            }
        }
        if (self.BranchId() != undefined) {
            self.Shift().BranchId(self.BranchId());
        }
        if (self.Shift().BranchId() == undefined) {
            Riddha.UI.Toast("Please select branch..")
            return;
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Shift()))
                .done(function (result) {
                    var shift = findShiftType("Id", result.Data.ShiftType);
                    if (shift) {
                        result.Data.ShiftType = shift.Name;
                    }
                    self.Shifts.push(new ShiftModel(result.Data));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.Reset();
                    self.CloseModal();
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Shift()))
                .done(function (result) {
                    var shift = findShiftType("Id", result.Data.ShiftType);
                    if (shift) {
                        result.Data.ShiftType = shift.Name;
                    }
                    self.Shifts.replace(self.SelectedShift(), new ShiftModel(ko.toJS(result.Data)));
                    Riddha.UI.Toast(result.Message, result.Status);
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                });
        }
    };

    function findShiftType(key, value) {
        var shift = ko.utils.arrayFirst(self.ShiftType(), function (item) {
            return item[key] == value;

        });
        return shift;
    }

    self.Reset = function () {
        self.Shift(new ShiftModel({ Id: self.Shift().Id() }));
    };

    self.Select = function (model) {
        self.SelectedShift(model);
        var id = 0;
        //shift Type id
        var shift = findShiftType("Name", model.ShiftType());
        if (shift) {
            id = shift.Id;
        }
        var data = ko.toJS(model);
        data.ShiftType = id;
        self.Shift(new ShiftModel(data));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (shift) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + shift.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.Shifts.remove(shift);
                    }
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#shiftCreationModel").modal('show');
    };

    $("#shiftCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#shiftCreationModel").modal('hide');
        self.Reset();
        self.ModeOfButton("Create");
    };

    self.TimeDeff = ko.computed(function () {
        var st = self.Shift().ShiftStartTime();
        var et = self.Shift().ShiftEndTime();
        var gst = self.Shift().ShiftStartGrace();
        var get = self.Shift().ShiftEndGrace();

        var startDiff = Riddha.util.getTimeAdd(st, gst);
        var endDiff = Riddha.util.getTimeDiff(et, get);
        return startDiff + " - " + endDiff;
    });

    self.ShiftHours = ko.computed(function () {
        var st = self.Shift().ShiftStartTime();
        var et = self.Shift().ShiftEndTime();
        var diff = Riddha.util.getTimeDiff(et, st);
        return diff;
    });

}

//LeaveMaster
function leaveMasterController() {
    var self = this;
    var config = new Riddha.config();
    var url = "/Api/LeaveMasterApi";
    self.LeaveMaster = ko.observable(new LeaveMasterModel());
    self.LeaveMasters = ko.observableArray([]);
    self.SelectedLeaveMaster = ko.observable();
    self.SelectedLeaveMasterIds = [];
    self.ModeOfButton = ko.observable('Create');

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

    self.CreateUpdate = function () {
        if (self.LeaveMaster().Code.hasError()) {
            return Riddha.util.localize.Required("Code");
        }
        if (self.LeaveMaster().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        }
        if (self.LeaveMaster().IsReplacementLeave() == false) {
            if (self.LeaveMaster().Balance.hasError()) {
                return Riddha.util.localize.Required("Number of Days");
            }
            if (self.LeaveMaster().Description.hasError()) {
                return Riddha.util.localize.Required("Description");
            }
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.LeaveMaster()))
                .done(function (result) {
                    if (result.Status == 4) {
                        var gender = findApplicableGender("Id", result.Data.ApplicableGender);
                        if (gender) {
                            result.Data.ApplicableGender = gender.Name;
                        }
                        self.LeaveMasters.push(new LeaveMasterModel(result.Data));
                        self.RefreshKendoGrid();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.LeaveMaster()))
                .done(function (result) {
                    if (result.Status == 4) {
                        var gender = findApplicableGender("Id", result.Data.ApplicableGender);
                        if (gender) {
                            result.Data.ApplicableGender = gender.Name;
                        }
                        self.LeaveMasters.replace(self.SelectedLeaveMaster(), new LeaveMasterModel(ko.toJS(result.Data)));
                        self.ModeOfButton("Create");
                        self.RefreshKendoGrid();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    function findApplicableGender(key, value) {
        var gender = ko.utils.arrayFirst(self.ApplicableGender(), function (item) {
            return item[key] == value;

        });
        return gender;
    }

    self.Reset = function () {
        self.LeaveMaster(new LeaveMasterModel({ Id: self.LeaveMaster().Id() }));
        self.ModeOfButton("Create");
    };
    self.Select = function (model) {
        if (self.SelectedLeaveMaster() == undefined || self.SelectedLeaveMaster().length > 1 || self.SelectedLeaveMaster().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        self.LeaveMaster(new LeaveMasterModel(ko.toJS(self.SelectedLeaveMaster())));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (leavemaster) {
        if (self.SelectedLeaveMaster() == undefined || self.SelectedLeaveMaster().length > 1 || self.SelectedLeaveMaster().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedLeaveMaster().Id(), null)
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

    self.ShowModal = function () {
        $("#leaveMasterCreationModel").modal('show');
    };

    $("#leaveMasterCreationModel").on('hidden.bs.modal', function () {
        self.RefreshKendoGrid();
        self.Reset();
    });

    self.CloseModal = function () {
        $("#leaveMasterCreationModel").modal('hide');
        self.RefreshKendoGrid();
        self.ModeOfButton("Create");
        self.Reset();
    };

    self.PullDefaultLeave = function () {
        Riddha.UI.Confirm("PullConfirm", function () {
            Riddha.ajax.get(url + "/PullLeaveMaster")
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    }

    //Kendo Grid
    self.KendoGridOptions = {
        title: "LeaveMaster",
        target: "#leaveMasterKendoGrid",
        url: "/Api/LeaveMasterApi/GetLeaveMasterKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        selectable: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: lang == "ne" ? "कोड" : "Code", width: 100 },
            //{ field: 'Name', title: lang == "ne" ? "नाम" : "Name", width: 270 },
            { field: 'Name', title: "Name", width: 270, hidden: lang == "en" ? false : true, width: 180 },
            { field: 'NameNp', title: "नाम", width: 270, hidden: lang == "ne" ? false : true, template: "#=NameNp==null?Name:NameNp#", width: 180 },
            { field: 'Balance', title: lang == "ne" ? "दिन" : "Number Of Days", filterable: false, template: "#=SuitableNumber(Balance)#", width: 100 },
            { field: 'ApplicableGender', title: lang == "ne" ? "लिङग" : "Applicable Gender", filterable: false, template: "#=getLeaveMasterApplicableGenderName(ApplicableGender)#", width: 110 },
            { field: 'IsPaidLeave', title: lang == "ne" ? "तलबिय बिदा" : "Is Paid Leave", template: "#=getYesNoNameByValue(IsPaidLeave)#", filterable: false, width: 100 },
            { field: 'IsLeaveCarryable', title: lang == "ne" ? "सार्नमिल्ने बिदा" : "Is Leave Carryable", template: "#=getYesNoNameByValue(IsLeaveCarryable)#", filterable: false, width: 110 },
            { field: 'IsReplacementLeave', title: lang == "ne" ? "प्रतिस्थापन बिदा" : "Is Replacement Leave", template: "#=getYesNoNameByValue(IsReplacementLeave)#", filterable: false },
            { field: 'MaximumLeaveBalance', title: lang == "ne" ? "अधिकतम बिदा शेष रकम" : "Maximum Leave Balance" },

        ],
        SelectedItem: function (item) {
            self.SelectedLeaveMaster(new LeaveMasterModel(item));
        },
        SelectedItems: function (items) {
            ko.utils.arrayForEach(items, function (data) {
                self.SelectedLeaveMasterIds.push(data.Id);
            })
        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedLeaveMaster(new LeaveMasterModel());
        $("#leaveMasterKendoGrid").getKendoGrid().dataSource.read();
    }
}

//Section
function sectionController() {
    var self = this;
    self.Section = ko.observable(new SectionModel());
    self.Sections = ko.observableArray([]);
    self.SelectedSection = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.Departments = ko.observableArray([]);
    var url = "/Api/SectionApi";
    self.Branches = ko.observableArray([]);
    self.BranchId = ko.observable(0);
    self.GridIsOpen = ko.observable(false);

    getBranches();
    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Branches(data);
            });
    };


    self.GetDepatmentName = function (id) {
        var mapped = ko.utils.arrayFirst(ko.toJS(self.Departments()), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    function getDepartments() {
        if (self.BranchId() == undefined) {
            return;
        }
        Riddha.ajax.get(url + "/GetDepartmentsForDropdown?branchId=" + self.BranchId(), null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Departments(data);
            });
    }



    self.CreateUpdate = function () {
        if (self.Section().DepartmentId() == undefined) {
            return Riddha.util.localize.Required("Department");
        }
        if (self.Section().Code.hasError()) {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Section().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        }
        if (self.BranchId() != undefined) {
            self.Section().BranchId(self.BranchId());
        }
        if (self.Section().BranchId() == undefined) {
            Riddha.UI.Toast("Please select branch..")
            return;
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Section()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.Section()))
                .done(function (result) {
                    self.Sections.replace(self.SelectedSection(), new SectionModel(ko.toJS(self.Section())));
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Reset = function () {
        self.Section(new SectionModel({ Id: self.Section().Id() }));
        self.ModeOfButton("Create");
    };

    self.Select = function (model) {
        if (self.SelectedSection() == undefined || self.SelectedSection().length > 1 || self.SelectedSection().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        self.Section(new SectionModel(ko.toJS(self.SelectedSection())));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (section) {
        if (self.SelectedSection() == undefined || self.SelectedSection().length > 1 || self.SelectedSection().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedSection().Id(), null)
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

    self.ShowModal = function () {
        getDepartments();
        $("#sectionCreationModel").modal('show');
    };

    $("#sectionCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#sectionCreationModel").modal('hide');
        self.Reset();
    };



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
        title: "Section Feed Entry",
        target: "#sectionKendoGrid",
        url: "/Api/SectionApi/GetSectionKendoGrid",
        height: 500,
        paramData: function () { return { BranchId: self.BranchId() } },
        multiSelect: false,
        selectable: true,
        group: true,
        groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: lang == "ne" ? "कोड" : "Code", width: 100 },
            { field: 'Name', title: "Section Name", width: 400, hidden: lang == "en" ? false : true },
            { field: 'NameNp', title: "फाँटको नाम", width: 400, hidden: lang == "ne" ? false : true, template: "#=NameNp==null?Name:NameNp#" },
            { field: 'DepartmentName', title: lang == "ne" ? "विभागको नाम" : "Department" },
            { field: 'BranchName', title: lang == "ne" ? "शाखा" : "Branch" },
        ],
        SelectedItem: function (item) {
            self.SelectedSection(new SectionModel(item));
        },
        SelectedItems: function (items) {

        },
        open: function (callback) {
            self.checkCallBack = callback;
        },
    };

    self.RefreshKendoGrid = function () {
        if (self.GridIsOpen() == true) {
            self.SelectedSection(new SectionModel());
            $("#sectionKendoGrid").getKendoGrid().dataSource.read();
        }
    }

    self.DestroyGrid = function () {
        var grid = $("#sectionKendoGrid").getKendoGrid()
        grid.destroy();
    }

    //Excel Opreation Starts from here
    self.trigerFileBrowse = function () {
        $("#UploadedFile").click();
    };

    self.UploadClick = function () {
        var xhr = new XMLHttpRequest();
        var file = document.getElementById('UploadedFile').files[0];
        if (file == undefined) {
            return Riddha.UI.Toast("Please Select Excel File to Upload", Riddha.CRM.Global.ResultStatus.processError);
        }
        Riddha.UI.Confirm("ExcelUploadConfirm", function () {
            xhr.open("POST", "/api/SectionApi/Upload");
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
}

// yo function LeaveMaster kendo grid ma enum ko value language anusar name lerauna use garey ko xa..
function getLeaveMasterApplicableGenderName(id) {
    var self = this;
    self.ApplicableGender = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सबै" : "All" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "पुरुष" : "Male" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "महिला" : "Female" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "अन्य" : "Others" },
    ]);
    var mapped = ko.utils.arrayFirst(self.ApplicableGender(), function (data) {
        return data.Id == id;
    });
    return mapped = (mapped || { Name: '' }).Name;
}

// yo function LeaveMaster kendo grid ma bool ko value language anusar name lerauna use garey ko xa..
function getYesNoNameByValue(id) {
    var self = this;
    self.value = ko.observable([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "होइन" : "No" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "हो" : "Yes" },
    ]);
    var mapped = ko.utils.arrayFirst(self.value(), function (data) {
        return data.Id == id;
    });
    return mapped = (mapped || { Name: '' }).Name;
}



function getSoftwareTypeTemp(name) {
    if (name == 'Desktop') {
        return "<span class='badge bg-aqua'>" + name + "</span>";
    }
    else {
        return "<span class='badge bg-green'>" + name + "</span>";
    }
};

function getSoftwarePackageTypeTemp(name) {
    if (name == 'Silver') {
        return "<span class='badge bg-silver'>" + name + "</span>";
    }
    else if (name == 'Gold') {
        return "<span class='badge bg-yellow'>" + name + "</span>";
    }
    else if (name == 'Platinum') {
        return "<span class='badge bg-aqua'>" + name + "</span>";
    }
    else {
        return "<span class='badge bg-green'>" + name + "</span>";
    }
};

function getCustomerStatusTemp(name) {
    if (name == 'New') {
        return "<span class='badge bg-aqua'>" + name + "</span>";
    }
    else if (name == 'Approved') {
        return "<span class='badge bg-green'>" + name + "</span>";
    }
    else {
        return "<span class='badge bg-red'>" + name + "</span>";
    }
};
