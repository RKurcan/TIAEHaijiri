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
    public class SEmployeeDesignationHistory : Riddhasoft.Services.Common.IBaseService<EEmployeeDesignationHistory>
    {
        RiddhaDBContext db = null;
        public SEmployeeDesignationHistory()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<EEmployeeDesignationHistory> Add(EEmployeeDesignationHistory model)
        {
            db.EmployeeDesignationHistory.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeDesignationHistory>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok,
            };
        }

        public ServiceResult<IQueryable<EEmployeeDesignationHistory>> List()
        {
            return new ServiceResult<IQueryable<EEmployeeDesignationHistory>>()
            {
                Data = db.EmployeeDesignationHistory,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EEmployeeDesignationHistory model)
        {
            db.EmployeeDesignationHistory.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmployeeDesignationHistory> Update(EEmployeeDesignationHistory model)
        {
            db.Entry<EEmployeeDesignationHistory>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeDesignationHistory>()
            {
                Data = model,
                Message = "UpdateSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
