using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using System;
using System.Data.Entity;
using System.Linq;
namespace Riddhasoft.Employee.Services
{
    public class SLeaveApplication
    {
        RiddhaDBContext db = null;
        public SLeaveApplication()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ELeaveApplication>> List()
        {
            return new ServiceResult<IQueryable<ELeaveApplication>>()
            {
                Data = db.LeaveApplication,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELeaveApplication> Add(ELeaveApplication model, EleaveApplicationLog log)
        {
            model.Branch = null;
            model.Employee = null;
            model.AdminRemark = "";
            var emp = db.Employee.Where(x => x.Id == model.EmployeeId).FirstOrDefault();
            var comp = db.Company.Where(x => x.Id == emp.Branch.CompanyId).FirstOrDefault();
            int? desigId = db.Employee.Where(x => x.Id == model.EmployeeId).FirstOrDefault().DesignationId;
            var a = (from c in db.LeaveMaster
                     where
                         //c.BranchId == model.BranchId
                         c.CompanyId == comp.Id
                         && c.Id == model.LeaveMasterId
                     select c
                       ).FirstOrDefault();
            decimal leaveBalance = 0;
            if (a.IsReplacementLeave)
            {
                leaveBalance = GetRemBal(model.LeaveMasterId, model.EmployeeId, log.FiscalYearId).Data;
            }
            else
            {
                if (comp.EmploymentStatusWiseLeave)
                {
                    EEmploymentStatusWiseLeavedBalance leaveBalanceobj = new EEmploymentStatusWiseLeavedBalance();
                    var contract = db.Contract.Where(x => x.EmployeeId == model.EmployeeId).ToList();
                    if (contract != null)
                    {
                        int employmentStatusId = contract.LastOrDefault().EmploymentStatusId;
                        leaveBalanceobj = db.EmploymentStatusWiseLeavedBalance.Where(x => x.LeaveId == model.LeaveMasterId && x.EmploymentStatusId == employmentStatusId).FirstOrDefault();
                        if (leaveBalanceobj != null)
                        {
                            leaveBalance = leaveBalanceobj.Balance;
                        }
                        else
                        {
                            return new ServiceResult<ELeaveApplication>()
                            {
                                Data = null,
                                Message = "Employment Status Wise Leave Balance not found..",
                                Status = ResultStatus.processError,
                            };
                        }
                    }
                    else
                    {
                        return new ServiceResult<ELeaveApplication>()
                        {
                            Data = null,
                            Message = "Employee not in contract..",
                            Status = ResultStatus.processError,
                        };
                    }
                }
                else
                {
                    EDesignationWiseLeavedBalance leaveBalanceobj = new EDesignationWiseLeavedBalance();
                    leaveBalanceobj = db.DesignationWiseLeavedBalance.Where(x => x.DesignationId == desigId && x.LeaveId == model.LeaveMasterId).FirstOrDefault();
                    leaveBalance = leaveBalanceobj.Balance;
                }
            }

            if (leaveBalance < log.LeaveCount)
            {
                return new ServiceResult<ELeaveApplication>()
                {
                    Data = null,
                    Message = "LeaveBalExceeded",
                    Status = ResultStatus.processError
                };
            }
            if (comp.AutoLeaveApproved)
            {
                model.LeaveStatus = LeaveStatus.Approve;
                model.ApprovedOn = System.DateTime.Now;
            }
            db.LeaveApplication.Add(model);
            db.SaveChanges();
            if (comp.AutoLeaveApproved)
            {
                log.LeaveApplicationId = model.Id;
                db.LeaveApplicationLog.Add(log);
                db.SaveChanges();
            }

            return new ServiceResult<ELeaveApplication>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "AddedSuccess"
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ELeaveApplication> Update(ELeaveApplication model)
        {
            decimal leaveCount = (model.To - model.From).Days + 1;
            int? desigId = db.Employee.Where(x => x.Id == model.EmployeeId).FirstOrDefault().DesignationId;
            var comp = db.Company.Where(x => x.Id == model.Employee.Branch.CompanyId).FirstOrDefault();
            if (comp.EmploymentStatusWiseLeave)
            {
                EEmploymentStatusWiseLeavedBalance leaveBalanceobj = new EEmploymentStatusWiseLeavedBalance();
                var contract = db.Contract.Where(x => x.EmployeeId == model.EmployeeId).ToList();
                if (contract != null)
                {
                    int employmentStatusId = contract.LastOrDefault().EmploymentStatusId;
                    leaveBalanceobj = db.EmploymentStatusWiseLeavedBalance.Where(x => x.LeaveId == model.LeaveMasterId && x.EmploymentStatusId == employmentStatusId).FirstOrDefault();
                    if (leaveBalanceobj != null)
                    {
                        if (leaveBalanceobj.Balance < leaveCount)
                        {
                            return new ServiceResult<ELeaveApplication>()
                            {
                                Data = null,
                                Message = "LeaveBalExceeded",
                                Status = ResultStatus.processError
                            };
                        }
                    }
                    else
                    {
                        return new ServiceResult<ELeaveApplication>()
                        {
                            Data = null,
                            Message = "Employment Status Wise Leave Balance not found..",
                            Status = ResultStatus.processError,
                        };
                    }
                }
                else
                {
                    return new ServiceResult<ELeaveApplication>()
                    {
                        Data = null,
                        Message = "Employee not in contract..",
                        Status = ResultStatus.processError,
                    };
                }
            }
            else
            {
                EDesignationWiseLeavedBalance leaveBalance = db.DesignationWiseLeavedBalance.Where(x => x.DesignationId == desigId && x.LeaveId == model.LeaveMasterId).FirstOrDefault();
                if (leaveBalance.Balance < leaveCount)
                {
                    return new ServiceResult<ELeaveApplication>()
                    {
                        Data = null,
                        Message = "LeaveBalExceeded",
                        Status = ResultStatus.processError
                    };
                }
            }

            var existingLeavApp = db.LeaveApplication.Where(x => x.Id == model.Id).FirstOrDefault();
            existingLeavApp.EmployeeId = model.EmployeeId;
            existingLeavApp.From = model.From;
            existingLeavApp.To = model.To;
            existingLeavApp.Description = model.Description;
            existingLeavApp.LeaveDay = model.LeaveDay;
            existingLeavApp.LeaveMasterId = model.LeaveMasterId;
            existingLeavApp.ApprovedById = model.ApprovedById;
            db.Entry(existingLeavApp).State = EntityState.Modified;
            db.SaveChanges();

            var existingAppLog = db.LeaveApplicationLog.Where(x => x.LeaveApplicationId == model.Id).FirstOrDefault();
            if (existingAppLog != null)
            {
                existingAppLog.LeaveCount = leaveCount;
                db.Entry(existingAppLog).State = EntityState.Modified;
                db.SaveChanges();
            }
            return new ServiceResult<ELeaveApplication>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ELeaveApplication model)
        {
            db.LeaveApplication.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<decimal> GetRemBal(int LeaveMasterId, int EmployeeId, int fiscalyearId)
        {

            if (EmployeeId == null || EmployeeId == 0)
            {
                return new ServiceResult<decimal>()
                {
                    Data = 0,
                    Message = "",
                    Status = ResultStatus.processError,
                };
            }
            //get fiscal year start date & end date
            SFiscalYear fiscalYearServices = new SFiscalYear();
            SEmployee employeeServices = new SEmployee();
            SCompany companyServices = new SCompany();
            var employee = employeeServices.List().Data.Where(x => x.Id == EmployeeId).FirstOrDefault();
            var comp = companyServices.List().Data.Where(x => x.Id == employee.Branch.CompanyId).FirstOrDefault();

            var fyYear = fiscalYearServices.List().Data.Where(x => x.Id == fiscalyearId);
            //get accured leave by employee id acc to current fiscal year
            decimal accuredleave = new SLeaveApplication().ListLeaveAppLog().Data.Where(x => x.LeaveApplication.EmployeeId == EmployeeId && x.LeaveApplication.LeaveMasterId == LeaveMasterId && x.FiscalYearId == fiscalyearId)
                .ToList()
                .Sum(x => x.LeaveCount);

            //Leave balance
            try
            {

                SDesignation desigServices = new SDesignation();
                var leaveInfo = db.LeaveMaster.Where(x => x.Id == LeaveMasterId).FirstOrDefault();
                decimal openingLeaveBalancen = 0;
                if (leaveInfo.IsReplacementLeave)
                {
                    var accuredReplacementBalance = db.ReplacementLeaveBalance.Where(x => x.EmployeeId == EmployeeId && x.FyId == fiscalyearId).FirstOrDefault();
                    openingLeaveBalancen = accuredReplacementBalance.RemainingBalance;
                    return new ServiceResult<decimal>()
                    {
                        Data = openingLeaveBalancen,
                        Status = ResultStatus.Ok
                    };
                    //get bal
                }
                else
                {
                    decimal carryForwardBal = new SLeaveSettlement().GetCurrentCarryForwardBal(EmployeeId, LeaveMasterId, fiscalyearId).Data;
                    decimal openingBalance = db.LeaveBalance.Where(x => x.EmployeeId == EmployeeId && x.LeaveMasterId == LeaveMasterId).Select(x => x.OpeningBalance).FirstOrDefault();
                    //ballance for next year after settlement
                    carryForwardBal = carryForwardBal == 0 ? openingBalance : carryForwardBal;
                    SEmploymentStatusWiseLeavedBalance services = new SEmploymentStatusWiseLeavedBalance();
                    SContract contractServices = new SContract();
                    var contract = contractServices.List().Data.Where(x => x.EmployeeId == EmployeeId).ToList();

                    if (comp.EmploymentStatusWiseLeave)
                    {
                        int employmentStatusId = contract.LastOrDefault().EmploymentStatusId;
                        if (leaveInfo.IsLeaveCarryable)
                        {
                            openingLeaveBalancen = carryForwardBal;
                        }
                        else
                        {
                            openingLeaveBalancen = services.List().Data.Where(x => x.LeaveId == LeaveMasterId && x.EmploymentStatusId == employmentStatusId).FirstOrDefault().Balance + carryForwardBal;
                        }
                    }
                    else
                    {
                        #region For bir hospital only
                        if (leaveInfo.IsLeaveCarryable)
                        {
                            openingLeaveBalancen = carryForwardBal;
                        }
                        else
                        {
                            openingLeaveBalancen = desigServices.ListLeaveQouta().Data.Where(x => x.LeaveId == LeaveMasterId && x.DesignationId == employee.DesignationId).FirstOrDefault().Balance + carryForwardBal;
                        }
                        #endregion
                        //openingLeaveBalancen = desigServices.ListLeaveQouta().Data.Where(x => x.LeaveId == LeaveMasterId && x.DesignationId == employee.DesignationId).FirstOrDefault().Balance + carryForwardBal;
                    }
                    return new ServiceResult<decimal>()
                    {
                        Data = openingLeaveBalancen - accuredleave,
                        Status = ResultStatus.Ok
                    };
                }
            }
            catch (Exception)
            {

                return new ServiceResult<decimal>()
                {
                    Data = 0,
                    Status = ResultStatus.processError,
                    Message = "Please Asign Leave Balance And Then Continue."
                };
            }
        }

        public ServiceResult<IQueryable<EleaveApplicationLog>> ListLeaveAppLog()
        {
            return new ServiceResult<IQueryable<EleaveApplicationLog>>()
            {
                Data = db.LeaveApplicationLog,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<bool> Reject(ELeaveApplication model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Status = ResultStatus.Ok,
                Message = "RejectSuccess"
            };
        }
        public ServiceResult<bool> Approve(ELeaveApplication model, int fyId)
        {
            decimal leaveCount;
            if (model.LeaveDay != LeaveDay.FullDay)
            {
                leaveCount = 0.5m;
            }
            else
            {
                leaveCount = (model.To - model.From).Days + 1;
            }

            if (model.LeaveMaster.IsReplacementLeave)
            {
                var repLeaveBal = db.ReplacementLeaveBalance.Where(x => x.FyId == fyId && x.EmployeeId == model.EmployeeId).FirstOrDefault();
                if (repLeaveBal.RemainingBalance < leaveCount)
                {
                    return new ServiceResult<bool>()
                    {
                        Data = false,
                        Message = "LeaveBalExceeded",
                        Status = ResultStatus.processError
                    };
                }
                repLeaveBal.RemainingBalance = repLeaveBal.RemainingBalance - leaveCount;
                db.Entry(repLeaveBal).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var emp = db.Employee.Where(x => x.Id == model.EmployeeId).FirstOrDefault();
                var comp = db.Company.Where(x => x.Id == emp.Branch.CompanyId).FirstOrDefault();
                if (comp.EmploymentStatusWiseLeave)
                {
                    EEmploymentStatusWiseLeavedBalance leaveBalanceobj = new EEmploymentStatusWiseLeavedBalance();
                    var contract = db.Contract.Where(x => x.EmployeeId == model.EmployeeId).ToList();
                    if (contract != null)
                    {
                        int employmentStatusId = contract.LastOrDefault().EmploymentStatusId;
                        leaveBalanceobj = db.EmploymentStatusWiseLeavedBalance.Where(x => x.LeaveId == model.LeaveMasterId && x.EmploymentStatusId == employmentStatusId).FirstOrDefault();
                        if (leaveBalanceobj != null)
                        {
                            if (leaveBalanceobj.Balance < leaveCount)
                            {
                                return new ServiceResult<bool>()
                                {
                                    Data = false,
                                    Message = "LeaveBalExceeded",
                                    Status = ResultStatus.processError
                                };
                            }
                        }
                        else
                        {
                            return new ServiceResult<bool>()
                            {
                                Data = false,
                                Message = "Employment Status Wise Leave Balance not found..",
                                Status = ResultStatus.processError,
                            };
                        }
                    }
                    else
                    {
                        return new ServiceResult<bool>()
                        {
                            Data = false,
                            Message = "Employee not in contract..",
                            Status = ResultStatus.processError,
                        };
                    }

                }
                else
                {
                    int? desigId = db.Employee.Where(x => x.Id == model.EmployeeId).FirstOrDefault().DesignationId;
                    EDesignationWiseLeavedBalance leaveBalance = db.DesignationWiseLeavedBalance.Where(x => x.DesignationId == desigId && x.LeaveId == model.LeaveMasterId).FirstOrDefault();
                    if (leaveBalance.Balance < leaveCount)
                    {
                        return new ServiceResult<bool>()
                        {
                            Data = false,
                            Message = "LeaveBalExceeded",
                            Status = ResultStatus.processError
                        };
                    }
                }

            }

            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            EleaveApplicationLog log = new EleaveApplicationLog()
            {
                FiscalYearId = fyId,
                LeaveApplicationId = model.Id,
                LeaveCount = leaveCount
            };
            db.LeaveApplicationLog.Add(log);
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Message = "ApproveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<bool> RemoveLeaveApplicationLog(EleaveApplicationLog model)
        {
            db.LeaveApplicationLog.Remove(model);
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Status = ResultStatus.Ok,
            };
        }
    }
}
