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
    public class SWeeklyRoster
    {
        RiddhaDBContext db = null;

        public SWeeklyRoster()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EWeeklyRoster>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EWeeklyRoster>>()
            {
                Data = db.WeeklyRoster,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public void AddWeeklyRosterRange(List<EWeeklyRoster> dataToSave)
        {
            db.WeeklyRoster.AddRange(dataToSave);
            db.SaveChanges();
        }
        public ServiceResult<int> RemoveWeekly(EWeeklyRoster model)
        {
            model = (db.WeeklyRoster.Where(x => x.EmployeeId == model.EmployeeId && x.ShiftId == model.ShiftId && x.Day == model.Day).FirstOrDefault());
            if (model != null)
            {
                db.WeeklyRoster.Remove(model);
                db.SaveChanges();
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Removed Successfully",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> RemoveByEmployeeId(int EmployeeId)
        {
            var existingRoster = db.WeeklyRoster.Where(x => x.EmployeeId == EmployeeId).ToList();
            if (existingRoster.Count > 0)
            {
                existingRoster.ForEach(x => clearRosterProxy(x));
                db.WeeklyRoster.RemoveRange(existingRoster);
                db.SaveChanges();
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Removed Successfully",
                Status = ResultStatus.Ok
            };
        }
        private EWeeklyRoster clearRosterProxy(EWeeklyRoster x)
        {
            x.Employee = null;
            x.Shift = null;
            return x;
        }
    }
}
