using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
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
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class SalaryPaymentApiController : ApiController
    {

        SEmployeeSalaryAndTaxPayable _employeeSalaryAndTaxPayableServices = null;
        SSalarySheet _salarySheetServices = null;
        SFiscalYear _fiscalYearServices = null;
        LocalizedString _loc = null;
        SEmployee _employee = null;
        SBank _bankServices = null;
        int BranchId = (int)RiddhaSession.BranchId;
        SAllowance _allowanceServices = null;
        string CurLang = RiddhaSession.Language;
        public SalaryPaymentApiController()
        {
            _employeeSalaryAndTaxPayableServices = new SEmployeeSalaryAndTaxPayable();
            _salarySheetServices = new SSalarySheet();
            _fiscalYearServices = new SFiscalYear();
            _employee = new SEmployee();
            _loc = new LocalizedString();
            _bankServices = new SBank();
            _allowanceServices = new SAllowance();
        }



        [HttpGet]
        public ServiceResult<List<SalaryPaymentVm>> GetEmpSalaryPayableInfo(string DepartmentIds, string SectionIds,
            string EmpIds, int MonthId ,bool IsGeneratepaySlip = false)
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
        
            var    ExistingSalarySheet = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId).ToList()
                                           join d in employees on c.EmployeeId equals d.Id
                                           where c.FiscalYearId == CurrentFiscalYearId
                                           && c.SalaryStatus == Riddhasoft.PayRoll.Entities.SalaryStatus.Payable
                                           && c.Month == MonthId
                                           select c).ToList();

            SCompany _companyServices = new SCompany();
            OrganizationType organizationType = _companyServices.List().Data.Where(x => x.Id == RiddhaSession.CompanyId).Select(x => x.OrganizationType).FirstOrDefault();



            if (ExistingSalarySheet.Count > 0)
            {
                //Renumeration Tax has been used as the ref
                //decimal RenumerationTax = 0;


                
                var result = (from c in ExistingSalarySheet
                              join d in _employeeSalaryAndTaxPayableServices.List().Data on c.EmployeeId equals d.EmployeeId
                              join e in employees on c.EmployeeId equals e.Id
                              where d.MonthId == c.Month && c.FiscalYearId == d.FiscalYearId && d.IsPaidSalary == false
                              select new SalaryPaymentVm()
                              {
                                  SalarySheetId = c.Id,
                                  SalaryPayableId  = d.Id,
                                  MonthId = c.Month,
                                  EmployeeId = c.EmployeeId,
                                  EmployeeName = c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  JoinedMonth = e.DateOfJoin.HasValue == true ? e.DateOfJoin.GetValueOrDefault().ToString("yyyy/MM/dd") : "",
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
                                  InsurancePaidbyOffice  = c.InsurancePaidbyOffice,
                                  DeductionAmount = c.DeductionAmount,
                                  Allowances = GetEmpAllowances(c.Id),
                                  Absent = c.Absent,
                                  EarlyOut = c.EarlyOut,
                                  Late = c.LateIn,
                                  Leave = c.Leave,
                                  NetSalary = c.NetSalary,
                                  RebateAmount = c.RebateAmount,
                                  IsApproved = c.IsApproved,
                                  OrganizationType = organizationType,
                                  AdditionAmount = c.AdditionAmount,
                                  AbsentDeductionAmount = c.AbsentDeductionAmount,
                                  EarlyOutDeductionAmount = c.EarlyOutDeductionAmount,
                                  LateDeductionAmount = c.LateDeductionAmount
                              }).OrderBy(x => x.EmployeeName).ToList();


                foreach (var item in result)
                {

                    item.TotalAllowancesAmount = item.Allowances.Sum(x => x.AllowanceAmount);
                    item.TotalTDS = item.SocialSecurityTax + item.RenumerationTax;
                    item.TotalEmployeeSSFAmount = item.SSEE + item.PFEE + item.PensionFundEE;
                    item.TotalEmployerSSFAmount = item.SSER + item.PFER + item.Gratituity + item.PensionFundER;
                }
                return new ServiceResult<List<SalaryPaymentVm>>()
                {
                    Data = result,
                    Status = ResultStatus.Ok
                };
            }

            return new ServiceResult<List<SalaryPaymentVm>>()
            {

                Data = null,
                Message = _loc.Localize("No Salary posted of this month"),
                Status = ResultStatus.processError
            };


        }


       
        public List<Allowance> GetEmpAllowances(int Masterid)
        {



            var result = new List<Allowance>();
            result = (from c in _allowanceServices.List().Data.Where(x => x.BranchId == BranchId).
                                       Where(x => x.AllowancePeriod == AllowancePeriod.Monthly).ToList()
                      join d in _salarySheetServices.GetEmpMonthlyAllowances().
                      Data.Where(x => x.MonthlySalarySheetPostingId == Masterid).ToList() on c.Id equals d.AllowanceId into joined
                      from d in joined.DefaultIfEmpty()
                      select new Allowance()
                      {
                          AllowanceId = d == null ? c.Id : d.AllowanceId,
                          AllowanceHead = d == null ? c.Name : CurLang == "ne" ? c.NameNp == null ? c.Name : c.NameNp : c.Name,
                          AllowanceAmount = d == null ? 0 : d.AllowanceAmount
                      }).ToList();
            //result = (from c in _salarySheetServices.GetEmpMonthlyAllowances().Data.Where(x => x.MonthlySalarySheetPostingId == Masterid).ToList()
            //          join d in _allowanceServices.List().Data on c.AllowanceId equals d.Id into joined 
            //          from d in joined.DefaultIfEmpty()
            //          select new Allowance()
            //          {
            //              AllowanceId = d== null ? 0 : c.AllowanceId,
            //              AllowanceHead = d == null ? c.AllowanceHead : CurLang== "ne" ?  d.NameNp == null ? d.Name : d.NameNp : d.Name,
            //              AllowanceAmount = c.AllowanceAmount
            //          }).OrderBy(x => x.AllowanceId).ToList();
            return result;

        }

        [HttpPost]
        public ServiceResult<SalaryPaymentPostVm> Post(SalaryPaymentPostVm vm) {



            if (vm.SalaryPaymentVm.Count > 0)
            {

                //var empSalarySheets = (from c in _salarySheetServices.GetSalarySheetInfo().Data
                //                       join d in vm.SalaryPaymentVm on c.Id equals d.SalarySheetId
                //                       select c).ToList();
                //var empSalaryPayables = (from c in _employeeSalaryAndTaxPayableServices.List().Data
                //                         join d in vm.SalaryPaymentVm on c.Id equals d.SalaryPayableId
                //                         select c).ToList();

                foreach (var item in vm.SalaryPaymentVm)
                {

                    // Update Salary Sheet status to paid
                    var empSalarySheet = _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.Id == item.SalarySheetId).FirstOrDefault();
                    empSalarySheet.SalaryStatus = SalaryStatus.Paid;
                    _salarySheetServices.UpdateSalarySheet(empSalarySheet);


                    // update salary payable status to paid 
                    var empSalarypayable = _employeeSalaryAndTaxPayableServices.List().Data.Where(x => x.Id == item.SalaryPayableId).FirstOrDefault();
                    empSalarypayable.IsPaidSalary = true;
                    empSalarypayable.PaymentMadeById = RiddhaSession.UserId;
                    empSalarypayable.PaymentMadeDateTime = DateTime.Now;
                    _employeeSalaryAndTaxPayableServices.Update(empSalarypayable);
                }


                return new ServiceResult<SalaryPaymentPostVm>()
                {
                    Data = vm,
                    Message = _loc.Localize("Salary Paid Successfully"),
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<SalaryPaymentPostVm>()
            {

                Data = null,
                Message = _loc.Localize("No Salary list of emp to pay"),
                Status = ResultStatus.processError
            };

        }



        [HttpGet]
        public ServiceResult<List<SalaryPaymentAdviceVm>> GeneratePaymentAdvice(string DepartmentIds, string SectionIds, 
            string EmpIds , int MonthId , int  BankId)
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


            var bankresult = _bankServices.List().Data.Where(x => x.Id == BankId);
            var ExistingSalarySheet = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId).ToList()
                                       join d in employees on c.EmployeeId equals d.Id
                                       join e in  bankresult on d.BankId equals e.Id 
                                       where c.FiscalYearId == CurrentFiscalYearId
                                       && c.Month == MonthId &&  c.SalaryStatus == Riddhasoft.PayRoll.Entities.SalaryStatus.Paid
                                       select new SalaryPaymentAdviceVm() {
                                           Amount = c.NetSalary,
                                           BankAccountNo = d.BankAccountNo,
                                           StaffName = RiddhaSession.Language == "en" ?  d.Name : d.NameNp == null ? d.Name : d.NameNp,
                                           BankId =  d.BankId,
                                           BankName =  e == null ? "" : e.Name
                                       }).ToList();


            if (ExistingSalarySheet.Count() > 0) {

                return new ServiceResult<List<SalaryPaymentAdviceVm>>()
                {

                    Data = ExistingSalarySheet,
                    Message = "",
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<List<SalaryPaymentAdviceVm>>()
            {

                Data = null,
                Message = _loc.Localize("No Salary posted of this month"),
                Status = ResultStatus.processError
            };


        }

        [HttpGet]
        public ServiceResult<List< DropDownVM>> GetBanks() {

            var data = (from c in _bankServices.List().Data.Where(x => x.BranchId == BranchId)
                        select new DropDownVM()
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Name = RiddhaSession.Language == "en" ? c.Name : c.NameNp == null ? c.Name : c.NameNp,
                            NameNp = c.NameNp
                        }).ToList();
            return new ServiceResult<List< DropDownVM>>()
            {
                Data = data,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<int> GetOrganizationType() {

            SCompany _companyServices = new SCompany();

            OrganizationType organizationType = _companyServices.List().Data.Where(x => x.Id == RiddhaSession.CompanyId).Select(x => x.OrganizationType).FirstOrDefault();
            
            return new ServiceResult<int>()
            {
                Data = (int)organizationType,
                Status = ResultStatus.Ok
            };
        }
    }

    public class SalaryPaymentVm : MonthlySalarySheetVM
    {

        public int SalarySheetId { get; set; }
        public int SalaryPayableId { get; set; }
        public int MonthId { get; set; }
        public decimal TotalEmployeeSSFAmount { get; set; }

        public decimal TotalEmployerSSFAmount { get; set; }
        public decimal TotalTDS { get; set; }

        public decimal TotalAllowancesAmount { get; set; }

        public string Designation { get; set; }
        public int Level { get; set; }

    }

    public class SalaryPaymentPostVm {

        public List<SalaryPaymentVm> SalaryPaymentVm { get; set; }
    }

    public class SalaryPaymentAdviceVm
    {
        public decimal Amount { get; set; }
        public string BankAccountNo { get; set; }
        public string StaffName { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
    }
}

