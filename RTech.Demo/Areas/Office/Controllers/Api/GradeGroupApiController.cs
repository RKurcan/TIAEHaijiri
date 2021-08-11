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
    public class GradeGroupApiController : ApiController
    {
        SGradeGroup gradeGroupServices = null;
        LocalizedString loc = null;
        public GradeGroupApiController()
        {
            gradeGroupServices = new SGradeGroup();
            loc = new LocalizedString();
        }
        [ActionFilter("1045")]
        public ServiceResult<List<GradeGroupGridVm>> Get()
        {
            SGradeGroup service = new SGradeGroup();
            var gradeGrouptLst = (from c in service.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId)
                                  select new GradeGroupGridVm()
                                 {
                                     Id = c.Id,
                                     BranchId = c.BranchId,
                                     Code = c.Code,
                                     Name = c.Name,
                                     NameNp = c.NameNp,
                                     Value = c.Value
                                 }).ToList();

            return new ServiceResult<List<GradeGroupGridVm>>()
            {
                Data = gradeGrouptLst,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EGradeGroup> Get(int id)
        {
            EGradeGroup gradeGroup = gradeGroupServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EGradeGroup>()
            {
                Data = gradeGroup,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("1029")]
        public ServiceResult<EGradeGroup> Post(EGradeGroup model)
        {
            model.CompanyId = RiddhaSession.CompanyId;
            var result = gradeGroupServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1008", "1029", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EGradeGroup>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [ActionFilter("1030")]
        public ServiceResult<EGradeGroup> Put(EGradeGroup model)
        {
            model.CompanyId = RiddhaSession.CompanyId;
            var result = gradeGroupServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1008", "1030", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EGradeGroup>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete, ActionFilter("1031")]
        public ServiceResult<int> Delete(int id)
        {
            var bank = gradeGroupServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = gradeGroupServices.Remove(bank);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1008", "1031", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
    public class GradeGroupGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int? BranchId { get; set; }
        public decimal Value { get; set; }
    }

}
