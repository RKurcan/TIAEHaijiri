using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Services
{
    public class SHolidayEmployee
    {
        RiddhaDBContext db = null;
        public SHolidayEmployee()
        {
            db = new DB.RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EHolidayEmployee>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EHolidayEmployee>>
            {
                Data = db.HolidayEmployee,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<List<EHolidayEmployee>> AddRange(List<EHolidayEmployee> list)
        {
            db.HolidayEmployee.AddRange(list);
            db.SaveChanges();
            return new Riddhasoft.Services.Common.ServiceResult<List<EHolidayEmployee>>
            {
                Data = list,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> RemoveRange(List<EHolidayEmployee> list)
        {
            db.HolidayEmployee.RemoveRange(list);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
    }
}
