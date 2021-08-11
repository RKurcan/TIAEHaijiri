using Riddhasoft.DB;
using Riddhasoft.HRM.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services
{
    public class STermination : Riddhasoft.Services.Common.IBaseService<ETermination>
    {
        RiddhaDBContext db = null;
        public STermination()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ETermination>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ETermination>>()
            {
                Data = db.Termination,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ETermination> Add(ETermination model)
        {
            db.Termination.Add(model);
            db.SaveChanges();
            return new ServiceResult<ETermination>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ETermination> Update(ETermination model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ETermination>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ETermination model)
        {
            db.Termination.Remove(model);
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
