using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Services
{
    public class SReplacementLeaveBalance
    {
        RiddhaDBContext db = null;
        public SReplacementLeaveBalance()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EReplacementLeaveBalance>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EReplacementLeaveBalance>>()
            {
                Data = db.ReplacementLeaveBalance,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeePresentInOffHist>> HistList()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeePresentInOffHist>>()
            {
                Data = db.EmployeePresentInOffHist,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EReplacementLeaveBalance> Add(EReplacementLeaveBalance model)
        {
            db.ReplacementLeaveBalance.Add(model);
            db.SaveChanges();
            return new ServiceResult<EReplacementLeaveBalance>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EReplacementLeaveBalance> Update(EReplacementLeaveBalance model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EReplacementLeaveBalance>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EReplacementLeaveBalance model)
        {
            db.ReplacementLeaveBalance.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Approve(List<EReplacementLeaveBalance> repLeaveBalLst, List<EEmployeePresentInOffHist> histLst)
        {
            int fyId = repLeaveBalLst[0].FyId;

            var existingHistory = db.EmployeePresentInOffHist.ToList();

            var inExistingHistory = (from c in existingHistory
                                     join d in histLst on c.Date.Date equals d.Date.Date
                                     select c).ToList();

            //var notInExistingHistory=
            //    (from c in existingHistory
            //     where !(from o in histLst
            //             select o.Date.Date)
            //            .Contains(c.Date.Date)
            //     select c).ToList();
            if (inExistingHistory.Count() > 0)
            {
                db.EmployeePresentInOffHist.AddRange(histLst);
                db.SaveChanges();

                var existingRepLeaveBals = db.ReplacementLeaveBalance.Where(x => x.FyId == fyId).ToList();
                var inExistingRepLeaveBals = (from c in existingRepLeaveBals
                                              join d in repLeaveBalLst on c.EmployeeId equals d.EmployeeId
                                              select c).ToList();

                var notInRepLeaveBals = (from c in existingRepLeaveBals
                                         where !(from o in repLeaveBalLst
                                                 select o.EmployeeId)
                                                .Contains(c.EmployeeId)
                                         select c).ToList();
                if (notInRepLeaveBals.Count() > 0)
                {
                    db.ReplacementLeaveBalance.AddRange(notInRepLeaveBals);
                    db.SaveChanges();
                }
                foreach (var item in inExistingRepLeaveBals)
                {
                    decimal remBal = 0;
                    remBal = item.RemainingBalance + repLeaveBalLst.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().RemainingBalance;
                    item.RemainingBalance = remBal;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }

            return new ServiceResult<int>()
            {
                Data = 1,
                Status = ResultStatus.Ok,
                Message = "ApprovedSuccess"
            };
        }

        public void Add(EEmployeePresentInOffHist hist, List<EReplacementLeaveBalance> repLst)
        {
            int fyId = repLst[0].FyId;
            var existingHist = db.EmployeePresentInOffHist.Where(x => x.EmployeeId == hist.EmployeeId && DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(hist.Date)).FirstOrDefault();
            if (existingHist == null)
            {
                db.EmployeePresentInOffHist.Add(hist);
                db.SaveChanges();

                var existingRepBal = db.ReplacementLeaveBalance.Where(x => x.FyId == fyId && x.EmployeeId == hist.EmployeeId).FirstOrDefault();
                if (existingRepBal == null)
                {
                    EReplacementLeaveBalance rep = new EReplacementLeaveBalance();
                    rep = repLst.Where(x => x.EmployeeId == hist.EmployeeId).FirstOrDefault();
                    db.ReplacementLeaveBalance.Add(rep);
                    db.SaveChanges();
                }

                else
                {
                    decimal remBal = existingRepBal.RemainingBalance;
                    decimal repBal = 1;
                    if (hist.PresentInHolidayOrDayOff == PresentInHolidayOrDayOff.Both)
                    {
                        repBal = 2;
                    }
                    existingRepBal.RemainingBalance = repBal + remBal;
                    db.Entry(existingRepBal).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}
