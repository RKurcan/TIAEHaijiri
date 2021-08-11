using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Controllers
{
    public class ActivationController : Controller
    {
        public ActionResult Partner(string Id)
        {
            SReseller resellerService=new SReseller();
            var resellerLogin = resellerService.ListResellerLogin().Data.Where(x => x.ActivationCode == Id).FirstOrDefault();
            string message;
            if (resellerLogin != null)
            {
                resellerLogin.IsActivated = true;
                resellerService.UpdateResellerLogin(resellerLogin);
                message = "Successfully Activated. Please Check Your Email For Login Username and Password.";
                var mail = new MailCommon();
                mail.SendMail(resellerLogin.Reseller.Email, "EHajiri Login Credential", string.Format("username : {0}, password : {1} ", resellerLogin.User.Name, resellerLogin.User.Password));
            }
            else
            {
                message = "Invalid activation try..";
            }
            ViewBag.Message = message;
            return PartialView();
        }
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            SReseller resellerService = new SReseller();
            SUser userServices = new SUser();
            var resellerLogin = resellerService.ListResellerLogin().Data.Where(x => x.ActivationCode ==id).FirstOrDefault();
            string message;
            if (resellerLogin != null)
            {
                var user = userServices.List().Data.Where(x => x.Id == resellerLogin.UserId).FirstOrDefault();
                user.Password = createRandomNumber();
                userServices.Update(user);
                message = "Successfully reset password. Please Check Your Email For Login Username and Password.";
                var mail = new MailCommon();
                mail.SendMail(resellerLogin.Reseller.Email, "Hamro-Hajiri Reset Password", string.Format("username : {0}, password : {1} ", user.Name, user.Password));
            }
            else
            {
                message = "Invalid attempt..";
            }
            ViewBag.Message = message;
            return PartialView();
        }

        public ActionResult ResetUserPassword(string user)
        {
            var message = "";
            string strReq = "";
            strReq = Request.RawUrl;
            strReq = strReq.Substring(strReq.IndexOf('?') + 1);
            if (!strReq.Equals(""))
            {
                strReq = DecryptQueryString(strReq);

                //Parse the value... this is done is very raw format..
                //you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;
                arrIndMsg = arrMsgs[0].Split('=');
                var userId = int.Parse(arrIndMsg[1]);
                arrIndMsg = arrMsgs[1].Split('=');
                var email = arrIndMsg[1];

                SUser userServices = new SUser();

                var userLogin = userServices.List().Data.Where(x => x.Id == userId).FirstOrDefault();

                if(userLogin != null)
                {
                    userLogin.Password = createRandomNumber();
                    userServices.Update(userLogin);
                    message = "Successfully reset password. Please Check Your Email For Login Username and Password.";
                    var mail = new MailCommon();
                    mail.SendMail(email, "Hamro-Hajiri Reset Password", string.Format("username : {0}, password : {1} ", userLogin.Name, userLogin.Password));
                }
                else
                {
                    message = "Invalid attempt..";
                }
                ViewBag.Message = message;
                return PartialView();
            }
            else
            {
                return HttpNotFound();
            }           
        }
        private string createRandomNumber()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }

        private string DecryptQueryString(string strQueryString)
        {
            EncryptDecryptQueryString objEDQueryString = new EncryptDecryptQueryString();
            return objEDQueryString.Decrypt(strQueryString, "Hajiri@1");
        }
    }
}