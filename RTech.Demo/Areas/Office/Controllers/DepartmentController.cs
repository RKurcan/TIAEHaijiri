using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("1005")]
    public class DepartmentController : Controller
    {
        //
        // GET: /Office/Department/
        public ActionResult Index()
        {
            return View();
        }
	}
}