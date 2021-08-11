using Riddhasoft.Employee.Entities;
using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class EmploymentStatusApiController : ApiController
    {
        int? branchId = RiddhaSession.BranchId;
        SEmploymentStatus employmentStatusServices = null;
        LocalizedString loc = null;
        public EmploymentStatusApiController()
        {
            employmentStatusServices = new SEmploymentStatus();
            loc = new LocalizedString();
        }

        public ServiceResult<List<EmploymentStatusGridVm>> Get()
        {
            var EmploymentStatusLst = (from c in employmentStatusServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                       select new EmploymentStatusGridVm()
                                       {
                                           Id = c.Id,
                                           Code = c.Code,
                                           Name = c.Name,
                                           Description = c.Description,
                                           IsContract = c.IsContract,
                                           EmploymentStatusName = Enum.GetName(typeof(EmploymentStatus), c.EmploymentStatus),
                                           EmploymentStatus = c.EmploymentStatus
                                       }).ToList();
            return new ServiceResult<List<EmploymentStatusGridVm>>()
            {
                Data = EmploymentStatusLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EEmploymentStatus> Get(int id)
        {
            EEmploymentStatus employmentStatus = employmentStatusServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EEmploymentStatus>()
            {
                Data = employmentStatus,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EEmploymentStatus> Post(EEmploymentStatus model)
        {
            model.BranchId = branchId;
            var result = employmentStatusServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7004", "7118", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EEmploymentStatus>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EEmploymentStatus> Put(EEmploymentStatus model)
        {
            model.BranchId = branchId;
            var result = employmentStatusServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7004", "7119", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EEmploymentStatus>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var employmentStatus = employmentStatusServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = employmentStatusServices.Remove(employmentStatus);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7004", "7120", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<List<EmploymentStatusWiseLeaveQuotaViewModel>> GetEmploymentStatusWiseLeaveQuota(int employmentStatusId)
        {
            int? branchId = RiddhaSession.BranchId;
            string language = RiddhaSession.Language.ToString();
            SEmploymentStatusWiseLeavedBalance employmentStatusWiseLeavedBalanceServices = new SEmploymentStatusWiseLeavedBalance();
            List<ELeaveMaster> leaveMastLst = new SLeaveMaster().List().Data.Where(x => x.BranchId == branchId).ToList();
            List<EEmploymentStatusWiseLeavedBalance> leaveQuotaLst = employmentStatusWiseLeavedBalanceServices.List().Data.Where(c => c.EmploymentStatusId == employmentStatusId).ToList();
            List<EmploymentStatusWiseLeaveQuotaViewModel> resultLst =
                                                        (from c in leaveMastLst
                                                         join p in leaveQuotaLst on c.Id equals p.LeaveId into ps
                                                         from j in ps.DefaultIfEmpty((new EEmploymentStatusWiseLeavedBalance()))
                                                         select new EmploymentStatusWiseLeaveQuotaViewModel()
                                                         {
                                                             Id = j.Id,
                                                             EmploymentStatusId = employmentStatusId,
                                                             Name = language == "ne" && c.Name != null ? c.NameNp : c.Name,
                                                             Balance = j.Id == 0 ? c.Balance : j.Balance,
                                                             LeaveId = c.Id,
                                                             MaxLimit = j.MaxLimit,
                                                             IsPaidLeave = j.Id == 0 ? c.IsPaidLeave : j.IsPaidLeave,
                                                             IsLeaveCarryable = j.Id == 0 ? c.IsLeaveCarryable : j.IsLeaveCarryable,
                                                             ApplicableGender = j.Id == 0 ? c.ApplicableGender : j.ApplicableGender,
                                                             IsMapped = j.IsMapped,
                                                             IsReplacementLeave = c.IsReplacementLeave
                                                         }).ToList();
            return new ServiceResult<List<EmploymentStatusWiseLeaveQuotaViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public ServiceResult<bool> ApplyLeaveQuota(EmploymentStatusWiseLeaveQuotaApplyVm lst)
        {
            LocalizedString loc = new LocalizedString();
            SEmploymentStatusWiseLeavedBalance employmentStatusWiseLeavedBalanceServices = new SEmploymentStatusWiseLeavedBalance();
            var result = employmentStatusWiseLeavedBalanceServices.ApplyLeaveQuota(lst.LeaveQuota);
            return new ServiceResult<bool>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
    public class EmploymentStatusGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsContract { get; set; }
        public string EmploymentStatusName { get; set; }
        public string Description { get; set; }
        public int? BranchId { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
    }
}
