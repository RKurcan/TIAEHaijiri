using Riddhasoft.Attendance.Entities;
using Riddhasoft.Device.Entities;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Entity.User;
using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Entities.Qualification;
using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.HRM.Entities.Travel;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;


namespace Riddhasoft.DB
{
    public class RiddhaDBContext : DbContext
    {
        public RiddhaDBContext()
            : base("name=RTechDemoContext")
        {

        }
        #region UserManagement
        public DbSet<EUser> User { get; set; }
        public DbSet<ECompanyLogin> CompanyLogin { get; set; }
        public DbSet<EResellerLogin> ResellerLogin { get; set; }
        public DbSet<EUserRole> UserRole { get; set; }
        public DbSet<EContext> Context { get; set; }
        public DbSet<ESessionDetail> SessionDetail { get; set; }
        public DbSet<EOwnerPermission> OwnerPermission { get; set; }
        public DbSet<EAuditTrial> AuditTrial { get; set; }

        #endregion
        #region Menu
        public DbSet<EMenu> Menu { get; set; }
        public DbSet<EUserGroupDataVisibility> UserGroupDataVisibility { get; set; }
        public DbSet<EMenuAction> MenuAction { get; set; }
        public DbSet<EUserGroupMenuRight> UserGroupMenuRight { get; set; }
        public DbSet<EUserGroupActionRight> UserGroupActionRight { get; set; }
        #endregion

        #region permission management
        public DbSet<ERoleOnController> RoleOnController { get; set; }
        public DbSet<ERoleOnModule> RoleOnModule { get; set; }

        public DbSet<ERoleOnControllerAction> RoleOnControllerAction { get; set; }
        #endregion

        #region office Setup
        public DbSet<EDepartmentWiseHoliday> DepartmentWiseHoliday { get; set; }
        public DbSet<ELateNote> LateNote { get; set; }
        public DbSet<ECompany> Company { get; set; }
        public DbSet<ECompanyLicense> CompanyLicense { get; set; }
        public DbSet<ECompanyLicenseLog> CompanyLicenseLog { get; set; }
        public DbSet<EBranch> Branch { get; set; }
        public DbSet<EDepartment> Department { get; set; }
        public DbSet<EShift> Shift { get; set; }
        public DbSet<ELeaveMaster> LeaveMaster { get; set; }
        public DbSet<EDesignationWiseLeavedBalance> DesignationWiseLeavedBalance { get; set; }
        public DbSet<EDesignationWiseLeavedBalanceHist> DesignationWiseLeavedBalanceHist { get; set; }
        public DbSet<EReplacementLeaveBalance> ReplacementLeaveBalance { get; set; }
        public DbSet<EEmployeePresentInOffHist> EmployeePresentInOffHist { get; set; }
        public DbSet<EDesignation> Designation { get; set; }
        public DbSet<ESection> Section { get; set; }
        public DbSet<EHoliday> Holiday { get; set; }
        public DbSet<EHolidayEmployee> HolidayEmployee { get; set; }
        public DbSet<EHolidayDetails> HolidayDetails { get; set; }
        public DbSet<EBank> Bank { get; set; }
        public DbSet<EReseller> Reseller { get; set; }
        public DbSet<EFiscalYear> FiscalYear { get; set; }
        public DbSet<ELeaveSettlement> LeaveSettlement { get; set; }
        public DbSet<ELeaveCarryForwardBalance> LeaveCarryForwardBalance { get; set; }

        public DbSet<EDemoRequest> DemoRequest { get; set; }

        public DbSet<EGradeGroup> GradeGroup { get; set; }
        public DbSet<ENotice> Notice { get; set; }
        public DbSet<ENoticeDetails> NoticeDetail { get; set; }
        #endregion

        #region Employee
        public DbSet<EEmployeeDesignationHistory> EmployeeDesignationHistory { get; set; }
        public DbSet<EDesktopProductKey> DesktopProductKey { get; set; }
        public DbSet<EEmployeeLateInAndEarlyOutRequest> EmployeeLateInAndEarlyOutRequest { get; set; }
        public DbSet<EOfficeVisitRequest> OfficeVisitRequest { get; set; }
        public DbSet<EManualPunchRequest> ManualPunchRequest { get; set; }
        public DbSet<EEmployee> Employee { get; set; }
        public DbSet<EEmployeeLogin> EmployeeLogin { get; set; }
        public DbSet<ELeaveApplication> LeaveApplication { get; set; }
        public DbSet<EleaveApplicationLog> LeaveApplicationLog { get; set; }
        public DbSet<ERoster> Roster { get; set; }
        public DbSet<EDateTable> DateTable { get; set; }
        public DbSet<ELeaveBalance> LeaveBalance { get; set; }

        public DbSet<EManualPunch> ManualPunch { get; set; }
        public DbSet<EOfficeVisit> OfficeVisit { get; set; }
        public DbSet<EOfficeVisitDetail> OfficeVisitDetail { get; set; }
        public DbSet<EKaj> Kaj { get; set; }
        public DbSet<EKajDetail> KajDetail { get; set; }

        public DbSet<EEmployeeShitList> EmployeeShitList { get; set; }
        public DbSet<EEmployeeWOList> EmployeeWOList { get; set; }
        public DbSet<EEvent> Event { get; set; }
        public DbSet<EEventDetails> EventDetail { get; set; }
        public DbSet<EWeeklyRoster> WeeklyRoster { get; set; }

        #endregion

        #region PayRoll
        public DbSet<EPayRollSetup> PayRollSetup { get; set; }
        public DbSet<EPayRollAdditionalAllowance> PayRollAdditionalAllowance { get; set; }
        public DbSet<EAllowance> Allowance { get; set; }
        public DbSet<EEmployeeAlowance> EmployeeAlowance { get; set; }
        public DbSet<EDeduction> Deduction { get; set; }
        public DbSet<EEmployeeDeduction> EmployeeDeduction { get; set; }
        public DbSet<EEmployeeGrade> EmployeeGrade { get; set; }
        public DbSet<EPayrollConfiguration> PayrollConfiguration { get; set; }
        public DbSet<EAllowanceHead> AllowanceHead { get; set; }

        public DbSet<EAdvanceSalary> AdvanceSalary { get; set; }
        public DbSet<ESocialMedialPost> SocialMedialPost { get; set; }


        public DbSet<EMonthlySalarySheetPosting> MonthlySalarySheetPostingMaster { get; set; }

        public DbSet<EMonthlySalarySheetAllowances> MonthlySalarySheetAllowances { get; set; }
        public DbSet<EMonthlySalarySheetDeductions> MonthlySalarySheetDeductions { get; set; }


        public DbSet<EEmployeeSalaryAndTaxPayable> EmployeeSalaryAndTaxPayable { get; set; }





        #endregion

        #region Device
        public DbSet<EModel> Model { get; set; }
        public DbSet<EDevice> Device { get; set; }
        public DbSet<EDeviceAssignment> DeviceAssignment { get; set; }
        public DbSet<ECompanyDeviceAssignment> CompanyDeviceAssignment { get; set; }
        public DbSet<EWdmsConfig> WdmsConfig { get; set; }
        public DbSet<EDevicewiseDepartment> DevicewiseDepartment { get; set; }

        #endregion

        #region Attendance
        public DbSet<Attendance.Entities.EAttendanceLog> AttendanceLog { get; set; }
        #endregion

        #region Mobile
        public DbSet<ENotification> Notification { get; set; }
        public DbSet<ENotificationDetail> NotificationDetail { get; set; }
        #endregion

        #region HRM
        public DbSet<EEmploymentStatusWiseLeavedBalance> EmploymentStatusWiseLeavedBalance { get; set; }
        public DbSet<EEmploymentStatusWiseLeavedBalanceHist> EmploymentStatusWiseLeavedBalanceHist { get; set; }
        public DbSet<EEmployeeOtherDocument> EmployeeOtherDocument { get; set; }
        public DbSet<EContract> Contract { get; set; }
        public DbSet<EResignation> Resignation { get; set; }
        public DbSet<ETermination> Termination { get; set; }
        public DbSet<EEmploymentStatus> EmploymentStatus { get; set; }
        public DbSet<EEmployeeDocument> EmployeeDocument { get; set; }
        public DbSet<ESkills> Skills { get; set; }
        public DbSet<EEducation> Education { get; set; }
        public DbSet<EExperience> Experience { get; set; }
        public DbSet<ELanguage> Language { get; set; }
        public DbSet<ELicense> License { get; set; }
        public DbSet<EMembership> Membership { get; set; }
        public DbSet<EEmployeeEducation> EmployeeEducation { get; set; }
        public DbSet<EEmployeeLanguage> EmployeeLanguage { get; set; }
        public DbSet<EEmployeeLicense> EmployeeLicense { get; set; }
        public DbSet<EEmployeeSkills> EmployeeSkills { get; set; }
        public DbSet<ECourse> Course { get; set; }
        public DbSet<ESession> Session { get; set; }
        public DbSet<EParticipant> Participant { get; set; }
        public DbSet<EParticipantDetail> ParticipantDetail { get; set; }
        public DbSet<ETravelRequest> TravelRequest { get; set; }
        public DbSet<ETravelInformation> TravelInformation { get; set; }
        public DbSet<ETravelEstimate> TravelEstimate { get; set; }
        public DbSet<ECustomField> CustomField { get; set; }

        public DbSet<EDisciplinaryCases> DisciplinaryCases { get; set; }
        public DbSet<EDisciplinaryCasesDetail> DisciplinaryCasesDetail { get; set; }
        public DbSet<EFOCTicket> FOCTicket { get; set; }
        public DbSet<EFOCTicketDetail> FOCTicketDetail { get; set; }

        #endregion

        #region Tax Setup

        public DbSet<ETaxSetup> TaxSetup { get; set; }
        public DbSet<ETaxSlabDetails> TaxSlabDetails { get; set; }



        #endregion

        #region Insurance

        public DbSet<EInsuranceCompany> InsuranceCompany { get; set; }

        public DbSet<EEmployeeInsuranceInformation> EmployeeInsuranceInformation { get; set; }

        #endregion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ECompany>()
           .HasRequired<EReseller>(p => p.Reseller)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<EEvent>()
         .HasRequired<EFiscalYear>(p => p.FiscalYear)
         .WithMany()
         .WillCascadeOnDelete(false);

            modelBuilder.Entity<EResignation>()
           .HasRequired<EEmployee>(p => p.ForwardTo)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<ETermination>()
          .HasRequired<EEmployee>(p => p.ForwardTo)
          .WithMany()
          .WillCascadeOnDelete(false);


            modelBuilder.Entity<ESession>()
        .HasRequired<ECourse>(p => p.Course)
        .WithMany()
        .WillCascadeOnDelete(false);

            modelBuilder.Entity<EDisciplinaryCases>()
          .HasRequired<EEmployee>(p => p.ForwardTo)
          .WithMany()
          .WillCascadeOnDelete(false);

        }
        public virtual List<AttendanceReportResult> SP_GET_ATTENDACE_REPORT(DateTime FromDate, DateTime ToDate, int branchId, string language, string empArray, string branches = "")
        {
            this.Database.CommandTimeout = 0;
            return this.Database.SqlQuery<AttendanceReportResult>("EXEC SP_GET_ATTENDACE_REPORT @BRANCH_ID,@FROMDATE,@TODATE,@LANGUAGE,@EMP_IDS",
                                                new SqlParameter("@BRANCH_ID", branchId),
                                                new SqlParameter("@FROMDATE", FromDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@TODATE", ToDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@LANGUAGE", language),
                                                new SqlParameter("@EMP_IDS", empArray),
                                                new SqlParameter("@SECTION_IDS", ""),
                                                new SqlParameter("@DEPT_IDS", ""),
                                                new SqlParameter("@BRANCH_IDs", branches)
            ).ToList();
            //return //((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AttendanceReportResult>("SP_GET_ATTENDACE_REPORT", branchIdParam, FROMDATEParam, TODATEParam);
        }


        public virtual List<EAttendanceLog> SP_GET_ATTENDACE_LOG_RAW_DATA(int branchId, DateTime FromDate, DateTime ToDate)
        {
            return this.Database.SqlQuery<EAttendanceLog>("EXEC SP_GET_ATTENDACE_LOG_RAW_DATA @BRANCH_ID,@FROM_DATE,@TO_DATE",
                                                new SqlParameter("@BRANCH_ID", branchId),
                                                new SqlParameter("@FROM_DATE", FromDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@TO_DATE", ToDate.ToString("yyyy/MM/dd"))
            ).ToList();

        }
        public virtual List<AttendanceReportResult> SP_GET_ATTENDACE_REPORT_BY_EMPLOYEE_ID(DateTime FromDate, DateTime ToDate, int EmployeeId)
        {
            this.Database.CommandTimeout = 0;
            return this.Database.SqlQuery<AttendanceReportResult>("EXEC SP_GET_ATTENDACE_REPORT_BY_EMPLOYEE_ID @EMPLOYEE_ID,@FROMDATE,@TODATE",
                                                new SqlParameter("@EMPLOYEE_ID", EmployeeId),
                                                new SqlParameter("@FROMDATE", FromDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@TODATE", ToDate.ToString("yyyy/MM/dd"))
            ).ToList();

            //return //((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AttendanceReportResult>("SP_GET_ATTENDACE_REPORT", branchIdParam, FROMDATEParam, TODATEParam);
        }
        public virtual List<EMNotification> SP_GET_NOTIFICATION_BY_EMP_ID(DateTime FromDate, DateTime ToDate, int EmployeeId)
        {
            return this.Database.SqlQuery<EMNotification>("EXEC SP_GET_NOTIFICATION_BY_EMP_ID @EMPLOYEE_ID,@FROMDATE,@TODATE",
                                                new SqlParameter("@EMPLOYEE_ID", EmployeeId),
                                                new SqlParameter("@FROMDATE", FromDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@TODATE", ToDate.ToString("yyyy/MM/dd"))
            ).ToList();

            //return //((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AttendanceReportResult>("SP_GET_ATTENDACE_REPORT", branchIdParam, FROMDATEParam, TODATEParam);
        }
        public virtual List<EMRoster> SP_GET_Roster_REPORT(DateTime FromDate, DateTime ToDate, int EmployeeId)
        {
            return this.Database.SqlQuery<EMRoster>("EXEC SP_GET_Roster_REPORT @EMPLOYEE_ID,@FROMDATE,@TODATE",
                                                new SqlParameter("@EMPLOYEE_ID", EmployeeId),
                                                new SqlParameter("@FROMDATE", FromDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@TODATE", ToDate.ToString("yyyy/MM/dd"))
            ).ToList();

            //return //((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AttendanceReportResult>("SP_GET_ATTENDACE_REPORT", branchIdParam, FROMDATEParam, TODATEParam);
        }
        public virtual List<EMLeaveInfo> SP_GET_LEAVEINFORMATION(int EmployeeId)
        {
            return this.Database.SqlQuery<EMLeaveInfo>("EXEC SP_GET_LEAVEINFORMATION @EMPLOYEE_ID",
                                                new SqlParameter("@EMPLOYEE_ID", EmployeeId)
            ).ToList();

            //return //((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AttendanceReportResult>("SP_GET_ATTENDACE_REPORT", branchIdParam, FROMDATEParam, TODATEParam);
        }
        public virtual List<EEmployeeLeaveSummary> SP_GET_LEAVE_SUMMARY(DateTime FromDate, DateTime ToDate, int branchId, string language, int FISCALYEAR)
        {
            return this.Database.SqlQuery<EEmployeeLeaveSummary>("EXEC SP_GET_LEAVE_SUMMARY @BRANCH_ID,@FROMDATE,@TODATE,@LANGUAGE,@FISCALYEAR",
                                                new SqlParameter("@BRANCH_ID", branchId),
                                                new SqlParameter("@FROMDATE", FromDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@TODATE", ToDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@LANGUAGE", language),
                                                new SqlParameter("@FISCALYEAR", FISCALYEAR)
            ).ToList();
        }

        public virtual List<AttendanceReportResult> SP_GET_EMPLOYEEWISE_ROSTER_REPORT(DateTime FromDate, DateTime ToDate, int branchId, string language, string empIds, string sectionIds, string deptIds)
        {
            return this.Database.SqlQuery<AttendanceReportResult>("EXEC SP_GET_EMPLOYEEWISE_ROSTER_REPORT @BRANCH_ID,@FROMDATE,@TODATE,@LANGUAGE,@EMP_IDS,@SECTION_IDS, @DEPT_IDS",
                                                new SqlParameter("@BRANCH_ID", branchId),
                                                new SqlParameter("@FROMDATE", FromDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@TODATE", ToDate.ToString("yyyy/MM/dd")),
                                                new SqlParameter("@LANGUAGE", language),
                                                new SqlParameter("@EMP_IDS", empIds == null ? String.Empty : empIds),
                                                new SqlParameter("@SECTION_IDS", sectionIds == null ? String.Empty : sectionIds),
                                                new SqlParameter("@DEPT_IDS", deptIds == null ? String.Empty : deptIds)
            ).ToList();
        }

        public virtual List<EmployeeDropdownViewModel> SP_GET_Employee_By_Sections(string sections, int activeInactiveMode)
        {
            return this.Database.SqlQuery<EmployeeDropdownViewModel>("EXEC SP_GET_Employee_By_Sections @ExpenseHeadIds,@FROM_DATE,@TODATE",
                                                new SqlParameter("@activeInactiveMode", activeInactiveMode),
                                                new SqlParameter("@sections", sections)
            ).ToList();
        }


        public virtual List<EMEmployeesPersonalInfo> SP_GET_EmployeePersonalInfo_By_Id(string EmpIds)
        {
            try
            {
                return this.Database.SqlQuery<EMEmployeesPersonalInfo>("EXEC SP_GET_EmployeeInfo_REPORT @EMP_IDS",
                                          new SqlParameter("@EMP_IDS", EmpIds)
                ).ToList();
            }
            catch (Exception ex)
            {

                return new List<EMEmployeesPersonalInfo>();
            }
           

        }

        public virtual List<ExpiredCompanyVm> SP_GET_Month_Wise_Expired_Company(DateTime fromDate, DateTime toDate)
        {
            return this.Database.SqlQuery<ExpiredCompanyVm>("EXEC SP_GET_Month_Wise_Expired_Company @FROMDATE,@TODATE",
                                           new SqlParameter("@FROMDATE", fromDate),
                                           new SqlParameter("@TODATE", toDate)
                 ).ToList();

        }

        public virtual List<EmployeeSalaryHistVm> SP_Employee_Salary_History(int fiscalYearId, string empIds)
        {
            return this.Database.SqlQuery<EmployeeSalaryHistVm>("EXEC SP_Employee_Salary_History @FISCALYEAR_ID,@EMP_IDS",
                                           new SqlParameter("@FISCALYEAR_ID", fiscalYearId),
                                           new SqlParameter("@EMP_IDS", empIds)
                 ).ToList();

        }

        public virtual List<EmployeeMultiPunchReportSPVm> SP_Employee_MultiPunch_report(int branchId, DateTime fromDate, DateTime toDate, string language)
        {
            return this.Database.SqlQuery<EmployeeMultiPunchReportSPVm>("EXEC SP_GET_MultiPunch_Report @BRANCH_ID,@FROM_DATE,@TO_DATE,@Language",
                                           new SqlParameter("@BRANCH_ID", branchId),
                                           new SqlParameter("@FROM_DATE", fromDate),
                                           new SqlParameter("@TO_DATE", toDate),
                                           new SqlParameter("@Language", language)
                 ).ToList();

        }
        public virtual List<PostAttendanceLogResult> SP_Post_AttendanceData(string branchCode,int userPin,int verifyMode,DateTime verifyTime,decimal temperature,string deviceSN)
        {
            return this.Database.SqlQuery<PostAttendanceLogResult>("EXEC SP_Post_AttendanceData @BRANCH_CODE,@USER_PIN,@VERIFY_MODE,@VERIFY_TIME,@TEMPERATURE,@DEVICE_SN",
                                           new SqlParameter("@BRANCH_CODE", branchCode),
                                           new SqlParameter("@USER_PIN", userPin),
                                           new SqlParameter("@VERIFY_MODE", verifyMode),
                                           new SqlParameter("@VERIFY_TIME", verifyTime),
                                           new SqlParameter("@TEMPERATURE", temperature),
                                            new SqlParameter("@DEVICE_SN", deviceSN)
                 ).ToList();

        }
    }
    public class PostAttendanceLogResult
    {
        public bool hasError { get; set; }
        public string message { get; set; }
    }
    public class AttendanceReportResult
    {

        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string SectionNameNp { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameNp { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public int EmployeeDeviceCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameNp { get; set; }
        public string MobileNo { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public bool NoPunch { get; set; }
        public bool SinglePunch { get; set; }
        public bool TwoPunch { get; set; }
        public bool FourPunch { get; set; }
        public bool MultiplePunch { get; set; }
        public TimeSpan? PlannedTimeIn { get; set; }
        public TimeSpan? EarlyGrace { get; set; }
        public TimeSpan? LateGrace { get; set; }
        public TimeSpan? PlannedTimeOut { get; set; }
        public TimeSpan? PLANNEDLUNCHSTART { get; set; }
        public TimeSpan? PLANNEDLUNCHEND { get; set; }
        public DateTime EngDate { get; set; }
        public string NepDate { get; set; }
        public ShiftType ShiftType { get; set; }
        public int ShiftTypeId { get; set; }
        public DateTime? PUNCHDATETIME { get; set; }
        public string HOLIDAYNAME { get; set; }
        public DateTime? HOLIDAYSTARTDATE { get; set; }
        public DateTime? HOLIDAYENDDATE { get; set; }
        public string LEAVENAME { get; set; }
        public string OFFICEVISIT { get; set; }
        public string KAJ { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? leaveEndDate { get; set; }
        public DateTime? PUNCHIN { get; set; }
        public DateTime? PUNCHOUT { get; set; }
        public DateTime? BREAKIN { get; set; }
        public DateTime? BREAKOUT { get; set; }
        public string WEEKEND { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string LeaveDescription { get; set; }
    }
    public class EEmployeeLeaveSummary
    {
        public int EmployeeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public int LeaveId { get; set; }
        public string LeaveName { get; set; }
        public string LeaveNameNp { get; set; }
        public decimal Balance { get; set; }
        public decimal TakenLeave { get; set; }
        public decimal RemLeave { get; set; }
    }
    public class EmployeeDropdownViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ExpiredCompanyVm
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContactPerson { get; set; }
        public string CompanyContactNo { get; set; }
        public DateTime CompanyExpiryDate { get; set; }
        public string ExpiryDate { get; set; }
        public string ResellerName { get; set; }
        public string ResellerAddress { get; set; }
        public string ResellerContactPerson { get; set; }
        public string ResellerContactNo { get; set; }
        public int ResellerId { get; set; }
    }

    public class EmployeeSalaryHistVm
    {
        public string Employee { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public decimal GradeValue { get; set; }
        public string GradeName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TaxableAmont { get; set; }
        public decimal SocialSecurityTax { get; set; }
        public decimal RenumerationTax { get; set; }
        public decimal PFEmployee { get; set; }
        public decimal PFEmployeer { get; set; }
        public decimal Gratituity { get; set; }
        public decimal SSEmployee { get; set; }
        public decimal SSEmployeer { get; set; }
        public decimal InsurancePremiumAmount { get; set; }
        public decimal CITAmount { get; set; }
        public string Absent { get; set; }
        public string Leave { get; set; }
        public string LateIn { get; set; }
        public string EarlyOut { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal RebateAmount { get; set; }
        public decimal NetSalary { get; set; }
        public decimal PensionFundEmployee { get; set; }
        public decimal PensionFundEmployeer { get; set; }
        public decimal InsurancePaidbyOffice { get; set; }
        public decimal AdditionAmount { get; set; }

    }

    public class EmployeeMultiPunchReportSPVm
    {
        public int EmployeeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public string PunchTime { get; set; }

     
        public int? HolidayId { get; set; }
        public string HoliodayName { get; set; }
        public string LeaveName { get; set; }
    }

}
