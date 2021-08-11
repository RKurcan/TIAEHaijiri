using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
namespace RTech.Demo.Controllers.MobileApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MForgotPasswordController : MRootController
    {
        public MForgotPasswordController()
        {

        }

        [HttpPost]
        public MobileResult<EMForgotPassword> ForgotPassword(string registeredUserName)
        {
            SEmployee employeeServices = new SEmployee();
            // validate registered username is mobile no or email
            
            var validateEmail = employeeServices.List().Data.Where(x => x.Email == registeredUserName).FirstOrDefault();
            if (validateEmail != null)
            {
                
            }
           
            return new MobileResult<EMForgotPassword>()
            {
                Data = null,
                Message = "",
                Status = MobileResultStatus.ProcessError,
                Token = null,
            };
        }
    }
}
