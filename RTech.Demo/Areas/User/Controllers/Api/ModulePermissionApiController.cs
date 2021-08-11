using Riddhasoft.Entity.User;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.User.Controllers.Api
{
    public class ModulePermissionApiController : ApiController
    {
        SRoleOnModule roleModuleServices = null;
        SUserRole roleServices = null;

        public ModulePermissionApiController()
        {
            roleModuleServices = new SRoleOnModule();
            roleServices = new SUserRole();
        }
        [HttpGet]
        public ServiceResult<List<ModuleViewModel>> GetModulesByRole(int id)
        {
            List<ModuleViewModel> list = new List<ModuleViewModel>();
            foreach (var item in roleModuleServices.List().Data.Where(x => x.RoleId == id))
            {
                ModuleViewModel moduleVm = new ModuleViewModel();
                moduleVm.Id = (int)item.Module;
                moduleVm.Name = Enum.GetName(typeof(Module), item.Module);
                list.Add(moduleVm);
            }
            return new ServiceResult<List<ModuleViewModel>>()
            {
                Data = list,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<List<ModulePermissionViewModel>> Get()
        {
            List<ModulePermissionViewModel> viewModelList = new List<ModulePermissionViewModel>();
            var roles = roleServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            //map enum into ModuleViewModel
            foreach (var role in roles)
            {
                ModulePermissionViewModel viewModel = new ModulePermissionViewModel();
                viewModel.RoleId = role.Id;
                viewModel.RoleName = role.Name;
                viewModel.Modules = ((IEnumerable<Module>)Enum.GetValues(typeof(Module))).Select(c => new ModuleViewModel() { Id = (int)c, Name = c.ToString(), Checked = IsMapped(role.Id, (int)c) }).ToList();
                viewModelList.Add(viewModel);
            }
            return new ServiceResult<List<ModulePermissionViewModel>>()
            {
                Data = viewModelList,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<ModulePermissionViewModel>> GetOwnerPermissions()
        {
            List<ModulePermissionViewModel> viewModelList = new List<ModulePermissionViewModel>();
            var roles = roleServices.List().Data.Where(x => x.BranchId ==null).ToList();
            //map enum into ModuleViewModel
            foreach (var role in roles)
            {
                ModulePermissionViewModel viewModel = new ModulePermissionViewModel();
                viewModel.RoleId = role.Id;
                viewModel.RoleName = role.Name;
                viewModel.Modules = OwnerPermissionData.GetOwnerPermissionList().Select(c => new ModuleViewModel() { Id = c.Id, Name = c.Controller, Checked = IsOwnerPermissionMapped(role.Id,c.Id) }).ToList();
                viewModelList.Add(viewModel);
            }
            return new ServiceResult<List<ModulePermissionViewModel>>()
            {
                Data = viewModelList,
                Status = ResultStatus.Ok
            };
        }
        private bool IsMapped(int roleId, int moduleId)
        {
            return roleModuleServices.List().Data.Where(x => x.RoleId == roleId && x.Module == (Module)moduleId).Any();
        }
        private bool IsOwnerPermissionMapped(int roleId, int controllerId)
        {
            return roleModuleServices.ListOwnerPermission().Data.Where(x => x.RoleId == roleId && x.ControllerId == controllerId).Any();
        }
        public ServiceResult<List<ModulePermissionViewModel>> Post(MOdulePermistionPostViewModel model)
        {
            List<ERoleOnModule> ModulesExistingLst = new List<ERoleOnModule>();
            ModulesExistingLst = roleModuleServices.List().Data.ToList();
            if (ModulesExistingLst.Count() > 0)
            {
                roleModuleServices.RemoveRange(ModulesExistingLst);
            }
            //save the viewModel into roles on module entiry
            foreach (var viewModel in model.viewModel)
            {
                ERoleOnModule roleOnModule = new ERoleOnModule();
                var selectedModule = viewModel.Modules.Where(x => x.Checked);
                foreach (var module in selectedModule)
                {
                    roleOnModule.Module = (Module)module.Id;
                    roleOnModule.RoleId = viewModel.RoleId;
                    roleModuleServices.Add(roleOnModule);
                }
            }
            return new ServiceResult<List<ModulePermissionViewModel>>()
            {
                Data = new List<ModulePermissionViewModel>(),
                Message = "Successfully Saved",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<ModulePermissionViewModel>> SaveOwnerPermission(MOdulePermistionPostViewModel model)
        {
            List<EOwnerPermission> ModulesExistingLst = new List<EOwnerPermission>();
            ModulesExistingLst = roleModuleServices.ListOwnerPermission().Data.ToList();
            if (ModulesExistingLst.Count() > 0)
            {
                roleModuleServices.RemoveRangeOwnerPermission(ModulesExistingLst);
            }
            //save the viewModel into roles on module entiry
            foreach (var viewModel in model.viewModel)
            {
                EOwnerPermission roleOnModule = new EOwnerPermission();
                var selectedModule = viewModel.Modules.Where(x => x.Checked);
                foreach (var module in selectedModule)
                {
                    roleOnModule.ControllerId = module.Id;
                    roleOnModule.RoleId = viewModel.RoleId;
                    roleModuleServices.AddOwnerPermission(roleOnModule);
                }
            }
            return new ServiceResult<List<ModulePermissionViewModel>>()
            {
                Data = new List<ModulePermissionViewModel>(),
                Message = "Successfully Saved",
                Status = ResultStatus.Ok
            };
        }

    }
    public class ModulePermissionViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<ModuleViewModel> Modules { get; set; }
    }
    public class ModuleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
    public class MOdulePermistionPostViewModel
    {
        public List<ModulePermissionViewModel> viewModel { get; set; }
    }
}
