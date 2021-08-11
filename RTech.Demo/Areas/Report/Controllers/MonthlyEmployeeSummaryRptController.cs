using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.Report.ReportViewModel;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Riddhasoft.Services.Common;

namespace RTech.Demo.Areas.Report.Controllers
{
    public class MonthlyEmployeeSummaryRptController : Controller
    {
        //
        // GET: /Report/MonthlyEmployeeSummaryRpt/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateReport(string id, string startDate, string endDate, int Daywise)
        {
            string ReportPath = @"\Areas\Report\RDLS\MonthlyEmployeeSummaryReport.rdlc";
            if (Daywise==1)
            {
                ReportPath = @"\Areas\Report\RDLS\MonthlyEmployeeSummaryDaywiseReport.rdlc";
            }
            WebApiApplication.configureCulture();
            string[] employees = id.Split(',');
            
            SMonthlyEmployeeSummaryReport reportService = new SMonthlyEmployeeSummaryReport();
            Riddhasoft.Services.Common.ServiceResult<List<MonthlyEmployeeSummaryReport>> result;
            result = reportService.Get(ConversionExtension.ToDateTime(startDate), ConversionExtension.ToDateTime(endDate));
            var reportData = (from c in result.Data
                              join d in employees
                              on c.EmployeeId equals int.Parse(d)
                              select c
                                 ).ToList();
            Session["ReportPath"] = ReportPath;
            Session["ReportData"] = reportData.ToDataTable<MonthlyEmployeeSummaryReport>();
            Session["ReportTitle"] = "Monthly Employee Summary Report";
            Session["CompanyName"] = "Riddhasoft";
            return RedirectToAction("Index", "ReportViewer", new { area = "Report" });
        }
    }
}