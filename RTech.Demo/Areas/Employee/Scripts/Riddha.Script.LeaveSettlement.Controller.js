/// <reference path="Riddha.Script.LeaveSettlement.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />

function leaveSettlementController() {
    var self = this;
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    var url = "/Api/LeaveSettlementApi";
    self.ModeOfButton = ko.observable('Create');
    self.LeaveSettlement = ko.observable(new LeaveSettlementViewModel());
    self.LeaveSettlements = ko.observableArray([]);
    self.SelectedLeaveSettlement = ko.observable();
    self.Employee = ko.observableArray([]);
    self.LeaveMaster = ko.observableArray([]);
    self.OpBal = ko.observable(0);
    self.LeaveTaken = ko.observable(0);
    self.RemLeave = ko.observable(0);
    self.SettlingLeave = ko.observable(0);
    self.ModeOfButton = ko.observable('Create');
    self.selectedLeaveMaster = ko.observable(new LeaveMasterDropdownModel());
    self.SettlingLeave = ko.computed(function () {
        var total = parseFloat(self.LeaveSettlement().Paid()) + parseFloat(self.LeaveSettlement().CarrytoNext());
        return total;
    }, self);
    //self.selectedLeaveMaster.subscribe(function (newValue) {

    //});
    self.GetLeaveInfo = function () {
        if (self.selectedLeaveMaster()) {
            self.LeaveSettlement().LeaveMasterId(self.selectedLeaveMaster().Id());
            Riddha.ajax.get(url + "/GetLeaveInfo?leaveId=" + self.selectedLeaveMaster().Id() + "&empId=" + self.LeaveSettlement().EmployeeId())
                .done(function (result) {
                    if (result.Status == 4) {
                        self.OpBal(result.Data.OpBal);
                        self.LeaveTaken(result.Data.LeaveTaken);
                        self.RemLeave(result.Data.RemLeave);
                    }
                    else {
                        Riddha.UI.Toast(result.Message, result.Status);
                        return;
                    }

                });
        }
        else {
            self.LeaveSettlement().LeaveMasterId(0);
        }
    }
    self.SettlementTypes = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "तिर्नु" : 'Paid' },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "अर्कोमा सार्नु " : 'CarrytoNext' }
    ]);

    self.GetSettlementTypeName = function (id) {
        var mapped = ko.utils.arrayFirst(self.SettlementTypes(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };

    //getEmployee();
    function getEmployee() {
        Riddha.ajax.get(url + "/GetEmployeesForDropdown")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), EmployeeDropdownModel);
                self.Employee(data);
                subscribeEmp(self.LeaveSettlement().EmployeeId());
            });
    };

    getLeaveSettlement();
    function getLeaveSettlement() {
        Riddha.ajax.get(url, null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), LeaveSettlementViewModel);
                self.LeaveSettlements(data);
            });
    };

    function subscribeEmp(Id) {
        if (Id == undefined) {
            return false;
        } else
            Riddha.ajax.get(url + "/GetDesigOrEmploymentStatusWiseLeave?empId=" + Id)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), LeaveMasterDropdownModel);
                    self.LeaveMaster(data);
                });
    };

    self.CreateUpdate = function () {
        if (self.LeaveSettlement().EmployeeId() == undefined) {
            return Riddha.util.localize.Required("Employee");
        }
        if (self.LeaveSettlement().LeaveMasterId() == undefined) {
            return Riddha.util.localize.Required("LeaveMaster");
        }

        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.LeaveSettlement()))
                .done(function (result) {
                    if (result.Status == 4) {
                        getLeaveSettlement();
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                })
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.LeaveSettlement()))
                .done(function (result) {
                    if (result.Status == 4) {
                        //self.LeaveSettlements.replace(self.SelectedLeaveSettlement(), new LeaveSettlementViewModel(ko.toJS(self.LeaveSettlement())));
                        getLeaveSettlement();
                        self.ModeOfButton("Create");
                        self.Reset();
                        self.CloseModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };

    self.Reset = function () {
        self.LeaveSettlement(new LeaveSettlementViewModel({ Id: self.LeaveSettlement().Id() }));
        self.LeaveMaster([]);
        self.OpBal(0);
        self.LeaveTaken(0);
        self.RemLeave(0);
        self.ModeOfButton('Create');
    };

    self.Select = function (model) {
        console.log(ko.toJS(model));
        self.SelectedLeaveSettlement(model);
        self.LeaveSettlement(new LeaveSettlementViewModel(ko.toJS(model)));
        subscribeEmp(model.EmployeeId());
        //self.GetLeaveInfo();
        Riddha.util.delayExecute(function () {
            ko.utils.arrayMap(self.LeaveMaster(), function (item) {
                if (item.Id() == model.LeaveMasterId()) {
                    self.selectedLeaveMaster(item);
                }
            });
            self.GetLeaveInfo();
            self.ModeOfButton('Update');
            self.ShowModal();
        }, 200);
    };

    self.Delete = function (leaveSettlement) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + leaveSettlement.Id(), null)
                .done(function (result) {
                    if (result.Status == 4) {
                        Riddha.UI.Toast(result.Message, result.Status);
                        //self.LeaveSettlements.remove(leaveSettlement);
                        getLeaveSettlement();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        })
    };

    self.ShowModal = function () {
        $("#leaveSettlementCreationModel").modal('show');
    };

    $("#leaveSettlementCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#leaveSettlementCreationModel").modal('hide');
        self.Reset();
    };
    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.LeaveSettlement().EmployeeId(item.Id);
            subscribeEmp(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    }
}