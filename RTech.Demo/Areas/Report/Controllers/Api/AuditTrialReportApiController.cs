using Riddhasoft.Entity.User;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
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
    public class AuditTrialReportApiController : ApiController
    {
        SAuditTrial _auditTrialServices = null;
        SUser _userServices = null;
        SMenu _menuServices = null;
        public AuditTrialReportApiController()
        {
            _menuServices = new SMenu();
            _userServices = new SUser();
            _auditTrialServices = new SAuditTrial();
        }

        [HttpPost]
        public KendoGridResult<List<AuditTrialReportVm>> GenerateReport(AuditTrialRptArguments vm)
        {
            var list = _auditTrialServices.List().Data.Where(x => DbFunctions.TruncateTime(x.LogTime) >= DbFunctions.TruncateTime(vm.OnDate) && DbFunctions.TruncateTime(x.LogTime) <= DbFunctions.TruncateTime(vm.ToDate)).ToList();
            var userList = _userServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId);
            var menuList = _menuServices.MenuList().Data;
            var actionList = _menuServices.ActionList().Data;
            var result = (from c in list.ToList()
                          join u in userList on c.UserId equals u.Id
                          join m in menuList on c.MenuCode equals m.Code
                          join a in actionList on c.ActionCode equals a.ActionCode
                          select new AuditTrialReportVm()
                          {
                              Action = a.Desc,
                              Menu = m.Name,
                              Date = c.LogTime.ToString("yyyy/MM/dd"),
                              Message = c.Message,
                              UserName = u.FullName == null ? "Admin" : u.FullName,
                          }).ToList();
            return new KendoGridResult<List<AuditTrialReportVm>>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take).ToList(),
                Message = "",
                Status = ResultStatus.Ok
            };
        }


    }
    public class AuditTrialReportVm
    {
        public string Menu { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }
    }
    public class AuditTrialRptArguments : KendoPageListArguments
    {
        public DateTime OnDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
