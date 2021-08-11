using Riddhasoft.Employee.Services;
using Riddhasoft.Report;
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
    public class EmployeeMultiPunchReportApiController : ApiController
    {


        [HttpPost]
        public ServiceResult<List<EmployeeMultiPunchReportVm>> GenerateReport(KendoReportViewModel vm)
        {
            SEmployeeMultiPunchReport multPunchReportServices = new SEmployeeMultiPunchReport();

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
            multPunchReportServices.FilteredEmployeeIDs = employees;
            multPunchReportServices.currentLanguage = RiddhaSession.Language;
            var result = multPunchReportServices.GetMultiPunchAttendanceReport(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt()).Data;
            return new ServiceResult<List<EmployeeMultiPunchReportVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }
}
