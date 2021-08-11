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
    public class SContext : Riddhasoft.Services.Common.IBaseService<EContext>
    {
        RiddhaDBContext db = null;
        public SContext()
        {
            db = new RiddhaDBContext();
        }

        public Common.ServiceResult<IQueryable<EContext>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EContext>>()
            {
                Data = db.Context,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<EContext> Add(EContext model)
        {
            db.Context.Add(model);
            db.SaveChanges();
            return new ServiceResult<EContext>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<EContext> Update(EContext model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EContext>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<int> Remove(EContext model)
        {
            db.Context.Remove(model);
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
