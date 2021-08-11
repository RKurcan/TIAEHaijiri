using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("2005")]
    public class ManualPunchController : Controller
    {
        //
        // GET: /Employee/ManualPunch/
        public ActionResult Index()
        {
            return View();
        }
	}
}