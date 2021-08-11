using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.HRM.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class LeaveSettlementApiController : ApiController
    {
        SLeaveSettlement leaveSettlementServices = null;
        LocalizedString loc = null;
        public LeaveSettlementApiController()
        {
            leaveSettlementServices = new SLeaveSettlement();
            loc = new LocalizedString();
        }
        [ActionFilter("3028")]
        public ServiceResult<List<LeaveSettlementViewModel>> Get()
        {
            string language = RiddhaSession.Language.ToString();
            int? branchId = RiddhaSession.BranchId;
            SLeaveSettlement service = new SLeaveSettlement();
            List<ELeaveSettlement> list = new List<ELeaveSettlement>();
            if (RiddhaSession.IsHeadOffice || RiddhaSession.DataVisibilityLevel == (int)DataVisibilityLevel.All)
            {
                list = service.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).ToList();
            }
            else
            {
                list = service.List().Data.Where(x => x.BranchId == branchId).ToList();
            }
            var emp = Common.GetEmployees().Data;
            var employeelist = (from c in list.ToList()
                                join d in emp on c.EmployeeId  equals d.Id
                                select new LeaveSettlementViewModel()
                                {
                                    Id = c.Id,
                                    EmployeeId = c.EmployeeId,
                                    FiscalYearId = c.FiscalYearId,
                                    FiscalYearName = c.FiscalYear.FiscalYear,
                                    LeaveMasterName = language == "ne" && c.LeaveMaster.NameNp != null ? c.LeaveMaster.NameNp : c.LeaveMaster.Name,
                                    Balance = c.Balance,
                                    EmployeeName = language == "ne" && c.Employee.NameNp != null ? c.Employee.NameNp : c.Employee.Name,
                                    LeaveMasterId = c.LeaveMasterId,
                                    SettlementType = c.SettlementType,
                                    Paid = getPaidBal(c.EmployeeId, c.LeaveMasterId, c.FiscalYearId),
                                    CarrytoNext = getCarryBal(c.EmployeeId, c.LeaveMasterId, c.FiscalYearId)
                                }).ToList();
            return new ServiceResult<List<LeaveSettlementViewModel>>()
            {
                Data = employeelist,
                Status = ResultStatus.Ok
            };
        }

        private decimal getPaidBal(int empId, int leaveId, int fyId)
        {
            decimal bal = 0;
            var leaveSettlement = leaveSettlementServices.List().Data.Where(x => x.FiscalYearId == fyId && x.LeaveMasterId == leaveId && x.EmployeeId == empId).ToList();
            foreach (var item in leaveSettlement)
            {
                if (item.SettlementType == SettlementType.Paid)
                {
                    bal = item.Balance;
                }
            }
            return bal;
        }
        private decimal getCarryBal(int empId, int leaveId, int fyId)
        {
            decimal bal = 0;
            var leaveSettlement = leaveSettlementServices.List().Data.Where(x => x.FiscalYearId == fyId && x.LeaveMasterId == leaveId && x.EmployeeId == empId).ToList();
            foreach (var item in leaveSettlement)
            {
                if (item.SettlementType == SettlementType.CarrytoNext)
                {
                    bal = item.Balance;
                }
            }
            return bal;
        }
        public ServiceResult<ELeaveSettlement> Get(int id)
        {
            ELeaveSettlement leaveSettlement = leaveSettlementServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ELeaveSettlement>()
            {
                Data = leaveSettlement,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<LeaveInfoVm> GetLeaveInfo(int leaveId, int empId)
        {
            int currentFiscalYearId = RiddhaSession.FYId;
            if (currentFiscalYearId == 0)
            {
                return new ServiceResult<LeaveInfoVm>()
                {
                    Data = null,
                    Message = loc.Localize("FiscalYearNotSet"),
                    Status = ResultStatus.processError
                };
            }
            if (leaveSettlementServices.IsLeaveSettled(empId, leaveId, currentFiscalYearId).Data)
            {
                return new ServiceResult<LeaveInfoVm>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = loc.Localize("LeaveSettledAlready")
                };
            }
            LeaveInfoVm vm = new LeaveInfoVm();
            var emp = new SEmployee().List().Data.Where(x => x.Id == empId).FirstOrDefault();
            decimal openingBalance = 0;
            if (RiddhaSession.EmploymentStatusWiseLeave)
            {
                var contract = new SContract().List().Data.Where(x => x.EmployeeId == empId).ToList();
                if (contract.Count() != 0)
                {
                    int employmentStatusId = contract.LastOrDefault().EmploymentStatusId;
                    openingBalance = new SEmploymentStatusWiseLeavedBalance().List().Data.Where(x => x.LeaveId == leaveId && x.EmploymentStatusId == employmentStatusId).FirstOrDefault().Balance;
                }
                else
                {
                    return new ServiceResult<LeaveInfoVm>()
                    {
                        Data = null,
                        Message = loc.Localize("This employee is not on any contract."),
                        Status = ResultStatus.processError
                    };
                }
            }
            else
            {
                openingBalance = new SDesignation().ListLeaveQouta().Data.Where(x => x.LeaveId == leaveId && x.DesignationId == emp.DesignationId).FirstOrDefault().Balance;
            }
            var currentSettlement = new SLeaveSettlement().List().Data.Where(x => x.EmployeeId == empId && x.LeaveMasterId == leaveId).ToList();
            vm.LeaveTaken = new SLeaveApplication().ListLeaveAppLog().Data.Where(x => x.LeaveApplication.EmployeeId == empId && x.LeaveApplication.LeaveMasterId == leaveId && x.FiscalYearId == currentFiscalYearId)
                .ToList()
                .Sum(x => x.LeaveCount);
            decimal settledLeave = 0;
            decimal carriedLeave = 0;
            if (currentSettlement.Count() > 0)
            {
                settledLeave = currentSettlement.Where(x => x.FiscalYearId == currentFiscalYearId).Sum(x => x.Balance);
                carriedLeave = leaveSettlementServices.GetCurrentCarryForwardBal(empId, leaveId, currentFiscalYearId).Data;
            }
            else
            {
                settledLeave = 0;
                carriedLeave = new SLeaveBalance().List().Data.Where(x => x.EmployeeId == empId && x.LeaveMasterId == leaveId).Select(x => x.OpeningBalance).FirstOrDefault();
            }
            vm.OpBal = openingBalance + carriedLeave;
            vm.RemLeave = (vm.OpBal - vm.LeaveTaken);
            return new ServiceResult<LeaveInfoVm>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("3018")]
        public ServiceResult<ELeaveSettlement> Post(LeaveSettlementViewModel model)
        {
            int currentFyId = RiddhaSession.FYId;
            if (leaveSettlementServices.IsLeaveSettled(model.EmployeeId, model.LeaveMasterId, currentFyId).Data)
            {
                return new ServiceResult<ELeaveSettlement>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = loc.Localize("LeaveSettledAlready")
                };
            }
            SLeaveApplication appServices = new SLeaveApplication();
            if (model.Paid + model.CarrytoNext != appServices.GetRemBal(model.LeaveMasterId, model.EmployeeId, currentFyId).Data)
            {
                return new ServiceResult<ELeaveSettlement>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = loc.Localize("SettlingBalShouldEqualRemBal")
                };
            }
            var result = new ServiceResult<ELeaveSettlement>();
            ELeaveSettlement settlement = new ELeaveSettlement();
            List<ELeaveSettlement> lst = new List<ELeaveSettlement>();
            int? branchId = RiddhaSession.CurrentUser.BranchId;

            if (model.CarrytoNext == 0)
            {
                settlement.BranchId = branchId;
                settlement.EmployeeId = model.EmployeeId;
                settlement.FiscalYearId = currentFyId;
                settlement.LeaveMasterId = model.LeaveMasterId;
                settlement.Balance = model.Paid;
                settlement.SettlementType = SettlementType.Paid;
                result = leaveSettlementServices.Add(settlement);
            }
            else if (model.Paid == 0)
            {
                settlement.BranchId = branchId;
                settlement.EmployeeId = model.EmployeeId;
                settlement.FiscalYearId = currentFyId;
                settlement.LeaveMasterId = model.LeaveMasterId;
                settlement.Balance = model.CarrytoNext;
                settlement.SettlementType = SettlementType.CarrytoNext;
                result = leaveSettlementServices.Add(settlement);
            }
            else
            {
                lst = new List<ELeaveSettlement>(){
                    new ELeaveSettlement(){BranchId=branchId,
                    Balance=model.Paid,
                    EmployeeId=model.EmployeeId,
                    FiscalYearId=currentFyId,
                    LeaveMasterId=model.LeaveMasterId,
                    SettlementType=SettlementType.Paid},
                    new ELeaveSettlement(){
                        BranchId=branchId,
                    Balance=model.CarrytoNext,
                    EmployeeId=model.EmployeeId,
                    FiscalYearId=currentFyId,
                    LeaveMasterId=model.LeaveMasterId,
                    SettlementType=SettlementType.CarrytoNext
                    }
                };
                result = leaveSettlementServices.AddRange(lst);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("3004", "3018", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
                }
            }
            return new ServiceResult<ELeaveSettlement>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status

            };
        }
        [ActionFilter("3019")]
        public ServiceResult<ELeaveSettlement> Put(LeaveSettlementViewModel model)
        {
            int currentFyId = RiddhaSession.FYId;
            SLeaveApplication appServices = new SLeaveApplication();
            if (model.Paid + model.CarrytoNext != appServices.GetRemBal(model.LeaveMasterId, model.EmployeeId, currentFyId).Data)
            {
                return new ServiceResult<ELeaveSettlement>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = loc.Localize("SettlingBalShouldEqualRemBal")
                };
            }
            var result = new ServiceResult<ELeaveSettlement>();
            ELeaveSettlement settlement = new ELeaveSettlement();
            List<ELeaveSettlement> lst = new List<ELeaveSettlement>();
            int? branchId = RiddhaSession.CurrentUser.BranchId;

            if (model.CarrytoNext == 0)
            {
                settlement.BranchId = branchId;
                settlement.EmployeeId = model.EmployeeId;
                settlement.FiscalYearId = currentFyId;
                settlement.LeaveMasterId = model.LeaveMasterId;
                settlement.Balance = model.Paid;
                settlement.SettlementType = SettlementType.Paid;
                result = leaveSettlementServices.Update(settlement);
            }
            else if (model.Paid == 0)
            {
                settlement.BranchId = branchId;
                settlement.EmployeeId = model.EmployeeId;
                settlement.FiscalYearId = currentFyId;
                settlement.LeaveMasterId = model.LeaveMasterId;
                settlement.Balance = model.CarrytoNext;
                settlement.SettlementType = SettlementType.CarrytoNext;
                result = leaveSettlementServices.Update(settlement);
            }
            else
            {
                lst = new List<ELeaveSettlement>(){
                    new ELeaveSettlement(){BranchId=branchId,
                    Balance=model.Paid,
                    EmployeeId=model.EmployeeId,
                    FiscalYearId=currentFyId,
                    LeaveMasterId=model.LeaveMasterId,
                    SettlementType=SettlementType.Paid},
                    new ELeaveSettlement(){
                        BranchId=branchId,
                    Balance=model.CarrytoNext,
                    EmployeeId=model.EmployeeId,
                    FiscalYearId=currentFyId,
                    LeaveMasterId=model.LeaveMasterId,
                    SettlementType=SettlementType.CarrytoNext
                    }
                };
                result = leaveSettlementServices.UpdateRange(lst);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("3004", "3019", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
                }
            }
            return new ServiceResult<ELeaveSettlement>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status

            };
        }
        [HttpDelete, ActionFilter("3020")]
        public ServiceResult<int> Delete(int id)
        {
            var result = leaveSettlementServices.Remove(id);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("3004", "3020", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<List<EmployeeDropdownVm>> GetEmployeesForDropdown()
        {
            int? branchId = RiddhaSession.BranchId;
            string language = RiddhaSession.Language.ToString();
            SEmployee employeeServices = new SEmployee();
            List<EmployeeDropdownVm> resultLst = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                                  select new EmployeeDropdownVm()
                                                  {
                                                      Id = c.Id,
                                                      Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name,
                                                      DesignationId = c.DesignationId,
                                                  }
                                                   ).ToList();
            return new ServiceResult<List<EmployeeDropdownVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<LeaveMasterDropdownVm>> GetDesigOrEmploymentStatusWiseLeave(int empId)
        {
            var emp = new SEmployee().List().Data.Where(x => x.Id == empId).FirstOrDefault();
            string language = RiddhaSession.Language.ToString();

            if (RiddhaSession.EmploymentStatusWiseLeave)
            {
                SContract contractServices = new SContract();
                var contract = contractServices.List().Data.Where(x => x.EmployeeId == empId).ToList();
                if (contract.Count() != 0)
                {
                    int employmentStatusId = contract.LastOrDefault().EmploymentStatusId;
                    SEmploymentStatusWiseLeavedBalance employmentStatusWiseLeavedBalanceServices = new SEmploymentStatusWiseLeavedBalance();
                    List<LeaveMasterDropdownVm> resultLst = (from c in employmentStatusWiseLeavedBalanceServices.List().Data.Where(x => x.EmploymentStatusId == employmentStatusId && (x.IsPaidLeave || x.IsLeaveCarryable)).ToList()
                                                             select new LeaveMasterDropdownVm()
                                                             {
                                                                 Id = c.LeaveId,
                                                                 Name = language == "ne" && c.Leave.NameNp != null ? c.Leave.NameNp : c.Leave.Name,
                                                                 IsLeaveCarryable = c.IsLeaveCarryable,
                                                                 IsPaidLeave = c.IsPaidLeave
                                                             }).ToList();
                    return new ServiceResult<List<LeaveMasterDropdownVm>>()
                    {
                        Data = resultLst,
                        Message = "",
                        Status = ResultStatus.Ok
                    };
                }
                else
                {
                    return new ServiceResult<List<LeaveMasterDropdownVm>>()
                    {
                        Data = null,
                        Message = loc.Localize("This employee is not on any contract."),
                        Status = ResultStatus.processError
                    };
                };
            }
            else
            {
                SDesignation desigServices = new SDesignation();
                List<LeaveMasterDropdownVm> resultLst = (from c in desigServices.ListLeaveQouta().Data.Where(x => x.DesignationId == emp.DesignationId && (x.IsPaidLeave || x.IsLeaveCarryable)).ToList()
                                                         select new LeaveMasterDropdownVm()
                                                         {
                                                             Id = c.LeaveId,
                                                             Name = language == "ne" && c.Leave.NameNp != null ? c.Leave.NameNp : c.Leave.Name,
                                                             IsLeaveCarryable = c.IsLeaveCarryable,
                                                             IsPaidLeave = c.IsPaidLeave
                                                         }).ToList();
                return new ServiceResult<List<LeaveMasterDropdownVm>>()
                {
                    Data = resultLst,
                    Status = ResultStatus.Ok
                };
            }

        }

        //[HttpGet]
        //public ServiceResult<List<LeaveMasterDropdownVm>> GetDesigWiseLeave(int desigId)
        //{
        //    string language = RiddhaSession.Language.ToString();
        //    SDesignation desigServices = new SDesignation();
        //    List<LeaveMasterDropdownVm> resultLst = (from c in desigServices.ListLeaveQouta().Data.Where(x => x.DesignationId == desigId && (x.IsPaidLeave || x.IsLeaveCarryable)).ToList()
        //                                             select new LeaveMasterDropdownVm()
        //                                             {
        //                                                 Id = c.LeaveId,
        //                                                 Name = language == "ne" && c.Leave.NameNp != null ? c.Leave.NameNp : c.Leave.Name,
        //                                                 IsLeaveCarryable = c.IsLeaveCarryable,
        //                                                 IsPaidLeave = c.IsPaidLeave
        //                                             }).ToList();
        //    return new ServiceResult<List<LeaveMasterDropdownVm>>()
        //    {
        //        Data = resultLst,
        //        Status = ResultStatus.Ok
        //    };
        //}
    }

    public class EmployeeDropdownVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DesignationId { get; set; }
    }
    public class LeaveMasterDropdownVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
    }
    public class LeaveInfoVm
    {
        public decimal OpBal { get; set; }
        public decimal LeaveTaken { get; set; }
        public decimal RemLeave { get; set; }
    }
    public class LeaveSettlementViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveMasterId { get; set; }
        public decimal Balance { get; set; }
        public decimal Paid { get; set; }
        public decimal CarrytoNext { get; set; }
        public SettlementType SettlementType { get; set; }
        public string LeaveMasterName { get; set; }
        public string FiscalYearName { get; set; }
        public string EmployeeName { get; set; }
        public int FiscalYearId { get; set; }
    }
}
