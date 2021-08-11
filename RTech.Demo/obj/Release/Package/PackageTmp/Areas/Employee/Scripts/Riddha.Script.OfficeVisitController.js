/// <reference path="Riddha.Script.Employee.Model.js" />

function officeVisitController() {
    var self = this;
    var url = "/Api/OfficeVisitApi";
    var lang = config.CurrentLanguage;
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.OfficeVisit = ko.observable(new OfficeVisitModel());
    self.OfficeVisits = ko.observableArray([]);
    self.OfficeVisitDetails = ko.observableArray([]);
    self.SelectedOfficeVisit = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.EmpIds = ko.observableArray([]);
    // GetOfficeVisites();
    self.Employees = ko.observableArray([]);
    self.GetEmployee = function () {
        if (self.Employee().Code() != '' || self.Employee().Name() != '') {
            Riddha.ajax.get("/Api/OfficeVisitApi/SearchEmployee/?empCode=" + self.Employee().Code() + "&empName=" + self.Employee().Name(), null)
           .done(function (result) {
               self.Employee(new EmpSearchViewModel(result.Data));
               self.OfficeVisit().EmployeeId(result.Data.Id);
           });
        } else
            return Riddha.UI.Toast("Please Enter Employee Code Or Name To Search", 2);
    }
    function GetOfficeVisites() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), OfficeVisitGridVm);
            self.OfficeVisits(data);
        });
    };

    self.CreateUpdate = function () {
        if (self.OfficeVisit().From() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("From");
        }
        else if (self.OfficeVisit().To() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("To");
        }
        if (self.OfficeVisit().From() > self.OfficeVisit().To()) {
            return Riddha.UI.Toast("The To Date must be greater than the From Date");
        }
        else if (self.OfficeVisit().Remark() == "") {
            return Riddha.util.localize.Required("Remark");
        }
        else if (self.OfficeVisit().FromTime() == 0) {
            return Riddha.util.localize.Required("Time");
        }
        else if (self.OfficeVisit().ToTime() == 0) {
            return Riddha.util.localize.Required("Time");
        }

        var data = { OfficeVisit: self.OfficeVisit(), FromTime: self.OfficeVisit().FromTime(), ToTime: self.OfficeVisit().ToTime(), EmpIds: self.EmpIds() };
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(data))
            .done(function (result) {
                if (result.Status == 4) {
                    //GetOfficeVisites();
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
                                //GetOfficeVisites();
                                self.RefreshKendoGrid();
                                self.ModeOfButton("Create");
                                self.CloseModal();
                            }
                            Riddha.UI.Toast(result.Message, result.Status);
                        });

        };
    }
    self.Reset = function () {
        self.OfficeVisit(new OfficeVisitModel({ Id: self.OfficeVisit().Id() }));
        self.Employee(new EmpSearchViewModel());
        self.EmpIds([]);
        self.EmpMultiOptions.multiSelect.value([]);
    };


    self.Select = function (model) {
        if (self.SelectedOfficeVisit() == undefined || self.SelectedOfficeVisit().length > 1 || self.SelectedOfficeVisit().Id() == 0) {
            Riddha.UI.Toast("Please select row to edit..", 0);
            return;
        }
        Riddha.ajax.get(url + "?id=" + self.SelectedOfficeVisit().Id())
        .done(function (result) {
            if (result.Status == 4) {
                self.OfficeVisit(new OfficeVisitModel(result.Data.OfficeVisit));
                self.OfficeVisit().FromTime(result.Data.FromTime);
                self.OfficeVisit().ToTime(result.Data.ToTime);
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

    self.Delete = function (officeVisit) {
        if (self.SelectedOfficeVisit() == undefined || self.SelectedOfficeVisit().length > 1 || self.SelectedOfficeVisit().Id() == 0) {
            Riddha.UI.Toast("Please select row to delete..", 0);
            return;
        }
        if (self.SelectedOfficeVisit().OfficeVisitStatus() == 1) {
            Riddha.UI.Toast("Already approved..Cannot deleted approved data", 0);
            return;
        };
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedOfficeVisit().Id(), null)
            .done(function (result) {
                self.OfficeVisits.remove(officeVisit)
                self.RefreshKendoGrid();
                self.ModeOfButton("Create");
                self.Reset();
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    self.ShowModal = function () {
        $("#officeVisiteCreationModel").modal('show');
    };

    self.CloseModal = function () {
        $("#officeVisiteCreationModel").modal('hide');
        self.ModeOfButton("Create");
        self.Reset();
    };
    self.EmpMultiOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
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

    //Kendo Grid
    self.KendoGridOptions = {
        title: "Office Visit",
        target: "#officeVisitKendoGrid",
        url: "/Api/OfficeVisitApi/GetOfficeVisitKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        selectable: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'From', title: lang == "ne" ? "मिति देखी" : "From", width: 250, filterable: false, template: "#=SuitableDate(From)+'-'+FromTime#" },
            { field: 'To', title: lang == "ne" ? "मिति सम्म" : "To", filterable: false, template: "#=SuitableDate(To)+'-'+ToTime#" },
            { field: 'Remark', title: lang == "ne" ? "टिप्पणी" : "Remark", filterable: false },
            {
                field: 'OfficeVisitStatus', title: lang == "ne" ? "स्थिति" : "Status", filterable: false, template: "#=getOfficeVisitStatusTemp(OfficeVisitStatus,OfficeVisitStatusName)#"
            },
        ],
        SelectedItem: function (item) {
            self.SelectedOfficeVisit(new OfficeVisitGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };
    self.RefreshKendoGrid = function () {
        self.SelectedOfficeVisit(new OfficeVisitGridVm());
        $("#officeVisitKendoGrid").getKendoGrid().dataSource.read();
    }
}

function OfficeVisitApprovalController() {
    var self = this;
    var url = "/Api/OfficeVisitApprovalApi";
    var officeVisitUrl = "/Api/OfficeVisitApi";
    var lang = config.CurrentLanguage;
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.OfficeVisit = ko.observable(new OfficeVisitModel());
    self.OfficeVisits = ko.observableArray([]);
    self.OfficeVisitDetails = ko.observableArray([]);
    self.SelectedOfficeVisit = ko.observable();
    self.EmpIds = ko.observableArray([]);
    self.Employees = ko.observableArray([]);

    self.EmpMultiOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
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


    self.KendoGridOptions = {
        title: "Office Visit Approval",
        target: "#officeVisitApprovalKendoGrid",
        url: "/Api/OfficeVisitApprovalApi/GetOfficeVisitApprovalKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        selectable: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'From', title: lang == "ne" ? "लागू गरिएको मिति" : "From", width: 250, template: "#=SuitableDate(From)+'-'+FromTime#", filterable: false },
            { field: 'To', title: lang == "ne" ? "छूट" : "To", template: "#=SuitableDate(To)+'-'+ToTime#", filterable: false },
            { field: 'Remark', title: lang == "ne" ? "टिप्पणी" : "Remark" },
            {
                field: 'OfficeVisitStatus', title: lang == "ne" ? "स्थिति" : "Status", filterable: false, template: "#=getOfficeVisitStatusTemp(OfficeVisitStatus,OfficeVisitStatusName)#"
            },
        ],
        SelectedItem: function (item) {
            self.SelectedOfficeVisit(new OfficeVisitApprovalGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        //self.SelectedOfficeVisit().Id(0);
        $("#officeVisitApprovalKendoGrid").getKendoGrid().dataSource.read();
        $("#officeVisitApprovalKendoGrid").getKendoGrid().dataSource.filter({});
    }

    self.View = function (model) {
        if (self.SelectedOfficeVisit() == undefined || self.SelectedOfficeVisit().length > 1 || self.SelectedOfficeVisit().Id() == 0) {
            Riddha.UI.Toast("Please select row to view..", 0);
            return;
        }
        Riddha.ajax.get(officeVisitUrl + "?id=" + self.SelectedOfficeVisit().Id())
        .done(function (result) {
            if (result.Status == 4) {
                self.OfficeVisit(new OfficeVisitModel(result.Data.OfficeVisit));
                self.OfficeVisit().FromTime(result.Data.FromTime);
                self.OfficeVisit().ToTime(result.Data.ToTime);
                self.OfficeVisit().IsApprove(result.Data.IsApprove);
                var data = Riddha.ko.global.arrayMap(result.Data.EmpLst, GlobalDropdownModel);
                self.Employees(data);
                self.EmpIds([]);
                ko.utils.arrayMap(result.Data.EmpLst, function (item) {
                    self.EmpIds.push(item.Id);
                });
                self.ShowModal();
            }
        })

    };

    self.Approve = function (item) {
        if (self.SelectedOfficeVisit().OfficeVisitStatus() == 1) {
            Riddha.UI.Toast("Already Approved...", 0);
            return;
        };
        Riddha.ajax.get("/Api/OfficeVisitApprovalApi/Approve?id=" + self.SelectedOfficeVisit().Id())
             .done(function (result) {
                 if (result.Status == 4) {
                     self.CloseModal();
                     self.RefreshKendoGrid();
                 }
                 Riddha.UI.Toast(result.Message, result.Status);
             });
    };

    self.Reject = function (item) {
        if (self.SelectedOfficeVisit().OfficeVisitStatus() == 1) {
            Riddha.UI.Toast("Already approved. Cannot Reject approved data. Approved...", 0);
            return;
        };
        Riddha.ajax.get("/Api/OfficeVisitApprovalApi/Reject?id=" + self.SelectedOfficeVisit().Id())
             .done(function (result) {
                 if (result.Status == 4) {
                     self.CloseModal();
                     self.RefreshKendoGrid();
                 }
                 Riddha.UI.Toast(result.Message, result.Status);
             });
    };

    self.ShowModal = function () {
        $("#officeVisiteApprovalModel").modal('show');
    };

    self.CloseModal = function () {
        $("#officeVisiteApprovalModel").modal('hide');
    };


}
//this template call from OfficeVisit Approval KendoGrid
function getOfficeVisitStatusTemp(id, name) {
    if (id == 2) {
        return "<span class='badge bg-red'>" + name + "</span>";
    }
    else if (id == 1) {
        return "<span class='badge bg-green'>" + name + "</span>";
    }
    else {
        return "<span class='badge bg-orange'>" + name + "</span>";
    }
};

function eventController() {
    var self = this;
    var url = "/Api/EventApi";
    self.Event = ko.observable(new EventModel());
    self.Events = ko.observableArray([]);
    self.SelectedEvent = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.Departments = ko.observableArray([]);
    self.SelectedTargets = ko.observableArray([]);
    self.Targets = ko.observableArray([]);
    self.Branches = ko.observableArray([]);
    self.NoOfDays = ko.observable(0);
    self.GetTargets = function () {
        self.Targets([]);
        self.SelectedTargets([]);
        if (self.Event().EventLevel() == 1) {
            getBranches();
        }
        else if (self.Event().EventLevel() == 2) {
            getDepartments();
        }
        else if (self.Event().EventLevel() == 3) {
            getSections();
        }
    }
    //getDepartments();
    function getDepartments() {
        Riddha.ajax.get(url + "/GetDepartments", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
            self.Targets(data);
        });
    };
    function getBranches() {
        Riddha.ajax.get(url + "/GetBranches", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
            self.Targets(data);
        });
    };
    function getEmployees() {
        Riddha.ajax.get("/Api/EmployeeApi", null)
        .done(function (result) {
            var datas = ko.utils.arrayMap(result.Data, function (item) {
                return new checkBoxModel({ Id: item.Id, Name: item.EmployeeName })
            });
            self.Targets(datas);
        });
    };
    function getSections() {
        Riddha.ajax.get(url + "/GetSections", null)
        .done(function (result) {
            var datas = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
            self.Targets(datas);
        });
    };

    self.EventLevels = ko.observableArray([
    { Id: 0, Name: config.CurrentLanguage == 'ne' ? "सबै" : "All" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "शाखा" : "Branch" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "विभाग" : "Department" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "फाँट" : "Section" },
    ]);

    Riddha.util.delayExecute(function () {
        GetEventGridLst();
    }, 500);


    //GetEventGridLst();
    //function GetEventGridLst() {
    //    Riddha.ajax.get(url)
    //    .done(function (result) {
    //        var datas = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EventModel);
    //        self.Events(datas);
    //    });
    //};

    function GetEventGridLst() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var datas = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EventGridVm);
            self.Events(datas);
        });
    };

    self.CreateUpdate = function (item) {
        if (self.Event().From() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("From");
        }
        if (self.Event().To() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("To");
        }
        if (self.Event().From() > self.Event().To()) {
            return Riddha.UI.Toast("The To Date must be greater than the From Date");
        }
        if (self.Event().Title.hasError()) {
            return Riddha.util.localize.Required("Title ");
        }
        if (self.Event().Description() == '') {
            return Riddha.util.localize.Required("Description");
        }

        if (self.SelectedTargets().length <= 0 && self.Event().EventLevel() != 0) {
            return Riddha.util.localize.Required("Target");

        }
        if (self.ModeOfButton() == 'Create') {
            var data = { Event: self.Event(), Targets: self.SelectedTargets() };
            Riddha.ajax.post(url, ko.toJS(data))
            .done(function (result) {
                self.Events.push(new EventModel(result.Data));
                self.CloseModal();
                GetEventGridLst();
            });
            Riddha.UI.Toast(result.Message, result.Status);

        }

        else if (self.ModeOfButton() == 'Update') {
            var data = { Event: self.Event(), Targets: self.SelectedTargets() };
            Riddha.ajax.put(url, ko.toJS(data))
          .done(function (result) {
              if (result.Status == 4) {
                  self.Events.replace(self.SelectedEvent(), new EventModel(ko.toJS(item)));
                  self.CloseModal();
                  GetEventGridLst();
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
                self.SelectedEvent(result.Data.Event);
                self.Event(new EventModel(result.Data.Event));
                self.GetTargets();
                Riddha.util.delayExecute(function () {
                    self.SelectedTargets(result.Data.Targets);
                    self.GetNoOfDays();
                    self.ShowModal();
                    self.ModeOfButton('Update');
                }, 100);
            }
        });
    };

    self.GeEventLevelName = function (id) {
        var mapped = ko.utils.arrayFirst(self.EventLevels(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    self.Reset = function () {
        self.Event(new EventModel({ Id: self.Event().Id() }));
        self.EnableButtons(true);
        self.NoOfDays(0);
    };

    self.Delete = function (event) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + event.Id(), null)
            .done(function (result) {
                self.Events.remove(event);
                self.Reset();
                self.ModeOfButton('Create');
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };
    self.EnableButtons = ko.observable(true);

    self.GetNoOfDays = function () {
        if (self.Event().From() == "NaN/aN/aN" || self.Event().To() == "NaN/aN/aN") {
            return false;
        }
        if (self.Event().From() == self.Event().To()) {
            self.NoOfDays(1);
            return false;
        }
        var noOfDays = getNoOfDays(self.Event().From(), self.Event().To());
        self.NoOfDays(noOfDays);
    }
    function getNoOfDays(fromDate, toDate) {
        if (fromDate == toDate) {
            return 1;
        }
        else {
            var date1 = new Date(fromDate);
            var date2 = new Date(toDate);
            var timeDiff = date2.getTime() - date1.getTime();
            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
            if (diffDays < 0) {
                self.Event().To("NaN/aN/aN");
                return 0;
            }
            else {

                return diffDays + 1;
            }
        }

    }

    self.ShowModal = function () {
        $("#eventViewModel").modal('show');
    }

    $("#eventViewModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#eventViewModel").modal('hide');
    }
}
