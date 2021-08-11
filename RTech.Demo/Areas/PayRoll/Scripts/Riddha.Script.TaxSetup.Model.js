

function TaxSetupMasterModel(item) {

    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.FiscalYearId = ko.observable(item.FiscalYearId || "");
    self.FiscalYear = ko.observable(item.FiscalYear || "");
    self.CreationDate = ko.observable(item.CreationDate || "");
    self.BranchId = ko.observable(item.BranchId || 0);

    //region Tax Slab for tax value above final value 
    self.TaxPercAboveFinalValue = ko.observable(item.TaxPercAboveFinalValue || 0);

    // end region
    // region Maximum Deduction Limit

    self.DeductionLimitAmount = ko.observable(item.DeductionLimitAmount || 0);
    self.DeductionLimitRatio = ko.observable(item.DeductionLimitRatio || "");
    self.RebatePercForFemaleUnmarried = ko.observable(item.RebatePercForFemaleUnmarried || 0);
    // end region
 
    //region SSF Information

    self.PFPercByEmployer = ko.observable(item.PFPercByEmployer || 0);
    self.PFPercByEmployee = ko.observable(item.PFPercByEmployee || 0);
    self.GratituityPercByEmployer = ko.observable(item.GratituityPercByEmployer || 0);
    self.SSPercByEmployer = ko.observable(item.SSPercByEmployer || 0);
    self.SSPercByEmployee = ko.observable(item.SSPercByEmployee || 0);
    self.PensionFundByEmployer = ko.observable(item.PensionFundByEmployer || 0);
    self.PensionFundByEmployee = ko.observable(item.PensionFundByEmployee || 0);

    // end region

}
function TaxSlabDetailModel(item) {

    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.SN = ko.observable(item.SN || 0);
    self.TaxPerc = ko.observable(item.TaxPerc || 0);
    self.IndividualAmount = ko.observable(item.IndividualAmount || 0);
    self.CoupleAmount = ko.observable(item.CoupleAmount || 0);

}

function FiscalYearDropDownModel(item) {

    var self = this;
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || 0);
    self.ISCurrent = ko.observable(item.ISCurrent || false);


}