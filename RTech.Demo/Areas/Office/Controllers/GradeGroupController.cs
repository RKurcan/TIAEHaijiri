using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("1008")]
    public class GradeGroupController : Controller
    {
        //
        // GET: /Office/GradeGroup/
        public ActionResult Index()
        {
            return View();
        }
	}
}