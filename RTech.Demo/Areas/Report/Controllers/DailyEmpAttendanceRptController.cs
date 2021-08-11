using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.Report.ReportViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RTech.Demo.Utilities;
using Riddhasoft.Services.Common;

namespace RTech.Demo.Areas.Report.Controllers
{
    public class DailyEmpAttendanceRptController : Controller
    {
        //
        // GET: /Report/DailyEmpAttendanceRpt/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateReport(string id, string onDate)
        {
            //WebApiApplication.configureCulture();
            int CompanyId = RiddhaSession.CurrentUser.Branch.CompanyId;
            string[] employees = id.Split(',');///Areas/Report/RDLS/DailyEmployeePerformanceReport.rdlc
            string ReportPath = @"\Areas\Report\RDLS\DailyEmployeePerformanceReport.rdlc";
            SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(RiddhaSession.BranchId.ToInt(),RiddhaSession.FYId);
            Riddhasoft.Services.Common.ServiceResult<List<AttendanceReportDetailViewModel>> result;
            result = reportService.Get(onDate.ToDateTime(),CompanyId);

            var reportData = (from c in result.Data
                              join d in employees
                              on c.EmployeeId equals int.Parse(d)
                              select c
                                 ).ToList();
            Session["ReportPath"] = ReportPath;
            Session["ReportData"] = reportData.ToDataTable<AttendanceReportDetailViewModel>();
            Session["ReportTitle"] = "Daily Attendance Report";
            Session["CompanyName"] = "Riddhasoft";
            return RedirectToAction("Index", "ReportViewer", new { area="Report"});
        }
	}
}