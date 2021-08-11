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
    public class SLanguage : Riddhasoft.Services.Common.IBaseService<ELanguage>
    {
        RiddhaDBContext db = null;
        public SLanguage()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ELanguage>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ELanguage>>()
            {
                Data = db.Language,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELanguage> Add(ELanguage model)
        {
            db.Language.Add(model);
            db.SaveChanges();
            return new ServiceResult<ELanguage>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELanguage> Update(ELanguage model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ELanguage>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ELanguage model)
        {
            db.Language.Remove(model);
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
