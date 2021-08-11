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
    public class SRoleOnController : Riddhasoft.Services.Common.IBaseService<ERoleOnController>
    {
        RiddhaDBContext db = null;
        public SRoleOnController()
        {
            db = new RiddhaDBContext();
        }

        public Common.ServiceResult<IQueryable<ERoleOnController>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ERoleOnController>>()
            {
                Data = db.RoleOnController,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<ERoleOnController> Add(ERoleOnController model)
        {
            db.RoleOnController.Add(model);
            db.SaveChanges();
            return new ServiceResult<ERoleOnController>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<ERoleOnController> Update(ERoleOnController model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ERoleOnController>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<int> Remove(ERoleOnController model)
        {
            db.RoleOnController.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Remove Successfully",
                Status = ResultStatus.Ok
            };
        }

        public void RemoveRange(List<ERoleOnController> ControllersExistingLst)
        {
            db.RoleOnController.RemoveRange(ControllersExistingLst);
            db.SaveChanges();
        }
    }
}
