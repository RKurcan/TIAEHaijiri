using Riddhasoft.DB;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SPayRollSetup : Riddhasoft.Services.Common.IBaseService<EPayRollSetup>
    {
        RiddhaDBContext db = null;
        public SPayRollSetup()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EPayRollSetup>> List()
        {
            return new ServiceResult<IQueryable<EPayRollSetup>>()
            {
                Data = db.PayRollSetup,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EPayRollSetup> Add(EPayRollSetup model)
        {
            var existingPayrolls = db.PayRollSetup.Where(x => x.EmployeeId == model.EmployeeId).ToList();
            if (existingPayrolls.Count() > 0)
            {
                var payrollToUpdate = existingPayrolls.Where(x => x.EndDate == null).FirstOrDefault();
                if (payrollToUpdate != null)
                {
                    payrollToUpdate.EndDate = model.EffectedFrom;
                    db.Entry(payrollToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            db.PayRollSetup.Add(model);
            db.SaveChanges();
            model.Employee = db.Employee.Find(model.EmployeeId);
            return new ServiceResult<EPayRollSetup>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EPayRollSetup> Update(EPayRollSetup model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EPayRollSetup>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EPayRollSetup model)
        {
            db.PayRollSetup.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Remove Successfully",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<UnapprovedPayrollCountModel> GetUnapprovedCount(int branchId)
        {
            UnapprovedPayrollCountModel model = new UnapprovedPayrollCountModel();
            model.Allowance = db.EmployeeAlowance.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.Deduction = db.EmployeeDeduction.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.Payroll = db.PayRollSetup.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.GradeGroup = db.EmployeeGrade.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            return new ServiceResult<UnapprovedPayrollCountModel>()
            {
                Data = model,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        #region Employee Grade upgrade service
        public ServiceResult<object> GradeUpgradeEmployee(EmpGradeUpgradeModel model)
        {
            //validate Effected date
            var query = from c in db.EmployeeGrade.Where(x => x.BranchId == model.EmployeeGrade.BranchId).ToList()
                        join d in model.EmpIds on c.EmployeeId equals d
                        where c.EffectedTo.Date >= model.EmployeeGrade.EffectedFrom
                        select c;

            if (!query.Any())
            {
                List<EEmployeeGrade> empGradeLst = new List<EEmployeeGrade>();
                for (int i = 0; i < model.EmpIds.Count(); i++)
                {
                    empGradeLst.Add(new EEmployeeGrade()
                    {
                        BranchId = model.EmployeeGrade.BranchId,
                        CreatedById = model.EmployeeGrade.CreatedById,
                        CreatedOn = model.EmployeeGrade.CreatedOn,
                        EffectedFrom = model.EmployeeGrade.EffectedFrom,
                        EffectedTo = model.EmployeeGrade.EffectedTo,
                        EmployeeId = model.EmpIds[i],
                        GradeGroupId = model.EmployeeGrade.GradeGroupId
                    });
                }
                db.EmployeeGrade.AddRange(empGradeLst);
                db.SaveChanges();
                return new ServiceResult<object>()
                {
                    Message = "Upgraded Successfully",
                    Status = ResultStatus.Ok
                };
            }
            else
            {
                return new ServiceResult<object>()
                {
                    Message = "Invalid Date to Upgrade Employee",
                    Status = ResultStatus.processError
                };
            }

        }

        //public ServiceResult<object> GradeUpgradeEmployee(EmpGradeUpgradeModel model)
        //{
        //    List<EEmployeeGrade> Employees = db.EmployeeGrade.Where(x => x.BranchId == model.EmployeeGrade.BranchId).ToList();
        //    List<EEmployeeGrade> empGradeLst = new List<EEmployeeGrade>();
        //    List<EEmployeeGrade> empGradeLstToUpdate = new List<EEmployeeGrade>();
        //    for (int i = 0; i < model.EmpIds.Count(); i++)
        //    {
        //        var emp = Employees.Where(x => x.EmployeeId == model.EmpIds[i] && x.GradeGroupId == model.EmployeeGrade.GradeGroupId).OrderByDescending(x=> x.EffectedTo).FirstOrDefault();
        //        if(emp != null)
        //        {
        //            emp.EffectedTo = model.EmployeeGrade.EffectedTo;
        //            db.Entry(emp).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            empGradeLst.Add(new EEmployeeGrade()
        //            {
        //                BranchId = model.EmployeeGrade.BranchId,
        //                CreatedById = model.EmployeeGrade.CreatedById,
        //                CreatedOn = model.EmployeeGrade.CreatedOn,
        //                EffectedFrom = model.EmployeeGrade.EffectedFrom,
        //                EffectedTo = model.EmployeeGrade.EffectedTo,
        //                EmployeeId = model.EmpIds[i],
        //                GradeGroupId = model.EmployeeGrade.GradeGroupId
        //            });
        //        }               
        //    }
        //    db.EmployeeGrade.AddRange(empGradeLst);
        //    db.SaveChanges();
        //    return new ServiceResult<object>()
        //        {
        //            Message = "Grade updated successfully",
        //            Status = ResultStatus.Ok
        //        };
        //}
        public ServiceResult<IQueryable<EEmployeeGrade>> ListEmpGradeGroup()
        {
            //var EmpGradeGroup = db.EmployeeGrade;
            //var GradeGroups = db.GradeGroup;
            //var result = (from c in EmpGradeGroup
            //              join d in GradeGroups
            //                  on c.GradeGroupId equals d.Id
            //              select new EmployeeGradeVm()
            //              {
            //                  Id = c.Id,
            //                  EmployeeId = c.EmployeeId,
            //                  GradeGroupId = c.GradeGroupId,
            //                  EffectedFrom = c.EffectedFrom,
            //                  EffectedTo = c.EffectedTo,
            //                  CreatedById = c.CreatedById,
            //                  CreatedOn = c.CreatedOn,
            //                  IsApproved = c.IsApproved,
            //                  BranchId = c.BranchId,
            //                  GradeGroupName = d.Name,
            //                  ApprovedById = c.ApprovedById,
            //                  ApprovedOn = c.ApprovedOn,
            //                  Employee = c.Employee,
            //              });
            return new ServiceResult<IQueryable<EEmployeeGrade>>()
            {
                Data = db.EmployeeGrade,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmployeeGrade> UpdateEmpGradeGroup(EEmployeeGrade model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeGrade>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "Updated Successfully"
            };
        }

        public ServiceResult<int> RemoveEmpGradeGroup(EEmployeeGrade model)
        {
            db.EmployeeGrade.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Remove Successfully",
                Status = ResultStatus.Ok
            };
        }
        #endregion

    }
    public class UnapprovedPayrollCountModel
    {
        public int Payroll { get; set; }
        public int Allowance { get; set; }
        public int Deduction { get; set; }
        public int GradeGroup { get; set; }
    }
    public class EmpGradeUpgradeModel
    {
        public int[] EmpIds { get; set; }
        public EEmployeeGrade EmployeeGrade { get; set; }
    }

    public class EmployeeGradeVm : EEmployeeGrade
    {
        public string GradeGroupName { get; set; }
    }
}
