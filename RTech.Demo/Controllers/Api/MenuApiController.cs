using Riddhasoft.Entity.User;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RTech.Demo.Extension;

namespace RTech.Demo.Controllers.Api
{
    public class MenuApiController : ApiController
    {
        SUserRole userRoleServices = null;
        LocalizedString loc = null;
        public MenuApiController()
        {
            userRoleServices = new SUserRole();
            loc = new LocalizedString();
        }
        [HttpGet]
        public ServiceResult<List<MenuViewModel>> GetMenus()
        {
            int roleId = RiddhaSession.RoleId;

            List<MenuViewModel> vmLst = new List<MenuViewModel>();
            List<EMenu> menus = new List<EMenu>();
            var allMenus = userRoleServices.ListMenus().Data.Where(x => x.IsWidget == false).ToList().Where(x => (int)x.SoftwarePackageType <= RiddhaSession.PackageId).OrderBy(x=>x.Order).ToList();

            if (roleId < 1)
            {
                menus = allMenus;
            }
            else
            {
                var menuRights = userRoleServices.ListMenuRights().Data.Where(x => x.RoleId == roleId).ToList();
                menus = (from c in userRoleServices.ListMenus().Data.ToList()
                         join d in menuRights on c.Id equals d.MenuId
                         select c).ToList();
            }
            var parents = new List<EMenu>();
            if (roleId < 1)
                parents = menus.Where(x => x.ParentCode == String.Empty).ToList();
            else
            {
                parents = (from c in allMenus
                           join d in menus on c.Code equals d.ParentCode
                           select c
                             ).DistinctBy(x => x.Id).ToList();
            }

            foreach (var item in parents)
            {
                MenuViewModel vm = new MenuViewModel();
                vm.Code = item.Code;
                vm.Name = item.Name;
                vm.NameNp = item.NameNp;
                vm.MenuIconCss = item.MenuIconCss;
                vm.Menus = menus.Where(x => x.ParentCode == item.Code).ToList();
                vmLst.Add(vm);
            }
            if (RiddhaSession.Language == "ne")
            {
                setNepaliName(vmLst);
            }
            return new ServiceResult<List<MenuViewModel>>()
            {
                Data = vmLst.ToList(),
                Status = ResultStatus.Ok
            };
        }

        private void setNepaliName(List<MenuViewModel> vmLst)
        {
            foreach (var menu in vmLst)
            {
                menu.Name = menu.NameNp;
                if (menu.Menus.Count > 0)
                {
                    setMenuNepaliName(menu.Menus);
                }
            }
        }

        private void setMenuNepaliName(List<EMenu> list)
        {
            foreach (var menu in list)
            {
                menu.Name = menu.NameNp;

            }
        }
        [HttpGet]
        public ServiceResult<string[]> GetActions(string menuCode)
        {
            int roleId = RiddhaSession.RoleId;
            string[] permittedActions;
            var allActions = userRoleServices.ListMenuActions().Data.Where(x => x.MenuCode == menuCode).ToList();
            if (roleId < 1)
            {
                permittedActions = (from c in allActions
                                    select c.ActionCode).ToArray();
            }
            else
            {
                permittedActions = (from c in allActions
                                    join d in userRoleServices.ListActionRights().Data.Where(x => x.RoleId == roleId) on c.Id equals d.MenuActionId
                                    select c.ActionCode).ToArray();
            }
            return new ServiceResult<string[]>()
            {
                Data = permittedActions,
                Status = ResultStatus.Ok
            };
        }
    }
    public class MenuViewModel : EMenu
    {
        public List<EMenu> Menus { get; set; }
    }
}
