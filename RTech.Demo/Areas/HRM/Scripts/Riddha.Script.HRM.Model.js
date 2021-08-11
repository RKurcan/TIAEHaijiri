/// <reference path="Riddha.Script.HRM.Controller.js" />
function ContractModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BeganOn = ko.observable(item.BeganOn || '').extend({ date: '' });
    self.EndedOn = ko.observable(item.EndedOn || '').extend({ date: '' });
    self.EmploymentStatusId = ko.observable(item.EmploymentStatusId || 0);
    self.EmploymentStatusName = ko.observable(item.EmploymentStatusName || '');
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '').extend({ date: '' });
    self.FileUrl = ko.observable(item.FileUrl || '');
}

function ResignationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.NoticeDate = ko.observable(item.NoticeDate || '').extend({ date: '' });
    self.DesiredResignDate = ko.observable(item.DesiredResignDate || '').extend({ date: '' });
    self.Reason = ko.observable(item.Reason || '');
    self.Details = ko.observable(item.Details || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.ForwardToId = ko.observable(item.ForwardToId || 0);
    self.FordwardToName = ko.observable(item.FordwardToName || '');
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '').extend({ date: '' });
    self.FileUrl = ko.observable(item.FileUrl || '');
}

function TerminationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.NoticeDate = ko.observable(item.NoticeDate || '').extend({ date: '' });
    self.ServiceEndDate = ko.observable(item.ServiceEndDate || '').extend({ date: '' });
    self.Reason = ko.observable(item.Reason || '');
    self.Details = ko.observable(item.Details || '');
    self.ChangeStatus = ko.observable(item.ChangeStatus || 0);
    self.ForwardToId = ko.observable(item.ForwardToId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.FordwardToName = ko.observable(item.FordwardToName || '');
    self.ChangeStatusName = ko.observable(item.ChangeStatusName || '');
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '').extend({ date: '' });
    self.FileUrl = ko.observable(item.FileUrl || '');
}

function EmployeeGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.DepartmentId = ko.observable(item.DepartmentId || 0);
    self.Section = ko.observable(item.Section || '');
    self.SectionId = ko.observable(item.SectionId || 0);
    self.DesignationName = ko.observable(item.DesignationName || '');
    self.DesignationId = ko.observable(item.DesignationId || 0);
    self.PhotoURL = ko.observable(item.PhotoURL || '');
    self.IdCardNo = ko.observable(item.PhotoURL || '');
}

function EmploymentStatusModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.IsContract = ko.observable(item.IsContract || false);
    self.EmploymentStatus = ko.observable(item.EmploymentStatus || 0);
    self.EmploymentStatusName = ko.observable(item.EmploymentStatusName || '');
    self.Description = ko.observable(item.Description || '');
    self.BranchId = ko.observable(item.BranchId || null);
}

function EmploymentStatusWiseLeavedBalanceModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.LeaveId = ko.observable(item.LeaveId || 0);
    self.EmploymentStatusId = ko.observable(item.EmploymentStatusId || 0);
    self.Balance = ko.observable(item.Balance || 0);
    self.Name = ko.observable(item.Name || '');
    self.NameNp = ko.observable(item.NameNp || '');
    self.MaxLimit = ko.observable(item.MaxLimit || 0);
    self.IsPaidLeave = ko.observable(item.IsPaidLeave || false);
    self.IsLeaveCarryable = ko.observable(item.IsLeaveCarryable || false);
    self.ApplicableGender = ko.observable(item.ApplicableGender || 0);
    self.IsMapped = ko.observable(item.IsMapped || false);
    self.IsReplacementLeave = ko.observable(item.IsReplacementLeave || false);
    self.LeaveIncreamentPeriod = ko.observable(item.LeaveIncreamentPeriod || 0);
}

function ContractVerificationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Code = ko.observable(item.Code || '');
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmploymentStatusName = ko.observable(item.EmploymentStatusName || '');
    self.Period = ko.observable(item.Period || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedBy = ko.observable(item.ApprovedBy || '');
    self.totalunApproveCount = ko.observable(item.totalunApproveCount || 0);
}

function ResignationVerificationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.NoticeDate = ko.observable(item.NoticeDate || '').extend({ date: '' });
    self.DesiredResignDate = ko.observable(item.DesiredResignDate || '');
    self.Reason = ko.observable(item.Reason || '');
    self.Details = ko.observable(item.Details || '');
    self.ForwardToName = ko.observable(item.ForwardToName || '');
    self.ForwardToId = ko.observable(item.ForwardToId || 0);
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedBy = ko.observable(item.ApprovedBy || '');
}

function TerminationVerificationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.NoticeDate = ko.observable(item.NoticeDate || '').extend({ date: '' });
    self.ServiceEndDate = ko.observable(item.ServiceEndDate || '');
    self.Reason = ko.observable(item.Reason || '');
    self.Details = ko.observable(item.Details || '');
    self.ForwardToName = ko.observable(item.ForwardToName || '');
    self.ForwardToId = ko.observable(item.ForwardToId || 0);
    self.ChangeStatusName = ko.observable(item.ChangeStatusName || '');
    self.ChangeStatus = ko.observable(item.ChangeStatus || 0);
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedBy = ko.observable(item.ApprovedBy || '');
}

function EducationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
    self.BranchId = ko.observable(item.BranchId || null);
}

function ExperienceModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Title = ko.observable(item.Title || '');
    self.Description = ko.observable(item.Description || '');
    self.OrganizationName = ko.observable(item.OrganizationName || '');
    self.BeganOn = ko.observable(item.BeganOn || '').extend({ date: '' });
    self.EndedOn = ko.observable(item.EndedOn || '').extend({ date: '' });
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BranchId = ko.observable(item.BranchId || null);
}

function LanguageModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
    self.BranchId = ko.observable(item.BranchId || null);
}

function LicenseModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
    self.BranchId = ko.observable(item.BranchId || null);
}

function MembershipModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BranchId = ko.observable(item.BranchId || null);
}

function SkillModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
    self.BranchId = ko.observable(item.BranchId || null);
}

function EmployeeEducationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EducationId = ko.observable(item.EducationId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function EducationSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
}

function EmployeeEducationGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.EducationCode = ko.observable(item.EducationCode || '');
    self.EducationDescription = ko.observable(item.EducationDescription || '');
    self.EducationId = ko.observable(item.EducationId || 0);
    self.EducationName = ko.observable(item.EducationName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function EmployeeSkillModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.SkillsId = ko.observable(item.SkillsId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function EmployeeSkillSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
}

function EmployeeSkillGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.SkillCode = ko.observable(item.SkillCode || '');
    self.SkillDescription = ko.observable(item.SkillDescription || '');
    self.SkillId = ko.observable(item.SkillId || 0);
    self.SkillName = ko.observable(item.SkillName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function EmployeeLicenseModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.LicenseId = ko.observable(item.LicenseId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function EmployeeLicenseSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function EmployeeLicenseGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.LicenseCode = ko.observable(item.LicenseCode || '');
    self.LicenseDescription = ko.observable(item.LicenseDescription || '');
    self.LicenseId = ko.observable(item.LicenseId || 0);
    self.LicenseName = ko.observable(item.LicenseName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function EmployeeLanguageModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.LanguageId = ko.observable(item.LanguageId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function EmployeeLanguageSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
}

function EmployeeLanguageGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.LanguageCode = ko.observable(item.LanguageCode || '');
    self.LanguageDescription = ko.observable(item.LanguageDescription || '');
    self.LanguageId = ko.observable(item.LanguageId || 0);
    self.LanguageName = ko.observable(item.LanguageName || '');
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '');
}

function QualificationModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedBy = ko.observable(item.ApprovedBy || '');
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
    self.totalRowNum = ko.observable(item.totalRowNum || 0);
}

function MembershipVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
    self.Description = ko.observable(item.Description || '');
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedBy = ko.observable(item.ApprovedBy || '');
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
}

function ExperienceVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.EmployeeCode = ko.observable(item.EmployeeCode || '');
    self.EmployeeName = ko.observable(item.EmployeeName || '');
    self.Code = ko.observable(item.Code || '');
    self.Title = ko.observable(item.Title || '');
    self.Description = ko.observable(item.Description || '');
    self.OrganizationName = ko.observable(item.OrganizationName || '');
    self.BeganOn = ko.observable(item.BeganOn || '').extend({ date: '' });
    self.EndedOn = ko.observable(item.EndedOn || '').extend({ date: '' });
    self.ApprovedById = ko.observable(item.ApprovedById || null);
    self.ApprovedBy = ko.observable(item.ApprovedBy || '');
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
}

function UnapprovedCountModel(item) {
    var self = this;
    item = item || {};
    self.Contract = ko.observable(item.Contract || 0);
    self.Resignation = ko.observable(item.Resignation || 0);
    self.Termination = ko.observable(item.Termination || 0);
    self.EmployeeEducation = ko.observable(item.EmployeeEducation || 0);
    self.EmployeeSkill = ko.observable(item.EmployeeSkill || 0);
    self.EmployeeExperience = ko.observable(item.EmployeeExperience || 0);
    self.EmployeeLicense = ko.observable(item.EmployeeLicense || 0);
    self.EmployeeMembership = ko.observable(item.EmployeeMembership || 0);
    self.EmployeeLanguage = ko.observable(item.EmployeeLanguage || 0);
}

function CourseModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.Title = ko.observable(item.Title || '');
    self.DepartmentId = ko.observable(item.DepartmentId || 0);
    self.Version = ko.observable(item.Version || 0);
    self.SubVersion = ko.observable(item.SubVersion || 0);
    self.Currency = ko.observable(item.Currency || 0);
    self.Cost = ko.observable(item.Cost || 0);
    self.Description = ko.observable(item.Description || '');
    self.CoordinatorId = ko.observable(item.CoordinatorId || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.CoordinatorName = ko.observable(item.CoordinatorName || '');
}

function DepartmentDropdownModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
}

function EmployeeSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Code = ko.observable(item.Code || '');
    self.Name = ko.observable(item.Name || '');
}

function CourseGridVm(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CoordinatorId = ko.observable(item.CoordinatorId || 0);
    self.CoordinatorName = ko.observable(item.CoordinatorName || '');
    self.Cost = ko.observable(item.Cost || 0);
    self.Currency = ko.observable(item.Currency || 0);
    self.DepartmentId = ko.observable(item.DepartmentId || 0);
    self.DepartmentName = ko.observable(item.DepartmentName || '');
    self.Description = ko.observable(item.Description || '');
    self.Version = ko.observable(item.Version || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.SubVersion = ko.observable(item.SubVersion || 0);
    self.Title = ko.observable(item.Title || '');
}

function SessionModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
    self.CourseId = ko.observable(item.CourseId || 0);
    self.Duration = ko.observable(item.Duration || '');
    self.Location = ko.observable(item.Location || '');
    self.Method = ko.observable(item.Method || 0);
    self.Description = ko.observable(item.Description || '');
    self.CourseName = ko.observable(item.CourseName || '');
    self.BranchId = ko.observable(item.BranchId || 0);
}

function CourseSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Title = ko.observable(item.Title || '');
}

function SessionSearchViewModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.Name = ko.observable(item.Name || '');
}

function ParticipantModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.SessionId = ko.observable(item.SessionId || 0);
    self.SessionName = ko.observable(item.SessionName || '');
    self.CourseName = ko.observable(item.CourseName || '');
    self.StartDate = ko.observable(item.StartDate || '').extend({ date: '' });
    self.EndDate = ko.observable(item.EndDate || '').extend({ date: '' });
    self.ParticipantStatus = ko.observable(item.ParticipantStatus || '');
    self.BranchId = ko.observable(item.BranchId || 0);
    self.CreatedById = ko.observable(item.CreatedById || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '').extend({ date: '' });
    self.ApprovedById = ko.observable(item.ApprovedById || 0);
    self.ApprovedOn = ko.observable(item.ApprovedOn || '').extend({ date: '' });
    self.IsApproved = ko.observable(item.IsApproved || false);
}

function ParticipantDetailModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.ParticipantId = ko.observable(item.ParticipantId || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
}

function CustomFieldModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.FieldName = ko.observable(item.FieldName || '');
    self.Length = ko.observable(item.Length || 0);
    self.FieldType = ko.observable(item.FieldType || 0);
    self.CompanyId = ko.observable(item.CompanyId || 0);
}

function DisciplinaryCaseModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.CaseName = ko.observable(item.CaseName || '');
    self.Description = ko.observable(item.Description || '');
    self.CreatedBy = ko.observable(item.CreatedBy || 0);
    self.CreatedOn = ko.observable(item.CreatedOn || '').extend({ date: '' });
    self.DisciplinaryStatus = ko.observable(item.DisciplinaryStatus || 0);
    self.DisciplinaryActions = ko.observable(item.DisciplinaryActions || 0);
    self.BranchId = ko.observable(item.BranchId || 0);
    self.ForwardToId = ko.observable(item.ForwardToId || 0);
}
function EmpDocModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.EmployeeId = ko.observable(item.EmployeeId || 0);
    self.PFNo = ko.observable(item.PFNo || '');
    self.CITNo = ko.observable(item.CITNo || '');
    self.BankACNo = ko.observable(item.BankACNo || '');
    self.PanNo = ko.observable(item.PanNo || '');
    self.PFFileUrl = ko.observable(item.PFFileUrl || '');
    self.CITFileUrl = ko.observable(item.CITFileUrl || '');
    self.AppointmentFileUrl = ko.observable(item.AppointmentFileUrl || '');
    self.ContractFileUrl = ko.observable(item.ContractFileUrl || '');
}

function EmployeeOtherDocumentModel(item) {
    var self = this;
    item = item || {};
    self.Id = ko.observable(item.Id || 0);
    self.FileName = ko.observable(item.FileName || '');
    self.FileUrl = ko.observable(item.FileUrl || '');
    self.EmployeeId = ko.observable(item.EmployeeId  || 0);
}