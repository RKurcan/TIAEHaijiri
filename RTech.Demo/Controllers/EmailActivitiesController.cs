using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace RTech.Demo.Controllers
{
    public class EmailActivitiesController : Controller
    {
        #region Mail template


        string managerApprovedTemplate = "<div class='panel panel-success'>" +
                                                     "<div class='panel-heading'>" +
                                                         "<h2></h2>" +
                                                     "</div>" +
                                                      "<div class='panel-body'>" +
                                                         "<p>" +
                                                             "Dear {0},</br>" +
                                                             "<p>office Visit Request from {1} to {2} is {3} sucessfully.</p>" +
                                                             "<p></p>" +
                                                             "<p><b>Employee Information</b></p>" +
                                                             "{4}" +
                                                         "</p>" +
                                                     "</div>" +
                                                 "</div>";


        string OfficeVisitRequestManagerApprovedTemplate = "<div class='panel panel-success'>" +
                                                         "<div class='panel-heading'>" +
                                                             "<h2></h2>" +
                                                         "</div>" +
                                                          "<div class='panel-body'>" +
                                                             "<p>" +
                                                                 "Dear {0},</br>" +
                                                                 "<p>office Visit Request from {1} to {2} is {3} sucessfully.</p>" +
                                                                 "<p></p>" +
                                                                 "<p><b>Employee Information</b></p>" +
                                                                 "{4}" +
                                                             "</p>" +
                                                         "</div>" +
                                                     "</div>";

        string ManualPunchRequestManagerApprovedTemplate = "<div class='panel panel-success'>" +
                                                     "<div class='panel-heading'>" +
                                                         "<h2></h2>" +
                                                     "</div>" +
                                                      "<div class='panel-body'>" +
                                                         "<p>" +
                                                             "Dear {0},</br>" +
                                                             "<p>Manual Punch Request on {1} is {2} sucessfully.</p>" +
                                                             "<p></p>" +
                                                             "<p><b>Employee Information</b></p>" +
                                                             "{3}" +
                                                         "</p>" +
                                                     "</div>" +
                                                 "</div>";

        string ManualPunchEmployeeApprovedTemplate = "<div class='panel panel-success'>" +
                                                              "<div class='panel-heading'>" +
                                                                  "<h2></h2>" +
                                                              "</div>" +
                                                               "<div class='panel-body'>" +
                                                                  "<p>" +
                                                                      "Dear {0},</br>" +
                                                                      "<p>Manual Punch Request  on {1} is {2}</p>" +
                                                                      "<p></p>" +
                                                                  "</p>" +
                                                              "</div>" +
                                                          "</div>";

        string OfficeVisitManagerApprovedTemplate = "<div class='panel panel-success'>" +
                                                         "<div class='panel-heading'>" +
                                                             "<h2></h2>" +
                                                         "</div>" +
                                                          "<div class='panel-body'>" +
                                                             "<p>" +
                                                                 "Dear {0},</br>" +
                                                                 "<p>office Visit  from {1} to {2} is {3} sucessfully.</p>" +
                                                                 "<p></p>" +
                                                                 "<p><b>Employee Information</b></p>" +
                                                                 "{4}" +
                                                             "</p>" +
                                                         "</div>" +
                                                     "</div>";

        string OfficeVisitEmployeeApprovedTemplate = "<div class='panel panel-success'>" +
                                                              "<div class='panel-heading'>" +
                                                                  "<h2></h2>" +
                                                              "</div>" +
                                                               "<div class='panel-body'>" +
                                                                  "<p>" +
                                                                      "Dear {0},</br>" +
                                                                      "<p>office Visit  from {1} to {2} is {3}</p>" +
                                                                      "<p></p>" +
                                                                  "</p>" +
                                                              "</div>" +
                                                          "</div>";

        string EmployeeInformationTemplate = "<p>{0}-{1}<br/>{2}-{3}</p>";
        #endregion

        // GET: EmailActivities
        public ActionResult Index(string title = "", string message = "")
        {
            TempData["Title"] = title;
            TempData["Message"] = message;
            return View();
        }
        public ActionResult LeaveReject(string id)
        {
            int leaveApplicationId = id.ToInt();//.Decrypt().ToInt();
            if (leaveApplicationId == 0)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Leave Reject", message = "Process error. Please contact your administrator" }));
            }
            SLeaveApplication leaveApplicationService = new SLeaveApplication();
            var leaveApplicationModel = leaveApplicationService.List().Data.Where(x => x.Id == leaveApplicationId).FirstOrDefault();
            if (leaveApplicationModel == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Not Found", message = "Process error. This leave has been expired or removed." }));
            }
            leaveApplicationModel.LeaveStatus = Riddhasoft.Employee.Entities.LeaveStatus.Reject;
            var result = leaveApplicationService.Reject(leaveApplicationModel);
            if (result.Status == ResultStatus.Ok)
            {

                //get receiver's email i.e reporting manager
                var requestingEmployee = leaveApplicationModel.Employee;
                if (requestingEmployee.ReportingManagerId != null)
                {
                    //prepare email template
                    SEmployee employeeServices = new SEmployee();
                    var manager = employeeServices.List().Data.Where(x => x.Id == requestingEmployee.ReportingManagerId).FirstOrDefault();
                    var mail = new MailCommon();
                    var subject = "Leave Rejected";
                    var message = "";

                    AlternateView htmlViewForManager = AlternateView.CreateAlternateViewFromString(
                        "<div class='panel panel-success'>" +
                                                    "<div class='panel-heading'>" +
                                                        "<h2></h2>" +
                                                    "</div>" +
                                                     "<div class='panel-body'>" +
                                                        "<p>" +
                                                            "Dear " + manager.Name + ",</br>" +
                                                            "<p>The leave (" + leaveApplicationModel.LeaveMaster.Name + ") requested by " + requestingEmployee.Name + "(" + requestingEmployee.Code + ") from " + leaveApplicationModel.From.ToString("dd/MM/yyyy") + " to " + leaveApplicationModel.To.ToString("dd/MM/yyyy") + " is rejected.</p>" +
                                                            "<p><b>Employee Information</b></p>" +
                                                            "<p>Designation: <b>" + requestingEmployee.Designation.Name + "</b></p>" +
                                                            "<p>Department: <b>" + requestingEmployee.Section.Department.Name + "</b></p>" +
                                                            "<p>Section: <b>" + requestingEmployee.Section.Name + "</b></p>" +
                                                            "<p></p>" +
                                                        "</p>" +
                                                    "</div>" +
                                                "</div>", null, "text/html");

                    //htmlView.LinkedResources.Add(LinkedImage);

                    AlternateView htmlViewForEmployee = AlternateView.CreateAlternateViewFromString(
                        "<div class='panel panel-success'>" +
                                                    "<div class='panel-heading'>" +
                                                        "<h2></h2>" +
                                                    "</div>" +
                                                     "<div class='panel-body'>" +
                                                        "<p>" +
                                                            "Dear " + requestingEmployee.Name + ",</br>" +
                                                            "<p>The leave (" + leaveApplicationModel.LeaveMaster.Name + ") request from " + leaveApplicationModel.From.ToString("dd/MM/yyyy") + " to " + leaveApplicationModel.To.ToString("dd/MM/yyyy") + " is rejected.</p>" +

                                                        "</p>" +
                                                    "</div>" +
                                                "</div>", null, "text/html");

                    new Thread(() =>
                    {
                        if (!string.IsNullOrEmpty(manager.Email))
                        {
                            mail.SendMail(manager.Email, subject, message, htmlViewForManager);
                        }
                        if (!string.IsNullOrEmpty(requestingEmployee.Email))
                        {
                            mail.SendMail(requestingEmployee.Email, subject, message, htmlViewForEmployee);
                        }
                    }).Start();

                    return RedirectToAction("Index", new RouteValueDictionary(
                    new { controller = "EmailActivities", action = "Index", title = "Leave Reject", message = "Leave application reject sucessfully." }));
                }
            }
            return RedirectToAction("Index", new RouteValueDictionary(
            new { controller = "EmailActivities", action = "Index", title = "Leave Reject", message = "Process error. Please contact your administrator" }));
        }

        public ActionResult LeaveApprove(string id, int fyid)
        {
            int leaveApplicationId = id.ToInt();//.Decrypt().ToInt();
            if (leaveApplicationId == 0)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Leave Approval", message = "Process error. Please contact your administrator" }));
            }
            SLeaveApplication leaveApplicationService = new SLeaveApplication();
            var leaveApplicationModel = leaveApplicationService.List().Data.Where(x => x.Id == leaveApplicationId).FirstOrDefault();
            if (leaveApplicationModel == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Not Found", message = "Process error. This leave has been expired or removed." }));
            }
            if (leaveApplicationModel.LeaveStatus > 0)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Leave Application", message = "Leave Application is already approved or reject." }));
            }
            var result = leaveApplicationService.Approve(leaveApplicationModel, fyid);
            if (result.Status == ResultStatus.Ok)
            {
                #region notification added by raz
                SNotification notificationServices = new SNotification();
                ENotification notification = new ENotification()
                {
                    CompanyId = leaveApplicationModel.Employee.Branch.CompanyId,
                    FiscalYearId = fyid,
                    EffectiveDate = leaveApplicationModel.From,
                    ExpiryDate = leaveApplicationModel.From,
                    Message = leaveApplicationModel.LeaveMaster.Name + " that has been requested by " + leaveApplicationModel.Employee.Name + " from " + leaveApplicationModel.From.ToString("yyyy/MM/dd") + " to " + leaveApplicationModel.To.ToString("yyyy/MM/dd") + " has been approved",
                    NotificationLevel = NotificationLevel.Employee,
                    NotificationType = NotificationType.Leave,
                    PublishDate = leaveApplicationModel.From.Date > DateTime.Now.Date ? leaveApplicationModel.From.AddDays(-1) : DateTime.Now,
                    Title = " Leave Approved for " + leaveApplicationModel.Employee.Name,
                    TranDate = DateTime.Now,
                    TypeId = leaveApplicationModel.Id
                };
                int[] targets = new int[1];
                targets[0] = leaveApplicationModel.EmployeeId;
                notificationServices.Add(notification, targets);
                #endregion

                SEmployee empServices = new SEmployee();
                leaveApplicationModel.LeaveStatus = Riddhasoft.Employee.Entities.LeaveStatus.Approve;
                leaveApplicationModel.ApprovedOn = DateTime.Now;
                string approveUserName = empServices.List().Data.Where(x => x.Id == leaveApplicationModel.Employee.ReportingManagerId).FirstOrDefault().Name;
                leaveApplicationService.Update(leaveApplicationModel);

                var requestingEmployee = leaveApplicationModel.Employee;

                if (requestingEmployee.ReportingManagerId != null)
                {
                    //prepare email template
                    SEmployee employeeServices = new SEmployee();
                    var manager = employeeServices.List().Data.Where(x => x.Id == requestingEmployee.ReportingManagerId).FirstOrDefault();
                    var mail = new MailCommon();
                    var subject = "Leave Approved";
                    var message = "";
                    AlternateView htmlViewForManager = AlternateView.CreateAlternateViewFromString(
                        "<div class='panel panel-success'>" +
                                                    "<div class='panel-heading'>" +
                                                        "<h2></h2>" +
                                                    "</div>" +
                                                     "<div class='panel-body'>" +
                                                        "<p>" +
                                                            "Dear " + manager.Name + ",</br>" +
                                                            "<p>The leave (" + leaveApplicationModel.LeaveMaster.Name + ") requested by " + requestingEmployee.Name + "(" + requestingEmployee.Code + ") from " + leaveApplicationModel.From.ToString("dd/MM/yyyy") + " to " + leaveApplicationModel.To.ToString("dd/MM/yyyy") + " is approved.</p>" +
                                                            "<p><b>Employee Information</b></p>" +
                                                            "<p>Designation: <b>" + requestingEmployee.Designation.Name + "</b></p>" +
                                                            "<p>Department: <b>" + requestingEmployee.Section.Department.Name + "</b></p>" +
                                                            "<p>Section: <b>" + requestingEmployee.Section.Name + "</b></p>" +
                                                            "<p></p>" +
                                                        "</p>" +
                                                    "</div>" +
                                                "</div>", null, "text/html");
                    AlternateView htmlViewForEmployee = AlternateView.CreateAlternateViewFromString(
                        "<div class='panel panel-success'>" +
                                                    "<div class='panel-heading'>" +
                                                        "<h2></h2>" +
                                                    "</div>" +
                                                     "<div class='panel-body'>" +
                                                        "<p>" +
                                                            "Dear " + requestingEmployee.Name + ",</br>" +
                                                            "<p>The leave (" + leaveApplicationModel.LeaveMaster.Name + ") request from " + leaveApplicationModel.From.ToString("dd/MM/yyyy") + " to " + leaveApplicationModel.To.ToString("dd/MM/yyyy") + " is approved by " + approveUserName + " </p>" +
                                                        "</p>" +
                                                    "</div>" +
                                                "</div>", null, "text/html");

                    new Thread(() =>
                    {
                        if (!string.IsNullOrEmpty(manager.Email))
                        {
                            mail.SendMail(manager.Email, subject, message, htmlViewForManager);
                        }
                        if (!string.IsNullOrEmpty(requestingEmployee.Email))
                        {
                            mail.SendMail(requestingEmployee.Email, subject, message, htmlViewForEmployee);
                        }
                    }).Start();
                    return RedirectToAction("Index", new RouteValueDictionary(
                   new { controller = "EmailActivities", action = "Index", title = "Leave Approval", message = "Leave application approved sucessfully." }));

                }
            }
            return RedirectToAction("Index", new RouteValueDictionary(
            new { controller = "EmailActivities", action = "Index", title = "Leave Approval", message = "Process error. Please contact your administrator" }));
        }

        public ActionResult OfficeVisitApprove(int id, int managerId)
        {
            SOfficeVisit officeVisitServices = new SOfficeVisit();
            SEmployee employeeServices = new SEmployee();
            var officeVisit = officeVisitServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (officeVisit == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Not Found", message = "Process error. This officevisit has been expired or removed." }));
            }
            var manager = employeeServices.List().Data.Where(x => x.Id == managerId).FirstOrDefault();
            if (officeVisit.IsApprove)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Process error.", message = "Office visit request is already approved." }));
            }
            officeVisit.IsApprove = true;
            officeVisit.ApprovedOn = DateTime.Now;
            officeVisit.ApprovedById = manager.UserId;
            officeVisit.OfficeVisitStatus = OfficeVisitStatus.Approve;
            var result = officeVisitServices.Approve(officeVisit);
            if (result.Status == ResultStatus.Ok)
            {
                var officeVisitDetails = officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisitId == id).ToList();
                string participants = "";
                foreach (var item in officeVisitDetails)
                {
                    participants += string.Format(EmployeeInformationTemplate, item.Employee.Code, item.Employee.Name, item.Employee.Designation.Name, item.Employee.Section.Department.Name);
                }
                var finalTemplate = string.Format(OfficeVisitManagerApprovedTemplate, manager.Name, officeVisit.From, officeVisit.To, "Approved", participants);
                var mail = new MailCommon();
                string subject = "Office Visit Approved";
                AlternateView htmlViewForManager = AlternateView.CreateAlternateViewFromString(finalTemplate, null, "text/html");
                if (!string.IsNullOrEmpty(manager.Email))
                {
                    new Thread(() =>
                    {
                        mail.SendMail(manager.Email, subject, "", htmlViewForManager);
                    }).Start();
                }
                foreach (var item in officeVisitDetails)
                {
                    var finalTemplateForEmployee = string.Format(OfficeVisitEmployeeApprovedTemplate, item.Employee.Name, officeVisit.From, officeVisit.To, "approved.");
                    AlternateView htmlViewForEmployee = AlternateView.CreateAlternateViewFromString(finalTemplateForEmployee, null, "text/html");
                    if (!string.IsNullOrEmpty(item.Employee.Email))
                    {
                        new Thread(() =>
                        {
                            mail.SendMail(item.Employee.Email, subject, "", htmlViewForEmployee);
                        }).Start();
                    }
                }
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Office Visit", message = "Office Visit approved sucessfully." }));
            }
            return RedirectToAction("Index", new RouteValueDictionary(
           new { controller = "EmailActivities", action = "Index", title = "Office Visit", message = "Process error. Please contact your administrator" }));
        }

        public ActionResult OfficeVisitReject(int id, int managerId)
        {
            SOfficeVisit officeVisitServices = new SOfficeVisit();
            var officeVisit = officeVisitServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (officeVisit == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Not Found", message = "Process error. This officevisit has been expired or removed." }));
            }
            officeVisit.OfficeVisitStatus = OfficeVisitStatus.Reject;
            var result = officeVisitServices.Reject(officeVisit);
            if (result.Status == ResultStatus.Ok)
            {
                SEmployee employeeServices = new SEmployee();
                var officeVisitDetails = officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisitId == id).ToList();
                string participants = "";
                foreach (var item in officeVisitDetails)
                {
                    participants += string.Format(EmployeeInformationTemplate, item.Employee.Code, item.Employee.Name, item.Employee.Designation.Name, item.Employee.Section.Department.Name);
                }
                var manager = employeeServices.List().Data.Where(x => x.Id == managerId).FirstOrDefault();
                var finalTemplate = string.Format(OfficeVisitManagerApprovedTemplate, manager.Name, officeVisit.From, officeVisit.To, "Rejected", participants);
                var mail = new MailCommon();
                string subject = "Office Visit Rejected";
                AlternateView htmlViewForManager = AlternateView.CreateAlternateViewFromString(finalTemplate, null, "text/html");
                if (!string.IsNullOrEmpty(manager.Email))
                {
                    new Thread(() =>
                    {
                        mail.SendMail(manager.Email, subject, "", htmlViewForManager);
                    }).Start();
                }
                foreach (var item in officeVisitDetails)
                {
                    var finalTemplateForEmployee = string.Format(OfficeVisitEmployeeApprovedTemplate, item.Employee.Name, officeVisit.From, officeVisit.To, "rejected.");
                    AlternateView htmlViewForEmployee = AlternateView.CreateAlternateViewFromString(finalTemplateForEmployee, null, "text/html");
                    if (!string.IsNullOrEmpty(item.Employee.Email))
                    {
                        mail.SendMail(item.Employee.Email, subject, "", htmlViewForEmployee);
                    }
                }
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Office Visit", message = "Office Visit rejected sucessfully." }));
            }
            return RedirectToAction("Index", new RouteValueDictionary(
          new { controller = "EmailActivities", action = "Index", title = "Office Visit", message = "Process error. Please contact your administrator" }));
        }

        public ActionResult OfficeVisitRequestApprove(int id, int managerId)
        {
            SOfficeVisitRequest _officeVisitRequestServices = new SOfficeVisitRequest();
            SEmployee employeeServices = new SEmployee();
            var officeVisitRequest = _officeVisitRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (officeVisitRequest == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Not Found", message = "Process error. This officevisit has been expired or removed." }));
            }
            if (officeVisitRequest.IsApprove)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Process error.", message = "Office visit request is already approved." }));
            }
            var manager = employeeServices.List().Data.Where(x => x.Id == managerId).FirstOrDefault();
            int currentFiscalYearId = new SFiscalYear().List().Data.Where(x => x.BranchId == manager.BranchId && x.CurrentFiscalYear).FirstOrDefault().Id;
            if (officeVisitRequest != null)
            {
                officeVisitRequest.IsApprove = true;
                officeVisitRequest.ApprovedById = manager.UserId;
                officeVisitRequest.ApprovedOn = DateTime.Now;
                var result = _officeVisitRequestServices.Update(officeVisitRequest);
                if (result.Status == ResultStatus.Ok)
                {
                    EOfficeVisit officeVisit = new EOfficeVisit()
                    {
                        ApprovedById = result.Data.ApprovedById,
                        ApprovedOn = result.Data.ApprovedOn,
                        BranchId = result.Data.BranchId,
                        From = result.Data.From,
                        IsApprove = result.Data.IsApprove,
                        OfficeVisitStatus = OfficeVisitStatus.Approve,
                        Remark = result.Data.Remark,
                        To = result.Data.To,
                    };
                    SOfficeVisit _officeVisitServices = new SOfficeVisit();
                    var officeVisitResult = _officeVisitServices.Add(officeVisit);
                    if (officeVisitResult.Status == ResultStatus.Ok)
                    {
                        EOfficeVisitDetail officeVisitDetail = new EOfficeVisitDetail()
                        {
                            EmployeeId = result.Data.EmployeeId,
                            OfficeVisitId = officeVisitResult.Data.Id,
                        };
                        var officeVisitDetailResult = _officeVisitServices.AddDetail(officeVisitDetail);
                        if (officeVisitDetailResult.Status == ResultStatus.Ok)
                        {
                            #region Notification
                            SNotification notificationServices = new SNotification();
                            var empQuery = employeeServices.List().Data.Where(x => x.BranchId == officeVisitDetailResult.Data.Employee.BranchId);
                            int[] notificationTargets = Common.intToArray(officeVisitDetailResult.Data.EmployeeId);
                            int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                            DateTime expiryDate = result.Data.ApprovedOn.Value.AddDays(notificationExpiryDays);
                            notificationServices.Add(new ENotification()
                            {
                                CompanyId = officeVisitRequest.Branch.CompanyId,
                                EffectiveDate = officeVisitResult.Data.ApprovedOn.ToDateTime(),
                                ExpiryDate = expiryDate,
                                FiscalYearId = currentFiscalYearId,
                                NotificationLevel = NotificationLevel.Employee,
                                NotificationType = NotificationType.OfficeVisit,
                                PublishDate = officeVisitResult.Data.ApprovedOn.ToDateTime(),
                                TranDate = DateTime.Now,
                                TypeId = result.Data.Id,
                                Title = "Office Visit request has been approved.",
                                Message = "This is to inform you that you're office visit request on " + result.Data.From.ToString("yyyy/MM/dd") + " - " + result.Data.From.ToString(@"hh\:mm") + " To " + result.Data.To.ToString("yyyy/MM/dd") + " - " + result.Data.To.ToString(@"hh\:mm") + " due to the reason of " + result.Data.Remark + " has been approved by " + manager.Name,
                            }, notificationTargets);
                            #endregion
                            #region Email
                            var officeVisitDetails = _officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisitId == officeVisitDetailResult.Data.OfficeVisitId).ToList();
                            string participants = "";
                            foreach (var item in officeVisitDetails)
                            {
                                var emp = employeeServices.List().Data.Where(x => x.Id == item.EmployeeId).FirstOrDefault();
                                participants += string.Format(EmployeeInformationTemplate, emp.Code, emp.Name, emp.Designation.Name, emp.Section == null ? "" : emp.Section.Department.Name);
                            }
                            var finalTemplate = string.Format(OfficeVisitRequestManagerApprovedTemplate, manager.Name, officeVisit.From, officeVisit.To, "Approved", participants);
                            var mail = new MailCommon();
                            string subject = "Office Visit Request Approved";
                            AlternateView htmlViewForManager = AlternateView.CreateAlternateViewFromString(finalTemplate, null, "text/html");
                            if (!string.IsNullOrEmpty(manager.Email))
                            {
                                mail.SendMail(manager.Email, subject, "", htmlViewForManager);
                            }
                            foreach (var item in officeVisitDetails)
                            {
                                var emp = employeeServices.List().Data.Where(x => x.Id == item.EmployeeId).FirstOrDefault();
                                var finalTemplateForEmployee = string.Format(OfficeVisitEmployeeApprovedTemplate, emp.Name, officeVisit.From, officeVisit.To, "approved.");
                                AlternateView htmlViewForEmployee = AlternateView.CreateAlternateViewFromString(finalTemplateForEmployee, null, "text/html");
                                if (!string.IsNullOrEmpty(emp.Email))
                                {
                                    mail.SendMail(emp.Email, subject, "", htmlViewForEmployee);
                                }
                            }
                            return RedirectToAction("Index", new RouteValueDictionary(
                            new { controller = "EmailActivities", action = "Index", title = "Office Visit Request", message = "Office visit request approved sucessfully." }));
                            #endregion
                        }
                    }
                }
            }
            return RedirectToAction("Index", new RouteValueDictionary(
            new { controller = "EmailActivities", action = "Index", title = "Office Visit", message = "Process error. Please contact your administrator" }));
        }

        public ActionResult OfficeVisitRequestReject(int id, int managerId)
        {
            SOfficeVisitRequest _officeVisitRequestServices = new SOfficeVisitRequest();
            SEmployee employeeServices = new SEmployee();
            var officeVisitRequest = _officeVisitRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (officeVisitRequest == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Not Found", message = "Process error. This officevisit has been expired or removed." }));
            }
            if (officeVisitRequest.IsApprove)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Process error.", message = "Office visit request is already approved." }));
            }
            var manager = employeeServices.List().Data.Where(x => x.Id == managerId).FirstOrDefault();
            int currentFiscalYearId = new SFiscalYear().List().Data.Where(x => x.BranchId == manager.BranchId && x.CurrentFiscalYear).FirstOrDefault().Id;
            if (officeVisitRequest != null)
            {
                officeVisitRequest.IsApprove = false;
                officeVisitRequest.ApprovedById = null;
                officeVisitRequest.ApprovedOn = null;
                var result = _officeVisitRequestServices.Update(officeVisitRequest);
                if (result.Status == ResultStatus.Ok)
                {
                    EOfficeVisit officeVisit = new EOfficeVisit()
                    {
                        ApprovedById = result.Data.ApprovedById,
                        ApprovedOn = result.Data.ApprovedOn,
                        BranchId = result.Data.BranchId,
                        From = result.Data.From,
                        IsApprove = false,
                        OfficeVisitStatus = OfficeVisitStatus.Reject,
                        Remark = result.Data.Remark,
                        To = result.Data.To,
                    };
                    SOfficeVisit _officeVisitServices = new SOfficeVisit();
                    var officeVisitResult = _officeVisitServices.Add(officeVisit);
                    if (officeVisitResult.Status == ResultStatus.Ok)
                    {
                        EOfficeVisitDetail officeVisitDetail = new EOfficeVisitDetail()
                        {
                            EmployeeId = result.Data.EmployeeId,
                            OfficeVisitId = officeVisitResult.Data.Id,
                        };
                        var officeVisitDetailResult = _officeVisitServices.AddDetail(officeVisitDetail);
                        if (officeVisitDetailResult.Status == ResultStatus.Ok)
                        {
                            #region Notification
                            SNotification notificationServices = new SNotification();
                            var empQuery = employeeServices.List().Data.Where(x => x.BranchId == officeVisitDetailResult.Data.Employee.BranchId);
                            int[] notificationTargets = Common.intToArray(officeVisitDetailResult.Data.EmployeeId);
                            int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                            DateTime expiryDate = result.Data.ApprovedOn.Value.AddDays(notificationExpiryDays);
                            notificationServices.Add(new ENotification()
                            {
                                CompanyId = officeVisitRequest.Branch.CompanyId,
                                EffectiveDate = officeVisitResult.Data.ApprovedOn.ToDateTime(),
                                ExpiryDate = expiryDate,
                                FiscalYearId = currentFiscalYearId,
                                NotificationLevel = NotificationLevel.Employee,
                                NotificationType = NotificationType.OfficeVisit,
                                PublishDate = officeVisitResult.Data.ApprovedOn.ToDateTime(),
                                TranDate = DateTime.Now,
                                TypeId = result.Data.Id,
                                Title = "Office Visit request has been rejected.",
                                Message = "This is to inform you that you're office visit request on " + result.Data.From.ToString("yyyy/MM/dd") + " - " + result.Data.From.ToString(@"hh\:mm") + " To " + result.Data.To.ToString("yyyy/MM/dd") + " - " + result.Data.To.ToString(@"hh\:mm") + " due to the reason of " + result.Data.Remark + " has been rejected by " + manager.Name,
                            }, notificationTargets);
                            #endregion
                            #region Email
                            var officeVisitDetails = _officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisitId == officeVisitDetailResult.Data.OfficeVisitId).ToList();
                            string participants = "";
                            foreach (var item in officeVisitDetails)
                            {
                                var emp = employeeServices.List().Data.Where(x => x.Id == item.EmployeeId).FirstOrDefault();
                                participants += string.Format(EmployeeInformationTemplate, emp.Code, emp.Name, emp.Designation.Name, emp.Section == null ? "" : emp.Section.Department.Name);
                            }
                            var finalTemplate = string.Format(OfficeVisitRequestManagerApprovedTemplate, manager.Name, officeVisit.From, officeVisit.To, "Rejected", participants);
                            var mail = new MailCommon();
                            string subject = "Office Visit Request Rejected";
                            AlternateView htmlViewForManager = AlternateView.CreateAlternateViewFromString(finalTemplate, null, "text/html");
                            if (!string.IsNullOrEmpty(manager.Email))
                            {
                                mail.SendMail(manager.Email, subject, "", htmlViewForManager);
                            }
                            foreach (var item in officeVisitDetails)
                            {
                                var emp = employeeServices.List().Data.Where(x => x.Id == item.EmployeeId).FirstOrDefault();
                                var finalTemplateForEmployee = string.Format(OfficeVisitEmployeeApprovedTemplate, emp.Name, officeVisit.From, officeVisit.To, "approved.");
                                AlternateView htmlViewForEmployee = AlternateView.CreateAlternateViewFromString(finalTemplateForEmployee, null, "text/html");
                                if (!string.IsNullOrEmpty(emp.Email))
                                {
                                    mail.SendMail(emp.Email, subject, "", htmlViewForEmployee);
                                }
                            }
                            return RedirectToAction("Index", new RouteValueDictionary(
                            new { controller = "EmailActivities", action = "Index", title = "Office Visit Request", message = "Office visit request rejected sucessfully." }));
                            #endregion
                        }
                    }
                }
            }
            return RedirectToAction("Index", new RouteValueDictionary(
            new { controller = "EmailActivities", action = "Index", title = "Office Visit", message = "Process error. Please contact your administrator" }));
        }
        public ActionResult ManualPunchRequestApprove(int id, int managerId)
        {
            SManualPunch manualPunchServices = new SManualPunch();
            SManualPunchRequest _manualPunchRequestServices = new SManualPunchRequest();
            SEmployee employeeServices = new SEmployee();
            var manualPunchRequest = _manualPunchRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (manualPunchRequest == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Not Found", message = "Process error. This manual punch request has been expired or removed." }));
            }
            if (manualPunchRequest.IsApproved)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Process error.", message = "Manual punch request is already approved." }));
            }
            var emp = new SEmployee().List().Data.Where(x => x.Id == manualPunchRequest.EmployeeId).FirstOrDefault();
            var manager = employeeServices.List().Data.Where(x => x.Id == managerId).FirstOrDefault();
            int currentFiscalYearId = new SFiscalYear().List().Data.Where(x => x.BranchId == emp.BranchId && x.CurrentFiscalYear).FirstOrDefault().Id;
            manualPunchRequest.IsApproved = true;
            manualPunchRequest.ApproveBy = RiddhaSession.UserId;
            manualPunchRequest.ApproveDate = DateTime.Now;
            var result = _manualPunchRequestServices.Update(manualPunchRequest);
            if (result.Status == ResultStatus.Ok)
            {
                //Common.AddAuditTrail("1005", "1021", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
                EManualPunch punchModel = new EManualPunch();
                punchModel.EmployeeId = manualPunchRequest.EmployeeId;
                punchModel.BranchId = manualPunchRequest.BranchId;
                punchModel.DateTime = manualPunchRequest.DateTime;
                punchModel.Remark = manualPunchRequest.Remark;
                var manualPunchResult = manualPunchServices.Add(punchModel);
                if (manualPunchResult.Status == ResultStatus.Ok)
                {
                    #region Notification

                    SNotification notificationServices = new SNotification();
                    int[] notificationTargets = Common.intToArray(emp.Id);
                    int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                    DateTime expiryDate = result.Data.ApproveDate.Value.AddDays(notificationExpiryDays);
                    notificationServices.Add(new ENotification()
                    {
                        CompanyId = emp.Branch.CompanyId,
                        EffectiveDate = manualPunchRequest.ApproveDate.ToDateTime(),
                        ExpiryDate = expiryDate,
                        FiscalYearId = currentFiscalYearId,
                        NotificationLevel = NotificationLevel.Employee,
                        NotificationType = NotificationType.ManualPunch,
                        PublishDate = manualPunchRequest.ApproveDate.ToDateTime(),
                        TranDate = DateTime.Now,
                        TypeId = result.Data.Id,
                        Title = "Manual punch request has been approved.",
                        Message = "This is to inform you that you're Manual punch on " + result.Data.DateTime.ToString("yyyy/MM/dd") + " - " + result.Data.DateTime.ToString(@"hh\:mm") + " due to the reason of " + result.Data.Remark + " has been approved by " + RiddhaSession.CurrentUser.FullName,
                    }, notificationTargets);
                    #endregion

                    #region Email
                    string participants = "";
                    participants += string.Format(EmployeeInformationTemplate, emp.Code, emp.Name, emp.Designation.Name, emp.Section == null ? "" : emp.Section.Department.Name);
                    var finalTemplate = string.Format(ManualPunchRequestManagerApprovedTemplate, manager.Name, manualPunchRequest.DateTime.ToString("yyyy/MM/dd") + " - " + manualPunchRequest.DateTime.ToString(@"hh\:mm"), "Approved", participants);
                    var mail = new MailCommon();
                    string subject = "Manual Punch Request Approved";
                    AlternateView htmlViewForManager = AlternateView.CreateAlternateViewFromString(finalTemplate, null, "text/html");
                    if (!string.IsNullOrEmpty(manager.Email))
                    {
                        mail.SendMail(manager.Email, subject, "", htmlViewForManager);
                    }
                    var finalTemplateForEmployee = string.Format(ManualPunchEmployeeApprovedTemplate, emp.Name, manualPunchRequest.DateTime.ToString("yyyy/MM/dd") + " - " + manualPunchRequest.DateTime.ToString(@"hh\:mm"), "approved.");
                    AlternateView htmlViewForEmployee = AlternateView.CreateAlternateViewFromString(finalTemplateForEmployee, null, "text/html");
                    if (!string.IsNullOrEmpty(emp.Email))
                    {
                        mail.SendMail(emp.Email, subject, "", htmlViewForEmployee);
                    }
                    return RedirectToAction("Index", new RouteValueDictionary(
                    new { controller = "EmailActivities", action = "Index", title = "Manual Punch Request", message = "Manual Puncht request approved sucessfully." }));
                    #endregion
                }
            }
            return RedirectToAction("Index", new RouteValueDictionary(
            new { controller = "EmailActivities", action = "Index", title = "Manual Punch Request", message = "Process error. Please contact your administrator" }));
        }

        public ActionResult KajRequestApprove(int id, int managerId)
        {
            SKaj _kajServices = new SKaj();
            SEmployee employeeServices = new SEmployee();
            var kaj = _kajServices.List().Data.Where(x => x.Id == id && x.KajStatus == KajStatus.New).FirstOrDefault();
            if (kaj == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Not Found", message = "Process error. This Kaj has been expired or removed." }));
            }
            if (kaj.IsApprove)
            {
                return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "EmailActivities", action = "Index", title = "Process error.", message = "Kaj request is already approved." }));
            }
            var manager = employeeServices.List().Data.Where(x => x.Id == managerId).FirstOrDefault();
            var fiscalYear = new SFiscalYear().List().Data.Where(x => x.BranchId == manager.BranchId && x.CurrentFiscalYear).FirstOrDefault();
            if (manager != null)
            {
                if (kaj != null)
                {
                    kaj.KajStatus = KajStatus.Approve;
                    kaj.ApprovedOn = System.DateTime.Now;
                    kaj.IsApprove = true;
                    kaj.ApprovedById = RiddhaSession.UserId;
                    var result = _kajServices.Approve(kaj);
                    if (result.Status == ResultStatus.Ok)
                    {
                        SKaj kajServices = new SKaj();
                        var kajDetails = kajServices.ListDetail().Data.Where(x => x.KajId == kaj.Id).FirstOrDefault();
                        var emp = new SEmployee().List().Data.Where(x => x.Id == kajDetails.EmployeeId).FirstOrDefault();
                        Common.AddAuditTrail("8021", "7229", DateTime.Now, managerId, id, result.Message);
                        #region Notification
                        decimal kajApplyDay = (kaj.To - kaj.From).Days + 1;
                        SNotification notificationServices = new SNotification();
                        var empQuery = new SEmployee().List().Data.Where(x => x.BranchId == emp.BranchId);
                        int[] notificationTargets = Common.intToArray(emp.Id);
                        int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                        DateTime expiryDate = result.Data.ApprovedOn.Value.AddDays(notificationExpiryDays);
                        notificationServices.Add(new ENotification()
                        {
                            CompanyId = emp.Branch.CompanyId,
                            ExpiryDate = expiryDate,
                            FiscalYearId = fiscalYear.Id,
                            EffectiveDate = result.Data.ApprovedOn.ToDateTime(),
                            NotificationLevel = NotificationLevel.Employee,
                            NotificationType = NotificationType.ManualPunch,
                            PublishDate = result.Data.ApprovedOn.ToDateTime(),
                            TranDate = DateTime.Now,
                            TypeId = result.Data.Id,
                            Title = "Kaj request has been approved.",
                            Message = "This is to inform you that you're Kaj request on " + result.Data.From.ToString("yyyy/MM/dd") + " - " + result.Data.From.ToString(@"hh\:mm") + " To " + result.Data.To.ToString("yyyy/MM/dd") + " - " + result.Data.To.ToString(@"hh\:mm") + " due to the reason of " + result.Data.Remark + " has been approved by " + RiddhaSession.CurrentUser.FullName,
                        }, notificationTargets);
                        #endregion

                        #region Email
                        string participants = "";
                        participants += string.Format(EmployeeInformationTemplate, emp.Code, emp.Name, emp.Designation.Name, emp.Section == null ? "" : emp.Section.Department.Name);
                        var finalTemplate = string.Format(managerApprovedTemplate, manager.Name, kaj.From.ToString("yyyy/MM/dd"), kaj.To.ToString("yyyy/MM/dd"), "Approved", participants);
                        var mail = new MailCommon();
                        string subject = "Kaj Request Approved";
                        AlternateView htmlViewForManager = AlternateView.CreateAlternateViewFromString(finalTemplate, null, "text/html");
                        if (!string.IsNullOrEmpty(manager.Email))
                        {
                            mail.SendMail(manager.Email, subject, "", htmlViewForManager);
                        }
                        var finalTemplateForEmployee = string.Format(OfficeVisitEmployeeApprovedTemplate, emp.Name, kaj.From.ToString("yyyy/MM/dd"), kaj.To.ToString("yyyy/MM/dd"), "approved.");
                        AlternateView htmlViewForEmployee = AlternateView.CreateAlternateViewFromString(finalTemplateForEmployee, null, "text/html");
                        if (!string.IsNullOrEmpty(emp.Email))
                        {
                            mail.SendMail(emp.Email, subject, "", htmlViewForEmployee);
                        }
                    }
                    return RedirectToAction("Index", new RouteValueDictionary(
                    new { controller = "EmailActivities", action = "Index", title = "Office Visit Request", message = "Kaj request approved sucessfully." }));
                    #endregion

                }
            }

            return RedirectToAction("Index", new RouteValueDictionary(
            new { controller = "EmailActivities", action = "Index", title = "Kaj Request Approve", message = "Manager not found. Please contact your administrator" }));
        }

        public ActionResult KajRequestReject(int id, int managerId)
        {
            return RedirectToAction("Index", new RouteValueDictionary(
            new { controller = "EmailActivities", action = "Index", title = "Kaj Request Reject", message = "Process error. Please contact your administrator" }));
        }
    }
}