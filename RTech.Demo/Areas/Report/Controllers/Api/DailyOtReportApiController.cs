
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
    public class DailyOtReportApiController : ApiController
    {
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            List<AttendanceReportDetailViewModel> reportData = new List<AttendanceReportDetailViewModel>();
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(RiddhaSession.BranchId.ToInt(), RiddhaSession.FYId, RiddhaSession.Language);
            List<AttendanceReportDetailViewModel> result = new List<AttendanceReportDetailViewModel>();
            result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime()).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            if (employees.Count() > 0)
            {
                result = (from c in result
                          join d in employees
                          on c.EmployeeId equals d
                          select c).ToList();
            }
            if (vm.Filter.Filters.Count() > 0)
            {
                switch (vm.Filter.Filters[0].Operator.ToLower())
                {
                    case "startswith":
                        reportData = (from c in result
                                      where (c.EmployeeName.ToLower().StartsWith(vm.Filter.Filters[0].Value.ToLower()) || c.EmployeeCode.ToLower().StartsWith(vm.Filter.Filters[0].Value.ToLower()))
                                      select c).Where(x => x.Ot != "00:00" && x.Ot != "").OrderBy(x => x.EmployeeName).Skip(vm.Skip).Take(vm.Take).ToList();
                        break;
                    case "eq":
                        reportData = (from c in result
                                      where (c.EmployeeName.ToLower() == (vm.Filter.Filters[0].Value.ToLower()) || c.EmployeeCode.ToLower() == (vm.Filter.Filters[0].Value.ToLower()))
                                      select c).Where(x => x.Ot != "00:00" && x.Ot != "").OrderBy(x => x.EmployeeName).Skip(vm.Skip).Take(vm.Take).ToList();
                        break;
                    default:
                        reportData = result;
                        break;
                }
            }
            else
            {
                reportData = (from c in result
                              select c).Where(x => x.Ot != "00:00" && x.Ot != "").OrderBy(x => x.EmployeeName).Skip(vm.Skip).Take(vm.Take).ToList();
            }
            if (vm.Sort.Count() > 0)
            {
                switch (vm.Sort[0].Field.ToLower())
                {
                    case "employeename":
                        if (vm.Sort[0].Dir.ToLower() == "asc")
                        {
                            reportData = reportData.Where(x => x.Ot != "00:00" && x.Ot != "").OrderBy(x => x.EmployeeName).Skip(vm.Skip).Take(vm.Take).ToList();
                        }
                        else
                        {
                            reportData = reportData.Where(x => x.Ot != "00:00" && x.Ot != "").OrderBy(x => x.EmployeeName).Skip(vm.Skip).Take(vm.Take).ToList();
                        }
                        break;
                    case "employeecode":
                        if (vm.Sort[0].Dir.ToLower() == "asc")
                        {
                            reportData = reportData.Where(x => x.Ot != "00:00" && x.Ot != "").OrderBy(x => x.EmployeeName).Skip(vm.Skip).Take(vm.Take).ToList();
                        }
                        else
                        {
                            reportData = reportData.Where(x => x.Ot != "00:00" && x.Ot != "").OrderBy(x => x.EmployeeName).Skip(vm.Skip).Take(vm.Take).ToList();
                        }
                        break;
                    default:
                        reportData = (from c in reportData
                                      select c).Where(x => x.Ot != "00:00" && x.Ot != "").OrderBy(x => x.EmployeeName).Skip(vm.Skip).Take(vm.Take).ToList();
                        break;
                }
            }
            return new KendoGridResult<object>()
            {
                Data = reportData,
                Status = ResultStatus.Ok,
                TotalCount = reportData.Count
            };
        }
    }
}
