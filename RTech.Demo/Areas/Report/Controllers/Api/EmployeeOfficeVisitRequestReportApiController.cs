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
    public class EmployeeOfficeVisitRequestReportApiController : ApiController
    {
        public KendoGridResult<List<EmployeeOfficeVisitRequestReportVm>> GenerateReport(KendoReportViewModel vm)
        {
            SEmployeeOfficeVisitRequestReport services = new SEmployeeOfficeVisitRequestReport();
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
            var result = services.GenerateReport(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt()).Data;
            result = (from c in result
                      join d in employees
                      on c.EmployeeId equals d
                      select c
                      ).ToList();
            return new KendoGridResult<List<EmployeeOfficeVisitRequestReportVm>>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take).ToList(),
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = result.Count(),
            };
        }
    }
}
