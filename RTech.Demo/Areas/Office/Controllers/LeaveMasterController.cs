using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("3001")]
    public class LeaveMasterController : Controller
    {
        //
        // GET: /Office/LeaveMaster/
        public ActionResult Index()
        {
            return View();
        }
	}
}