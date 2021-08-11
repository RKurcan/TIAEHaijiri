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
    public class BankApiController : ApiController
    {
        SBank bankServices = null;
        LocalizedString loc = null;
        public BankApiController()
        {
            bankServices = new SBank();
            loc = new LocalizedString();
        }
        [ActionFilter("1047")]
        public ServiceResult<List<BankGridVm>> Get()
        {
            SBank service = new SBank();
            var bankLst = (from c in service.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId)
                           select new BankGridVm()
                           {
                               Id = c.Id,
                               BranchId = c.BranchId,
                               Code = c.Code,
                               Name = c.Name,
                               NameNp = c.NameNp,
                               Address = c.Address,
                               AddressNp = c.AddressNp
                           }).ToList();
            return new ServiceResult<List<BankGridVm>>()
            {
                Data = bankLst,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EBank> Get(int id)
        {
            EBank bank = bankServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EBank>()
            {
                Data = bank,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("1035")]
        public ServiceResult<EBank> Post(EBank model)
        {
            model.CompanyId = RiddhaSession.CompanyId;
            var result = bankServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1010", "1035", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EBank>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [ActionFilter("1036")]
        public ServiceResult<EBank> Put(EBank model)
        {
            model.CompanyId = RiddhaSession.CompanyId;
            var result = bankServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1010", "1036", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EBank>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete, ActionFilter("1037")]
        public ServiceResult<int> Delete(int id)
        {
            var bank = bankServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = bankServices.Remove(bank);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1010", "1037", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
    public class BankGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string Address { get; set; }
        public string AddressNp { get; set; }
        public int? BranchId { get; set; }
    }
}
