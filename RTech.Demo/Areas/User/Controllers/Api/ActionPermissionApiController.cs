using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.User.Controllers.Api
{
    public class ActionPermissionApiController : ApiController
    {
        SUserRole roleServices = null;
        SRoleOnControllerAction actionServices = null;
        public ActionPermissionApiController()
        {
            roleServices = new SUserRole();
            actionServices = new SRoleOnControllerAction();
        }
        public ServiceResult<IQueryable<ERoleOnControllerAction>> Get()
        {
            return actionServices.List();
        }
        [HttpGet]
        public ServiceResult<List<ActionViewModel>> GetActionByController(int id, int roleId)
        {
            List<ActionViewModel> list = new List<ActionViewModel>();
            foreach (var item in ActionControllerData.GetData().Where(x => (int)x.ControllerId == id))
            {
                ActionViewModel vm = new ActionViewModel();
                vm.Id = (int)item.ActionId;
                vm.Name = Enum.GetName(typeof(AppActionName), item.ActionId);
                vm.Checked = IsMapped(roleId, item.ActionId);
                list.Add(vm);
            }
            return new ServiceResult<List<ActionViewModel>>()
            {
                Data = list,
                Status = ResultStatus.Ok
            };
        }
        private bool IsMapped(int roleId, AppActionName appActionName)
        {
            return actionServices.List().Data.Where(x => x.RoleId == roleId && x.Action == appActionName).Any();
        }
        public ServiceResult<ActionPermissionViewModel> Post(ActionPermissionViewModel viewModel)
        {
            List<ERoleOnControllerAction> actionsExistingList = new List<ERoleOnControllerAction>();
            actionsExistingList = actionServices.List().Data.Where(x => x.RoleId == viewModel.RoleId).ToList();
            if (actionsExistingList.Count()>0)
            {
                actionServices.RemoveRange(actionsExistingList);   
            }
            foreach (var item in viewModel.Actions.Where(x => x.Checked))
            {
                ERoleOnControllerAction model = new ERoleOnControllerAction();
                model.RoleId = viewModel.RoleId;
                model.Action = (AppActionName)item.Id;
                actionServices.Add(model);

            }
            return new ServiceResult<ActionPermissionViewModel>()
            {
                Data = viewModel,
                Message = "Succesfully Saved",
                Status = ResultStatus.Ok
            };
        }
    }

    public class ActionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }

    }

    public class ActionPermissionViewModel
    {
        public int RoleId { get; set; }
        public List<ActionViewModel> Actions { get; set; }

    }




}
