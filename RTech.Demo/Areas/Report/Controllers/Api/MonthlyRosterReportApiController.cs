using Riddhasoft.Employee.Services;
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
    public class MonthlyRosterReportApiController : ApiController
    {
        private SDateTable dateTableServices = null;
        SMonthlyWiseReport reportServices = null;
        public MonthlyRosterReportApiController()
        {
            dateTableServices = new SDateTable();
            reportServices = new SMonthlyWiseReport(RiddhaSession.Language);
        }
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            string opDate = RiddhaSession.OperationDate;
            DateTime fromDate;
            DateTime toDate;
            if (opDate == "ne")
            {
                var dates = dateTableServices.GetDaysInNepaliMonth(vm.Year, vm.MonthId);
                fromDate = dates.First().EngDate;
                toDate = dates.Last().EngDate;
            }
            else
            {
                var firstDayInEnglishMonth = dateTableServices.GetFirstDayInEnglishMonth(vm.Year, vm.MonthId);
                var lastDayInEnglishMonth = dateTableServices.GetLastDayInEnglishMonth(vm.Year, vm.MonthId);
                fromDate = firstDayInEnglishMonth.EngDate;
                toDate = lastDayInEnglishMonth.EngDate;
            }
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            int totalRecords = employees.Count();
            employees = employees.Skip(vm.Skip).Take(vm.Take).ToArray();
            reportServices.FilteredEmployeeIDs = employees;
            var result = reportServices.GetMonthlyRosterReportFromSp(fromDate, toDate, RiddhaSession.BranchId.ToInt(), vm.EmpIds, vm.DeptIds, vm.SectionIds).Data.ToList();
            dynamic[] data = new dynamic[] { };
            if (result.Count() > 0)
            {
                var reportData = (from c in result
                                  join d in employees on c.EmployeeId equals d
                                  select new
                                  {
                                      EmployeeName = c.EmployeeName,
                                      SectionName = c.SectionName,
                                      DepartmentName = c.DepartmentName,
                                      EngDate = getWorkDate(c),
                                      NepDate=c.NepDate,
                                      ShiftCode=c.ShiftCode,
                                      ShiftName=c.ShiftName
                                  }
                                   ).AsEnumerable();
                data = reportData.ToPivotArray(x => x.EngDate, x => x.EmployeeName, x => x.First().ShiftCode);
            }
            else
            {
                var reportData = (from c in result
                                  select new
                                  {
                                      EmployeeName = c.EmployeeName,
                                      SectionName = c.SectionName,
                                      DepartmentName = c.DepartmentName,
                                      EngDate = getWorkDate(c),
                                      NepDate = c.NepDate,
                                      ShiftCode = c.ShiftCode,
                                      ShiftName = c.ShiftName
                                  }
                                   ).AsEnumerable();
                data = reportData.ToPivotArray(x => x.EngDate, x => x.EmployeeName, x => x.First().ShiftCode);
            }

            return new KendoGridResult<object>()
            {
                Data = data,
                Status = ResultStatus.Ok,
                TotalCount = totalRecords
            };
        }
        private object getWorkDate(MonthlyRosterReport c)
        {
            switch (RiddhaSession.OperationDate)
            {
                case "en":
                    return "Day" + c.EngDate.Substring(8, 2);
                case "ne":
                    return "Day" + c.NepDate.Substring(8, 2);
                default:
                    return "Day" + c.EngDate.Substring(8, 2);
            }
        }
    }
}
