using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.PayRoll.Controllers
{
    public class SalarySheetController : Controller
    {
        // GET: PayRoll/SalarySheet
        public ActionResult Index()
        {
            return View();
        }
    }
}