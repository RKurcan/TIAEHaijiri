using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class DemoRequestApiController : ApiController
    {
        protected SDemoRequest _demoRequestServices = null;
        protected LocalizedString _loc = null;
        private string userEmail = null;
        private string userName = null;
        private string userAddress = null;
        private string userContactPerson = null;
        private string userContact = null;
        public DemoRequestApiController()
        {
            _demoRequestServices = new SDemoRequest();
            _loc = new LocalizedString();
        }

        [HttpGet]
        public ServiceResult<List<EDemoRequest>> Get()
        {
            var result = _demoRequestServices.List().Data.ToList();
            return new ServiceResult<List<EDemoRequest>>()
            {
                Data = result,
                Message = "",
            };
        }

        [HttpPost]
        public ServiceResult<EDemoRequest> Post([FromBody]EDemoRequest model)
        {
            var result = _demoRequestServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                userEmail = result.Data.Email;
                userName = result.Data.Name;
                userContact = result.Data.ContactNo;
                userContactPerson = result.Data.ContactPerson;
                userAddress = result.Data.Address;
                ThreadStart sendMail = new ThreadStart(SendMail);
                ThreadStart sendFeedBackMail = new ThreadStart(SendFeedbackMail);
                Thread thread = new Thread(sendMail);
                Thread feedbackThread = new Thread(sendFeedBackMail);
                thread.Start();
                //feedbackThread.Start();
            }
            return new ServiceResult<EDemoRequest>()
            {
                Data = model,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var demoRequest = _demoRequestServices.List().Data.FirstOrDefault(x => x.Id == id);
            if (demoRequest != null)
            {
                var result = _demoRequestServices.Remove(demoRequest);
                return new ServiceResult<int>()
                {
                    Data = 0,
                    Message = _loc.Localize(result.Message),
                    Status = result.Status
                };
            }
            return new ServiceResult<int>()
            {
                Data = 0,
                Status = ResultStatus.processError,
                Message = "Not Found"
            };
        }
        public void SendMail()
        {
            var mail = new MailCommon();
            var subject = "Demo Login Credentials";
            var message = "Dear " +userName+","+
                          "<br>" +
                          "Below are demo login credentials." +
                          "<br>"+
                          "<b>Company Code</b>:tsc" +
                          "<br>" +
                          "<b>User Name</b>:admin" +
                          "<br>" +
                          "<b>Password</b>: Adm!n123" +
                          "<br>" +
                          "Login:<a href='http://www.ehajiri.com.np/user/user/login'>www.ehajiri.com.np/user/user/login</a>";
            mail.SendMail(userEmail, subject, message);
        }

        public void SendFeedbackMail()
        {
            MailCommon mail = new MailCommon();
            string[] receivers = new string[] {"support@barcodenepal.com" };
            string subject = userName + " requested for demo login credentials";
            string message = userName +" from "+userAddress+ " requested for demo login credentials " + DateTime.Now.ToString("yyyy/MM/dd") + Environment.NewLine + "Contact No: " + userContact + Environment.NewLine + "Contact Person:" + userContactPerson + Environment.NewLine + "Email: " + userEmail + " .";
            for (int i = 0; i < receivers.Count(); i++)
            {
                //mail.SendMail(receivers[i], subject, message);
            }
                
        }
    }
}
