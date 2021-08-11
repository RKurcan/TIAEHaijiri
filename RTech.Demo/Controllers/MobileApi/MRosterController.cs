using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Mobile.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTech.Demo.Controllers.MobileApi
{
     [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MRosterController : MRootController
    {
        [HttpGet]
        public MobileResult<List<EMRoster>> GetRoster(int empId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                SMRoster rs = new SMRoster();
                List<EMRoster> rosterLst = rs.getRoster(empId);
                return new MobileResult<List<EMRoster>>()
                {
                    Data = rosterLst,
                    Status = MobileResultStatus.Ok
                };
            }
            return InvalidTokeResult<List<EMRoster>>();
        }
    }
}
