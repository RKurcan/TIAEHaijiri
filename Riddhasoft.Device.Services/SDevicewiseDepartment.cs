using Riddhasoft.DB;
using Riddhasoft.Device.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Device.Services
{
    public class SDevicewiseDepartment : Riddhasoft.Services.Common.IBaseService<EDevicewiseDepartment>
    {
        RiddhaDBContext db = null;
        public SDevicewiseDepartment()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EDevicewiseDepartment>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EDevicewiseDepartment>>()
            {
                Data = db.DevicewiseDepartment,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDevicewiseDepartment> Add(EDevicewiseDepartment model)
        {
            db.DevicewiseDepartment.Add(model);
            db.SaveChanges();
            return new ServiceResult<EDevicewiseDepartment>()
            {
                Data = model,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDevicewiseDepartment> Update(EDevicewiseDepartment model)
        {
            db.Entry<EDevicewiseDepartment>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDevicewiseDepartment>()
            {
                Data = model,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EDevicewiseDepartment model)
        {
            db.DevicewiseDepartment.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(List<EDevicewiseDepartment> list)
        {
            db.DevicewiseDepartment.RemoveRange(list);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }
}
