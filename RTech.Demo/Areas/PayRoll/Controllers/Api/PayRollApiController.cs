using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class PayRollApiController : ApiController
    {
        SPayRollSetup payRollSetupServices = null;
        SEmployee employeeServices = null;
        SAllowance _allowanceServices = null;
        SDeduction _deductionServices = null;
        SUser userServices = null;
        SGradeGroup _gradeGroupServices = null;
        STaxSetup _taxSetupServices = null;
        SFiscalYear _fiscalYearServices = null;
        public PayRollApiController()
        {
            payRollSetupServices = new SPayRollSetup();
            employeeServices = new SEmployee();
            _allowanceServices = new SAllowance();
            _deductionServices = new SDeduction();
            userServices = new SUser();
            _gradeGroupServices = new SGradeGroup();
            _taxSetupServices = new STaxSetup();
            _fiscalYearServices = new SFiscalYear();
        }
        public ServiceResult<List<EPayRollSetup>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            List<EPayRollSetup> payrollList = payRollSetupServices.List().Data.Where(x => x.Employee.BranchId == branchId).ToList();
            return new ServiceResult<List<EPayRollSetup>>()
            {
                Data = payrollList,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public KendoGridResult<List<EmployeeGridVm>> GetEmpKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            SEmployee service = new SEmployee();
            IQueryable<EEmployee> empQuery = service.List().Data.Where(x => x.BranchId == branchId && x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated);
            int totalRowNum = empQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            IQueryable<EEmployee> paginatedQuery;
            switch (searchField)
            {
                case "IdCardNo":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "DepartmentName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "Section":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "Mobile":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Mobile.StartsWith(searchValue)).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Mobile == searchValue).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "DesignationName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Designation.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Designation.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                default:
                    paginatedQuery = empQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    break;
            }
            var gradeGroupQuery = _gradeGroupServices.List().Data;
            var results = payRollSetupServices.ListEmpGradeGroup().Data.Where(x => x.BranchId == branchId && x.IsApproved && DbFunctions.TruncateTime(x.EffectedTo) >= DbFunctions.TruncateTime(DateTime.Now)).ToList();
            var employeelist = (from c in paginatedQuery.ToList()
                                join d in results on c.Id equals d.EmployeeId into ps
                                from d in ps.DefaultIfEmpty(new EEmployeeGrade())
                                select new EmployeeGridVm()
                                {
                                    Id = c.Id,
                                    IdCardNo = c.Code,
                                    DateOfJoin = c.DateOfJoin,
                                    DepartmentName = c.Section == null ? "" : c.Section.Department.Code + " - " + (!string.IsNullOrEmpty(c.Section.Department.NameNp) && language == "ne" ? c.Section.Department.NameNp : c.Section.Department.Name),
                                    EmployeeName = !string.IsNullOrEmpty(c.NameNp) ? c.Name + "  (" + c.NameNp + ")" : c.Name,
                                    Mobile = c.Mobile,
                                    Email = c.Email,
                                    PhotoURL = c.ImageUrl,
                                    SectionId = c.SectionId,
                                    Section = c.Section == null ? "" : c.Section.Code + " - " + (!string.IsNullOrEmpty(c.Section.NameNp) && language == "ne" ? c.Section.NameNp : c.Section.Name),
                                    DepartmentId = c.Section == null ? 0 : c.Section.DepartmentId,
                                    DesignationName = c.Designation == null ? "" : c.Designation.Code + " - " + (language == "ne" ? c.Designation.NameNp : c.Designation.Name),
                                    GradeGroupName = getGradeGroupName(gradeGroupQuery, ps.FirstOrDefault()),

                                }).ToList();
            return new KendoGridResult<List<EmployeeGridVm>>()
            {
                Data = employeelist,
                Status = ResultStatus.Ok,
                TotalCount = totalRowNum
            };
        }

        private string getGradeGroupName(IQueryable<EGradeGroup> gradeGroupQuery, EEmployeeGrade eEmployeeGrade)
        {
            if (eEmployeeGrade == null)
            {
                return "";
            }
            else
            {
                int gradegroupId = eEmployeeGrade.GradeGroupId;
                return gradeGroupQuery.Where(x => x.Id == gradegroupId).FirstOrDefault().Name;
            }
        }
        [HttpGet]
        public ServiceResult<List<AllowanceDropdownVm>> GetAllowanceForDropdown()
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            var resultLst = (from c in _allowanceServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                             select new AllowanceDropdownVm()
                             {
                                 Id = c.Id,
                                 Name = c.Code + " - " + c.Name,
                                 AllowanceCalculatedBy = c.AllowanceCalculatedBy,
                                 AllowancePaidPer = c.AllowancePaidPer,
                                 Value = c.Value.ToString(),
                                 MinimumWorkingHour = c.MinimumWorkingHour.ToString(@"hh\:mm"),
                                 AllowancePeriod = c.AllowancePeriod,
                             }).ToList();
            return new ServiceResult<List<AllowanceDropdownVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<EDeduction>> GetDeductionForDropdown()
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            var resultLst = (from c in _deductionServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                             select new EDeduction()
                             {
                                 Id = c.Id,
                                 Name = c.Code + " - " + c.Name,
                                 DeductionCalculatedBy = c.DeductionCalculatedBy,
                                 DeductionPaidPer = c.DeductionPaidPer,
                                 Value = c.Value
                             }).ToList();
            return new ServiceResult<List<EDeduction>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EmpSearchViewModel> SearchEmployee(string empCode, string empName)
        {
            EmpSearchViewModel vm = new EmpSearchViewModel();
            var empList = employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            EEmployee employee = new EEmployee();
            if (empCode != null)
            {
                employee = empList.Where(x => x.Code == empCode).FirstOrDefault();
            }
            else if (empName != null)
            {
                employee = empList.Where(x => x.Name.ToUpper().Contains(empName.ToUpper())).FirstOrDefault();
            }
            if (employee != null)
            {
                vm.Id = employee.Id;
                vm.Code = employee.Code;
                vm.Name = employee.Name;
                vm.Designation = employee.Designation.Name;
                vm.Department = employee.Section.Department.Name;
                vm.Section = employee.Section.Name;
                vm.Photo = employee.ImageUrl;
            }
            return new ServiceResult<EmpSearchViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EPayRollSetup> Get(int id)
        {
            EPayRollSetup payRollSetup = payRollSetupServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EPayRollSetup>()
            {
                Data = payRollSetup,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<EPayRollSetup>> GetPayrollHist(int empId)
        {
            var payrollHist = payRollSetupServices.List().Data.Where(x => x.EmployeeId == empId).OrderBy(x => x.Id).ToList();
            return new ServiceResult<List<EPayRollSetup>>()
            {
                Data = payrollHist,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<EPayRollSetup>> Post(EPayRollSetup model)
        {
            int userId = RiddhaSession.UserId;
            model.CreatedById = userId;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.BranchId = (int)RiddhaSession.BranchId;
            model.IsApproved = false;
            var result = payRollSetupServices.Add(model);
            List<EPayRollSetup> payrollHist = new List<EPayRollSetup>();
            if (result.Status == ResultStatus.Ok)
            {
                payrollHist = payRollSetupServices.List().Data.Where(x => x.EmployeeId == model.EmployeeId).OrderByDescending(x => x.Id).ToList();
                Common.AddAuditTrail("8003", "7210", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, result.Message);
            }
            return new ServiceResult<List<EPayRollSetup>>()
            {
                Data = payrollHist,
                Status = result.Status,
                Message = result.Message
            };
        }
        public ServiceResult<List<EPayRollSetup>> Put(EPayRollSetup model)
        {
            model.Employee = null;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.CreatedById = RiddhaSession.UserId;
            model.BranchId = (int)RiddhaSession.BranchId;
            model.IsApproved = false;
            var result = payRollSetupServices.Update(model);
            List<EPayRollSetup> payrollHist = new List<EPayRollSetup>();
            if (result.Status == ResultStatus.Ok)
            {
                payrollHist = payRollSetupServices.List().Data.Where(x => x.EmployeeId == model.EmployeeId).OrderByDescending(x => x.Id).ToList();
                Common.AddAuditTrail("8003", "7211", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, result.Message);
            }
            return new ServiceResult<List<EPayRollSetup>>()
            {
                Data = payrollHist,
                Status = result.Status,
                Message = result.Message
            };
        }
        [HttpDelete]
        public ServiceResult<List<EPayRollSetup>> Delete(int id)
        {
            var payRollSetup = payRollSetupServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = payRollSetupServices.Remove(payRollSetup);
            List<EPayRollSetup> payrollHist = new List<EPayRollSetup>();
            if (result.Status == ResultStatus.Ok)
            {
                payrollHist = payRollSetupServices.List().Data.Where(x => x.EmployeeId == payRollSetup.EmployeeId).OrderByDescending(x => x.Id).ToList();
                Common.AddAuditTrail("8003", "7211", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, result.Message);
            }
            return new ServiceResult<List<EPayRollSetup>>()
            {
                Data = payrollHist,
                Status = result.Status,
                Message = result.Message
            };
        }

        #region Payroll Employee Verification

        [HttpPost]
        public KendoGridResult<List<PayrollVerificationKendoVm>> GetPayrollKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            var userQuery = userServices.List().Data;
            IQueryable<EPayRollSetup> payrollQuery = payRollSetupServices.List().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = payrollQuery.Count();
            var lst = payrollQuery.OrderByDescending(x => x.Id).Skip(vm.Skip).Take(vm.Take).ToList();
            var payrollLst = (from c in lst
                              join u in userQuery
                                  on new { User = c.ApprovedById } equals new { User = (int?)u.Id } into lj
                              from x in lj.DefaultIfEmpty()
                              select new PayrollVerificationKendoVm()
                              {
                                  Id = c.Id,
                                  EmployeeId = c.EmployeeId,
                                  EmployeeName = c.Employee.Code + " - " + c.Employee.Name,
                                  Department = c.Employee.Section == null ? "" : !string.IsNullOrEmpty(c.Employee.Section.Department.NameNp) && language == "ne" ? c.Employee.Section.Department.NameNp : c.Employee.Section.Department.Name,
                                  Section = c.Employee.Section == null ? "" : !string.IsNullOrEmpty(c.Employee.Section.NameNp) && language == "ne" ? c.Employee.Section.NameNp : c.Employee.Section.Name,
                                  Designation = c.Employee.Designation == null ? "" : !string.IsNullOrEmpty(c.Employee.Designation.NameNp) && language == "ne" ? c.Employee.Designation.NameNp : c.Employee.Designation.Name,
                                  EffectedFrom = c.EffectedFrom.ToString("yyyy/MM/dd"),
                                  BasicSalary = c.BasicSalary,
                                  ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                  ApprovedBy = x == null ? "" : x.Name,
                                  ApprovedById = c.ApprovedById,
                                  IsApproved = c.IsApproved
                              }).ToList();
            return new KendoGridResult<List<PayrollVerificationKendoVm>>()
            {
                Data = payrollLst,
                Status = ResultStatus.Ok,
                TotalCount = totalRowNum
            };
        }

        [HttpGet]
        public ServiceResult<List<PayrollVerificationVm>> GetVerificationPayrollByEmpId(int empId)
        {

            var parolllist = (from c in payRollSetupServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                              select new PayrollVerificationVm()
                              {
                                  Id = c.Id,
                                  EmployeeName = !string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.Name + "  (" + c.Employee.NameNp + ")" : c.Employee.Name,
                                  EmployeeId = c.EmployeeId,
                                  BasicSalry = c.BasicSalary,
                                  CITRate = c.CITRate,
                                  EffectedFrom = c.EffectedFrom.ToString("yyyy/MM/dd"),
                                  GrossAmount = c.GrossAmount,
                                  OtPayPer = Enum.GetName(typeof(PayRatePer), c.OTPayPer),
                                  OtRatePerHour = c.OtRatePerHour,
                                  PFRate = c.PFRate,
                                  SalaryPaidPer = Enum.GetName(typeof(SalaryPaidPer), c.SalaryPaidPer),
                                  TDS = c.TDS,
                                  TDSPaidBy = Enum.GetName(typeof(TdsPaidBy), c.TdsPaidBy),
                                  ApprovedById = c.ApprovedById
                              }).ToList();
            return new ServiceResult<List<PayrollVerificationVm>>()
            {
                Data = parolllist,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EPayRollSetup> Approve(int id, int empId)
        {
            string msg = "";
            var payroll = payRollSetupServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (payroll != null)
            {
                if (payroll.IsApproved == false)
                {
                    payroll.ApprovedById = RiddhaSession.CurrentUser.Id;
                    payroll.ApprovedOn = System.DateTime.Now;
                    payroll.IsApproved = true;
                    var result = payRollSetupServices.Update(payroll);
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EPayRollSetup>()
            {
                Data = payroll,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EPayRollSetup> Revert(int id, int empId)
        {
            string msg = "";
            var payroll = payRollSetupServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (payroll != null)
            {
                if (payroll.IsApproved)
                {
                    payroll.ApprovedById = null;
                    payroll.ApprovedOn = null;
                    payroll.IsApproved = false;
                    var result = payRollSetupServices.Update(payroll);
                    msg = "Reverted Successfully";
                }
                else
                {
                    msg = "Allowance has not been approved yet.";
                }
            }
            return new ServiceResult<EPayRollSetup>()
            {
                Data = payroll,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<UnapprovedPayrollCountModel> GetUnapprovedCount()
        {
            return payRollSetupServices.GetUnapprovedCount((int)RiddhaSession.BranchId);
        }


        #endregion

        #region Employee Grade upgrade
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetGradeGroupsForDropdown()
        {
            int companyId = RiddhaSession.CompanyId;
            var gradeGroups = _gradeGroupServices.List().Data.Where(x => x.CompanyId == companyId);
            List<DropdownViewModel> resultLst = (from c in gradeGroups
                                                 select new DropdownViewModel()
                                                 {
                                                     Name = c.Code + " - " + c.Name,
                                                     Id = c.Id
                                                 }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public ServiceResult<object> GradeUpgradeEmployee(EmpGradeUpgradeModel model)
        {
            model.EmployeeGrade.CreatedOn = DateTime.Now;
            model.EmployeeGrade.CreatedById = RiddhaSession.UserId;
            model.EmployeeGrade.BranchId = RiddhaSession.BranchId ?? 0;
            var result = payRollSetupServices.GradeUpgradeEmployee(model);
            return result;
        }

        [HttpGet]
        public ServiceResult<List<EmpGradeVm>> GetEmpGradeUpgrades(int empId)
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            var EmpGrades = payRollSetupServices.ListEmpGradeGroup().Data.Where(x => x.EmployeeId == empId).ToList();

            SUser userService = new SUser();
            var users = userService.List().Data.ToList();

            var result = (from c in EmpGrades
                          join d in users
                            on c.ApprovedById equals d.Id into temp
                          from e in temp.DefaultIfEmpty()
                          select new EmpGradeVm()
                          {
                              Id = c.Id,
                              GradeGroupId = c.GradeGroupId,
                              EffectedFrom = c.EffectedFrom.ToString(),
                              EffectedTo = c.EffectedTo.ToString(),
                              ApprovedBy = e == null ? "Not approved" : e.Name,
                          }).ToList();
            return new ServiceResult<List<EmpGradeVm>>()
            {
                Data = result,
            };
        }

        [HttpPost]
        public KendoGridResult<List<EmpGradeVm>> GetGradeGroupKendoGrid()
        {
            int branchId = (int)RiddhaSession.BranchId;
            int companyId = RiddhaSession.CompanyId;
            var gradeGroupsQuery = payRollSetupServices.ListEmpGradeGroup().Data.Where(x => x.BranchId == branchId).ToList();

            var gradeGroups = _gradeGroupServices.List().Data.Where(x => x.CompanyId == companyId);

            SUser userService = new SUser();
            var users = userService.List().Data.ToList();

            List<EmpGradeVm> empGrades = (from c in gradeGroupsQuery
                                          join f in gradeGroups
                                             on c.GradeGroupId equals f.Id
                                          join d in users
                                            on c.ApprovedById equals d.Id into temp
                                          from e in temp.DefaultIfEmpty()
                                          select new EmpGradeVm()
                                          {
                                              Id = c.Id,
                                              EmployeeId = c.EmployeeId,
                                              EmployeeName = c.Employee.Name,
                                              GradeGroupId = c.GradeGroupId,
                                              GradeGroupName = f.Name,
                                              EffectedFrom = c.EffectedFrom.ToString("yyyy/MM/dd"),
                                              EffectedTo = c.EffectedTo.ToString("yyyy/MM/dd"),
                                              ApprovedById = c.ApprovedById,
                                              ApprovedBy = e == null ? "" : e.Name,
                                              ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                              IsApproved = c.IsApproved
                                          }).ToList();
            return new KendoGridResult<List<EmpGradeVm>>()
            {
                Data = empGrades.OrderBy(x => x.IsApproved).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = empGrades.Count(),
            };
        }
        [HttpPut]
        public ServiceResult<int> UpdateEmpGradeGroup(EmpGradeVm model)
        {
            var empGradeGroup = payRollSetupServices.ListEmpGradeGroup().Data.Where(x => x.Id == model.Id).FirstOrDefault();
            empGradeGroup.GradeGroupId = model.GradeGroupId;
            empGradeGroup.EffectedFrom = DateTime.Parse(model.EffectedFrom);
            empGradeGroup.EffectedTo = DateTime.Parse(model.EffectedTo);
            empGradeGroup.IsApproved = false;
            empGradeGroup.ApprovedById = null;
            empGradeGroup.ApprovedOn = null;
            var result = payRollSetupServices.UpdateEmpGradeGroup(empGradeGroup);
            return new ServiceResult<int>()
            {
                Data = 1,
                Status = result.Status,
                Message = result.Message
            };
        }

        [HttpGet]
        public ServiceResult<List<EmpGradeVm>> DeleteEmpGradeGroup(int id)
        {
            var EmpGradeGroup = payRollSetupServices.ListEmpGradeGroup().Data.Where(x => x.Id == id).FirstOrDefault();
            if (EmpGradeGroup.IsApproved)
            {
                return new ServiceResult<List<EmpGradeVm>>()
                {
                    Status = ResultStatus.processError,
                    Message = "Approved grade groups cannot be deleted."
                };
            }
            var result = payRollSetupServices.RemoveEmpGradeGroup(EmpGradeGroup);
            List<EmpGradeVm> gradeGroupHist = new List<EmpGradeVm>();
            if (result.Status == ResultStatus.Ok)
            {
                gradeGroupHist = (from c in payRollSetupServices.ListEmpGradeGroup().Data.Where(x => x.EmployeeId == EmpGradeGroup.EmployeeId).ToList()
                                  select new EmpGradeVm()
                                  {
                                      Id = c.Id,
                                      GradeGroupId = c.GradeGroupId,
                                      EffectedFrom = c.EffectedFrom.ToString(),
                                      EffectedTo = c.EffectedTo.ToString(),
                                      ApprovedBy = c.ApprovedById.ToString(),
                                  }).ToList();
            }

            return new ServiceResult<List<EmpGradeVm>>()
            {
                Data = gradeGroupHist,
                Status = result.Status,
                Message = result.Message
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeGrade> ApproveGradeGroup(int id)
        {
            string msg = "";
            var empGrade = payRollSetupServices.ListEmpGradeGroup().Data.Where(x => x.Id == id).FirstOrDefault();
            if (empGrade != null)
            {
                if (empGrade.IsApproved == false)
                {
                    empGrade.ApprovedById = RiddhaSession.CurrentUser.Id;
                    empGrade.ApprovedOn = System.DateTime.Now;
                    empGrade.IsApproved = true;
                    var result = payRollSetupServices.UpdateEmpGradeGroup(empGrade);
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EEmployeeGrade>()
            {
                Data = empGrade,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeGrade> RevertGradeGroup(int id)
        {
            string msg = "";
            var empGrade = payRollSetupServices.ListEmpGradeGroup().Data.Where(x => x.Id == id).FirstOrDefault();
            if (empGrade != null)
            {
                if (empGrade.IsApproved)
                {
                    empGrade.ApprovedById = null;
                    empGrade.ApprovedOn = null;
                    empGrade.IsApproved = false;
                    var result = payRollSetupServices.UpdateEmpGradeGroup(empGrade);
                    msg = "Reverted Successfully";
                }
                else
                {
                    msg = "Already Reverted";
                }
            }
            return new ServiceResult<EEmployeeGrade>()
            {
                Data = empGrade,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }
        #endregion


        #region Tax Services 

        public ServiceResult<TaxInfoOfCurrentFiscalYear> GetTaxInfoOfCurrentFiscalYear()
        {

            var CurrentFiscalYearId = _fiscalYearServices.List().Data.Where(x => x.CurrentFiscalYear == true).FirstOrDefault();
            var result = (from c in _taxSetupServices.List().Data.Where(x => x.FiscalYearId == CurrentFiscalYearId.Id)
                          select new TaxInfoOfCurrentFiscalYear()
                          {
                              Gratituity = c.GratituityPercByEmployer,
                              PFEmployee = c.PFPercByEmployee,
                              PFEmployer = c.PFPercByEmployer,
                              SSEpmployee = c.SSPercByEmployee,
                              SSEpmployer = c.SSPercByEmployer
                          }).FirstOrDefault();
            return new ServiceResult<TaxInfoOfCurrentFiscalYear>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
        #endregion
    }

    public class PayrollVerificationKendoVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string EffectedFrom { get; set; }
        public decimal BasicSalary { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsApproved { get; set; }
        public int? ApprovedById { get; set; }
    }

    public class PayrollVerificationVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal BasicSalry { get; set; }
        public string EffectedFrom { get; set; }
        public string SalaryPaidPer { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal OtRatePerHour { get; set; }
        public string OtPayPer { get; set; }
        public decimal TDS { get; set; }
        public string TDSPaidBy { get; set; }
        public decimal? CITRate { get; set; }
        public decimal? PFRate { get; set; }
        public int? ApprovedById { get; set; }
    }

    public class AllowanceDropdownVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string MinimumWorkingHour { get; set; }
        public AllowancePeriod AllowancePeriod { get; set; }
        public AllowanceCalculatedBy AllowanceCalculatedBy { get; set; }
        public AllowancePaidPer AllowancePaidPer { get; set; }
    }

    public class EmpGradeVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int GradeGroupId { get; set; }
        public string GradeGroupName { get; set; }
        public string EffectedFrom { get; set; }
        public string EffectedTo { get; set; }
        public int? ApprovedById { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
    }

    public class TaxInfoOfCurrentFiscalYear
    {

        public decimal PFEmployee { get; set; }

        public decimal PFEmployer { get; set; }
        public decimal SSEpmployee { get; set; }

        public decimal SSEpmployer { get; set; }

        public decimal Gratituity { get; set; }


    }
}
