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
    public class PayRollReportController : Controller
    {
        //
        // GET: /Report/PayRollReport/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateReport(string id, string onDate, string month)
        {
            WebApiApplication.configureCulture();

            string[] employees = id.Split(',');
            string ReportPath = @"\Areas\Report\RDLS\PayrollCalculationrpt.rdlc";
            SDailyEarlyInReport reportService = new SDailyEarlyInReport();
            Riddhasoft.Services.Common.ServiceResult<List<AttendanceReportDetailViewModel>> result;
            result = reportService.Get(System.DateTime.Now);

            var reportData = (from c in result.Data
                              join d in employees
                              on c.EmployeeId equals int.Parse(d)
                              select c
                                 ).ToList();
            Session["ReportPath"] = ReportPath;
            Session["ReportData"] = reportData.ToDataTable<AttendanceReportDetailViewModel>();
            Session["ReportTitle"] = "Monthly Employee Salary Slip";
            Session["CompanyName"] = "Riddhasoft";
            return RedirectToAction("Index", "ReportViewer", new { area = "Report" });
        }

        

    }
    
}