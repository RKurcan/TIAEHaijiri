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
    public class MKajController : MRootController
    {
        #region Mail Template
        string KajManagerRequestTemplate = "<div class='panel panel-success'>" +
                                                          "<div class='panel-heading'>" +
                                                              "<h2></h2>" +
                                                          "</div>" +
                                                           "<div class='panel-body'>" +
                                                              "<p>" +
                                                                  "Dear {0},</br>" +
                                                                  "<p>You are requested to approve Kaj Request from {1} to {2} due to the reason of {3}</p>" +
                                                                  "<p></p>" +
                                                                  "<p><b>Employee Information</b></p>" +
                                                                  "{4}" +
                                                                  "<p></p>" +
                                                                  "<p>Please Click at link to approve or reject the request</p>" +
                                                                  "<p>" +
                                                                  "   <a style='display: block;width: 115px;height: 25px;background: #5CB85C;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='{5}/EmailActivities/KajRequestApprove?id={6}&managerId={7}'>Approve</a>" +
                                                                  "   <a style='display: block;width: 115px;height: 25px;background: #F0AD4E;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='{8}/EmailActivities/KajRequestReject?id={9}&managerId={10}' >Reject</a>" +
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
        public MKajController()
        {

        }

        [HttpPost]
        public MobileResult<EMKaj> RequestKaj(EMKaj model)
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
                    return new MobileResult<EMKaj>()
                    {
                        Data = null,
                        Message = "Current fiscal year is not set. Please request you're admin to set current fiscal year.",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
                EKaj kaj = new EKaj()
                {
                    ApprovedById = null,
                    ApprovedOn = null,
                    BranchId = (int)emp.BranchId,
                    From = model.FromDate.Add(model.FromTime),
                    IsApprove = false,
                    KajStatus = KajStatus.New,
                    Remark = model.Remark,
                    To = model.ToDate.Add(model.ToTime),
                };

                var kajResult = new SMKaj().AddKaj(kaj);
                if (kajResult.Status == MobileResultStatus.Ok)
                {
                    EKajDetail kajDetail = new EKajDetail()
                    {
                        EmployeeId = model.EmployeeId,
                        KajId = kajResult.Data.Id,
                    };
                    var result = new SMKaj().AddKajDetail(kajDetail);
                    if (result.Status == MobileResultStatus.Ok)
                    {
                        new Thread(() =>
                        {
                            #region Notification
                            var empQuery = new SEmployee().List().Data;
                            decimal kajApplyDay = (kajResult.Data.To - kajResult.Data.From).Days + 1;
                            SNotification notificationServices = new SNotification();
                            int[] notificationTargets = (from c in empQuery.Where(x => x.BranchId == emp.BranchId && x.IsManager).ToList()
                                                         select c.Id).ToArray();
                            int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                            DateTime expiryDate = kajResult.Data.From.AddDays(notificationExpiryDays);
                            notificationServices.Add(new ENotification()
                            {
                                CompanyId = emp.Branch.CompanyId,
                                EffectiveDate = kajResult.Data.From,
                                ExpiryDate = expiryDate,
                                FiscalYearId = fiscalYear.Id,
                                NotificationLevel = NotificationLevel.Employee,
                                NotificationType = NotificationType.Kaj,
                                PublishDate = DateTime.Now,
                                TranDate = DateTime.Now,
                                TypeId = result.Data.Id,
                                Title = "Kaj has been requested by " + emp.Code + "-" + emp.Name + " for " + kajApplyDay.ToString() + " From " + kajResult.Data.From.ToString("yyyy/MM/dd") + " To " + kajResult.Data.To.ToString("yyyy/MM/dd"),
                                Message = "This is to inform you that " + emp.Name + " has requested kaj for " + kajApplyDay.ToString() + " days due to the reason of " + model.Remark,
                            }, notificationTargets);
                            #endregion
                            #region Email
                            if (PackageId >= 3)
                            {
                                if (emp.ReportingManagerId != null && emp.ReportingManagerId != 0)
                                {
                                    var manager = employeeServices.List().Data.Where(x => x.Id == emp.ReportingManagerId).FirstOrDefault();
                                    string participants = "";
                                    participants += string.Format(EmployeeInformationTemplate, emp.Code, emp.Name, emp.Designation.Name, emp.Section.Department.Name);
                                    if (manager != null && !string.IsNullOrEmpty(manager.Email))
                                    {
                                        //manager email template
                                        var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                                        var finalTemplate = string.Format(KajManagerRequestTemplate, manager.Name, model.FromDate.ToString("yyyy/MM/dd"), model.ToDate.ToString("yyyy/MM/dd"), model.Remark, participants, baseUrl, kajResult.Data.Id, manager.Id, baseUrl, kajResult.Data.Id, manager.Id);
                                        var mail = new MailCommon();
                                        string subject = "Kaj Request";
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

                        return new MobileResult<EMKaj>()
                        {
                            Status = MobileResultStatus.Ok,
                            Message = "Thank you. Kaj is requested successfully. We will notify you soon after your request is approved."
                        };
                    }
                }
            }
            return InvalidTokeResult<EMKaj>();
        }
    }
}
