using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class MonthlyOfficeVisitReportApiController : ApiController
    {
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int BranchId = RiddhaSession.BranchId ?? 0;
            string language = RiddhaSession.Language;
            DateTime OnDate = DateTime.Parse(vm.OnDate).Date;
            DateTime ToDate = DateTime.Parse(vm.ToDate).Date;
            SOfficeVisit officeVisitServices = new SOfficeVisit();
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            var officeVisit = officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisit.BranchId == BranchId && DbFunctions.TruncateTime(x.OfficeVisit.From) >= OnDate && DbFunctions.TruncateTime(x.OfficeVisit.To) <= ToDate).ToList();
            var result = (from c in officeVisit
                          join d in employees
                             on c.EmployeeId equals d
                          select new OfficeVisitReportVM()
                          {
                              EmployeeCode = c.Employee.Code,
                              EmployeeName = language == "ne" && c.Employee.NameNp != null ? c.Employee.NameNp : c.Employee.Name,
                              From = c.OfficeVisit.From.ToString("yyyy/MM/dd"),
                              To = c.OfficeVisit.To.ToString("yyyy/MM/dd"),
                              Remark = c.OfficeVisit.Remark
                          }).ToList();
            return new KendoGridResult<object>()
            {
                Data = result,
                TotalCount = result.Count
            };
        }

        [HttpPost]
        public KendoGridResult<object> GenerateDailyReport(KendoReportViewModel vm)
        {
            int BranchId = RiddhaSession.BranchId ?? 0;
            string language = RiddhaSession.Language;
            DateTime OnDate = DateTime.Parse(vm.OnDate).Date;
            SOfficeVisit officeVisitServices = new SOfficeVisit();
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            var officeVisit = officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisit.BranchId == BranchId && DbFunctions.TruncateTime(x.OfficeVisit.From) >= OnDate).ToList();
            var result = (from c in officeVisit
                          join d in employees
                             on c.EmployeeId equals d
                          select new OfficeVisitReportVM()
                          {
                              EmployeeCode = c.Employee.Code,
                              EmployeeName = language == "ne" && c.Employee.NameNp != null ? c.Employee.NameNp : c.Employee.Name,
                              From = c.OfficeVisit.From.ToString("yyyy/MM/dd"),
                              To = c.OfficeVisit.To.ToString("yyyy/MM/dd"),
                              Remark = c.OfficeVisit.Remark
                          }).ToList();
            return new KendoGridResult<object>()
            {
                Data = result,
                TotalCount = result.Count
            };
        }
    }
    public class OfficeVisitReportVM
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Remark { get; set; }
    }
}
