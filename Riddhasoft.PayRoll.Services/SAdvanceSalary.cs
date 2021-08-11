using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SAdvanceSalary : Riddhasoft.Services.Common.IBaseService<EAdvanceSalary>
    {
        DB.RiddhaDBContext db = null;
        public SAdvanceSalary()
        {
            db = new DB.RiddhaDBContext();
        }
        public ServiceResult<EAdvanceSalary> Add(EAdvanceSalary model)
        {
            db.AdvanceSalary.Add(model);
            db.SaveChanges();
            return new ServiceResult<EAdvanceSalary>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EAdvanceSalary>> List()
        {
            return new ServiceResult<IQueryable<EAdvanceSalary>>()
            {
                Data  = db.AdvanceSalary,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EAdvanceSalary model)
        {
            db.AdvanceSalary.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EAdvanceSalary> Update(EAdvanceSalary model)
        {
            db.Entry<EAdvanceSalary>(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return new ServiceResult<EAdvanceSalary>()
            {

                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
