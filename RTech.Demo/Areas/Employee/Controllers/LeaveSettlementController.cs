using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("3004")]
    public class LeaveSettlementController : Controller
    {
        //
        // GET: /Employee/LeaveSettlement/
        public ActionResult Index()
        {
            return View();
        }
	}
}