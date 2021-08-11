/// <reference path="Riddha.Script.Office.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Department.Model.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />

function departmentController() {
    var self = this;
    var url = "/Api/DepartmentApi";
    self.Department = ko.observable(new DepartmentModel());
    self.SelectedDepartment = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.Devices = ko.observableArray([]);
    self.Branches = ko.observableArray([]);


    getBranches();
    function getBranches() {
        Riddha.ajax.get("/Api/BranchApi/GetBranchForDropdown", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
                self.Branches(data);
            });
    };

    getDevices();
    function getDevices() {
        Riddha.ajax.get("/Api/DepartmentApi/GetDevices", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Devices(data);
            });
    };

    //self.CreateUpdate = function () {
    //    if (self.Department().Code() == "") {
    //        return Riddha.util.localize.Required("Code");
    //    }
    //    if (self.Department().Name() == "") {
    //        return Riddha.util.localize.Required("Name");
    //    }
    //    if (self.ModeOfButton() == 'Create') {
    //        Riddha.ajax.post(url, ko.toJS(self.Department()))
    //        .done(function (result) {
    //            if (result.Status == 4) {
    //                self.RefreshKendoGrid();
    //                self.Reset();
    //                self.CloseModal();
    //            }
    //            Riddha.UI.Toast(result.Message, result.Status);
    //        });
    //    }
    //    else if (self.ModeOfButton() == 'Update') {
    //        Riddha.ajax.put(url, ko.toJS(self.Department()))
    //        .done(function (result) {
    //            if (result.Status == 4) {
    //                self.RefreshKendoGrid();
    //                self.ModeOfButton("Create");
    //                self.Reset();
    //                self.CloseModal();
    //            }
    //            Riddha.UI.Toast(result.Message, result.Status);
    //        });
    //    }
    //};
    self.CreateUpdate = function () {
        if (self.Department().BranchId() == undefined) {
            Riddha.util.localize.Required("Branch");
            return;
        }
        if (self.Department().Code() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Department().Name() == "") {
            return Riddha.util.localize.Required("Name");
        }
        var data = { Department: self.Department(), Devices: self.Devices() };
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(data))
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
            Riddha.ajax.put(url, ko.toJS(data))
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };


    self.Reset = function () {
        self.Department(new DepartmentModel({ Id: self.Department().Id() }));
        ko.utils.arrayForEach(self.Devices(), function (data) {
            data.Checked(false);
        })
        //self.ModeOfButton('Create');
    };

    self.Select = function (model) {
        if (self.SelectedDepartment() == undefined || self.SelectedDepartment().length > 1 || self.SelectedDepartment().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        };
        Riddha.ajax.get(url + "/GetDepartment/" + self.SelectedDepartment().Id(), null)
        .done(function (result) {
            self.Department(new DepartmentModel(result.Data.Department));
            var devicewiseDepartment = Riddha.ko.global.arrayMap(result.Data.Devices, checkBoxModel);
            ko.utils.arrayForEach(devicewiseDepartment, function (data) {
                var mapped = Riddha.ko.global.Compare(self.Devices, 'Id', data.Id);
                if (mapped) {
                    mapped.Checked(true);
                }
            });
            self.ModeOfButton('Update');
            self.ShowModal();
        })

    };

    self.Delete = function (department) {
        if (self.SelectedDepartment() == undefined || self.SelectedDepartment().length > 1 || self.SelectedDepartment().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedDepartment().Id(), null)
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
        $("#departmentCreationModel").modal('show');
    };

    $("#departmentCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#departmentCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };

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
            xhr.open("POST", "/api/DepartmentApi/Upload");
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

    self.KendoGridOptions = {
        title: "Department",
        target: "#depKendoGrid",
        url: "/Api/DepartmentApi/GetDepKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        selectable: true,
        group: true,
        toolbar: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: lang == "ne" ? "कोड" : "Code", width: 100, filterable: true },
            { field: 'Name', title: "Department Name", width: 400, hidden: lang == "en" ? false : true, filterable: true },
            { field: 'NameNp', title: "विभागको नाम ", width: 400, hidden: lang == "ne" ? false : true, template: "#=NameNp==null?Name:NameNp#", filterable: true },
            { field: 'NumberOfStaff', title: lang == "ne" ? "कर्मचारी संख्या" : "Number of Staff", template: "#=SuitableNumber(NumberOfStaff)#", filterable: false },
            { field: 'BranchName', title: lang == "ne" ? "शाखा" : "Branch", width: 250, filterable: true },
        ],
        SelectedItem: function (item) {
            self.SelectedDepartment(new DepartmentModel(item));
        },
        SelectedItems: function (items) {
            //var data = Riddha.ko.global.arrayMap(items, DepartmentGridVm);
            //self.SelectedDepartment(data);
        }
    };
    self.RefreshKendoGrid = function () {
        self.SelectedDepartment(new DepartmentModel());
        $("#depKendoGrid").getKendoGrid().dataSource.read();
    }
}

function fiscalYearController() {
    var self = this;
    self.FiscalYear = ko.observable(new FiscalYearModel());
    self.FiscalYears = ko.observableArray([]);
    self.SelectedFiscalYear = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    var url = "/Api/FiscalYearApi";
    GetFiscalYears();

    function GetFiscalYears() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), FiscalYearGridVm);
            self.FiscalYears(data);
        });
    };

    self.CreateUpdate = function () {
        if (self.FiscalYear().FiscalYear.hasError()) {
            return Riddha.util.localize.Required("FiscalYear");
        }
        if (self.FiscalYear().StartDate() == "NaN/aN/aN") {
            // return Riddha.UI.Toast("Start Date is Requierd", 2)
            return Riddha.util.localize.Required("StartDateIsRequired");
        }
        if (self.FiscalYear().EndDate() == "NaN/aN/aN") {
            // return Riddha.UI.Toast("End Date is Requierd", 2)
            return Riddha.util.localize.Required("EndDateIsRequired");
        }
        if (self.FiscalYear().StartDate() > self.FiscalYear().EndDate()) {
            //return Riddha.UI.Toast("TheEndDatemustbegreatethantheStartDate");
            return Riddha.util.localize.Required("TheEndDatemustbegreatethantheStartDate");
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.FiscalYear()))
                     .done(function (result) {
                         if (result.Status == 4) {
                             GetFiscalYears();
                             self.Reset();
                             self.CloseModal();
                         };
                         Riddha.UI.Toast(result.Message, result.Status);
                     });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.FiscalYear()))
            .done(function (result) {
                if (result.Status == 4) {
                    GetFiscalYears();
                    self.ModeOfButton("Create");
                    self.Reset();
                    self.CloseModal();
                };
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };

    self.Reset = function () {
        self.FiscalYear(new FiscalYearModel({ Id: self.FiscalYear().Id() }));
        self.ModeOfButton("Create");

    };

    self.Select = function (model) {
        self.SelectedFiscalYear(model);
        self.FiscalYear(new FiscalYearModel(ko.toJS(model)));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (FiscalYear) {
        if (FiscalYear.CurrentFiscalYear() == true) {
            Riddha.UI.Toast("Current Fiscal Year cannot be deleted..");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + FiscalYear.Id(), null)
            .done(function (result) {
                self.FiscalYears.remove(FiscalYear);
                self.ModeOfButton("Create");
                self.Reset();
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    self.ShowModal = function () {
        $("#fiscalYearCreationModel").modal('show');
    };

    $("#fiscalYearCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#fiscalYearCreationModel").modal('hide');
        self.ModeOfButton("Create");
    };
}

function noticeController() {
    var self = this;
    var url = "/Api/NoticeApi";
    self.Notice = ko.observable(new NoticeModel());
    self.Notices = ko.observableArray([]);
    self.SelectedNotice = ko.observable();
    self.Targets = ko.observableArray([]);
    self.SelectedTargets = ko.observableArray([]);
    self.CheckAllTargets = ko.observable(false);
    self.ModeOfButton = ko.observable('Create');
    self.NoticeLevels = ko.observableArray([
   { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सबै " : "All" },
   { Id: 1, Name: config.CurrentLanguage == 'ne' ? "शाखा " : "Branch" },
   { Id: 2, Name: config.CurrentLanguage == 'ne' ? "विभाग " : "Department" },
   { Id: 3, Name: config.CurrentLanguage == 'ne' ? "फाँट" : "Section" },
    ]);

    self.GetNoticeLevelName = function (id) {
        var mapped = ko.utils.arrayFirst(self.NoticeLevels(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };
    self.GetTargets = function () {
        self.Targets([]);
        self.SelectedTargets([]);
        if (self.Notice().NoticeLevel() == 1) {
            getBranches();
        }
        else if (self.Notice().NoticeLevel() == 2) {
            getDepartments();
        }
        else if (self.Notice().NoticeLevel() == 3) {
            getSections();
        }
    }
    Riddha.util.delayExecute(function () {
        GetNoticeGridLst();
    }, 500);
    //function GetNoticeGridLst() {
    //    Riddha.ajax.get(url)
    //    .done(function (result) {
    //        var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), NoticeModel);
    //        self.Notices(data);
    //    });
    //};

    function GetNoticeGridLst() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), NoticeGridVm);
            self.Notices(data);
        });
    };

    function getBranches() {
        Riddha.ajax.get(url + "/GetBranches")
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
            self.Targets(data);
        });
    };
    function getDepartments() {
        Riddha.ajax.get(url + "/GetDepartments", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
            self.Targets(data);
        });
    };
    function getSections() {
        Riddha.ajax.get(url + "/GetSections", null)
        .done(function (result) {
            var datas = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
            self.Targets(datas);
        });
    };
    self.CheckAllTargets.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Targets(), function (item) {
            item.Checked(newValue);
        });

    });

    self.CreateUpdate = function (item) {
        if (self.Notice().Title.hasError()) {
            return Riddha.util.localize.Required("Title ");

        }
        if (self.Notice().Description() == '') {
            return Riddha.util.localize.Required("Description");
        }
        if (self.Notice().PublishedOn() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("PublishedOn");
        }
        if (self.Notice().ExpiredOn() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("ExpiredDate");
        }
        if (self.Notice().PublishedOn() > self.Notice().ExpiredOn()) {
            return Riddha.UI.Toast("The Expired Date must be greater than the Publish Date");
        }

        if (self.SelectedTargets().length <= 0 && self.Notice().NoticeLevel() != 0) {
            return Riddha.util.localize.Required("Target ");

        }
        var data = { Notice: self.Notice(), Targets: self.SelectedTargets() };
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(data))
            .done(function (result) {
                self.Notices.push(new NoticeModel(result.Data));
                GetNoticeGridLst();
                Riddha.UI.Toast(result.Message, result.Status);
                self.CloseModal();
            });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(data))
             .done(function (result) {
                 if (result.Status == 4) {
                     GetNoticeGridLst();
                     //self.Notices.replace(self.SelectedNotice(), new NoticeModel(ko.toJS(item)));
                     self.CloseModal();
                     self.ModeOfButton('Create');
                 }
                 Riddha.UI.Toast(result.Message, result.Status);
             });
        }
    };
    self.Select = function (model) {
        Riddha.ajax.get(url + "?id=" + model.Id())
        .done(function (result) {
            if (result.Status == 4) {
                self.SelectedNotice(result.Data.Notice);
                self.Notice(new NoticeModel(result.Data.Notice));
                self.ShowModal();
                debugger;
                self.GetTargets();
                Riddha.util.delayExecute(function () {
                    self.SelectedTargets(result.Data.Targets);

                    self.ModeOfButton('Update');
                }, 100);
            }
        });
    };

    self.Reset = function () {
        self.Notice(new NoticeModel({ Id: self.Notice().Id() }));
        self.EnableButtons(true);
    };

    self.Delete = function (notice) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + notice.Id(), null)
            .done(function (result) {
                self.Notices.remove(notice);
                self.Reset();
                self.ModeOfButton('Create');
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };
    self.EnableButtons = ko.observable(true);
    self.Review = function (item) {
        //call get function to show the data
        self.SelectedNotice(item);
        self.Notice(new NoticeModel(ko.toJS(item)));
        self.ShowModal();
        self.EnableButtons(false);
        self.ModeOfButton("Update");
    };

    //self.Publish = function (item) {
    //    Riddha.UI.Confirm("Confirm to publish this notice?", function () {
    //        Riddha.ajax.get(url + "/Publish" + "/?id=" + item.Id())
    //       .done(function (result) {
    //           Riddha.UI.Toast(result.Message, result.Status);
    //       })
    //    });
    //};







    self.ShowModal = function () {
        $("#noticeViewModel").modal('show');
    }

    $("#noticeViewModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#noticeViewModel").modal('hide');
    }
}