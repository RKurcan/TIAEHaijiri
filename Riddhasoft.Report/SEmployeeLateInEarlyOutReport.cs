using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Riddhasoft.Report
{
    public class SEmployeeLateInEarlyOutReport
    {
        RiddhaDBContext db = null;
        public SEmployeeLateInEarlyOutReport()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<List<EmployeeLateInEarlyOutReportVm>> GenerateReport(DateTime FromDate, DateTime ToDate, int branchId)
        {
            var data = db.EmployeeLateInAndEarlyOutRequest.Where(x => x.Employee.BranchId == branchId && DbFunctions.TruncateTime(x.RequestedDate) >= DbFunctions.TruncateTime(FromDate) && DbFunctions.TruncateTime(x.RequestedDate) <= DbFunctions.TruncateTime(ToDate) && x.IsApproved).ToList();
            var users = db.User.Where(x => x.BranchId == branchId);
            var result = (from c in data.ToList()
                          join d in users on c.ApproveById equals d.Id
                          select new EmployeeLateInEarlyOutReportVm()
                          {
                              EmployeeId = c.EmployeeId,
                              ActualInTime = c.ActualInTime.HasValue ? c.ActualInTime.Value.ToString(@"hh\:mm") : "",
                              ActualOutTime = c.ActualOutTime.HasValue ? c.ActualOutTime.Value.ToString(@"hh\:mm") : "",
                              ApproveBy = d.FullName,
                              Department = c.Employee.Section.Department.Name,
                              Employee = c.Employee.Code + " - " + c.Employee.Name,
                              ApproveDate = c.ApproveDate.Value.ToString("yyyy/MM/dd"),
                              EarlyOutTime = c.EarlyOutTime.HasValue ? c.EarlyOutTime.Value.ToString(@"hh\:mm") : "",
                              LateInEarlyOutRequestType = Enum.GetName(typeof(LateInEarlyOutRequestType), c.LateInEarlyOutRequestType),
                              LateInTime = c.LateInTime.HasValue ? c.LateInTime.Value.ToString(@"hh\:mm") : "",
                              PlannedInTime = c.PlannedInTime.HasValue ? c.PlannedInTime.Value.ToString(@"hh\:mm") : "",
                              PlannedOutTime = c.PlannedOutTime.HasValue ? c.PlannedOutTime.Value.ToString(@"hh\:mm") : "",
                              Remark = c.Remark,
                              RequestedDate = c.RequestedDate.ToString("yyyy/MM/dd"),
                              Section = c.Employee.Section.Name,
                          }).ToList();
            return new ServiceResult<List<EmployeeLateInEarlyOutReportVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }
    public class EmployeeLateInEarlyOutReportVm
    {
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Remark { get; set; }
        public string ApproveBy { get; set; }
        public string ApproveDate { get; set; }
        public string SystemDate { get; set; }
        public string IsApproved { get; set; }
        public string RequestedDate { get; set; }
        public string ActualInTime { get; set; }
        public string ActualOutTime { get; set; }
        public string PlannedInTime { get; set; }
        public string PlannedOutTime { get; set; }
        public string LateInTime { get; set; }
        public string EarlyOutTime { get; set; }
        public string LateInEarlyOutRequestType { get; set; }
    }
}
