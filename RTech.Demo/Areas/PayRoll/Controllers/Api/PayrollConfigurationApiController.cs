using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class PayrollConfigurationApiController : ApiController
    {
        SPayrollConfiguration _payrollConfigServices = null;
        LocalizedString _loc = null;
        public PayrollConfigurationApiController()
        {
            _payrollConfigServices = new SPayrollConfiguration();
            _loc = new LocalizedString();
        }

        [HttpGet]
        public ServiceResult<EPayrollConfiguration> GetPayrollConfig()
        {
            var payRollConfig = _payrollConfigServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).FirstOrDefault();
            return new ServiceResult<EPayrollConfiguration>()
            {
                Data = payRollConfig,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EPayrollConfiguration> Get(int id)
        {
            EPayrollConfiguration payRollConfig = _payrollConfigServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EPayrollConfiguration>()
            {
                Data = payRollConfig,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EPayrollConfiguration> Post(EPayrollConfiguration model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            var result = _payrollConfigServices.Add(model);
            return new ServiceResult<EPayrollConfiguration>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EPayrollConfiguration> Put([FromBody]EPayrollConfiguration model)
        {
            var result = _payrollConfigServices.Update(model);
            return new ServiceResult<EPayrollConfiguration>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var payRollConfig = _payrollConfigServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _payrollConfigServices.Remove(payRollConfig);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
}
