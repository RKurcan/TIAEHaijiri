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
    public class SEmployeeLanguage : Riddhasoft.Services.Common.IBaseService<EEmployeeLanguage>
    {
        RiddhaDBContext db = null;
        public SEmployeeLanguage()
        {
            db = new RiddhaDBContext();
        }


        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeLanguage>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeLanguage>>()
            {
                Data = db.EmployeeLanguage,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeLanguage> Add(EEmployeeLanguage model)
        {
            db.EmployeeLanguage.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeLanguage>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeLanguage> Update(EEmployeeLanguage model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeLanguage>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EEmployeeLanguage model)
        {
            db.EmployeeLanguage.Remove(model);
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
