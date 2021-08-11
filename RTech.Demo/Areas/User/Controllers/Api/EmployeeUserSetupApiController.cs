using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
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
    public class EmployeeUserSetupApiController : ApiController
    {

        SEmployeeLogin employeeLoginServices = null;
        LocalizedString loc = null;
        public EmployeeUserSetupApiController()
        {
            employeeLoginServices = new SEmployeeLogin();
            loc = new LocalizedString();
        }
        public ServiceResult<List<EmployeeLoginGridVm>> Get()
        {

            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.CurrentUser.BranchId;
            SEmployeeLogin service = new SEmployeeLogin();
            var employeeUserlist = (from c in service.List().Data.Where(x => x.BranchId == branchId)
                                    select new EmployeeLoginGridVm()
                                {
                                    Id = c.Id,
                                    BranchName = language == "ne" && c.Branch.NameNp != null ? c.Branch.NameNp : c.Branch.Name,
                                    BranchId = c.BranchId,
                                    EmployeeName = language == "ne" && c.Employee.NameNp != null ? c.Employee.NameNp : c.Employee.Name,
                                    EmployeeId = (int)c.EmployeeId,
                                    UserName = c.UserName,
                                    Password = c.Password,
                                    RoleName = c.Role.Name,
                                    RoleId = c.RoleId,
                                }).ToList();

            return new ServiceResult<List<EmployeeLoginGridVm>>()
            {
                Data = employeeUserlist,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EEmployeeLogin> Get(int id)
        {
            EEmployeeLogin employee = employeeLoginServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EEmployeeLogin>()
            {
                Data = employee,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public ServiceResult<EEmployeeLogin> Post([FromBody]EEmployeeLogin model)
        {
            model.IsActivated = true;
            var result = employeeLoginServices.Add(model);
            return new ServiceResult<EEmployeeLogin>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPut]
        public ServiceResult<EEmployeeLogin> Put([FromBody]EEmployeeLogin model)
        {
            model.IsActivated = true;
            var result = employeeLoginServices.Update(model);
            return new ServiceResult<EEmployeeLogin>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var employee = employeeLoginServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = employeeLoginServices.Remove(employee);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetRolesForDropdown()
        {
            string language = RiddhaSession.Language.ToString();
            int? branchId = RiddhaSession.CurrentUser.BranchId;
            SUserRole userRoleServices = new SUserRole();
            List<DropdownViewModel> resultLst = (from c in userRoleServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = c.Name
                                                 }
                                                   ).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetEmployeesForDropdown()
        {
            string language = RiddhaSession.Language.ToString();
            int? branchId = RiddhaSession.CurrentUser.BranchId;
            SEmployee employeeServices = new SEmployee();
            List<DropdownViewModel> resultLst = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = c.Name
                                                 }
                                                   ).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
    }

    public class EmployeeLoginGridVm
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }

}