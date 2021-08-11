using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTech.Demo.Controllers.MobileApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MEmployeeApiController : MRootController
    {
        public MEmployeeApiController()
        {

        }
        [HttpPost]
        public MobileResult<bool> ChangeUserNameAndPassword(int employeeId, string userName, string currentPassword, string newPassword)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                var emp = new SEmployee().List().Data.Where(x => x.Id == employeeId).FirstOrDefault();
                if (emp != null)
                {
                    var user = new SUser().List().Data.Where(x => x.Id == emp.UserId && x.BranchId == emp.BranchId).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Password == currentPassword)
                        {
                            user.Name = userName;
                            user.Password = newPassword;

                            var result = new SUser().Update(user);
                            if (result.Status == ResultStatus.Ok)
                            {
                                return new MobileResult<bool>()
                                {
                                    Data = true,
                                    Message = "Username & password changed successfully. Please Re-Login with ur new username & password.",
                                    Status = MobileResultStatus.Ok,
                                    Token = token,
                                };
                            }
                            else
                            {
                                return new MobileResult<bool>()
                                {
                                    Data = false,
                                    Message = "Process error. Please try again later.",
                                    Status = MobileResultStatus.Ok,
                                    Token = null,
                                };
                            }
                        }
                        return new MobileResult<bool>()
                        {
                            Data = false,
                            Message = "Invalid current password. The password you entered doesn't match.",
                            Status = MobileResultStatus.ProcessError,
                            Token = null,
                        };
                    }
                    return new MobileResult<bool>()
                    {
                        Data = false,
                        Message = "User not found.",
                        Status = MobileResultStatus.ProcessError,
                        Token = null,
                    };
                }
                return new MobileResult<bool>()
                {
                    Data = false,
                    Message = "Employee not found.",
                    Status = MobileResultStatus.ProcessError,
                    Token = null,
                };
            }
            return InvalidTokeResult<bool>();
        }
    }
}
