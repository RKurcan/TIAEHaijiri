using Riddhasoft.DB;
using Riddhasoft.HRM.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services
{
    public class SEmploymentStatusWiseLeavedBalanceHist :Riddhasoft.Services.Common.IBaseService<EEmploymentStatusWiseLeavedBalanceHist>
    {
        RiddhaDBContext db = null;
        public SEmploymentStatusWiseLeavedBalanceHist()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<EEmploymentStatusWiseLeavedBalanceHist> Add(EEmploymentStatusWiseLeavedBalanceHist model)
        {
            db.EmploymentStatusWiseLeavedBalanceHist.Add(model);
            db.SaveChanges();

            return new ServiceResult<EEmploymentStatusWiseLeavedBalanceHist>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EEmploymentStatusWiseLeavedBalanceHist>> List()
        {
            return new ServiceResult<IQueryable<EEmploymentStatusWiseLeavedBalanceHist>>()
            {
                Data = db.EmploymentStatusWiseLeavedBalanceHist,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EEmploymentStatusWiseLeavedBalanceHist model)
        {
            db.EmploymentStatusWiseLeavedBalanceHist.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmploymentStatusWiseLeavedBalanceHist> Update(EEmploymentStatusWiseLeavedBalanceHist model)
        {
            db.Entry<EEmploymentStatusWiseLeavedBalanceHist>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmploymentStatusWiseLeavedBalanceHist>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
