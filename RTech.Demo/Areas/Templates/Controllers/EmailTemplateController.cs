using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Templates.Controllers
{
    public class EmailTemplateController : Controller
    {
        // GET: Templates/EmailTemplate
        public ActionResult Index(string msg)
        {
            ViewBag.MailBody=msg;
            return View();
        }
    }
}