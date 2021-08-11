using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SBranch : Riddhasoft.Services.Common.IBaseService<EBranch>
    {
        RiddhaDBContext db = null;
        private bool _proxy;
        public SBranch()
        {
            db = new DB.RiddhaDBContext();
        }

        public SBranch(bool proxy)
        {
            this._proxy = proxy;
            db = new DB.RiddhaDBContext();
            if (this._proxy == false)
            {
                db.Configuration.ProxyCreationEnabled = false;
            }
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EBranch>> List()
        {
            return new ServiceResult<IQueryable<EBranch>>()
            {
                Data = db.Branch.OrderBy(x => x.Name),
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<EBranch> Add(EBranch model)
        {
            db.Branch.Add(model);
            db.SaveChanges();
            return new ServiceResult<EBranch>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EBranch> Update(EBranch model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EBranch>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };

        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EBranch model)
        {

            try
            {
                int departmentCount = db.Department.Count(x => x.BranchId == model.Id);
                if (departmentCount > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("There {1} {0} department on this branch. Branch Can't be deleted.", departmentCount, departmentCount == 1 ? "is" : "are"),
                        Status = ResultStatus.dataBaseError
                    };
                }

                int userCount = db.User.Where(x => x.BranchId == model.Id).Count();
                if (userCount > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("There {1} {0} user on this branch. Branch Can't be deleted.", userCount, userCount == 1 ? "is" : "are"),
                        Status = ResultStatus.dataBaseError
                    };
                }

                model = db.Branch.Remove(db.Branch.Find(model.Id));
                db.SaveChanges();
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = "RemoveSuccess",
                    Status = ResultStatus.Ok
                };
            }
            catch (SqlException ex)
            {

                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.dataBaseError
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.unHandeledError
                };
            }
        }

        
    }
}
