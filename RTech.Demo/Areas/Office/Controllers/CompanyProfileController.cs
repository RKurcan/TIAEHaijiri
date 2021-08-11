using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("1003")]
    public class CompanyProfileController : Controller
    {
        //
        // GET: /Office/CompanyProfile/
        public ActionResult Index()
        {
            return View();
        }
	}
}