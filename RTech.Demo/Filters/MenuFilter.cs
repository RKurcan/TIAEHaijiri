using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RTech.Demo.Filters
{
    public class MenuFilter : AuthorizeAttribute
    {
        private string menuCode;
        SUserRole userRoleServices = null;
        public MenuFilter(string menuCode)
        {
            this.menuCode = menuCode;
            this.userRoleServices = new SUserRole();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            int roleId=RiddhaSession.RoleId;
            if (roleId==0)
            {
                return true;
            }
            bool authorize = userRoleServices.ListMenuRights().Data.Where(x => x.RoleId == roleId && x.Menu.Code == menuCode).Any();
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "NoPermission", controller = "home", area = "" }));
        }
    }
}