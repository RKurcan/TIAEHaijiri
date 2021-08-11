using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class AllowanceApiController : ApiController
    {
        SAllowance _allowanceServices = null;

        SUser _userServices = null;
        LocalizedString loc = null;
        public AllowanceApiController()
        {
            _allowanceServices = new SAllowance();
            _userServices = new SUser();
            loc = new LocalizedString();
        }
        public ServiceResult<List<EAllowance>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var allowances = _allowanceServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            return new ServiceResult<List<EAllowance>>()
            {
                Data = allowances,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public KendoGridResult<List<AllowanceGridVm>> GetAllowanceKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;

            List<EAllowance> allowanceQuery = _allowanceServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var allowanceList = (from c in allowanceQuery
                                 select new AllowanceGridVm()
                                   {
                                       Id = c.Id,
                                       Code = c.Code,
                                       Name = c.Name,
                                       Value = c.Value,
                                       NameNp = c.NameNp,
                                       ValueName = getValue(c.AllowanceCalculatedBy,c.Value),
                                       AllowancePeriod = c.AllowancePeriod,
                                       AllowancePeriodName = Enum.GetName(typeof(AllowancePeriod), c.AllowancePeriod),
                                       MinimumWorkingHour = c.MinimumWorkingHour.ToString(@"hh\:mm"),
                                       AllowanceCalculatedBy = c.AllowanceCalculatedBy,
                                       AllowanceCalculatedByName = Enum.GetName(typeof(AllowanceCalculatedBy), c.AllowanceCalculatedBy),
                                       AllowancePaidPer = c.AllowancePaidPer,
                                       AllowancePaidPerName = getAllowancePaidPer(c.AllowanceCalculatedBy,c.AllowancePaidPer)
                                   }).OrderBy(x => x.Id).ToList();
            return new KendoGridResult<List<AllowanceGridVm>>()
            {
                Data = allowanceList,
                Status = ResultStatus.Ok,
                TotalCount = allowanceList.Count()
            };
        }

        private string getAllowancePaidPer(AllowanceCalculatedBy allownceCalculatedBy, AllowancePaidPer allowancePaidper)
        {
            string Paidper = "";
            if((int)allownceCalculatedBy == 1){
                Paidper = Enum.GetName(typeof(AllowancePaidPer), allowancePaidper);
            }
            return Paidper;
        }
        private string getValue(AllowanceCalculatedBy allownceCalculatedBy,decimal Value)
        {
            string value = "";
            if((int)allownceCalculatedBy == 0){
                value = Value.ToString();
            }
            else
            {
                value = Value.ToString() + " %";
            }
            return value;
        }

        public ServiceResult<EAllowance> Get(int id)
        {
            EAllowance bank = _allowanceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EAllowance>()
            {
                Data = bank,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EAllowance> Post([FromBody]EAllowance model)
        {
            model.BranchId = RiddhaSession.BranchId ?? 0;
            var result = _allowanceServices.Add(model);
            return new ServiceResult<EAllowance>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EAllowance> Put([FromBody]EAllowance model)
        {
            model.BranchId = RiddhaSession.BranchId ?? 0;
            var result = _allowanceServices.Update(model);
            return new ServiceResult<EAllowance>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var allowance = _allowanceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _allowanceServices.Remove(allowance);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
            
        }
        #region Employee Allowance
        [HttpGet]
        public ServiceResult<List<EEmployeeAlowance>> GetEmpAllowance(int empId)
        {
            var result = _allowanceServices.ListEmpAllowance().Data.Where(x => x.EmployeeId == empId).ToList();
            return new ServiceResult<List<EEmployeeAlowance>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public ServiceResult<EEmployeeAlowance> AddEmpAllowance([FromBody]EEmployeeAlowance model)
        {
            model.BranchId = RiddhaSession.BranchId ?? 0;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.IsApproved = false;
            var result = _allowanceServices.AddEmpAllowance(model);
            return new ServiceResult<EEmployeeAlowance>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPut]
        public ServiceResult<EEmployeeAlowance> UpdateEmpAllowance([FromBody]EEmployeeAlowance model)
        {
            model.BranchId = RiddhaSession.BranchId ?? 0;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.IsApproved = false;
            var result = _allowanceServices.UpdateEmpAllowance(model);
            return new ServiceResult<EEmployeeAlowance>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<int> DeleteEmpAllowance(int id)
        {
            var empAllowance = _allowanceServices.ListEmpAllowance().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _allowanceServices.RemoveEmpAllowance(empAllowance);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        #endregion

        #region Employee Allowance Verification
        [HttpPost]
        public KendoGridResult<List<AllowanceVerificationKendoVm>> GetEmpAllowanceKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            var userQuery = _userServices.List().Data;
            IQueryable<EEmployeeAlowance> allowQuery = _allowanceServices.ListEmpAllowance().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = allowQuery.Count();
            var lst = allowQuery.OrderByDescending(x => x.Id).Skip(vm.Skip).Take(vm.Take).ToList();
            var allowLst = (from c in lst
                            join u in userQuery
                                on new { User = c.ApprovedById } equals new { User = (int?)u.Id } into lj
                            from x in lj.DefaultIfEmpty()
                            select new AllowanceVerificationKendoVm()
                            {
                                Id = c.Id,
                                EmployeeId = c.EmployeeId,
                                Employee = c.Employee.Code + " - " + c.Employee.Name,
                                Designation = c.Employee.Designation== null? "" : c.Employee.Designation.Name,
                                Department = c.Employee.Section == null ? "" : !string.IsNullOrEmpty(c.Employee.Section.Department.NameNp) && language == "ne" ? c.Employee.Section.Department.NameNp : c.Employee.Section.Department.Name,
                                Section = c.Employee.Section == null ? "" : !string.IsNullOrEmpty(c.Employee.Section.NameNp) && language == "ne" ? c.Employee.Section.NameNp : c.Employee.Section.Name,
                                Value = c.AllowanceCalculatedBy == AllowanceCalculatedBy.Percentage ? c.Value.ToString() + " %" : c.Value.ToString(),
                                AllowancePaidPer = c.AllowanceCalculatedBy == AllowanceCalculatedBy.Percentage ? c.AllowancePaidPer.ToString() : "",
                                Allowance = c.Allowance.Name,
                                ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                ApprovedBy = x == null ? "" : x.Name,
                                ApprovedById = c.ApprovedById,
                                IsApproved = c.IsApproved
                            }).ToList();
            return new KendoGridResult<List<AllowanceVerificationKendoVm>>()
            {
                Data = allowLst,
                Status = ResultStatus.Ok,
                TotalCount = totalRowNum
            };
        }

        [HttpGet]
        public ServiceResult<List<AllowanceVerificationVm>> GetVerificationAllowanceByEmpId(int empId)
        {

            var allowLst = (from c in _allowanceServices.ListEmpAllowance().Data.Where(x => x.EmployeeId == empId).ToList()
                            select new AllowanceVerificationVm()
                            {
                                Id = c.Id,
                                EmployeeName = !string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.Name + "  (" + c.Employee.NameNp + ")" : c.Employee.Name,
                                EmployeeId = c.EmployeeId,
                                Value = getValue(c.AllowanceCalculatedBy, c.Value),
                                AllowanceCalculatedBy = Enum.GetName(typeof(AllowanceCalculatedBy), c.AllowanceCalculatedBy),
                                AllowanceId = c.AllowanceId,
                                AllowanceName = c.Allowance.Name,
                                AllowancePaidPer = getAllowancePaidPer(c.AllowanceCalculatedBy,c.AllowancePaidPer),
                                ApprovedById = c.ApprovedById,
                            }).ToList();
            return new ServiceResult<List<AllowanceVerificationVm>>()
            {
                Data = allowLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EEmployeeAlowance> Approve(int id, int empId)
        {
            string msg = "";
            var allowance = _allowanceServices.ListEmpAllowance().Data.Where(x => x.Id == id).FirstOrDefault();
            if (allowance != null)
            {
                if (allowance.IsApproved == false)
                {
                    allowance.ApprovedById = RiddhaSession.CurrentUser.Id;
                    allowance.ApprovedOn = System.DateTime.Now;
                    allowance.IsApproved = true;
                    var result = _allowanceServices.UpdateEmpAllowance(allowance);
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EEmployeeAlowance>()
            {
                Data = allowance,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeAlowance> Revert(int id, int empId)
        {
            string msg = "";
            var allowance = _allowanceServices.ListEmpAllowance().Data.Where(x => x.Id == id).FirstOrDefault();
            if (allowance != null)
            {
                if (allowance.IsApproved)
                {
                    allowance.ApprovedById = null;
                    allowance.ApprovedOn = null;
                    allowance.IsApproved = false;
                    var result = _allowanceServices.UpdateEmpAllowance(allowance);
                    msg = "Reverted Successfully";
                }
                else
                {
                    msg = "Allowance has not been approved yet.";
                }
            }
            return new ServiceResult<EEmployeeAlowance>()
            {
                Data = allowance,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }
        #endregion

    }
    public class AllowanceVerificationKendoVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Allowance { get; set; }
        public string AllowancePeriod { get; set; }
        public string Value { get; set; }
        public string AllowancePaidPer { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public int? ApprovedById { get; set; }
        public bool IsApproved { get; set; }
    }

    public class AllowanceVerificationVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int AllowanceId { get; set; }
        public string AllowanceName { get; set; }
        public string Value { get; set; }
        public string AllowanceCalculatedBy { get; set; }
        public string AllowancePaidPer { get; set; }
        public int? ApprovedById { get; set; }
    }
    public class AllowanceGridVm
    {
        public int Id { get; set; }
        public string NameNp { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string ValueName { get; set; }
        public string MinimumWorkingHour { get; set; }
        public string AllowancePeriodName { get; set; }
        public AllowancePeriod AllowancePeriod { get; set; }
        public string AllowanceCalculatedByName { get; set; }
        public AllowanceCalculatedBy AllowanceCalculatedBy { get; set; }
        public string AllowancePaidPerName { get; set; }
        public AllowancePaidPer AllowancePaidPer { get; set; }
    }
}
