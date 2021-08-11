//function PayRollModel(item) {
//    var self = this;
//    item = item || {};
//    self.Id = ko.observable(item.Id || 0);
//    self.Employee = ko.observable((item.Employee || {}));
//    self.EmployeeId = ko.observable(item.EmployeeId || 0);
//    self.EffectedFrom = ko.observable(item.EffectedFrom || '').extend({ date: '' });
//    self.BasicSaliry = ko.observable(item.BasicSaliry || 0).extend({ required: 'PayRoll BasicSaliry is Required' });
//    self.SalaryPaidPer = ko.observable(item.SalaryPaidPer || 0);
//    self.PFNo = ko.observable(item.PFNo || '');
//    self.ESINo = ko.observable(item.ESINo || '');
//    self.PANNo = ko.observable(item.PANNo || '');
//    self.PaymentBy = ko.observable(item.PaymentBy || 0);
//    self.TypeOfEmployee = ko.observable(item.TypeOfEmployee || 0);
//    self.GrossAmount = ko.observable(item.GrossAmount || 0);
//    self.AccountNo = ko.observable(item.AccountNo || '');
//    self.OtRatePerHour = ko.observable(item.OtRatePerHour || 0);
//    self.OTPayPer = ko.observable(item.OTPayPer || 0);
//    self.Conveyance = ko.observable(item.Conveyance || 0);
//    self.ConveyancePayPer = ko.observable(item.ConveyancePayPer || 0);
//    self.Medical = ko.observable(item.Medical || 0);
//    self.MedicalPayPer = ko.observable(item.MedicalPayPer || 0);
//    self.HRA = ko.observable(item.HRA || 0);
//    self.HRAPayPer = ko.observable(item.HRAPayPer || 0);
//    self.TDS = ko.observable(item.TDS || 0);
//    self.TdsPaidBy = ko.observable(item.TdsPaidBy || 0);
//    self.DA = ko.observable(item.DA || 0);
//    self.DApaidBy = ko.observable(item.DApaidBy || 0);
//    self.CITRate = ko.observable(item.CITRate || 0);
//    self.BankId = ko.observable(item.BankId || 0);
//}
function PayRollModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Employee = ko.observable((item.Employee || {}));
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EffectedFrom = ko.observable(item.EffectedFrom || '').extend({ date: '' });
    self.EndDate = ko.observable((item.EndDate == null ? "NaN/aN/aN" : item.EndDate) || '').extend({ date: '' });
    self.BasicSalary = ko.observable(item.BasicSalary || 0).extend({ required: 'PayRoll BasicSalary is Required' });
    self.SalaryPaidPer = ko.observable(item.SalaryPaidPer || 0);
    self.PaymentBy = ko.observable(item.PaymentBy || 0);
    self.GrossAmount = ko.observable(item.GrossAmount || 0);
    self.OtRatePerHour = ko.observable(item.OtRatePerHour || 0);
    self.OTPayPer = ko.observable(item.OTPayPer || 0);
    self.TDS = ko.observable(item.TDS || 0);
    self.TdsPaidBy = ko.observable(item.TdsPaidBy || 0);
    self.CITRate = ko.observable(item.CITRate || 0);
    self.PFRate = ko.observable(item.PFRate || 0);
    //Late Grace
    self.EnableLateDeduction = ko.observable(item.EnableLateDeduction || false);
    self.LateGraceDay = ko.observable(item.LateGraceDay || 0);
    self.LateDeductionBy = ko.observable(item.LateDeductionBy || 0);
    self.LateDeductionRate = ko.observable(item.LateDeductionRate || 0);
    //Early Grace
    self.EnableEarlyDeduction = ko.observable(item.EnableEarlyDeduction || false);
    self.EarlyGraceDay = ko.observable(item.EarlyGraceDay || 0);
    self.EarlyDeductionBy = ko.observable(item.EarlyDeductionBy || 0);
    self.EarlyDeductionRate = ko.observable(item.EarlyDeductionRate || 0);
    self.SSFEffectedFromDate = ko.observable(item.SSFEffectedFromDate || '').extend({ date: '' });

}
function EmployeeGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.IdCardNo = ko.observable(item.IdCardNo || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.DateOfJoin = ko.observable(item.DateOfJoin || '').extend({ date: '' });
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.Mobile = ko.observable(item.Mobile || '');
    self.Email = ko.observable(item.Email || '');
    self.PhotoURL = ko.observable(item.PhotoURL || '');
    self.DepartmentId = ko.observable(item.DepartmentId || 0);
    self.SectionId = ko.observable(item.SectionId || 0);
    self.DesignationName = ko.observable(item.DesignationName || '');
    self.GradeGroupId = ko.observable(item.GradeGroupId || '');
    self.GradeGroupName = ko.observable(item.GradeGroupName);
    self.IsActivated = ko.observable(item.IsActivated || false);
    self.UserId = ko.observable(item.UserId || 0);
    self.Status = ko.observable(item.Status);
}
function PayRollAdditionalAllowanceModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.PayrollId = ko.observable(item.PayrollId || 0);
    self.AllowanceName = ko.observable(item.AllowanceName || '');
    self.AllowanceValue = ko.observable(item.AllowanceValue || 0);
}
function EmpSearchViewModel(item) {
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
function AllowanceModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.Value = ko.observable(item.Value || 0);
    self.AllowancePeriod = ko.observable(item.AllowancePeriod || 0);
    self.MinimumWorkingHour = ko.observable(item.MinimumWorkingHour || "00:00");
    self.AllowanceCalculatedBy = ko.observable(item.AllowanceCalculatedBy || 0);
    self.AllowancePaidPer = ko.observable(item.AllowancePaidPer || 0);
}
function DeductionModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Value = ko.observable(item.Value || 0);
    self.DeductionCalculatedBy = ko.observable(item.DeductionCalculatedBy || 0);
    self.DeductionPaidPer = ko.observable(item.DeductionPaidPer || 0);
}
function EmpAllowanceModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.AllowanceId = ko.observable(item.AllowanceId || 0);
    self.Value = ko.observable(item.Value || 0);
    self.AllowanceCalculatedBy = ko.observable(item.AllowanceCalculatedBy || 0);
    self.AllowancePaidPer = ko.observable(item.AllowancePaidPer || 0);
    self.AllowancePeriod = ko.observable(item.AllowancePeriod || 0);
    self.MinimumWorkingHour = ko.observable(item.MinimumWorkingHour || "00:00");
    self.FromDate = ko.observable(item.FromDate || '').extend({ date: '' });
    self.ToDate = ko.observable(item.ToDate || '').extend({ date: '' });
}
function EmpDeductionModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.DeductionId = ko.observable(item.DeductionId || 0);
    self.Value = ko.observable(item.Value || 0);
    self.DeductionCalculatedBy = ko.observable(item.DeductionCalculatedBy || 0);
    self.DeductionPaidPer = ko.observable(item.DeductionPaidPer || 0);
}
function UnapprovedCountModel(item) {
    var self = this;
    item = item || {};
    self.Payroll = ko.observable(item.Payroll || 0);
    self.Allowance = ko.observable(item.Allowance || 0);
    self.Deduction = ko.observable(item.Deduction || 0);
    self.GradeGroup = ko.observable(item.GradeGroup || 0);
}

function PayrollVerificationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.BasicSalry = ko.observable(item.BasicSalry || 0);
    self.EffectedFrom = ko.observable(item.EffectedFrom || '').extend({ date: '' });
    self.SalaryPaidPer = ko.observable(item.SalaryPaidPer || '');
    self.GrossAmount = ko.observable(item.GrossAmount || 0);
    self.OtRatePerHour = ko.observable(item.OtRatePerHour || 0);
    self.OtPayPer = ko.observable(item.OtPayPer || '');
    self.TDS = ko.observable(item.TDS || 0);
    self.TDSPaidBy = ko.observable(item.TDSPaidBy || '');
    self.CITRate = ko.observable(item.CITRate || 0);
    self.PFRate = ko.observable(item.PFRate || 0);
}

function AllowanceVerificationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.AllowanceId = ko.observable(item.AllowanceId || 0);
    self.Value = ko.observable(item.Value || 0);
    self.AllowanceName = ko.observable(item.AllowanceName || '');
    self.AllowawncePeriod = ko.observable(item.AllowancePeriod || '');
    self.AllowanceCalculatedBy = ko.observable(item.AllowanceCalculatedBy || '');
    self.AllowancePaidPer = ko.observable(item.AllowancePaidPer || '');
    self.ApprovedById = ko.observable(item.ApprovedById || null);
}

function DeductionVerificationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.DeductionId = ko.observable(item.DeductionId || 0);
    self.DeductionName = ko.observable(item.DeductionName || '');
    self.DeductionCalculatedBy = ko.observable(item.DeductionCalculatedBy || '');
    self.DeductionPaidPer = ko.observable(item.DeductionPaidPer || '');
    self.ApprovedById = ko.observable(item.ApprovedById || null);
}
function EmpGradeUpgradeModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || 0);
    self.GradeGroupId = ko.observable(item.GradeGroupId || 0);
    self.GradeGroupName = ko.observable(item.GradeGroupName || 0);
    self.EffectedFrom = ko.observable(item.EffectedFrom || '').extend({ date: '' });
    self.EffectedTo = ko.observable(item.EffectedTo || '').extend({ date: '' });
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedBy = ko.observable(item.ApprovedBy || null);
}
function AllowanceGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Value = ko.observable(item.Value || 0);
    self.AllowancePeroid = ko.observable(item.AllowancePeroid || '');
    self.MinimumWorkingHour = ko.observable(item.MinimumWorkingHour || "00:00");
    self.AllowanceCalculatedBy = ko.observable(item.AllowanceCalculatedBy || 0);
    self.AllowancePaidPer = ko.observable(item.AllowancePaidPer == null ? '' : item.AllowancePaidPer);
}

function EmpAdvanceSalaryModel(item) {

    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.PreviousDue = ko.observable(item.PreviousDue || 0);
    self.RequestAmount = ko.observable(item.RequestAmount || 0);
    self.Interest = ko.observable(item.Interest || 0);
    self.TotalAmount = ko.computed(function () {
        if (self.RequestAmount() == 0) {

            return 0;
        }
        else if (self.Interest() == 0) {

            return self.RequestAmount();
        } else
        {
            return (parseFloat(self.RequestAmount()) * (parseFloat(self.Interest()) / 100) + parseFloat(self.RequestAmount())).toFixed(2);
        }

    });

    self.Installment = ko.observable(item.Installment || 0);
    //self.InstallmentAmount = ko.observable(item.InstallmentAmount || 0);
    self.InstallmentAmount = ko.computed(function () {

        if (self.TotalAmount() == 0 || self.Installment() == 0) {

            return 0;
        }
        else {

            return (parseFloat(self.TotalAmount()) / parseFloat(self.Installment())).toFixed(2);
        }

    });

    self.BranchId = ko.observable(item.BranchId || 0);
    self.RequestedDate = ko.observable(item.RequestedDate || "").extend({ date: "yyyy/MM/dd" });
    self.CreationDate = ko.observable(item.CreationDate || "").extend({ date: "yyyy/MM/dd" });

}


function TaxInfoOfCurrentFiscalYear(item) {

    var self = this;
    item = item || {};
    self.Gratituity = ko.observable(item.Gratituity || 0);
    self.PFEmployee = ko.observable(item.PFEmployee || 0);
    self.PFEmployer = ko.observable(item.PFEmployer || 0);
    self.SSEpmployee = ko.observable(item.SSEpmployee || 0);
    self.SSEpmployer = ko.observable(item.SSEpmployer || 0);
}