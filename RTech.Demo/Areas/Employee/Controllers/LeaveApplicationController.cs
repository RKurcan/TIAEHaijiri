using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("3002")]
    public class LeaveApplicationController : Controller
    {
        //
        // GET: /Employee/LeaveApplication/
        public ActionResult Index()
        {
            return View();
        }
	}
}