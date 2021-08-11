/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="riddha.script.taxsetup.model.js" />

function TaxSetupController() {

    var self = this;
    var url = "/Api/TaxSetupApi";
    self.TaxSetupMaster = ko.observable(new TaxSetupMasterModel());
    self.TaxSlab = ko.observable(new TaxSlabDetailModel());
    self.TaxSlabDetails = ko.observableArray([]);
    self.SelectTaxSetupMaster = ko.observable();
    self.SelectedTaxSlab = ko.observable();
    self.FiscalYears = ko.observableArray([]);
    self.TaxSetupModeOfButton = ko.observable("Add");
    self.TaxSlabModeOfButton = ko.observable("Add");
    self.DeductionLimitRatioDividened = ko.observable(0);
    self.DeductionLimitRatioDivisor = ko.observable(0);
    GetFiscalYears();

    function GetFiscalYears() {
        
        Riddha.ajax.get(url + "/GetFiscalYears").done(function (result) {

            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), FiscalYearDropDownModel);
            self.FiscalYears(data);
        })
    };


    self.KendoGridOptions = {
        title: "Tax Setup",
        target: "#taxSetupKendoGrid",
        url: url + "/GetTaxSetupKendoGrid",
        height: 500,
        paramData: {},
        multiSelect: true,
        group: true,
        selectable: true,
        //groupParam: { field: "DesignationName" },
        columns: [
            { field: '#', title: lang == "ne" ? "क्र.स" : "S.N", width: 35, template: "#=SuitableNumber(++record)#", filterable: false },
            { field: 'FiscalYear', title: lang == "ne" ? "आर्थिक वर्ष" : "FiscalYear", width: 120, },
            {
                command: [
                    { name: "view", template: '<a class="k-grid-view k-button" ><span class="fa fa-eye text-green"  ></span></a>', click: View }
                ],
                title: lang == "ne" ? "कार्य" : "Action",
                width: "200px"
            }

        ],

        SelectedItem: function (item) {

            self.SelectTaxSetupMaster(item);
        },
        SelectedItems: function (items) {
        },
        //open: function (callBack) {
        //    self.GetOnDemandKendoGrid.Resignation = callBack;
        //}
    }

    self.RefreshKendoGrid = function () {
        $("#taxSetupKendoGrid").getKendoGrid().dataSource.read();
    };


    function ManageSerialNoOfTaxSlab() {

        ko.utils.arrayForEach(self.TaxSlabDetails(), function (data) {

            var indexOf = self.TaxSlabDetails().indexOf(data);
            self.TaxSlabDetails()[indexOf].SN(indexOf + 1);

        });
    }

    function View(e) {
        debugger;
        var grid = $("#taxSetupKendoGrid").getKendoGrid();
        var item = grid.dataItem($(e.target).closest("tr"));

        Riddha.ajax.get(url + "?Id=" + item.Id).done(function (result) {

            if (result.Status == 4) {

                //var DeductionLimitRatio = result.Data.TaxSetupMaster.DeductionLimitRatio;
                //if (DeductionLimitRatio != null && DeductionLimitRatio != "") {
                //    DeductionLimitRatio = DeductionLimitRatio.split("/");
                //    self.DeductionLimitRatioDividened(DeductionLimitRatio[0]);
                //    self.DeductionLimitRatioDivisor(DeductionLimitRatio[1]);
                //}
                SplitDeductionLimitRatio(result.Data.TaxSetupMaster.DeductionLimitRatio);
                self.TaxSetupMaster(new TaxSetupMasterModel(ko.toJS(result.Data.TaxSetupMaster)));
                var taxSlabDetails = Riddha.ko.global.arrayMap(result.Data.TaxSlabDetails, TaxSlabDetailModel);
                self.TaxSlabDetails(taxSlabDetails);
                $("#taxSetupCreationModelView").modal('show');
            }
        });
    };

    function GetCurrentFiscalYear() {

        var CurrentFiscalYear = ko.utils.arrayFirst(self.FiscalYears(), function (item) {
            return item.ISCurrent() == true;
        });

        return CurrentFiscalYear.Id();
        self.TaxSetupMaster().FiscalYearId(CurrentFiscalYear.Id());

    }
   
    function GetCurrentFiscalYearName(Id) {


        var FiscalYear = ko.utils.arrayFirst(self.FiscalYears(), function (item) {
            return item.Id() == Id;
        });

        return FiscalYear.Name();
    }

    // Region Tax Slab 
    self.AddTaxSlabDetails = function (model) {

        if (self.TaxSlab().TaxPerc() == 0) {

            Riddha.UI.Toast("Please enter the tax percentage", 0);
            return;
        }

        if (self.TaxSlab().IndividualAmount() == 0) {

            Riddha.UI.Toast("Please enter the Individual Amount", 0);
            return;
        }

        if (self.TaxSlab().CoupleAmount() == 0) {

            Riddha.UI.Toast("Please enter the Couple Amount", 0);
            return;
        }
        if (self.TaxSlabModeOfButton() == "Add") {

            self.TaxSlabDetails.push(model);
        }
        else {
            self.TaxSlabDetails.replace(self.SelectedTaxSlab(), self.TaxSlab());
        }

        self.ResetTaxSlabDetails();

    }

    self.ResetTaxSlabDetails = function () {

        self.TaxSlabModeOfButton("Add");
        self.TaxSlab(new TaxSlabDetailModel());
        ManageSerialNoOfTaxSlab();

    }
    self.EditTaxSlabDetails = function (model) {

        self.SelectedTaxSlab(model);
        self.TaxSlab(new TaxSlabDetailModel(ko.toJS(model)));
        self.TaxSlabModeOfButton('Update');

    }
    self.RemoveTaxSlabDetails = function (model) {
        self.TaxSlabDetails.remove(model);
        self.ResetTaxSlabDetails();
    }

    // End Region

    // Region Tax Master Setup 

    self.CreateOrUpdate = function () {

        if (self.TaxSetupMaster().FiscalYearId() == 0 || self.TaxSetupMaster().FiscalYearId() == undefined) {
            Riddha.UI.Toast("Please select fiscal year", 0);
            return;
        }
        if (self.TaxSlabDetails().length == 0) {

            Riddha.UI.Toast("Please add tax slab details", 0);
            return;
        }
        if (self.TaxSetupMaster().TaxPercAboveFinalValue() == 0) {

            Riddha.UI.Toast("Please add tax Tax Percentage Above Final Value", 0);
            return;
        }

        self.TaxSetupMaster().FiscalYear(GetCurrentFiscalYearName(self.TaxSetupMaster().FiscalYearId()));
        self.TaxSetupMaster().DeductionLimitRatio(self.DeductionLimitRatioDividened() + "/" + self.DeductionLimitRatioDivisor());

        var data = { TaxSetupMaster: ko.toJS(self.TaxSetupMaster()), TaxSlabDetails: ko.toJS(self.TaxSlabDetails()) };

        if (self.TaxSetupModeOfButton() == "Add") {
            Riddha.ajax.post(url, data).done(function (result) {

                if (result.Status == 4) {
                    self.CloseModal();
                    self.RefreshKendoGrid();

                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
        else {
            Riddha.ajax.put(url, data).done(function (result) {
                if (result.Status == 4) {
                    self.CloseModal();
                    self.RefreshKendoGrid();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    }
    self.Reset = function () {
        self.TaxSetupMaster(new TaxSetupMasterModel());
        self.TaxSlabDetails([]);
        self.ResetTaxSlabDetails();
        self.DeductionLimitRatioDividened(0);
        self.DeductionLimitRatioDivisor(0);
        self.TaxSetupModeOfButton("Add");
    }
    self.Select = function () {

        if (self.SelectTaxSetupMaster().Id == undefined) {

            Riddha.UI.Toast("Please select the row to delete", 0);
            return;
        }
        Riddha.ajax.get(url + "?Id=" + self.SelectTaxSetupMaster().Id).done(function (result) {

            if (result.Status == 4) {

                //var DeductionLimitRatio = result.Data.TaxSetupMaster.DeductionLimitRatio;
                //if (DeductionLimitRatio != null && DeductionLimitRatio != "") {
                //    DeductionLimitRatio = DeductionLimitRatio.split("/");
                //    self.DeductionLimitRatioDividened(DeductionLimitRatio[0]);
                //    self.DeductionLimitRatioDivisor(DeductionLimitRatio[1]);
                //}
                
                    SplitDeductionLimitRatio(result.Data.TaxSetupMaster.DeductionLimitRatio);
                    self.TaxSetupMaster(new TaxSetupMasterModel(ko.toJS(result.Data.TaxSetupMaster)));

                    var taxSlabDetails = Riddha.ko.global.arrayMap(result.Data.TaxSlabDetails, TaxSlabDetailModel);
                    self.TaxSlabDetails(taxSlabDetails);
                    self.TaxSetupModeOfButton("Update");
                    self.ShowModal();
                

            }
        });

    }
    self.View = function () {

        if (self.SelectTaxSetupMaster().Id == undefined) {

            Riddha.UI.Toast("Please select the row to delete", 0);
            return;
        }
        Riddha.ajax.get(url + "?Id=" + self.SelectTaxSetupMaster().Id).done(function (result) {

            if (result.Status == 4) {

                //var DeductionLimitRatio = result.Data.TaxSetupMaster.DeductionLimitRatio;
                //if (DeductionLimitRatio != null && DeductionLimitRatio != "") {
                //    DeductionLimitRatio = DeductionLimitRatio.split("/");
                //    self.DeductionLimitRatioDividened(DeductionLimitRatio[0]);
                //    self.DeductionLimitRatioDivisor(DeductionLimitRatio[1]);
                //}
                SplitDeductionLimitRatio(result.Data.TaxSetupMaster.DeductionLimitRatio);
                self.TaxSetupMaster(new TaxSetupMasterModel(ko.toJS(result.Data.TaxSetupMaster)));
                var taxSlabDetails = Riddha.ko.global.arrayMap(result.Data.TaxSlabDetails, TaxSlabDetailModel);
                self.TaxSlabDetails(taxSlabDetails);
                $("#taxSetupCreationModelView").modal('show');
            }
        });
    }
    self.Delete = function (model) {

        if (self.SelectTaxSetupMaster() == undefined || self.SelectTaxSetupMaster().Id == null) {

            Riddha.UI.Toast("Please select the row to edit", 0);
            return;
        }
        Riddha.UI.Confirm("ConfirmDelete", function () {
            Riddha.ajax.delete(url + "/" + self.SelectTaxSetupMaster().Id)
                .done(function (result) {
                    if (result.Status == 4) {
                        self.RefreshKendoGrid();
                        self.Reset();
                    }
                    Riddha.UI.Toast(result.Message, result.Status);

                });
        });

    }

    self.ShowModal = function () {
        if (self.TaxSetupModeOfButton() == "Add") {

            self.TaxSetupMaster().FiscalYearId(GetCurrentFiscalYear());
        }
        $("#taxSetupCreationModel").modal('show');
    }
    self.CloseModal = function () {

        $("#taxSetupCreationModel").modal('hide');
        self.Reset();

    }
    self.CopyFromLastFiscalYear = function () {


        Riddha.ajax.get(url + "/GetLastFiscalYearDetails" ).done(function (result) {

            if (result.Status == 4) {

                var Id = 0;
                if (self.TaxSetupModeOfButton() == "Update") {

                    Id =  self.TaxSetupMaster().Id();
                }
                SplitDeductionLimitRatio(result.Data.TaxSetupMaster.DeductionLimitRatio);
                var ChoosenFiscalYear = self.TaxSetupMaster().FiscalYearId();
                self.TaxSetupMaster(new TaxSetupMasterModel(ko.toJS(result.Data.TaxSetupMaster)));
                self.TaxSetupMaster().Id(Id);
                self.TaxSetupMaster().FiscalYearId(ChoosenFiscalYear);
                var taxSlabDetails = Riddha.ko.global.arrayMap(result.Data.TaxSlabDetails, TaxSlabDetailModel);
                self.TaxSlabDetails(taxSlabDetails);

            } else {

                Riddha.UI.Toast(result.Message, result.Status);

            }
        });
    }

    self.CheckIfFiscalYearSetupAlreadyExist = function () {


        //if (self.TaxSetupModeOfButton() == "Update") {

        //    // Check if the fiscal Year data already exist
        //    var grid = $("#taxSetupKendoGrid").getKendoGrid();
        //    grid.dataItems().forEach(function (data) {
        //        if (data.FiscalYearId == self.TaxSetupMaster().FiscalYearId())
        //            Riddha.UI.Toast("Tax Setup for this Fiscal Year already exist...", 0);
        //        return;
        //    });
        //    return;
        //}
        if (self.TaxSetupMaster().FiscalYearId() == 0 || self.TaxSetupMaster().FiscalYearId() == undefined) {
            self.Reset();
            return;
        }
        Riddha.ajax.get(url + "/CheckIfFiscalYearSetupAlreadyExist?FiscalYearId=" + self.TaxSetupMaster().FiscalYearId())
            .done(function (result) {

                if (result.Status == 4) {

                    if (result.Data.Id >  0) {
                        debugger;
                        self.SelectTaxSetupMaster(result.Data);
                        self.Select();
                    } else {

                        var FiscalYear = self.TaxSetupMaster().FiscalYearId(); 
                        self.Reset();
                        self.TaxSetupMaster().FiscalYearId(FiscalYear);

                    }

                } 
            });

    }
    // End region
    function SplitDeductionLimitRatio(DeductionLimitRatio) {

        if (DeductionLimitRatio != null && DeductionLimitRatio != "") {
            DeductionLimitRatio = DeductionLimitRatio.split("/");
            self.DeductionLimitRatioDividened(DeductionLimitRatio[0]);
            self.DeductionLimitRatioDivisor(DeductionLimitRatio[1]);
        }

    }
   
}