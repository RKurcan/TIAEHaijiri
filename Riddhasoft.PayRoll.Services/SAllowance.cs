using Riddhasoft.DB;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SAllowance
    {
        RiddhaDBContext db = null;
        public SAllowance()
        {
            db = new RiddhaDBContext();
        }
        public ServiceResult<IQueryable<EAllowance>> List()
        {
            return new ServiceResult<IQueryable<EAllowance>>()
            {
                Data = db.Allowance,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EAllowance> Add(EAllowance model)
        {
            db.Allowance.Add(model);
            db.SaveChanges();
            return new ServiceResult<EAllowance>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "AddedSuccess"
            };
        }
        public ServiceResult<EAllowance> Update(EAllowance model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EAllowance>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "UpdatedSuccess"
            };
        }
        public ServiceResult<int> Remove(EAllowance model)
        {
            //db.Allowance.Remove(model);
            //db.SaveChanges();
            //return new ServiceResult<int>()
            //{
            //    Data = 1,
            //    Status = ResultStatus.Ok,
            //    Message = "RemoveSuccess"
            //};
            try
            {
                int allowanceApplication = db.EmployeeAlowance.Count(x => x.AllowanceId == model.Id);
                if (allowanceApplication > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("This allowance is used in {0} {1}. Allowance can't be deleted.", allowanceApplication, allowanceApplication == 1?"payroll":"payrolls"),
                        Status = ResultStatus.dataBaseError
                    };
                }
                db.Allowance.Remove(model);
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
        public ServiceResult<IQueryable<EEmployeeAlowance>> ListEmpAllowance()
        {
            return new ServiceResult<IQueryable<EEmployeeAlowance>>()
            {
                Data = db.EmployeeAlowance,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmployeeAlowance> AddEmpAllowance(EEmployeeAlowance model)
        {
            db.EmployeeAlowance.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeAlowance>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "AddedSuccess"
            };
        }

        public ServiceResult<EEmployeeAlowance> UpdateEmpAllowance(EEmployeeAlowance model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeAlowance>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "UpdatedSuccess"
            };
        }
        public ServiceResult<int> RemoveEmpAllowance(EEmployeeAlowance model)
        {
            db.EmployeeAlowance.Remove(model);
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
