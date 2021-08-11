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
    public class TravelInformationApiController : ApiController
    {
        STravelInformation _travelInformationServices = null;
        LocalizedString _loc = null;
        public TravelInformationApiController()
        {
            _travelInformationServices = new STravelInformation();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<TravelInformationGridVm>> Get()
        {
            int branchId = (int)RiddhaSession.BranchId;
            var travelInformationLst = (from c in _travelInformationServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                        select new TravelInformationGridVm()
                                        {
                                            Id = c.Id,
                                            BranchId = c.BranchId,
                                            DepartureTime = c.DepartureTime,
                                            DestinationAddress = c.DestinationAddress,
                                            MainDestination = c.MainDestination,
                                            TravelPeriodFrom = c.TravelPeriodFrom.ToString("yyyy/MM/dd"),
                                            TravelPeriodTo = c.TravelPeriodTo.ToString("yyyy/MM/dd"),
                                            TravelRequestId = c.TravelRequestId
                                        }).ToList();
            return new ServiceResult<List<TravelInformationGridVm>>()
            {
                Data = travelInformationLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ETravelInformation> Get(int id)
        {
            ETravelInformation travelInformation = _travelInformationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ETravelInformation>()
            {
                Data = travelInformation,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<TravelInformationGridVm>> GetTravelInformationByRequestId(int RequestId)
        {
            int branchId = (int)RiddhaSession.BranchId;
            var travelInformationLst = (from c in _travelInformationServices.List().Data.Where(x => x.BranchId == branchId && x.TravelRequestId == RequestId).ToList()
                                        select new TravelInformationGridVm()
                                        {
                                            Id = c.Id,
                                            BranchId = c.BranchId,
                                            DepartureTime = c.DepartureTime,
                                            DestinationAddress = c.DestinationAddress,
                                            MainDestination = c.MainDestination,
                                            TravelPeriodFrom = c.TravelPeriodFrom.ToString("yyyy/MM/dd"),
                                            TravelPeriodTo = c.TravelPeriodTo.ToString("yyyy/MM/dd"),
                                            TravelRequestId = c.TravelRequestId
                                        }).ToList();
            return new ServiceResult<List<TravelInformationGridVm>>()
            {
                Data = travelInformationLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ETravelInformation> Post(ETravelInformation model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            var result = _travelInformationServices.Add(model);
            return new ServiceResult<ETravelInformation>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ETravelInformation> Put(ETravelInformation model)
        {
            var result = _travelInformationServices.Update(model);
            return new ServiceResult<ETravelInformation>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var travelInformation = _travelInformationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _travelInformationServices.Remove(travelInformation);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
    public class TravelInformationGridVm
    {
        public int Id { get; set; }
        public string MainDestination { get; set; }
        public string TravelPeriodFrom { get; set; }
        public string TravelPeriodTo { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string DestinationAddress { get; set; }
        public int BranchId { get; set; }
        public int TravelRequestId { get; set; }
    }
}
