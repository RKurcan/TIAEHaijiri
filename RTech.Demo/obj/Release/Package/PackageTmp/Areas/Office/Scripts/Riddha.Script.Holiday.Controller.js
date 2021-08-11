/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Holiday.Model.js" />
function holidayController() {
    var self = this;
    var config = new Riddha.config();
    url = "/Api/HolidayApi";
    self.ModeOfButton = ko.observable('Create');
    self.Holiday = ko.observable(new HolidayModel());
    self.Holidays = ko.observableArray([]);
    self.HolidayDetail = ko.observable(new HolidayDetailModel());
    self.HolidayDetails = ko.observableArray([]);
    self.FiscalYear = ko.observableArray([]);
    self.Lang = ko.observable(config.CurrentLanguage);

    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.Departments = ko.observableArray([]);
    self.Sections = ko.observableArray([]);
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

    getFiscalYear();
    self.SelectedHoliday = ko.observable();
    self.ApplicableGender = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सबै" : "All" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "पुरुष" : "Male" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "महिला" : "Female" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "अन्य" : "Others" },
    ]);

    self.GetApplicableGenderName = function (id) {
        var mapped = ko.utils.arrayFirst(self.ApplicableGender(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.ApplicableReligion = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सबै" : "All" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "हिन्दू" : "Hinduism" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "बौद्ध " : "Buddhism" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "मुस्लिम " : "Islam" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "ईसाइ" : "Christianity" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "जुदाइजम " : "Judaism" },
    ]);

    self.GetApplicableReligionName = function (id) {
        var mapped = ko.utils.arrayFirst(self.ApplicableReligion(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.HolidayType = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "धार्मिक" : "Religious" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "अधार्मिक" : "NonReligious" },
    ]);

    self.GetHolidayTypeName = function (id) {
        var mapped = ko.utils.arrayFirst(self.HolidayType(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    function getFiscalYear() {
        Riddha.ajax.get("/Api/HolidayApi/GetFiscalYear", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DropDownModel);
                self.FiscalYear(data);
            });
    };

    self.GetFiscalYearName = function (id) {
        var mapped = ko.utils.arrayFirst(self.FiscalYear(), function (data) {
            return data.Id() == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };



    self.CreateUpdate = function () {
        self.IsValid = ko.observable(false);
        ko.utils.arrayForEach(self.Departments(), function (result) {
            if (result.Checked()) {
                self.IsValid(true);
            }
        });
        if (self.IsValid() == false) {
            Riddha.UI.Toast('Please select department to save holiday.', 0);
            return;
        }

        if (self.Holiday().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        };
        if (self.BranchId() != undefined) {
            self.Holiday().BranchId(self.BranchId);
        }
        if (self.Holiday().BranchId() == undefined) {
            Riddha.UI.Toast("Please select branch..")
            return;
        }

        var sec = getSelectedSec();
        if (self.ModeOfButton() == 'Create') {
            var data = { Holiday: self.Holiday(), HolidayDetails: self.HolidayDetails(), SectionIds: sec };
            Riddha.ajax.post(url, ko.toJS(data))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);

                });
        }
        else if (self.ModeOfButton() == 'Update') {
            var data = { Holiday: self.Holiday(), HolidayDetails: self.HolidayDetails(), SectionIds: sec };
            Riddha.ajax.put(url, ko.toJS(data))
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
        self.Holiday(new HolidayModel({ Id: self.Holiday().Id() }));
        self.HolidayDetails([]);
        self.ModeOfButton('Create');
        self.CheckAllDepartments(false);
        self.CheckAllSections(false);
        ko.utils.arrayForEach(self.Departments(), function (dep) {
            dep.Checked(false);
        })
        self.Sections([]);
    };


    self.depArray = ko.observableArray([]);
    self.secArray = ko.observableArray([]);
    self.Select = function (holiday) {
        if (self.SelectedHoliday() == undefined || self.SelectedHoliday().length > 1 || self.SelectedHoliday().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        Riddha.ajax.get(url + "/GetHoliday/" + self.SelectedHoliday().Id(), null)
            .done(function (result) {
                self.Holiday(new HolidayModel(result.Data.Holiday));
                var detailData = Riddha.ko.global.arrayMap(result.Data.HolidayDetails, HolidayDetailModel);
                self.HolidayDetails(detailData);

                self.depArray(result.Data.DepartmentIds.split(','));
                self.secArray(result.Data.SectionIds.split(','));
                ko.utils.arrayForEach(self.depArray(), function (data) {
                    ko.utils.arrayForEach(self.Departments(), function (dep) {
                        if (data == dep.Id()) {
                            dep.Checked(true);
                        }
                    })
                });
                self.GetSections();
                Riddha.util.delayExecute(function () {
                    ko.utils.arrayForEach(self.secArray(), function (data) {
                        ko.utils.arrayForEach(self.Sections(), function (sec) {
                            if (data == sec.Id()) {
                                sec.Checked(true);
                            }
                        })
                    });
                }, 500);

                self.ModeOfButton("Update");
                self.ShowModal();
                self.SelectedHoliday(holiday);
            });
    };


    self.Delete = function (holiday) {
        if (self.SelectedHoliday() == undefined || self.SelectedHoliday().length > 1 || self.SelectedHoliday().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedHoliday().Id(), null)
                .done(function (result) {
                    self.RefreshKendoGrid();
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };


    self.ShowModal = function () {
        getDepartments();
        $("#holidayCreationModel").modal('show');
    };

    $("#holidayCreationModel").on('hidden.bs.modal', function () {
        self.HolidayDetailsModeOfButton('Add');
        self.ResetHolidayDetail();
        self.Reset();
        self.RefreshKendoGrid();
    });

    self.CloseModal = function () {
        $("#holidayCreationModel").modal('hide');
        self.HolidayDetailsModeOfButton('Add');
        self.ResetHolidayDetail();
        self.Reset();
        self.RefreshKendoGrid();
    };

    self.PullDefaultHoliday = function () {
        Riddha.UI.Confirm("PullConfirm", function () {
            Riddha.ajax.get(url + "/PullHolidays")
                .done(function (result) {
                    if (result.Status == 4) {
                        //getHolidays();
                        self.RefreshKendoGrid();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        });
    }

    self.HolidayDetailsModeOfButton = ko.observable('Add');
    self.AddHolidayDetails = function (item) {
        if (self.HolidayDetailsModeOfButton() == 'Add') {
            if (self.HolidayDetail().FiscalYearId() == undefined) {
                return false;
            }
            if (self.HolidayDetail().BeginDate() == "NaN/aN/aN" || self.HolidayDetail().EndDate() == "NaN/aN/aN" || self.HolidayDetail().EndDate() == "" || self.HolidayDetail().BeginDate() == "") {
                return false;
            }
            if (self.HolidayDetail().BeginDate() > self.HolidayDetail().EndDate()) {
                return false;
            }
            self.HolidayDetails.push(new HolidayDetailModel(ko.toJS(item)));
            self.ResetHolidayDetail();
        }
        else {

        }
        self.HolidayDetail(new HolidayDetailModel());
        self.HolidayDetailsModeOfButton('Add');
    };

    self.SelectHolidayDetail = function (model) {
        self.HolidayDetail(model);
        self.HolidayDetailsModeOfButton('Update');
    };

    self.ResetHolidayDetail = function () {
        self.HolidayDetail(new HolidayDetailModel({ Id: self.HolidayDetail().Id() }));
        self.HolidayDetailsModeOfButton = ko.observable('Add');
    };

    self.DeleteHolidayDetail = function (model) {
        self.HolidayDetails.remove(model);
    };

    self.GetNoOfDays = function () {
        if (self.HolidayDetail().BeginDate() == "NaN/aN/aN" || self.HolidayDetail().EndDate() == "NaN/aN/aN" || self.HolidayDetail().EndDate() == "" || self.HolidayDetail().BeginDate() == "") {
            return false;
        }
        if (self.HolidayDetail().BeginDate() == self.HolidayDetail().EndDate()) {
            self.HolidayDetail().NumberOfDays(1);
            return false;
        }
        var noOfDays = getNoOfDays(self.HolidayDetail().BeginDate(), self.HolidayDetail().EndDate());
        self.HolidayDetail().NumberOfDays(noOfDays);
    }

    function getNoOfDays(fromDate, toDate) {
        var date1 = new Date(fromDate);
        var date2 = new Date(toDate);
        var timeDiff = date2.getTime() - date1.getTime();
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        if (diffDays < 0) {
            self.HolidayDetail().EndDate('');
            return 0;
        }
        else {
            return diffDays + 1;
        }
    }

    //Kendo Grid Starts from here
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
        title: "Holiday",
        target: "#holidayKendoGrid",
        url: "/Api/HolidayApi/GetHolidayKendoGrid",
        height: 490,
        paramData: function () { return { BranchId: self.BranchId() } },
        multiSelect: true,
        selectable: true,
        group: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Date', title: lang == "ne" ? "मिति" : "Date", width: 100, template: "#=SuitableDate(Date)#", filterable: false },
            { field: 'Name', title: "Holiday Name", width: 270, hidden: lang == "en" ? false : true },
            { field: 'NameNp', title: "छुट्टीको नाम ", width: 270, hidden: lang == "ne" ? false : true, template: "#=NameNp==null?Name:NameNp#", filterable: false },
            { field: 'HolidayType', title: lang == "ne" ? "छुट्टीको प्रकार" : "Holiday Type", filterable: false, template: "#=getHolidayTypeName(HolidayType)#" },
            { field: 'ApplicableReligion', title: lang == "ne" ? "लागू हुने धर्म्" : "Applicable Religion", filterable: false, template: "#=getHolidayApplicableReligionName(ApplicableReligion)#" },
            { field: 'ApplicableGender', title: lang == "ne" ? "लागू हुने लिङग" : "Applicable Gender", filterable: false, template: "#=getHolidayApplicableGenderName(ApplicableGender)#" },
        ],
        SelectedItem: function (item) {
            self.SelectedHoliday(new HolidayModel(item));
        },
        SelectedItems: function (items) {

        },
        open: function (callback) {
            self.checkCallBack = callback;
        },
    };

    self.RefreshKendoGrid = function () {
        self.SelectedHoliday(new HolidayModel());
        $("#holidayKendoGrid").getKendoGrid().dataSource.read();
    }

    self.DestroyGrid = function () {
        var grid = $("#holidayKendoGrid").getKendoGrid()
        grid.destroy();
    }

    //Department Wise Holiday added by Raz on nov 5th 2019

    function getDepartments() {
        //Riddha.ajax.get("/Api/EmployeeApi/GetDepartment", null)
        Riddha.ajax.get("/Api/EmployeeApi/GetDepartmentsForDropdown?branchId=" + self.BranchId(), null)
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
            Riddha.ajax.get("/Api/HolidayApi/GetSections?depIds=" + departments)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Sections(data);
                });
        }
    };


    function getSelectedDep() {
        var dep = "";
        ko.utils.arrayForEach(self.Departments(), function (data) {
            if (data.Checked() == true) {
                if (dep.length != 0)
                    dep += "," + data.Id();
                else
                    dep = data.Id() + '';
            }
        });
        return dep;
    }

    function getSelectedSec() {
        var sec = "";
        ko.utils.arrayForEach(self.Sections(), function (data) {
            if (data.Checked() == true) {
                if (sec.length != 0)
                    sec += "," + data.Id();
                else
                    sec = data.Id() + '';
            }
        });
        return sec;
    }
}
// yo function holiday kendo grid ma enum ko language anusar name lerauna use garey ko xa.. 
function getHolidayTypeName(id) {
    var self = this;
    self.HolidayTypes = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "धार्मिक" : "Religious" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "अधार्मिक" : "NonReligious" },
    ]);
    var mapped = ko.utils.arrayFirst(self.HolidayTypes(), function (data) {
        return data.Id == id;
    });
    return mapped = (mapped || { Name: '' }).Name;
}

// yo function holiday kendo grid ma enum ko language anusar name lerauna use garey ko xa..
function getHolidayApplicableGenderName(id) {
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


// yo function holiday kendo grid ma enum ko language anusar name lerauna use garey ko xa..
function getHolidayApplicableReligionName(id) {
    var self = this;
    self.ApplicableReligion = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सबै" : "All" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "हिन्दू" : "Hinduism" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "बौद्ध " : "Buddhism" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "मुस्लिम " : "Islam" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "ईसाइ" : "Christianity" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "जुदाइजम " : "Judaism" },
    ]);
    var mapped = ko.utils.arrayFirst(self.ApplicableReligion(), function (data) {
        return data.Id == id;
    });
    return mapped = (mapped || { Name: '' }).Name;
}