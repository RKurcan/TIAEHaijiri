using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Entities.Qualification;
using Riddhasoft.HRM.Services.Qualification;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class MembershipApiController : ApiController
    {
        SMembership membershipServices = null;
        LocalizedString loc = null;
        SUser userServices = null;
        SEmployee employeeServices = null;
        public MembershipApiController()
        {
            membershipServices = new SMembership();
            loc = new LocalizedString();
            userServices = new SUser();
            employeeServices = new SEmployee();
        }
        public ServiceResult<List<MembershipGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var membershipLst = (from c in membershipServices.List().Data.Where(x => x.BranchId == branchId)
                                 select new MembershipGridVm()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     Name = c.Name,
                                     Description = c.Description,
                                     BranchId = c.BranchId,
                                 }).ToList();
            return new ServiceResult<List<MembershipGridVm>>()
            {
                Data = membershipLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<MembershipGridVm>> GetMembershipByEmpId(int empId)
        {
            var employeeMembershipLst = (from c in membershipServices.List().Data.Where(x => x.EmployeeId == empId)
                                         select new MembershipGridVm()
                                         {
                                             Id = c.Id,
                                             Code = c.Code,
                                             Name = c.Name,
                                             Description = c.Description,
                                             BranchId = c.BranchId,
                                             EmployeeId = c.EmployeeId
                                         }).ToList();
            return new ServiceResult<List<MembershipGridVm>>()
            {
                Data = employeeMembershipLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EMembership> Get(int id)
        {
            EMembership membership = membershipServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EMembership>()
            {
                Data = membership,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EMembership> Post(EMembership model)
        {
            model.BranchId = RiddhaSession.BranchId;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = System.DateTime.Now;
            var result = membershipServices.Add(model);
            return new ServiceResult<EMembership>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EMembership> Put(EMembership model)
        {
            var result = membershipServices.Update(model);
            return new ServiceResult<EMembership>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var membership = membershipServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = membershipServices.Remove(membership);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #region  Employee Membership Verification
        [HttpPost]
        public KendoGridResult<List<MembershipVerificationGridVm>> GetEmpMembershipKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;

            IQueryable<EMembership> employeeMembershipQuery = membershipServices.List().Data.Where(x => x.BranchId == branchId && x.IsApproved ==false);
            var userQuery = userServices.List().Data;
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var membershipist = (from c in employeeMembershipQuery.ToList()
                                 join d in Common.GetEmployees().Data.ToList() on c.Id equals d.Id
                                 select new MembershipVerificationGridVm()
                                 {
                                     Id = c.Id,
                                     EmployeeName = d.Name,
                                     EmployeeCode = d.Code,
                                     EmployeeId = c.EmployeeId,
                                     Code = c.Code,
                                     Name = c.Name,
                                     Description = c.Description,
                                     ApprovedById = c.ApprovedById,
                                     ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                     ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById),
                                 }).OrderByDescending(x => x.Id).ToList();
            return new KendoGridResult<List<MembershipVerificationGridVm>>()
            {
                Data = membershipist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = membershipist.Count(),
            };
        }
        private string getapprovedByUser(IQueryable<Riddhasoft.User.Entity.EUser> userQuery, int? approvedById)
        {
            if (approvedById == null)
            {
                return "";
            }
            else
            {
                return userQuery.Where(x => x.Id == (int)approvedById).FirstOrDefault().Name;
            }
        }
        [HttpGet]
        public ServiceResult<List<MembershipVerificationGridVm>> GetVerificationMembershipByEmpId(int empId)
        {
            var membership = (from c in membershipServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                              join d in employeeServices.List().Data
                              on c.EmployeeId equals d.Id
                              select new MembershipVerificationGridVm()
                              {
                                  Id = c.Id,
                                  EmployeeName = d.Name,
                                  EmployeeCode = d.Code,
                                  EmployeeId = c.EmployeeId,
                                  Code = c.Code,
                                  Name = c.Name,
                                  Description = c.Description,
                                  ApprovedById = c.ApprovedById,
                              }).ToList();
            return new ServiceResult<List<MembershipVerificationGridVm>>()
            {
                Data = membership,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EMembership> Approve(int id, int empId)
        {
            string msg = "";
            var membership = membershipServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (membership != null)
            {
                if (membership.IsApproved == false)
                {
                    membership.ApprovedById = RiddhaSession.CurrentUser.Id;
                    membership.ApprovedOn = System.DateTime.Now;
                    membership.IsApproved = true;
                    var result = membershipServices.Update(membership);
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EMembership>()
            {
                Data = membership,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }


        [HttpGet]
        public ServiceResult<EMembership> Revert(int id, int empId)
        {
            string msg = "";
            ResultStatus status = new ResultStatus();
            var membership = membershipServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (membership != null)
            {
                if (membership.IsApproved)
                {
                    membership.ApprovedById = null;
                    membership.ApprovedOn = null;
                    membership.IsApproved = false;
                    var result = membershipServices.Update(membership);
                    msg = "Reverted Successfully";
                    status = ResultStatus.Ok;
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<EMembership>()
            {
                Data = membership,
                Message = msg,
                Status = status
            };
        }
        #endregion

    }
    public class MembershipGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EmployeeId { get; set; }
        public int? BranchId { get; set; }
    }
    public class MembershipVerificationGridVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ApprovedById { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public int BranchId { get; set; }
    }
}
