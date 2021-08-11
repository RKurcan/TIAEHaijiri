using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Report.Controllers
{
    public class AttendanceReportController : Controller
    {
        //
        // GET: /Report/AttendanceReport/
        public ActionResult Index()
        {
            return PartialView();
        }
	}
}