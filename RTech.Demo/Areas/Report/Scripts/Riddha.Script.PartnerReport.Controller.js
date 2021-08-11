/// <reference path="riddha.script.partnerreport.model.js" />


function partnerReportController() {
    var self = this;
    var config = new Riddha.config();
    var language = config.CurrentLanguage;
    var curDate = config.CurDate;
    var opDate = config.CurrentOperationDate;
    var url = "/Api/PartnerReportApi";
    self.OnDate = ko.observable(curDate).extend({ curDate: 'yyyy/MM/dd' });
    self.EndDate = ko.observable(curDate).extend({ curDate: 'yyyy/MM/dd' });
    self.Months = ko.observableArray([]);
    self.MonthId = ko.observable(0);
    self.ReportId = ko.observable(0);
    self.ReportTitle = ko.observable('');
    self.Year = ko.observable(Riddha.util.getYear(curDate));
    self.dateLength = ko.observable(28);
    self.CustomerExpiryReportArray = ko.observableArray([]);
    self.MonthWiseNewCustomerReportArray = ko.observableArray([]);
    self.MonthWiseDesktopCustomerReportArray = ko.observableArray([]);
    self.PageSizeByDate = ko.observable(0);

    var companyName = localStorage.getItem('companyName');
    var companyPhone = localStorage.getItem('companyPhone');
    var companyEmail = localStorage.getItem('companyEmail');
    var companyLogo = localStorage.getItem('companyLogo');
    $('.companyName').text(companyName);
    $('.companyPhone').text(companyPhone);
    $('.companyEmail').text(companyEmail);
    $(".companyImage").attr("src", companyLogo);

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

    self.ComputeMaxDate = ko.computed(function () {
        Riddha.global.getMaxDaysInMonth(self.Year(), self.MonthId())
            .then(function (maxDays) {
                self.dateLength(maxDays);
                //for ondate and end date 
                setDefDate()
            });
    });

    self.GenerateCustomerExpiryReport = function () {
        Riddha.ajax.post("/Api/PartnerReportApi/GenerateReport?fromDate=" + self.OnDate() + "&toDate=" + self.EndDate())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), CustomerExpiryReportModel);
                self.CustomerExpiryReportArray(data);
                $("#monthWiseCustomerExpiryModal").modal('show');
            })
    };

    self.GenerateMonthWiseNewCustomerReport = function (reportId, reportTitle) {
        self.ReportId(reportId);
        self.ReportTitle(reportTitle);
        Riddha.ajax.post("/Api/PartnerReportApi/GetMonthWiseCustomerReport")
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), MonthWiseNewCustomerReportModel);
                self.MonthWiseNewCustomerReportArray(data);
                $("#monthWiseNewCustomerReportModal").modal('show');
            })
    };

    self.GenerateMonthWiseDesktopCustomerReport = function () {
        Riddha.ajax.post("/Api/PartnerReportApi/GenerateMonthWiseDesktopCustomerReport?fromDate=" + self.OnDate() + "&toDate=" + self.EndDate())
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), CustomerDesktopreportVm);
                self.MonthWiseDesktopCustomerReportArray(data);
                $("#monthWiseDesktopCustomerReportModal").modal('show');
            })
    };

    self.ShowModal = function (reportId, reportTitle) {
        setDefDate();
        self.ReportId(reportId);
        self.ReportTitle(reportTitle);
        $("#ReportModal").modal('show');
    };

    self.CloseModal = function () {
        $("#ReportModal").modal('hide');
    }

}