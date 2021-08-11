using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.User.Controllers
{
    public class ControllerPermissionController : Controller
    {
        //
        // GET: /User/ControllerPermission/
        public ActionResult Index()
        {
            return View();
        }
	}

    //public class ControllerPermissionViewModel
    //{
    //    public int RoleId { get; set; }
    //    public string RoleName { get; set; }
    //    public int ModuleId { get; set; }
    //    public string ModuleName { get; set; }

    //}
    //public class ControllerViewModel
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public bool Checked { get; set; }
    //}
}