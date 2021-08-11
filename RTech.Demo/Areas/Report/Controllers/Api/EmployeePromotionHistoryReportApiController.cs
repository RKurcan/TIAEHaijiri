using Riddhasoft.DB;
using Riddhasoft.Employee.Services;
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
    public class EmployeePromotionHistoryReportApiController : ApiController
    {
        RiddhaDBContext db = null;
        SEmployeeDesignationHistory _employeeDesignationHistoryServices = null;
        public EmployeePromotionHistoryReportApiController()
        {
            db = new RiddhaDBContext();
            _employeeDesignationHistoryServices = new SEmployeeDesignationHistory();
        }
        public KendoGridResult<List<EmployeePromotionHistoryReportVm>> GenerateReport(PromotionHistReportArg arg)
        {
            string[] emp = arg.EmployeeIds.Split(',');
            var designation = new SDesignation().List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId).ToList();
            var result = (from c in _employeeDesignationHistoryServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                          join d in emp on c.EmployeeId equals int.Parse(d)
                          join e in designation on c.DesignationId equals e.Id into joinedT
                          from e in joinedT.DefaultIfEmpty()
                          select new EmployeePromotionHistoryReportVm()
                          {
                              AddedDate = c.DateTime.ToString("yyyy/MM/dd"),
                              Department = c.Employee.Section == null?"": c.Employee.Section.Department.Name,
                              DesignationName  = e ==null?"": e.Name,
                              EmployeeName = c.Employee.Name,
                              Section = c.Employee.Section ==null?"": c.Employee.Section.Name
                          }).ToList();
            return new KendoGridResult<List<EmployeePromotionHistoryReportVm>>()
            {
                Data  = result.Skip(arg.Skip).Take(arg.Take).ToList(),
                Message ="",
                Status =ResultStatus.Ok,
                TotalCount =result.Count()
            };
        }
    }

    public class PromotionHistReportArg : KendoPageListArguments
    {
        public string EmployeeIds { get; set; }
    }

    public class EmployeePromotionHistoryReportVm
    {
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string DesignationName { get; set; }
        public string AddedDate { get; set; }

    }
}
