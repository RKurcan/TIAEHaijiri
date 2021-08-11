using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("3003")]
    public class LeaveApprovalController : Controller
    {
        //
        // GET: /Employee/LeaveApproval/
        public ActionResult Index()
        {
            return View();
        }
	}
}