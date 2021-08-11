using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
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
    public class EmployeeAllowanceReportApiController : ApiController
    {
        SDateTable dateTableServices = null;
        SMonthlyWiseReport reportServices = null;
        public EmployeeAllowanceReportApiController()
        {
            dateTableServices = new SDateTable();
            reportServices = new SMonthlyWiseReport();
        }
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId = RiddhaSession.BranchId ?? 0;
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            reportServices.FilteredEmployeeIDs = employees;
            var result = reportServices.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt()).Data;
            result = result.Where(x => x.Remark != "Weekend" && x.Remark != "Holiday").ToList();

            SAllowance allowanceServices = new SAllowance();
            List<EEmployeeAlowance> allowances = allowanceServices.ListEmpAllowance().Data.Where(x => x.BranchId == branchId && x.IsApproved).ToList();

            //var dates = new SDateTable().GetDaysInEnglishMonth(20+17, 01);

            var reportData = (from c in allowances 
                              join d in employees
                                on c.EmployeeId equals d
                              join e in result
                                on c.EmployeeId equals e.EmployeeId
                          //join d in dates on c.FromDate equals d.EngDate into joined
                          //from j in joined.DefaultIfEmpty(new EDateTable())
                         select new EmployeeAllowanceGridVm()
                         {
                            Id = c.Id,
                            EngDate=e.WorkDate,
                            EmployeeCode = c.Employee.Code,
                            EmployeeName = c.Employee.Name,
                            Allowance = c.Allowance.Name,
                            AllowanceValue = getAllowance(e.Actual,c.Allowance.MinimumWorkingHour,e.WorkDate,c.FromDate,c.ToDate,c.Allowance.Value,e.Remark),
                            Designation = c.Employee.Designation.Name,
                            Department = c.Employee.Section.Department.Name,
                            Section = c.Employee.Section.Name
                         }).ToList();

            //foreach (var item in allowances.Select(x=>x.Employee).Distinct())
            //{
                
            //}
            return new KendoGridResult<object>()
            {
                Data = reportData,
                Status = ResultStatus.Ok
            };

        }

        private decimal getAllowance(string actual,TimeSpan min,string ActualDate,DateTime FromDate,DateTime ToDate,decimal value,string Remark)
        {
            DateTime workDate = DateTime.Parse(ActualDate);
            TimeSpan actualTime = TimeSpan.Parse(actual);
            if (workDate >= FromDate && workDate <= ToDate)
            {
                if (Remark == "Absent" || Remark == "Leave")
                {
                    return 0;
                }
                else if (actualTime > min)
                {
                    return value;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
           
        }
        private string getDate(int p)
        {
            throw new NotImplementedException();
        }

        public class EmployeeAllowanceGridVm
        {
            public int Id { get; set; }
            public string EmployeeName { get; set; }
            public string Allowance { get; set; }
            public decimal AllowanceValue { get; set; }
            public string EmployeeCode { get; set; }
            public string Designation { get; set; }
            public string Department { get; set; }
            public string Section { get; set; }

            public string EngDate { get; set; }
        }
    }
}
