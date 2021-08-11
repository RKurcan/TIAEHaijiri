using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("2007")]
    public class EventController : Controller
    {
        //
        // GET: /Employee/Event/
        public ActionResult Index()
        {
            return View();
        }
	}
}