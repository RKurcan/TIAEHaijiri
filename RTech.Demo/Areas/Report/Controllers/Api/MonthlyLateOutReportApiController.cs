using Riddhasoft.Employee.Entities;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class MonthlyLateOutReportApiController : ApiController
    {
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            SMonthlyWiseReport reportService = new SMonthlyWiseReport(RiddhaSession.Language);
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            reportService.FilteredEmployeeIDs = employees;
            var result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt()).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();
            if (employees.Count() > 0)
            {
                reportData = (from c in result
                              join d in employees
                              on c.EmployeeId equals d
                              where c.LateOut != "00:00" && c.LateOut != ""
                              select c
                                  ).ToList();
            }
            else
            {
                reportData = result;
            }

            return new KendoGridResult<object>()
            {
                Data = reportData.OrderBy(x => x.WorkDate),//.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = reportData.Count
            };
        }
    }
}
