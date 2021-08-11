using Riddhasoft.DB;
using Riddhasoft.Services.Common;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.User
{
    public class SUser : Riddhasoft.Services.Common.IBaseService<EUser>
    {
        RiddhaDBContext db = null;
        public SUser()
        {
            db = new RiddhaDBContext();
        }

        public Common.ServiceResult<IQueryable<EUser>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EUser>>()
            {
                Data = db.User,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<EUser> Add(EUser model)
        {
            db.User.Add(model);
            db.SaveChanges();
            return new ServiceResult<EUser>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<EUser> Update(EUser model)
        {
            var user = db.User.Where(x => x.Id == model.Id).Single();
            user.FullName = model.FullName;
            user.Name = model.Name;
            user.Password = model.Password;
            user.PhotoURL = model.PhotoURL;
            user.RoleId = user.RoleId == null ? null : model.RoleId;
            user.BranchId = model.BranchId;
            user.Email = model.Email;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EUser>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<int> Remove(EUser model)
        {
            model.IsDeleted = true;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public void AddCompanyLogin(ECompanyLogin compLogin)
        {
            db.CompanyLogin.Add(compLogin);
            db.SaveChanges();
        }
        public void AddResellerLogin(EResellerLogin resellerLogin)
        {
            db.ResellerLogin.Add(resellerLogin);
            db.SaveChanges();
        }
        public IQueryable<EResellerLogin> GetResellerLoginLst()
        {
            return db.ResellerLogin;
        }
        public int FindResellerUser(int resellerId)
        {
            var resellerLoginUser = db.ResellerLogin.Where(x => x.ResellerId == resellerId).FirstOrDefault() ?? new EResellerLogin();
            return resellerLoginUser.UserId;
        }

        public int FindCompanyUser(int compId)
        {
            var compLoginUser = db.CompanyLogin.Where(x => x.CompanyId == compId).FirstOrDefault() ?? new ECompanyLogin();
            return compLoginUser.UserId;
        }

        public bool CheckDuplicate(string userName, int branchId, int userId)
        {
            if (userId==0)
            {
                return db.User.Where(x => x.BranchId == branchId && x.IsDeleted == false && x.Name.ToUpper() == userName.ToUpper()).Any();
            }
            else
            {
                return db.User.Where(x => x.BranchId == branchId && x.IsDeleted == false && x.Name.ToUpper() == userName.ToUpper() && x.Id!=userId).Any();
            }
        }
    }
}
