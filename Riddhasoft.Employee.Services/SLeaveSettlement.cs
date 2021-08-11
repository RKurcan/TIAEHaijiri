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
    public class SLeaveSettlement 
    {
        RiddhaDBContext db = null;
        public SLeaveSettlement()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ELeaveSettlement>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ELeaveSettlement>>()
            {
                Data = db.LeaveSettlement,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELeaveSettlement> Add(ELeaveSettlement model)
        {
            db.LeaveSettlement.Add(model);
            db.SaveChanges();
            if (model.SettlementType == SettlementType.CarrytoNext)
            {
                ELeaveCarryForwardBalance bal = new ELeaveCarryForwardBalance();
                bal.Balance = model.Balance;
                bal.EmployeeId = model.EmployeeId;
                bal.LeaveMasterId = model.LeaveMasterId;
                bal.FiscalYearId = model.FiscalYearId;
                AddLeaveCarryForwardBal(bal);
            }
            return new ServiceResult<ELeaveSettlement>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELeaveSettlement> Update(ELeaveSettlement model)
        {
            var existingData = db.LeaveSettlement.Where(x => x.EmployeeId == model.EmployeeId && x.LeaveMasterId == model.LeaveMasterId && x.FiscalYearId == model.FiscalYearId).ToList();
            if (existingData.Count() > 0)
            {
                db.LeaveSettlement.RemoveRange(existingData);
                db.SaveChanges();
                if (model.SettlementType == SettlementType.CarrytoNext)
                {
                    var existingCarry = db.LeaveCarryForwardBalance.Where(x => x.EmployeeId == model.EmployeeId && x.LeaveMasterId == model.LeaveMasterId && x.FiscalYearId == model.FiscalYearId).FirstOrDefault();
                    if (existingCarry != null)
                    {
                        db.LeaveCarryForwardBalance.Remove(existingCarry);
                        db.SaveChanges();
                    }
                    ELeaveCarryForwardBalance bal = new ELeaveCarryForwardBalance();
                    bal.Balance = model.Balance;
                    bal.EmployeeId = model.EmployeeId;
                    bal.LeaveMasterId = model.LeaveMasterId;
                    bal.FiscalYearId = model.FiscalYearId;
                    AddLeaveCarryForwardBal(bal);
                }
            }
            db.LeaveSettlement.Add(model);
            db.SaveChanges();
            return new ServiceResult<ELeaveSettlement>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(int id)
        {
            var leaveSettlement = db.LeaveSettlement.Where(x => x.Id == id).FirstOrDefault();

            var rowsToDelete = db.LeaveSettlement.Where(x => x.EmployeeId == leaveSettlement.EmployeeId && x.LeaveMasterId == leaveSettlement.LeaveMasterId && x.FiscalYearId == leaveSettlement.FiscalYearId).ToList();
            db.LeaveSettlement.RemoveRange(rowsToDelete);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<ELeaveSettlement> AddRange(List<ELeaveSettlement> lst)
        {
            db.LeaveSettlement.AddRange(lst);
            db.SaveChanges();

            ELeaveSettlement model = lst.Where(x => x.SettlementType == SettlementType.CarrytoNext).FirstOrDefault();
            ELeaveCarryForwardBalance bal = new ELeaveCarryForwardBalance();
            bal.Balance = model.Balance;
            bal.EmployeeId = model.EmployeeId;
            bal.LeaveMasterId = model.LeaveMasterId;
            bal.FiscalYearId = model.FiscalYearId;
            AddLeaveCarryForwardBal(bal);
            return new ServiceResult<ELeaveSettlement>()
            {
                Data = new ELeaveSettlement(),
                Status = ResultStatus.Ok,
                Message = "AddedSuccess"
            };
        }

        public bool AddLeaveCarryForwardBal(ELeaveCarryForwardBalance model)
        {
            var existingBal = db.LeaveCarryForwardBalance.Where(x => x.EmployeeId == model.EmployeeId && x.LeaveMasterId == model.LeaveMasterId).FirstOrDefault();
            if (existingBal != null)
            {
                db.LeaveCarryForwardBalance.Remove(existingBal);
                db.SaveChanges();
            }
            var result = db.LeaveCarryForwardBalance.Add(model);
            db.SaveChanges();
            return result.Id > 0;
        }

        public ServiceResult<decimal> GetCurrentCarryForwardBal(int empId, int leaveId, int currentFyId)
        {
            int? desigId = db.Employee.Where(x => x.Id == empId).FirstOrDefault().DesignationId;
            decimal carryBalance = 0;
            var currentFy = db.FiscalYear.Where(x => x.Id == currentFyId).FirstOrDefault();
            var previousFy = db.FiscalYear.Where(x => x.StartDate.Year == currentFy.StartDate.Year - 1).FirstOrDefault();
            if (previousFy != null)
            {
                var carryForwardBal = db.LeaveCarryForwardBalance.Where(x => x.EmployeeId == empId && x.LeaveMasterId == leaveId && x.FiscalYearId == previousFy.Id).FirstOrDefault() ?? new ELeaveCarryForwardBalance();
                if (carryForwardBal.Id > 0)
                {
                    carryBalance = carryForwardBal.Balance;
                }
                else
                {
                    //carryBalance = db.DesignationWiseLeavedBalance.Where(x => x.DesignationId == desigId && x.LeaveId == leaveId).FirstOrDefault().Balance;
                    carryBalance = 0;
                }
            }
            return new ServiceResult<decimal>()
            {
                Data = carryBalance,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<bool> IsLeaveSettled(int empId, int leaveId, int fyId)
        {
            var settleLeave = db.LeaveSettlement.Where(x => x.EmployeeId == empId && x.LeaveMasterId == leaveId && x.FiscalYearId == fyId).FirstOrDefault();
            return new ServiceResult<bool>()
            {
                Data = settleLeave != null,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<ELeaveSettlement> UpdateRange(List<ELeaveSettlement> lst)
        {
            List<ELeaveSettlement> existingLeaveSettl = new List<ELeaveSettlement>();
            int empId = lst[0].EmployeeId;
            int leaveId = lst[0].LeaveMasterId;
            int fyId = lst[0].FiscalYearId;
            existingLeaveSettl = db.LeaveSettlement.Where(x => x.EmployeeId == empId && x.LeaveMasterId == leaveId && x.FiscalYearId == fyId).ToList();
            if (existingLeaveSettl.Count() > 0)
            {
                db.LeaveSettlement.RemoveRange(existingLeaveSettl);
                db.SaveChanges();
                foreach (var item in lst)
                {
                    if (item.SettlementType == SettlementType.CarrytoNext)
                    {
                        var existingCarry = db.LeaveCarryForwardBalance.Where(x => x.EmployeeId == item.EmployeeId && x.LeaveMasterId == item.LeaveMasterId && x.FiscalYearId == item.FiscalYearId).FirstOrDefault();
                        if (existingCarry != null)
                        {
                            db.LeaveCarryForwardBalance.Remove(existingCarry);
                            db.SaveChanges();
                        }
                        ELeaveCarryForwardBalance bal = new ELeaveCarryForwardBalance();
                        bal.Balance = item.Balance;
                        bal.EmployeeId = item.EmployeeId;
                        bal.LeaveMasterId = item.LeaveMasterId;
                        bal.FiscalYearId = item.FiscalYearId;
                        AddLeaveCarryForwardBal(bal);
                    }
                }
            }
            db.LeaveSettlement.AddRange(lst);
            db.SaveChanges();
            return new ServiceResult<ELeaveSettlement>()
            {
                Data = lst[0],
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
