using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.User.Controllers.Api
{
    public class UserApiController : ApiController
    {
        SUser userServices = null;
        SEmployeeLogin empLoginServices = null;
        SEmployee empServices = null;
        LocalizedString loc = null;
        SBranch branchServices = null;
        public UserApiController()
        {
            userServices = new SUser();
            loc = new LocalizedString();
            branchServices = new SBranch();
            empLoginServices = new SEmployeeLogin();
            empServices = new SEmployee();
        }
        public ServiceResult<List<UserGridVm>> Get()
        {
            SUser service = new SUser();
            int? branchId = RiddhaSession.BranchId;
            int userType = RiddhaSession.UserType;
            if (userType == 0 || userType == 3)
            {
                var owneruserLst = (from c in service.List().Data.Where(x => x.BranchId == null && x.IsDeleted == false)
                                    select new UserGridVm()
                                    {
                                        Id = c.Id,
                                        BranchId = null,
                                        FullName = c.FullName,
                                        IsSuspended = c.IsSuspended,
                                        Name = c.Name,
                                        Password = c.Password,
                                        PhotoURL = c.PhotoURL,
                                        RoleId = c.RoleId,
                                        UserType = c.UserType,
                                        Email = c.Email,
                                    }).ToList();
                return new ServiceResult<List<UserGridVm>>()
                {
                    Data = owneruserLst,
                    Status = ResultStatus.Ok
                };
            }
            var userLst = (from c in service.List().Data.Where(x => x.BranchId == branchId && x.IsDeleted == false)
                           select new UserGridVm()
                           {
                               Id = c.Id,
                               BranchId = c.BranchId,
                               FullName = c.FullName,
                               IsSuspended = c.IsSuspended,
                               Name = c.Name,
                               Password = c.Password,
                               PhotoURL = c.PhotoURL,
                               RoleId = c.RoleId,
                               UserType = c.UserType,
                               Email = c.Email,
                           }).ToList();

            return new ServiceResult<List<UserGridVm>>()
            {
                Data = userLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EUser> Get(int id)
        {
            EUser user = userServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EUser>()
            {
                Data = user,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EUser> Post(UserVm model)
        {
            var branchId = RiddhaSession.BranchId;
            //model.User.BranchId = branchId;
            model.User.UserType = UserType.User;
            model.User.IsSuspended = false;
            if (RiddhaSession.UserType == 0 || RiddhaSession.UserType == 3)
            {
                model.User.BranchId = null;
                model.User.UserType = UserType.Owner;
                var userResult = userServices.Add(model.User);
                return new ServiceResult<EUser>()
                {
                    Data = userResult.Data,
                    Message = loc.Localize(userResult.Message),
                    Status = userResult.Status
                };
            }

            if (model.EmpId != 0)
            {
                var emp = empServices.List().Data.Where(x => x.Id == model.EmpId).FirstOrDefault();
                if (emp.UserId != null)
                {
                    return new ServiceResult<EUser>()
                    {
                        Data = null,
                        Message = "A user for employee " + emp.Name + " already exists.",
                        Status = ResultStatus.processError
                    };
                }

            }

            var result = userServices.Add(model.User);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("4002", "4006", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.User.Id, loc.Localize(result.Message));
            }
            if (model.EmpId != 0)
            {
                EEmployee employee = empServices.List().Data.Where(x => x.Id == model.EmpId).FirstOrDefault();
                if (employee != null)
                {
                    employee.UserId = model.User.Id;
                    empServices.Update(employee);
                }
                empLoginServices.Add(new EEmployeeLogin()
                {
                    EmployeeId = model.EmpId,
                    UserName = model.User.Name,
                    Password = model.User.Password,
                    BranchId = model.User.BranchId ?? 0,
                    RoleId = model.User.RoleId ?? 0,
                    IsActivated = false
                });
            }
            return new ServiceResult<EUser>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EUser> Put(UserVm model)
        {
            ServiceResult<EUser> result;
            if (RiddhaSession.UserType == 0 || RiddhaSession.UserType == 3)
            {
                var ownerUser = userServices.List().Data.Where(x => x.Id == model.User.Id).FirstOrDefault();
                ownerUser.BranchId = null;
                ownerUser.FullName = model.User.FullName;
                ownerUser.IsSuspended = false;
                ownerUser.Name = model.User.Name;
                ownerUser.Password = model.User.Password;
                if (ownerUser.UserType == UserType.Owner)
                {
                    ownerUser.UserType = UserType.Owner;
                    ownerUser.RoleId = model.User.RoleId;
                }
                else if (ownerUser.UserType == UserType.Admin)
                {
                    ownerUser.UserType = UserType.Admin;
                    ownerUser.RoleId = null;
                }
                var userResult = userServices.Update(ownerUser);

                return new ServiceResult<EUser>()
                {
                    Data = userResult.Data,
                    Message = loc.Localize(userResult.Message),
                    Status = userResult.Status
                };
            }
            if (model.EmpId != 0)
            {
                var emp = empServices.List().Data.Where(x => x.Id == model.EmpId).FirstOrDefault();
                //If the employee has not been linked to any user
                //link user to employee and add login in employee login table
                if (emp.UserId == null)
                {
                    //Clearing UserId field from previous employee in EEmployee Table and login details from EmployeeLogin table
                    var currentEmp = empServices.List().Data.Where(x => x.UserId == model.User.Id).FirstOrDefault();
                    if (currentEmp != null)
                    {
                        var currentEmpLogin = empLoginServices.List().Data.Where(x => x.EmployeeId == currentEmp.Id).FirstOrDefault();
                        empLoginServices.Remove(currentEmpLogin);
                        currentEmp.UserId = null;
                        empServices.Update(currentEmp);
                    }

                    result = userServices.Update(model.User);
                    emp.UserId = model.User.Id;
                    empServices.Update(emp);
                    empLoginServices.Add(new EEmployeeLogin()
                    {
                        EmployeeId = model.EmpId,
                        UserName = model.User.Name,
                        Password = model.User.Password,
                        BranchId = model.User.BranchId ?? 0,
                        RoleId = model.User.RoleId ?? 0,
                        IsActivated = false
                    });

                }
                // if the user linked to the table is the current user
                // update credentials in login and user table
                else if (emp.UserId == model.User.Id)
                {
                    result = userServices.Update(model.User);
                    EEmployeeLogin employeeLogin = empLoginServices.List().Data.Where(x => x.EmployeeId == emp.Id).FirstOrDefault();
                    employeeLogin.UserName = model.User.Name;
                    employeeLogin.Password = model.User.Password;
                    employeeLogin.BranchId = model.User.BranchId ?? 0;
                    employeeLogin.RoleId = model.User.RoleId ?? 0;

                    empLoginServices.Update(employeeLogin);
                }
                // Else the selected employee is already linked to another user
                // and cannot be linked to other users
                else
                {
                    return new ServiceResult<EUser>()
                    {
                        Data = null,
                        Message = "A user for employee " + emp.Name + " already exists.",
                        Status = ResultStatus.processError
                    };
                }
            }
            else
            {
                var currentEmp = empServices.List().Data.Where(x => x.UserId == model.User.Id).FirstOrDefault();
                if (currentEmp != null)
                {
                    currentEmp.UserId = null;
                    empServices.Update(currentEmp);
                }
                result = userServices.Update(model.User);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("4002", "4007", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.User.Id, loc.Localize(result.Message));
                }
            }

            return new ServiceResult<EUser>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };


        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var userRole = userServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var employee = empServices.List().Data.Where(x => x.UserId == id).FirstOrDefault();
            if (employee != null)
            {
                employee.UserId = null;
                empServices.Update(employee);
                EEmployeeLogin empLogin = empLoginServices.List().Data.Where(x => x.EmployeeId == employee.Id).FirstOrDefault();
                empLoginServices.Remove(empLogin);
            }
            if (userRole.RoleId == null)
            {
                return new ServiceResult<int>()
                {
                    Data = 0,
                    Message = "Admin can not be deleted",
                    Status = ResultStatus.processError
                };
            }
            var result = userServices.Remove(userRole);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("4002", "4008", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetBranchesForDropdown()
        {
            string language = RiddhaSession.Language.ToString();
            int companyId = RiddhaSession.CompanyId;
            var branch = branchServices.List().Data.Where(x => x.Id == RiddhaSession.BranchId && x.IsHeadOffice).FirstOrDefault();
            List<EBranch> branchList = new List<EBranch>();
            if (branch != null)
            {
                branchList = branchServices.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId).ToList();
            }
            else
            {
                branchList = branchServices.List().Data.Where(x => x.Id == RiddhaSession.BranchId).ToList();
            }
            var branches = (from c in branchList.ToList()
                            select new DropdownViewModel
                            {
                                Id = c.Id,
                                Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                            }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = branches,
                Status = ResultStatus.Ok,
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetRolesForDropdown()
        {
            string language = RiddhaSession.Language.ToString();
            int userType = RiddhaSession.UserType;
            int? branchId = RiddhaSession.BranchId;
            SUserRole roleServices = new SUserRole();
            List<DropdownViewModel> resultLst = new List<DropdownViewModel>();
            if (userType == 0 || userType == 3)
            {
                resultLst = (from c in roleServices.List().Data.Where(x => x.BranchId == null).ToList()
                             select new DropdownViewModel()
                             {
                                 Id = c.Id,
                                 Name = language == "ne" &&
                                 c.NameNp != null ? c.NameNp : c.Name
                             }).ToList();

            }
            else
            {
                resultLst = (from c in roleServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                             select new DropdownViewModel()
                             {
                                 Id = c.Id,
                                 Name = language == "ne" &&
                                 c.NameNp != null ? c.NameNp : c.Name
                             }).ToList();
            }

            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetRolesForDropdown(int branchId)
        {
            string language = RiddhaSession.Language.ToString();
            SUserRole roleServices = new SUserRole();
            List<DropdownViewModel> resultLst = new List<DropdownViewModel>();
            resultLst = (from c in roleServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                         select new DropdownViewModel()
                         {
                             Id = c.Id,
                             Name = language == "ne" &&
                             c.NameNp != null ? c.NameNp : c.Name
                         }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<UserGridVm>> GetUserKendoGrid(KendoPageListArguments arg)
        {
            int branchId = arg.BranchId;
            SUser userService = new SUser();
            SEmployee empServices = new SEmployee();
            List<EUser> users = new List<EUser>();
            IQueryable<EUser> userQuery;
            userQuery = userService.List().Data.Where(x => x.BranchId == branchId && x.IsDeleted ==false);
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EUser> paginatedQuery;
            var user = new SUser().List().Data.Where(x => x.BranchId == branchId);
            switch (searchField)
            {
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = userQuery.Where(x => x.Name.ToLower().Trim().StartsWith(searchValue.ToLower().Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Id);
                    }
                    else
                    {
                        paginatedQuery = userQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Id);
                    }
                    break;
                case "FullName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = userQuery.Where(x => x.FullName.ToLower().Trim().StartsWith(searchValue.ToLower().Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.RoleId);
                    }
                    else
                    {
                        paginatedQuery = userQuery.Where(x => x.FullName.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.RoleId);
                    }
                    break;
                default:
                    paginatedQuery = userQuery.OrderByDescending(x => x.Id).ThenBy(x => x.RoleId);
                    break;
            }

            int userType = RiddhaSession.UserType;
            if (userType == 3)
            {
                var owneruserLst = (from c in userService.List().Data.Where(x => x.BranchId == null && x.IsDeleted == false && (x.UserType == UserType.Owner || x.UserType == UserType.Admin))
                                    select new UserGridVm()
                                    {
                                        Id = c.Id,
                                        BranchId = null,
                                        FullName = c.FullName,
                                        IsSuspended = c.IsSuspended,
                                        Name = c.Name,
                                        Password = c.Password,
                                        PhotoURL = c.PhotoURL,
                                        RoleId = c.RoleId,
                                        RoleName = c.Role == null ? "" : c.Role.Name,
                                        UserType = c.UserType
                                    }).ToList();
                return new KendoGridResult<List<UserGridVm>>()
                {
                    Data = owneruserLst.Skip(arg.Skip).Take(arg.Take).ToList(),
                    Status = ResultStatus.Ok,
                    TotalCount = owneruserLst.Count(),
                };
            }

            var branch = new SBranch().List().Data.Where(x => x.Id == branchId).FirstOrDefault();
            
            if (branch != null)
            {
                //if (branch.IsHeadOffice)
                //{
                //    var branches = new SBranch().List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId).ToList();
                //    users = (from c in userService.List().Data.Where(x => x.IsDeleted == false).ToList()
                //             join d in branches on c.BranchId equals d.Id
                //             select c
                //    ).ToList();
                //}
                //else
                //{
                //    users = userService.List().Data.Where(x => x.BranchId == branchId && x.IsDeleted == false).ToList();
                //}
                if (branch.IsHeadOffice)
                {
                    var branches = new SBranch().List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId).ToList();
                    users = (from c in paginatedQuery.ToList()
                             join d in branches on c.BranchId equals d.Id
                             select c
                    ).ToList();
                }
                else
                {
                    users = paginatedQuery.ToList();
                }
            }

            //List<EEmployee> employees = empServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            var emp = Common.GetEmployees().Data;
            var userLst = (from c in paginatedQuery.ToList()
                           join e in emp on c.Id equals e.UserId
                           into temp
                           from e in temp.DefaultIfEmpty(new EEmployee())
                           select new UserGridVm()
                           {
                               Id = c.Id,
                               BranchId = c.BranchId,
                               BranchName = c.Branch.Name,
                               FullName = c.FullName == null ? "" : c.FullName,
                               IsSuspended = c.IsSuspended,
                               Name = c.Name,
                               Password = c.Password,
                               PhotoURL = c.PhotoURL == null ? "" : c.PhotoURL,
                               RoleId = c.RoleId,
                               RoleName = c.Role == null ? "" : c.Role.Name,
                               UserType = c.UserType,
                               EmpId = e.Id,
                               EmpName = e.Code == null ? "" : e.Code + " - " + e.Name
                           }).ToList();
            return new KendoGridResult<List<UserGridVm>>()
            {
                Data = userLst.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = userLst.Count(),
            };
        }

        [HttpGet]
        public ServiceResult<ChangePasswordViewModel> ChangePassword()
        {
            ChangePasswordViewModel vm = new ChangePasswordViewModel();
            var user = RiddhaSession.CurrentUser;
            vm.UserName = user.Name;
            vm.Password = user.Password;
            return new ServiceResult<ChangePasswordViewModel>()
            {
                Data = vm,
                Status = ResultStatus.Ok,
            };
        }

        [HttpPost]
        public ServiceResult<int> ChangePassword(ChangePasswordViewModel vm)
        {
            ServiceResult<int> result = new ServiceResult<int>();
            EUser user = userServices.List().Data.Where(x => x.Id == RiddhaSession.CurrentUser.Id).FirstOrDefault();
            if (user.Password != vm.CurrentPassword)
            {
                return new ServiceResult<int>() { Data = 0, Message = "Invalid User or Password", Status = ResultStatus.processError };
            }
            else if (vm.NewPassword != vm.ConfirmPassword)
            {
                return new ServiceResult<int>() { Data = 1, Message = "Passwords do not match", Status = ResultStatus.processError };
            }
            else
            {
                user.Password = vm.NewPassword;
                userServices.Update(user);
                return new ServiceResult<int>() { Data = 1, Message = "Changed Successfully", Status = ResultStatus.Ok };
            }
        }

        [HttpGet]
        public ServiceResult<bool> ResetUserPassword(string companycode, string username, string email)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            var branch = branchServices.List().Data.Where(x => x.Code.ToLower() == companycode.ToLower()).FirstOrDefault();
            if (branch != null)
            {
                // var user = empLoginServices.List().Data.Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault();
                var user = userServices.List().Data.Where(x => x.BranchId == branch.Id && x.Name == username && x.IsDeleted == false).FirstOrDefault();
                if (user != null)
                {
                    //If admin
                    if (user.RoleId == null)
                    {
                        if (branch.Email == email)
                        {
                            SendPasswordResetMail(branch.Email, user.Id);
                            result.Data = true;
                            result.Status = ResultStatus.Ok;
                            result.Message = "Reset Password link has been sent via email.";
                        }
                        else
                        {
                            result.Data = true;
                            result.Status = ResultStatus.Ok;
                            result.Message = "Email do not match.";
                        }

                    }
                    //If User
                    else
                    {
                        var Employee = empServices.List().Data.Where(x => x.UserId == user.Id).FirstOrDefault();
                        if (Employee == null)
                        {
                            result.Data = false;
                            result.Status = ResultStatus.processError;
                            result.Message = "Cannot send password reset link. There is no email associated with the provided username. Please contact admin.";
                        }
                        else if (Employee.Email == email)
                        {
                            var userId = Employee.UserId ?? 0;
                            SendPasswordResetMail(Employee.Email, userId);
                            result.Data = true;
                            result.Status = ResultStatus.Ok;
                            result.Message = "Reset Password link has been sent via email.";
                        }
                        else if (Employee.Email.Trim() == "") //NO Email
                        {
                            result.Data = true;
                            result.Status = ResultStatus.Ok;
                            result.Message = "Cannot send password reset link. There is no email associated with the provided username. Please contact admin.";
                        }
                        else //Email do not match
                        {
                            result.Data = true;
                            result.Status = ResultStatus.Ok;
                            result.Message = "Invalid Email";
                        }

                    }
                }
                else
                {
                    result.Data = false;
                    result.Status = ResultStatus.processError;
                    result.Message = "No user found with the provided username";
                    return result;
                }

            }
            else
            {
                result.Data = false;
                result.Status = ResultStatus.processError;
                result.Message = "Invalid Company Code";
            }

            return result;
        }

        public void SendPasswordResetMail(string email, int userId)
        {
            MailCommon mail = new MailCommon();
            string encryptedUser = EncryptQueryString("user=" + userId.ToString() + "&email=" + email);
            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var link = string.Format("{0}/activation/ResetUserPassword?{1}", baseUrl, encryptedUser);
            //var link = "Password reset";
            mail.SendMail(email, "Hamro-Hajiri Password reset Link.", link);

        }

        public string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Encrypt(strQueryString, "Hajiri@1");
        }

        [HttpPost]
        public ServiceResult<List<EmpSearchViewModel>> GetEmpLstForAutoComplete(EKendoAutoComplete model)
        {

            string lang = RiddhaSession.Language;
            int? branchId = model.BranchId;
            List<EmpSearchViewModel> resultLst = new List<EmpSearchViewModel>();
            if (model != null)
            {
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                List<EEmployee> empLst = new List<EEmployee>();
                empLst = new SEmployee().List().Data.Where(x => x.BranchId == branchId).ToList();
                if (searchText == null || searchText == "___")
                {
                    empLst = empLst.OrderBy(x => x.Name).Take(50).ToList();
                }
                else
                {
                    empLst = empLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).OrderBy(x => x.Name).Take(50).ToList();
                }
                resultLst = (from c in empLst
                             select new EmpSearchViewModel()
                             {
                                 Id = c.Id,
                                 Code = c.Code,
                                 Name = c.Code + " - " + c.Name + " - " + (c.Mobile ?? ""),
                                 EmployeeName = c.Name,
                                 Designation = c.Designation == null ? "" : c.Designation.Name,
                                 Section = c.Section == null ? "" : c.Section.Name,
                                 Department = c.Section == null ? "" : c.Section.Department.Name,
                                 DesignationId = c.DesignationId,
                                 Photo = c.ImageUrl,
                                 Email = c.Email,
                                 ReportingManagerId = c.ReportingManagerId,
                             }).ToList();
            }
            return new ServiceResult<List<EmpSearchViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
    }

    public class UserGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public UserType UserType { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhotoURL { get; set; }
        public bool IsSuspended { get; set; }
        public int? EmpId { get; set; }
        public string EmpName { get; set; }
    }
    public class UserVm
    {
        public EUser User { get; set; }
        public int EmpId { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
    }
}
