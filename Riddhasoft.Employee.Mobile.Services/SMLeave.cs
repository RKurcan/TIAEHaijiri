using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMLeave
    {
        DB.RiddhaDBContext db;
        public SMLeave()
        {
            db = new DB.RiddhaDBContext();
        }
        public List<EMLeaveInfo> GetLeaveInfo(int empId)
        {
            List<EMLeaveInfo> leaveInfos = db.SP_GET_LEAVEINFORMATION(empId).ToList();
            return leaveInfos;
        }
        public MobileResult<ELeaveApplication> RequestLeave(ELeaveApplication model)
        {
            db.LeaveApplication.Add(model);
            db.SaveChanges();
            return new MobileResult<ELeaveApplication>()
            {
                Data = model,
                Message = "Leave Requested Successfully",
                Status = MobileResultStatus.Ok
            };
        }
    }
}
