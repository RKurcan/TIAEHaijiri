using Riddhasoft.DB;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.Report
{
    public class SEmployeeOfficeVisitRequestReport
    {
        RiddhaDBContext db = null;
        public SEmployeeOfficeVisitRequestReport()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<List<EmployeeOfficeVisitRequestReportVm>> GenerateReport(DateTime fromDate, DateTime toDate, int branchId)
        {
            var data = db.OfficeVisitRequest.Where(x => x.Employee.BranchId == branchId && DbFunctions.TruncateTime(x.From) >= fromDate && DbFunctions.TruncateTime(x.To) <= toDate && x.IsApprove).ToList();
            var users = db.User.Where(x => x.BranchId == branchId);
            var result = (from c in data.ToList()
                          join d in users on c.ApprovedById equals d.Id
                          select new EmployeeOfficeVisitRequestReportVm()
                          {
                              EmployeeId = c.EmployeeId,
                              Approveby = d.FullName,
                              Department = c.Employee.Section.Department.Name,
                              Employee = c.Employee.Code + " - " + c.Employee.Name,
                              ApproveDate = c.ApprovedOn.Value.ToString("yyyy/MM/dd"),
                              Remark = c.Remark,
                              FromDate = c.From.ToString("yyyy/MM/dd"), 
                              ToDate = c.To.ToString("yyyy/MM/dd") ,
                          }).ToList();

            return new ServiceResult<List<EmployeeOfficeVisitRequestReportVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

    public class EmployeeOfficeVisitRequestReportVm
    {
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public string Department { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remark { get; set; }
        public string ApproveDate { get; set; }
        public string Approveby { get; set; }
    }
}
