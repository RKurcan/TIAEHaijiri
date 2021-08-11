using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("1001")]
    public class FiscalYearController : Controller
    {
        //
        // GET: /Office/FiscalYear/
        public ActionResult Index()
        {
            return View();
        }
	}
}