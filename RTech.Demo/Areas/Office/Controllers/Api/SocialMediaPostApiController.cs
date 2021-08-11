using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class SocialMediaPostApiController : ApiController
    {
        SSocialMedialPost _socialMedialPost = null;
        LocalizedString loc = null;
        public SocialMediaPostApiController()
        {
            _socialMedialPost = new SSocialMedialPost();
            loc = new LocalizedString();
        }

        public ServiceResult<List<ESocialMedialPost>> Get()
        {
            var result = _socialMedialPost.List().Data.ToList();
            return new ServiceResult<List<ESocialMedialPost>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ESocialMedialPost> Post(ESocialMedialPost model)
        {
            var result = _socialMedialPost.Add(model);
            return new ServiceResult<ESocialMedialPost>()
            {
                Data = result.Data,
                Message = result.Message,
                Status = result.Status,
            };
        }

        public ServiceResult<ESocialMedialPost> Put(ESocialMedialPost model)
        {
            var result = _socialMedialPost.Update(model);
            return new ServiceResult<ESocialMedialPost>()
            {
                Data = result.Data,
                Message = result.Message,
                Status = result.Status,
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var data = _socialMedialPost.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _socialMedialPost.Remove(data);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<ESocialMedialPost> GetPublisPost()
        {
            var data = _socialMedialPost.List().Data.Where(x => x.Publish).FirstOrDefault();
            return new ServiceResult<ESocialMedialPost>()
            {
                Data = data,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }
}
