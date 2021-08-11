using Riddhasoft.DB;
using Riddhasoft.Entity.User;
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
    public class SRoleOnModule : Riddhasoft.Services.Common.IBaseService<ERoleOnModule>
    {
        RiddhaDBContext db = null;
        public SRoleOnModule()
        {
            db = new RiddhaDBContext();
        }
        public Common.ServiceResult<IQueryable<ERoleOnModule>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ERoleOnModule>>()
            {
                Data = db.RoleOnModule,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<ERoleOnModule> Add(ERoleOnModule model)
        {
            db.RoleOnModule.Add(model);
            db.SaveChanges();
            return new ServiceResult<ERoleOnModule>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<ERoleOnModule> Update(ERoleOnModule model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ERoleOnModule>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<int> Remove(ERoleOnModule model)
        {
            db.RoleOnModule.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Remove Successfully",
                Status = ResultStatus.Ok
            };
        }

        public void RemoveRange(List<ERoleOnModule> ModulesExistingLst)
        {
            db.RoleOnModule.RemoveRange(ModulesExistingLst);
            db.SaveChanges();
        }

        public ServiceResult<IQueryable<EOwnerPermission>> ListOwnerPermission()
        {
            return new ServiceResult<IQueryable<EOwnerPermission>>()
            {
                Data = db.OwnerPermission,
                Status = ResultStatus.Ok
            };

        }

        public void RemoveRangeOwnerPermission(List<EOwnerPermission> ModulesExistingLst)
        {
            db.OwnerPermission.RemoveRange(ModulesExistingLst);
            db.SaveChanges();
        }

        public ServiceResult<EOwnerPermission> AddOwnerPermission(EOwnerPermission roleOnModule)
        {
            db.OwnerPermission.Add(roleOnModule);
            db.SaveChanges();
            return new ServiceResult<EOwnerPermission>()
            {
                Data = roleOnModule,
                Status = ResultStatus.Ok,
                Message = "Added Successfully"
            };
        }
    }



}
