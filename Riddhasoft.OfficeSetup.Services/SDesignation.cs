using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SDesignation : Riddhasoft.Services.Common.IBaseService<EDesignation>
    {
        RiddhaDBContext db = null;
        public SDesignation()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EDesignation>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EDesignation>>()
            {
                Data = db.Designation,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDesignation> Add(EDesignation model)
        {
            db.Designation.Add(model);
            db.SaveChanges();
            return new ServiceResult<EDesignation>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok

            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDesignation> Update(EDesignation model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDesignation>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EDesignation model)
        {

            try
            {
                int employeeCount = db.Employee.Count(x => x.DesignationId == model.Id);
                if (employeeCount > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("There {1} {0} employee on this Designation. Designation Can't be deleted.", employeeCount, employeeCount == 1 ? "is" : "are"),
                        Status = ResultStatus.dataBaseError
                    };
                }
                model = db.Designation.Remove(db.Designation.Find(model.Id));
                db.SaveChanges();
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = "RemoveSuccess",
                    Status = ResultStatus.Ok
                };
            }
            catch (SqlException ex)
            {

                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.dataBaseError
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.unHandeledError
                };
            }
        }

        public ServiceResult<bool> ApplyLeaveQuota(List<EDesignationWiseLeavedBalance> lst)
        {
            lst.ForEach(x => x.CreatedOn = DateTime.Now);
            List<EDesignationWiseLeavedBalanceHist> hst = (from c in lst.Where(x=>x.IsMapped).ToList()
                                                           select new EDesignationWiseLeavedBalanceHist()
                                                           {
                                                               ApplicableGender=c.ApplicableGender,
                                                               Balance=c.Balance,
                                                               CreatedOn=c.CreatedOn,
                                                               DesignationId=c.DesignationId,
                                                               IsLeaveCarryable=c.IsLeaveCarryable,
                                                               IsMapped=true,
                                                               IsPaidLeave=c.IsPaidLeave,
                                                               LeaveId=c.LeaveId,
                                                               MaxLimit=c.MaxLimit
                                                           }).ToList();
            db.DesignationWiseLeavedBalanceHist.AddRange(hst);
            db.SaveChanges();

            int designationId=lst.FirstOrDefault().DesignationId;

            var list = db.DesignationWiseLeavedBalance.Where(x => x.DesignationId == designationId).ToList();
            var existingLeaveQuata = (from c in list
                                      join d in lst on c.Id equals d.Id
                                      select c).ToList();
            if (existingLeaveQuata.Count>0)
            {
                db.DesignationWiseLeavedBalance.RemoveRange(existingLeaveQuata);
                db.SaveChanges();
            }

            db.DesignationWiseLeavedBalance.AddRange(lst.Where(x=>x.IsMapped).ToList());
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Message = "AppliedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EDesignationWiseLeavedBalance>> ListLeaveQouta()
        {
            return new ServiceResult<IQueryable<EDesignationWiseLeavedBalance>>()
            {
                Data = db.DesignationWiseLeavedBalance,
                Status = ResultStatus.Ok
            };
        }
    }
}
