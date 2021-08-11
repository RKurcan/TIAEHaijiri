

function SalaryPayslipModel(item) {

    var self = this;
    item = item || {};

}

function SalaryPaySlipInfoModel(item) {


    var self = this;
    item = item || {};
    self.Name = ko.observable(item.Name || "");
    self.Amount = ko.observable(item.Amount || 0);
}

function SalaryPayslipMasterModel(item) {

    var self = this;
    item = item || {};
    self.SalaryPayableId = ko.observable(item.SalaryPayableId || 0);
    self.Month = ko.observable(item.Month || "");
    self.EmployeeName = ko.observable(item.EmployeeName || "");
    self.EmployeeCode = ko.observable(item.EmployeeCode || "");
    self.Designation = ko.observable(item.Designation || "");
    self.SalaryPaySlipInfos = ko.observableArray(item.SalaryPaySlipInfos || []);
    

}