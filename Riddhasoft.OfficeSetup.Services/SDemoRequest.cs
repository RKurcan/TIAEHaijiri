using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SDemoRequest : IBaseService<EDemoRequest>
    {
        private RiddhaDBContext db = null;

        public SDemoRequest()
        {
            db = new RiddhaDBContext();
        }
        public ServiceResult<IQueryable<EDemoRequest>> List()
        {
            return new ServiceResult<IQueryable<EDemoRequest>>()
            {
                Data = db.DemoRequest,
                Status = ResultStatus.Ok,
                Message = ""
            };
        }

        public ServiceResult<EDemoRequest> Add(EDemoRequest model)
        {
            db.DemoRequest.Add(model);
            db.SaveChanges();
            return new ServiceResult<EDemoRequest>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EDemoRequest> Update(EDemoRequest model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDemoRequest>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EDemoRequest model)
        {
            db.DemoRequest.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 0,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
