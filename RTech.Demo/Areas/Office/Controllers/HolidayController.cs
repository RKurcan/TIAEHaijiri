using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("2004")]
    public class HolidayController : Controller
    {
        //
        // GET: /Office/Holiday/
        public ActionResult Index()
        {
            return View();
        }
	}
}