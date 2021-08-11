using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Services
{
    public class SRoster : Riddhasoft.Services.Common.IBaseService<ERoster>
    {
        RiddhaDBContext db = null;
        public SRoster()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ERoster>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ERoster>>()
            {
                Data = db.Roster,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ERoster> Add(ERoster model)
        {
            db.Roster.Add(model);
            db.SaveChanges();
            return new ServiceResult<ERoster>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ERoster> Update(ERoster model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ERoster>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ERoster model)
        {
            db.Roster.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> RemoveByEmployeeId(int EmployeeId,int Year,int MonthId)
        {
            var existingRoster = db.Roster.Where(x=>x.EmployeeId==EmployeeId && x.Date.Month==MonthId && x.Date.Year==Year).ToList();
            if (existingRoster.Count > 0)
            {
                existingRoster.ForEach(x =>clearRosterProxy(x));
                db.Roster.RemoveRange(existingRoster);
                db.SaveChanges();
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Removed Successfully",
                Status = ResultStatus.Ok
            };
        }

        private ERoster clearRosterProxy(ERoster x)
        {
            x.Employee = null;
            x.Shift = null;
            return x;
        }

        public void AddRange(List<ERoster> rosterLst,int empId)
        {
            var existingRosters = (from c in db.Roster.Where(x=>x.EmployeeId==empId).ToList()
                                   join d in rosterLst on c.Date.Date equals d.Date.Date
                                   select c).ToList();
            if (existingRosters.Count()>0)
            {
                db.Roster.RemoveRange(existingRosters);
                db.SaveChanges();
            }
            rosterLst = rosterLst.Where(x => x.ShiftId != 0).ToList();
            db.Roster.AddRange(rosterLst);
            db.SaveChanges();
        }

        public void AddRange(List<ERoster> dataToSave)
        {
            db.Roster.AddRange(dataToSave);
            db.SaveChanges();
        }
    }
}
