using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class STaxSetup : Riddhasoft.Services.Common.IBaseService<ETaxSetup>
    {
        Riddhasoft.DB.RiddhaDBContext db = null;
        public STaxSetup()
        {
            db = new DB.RiddhaDBContext();
        }

        #region Tax Setup

        public ServiceResult<ETaxSetup> Add(ETaxSetup model)
        {
            db.TaxSetup.Add(model);
            db.SaveChanges();
            return new ServiceResult<ETaxSetup>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<ETaxSetup>> List()
        {
            return new ServiceResult<IQueryable<ETaxSetup>>()
            {
                Data = db.TaxSetup,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(ETaxSetup model)
        {
            db.TaxSetup.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<ETaxSetup> Update(ETaxSetup model)
        {
            db.Entry<ETaxSetup>(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ETaxSetup>()
            {

                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }


        #endregion

        #region Tax Slab
        public ServiceResult<List<ETaxSlabDetails>> AddTaxSlabDetails(List<ETaxSlabDetails> taxSlabDetails)
        {

            db.TaxSlabDetails.AddRange(taxSlabDetails);
            db.SaveChanges();
            return new ServiceResult<List<ETaxSlabDetails>>()
            {
                Data = taxSlabDetails,
                Message = "AddedSuccess"

            };
        }

        public ServiceResult<IQueryable<ETaxSlabDetails>> GetTaxSlabDetails() {

            return new ServiceResult<IQueryable<ETaxSlabDetails>>()
            {
                Data = db.TaxSlabDetails,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> DeleteTaxSlabDetails(List<ETaxSlabDetails> taxSlabDetails)
        {

            db.TaxSlabDetails.RemoveRange(taxSlabDetails);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
            
        }
        #endregion


    }
}
