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
    public class SLeaveBalance : Riddhasoft.Services.Common.IBaseService<ELeaveBalance>
    {
        RiddhaDBContext db = null;
        public SLeaveBalance()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ELeaveBalance>> List()
        {
            return new ServiceResult<IQueryable<ELeaveBalance>>()
            {
                Data = db.LeaveBalance,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELeaveBalance> Add(ELeaveBalance model)
        {
            db.LeaveBalance.Add(model);
            db.SaveChanges();
            return new ServiceResult<ELeaveBalance>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELeaveBalance> Update(ELeaveBalance model)
        {
            var remainingLeave = 0; //new SLeaveApplication().GetRemBal(model.LeaveMasterId, model.EmployeeId).Data;
            model.RemainingBalance = remainingLeave;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ELeaveBalance>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public void UpdateOpeningBalance(List<ELeaveBalance> leaveBalances)
        {
            leaveBalances = leaveBalances.Where(x => x.OpeningBalance > 0).ToList();
            foreach (var id in leaveBalances.Select(x=>x.EmployeeId).Distinct().ToArray())
            {
                var existingBalances = db.LeaveBalance.Where(x => x.EmployeeId == id).ToList();
                existingBalances.ForEach(x =>RemoveEmployeAndLeaveObject(x));
                if (existingBalances.Count>0)
                {
                    db.LeaveBalance.RemoveRange(existingBalances); 
                }
            }
            leaveBalances.ForEach(x => RemoveEmployeAndLeaveObject(x));
            db.LeaveBalance.AddRange(leaveBalances);

            db.SaveChanges();
        }

        private void RemoveEmployeAndLeaveObject(ELeaveBalance x)
        {
            x.Employee = null;
            x.LeaveMaster = null;
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ELeaveBalance model)
        {
            db.LeaveBalance.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> RemoveRange(List<ELeaveBalance> lst)
        {
            db.LeaveBalance.RemoveRange(lst);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
