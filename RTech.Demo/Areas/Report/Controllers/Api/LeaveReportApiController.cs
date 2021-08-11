using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class LeaveReportApiController : ApiController
    {
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            SLeaveReport reportService = new SLeaveReport(RiddhaSession.Language);
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            var result = reportService.GetLeaveReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt(), RiddhaSession.FYId).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            List<EEmployeeLeaveSummary> reportData = new List<EEmployeeLeaveSummary>();
            if (employees.Count() > 0)
            {
                reportData = (from c in result
                              join d in employees
                              on c.EmployeeId equals d
                              select c
                                  ).ToList();
            }
            else
            {
                reportData = result;
            }
            return new KendoGridResult<object>()
            {
                Data = reportData.OrderBy(x => x.Code).Skip(vm.Skip).Take(vm.Take),
                //Data = reportData.OrderBy(x => x.Code).GroupBy(x => x.LeaveId).Skip(vm.Skip).Take(vm.Take),
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = reportData.Count
            };
        }

        [HttpPost]
        public KendoGridResult<List<LeaveMasterWiseLeaveVm>> GenerateLeaveMasterWiseLeaveReport(KendoReportViewModel vm)
        {
            int branchId = (int)RiddhaSession.BranchId;
            SLeaveApplication leaveApplicationServices = new SLeaveApplication();
            List<LeaveMasterWiseLeaveVm> reportData = new List<LeaveMasterWiseLeaveVm>();
            string[] leaveMasterIds = vm.LeaveMasterIds.Split(',');
            var leaveApplications = leaveApplicationServices.List().Data.Where(x => x.BranchId == branchId);
            reportData = (from c in leaveApplications.ToList()
                          join d in leaveMasterIds on c.LeaveMasterId equals int.Parse(d)
                          select new LeaveMasterWiseLeaveVm()
                          {
                              Department = c.Employee.Section == null ? "" : c.Employee.Section.Department.Name,
                              Section = c.Employee == null ? "" : c.Employee.Section.Name,
                              Description = c.Description,
                              Employee = c.Employee.Code + "-" + c.Employee.Name,
                              FromDate = c.From.ToString("yyyy/MM/dd"),
                              ToDate = c.To.ToString("yyyy/MM/dd"),
                              LeaveName = c.LeaveMaster.Name,
                              LeaveDays = (c.To - c.From).Days + 1,
                          }).ToList();
            return new KendoGridResult<List<LeaveMasterWiseLeaveVm>>()
            {
                Data = reportData.OrderBy(x => x.Employee).Skip(vm.Skip).Take(vm.Take).ToList(),
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = reportData.Count
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetFiscalYear()
        {
            SFiscalYear _fiscalYearServices = new SFiscalYear();
            var result = (from c in _fiscalYearServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                          select new DropdownViewModel()
                          {
                              Id = c.Id,
                              Name = c.FiscalYear
                          }).ToList();

            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

    public class LeaveMasterWiseLeaveVm
    {
        public string Department { get; set; }
        public string Section { get; set; }
        public string Employee { get; set; }
        public string LeaveName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int LeaveDays { get; set; }
        public string Description { get; set; }
    }
}
