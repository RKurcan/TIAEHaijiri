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
    public class SResignation : Riddhasoft.Services.Common.IBaseService<EResignation>
    {
        RiddhaDBContext db = null;
        public SResignation()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EResignation>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EResignation>>()
            {
                Data = db.Resignation,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EResignation> Add(EResignation model)
        {
            db.Resignation.Add(model);
            db.SaveChanges();
            return new ServiceResult<EResignation>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EResignation> Update(EResignation model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EResignation>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EResignation model)
        {
            db.Resignation.Remove(model);
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
