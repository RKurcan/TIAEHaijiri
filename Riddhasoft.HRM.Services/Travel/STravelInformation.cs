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
    public class STravelInformation : Riddhasoft.Services.Common.IBaseService<ETravelInformation>
    {
        RiddhaDBContext db = null;
        public STravelInformation()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ETravelInformation>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ETravelInformation>>()
            {
                Data = db.TravelInformation,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ETravelInformation> Add(ETravelInformation model)
        {
            db.TravelInformation.Add(model);
            db.SaveChanges();
            return new ServiceResult<ETravelInformation>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ETravelInformation> Update(ETravelInformation model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ETravelInformation>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ETravelInformation model)
        {
            db.TravelInformation.Remove(model);
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
