using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Services;
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
    public class CustomFieldApiController : ApiController
    {
        SCustomField _customFieldServices = null;
        LocalizedString _loc = null;
        public CustomFieldApiController()
        {
            _customFieldServices = new SCustomField();
            _loc = new LocalizedString();

        }
        public ServiceResult<List<CostomFieldGridVm>> Get()
        {
            int companyId = RiddhaSession.CompanyId;
            var customFieldLst = (from c in _customFieldServices.List().Data.Where(x => x.CompanyId == companyId).ToList()
                                  select new CostomFieldGridVm()
                           {
                               Id = c.Id,
                               CompanyId = c.CompanyId,
                               FieldName = c.FieldName,
                               Length = c.Length,
                               FieldType = Enum.GetName(typeof(FieldType), c.FieldType)

                           }).ToList();
            return new ServiceResult<List<CostomFieldGridVm>>()
            {
                Data = customFieldLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ECustomField> Get(int id)
        {
            ECustomField customField = _customFieldServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ECustomField>()
            {
                Data = customField,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<ECustomField> Post(ECustomField model)
        {
            model.CompanyId = RiddhaSession.CompanyId;
            var result = _customFieldServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8010", "7156", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ECustomField>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ECustomField> Put(ECustomField model)
        {
            var result = _customFieldServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8010", "7157", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ECustomField>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var customField = _customFieldServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _customFieldServices.Remove(customField);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8010", "7158", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }

    public class CostomFieldGridVm
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public int Length { get; set; }
        public int CompanyId { get; set; }
    }
}
