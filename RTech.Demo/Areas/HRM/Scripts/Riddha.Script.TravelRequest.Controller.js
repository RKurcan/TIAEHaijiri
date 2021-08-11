/// <reference path="Riddha.Script.TravelRequest.Model.js" />
/// <reference path="../../Scripts/knockout-2.3.0.js" />
/// <reference path="../../Scripts/App/Globals/Riddha.Globals.ko.js" />

function travelRequestController() {
    var self = this;
    var url = "/Api/TravelRequestApi";
    var travelInformationUrl = "/Api/TravelInformationApi";
    var travelEstimateUrl = "/Api/TravelEstimateApi";
    var config = new Riddha.config();
    var lang = config.CurrentLanguage;
    self.ModeOfButton = ko.observable('Request');
    self.TravelInformationModeOfButton = ko.observable('Create');
    self.TravelEstimateModeOfButton = ko.observable('Create');

    //Travel Request Model
    self.TravelRequest = ko.observable(new TravelRequestModel());
    self.TravelRequests = ko.observableArray([]);
    self.SelectedTravelRequest = ko.observable();
    self.Employees = ko.observable(new EmployeeSearchViewModel());

    //Travel Infromation Model
    self.TravelInformation = ko.observable(new TravelInformationModel());
    self.TravelInformations = ko.observableArray([]);
    self.SelectedTravelInformation = ko.observable();

    //Travel Estimate Model
    self.TravelEstimate = ko.observable(new TravelEstimateModel());
    self.TravelEstimates = ko.observableArray([]);
    self.SelectedTravelEstimate = ko.observable();

    self.Currency = ko.observableArray([
      { Id: 0, Name: config.CurrentLanguage == 'ne' ? "नेपाली रुपैयाँ" : "NepaliRupees" },
      { Id: 1, Name: config.CurrentLanguage == 'ne' ? "भारतीय रुपैयाँ" : "IndianRupees" },
      { Id: 2, Name: config.CurrentLanguage == 'ne' ? "अमेरिकी डलर" : "AmericanDollar" },
    ]);

    self.EmployeeAutoCompleteOptions = {
        dataTextField: "Name",
        url: "/Api/TravelRequestApi/GetEmployeeLstForAutoComplete",
        select: function (item) {
            self.Employees(new EmployeeSearchViewModel(item));
            self.TravelRequest().EmployeeId(item.Id);
        },
        placeholder: lang == "ne" ? "कर्मचारी छान्नुहोस" : "Search Employee"
    };

    GetTravelRequest();
    function GetTravelRequest() {
        Riddha.ajax.get(url)
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), TravelRequestModel);
            self.TravelRequests(data);
        });
    };

    self.CreateUpdate = function () {
        if (self.TravelRequest().EmployeeId() == 0) {
            return Riddha.util.localize.Required("Employee");
        }
        if (self.ModeOfButton() == 'Request') {
            Riddha.ajax.post(url, ko.toJS(self.TravelRequest()))
            .done(function (result) {
                if (result.Status == 4) {
                    self.TravelRequest().Id(result.Data.Id);
                    GetTravelRequest();
                    //self.Reset();
                    //self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
        else if (self.ModeOfButton() == 'Update') {
            Riddha.ajax.put(url, ko.toJS(self.TravelRequest()))
            .done(function (result) {
                if (result.Status == 4) {
                    GetTravelRequest();
                    //self.Reset();
                    self.CloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };

    self.Reset = function () {
        self.TravelRequest(new TravelRequestModel({ Id: self.TravelRequest().Id() }));
        self.ModeOfButton("Request");
    };

    self.Select = function (model) {
        self.Employees(new EmployeeSearchViewModel({ Designation: model.DesignationName(), Department: model.DepartmentName(), Section: model.Section(), Photo: model.Photo() }));
        self.SelectedTravelRequest(model);
        self.TravelRequest(new TravelRequestModel(ko.toJS(model)));
        GetTravelInformation();
        GetTravelEstimate();
        self.ModeOfButton('Update');
        self.ShowModal();
    };

    self.Delete = function (travelRequest) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(url + "/" + travelRequest.Id(), null)
            .done(function (result) {
                self.TravelRequests.remove(travelRequest)
                self.Reset();
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    self.ShowModal = function () {
        $("#travelRequestModal").modal('show');
    };

    $("#travelRequestModal").on('hidden.bs.modal', function () {
        self.Reset();
        self.TravelInfoReset();
        self.TravelEstimateReset();
        self.TravelInformations([]);
        self.TravelEstimates([]);
        self.Employees(new EmployeeSearchViewModel());
        self.TravelRequest(new TravelRequestModel());
    });

    self.CloseModal = function () {
        $("#travelRequestModal").modal('hide');
        self.Reset();
        self.TravelInfoReset();
        self.TravelEstimateReset();
        self.TravelInformations([]);
        self.TravelEstimates([]);
        self.Employees(new EmployeeSearchViewModel());
        self.TravelRequest(new TravelRequestModel());
    }


    //Travel Information Crud

    //GetTravelInformation();
    function GetTravelInformation() {
        //requestId send
        Riddha.ajax.get(travelInformationUrl + "/GetTravelInformationByRequestId?RequestId=" + self.TravelRequest().Id())
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), TravelInformationModel);
            self.TravelInformations(data);
        });
    };

    self.TravelInfoCreateUpdate = function () {
        if (self.TravelInformation().TravelRequestId() == 0) {
            return Riddha.util.localize.Required("Travel Request");
        }
        if (self.TravelInformation().MainDestination() == "") {
            return Riddha.util.localize.Required("MainDestination");
        }
        if (self.TravelInformation().TravelPeriodFrom() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("TravelPeriodFrom");
        }
        if (self.TravelInformation().TravelPeriodTo() == "NaN/aN/aN") {
            return Riddha.util.localize.Required("TravelPeriodTo");
        }
        if (self.TravelInformationModeOfButton() == 'Create') {
            Riddha.ajax.post(travelInformationUrl, ko.toJS(self.TravelInformation()))
            .done(function (result) {
                if (result.Status == 4) {
                    GetTravelInformation();
                    self.TravelInfoReset();
                    self.TravelInfoCloseModal();
                    self.SelectedTravelRequest(new TravelRequestModel(result.Data));
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
        else if (self.TravelInformationModeOfButton() == 'Update') {
            Riddha.ajax.put(travelInformationUrl, ko.toJS(self.TravelInformation()))
            .done(function (result) {
                if (result.Status == 4) {
                    GetTravelInformation();
                    self.TravelInfoReset();
                    self.TravelInfoCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };

    self.TravelInfoReset = function () {
        self.TravelInformation(new TravelInformationModel({ Id: self.TravelInformation().Id() }));
        self.TravelInformation(new TravelInformationModel({ TravelRequestId: self.TravelInformation().TravelRequestId() }));
        self.TravelInformationModeOfButton("Create");
    };

    self.TravelInfoSelect = function (model) {
        self.SelectedTravelInformation(model);
        self.TravelInformation(new TravelInformationModel(ko.toJS(model)));
        self.TravelInformationModeOfButton('Update');
        self.TravelInformationShowModal();
    };

    self.TravelInfoDelete = function (travelInfo) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(travelInformationUrl + "/" + travelInfo.Id(), null)
            .done(function (result) {
                self.TravelInformations.remove(travelInfo)
                self.TravelInfoReset();
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    self.TravelInformationShowModal = function () {
        //reserve request id 
        self.TravelInformation().TravelRequestId(self.TravelRequest().Id());
        $("#travelInformationModal").modal('show');
    };

    $("#travelInformationModal").on('hidden.bs.modal', function () {
        self.TravelInfoReset();
    });

    self.TravelInfoCloseModal = function () {
        $("#travelInformationModal").modal('hide');
        self.TravelInfoReset();
    };

    //Travel  Estimate Crud

    self.TotalEstimateAmount = ko.observable(0);
    self.TotalEstimateAmount = ko.computed(function () {
        var colOneTot = 0;
        ko.utils.arrayForEach(self.TravelEstimates(), function (column) {
            colOneTot += parseFloat(column.Amount());
        });
        return colOneTot;
    });

    self.CurrencyPaidIn = ko.observableArray([
     { Id: 0, Name: config.CurrentLanguage == 'ne' ? "नेपाली रुपैयाँ" : "Nepali Rupees" },
     { Id: 1, Name: config.CurrentLanguage == 'ne' ? "भारतीय रुपैयाँ" : "Indian Rupees" },
     { Id: 2, Name: config.CurrentLanguage == 'ne' ? "अमेरिकी डलर" : "American Dollar" },
    ]);

    self.PaidBy = ko.observableArray([
     { Id: 0, Name: config.CurrentLanguage == 'ne' ? "कार्यालय" : "Office" },
     { Id: 1, Name: config.CurrentLanguage == 'ne' ? "कर्मचारी" : "Staff" },
    ]);

    self.ExpenseType = ko.observableArray([
   { Id: 0, Name: config.CurrentLanguage == 'ne' ? "होटल खर्च" : "Hotel Expense" },
   { Id: 1, Name: config.CurrentLanguage == 'ne' ? "खाना खर्च" : "Meal Expense" },
   { Id: 2, Name: config.CurrentLanguage == 'ne' ? "इन्धन खर्च" : "Fuel Expense" },
   { Id: 3, Name: config.CurrentLanguage == 'ne' ? "व्यापार यात्रा खर्च" : "Business Travel Expense" },
   { Id: 4, Name: config.CurrentLanguage == 'ne' ? "परोपकारी दान खर्च" : "Charitable Donation Expense" },
   { Id: 5, Name: config.CurrentLanguage == 'ne' ? "सामान्य कार्यालय आपूर्ति खर्च" : "General Office Supplies Expense" },
    ]);

    function GetTravelEstimate() {
        Riddha.ajax.get(travelEstimateUrl + "/GetTravelEstimateByTravelRequestId?RequestId=" + self.TravelRequest().Id())
        .done(function (result) {
            var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), TravelEstimateModel);
            self.TravelEstimates(data);
        });
    };

    self.TravelEstimateCreateUpdate = function () {
        if (self.TravelEstimate().TravelRequestId() == 0) {
            return Riddha.util.localize.Required("Travel Request");
        }
        if (self.TravelEstimateModeOfButton() == 'Create') {
            Riddha.ajax.post(travelEstimateUrl, ko.toJS(self.TravelEstimate()))
            .done(function (result) {
                if (result.Status == 4) {
                    GetTravelEstimate();
                    self.TravelEstimateReset();
                    self.TravelEstimateCloseModal();
                    self.SelectedTravelRequest(new TravelRequestModel(result.Data));
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
        else if (self.TravelEstimateModeOfButton() == 'Update') {
            Riddha.ajax.put(travelEstimateUrl, ko.toJS(self.TravelEstimate()))
            .done(function (result) {
                if (result.Status == 4) {
                    GetTravelEstimate();
                    self.TravelEstimateReset();
                    self.TravelEstimateCloseModal();
                }
                Riddha.UI.Toast(result.Message, result.Status);
            });
        }
    };

    self.TravelEstimateReset = function () {
        self.TravelEstimate(new TravelEstimateModel({ Id: self.TravelEstimate().Id() }));
        self.TravelEstimate(new TravelEstimateModel({ TravelRequestId: self.TravelEstimate().TravelRequestId() }));
        self.TravelEstimateModeOfButton("Create");
    };

    self.TravelEstimateSelect = function (model) {
        self.SelectedTravelEstimate(model);
        self.TravelEstimate(new TravelEstimateModel(ko.toJS(model)));
        self.TravelEstimateModeOfButton('Update');
        self.TravelEstimateShowModal();
    };

    self.TravelEstimateDelete = function (travelEstimate) {
        Riddha.UI.Confirm("DeleteConfirm", function () {
            Riddha.ajax.delete(travelEstimateUrl + "/" + travelEstimate.Id(), null)
            .done(function (result) {
                self.TravelEstimates.remove(travelEstimate)
                self.TravelEstimateReset();
                Riddha.UI.Toast(result.Message, result.Status);
            });
        })
    };

    self.TravelEstimateShowModal = function () {
        //reserve request id 
        self.TravelEstimate().TravelRequestId(self.TravelRequest().Id());
        $("#travelEstimateModal").modal('show');
    };


    $("#travelEstimateModal").on('hidden.bs.modal', function () {
        self.TravelEstimateReset();
    });

    self.TravelEstimateCloseModal = function () {
        $("#travelEstimateModal").modal('hide');
        self.TravelEstimateReset();
    }
}