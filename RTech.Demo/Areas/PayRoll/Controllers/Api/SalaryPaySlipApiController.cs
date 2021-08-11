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
    public class SalaryPaySlipApiController : ApiController
    {

        SEmployeeSalaryAndTaxPayable _employeeSalaryAndTaxPayableServices = null;
        SSalarySheet _salarySheetServices = null;
        SFiscalYear _fiscalYearServices = null;
        LocalizedString _loc = null;
        SEmployee _employee = null;
        int BranchId = (int)RiddhaSession.BranchId;
        SAllowance _allowanceServices = null;
        string CurLang = RiddhaSession.Language;
        SDesignation _designationServices = null;
        public SalaryPaySlipApiController()
        {
            _employeeSalaryAndTaxPayableServices = new SEmployeeSalaryAndTaxPayable();
            _salarySheetServices = new SSalarySheet();
            _fiscalYearServices = new SFiscalYear();
            _employee = new SEmployee();
            _loc = new LocalizedString();
            _allowanceServices = new SAllowance();
            _designationServices = new SDesignation();
        }
        [HttpGet]
        public ServiceResult<List<SalaryPaymentVm>> GetEmpSalaryPayableInfo(string DepartmentIds, string SectionIds, string EmpIds , int MonthId)
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

            employees = (from c in employees
                         join d in _designationServices.List().Data.OrderBy(x => x.DesignationLevel)
                         on c.DesignationId equals d.Id into joined 
                         from d in joined.DefaultIfEmpty()
                         select c).ToList();


            var CurrentFiscalYearId = _fiscalYearServices.List().Data.Where(x => x.BranchId == BranchId && x.CurrentFiscalYear == true).Select(x => x.Id).FirstOrDefault();

            var ExistingSalarySheet = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId).ToList()
                                       join d in employees on c.EmployeeId equals d.Id
                                       where c.FiscalYearId == CurrentFiscalYearId && c.Month == MonthId
                                       && c.SalaryStatus == Riddhasoft.PayRoll.Entities.SalaryStatus.Paid
                                       select c).ToList();




            if (ExistingSalarySheet.Count > 0)
            {
                //Renumeration Tax has been used as the ref
                //decimal RenumerationTax = 0;

                var result = (from c in ExistingSalarySheet
                              join d in _employeeSalaryAndTaxPayableServices.List().Data on c.EmployeeId equals d.EmployeeId
                              join e in employees on c.EmployeeId equals e.Id
                              where d.MonthId == c.Month && c.FiscalYearId == d.FiscalYearId 
                              select new SalaryPaymentVm()
                              {
                                  SalarySheetId = c.Id,
                                  SalaryPayableId = d.Id,
                                  MonthId = c.Month,
                                  EmployeeId = c.EmployeeId,
                                  EmployeeName = CurLang == "ne" ? (e.NameNp == null ? c.EmployeeName : e.NameNp) :  c.EmployeeName,
                                  EmployeeCode = c.EmployeeCode,
                                  JoinedMonth = e.DateOfJoin.HasValue == true ? e.DateOfJoin.GetValueOrDefault().ToString("yyyy/MM/dd") : "",
                                  DepartmentName = c.DepartmentName,
                                  Designation = c.Employee.Designation == null ? "" : 
                                  CurLang == "ne" ? (e.Designation.NameNp == null ? e.Designation.Name : e.Designation.NameNp) : e.Designation.Name,
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
                                  AdditionAmount = c.AdditionAmount,
                                  Level = e.Designation == null ? 0 : e.Designation.DesignationLevel,
                                  AbsentDeductionAmount = c.AbsentDeductionAmount,
                                  EarlyOutDeductionAmount = c.EarlyOutDeductionAmount,
                                  LateDeductionAmount = c.LateDeductionAmount
                              }).OrderBy(x => x.Level).ToList();


                if (result.Count == 0) {
                    return new ServiceResult<List<SalaryPaymentVm>>()
                    {

                        Data = null,
                        Message = _loc.Localize("No Salary has been paid"),
                        Status = ResultStatus.processError
                    };
                }

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
         
            return result;

        }

        [HttpPost]
        public ServiceResult<SalaryPaymentPostVm> Post(SalaryPaymentPostVm vm)
        {



            if (vm.SalaryPaymentVm.Count > 0)
            {

                //var empSalarySheets = (from c in _salarySheetServices.GetSalarySheetInfo().Data
                //                       join d in vm.SalaryPaymentVm on c.Id equals d.SalarySheetId
                //                       select c).ToList();
                var empSalaryPayables = (from c in _employeeSalaryAndTaxPayableServices.List().Data.ToList()
                                         join d in vm.SalaryPaymentVm.ToList() on c.Id equals d.SalaryPayableId
                                         select c).ToList();

                foreach (var empSalarypayable in empSalaryPayables)
                {


                    // update salary payable status to paid 

                    empSalarypayable.PaySlipGenerated = true;
                    empSalarypayable.PaySlipGeneratedById = RiddhaSession.UserId;
                    empSalarypayable.PaySlipGeneratedDateTime = DateTime.Now;
                    _employeeSalaryAndTaxPayableServices.Update(empSalarypayable);
                }


                return new ServiceResult<SalaryPaymentPostVm>()
                {
                    Data = vm,
                    Message = _loc.Localize("PaySlip Generated Successfully"),
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<SalaryPaymentPostVm>()
            {

                Data = null,
                Message = _loc.Localize("No Salary list to generate payslip"),
                Status = ResultStatus.processError
            };

        }
    }
}
