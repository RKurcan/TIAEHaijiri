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
    public class EmployeeResignationReportApiController : ApiController
    {
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            SResignation resignServices = new SResignation();
            List<EUser> users = new List<EUser>();
            SUser userServices = new SUser();
            users = userServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            int[] employeeIds = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            var resignationQuery = resignServices.List().Data.Where(x => x.BranchId == branchId && x.IsApproved == true);
            List<ResignationGridViewModel>  result = new List<ResignationGridViewModel>();
            result = (from c in resignationQuery.ToList()
                      join d in employeeIds
                            on c.EmployeeId equals d
                      join e in users
                            on c.ApprovedById equals e.Id
                      select new ResignationGridViewModel()
                      {
                           EmployeeName = c.Employee.Code+" - "+c.Employee.Name,
                           ResignCode = c.Code,
                           NoticeDate = c.NoticeDate.ToString("yyyy/MM/dd"),
                           DesiredResignDate = c.DesiredResignDate.ToString("yyyy/MM/dd"),
                           Reason = c.Reason,
                           Details = c.Details,
                           ApprovedBy = e.FullName??e.Name
                      }).ToList();
            return new KendoGridResult<object>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = resignationQuery.Count()
            };
        }
	}

    public class ResignationGridViewModel
    {
        public string EmployeeName { get; set; }
        public string ResignCode { get; set; }
        public string NoticeDate { get; set; }
        public string DesiredResignDate { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
        public string ApprovedBy { get; set; }
    }
}