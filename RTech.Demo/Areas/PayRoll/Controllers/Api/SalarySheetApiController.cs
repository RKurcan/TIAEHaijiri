using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Areas.PayRoll.ViewModels;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class SalarySheetApiController : ApiController
    {


        SAllowance _allowanceServices = null;
        SDeduction _deductionServices = null;
        SPayRollSetup _payRollSetupServices = null;
        SFiscalYear _fiscalYearServices = null;
        STaxSetup _taxSetupServices = null;
        SEmployee _employeeServices = null;

        LocalizedString _loc = null;
        SSalarySheet _salarySheetServices = null;
        SEmployeeInsuranceInformation _employeeInsuranceInformationServices = null;

        int BranchId = (int)RiddhaSession.BranchId;
        string curLang = RiddhaSession.Language;
        SPayrollConfiguration _payrollConfigurationServices = null;
        OrganizationType organizationType;

        SCompany _companyServices = null;
        public SalarySheetApiController()
        {
            _allowanceServices = new SAllowance();
            _deductionServices = new SDeduction();
            _payRollSetupServices = new SPayRollSetup();
            _fiscalYearServices = new SFiscalYear();
            _taxSetupServices = new STaxSetup();
            _employeeServices = new SEmployee();
            _salarySheetServices = new SSalarySheet();
            _loc = new LocalizedString();
            _employeeInsuranceInformationServices = new SEmployeeInsuranceInformation();
            _payrollConfigurationServices = new SPayrollConfiguration();
            _companyServices = new SCompany();
        }
        [HttpGet]
        public ServiceResult<List<MonthlySalarySheetVM>> GetMonthlySalarySheets(string OnDate, string ToDate, string DepartmentIds, string SectionIds, string EmpIds, int MonthId)
        {

            if (GetTaxSetupInfo() == null)
            {

                return new ServiceResult<List<MonthlySalarySheetVM>>()
                {
                    Data = null,
                    Message = _loc.Localize("Tax information has not been setup for current fiscal year..."),
                    Status = ResultStatus.processError
                };
            }



            organizationType = _companyServices.List().Data.Where(x => x.Id == RiddhaSession.CompanyId).Select(x => x.OrganizationType).FirstOrDefault();

            var result = new List<MonthlySalarySheetVM>();
            //var payrolls = _payRollSetupServices.List().Data.Where(x => x.BranchId == BranchId 
            //&& x.IsApproved == true && 
            //(x.Employee.EmploymentStatus != EmploymentStatus.Resigned 
            //&& x.Employee.EmploymentStatus != EmploymentStatus.Terminated));
            DateTime startDate = OnDate.ToDateTime();
            DateTime endDate = ToDate.ToDateTime();
            int daysInMonth = (endDate - startDate).Days + 1;
            var payrolls = GetEmployeePayrolls(BranchId, startDate, endDate);

            if (DepartmentIds != null && DepartmentIds.Length > 0)
            {

                int[] depIds = DepartmentIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                payrolls = from c in payrolls
                           join d in depIds on c.Employee.Section.DepartmentId equals d
                           select c;
            }
            if (SectionIds != null && SectionIds.Length > 0)
            {

                int[] secIds = SectionIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                payrolls = from c in payrolls
                           join d in secIds on c.Employee.SectionId equals d
                           select c;
            }
            if (EmpIds != null && EmpIds.Length > 0)
            {

                int[] empIds = EmpIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                payrolls = from c in payrolls
                           join d in empIds on c.EmployeeId equals d
                           select c;
            }

            var CurrentFiscalYear = _fiscalYearServices.List().Data.Where(x => x.BranchId == BranchId && x.CurrentFiscalYear == true).FirstOrDefault();

            int CurrentFiscalYearId = CurrentFiscalYear.Id;
            var ExistingSalarySheet = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId)
                                       where c.FiscalYearId == CurrentFiscalYearId && c.Month == MonthId
                                       select c).ToList();


            //payrolls = from c in payrolls
            //           where c.EffectedFrom >= startDate
            //           select c;
            //Renumeration Tax has been used as the ref
            //decimal RenumerationTax = 0;
            result = (from payroll in payrolls.ToList()
                      join salarySheet in ExistingSalarySheet on payroll.EmployeeId equals salarySheet.EmployeeId into joined
                      from salarySheet in joined.DefaultIfEmpty()
                      join insurance in _employeeInsuranceInformationServices.List().Data.Where(x => x.IssueDate <= startDate
                                                    && x.ExpiryDate >= endDate)
                      on payroll.EmployeeId equals insurance.EmployeeId into insuranceJoined
                      from insurance in insuranceJoined.DefaultIfEmpty()
                      select new MonthlySalarySheetVM()
                      {
                          Id = salarySheet == null ? 0 : salarySheet.Id,
                          EmployeeId = payroll.EmployeeId,
                          EmployeeName = payroll.Employee.Name,
                          EmployeeCode = payroll.Employee.Code,
                          SSFEffectiveFromDate = payroll.SSFEffectedFromDate,
                          JoinedMonth = payroll.Employee.DateOfJoin.HasValue == true ? payroll.Employee.DateOfJoin.GetValueOrDefault().ToString("yyyy/MM/dd") : "",
                          EmploymentStatus = payroll.Employee.EmploymentStatus,
                          DepartmentName = payroll.Employee.Section == null ? "" : payroll.Employee.Section.Department.Name,
                          GenderEnum = payroll.Employee.Gender == Gender.Female ? Gender.Female : Gender.Male,
                          Gender = payroll.Employee.Gender == Gender.Male ? "M" : "F",
                          MaritalStatusEnum = payroll.Employee.MaritialStatus == MaritialStatus.Married ? MaritialStatus.Married : MaritialStatus.UnMarried,
                          MaritalStatus = payroll.Employee.MaritialStatus == MaritialStatus.Married ? "C" : "S",
                          BasicSalary = payroll.BasicSalary,
                          salarySheethasBeenCreated = salarySheet == null ? false : true,
                          Grade = salarySheet == null ? 0 : salarySheet.Grade,
                          GradeName = salarySheet == null ? "" : salarySheet.GradeName,
                          PFEE = salarySheet == null ? 0 : salarySheet.PFEE,
                          PFER = salarySheet == null ? 0 : salarySheet.PFER,
                          Gratituity = salarySheet == null ? 0 : salarySheet.Gratituity,
                          SSEE = salarySheet == null ? 0 : salarySheet.SSEE,
                          SSER = salarySheet == null ? 0 : salarySheet.SSER,
                          PensionFundEE = salarySheet == null ? 0 : salarySheet.PensionFundEE,
                          PensionFundER = salarySheet == null ? 0 : salarySheet.PensionFundER,
                          CITRate = payroll.CITRate,
                          CITAmount = salarySheet == null ? 0 : salarySheet.CITAmount,
                          TaxableAmount = salarySheet == null ? 0 : salarySheet.TaxableAmount,
                          SocialSecurityTax = salarySheet == null ? 0 : salarySheet.SocialSecurityTax,
                          RenumerationTax = salarySheet == null ? 0 : salarySheet.RenumerationTax,

                          InsurancePremiumAmount = insurance == null ? 0 : insurance.PremiumAmount,
                          InsurancePaidbyOffice = salarySheet == null ? 0 : salarySheet.InsurancePaidbyOffice,
                          DeductionAmount = salarySheet == null ? 0 : salarySheet.DeductionAmount,
                          IsApproved = salarySheet == null ? false : salarySheet.IsApproved,
                          NetSalary = salarySheet == null ? 0 : salarySheet.NetSalary,
                          SheetOfFromDate = startDate,
                          SheetOfToDate = endDate,
                          OrganizationType = organizationType,
                          AdditionAmount = salarySheet == null ? 0 : salarySheet.AdditionAmount,
                          AbsentDeductionAmount = salarySheet == null ? 0 : salarySheet.AbsentDeductionAmount,
                          EarlyOutDeductionAmount = salarySheet == null ? 0 : salarySheet.EarlyOutDeductionAmount,
                          LateDeductionAmount = salarySheet == null ? 0 : salarySheet.LateDeductionAmount,
                          OTHours  = salarySheet == null? "" : salarySheet.OtHours,
                          OTAmount  = salarySheet == null ? 0 : salarySheet.OtAmount,

                      }).OrderBy(x => x.EmployeeName).ToList();

            string Message = "";
            if (result.Where(x => x.IsApproved == true).Count() > 0)
            {

                Message = _loc.Localize("Some of Employee salary posting has already been approved ,you cannot make edition to those");

            }
            SMonthlyWiseReport reportService = new SMonthlyWiseReport(RiddhaSession.Language);
            reportService.FilteredEmployeeIDs = new int[0] { };
            var attendancedata = reportService.GetAttendanceReportFromSp(OnDate.ToDateTime(), ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt()).Data;

            decimal EmpryInsuranceContributionPerc = _payrollConfigurationServices.List().Data.Where(x => x.BranchId == BranchId)
                                                       .Select(x => x.InsuranceContributionByEmpyr).FirstOrDefault();
            foreach (var item in result)
            {
                if (!item.salarySheethasBeenCreated)
                {
                    item.GrossSalary = item.BasicSalary + item.Grade;
                    item.Allowances = GetEmpAllowances(item.EmployeeId, ToDate, item.BasicSalary, item.GrossSalary);

                    var gradeInfo = GetGradeInfo(item.EmployeeId, ToDate);
                    item.Grade = gradeInfo.Value;
                    item.GradeName = RiddhaSession.Language == "en" ? gradeInfo.Name : gradeInfo.NameNp;
                    var SSFInfo = PrepareSSFFundModel(organizationType, item.BasicSalary,
                        item.Grade, item.EmploymentStatus, item.SSFEffectiveFromDate, startDate);

                    item.PFEE = SSFInfo.PFEE;
                    item.PFER = SSFInfo.PFER;
                    item.Gratituity = SSFInfo.Gratituity;
                    item.SSEE = SSFInfo.SSEE;
                    item.SSER = SSFInfo.SSER;
                    item.PensionFundEE = SSFInfo.PensionFundEE;
                    item.PensionFundER = SSFInfo.PensionFundER;
                    //if (organizationType == OrganizationType.Government)
                    //{

                    //    // If organization type is government the SSF will be calculated from
                    //    // Basic salary + Grade
                    //    SSFInfo = GetSSFInformation(item.BasicSalary + item.Grade);
                    //}
                    //else
                    //{
                    //    SSFInfo = GetSSFInformation(item.BasicSalary);
                    //}
                    //if (organizationType == OrganizationType.Government 
                    //    && item.EmploymentStatus == EmploymentStatus.OnContract)
                    //{
                    //    item.PFEE = 0;
                    //    item.PFER = 0;
                    //    item.Gratituity = 0;
                    //    item.SSEE = 0;
                    //    item.SSER = 0;
                    //    item.PensionFundEE = 0;
                    //    item.PensionFundER = 0;
                    //}
                    //else
                    //{
                    //    item.PFEE = Math.Round( SSFInfo.PFEE , 2);
                    //    item.PFER = Math.Round(SSFInfo.PFER , 2 );
                    //    item.Gratituity = Math.Round(SSFInfo.Gratituity , 2);
                    //    item.SSEE = Math.Round(SSFInfo.SSEE , 2);
                    //    item.SSER = Math.Round(SSFInfo.SSER , 2);
                    //    item.PensionFundEE = Math.Round( SSFInfo.PensionFundEE , 2);
                    //    item.PensionFundER = Math.Round( SSFInfo.PensionFundER , 2);
                    //}


                    item.GrossSalary = item.Allowances.Sum(x => x.AllowanceAmount) + item.BasicSalary + item.Grade;

                    item.CITAmount = Math.Round(((item.CITRate / 100) * item.BasicSalary), 2);

                    item.InsurancePaidbyOffice = item.InsurancePremiumAmount * (EmpryInsuranceContributionPerc / 100);
                    if (organizationType == OrganizationType.Government)
                    {
                        item.TaxableAmount = item.GrossSalary - item.PFEE + item.PFER - item.SSEE - item.CITAmount - item.InsurancePremiumAmount + item.InsurancePaidbyOffice;
                    }
                    else
                    {
                        item.InsurancePremiumAmount = item.InsurancePremiumAmount - item.InsurancePaidbyOffice;
                        item.TaxableAmount = item.GrossSalary - item.PFEE - item.SSEE - item.CITAmount - item.InsurancePremiumAmount;
                    }


                }
                else
                {


                    item.Allowances = (from c in _allowanceServices.List().Data.Where(x => x.BranchId == BranchId).
                                       Where(x => x.AllowancePeriod == AllowancePeriod.Monthly).ToList()
                                       join d in _salarySheetServices.GetEmpMonthlyAllowances().
                                       Data.Where(x => x.MonthlySalarySheetPostingId == item.Id).ToList() on c.Id equals d.AllowanceId into joined
                                       from d in joined.DefaultIfEmpty()
                                       select new Allowance()
                                       {
                                           AllowanceId = d == null ? c.Id : d.AllowanceId,
                                           AllowanceHead = d == null ? c.Name : curLang == "ne" ? c.NameNp == null ? c.Name : c.NameNp : c.Name,
                                           AllowanceAmount = d == null ? 0 : d.AllowanceAmount
                                       }).ToList();


                }

                var empAttendanceInfo = (from c in attendancedata.Where(x => x.EmployeeId == item.EmployeeId)
                                         select c).ToList();

                item.Absent = empAttendanceInfo.Where(j => j.Remark.ToLower() == "absent").Count().ToString();
                item.Present = empAttendanceInfo.Where(j => j.Remark.ToLower() == "present").Count().ToString();
                item.Weekend = empAttendanceInfo.Where(j => j.Weekend.ToLower() == "yes").Count();
                item.DaysWorked = empAttendanceInfo.Where(x => x.EmployeeId == item.Id).
                   Count(x => x.Remark == "Present" && x.Holiday != "Yes" && x.Weekend != "Yes");
                item.HolidayCount = empAttendanceInfo.Where(x => x.EmployeeId == item.Id).Count(x => x.Holiday == "Yes");
                item.EarlyOut = empAttendanceInfo.Where(j => j.EarlyOut != "").Count().ToString();
                item.Late = empAttendanceInfo.Where(j => j.LateIn != "").Count().ToString();
                item.Leave = empAttendanceInfo.Where(j => j.OnLeave.ToLower() == "yes").Count().ToString();

                int OTdays = empAttendanceInfo.Where(j => j.Ot != null).Count();

                if (OTdays > 0)
                {

                    var empOTs = empAttendanceInfo.Where(j => j.Ot != ""
                                                        && j.Ot != "00:00").
                                                        Select(x => x.Ot).ToArray();
                    var OTInfo = GetEmployeeOTInformation(empOTs, item.EmployeeId);
                    item.OTAmount = OTInfo.OtAmount;
                    item.OTHours = OTInfo.OtHours;

                }

                if (int.Parse(item.Late) > 0)
                {
                    item.Latehours = empAttendanceInfo.Where(x => x.LateIn != "" && x.LateIn != "00:00").Select(c => TimeSpan.Parse(c.LateIn))
                                                .Aggregate((working, next) => working.Add(next)).TotalHours.ToString();
                }
                else
                {

                    item.Latehours = "0";
                }
                if (int.Parse(item.EarlyOut) > 0)
                {
                    item.EarlyOutHours = empAttendanceInfo.Where(x => x.EarlyOut != "").Select(c => TimeSpan.Parse(c.EarlyOut))
                                                .Aggregate((working, next) => working.Add(next)).TotalHours.ToString();
                }
                else
                {

                    item.EarlyOutHours = "0";
                }

                //Worked = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Actual.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //Ot = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Ot.ToTimeSpan())).TotalHours.ToString("#00.00"),

                item.DaysInMonth = daysInMonth;

                if (!item.salarySheethasBeenCreated)
                {

                    var deduction = CalculatePayrollDeductionAmount(item);
                    item.AbsentDeductionAmount = deduction.AbsentDeductionAmount;
                    item.LateDeductionAmount = deduction.LateDeductionAmount;
                    item.EarlyOutDeductionAmount = deduction.EarlyDeductionAmount;
                }


            }


            var date = RiddhaSession.CurDate;
            return new ServiceResult<List<MonthlySalarySheetVM>>()
            {
                Data = result,
                Status = ResultStatus.Ok,
                Message = Message
            };

        }
        /// <summary>
        /// Calculate Deduction Amount for Absent , Late , Early
        /// </summary>
        private DeductionAmountVm CalculatePayrollDeductionAmount(MonthlySalarySheetVM salarySheet)
        {

            var payroll = _payRollSetupServices.List().Data.Where(x => x.EmployeeId == salarySheet.EmployeeId).FirstOrDefault();
            decimal lateDeduction = 0M;
            decimal earlyDeduction = 0M;
            decimal unitsalary = 0M;
            decimal WeekendAmount = 0M;
            decimal grossSalary = 0M;
            decimal Absent = 0M;
            if (salarySheet.BasicSalary != 0)
            {
                switch (payroll.SalaryPaidPer)
                {
                    case SalaryPaidPer.month:
                        unitsalary = salarySheet.BasicSalary / salarySheet.DaysInMonth;
                        WeekendAmount = unitsalary * salarySheet.Weekend;
                        grossSalary = unitsalary * (salarySheet.DaysWorked + salarySheet.HolidayCount) + WeekendAmount + salarySheet.Grade;

                        lateDeduction = getLateDuductionAmount(int.Parse(salarySheet.Late), decimal.Parse(salarySheet.Latehours)
                            , payroll, unitsalary);
                        earlyDeduction = getEarlyDuductionAmount(int.Parse(salarySheet.EarlyOut),
                            decimal.Parse(salarySheet.EarlyOutHours), payroll, unitsalary);
                        Absent = unitsalary * int.Parse(salarySheet.Absent);
                        break;
                    case SalaryPaidPer.hour:
                        //grossSalary = ((decimal)workedHours.TotalHours * BasicSalary) + gradeAmount;
                        //WeekendAmount = 0;
                        //holidayCount = 0;
                        //WeekendCount = 0;

                        break;
                    default:
                        //unitsalary = BasicSalary / daysInMonth;
                        //WeekendAmount = unitsalary * WeekendCount;
                        //grossSalary = unitsalary * (DaysWorked + holidayCount) + WeekendAmount + gradeAmount;
                        //overTime = calculateOverTime(roughData, item.Id);

                        break;
                }
            }
            else
            {
                grossSalary = 0;
            }

            return new DeductionAmountVm()
            {

                AbsentDeductionAmount = Math.Round(Absent, 2),
                EarlyDeductionAmount = Math.Round(earlyDeduction, 2),
                LateDeductionAmount = Math.Round(lateDeduction, 2)
            };
        }


        private EmployeeOTInfoVm GetEmployeeOTInformation(string[] othoursPerdays, int EmpId)
        {
            decimal OtRate = GetOTRatePerHour(EmpId);
            var empOTConfigurationInfo = GetEmployeeOTConfiguration(EmpId);
            TimeSpan totalOThours = new TimeSpan();
            decimal totalOtAmount = 0;
            if (!empOTConfigurationInfo.IsOTAllowed)
            {
                return new EmployeeOTInfoVm();
            }
            else
            {
                TimeSpan MinOTHour = (TimeSpan)empOTConfigurationInfo.MinOTHour;
                TimeSpan MaxOTHour = (TimeSpan)empOTConfigurationInfo.MaxOTHour;
                for (int i = 0; i < othoursPerdays.Length; i++)
                {
                    TimeSpan EmpOTHours = CalculateOTHours(MinOTHour, MaxOTHour, othoursPerdays[i]);
                    totalOThours = EmpOTHours + totalOThours;
                }
            }

            return new EmployeeOTInfoVm()
            {

                OtHours =  (Math.Round(totalOThours.TotalHours , 2)).ToString(),
                OtAmount = Math.Round(OtRate * decimal.Parse(totalOThours.TotalHours.ToString()) , 2),
            };
        }

        private decimal GetOTRatePerHour(int EmpId)
        {
            var payroll = _payRollSetupServices.List().Data.Where(x => x.EmployeeId == EmpId).FirstOrDefault();
            if (payroll != null)
            {
                return payroll.OtRatePerHour;
            }
            return 0;
        }
        public TimeSpan CalculateOTHours(TimeSpan MinOTHour, TimeSpan MaxOTHour, string OTHours)
        {

            TimeSpan totalOTTime = new TimeSpan();
            TimeSpan EmpOTtime = TimeSpan.Parse(OTHours);
            if (EmpOTtime > MinOTHour)
            {
                var timeExceededMaxOTTime = decimal.Parse(MaxOTHour.TotalHours.ToString())
                    - decimal.Parse(EmpOTtime.TotalHours.ToString());
                if (timeExceededMaxOTTime < 0)
                {

                    totalOTTime = MaxOTHour;
                }
                else
                {
                    totalOTTime = EmpOTtime;
                }
            }
            return totalOTTime;

        }

        private EEmployee GetEmployeeOTConfiguration(int EmpId)
        {
            var empOTConfigurationInfo = _employeeServices.List().Data.Where(x => x.Id == EmpId).FirstOrDefault();
            return empOTConfigurationInfo;
        }



        private decimal getLateDuductionAmount(int Latedays, decimal LateHours, EPayRollSetup payroll, decimal unitSalary)
        {
            if (payroll.EnableLateDeduction == false)
                return 0m;
            //var totalLateDays = roughData.Where(x => x.EmployeeId == payroll.EmployeeId && x.LateIn != "00:00").ToList();
            var totalLateDays = Latedays;
            if (totalLateDays > payroll.LateGraceDay)
            {
                //var totalLateHours = totalLateDays.Select(c => TimeSpan.Parse(c.LateIn))
                //                            .Aggregate((working, next) => working.Add(next));
                var totalLateHours = LateHours;
                switch (payroll.LateDeductionBy)
                {
                    case LateDeductionBy.Days:


                        return unitSalary * totalLateDays;
                    case LateDeductionBy.HalfDay:
                        return (unitSalary * totalLateDays) / 2;
                    case LateDeductionBy.Hour:

                        return (decimal)LateHours * payroll.LateDeductionRate;
                    case LateDeductionBy.SingleDay:

                        if (LateHours > 0)
                            return unitSalary;
                        else
                            return 0M;
                    default:
                        return 0M;
                }
            }
            return 0M;

        }
        private decimal getEarlyDuductionAmount(int EarlyOutdays, decimal EarlyOutHours, EPayRollSetup payroll, decimal unitSalary)
        {
            if (payroll.EnableEarlyDeduction == false)
                return 0m;
            var totalEarlyDays = EarlyOutdays;
            if (totalEarlyDays > payroll.EarlyGraceDay)
            {
                //var totalEarlyHours = totalEarlyDays.Select(c => TimeSpan.Parse(c.LateIn))
                //                            .Aggregate((working, next) => working.Add(next));
                var totalEarlyHours = EarlyOutHours;
                switch (payroll.EarlyDeductionBy)
                {
                    case LateDeductionBy.Days:
                        return unitSalary * totalEarlyDays;
                    case LateDeductionBy.HalfDay:
                        return (unitSalary * totalEarlyDays) / 2;
                    case LateDeductionBy.Hour:

                        return (decimal)EarlyOutHours * payroll.EarlyDeductionRate;
                    case LateDeductionBy.SingleDay:

                        if (EarlyOutHours > 0)
                            return unitSalary;
                        else
                            return 0M;
                    default:
                        return 0M;
                }
            }
            return 0M;

        }

        public IQueryable<EPayRollSetup> GetEmployeePayrolls(int BranchId, DateTime onDate, DateTime ToDate)
        {

            IQueryable<EPayRollSetup> payrolls;

            // payrolls = from c in _payRollSetupServices.List().Data.Where(x => x.BranchId == BranchId
            //&& x.IsApproved == true &&
            //(x.Employee.EmploymentStatus != EmploymentStatus.Resigned
            //&& x.Employee.EmploymentStatus != EmploymentStatus.Terminated))
            //            where c.EffectedFrom >= onDate && c.EffectedFrom <= ToDate
            //            select c;
            payrolls = from c in _payRollSetupServices.List().Data.Where(x => x.BranchId == BranchId
            && x.IsApproved == true &&
            (x.Employee.EmploymentStatus != EmploymentStatus.Resigned
            && x.Employee.EmploymentStatus != EmploymentStatus.Terminated))
                       select c;

            return payrolls;

        }

        public SSFInformationVM PrepareSSFFundModel(OrganizationType organizationType,
            decimal BasicSalary, decimal Grade, EmploymentStatus employmentStatus
            , DateTime? SSFEffectiveDate, DateTime SalaryDate)
        {

            var SSFInfo = new SSFInformationVM();
            if (!IsEligibleForSSFFund(SSFEffectiveDate, SalaryDate) && organizationType == OrganizationType.NonGovernment)
            {
                return SSFInfo;
            }
            if (organizationType == OrganizationType.Government)
            {

                // If organization type is government the SSF will be calculated from
                // Basic salary + Grade
                SSFInfo = GetSSFInformation(BasicSalary + Grade);
            }
            else
            {
                SSFInfo = GetSSFInformation(BasicSalary);
            }

            if (organizationType == OrganizationType.Government
                       && employmentStatus == EmploymentStatus.OnContract)
            {
                SSFInfo.PFEE = 0;
                SSFInfo.PFER = 0;
                SSFInfo.Gratituity = 0;
                SSFInfo.SSEE = 0;
                SSFInfo.SSER = 0;
                SSFInfo.PensionFundEE = 0;
                SSFInfo.PensionFundER = 0;
            }
            else
            {
                SSFInfo.PFEE = Math.Round(SSFInfo.PFEE, 2);
                SSFInfo.PFER = Math.Round(SSFInfo.PFER, 2);
                SSFInfo.Gratituity = Math.Round(SSFInfo.Gratituity, 2);
                SSFInfo.SSEE = Math.Round(SSFInfo.SSEE, 2);
                SSFInfo.SSER = Math.Round(SSFInfo.SSER, 2);
                SSFInfo.PensionFundEE = Math.Round(SSFInfo.PensionFundEE, 2);
                SSFInfo.PensionFundER = Math.Round(SSFInfo.PensionFundER, 2);
            }

            return SSFInfo;
        }
        public bool IsEligibleForSSFFund(DateTime? SSFEffectiveDate, DateTime SalaryDate)
        {

            if (SSFEffectiveDate.HasValue == false)
            {
                return false;
            }

            else if (SalaryDate >= SSFEffectiveDate.GetValueOrDefault())
            {

                return true;
            }
            else
            {
                return true;
            }



        }


        public ServiceResult<string> GetCurrentFiscalYear()
        {

            var CurrentFiscalYear = _fiscalYearServices.List().Data.Where(x => x.BranchId == BranchId && x.CurrentFiscalYear == true).FirstOrDefault();
            return new ServiceResult<string>()
            {

                Data = CurrentFiscalYear.FiscalYear,
                Status = ResultStatus.Ok
            };
        }
        public List<Allowance> GetEmpAllowances(int EmpId, string ToDate, decimal BasicSalary, decimal GrossSalary)
        {

            var result = new List<Allowance>();
            var AllowanceValidUptoDate = ToDate.ToDateTime();
            var empallowance = _allowanceServices.ListEmpAllowance().Data.Where(x => x.BranchId == BranchId
                                                                                && x.EmployeeId == EmpId
                                                                                && x.IsApproved == true
                                                                                && x.AllowancePeriod == AllowancePeriod.Monthly
                                                                                && (x.FromDate <= AllowanceValidUptoDate
                                                                                && x.ToDate >= AllowanceValidUptoDate)).ToList();
            result = (from c in _allowanceServices.List().Data.Where(x => x.BranchId == BranchId
                                                                  && x.AllowancePeriod == AllowancePeriod.Monthly).ToList()

                      join d in empallowance on c.Id equals d.AllowanceId into joined
                      from d in joined.DefaultIfEmpty()
                      select new Allowance()
                      {
                          AllowanceId = c.Id,
                          AllowanceHead = curLang == "ne" ? (c.NameNp == null ? c.Name : c.NameNp) : c.Name,
                          AllowanceAmount = d == null ? 0 : d.AllowanceCalculatedBy ==
                          AllowanceCalculatedBy.Percentage ? (d.AllowancePaidPer == AllowancePaidPer.BasicSalary ?
                          (d.Value / 100) * BasicSalary : (d.Value / 100) * GrossSalary) : d.Value

                      }).OrderBy(x => x.AllowanceId).ToList();


            return result;

        }


        public ServiceResult<List<EDeduction>> GetDeductions()
        {

            var result = new List<EDeduction>();
            return new ServiceResult<List<EDeduction>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }


        public SSFInformationVM GetSSFInformation(decimal BasicSalary)
        {

            var taxInfo = GetTaxSetupInfo();

            SSFInformationVM ssfInfo = new SSFInformationVM();
            ssfInfo.PFEE = (taxInfo.PFPercByEmployee / 100) * BasicSalary;
            ssfInfo.PFER = (taxInfo.PFPercByEmployer / 100) * BasicSalary;
            ssfInfo.Gratituity = (taxInfo.GratituityPercByEmployer / 100) * BasicSalary;
            ssfInfo.SSEE = (taxInfo.SSPercByEmployee / 100) * BasicSalary;
            ssfInfo.SSER = (taxInfo.SSPercByEmployer / 100) * BasicSalary;
            ssfInfo.PensionFundER = (taxInfo.PensionFundPercByEmployer / 100) * BasicSalary;
            ssfInfo.PensionFundEE = (taxInfo.PensionFundPercByEmployee / 100) * BasicSalary;
            return ssfInfo;
        }


        //public decimal GetGrossSalary(int EmpId, decimal BasicSalary , string ToDate )
        //{

        //    decimal totalGrossSalary = GetEmpAllowances(EmpId , ToDate).Sum(x => x.AllowanceAmount) + BasicSalary + GetGradeAmount(EmpId);
        //    return totalGrossSalary;
        //}


        //public decimal GetTaxableAmount(int EmpId, decimal BasicSalary, decimal CITRate)
        //{
        //    var ssfInfo = GetSSFInformation(BasicSalary);
        //    decimal GrossSalary = GetGrossSalary(EmpId, BasicSalary);
        //    decimal CITAmount = GetCITAmount(CITRate, GrossSalary);
        //    decimal TaxableAmount = GrossSalary - ssfInfo.PFEE - ssfInfo.SSEE - CITAmount - GetInsuranceAmount(EmpId);
        //    return TaxableAmount;
        //}


        public decimal GetInsuranceAmount(int EmpId)
        {

            //var Insuranceamount = _S
            return 0;
        }

        private EGradeGroup GetGradeInfo(int EmpId, string ValidUptoDate)
        {

            SGradeGroup _gradeGroup = new SGradeGroup();

            var GradeValidUpto = ValidUptoDate.ToDateTime();
            var empGradeGroups = _payRollSetupServices.ListEmpGradeGroup().Data.
                                                    Where(x => x.BranchId == BranchId
                                                    && x.EmployeeId == EmpId
                                                    && (x.EffectedFrom <= GradeValidUpto
                                                    && x.EffectedTo >= GradeValidUpto)
                                                    && x.IsApproved == true);
            var gradeGroupInfo = (from c in empGradeGroups.ToList()
                                  join d in _gradeGroup.List().Data.ToList() on c.GradeGroupId equals d.Id
                                  select d).FirstOrDefault();

            decimal gradegroupAmount = 0M;


            if (gradeGroupInfo != null)
            {

                //gradegroupAmount = gradeGroupInfo.Value;
                return gradeGroupInfo;
            }
            return new EGradeGroup();
        }

        public decimal GetTDSAmount(MaritialStatus maritialStatus, decimal TaxableAmount, ref decimal RenumurationTax, OrganizationType organizationType)
        {

            if (organizationType == OrganizationType.Government)
            {

                return Math.Round(TaxableAmount * 0.01M, 2);
            }
            var taxInfo = GetTaxSetupInfo();

            if (taxInfo == null)
            {

                return 0;
            }
            var taxSlabInfo = _taxSetupServices.GetTaxSlabDetails().Data.Where(x => x.TaxSetupId == taxInfo.Id).ToList();
            TaxableAmount = TaxableAmount * 12;
            decimal SocialSecurityTax = 0;
            // Renumeration Tax is used as the reference type
            RenumurationTax = 0;
            decimal TaxSlabAmount = 0;
            foreach (var taxSlab in taxSlabInfo.OrderBy(x => x.SN))
            {

                TaxSlabAmount = maritialStatus == MaritialStatus.Married ? taxSlab.CoupleAmount : taxSlab.IndividualAmount;


                if (taxSlab.SN == 1)
                {

                    SocialSecurityTax = TaxableAmount * (taxSlab.TaxPerc / 100);
                }
                else
                {
                    RenumurationTax = RenumurationTax + (TaxableAmount * (taxSlab.TaxPerc / 100));
                }
                TaxableAmount = TaxableAmount - TaxSlabAmount;

                if (TaxableAmount < 0)
                {
                    break;
                }
            }

            // Tax above the taxslab
            if (TaxableAmount > 0)
            {
                RenumurationTax = RenumurationTax + (taxInfo.TaxPercAboveFinalValue / 100);
            }

            if (RenumurationTax > 0)
            {
                RenumurationTax = Math.Round((RenumurationTax / 12), 2);
            }
            return Math.Round((SocialSecurityTax / 12), 2);
        }
        public ETaxSetup GetTaxSetupInfo()
        {

            var CurrentFiscalYearId = _fiscalYearServices.List().Data.Where(x => x.BranchId == BranchId && x.CurrentFiscalYear == true)
                                                                 .Select(x => x.Id).FirstOrDefault();
            var taxInfo = _taxSetupServices.List().Data.Where(x => x.BranchId == BranchId
                                                               && x.FiscalYearId == CurrentFiscalYearId)
                                                               .FirstOrDefault();
            return taxInfo;
        }

        public decimal GetCITAmount(decimal CITperc, decimal GrossSalary)
        {
            var amount = (CITperc / 100) * GrossSalary;
            return amount;
        }


        [HttpPost]
        public ServiceResult<MonthlySalaryPostingVM> Post(MonthlySalaryPostingVM vm)
        {

            if (vm.MonthlySalarySheet == null)
            {

                return new ServiceResult<MonthlySalaryPostingVM>()
                {
                    Data = vm,
                    Message = _loc.Localize("No Salary to post"),
                    Status = ResultStatus.processError
                };
            }
            var CurrrentFiscalYearId = _fiscalYearServices.List().Data.Where(x => x.BranchId == BranchId && x.CurrentFiscalYear == true).Select(x => x.Id).FirstOrDefault();

            int[] EmpIds = vm.MonthlySalarySheet.Select(x => x.EmployeeId).ToArray();
            var ExistingSalarySheetDetials = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId
                                                                                                    && x.FiscalYearId == CurrrentFiscalYearId
                                                                                                    && x.Month == vm.MonthId
                                                                                                    && x.IsApproved == false)
                                              select c).ToList();

            if (ExistingSalarySheetDetials.Count > 0)
            {

                var toUpdateEmpsSalarySheetDetials = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId
                                                                                                           && x.FiscalYearId == CurrrentFiscalYearId
                                                                                                           && x.Month == vm.MonthId
                                                                                                           && x.IsApproved == false)
                                                      join d in EmpIds on c.EmployeeId equals d
                                                      select c).ToList();

                _salarySheetServices.RemoveSalarySheetInfo(toUpdateEmpsSalarySheetDetials);
            }
            // Salary Posting master detail
            var MonthlySalarySheetMasters = (from c in vm.MonthlySalarySheet.Where(x => x.IsApproved == false).ToList()
                                             select new EMonthlySalarySheetPosting()
                                             {
                                                 EmployeeId = c.EmployeeId,
                                                 EmployeeName = c.EmployeeName,
                                                 EmployeeCode = c.EmployeeCode,
                                                 DepartmentName = c.DepartmentName,
                                                 BasicSalary = c.BasicSalary,
                                                 Grade = c.Grade,
                                                 GradeName = c.GradeName,
                                                 GrossSalary = c.GrossSalary,
                                                 GenderEnum = c.GenderEnum,
                                                 MaritalStatusEnum = c.MaritalStatusEnum,
                                                 CITAmount = c.CITAmount,
                                                 TaxableAmount = c.TaxableAmount,
                                                 InsurancePremiumAmount = c.InsurancePremiumAmount,
                                                 InsurancePaidbyOffice = c.InsurancePaidbyOffice,
                                                 FiscalYearId = CurrrentFiscalYearId,
                                                 Month = vm.MonthId,
                                                 Year = DateTime.Now.Year,
                                                 CreationDateTime = DateTime.Now,
                                                 RenumerationTax = c.RenumerationTax,
                                                 SocialSecurityTax = c.SocialSecurityTax,
                                                 PFEE = c.PFEE,
                                                 PFER = c.PFER,
                                                 SSEE = c.SSEE,
                                                 SSER = c.SSER,
                                                 PensionFundEE = c.PensionFundEE,
                                                 PensionFundER = c.PensionFundER,
                                                 Gratituity = c.Gratituity,
                                                 Absent = c.Absent,
                                                 Leave = c.Leave,
                                                 LateIn = c.Late,
                                                 EarlyOut = c.EarlyOut,
                                                 DeductionAmount = c.DeductionAmount,
                                                 RebateAmount = c.RebateAmount,
                                                 NetSalary = c.NetSalary,
                                                 CreatedByUserId = (int)RiddhaSession.UserId,
                                                 BranchId = BranchId,
                                                 AdditionAmount = c.AdditionAmount,
                                                 AbsentDeductionAmount = c.AbsentDeductionAmount,
                                                 EarlyOutDeductionAmount = c.EarlyOutDeductionAmount,
                                                 LateDeductionAmount = c.LateDeductionAmount,
                                                 FromDate = vm.FromDate,
                                                 EndDate = vm.EndDate,
                                                 OtAmount = c.OTAmount,
                                                 OtHours = c.OTHours
                                             }).ToList();


            var masterResult = _salarySheetServices.AddMonthlySalaryPostMasterInfo(MonthlySalarySheetMasters);

            // Allowances 
            var allowances = new List<EMonthlySalarySheetAllowances>();
            foreach (var item in vm.MonthlySalarySheet)
            {
                if (item.Allowances != null)
                {
                    allowances.AddRange((from c in item.Allowances
                                         select new EMonthlySalarySheetAllowances()
                                         {
                                             AllowanceHead = c.AllowanceHead,
                                             AllowanceAmount = c.AllowanceAmount,
                                             AllowanceId = c.AllowanceId,
                                             MonthlySalarySheetPostingId = masterResult.Data.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().Id
                                         }).ToList());
                }
            }
            _salarySheetServices.AddMonthlySalarySheetAllowances(allowances);

            //var deductions = from c in vm.MonthlySalarySheet.PayDeductions 

            return new ServiceResult<MonthlySalaryPostingVM>()
            {
                Data = vm,
                Message = _loc.Localize(masterResult.Message),
                Status = masterResult.Status
            };
        }

        [HttpGet]
        public ServiceResult<List<DropDownVM>> GetEmployees(int DepartmentId = 0)
        {
            var empData = _employeeServices.List().Data.Where(x => x.BranchId == BranchId);
            if (DepartmentId > 0)
            {
                empData = _employeeServices.List().Data.Where(x => x.BranchId == BranchId);
            }
            var result = (from c in _employeeServices.List().Data.Where(x => x.BranchId == BranchId)
                          select new DropDownVM()
                          {
                              Id = c.Id,
                              Code = c.Code,
                              Name = RiddhaSession.Language == "en" ? c.Name : string.IsNullOrEmpty(c.NameNp) == true ? c.Name : c.NameNp
                          }).ToList();
            return new ServiceResult<List<DropDownVM>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<DropDownVM>> GetDepartments()
        {
            SDepartment _departmentServices = new SDepartment();
            var result = (from c in _departmentServices.List().Data.Where(x => x.BranchId == BranchId)
                          select new DropDownVM()
                          {
                              Id = c.Id,
                              Code = c.Code,
                              Name = RiddhaSession.Language == "en" ? c.Name : c.NameNp ?? c.Name
                          }).ToList();
            return new ServiceResult<List<DropDownVM>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<TDSAmountVM> GetTaxAmountInfo(MaritialStatus maritialStatus, decimal TaxableAmount)
        {

            decimal SocialSecurityTax = 0M;
            decimal RenumurationTax = 0M;
            organizationType = _companyServices.List().Data.Where(x => x.Id == RiddhaSession.CompanyId).Select(x => x.OrganizationType).FirstOrDefault();

            SocialSecurityTax = GetTDSAmount(maritialStatus, TaxableAmount, ref RenumurationTax, organizationType);

            return new ServiceResult<TDSAmountVM>()
            {
                Data = new TDSAmountVM() { RenumerationTax = RenumurationTax, SocialSecurityTax = SocialSecurityTax },
                Status = ResultStatus.Ok
            };

        }


        public ServiceResult<decimal> GetRebateAmount(MaritialStatus maritialStatus, Gender gender, decimal taxAmount)
        {

            decimal rebateAmount = 0;
            if (gender == Gender.Female && maritialStatus == MaritialStatus.UnMarried)
            {

                var rebatePerc = GetTaxSetupInfo() == null ? 0 : GetTaxSetupInfo().RebatePercForFemaleUnmarried;
                if (rebatePerc > 0)
                {

                    rebateAmount = (rebatePerc / 100) * taxAmount;
                }
            }
            return new ServiceResult<decimal>()
            {
                Data = rebateAmount,
                Status = ResultStatus.Ok
            };
        }
    }


    public class TDSAmountVM
    {

        public decimal SocialSecurityTax { get; set; }
        public decimal RenumerationTax { get; set; }
    }

    public class DeductionAmountVm
    {

        public decimal EarlyDeductionAmount { get; set; }
        public decimal LateDeductionAmount { get; set; }
        public decimal AbsentDeductionAmount { get; set; }
    }

    public class EmployeeOTInfoVm
    {
        public string OtHours { get; set; }
        public decimal OtAmount { get; set; }
    }
}
