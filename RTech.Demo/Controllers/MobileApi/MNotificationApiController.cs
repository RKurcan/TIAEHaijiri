using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;

namespace RTech.Demo.Controllers.MobileApi
{
    public class MNotificationApiController : MRootController
    {
        [HttpGet]
        public MobileResult<List<EMNotification>> GetNotificationByEmployee(int empId, int skipIndex = 0)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    int CurrentFiscalYearId = new SFiscalYear().List().Data.Where(x => x.BranchId == emp.BranchId && x.CurrentFiscalYear).FirstOrDefault().Id;
                    int takeIndex = WebConfigurationManager.AppSettings["NotificationTakeIndex"].ToInt();
                    SNotification notificatinService = new SNotification();
                    List<EMNotification> list = new List<EMNotification>();

                    var selfDetail = (from c in notificatinService.ListDetails().Data.Where(x => x.TargetId == empId && x.Notification.FiscalYearId == CurrentFiscalYearId && x.Notification.NotificationLevel== NotificationLevel.Employee).ToList()
                                      select new EMNotification()
                                      {
                                          Message = c.Notification.Message,
                                          Date = c.Notification.PublishDate.ToString("yyyy/MM/dd"),
                                          Title = c.Notification.Title,
                                          Type = Enum.GetName(typeof(NotificationType), c.Notification.NotificationType),
                                          DateAndTime = c.Notification.PublishDate,
                                      }).ToList();
                    list.AddRange(selfDetail);

                    var allNotification = (from c in notificatinService.List().Data.Where(x => x.FiscalYearId == CurrentFiscalYearId && x.NotificationLevel == NotificationLevel.All).ToList()
                                           select new EMNotification()
                                           {
                                               Message = c.Message,
                                               Date = c.PublishDate.ToString("yyyy/MM/dd"),
                                               Title = c.Title,
                                               Type = Enum.GetName(typeof(NotificationType), c.NotificationType),
                                               DateAndTime = c.PublishDate,
                                           }).ToList();
                    list.AddRange(allNotification);
                    return new MobileResult<List<EMNotification>>()
                    {
                        Data = list.OrderByDescending(x => x.Date).ToList(),
                        Message = "",
                        Status = MobileResultStatus.Ok
                    };
                }
                else
                {
                    return new MobileResult<List<EMNotification>>()
                    {
                        Data = null,
                        Message = "Process error.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return new MobileResult<List<EMNotification>>()
            {
                Data = null,
                Message = "Invalid Token",
                Status = MobileResultStatus.InvalidToken,
            };
        }

    }
}
