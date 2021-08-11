using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.HRM.Entities.Travel;
using Riddhasoft.HRM.Services.Travel;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class TravelEstimateApiController : ApiController
    {
        STravelEstimate _travelEstimateServices = null;
        LocalizedString _loc = null;
        public TravelEstimateApiController()
        {
            _travelEstimateServices = new STravelEstimate();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<TravelEstimateGridVm>> Get()
        {
            int branchId = (int)RiddhaSession.BranchId;
            var travelEstimateLst = (from c in _travelEstimateServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                     select new TravelEstimateGridVm()
                                     {
                                         Id = c.Id,
                                         Amount = c.Amount,
                                         CurrencyPaidIn = Enum.GetName(typeof(Currency), c.CurrencyPaidIn),
                                         ExpenseType = Enum.GetName(typeof(ExpenseType), c.ExpenseType),
                                         PaidBy = Enum.GetName(typeof(PaidBy), c.PaidBy),
                                         Remark = c.Remark,
                                         BranchId = c.BranchId,
                                         TravelRequestId = c.TravelRequestId
                                     }).ToList();
            return new ServiceResult<List<TravelEstimateGridVm>>()
            {
                Data = travelEstimateLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<TravelEstimateGridVm>> GetTravelEstimateByTravelRequestId(int RequestId)
        {
            int branchId = (int)RiddhaSession.BranchId;
            var travelEstimateLst = (from c in _travelEstimateServices.List().Data.Where(x => x.BranchId == branchId && x.TravelRequestId == RequestId).ToList()
                                     select new TravelEstimateGridVm()
                                     {
                                         Id = c.Id,
                                         Amount = c.Amount,
                                         CurrencyPaidIn = Enum.GetName(typeof(Currency), c.CurrencyPaidIn),
                                         ExpenseType = Enum.GetName(typeof(ExpenseType), c.ExpenseType),
                                         PaidBy = Enum.GetName(typeof(PaidBy), c.PaidBy),
                                         Remark = c.Remark,
                                         BranchId = c.BranchId,
                                         TravelRequestId = c.TravelRequestId
                                     }).ToList();
            return new ServiceResult<List<TravelEstimateGridVm>>()
            {
                Data = travelEstimateLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ETravelEstimate> Get(int id)
        {
            ETravelEstimate travelEstimate = _travelEstimateServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ETravelEstimate>()
            {
                Data = travelEstimate,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ETravelEstimate> Post(ETravelEstimate model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            var result = _travelEstimateServices.Add(model);
            return new ServiceResult<ETravelEstimate>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ETravelEstimate> Put(ETravelEstimate model)
        {
            var result = _travelEstimateServices.Update(model);
            return new ServiceResult<ETravelEstimate>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var travelEstimate = _travelEstimateServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _travelEstimateServices.Remove(travelEstimate);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
    public class TravelEstimateGridVm
    {
        public int Id { get; set; }
        public string ExpenseType { get; set; }
        public string CurrencyPaidIn { get; set; }
        public decimal Amount { get; set; }
        public string PaidBy { get; set; }
        public string Remark { get; set; }
        public int BranchId { get; set; }
        public int TravelRequestId { get; set; }

    }
}
