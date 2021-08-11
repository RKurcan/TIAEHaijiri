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
    public class SDeduction
    {
         RiddhaDBContext db = null;
         public SDeduction()
        {
            db = new RiddhaDBContext();
        }
        public ServiceResult<IQueryable<EDeduction>> List()
        {
            return new ServiceResult<IQueryable<EDeduction>>()
            {
                Data = db.Deduction,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EDeduction> Add(EDeduction model)
        {
            db.Deduction.Add(model);
            db.SaveChanges();
            return new ServiceResult<EDeduction>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "AddedSuccess"
            };
        }
        public ServiceResult<EDeduction> Update(EDeduction model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDeduction>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "UpdatedSuccess"
            };
        }
        public ServiceResult<int> Remove(EDeduction model)
        {
            db.Deduction.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Status = ResultStatus.Ok,
                Message = "RemoveSuccess"
            };
        }
        public ServiceResult<IQueryable<EEmployeeDeduction>> ListEmpDeduction()
        {
            return new ServiceResult<IQueryable<EEmployeeDeduction>>()
            {
                Data = db.EmployeeDeduction,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmployeeDeduction> AddEmpDeduction(EEmployeeDeduction model)
        {
            db.EmployeeDeduction.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeDeduction>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "AddedSuccess"
            };
        }

        public ServiceResult<EEmployeeDeduction> UpdateEmpDeduction(EEmployeeDeduction model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeDeduction>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "UpdatedSuccess"
            };
        }
        public ServiceResult<int> RemoveEmpDeduction(EEmployeeDeduction model)
        {
            db.EmployeeDeduction.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Status = ResultStatus.Ok,
                Message = "RemoveSuccess"
            };
        }
    }
}
