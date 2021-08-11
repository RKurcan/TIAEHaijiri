using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Mobile.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
namespace RTech.Demo.Controllers.MobileApi
{
     [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MHomeApiController : MRootController
    {
       
        public MobileResult<List<EMNotification>> GetNotifications(int empId)
        {

            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {


                //List<RTech.Demo.Models.MHomeViewModel.MNotificationViewModel> resultData = (from c in notificationServices.List().Data.ToList()
                //                                                                            select new RTech.Demo.Models.MHomeViewModel.MNotificationViewModel()
                //                                                                            {
                //                                                                                Title = c.Title,
                //                                                                                Message = c.Message,
                //                                                                                Type = c.NotificationType,
                //                                                                                PublishTime = getNotificationPublishTime(c.PublishDate)
                //                                                                            }).ToList();
                SMNotification notificatinService = new SMNotification();

                List<EMNotification> notificationLst = notificatinService.GetNotificationByEmployee(empId);
              
                return new MobileResult<List<EMNotification>>()
                {
                    Data = notificationLst,
                    Status = MobileResultStatus.Ok
                };
            }
            return InvalidTokeResult<List<EMNotification>>();
        }
        [HttpGet]
        public MobileResult<List<EMUpcomming>> GetUpcommings(int empId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SMNotification notificatinService = new SMNotification();
                List<EMUpcomming> upcommingsLst = notificatinService.GetUpCommingsByEmployee(empId);
                return new MobileResult<List<EMUpcomming>>()
                {
                    Data = upcommingsLst,
                    Status = MobileResultStatus.Ok
                };
            }
            return InvalidTokeResult<List<EMUpcomming>>();
        }

        [HttpGet]
        public MobileResult<EMHomeAttendanceInfo> GetHomeAttendanceInfo(int empId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SMHome homeService=new SMHome();
                EMHomeAttendanceInfo homeAttendanceInfo = homeService.GetHomeAttendanceInfo(empId);
                return new MobileResult<EMHomeAttendanceInfo>()
                {
                    Data = homeAttendanceInfo,
                    Status = MobileResultStatus.Ok
                };
            }
            return InvalidTokeResult<EMHomeAttendanceInfo>();
        }
        [HttpGet]
        public MobileResult<EMonthlyAttendanceSummary> GetMonthlyAttendanceSummary(int empId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SMHome homeService = new SMHome();
                EMonthlyAttendanceSummary homeattendanceSummary = homeService.GetAttendanceSummary(empId);
                
                return new MobileResult<EMonthlyAttendanceSummary>()
                {
                    Data = homeattendanceSummary,
                    Status = MobileResultStatus.Ok
                };
            }
            return InvalidTokeResult<EMonthlyAttendanceSummary>();
        }
       
        private string getNotificationPublishTime(DateTime dateTime)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 60)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 120)
            {
                return "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            return dateTime.ToString("yyyy/MM/dd");
            //if (delta < 2592000) // 30 * 24 * 60 * 60
            //{
            //    return ts.Days + " days ago";
            //}
            //if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            //{
            //    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            //    return months <= 1 ? "one month ago" : months + " months ago";
            //}
            //int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            //return years <= 1 ? "one year ago" : years + " years ago";
        }

    }
}
