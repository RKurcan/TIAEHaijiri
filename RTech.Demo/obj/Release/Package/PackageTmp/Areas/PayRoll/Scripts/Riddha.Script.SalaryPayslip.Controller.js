/// <reference path="../../../scripts/knockout-2.3.0.js" />
/// <reference path="../../../Scripts/App/Globals/Riddha.Globals.ko.js" />
/// <reference path="riddha.script.salarypayslip.model.js" />



function SalaryPayslipController() {
    var self = this;
    var url = "/Api/SalaryPaySlipApi";
    self.SalaryPayment = ko.observable(new SalaryPaymentModel());
    self.SalaryPayments = ko.observableArray([]);
    self.AllowanceHeads = ko.observable([]);
    self.Departments = ko.observableArray([]);
    self.Employees = ko.observableArray([]);
    self.DepartmentId = ko.observable(0);
    self.EmployeeId = ko.observable(0);
    self.FilteredEmployees = ko.observableArray([]);
    self.CheckAllDepartments = ko.observable(false);
    self.CheckAllSections = ko.observable(false);
    self.CheckAllEmployees = ko.observable(false);
    self.Sections = ko.observableArray([]);
    var config = new Riddha.config();
    var curDate = config.CurDate;
    self.Months = ko.observableArray([]);

    self.FiscalYear = ko.observable("");
    self.OnDate = ko.observable('').extend({ Date: 'yyyy/MM/dd' });
    self.EndDate = ko.observable('').extend({ Date: 'yyyy/MM/dd' });
    self.MonthId = ko.observable(Riddha.util.getMonth(curDate));
    self.Year = ko.observable(Riddha.util.getYear(curDate));

    self.TotalTaxableAmount = ko.observable(0);
    self.TotalTaxAmount = ko.observable(0);
    self.TotalNetSalary = ko.observable(0);

    self.HeaderHeight = ko.observable(0);


    self.CompanyName = ko.observable(localStorage.companyName);
    self.CompanyAddress = ko.observable(localStorage.companyAddress);
    self.SalaryPaymentDesc = function () {
        if (config.CurrentLanguage == "ne") {

            return self.GetMonthName(self.MonthId()) + " महिनाको तलब";

        }
        else {


            return self.GetMonthName(self.MonthId()) + " Month Salary";

        }

    }


    //NonGovernment  = 0,
    //Government = 1
    self.OrganizationType = ko.observable(0);
    GetOrganizationType();

    function GetOrganizationType() {

        Riddha.ajax.get("/Api/SalaryPaymentApi" + "/GetOrganizationType").done(function (result) {
            if (result.Status == 4) {
                self.OrganizationType(result.Data);
                if (self.OrganizationType() == 1) {

                    $("#governmentPaysheet").show();
                    $("#nonGovernmentPaySheet").hide();
                } else {


                    $("#governmentPaysheet").hide();
                    $("#nonGovernmentPaySheet").show();
                }
            }

        })
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
        //self.PageSizeByDate(tDay)
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



    function getSelectedEmps() {
        var employees = "";
        ko.utils.arrayForEach(self.FilteredEmployees(), function (data) {
            if (data.Checked() == true) {
                if (employees.length != 0)
                    employees += "," + data.Id();
                else
                    employees = data.Id() + '';
            }
        });
        return employees;
    }
    function getSelectedDepartments() {
        var deps = "";
        ko.utils.arrayForEach(self.Departments(), function (data) {
            if (data.Checked() == true) {
                if (deps.length != 0)
                    deps += "," + data.Id();
                else
                    deps = data.Id() + '';
            }
        });
        return deps;
    }
    function getSelectedSections() {
        var sections = "";
        ko.utils.arrayForEach(self.Sections(), function (data) {
            if (data.Checked() == true) {
                if (sections.length != 0)
                    sections += "," + data.Id();
                else
                    sections = data.Id() + '';
            }
        });
        return sections;
    }

    if (config.CurrentOperationDate == "en") {
        self.Months([
            { Id: 1, Name: "January" },
            { Id: 2, Name: "February" },
            { Id: 3, Name: "March" },
            { Id: 4, Name: "April" },
            { Id: 5, Name: "May" },
            { Id: 6, Name: "June" },
            { Id: 7, Name: "July" },
            { Id: 8, Name: "August" },
            { Id: 9, Name: "September" },
            { Id: 10, Name: "October" },
            { Id: 11, Name: "November" },
            { Id: 12, Name: "December" }
        ]);
    }
    else {
        self.Months([
            { Id: 1, Name: config.CurrentLanguage == 'ne' ? "बैसाख" : "Baishakh" },
            { Id: 2, Name: config.CurrentLanguage == 'ne' ? "जेठ" : "Jestha" },
            { Id: 3, Name: config.CurrentLanguage == 'ne' ? "असार" : "Asar" },
            { Id: 4, Name: config.CurrentLanguage == 'ne' ? "साउन" : "Shrawan" },
            { Id: 5, Name: config.CurrentLanguage == 'ne' ? "भदौ" : "Bhadra" },
            { Id: 6, Name: config.CurrentLanguage == 'ne' ? "असोज" : "Aswin" },
            { Id: 7, Name: config.CurrentLanguage == 'ne' ? "कार्तिक" : "Kartik" },
            { Id: 8, Name: config.CurrentLanguage == 'ne' ? "मंसिर" : "Mansir" },
            { Id: 9, Name: config.CurrentLanguage == 'ne' ? "पुष" : "Poush" },
            { Id: 10, Name: config.CurrentLanguage == 'ne' ? "माघ" : "Magh" },
            { Id: 11, Name: config.CurrentLanguage == 'ne' ? "फाल्गुन" : "Falgun" },
            { Id: 12, Name: config.CurrentLanguage == 'ne' ? "चैत्र" : "Chaitra" },
        ]);
    }

    if (config.CurrentLanguage == "ne" && config.CurrentOperationDate == "en") {
        self.Months([
            { Id: 1, Name: "जनवरी" },
            { Id: 2, Name: "फेब्रुअरी" },
            { Id: 3, Name: "मार्च" },
            { Id: 4, Name: "अप्रिल" },
            { Id: 5, Name: "मे" },
            { Id: 6, Name: "जून" },
            { Id: 7, Name: "जुलाई" },
            { Id: 8, Name: "अगस्ट" },
            { Id: 9, Name: "सेप्टेम्बर" },
            { Id: 10, Name: "अक्टोबर" },
            { Id: 11, Name: "नोभेम्बर" },
            { Id: 12, Name: "डिसेम्बर" }
        ]);
    }


    self.GetMonthName = function (MonthId) {
        var value = ko.utils.arrayFirst(self.Months(), function (item) {

            return item.Id == MonthId;
        })
        return value.Name;
    }
    //GetSalaryPayment();
    function GetSalaryPayment() {


        var departments = getSelectedDepartments();
        var sections = getSelectedSections();
        var employees = getSelectedEmps();
        debugger;
        Riddha.ajax.get(url + "/GetEmpSalaryPayableInfo?DepartmentIds=" + departments
            + "&SectionIds=" + sections + "&EmpIds=" + employees + "&MonthId=" + self.MonthId())
            .done(function (result) {
                debugger;
                if (result.Status == 4) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), SalaryPaymentModel);
                    self.SalaryPayments(data);
                    if (result.Data.length > 0) {
                        var allowanceHeads = Riddha.ko.global.arrayMap(ko.toJS(result.Data[0].Allowances), AllowanceModel);
                        self.AllowanceHeads(allowanceHeads);
                    }
                    CalculateTotalSummary();
                }
                else {
                    self.ResetSalarySheet();
                    CalculateTotalSummary();
                    Riddha.UI.Toast(result.Message, result.Status);
                }

            });
    }

    self.TotalGrossSalary = ko.observable(0);
    self.TotalInsuranceAmount = ko.observable(0);
    self.TotalPFEmployee = ko.observable(0);
    self.TotalPensionEmployee = ko.observable(0);
    self.TotalCITAmount = ko.observable(0);
    self.TotalInsuranceAmountER = ko.observable(0);
    self.TotalPFEmployer = ko.observable(0);
    self.TotalPensionEmployer = ko.observable(0);
    self.TotalBasicSalary = ko.observable(0);
    self.TotalDeductionAmount = ko.observable(0);
    self.TotalAdditionAmount = ko.observable(0);
    self.TotalGradeAmount = ko.observable(0);

    function CalculateTotalSummary() {

        var totaltaxableAmount = 0;
        var totaltaxAmount = 0;
        var totalNetSalary = 0;
        var totalGrossSalary = 0;
        var totalInsuranceAmount = 0;
        var totalPFEmployee = 0;
        var totalPensionEmployee = 0;
        var totalCIT = 0;
        var totalInsuranceAmountER = 0;
        var totalPFER = 0;
        var totalPensionER = 0;
        var totalBasicSalary = 0;
        var totalDeduction = 0;
        var totalAddition = 0;
        var totalGrade = 0;

        for (var j = 0; j < self.AllowanceHeads().length; j++) {

            self.AllowanceHeads()[j].AllowanceAmount(0);
        }

        for (var i = 0; i < self.SalaryPayments().length; i++) {

            totaltaxableAmount = totaltaxableAmount + self.SalaryPayments()[i].TaxableAmount();
            totaltaxAmount = totaltaxAmount + self.SalaryPayments()[i].TotalTDS();
            totalNetSalary = totalNetSalary + self.SalaryPayments()[i].NetSalary();
            totalGrossSalary = totalGrossSalary + self.SalaryPayments()[i].GrossSalary();
            totalInsuranceAmount = totalInsuranceAmount + self.SalaryPayments()[i].InsurancePremiumAmount();
            totalPFEmployee = totalPFEmployee + self.SalaryPayments()[i].PFEE();
            totalPensionEmployee = totalPensionEmployee + self.SalaryPayments()[i].PensionFundEE();
            totalCIT = totalCIT + self.SalaryPayments()[i].CITAmount();
            totalInsuranceAmountER = totalInsuranceAmountER + self.SalaryPayments()[i].InsurancePaidbyOffice();
            totalPFER = totalPFER + self.SalaryPayments()[i].PFER();
            totalPensionER = totalPensionER + self.SalaryPayments()[i].PensionFundER();
            totalBasicSalary = totalBasicSalary + self.SalaryPayments()[i].BasicSalary();
            totalDeduction = totalDeduction + self.SalaryPayments()[i].DeductionAmount();
            totalAddition = totalAddition + self.SalaryPayments()[i].AdditionAmount();
            totalGrade = totalGrade + self.SalaryPayments()[i].Grade();
            debugger;
            //var allowances = self.SalaryPayments()[i].Allowances();
            //for (var j = 0; j < allowances.length; j++) {

            //    ko.utils.arrayFirst(self.AllowanceHeads(), function (item) {

            //        if (item.AllowanceId() == allowances[j].AllowanceId)
            //        {
            //            item.AllowanceAmount(parseFloat(item.AllowanceAmount()) + parseFloat(allowances[j].AllowanceAmount));
            //            return;
            //        }
            //    });
            //}
            var allowances = self.SalaryPayments()[i].Allowances();

            for (var j = 0; j < self.AllowanceHeads().length; j++) {

                for (var k = 0; k < allowances.length; k++) {

                    if (self.AllowanceHeads()[j].AllowanceId() == allowances[k].AllowanceId) {

                        self.AllowanceHeads()[j].AllowanceAmount(parseFloat(self.AllowanceHeads()[j].AllowanceAmount()) + parseFloat(allowances[k].AllowanceAmount));
                    }
                }
            }
        }
        self.TotalTaxableAmount(totaltaxableAmount.toFixed(2));
        self.TotalTaxAmount(totaltaxAmount.toFixed(2));
        self.TotalNetSalary(totalNetSalary.toFixed(2));
        self.TotalGrossSalary(totalGrossSalary.toFixed(2));
        self.TotalInsuranceAmount(totalInsuranceAmount.toFixed(2));
        self.TotalPFEmployee(totalPFEmployee.toFixed(2));
        self.TotalPensionEmployee(totalPensionEmployee.toFixed(2));
        self.TotalCITAmount(totalCIT.toFixed(2));
        self.TotalInsuranceAmountER(totalInsuranceAmountER.toFixed(2));
        self.TotalPFEmployer(totalPFER.toFixed(2));
        self.TotalPensionEmployer(totalPensionER.toFixed(2));
        self.TotalBasicSalary(totalBasicSalary.toFixed(2));
        self.TotalDeductionAmount(totalDeduction.toFixed(2));
        self.TotalAdditionAmount(totalAddition.toFixed(2));
        self.TotalGradeAmount(totalGrade.toFixed(2));


    }

    self.ResetSalarySheet = function () {
        self.AllowanceHeads([]);
        self.SalaryPayments([]);


    }
    getDepartments();
    function getDepartments() {
        Riddha.ajax.get("/Api/SalarySheetApi/GetDepartments", null)
            .done(function (result) {
                var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                self.Departments(data);
            });
    };


    self.CheckAllDepartments.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Departments(), function (item) {
            item.Checked(newValue);
        });
        if (newValue) {
            self.GetSections();
        }
        else {
            self.Sections([]);
            self.CheckAllSections(false);
        }
    });

    self.GetSections = function () {
        var departments = "";
        ko.utils.arrayForEach(self.Departments(), function (data) {
            if (data.Checked() == true) {
                if (departments.length != 0)
                    departments += "," + data.Id();
                else
                    departments = data.Id() + '';
            }
            else {
                self.Sections([]);
            }
        });
        if (departments.length > 0) {
            Riddha.ajax.get("/Api/SectionApi/GetSectionsByDepartment/" + departments)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Sections(data);
                });
        }
    };
    self.CheckAllSections.subscribe(function (newValue) {
        ko.utils.arrayForEach(self.Sections(), function (item) {
            item.Checked(newValue);
        });
        if (newValue) {
            self.GetEmployee();
        }
        else {
            self.Employees([]);
        }
    });
    self.SearchItemText = ko.observable('');
    self.SearchItemText.subscribe(function (newValue) {
        if (newValue == '') {
            self.FilteredEmployees(self.Employees());
        } else {
            self.FilteredEmployees(Riddha.ko.global.filter(self.Employees, newValue));
        }
    })


    self.GetEmployee = function () {
        var sections = "";
        ko.utils.arrayForEach(self.Sections(), function (data) {
            if (data.Checked() == true) {
                if (sections.length != 0)
                    sections += "," + data.Id();
                else
                    sections = data.Id() + '';
            }
            else {
                self.FilteredEmployees([]);
            }
        });
        if (sections.length > 0) {
            Riddha.ajax.get("/Api/AttendanceReportApi/GetEmployeeBySection?id=" + sections + "&activeInactiveMode=" + 0)
                .done(function (result) {
                    var data = Riddha.ko.global.arrayMap(ko.toJS(result.Data), checkBoxModel);
                    self.Employees(data);
                    self.FilteredEmployees(data);
                });
        }
    };





    self.Refresh = function () {
        GetSalaryPayment();

    }


    self.SendToSalaryReview = function () {


    }

    self.SSFInfo = ko.observable(new SSFInFoModel());
    self.Allowances = ko.observableArray([]);

    self.ViewAllowances = function (model) {
        debugger;
        self.Allowances(model.Allowances);
        $("#AllowanceViewModal").modal('show');
    }
    self.ViewSSFInfo = function (model) {

        debugger;
        self.SSFInfo().PFEE(model.PFEE());
        self.SSFInfo().PFER(model.PFER());
        self.SSFInfo().Gratituity(model.Gratituity());
        self.SSFInfo().SSEE(model.SSEE());
        self.SSFInfo().SSER(model.SSER());
        self.SSFInfo().PensionFundER(model.PensionFundER());
        self.SSFInfo().PensionFundEE(model.PensionFundEE());
        $("#SSFViewModal").modal('show');
    }

    self.TDSInfo = ko.observable(new TDSInfo());

    self.ViewTDSInfo = function (model) {

        self.TDSInfo().SocialSecurityTax(model.SocialSecurityTax());
        self.TDSInfo().RenumerationTax(model.RenumerationTax());

        $("#TDSInfoViewModal").modal('show');
    }

    self.PayDeductionInfo = ko.observable(new PayDeductionInfoModel());
    self.ViewDeductionInfo = function (model) {

        self.PayDeductionInfo().Absent(model.Absent());
        self.PayDeductionInfo().Leave(model.Leave());
        self.PayDeductionInfo().Late(model.Late());
        self.PayDeductionInfo().EarlyOut(model.EarlyOut());
        self.PayDeductionInfo().DeductionAmount(model.DeductionAmount());

        $("#DeductionInfoViewModal").modal('show')
    }

    GetFiscalYear();
    function GetFiscalYear() {
        Riddha.ajax.get("/Api/SalarySheetApi/GetCurrentFiscalYear").done(function (result) {
            if (result.Status == 4) {
                self.FiscalYear(result.Data);
            }
        });

    }

    GetHeaderHeight();
    function GetHeaderHeight() {

        var element = $("#salarySheetTbl th");
        self.HeaderHeight(element[0].offsetHeight);
    }

    self.SalaryPaySlipInfo = ko.observable(new SalaryPaySlipInfoModel());
    self.SalaryPaySlipInfos = ko.observableArray([]);
    self.SalaryPayslipMaster = ko.observable(new SalaryPayslipMasterModel());

    self.SalaryPayslipMasters = ko.observableArray([]);
    self.GeneratePaySlip = function () {


        if (self.SalaryPayments().length == 0) {

            Riddha.UI.Toast("Salary Sheet is empty", 0);
            return;
        }
        Riddha.UI.Confirm("Are you sure you want to generate salary payslip?", function () {
            Riddha.ajax.post(url, { SalaryPaymentVm: ko.toJS(self.SalaryPayments()) }).done(function (result) {
                if (result.Status == 4) {

                    self.SalaryPayslipMasters([]);
                    ko.utils.arrayForEach(self.SalaryPayments(), function (item) {


                        //self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: "Name", Amount: item.EmployeeName() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'आधारभूत तलब' : "Basic Salary", Amount: item.BasicSalary() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'ग्रेड' : "Grade", Amount: item.Grade() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'कुल भत्ता' : "Total Allowances", Amount: item.TotalAllowancesAmount() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'कटौती खाता' : "Pay Deduction", Amount: item.DeductionAmount() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'कुल तलब' : "Gross Salary", Amount: item.GrossSalary() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'बीमा कटौती' : "Insurance Amount", Amount: item.InsurancePremiumAmount() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'बीमा थप' : "Insurance Amount (Empyr)", Amount: item.InsurancePaidbyOffice() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'ना.ल.कोष' : "Contribution To CIT", Amount: item.CITAmount() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'क.सा.को कटौती' : "Contribution To SSF ", Amount: item.TotalEmployeeSSFAmount() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'क.सा.को थप' : "SSF Employer", Amount: item.TotalEmployerSSFAmount() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'भुक्तानीयोग्य रकम' : "Taxable Amount", Amount: item.TaxableAmount() }));
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'सा.सुर.कर' : "TDS", Amount: item.TotalTDS() }))
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'छूट रकम' : "Rebate Amount", Amount: item.RebateAmount() }))
                        self.SalaryPaySlipInfos.push(new SalaryPaySlipInfoModel({ Name: config.CurrentLanguage == 'ne' ? 'कुल प्राप्त' : "Net Salary Take Home", Amount: item.NetSalary() }));


                        self.SalaryPayslipMasters.push(
                            new SalaryPayslipMasterModel(
                                {
                                    SalaryPayableId: item.SalaryPayableId(),
                                    Month: self.GetMonthName(item.MonthId()),
                                    EmployeeName: item.EmployeeName(),
                                    EmployeeCode: item.EmployeeCode(),
                                    Designation: item.Designation(),
                                    SalaryPaySlipInfos: ko.toJS(self.SalaryPaySlipInfos())
                                }));
                        self.SalaryPaySlipInfos([]);

                    });
                    //$("#PaySlipPreviewViewModal").modal('show');
                    $("#PaySlipMultiplePreviewModal").modal('show');
                }
                Riddha.UI.Toast(result.Message, result.Status)
            });

        });



    };

    self.Print = function (id) {
        //$("#orderConfirmationPrint").modal('show');
        var mywindow = window.open('', 'PRINT', '');

        mywindow.document.write('<html><head>');
        mywindow.document.write('<link rel="stylesheet" href="/bootstrap/css/bootstrap.min.css">');
        mywindow.document.write('<link rel="stylesheet" href="/dist/css/AdminLTE.min.css">');
        mywindow.document.write('<link href="~/Content/Site.css" rel="stylesheet" />');
        mywindow.document.write('');
        mywindow.document.write('</head><body >');

        mywindow.document.write($(id).html());
        mywindow.document.write('</body></html>');

        mywindow.document.close(); // necessary for IE >= 10
        mywindow.focus(); // necessary for IE >= 10*/

        setTimeout(function () {
            mywindow.print();
            mywindow.close();
            return true;
        }, 1000);





        //$(id).print({
        //    globalStyles: true,
        //    mediaPrint: false,
        //    stylesheet: null,
        //    noPrintSelector: ".no-print",
        //    iframe: true,
        //    append: null,
        //    prepend: null,
        //    manuallyCopyFormValues: true,
        //    deferred: $.Deferred(),
        //    timeout: 750,
        //    title: null,
        //    doctype: '<!doctype html>'


        //});
    }

    self.ExportExcel = function (id) {

        var tab_text = "<table border='2px'>";
        var textRange; var j = 0;
        tab = document.getElementById(id); // id of table
        var headerRowCount = $("#" + id + " thead tr").length;
        for (j = 0; j < tab.rows.length; j++) {
            if (j < headerRowCount) {
                tab_text += "<tr bgcolor='#87AFC6'>";
            }
            else {
                tab_text += "<tr>";
            }
            tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
            //tab_text=tab_text+"</tr>";
        }

        tab_text = tab_text + "</table>";
        tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
        tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
        tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
        {
            txtArea1.document.open("txt/html", "replace");
            txtArea1.document.write(tab_text);
            txtArea1.document.close();
            txtArea1.focus();
            sa = txtArea1.document.execCommand("SaveAs", true, "Report.xls");
        }
        else                 //other browser not tested on IE 11
            sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

        return (sa);
    }




}