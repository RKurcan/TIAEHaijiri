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
    public class SEmployeeSkills : Riddhasoft.Services.Common.IBaseService<EEmployeeSkills>
    {
        RiddhaDBContext db = null;
        public SEmployeeSkills()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeSkills>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeSkills>>()
            {
                Data = db.EmployeeSkills,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeSkills> Add(EEmployeeSkills model)
        {
            db.EmployeeSkills.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeSkills>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeSkills> Update(EEmployeeSkills model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeSkills>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EEmployeeSkills model)
        {
            db.EmployeeSkills.Remove(model);
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
