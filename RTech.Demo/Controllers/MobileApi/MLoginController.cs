using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Mobile.Services;
using Riddhasoft.Services.Common;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTech.Demo.Controllers.MobileApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MLoginController : ApiController
    {
        SMobileUser mobileUserServices = null;
        public MLoginController()
        {
            mobileUserServices = new SMobileUser();
        }
        [HttpPost]
        public MobileResult<EMEmployeeProfile> Login(EMLogin model)
        {
            var result = mobileUserServices.Login(model);
            return result;
        }
        [HttpGet]
        public MobileResult<EMEmployeeProfile> GetEmployeeInfoById(int employeeId)
        {
            var result = mobileUserServices.GetEmployeeInfoById(employeeId);
            return new MobileResult<EMEmployeeProfile>()
            {
                Data = result.Data,
                Message = result.Message,
                Status = result.Status
            };

        }

        [HttpGet]
        public MobileResult<EMEmployeesPersonalInfo> GetEmployeePersonalInfoById(int employeeId)
        {
            var result = mobileUserServices.GetEmployeePersonalInfoById(employeeId);
            return new MobileResult<EMEmployeesPersonalInfo>()
            {
                Data = result.Data,
                Message = result.Message,
                Status = result.Status
            };

        }
        [HttpGet]
        public MobileResult<int> GetApiVersion()
        {
            var result = mobileUserServices.GetApiVersion();
            return new MobileResult<int>()
            {
                Data = result,
                Message = "",
                Status = MobileResultStatus.Ok
            };

        }

        

    }


}
