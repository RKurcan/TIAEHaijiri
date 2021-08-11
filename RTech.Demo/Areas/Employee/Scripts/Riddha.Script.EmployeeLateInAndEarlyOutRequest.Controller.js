/// <reference path="riddha.script.employeelateinandearlyoutrequest.model.js" />

/// <reference path="../../../webcamjs-master/webcam.js" />
/// <reference path="../../../webcamjs-master/webcam.min.js" />

function employeeLateInAndEarlyOutRequestController() {
    var self = this;
    var url = "/Api/EmployeeLateInAndEarlyOutRequestApi";
    self.EmployeeLateInAndEarlyOutRequest = ko.observable(new EmployeeLateInAndEarlyOutRequestModel());

    self.SelectedEmployeeLateInAndEarlyOutRequest = ko.observable();

    self.ModeOfButton = ko.observable('Create');
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);
    self.LateInEarlyOutRequestTypes = ko.observableArray([

        { Id: 0, Name: "Late In" },
        { Id: 1, Name: "Early Out" }

    ]);
    self.KendoGridOptions = {
        title: "EmployeeLateInAndEarlyOutRequest",
        target: "#employeeLateInAndEarlyOutRequestKendoGrid",
        url: "/Api/EmployeeLateInAndEarlyOutRequestApi/GetKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        selectable: true,
        group: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'EmployeeCode', title: lang == "ne" ? "कोड" : "Code", width: 80, filterable: true },
            { field: 'EmployeeName', title: lang == "ne" ? "नाम" : "Name", width: 180, filterable: true },
            { field: 'RequestDate', title: lang == "ne" ? "अनुरोध मिति" : "Request Date", width: 100, template: "#=SuitableDate(RequestDate)#", filterable: false },
            { field: 'Remark', title: lang == "ne" ? "कारण" : "	Reason", width: 200, filterable: false },
            { field: 'LateInEarlyOutRequestTypeName', title: lang == "ne" ? "अनुरोध प्रकार" : "Request Type", width: 95, template: "#=GetBadge(LateInEarlyOutRequestTypeName)#", filterable: false },
            { field: 'IsApproved', title: lang == "ne" ? "स्वीकृत भएको छ ?" : "Is Approved", width: 85, template: "#=GetBadge(IsApproved)#", filterable: false },
            { field: 'ApproveByName', title: lang == "ne" ? "नामबाट स्वीकृत" : "Approve By", filterable: false },
            { field: 'ApproveDate', title: lang == "ne" ? "स्वीकृत मिति" : "Approved Date", width: 100, template: "#=SuitableDate(ApproveDate)#", filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedEmployeeLateInAndEarlyOutRequest(new EmployeeLateInAndEarlyOutRequestModel(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedEmployeeLateInAndEarlyOutRequest(new EmployeeLateInAndEarlyOutRequestModel());
        $("#employeeLateInAndEarlyOutRequestKendoGrid").getKendoGrid().dataSource.read();
    };

    self.ViewDetails = function () {
        if (self.SelectedEmployeeLateInAndEarlyOutRequest() == undefined || self.SelectedEmployeeLateInAndEarlyOutRequest().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToViewDetails");
            return;
        }
        Riddha.ajax.get(url + "/Get?id=" + self.SelectedEmployeeLateInAndEarlyOutRequest().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.EmployeeLateInAndEarlyOutRequest(new EmployeeLateInAndEarlyOutRequestModel(ko.toJS(result.Data)));
                    self.ShowModal();
                }
                else {
                    Riddha.UI.Toast(result.Message, result.Status);
                }
            });
    };


    self.Approve = function () {
        if (self.SelectedEmployeeLateInAndEarlyOutRequest() == undefined || self.SelectedEmployeeLateInAndEarlyOutRequest().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToApprove");
            return;
        }
        if (self.SelectedEmployeeLateInAndEarlyOutRequest().IsApproved() == "YES") {
            Riddha.UI.Toast("Already Approved.", 0);
            return;
        }
        Riddha.ajax.get(url + "/Approve?id=" + self.SelectedEmployeeLateInAndEarlyOutRequest().Id())
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }



    self.ShowModal = function () {
        $("#employeeLateInAndEarlyOutRequestModal").modal('show');
    };

    $("#employeeLateInAndEarlyOutRequestModal").on('hidden.bs.modal', function () {
        //self.Reset();
        //self.RefreshKendoGrid();
        //self.ModeOfButton("Create");
    });

    self.CloseModal = function () {
        $("#employeeLateInAndEarlyOutRequestModal").modal('hide');
        //self.Reset();
        //self.RefreshKendoGrid();
        //self.ModeOfButton("Create");
    };


    //region Early In and Early Out Request Creation on web portal
    //Added by Ganesh 2020/12/14
    //get employee for auto complete
    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/EmployeeApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.Employee(new EmpSearchViewModel(item));
            self.EmployeeLateInAndEarlyOutRequest().EmployeeId(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    }

    self.ShowCreationModal = function () {

        $("#lateInEarlyOutRequestCreationnModal").modal('show');
    };
    self.CloseCreationModal = function () {
       
        $("#lateInEarlyOutRequestCreationnModal").modal('hide');
    };
    self.getTimeByRequestType = function () {

        if (self.EmployeeLateInAndEarlyOutRequest().RequestDate() == "NaN/aN/aN") {
            return Riddha.UI.Toast("select requested Date", 0);
        }
        if (self.EmployeeLateInAndEarlyOutRequest().EmployeeId() == undefined || self.EmployeeLateInAndEarlyOutRequest().EmployeeId() == 0) {
            return Riddha.UI.Toast("select employee", 0);
        }
        Riddha.ajax.get(url + "/GetPunchTimeforLateInAccordingtoDateTimeAndEmpId?dateTime=" + self.EmployeeLateInAndEarlyOutRequest().RequestDate() +
            "&empId=" + self.EmployeeLateInAndEarlyOutRequest().EmployeeId() + "&requestType=" + self.EmployeeLateInAndEarlyOutRequest().LateInEarlyOutRequestType())
            .done(function (result) {
                if (result.Status == 4) {

                    self.EmployeeLateInAndEarlyOutRequest().PunchInTime(result.Data.PunchTime);
                    self.EmployeeLateInAndEarlyOutRequest().PlannedInTime(result.Data.PlannedInTime);
                    self.EmployeeLateInAndEarlyOutRequest().PunchOutTime(result.Data.ActualOutTime);
                    self.EmployeeLateInAndEarlyOutRequest().PlannedOutTime(result.Data.PlannedOutTime);
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
    }

    self.CreateUpdate = function () {
        if (self.EmployeeLateInAndEarlyOutRequest().EmployeeId() == 0) {
            Riddha.UI.Toast("Please select employee.", 0);
            return;
        }
        if (self.EmployeeLateInAndEarlyOutRequest().Remark() == "") {
            Riddha.UI.Toast("Remark is Required.", 0);
            return;
        }
        if (self.EmployeeLateInAndEarlyOutRequest().RequestDate() == "" || self.EmployeeLateInAndEarlyOutRequest().RequestDate() == "NaN/aN/aN") {
            Riddha.UI.Toast("Request Date is Required.", 0);
            return;
        }
    
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.EmployeeLateInAndEarlyOutRequest()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseCreationModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.EmployeeLateInAndEarlyOutRequest()))
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.CloseCreationModal();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);
                });
        }
    };


}




function GetBadge(name) {
    if (name == 'LateIn') {
        return "<span class='badge bg-aqua'>" + name + "</span>";
    }
    else if (name == 'YES') {
        return "<span class='badge bg-green'>" + name + "</span>";
    }
    else if (name == 'NO') {
        return "<span class='badge bg-orange'>" + name + "</span>";
    }
    else {
        return "<span class='badge bg-red'>" + name + "</span>";
    }
};

