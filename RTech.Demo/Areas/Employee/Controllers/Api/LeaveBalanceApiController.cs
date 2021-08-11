using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class LeaveBalanceApiController : ApiController
    {
        SLeaveBalance leaveBalanceServices = null;
        SEmployee employeeServices = null;
        SLeaveMaster leavMastServices = null;
        LocalizedString loc = null;
        public LeaveBalanceApiController()
        {
            leaveBalanceServices = new SLeaveBalance();
            employeeServices = new SEmployee();
            leavMastServices = new SLeaveMaster();
            loc = new LocalizedString();
        }
        public ServiceResult<IQueryable<ELeaveBalance>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var leaveBalance = branchId != null ? leaveBalanceServices.List().Data.Where(x => x.Employee.BranchId == branchId) : leaveBalanceServices.List().Data;
            return new ServiceResult<IQueryable<ELeaveBalance>>()
            {
                Data = leaveBalance,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ELeaveBalance> Get(int id)
        {
            var leaveBalance = leaveBalanceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ELeaveBalance>()
            {
                Data = leaveBalance,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<LeaveBalanceViewModel> SearchEmployee(string empCode, string empName)
        {
            var branchId = RiddhaSession.BranchId;
            LeaveBalanceViewModel vm = new LeaveBalanceViewModel();
            vm.Employee = new EmpSearchViewModel();
            vm.LeaveMasters = new List<LeaveMasterViewModel>();
            var empList = employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            EEmployee employee = new EEmployee();
            if (empCode != null)
            {
                employee = empList.Where(x => x.Code == empCode).FirstOrDefault();
            }
            else if (empName != null)
            {
                employee = empList.Where(x => x.Name.ToUpper().Contains(empName.ToUpper())).FirstOrDefault();
            }
            if (employee != null)
            {
                vm.Employee.Id = employee.Id;
                vm.Employee.Code = employee.Code;
                vm.Employee.Name = employee.Name;
                vm.Employee.Designation = employee.Designation == null ? "" : employee.Designation.Name;
                vm.Employee.Department = employee.Section.Department == null ? "" : employee.Section.Department.Name;
                vm.Employee.Section = employee.Section == null ? "" : employee.Section.Name;
                vm.Employee.Photo = employee.ImageUrl;
                var existingLeaveBal = leaveBalanceServices.List().Data.Where(x => x.EmployeeId == employee.Id).ToList();
                if (existingLeaveBal.Count() > 0)
                {
                    vm.LeaveMasters = (from c in leavMastServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                       join d in existingLeaveBal
                                         on c.Id equals d.LeaveMasterId
                                         into temp
                                       from j in temp.DefaultIfEmpty(new ELeaveBalance())
                                       select new LeaveMasterViewModel()
                                         {
                                             Id = c.Id,
                                             Name = c.Name,
                                             OpeningBal = j.OpeningBalance
                                         }).ToList();
                }
            }

            return new ServiceResult<LeaveBalanceViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<LeaveBalanceViewModel> Post([FromBody]LeaveBalanceViewModel vm)
        {
            var existingLeaveBal = leaveBalanceServices.List().Data.Where(x => x.EmployeeId == vm.Employee.Id).ToList();
            if (existingLeaveBal.Count() > 0)
            {
                leaveBalanceServices.RemoveRange(existingLeaveBal);
            }
            ELeaveBalance model = new ELeaveBalance();
            foreach (var item in vm.LeaveMasters)
            {
                model.EmployeeId = vm.Employee.Id;
                model.LeaveMasterId = item.Id;
                model.OpeningBalance = item.OpeningBal;
                model.RemainingBalance = item.OpeningBal;
                leaveBalanceServices.Add(model);
            }
            return new ServiceResult<LeaveBalanceViewModel>()
            {
                Data = vm,
                Message =loc.Localize("AddedSuccess"),
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ELeaveBalance> Put([FromBody]ELeaveBalance model)
        {
            model.Employee.BranchId = RiddhaSession.BranchId;
            var result= leaveBalanceServices.Update(model);
            return new ServiceResult<ELeaveBalance>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var leaveBalance = leaveBalanceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result= leaveBalanceServices.Remove(leaveBalance);
            return new ServiceResult<int>()
            {

                Data = result.Data,
                Message = loc.Localize(result.Message)
            };
        }
    }
    public class LeaveBalanceViewModel
    {
        public EmpSearchViewModel Employee { get; set; }
        public List<LeaveMasterViewModel> LeaveMasters { get; set; }
    }

    public class LeaveMasterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OpeningBal { get; set; }
    }
}
