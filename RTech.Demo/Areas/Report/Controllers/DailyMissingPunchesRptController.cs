using RTech.Demo.Utilities;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.Report.ReportViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Riddhasoft.Services.Common;

namespace RTech.Demo.Areas.Report.Controllers
{
    public class DailyMissingPunchesRptController : Controller
    {
        //
        // GET: /Report/DailyMissingPunchesRpt/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateReport(string id, string onDate)
        {
            WebApiApplication.configureCulture();

            string[] employees = id.Split(',');///Areas/Report/RDLS/DailyEmployeePerformanceReport.rdlc
            string ReportPath = @"\Areas\Report\RDLS\DailyMissingPunchesReport.rdlc";
            SDailyMissingPunchesReport reportService = new SDailyMissingPunchesReport();
            Riddhasoft.Services.Common.ServiceResult<List<AttendanceReportDetailViewModel>> result;
            result = reportService.Get(onDate.ToDateTime());

            var reportData = (from c in result.Data
                              join d in employees
                              on c.EmployeeId equals int.Parse(d)
                              select c
                                 ).ToList();
            Session["ReportPath"] = ReportPath;
            Session["ReportData"] = reportData.ToDataTable<AttendanceReportDetailViewModel>();
            Session["ReportTitle"] = "Daily Missing Punches Report";
            Session["CompanyName"] = "Riddhasoft";
            return RedirectToAction("Index", "ReportViewer", new { area = "Report" });
        }
	}
}