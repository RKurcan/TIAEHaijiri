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
    public class SDepartmentWiseHoliday : Riddhasoft.Services.Common.IBaseService<EDepartmentWiseHoliday>
    {
        RiddhaDBContext db = null;
        public SDepartmentWiseHoliday()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<EDepartmentWiseHoliday> Add(EDepartmentWiseHoliday model)
        {
            db.DepartmentWiseHoliday.Add(model);
            db.SaveChanges();
            return new ServiceResult<EDepartmentWiseHoliday>()
            {
                Data = model,
                Message = "AddedSucess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<EDepartmentWiseHoliday>> Add(List<EDepartmentWiseHoliday> model)
        {
            db.DepartmentWiseHoliday.AddRange(model);
            db.SaveChanges();
            return new ServiceResult<List<EDepartmentWiseHoliday>>()
            {
                Data = model,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EDepartmentWiseHoliday>> List()
        {
            return new ServiceResult<IQueryable<EDepartmentWiseHoliday>>()
            {
                Data = db.DepartmentWiseHoliday,
                Status = ResultStatus.Ok,
                Message = "",
            };
        }

        public ServiceResult<int> Remove(EDepartmentWiseHoliday model)
        {
            db.DepartmentWiseHoliday.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSucess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(List<EDepartmentWiseHoliday> model)
        {
            db.DepartmentWiseHoliday.RemoveRange(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSucess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EDepartmentWiseHoliday> Update(EDepartmentWiseHoliday model)
        {
            db.Entry<EDepartmentWiseHoliday>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDepartmentWiseHoliday>()
            {
                Data = model,
                Message = "UpdateSucess",
                Status = ResultStatus.Ok
            };
        }
    }
}
