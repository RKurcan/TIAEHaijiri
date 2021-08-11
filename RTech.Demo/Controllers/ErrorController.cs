using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string title,string message)
        {
            ViewBag.Title = title;
            ViewBag.Message = message;
            ViewBag.Stacktrace = TempData["Stacktrace"] as string;
            return View();
        }
    }
}