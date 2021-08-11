using Riddhasoft.DB;
using Riddhasoft.HRM.Entities.Travel;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services.Travel
{
    public class STravelEstimate : Riddhasoft.Services.Common.IBaseService<ETravelEstimate>
    {
        RiddhaDBContext db = null;
        public STravelEstimate()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ETravelEstimate>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ETravelEstimate>>()
            {
                Data = db.TravelEstimate,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ETravelEstimate> Add(ETravelEstimate model)
        {
            db.TravelEstimate.Add(model);
            db.SaveChanges();
            return new ServiceResult<ETravelEstimate>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ETravelEstimate> Update(ETravelEstimate model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ETravelEstimate>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ETravelEstimate model)
        {
            db.TravelEstimate.Remove(model);
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
