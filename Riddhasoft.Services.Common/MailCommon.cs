using System;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Riddhasoft.Services.Common
{
    public class MailCommon
    {
        public MailMessage CreateMessage(MailAddress FromMailAddress, MailAddress ToMailAddress, string Subject, string Body, string AttachmentPath)
        {
            MailMessage msgMail = new MailMessage();

            msgMail.From = FromMailAddress;
            msgMail.To.Add(ToMailAddress);
            msgMail.Body = Body;
            msgMail.IsBodyHtml = true;
            msgMail.Attachments.Add(new Attachment(AttachmentPath));
            return msgMail;
        }
        public MailMessage CreateMessage(MailAddress FromMailAddress, MailAddress ToMailAddress, string Subject, string Body)
        {
            MailMessage msgMail = new MailMessage();

            msgMail.From = FromMailAddress;
            msgMail.To.Add(ToMailAddress);
            msgMail.Body = Body;
            msgMail.IsBodyHtml = true;
            return msgMail;
        }
        public MailMessage CreateMessage(MailAddress FromMailAddress, MailAddress ToMailAddress, string Subject, string Body, Attachment attachment)
        {
            MailMessage msgMail = new MailMessage();

            msgMail.From = FromMailAddress;
            msgMail.To.Add(ToMailAddress);
            msgMail.Body = Body;
            msgMail.IsBodyHtml = true;
            msgMail.Attachments.Add(attachment);
            return msgMail;
        }
        private bool SendEmail(MailMessage msgMail, NetworkCredential cred)
        {
            #region smtp
            SmtpClient mailClient;
            smtpHost host = smtpHost.gmail;
            string hostName = "";

            hostName = GetValueAsString(host);
            mailClient = new SmtpClient(hostName, 587);

            mailClient.UseDefaultCredentials = false;
            mailClient.EnableSsl = true;
            mailClient.Credentials = cred;
            #endregion

            try
            {
                mailClient.Send(msgMail);
            }
            catch (System.IO.IOException)
            {

                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                //do something if required
            }
            return true;

        }
        public void SendMail(string receiver, string subject, string message)
        {

            //ehajiri@gmail.com
            MailAddress from = new MailAddress("ehajiri@barcodenepal.com");
            NetworkCredential cred = new NetworkCredential("ehajiri@barcodenepal.com", "@ttend@nce1!");
            MailAddress to = new MailAddress(receiver);
            var mailMsg = CreateMessage(from, to, subject, message);

            mailMsg.Subject = string.Format(subject);
            SendEmail(mailMsg, cred);
            mailMsg.Dispose();
        }
        public void SendMail(string receiver, string subject, string message, ref bool status)
        {
            //ehajiri@gmail.com
            MailAddress from = new MailAddress("ehajiri@barcodenepal.com");
            NetworkCredential cred = new NetworkCredential("ehajiri@barcodenepal.com", "@ttend@nce1!");
            MailAddress to = new MailAddress(receiver);
            var mailMsg = CreateMessage(from, to, subject, message);

            mailMsg.Subject = string.Format(subject);
            status = SendEmail(mailMsg, cred);
            mailMsg.Dispose();
        }
        public void SendMail(string receiver, string subject, string message, AlternateView htmlView, string attachmentPath = "")
        {

            //ehajiri@gmail.com
            MailAddress from = new MailAddress("ehajiri@barcodenepal.com");
            NetworkCredential cred = new NetworkCredential("ehajiri@barcodenepal.com", "@ttend@nce1!");
            MailAddress to = new MailAddress(receiver);
            var mailMsg = CreateMessage(from, to, subject, message);

            mailMsg.Subject = string.Format(subject);

            mailMsg.AlternateViews.Add(htmlView);
            if (string.IsNullOrEmpty(attachmentPath) == false)
                mailMsg.Attachments.Add(new Attachment(attachmentPath));
            SendEmail(mailMsg, cred);
            mailMsg.Dispose();
        }
        public void SendMail(string receiver, string subject, string message, string sender, string password)
        {

            MailAddress from = new MailAddress(sender);
            NetworkCredential cred = new NetworkCredential(sender, password);
            MailAddress to = new MailAddress(receiver);
            var mailMsg = CreateMessage(from, to, subject, message);

            mailMsg.Subject = string.Format(subject);
            SendEmail(mailMsg, cred);
            mailMsg.Dispose();
        }
        public static string GetValueAsString(smtpHost host)
        {
            // get the field 
            var field = host.GetType().GetField(host.ToString());
            var customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes.Length > 0)
            {
                return (customAttributes[0] as DescriptionAttribute).Description;
            }
            else
            {
                return host.ToString();
            }
        }

        public void SendCustomizedEmail(string mailTemplateLink)
        {
            if (!string.IsNullOrEmpty(mailTemplateLink))
            {
                string mailBody = GetTemplate(mailTemplateLink);
                string receivingEmailAddress = ConfigurationManager.AppSettings["LicenseReceivingEmailAddress"];
                if (receivingEmailAddress != null)
                {
                    SendMail(receivingEmailAddress, "New Company License", mailBody, "ehajiri@barcodenepal.com", "@ttend@nce1!");
                }
            }
        }
        public void SendCustomizedEmail(string mailTemplateLink, string mailSubject)
        {
            if (!string.IsNullOrEmpty(mailTemplateLink))
            {
                string mailBody = GetTemplate(mailTemplateLink);
                string receivingEmailAddress = ConfigurationManager.AppSettings["LicenseReceivingEmailAddress"];
                if (receivingEmailAddress != null)
                {
                    SendMail(receivingEmailAddress, mailSubject, mailBody, "ehajiri@barcodenepal.com", "@ttend@nce1!");
                }
            }
        }
        public static string GetTemplate(string link)
        {
            using (var myWebClient = new WebClient())
            {
                myWebClient.Headers["User-Agent"] = "MOZILLA/5.0 (WINDOWS NT 6.1; WOW64) APPLEWEBKIT/537.1 (KHTML, LIKE GECKO) CHROME/21.0.1180.75 SAFARI/537.1";

                string page = myWebClient.DownloadString(link);

                return page;
            }
        }
    }
    public enum smtpHost
    {
        [Description("smtp.gmail.com")]
        gmail = 0,
        [Description("smtp.gmail.com")]
        live = 1,
        [Description("smtp.mail.yahoo.com")]
        yahoo = 2,
        [Description("smtp.aim.com")]
        aim = 3
    }
}
