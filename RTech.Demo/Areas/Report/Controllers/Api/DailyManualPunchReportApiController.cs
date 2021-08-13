using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class DailyManualPunchReportApiController : ApiController
    {
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int BranchId = RiddhaSession.BranchId??0;
            string language =RiddhaSession.Language;
            DateTime OnDate = DateTime.Parse(vm.OnDate).Date;
            SManualPunch manualPunchServices = new SManualPunch();
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            var manualPunches = manualPunchServices.List().Data.Where(x => x.BranchId == BranchId && DbFunctions.TruncateTime(x.DateTime) == OnDate).ToList();
            var result = (from c in manualPunches
                         join d in employees
                            on c.EmployeeId equals d
                         select new ManualPunchGridViewModel()
                         {
                             EmployeeCode = c.Employee.Code,
                             EmployeeName = language == "ne" && c.Employee.NameNp != null ? c.Employee.NameNp : c.Employee.Name,
                             Date = c.DateTime.ToString("yyyy/MM/dd"),
                             Time = c.DateTime.ToString(@"hh\:mm"),
                             Remarks  = c.Remark
                         }).ToList();
            return new KendoGridResult<object>()
            {
                Data = result.OrderBy(x=>x.EmployeeName),
                TotalCount = result.Count
            };
        }
    }

    public class ManualPunchGridViewModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Remarks { get; set; }
    }
}
