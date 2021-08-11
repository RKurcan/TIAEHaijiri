using Riddhasoft.Entity.User;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Extension;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
namespace RTech.Demo.Areas.User.Controllers.Api
{
    public class UserRoleApiController : ApiController
    {
        SUserRole userRoleServices = null;
        LocalizedString loc = null;

        public UserRoleApiController()
        {
            userRoleServices = new SUserRole();
            loc = new LocalizedString();
        }
        public ServiceResult<List<UserRoleGridVm>> Get()
        {
            SUserRole service = new SUserRole();
            int? branchId = RiddhaSession.BranchId;
            int userType = RiddhaSession.UserType;
            if (userType == 0 || userType == 3)
            {
                var owneruserRoleLst = (from c in service.List().Data.Where(x => x.BranchId == null)
                                        select new UserRoleGridVm()
                                        {
                                            Id = c.Id,
                                            BranchId = 0,
                                            Name = c.Name,
                                            NameNp = c.NameNp,
                                            Priority = c.Priority
                                        }).ToList();
                return new ServiceResult<List<UserRoleGridVm>>()
                {
                    Data = owneruserRoleLst,
                    Status = ResultStatus.Ok
                };
            }
            else
            {
                var userRoleLst = (from c in service.List().Data.Where(x => x.BranchId == branchId)
                                   select new UserRoleGridVm()
                                   {
                                       Id = c.Id,
                                       BranchId = c.BranchId ?? 0,
                                       Name = c.Name,
                                       NameNp = c.NameNp,
                                       Priority = c.Priority
                                   }).ToList();

                return new ServiceResult<List<UserRoleGridVm>>()
                {
                    Data = userRoleLst,
                    Status = ResultStatus.Ok
                };
            }
        }
        public ServiceResult<List<UserRoleGridVm>> Get(int branchId)
        {
            SUserRole service = new SUserRole();
            int BranchId = branchId;
            var userRoleLst = (from c in service.List().Data.Where(x => x.BranchId == BranchId)
                               select new UserRoleGridVm()
                               {
                                   Id = c.Id,
                                   BranchId = c.BranchId ?? 0,
                                   Name = c.Name,
                                   NameNp = c.NameNp,
                                   Priority = c.Priority
                               }).ToList();

            return new ServiceResult<List<UserRoleGridVm>>()
            {
                Data = userRoleLst,
                Status = ResultStatus.Ok
            };
        }

        //public ServiceResult<EUserRole> Get(int id)
        //{
        //    EUserRole userRole = userRoleServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
        //    return new ServiceResult<EUserRole>()
        //    {
        //        Data = userRole,
        //        Message = "Added Successfully",
        //        Status = ResultStatus.Ok
        //    };
        //}
        [HttpGet]
        public ServiceResult<int> GetDataVisibilityLevel(int roleId)
        {
            int dataVisibilityLevel = userRoleServices.GetDataVisibilityLevel(roleId).Data;
            return new ServiceResult<int>()
            {
                Data = dataVisibilityLevel,

            };
        }
        [HttpPost]
        public ServiceResult<EUserRole> Post(EUserRole model)
        {
            //if (RiddhaSession.UserType == 0 || RiddhaSession.UserType == 3)
            //{
            //    model.BranchId = null;
            //}
            //else
            //{
            //    model.BranchId = RiddhaSession.BranchId;
            //}
            if (RiddhaSession.UserType == 0 || RiddhaSession.UserType == 3)
            {
                model.BranchId = null;
            }
            var result = userRoleServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("4001", "4003", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EUserRole>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPost]
        public ServiceResult<EUserGroupDataVisibility> CreateDataVisibility(EUserGroupDataVisibility model)
        {
            var result = userRoleServices.UpdateDataVisibility(model);
            return new ServiceResult<EUserGroupDataVisibility>()
            {
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<string[]> GetRolePermissions(int roleId)
        {
            var actionRights = userRoleServices.ListActionRights().Data.Where(x => x.RoleId == roleId);
            string[] result = (from c in actionRights
                               select c.MenuAction.ActionCode).ToArray();
            return new ServiceResult<string[]>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };

        }
        [HttpPost]
        public ServiceResult<PermissionViewModel> PermitRoles(PermissionViewModel model)
        {
            List<EUserGroupMenuRight> menuRights = new List<EUserGroupMenuRight>();
            List<EUserGroupActionRight> actionRights = new List<EUserGroupActionRight>();
            if (model.TreeViewLst != null)
            {
                menuRights = (from c in model.TreeViewLst.Where(x => x.hasChildren == false)
                              join d in userRoleServices.ListMenus().Data on c.parentId equals d.Code
                              select new EUserGroupMenuRight()
                              {
                                  MenuId = d.Id,
                                  RoleId = model.RoleId
                              }).DistinctBy(x => x.MenuId).ToList();
                actionRights = (from c in model.TreeViewLst.Where(x => x.hasChildren == false)
                                join d in userRoleServices.ListMenuActions().Data on c.id equals d.ActionCode
                                select new EUserGroupActionRight()
                                {
                                    MenuActionId = d.Id,
                                    RoleId = model.RoleId
                                }).ToList();
            }
            var result = userRoleServices.PermitRoles(menuRights, actionRights, model.RoleId);
            return new ServiceResult<PermissionViewModel>()
            {
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        public ServiceResult<EUserRole> Put(EUserRole model)
        {
            //if (RiddhaSession.UserType == 0 || RiddhaSession.UserType == 3)
            //{
            //    model.BranchId = null;
            //}
            //else
            //{
            //    model.BranchId = RiddhaSession.BranchId;
            //}
            if (RiddhaSession.UserType == 0 || RiddhaSession.UserType == 3)
            {
                model.BranchId = null;
            }
            var result = userRoleServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("4001", "4004", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EUserRole>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var userRole = userRoleServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = userRoleServices.Remove(userRole);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("4001", "4005", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }

    public class UserRoleGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int Priority { get; set; }
        public int BranchId { get; set; }
    }
    public class PermissionViewModel
    {
        public int RoleId { get; set; }
        public List<KendoTreeViewModel> TreeViewLst { get; set; }
    }
    public class KendoTreeViewModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public bool hasChildren { get; set; }
        public string parentId { get; set; }
    }

}
