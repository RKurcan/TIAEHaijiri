using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.Employee.Services
{
    public class SEmployeeLateInAndEarlyOutRequest : Riddhasoft.Services.Common.IBaseService<EEmployeeLateInAndEarlyOutRequest>
    {
        RiddhaDBContext db = null;
        public SEmployeeLateInAndEarlyOutRequest()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<EEmployeeLateInAndEarlyOutRequest> Add(EEmployeeLateInAndEarlyOutRequest model)
        {
            db.EmployeeLateInAndEarlyOutRequest.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeLateInAndEarlyOutRequest>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EEmployeeLateInAndEarlyOutRequest>> List()
        {
            return new ServiceResult<IQueryable<EEmployeeLateInAndEarlyOutRequest>>()
            {
                Data = db.EmployeeLateInAndEarlyOutRequest,
                Status = ResultStatus.Ok,
                Message = "",
            };
        }

        public ServiceResult<int> Remove(EEmployeeLateInAndEarlyOutRequest model)
        {
            db.EmployeeLateInAndEarlyOutRequest.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmployeeLateInAndEarlyOutRequest> Update(EEmployeeLateInAndEarlyOutRequest model)
        {
            db.Entry<EEmployeeLateInAndEarlyOutRequest>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeLateInAndEarlyOutRequest>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
