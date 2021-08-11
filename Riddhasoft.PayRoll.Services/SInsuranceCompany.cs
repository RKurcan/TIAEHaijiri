using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SInsuranceCompany : Riddhasoft.Services.Common.IBaseService<EInsuranceCompany>
    {

        DB.RiddhaDBContext db = null;
        public SInsuranceCompany()
        {
            db = new DB.RiddhaDBContext();
        }
        public ServiceResult<EInsuranceCompany> Add(EInsuranceCompany model)
        {
            db.InsuranceCompany.Add(model);
            db.SaveChanges();
            return new ServiceResult<EInsuranceCompany>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EInsuranceCompany>> List()
        {

            return new ServiceResult<IQueryable<EInsuranceCompany>>()
            {
                Data = db.InsuranceCompany,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EInsuranceCompany model)
        {
            db.InsuranceCompany.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EInsuranceCompany> Update(EInsuranceCompany model)
        {

            db.Entry<EInsuranceCompany>(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EInsuranceCompany>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
