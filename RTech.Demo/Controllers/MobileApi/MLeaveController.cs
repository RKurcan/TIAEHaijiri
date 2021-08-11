using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using static RTech.Demo.Utilities.WDMS;

namespace RTech.Demo.Controllers.MobileApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MLeaveController : MRootController
    {
        private SBranch _branchServices = null;
        private LocalizedString _loc = null;

        public MLeaveController()
        {
            _branchServices = new SBranch();
            _loc = new LocalizedString();
        }

        [HttpGet]
        public MobileResult<List<EMLeaveInfo>> GetLeaveInfo(int empId)
        {
            SDesignation desigServices = new SDesignation();
            var emp = new SEmployee().List().Data.Where(x => x.Id == empId).FirstOrDefault();
            var currentFiscalYear = new SFiscalYear().List().Data.Where(x => x.CompanyId == emp.Branch.CompanyId && x.CurrentFiscalYear).FirstOrDefault();
            if (currentFiscalYear == null)
            {
                return new MobileResult<List<EMLeaveInfo>>()
                {
                    Data = null,
                    Message = "Current fiscal year is not set. Please request you're admin to set current fiscal year.",
                    Status = MobileResultStatus.ProcessError,
                };
            }
            List<EMLeaveInfo> resultLst = (from c in desigServices.ListLeaveQouta().Data.Where(x => x.DesignationId == emp.DesignationId).ToList()
                                           select new EMLeaveInfo()
                                           {
                                               LeaveId = c.LeaveId,
                                               LeaveName = c.Leave.Name,
                                           }).ToList();

            SLeaveApplication leaveAppicationServices = new SLeaveApplication();
            foreach (var item in resultLst)
            {
                item.RemLeave = leaveAppicationServices.GetRemBal(item.LeaveId, empId, currentFiscalYear.Id).Data;
            }
            return new MobileResult<List<EMLeaveInfo>>()
            {
                Data = resultLst,
                Status = MobileResultStatus.Ok
            };
        }

        [HttpGet]
        public MobileResult<List<DropdownViewModel>> GetIsManager(int empId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                var emp = new SEmployee().List().Data.Where(x => x.Id == empId).FirstOrDefault();
                var result = (from c in new SEmployee().List().Data.Where(x => x.BranchId == emp.BranchId && x.IsManager).ToList()
                              select new DropdownViewModel()
                              {
                                  Id = c.Id,
                                  Name = c.Code + "-" + c.Name,
                              }).ToList();
                foreach (var item in result)
                {
                    if (item.Id == emp.ReportingManagerId)
                    {
                        item.IsManager = true;
                    }
                }
                return new MobileResult<List<DropdownViewModel>>()
                {
                    Data = result,
                    Message = "",
                    Status = MobileResultStatus.Ok,
                };
            }
            return InvalidTokeResult<List<DropdownViewModel>>();
        }


        [HttpPost]
        public MobileResult<EMLeaveRequest> RequestLeave(EMLeaveRequest model)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SLeaveSettlement leaveSettlementServices = new SLeaveSettlement();

                SLeaveApplication leaveAppicationServices = new SLeaveApplication();
                var emp = new SEmployee().List().Data.Where(x => x.Id == model.EmployeeId).FirstOrDefault();
                int currentFiscalYearId = new SFiscalYear().List().Data.Where(x => x.BranchId == emp.BranchId && x.CurrentFiscalYear).FirstOrDefault().Id;

                if (leaveSettlementServices.IsLeaveSettled(model.EmployeeId, model.LeaveMasterId, currentFiscalYearId).Data)
                {
                    return new MobileResult<EMLeaveRequest>()
                    {
                        Data = null,
                        Message = _loc.Localize("LeaveSettledAlready"),
                        Status = MobileResultStatus.ProcessError
                    };
                }
                if (currentFiscalYearId == 0)
                {
                    return new MobileResult<EMLeaveRequest>()
                    {
                        Data = null,
                        Message = _loc.Localize("FiscalYearNotSet"),
                        Status = MobileResultStatus.ProcessError
                    };
                }
                decimal remLeave = leaveAppicationServices.GetRemBal(model.LeaveMasterId, model.EmployeeId, currentFiscalYearId).Data;
                if (remLeave == 0)
                {
                    return new MobileResult<EMLeaveRequest>()
                    {
                        Data = null,
                        Message = _loc.Localize("NoRemainingLeave"),
                        Status = MobileResultStatus.ProcessError
                    };
                }
                ELeaveApplication leaveAppModel = new ELeaveApplication();
                leaveAppModel.EmployeeId = model.EmployeeId;
                leaveAppModel.BranchId = emp.BranchId;
                leaveAppModel.CreatedById = (int)emp.UserId;
                leaveAppModel.From = model.From;
                if (model.LeaveDay != LeaveDay.FullDay)
                {
                    leaveAppModel.To = model.From;
                }
                else
                {
                    leaveAppModel.To = model.To;
                }
                leaveAppModel.LeaveMasterId = model.LeaveMasterId;
                leaveAppModel.TransactionDate = DateTime.Now;
                leaveAppModel.ApprovedById = model.ApprovedById;
                leaveAppModel.LeaveDay = model.LeaveDay;
                leaveAppModel.Description = model.Description;
                leaveAppModel.LeaveStatus = LeaveStatus.New;
                EleaveApplicationLog log = new EleaveApplicationLog();
                log.FiscalYearId = currentFiscalYearId;
                log.LeaveCount = (model.To - model.From).Days + 1;
                var result = leaveAppicationServices.Add(leaveAppModel, log);
                if (result.Status == ResultStatus.Ok)
                {

                    decimal leaveApplyDay = (model.To - model.From).Days + 1;
                    var empQuery = new SEmployee().List().Data;
                    SNotification notificationServices = new SNotification();
                    int[] notificationTargets = (from c in empQuery.Where(x => x.BranchId == emp.BranchId && x.IsManager).ToList()
                                                 select c.Id).ToArray();
                    notificationServices.Add(new ENotification()
                    {
                        CompanyId = emp.Branch.CompanyId,
                        EffectiveDate = model.From,
                        ExpiryDate = model.To,
                        FiscalYearId = currentFiscalYearId,
                        NotificationLevel = NotificationLevel.Department,
                        NotificationType = NotificationType.LeaveRequest,
                        PublishDate = model.From,
                        TranDate = DateTime.Now,
                        TypeId = result.Data.Id,
                        Title = result.Data.LeaveMaster.Name + " has been requested by " + emp.Name + " for " + leaveApplyDay.ToString() + " days.",
                        Message = "This is to inform you that " + emp.Name + " has requested " + result.Data.LeaveMaster.Name + " for " + leaveApplyDay.ToString() + " days due to the reason of " + model.Description
                    }, notificationTargets);

                    if (emp.Branch.Company.SoftwarePackageType == SoftwarePackageType.Corporate)
                    {
                        new Thread(() =>
                        {
                            try
                            {
                                SendMailTemplateToManager(result.Data, currentFiscalYearId);
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                            }
                        }).Start();

                    }
                    return new MobileResult<EMLeaveRequest>()
                    {
                        Status = MobileResultStatus.Ok,
                        Message = "Thank you.Leave is requested successfully. We will notify you soon after your request is approved."
                    };
                }
                else
                {
                    return new MobileResult<EMLeaveRequest>()
                    {
                        Data = null,
                        Message = _loc.Localize(result.Message),
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            return InvalidTokeResult<EMLeaveRequest>();
        }

        private void SendMailTemplateToManager(ELeaveApplication data, int currentFiscalYearId)
        {
            SEmployee employeeServices = new SEmployee();
            SDateTable dateTableServices = new SDateTable();
            var requestingEmployee = employeeServices.List().Data.Where(x => x.Id == data.EmployeeId).FirstOrDefault();
            if (requestingEmployee.ReportingManagerId != null && requestingEmployee.ReportingManagerId != 0)
            {
                //prepare email template
                var manager = employeeServices.List().Data.Where(x => x.Id == requestingEmployee.ReportingManagerId).FirstOrDefault();
                if (!string.IsNullOrEmpty(manager.Email))
                {
                    var mail = new MailCommon();
                    var subject = "Leave Application";
                    var message = "";
                    string fromDateNP = dateTableServices.ConvertToNepDate(data.From);
                    string toDateNP = dateTableServices.ConvertToNepDate(data.To);
                    var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                    var requestCode = data.Id;
                    decimal leaveCount = (data.To - data.From).Days + 1;
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "<div class='panel panel-success'>" +
                                                    "<div class='panel-heading'>" +
                                                        "<h2></h2>" +
                                                    "</div>" +
                                                     "<div class='panel-body'>" +
                                                        "<p>" +
                                                            "Dear " + manager.Name + ",</br>" +
                                                            "<p>" + requestingEmployee.Name + "(" + requestingEmployee.Code + ") has requested leave (" + data.LeaveMaster.Name + ") from " + data.From.ToString("dd/MM/yyyy") + " (" + fromDateNP + ") " + " to " + data.To.ToString("dd/MM/yyyy") + " (" + toDateNP + ") " + "</p>" +
                                                            "<p>Leave Days: <b>" + leaveCount + "</b></p>" +
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
                    mail.SendMail(manager.Email, subject, message, htmlView);
                }
            }
        }
    }
    public class EMLeaveRequest
    {
        public int EmployeeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int LeaveMasterId { get; set; }
        public int LeaveStatus { get; set; }
        public int ApprovedById { get; set; }
        public LeaveDay LeaveDay { get; set; }
        public string Description { get; set; }
    }
}
