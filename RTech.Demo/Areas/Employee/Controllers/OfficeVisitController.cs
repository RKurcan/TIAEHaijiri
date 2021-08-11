using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("2008")]
    public class OfficeVisitController : Controller
    {
        //
        // GET: /Employee/OfficeVisite/
        public ActionResult Index()
        {
            return View();
        }
	}
}