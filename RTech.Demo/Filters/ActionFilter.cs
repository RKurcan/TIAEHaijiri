using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace RTech.Demo.Filters
{
    public class ActionFilter : AuthorizeAttribute
    {
        private string[] _actionCodes;
        SUserRole userRoleServices = null;
        public ActionFilter(params string[] actionCodes)
        {
            this._actionCodes = actionCodes;
            this.userRoleServices = new SUserRole();
        }
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool isAuthroized = base.IsAuthorized(actionContext);
            int roleId=RiddhaSession.RoleId;
            if (roleId==0)
            {
                return true;
            }
            userRoleServices = new SUserRole();
            var  actionList = userRoleServices.ListActionRights().Data.Where(x => x.RoleId == roleId ).ToList();
            var authorize = (from c in actionList
                             join d in _actionCodes
                             on c.MenuAction.ActionCode equals d
                             select c
                                ).Any();
            return authorize;
        }
    }
}