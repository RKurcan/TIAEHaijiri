using RTech.Demo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Device.Controllers
{
    [MenuFilter("1002")]
    public class CompanyDeviceController : Controller
    {
        //
        // GET: /Device/CompanyDevice/
        public ActionResult Index()
        {
            return View();
        }
	}
}