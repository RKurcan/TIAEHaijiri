using Riddhasoft.Entity.User;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RTech.Demo.Filters
{
    public class RiddhaMVCAuthorizeAttribute : AuthorizeAttribute
    {
        private ApplicationRole[] _roles;

        public RiddhaMVCAuthorizeAttribute(params ApplicationRole[] roles)
        {
            {
                ApplicationRole[] defaultRole = { ApplicationRole.User };
                if (_roles == null)
                {
                    _roles = defaultRole;
                }
                else
                    this._roles = roles;
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            bool authorize = false;
            string controllerName = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
            string actionName = httpContext.Request.RequestContext.RouteData.GetRequiredString("action");
            string areaName = (httpContext.Request.RequestContext.RouteData.DataTokens["area"] ?? "").ToString();
            if (controllerName.ToLower() == "user" && (actionName.ToLower() == "login" || actionName.ToLower() == "logout" || actionName.ToLower() == "changepassword" || actionName.ToLower() == "lockscreen" || actionName.ToLower() == "reseller" || actionName.ToLower() == "owner" || actionName.ToLower() == "captcha" || actionName.ToLower() == "resellerlogin" ||actionName.ToLower()== "becomeapartner"))
            {
                return true;
            }
            if (controllerName.ToLower() == "activation" && (actionName.ToLower() == "partner" || actionName.ToLower() == "resetpassword" || actionName.ToLower() == "resetuserpassword"))
            {
                return true;
            }
            if (controllerName.ToLower() == "forgetpassword" && (actionName.ToLower() == "index"))
            {
                return true;
            }
            if (controllerName.ToLower() == "home" && actionName.ToLower() == "nopermission" || (controllerName.ToLower() == "iclock") || controllerName.ToLower() == "fileupload")
            {
                return true;
            }
            if (controllerName.ToLower() == "emailactivities" ||controllerName.ToLower()== "emailtemplate")
            {
                return true;
            }
            if (controllerName.ToLower() == "error" )
            {
                return true;
            }
            //if ((authorize = _roles.Contains(RiddhaSession.ApplicationRole)) == false)
            //    return true;
            EUser user = RiddhaSession.CurrentUser;
            if (user == null)
            {
                return false;
            }
            switch (user.UserType)
            {
                case UserType.User:
                    authorize = true;//userAuth(httpContext, controllerName, areaName);
                    break;
                case UserType.Reseller:
                    authorize = resellerAuth(httpContext, controllerName, actionName);
                    break;
                case UserType.Admin:
                    authorize = true;
                    break;
                case UserType.Owner:
                    authorize = ownerAuth(httpContext, controllerName);
                    break;
                default:
                    break;
            }
            //var user = context.AppUser.Where(m => m.UserID == GetUser.CurrentUser/* getting user form current context */ && m.Role == role &&  
            //m.IsActive == true); // checking active users with allowed roles.  
            //if (user.Count() > 0)  
            //{  
            //   authorize = true; /* return true if Entity has current user(active) with specific role */  
            //}  
            SetCulture();
            return authorize;
        }

        private bool ownerAuth(HttpContextBase httpContext, string controllerName)
        {
            if (controllerName.ToLower() == "home")
            {
                return true;
            }
            EUserRole role = RiddhaSession.CurrentUser.Role;
            var data = OwnerPermissionData.GetOwnerPermissionList().Where(x => x.Controller.ToLower() == controllerName.ToLower()).FirstOrDefault();
            if (data == null)
            {
                return false;
            }
            int controllerId = data.Id;
            List<EOwnerPermission> permittedData = new SRoleOnModule().ListOwnerPermission().Data.Where(x => x.RoleId == role.Id && x.ControllerId == controllerId).ToList();
            return permittedData.Count() > 0;
        }
        private void SetCulture()
        {
            string language = CultureHelper.GetImplementedCulture(RiddhaSession.Language);
            CultureInfo ci = CultureInfo.GetCultureInfo(language);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
        private bool resellerAuth(HttpContextBase httpContext, string controllerName, string actionName)
        {
            List<ResellerActionPermission> userActionPerLst = GetResellerPermissionList();
            return userActionPerLst.Where(x => x.Controller.ToUpper() == controllerName.ToUpper() && x.Action.ToUpper() == actionName.ToUpper()).Any();
        }
        private List<ResellerActionPermission> GetResellerPermissionList()
        {
            return new List<ResellerActionPermission>(){
                new ResellerActionPermission(){Id=1,Module="Home",Controller="Home",Action="Index"},
                new ResellerActionPermission(){Id=1,Module="Office",Controller="Company",Action="Index"},
                new ResellerActionPermission(){Id=1,Module="Device",Controller="CompanyDeviceAssignment",Action="Index"},
                new ResellerActionPermission(){Id=1,Module="Office",Controller="DesktopProductKey",Action="Index"},
                new ResellerActionPermission(){Id=1,Module="Office",Controller="ClientSearch",Action="Index"},
                new ResellerActionPermission(){Id=1,Module="Report",Controller="ResellerReport",Action="Index"},
            };
        }
        private bool userAuth(HttpContextBase httpContext, string controllerName, string areaName)
        {
            SUserRole userRoleServices = new SUserRole();
            int roleId = RiddhaSession.RoleId;
            if (roleId == 0)
            {
                return true;
            }
            var menuRights = userRoleServices.ListMenuRights().Data;
            string url = "/" + areaName.ToLower() + "/" + controllerName.ToLower();
            var requestedMenu = new SUserRole().ListMenus().Data.Where(x => x.MenuUrl.ToLower() == url).FirstOrDefault();
            return menuRights.Where(x => x.MenuId == requestedMenu.Id && x.RoleId == roleId).Any();
        }

        private List<string> GetControllerList()
        {
            return new List<string>() { "device", "reseller", "company", "companydeviceassignment", "deviceassignment", "model" };
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var currentUser = RiddhaSession.CurrentUser;
            if (currentUser != null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "NoPermission", controller = "home", area = "" }));
            }
            else
            {

                //request uri is not login 
                if ((filterContext.HttpContext.Request.Url.LocalPath == "/User/User/Login" || filterContext.HttpContext.Request.Url.LocalPath == "/") || RiddhaSession.UserId == 0)
                //then lock screen
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "login", controller = "user", area = "User" }));


                }
                //else
                //lock screen
                else
                {
                    RiddhaSession.RequestUrl = filterContext.HttpContext.Request.Url.LocalPath;
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "lockscreen", controller = "user", area = "User" }));
                }

            }
        }
    }
    public enum ApplicationRole
    {
        User,
        Reseller,
        Owener
    }
    public class ResellerActionPermission
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }

}