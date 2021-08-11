/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="Riddha.Script.Branch.Model.js" />

function branchController() {
    var self = this;
    var url = "/Api/BranchApi";
    self.Branch = ko.observable(new BranchModel());
    self.Companies = ko.observableArray(getCompanies());
    self.Branches = ko.observableArray(getBranches());
    self.SelectedBranch = ko.observable();
    self.ModeOfButton = ko.observable('Create');

    function getCompanies() {
        Riddha.ajax.get(url + "/GetCompany", null)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), DropdownViewModel);
            self.Companies(data);
        });
    };

    function getBranches() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), BranchModel);
            self.Branches(data);
        });
    };

    self.KendoGridOptions = {
        title: "Branch",
        target: "#branchKendoGrid",
        url: "/Api/BranchApi/GetKendoGrid",
        height: 490,
        paramData: {},
        multiSelect: false,
        selectable: true,
        group: true,
        //groupParam: { field: "DepartmentName" },
        columns: [
            { field: 'CompanyName', title: lang == "ne" ? "CompanyName" : "CompanyName", width: 200, filterable: false },
            { field: 'Code', title: lang == "ne" ? "कोड" : "Code", width: 80 },
            { field: 'Name', title: "Branch Name", width: 200, hidden: lang == "en" ? false : true },
            { field: 'NameNp', title: " नाम", width: 200, hidden: lang == "ne" ? false : true, template: "#=NameNp==null?Name:NameNp#" },
            { field: 'Address', title: "Address", width: 150, hidden: lang == "en" ? false : true  ,filterable: false},
            { field: 'AddressNp', title: "Address", width: 150, hidden: lang == "ne" ? false : true, template: "#=AddressNp==null?Address:AddressNp#", filterable: false },
            { field: 'ContactNo', title: "Contact No", width: 100, hidden: lang == "en" ? false : true, filterable: false },
            { field: 'Email', title: "Email", width: 100, hidden: lang == "en" ? false : true, filterable: false },
        ],
        SelectedItem: function (item) {
            self.SelectedBranch(new BranchModel(item));
        },
        SelectedItems: function (items) {

        }
    };
    self.RefreshKendoGrid = function () {
        self.SelectedBranch(new BranchModel());
        $("#branchKendoGrid").getKendoGrid().dataSource.read();
    };

    self.CreateUpdate = function () {

        if (self.Branch().Code() == "") {
            return Riddha.util.localize.Required("Code");
        }
        if (self.Branch().Name.hasError()) {
            return Riddha.util.localize.Required("Name");
        }
        if (self.Branch().Address.hasError()) {
            return Riddha.util.localize.Required("Address");
        }
        if (self.Branch().ContactNo.hasError()) {
            return Riddha.util.localize.Required("ContactNo");
        }

        if (self.Branch().CompanyId() == undefined) {
            return Riddha.util.localize.Required("Company");
        }
        if (self.ModeOfButton() == 'Create') {
            Riddha.ajax.post(url, ko.toJS(self.Branch()))
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
            Riddha.ajax.put(url, ko.toJS(self.Branch()))
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
        self.Branch(new BranchModel({ Id: self.Branch().Id() }));
        self.ModeOfButton('Create');
    };
    self.Select = function (model) {
        if (self.SelectedBranch()==undefined) {
            Riddha.UI.Toast("Please select branch.", 0);
            return;
        }
        //self.SelectedBranch(model);
        self.Branch(new BranchModel(ko.toJS(self.SelectedBranch())));
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (branch) {
        if (self.SelectedBranch() == undefined) {
            Riddha.UI.Toast("Please select branch.", 0);
            return;
        }
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + self.SelectedBranch().Id(), null)
            .done(function (result) {
                if (result.Status == 4) {
                    self.RefreshKendoGrid();
                    //self.Branches.remove(branch);
                    self.ModeOfButton("Create");
                    self.Reset();
                }
                
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    self.ShowModal = function () {
        $("#branchCreationModel").modal('show');
    };

    $("#branchCreationModel").on('hidden.bs.modal', function () {
        self.Reset();
    });

    self.CloseModal = function () {
        $("#branchCreationModel").modal('hide');
    };
}