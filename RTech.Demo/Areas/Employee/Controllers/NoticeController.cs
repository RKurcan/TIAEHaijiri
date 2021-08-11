using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("2006")]
    public class NoticeController : Controller
    {
        //
        // GET: /Employee/Notice/
        public ActionResult Index()
        {
            return View();
        }
    }
}