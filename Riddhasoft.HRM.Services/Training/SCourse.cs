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
    public class SCourse : Riddhasoft.Services.Common.IBaseService<ECourse>
    {
        RiddhaDBContext db = null;
        public SCourse()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ECourse>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ECourse>>()
            {
                Data = db.Course,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ECourse> Add(ECourse model)
        {
            db.Course.Add(model);
            db.SaveChanges();
            return new ServiceResult<ECourse>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ECourse> Update(ECourse model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ECourse>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ECourse model)
        {
            db.Course.Remove(model);
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
