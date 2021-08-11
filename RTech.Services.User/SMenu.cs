using Riddhasoft.DB;
using Riddhasoft.Entity.User;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.User
{
    public class SMenu
    {
        RiddhaDBContext db = null;
        public SMenu()
        {
            db = new RiddhaDBContext();
        }
        public ServiceResult<IQueryable<EMenu>> MenuList()
        {
            return new ServiceResult<IQueryable<EMenu>>()
            {
                Data = db.Menu,
                Message = "",
                Status = ResultStatus.Ok,
            };
        }
        public ServiceResult<IQueryable<EMenuAction>> ActionList()
        {
            return new ServiceResult<IQueryable<EMenuAction>>()
            {
                Data = db.MenuAction,
                Message = "",
                Status = ResultStatus.Ok,
            };
        }


    }
}
