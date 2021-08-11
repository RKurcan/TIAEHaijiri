using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("1006")]
    public class SectionController : Controller
    {
        //
        // GET: /Office/Section/
        public ActionResult Index()
        {
            return View();
        }
	}
}