using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.Report.ReportViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
namespace RTech.Demo.Areas.Report.Controllers
{
    public class DailyLeaveRptController : Controller
    {
        //
        // GET: /Report/DailyLeaveRpt/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GenerateReport(string id, string onDate)
        {
            WebApiApplication.configureCulture();

            string[] employees = id.Split(',');///Areas/Report/RDLS/DailyEmployeePerformanceReport.rdlc
            string ReportPath = @"\Areas\Report\RDLS\DailyLeaveReport.rdlc";
            SDailyLeavReport reportService = new SDailyLeavReport();
            Riddhasoft.Services.Common.ServiceResult<List<AttendanceReportDetailViewModel>> result;
            result = reportService.Get(onDate.ToDateTime());

            var reportData = (from c in result.Data
                              join d in employees
                              on c.EmployeeId equals int.Parse(d)
                              select c
                                 ).ToList();
            Session["ReportPath"] = ReportPath;
            Session["ReportData"] = reportData.ToDataTable<AttendanceReportDetailViewModel>();
            Session["ReportTitle"] = "Daily Leave  Report";
            Session["CompanyName"] = "Riddhasoft";
            return RedirectToAction("Index", "ReportViewer", new { area = "Report" });
        }
    }
}