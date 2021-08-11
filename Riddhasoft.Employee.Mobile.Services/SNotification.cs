using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Globals.Conversion;
using System.Collections.Generic;
using System.Linq;

namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMNotification
    {
        RiddhaDBContext db = new RiddhaDBContext();
        public List<EMNotification> GetNotificationByEmployee(int empId)
        {
            return (from c in db.SP_GET_NOTIFICATION_BY_EMP_ID(System.DateTime.Now, System.DateTime.Now, empId)
                    select new EMNotification()
                    {
                        Date = c.Date,
                        Message = c.Message,
                        Title = c.Title,
                        Type = ((NotificationType)c.Type.ToInt()).ToString()

                    }).ToList();
        }
        public List<EMUpcomming> GetUpCommingsByEmployee(int empId)
        {
            return (from c in db.SP_GET_NOTIFICATION_BY_EMP_ID(System.DateTime.Now, System.DateTime.Now.AddDays(30), empId)
                    select new EMUpcomming()
                    {
                        Date = c.Date,
                        Desc = c.Message,
                        Title = c.Title,
                        Type = ((NotificationType)c.Type.ToInt()).ToString(),
                        RemDays = c.RemDays
                    }
                        ).ToList();
        }

    }
}
