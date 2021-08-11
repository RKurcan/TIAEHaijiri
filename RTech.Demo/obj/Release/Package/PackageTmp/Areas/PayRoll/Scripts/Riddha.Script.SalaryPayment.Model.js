

function SalaryPaymentModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.SalarySheetId = ko.observable(item.SalarySheetId || 0);
    self.SalaryPayableId = ko.observable(item.SalaryPayableId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.JoinedMonth = ko.observable(GetJoinedMonth(item.JoinedMonth) || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.Designation = ko.observable(item.Designation || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.Designation = ko.observable(item.Designation || '');
    self.MaritalStatusEnum = ko.observable(item.MaritalStatusEnum || 0);
    self.MaritalStatus = ko.observable(item.MaritalStatus || '');
    self.GenderEnum = ko.observable(item.GenderEnum || 0);
    self.Gender = ko.observable(item.Gender || '');


    self.GradeName = ko.observable(item.GradeName || '');
    self.Grade = ko.observable(item.Grade || 0);
    self.BasicSalary = ko.observable(item.BasicSalary || 0);

    //Pay Deduction 
    self.Absent = ko.observable(item.Absent || 0);
    self.Leave = ko.observable(item.Leave || 0);
    self.Late = ko.observable(item.Late || 0);
    self.EarlyOut = ko.observable(item.EarlyOut || 0);

    //Pay Deduction Amount
    self.AbsentDeductionAmount = ko.observable(item.AbsentDeductionAmount || 0);
    self.LateDeductionAmount = ko.observable(item.LateDeductionAmount || 0);
    self.EarlyOutDeductionAmount = ko.observable(item.EarlyOutDeductionAmount || 0);


    self.PFEE = ko.observable(item.PFEE || 0);
    self.PFER = ko.observable(item.PFER || 0);
    self.Gratituity = ko.observable(item.Gratituity || 0);
    self.SSEE = ko.observable(item.SSEE || 0);
    self.SSER = ko.observable(item.SSER || 0);
    self.PensionFundER = ko.observable(item.PensionFundER || 0);
    self.PensionFundEE = ko.observable(item.PensionFundEE || 0);


    // Allowance 
    self.Allowances = ko.observableArray(item.Allowances || []);


    // Gross Salary 
    // Need to be changed  
    self.DeductionAmount = ko.observable(item.DeductionAmount || 0);

    self.AdditionAmount = ko.observable(item.AdditionAmount || 0);
    self.GrossSalary = ko.observable(item.GrossSalary || 0);




    // Insurance 
    self.InsurancePremiumAmount = ko.observable(item.InsurancePremiumAmount || 0);
    self.InsurancePaidbyOffice = ko.observable(item.InsurancePaidbyOffice || 0);
    self.CITAmount = ko.observable(item.CITAmount || 0);
    self.TaxableAmount = ko.observable(item.TaxableAmount || 0);
    self.SocialSecurityTax = ko.observable(item.SocialSecurityTax || 0);
    self.RenumerationTax = ko.observable(item.RenumerationTax || 0);

    self.RebateAmount = ko.observable(item.RebateAmount || 0);
    self.NetSalary = ko.observable(item.NetSalary || 0);
    self.MonthId = ko.observable(item.MonthId || undefined);
    self.TotalAllowancesAmount = ko.observable(item.TotalAllowancesAmount || 0);
    self.TotalEmployeeSSFAmount = ko.observable(item.TotalEmployeeSSFAmount || 0);
    self.TotalEmployerSSFAmount = ko.observable(item.TotalEmployerSSFAmount || 0);
    self.TotalTDS = ko.observable(item.TotalTDS || 0);

    // The variable has been used in generate salary slip screen 

    self.GeneratePaySlip = ko.observable(item.GeneratePaySlip || 0);


}



function AllowanceModel(item) {
    var self = this;
    item = item || {};
    self.AllowanceId = ko.observable(item.AllowanceId || 0);
    self.AllowanceHead = ko.observable(item.AllowanceHead || 0);
    self.AllowanceAmount = ko.observable(item.AllowanceAmount || 0);
} 

function SSFInFoModel(item) {

    var self = this;
    item = item || {};
    self.PFEE = ko.observable(item.PFEE || 0);
    self.PFER = ko.observable(item.PFER || 0);
    self.Gratituity = ko.observable(item.Gratituity || 0);
    self.SSEE = ko.observable(item.SSEE || 0);
    self.SSER = ko.observable(item.SSER || 0);
    self.PensionFundER = ko.observable(item.PensionFundER || 0);
    self.PensionFundEE = ko.observable(item.PensionFundEE || 0);


}

function TDSInfo(item) {

    var self = this;
    item = item || {};
    self.SocialSecurityTax = ko.observable(item.SocialSecurityTax || 0);
    self.RenumerationTax = ko.observable(item.RenumerationTax || 0);

}

function PayDeductionInfoModel(item) {
    var self = this;
    item = item || {};
    self.Absent = ko.observable(item.Absent || 0);
    self.Leave = ko.observable(item.Leave || 0);
    self.Late = ko.observable(item.Late || 0);
    self.EarlyOut = ko.observable(item.EarlyOut || 0);
    self.DeductionAmount = ko.observable(item.DeductionAmount || 0);

}

function PaymentAdviceModel(item) {

    var self = this;
    item = item || {};
    self.Amount = ko.observable(item.Amount || 0);
    self.BankAccountNo = ko.observable(item.BankAccountNo || "");
    self.StaffName = ko.observable(item.StaffName || "");
    self.BankId = ko.observable(item.BankId || 0);
    self.BankName = ko.observable(item.BankName || '');

}

function PaymentAdviceMasterModel(item) {

    var self = this;
    item = item || {};
    self.To = ko.observable(item.To || '');
    self.LetterFormat = ko.observable(item.LetterFormat || 1);
    self.BankName = ko.observable(item.BankName || "रा.वा. बैंक");
    self.BankAddress = ko.observable(item.BankAddress || "तानसेन, पाल्पा");
    self.Date = ko.observable(item.Date || '');
    self.Month = ko.observable(item.Month || '');
    self.MonthId = ko.observable(item.MonthId || '');
    self.BankId = ko.observable(item.BankId || '');
    var companyName = localStorage.getItem('companyName');
    var companyAddress = localStorage.getItem('companyAddress');
    self.CompanyName = ko.observable(companyName);
    self.CompanyAddress = ko.observable(companyAddress);
    self.TotalAmount = ko.observable(item.TotalAmount || '0');
    self.AmountInWords = ko.observable(item.AmountInWords || '');

}
function BankDropDownModel(item) {

    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
}

function GetJoinedMonth(value) {

    var config = new Riddha.config();
    if (value == "" || value == undefined) {

        return "";
    }
    else {

        var date = value;
        if (config.CurrentOperationDate == "ne") {
            date = AD2BS(value);
        };
        var monthId = 0;
        if (date.split("/")[2] > 15) {

            monthId = parseInt(date.split("/")[1]) + 1;
            if (monthId > 12) {

                monthId = 1;
            }
            date = date.split("/")[0] + '/' + (monthId) + '/' + date.split("/")[2];
        }
        else {

            monthId = parseInt(date.split("/")[1]);
            date = date.split("/")[0] + '/' + (monthId) + '/' + date.split("/")[2];
        }

        return GetMonthName(monthId);

    }
}

function GetMonthName(Id) {
    var config = new Riddha.config();
    self.Months = ko.observableArray([]);
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

    var data = ko.utils.arrayFirst(self.Months(), function (item) {

        return Id == item.Id;
    });
    if (data != null) {

        return data.Name;
    }
    return "";



}