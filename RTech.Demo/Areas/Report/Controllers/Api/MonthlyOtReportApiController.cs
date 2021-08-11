using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class MonthlyOtReportApiController : ApiController
    {
        private SDateTable dateTableServices = null;
        SMonthlyWiseReport reportServices = null;
        public MonthlyOtReportApiController()
        {
            dateTableServices = new SDateTable();
            reportServices = new SMonthlyWiseReport(RiddhaSession.Language);
        }
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            SMonthlyWiseReport reportService = new SMonthlyWiseReport(RiddhaSession.Language);
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            var companyConfig = new SCompany().List().Data.Where(x => x.Id == RiddhaSession.CompanyId).FirstOrDefault();
            reportService.minimumOTHour = companyConfig.MinimumOTHour;
            reportService.FilteredEmployeeIDs = employees;
            var result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt(),vm.OTV2).Data;
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
                              where c.Ot != "00:00" && c.Ot != ""
                              select c
                                  ).ToList();
            }
            else
            {
                reportData = result;
            }

            return new KendoGridResult<object>()
            {
                Data = reportData.OrderBy(x => x.EmployeeName),//.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = reportData.Count
            };
        }

        [HttpPost]
        public KendoGridResult<object> GenerateMonthlyOtReport(KendoReportViewModel vm)
        {
            string opDate = RiddhaSession.OperationDate;
            string currentLanguage = RiddhaSession.Language;
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
            var result = reportServices.GetAttendanceReportFromSp(fromDate, toDate, RiddhaSession.BranchId.ToInt()).Data.ToList();
            dynamic[] data = new dynamic[] { };
            if (result.Count() > 0)
            {
                var reportData = (from c in result
                                  join d in employees on c.EmployeeId equals d
                                  select new
                                  {
                                      EmployeeName = c.EmployeeCode + "-" + c.EmployeeName,
                                      //Ot = c.Ot == "00:00" ? "-" : c.Ot, 
                                      Ot = (c.Ot == "00:00" && c.Ot == "") ? "-" : c.Ot,
                                      WorkDate = getWorkDate(c)
                                  }).AsEnumerable();
                data = reportData.ToPivotArray(x => x.WorkDate, x => x.EmployeeName, x => x.First().Ot);
            }
            else
            {
                var reportData = (from c in result
                                  select new
                                  {
                                      EmployeeName = c.EmployeeCode + "-" + c.EmployeeName,
                                      WorkDate = getWorkDate(c),
                                      //Ot = c.Ot == "00:00" ? "-" : c.Ot,
                                      Ot = (c.Ot == "00:00" && c.Ot == "") ? "-" : c.Ot,
                                  }
                               ).AsEnumerable();
                data = reportData.ToPivotArray(x => x.WorkDate, x => x.EmployeeName, x => x.First().Ot);
            }
            return new KendoGridResult<object>()
            {
                Data = data,
                Status = ResultStatus.Ok,
                TotalCount = totalRecords
            };
        }
        private object getWorkDate(MonthlyWiseReport c)
        {
            switch (RiddhaSession.OperationDate)
            {
                case "en":
                    return "Day" + c.WorkDate.Substring(8, 2);
                case "ne":
                    return "Day" + c.NepDate.Substring(8, 2);
                default:
                    return "Day" + c.WorkDate.Substring(8, 2);
            }
        }
    }
}
