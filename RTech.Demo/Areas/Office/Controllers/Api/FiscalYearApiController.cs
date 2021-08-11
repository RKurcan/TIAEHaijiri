using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{

    public class FiscalYearApiController : ApiController
    {
        SFiscalYear fiscalYearServices = null;
        LocalizedString loc = null;
        public FiscalYearApiController()
        {
            fiscalYearServices = new SFiscalYear();
            loc = new LocalizedString();
        }
        [ActionFilter("1038")]
        public ServiceResult<List<FiscalYearGridVm>> Get()
        {
            int companyId = RiddhaSession.CompanyId;
            SFiscalYear service = new SFiscalYear();
            var fiscalYeartLst = (from c in service.List().Data.Where(x => x.Branch.CompanyId == companyId)
                                  select new FiscalYearGridVm()
                                  {
                                      Id = c.Id,
                                      BranchId = c.BranchId,
                                      CurrentFiscalYear = c.CurrentFiscalYear,
                                      StartDate = c.StartDate,
                                      EndDate = c.EndDate,
                                      FiscalYear = c.FiscalYear
                                  }).ToList();

            return new ServiceResult<List<FiscalYearGridVm>>()
            {
                Data = fiscalYeartLst,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EFiscalYear> Get(int id)
        {
            EFiscalYear fiscalYear = fiscalYearServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EFiscalYear>()
            {
                Data = fiscalYear,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<FiscalYearDropdownModel>> GetFiscalYearsForDropdown()
        {
            List<FiscalYearDropdownModel> lst = (from c in fiscalYearServices.List().Data
                                                 select new FiscalYearDropdownModel()
                                                 {
                                                     Id = c.Id,
                                                     FiscalYear = c.FiscalYear
                                                 }).ToList();
            return new ServiceResult<List<FiscalYearDropdownModel>>()
            {
                Data = lst,
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("1011")]
        public ServiceResult<EFiscalYear> Post(EFiscalYear model)
        {
            model.BranchId = RiddhaSession.BranchId;
            model.CompanyId = RiddhaSession.CompanyId;
            if (model.CurrentFiscalYear == true)
            {
                EFiscalYear checkedFiscalYear = fiscalYearServices.List().Data.Where(x => x.CurrentFiscalYear == true && x.Branch.CompanyId == model.CompanyId).FirstOrDefault();
                if (checkedFiscalYear != null)
                {
                    checkedFiscalYear.CurrentFiscalYear = false;
                    var rs = fiscalYearServices.Update(checkedFiscalYear);
                }
            }
            var result = fiscalYearServices.Add(model);
            if (result.Status == ResultStatus.Ok && model.CurrentFiscalYear == true)
            {
                RiddhaSession.FYId = result.Data.Id;
                Common.AddAuditTrail("1001", "1011", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }

            return new ServiceResult<EFiscalYear>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }
        [ActionFilter("1012")]
        public ServiceResult<EFiscalYear> Put(EFiscalYear model)
        {
            model.CompanyId = RiddhaSession.CompanyId;
            var result = fiscalYearServices.Update(model);
            if (result.Status == ResultStatus.Ok && model.CurrentFiscalYear == true)
            {
                RiddhaSession.FYId = result.Data.Id;
                Common.AddAuditTrail("1001", "1012", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EFiscalYear>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }
        [HttpDelete, ActionFilter("1013")]
        public ServiceResult<int> Delete(int id)
        {
            var fiscalYear = fiscalYearServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = fiscalYearServices.Remove(fiscalYear);
            if (result.Status ==ResultStatus.Ok)
            {
                Common.AddAuditTrail("1001", "1013", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
    public class FiscalYearDropdownModel
    {
        public int Id { get; set; }
        public string FiscalYear { get; set; }
    }

    public class FiscalYearGridVm
    {
        public int Id { get; set; }
        public string FiscalYear { get; set; }
        public bool CurrentFiscalYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? BranchId { get; set; }
    }
}
