using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Services
{
    public class SOfficeVisitRequest : Riddhasoft.Services.Common.IBaseService<EOfficeVisitRequest>
    {
        RiddhaDBContext db = null;
        public SOfficeVisitRequest()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<EOfficeVisitRequest> Add(EOfficeVisitRequest model)
        {
            db.OfficeVisitRequest.Add(model);
            db.SaveChanges();
            return new ServiceResult<EOfficeVisitRequest>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EOfficeVisitRequest>> List()
        {
            return new ServiceResult<IQueryable<EOfficeVisitRequest>>()
            {
                Data = db.OfficeVisitRequest,
                Message ="",
                Status =ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EOfficeVisitRequest model)
        {
            db.OfficeVisitRequest.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EOfficeVisitRequest> Update(EOfficeVisitRequest model)
        {
            db.Entry<EOfficeVisitRequest>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EOfficeVisitRequest>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
