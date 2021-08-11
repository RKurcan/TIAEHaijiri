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
    public class MManualPunchRequestController : MRootController
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
        string PunchRequestManagerRequestTemplate = "<div class='panel panel-success'>" +
                                                              "<div class='panel-heading'>" +
                                                                  "<h2></h2>" +
                                                              "</div>" +
                                                               "<div class='panel-body'>" +
                                                                  "<p>" +
                                                                      "Dear {0},</br>" +
                                                                      "<p>You are requested to approve manual punch request on {1}</p>" +
                                                                      "<p></p>" +
                                                                      "<p><b>Employee Information</b></p>" +
                                                                      "{2}" +
                                                                      "<p></p>" +
                                                                      "<p>Please Click at link to approve or reject the request</p>" +
                                                                      "<p>" +
                                                                      "   <a style='display: block;width: 115px;height: 25px;background: #5CB85C;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='{3}/EmailActivities/ManualPunchRequestApprove?id={4}&managerId={5}'>Approve</a>" +
                                                                      "   <a style='display: block;width: 115px;height: 25px;background: #F0AD4E;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='{6}/EmailActivities/ManualPunchRequestReject?id={7}&managerId={8}' >Reject</a>" +
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
        public MManualPunchRequestController()
        {

        }
        [HttpPost]
        public MobileResult<EMManualPunchRequest> RequestManualPunch(EMManualPunchRequest model)
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
                    return new MobileResult<EMManualPunchRequest>()
                    {
                        Data = null,
                        Message = "Current fiscal year is not set. Please request you're admin to set current fiscal year.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }

                EManualPunchRequest punchRequest = new EManualPunchRequest()
                {
                    EmployeeId = model.EmployeeId,
                    BranchId = (int)emp.BranchId,
                    DateTime = DateTime.Parse(model.Date).AddTicks(model.Time.ToTimeSpan().Ticks),
                    Altitude = model.Altitude,
                    Image = model.Image,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Remark = model.Remark,
                    SystemDate = DateTime.Now,
                };
                var result = new SMManualPunchRequest().Add(punchRequest);
                if (result.Status == MobileResultStatus.Ok)
                {
                    new Thread(() =>
                    {
                        #region Notification
                        SNotification notificationServices = new SNotification();
                        var empQuery = new SEmployee().List().Data.Where(x => x.BranchId == emp.BranchId);
                        int[] notificationTargets = (from c in empQuery.Where(x => x.BranchId == emp.BranchId && x.IsManager).ToList()
                                                     select c.Id).ToArray();
                        int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                        DateTime expiryDate = result.Data.DateTime.AddDays(notificationExpiryDays);
                        notificationServices.Add(new ENotification()
                        {
                            CompanyId = emp.Branch.CompanyId,
                            EffectiveDate = result.Data.DateTime,
                            ExpiryDate = expiryDate,
                            FiscalYearId = fiscalYear.Id,
                            NotificationLevel = NotificationLevel.Employee,
                            NotificationType = NotificationType.ManualPunch,
                            PublishDate = result.Data.DateTime,
                            TranDate = DateTime.Now,
                            TypeId = result.Data.Id,
                            Title = "Manual punch has been requested by " + emp.Name + " on " + result.Data.DateTime.ToString("yyyy/MM/dd") + " - " + result.Data.DateTime.ToString(@"hh\:mm"),
                            Message = "This is to inform you that " + emp.Name + " has requested Manual punch on " + result.Data.DateTime.ToString("yyyy/MM/dd") + " - " + result.Data.DateTime.ToString(@"hh\:mm") + " due to the reason of " + result.Data.Remark,
                        }, notificationTargets);
                        #endregion

                        #region Email

                        if (PackageId >= 3)
                        {
                            if (emp.ReportingManagerId != null)
                            {
                                var manager = employeeServices.List().Data.Where(x => x.Id == emp.ReportingManagerId).FirstOrDefault();
                                string participants = "";
                                participants += string.Format(EmployeeInformationTemplate, emp.Code, emp.Name, emp.Designation.Name, emp.Section.Department.Name);
                                if (manager != null && manager.Email != null)
                                {
                                    //manager email template
                                    var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                                    DateTime dt = DateTime.Parse(model.Date);
                                    var finalTemplate = string.Format(PunchRequestManagerRequestTemplate, manager.Name, dt.ToString("yyyy/MM/dd") + " - " + model.Time + " due to the reason of " + model.Remark, participants, baseUrl, result.Data.Id, manager.Id, baseUrl, result.Data.Id, manager.Id);
                                    var mail = new MailCommon();
                                    string subject = "Manual Punch Request";
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
                    return new MobileResult<EMManualPunchRequest>()
                    {
                        Status = MobileResultStatus.Ok,
                        Message = "Thank you. Manual punch is requested successfully. We will notify you soon after your request is approved."
                    };
                }
            }
            return InvalidTokeResult<EMManualPunchRequest>();
        }
        [HttpGet]
        public MobileResult<EMServerInfo> GetServerInfo()
        {
            EMServerInfo result = new EMServerInfo();
            result.Date = DateTime.Now;
            return new MobileResult<EMServerInfo>()
            {
                Data = result,
                Message = "",
                Status = MobileResultStatus.Ok
            };

        }
    }
    public class EMServerInfo
    {
        public DateTime Date { get; set; }
    }

}
