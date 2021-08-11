using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Mobile.Services;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using static RTech.Demo.Utilities.WDMS;

namespace RTech.Demo.Controllers.MobileApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MOfficeVisitController : MRootController
    {
        #region Mailtemplate
        /// <summary>
        /// 0-managername
        /// 1-from time
        /// 2-to time
        /// 
        /// 3-employee list
        /// 4-approveBaseUrl
        /// 5-saved office visit primary key for approve
        /// 6-managerId
        /// 7-rejectBaseUrl
        /// 8-saved office visit primary key for reject
        /// </summary>
        string OfficeVisitManagerRequestTemplate = "<div class='panel panel-success'>" +
                                                              "<div class='panel-heading'>" +
                                                                  "<h2></h2>" +
                                                              "</div>" +
                                                               "<div class='panel-body'>" +
                                                                  "<p>" +
                                                                      "Dear {0},</br>" +
                                                                      "<p>You are requested to approve office Visit  from {1} to {2}</p>" +
                                                                      "<p></p>" +
                                                                      "<p><b>Employee Information</b></p>" +
                                                                      "{3}" +
                                                                      "<p></p>" +
                                                                      "<p>Please Click at link to approve or reject the request</p>" +
                                                                      "<p>" +
                                                                      "   <a style='display: block;width: 115px;height: 25px;background: #5CB85C;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='{4}/EmailActivities/OfficeVisitRequestApprove?id={5}&managerId={6}'>Approve</a>" +
                                                                      "   <a style='display: block;width: 115px;height: 25px;background: #F0AD4E;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='{7}/EmailActivities/OfficeVisitRequestReject?id={8}&managerId={9}' >Reject</a>" +
                                                                      "</p>" +
                                                                  "</p>" +
                                                              "</div>" +
                                                          "</div>";
        /// <summary>
        /// 0-code
        /// 1-name
        /// 2-designation
        /// 3-department
        /// 
        /// </summary>
        string EmployeeInformationTemplate = "<p>{0}-{1}<br/>{2}-{3}</p>";
        #endregion

        [HttpPost]
        public MobileResult<EMOfficeVisitRequest> RequestOfficeVisit(EMOfficeVisitRequest model)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {

                SEmployee employeeServices = new SEmployee();
                var emp = employeeServices.List().Data.Where(x => x.Id == model.EmployeeId).FirstOrDefault();
                int PackageId = (int)emp.Branch.Company.SoftwarePackageType;

                // validate current fiscal year for notification table 
                var fiscalYear = new SFiscalYear().List().Data.Where(x => x.CompanyId == emp.Branch.CompanyId && x.CurrentFiscalYear).FirstOrDefault();
                if (fiscalYear == null)
                {
                    return new MobileResult<EMOfficeVisitRequest>()
                    {
                        Data = null,
                        Message = "Current fiscal year is not set. Please request you're admin to set current fiscal year.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
                EOfficeVisitRequest officeVisitRequest = new EOfficeVisitRequest()
                {
                    BranchId = (int)emp.BranchId,
                    From = model.FromDate.AddTicks(model.FromTime.Ticks),
                    EmployeeId = model.EmployeeId,
                    Remark = model.Remark,
                    SystemDate = DateTime.Now,
                    Altitude = model.Altitude,
                    Branch = null,
                    Employee = null,
                    Image = model.Image,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    To = model.ToDate.AddTicks(model.ToTime.Ticks),
                };
                var officeVisitRequestResult = new SMOfficeVisitRequest().AddOfficeVisitRequest(officeVisitRequest);
                if (officeVisitRequestResult.Status == MobileResultStatus.Ok)
                {

                    new Thread(() =>
                    {
                        #region Notification
                        //TODO: Office Visit Mail work if company package is cooprate
                        decimal officeVisitApplyDay = (officeVisitRequestResult.Data.To - officeVisitRequestResult.Data.From).Days + 1;
                        SNotification notificationServices = new SNotification();
                        var empQuery = new SEmployee().List().Data.Where(x => x.BranchId == emp.BranchId);
                        int[] notificationTargets = (from c in empQuery.Where(x => x.BranchId == emp.BranchId && x.IsManager).ToList()
                                                     select c.Id).ToArray();
                        int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                        DateTime expiryDate = officeVisitRequestResult.Data.From.AddDays(notificationExpiryDays);
                        notificationServices.Add(new ENotification()
                        {
                            CompanyId = emp.Branch.CompanyId,
                            EffectiveDate = officeVisitRequestResult.Data.From,
                            ExpiryDate = expiryDate,
                            FiscalYearId = fiscalYear.Id,
                            NotificationLevel = NotificationLevel.Employee,
                            NotificationType = NotificationType.ManualPunch,
                            PublishDate = DateTime.Now,
                            TranDate = DateTime.Now,
                            TypeId = officeVisitRequestResult.Data.Id,
                            Title = "Office Visit has been requested by " + emp.Code + "-" + emp.Name + " for " + officeVisitApplyDay.ToString() + " days. From " + officeVisitRequestResult.Data.From.ToString("yyyy/MM/dd") + " To " + officeVisitRequestResult.Data.To.ToString("yyyy/MM/dd"),
                            Message = "This is to inform you that " + emp.Name + " has requested Office Visit for " + officeVisitApplyDay.ToString() + " days. From " + officeVisitRequestResult.Data.From.ToString("yyyy/MM/dd") + " To " + officeVisitRequestResult.Data.To.ToString("yyyy/MM/dd") + " due to the reason of " + officeVisitRequestResult.Data.Remark,
                        }, notificationTargets);
                        #endregion

                        #region Email

                        if (PackageId >= 3)
                        {
                            var visitors = employeeServices.List().Data.Where(x => x.Id == model.EmployeeId).FirstOrDefault();
                            if (visitors.ReportingManagerId != null)
                            {
                                var manager = employeeServices.List().Data.Where(x => x.Id == visitors.ReportingManagerId).FirstOrDefault();
                                string participants = "";
                                participants += string.Format(EmployeeInformationTemplate, visitors.Code, visitors.Name, visitors.Designation.Name, visitors.Section.Department.Name);
                                if (manager != null && !string.IsNullOrEmpty(manager.Email))
                                {
                                    //manager email template
                                    var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                                    var finalTemplate = string.Format(OfficeVisitManagerRequestTemplate, manager.Name, model.FromDate.ToString("yyyy/MM/dd"), model.ToDate.ToString("yyyy/MM/dd"), participants, baseUrl, officeVisitRequestResult.Data.Id, manager.Id, baseUrl, officeVisitRequestResult.Data.Id, manager.Id);
                                    var mail = new MailCommon();
                                    string subject = "Office Visit Request";
                                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(finalTemplate, null, "text/html");
                                    try
                                    {
                                        mail.SendMail(manager.Email, subject, "", htmlView);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Write(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                                    }
                                }
                            }
                        }
                        #endregion
                    }).Start();
                    return new MobileResult<EMOfficeVisitRequest>()
                    {
                        Status = MobileResultStatus.Ok,
                        Message = "Thank you.Ofice visit is requested successfully. We will notify you soon after your request is approved."
                    };
                }

            }
            return InvalidTokeResult<EMOfficeVisitRequest>();
        }
    }
}
