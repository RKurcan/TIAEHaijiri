using Riddhasoft.DB;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Report
{
    public class SEmployeeMultiPunchReport
    {
        RiddhaDBContext db = null;
        public int[] FilteredEmployeeIDs { get; set; }
        public string currentLanguage { get; set; }
        public SEmployeeMultiPunchReport()
        {
            db = new RiddhaDBContext();
        }
        public ServiceResult<List<EmployeeMultiPunchReportVm>> GetMultiPunchAttendanceReport(DateTime fromDate, DateTime toDate, int branchId)
        {
            string emp = string.Join(",", FilteredEmployeeIDs);
            var result = db.SP_Employee_MultiPunch_report(branchId, fromDate, toDate,currentLanguage);
            if (FilteredEmployeeIDs.Length > 0)
            {
                result = (from c in result
                          join d in FilteredEmployeeIDs
                          on c.EmployeeId equals d
                          select c
                                      ).ToList();
            }
            List<EmployeeMultiPunchReportVm> reportList = new List<EmployeeMultiPunchReportVm>();
            reportList = (from c in result
                          select new EmployeeMultiPunchReportVm()
                          {
                              Code = c.Code,
                              Date = c.Date.ToString("yyyy/MM/dd"),
                              EmployeeId = c.EmployeeId,
                              HolidayId = c.HolidayId,
                              HoliodayName = c.HoliodayName,
                              LeaveName = c.LeaveName,
                              Name = c.Code+"-"+c.Name,
                              PunchTime = c.PunchTime,
                              Day = c.Day
                          }).ToList();
            return new ServiceResult<List<EmployeeMultiPunchReportVm>>()
            {
                Data = reportList,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

    public class EmployeeMultiPunchReportVm
    {
        public int EmployeeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Day { get; set; }
        public string PunchTime { get; set; }
        public int? HolidayId { get; set; }
        public string HoliodayName { get; set; }
        public string LeaveName { get; set; }

    }
}
