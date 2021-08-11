using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Services
{
   public class SGradeGroup: Riddhasoft.Services.Common.IBaseService<EGradeGroup>
    {
       RiddhaDBContext db = null;
       public SGradeGroup()
       {
           db = new RiddhaDBContext();
       }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EGradeGroup>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EGradeGroup>>()
            {
                Data = db.GradeGroup,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EGradeGroup> Add(EGradeGroup model)
        {
            db.GradeGroup.Add(model);
            db.SaveChanges();
            return new ServiceResult<EGradeGroup>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok,

            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EGradeGroup> Update(EGradeGroup model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EGradeGroup>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };

        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EGradeGroup model)
        {
            if (model != null)
            {
                int existingUser = db.Employee.Where(x => x.GradeGroupId == model.Id).Count();
                if (existingUser > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Status = ResultStatus.dataBaseError,
                        Message = string.Format("There are {0} numbers of Employee in this grade group. Gradegroup can't be deleted.", existingUser)
                    };
                }
                db.GradeGroup.Remove(model);
                db.SaveChanges();
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = "RemoveSuccess",
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<int>()
            {
                Data = 0,
                Message = "ProcessError",
                Status = ResultStatus.processError
            };
        }
    }
}
