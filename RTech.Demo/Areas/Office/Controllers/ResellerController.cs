using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Office.Controllers
{
    public class ResellerController : Controller
    {
        //
        // GET: /Office/Reseller/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResellerLogin()
        {
            ViewBag.Message = "";
            return PartialView("_ResellerLogin");
        }

        public ActionResult Captcha()
        {
            CaptchaHelper captchaHelper = new CaptchaHelper();
            return File(captchaHelper.DrawByte(), "image/jpeg");
        }
	}
}