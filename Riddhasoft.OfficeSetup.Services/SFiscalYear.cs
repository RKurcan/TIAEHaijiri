using Riddhasoft.DB;
using Riddhasoft.Services.Common;
using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SFiscalYear : Riddhasoft.Services.Common.IBaseService<EFiscalYear>
    {
        RiddhaDBContext db = null;
        public SFiscalYear()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EFiscalYear>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EFiscalYear>>()
            {
                Data = db.FiscalYear,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EFiscalYear> Add(EFiscalYear model)
        {
            db.FiscalYear.Add(model);
            db.SaveChanges();
            return new ServiceResult<EFiscalYear>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EFiscalYear> Update(EFiscalYear model)
        {
            if (model.CurrentFiscalYear)
            {
                var currentFy = db.FiscalYear.Where(x => x.CurrentFiscalYear == true && x.BranchId == model.BranchId && x.Id != model.Id).FirstOrDefault();
                if (currentFy != null)
                {
                    currentFy.CurrentFiscalYear = false;
                    db.Entry(currentFy).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EFiscalYear>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EFiscalYear model)
        {
            db.FiscalYear.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
