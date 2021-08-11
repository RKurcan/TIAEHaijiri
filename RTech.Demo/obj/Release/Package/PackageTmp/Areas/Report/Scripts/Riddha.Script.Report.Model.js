function ReportItemModel(item) {
    var self = this;
    item = item || {};
    self.SN = ko.observable(item.SN || 0);
    self.ReportId = ko.observable(item.ReportId || 0);
    self.Report = ko.observable(item.Report || 0);
    self.Description = ko.observable(item.Description || 0);
}


function EmployeeSummaryModel(item) {
    var self = this;
    item = item || {};
    self.SN = ko.observable(item.SN || 0);
    self.DepartmentCode = ko.observable(item.DepartmentCode || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.DepartmentNamee = ko.observable(item.DepartmentNamee || '');
    self.SectionCode = ko.observable(item.SectionCode || '');
    self.SectionName = ko.observable(item.SectionName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeDeviceCode = ko.observable(item.EmployeeDeviceCode || 0);
    self.MobileNo = ko.observable(item.MobileNo || '');
    self.EmploymentStatus = ko.observable(item.EmploymentStatus || 0);
    self.EmploymentStatusString = ko.observable(item.EmploymentStatusString || '');
    self.WorkDate = ko.observable(item.WorkDate || '');
    self.Present = ko.observable(item.Present || '');
    self.NepDate = ko.observable(item.NepDate || '');
    self.DayName = ko.observable(item.DayName || '');
    self.ShiftName = ko.observable(item.ShiftName || '');
    self.PlannedTimeIn = ko.observable(item.PlannedTimeIn || '');
    self.PlannedTimeOut = ko.observable(item.PlannedTimeOut || '');
    self.PlannedLunchIn = ko.observable(item.PlannedLunchIn || '');
    self.PlannedLunchOut = ko.observable(item.PlannedLunchOut || '');
    self.ShiftStartGrace = ko.observable(item.ShiftStartGrace || '');
    self.ShiftEndGrace = ko.observable(item.ShiftEndGrace || '');
    self.ActualTimeIn = ko.observable(item.ActualTimeIn || '');
    self.ActualTimeOut = ko.observable(item.ActualTimeOut || '');
    self.ActualLunchIn = ko.observable(item.ActualLunchIn || '');

    self.PresentInHoliday = ko.observable(item.PresentInHoliday || '');
    self.PresentInDayOff = ko.observable(item.PresentInDayOff || '');
    self.Misc = ko.observable(item.Misc || '');
    self.Leave = ko.observable(item.Leave || '');
    self.Worked = ko.observable(item.Worked || '');

    self.ActualLunchOut = ko.observable(item.ActualLunchOut || '');
    self.ShiftTypeId = ko.observable(item.ShiftTypeId || 0);
    self.Standard = ko.observable(item.Standard || '');
    self.Normal = ko.observable(item.Normal || '');
    self.Ot = ko.observable(item.Ot || '');
    self.Actual = ko.observable(item.Actual || '');
    self.OnLeave = ko.observable(item.OnLeave || '');
    self.Holiday = ko.observable(item.Holiday || '');
    self.Weekend = ko.observable(item.Weekend || '');
    self.Absent = ko.observable(item.Absent || '');
    self.OfficeOut = ko.observable(item.OfficeOut || '');
    self.LateIn = ko.observable(item.LateIn || '');
    self.EarlyIn = ko.observable(item.EarlyIn || '');
    self.EarlyOut = ko.observable(item.EarlyOut || '');
    self.LateOut = ko.observable(item.LateOut || '');
    self.DutyDay = ko.observable(item.DutyDay || '');
    self.Remark = ko.observable(item.Remark || '');
    self.Remarks = ko.observable(item.Remarks || '');
    self.LeaveName = ko.observable(item.LeaveName || '');
    self.OfficeVisit = ko.observable(item.OfficeVisit || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeNameNp = ko.observable(item.EmployeeNameNp || '');
    self.SinglePunch = ko.observable(item.SinglePunch || false);
    self.NoPunch = ko.observable(item.NoPunch || false);
    self.TwoPunch = ko.observable(item.TwoPunch || false);
    self.FourPunch = ko.observable(item.FourPunch || false);
    self.MultiplePunch = ko.observable(item.MultiplePunch || false);
    self.RoundTheClock = ko.observable(item.RoundTheClock || false);
    self.getShiftHours = ko.observable(item.getShiftHours || '');
    self.HolidayName = ko.observable(item.HolidayName || '');

    self.TotalDays = ko.observable(item.TotalDays || '');
    self.ShiftWorkedTime = ko.observable(item.ShiftWorkedTime || '');
}

function ManualPunchReportModel(item) {
    var self = this;
    item = item || {};
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Date = ko.observable(item.Date || '');
    self.Time = ko.observable(item.Time || '');
    self.Remarks = ko.observable(item.Remarks || '');
}

function MonthlyEarlyInModel(item) {
    var self = this;
    item = item || {};
    self.SN = ko.observable(item.SN || 0);
    self.DepartmentCode = ko.observable(item.DepartmentCode || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.DepartmentNamee = ko.observable(item.DepartmentNamee || '');
    self.SectionCode = ko.observable(item.SectionCode || '');
    self.SectionName = ko.observable(item.SectionName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeDeviceCode = ko.observable(item.EmployeeDeviceCode || 0);
    self.MobileNo = ko.observable(item.MobileNo || '');
    self.EmploymentStatus = ko.observable(item.EmploymentStatus || 0);
    self.EmploymentStatusString = ko.observable(item.EmploymentStatusString || '');
    self.WorkDate = ko.observable(item.WorkDate || '');

    self.NepDate = ko.observable(item.NepDate || '');
    self.DayName = ko.observable(item.DayName || '');
    self.ShiftName = ko.observable(item.ShiftName || '');
    self.PlannedTimeIn = ko.observable(item.PlannedTimeIn || '');
    self.PlannedTimeOut = ko.observable(item.PlannedTimeOut || '');
    self.PlannedLunchIn = ko.observable(item.PlannedLunchIn || '');
    self.PlannedLunchOut = ko.observable(item.PlannedLunchOut || '');
    self.ShiftStartGrace = ko.observable(item.ShiftStartGrace || '');
    self.ShiftEndGrace = ko.observable(item.ShiftEndGrace || '');
    self.ActualTimeIn = ko.observable(item.ActualTimeIn || '');
    self.ActualTimeOut = ko.observable(item.ActualTimeOut || '');
    self.ActualLunchIn = ko.observable(item.ActualLunchIn || '');

    self.ActualLunchOut = ko.observable(item.ActualLunchOut || '');
    self.ShiftTypeId = ko.observable(item.ShiftTypeId || 0);
    self.Standard = ko.observable(item.Standard || '');
    self.Normal = ko.observable(item.Normal || '');
    self.Ot = ko.observable(item.Ot || '');
    self.Actual = ko.observable(item.Actual || '');
    self.OnLeave = ko.observable(item.OnLeave || '');
    self.Holiday = ko.observable(item.Holiday || '');
    self.Weekend = ko.observable(item.Weekend || '');
    self.Absent = ko.observable(item.Absent || '');
    self.LateIn = ko.observable(item.LateIn || '');
    self.EarlyIn = ko.observable(item.EarlyIn || '');
    self.EarlyOut = ko.observable(item.EarlyOut || '');
    self.LateOut = ko.observable(item.LateOut || '');

    self.Remark = ko.observable(item.Remark || '');
    self.LeaveName = ko.observable(item.LeaveName || '');
    self.OfficeVisit = ko.observable(item.OfficeVisit || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeNameNp = ko.observable(item.EmployeeNameNp || '');
    self.SinglePunch = ko.observable(item.SinglePunch || false);
    self.NoPunch = ko.observable(item.NoPunch || false);
    self.TwoPunch = ko.observable(item.TwoPunch || false);
    self.FourPunch = ko.observable(item.FourPunch || false);
    self.MultiplePunch = ko.observable(item.MultiplePunch || false);
    self.RoundTheClock = ko.observable(item.RoundTheClock || false);
    self.getShiftHours = ko.observable(item.getShiftHours || '');
    self.HolidayName = ko.observable(item.HolidayName || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');


}



function AttendanceReportDetailModel(item) {
    var self = this;
    item = item || {};
    self.SN = ko.observableArray(item.SN || 0);
    self.DepartmentCode = ko.observable(item.DepartmentCode || '');
    self.SectionCode = ko.observable(item.SectionCode || '');
    self.SectionName = ko.observable(item.SectionName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeDeviceCode = ko.observable(item.EmployeeDeviceCode || 0);
    self.MobileNo = ko.observable(item.MobileNo || '');
    self.EmploymentStatusString = ko.observable(item.EmploymentStatusString || '');
    self.WorkDate = ko.observable(item.WorkDate || '');
    self.NepDate = ko.observable(item.NepDate || '');
    self.DayName = ko.observable(item.DayName || '');
    self.ShiftName = ko.observable(item.ShiftName || '');
    self.PlannedTimeIn = ko.observable(item.PlannedTimeIn || '');
    self.PlannedTimeOut = ko.observable(item.PlannedTimeOut || '');
    self.PlannedLunchIn = ko.observable(item.PlannedLunchIn || '');
    self.PlannedLunchOut = ko.observable(item.PlannedLunchOut || '');
    self.ShiftStartGrace = ko.observable(item.ShiftStartGrace || '');
    self.ShiftEndGrace = ko.observable(item.ShiftEndGrace || '');
    self.ActualTimeIn = ko.observable(item.ActualTimeIn || '');
    self.ActualTimeOut = ko.observable(item.ActualTimeOut || '');
    self.ActualLunchIn = ko.observable(item.ActualLunchIn || '');
    self.ActualLunchOut = ko.observable(item.ActualLunchOut || '');
    self.ShiftTypeId = ko.observable(item.ShiftTypeId || 0);
    self.Standard = ko.observable(item.Standard || '');
    self.Normal = ko.observable(item.Normal || '');
    self.Ot = ko.observable(item.Ot || '');
    self.Actual = ko.observable(item.Actual || '');
    self.OnLeave = ko.observable(item.OnLeave || '');
    self.Holiday = ko.observable(item.Holiday || '');
    self.Weekend = ko.observable(item.Weekend || '');
    self.Absent = ko.observable(item.Absent || '');
    self.LateIn = ko.observable(item.LateIn || '');
    self.EarlyIn = ko.observable(item.EarlyIn || '');
    self.EarlyOut = ko.observable(item.EarlyOut || '');
    self.LateOut = ko.observable(item.LateOut || '');
    self.Remark = ko.observable(item.Remark || '');
    self.LeaveName = ko.observable(item.LeaveName || '');
    self.OfficeVisit = ko.observable(item.OfficeVisit || '');
    self.Kaj = ko.observable(item.Kaj || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeNameNp = ko.observable(item.EmployeeNameNp || '');
    self.SinglePunch = ko.observable(item.SinglePunch || false);
    self.NoPunch = ko.observable(item.NoPunch || false);
    self.TwoPunch = ko.observable(item.TwoPunch || false);
    self.FourPunch = ko.observable(item.FourPunch || false);
    self.MultiplePunch = ko.observable(item.MultiplePunch || false);
    self.RoundTheClock = ko.observable(item.RoundTheClock || false);
    self.getShiftHours = ko.observable(item.getShiftHours || '');
    self.HolidayName = ko.observable(item.HolidayName || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
}



function MOnthWiseEmployeeGroupReportModel(item) {


    var self = this;
    item = item || {};
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.DepartmentCode = ko.observable(item.DepartmentCode || '');
    self.MOnthWiseEmployeeGroupReportVm = ko.observableArray(item.MOnthWiseEmployeeGroupReportVm || []);
    //self.EmployeeId = ko.observable(item.EmployeeId || 0);
    //self.EmployeeName = ko.observable(item.EmployeeName || '');
    //self.monthlyWiseReports = ko.observable(item.monthlyWiseReports || []); 
}

function InsuanceReportModel(item) {
    var self = this;
    item = item || {};
    self.EmployeeCode = ko.observable(item.EmployeeCode || "");
    self.EmployeeName = ko.observable(item.EmployeeName || "");
    self.Designation = ko.observable(item.Designation || "");
    self.InsurancePolicyNo = ko.observable(item.InsurancePolicyNo || "");
    self.EmployeeContributionAmount = ko.observable(item.EmployeeContributionAmount || 0);
    self.EmployerContributionAmount = ko.observable(item.EmployerContributionAmount || 0);
    self.TotalDeduction = ko.observable(item.TotalDeduction || 0);
    self.CodeNo = ko.observable(item.CodeNo || "");
    self.SheetRollNo = ko.observable(item.SheetRollNo || "");
    self.Remarks = ko.observable(item.Remarks || "");
}
function ProvidentFundReportModel(item) {
    var self = this;
    item = item || {};
    self.EmployeeCode = ko.observable(item.EmployeeCode || "");
    self.EmployeeName = ko.observable(item.EmployeeName || "");
    self.Designation = ko.observable(item.Designation || "");
    self.ProvidentFundNo = ko.observable(item.ProvidentFundNo || "");
    self.EmployeeContributionAmount = ko.observable(item.EmployeeContributionAmount || 0);
    self.EmployerContributionAmount = ko.observable(item.EmployerContributionAmount || 0);
    self.TotalDeduction = ko.observable(item.TotalDeduction || 0);
    self.CodeNo = ko.observable(item.CodeNo || "");
    self.SheetRollNo = ko.observable(item.SheetRollNo || "");
    self.Remarks = ko.observable(item.Remarks || "");
}

function CITReportModel(item) {
    var self = this;
    item = item || {};
    self.EmployeeCode = ko.observable(item.EmployeeCode || "");
    self.EmployeeName = ko.observable(item.EmployeeName || "");
    self.Designation = ko.observable(item.Designation || "");
    self.CITNo = ko.observable(item.CITNo || "");
    self.TotalDeduction = ko.observable(item.TotalDeduction || 0);
    self.Remarks = ko.observable(item.Remarks || "");
}

function FiscalYearDropdownVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.CurrentFiscalYear = ko.observable(item.CurrentFiscalYear || false);
}

function MonthlyMultiPunchReportModel(item) {
    var self = this;
    item = item || {};
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Date = ko.observable(item.Date || '');
    self.Day = ko.observable(item.Day || '');
    self.PunchTime = ko.observable(item.PunchTime || '');
    self.HolidayId = ko.observable(item.HolidayId || 0);
    self.HoliodayName = ko.observable(item.HoliodayName || '');
    self.LeaveName = ko.observable(item.LeaveName || '');
}

