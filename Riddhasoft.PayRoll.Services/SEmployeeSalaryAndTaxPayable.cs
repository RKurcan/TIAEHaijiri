using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SEmployeeSalaryAndTaxPayable : Riddhasoft.Services.Common.IBaseService<EEmployeeSalaryAndTaxPayable>
    {

        Riddhasoft.DB.RiddhaDBContext db = null;
        public SEmployeeSalaryAndTaxPayable()
        {
            db = new DB.RiddhaDBContext();
        }
        public ServiceResult<EEmployeeSalaryAndTaxPayable> Add(EEmployeeSalaryAndTaxPayable model)
        {
            db.EmployeeSalaryAndTaxPayable.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeSalaryAndTaxPayable>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List< EEmployeeSalaryAndTaxPayable>> AddEmployeeSalaryAndTaxPayableList(List< EEmployeeSalaryAndTaxPayable> employeeSalaryAndTaxPayables)
        {
            db.EmployeeSalaryAndTaxPayable.AddRange(employeeSalaryAndTaxPayables);
            db.SaveChanges();
            return new ServiceResult<List<EEmployeeSalaryAndTaxPayable>>()
            {
                Data = employeeSalaryAndTaxPayables,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<EEmployeeSalaryAndTaxPayable>> List()
        {

            return new ServiceResult<IQueryable<EEmployeeSalaryAndTaxPayable>>()
            {
                Data = db.EmployeeSalaryAndTaxPayable,
                Status = ResultStatus.Ok
            };
        }

        
        public ServiceResult<int> Remove(EEmployeeSalaryAndTaxPayable model)
        {

            db.EmployeeSalaryAndTaxPayable.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmployeeSalaryAndTaxPayable> Update(EEmployeeSalaryAndTaxPayable model)
        {
            db.Entry<EEmployeeSalaryAndTaxPayable>(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeSalaryAndTaxPayable>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
