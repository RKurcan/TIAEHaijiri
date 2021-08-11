using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.Report.ReportViewModel;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Riddhasoft.Services.Common;
using Riddhasoft.Employee.Services;
using RTech.Demo.Areas.Report.Controllers.Api;

namespace RTech.Demo.Areas.Report.Controllers
{
    public class MonthlyWiseAttendanceRptController : Controller
    {
        //
        // GET: /Report/MonthlyWiseAttendanceRpt/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateReport(KendoReportViewModel vm)
        {
            int[] employees;
            if (vm.ActiveInactiveMode == 0)
            {
                employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            }
            else
            {
                var allEmployee = new SEmployee().List().Data.Where(x => x.BranchId == RiddhaSession.BranchId);
                string filteredEmp = string.Join(",", allEmployee.Select(x => x.Id));
                employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, filteredEmp).Data;
            }
            SMonthlyWiseReport reportService = new SMonthlyWiseReport();
            string ReportPath = "MonthlyWiseReport.rdlc";

            reportService.FilteredEmployeeIDs = employees;
            reportService.CurrentOperationDate = RiddhaSession.OperationDate;
            var reportData = reportService.GetAttendanceReportFromSpForRDLC(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt(), vm.OTV2,vm.BranchIds).Data;
            TempData["ReportPath"] = ReportPath;
            TempData["ReportData"] = reportData.ToDataTable<MonthlyWiseReport>();
            TempData["ReportTitle"] = "Monthly Wise Attendance Report";
            TempData["CompanyName"] = "Riddhasoft";
            switch (vm.Type)
            {
                case 0:
                    TempData["type"] = "PDF";
                    break;
                case 1:
                    TempData["type"] = "EXCEL";
                    break;
                case 2:
                    TempData["type"] = "WORD";
                    break;
                default:
                    TempData["type"] = "PDF";
                    break;
            }
            return RedirectToAction("Index", "ReportViewer", new { area = "Report" });
        }

        [HttpGet]
        public ActionResult GenerateMultiPunchReport(KendoReportViewModel vm)
        {
            int[] employees;
            if (vm.ActiveInactiveMode == 0)
            {
                employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            }
            else
            {
                var allEmployee = new SEmployee().List().Data.Where(x => x.BranchId == RiddhaSession.BranchId);
                string filteredEmp = string.Join(",", allEmployee.Select(x => x.Id));
                employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, filteredEmp).Data;
            }
            SMonthlyWiseReport reportService = new SMonthlyWiseReport();
            string ReportPath = "MultiPunchReport.rdlc";

            reportService.FilteredEmployeeIDs = employees;
            reportService.CurrentOperationDate = RiddhaSession.OperationDate;
            var reportData = reportService.GetMultiPunch(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt()).Data;
            TempData["ReportPath"] = ReportPath;
            TempData["ReportData"] = reportData.ToDataTable<MultipunchReportViewModel>();
            TempData["ReportTitle"] = "Monthly Wise Attendance Report";
            TempData["CompanyName"] = "Riddhasoft";
            switch (vm.Type)
            {
                case 0:
                    TempData["type"] = "PDF";
                    break;
                case 1:
                    TempData["type"] = "EXCEL";
                    break;
                case 2:
                    TempData["type"] = "WORD";
                    break;
                default:
                    TempData["type"] = "PDF";
                    break;
            }
            return RedirectToAction("Index", "ReportViewer", new { area = "Report" });
        }
    }
}