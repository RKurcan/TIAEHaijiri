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
    public class SLicense : Riddhasoft.Services.Common.IBaseService<ELicense>
    {
        RiddhaDBContext db = null;
        public SLicense()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ELicense>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ELicense>>()
            {
                Data = db.License,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELicense> Add(ELicense model)
        {
            db.License.Add(model);
            db.SaveChanges();
            return new ServiceResult<ELicense>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELicense> Update(ELicense model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ELicense>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ELicense model)
        {
            db.License.Remove(model);
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
