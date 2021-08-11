using Riddhasoft.DB;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.User.Entity;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SReseller : Riddhasoft.Services.Common.IBaseService<EReseller>
    {
        RiddhaDBContext db = null;
        public SReseller()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EReseller>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EReseller>>()
            {
                Data = db.Reseller,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EReseller> Add(EReseller model)
        {
            model = db.Reseller.Add(model);
            db.SaveChanges();
            return new ServiceResult<EReseller>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EReseller> Update(EReseller model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EReseller>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<EResellerLogin> UpdateResellerLogin(EResellerLogin model)
        {
            model.Reseller = null;
            model.User = null;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EResellerLogin>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EReseller model)
        {
            try
            {
                db.Reseller.Remove(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return new ServiceResult<int>()
            {
                Data = 1,
                Message = ex.Message,
                Status = ResultStatus.dataBaseError
            };
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Remove Successfully",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EResellerLogin>> ListResellerLogin()
        {
            return new ServiceResult<IQueryable<EResellerLogin>>()
            {
                Data = db.ResellerLogin,
                Status = ResultStatus.Ok
            };
        }
    }
}
