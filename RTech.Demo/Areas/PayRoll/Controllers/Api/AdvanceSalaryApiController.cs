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
    public class AdvanceSalaryApiController : ApiController
    {
        SAdvanceSalary _advanceSalaryServices = null;
        int BranchId = (int)RiddhaSession.BranchId;
        LocalizedString _loc = null;
        public AdvanceSalaryApiController()
        {
            _advanceSalaryServices = new SAdvanceSalary();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<EAdvanceSalary>> Get()
        {
            var result = _advanceSalaryServices.List().Data.ToList();
            return new ServiceResult<List<EAdvanceSalary>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<EAdvanceSalary>> Get(int EmpId)
        {
            var result = _advanceSalaryServices.List().Data.Where(x => x.EmployeeId == EmpId).ToList();
            return new ServiceResult<List<EAdvanceSalary>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<decimal> GetPreviousDue(int EmpId)
        {

            decimal PreviousDue = 0;
            return new ServiceResult<decimal>()
            {
                Data = PreviousDue,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EAdvanceSalary> Post(EAdvanceSalary model)
        {
            model.BranchId = BranchId;
            model.CreationDate = DateTime.Now;
            var result = _advanceSalaryServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8003", "7247", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<EAdvanceSalary>()
            {
                Data = model,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        public ServiceResult<EAdvanceSalary> Put(EAdvanceSalary model)
        {
            model.BranchId = BranchId;
            var result = _advanceSalaryServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8003", "7248", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<EAdvanceSalary>()
            {
                Data = model,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        public ServiceResult<int> Delete(int Id)
        {
            var data = _advanceSalaryServices.List().Data.Where(x => x.Id == Id).FirstOrDefault();
            var result = _advanceSalaryServices.Remove(data);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8003", "7249", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };

        }
    }
}
