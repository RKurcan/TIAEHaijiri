/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="Riddha.Script.FOC.Model.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
function focController() {
    var self = this;
    var config = new Riddha.config();
    var cudate = config.CurDate;
    var url = "/Api/FOCTicketApi";
    self.ModeOfButton = ko.observable('Create');
    self.FOCTicket = ko.observable(new FocTicketModel());
    self.FOCTickets = ko.observableArray([]);
    self.FOCTicketDetail = ko.observable(new FocTicketDetailModel());
    self.FOCTicketDetails = ko.observableArray([]);
    self.Lang = ko.observable(config.CurrentLanguage);
    self.Employee = ko.observable(new EmpSearchViewModel());
    self.EmployeeId = ko.observable(self.Employee().Id || 0);
    self.RecommendedBy = ko.observable();
    self.ApprovedBy = ko.observable();
    self.SelectedFOCTicket = ko.observable();
    self.SelectedFOCTicketApproval = ko.observable();
    self.FOCTicket().AppliedDate(cudate);

    self.EmpAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/FOCTicketApi/GetEmpLstForAutoComplete",
        select: function (item) {
            self.Employee(new EmpSearchViewModel(item));
            self.FOCTicket().EmployeeId(item.Id);
            if (self.FOCTicketDetail().Relation() == 0) {
                self.FOCTicketDetail().Name(item.EmployeeName);
            }
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Select Employee"
    };
    self.FOCTicketDetail().Relation.subscribe(function (newValue) {
        if (newValue == 0) {
            self.FOCTicketDetail().Name(self.Employee().EmployeeName());
        } else {
            self.FOCTicketDetail().Name('');
        }
    })
    getRecommendedBy();
    getApprovedBy();
    function getRecommendedBy() {
        Riddha.ajax.get(url + "/GetRecommendedByForDropdown", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
            self.RecommendedBy(data);
        });
    }

    function getApprovedBy() {
        Riddha.ajax.get(url + "/GetRecommendedByForDropdown", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), GlobalDropdownModel);
            self.ApprovedBy(data);
        });
    }

    self.Relation = ko.observableArray([
        { Id: 0, Name: config.CurrentLanguage == 'ne' ? "आत्म" : "Self" },
        { Id: 1, Name: config.CurrentLanguage == 'ne' ? "पति" : "Spouse" },
        { Id: 2, Name: config.CurrentLanguage == 'ne' ? "बच्चाहरु" : "Children" },
        { Id: 3, Name: config.CurrentLanguage == 'ne' ? "अभिभावक" : "Parent" },
        { Id: 4, Name: config.CurrentLanguage == 'ne' ? "ग्रन्ड पयारेन्ट" : "GrandParent" },
        { Id: 5, Name: config.CurrentLanguage == 'ne' ? "इन ल" : "InLaw" },
    ]);

    self.GetRelationDayName = function (id) {
        var mapped = ko.utils.arrayFirst(self.Relation(), function (data) {
            return data.Id == id();
        });
        return mapped = (mapped || { Name: '' }).Name;
    };


    //Detais Crud 
    self.FOCTicketDetailsModeOfButton = ko.observable('Add');

    self.AddFOCTicketDetails = function (item) {
        if (self.FOCTicketDetail().Name().trim() == "") {
            Riddha.util.localize.Required("Name");
            return;
        }
        if (self.FOCTicketDetailsModeOfButton() == 'Add') {
            if (self.FOCTicketDetail().Name() == "") {
                Riddha.UI.Toast("Please enter name..", 0);
                return;
            }
            self.FOCTicketDetails.push(new FocTicketDetailModel(ko.toJS(item)));
            self.ResetFOCTicketDetails();
        }
        else {

        }
        self.FOCTicketDetail(new FocTicketDetailModel());
        self.FOCTicketDetailsModeOfButton('Add');
    };

    self.SelectFOCTicketDetails = function (model) {
        self.FOCTicketDetail(model);
        self.FOCTicketDetailsModeOfButton('Update');
    };

    self.ResetFOCTicketDetails = function () {
        self.FOCTicketDetail(new FocTicketDetailModel({ Id: self.FOCTicketDetail().Id() }));
        self.FOCTicketDetailsModeOfButton = ko.observable('Add');
    };

    self.DeleteFOCTicketDetails = function (model) {
        self.FOCTicketDetails.remove(model);
    };

    //FOC Master Crud
    self.CreateUpdate = function () {
        if (self.FOCTicket().EmployeeId() == 0 || self.FOCTicket().EmployeeId() == undefined) {
            Riddha.util.localize.Required("Employee");
            return;
        }
        if (self.FOCTicket().AppliedDate() == "NaN/aN/aN" || self.FOCTicket().AppliedDate() == undefined) {
            Riddha.util.localize.Required("AppliedDate");
            return;
        }
        if (self.FOCTicket().SectorAFrom() == "") {
            Riddha.util.localize.Required("Sector (A) From");
            return;
        }
        if (self.FOCTicket().SectorATo() == "") {
            Riddha.util.localize.Required("Sector (A) To");
            return;
        }

        if (self.FOCTicket().SectorADateOfFlight() == "NaN/aN/aN") {
            Riddha.util.localize.Required("Sector (A) Date of Flight");
            return;
        }
        if (self.FOCTicket().RequestType() == "1") {
            if (self.FOCTicket().SectorBFrom() == "") {
                Riddha.util.localize.Required("Sector (B) From");
                return;
            }
            if (self.FOCTicket().SectorBTo() == "") {
                Riddha.util.localize.Required("Sector (B) To");
                return;
            }

            if (self.FOCTicket().SectorBDateOfFlight() == "NaN/aN/aN") {
                Riddha.util.localize.Required("Sector (B) Date of Flight");
                return;
            }
        }

        if (self.FOCTicket().RecommendedBy() == undefined || self.FOCTicket().RecommendedBy() == 0) {
            Riddha.util.localize.Required("RecommendedBy");
            return;
        }
        if (self.FOCTicket().ApprovedById() == undefined || self.FOCTicket().ApprovedById() == 0) {
            Riddha.util.localize.Required("ApprovedBy");
            return;
        }

        if (self.ModeOfButton() == 'Create') {
            var data = { FOCTicket: self.FOCTicket(), FOCTicketDetail: self.FOCTicketDetails() };
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
            var data = { FOCTicket: self.FOCTicket(), FOCTicketDetail: self.FOCTicketDetails() };
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
        self.FOCTicket(new FocTicketModel({ Id: self.FOCTicket().Id() }));
        self.FOCTicketDetails([]);
        self.Employee(new EmpSearchViewModel());
        self.ModeOfButton('Create');
    };

    self.Select = function (focTicket) {
        if (self.SelectedFOCTicket() == undefined || self.SelectedFOCTicket().length > 1 || self.SelectedFOCTicket().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToEdit");
            return;
        }
        self.Employee(new EmpSearchViewModel(ko.toJS(self.SelectedFOCTicket())));
        Riddha.ajax.get(url + "/GetFOCTicket/" + self.SelectedFOCTicket().Id(), null)
        .done(function (result) {
            self.FOCTicket(new FocTicketModel(result.Data.FOCTicket));
            self.FOCTicket().RequestType(result.Data.FOCTicket.RequestType.toString());
            var detailData = Riddha.ko.global.arrayMap(result.Data.FOCTicketDetail, FocTicketDetailModel);
            self.FOCTicketDetails(detailData);
            self.ModeOfButton("Update");
            self.ShowModal();
            self.SelectedFOCTicket(focTicket);
        });
    };

    self.Delete = function (focTicket) {
        if (self.SelectedFOCTicket() == undefined || self.SelectedFOCTicket().length > 1 || self.SelectedFOCTicket().Id() == 0) {
            Riddha.util.localize.Required("PleaseSelectRowToDelete");
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedFOCTicket().Id(), null)
            .done(function (result) {
                self.RefreshKendoGrid();
                self.ModeOfButton("Create");
                self.Reset();
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    //Kendo Grid 
    self.KendoGridOptions = {
        title: "FOCTicket",
        target: "#focKendoGrid",
        url: "/Api/FOCTicketApi/GetFOCKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        selectable: true,
        group: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Name', title: lang == "ne" ? "कर्मचारी" : "Employee", width: 250 },
            { field: 'Department', title: lang == "ne" ? "विभाग" : "Department", width: 200 },
            { field: 'AppliedDate', title: lang == "ne" ? "लागू गरिएको मिति" : "AppliedDate", filterable: false },
            { field: 'Rebate', title: lang == "ne" ? "छूट" : "Rebate", filterable: false },
            { field: 'RequestType', title: lang == "ne" ? "अनुरोधको प्रकार" : "RequestType", filterable: false },
            { field: 'RecommendedByName', title: lang == "ne" ? "सिफारिस गर्ने" : "Recommended By", filterable: false },
            { field: 'ApprovedByName', title: lang == "ne" ? "अनुमोदन गर्ने" : "Approved By", filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedFOCTicket(new FocGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };

    self.RefreshKendoGrid = function () {
        self.SelectedFOCTicket(new FocTicketModel());
        $("#focKendoGrid").getKendoGrid().dataSource.read();
    }

    self.ShowModal = function () {
        $("#focCreationModal").modal('show');
    };

    self.ShowFocDetailModal = function () {
        $("#focViewDetailModal").modal('show');
    };

    $("#focCreationModal").on('hidden.bs.modal', function () {
        self.Reset();
        self.FOCTicketDetail().Name('');
    });

    self.CloseModal = function () {
        $("#focCreationModal").modal('hide');
        self.FOCTicketDetail().Name('');
        self.Reset();
    };


    //Approval
    //Foc Approval Kendo Grid 
    self.KendoGridOptionsForFocApproval = {
        title: "FOCTicketApproval",
        target: "#focApprovalKendoGrid",
        url: "/Api/FOCTicketApi/GetFOCApprovalKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: true,
        group: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: 'Name', title: lang == "ne" ? "कर्मचारी" : "Employee", width: 200 },
            { field: 'Department', title: lang == "ne" ? "विभाग" : "Department", width: 180 },
            { field: 'AppliedDate', title: lang == "ne" ? "लागू गरिएको मिति" : "AppliedDate", filterable: false },
            { field: 'RequestType', title: lang == "ne" ? "अनुरोधको प्रकार" : "RequestType", filterable: false },
            { field: 'RecommendedByName', title: lang == "ne" ? "सिफारिस गर्ने" : "Recommended By", filterable: false },
            { field: 'ApprovedByName', title: lang == "ne" ? "मज्जुरी गर्ने" : "Approved By", filterable: false },
            //{ field: 'IsApproved', title: lang == "ne" ? "मज्जुरी" : "IsApproved", filterable: false, template: "#=IsApproved==true?'Yes':'No'#" },
            {
                field: 'IsApproved', title: lang == "ne" ? "मज्जुरी" : "IsApproved", template: "#=getFOCApprovalStatusTemp(IsApproved)#"
            },
            { field: 'ApprovedOn', title: lang == "ne" ? "मज्जुरी गरिएको मिति" : "Approved On", filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedFOCTicketApproval(new FocGridVm(item));
        },
        SelectedItems: function (items) {

        }
    };
    self.RefreshFocApprovalKendoGrid = function () {
        $("#focApprovalKendoGrid").getKendoGrid().dataSource.read();
    }

    self.FocApprovalView = function (focTicket) {
        if (self.SelectedFOCTicketApproval() == undefined || self.SelectedFOCTicketApproval().length > 1) {
            Riddha.UI.Toast("Please select row to view details..", 0);
            return;
        }
        self.Employee(new EmpSearchViewModel(ko.toJS(self.SelectedFOCTicketApproval())));
        Riddha.ajax.get(url + "/GetFOCTicket/" + self.SelectedFOCTicketApproval().Id(), null)
        .done(function (result) {
            self.FOCTicket(new FocTicketModel(result.Data.FOCTicket));
            self.FOCTicket().RequestType(result.Data.FOCTicket.RequestType.toString());
            var detailData = Riddha.ko.global.arrayMap(result.Data.FOCTicketDetail, FocTicketDetailModel);
            self.FOCTicketDetails(detailData);
            self.ShowFocDetailModal();
        });
    };

    self.Approve = function (item) {
        if (self.SelectedFOCTicketApproval() == undefined || self.SelectedFOCTicketApproval().length > 1) {
            Riddha.UI.Toast("Please select row to Approve..", 0);
            return;
        }
        if (self.SelectedFOCTicketApproval().IsApproved() == true) {
            Riddha.UI.Toast("Already Approved..", 0);
            return;
        };
        self.FOCTicket().Id(self.SelectedFOCTicketApproval().Id());
        Riddha.ajax.get("/Api/FOCTicketApi/Approve?id=" + self.SelectedFOCTicketApproval().Id())
             .done(function (result) {
                 if (result.Status == 4) {
                     self.CloseFocDetailModal();
                     self.RefreshFocApprovalKendoGrid();
                 }
                 Riddha.UI.Toast(result.Message, result.Status);
             });
    };

    self.Revert = function (item) {
        if (self.SelectedFOCTicketApproval() == undefined || self.SelectedFOCTicketApproval().length > 1) {
            Riddha.UI.Toast("Please select row to Revert..", 0);
            return;
        }
        if (self.SelectedFOCTicketApproval().IsApproved() == false) {
            Riddha.UI.Toast("This travel ticket is not approved cannot be reverted..", 0);
            return;
        };
        self.FOCTicket().Id(self.SelectedFOCTicketApproval().Id());
        Riddha.ajax.get("/Api/FOCTicketApi/Revert?id=" + self.SelectedFOCTicketApproval().Id())
             .done(function (result) {
                 if (result.Status == 4) {
                     self.CloseFocDetailModal();
                     self.RefreshFocApprovalKendoGrid();
                 }
                 Riddha.UI.Toast(result.Message, result.Status);
             });
    };


    self.ShowFocDetailModal = function () {
        $("#focViewDetailModal").modal('show');
    };

    $("#focViewDetailModal").on('hidden.bs.modal', function () {
    });

    self.CloseFocDetailModal = function () {
        $("#focViewDetailModal").modal('hide');
    };


}


// This function called from Foc ticket Approval Kendo grid 
function getFOCApprovalStatusTemp(status) {
    if (status == true) {
        return "<span class='badge bg-green'>Yes</span>";
    }
    else {
        return "<span class='badge bg-orange'>No</span>";
    }
};


