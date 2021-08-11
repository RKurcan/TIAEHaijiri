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
    public class InsuranceCompanyApiController : ApiController
    {
        SInsuranceCompany _insuranceServices = null;
        LocalizedString _loc = null;
        public InsuranceCompanyApiController()
        {
            _insuranceServices = new SInsuranceCompany();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<InsuranceGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var insuranceLst = (from c in _insuranceServices.List().Data.Where(x => x.BranchId == branchId)
                                select new InsuranceGridVm()
                                {
                                    Id = c.Id,
                                    Code = c.Code,
                                    Name = c.Name,
                                    NameNp = c.NameNp,
                                    Address = c.Address,
                                    BranchId = c.BranchId,
                                    ContactNo = c.ContactNo
                                }).ToList();
            return new ServiceResult<List<InsuranceGridVm>>()
            {
                Data = insuranceLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public KendoGridResult<List<InsuranceGridVm>> GetInsuranceKendoGrid(KendoPageListArguments vm)
        {
            var branchId = RiddhaSession.BranchId;
            IQueryable<EInsuranceCompany> insuranceQuery = _insuranceServices.List().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = insuranceQuery.Count();

            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            IQueryable<EInsuranceCompany> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = insuranceQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = insuranceQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = insuranceQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = insuranceQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Address":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = insuranceQuery.Where(x => x.Address.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = insuranceQuery.Where(x => x.Address == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "ContactNo":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = insuranceQuery.Where(x => x.ContactNo.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = insuranceQuery.Where(x => x.ContactNo == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                default:
                    paginatedQuery = insuranceQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    break;
            }
            var insuranceLst = (from c in paginatedQuery
                                select new InsuranceGridVm()
                                {
                                    Id = c.Id,
                                    Code = c.Code,
                                    Name = c.Name,
                                    NameNp = c.NameNp,
                                    Address = c.Address,
                                    ContactNo = c.ContactNo,
                                    BranchId = c.BranchId
                                }).ToList();
            return new KendoGridResult<List<InsuranceGridVm>>()
            {
                Data = insuranceLst.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = insuranceLst.Count()
            };
        }
        public ServiceResult<EInsuranceCompany> Get(int id)
        {
            EInsuranceCompany insurance = _insuranceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EInsuranceCompany>()
            {
                Data = insurance,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EInsuranceCompany> Post(EInsuranceCompany model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            var result = _insuranceServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8025", "7239", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<EInsuranceCompany>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EInsuranceCompany> Put(EInsuranceCompany model)
        {
            var result = _insuranceServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8025", "7240", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<EInsuranceCompany>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var insurance = _insuranceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _insuranceServices.Remove(insurance);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8025", "7240", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
    public class InsuranceGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public int BranchId { get; set; }
    }
}
