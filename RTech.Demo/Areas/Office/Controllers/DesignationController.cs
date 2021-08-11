using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    [MenuFilter("1007")]
    public class DesignationController : Controller
    {
        //
        // GET: /Office/Designation/
        public ActionResult Index()
        {
            
            return View();
        }
        
	}
}