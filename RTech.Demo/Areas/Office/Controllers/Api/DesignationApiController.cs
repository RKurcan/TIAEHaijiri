using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class DesignationApiController : ApiController
    {
        SDesignation designationServices = null;
        LocalizedString loc = null;
        public DesignationApiController()
        {
            designationServices = new SDesignation();
            loc = new LocalizedString();
        }
        [ActionFilter("1044")]
        public ServiceResult<List<DesignationGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SDesignation service = new SDesignation();
            List<EDesignation> list = new List<EDesignation>();
            var designationtLst = (from c in service.List().Data.Where(x => x.BranchId == branchId).OrderByDescending(x => x.DesignationLevel)
                                   select new DesignationGridVm()
                                   {
                                       Id = c.Id,
                                       BranchId = c.BranchId,
                                       Code = c.Code,
                                       Name = c.Name,
                                       NameNp = c.NameNp,
                                       DesignationLevel = c.DesignationLevel,
                                       MaxSalary = c.MaxSalary,
                                       MinSalary = c.MinSalary,
                                       CompanyId = c.CompanyId
                                   }).ToList();

            return new ServiceResult<List<DesignationGridVm>>()
            {
                Data = designationtLst,
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("1044"), HttpPost]
        public KendoGridResult<List<DesignationGridVm>> GetKendoGrid(KendoPageListArguments arg)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            SDesignation designationServices = new SDesignation();
            IQueryable<EDesignation> designationQuery;

            designationQuery = designationServices.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId);
            int totalRowNum = designationQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EDesignation> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = designationQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                        totalRowNum = paginatedQuery.Count();
                    }
                    else
                    {
                        paginatedQuery = designationQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                        totalRowNum = paginatedQuery.Count();
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = designationQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                        totalRowNum = paginatedQuery.Count();
                    }
                    else
                    {
                        paginatedQuery = designationQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                        totalRowNum = paginatedQuery.Count();
                    }
                    break;
                default:
                    paginatedQuery = designationQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    totalRowNum = designationQuery.Count();
                    break;
            }
            var designationlist = (from c in paginatedQuery
                                   select new DesignationGridVm()
                                   {
                                       Id = c.Id,
                                       BranchId = c.BranchId,
                                       Code = c.Code,
                                       Name = c.Name,
                                       NameNp = c.NameNp,
                                       DesignationLevel = c.DesignationLevel,
                                       MaxSalary = c.MaxSalary,
                                       MinSalary = c.MinSalary,
                                       CompanyId = c.CompanyId
                                   }).ToList();
            return new KendoGridResult<List<DesignationGridVm>>()
            {
                Data = designationlist.OrderBy(x=>x.DesignationLevel).Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = designationlist.Count()
            };
        }
        public ServiceResult<EDesignation> Get(int id)
        {
            EDesignation designation = designationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EDesignation>()
            {
                Data = designation,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("1025")]
        public ServiceResult<EDesignation> Post(EDesignation model)
        {
            //model.BranchId = RiddhaSession.BranchId;
            model.CompanyId = RiddhaSession.CompanyId;
            var result = designationServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1007", "1025", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EDesignation>()
            {
                Status = result.Status,
                Data = result.Data,
                Message = loc.Localize(result.Message)
            };
        }
        [ActionFilter("1026")]
        public ServiceResult<EDesignation> Put(EDesignation model)
        {
            //model.BranchId = RiddhaSession.BranchId;
            model.CompanyId = RiddhaSession.CompanyId;
            var result = designationServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1007", "1026", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EDesignation>()
            {
                Status = result.Status,
                Data = result.Data,
                Message = loc.Localize(result.Message)
            };
        }
        [HttpDelete, ActionFilter("1027")]
        public ServiceResult<int> Delete(int id)
        {
            var designation = designationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = designationServices.Remove(designation);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1007", "1027", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Status = result.Status,
                Data = result.Data,
                Message = loc.Localize(result.Message)
            };
        }
        [HttpPost]
        public ServiceResult<bool> ApplyLeaveQuota(DesigWiseApplyVm lst)
        {
            LocalizedString loc = new LocalizedString();
            var result = designationServices.ApplyLeaveQuota(lst.LeaveQuota);
            return new ServiceResult<bool>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet, ActionFilter("1028")]
        public ServiceResult<List<DesigWiseLeaveQuotaViewModel>> GetDesigWiseLeaveQuota(int desigId)
        {
            string language = RiddhaSession.Language.ToString();
            //List<ELeaveMaster> leaveMastLst = new SLeaveMaster().List().Data.Where(x => x.BranchId == branchId).ToList();
            List<ELeaveMaster> leaveMastLst = new SLeaveMaster().List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId).ToList();
            List<EDesignationWiseLeavedBalance> leaveQuotaLst = designationServices.ListLeaveQouta().Data.Where(c => c.DesignationId == desigId).ToList();
            List<DesigWiseLeaveQuotaViewModel> resultLst =
                                                        (from c in leaveMastLst
                                                         join p in leaveQuotaLst on c.Id equals p.LeaveId into ps
                                                         from j in ps.DefaultIfEmpty((new EDesignationWiseLeavedBalance()))
                                                         select new DesigWiseLeaveQuotaViewModel()
                                                         {
                                                             DesignationId = desigId,
                                                             Name = language == "ne" && c.Name != null ? c.NameNp : c.Name,
                                                             Balance = j.Id == 0 ? c.Balance : j.Balance,
                                                             Id = j.Id,
                                                             LeaveId = c.Id,
                                                             MaxLimit = j.MaxLimit,
                                                             IsPaidLeave = j.Id == 0 ? c.IsPaidLeave : j.IsPaidLeave,
                                                             IsLeaveCarryable = j.Id == 0 ? c.IsLeaveCarryable : j.IsLeaveCarryable,
                                                             ApplicableGender = j.Id == 0 ? c.ApplicableGender : j.ApplicableGender,
                                                             IsMapped = j.IsMapped,
                                                             IsReplacementLeave = c.IsReplacementLeave
                                                         }).ToList();
            return new ServiceResult<List<DesigWiseLeaveQuotaViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
    }

    public class DesigWiseLeaveQuotaViewModel
    {
        public int Id { get; set; }
        public int LeaveId { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int DesignationId { get; set; }
        public decimal Balance { get; set; }
        public decimal MaxLimit { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public bool IsMapped { get; set; }
        public bool IsReplacementLeave { get; set; }
    }

    public class DesigWiseApplyVm
    {
        public List<EDesignationWiseLeavedBalance> LeaveQuota { get; set; }
    }

    public class DesignationGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int DesignationLevel { get; set; }
        public decimal? MaxSalary { get; set; }
        public decimal? MinSalary { get; set; }
        public int? BranchId { get; set; }
        public int CompanyId { get; set; }
    }
}
