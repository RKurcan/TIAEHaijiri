using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
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
    public class SLeaveMaster : Riddhasoft.Services.Common.IBaseService<ELeaveMaster>
    {
        RiddhaDBContext db = null;
        public SLeaveMaster()
        {
            db = new RiddhaDBContext();
        }
        public SLeaveMaster(RiddhaDBContext db)
        {
            this.db = db;
        }
        public RiddhaDBContext GetDbContext()
        {
            return db;
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ELeaveMaster>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ELeaveMaster>>()
            {
                Data = db.LeaveMaster,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<ELeaveMaster> Add(ELeaveMaster model)
        {
            db.LeaveMaster.Add(model);
            db.SaveChanges();
            return new ServiceResult<ELeaveMaster>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<ELeaveMaster> Update(ELeaveMaster model)
        {
            if (model.IsReplacementLeave)
            {
                var leaveMaster = db.LeaveMaster.Where(x => x.IsReplacementLeave == true && x.BranchId == model.BranchId && x.Id != model.Id).FirstOrDefault();
                if (leaveMaster != null)
                {
                    leaveMaster.IsReplacementLeave = false;
                    leaveMaster.CreatedOn = System.DateTime.Now;
                    db.Entry(leaveMaster).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            model.CreatedOn = System.DateTime.Now;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ELeaveMaster>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public List<ELeaveBalance> GetELeaveBalances(int[] empids)
        {
            var eleavebalances = (from c in db.LeaveBalance
                                  join d in empids
                                  on c.EmployeeId equals d
                                  select c
                                ).Include(x => x.Employee).ToList();

            return eleavebalances;
        }
        public List<ELeaveBalance> GetELeaveBalances()
        {
            var eleavebalances = (from c in db.LeaveBalance
                                  select c
                                ).Include(x => x.Employee).ToList();

            return eleavebalances;
        }
        public List<ELeaveBalance> GetELeaveBalances(int companyId)
        {
            var leaveMaster = db.LeaveMaster.Where(x => x.CompanyId == companyId).ToList();
            var eleavebalances = (from c in db.LeaveBalance.ToList()
                                  join d in leaveMaster on c.LeaveMasterId equals d.Id
                                  select c
                                ).ToList();

            return eleavebalances;
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ELeaveMaster model)
        {
            //db.LeaveMaster.Remove(model);
            //db.SaveChanges();
            //return new ServiceResult<int>()
            //{
            //    Data = 1,
            //    Message = "Remove Successfully",
            //    Status = ResultStatus.Ok
            //};
            try
            {
                int leaveApplication = db.LeaveApplication.Count(x => x.LeaveMasterId == model.Id);
                if (leaveApplication > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("There {1} {0} Leave Application on this Leave Master. Leave Master Can't be deleted.", leaveApplication, leaveApplication == 1 ? "is" : "are"),
                        Status = ResultStatus.dataBaseError
                    };
                }
                model = db.LeaveMaster.Remove(db.LeaveMaster.Find(model.Id));
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

        public ServiceResult<List<ELeaveMaster>> PullLeaveMaster(List<ELeaveMaster> lst, int branchId)
        {
            //retrieve all leave for current company
            List<ELeaveMaster> existingLeaveList = db.LeaveMaster.Where(x => x.BranchId == branchId).ToList();
            //check for duplicate while inserting from loop
            //list compare duplicate .
            //remove duplicate(match)
            List<ELeaveMaster> leaveLstToAdd = (from c in lst
                                                where !(from o in existingLeaveList
                                                        select o.Name.ToLower())
                                                       .Contains(c.Name.ToLower())
                                                select c).ToList();
            //insert new only
            if (leaveLstToAdd.Count() > 0)
            {
                db.LeaveMaster.AddRange(leaveLstToAdd);
                db.SaveChanges();
                return new ServiceResult<List<ELeaveMaster>>()
                {
                    Data = existingLeaveList,
                    Status = ResultStatus.Ok,
                    Message = "ImportSuccess"
                };
            }
            return new ServiceResult<List<ELeaveMaster>>()
            {
                Data = null,
                Status = ResultStatus.processError,
                Message = "DataAlreadyPulled"
            };
        }

        //public ServiceResult<int> RemoveAll(int[] ids)
        //{
        //    try
        //    {
        //        var leaveApplications = from c in db
        //        if (leaveApplication > 0)
        //        {
        //            return new ServiceResult<int>()
        //            {
        //                Data = 1,
        //                Message = string.Format("There {1} {0} Leave Application on this Leave Master. Leave Master Can't be deleted.", leaveApplication, leaveApplication == 1 ? "is" : "are"),
        //                Status = ResultStatus.dataBaseError
        //            };
        //        }
        //        model = db.LeaveMaster.Remove(db.LeaveMaster.Find(model.Id));
        //        db.SaveChanges();
        //        return new ServiceResult<int>()
        //        {
        //            Data = 1,
        //            Message = "RemoveSuccess",
        //            Status = ResultStatus.Ok
        //        };
        //    }
        //    catch (SqlException ex)
        //    {

        //        return new ServiceResult<int>()
        //        {
        //            Data = 1,
        //            Message = ex.Message,
        //            Status = ResultStatus.dataBaseError
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResult<int>()
        //        {
        //            Data = 1,
        //            Message = ex.Message,
        //            Status = ResultStatus.unHandeledError
        //        };
        //    }
        //}
    }
}
