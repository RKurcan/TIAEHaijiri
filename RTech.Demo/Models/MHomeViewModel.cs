using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Models
{
    public class MHomeViewModel
    {
        public class MNotificationViewModel
        {
            public string Title { get; set; }
            public string Message { get; set; }
            public string PublishTime { get; set; }
            public NotificationType Type { get; set; }
        }
    }
}