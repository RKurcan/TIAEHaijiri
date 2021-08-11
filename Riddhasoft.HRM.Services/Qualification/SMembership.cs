using Riddhasoft.DB;
using Riddhasoft.HRM.Entities.Qualification;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services.Qualification
{
    public class SMembership : Riddhasoft.Services.Common.IBaseService<EMembership>
    {
        RiddhaDBContext db = null;
        public SMembership()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EMembership>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EMembership>>()
            {
                Data = db.Membership,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EMembership> Add(EMembership model)
        {
            db.Membership.Add(model);
            db.SaveChanges();
            return new ServiceResult<EMembership>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EMembership> Update(EMembership model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EMembership>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EMembership model)
        {
            db.Membership.Remove(model);
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
