using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("1009")]
    public class ShiftController : Controller
    {
        //
        // GET: /Office/Shift/
        public ActionResult Index()
        {
            return View();
        }
	}
}