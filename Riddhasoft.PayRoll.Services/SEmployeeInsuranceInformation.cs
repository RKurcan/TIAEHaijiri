using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SEmployeeInsuranceInformation : Riddhasoft.Services.Common.IBaseService<EEmployeeInsuranceInformation>
    {
        DB.RiddhaDBContext db = null;
        public SEmployeeInsuranceInformation()
        {
            db = new DB.RiddhaDBContext();
        }
        public ServiceResult<EEmployeeInsuranceInformation> Add(EEmployeeInsuranceInformation model)
        {

            db.EmployeeInsuranceInformation.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeInsuranceInformation>()
            {

                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EEmployeeInsuranceInformation>> List()
        {
            return new ServiceResult<IQueryable<EEmployeeInsuranceInformation>>()
            {
                Data =  db.EmployeeInsuranceInformation,
                Status = ResultStatus.Ok

            };
        }

        public ServiceResult<int> Remove(EEmployeeInsuranceInformation model)
        {

            db.EmployeeInsuranceInformation.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status  = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmployeeInsuranceInformation> Update(EEmployeeInsuranceInformation model)
        {

            db.Entry<EEmployeeInsuranceInformation>(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeInsuranceInformation>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
