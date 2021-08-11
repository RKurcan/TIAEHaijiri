using Riddhasoft.DB;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SAllowanceHead : Riddhasoft.Services.Common.IBaseService<EAllowanceHead>
    {
        RiddhaDBContext db = null;
        public SAllowanceHead()
        {
            db = new RiddhaDBContext();
        }
        public ServiceResult<EAllowanceHead> Add(EAllowanceHead model)
        {
            db.AllowanceHead.Add(model);
            db.SaveChanges();

            return new ServiceResult<EAllowanceHead>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EAllowanceHead>> List()
        {
            return new ServiceResult<IQueryable<EAllowanceHead>>()
            {
                Data = db.AllowanceHead,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EAllowanceHead model)
        {
            db.AllowanceHead.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EAllowanceHead> Update(EAllowanceHead model)
        {
            db.Entry<EAllowanceHead>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EAllowanceHead>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
