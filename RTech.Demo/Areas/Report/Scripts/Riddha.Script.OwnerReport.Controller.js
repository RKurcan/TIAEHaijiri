/// <reference path="Riddha.Script.OwnerReport.Model.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />

function ownerReportController(date) {
    var self = this;
    var url = "/Api/ResellerDeviceAssignmentReportApi";
    var config = new Riddha.config();
    var curDate = config.CurDate;
    var opDate = config.CurrentOperationDate;
    var lang = config.CurrentLanguage;
    self.OnDate = ko.observable(date).extend({ date: 'yyyy/MM/dd' });
    self.EndDate = ko.observable(date).extend({ date: 'yyyy/MM/dd' });
    self.Months = ko.observableArray([]);
    self.MonthId = ko.observable(0);
    self.Year = ko.observable(Riddha.util.getYear(curDate));


    self.CustomerExpiryReportArray = ko.observableArray([]);
    self.MonthWiseNewCustomerReportArray = ko.observableArray([]);
    self.MonthWiseDesktopCustomerReportArray = ko.observableArray([]);
    self.ReportId = ko.observable(0);
    self.ReportTitle = ko.observable('');
    self.Resellers = ko.observableArray([]);
    self.FilteredResellers = ko.observableArray([]);
    self.SearchResellerText = ko.observable('');
    var docHeight = $(window).height() - 0;
    self.PageSizeByDate = ko.observable(0);

    getMonths();
    function getMonths() {
        var monthArray = new Array();
        if (config.CurrentOperationDate == "ne") {
            if (config.CurrentLanguage == 'ne') {
                monthArray = ["बैशाख", "जेठ ", "असार", "श्रावण", "भदौ", "असोज", "कार्तिक", "मंसिर", "पुष", "माघ", "फाल्गुन", "चैत्र"];
            }
            else {
                monthArray = ["Baisakh", "Jestha", "Ashad", "Shrawan", "Bhadra", "Asoj", "Kartik", "Mangshir", "Poush", "Magh", "Falgun", "Chaitra"];
            }

        }
        else {
            if (config.CurrentLanguage == 'ne') {
                monthArray = ["जनवरी", "फेब्रुअरी", "मार्च", "अप्रिल", "मे", "जून", "जुलाई", "अगस्ट", "सेप्टेम्बर", "अक्टोबर", "नोभेम्बर", "डिसेम्बर"];
            }
            else {
                monthArray = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            }
        }
        for (var i = 0; i < 12; i++) {
            self.Months.push(new GlobalDropdownModel({ Id: i + 1, Name: monthArray[i] }));
        }
        self.MonthId(Riddha.util.getMonth(curDate));
    }

    self.DateDiffrence = function () {
        var dateFirst = new Date(self.OnDate());
        var dateSecond = new Date(self.EndDate());
        // time difference
        var timeDiff = Math.abs(dateSecond.getTime() - dateFirst.getTime());
        // days difference
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        var one = 1;
        var tDay = diffDays + one;
        // difference
        self.PageSizeByDate(tDay)
    }

    self.dateLength = ko.observable(28);
    self.ComputeMaxDate = ko.computed(function () {
        Riddha.global.getMaxDaysInMonth(self.Year(), self.MonthId())
            .then(function (maxDays) {
                self.dateLength(maxDays);
                //for ondate and end date 
                setDefDate()
            });
    });
    function setDefDate() {
        if (Riddha.config().CurrentOperationDate == 'en') {
            self.OnDate(setDateToTextBox(self.Year(), self.MonthId(), 1));
            self.EndDate(setDateToTextBox(self.Year(), self.MonthId(), self.dateLength()));
        }
        if (Riddha.config().CurrentOperationDate == 'ne') {
            var onDateNe = setDateToTextBox(self.Year(), self.MonthId(), 1);
            self.OnDate(BS2AD(onDateNe));
            var toDateNe = setDateToTextBox(self.Year(), self.MonthId(), self.dateLength());
            self.EndDate(BS2AD(toDateNe));
        }
        function setDateToTextBox(year, month, id) {
            return '' + year + '/' + Riddha.util.padLeft(month, 2) + '/' + Riddha.util.padLeft(id, 2);
        }
    }

    getResellers();
    function getResellers() {
        Riddha.ajax.get("/Api/ResellerDeviceAssignmentReportApi/GetResellersForDropdown")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Resellers(data);
                self.FilteredResellers(data);
            });
    };

    self.SearchResellerText.subscribe(function (newValue) {
        if (newValue == '') {
            self.FilteredResellers(self.Resellers());
        } else {
            self.FilteredResellers(Riddha.ko.global.filter(self.Resellers, newValue));
        }
    });

    function getSelectedResellers() {
        var resellers = "";
        ko.utils.arrayForEach(self.Resellers(), function (data) {
            if (data.Checked() == true) {
                if (resellers.length != 0)
                    resellers += "," + data.Id();
                else
                    resellers = data.Id() + '';
            }
        });
        return resellers;
    };

    self.KendoGridOptionsForResellerDeviceAssignment = function (title) {
        var resellers = getSelectedResellers();
        return {
            title: "Reseller Device Assignment Report",
            target: "#kendoResellerDeviceAssignmentReportWindow",
            url: "/Api/ResellerDeviceAssignmentReportApi/GenerateReport",
            paramData: { ResellerIds: resellers },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            groupParam: [{ field: "ResellerName" }, { field: "DeviceModel" }],
            pageSize: 50,
            columns: self.columSpecForResellerDeviceAssignment
        }
    };

    self.columSpecForResellerDeviceAssignment = [
        { field: "ResellerName", title: "Reseller", filterable: false, sortable: false },
        { field: "ResellerAddress", title: "Address", filterable: false, sortable: false },
        { field: "ResellerContact", title: "Contact", filterable: false },
        { field: "ContactPerson", title: "Contact Person", filterable: false, sortable: false },
        { field: "DeviceModel", title: "Model", filterable: false, sortable: false },
        { field: "DeviceSerialNo", title: "Serial No.", filterable: false, sortable: false },
        { field: "AssignedDate", title: "Assigned Date", filterable: false, sortable: false },
    ];

    self.ShowOrgIssueModal = function (id, title) {
        self.ReportId(id);
        self.ReportTitle(title);
        $("#showOrgIssueModal").modal('show');
    };

    self.CloseOrgIssueModal = function () {
        $("#showOrgIssueModal").modal('hide');
    };

    self.Active = ko.observable(false);
    self.KendoGridOptionsForCompanyStatus = function (title) {
        var resellers = getSelectedResellers();
        return {
            title: "Company Active / Inavtive Status Report",
            target: "#kendocompanyStatusReportWindow",
            url: "/Api/ResellerDeviceAssignmentReportApi/GenerateCompanyStatsuReport",
            paramData: { Active: self.Active },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            group: false,
            //groupParam: [{ field: "ResellerName" }, { field: "DeviceModel" }],
            pageSize: 50,
            columns: self.columSpecForCompanyStatusReport
        }
    };

    self.columSpecForCompanyStatusReport = [
        { field: "Code", title: "Code", filterable: false, sortable: false },
        { field: "Name", title: "Name", filterable: false, sortable: false },
        { field: "Address", title: "Address", filterable: false, sortable: false },
        { field: "ContactPerson", title: "Contact Person", filterable: false, sortable: false },
        { field: "ContactNo", title: "Contact No", filterable: false, sortable: false },
        { field: "LastLogin", title: "Last Login", filterable: false, sortable: false },
        { field: "ResellerName", title: "Reseller", filterable: false, sortable: false },
        { field: "ResellerContact", title: "Contact", filterable: false },
    ];

    self.ShowCompanyStatusReportModal = function (id, title) {
        self.ReportId(id);
        self.ReportTitle(title);
        $("#companyStatusReportModal").modal('show');
    };

    self.CloseCompanyStatusReportModal = function () {
        $("#companyStatusReportModal").modal('hide');
    };

    self.GenerateCustomerExpiryReport = function () {
        Riddha.ajax.post("/Api/PartnerReportApi/GenerateReport?fromDate=" + self.OnDate() + "&toDate=" + self.EndDate())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), CustomerExpiryReportModel);
                self.CustomerExpiryReportArray(data);
                $("#monthWiseCustomerExpiryModal").modal('show');
            })
    }

    self.GenerateMonthWiseNewCustomerReport = function (reportId, reportTitle) {
        self.ReportId(reportId);
        self.ReportTitle(reportTitle);
        Riddha.ajax.post("/Api/PartnerReportApi/GetMonthWiseCustomerReport")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthWiseNewCustomerReportModel);
                self.MonthWiseNewCustomerReportArray(data);
                $("#monthWiseNewCustomerReportModal").modal('show');
            })
    }

    self.GenerateMonthWiseDesktopCustomerReport = function () {
        Riddha.ajax.post("/Api/PartnerReportApi/GenerateMonthWiseDesktopCustomerReport?fromDate=" + self.OnDate() + "&toDate=" + self.EndDate())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), CustomerDesktopreportVm);
                self.MonthWiseDesktopCustomerReportArray(data);
                $("#monthWiseDesktopCustomerReportModal").modal('show');
            })
    };

    self.ShowModal = function (reportId, reportTitle) {
        self.OnDate(curDate);
        setDefDate();
        self.ReportId(reportId);
        self.ReportTitle(reportTitle);
        $("#ReportModal").modal('show');
    }

    self.CloseModal = function () {
        $("#ReportModal").modal('hide');
    };

    self.KendoGridOptionsForMonthWiseRenew = function (title) {

        return {
            title: "Month Wise Customer Renew Report",
            target: "#renewReportWindow",
            url: "/Api/ResellerDeviceAssignmentReportApi/GetMonthWiseCustomerRenewReport",
            paramData: { onDate: self.OnDate(), EndDate: self.EndDate() },
            height: docHeight,
            multiSelect: false,
            maximize: true,
            actions: [
                "Close"
            ],
            group: false,
            groupParam: [
                { field: "ResellerName" }],
            //sort: { field: "WorkDate", dir: "asc" },
            pageSize: 50,
            columns: self.columSpecForRenew
        }
    }

    self.columSpecForRenew = [

        { field: "CompanyCode", title: "Company Code", filterable: false, sortable: false },
        { field: "CompanyName", title: "Company Name", filterable: false, sortable: false },
        { field: "CompanyAddress", title: "Company Address", filterable: false, sortable: false },
        { field: "CompanyContact", title: "Company Contact", filterable: false, sortable: false },
        { field: "CompanyContactPerson", title: "Company Contact Person", filterable: false, sortable: false },
        { field: "RenewDate", title: "Renew Date", filterable: false, sortable: false, template: "#=SuitableDate(RenewDate)#" },
        { field: "ResellerName", title: "Reseller", filterable: false, sortable: false },
        { field: "ResellerContactPerson", title: "Reseller Contact Person", filterable: false, sortable: false },
        { field: "ResellerContact", title: "Reseller Contact", filterable: false },
    ];
}