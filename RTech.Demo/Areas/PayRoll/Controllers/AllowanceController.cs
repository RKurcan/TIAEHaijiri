using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.PayRoll.Controllers
{
    public class AllowanceController : Controller
    {
        [MenuFilter("8001")]
        //
        // GET: /PayRoll/Allowance/
        public ActionResult Index()
        {
            return View();
        }
	}
}