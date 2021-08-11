using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class DeductionApiController : ApiController
    {
        SDeduction _deductionServices = null;
        SUser _userServices = null;
        LocalizedString loc = null;
        public DeductionApiController()
        {
            _deductionServices = new SDeduction();
            _userServices = new SUser();
            loc = new LocalizedString();
        }
        public ServiceResult<List<EDeduction>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var deductions = _deductionServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            return new ServiceResult<List<EDeduction>>()
            {
                Data = deductions,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EDeduction> Get(int id)
        {
            EDeduction deduction = _deductionServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EDeduction>()
            {
                Data = deduction,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EDeduction> Post([FromBody]EDeduction model)
        {
            model.BranchId = RiddhaSession.BranchId ?? 0;
            var result = _deductionServices.Add(model);
            return new ServiceResult<EDeduction>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EDeduction> Put([FromBody]EDeduction model)
        {
            model.BranchId = RiddhaSession.BranchId ?? 0;
            var result = _deductionServices.Update(model);
            return new ServiceResult<EDeduction>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var bank = _deductionServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _deductionServices.Remove(bank);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        #region Employee Deduction
        [HttpGet]
        public ServiceResult<List<EEmployeeDeduction>> GetEmpDeduction(int empId)
        {
            var result = _deductionServices.ListEmpDeduction().Data.Where(x => x.EmployeeId == empId).ToList();
            return new ServiceResult<List<EEmployeeDeduction>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public ServiceResult<EEmployeeDeduction> AddEmpDeduction([FromBody]EEmployeeDeduction model)
        {
            model.BranchId = RiddhaSession.BranchId ?? 0;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.IsApproved = false;
            var result = _deductionServices.AddEmpDeduction(model);
            return new ServiceResult<EEmployeeDeduction>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPut]
        public ServiceResult<EEmployeeDeduction> UpdateEmpDeduction([FromBody]EEmployeeDeduction model)
        {
            model.BranchId = RiddhaSession.BranchId ?? 0;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.IsApproved = false;
            var result = _deductionServices.UpdateEmpDeduction(model);
            return new ServiceResult<EEmployeeDeduction>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<int> DeleteEmpDeduction(int id)
        {
            var empDeduction = _deductionServices.ListEmpDeduction().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _deductionServices.RemoveEmpDeduction(empDeduction);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        #endregion

        #region Employee Deduction  Verification
        [HttpPost]
        public KendoGridResult<List<DeductionVerificationKendoVm>> GetDeductionKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            var userQuery = _userServices.List().Data;
            IQueryable<EEmployeeDeduction> deducQuery = _deductionServices.ListEmpDeduction().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = deducQuery.Count();
            var lst = deducQuery.OrderByDescending(x => x.Id).Skip(vm.Skip).Take(vm.Take).ToList();
            var deducLst = (from c in lst
                            join u in userQuery
                                on new { User = c.ApprovedById } equals new { User = (int?)u.Id } into lj
                            from x in lj.DefaultIfEmpty()
                            select new DeductionVerificationKendoVm()
                            {
                                Id = c.Id,
                                EmployeeId = c.EmployeeId,
                                Employee = c.Employee.Code + " - " + c.Employee.Name,
                                Department = c.Employee.Section == null ? "" : !string.IsNullOrEmpty(c.Employee.Section.Department.NameNp) && language == "ne" ? c.Employee.Section.Department.NameNp : c.Employee.Section.Department.Name,
                                Section = c.Employee.Section == null ? "" : !string.IsNullOrEmpty(c.Employee.Section.NameNp) && language == "ne" ? c.Employee.Section.NameNp : c.Employee.Section.Name,
                                Value = c.DeductionCalculatedBy == DeductionCalculatedBy.Percentage ? c.Value.ToString() + " %" : c.Value.ToString(),
                                DeductionPaidPer = c.DeductionPaidPer.ToString(),
                                Deduction = c.Deduction.Name,
                                ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                Designation = c.Employee.Designation== null ? "" : c.Employee.Designation.Name,
                                ApprovedBy = x == null ? "" : x.Name,
                                ApprovedById = c.ApprovedById,
                                IsApproved = c.IsApproved
                            }).ToList();
            return new KendoGridResult<List<DeductionVerificationKendoVm>>()
            {
                Data = deducLst,
                Status = ResultStatus.Ok,
                TotalCount = totalRowNum
            };
        }

        [HttpGet]
        public ServiceResult<List<DeductionVerificationVm>> GetVerificationDeductionByEmpId(int empId)
        {
            var deductionLst = (from c in _deductionServices.ListEmpDeduction().Data.Where(x => x.EmployeeId == empId).ToList()
                                select new DeductionVerificationVm()
                                {
                                    Id = c.Id,
                                    EmployeeName = !string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.Name + "  (" + c.Employee.NameNp + ")" : c.Employee.Name,
                                    EmployeeId = c.EmployeeId,
                                    DeductionCalculatedBy = Enum.GetName(typeof(DeductionCalculatedBy), c.DeductionCalculatedBy),
                                    DeductionId = c.DeductionId,
                                    DeductionName = c.Deduction.Name,
                                    DeductionPaidPer = Enum.GetName(typeof(DeductionPaidPer), c.DeductionPaidPer),
                                    ApprovedById = c.ApprovedById,
                                }).ToList();
            return new ServiceResult<List<DeductionVerificationVm>>()
            {
                Data = deductionLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeDeduction> Approve(int id, int empId)
        {
            string msg = "";
            var deduction = _deductionServices.ListEmpDeduction().Data.Where(x => x.Id == id).FirstOrDefault();
            if (deduction != null)
            {
                if (deduction.IsApproved == false)
                {
                    deduction.ApprovedById = RiddhaSession.CurrentUser.Id;
                    deduction.ApprovedOn = System.DateTime.Now;
                    deduction.IsApproved = true;
                    var result = _deductionServices.UpdateEmpDeduction(deduction);
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EEmployeeDeduction>()
            {
                Data = deduction,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeDeduction> Revert(int id, int empId)
        {
            string msg = "";
            var deduction = _deductionServices.ListEmpDeduction().Data.Where(x => x.Id == id).FirstOrDefault();
            if (deduction != null)
            {
                if (deduction.IsApproved)
                {
                    deduction.ApprovedById = null;
                    deduction.ApprovedOn = null;
                    deduction.IsApproved = false;
                    var result = _deductionServices.UpdateEmpDeduction(deduction);
                    msg = "Reverted Successfully";
                }
                else
                {
                    msg = "Already Reverted";
                }
            }
            return new ServiceResult<EEmployeeDeduction>()
            {
                Data = deduction,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }
        #endregion
    }
    public class DeductionVerificationKendoVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Deduction { get; set; }
        public string Value { get; set; }
        public string DeductionPaidPer { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public int? ApprovedById { get; set; }
        public bool IsApproved { get; set; }
    }

    public class DeductionVerificationVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int DeductionId { get; set; }
        public string DeductionName { get; set; }
        public string DeductionCalculatedBy { get; set; }
        public string DeductionPaidPer { get; set; }
        public int? ApprovedById { get; set; }
    }
}
