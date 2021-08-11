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
    public class ControllerPermissionApiController : ApiController
    {
        SUserRole roleServices = null;
        SRoleOnController controllerServices = null;
        public ControllerPermissionApiController()
        {
            roleServices = new SUserRole();
            controllerServices = new SRoleOnController();
        }
        public ServiceResult<IQueryable<ERoleOnController>> Get()
        {
            return controllerServices.List();
        }
        [HttpGet]
        public ServiceResult<List<ControllerViewModel>> GetControllerByModule(int id, int roleId)
        {
            List<ControllerViewModel> list = new List<ControllerViewModel>();
            foreach (var item in ModuleControllerData.GetData().Where(x => (int)x.ModuleId == id))
            {
                ControllerViewModel vm = new ControllerViewModel();
                vm.Id = (int)item.ControllerId;
                vm.Name = Enum.GetName(typeof(AppController), item.ControllerId);
                vm.Checked = IsMapped(roleId, item.ControllerId);
                list.Add(vm);
            }
            return new ServiceResult<List<ControllerViewModel>>()
            {
                Data = list,
                Status = ResultStatus.Ok
            };
        }
        private bool IsMapped(int roleId, AppController appController)
        {
            return controllerServices.List().Data.Where(x => x.RoleId == roleId && x.AppController == appController).Any();
        }
        public ServiceResult<ControllerPermissionViewModel> Post(ControllerPermissionViewModel viewModel)
        {
            List<ERoleOnController> controllersExistingLst = new List<ERoleOnController>();
            controllersExistingLst = (from c in controllerServices.List().Data.Where(x => x.RoleId == viewModel.RoleId).ToList()
                                      join d in viewModel.Controllers on (int)c.AppController equals d.Id
                                      select c).ToList();
            if (controllersExistingLst.Count() > 0)
            {
                controllerServices.RemoveRange(controllersExistingLst);
            }
            //save the viewModel into roles on module entiry
            foreach (var item in viewModel.Controllers.Where(x => x.Checked))
            {
                ERoleOnController model = new ERoleOnController();
                model.RoleId = viewModel.RoleId;
                model.AppController = (AppController)item.Id;
                controllerServices.Add(model);
            }

            return new ServiceResult<ControllerPermissionViewModel>()
            {
                Data = viewModel,
                Message = "Succesfully Saved",
                Status = ResultStatus.Ok
            };
        }
    }
    public class ControllerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
    public class ControllerPermissionViewModel
    {
        public int RoleId { get; set; }
        public List<ControllerViewModel> Controllers { get; set; }
    }
}
