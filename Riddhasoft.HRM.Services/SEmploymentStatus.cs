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
    public class SEmploymentStatus : Riddhasoft.Services.Common.IBaseService<EEmploymentStatus>
    {
        RiddhaDBContext db = null;
        public SEmploymentStatus()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmploymentStatus>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmploymentStatus>>()
            {
                Data = db.EmploymentStatus,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmploymentStatus> Add(EEmploymentStatus model)
        {
            db.EmploymentStatus.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmploymentStatus>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmploymentStatus> Update(EEmploymentStatus model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmploymentStatus>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EEmploymentStatus model)
        {
            db.EmploymentStatus.Remove(model);
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
