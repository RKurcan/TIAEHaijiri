using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class PayRollReportApiController : ApiController
    {
        string curLang = RiddhaSession.Language;
        public KendoGridResult<List<PayrollCalculationReportViewModel>> GenerateReport(KendoReportViewModel vm)
        {
            SPayrollCalculationReport reportService = new SPayrollCalculationReport();

            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            int totalRecords = employees.Count();
            employees = employees.Skip(vm.Skip).Take(vm.Take).ToArray();
            var result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt(), employees).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            return new KendoGridResult<List<PayrollCalculationReportViewModel>>()
            {
                Data = result,
                TotalCount = totalRecords,
                Status = ResultStatus.Ok
            };

        }


        private List<PayRollReportItem> populateList()
        {
            List<PayRollReportItem> Reports = new List<PayRollReportItem>() {
                new PayRollReportItem(){
                SN=1,
                Report="Employee Salary Slip Report",
                ReportId=20,
                Description="Genrate Salary Slip For Employee."
                },
                new PayRollReportItem(){
                SN=2,
                ReportId=21,
                Report="Employee PayRoll Summary Report",
                Description="Report on Employees PayRoll Summary"
                },
            };
            return Reports;
        }

        [HttpPost]
        public ServiceResult<List<InsuranceReportVm>> GenerateInsuranceReport(KendoReportViewModel args)
        {

            SEmployeeSalaryAndTaxPayable _employeeSalaryAndTaxPayableServices = new SEmployeeSalaryAndTaxPayable();
            SSalarySheet _salarySheetServices = new SSalarySheet();
            SFiscalYear _fiscalYearServices = new SFiscalYear();
            LocalizedString _loc = new LocalizedString();
            SEmployee _employee = new SEmployee();
            SEmployeeInsuranceInformation _employeeInsuranceInformationServices = new SEmployeeInsuranceInformation();
            int BranchId = (int)RiddhaSession.BranchId;
            List<EEmployee> employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                                         select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            if (args.DeptIds != null && args.DeptIds.Length > 0)
            {

                int[] depIds = args.DeptIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in depIds on c.Section.DepartmentId equals d
                             select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            }
            if (args.SectionIds != null && args.SectionIds.Length > 0)
            {

                int[] secIds = args.SectionIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in secIds on c.SectionId equals d
                             select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            }
            if (args.EmpIds != null && args.EmpIds.Length > 0)
            {

                int[] empIds = args.EmpIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in empIds on c.Id equals d
                             select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            }
            DateTime FromDate = Convert.ToDateTime(args.OnDate);
            DateTime ToDate = Convert.ToDateTime(args.ToDate);

            SCompany _companyServices = new SCompany();

            OrganizationType organizationType = _companyServices.List().Data.
                Where(x => x.Id == RiddhaSession.CompanyId).Select(x => x.OrganizationType).FirstOrDefault();


            var ExistingSalarySheet = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId).ToList()
                                       join d in employees on c.EmployeeId equals d.Id
                                       where c.FiscalYearId == args.FiscalYearId &&
                                       c.Month <= args.MonthId && c.Month >= args.ToMonthId &&
                                       c.SalaryStatus == Riddhasoft.PayRoll.Entities.SalaryStatus.Paid
                                       select c).OrderBy(x=>x.Employee.Designation.DesignationLevel).ToList();


            var result = (from c in ExistingSalarySheet
                          group c by new { c.EmployeeId } into grp
                          join d in _employeeInsuranceInformationServices.List().Data on grp.FirstOrDefault().EmployeeId equals d.EmployeeId
                          select new InsuranceReportVm()
                          {
                              EmployeeCode = grp.FirstOrDefault().EmployeeCode,
                              EmployeeName = curLang == "en" ? grp.FirstOrDefault().Employee.Name :
                              (grp.FirstOrDefault().Employee.NameNp == null ? grp.FirstOrDefault().Employee.Name : grp.FirstOrDefault().Employee.NameNp),
                              EmployeeContributionAmount = organizationType == OrganizationType.Government
                              ? grp.Sum(x => x.InsurancePremiumAmount) - grp.Sum(x => x.InsurancePaidbyOffice) : grp.Sum(x => x.InsurancePremiumAmount),
                              EmployerContributionAmount = grp.Sum(x => x.InsurancePaidbyOffice),
                              InsurancePolicyNo = d.PolicyNo,
                              TotalDeduction = organizationType == OrganizationType.Government
                              ? grp.Sum(x => x.InsurancePremiumAmount) : grp.Sum(x => x.InsurancePremiumAmount) + grp.Sum(x => x.InsurancePaidbyOffice),
                              CodeNo = "",
                              Designation = curLang == "en" ? grp.FirstOrDefault().Employee.Designation.Name :
                              (grp.FirstOrDefault().Employee.Designation.NameNp == null ? grp.FirstOrDefault().Employee.Designation.Name : grp.FirstOrDefault().Employee.Designation.NameNp),
                              Remarks = "",
                              SheetRollNo = grp.FirstOrDefault().EmployeeCode,
                          }).ToList();
            return new ServiceResult<List<InsuranceReportVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok

            };
        }
        [HttpPost]
        public ServiceResult<List<ProvidentFundReportVm>> GenerateProvidentFundReport(KendoReportViewModel args)
        {

            SEmployeeSalaryAndTaxPayable _employeeSalaryAndTaxPayableServices = new SEmployeeSalaryAndTaxPayable();
            SSalarySheet _salarySheetServices = new SSalarySheet();
            SFiscalYear _fiscalYearServices = new SFiscalYear();
            LocalizedString _loc = new LocalizedString();
            SEmployee _employee = new SEmployee();
            SEmployeeInsuranceInformation _employeeInsuranceInformationServices = new SEmployeeInsuranceInformation();
            int BranchId = (int)RiddhaSession.BranchId;
            List<EEmployee> employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                                         select c).OrderBy(x=>x.Designation.DesignationLevel).ToList();
            if (args.DeptIds != null && args.DeptIds.Length > 0)
            {

                int[] depIds = args.DeptIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in depIds on c.Section.DepartmentId equals d
                             select c).OrderBy(x=>x.Designation.DesignationLevel).ToList();
            }
            if (args.SectionIds != null && args.SectionIds.Length > 0)
            {
                int[] secIds = args.SectionIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in secIds on c.SectionId equals d
                             select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            }
            if (args.EmpIds != null && args.EmpIds.Length > 0)
            {

                int[] empIds = args.EmpIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in empIds on c.Id equals d

                             select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            }
            DateTime FromDate = Convert.ToDateTime(args.OnDate);
            DateTime ToDate = Convert.ToDateTime(args.ToDate);
            SEmployeeDocument employeeDocumentServices = new SEmployeeDocument();
            SCompany _companyServices = new SCompany();

            OrganizationType organizationType = _companyServices.List().Data.
                Where(x => x.Id == RiddhaSession.CompanyId).Select(x => x.OrganizationType).FirstOrDefault();


            var ExistingSalarySheet = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId).ToList()
                                       join d in employees on c.EmployeeId equals d.Id
                                       where c.FiscalYearId == args.FiscalYearId &&
                                       c.Month <= args.MonthId && c.Month >= args.ToMonthId &&
                                       c.SalaryStatus == Riddhasoft.PayRoll.Entities.SalaryStatus.Paid
                                       select c).OrderBy(x=>x.Employee.Designation.DesignationLevel).ToList();

            var result = (from c in ExistingSalarySheet.Where(x => (x.PFEE > 0 || x.PFER > 0))
                          group c by new { c.EmployeeId } into grp
                          join e in employeeDocumentServices.List().Data on grp.FirstOrDefault().Employee.Id equals e.EmployeeId
                          select new ProvidentFundReportVm()
                          {
                              EmployeeCode = grp.FirstOrDefault().EmployeeCode,
                              EmployeeName = curLang == "en" ? grp.FirstOrDefault().Employee.Name :
                              (grp.FirstOrDefault().Employee.NameNp == null ? grp.FirstOrDefault().Employee.Name : grp.FirstOrDefault().Employee.NameNp),
                              EmployeeContributionAmount = grp.Sum(x => x.PFEE) > 0 ?
                              (organizationType == OrganizationType.Government
                              ? grp.Sum(x => x.PFEE) - grp.Sum(x => x.PFER) : grp.Sum(x => x.PFEE)) : grp.Sum(x => x.PFEE),
                              EmployerContributionAmount = grp.Sum(x => x.PFER),
                              //ProvidentFundNo = grp.FirstOrDefault().Employee.PFNo,
                              ProvidentFundNo = e.PFNo,
                              TotalDeduction = organizationType == OrganizationType.Government
                              ? grp.Sum(x => x.PFEE) : grp.Sum(x => x.PFEE) + grp.Sum(x => x.PFER),
                              Designation = curLang == "en" ? grp.FirstOrDefault().Employee.Designation.Name :
                              (grp.FirstOrDefault().Employee.Designation.NameNp == null ? grp.FirstOrDefault().Employee.Designation.Name : grp.FirstOrDefault().Employee.Designation.NameNp),
                              Remarks = "",
                              SheetRollNo = grp.FirstOrDefault().EmployeeCode,
                          }).ToList();

            return new ServiceResult<List<ProvidentFundReportVm>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public ServiceResult<List<CITReportVm>> GenerateCITReport(KendoReportViewModel args)
        {
            SEmployeeSalaryAndTaxPayable _employeeSalaryAndTaxPayableServices = new SEmployeeSalaryAndTaxPayable();
            SSalarySheet _salarySheetServices = new SSalarySheet();
            SFiscalYear _fiscalYearServices = new SFiscalYear();
            LocalizedString _loc = new LocalizedString();
            SEmployee _employee = new SEmployee();
            SEmployeeInsuranceInformation _employeeInsuranceInformationServices = new SEmployeeInsuranceInformation();
            int BranchId = (int)RiddhaSession.BranchId;
            List<EEmployee> employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                                         select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            if (args.DeptIds != null && args.DeptIds.Length > 0)
            {

                int[] depIds = args.DeptIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in depIds on c.Section.DepartmentId equals d
                             select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            }
            if (args.SectionIds != null && args.SectionIds.Length > 0)
            {

                int[] secIds = args.SectionIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in secIds on c.SectionId equals d
                             select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            }
            if (args.EmpIds != null && args.EmpIds.Length > 0)
            {

                int[] empIds = args.EmpIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                employees = (from c in _employee.List().Data.Where(x => x.BranchId == BranchId)
                             join d in empIds on c.Id equals d
                             select c).OrderBy(x => x.Designation.DesignationLevel).ToList();
            }
            DateTime FromDate = Convert.ToDateTime(args.OnDate);
            DateTime ToDate = Convert.ToDateTime(args.ToDate);

            SCompany _companyServices = new SCompany();
            SEmployeeDocument employeeDocumentServices = new SEmployeeDocument();

            OrganizationType organizationType = _companyServices.List().Data.
                Where(x => x.Id == RiddhaSession.CompanyId).Select(x => x.OrganizationType).FirstOrDefault();


            var ExistingSalarySheet = (from c in _salarySheetServices.GetSalarySheetInfo().Data.Where(x => x.BranchId == BranchId).ToList()
                                       join d in employees on c.EmployeeId equals d.Id

                                       where c.FiscalYearId == args.FiscalYearId &&
                                       c.Month <= args.MonthId && c.Month >= args.ToMonthId &&
                                       c.SalaryStatus == Riddhasoft.PayRoll.Entities.SalaryStatus.Paid
                                       select c).OrderBy(x=>x.Employee.Designation.DesignationLevel).ToList();

            var result = (from c in ExistingSalarySheet
                          group c by new { c.EmployeeId } into grp
                          join e in employeeDocumentServices.List().Data on grp.FirstOrDefault().Employee.Id equals e.EmployeeId
                          select new CITReportVm()
                          {
                              EmployeeCode = grp.FirstOrDefault().EmployeeCode,
                              EmployeeName = curLang == "en" ? grp.FirstOrDefault().Employee.Name :
                              (grp.FirstOrDefault().Employee.NameNp == null ? grp.FirstOrDefault().Employee.Name : grp.FirstOrDefault().Employee.NameNp),
                              TotalDeduction = grp.Sum(x => x.CITAmount),
                              Designation = curLang == "en" ? grp.FirstOrDefault().Employee.Designation.Name :
                              (grp.FirstOrDefault().Employee.Designation.NameNp == null ? grp.FirstOrDefault().Employee.Designation.Name : grp.FirstOrDefault().Employee.Designation.NameNp),
                              Remarks = "",
                              //CITNo = grp.FirstOrDefault().Employee.CITNo
                              CITNo = e.CITNo
                          }).ToList();
            return new ServiceResult<List<CITReportVm>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<FiscalYearDropDownVm>> GetFiscalYears()
        {

            int branchId = (int)RiddhaSession.BranchId;
            SFiscalYear fiscalYearServices = new SFiscalYear();
            var data = (from c in fiscalYearServices.List().Data.Where(x => x.BranchId == branchId)
                        select new FiscalYearDropDownVm()
                        {
                            Id = c.Id,
                            Name = c.FiscalYear,
                            CurrentFiscalYear = c.CurrentFiscalYear
                        }).OrderByDescending(x => x.Id).ToList();
            return new ServiceResult<List<FiscalYearDropDownVm>>()
            {
                Data = data,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<EmployeeSalaryHistVm>> GenerateSalaryHistory(SalaryHistoryReportParam arg)
        {
            RiddhaDBContext db = new RiddhaDBContext();
            var result = db.SP_Employee_Salary_History(arg.FiscalYearId, arg.EmpIds);
            return new KendoGridResult<List<EmployeeSalaryHistVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = result.Count()
            };

        }
    }

    public class SalaryHistoryReportParam : KendoPageListArguments
    {
        public int FiscalYearId { get; set; }
        public string EmpIds { get; set; }
    }
    public class PayRollReportItem
    {
        public int SN { get; set; }
        public int ReportId { get; set; }
        public string Report { get; set; }
        public string Description { get; set; }
    }

    public class InsuranceReportVm
    {


        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string InsurancePolicyNo { get; set; }
        public decimal EmployeeContributionAmount { get; set; }
        public decimal EmployerContributionAmount { get; set; }

        public decimal TotalDeduction { get; set; }

        public string CodeNo { get; set; }
        public string SheetRollNo { get; set; }
        public string Remarks { get; set; }

    }
    public class ProvidentFundReportVm
    {
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string ProvidentFundNo { get; set; }
        public decimal EmployeeContributionAmount { get; set; }
        public decimal EmployerContributionAmount { get; set; }
        public decimal TotalDeduction { get; set; }
        public string SheetRollNo { get; set; }
        public string Remarks { get; set; }

    }

    public class CITReportVm
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string CITNo { get; set; }
        public decimal TotalDeduction { get; set; }
        public string Remarks { get; set; }

    }

    public class FiscalYearDropDownVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CurrentFiscalYear { get; set; }
    }
}
