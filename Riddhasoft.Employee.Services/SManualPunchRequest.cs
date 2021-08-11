using System.Data.Entity;
using System.Linq;
using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;

namespace Riddhasoft.Employee.Services
{
    public class SManualPunchRequest: Riddhasoft.Services.Common.IBaseService<EManualPunchRequest>
    {
        RiddhaDBContext db = null;
        public SManualPunchRequest()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<EManualPunchRequest> Add(EManualPunchRequest model)
        {
            db.ManualPunchRequest.Add(model);
            db.SaveChanges();
            return new ServiceResult<EManualPunchRequest>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EManualPunchRequest>> List()
        {
            return new ServiceResult<IQueryable<EManualPunchRequest>>()
            {
                Data = db.ManualPunchRequest,
                Message ="",
                Status =ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EManualPunchRequest model)
        {
            db.ManualPunchRequest.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EManualPunchRequest> Update(EManualPunchRequest model)
        {
            db.Entry<EManualPunchRequest>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EManualPunchRequest>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
