/// <reference path="Riddha.Script.Employee.Model.js" />

function kajController() {
    var self = this;
    var url = "/Api/KajApi";
    var lang = config.CurrentLanguage;
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.Kaj = ko.observable(new KajModel());
    self.Kajs = ko.observableArray([]);
    self.KajDetails = ko.observableArray([]);
    self.SelectedKaj = ko.observable();
    self.ModeOfButton = ko.observable('Create');
    self.EmpIds = ko.observableArray([]);
    self.Employees = ko.observableArray([]);

    self.GetEmployee = function () {
        if (self.Employee().Code() != '' || self.Employee().Name() != '') {
            Riddha.ajax.get("/Api/KajApi/SearchEmployee/?empCode=" + self.Employee().Code() + "&empName=" + self.Employee().Name(), null)
                .done(function (result) {
                    self.Employee(new EmpSearchViewModel(result.Data));
                    self.Kaj().EmployeeId(result.Data.Id);
                });
        } else
            return Riddha.UI.Toast("Please Enter Employee Code Or Name To Search", 2);
    };

    self.CreateUpdate = function () {
        if (self.Kaj().From() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("From");
        }
        else if (self.Kaj().To() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("To");
        }
        if (self.Kaj().From() > self.Kaj().To()) {
            return Riddha.UI.Toast("The To Date must be greater than the From Date");
        }
        else if (self.Kaj().Remark() == "") {
            return Riddha.util.localize.Required("Remark");
        }
        else if (self.Kaj().FromTime() == 0) {
            return Riddha.util.localize.Required("Time");
        }
        else if (self.Kaj().ToTime() == 0) {
            return Riddha.util.localize.Required("Time");
        }

        var data = { Kaj: self.Kaj(), FromTime: self.Kaj().FromTime(), ToTime: self.Kaj().ToTime(), EmpIds: self.EmpIds() };
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
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        };
    }

    self.Reset = function () {
        self.Kaj(new KajModel({ Id: self.Kaj().Id() }));
        self.Employee(new EmpSearchViewModel());
        self.EmpIds([]);
        self.EmpMultiOptions.multiSelect.value([]);
    };

    self.Select = function (model) {
        if (self.SelectedKaj() == undefined || self.SelectedKaj().length > 1 || self.SelectedKaj().Id() == 0) {
            Riddha.UI.Toast("Please select row to edit..", 0);
            return;
        }
        Riddha.ajax.get(url + "?id=" + self.SelectedKaj().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.Kaj(new KajModel(result.Data.Kaj));
                    self.Kaj().FromTime(result.Data.FromTime);
                    self.Kaj().ToTime(result.Data.ToTime);
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

    self.Delete = function (kaj) {
        if (self.SelectedKaj() == undefined || self.SelectedKaj().length > 1 || self.SelectedKaj().Id() == 0) {
            Riddha.UI.Toast("Please select row to delete..", 0);
            return;
        };
        if (self.SelectedKaj().KajStatus() == 1) {
            Riddha.UI.Toast("Already approved..Cannot deleted approved data", 0);
            return;
        };
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedKaj().Id(), null)
                .done(function (result) {
                    self.Kajs.remove(kaj)
                    self.RefreshKendoGrid();
                    self.ModeOfButton("Create");
                    self.Reset();
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#kajCreationModel").modal('show');
    };

    self.CloseModal = function () {
        $("#kajCreationModel").modal('hide');
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
    };

    self.KendoGridOptions = {
        title: "Kaj",
        target: "#kajKendoGrid",
        url: "/Api/KajApi/GetKajKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        selectable: true,
        group: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'From', title: lang == "ne" ? "मिति देखी" : "From", width: 250, filterable: false, template: "#=SuitableDate(From)+'-'+FromTime#" },
            { field: 'To', title: lang == "ne" ? "मिति सम्म" : "To", filterable: false, template: "#=SuitableDate(To)+'-'+ToTime#" },
            { field: 'Remark', title: lang == "ne" ? "टिप्पणी" : "Remark", filterable: true },
            {
                field: 'KajStatus', title: lang == "ne" ? "स्थिति" : "Status", filterable: false, template: "#=getKajVisitStatusTemp(KajStatus,KajStatusName)#"
            },
        ],
        SelectedItem: function (item) {
            self.SelectedKaj(new KajGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };
    self.RefreshKendoGrid = function () {
        self.SelectedKaj(new KajGridVm());
        $("#kajKendoGrid").getKendoGrid().dataSource.read();
    }
}

function KajApprovalController() {
    var self = this;
    var url = "/Api/KajApi";
    var lang = config.CurrentLanguage;
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.Kaj = ko.observable(new KajModel());
    self.Kajs = ko.observableArray([]);
    self.KajDetails = ko.observableArray([]);
    self.SelectedKaj = ko.observable();
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

    self.IsVisible = ko.observable(true);
    self.KendoGridOptions = {
        title: "Kaj Approval",
        target: "#kajApprovalKendoGrid",
        url: "/Api/KajApi/GetKajKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        group: true,
        selectable: true,
        columns: [
            { field: '#', title: lang == "ne" ? "S.No" : "S.No", width: 80, template: "#= ++record #", filterable: false },
            { field: 'From', title: lang == "ne" ? "लागू गरिएको मिति" : "From", width: 250, template: "#=SuitableDate(From)+'-'+FromTime#", filterable: false },
            { field: 'To', title: lang == "ne" ? "छूट" : "To", template: "#=SuitableDate(To)+'-'+ToTime#", filterable: false },
            { field: 'Remark', title: lang == "ne" ? "टिप्पणी" : "Remark" },
            {
                field: 'KajStatus', title: lang == "ne" ? "स्थिति" : "Status", filterable: false, template: "#=getKajVisitStatusTemp(KajStatus,KajStatusName)#"
            },
            {
                command: [
                    { name: "view", template: '<a class="k-grid-view k-button"><span class="fa fa-eye text-green"></span></a>', click: View },
                    { name: "approve", template: '<a class="k-grid-approve k-button"><span class="fa fa-check text-blue"></span></a>', click: Approve }
                ],
                title: lang == "ne" ? "कार्य" : "Action",
                width: "180px"
            }
        ],
        SelectedItem: function (item) {
            self.SelectedKaj(new KajGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };
    self.RefreshKendoGrid = function () {
        $("#kajApprovalKendoGrid").getKendoGrid().dataSource.read();
        $("#kajApprovalKendoGrid").getKendoGrid().dataSource.filter({});
    };


    self.Approve = function (item) {
        if (self.Kaj().IsApprove()) {
            Riddha.UI.Toast("Already Approved.", 0);
            return;
        }
        Riddha.ajax.get("/Api/KajApi/Approve?id=" + self.Kaj().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.CloseModal();
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.Reject = function (item) {
        if (self.Kaj().KajStatus() == 1) {
            Riddha.UI.Toast("Already approved. Cannot Reject approved data. Approved...", 0);
            return;
        };
        Riddha.ajax.get("/Api/KajApi/Reject?id=" + self.Kaj().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.CloseModal();
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.ShowModal = function () {
        $("#kajApprovalModel").modal('show');
    };

    self.CloseModal = function () {
        $("#kajApprovalModel").modal('hide');
    };

    $("#kajApprovalKendoGrid").kendoTooltip({
        filter: ".k-grid-view",
        position: "bottom",
        content: function (e) {
            return lang == "ne" ? "विवरण हेर्नुहोस्" : "View Details";
        }
    });
    $("#kajApprovalKendoGrid").kendoTooltip({
        filter: ".k-grid-approve",
        position: "bottom",
        content: function (e) {
            return lang == "ne" ? "अनुमोदन गर्नुहोस्" : "Approve";
        }
    });

    function View(e) {
        var grid = $("#kajApprovalKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        Riddha.ajax.get(url + "?id=" + item.Id)
            .done(function (result) {
                if (result.Status == 4) {
                    self.Kaj(new KajModel(result.Data.Kaj));
                    self.Kaj().FromTime(result.Data.FromTime);
                    self.Kaj().ToTime(result.Data.ToTime);
                    self.Kaj().IsApprove(result.Data.IsApprove);
                    var data = Riddha.ko.global.arrayMap(result.Data.EmpLst, GlobalDropdownModel);
                    self.Employees(data);
                    self.EmpIds([]);
                    ko.utils.arrayMap(result.Data.EmpLst, function (item) {
                        self.EmpIds.push(item.Id);
                    });
                    self.ShowModal();
                }
            })
    }

    function Approve(e) {
        var grid = $("#kajApprovalKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        Riddha.ajax.get("/Api/KajApi/Approve?id=" + item.Id)
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }
}

//this template call from Kaj Approval KendoGrid
function getKajVisitStatusTemp(id, name) {
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