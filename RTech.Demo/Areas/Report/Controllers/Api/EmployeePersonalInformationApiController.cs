using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
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
    public class EmployeePersonalInformationApiController : ApiController
    {
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId = RiddhaSession.BranchId ?? 0;

            SEmployee employeeService = new SEmployee();
            List<EEmployee> employees = new List<EEmployee>();
            employees = employeeService.List().Data.Where(x => x.BranchId == branchId).ToList();

            int[] employeeIds = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            List<EmployeeDetailsGridViewModel> result = new List<EmployeeDetailsGridViewModel>();
            result = (from e in employees
                      join d in employeeIds
                        on e.Id equals d
                      select new EmployeeDetailsGridViewModel()
                      {
                          EmployeeName = e.Code + " - " + e.Name,
                          Designation = e.Designation == null ? "" : e.Designation.Code + " - " + e.Designation.Name,
                          Department = e.Section.Department == null ? "" : e.Section.Department.Code + " - " + e.Section.Department.Name,
                          Section = e.Section == null ? "" : e.Section.Code + " - " + e.Section.Name,
                          GradeGroup = e.GradeGroup == null ? "" : e.GradeGroup.Name,
                          EmployeeDeviceCode = e.DeviceCode,
                          JoinDate = e.DateOfJoin.HasValue ? e.DateOfJoin.Value.ToString("yyyy/MM/dd") : "",
                          PhoneNo = e.Mobile,
                          Address = e.PermanentAddress,
                      }).ToList();
            return new KendoGridResult<object>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = result.Count()
            };
        }
    }

    public class EmployeeDetailsGridViewModel
    {
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string GradeGroup { get; set; }
        public int EmployeeDeviceCode { get; set; }
        public string JoinDate { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
    }
}
