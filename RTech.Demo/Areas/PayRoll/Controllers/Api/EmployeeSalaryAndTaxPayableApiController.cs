using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Areas.PayRoll.ViewModels;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class EmployeeSalaryAndTaxPayableApiController : ApiController
    {

        SEmployeeSalaryAndTaxPayable _employeeSalaryAndTaxPayableServices = null;
        SSalarySheet _salarySheetServices = null;
        int BranchId = (int)RiddhaSession.BranchId;
        SFiscalYear _fiscalYearServices = null;
        LocalizedString _loc = null;
        SEmployee _employee = null;
        string CurLang = RiddhaSession.Language;
        SAllowance _allowanceServices = null;
        public EmployeeSalaryAndTaxPayableApiController()
        {
            _employeeSalaryAndTaxPayableServices = new SEmployeeSalaryAndTaxPayable();
            _salarySheetServices = new SSalarySheet();
            _employee = new SEmployee();
            _loc = new LocalizedString();
            _fiscalYearServices = new SFiscalYear();
            _allowanceServices = new SAllowance();

        }
        [HttpPost]
        public ServiceResult<MonthlySalaryPostingVM> Post(MonthlySalaryPostingVM vm)
        {
            var empsSalarySheetInfoToApprove = (from c in vm.MonthlySalarySheet.ToList()
                                                join d in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.IsApproved == false) on c.Id equals d.Id
                                                select d).ToList();

            if (empsSalarySheetInfoToApprove.Count() > 0)
            {

                //empsSalarySheetInfoToApprove.ForEach(x => x.ApprorvedByUserId = (int)RiddhaSession.UserId);
                //empsSalarySheetInfoToApprove.ForEach(x => x.ApprovedDateTime = DateTime.Now);

                foreach (var item in empsSalarySheetInfoToApprove)
                {

                    item.IsApproved = true;
                    item.ApprorvedByUserId = (int)RiddhaSession.UserId;
                    item.ApprovedDateTime = DateTime.Now;
                    item.SalaryStatus = SalaryStatus.Payable;
                }
                _salarySheetServices.ApproveMonthlySalarySheet(empsSalarySheetInfoToApprove);

                var employeeSalaryAndTaxPayables = (from c in empsSalarySheetInfoToApprove
                                                    select new EEmployeeSalaryAndTaxPayable()
                                                    {
                                                        Id = c.Id,
                                                        EmployeeId = c.EmployeeId,
                                                        EmployeeCode = c.EmployeeCode,
                                                        EmployeeName = c.EmployeeName,
                                                        DepartmentName = c.DepartmentName,
                                                        FiscalYearId = c.FiscalYearId,
                                                        MonthId = c.Month,
                                                        Year = c.Year,
                                                        NetSalary = c.NetSalary,
                                                        Gratituity = c.Gratituity,
                                                        PFEE = c.PFEE,
                                                        PFER = c.PFER,
                                                        PensionFundEE = c.PensionFundEE,
                                                        PensionFundER = c.PensionFundER,
                                                        RenumerationTax = c.RenumerationTax,
                                                        SocialSecurityTax = c.SocialSecurityTax,
                                                        SSEE = c.SSEE,
                                                        SSER = c.SSER,
                                                        CITAmount = c.CITAmount,
                                                        RebateAmount = c.RebateAmount,
                                                        CreatedByUserId = RiddhaSession.UserId,
                                                        CreationDateTime = DateTime.Now,
                                                        InsuranceAmount = c.InsurancePremiumAmount,
                                                        InsurancePaidbyOffice = c.InsurancePaidbyOffice,
                                                        TaxableAmount = c.TaxableAmount,
                                                        TotalDeductionAmount = c.DeductionAmount,
                                                        TotalAdditionAmount = c.AdditionAmount,
                                                        AbsentDeductionAmount = c.AbsentDeductionAmount,
                                                        EarlyOutDeductionAmount = c.EarlyOutDeductionAmount,
                                                        LateDeductionAmount = c.LateDeductionAmount,
                                                        TotalOTAmount = c.OtAmount
                                                    }).ToList();

                foreach (var payable in employeeSalaryAndTaxPayables)
                {

                    var allowances = vm.MonthlySalarySheet.Where(x => x.EmployeeId == payable.EmployeeId).Select(x => x.Allowances).FirstOrDefault();

                    payable.TotalAllowancesAmount = 0M;
                    if (allowances != null)
                    {
                        payable.TotalAllowancesAmount = allowances.Sum(x => x.AllowanceAmount);
                    }


                }

                var result = _employeeSalaryAndTaxPayableServices.AddEmployeeSalaryAndTaxPayableList(employeeSalaryAndTaxPayables);
                return new ServiceResult<MonthlySalaryPostingVM>()
                {
                    Data = vm,
                    Message = _loc.Localize(result.Message),
                    Status = result.Status
                };
            }
            return new ServiceResult<MonthlySalaryPostingVM>()
            {
                Data = null,
                Message = _loc.Localize("Salary Sheet has already been approved"),
                Status = ResultStatus.processError
            };
        }

        [HttpGet]
        public ServiceResult<List<MonthlySalarySheetVM>> GetMonthlySalarySheetToApprove(int MonthId, string DepartmentIds, string SectionIds, string EmpIds)
        {

            List<EEmployee> employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                                         select c).ToList();
            if (DepartmentIds != null && DepartmentIds.Length > 0)
            {

                int[] depIds = DepartmentIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in depIds on c.Section.DepartmentId equals d
                             select c).ToList();
            }
            if (SectionIds != null && SectionIds.Length > 0)
            {

                int[] secIds = SectionIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in secIds on c.SectionId equals d
                             select c).ToList();
            }
            if (EmpIds != null && EmpIds.Length > 0)
            {

                int[] empIds = EmpIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in empIds on c.Id equals d
                             select c).ToList();
            }


            var CurrentFiscalYearId = _fiscalYearServices.List().Data.Where(x => x.BranchId == BranchId && x.CurrentFiscalYear == true).Select(x => x.Id).FirstOrDefault();
            var ExistingSalarySheet = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId).ToList()
                                       join d in employees on c.EmployeeId equals d.Id
                                       where c.FiscalYearId == CurrentFiscalYearId && c.Month == MonthId && c.SalaryStatus == SalaryStatus.New
                                       select c).ToList();


            if (ExistingSalarySheet.Count > 0)
            {
                //Renumeration Tax has been used as the ref
                //decimal RenumerationTax = 0;
                var result = (from c in ExistingSalarySheet
                              join d in employees on c.EmployeeId equals d.Id
                              select new MonthlySalarySheetVM()
                              {
                                  Id = c.Id,
                                  EmployeeId = c.EmployeeId,
                                  EmployeeName = c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  JoinedMonth = d.DateOfJoin.HasValue == true ? d.DateOfJoin.GetValueOrDefault().ToString("yyyy/MM/dd") : "",
                                  DepartmentName = c.DepartmentName,
                                  GenderEnum = c.GenderEnum,
                                  Gender = c.GenderEnum == Gender.Male ? "M" : "F",
                                  MaritalStatusEnum = c.MaritalStatusEnum,
                                  MaritalStatus = c.MaritalStatusEnum == MaritialStatus.Married ? "C" : "S",
                                  BasicSalary = c.BasicSalary,
                                  Grade = c.Grade,
                                  GradeName = c.GradeName,
                                  PFEE = c.PFEE,
                                  PFER = c.PFER,
                                  Gratituity = c.Gratituity,
                                  SSEE = c.SSEE,
                                  SSER = c.SSER,
                                  PensionFundEE = c.PensionFundEE,
                                  PensionFundER = c.PensionFundER,
                                  GrossSalary = c.GrossSalary,
                                  CITAmount = c.CITAmount,
                                  TaxableAmount = c.TaxableAmount,
                                  SocialSecurityTax = c.SocialSecurityTax,
                                  RenumerationTax = c.RenumerationTax,
                                  InsurancePremiumAmount = c.InsurancePremiumAmount,
                                  InsurancePaidbyOffice = c.InsurancePaidbyOffice,
                                  DeductionAmount = c.DeductionAmount,
                                  Allowances = GetEmpAllowances(c.Id),
                                  Absent = c.Absent,
                                  EarlyOut = c.EarlyOut,
                                  Late = c.LateIn,
                                  Leave = c.Leave,
                                  NetSalary = c.NetSalary,
                                  RebateAmount = c.RebateAmount,
                                  IsApproved = c.IsApproved,
                                  AdditionAmount = c.AdditionAmount,
                                  AbsentDeductionAmount = c.AbsentDeductionAmount,
                                  EarlyOutDeductionAmount = c.EarlyOutDeductionAmount,
                                  LateDeductionAmount = c.LateDeductionAmount,
                                  OTHours = c.OtHours,
                                  OTAmount = c.OtAmount
                              }).OrderBy(x => x.EmployeeName).ToList();
                return new ServiceResult<List<MonthlySalarySheetVM>>()
                {
                    Data = result,
                    Status = ResultStatus.Ok
                };
            }

            return new ServiceResult<List<MonthlySalarySheetVM>>()
            {

                Data = null,
                Message = _loc.Localize("No Salary posted of this month"),
                Status = ResultStatus.processError
            };


        }

        public List<Allowance> GetEmpAllowances(int MasterId)
        {


            var result = new List<Allowance>();
            result = (from c in _allowanceServices.List().Data.Where(x => x.BranchId == BranchId).
                                       Where(x => x.AllowancePeriod == AllowancePeriod.Monthly).ToList()
                      join d in _salarySheetServices.GetEmpMonthlyAllowances().
                      Data.Where(x => x.MonthlySalarySheetPostingId == MasterId).ToList() on c.Id equals d.AllowanceId into joined
                      from d in joined.DefaultIfEmpty()
                      select new Allowance()
                      {
                          AllowanceId = d == null ? c.Id : d.AllowanceId,
                          AllowanceHead = d == null ? c.Name : CurLang == "ne" ? c.NameNp == null ? c.Name : c.NameNp : c.Name,
                          AllowanceAmount = d == null ? 0 : d.AllowanceAmount
                      }).ToList();
            //result = (from c in _salarySheetServices.GetEmpMonthlyAllowances().Data.Where(x => x.MonthlySalarySheetPostingId == MasterId).ToList()
            //          join d in _allowanceServices.List().Data on c.AllowanceId equals d.Id into joined
            //          from d in joined.DefaultIfEmpty()
            //          select new Allowance()
            //          {
            //              AllowanceId = c.Id,
            //              AllowanceHead = d == null ? c.AllowanceHead : CurLang == "ne" ? d.NameNp == null ? d.Name : d.NameNp : d.Name,
            //              AllowanceAmount = c.AllowanceAmount
            //          }).OrderBy(x => x.AllowanceId).ToList();
            return result;

        }
    }

    public class EmployeeSalaryAndTaxPayableVM
    {

        public int FiscalYearId { get; set; }
        public int MonthId { get; set; }
        public int Year { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }

        #region Salary Payable


        public decimal NetSalary { get; set; }

        public bool IsPaidSalary { get; set; }

        #endregion

        #region Tax Payable
        /// <summary>
        /// PF Employee 
        /// </summary>
        public decimal PFEE { get; set; }

        /// <summary>
        /// PF Employer 
        /// </summary>
        public decimal PFER { get; set; }

        public decimal Gratituity { get; set; }

        /// <summary>
        /// Social Security Employee
        /// </summary>
        public decimal SSEE { get; set; }
        /// <summary>
        /// Social Security Employer
        /// </summary>
        public decimal SSER { get; set; }

        public decimal PensionFundEE { get; set; }
        public decimal PensionFundER { get; set; }

        public decimal CITAmount { get; set; }

        public decimal SocialSecurityTax { get; set; }
        public decimal RenumerationTax { get; set; }

        public bool IsPaidSSF { get; set; }
        public bool IsPaidCIT { get; set; }

        public bool IsPaidTax { get; set; }

        #endregion

    }
}
