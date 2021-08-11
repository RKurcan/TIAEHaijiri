using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.HRM.Services.Training;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class SessionApiController : ApiController
    {
        SSession _sessionServices = null;
        SCourse _courseServices = null;
        LocalizedString _loc = null;
        public SessionApiController()
        {
            _sessionServices = new SSession();
            _courseServices = new SCourse();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<SessionGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var sessionLst = (from c in _sessionServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                              select new SessionGridVm()
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 BranchId = c.BranchId,
                                 CourseId = c.CourseId,
                                 CourseName = c.Course.Title,
                                 Description = c.Description,
                                 Duration = c.Duration,
                                 Location = c.Location,
                                 Method = Enum.GetName(typeof(Method), c.Method)
                             }).ToList();
            return new ServiceResult<List<SessionGridVm>>()
            {
                Data = sessionLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ESession> Get(int id)
        {
            ESession session = _sessionServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ESession>()
            {
                Data = session,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ESession> Post(ESession model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            var result = _sessionServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8007", "7130", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ESession>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ESession> Put(ESession model)
        {
            var result = _sessionServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8007", "7131", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ESession>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var session = _sessionServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _sessionServices.Remove(session);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8007", "7132", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPost]
        public ServiceResult<List<CourseAutoCompleteVm>> GetCourseLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<CourseAutoCompleteVm> resultLst = new List<CourseAutoCompleteVm>();
            if (model != null)
            {
                var courseLst = _courseServices.List().Data.Where(x => x.BranchId == branchId);
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                if (searchText == "___")
                {
                    courseLst = courseLst.OrderBy(x => x.Title).Take(20);
                }
                else
                {
                    courseLst = courseLst.Where(x => x.Title.ToLower().Contains(searchText.ToLower()));
                }
                if (courseLst != null)
                {
                    resultLst = (from c in courseLst
                                 select new CourseAutoCompleteVm()
                                 {
                                     Id = c.Id,
                                     Name = c.Title
                                 }).OrderBy(x => x.Name).ToList();
                }
            }
            return new ServiceResult<List<CourseAutoCompleteVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
    }
    public class SessionGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public string Location { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public int BranchId { get; set; }
    }
    public class CourseAutoCompleteVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
