using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Areas.Report.Controllers.Api;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (RiddhaSession.CurrentToken != null)
            {

                EUser curuser = RTech.Demo.Utilities.RiddhaSession.CurrentUser ?? new Riddhasoft.User.Entity.EUser();
                if (curuser.UserType == UserType.Admin || curuser.UserType == UserType.Owner)
                {
                    return View("_OwnerHome");
                }
                else if (curuser.UserType == UserType.Reseller)
                {
                    return View("_PartnerHome");
                }
                return View();
            }
            else
            {
                return RedirectToAction("login", "User", new { area = "User" });
            }
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult DynamicColumn()
        {
            return View();
        }
        public ActionResult InitialPage()
        {
            return View("_landingpage");
        }
        public ActionResult NoPermission()
        {
            return View("_NoPermissionPage");
        }

    }
}
