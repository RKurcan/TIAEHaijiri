using Riddhasoft.DB;
using Riddhasoft.HRM.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.HRM.Services
{
    public class SEmploymentStatusWiseLeavedBalance : Riddhasoft.Services.Common.IBaseService<EEmploymentStatusWiseLeavedBalance>
    {
        RiddhaDBContext db = null;
        public SEmploymentStatusWiseLeavedBalance()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<EEmploymentStatusWiseLeavedBalance> Add(EEmploymentStatusWiseLeavedBalance model)
        {
            db.EmploymentStatusWiseLeavedBalance.Add(model);
            db.SaveChanges();

            return new ServiceResult<EEmploymentStatusWiseLeavedBalance>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EEmploymentStatusWiseLeavedBalance>> List()
        {
            return new ServiceResult<IQueryable<EEmploymentStatusWiseLeavedBalance>>()
            {
                Data = db.EmploymentStatusWiseLeavedBalance,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EEmploymentStatusWiseLeavedBalance model)
        {
            db.EmploymentStatusWiseLeavedBalance.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmploymentStatusWiseLeavedBalance> Update(EEmploymentStatusWiseLeavedBalance model)
        {
            db.Entry<EEmploymentStatusWiseLeavedBalance>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmploymentStatusWiseLeavedBalance>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<bool> ApplyLeaveQuota(List<EEmploymentStatusWiseLeavedBalance> lst)
        {
            lst.ForEach(x => x.CreatedOn = DateTime.Now);
            List<EEmploymentStatusWiseLeavedBalanceHist> hst = (from c in lst.Where(x => x.IsMapped).ToList()
                                                                select new EEmploymentStatusWiseLeavedBalanceHist()
                                                                {
                                                                    ApplicableGender = c.ApplicableGender,
                                                                    Balance = c.Balance,
                                                                    CreatedOn = c.CreatedOn,
                                                                    EmploymentStatusId = c.EmploymentStatusId,
                                                                    IsLeaveCarryable = c.IsLeaveCarryable,
                                                                    IsMapped = true,
                                                                    IsPaidLeave = c.IsPaidLeave,
                                                                    LeaveId = c.LeaveId,
                                                                    MaxLimit = c.MaxLimit

                                                                }).ToList();
            db.EmploymentStatusWiseLeavedBalanceHist.AddRange(hst);
            db.SaveChanges();
            int employmentStatusId = lst.FirstOrDefault().EmploymentStatusId;

            var list = db.EmploymentStatusWiseLeavedBalance.Where(x => x.EmploymentStatusId == employmentStatusId).ToList();
            var existingLeaveQuata = (from c in list
                                      join d in lst on c.Id equals d.Id
                                      select c).ToList();
            if (existingLeaveQuata.Count > 0)
            {
                db.EmploymentStatusWiseLeavedBalance.RemoveRange(existingLeaveQuata);
                db.SaveChanges();
            }
            db.EmploymentStatusWiseLeavedBalance.AddRange(lst.Where(x => x.IsMapped).ToList());
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Message = "AppliedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
