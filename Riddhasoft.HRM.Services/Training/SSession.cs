using Riddhasoft.DB;
using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services.Training
{
    public class SSession : Riddhasoft.Services.Common.IBaseService<ESession>
    {
        RiddhaDBContext db = null;
        public SSession()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ESession>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ESession>>()
            {
                Data = db.Session,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ESession> Add(ESession model)
        {
            db.Session.Add(model);
            db.SaveChanges();
            return new ServiceResult<ESession>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ESession> Update(ESession model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ESession>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ESession model)
        {
            db.Session.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
