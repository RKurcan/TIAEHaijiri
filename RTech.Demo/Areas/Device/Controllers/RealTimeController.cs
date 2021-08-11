using Riddhasoft.Attendance.Entities;
using Riddhasoft.Attendance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Device.Controllers
{
    public class RealTimeController : Controller
    {
        // GET: Device/RealTime
        public ActionResult Index()
        {
            return View();
        }
        public async Task<string> Push(UserModel user)
        {
            EAttendanceLog data = new EAttendanceLog()
            {
                DeviceId=user.DeviceID,
                EmployeeId = user.EnrollNumber,
                DateTime = user.LogDate,
                Remark = "",
                VerifyMode = user.VerifyMode
            };
            SAttendanceLog sLogService = new SAttendanceLog();
            sLogService.Add(data);
            //if (smsConfiguration.EnableInPunchSMS)
            //{
            //    var employee = new SEmployee().List().Data.Where(x => x.DeviceCode == user.EnrollNumber).FirstOrDefault();
            //    if (employee != null)
            //    {
            //        var smsModel = new SMSModel()
            //        {
            //            Authentication = new Authentication()
            //            {
            //                Username = "internationalsms",
            //                Password = "HZlGhtj",
            //            },
            //            Messages = new List<Message>(){
            //                new Message()
            //                {
            //                    Sender="RiddhaTest",
            //                    Text="real time punch sms test, punched Enroll Id is "+user.EnrollNumber+" and punch date is "+user.LogDate,
            //                    Recipients = new List<Recipient>()
            //                    {
            //                        new Recipient()
            //                        {
            //                            Gsm=employee.Mobile,
            //                            MessageId="",
            //                        }
            //                     }
            //                }
            //            }
            //        };
            //        var SmsService = new SSMS();
            //        await SmsService.SendSMS(smsModel);
            //    }
            //}
            return "ok";
        }
        public async Task<string> SendSMS(SMSModel message)
        {
            //"muinint"
            //"intmu"
            // http://121.241.242.114:8080/sendsms?username=kap7-muinint&password=xxxxxx&type=0&dlr=1&destination=91xxxxxxxx&source=xxxxx&message=xxxxxxx
            var recipent = "";
            foreach (var msg in message.Messages)
            {
                recipent = "";
                foreach (var rec in msg.Recipients)
                {
                    recipent += recipent.Length > 0 ? "," : "" + rec.Gsm;
                }
                string apiUrl = string.Format("http://121.241.242.114:8080/sendsms?username=kap7-{0}&password={1}&type=0&dlr=1&destination={4}&source=SAS&message={3}", message.Authentication.Username, message.Authentication.Password, recipent, msg.Text);
                WebRequest request = WebRequest.Create(apiUrl);
                request.Method = "GET";
                HttpWebResponse httpResponse = ((HttpWebResponse)request.GetResponse());
            }
            //WebResponse response = request.GetResponse(); 
            return "";
        }
    }
    public class UserModel
    {
        public int EnrollNumber { get; set; }

        public int VerifyMode { get; set; }

        public int InOutMode { get; set; }

        public DateTime LogDate { get; set; }

        public string DeviceIP { get; set; }

        public int DevicePort { get; set; }

        public int DeviceID { get; set; }

        public string StrSerialNo { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Authentication
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SMSModel
    {
        public Authentication Authentication { get; set; }
        public List<Message> Messages { get; set; }

    }
    public class Message
    {
        public string Sender { get; set; }
        public string Text { get; set; }

        public List<Recipient> Recipients { get; set; }

    }
    public class Recipient
    {
        public string Gsm { get; set; }
        public string MessageId { get; set; }
    }
}