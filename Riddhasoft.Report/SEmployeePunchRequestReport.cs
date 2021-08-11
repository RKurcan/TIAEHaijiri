using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.Report
{
    public class SEmployeePunchRequestReport
    {
        RiddhaDBContext db = null;
        public SEmployeePunchRequestReport()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<List<EmployeePunchRequestReportVm>> GenerateReport(DateTime FromDate, DateTime ToDate, int branchId)
        {
            var data = db.ManualPunchRequest.Where(x => x.Employee.BranchId == branchId && DbFunctions.TruncateTime(x.DateTime) >= DbFunctions.TruncateTime(FromDate) && DbFunctions.TruncateTime(x.DateTime) <= DbFunctions.TruncateTime(ToDate) && x.IsApproved).ToList();
            var users = db.User.Where(x => x.BranchId == branchId);
            var result = (from c in data.ToList()
                          join d in users on c.ApproveBy equals d.Id
                          select new EmployeePunchRequestReportVm()
                          {
                              EmployeeId = c.EmployeeId,
                              ApproveBy = d.FullName,
                              Department = c.Employee.Section.Department.Name,
                              Employee = c.Employee.Code + " - " + c.Employee.Name,
                              ApproveDate = c.ApproveDate.Value.ToString("yyyy/MM/dd"),
                              Remark = c.Remark,
                              Section = c.Employee.Section.Name,
                              RequestDate = c.DateTime.ToString("yyyy/MM/dd"),
                          }).ToList();
            return new ServiceResult<List<EmployeePunchRequestReportVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

    public class EmployeePunchRequestReportVm
    {
        public string Employee { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public int EmployeeId { get; set; }
        public string RequestDate { get; set; }
        public string Remark { get; set; }
        public string ApproveBy { get; set; }
        public string ApproveDate { get; set; }
    }
}
