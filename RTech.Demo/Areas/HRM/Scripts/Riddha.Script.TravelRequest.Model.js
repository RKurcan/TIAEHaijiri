/// <reference path="Riddha.Script.TravelRequest.Controller.js" />
function TravelRequestModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Currency = ko.observable(item.Currency || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.Purpose = ko.observable(item.Purpose || '');
    self.SumOfEstimateExpense = ko.observable(item.SumOfEstimateExpense || 0);
    self.ApplyForCashAdvance = ko.observable(item.ApplyForCashAdvance || false);
    self.AdvanceAmount = ko.observable(item.AdvanceAmount || 0);
    self.Comment = ko.observable(item.Comment || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.DesignationName = ko.observable(item.DesignationName || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
}

function EmployeeSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Designation = ko.observable(item.Designation || '');
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
}

function TravelInformationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.MainDestination = ko.observable(item.MainDestination || '');
    self.TravelPeriodFrom = ko.observable(item.TravelPeriodFrom || '').extend({ date: '' });
    self.TravelPeriodTo = ko.observable(item.TravelPeriodTo || '').extend({ date: '' });
    self.DepartureTime = ko.observable(item.DepartureTime || '');
    self.DestinationAddress = ko.observable(item.DestinationAddress || '');
    self.BranchId = ko.observable(item.BranchId || 0);
    self.TravelRequestId = ko.observable(item.TravelRequestId || 0);
}

function TravelEstimateModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ExpenseType = ko.observable(item.ExpenseType || 0);
    self.Remark = ko.observable(item.Remark || '');
    self.CurrencyPaidIn = ko.observable(item.CurrencyPaidIn || 0);
    self.Amount = ko.observable(item.Amount || 0);
    self.PaidBy = ko.observable(item.PaidBy || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.TravelRequestId = ko.observable(item.TravelRequestId || 0);
}