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
    public class BranchApiController : ApiController
    {
        SBranch branchServices = null;
        SCompany _companyService = null;
        LocalizedString loc = null;
        string lang = RiddhaSession.Language;
        // SUser userServices = null;
        public BranchApiController()
        {
            branchServices = new SBranch();
            loc = new LocalizedString();
            _companyService = new SCompany();
        }

        [ActionFilter("1041")]
        public ServiceResult<List<BranchGridVm>> Get()
        {
            string lang = RiddhaSession.Language;
            int companyId = RiddhaSession.CompanyId;
            var branchLst = (from c in branchServices.List().Data.Where(x => x.CompanyId == companyId)
                             select new BranchGridVm()
                             {
                                 Id = c.Id,
                                 CompanyId = c.CompanyId,
                                 //CompanyName = c.Company.Name,
                                 CompanyName = c.Company == null ? "" : ((!string.IsNullOrEmpty(c.Company.NameNp) && lang == "ne") ? c.Company.NameNp : c.Company.Name),
                                 Code = c.Code,
                                 Name = c.Name,
                                 NameNp = c.NameNp,
                                 Address = c.Address,
                                 AddressNp = c.AddressNp,
                                 ContactNo = c.ContactNo,
                                 Email = c.Email,
                             }).ToList();
            return new ServiceResult<List<BranchGridVm>>()
            {
                Data = branchLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost, ActionFilter("1041")]
        public KendoGridResult<List<BranchGridVm>> GetKendoGrid(KendoPageListArguments arg)
        {

            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            SBranch branchServices = new SBranch();
            IQueryable<EBranch> branchQuery;
            var headOfficeBranch = branchServices.List().Data.Where(x => x.Id == RiddhaSession.BranchId && x.IsHeadOffice).FirstOrDefault();
            if (headOfficeBranch != null)
            {
                branchQuery = branchServices.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId && x.IsHeadOffice != true);
            }
            else
            {
                branchQuery = branchServices.List().Data.Where(x => x.Id == RiddhaSession.BranchId);
            }

            int totalRowNum = branchQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EBranch> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = branchQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = branchQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = branchQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = branchQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                default:
                    paginatedQuery = branchQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    break;
            }
            var branchlist = (from c in paginatedQuery
                              select new BranchGridVm()
                              {
                                  Id = c.Id,
                                  CompanyId = c.CompanyId,
                                  CompanyName = c.Company == null ? "" : ((!string.IsNullOrEmpty(c.Company.NameNp) && lang == "ne") ? c.Company.NameNp : c.Company.Name),
                                  Code = c.Code,
                                  Name = c.Name,
                                  NameNp = c.NameNp,
                                  Address = c.Address,
                                  AddressNp = c.AddressNp,
                                  ContactNo = c.ContactNo,
                                  Email = c.Email,
                              }).ToList();
            return new KendoGridResult<List<BranchGridVm>>()
            {
                Data = branchlist.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = branchlist.Count()
            };
        }
        // GET api/countryapi/5
        public ServiceResult<EBranch> Get(int id)
        {
            var branch = branchServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EBranch>()
            {
                Data = branch,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        // POST api/countryapi
        [ActionFilter("1016")]
        public ServiceResult<EBranch> Post(EBranch model)
        {
            var branches = branchServices.List().Data.Where(x => x.Code.ToLower() == model.Code).FirstOrDefault();
            if (branches != null)
            {
                return new ServiceResult<EBranch>()
                {
                    Data = null,
                    Message = "Code already exist. Please select new code.",
                    Status = ResultStatus.processError
                };
            }
            model.CompanyId = RiddhaSession.CompanyId;
            var result = branchServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1004", "1016", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EBranch>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }

        // PUT api/countryapi/5
        [ActionFilter("1017")]
        public ServiceResult<EBranch> Put(EBranch model)
        {
            var result = branchServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1004", "1017", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EBranch>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }
        // DELETE api/countryapi/5
        [ActionFilter("1018")]
        public ServiceResult<int> Delete(int id)
        {
            var branch = branchServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = branchServices.Remove(branch);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1004", "1018", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<List<CompanyDropDowmVm>> GetCompany()
        {
            string language = RiddhaSession.Language;
            int companyId = RiddhaSession.CompanyId;
            var companyLst = (from c in _companyService.List().Data.Where(x => x.Id == companyId)
                              select new CompanyDropDowmVm()
                              {
                                  Id = c.Id,
                                  //Name = c.Name,
                                  Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                              }).ToList();
            return new ServiceResult<List<CompanyDropDowmVm>>()
            {
                Data = companyLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<CompanyDropDowmVm>> GetBranchForDropdown()
        {
            List<EBranch> list = new List<EBranch>();
            if (RiddhaSession.DataVisibilityLevel == 4)
            {
                list = branchServices.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId).ToList();
            }
            else
            {
                list = branchServices.List().Data.Where(x => x.Id == RiddhaSession.BranchId).ToList();
            }
            var result = (from c in list
                          select new CompanyDropDowmVm()
                          {
                              Id = c.Id,
                              Name = c.Name
                          }).OrderBy(x => x.Id).ToList();
            return new ServiceResult<List<CompanyDropDowmVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }
    public class BranchGridVm
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string Address { get; set; }
        public string AddressNp { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
    }
    public class CompanyDropDowmVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
