using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Controllers.MobileApi
{
    public class MRootController : ApiController
    {
        protected MobileResult<T> InvalidTokeResult<T>()
        {
            return new MobileResult<T>()
            {
                Status = MobileResultStatus.InvalidToken,
                Message = "Invalid Token."
            };
        }
    }
}
