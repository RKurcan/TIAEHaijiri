using Riddhasoft.DB;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SPayrollConfiguration : Riddhasoft.Services.Common.IBaseService<EPayrollConfiguration>
    {
        RiddhaDBContext db = null;
        public SPayrollConfiguration()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EPayrollConfiguration>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EPayrollConfiguration>>()
            {
                Data = db.PayrollConfiguration,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EPayrollConfiguration> Add(EPayrollConfiguration model)
        {
            db.PayrollConfiguration.Add(model);
            db.SaveChanges();
            return new ServiceResult<EPayrollConfiguration>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EPayrollConfiguration> Update(EPayrollConfiguration model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EPayrollConfiguration>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EPayrollConfiguration model)
        {
            db.PayrollConfiguration.Remove(model);
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
