using Riddhasoft.DB;
using Riddhasoft.HRM.Entities.Qualification;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services.Qualification
{
    public class SEmployeeLicense : Riddhasoft.Services.Common.IBaseService<EEmployeeLicense>
    {
        RiddhaDBContext db = null;
        public SEmployeeLicense()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeLicense>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeLicense>>()
            {
                Data = db.EmployeeLicense,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeLicense> Add(EEmployeeLicense model)
        {
            db.EmployeeLicense.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeLicense>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeLicense> Update(EEmployeeLicense model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeLicense>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EEmployeeLicense model)
        {
            db.EmployeeLicense.Remove(model);
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
