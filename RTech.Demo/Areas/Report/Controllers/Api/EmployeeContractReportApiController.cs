using Riddhasoft.HRM.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Report;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class EmployeeContractReportApiController : ApiController
    {
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId=RiddhaSession.BranchId??0;
            SContract contractService = new SContract();
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            List<EUser>  users = new List<EUser>();
            SUser userServices = new SUser();
            users = userServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            var contractQuery=contractService.List().Data.Where(x => x.BranchId == branchId && x.IsApproved == true);
            List<HrContractReportGridViewModel> result = new List<HrContractReportGridViewModel>();
            result = (from c in contractQuery.ToList()
                      join d in employees
                             on c.EmployeeId equals d //into joined
                      join e in users 
                             on c.ApprovedById equals e.Id 
                      select new HrContractReportGridViewModel()
                      {
                          BeganOn = c.BeganOn.ToString("yyyy/MM/dd"),
                          ContractCode = c.Code,
                          EmployeeName = c.Employee.Code + " - " + c.Employee.Name,
                          EmploymentStatus = c.EmploymentStatus.Name,
                          EndedOn = c.EndedOn.ToString("yyyy/MM/dd"),
                          ApprovedBy = e.FullName??e.Name
                      }).ToList();
            return new KendoGridResult<object>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = contractQuery.Count()
            };
        }

    }

    public class HrContractReportGridViewModel
    {
        public string EmployeeName { get; set; }
        public string ContractCode { get; set; }
        public string BeganOn { get; set; }
        public string EndedOn { get; set; }
        public string EmploymentStatus { get; set; }
        public string ApprovedBy { get; set; }
    }
}