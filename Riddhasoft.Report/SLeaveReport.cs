using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Report
{
    public class SLeaveReport
    {
        RiddhaDBContext db = null;
        private string currentLanguage = "";
        public SLeaveReport(string currentLanguage = "")
        {
            db = new RiddhaDBContext();
            this.currentLanguage = currentLanguage;
        }
        public ServiceResult<List<EEmployeeLeaveSummary>> GetLeaveReportFromSp(DateTime FromDate, DateTime ToDate, int branchId,int FISCALYEAR)
        {
            var result = db.SP_GET_LEAVE_SUMMARY(FromDate, ToDate, branchId, currentLanguage, FISCALYEAR);
            List<EEmployeeLeaveSummary> reportList = new List<EEmployeeLeaveSummary>();
            result = (from c in result
                      select new EEmployeeLeaveSummary()
                      {
                          EmployeeId = c.EmployeeId,
                          Balance = c.Balance,
                          Code = c.Code,
                          LeaveId = c.LeaveId,
                          //Name =c.Code+" - "+ c.Name,
                          //LeaveName = c.LeaveName,
                          LeaveName = currentLanguage == "ne" && c.LeaveNameNp != null ? c.LeaveNameNp : c.LeaveName,
                          Name = c.Code + "-" + (currentLanguage == "ne" && c.NameNp != null ? c.NameNp : c.Name),
                          RemLeave = c.RemLeave,
                          TakenLeave = c.TakenLeave,
                          EmploymentStatus = c.EmploymentStatus
                      }).ToList();
            return new ServiceResult<List<EEmployeeLeaveSummary>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

}
