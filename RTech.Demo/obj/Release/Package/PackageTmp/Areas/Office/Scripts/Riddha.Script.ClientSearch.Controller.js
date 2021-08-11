/// <reference path="riddha.script.clientsearch.model.js" />


function clientSearchController() {
    var self = this;
    var url = "/Api/ClientSearchApi";
    self.Company = ko.observable(new ClientGridVm());
    self.CompanyReseller = ko.observable(new CompanyResellerInfoVm());
    self.SelectedCompany = ko.observable();
    self.CompanyLicenseArray = ko.observableArray([]);
    self.CompanyLicense = ko.observable(new CompanyLicenseLogVm());
    self.CompanyLoginArray = ko.observableArray([]);
    self.ClientName = ko.observable('');
    self.UserType = ko.observable(Riddha.UserType);
    self.SearchText = ko.observable('');

    self.Search = function () {
        self.RefreshKendoGrid();
    };


    self.PaymentMethods = ko.observableArray([
        { Id: 0, Name: "Credit" },
        { Id: 1, Name: "Cash" },
        { Id: 2, Name: "Bank" },
        { Id: 3, Name: "OnlineTransfer" },
    ]);

    self.KendoGridOptions = {
        title: "Client Search",
        target: "#companyKendoGrid",
        url: "/Api/ClientSearchApi/GetKendoGrid",
        height: 490,
        paramData: function () { return { SearchText: self.SearchText() } },
        multiSelect: true,
        group: true,
        columns: [
            { field: '#', title: lang == "ne" ? "SN" : "SN", width: 40, template: "#= ++record #", filterable: false },
            { field: 'Code', title: "Code", width: 100, filterable: true },
            { field: 'Name', title: "Name", width: 150, filterable: true },
            { field: 'Address', title: "Address", width: 150, filterable: false },
            { field: 'ContactNo', title: "Contact No.", width: 100, filterable: false },
            { field: 'ContactPerson', title: "Contact Person", width: 100, filterable: false },
            { field: 'SoftwarePackage', title: "Package", width: 50, filterable: false },
            { field: 'ResellerName', title: "Reseller", width: 150, filterable: false },
            {
                command: [
                    { name: "details", template: '<a class="k-grid-details k-button" ><span class="fa fa-eye text-ornage"  ></span></a>', click: ViewDetails },
                ],
                title: lang == "ne" ? "कार्य" : "Action",
                width: "100px"
            }
        ],
        SelectedItem: function (item) {
            self.SelectedCompany(new ClientGridVm(item));
        },
        SelectedItems: function (items) {
            //var data = Riddha.ko.global.arrayMap(items, DepartmentGridVm);
            //self.SelectedDepartment(data);
        }
    };

    function ViewDetails(e) {
        var grid = $("#companyKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));
        self.ClientName(item.Name);
        Riddha.ajax.get(url + "/GetClientInfo?companyId=" + item.Id)
            .done(function (result) {
                //Reseller
                self.CompanyReseller(new CompanyResellerInfoVm(result.Data.CompanyResellerInfo));
                //CompanyLicenseLog
                var licenseData = Riddha.ko.global.arrayMap(result.Data.CompanyLicenseLog, CompanyLicenseLogVm);
                self.CompanyLicenseArray(licenseData);
                //CompanyLogin
                var loginData = Riddha.ko.global.arrayMap(result.Data.CompanyLogin, CompanyLoginVm);
                self.CompanyLoginArray(loginData);
                $("#clientDetailModal").modal('show');
            });
    };

    //
    self.UpdateCompanyLicense = function (model) {
        self.CompanyLicense(ko.toJS(model));
        Riddha.ajax.put(url, ko.toJS(self.CompanyLicense()))
            .done(function (result) {
                Riddha.UI.Toast(result.Message, result.Status);
            });
    };

    self.VerifyCompanyLicense = function (model) {
        self.CompanyLicense(ko.toJS(model));
        Riddha.ajax.put(url + "/Verify", ko.toJS(self.CompanyLicense()))
            .done(function (result) {
                if (result.Status == 4) {
                    Riddha.UI.Toast("Verify Successfully.", 4);
                }
            });
    };

    self.RefreshKendoGrid = function () {
        self.SelectedCompany(new ClientGridVm());
        $("#companyKendoGrid").getKendoGrid().dataSource.read();
    }

    self.CloseModal = function () {
        $("#clientDetailModal").modal('hide');
    };

}