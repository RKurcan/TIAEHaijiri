using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("2003")]
    public class WeeklyRosterController : Controller
    {
        //
        // GET: /Employee/WeeklyRoster/
        public ActionResult Index()
        {
            return View();
        }
	}
}