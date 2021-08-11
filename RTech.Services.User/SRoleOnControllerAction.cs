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
    public class SRoleOnControllerAction : Riddhasoft.Services.Common.IBaseService<ERoleOnControllerAction>
    {
        RiddhaDBContext db = null;
        public SRoleOnControllerAction()
        {
            db = new RiddhaDBContext();
        }
        public Common.ServiceResult<IQueryable<ERoleOnControllerAction>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ERoleOnControllerAction>>()
            {
                Data = db.RoleOnControllerAction,
                Message="",
                Status=ResultStatus.Ok
            };
        }

        public Common.ServiceResult<ERoleOnControllerAction> Add(ERoleOnControllerAction model)
        {
            db.RoleOnControllerAction.Add(model);
            db.SaveChanges();
            return new ServiceResult<ERoleOnControllerAction>()
            {
                Data=model,
                Message="Added Successfully",
                Status=ResultStatus.Ok
            };
        }

        public Common.ServiceResult<ERoleOnControllerAction> Update(ERoleOnControllerAction model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ERoleOnControllerAction>()
            {
                Data=model,
                Message="Updated Successfully",
                Status=ResultStatus.Ok
            };
        }

        public Common.ServiceResult<int> Remove(ERoleOnControllerAction model)
        {
            db.RoleOnControllerAction.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data=1,
                Message="Removed Successfully",
                Status=ResultStatus.Ok
            };
        }

        public void RemoveRange(List<ERoleOnControllerAction> actionsExistingList)
        {
            db.RoleOnControllerAction.RemoveRange(actionsExistingList);
            db.SaveChanges();
        }


    }
}
