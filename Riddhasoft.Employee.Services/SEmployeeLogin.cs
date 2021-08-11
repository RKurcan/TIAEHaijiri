using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Services
{
    public class SEmployeeLogin : Riddhasoft.Services.Common.IBaseService<EEmployeeLogin>
    {
        RiddhaDBContext db = null;
        public SEmployeeLogin()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeLogin>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeLogin>>()
            {
                Data = db.EmployeeLogin,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeLogin> Add(EEmployeeLogin model)
        {
            if (checkDuplicateUser(model.UserName,0))
            {
                return new ServiceResult<EEmployeeLogin>()
                {
                    Data = null,
                    Message = "Duplicate",
                    Status = ResultStatus.processError
                };
            }
            else
            {
                db.EmployeeLogin.Add(model);
                db.SaveChanges();
                return new ServiceResult<EEmployeeLogin>()
                {
                    Data = model,
                    Message = "AddedSuccess",
                    Status = ResultStatus.Ok
                };
            }
        }
        private bool checkDuplicateUser(string userName,int id)
        {
            bool result = false;
            if (id==0)
            {
               result= db.EmployeeLogin.Where(x => x.UserName.ToLower() == userName.ToLower()).Any();
            }
            else
            {
               result= db.EmployeeLogin.Where(x => x.UserName.ToLower() == userName.ToLower() && x.Id!=id).Any();
            }
            return result;
        }
        public Riddhasoft.Services.Common.ServiceResult<EEmployeeLogin> Update(EEmployeeLogin model)
        {
            if (checkDuplicateUser(model.UserName, model.Id))
            {
                return new ServiceResult<EEmployeeLogin>()
                {
                    Data = model,
                    Message = "Duplicate",
                    Status = ResultStatus.processError
                };
            }
            else
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return new ServiceResult<EEmployeeLogin>()
                {
                    Data = model,
                    Message = "UpdatedSuccess",
                    Status = ResultStatus.Ok
                };
            }
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EEmployeeLogin model)
        {
            db.EmployeeLogin.Remove(model);
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
