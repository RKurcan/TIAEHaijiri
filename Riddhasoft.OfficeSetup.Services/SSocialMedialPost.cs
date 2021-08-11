using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SSocialMedialPost : Riddhasoft.Services.Common.IBaseService<ESocialMedialPost>
    {
        RiddhaDBContext db = null;
        public SSocialMedialPost()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<ESocialMedialPost> Add(ESocialMedialPost model)
        {
            db.SocialMedialPost.Add(model);
            db.SaveChanges();
            return new ServiceResult<ESocialMedialPost>()
            {
                Data = model,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<ESocialMedialPost>> List()
        {
            return new ServiceResult<IQueryable<ESocialMedialPost>>()
            {
                Data = db.SocialMedialPost,
                Status = ResultStatus.Ok,
                Message = "",
            };
        }

        public ServiceResult<int> Remove(ESocialMedialPost model)
        {
            db.SocialMedialPost.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<ESocialMedialPost> Update(ESocialMedialPost model)
        {
            db.Entry<ESocialMedialPost>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ESocialMedialPost>()
            {
                Data = model,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }
}
