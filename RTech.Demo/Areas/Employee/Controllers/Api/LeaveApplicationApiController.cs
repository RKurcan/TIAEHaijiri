using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.HRM.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web.Http;
using static RTech.Demo.Utilities.WDMS;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class LeaveApplicationApiController : ApiController
    {
        SLeaveApplication leaveAppicationServices = null;
        SEmployee employeeServices = null;
        SLeaveMaster leaveMasterServices = null;
        SLeaveBalance leaveBalServices = null;
        LocalizedString loc = null;
        SUser userServices = null;
        SDateTable dateTableServices = null;
        string language = RiddhaSession.Language;
        public LeaveApplicationApiController()
        {
            leaveAppicationServices = new SLeaveApplication();
            employeeServices = new SEmployee();
            leaveMasterServices = new SLeaveMaster();
            leaveBalServices = new SLeaveBalance();
            loc = new LocalizedString();
            userServices = new SUser();
            dateTableServices = new SDateTable();
        }
        [ActionFilter("3026")]
        public ServiceResult<List<LeaveApplicationGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SLeaveApplication service = new SLeaveApplication();
            var maxDate = DateTime.Today.AddDays(1);
            var applicationList = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                                   select new LeaveApplicationGridVm()
                                   {
                                       Id = c.Id,
                                       EmployeeCode = c.Employee.Code,
                                       EmployeeName = c.Employee.Name,
                                       LeaveMaster = c.LeaveMaster.Name,
                                       LeaveMasterId = c.LeaveMasterId,
                                       From = c.From.ToString("yyyy/MM/dd"),
                                       To = c.To.ToString("yyyy/MM/dd"),
                                       LeaveDayName = Enum.GetName(typeof(LeaveDay), c.LeaveDay),
                                       Description = c.Description,
                                       ApprovedById = c.ApprovedById,
                                       LeaveStatusName = Enum.GetName(typeof(LeaveStatus), c.LeaveStatus),
                                   }).ToList();

            return new ServiceResult<List<LeaveApplicationGridVm>>()
            {
                Data = applicationList,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ELeaveApplication> Get(int id)
        {
            var leaveAppication = leaveAppicationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var leaveapplication = (from c in leaveAppicationServices.List().Data.Where(x => x.Id == id)
                                    select new EmpSearchViewModel()
                                    {
                                        Id = c.Id,
                                        EmployeeId = c.EmployeeId,
                                        Code = c.Employee.Code,
                                        Name = c.Employee.Name,
                                        Section = c.Employee.Section.Name,
                                        Designation = c.Employee.Designation.Name,
                                        DesignationId = c.Employee.DesignationId,
                                        Department = c.Employee.Section.Department.Name,
                                        Photo = c.Employee.ImageUrl

                                    }).ToList();
            return new ServiceResult<ELeaveApplication>()
            {
                Data = leaveAppication,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<decimal> GetRemLeave(int leaveMastId, int empId)
        {
            SLeaveSettlement leaveSettlementServices = new SLeaveSettlement();
            int currentfiscalYearId = RiddhaSession.FYId;
            if (currentfiscalYearId == 0)
            {
                return new ServiceResult<decimal>()
                {
                    Data = 0,
                    Message = loc.Localize("FiscalYearNotSet"),
                    Status = ResultStatus.processError
                };
            }
            int currentFyId = RiddhaSession.FYId;
            if (leaveSettlementServices.IsLeaveSettled(empId, leaveMastId, currentfiscalYearId).Data)
            {
                return new ServiceResult<decimal>()
                {
                    Data = 0,
                    Status = ResultStatus.processError,
                    Message = loc.Localize("LeaveSettledAlready")
                };
            }
            SDesignation desigServices = new SDesignation();
            decimal remLeave = leaveAppicationServices.GetRemBal(leaveMastId, empId, currentfiscalYearId).Data;
            return new ServiceResult<decimal>()
            {
                Data = remLeave,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EmpSearchViewModel> SearchEmployee(string empCode, string empName)
        {
            EmpSearchViewModel vm = new EmpSearchViewModel();
            var empList = employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated).ToList();
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

                vm.Id = employee.Id;
                vm.Code = employee.Code;
                vm.Name = employee.Name;
                vm.Designation = employee.Designation == null ? "" : employee.Designation.Name;
                vm.DesignationId = employee.DesignationId;
                vm.Section = employee.Section == null ? "" : employee.Section.Name;
                vm.Department = employee.Section == null ? "" : employee.Section.Department == null ? "" : employee.Section.Department.Name;
                vm.Photo = employee.ImageUrl;
            }
            return new ServiceResult<EmpSearchViewModel>()
            {
                Data = vm,
                Message = loc.Localize("AddedSuccessfully"),
                Status = ResultStatus.Ok
            };
        }

        [ActionFilter("3010")]
        public ServiceResult<ELeaveApplication> Post(LeaveApplicationViewModel vm)
        {
            bool isInValid = validateDulicateLeaveOnSameDate(vm);
            if (!isInValid)
            {
                return new ServiceResult<ELeaveApplication>()
                {
                    Data = null,
                    Message = "Already on leave.",
                    Status = ResultStatus.processError,
                };
            }
            SLeaveSettlement leaveSettlementServices = new SLeaveSettlement();
            LocalizedString loc = new LocalizedString();
            int currentFiscalYearId = RiddhaSession.FYId;
            if (leaveSettlementServices.IsLeaveSettled(vm.EmployeeId, vm.LeaveMasterId, currentFiscalYearId).Data)
            {
                return new ServiceResult<ELeaveApplication>()
                {
                    Data = null,
                    Message = loc.Localize("LeaveSettledAlready"),
                    Status = ResultStatus.processError
                };
            }
            if (currentFiscalYearId == 0)
            {
                return new ServiceResult<ELeaveApplication>()
                {
                    Data = null,
                    Message = loc.Localize("FiscalYearNotSet"),
                    Status = ResultStatus.processError
                };
            }
            decimal remLeave = leaveAppicationServices.GetRemBal(vm.LeaveMasterId, vm.EmployeeId, currentFiscalYearId).Data;
            if (remLeave == 0)
            {
                return new ServiceResult<ELeaveApplication>()
                {
                    Data = null,
                    Message = loc.Localize("NoRemainingLeave"),
                    Status = ResultStatus.processError
                };
            }
            ELeaveApplication leaveAppModel = new ELeaveApplication();
            leaveAppModel.EmployeeId = vm.EmployeeId;
            leaveAppModel.BranchId = RiddhaSession.BranchId;
            leaveAppModel.CreatedById = RiddhaSession.UserId;
            leaveAppModel.From = vm.From;
            if (vm.LeaveDay != LeaveDay.FullDay)
            {
                leaveAppModel.To = vm.From;
            }
            else
            {
                leaveAppModel.To = vm.To;
            }
            leaveAppModel.LeaveMasterId = vm.LeaveMasterId;
            leaveAppModel.TransactionDate = DateTime.Now;
            leaveAppModel.ApprovedById = vm.ApprovedById;
            leaveAppModel.LeaveDay = vm.LeaveDay;
            leaveAppModel.Description = vm.Description;
            leaveAppModel.LeaveStatus = LeaveStatus.New;
            leaveAppModel.AdminRemark = "";
            EleaveApplicationLog log = new EleaveApplicationLog();
            log.FiscalYearId = currentFiscalYearId;
            log.LeaveCount = (vm.To - vm.From).Days + 1;
            var result = leaveAppicationServices.Add(leaveAppModel, log);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("3002", "3010", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Id, loc.Localize(result.Message));
                if (RiddhaSession.PackageId == 3)
                {
                    var requestingEmployee = employeeServices.List().Data.Where(x => x.Id == vm.EmployeeId).FirstOrDefault();
                    if (requestingEmployee != null)
                    {
                        if (requestingEmployee.ReportingManagerId != null)
                        {
                            //prepare email template
                            var manager = employeeServices.List().Data.Where(x => x.Id == requestingEmployee.ReportingManagerId).FirstOrDefault();
                            if (manager != null)
                            {
                                if (!string.IsNullOrEmpty(manager.Email))
                                {
                                    var mail = new MailCommon();
                                    var subject = "Leave Application";
                                    var message = "";
                                    string fromDateNP = dateTableServices.ConvertToNepDate(vm.From);
                                    string toDateNP = dateTableServices.ConvertToNepDate(vm.To);
                                    var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                                    var requestCode = result.Data.Id;
                                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                                        "<div class='panel panel-success'>" +
                                                                    "<div class='panel-heading'>" +
                                                                        "<h2></h2>" +
                                                                    "</div>" +
                                                                     "<div class='panel-body'>" +
                                                                        "<p>" +
                                                                            "Dear " + manager.Name + ",</br>" +
                                                                            "<p>" + requestingEmployee.Name + "(" + requestingEmployee.Code + ") has requested leave (" + result.Data.LeaveMaster.Name + ") from " + vm.From.ToString("dd/MM/yyyy") + " (" + fromDateNP + ") " + " to " + vm.To.ToString("dd/MM/yyyy") + " (" + toDateNP + ") " + "</p>" +
                                                                            "<p>Leave Days: <b>" + ((vm.To - vm.From).Days + 1) + "</b></p>" +
                                                                            "<p><b>Employee Information</b></p>" +
                                                                            "<p>Designation: <b>" + requestingEmployee.Designation.Name + "</b></p>" +
                                                                            "<p>Department: <b>" + requestingEmployee.Section.Department.Name + "</b></p>" +
                                                                            "<p>Section: <b>" + requestingEmployee.Section.Name + "</b></p>" +
                                                                            "<p></p>" +
                                                                            "<p>Please Click at link to approve or reject the leave request</p>" +
                                                                            "<p>" +
                                                                            "   <a style='display: block;width: 115px;height: 25px;background: #5CB85C;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='" + baseUrl + "/EmailActivities/LeaveApprove?id=" + requestCode + "&fyid=" + currentFiscalYearId + "'>Approve</a>" +
                                                                            "   <a style='display: block;width: 115px;height: 25px;background: #F0AD4E;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='" + baseUrl + "/EmailActivities/LeaveReject?id=" + requestCode + "' >Reject</a>" +
                                                                            "</p>" +
                                                                        "</p>" +
                                                                    "</div>" +
                                                                "</div>", null, "text/html");
                                    try
                                    {
                                        new Thread(() =>
                                        {
                                            mail.SendMail(manager.Email, subject, message, htmlView);
                                        }).Start();
                                    }
                                    catch (Exception ex)
                                    {

                                        Log.Write(ex.Message);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new ServiceResult<ELeaveApplication>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        private bool validateDulicateLeaveOnSameDate(LeaveApplicationViewModel vm)
        {
            string fromDate = vm.From.ToString("yyyy/MM/dd");
            string toDate = vm.To.ToString("yyyy/MM/dd");

            var result = leaveAppicationServices.List().Data.
                Where(x => x.EmployeeId == vm.EmployeeId && x.LeaveStatus == LeaveStatus.Approve
                && ((DbFunctions.TruncateTime(vm.From) >= DbFunctions.TruncateTime(x.From)
                && DbFunctions.TruncateTime(vm.From) <= DbFunctions.TruncateTime(x.To)
                )
                || (DbFunctions.TruncateTime(vm.To) >= DbFunctions.TruncateTime(x.From)
                && DbFunctions.TruncateTime(vm.To) <= DbFunctions.TruncateTime(x.To)
                ))).ToList();
            return result.Count == 0 ? true : false;

        }

        [ActionFilter("3011")]
        public ServiceResult<ELeaveApplication> Put(ELeaveApplication model)
        {
            LocalizedString loc = new LocalizedString();
            int currentFiscalYearId = RiddhaSession.FYId;
            if (currentFiscalYearId == 0)
            {
                return new ServiceResult<ELeaveApplication>()
                {
                    Data = null,
                    Message = loc.Localize("FiscalYearNotSet"),
                    Status = ResultStatus.processError
                };
            }
            decimal remLeave = leaveAppicationServices.GetRemBal(model.LeaveMasterId, model.EmployeeId, currentFiscalYearId).Data;
            if (remLeave == 0)
            {
                return new ServiceResult<ELeaveApplication>()
                {
                    Data = null,
                    Message = loc.Localize("NoRemainingLeave"),
                    Status = ResultStatus.processError
                };
            }
            var result = leaveAppicationServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("3002", "3011", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ELeaveApplication>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [ActionFilter("3012")]
        public ServiceResult<int> Delete(int id)
        {
            var leaveAppication = leaveAppicationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (leaveAppication.LeaveStatus == LeaveStatus.New || leaveAppication.LeaveStatus == LeaveStatus.Reject)
            {
                var result = leaveAppicationServices.Remove(leaveAppication);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("3002", "3012", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
                }
                return new ServiceResult<int>()
                {
                    Data = result.Data,
                    Message = loc.Localize(result.Message),
                    Status = result.Status
                };
            }
            else
            {
                return new ServiceResult<int>()
                {
                    Message = "Approve leave cannot be deleted..",
                    Status = ResultStatus.processError
                };
            }
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetEmployeesForDropdown()
        {
            int? branchId = RiddhaSession.BranchId;
            string language = RiddhaSession.Language.ToString();
            SEmployee employeeServices = new SEmployee();
            List<DropdownViewModel> resultLst = new List<DropdownViewModel>();
            if (RiddhaSession.PackageId == 3)
            {
                var currentEmployee = employeeServices.List().Data.Where(x => x.UserId == RiddhaSession.UserId).FirstOrDefault();
                if (currentEmployee != null)
                {
                    if (currentEmployee.ReportingManagerId > 0)
                    {
                        resultLst = (from c in employeeServices.List().Data.Where(x => x.Id == currentEmployee.ReportingManagerId).ToList()
                                     select new DropdownViewModel()
                                     {
                                         Id = c.Id,
                                         Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                     }).ToList();
                    }
                    return new ServiceResult<List<DropdownViewModel>>()
                    {
                        Data = resultLst,
                        Message = "",
                        Status = ResultStatus.Ok
                    };
                }
                else
                {
                    resultLst = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager == true).ToList()
                                 select new DropdownViewModel()
                                 {
                                     Id = c.Id,
                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                 }).ToList();
                    return new ServiceResult<List<DropdownViewModel>>()
                    {
                        Data = resultLst,
                        Message = "",
                        Status = ResultStatus.Ok
                    };
                }

            }
            resultLst = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager == true).ToList()
                         select new DropdownViewModel()
                         {
                             Id = c.Id,
                             Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                         }
                                                   ).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetEmployeesForDropdown(int empId)
        {
            int? branchId = RiddhaSession.BranchId;
            string language = RiddhaSession.Language.ToString();
            SEmployee employeeServices = new SEmployee();
            List<DropdownViewModel> resultLst = new List<DropdownViewModel>();
            var emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
            if (emp != null)
            {
                if (emp.ReportingManagerId > 0)
                {
                    var reportingManager = employeeServices.List().Data.Where(x => x.Id == emp.ReportingManagerId).ToList();
                    resultLst = (from c in reportingManager
                                 select new DropdownViewModel()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     IsManager = c.IsManager,
                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                 }).ToList();

                    return new ServiceResult<List<DropdownViewModel>>()
                    {
                        Data = resultLst,
                        Message = "",
                        Status = ResultStatus.Ok
                    };
                }
                else
                {
                    resultLst = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager).ToList()
                                 select new DropdownViewModel()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     IsManager = c.IsManager,
                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                 }).ToList();
                }
            }
            else
            {
                resultLst = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager).ToList()
                             select new DropdownViewModel()
                             {
                                 Id = c.Id,
                                 Code = c.Code,
                                 IsManager = c.IsManager,
                                 Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                             }).ToList();
            }
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]

        public KendoGridResult<List<DropdownViewModel>> GetEmployeesForSearchGrid(EmployeeGridArgs args)
        {
            int? branchId = RiddhaSession.BranchId;
            string language = RiddhaSession.Language.ToString();
            SEmployee employeeServices = new SEmployee();
            List<DropdownViewModel> resultLst = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                                 where c.Name.ToLower().StartsWith(args.text)
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Code = c.Code,
                                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                                 }).ToList();
            return new KendoGridResult<List<DropdownViewModel>>()
            {
                Data = resultLst.Skip(args.Skip).Take(args.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = resultLst.Count
            };
        }
        //[HttpGet]
        //public ServiceResult<List<LeaveMasterDropdownVm>> GetDesigWiseLeave(int desigId)
        //{
        //    string language = RiddhaSession.Language.ToString();
        //    SDesignation desigServices = new SDesignation();
        //    //List<LeaveMasterDropdownVm> resultLst = (from c in desigServices.ListLeaveQouta().Data.Where(x => x.DesignationId == desigId && (x.IsPaidLeave || x.IsLeaveCarryable || x.IsReplacementLeave)).ToList()
        //    List<LeaveMasterDropdownVm> resultLst = (from c in desigServices.ListLeaveQouta().Data.Where(x => x.DesignationId == desigId).ToList()
        //                                             select new LeaveMasterDropdownVm()
        //                                             {
        //                                                 Id = c.LeaveId,
        //                                                 Name = language == "ne" && c.Leave.NameNp != null ? c.Leave.NameNp : c.Leave.Name,
        //                                                 IsLeaveCarryable = c.IsLeaveCarryable,
        //                                                 IsPaidLeave = c.IsPaidLeave
        //                                             }
        //                                           ).ToList();
        //    return new ServiceResult<List<LeaveMasterDropdownVm>>()
        //    {
        //        Data = resultLst,
        //        Status = ResultStatus.Ok
        //    };
        //}
        [HttpGet]
        public ServiceResult<List<LeaveMasterDropdownVm>> GetDesigAndEmploymentWiseLeave(int empId)
        {
            string language = RiddhaSession.Language.ToString();
            var employee = new SEmployee().List().Data.Where(x => x.Id == empId).FirstOrDefault();
            if (employee != null)
            {
                if (employee.Branch.Company.EmploymentStatusWiseLeave)
                {
                    SEmploymentStatusWiseLeavedBalance employmentStatusWiseLeavedBalanceServices = new SEmploymentStatusWiseLeavedBalance();
                    SContract contractServices = new SContract();
                    //TODO: validate multiple contract and contract date
                    var contract = contractServices.List().Data.Where(x => x.EmployeeId == empId).ToList();
                    if (contract.Count() != 0)
                    {
                        int employmentStatusId = contract.LastOrDefault().EmploymentStatusId;
                        List<LeaveMasterDropdownVm> resultLst = (from c in employmentStatusWiseLeavedBalanceServices.List().Data.Where(x => x.EmploymentStatusId == employmentStatusId).ToList()
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
                    }
                }
                else
                {
                    if (employee.DesignationId == null)
                    {
                        return new ServiceResult<List<LeaveMasterDropdownVm>>()
                        {
                            Data = null,
                            Message = loc.Localize("This employee does not have any designation. Please assign designation."),
                            Status = ResultStatus.processError,
                        };
                    }
                    SDesignation desigServices = new SDesignation();
                    List<LeaveMasterDropdownVm> resultLst = (from c in desigServices.ListLeaveQouta().Data.Where(x => x.DesignationId == employee.DesignationId).ToList()
                                                             select new LeaveMasterDropdownVm()
                                                             {
                                                                 Id = c.LeaveId,
                                                                 Name = language == "ne" && c.Leave.NameNp != null ? c.Leave.NameNp : c.Leave.Name,
                                                                 IsLeaveCarryable = c.IsLeaveCarryable,
                                                                 IsPaidLeave = c.IsPaidLeave
                                                             }
                                                           ).ToList();
                    return new ServiceResult<List<LeaveMasterDropdownVm>>()
                    {
                        Data = resultLst,
                        Status = ResultStatus.Ok
                    };
                }
            }
            return new ServiceResult<List<LeaveMasterDropdownVm>>()
            {
                Data = null,
                Message = "Process Error",
                Status = ResultStatus.processError
            };
        }
        [HttpGet]
        public ServiceResult<LeaveApplicationViewModel> Approve(int id)
        {
            string msg = "";
            var leaveApproval = leaveAppicationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (leaveApproval != null)
            {
                if (leaveApproval.LeaveStatus == LeaveStatus.Approve)
                {
                    return new ServiceResult<LeaveApplicationViewModel>()
                    {
                        Data = null,
                        Message = "This leave application is already Approved.",
                        Status = ResultStatus.processError
                    };
                }
                if (leaveApproval.LeaveStatus == LeaveStatus.Reject)
                {
                    return new ServiceResult<LeaveApplicationViewModel>()
                    {
                        Data = null,
                        Message = "This leave application is already rejected so can't Approve.",
                        Status = ResultStatus.processError
                    };
                }
                if (leaveApproval.LeaveStatus == LeaveStatus.New)
                {
                    leaveApproval.LeaveStatus = LeaveStatus.Approve;
                    leaveApproval.ApprovedOn = System.DateTime.Now;
                    var result = leaveAppicationServices.Update(leaveApproval);
                    if (result.Status == ResultStatus.Ok)
                    {
                        msg = "Approved Successfully";
                    }
                }
            }
            return new ServiceResult<LeaveApplicationViewModel>()
            {
                Data = null,
                Status = ResultStatus.Ok,
                Message = msg
            };
        }

        [HttpGet]
        public ServiceResult<LeaveApplicationViewModel> Reject(int id)
        {
            string msg = "";
            var leaveApproval = leaveAppicationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();

            if (leaveApproval != null)
            {
                if (leaveApproval.LeaveStatus == LeaveStatus.Approve)
                {
                    return new ServiceResult<LeaveApplicationViewModel>()
                    {
                        Data = null,
                        Status = ResultStatus.processError,
                        Message = "This leave application is already approved so can't Reject."
                    };
                }
                if (leaveApproval.LeaveStatus == LeaveStatus.Reject)
                {
                    return new ServiceResult<LeaveApplicationViewModel>()
                    {
                        Data = null,
                        Status = ResultStatus.processError,
                        Message = "This leave application is already Rejected."
                    };
                }

                if (leaveApproval.LeaveStatus == LeaveStatus.New)
                {
                    leaveApproval.LeaveStatus = LeaveStatus.Reject;
                    var result = leaveAppicationServices.Update(leaveApproval);
                    if (result.Status == ResultStatus.Ok)
                    {
                        msg = "Rejected Successfully";
                    }
                }
            }
            return new ServiceResult<LeaveApplicationViewModel>()
            {
                Data = null,
                Status = ResultStatus.Ok,
                Message = msg
            };
        }

        #region KenodGrid
        [HttpPost, ActionFilter("3026")]
        public KendoGridResult<List<LeaveApplicationGridVm>> GetLeaveApplicationKendoGrid(KendoPageListArguments vm)
        {
            var branchId = RiddhaSession.BranchId;
            var allEmp = employeeServices.List().Data.Where(x => x.BranchId == branchId);

            SLeaveApplication service = new SLeaveApplication();
            IQueryable<ELeaveApplication> leaveApplicationQuery = null;
            IQueryable<ELeaveApplication> paginatedQuery;
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            int count = 0;
            if (RiddhaSession.IsHeadOffice && RiddhaSession.DataVisibilityLevel == (int)DataVisibilityLevel.All)
            {
                leaveApplicationQuery = service.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).OrderByDescending(x => x.Id);
                count = service.List().Data.Where(x => x.BranchId == branchId).Count();
            }
            else
            {
                leaveApplicationQuery = service.List().Data.Where(x => x.BranchId == branchId).OrderByDescending(x => x.Id);
                count = service.List().Data.Where(x => x.BranchId == branchId).Count();
            }
            switch (searchField)
            {
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = leaveApplicationQuery.Where(x => x.Employee.Name.Trim().ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).Skip(vm.Skip).Take(vm.Take);
                        count = paginatedQuery.Count();
                    }
                    else
                    {
                        paginatedQuery = leaveApplicationQuery.Where(x => x.Employee.Name.Trim().ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "LeaveMaster":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = leaveApplicationQuery.Where(x => x.LeaveMaster.Name.Trim().ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).Skip(vm.Skip).Take(vm.Take);
                        count = paginatedQuery.Count();
                    }
                    else
                    {
                        paginatedQuery = leaveApplicationQuery.Where(x => x.LeaveMaster.Name.Trim().ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).Skip(vm.Skip).Take(vm.Take);
                        count = paginatedQuery.Count();
                    }
                    break;
                default:
                    paginatedQuery = leaveApplicationQuery.OrderByDescending(x => x.Id).Skip(vm.Skip).Take(vm.Take);
                    break;
            }
            var empQuery = Common.GetEmployees().Data;
            var leaveApplicationlist = (from c in paginatedQuery.ToList()
                                            //join d in employeeServices.List().Data.Where(x => x.BranchId == branchId) on c.ApprovedById equals d.Id
                                        join e in empQuery on c.EmployeeId equals e.Id
                                        select new LeaveApplicationGridVm()
                                        {
                                            Id = c.Id,
                                            EmployeeId = c.EmployeeId,
                                            EmployeeCode = e.Code,
                                            EmployeeName = c.Employee.Code + "-" + (language == "ne" && c.Employee.NameNp != null ? c.Employee.NameNp : c.Employee.Name),
                                            LeaveMaster = language == "ne" && c.LeaveMaster.NameNp != null ? c.LeaveMaster.NameNp : c.LeaveMaster.Name,
                                            LeaveMasterId = c.LeaveMasterId,
                                            From = c.From.ToString("yyyy/MM/dd"),
                                            To = c.To.ToString("yyyy/MM/dd"),
                                            LeaveDayName = Enum.GetName(typeof(LeaveDay), c.LeaveDay),
                                            LeaveDay = c.LeaveDay,
                                            Description = c.Description,
                                            ApprovedById = c.ApprovedById,
                                            LeaveStatusName = Enum.GetName(typeof(LeaveStatus), c.LeaveStatus),
                                            LeaveStatus = c.LeaveStatus,
                                            Days = (c.To - c.From).Days + 1,
                                            //ApprovedByUser = language == "ne" && d.NameNp != null ? d.NameNp : d.Name,
                                            ApprovedByUser = getapprovedByUser(allEmp, c.ApprovedById)
                                        }).ToList();
            return new KendoGridResult<List<LeaveApplicationGridVm>>()
            {
                Data = leaveApplicationlist.OrderByDescending(x => (x.LeaveStatus == LeaveStatus.New)).ThenByDescending(x => x.Id).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = count,
            };
        }

        private string getapprovedByUser(IQueryable<EEmployee> allEmp, int? approvedById)
        {
            if (approvedById == null)
            {
                return "";
            }
            else
            {
                var emp = allEmp.Where(x => x.Id == (int)approvedById).FirstOrDefault();
                return emp == null ? "" : language == "ne" && emp.NameNp != null ? emp.NameNp : emp.Name;
            }
        }
        #endregion

        [HttpPost]
        public ServiceResult<List<DropdownViewModel>> GetApproveByEmployeesForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            string language = RiddhaSession.Language.ToString();
            SEmployee employeeServices = new SEmployee();
            List<DropdownViewModel> resultLst = new List<DropdownViewModel>();
            string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
            IQueryable<EEmployee> empLst;
            if (RiddhaSession.PackageId == 3)
            {
                var currentEmployee = employeeServices.List().Data.Where(x => x.UserId == RiddhaSession.UserId).FirstOrDefault();
                if (currentEmployee != null)
                {
                    if (currentEmployee.ReportingManagerId > 0)
                    {
                        empLst = employeeServices.List().Data.Where(x => x.Id == currentEmployee.ReportingManagerId);
                        if (searchText == null || searchText == "___")
                        {
                            empLst = empLst.OrderBy(x => x.Name).Take(50);
                        }
                        else
                        {
                            empLst = empLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).OrderBy(x => x.Name).Take(50);
                        }
                        resultLst = (from c in empLst.ToList()
                                     select new DropdownViewModel()
                                     {
                                         Id = c.Id,
                                         Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                     }).ToList();
                    }
                    return new ServiceResult<List<DropdownViewModel>>()
                    {
                        Data = resultLst,
                        Message = "",
                        Status = ResultStatus.Ok
                    };
                }
                else
                {
                    empLst = employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager == true);
                    if (searchText == null || searchText == "___")
                    {
                        empLst = empLst.OrderBy(x => x.Name).Take(50);
                    }
                    else
                    {
                        empLst = empLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).OrderBy(x => x.Name).Take(50);
                    }
                    resultLst = (from c in empLst.ToList()
                                 select new DropdownViewModel()
                                 {
                                     Id = c.Id,
                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                 }).ToList();
                    return new ServiceResult<List<DropdownViewModel>>()
                    {
                        Data = resultLst,
                        Message = "",
                        Status = ResultStatus.Ok
                    };
                }
            }
            empLst = employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager == true);
            if (searchText == null || searchText == "___")
            {
                empLst = empLst.OrderBy(x => x.Name).Take(50);
            }
            else
            {
                empLst = empLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).OrderBy(x => x.Name).Take(50);
            }
            resultLst = (from c in empLst.ToList()
                         select new DropdownViewModel()
                         {
                             Id = c.Id,
                             Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                         }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
    }

    public class LeaveApplicationViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int LeaveMasterId { get; set; }
        public decimal LeaveCount { get; set; }
        public int? ApprovedById { get; set; }
        public LeaveDay LeaveDay { get; set; }
        public string Description { get; set; }

    }

    public class LeaveApplicationGridVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        //public int Days
        //{
        //    get
        //    {
        //        return (To - From).Days + 1;
        //    }
        //    set
        //    {
        //        this.Days = value;
        //    }
        //}
        public int Days { get; set; }
        public string ApprovedByUser { get; set; }
        public string LeaveMaster { get; set; }
        public int LeaveMasterId { get; set; }
        public int RemainingBalance { get; set; }
        public string Description { get; set; }
        public string LeaveDayName { get; set; }
        public LeaveDay LeaveDay { get; set; }
        public int? ApprovedById { get; set; }
        public LeaveStatus LeaveStatus { get; set; }

        public string LeaveStatusName { get; set; }
    }
    public class EmployeeGridArgs : KendoPageListArguments
    {
        public string text { get; set; }
    }
}
