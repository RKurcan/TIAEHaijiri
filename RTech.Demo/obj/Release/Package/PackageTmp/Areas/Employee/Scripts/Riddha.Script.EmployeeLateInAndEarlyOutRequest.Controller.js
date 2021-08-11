/// <reference path="riddha.script.employeelateinandearlyoutrequest.model.js" />


function employeeLateInAndEarlyOutRequestController() {
    var self = this;
    var url = "/Api/EmployeeLateInAndEarlyOutRequestApi";
    self.EmployeeLateInAndEarlyOutRequest = ko.observable(new EmployeeLateInAndEarlyOutRequestModel());
    self.SelectedEmployeeLateInAndEarlyOutRequest = ko.observable();

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
            Riddha.UI.Toast("Already Approved.",0);
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

