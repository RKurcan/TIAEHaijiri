using Riddhasoft.DB;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Services.Common;

namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMForgotPassword
    {
        RiddhaDBContext db = null;
        public SMForgotPassword()
        {
            db = new RiddhaDBContext();
        }

        public void SendMail(EMForgotPassword model)
        {
            var mail = new MailCommon();
            mail.SendMail(model.Receiver, model.Subject, model.Message);
        }
    }
}
