using Riddhasoft.DB;
using Riddhasoft.Entity.User;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.User
{
    public class SSessionDetail : Riddhasoft.Services.Common.IBaseService<ESessionDetail>
    {
        RiddhaDBContext db = null;
        public SSessionDetail()
        {
            db = new RiddhaDBContext();
        }

        public Common.ServiceResult<IQueryable<ESessionDetail>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ESessionDetail>>()
            {
                Data = db.SessionDetail,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<ESessionDetail> Add(ESessionDetail model)
        {
            db.SessionDetail.Add(model);
            db.SaveChanges();
            return new ServiceResult<ESessionDetail>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<ESessionDetail> Update(ESessionDetail model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ESessionDetail>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<int> Remove(ESessionDetail model)
        {
            db.SessionDetail.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Remove Successfully",
                Status = ResultStatus.Ok
            };
        }
    }
}
