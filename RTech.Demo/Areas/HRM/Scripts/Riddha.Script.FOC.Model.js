/// <reference path="Riddha.Script.FOC.Controller.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />
function FocTicketModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.AppliedDate = ko.observable(item.AppliedDate || '').extend({ date: '' });
    self.RequestType = ko.observable(item.RequestType || '1');
    self.SectorAFrom = ko.observable(item.SectorAFrom || '');
    self.SectorATo = ko.observable(item.SectorATo || '');
    self.SectorBFrom = ko.observable(item.SectorBFrom || '');
    self.SectorBTo = ko.observable(item.SectorBTo || '');
    self.SectorADateOfFlight = ko.observable(item.SectorADateOfFlight || '').extend({ date: '' });
    self.SectorBDateOfFlight = ko.observable(item.SectorBDateOfFlight || '').extend({ date: '' });
    self.SectorAFlightNo = ko.observable(item.SectorAFlightNo || '');
    self.SectorBFlightNo = ko.observable(item.SectorBFlightNo || '');
    self.Rebate = ko.observable(item.Rebate || 0).extend({ Percent: '' });
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '').extend({ date: '' });
    self.RecommendedBy = ko.observable(item.RecommendedBy || 0);
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
    self.IsApproved = ko.observable(item.IsApproved || false);
    self.BranchId = ko.observable(item.BranchId || 0);
}

function FocTicketDetailModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.FOCTicketId = ko.observable(item.FOCTicketId || 0);
    self.Name = ko.observable(item.Name || '');
    self.Relation = ko.observable(item.Relation || 0);
}
function EmpSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.Designation = ko.observable(item.Designation || '');
    self.DesignationId = ko.observable(item.DesignationId || 0);
    self.Department = ko.observable(item.Department || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
}

function FocGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.Name = ko.observable(item.Name || '');
    self.Department = ko.observable(item.Department || '');
    self.AppliedDate = ko.observable(item.AppliedDate || '').extend({ date: '' });
    self.Rebate = ko.observable(item.Rebate || 0);
    self.RequestType = ko.observable(item.RequestType || '');
    self.RecommendedBy = ko.observable(item.RecommendedBy || 0);
    self.RecommendedByName = ko.observable(item.RecommendedByName || '');
    self.ApprovedById = ko.observable(item.ApprovedById || 0);
    self.ApprovedByName = ko.observable(item.ApprovedByName || 0);
    self.Code = ko.observable(item.Code || '');
    self.Designation = ko.observable(item.Designation || '');
    self.Section = ko.observable(item.Section || '');
    self.Photo = ko.observable(item.Photo || '');
    self.IsApproved = ko.observable(item.IsApproved || false);
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
}