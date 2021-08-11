using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class LeaveApprovalApiController : ApiController
    {
        SLeaveApplication leaveAppicationServices = null;
        SNotification notificationServices = null;
        LocalizedString loc = null;
        SEmployee employeeServices = null;
        string language = RiddhaSession.Language;
        public LeaveApprovalApiController()
        {
            leaveAppicationServices = new SLeaveApplication();
            notificationServices = new SNotification();
            employeeServices = new SEmployee();
            loc = new LocalizedString();
        }
        [ActionFilter("3027")]
        public ServiceResult<List<LeaveApprovalViewModel>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SLeaveApplication service = new SLeaveApplication();
            var maxDate = DateTime.Today.AddDays(1);
            var applicationList = (from c in service.List().Data.Where(x => x.BranchId == branchId && x.LeaveStatus == LeaveStatus.New).ToList()
                                   select new LeaveApprovalViewModel()
                                   {
                                       Id = c.Id,
                                       EmpCode = c.Employee.Code,
                                       EmpName = c.Employee.Name,
                                       Photo = c.Employee.ImageUrl,
                                       Leave = c.LeaveMaster.Name,
                                       From = c.From.ToString("yyyy/MM/dd"),
                                       To = c.To.ToString("yyyy/MM/dd"),
                                       LeaveDay = Enum.GetName(typeof(LeaveDay), c.LeaveDay),
                                       Description = c.Description,
                                       LeaveStatusName = Enum.GetName(typeof(LeaveStatus), c.LeaveStatus),
                                       Department = c.Employee.Section.Department.Name,
                                       Section = c.Employee.Section.Name,
                                       Designation = c.Employee.Designation.Name,
                                       EmployeeId = c.EmployeeId,
                                       RemLeave = GetRemLeave(c.LeaveMasterId, c.EmployeeId),
                                       LeaveCount = (c.To - c.From).Days + 1,
                                       AdminRemark = c.AdminRemark,
                                       LeaveStatus = c.LeaveStatus,
                                   }).ToList();

            return new ServiceResult<List<LeaveApprovalViewModel>>()
            {
                Data = applicationList,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<LeaveApprovalViewModel> GetLeaveApprovalDetails(int leaveAppId)
        {
            SLeaveApplication service = new SLeaveApplication();
            var leaveApp = service.List().Data.Where(x => x.Id == leaveAppId).FirstOrDefault();
            var result = new LeaveApprovalViewModel()
            {
                Id = leaveApp.Id,
                EmpCode = leaveApp.Employee.Code,
                EmpName = leaveApp.Employee.Code + '-' + leaveApp.Employee.Name,
                Photo = leaveApp.Employee.ImageUrl,
                Leave = leaveApp.LeaveMaster.Name,
                From = leaveApp.From.ToString("yyyy/MM/dd"),
                To = leaveApp.To.ToString("yyyy/MM/dd"),
                LeaveDay = Enum.GetName(typeof(LeaveDay), leaveApp.LeaveDay),
                Description = leaveApp.Description,
                LeaveStatusName = Enum.GetName(typeof(LeaveStatus), leaveApp.LeaveStatus),
                LeaveStatus = leaveApp.LeaveStatus,
                Department = leaveApp.Employee.Section.Department.Name,
                Section = leaveApp.Employee.Section.Name,
                Designation = leaveApp.Employee.Designation.Name,
                EmployeeId = leaveApp.EmployeeId,
                RemLeave = GetRemLeave(leaveApp.LeaveMasterId, leaveApp.EmployeeId),
                LeaveCount = (leaveApp.To - leaveApp.From).Days + 1,
                AdminRemark = leaveApp.AdminRemark,
                ApprovedByUser = employeeServices.List().Data.Where(x => x.Id == leaveApp.ApprovedById).FirstOrDefault().Name,
            };
            return new ServiceResult<LeaveApprovalViewModel>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
        private decimal GetRemLeave(int leaveMastId, int empId)
        {
            int currentfiscalYearId = RiddhaSession.FYId;
            if (currentfiscalYearId == 0)
            {
                return 0;
            }
            SDesignation desigServices = new SDesignation();
            decimal remLeave = leaveAppicationServices.GetRemBal(leaveMastId, empId, currentfiscalYearId).Data;
            return remLeave;
        }
        [HttpGet, ActionFilter("3013")]
        public ServiceResult<LeaveApprovalViewModel> Approve(int id, string remarks)
        {
            int fyId = RiddhaSession.FYId;
            var companyId = RiddhaSession.CompanyId;
            int currentfiscalYearId = RiddhaSession.FYId;
            if (currentfiscalYearId == 0)
            {
                return new ServiceResult<LeaveApprovalViewModel>()
                {
                    Data = null,
                    Message = "Current fiscal year is not set. Please request you're admin to set current fiscal year.",
                    Status = ResultStatus.processError
                };
            }
            string msg = "";
            ResultStatus status = new ResultStatus();
            var leaveApproval = leaveAppicationServices.List().Data.Where(x => x.Id == id && x.LeaveStatus == LeaveStatus.New).FirstOrDefault();
            if (leaveApproval != null)
            {
                leaveApproval.LeaveStatus = LeaveStatus.Approve;
                leaveApproval.ApprovedOn = System.DateTime.Now;
                leaveApproval.AdminRemark = remarks;
                var result = leaveAppicationServices.Approve(leaveApproval, currentfiscalYearId);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("3003", "3013", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
                    ENotification notification = new ENotification()
                    {
                        CompanyId = companyId,
                        FiscalYearId = fyId,
                        EffectiveDate = leaveApproval.From,
                        ExpiryDate = leaveApproval.From,
                        Message = remarks == null || remarks == String.Empty ? (leaveApproval.LeaveMaster.Name + " that has been requested by " + leaveApproval.Employee.Name + " from " + leaveApproval.From.ToString("yyyy/MM/dd") + " to " + leaveApproval.To.ToString("yyyy/MM/dd") + " has been approved") : remarks,
                        NotificationLevel = NotificationLevel.Employee,
                        NotificationType = NotificationType.Leave,
                        PublishDate = leaveApproval.From.Date > DateTime.Now.Date ? leaveApproval.From.AddDays(-1) : DateTime.Now,
                        Title = " Leave Approved for " + leaveApproval.Employee.Name,
                        TranDate = DateTime.Now,
                        TypeId = leaveApproval.Id
                    };
                    int[] targets = new int[1];
                    targets[0] = leaveApproval.EmployeeId;
                    notificationServices.Add(notification, targets);
                    msg = loc.Localize(result.Message);
                    status = ResultStatus.Ok;
                }
            }
            else
            {
                status = ResultStatus.processError;
                msg = "Already approved";
            }
            return new ServiceResult<LeaveApprovalViewModel>()
            {
                Data = null,
                Status = status,
                Message = msg
            };
        }
        [HttpGet, ActionFilter("3014")]
        public ServiceResult<LeaveApprovalViewModel> Reject(int id, string remarks)
        {
            int fyId = RiddhaSession.FYId;
            var companyId = RiddhaSession.CompanyId;
            string msg = "";
            ResultStatus status = new ResultStatus();
            var leaveApproval = leaveAppicationServices.List().Data.Where(x => x.Id == id && x.LeaveStatus == LeaveStatus.New).FirstOrDefault();
            string empName = "";
            empName = leaveApproval.Employee.Name;
            if (leaveApproval != null)
            {
                leaveApproval.LeaveStatus = LeaveStatus.Reject;
                leaveApproval.ApprovedOn = DateTime.Now;
                leaveApproval.AdminRemark = remarks;
                var result = leaveAppicationServices.Reject(leaveApproval);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("3003", "3014", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
                    ENotification notification = new ENotification()
                    {
                        CompanyId = companyId,
                        FiscalYearId = fyId,
                        EffectiveDate = leaveApproval.From,
                        ExpiryDate = leaveApproval.From,
                        Message = remarks == null || remarks == String.Empty ? (leaveApproval.LeaveMaster.Name + " that has been requested by " + empName + " from " + leaveApproval.From.ToString("yyyy/MM/dd") + " to " + leaveApproval.To.ToString("yyyy/MM/dd") + " has been rejected") : remarks,
                        NotificationLevel = NotificationLevel.Employee,
                        NotificationType = NotificationType.Leave,
                        PublishDate = leaveApproval.From.Date < DateTime.Now.Date ? leaveApproval.From.AddDays(-1) : DateTime.Now,
                        Title = " Leave Rejected for " + empName,
                        TranDate = DateTime.Now,
                        TypeId = leaveApproval.Id
                    };
                    int[] targets = new int[1];
                    targets[0] = leaveApproval.EmployeeId;
                    notificationServices.Add(notification, targets);
                    msg = loc.Localize(result.Message);
                    status = ResultStatus.Ok;
                }
            }
            else
            {
                status = ResultStatus.processError;
                msg = "Leave already approved. Cannot Reject approved leave.";
            }
            return new ServiceResult<LeaveApprovalViewModel>()
            {
                Data = null,
                Status = status,
                Message = msg
            };
        }

        [HttpGet]
        public ServiceResult<LeaveApprovalViewModel> Revert(int id)
        {
            var leave = leaveAppicationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (leave != null)
            {
                var leavelog = leaveAppicationServices.ListLeaveAppLog().Data.Where(x => x.LeaveApplicationId == leave.Id).FirstOrDefault();
                var result = leaveAppicationServices.RemoveLeaveApplicationLog(leavelog);
                if (result.Status == ResultStatus.Ok)
                {
                    leave.Branch = null;
                    leave.CreatedBy = null;
                    leave.Employee = null;
                    leave.LeaveMaster = null;
                    leave.LeaveStatus = LeaveStatus.Revert;
                    leaveAppicationServices.Update(leave);
                };
                return new ServiceResult<LeaveApprovalViewModel>()
                {
                    Message = "Reverted Succesfully",
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<LeaveApprovalViewModel>()
            {
                Data = null,
                Message = "",
                Status = ResultStatus.processError
            };
        }

        #region Kendo Grid
        [HttpPost, ActionFilter("3027")]
        public KendoGridResult<List<LeaveApprovalViewModel>> GetLeaveApprovalKendoGrid(KendoPageListArguments arg)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            SLeaveApplication leaveApplcationService = new SLeaveApplication();
            var empQuery = Common.GetEmployees().Data;

            IQueryable<EEmployee> allEmp = employeeServices.List().Data.Where(x => x.BranchId == branchId);
            IQueryable<ELeaveApplication> leaveApplicationQuery = null;
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            int count = 0;
            IQueryable<ELeaveApplication> paginatedQuery;
            if (RiddhaSession.IsHeadOffice && RiddhaSession.DataVisibilityLevel == (int)DataVisibilityLevel.All)
            {
                leaveApplicationQuery = leaveApplcationService.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).OrderByDescending(x => x.Id);
                count = leaveApplcationService.List().Data.Where(x => x.BranchId == branchId).Count();
            }
            else
            {
                leaveApplicationQuery = leaveApplcationService.List().Data.Where(x => x.BranchId == branchId).OrderByDescending(x => x.Id);
                count = leaveApplcationService.List().Data.Where(x => x.BranchId == branchId).Count();
            }

            switch (searchField)
            {
                case "EmpName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = leaveApplicationQuery.Where(x => x.Employee.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).Skip(arg.Skip).Take(arg.Take);
                        count = paginatedQuery.Count();
                    }
                    else
                    {
                        paginatedQuery = leaveApplicationQuery.Where(x => x.Employee.Name == searchValue.Trim()).OrderByDescending(x => x.Id).Skip(arg.Skip).Take(arg.Take);
                        count = paginatedQuery.Count();
                    }
                    break;
                case "Leave":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = leaveApplicationQuery.Where(x => x.LeaveMaster.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).Skip(arg.Skip).Take(arg.Take);
                        count = paginatedQuery.Count();
                    }
                    else
                    {
                        paginatedQuery = leaveApplicationQuery.Where(x => x.LeaveMaster.Name == searchValue.Trim()).OrderByDescending(x => x.Id).Skip(arg.Skip).Take(arg.Take);
                        count = paginatedQuery.Count();
                    }
                    break;
                default:
                    paginatedQuery = leaveApplicationQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Id).Skip(arg.Skip).Take(arg.Take);
                    break;
            }
            var leaveApprovallist = (from c in paginatedQuery.ToList()
                                     join e in empQuery on c.EmployeeId equals e.Id
                                     select new LeaveApprovalViewModel()
                                     {
                                         Id = c.Id,
                                         EmpName = c.Employee.Code + '-' + (language == "ne" && c.Employee.NameNp != null ? c.Employee.NameNp : c.Employee.Name),
                                         Leave = language == "ne" && c.LeaveMaster.NameNp != null ? c.LeaveMaster.NameNp : c.LeaveMaster.Name,
                                         From = c.From.ToString("yyyy/MM/dd"),
                                         To = c.To.ToString("yyyy/MM/dd"),
                                         LeaveDayEnum = c.LeaveDay,
                                         LeaveStatusName = Enum.GetName(typeof(LeaveStatus), c.LeaveStatus),
                                         LeaveStatus = c.LeaveStatus,
                                         LeaveCount = (c.To - c.From).Days + 1,
                                         ApprovedByUser = getapprovedByUser(allEmp, c.ApprovedById),
                                     }).OrderBy(x => x.LeaveStatus).ToList();
            return new KendoGridResult<List<LeaveApprovalViewModel>>()
            {
                Data = leaveApprovallist.OrderByDescending(x => (x.LeaveStatus == LeaveStatus.New)).ThenByDescending(x => x.Id).ToList(),
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
    }
    public class LeaveApprovalViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Leave { get; set; }
        public string LeaveDay { get; set; }
        public LeaveDay LeaveDayEnum { get; set; }
        public decimal RemLeave { get; set; }
        public int LeaveCount { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Photo { get; set; }
        public string Designation { get; set; }
        public string AdminRemark { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Description { get; set; }
        public string LeaveStatusName { get; set; }
        public string ApprovedByUser { get; set; }
        public LeaveStatus LeaveStatus { get; set; }

    }
}
