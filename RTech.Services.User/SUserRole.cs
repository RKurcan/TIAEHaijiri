using Riddhasoft.DB;
using Riddhasoft.Entity.User;
using Riddhasoft.Services.Common;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.User
{
    public class SUserRole : Riddhasoft.Services.Common.IBaseService<EUserRole>
    {
        RiddhaDBContext db = null;
        public SUserRole()
        {
            db = new RiddhaDBContext();
        }

        public Common.ServiceResult<IQueryable<EUserRole>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EUserRole>>()
            {
                Data = db.UserRole,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<EUserRole> Add(EUserRole model)
        {
            db.UserRole.Add(model);
            db.SaveChanges();
            return new ServiceResult<EUserRole>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<EUserRole> Update(EUserRole model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EUserRole>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Common.ServiceResult<int> Remove(EUserRole model)
        {
            try
            {
                int userCount = db.User.Count(x => x.RoleId == model.Id);
                if (userCount > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("There {1} {0} User on this User Role. User Role Can't be deleted.", userCount, userCount == 1 ? "is" : "are"),
                        Status = ResultStatus.dataBaseError
                    };
                }
                model = db.UserRole.Remove(db.UserRole.Find(model.Id));
                db.SaveChanges();
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = "RemoveSuccess",
                    Status = ResultStatus.Ok
                };
            }
            catch (SqlException ex)
            {

                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.dataBaseError
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.unHandeledError
                };
            }
        }

        public ServiceResult<IQueryable<EMenu>> ListMenus()
        {
            return new ServiceResult<IQueryable<EMenu>>()
            {
                Data = db.Menu,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EMenuAction>> ListMenuActions()
        {
            return new ServiceResult<IQueryable<EMenuAction>>()
            {
                Data = db.MenuAction,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<bool> PermitRoles(List<EUserGroupMenuRight> menuRights, List<EUserGroupActionRight> actionRights, int roleId)
        {
            var existingMenuRights = db.UserGroupMenuRight.Where(x => x.RoleId == roleId).ToList();
            if (existingMenuRights.Count() > 0)
            {
                db.UserGroupMenuRight.RemoveRange(existingMenuRights);
                db.SaveChanges();
            }
            if (menuRights.Count() > 0)
            {
                db.UserGroupMenuRight.AddRange(menuRights);
                db.SaveChanges();
            }
            var existingActionRights = db.UserGroupActionRight.Where(x => x.RoleId == roleId).ToList();
            if (existingActionRights.Count() > 0)
            {
                db.UserGroupActionRight.RemoveRange(existingActionRights);
                db.SaveChanges();
            }
            if (actionRights.Count() > 0)
            {
                db.UserGroupActionRight.AddRange(actionRights);
                db.SaveChanges();
            }

            return new ServiceResult<bool>()
            {
                Data = true,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EUserGroupActionRight>> ListActionRights()
        {
            return new ServiceResult<IQueryable<EUserGroupActionRight>>()
            {
                Data = db.UserGroupActionRight,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EUserGroupMenuRight>> ListMenuRights()
        {
            return new ServiceResult<IQueryable<EUserGroupMenuRight>>()
            {
                Data = db.UserGroupMenuRight,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> GetDataVisibilityLevel(int roleId)
        {
            var vis = db.UserGroupDataVisibility.Where(x => x.RoleId == roleId).FirstOrDefault() ?? new EUserGroupDataVisibility();
            return new ServiceResult<int>()
            {
                Data = (int)vis.DataVisibilityLevel
            };
        }

        public ServiceResult<object> UpdateDataVisibility(EUserGroupDataVisibility model)
        {
            var existing = db.UserGroupDataVisibility.Where(x => x.RoleId == model.RoleId);
            if (existing.Count() > 0)
            {
                db.UserGroupDataVisibility.RemoveRange(existing);
                db.SaveChanges();

            }
            db.UserGroupDataVisibility.Add(model);
            db.SaveChanges();
            return new ServiceResult<object>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "Data Visibility Updated Successfully...!!"
            };
        }
    }
}
