using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class AllowanceHeadApiController : ApiController
    {
        SAllowanceHead _allowanceHeadServices = null;
        LocalizedString _loc = null;

        public AllowanceHeadApiController()
        {
            _allowanceHeadServices = new SAllowanceHead();
            _loc = new LocalizedString();
        }

        public ServiceResult<List<AllowanceHeadGridVm>> Get()
        {
            int branchId = (int)RiddhaSession.BranchId;

            var allowanceHeadList = (from c in _allowanceHeadServices.List().Data.Where(x => x.BranchId == branchId)
                                     select new AllowanceHeadGridVm()
                                     {
                                         BranchId = c.BranchId,
                                         Code = c.Code,
                                         Id = c.Id,
                                         Name = c.Name
                                     }).ToList();

            return new ServiceResult<List<AllowanceHeadGridVm>>()
            {
                Data = allowanceHeadList,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<AllowanceHeadGridVm>> GetAllowanceHeadKendoGrid(KendoPageListArguments vm)
        {
            int branchId = (int)RiddhaSession.BranchId;
            IQueryable<EAllowanceHead> allowanceHeadQuery = _allowanceHeadServices.List().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = allowanceHeadQuery.Count();

            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            IQueryable<EAllowanceHead> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = allowanceHeadQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = allowanceHeadQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = allowanceHeadQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = allowanceHeadQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;

                default:
                    paginatedQuery = allowanceHeadQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    break;
            }
            var allowanceHeadList = (from c in paginatedQuery.ToList()
                                     select new AllowanceHeadGridVm()
                                     {
                                         Id = c.Id,
                                         Code = c.Code,
                                         Name = c.Name,
                                         BranchId = c.BranchId,
                                     }).ToList();
            return new KendoGridResult<List<AllowanceHeadGridVm>>()
            {
                Data = allowanceHeadList.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = allowanceHeadList.Count()
            };
        }

        [HttpPost]
        public ServiceResult<EAllowanceHead> Post(EAllowanceHead model)
        {
            model.CreationDate = DateTime.Now;
            model.BranchId = (int)RiddhaSession.BranchId;
            var result = _allowanceHeadServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8024", "7243", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<EAllowanceHead>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPut]
        public ServiceResult<EAllowanceHead> Put(EAllowanceHead model)
        {
            var result = _allowanceHeadServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8024", "7244", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<EAllowanceHead>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var insurance = _allowanceHeadServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _allowanceHeadServices.Remove(insurance);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8024", "7245", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<AllowanceHeadGridVm> GetAllowanceHeadById(int id)
        {
            var allowanceHead = (from c in _allowanceHeadServices.List().Data.Where(x => x.Id == id).ToList()
                                 select new AllowanceHeadGridVm()
                                 {
                                     BranchId = c.BranchId,
                                     Code = c.Code,
                                     Id = c.Id,
                                     Name = c.Name,
                                     CreationDate = c.CreationDate.ToString("yyyy/MM/dd")
                                 }).FirstOrDefault();

            return new ServiceResult<AllowanceHeadGridVm>()
            {
                Data = allowanceHead,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

    public class AllowanceHeadGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public string CreationDate { get; set; }
    }
}
