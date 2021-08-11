using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class ShiftApiController : ApiController
    {
        SShift shiftServices = null;
        LocalizedString loc = null;

        public ShiftApiController()
        {
            shiftServices = new SShift();
            loc = new LocalizedString();
        }
        [ActionFilter("1046")]
        public ServiceResult<List<ShiftGridVm>> GetShift( int branchId)
        {
            if (branchId ==0)
            {
                branchId = (int)RiddhaSession.BranchId;
            }
            SShift service = new SShift();
            var shiftLst = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                            select new ShiftGridVm()
                            {
                                Id = c.Id,
                                ShiftCode = c.ShiftCode,
                                ShiftName = c.ShiftName,
                                NameNp = c.NameNp,
                                ShiftStartTime = c.ShiftStartTime,
                                ShiftEndTime = c.ShiftEndTime,
                                LunchStartTime = c.LunchStartTime,
                                LunchEndTime = c.LunchEndTime,
                                ShiftType = Enum.GetName(typeof(ShiftType), c.ShiftType),
                                NumberOfStaff = c.NumberOfStaff,
                                BranchId = c.BranchId,
                                LateGrace = c.LateGrace,
                                EarlyGrace = c.EarlyGrace,
                                ShortDayWorkingEnable = c.ShortDayWorkingEnable,
                                ShiftStartGrace = c.ShiftStartGrace,
                                ShiftEndGrace = c.ShiftEndGrace,
                                StartMonth = c.StartMonth,
                                EndMonth = c.EndMonth,
                                StartDays = c.StartDays,
                                EndDays = c.EndDays,
                                HalfDayWorkingHour = c.HalfDayWorkingHour,
                                DeclareAbsentForLateIn = c.DeclareAbsentForLateIn,
                                DeclareAbsentForEarlyOut = c.DeclareAbsentForEarlyOut
                            }).ToList();

            return new ServiceResult<List<ShiftGridVm>>()
            {
                Data = shiftLst,
                Status = ResultStatus.Ok,
                Message = "",
            };
        }

        public ServiceResult<EShift> Get(int id)
        {
            EShift shift = shiftServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EShift>()
            {
                Data = shift,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("1032")]
        public ServiceResult<EShift> Post(EShift model)
        {
            //model.BranchId = RiddhaSession.BranchId;
            var result = shiftServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1009", "1032", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EShift>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [ActionFilter("1033")]
        public ServiceResult<EShift> Put(EShift model)
        {
            var result = shiftServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1009", "1033", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EShift>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status

            };
        }
        [HttpDelete, ActionFilter("1034")]
        public ServiceResult<int> Delete(int id)
        {
            var shift = shiftServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = shiftServices.Remove(shift);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1009", "1034", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
    public class ShiftGridVm
    {
        public int Id { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string NameNp { get; set; }
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public TimeSpan LunchStartTime { get; set; }
        public TimeSpan LunchEndTime { get; set; }
        public string ShiftType { get; set; }
        public int NumberOfStaff { get; set; }
        public int? BranchId { get; set; }
        public bool ShortDayWorkingEnable { get; set; }
        public TimeSpan ShiftStartGrace { get; set; }
        public TimeSpan ShiftEndGrace { get; set; }
        public NepaliMonth StartMonth { get; set; }
        public int StartDays { get; set; }
        public NepaliMonth EndMonth { get; set; }
        public int EndDays { get; set; }
        public TimeSpan? EarlyGrace { get; set; }
        public TimeSpan? LateGrace { get; set; }
        public TimeSpan? HalfDayWorkingHour { get; set; }
        public bool DeclareAbsentForLateIn { get; set; }
        public bool DeclareAbsentForEarlyOut { get; set; }
    }
}
