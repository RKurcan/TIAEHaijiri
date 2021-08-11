using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riddhasoft.Globals.Conversion;
namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMRoster
    {
        Riddhasoft.DB.RiddhaDBContext db;
        public SMRoster()
        {
            db = new DB.RiddhaDBContext();
        }
        public List<EMRoster> getRoster(int EmpId)
        {
            SDateTable ds = new SDateTable();
            string currentNepDate=ds.ConvertToNepDate(System.DateTime.Now.Date);
            var dates = currentNepDate.Split('/');
            var daysInMonth= ds.GetDaysInNepaliMonth(dates[0].ToInt(),dates[1].ToInt());
            List<EMRoster> rosters= db.SP_GET_Roster_REPORT(daysInMonth.First().EngDate, daysInMonth.Last().EngDate, EmpId).ToList();
            return rosters;
        }
    }
}
