using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.EHajiriPro.Desktop
{
    class MailCommon
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
        public void SendEmail(MailMessage msgMail, NetworkCredential cred)
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

            mailClient.Send(msgMail);

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
