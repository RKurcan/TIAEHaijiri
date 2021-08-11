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
    public class STravelRequest : Riddhasoft.Services.Common.IBaseService<ETravelRequest>
    {
        RiddhaDBContext db = null;
        public STravelRequest()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ETravelRequest>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ETravelRequest>>()
            {
                Data = db.TravelRequest,
                Message = "",
                Status = ResultStatus.Ok,
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ETravelRequest> Add(ETravelRequest model)
        {
            db.TravelRequest.Add(model);
            db.SaveChanges();
            return new ServiceResult<ETravelRequest>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ETravelRequest> Update(ETravelRequest model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ETravelRequest>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ETravelRequest model)
        {
            db.TravelRequest.Remove(model);
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
