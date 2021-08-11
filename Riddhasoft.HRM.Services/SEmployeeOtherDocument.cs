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
    public class SEmployeeOtherDocument
    {
        RiddhaDBContext db = null;
        public SEmployeeOtherDocument()
        {
            db = new RiddhaDBContext();
        }
        public ServiceResult<IQueryable<EEmployeeOtherDocument>> List()
        {
            return new ServiceResult<IQueryable<EEmployeeOtherDocument>>()
            {
                Data = db.EmployeeOtherDocument,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EEmployeeOtherDocument> Add(EEmployeeOtherDocument model)
        {
            db.EmployeeOtherDocument.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeOtherDocument>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EEmployeeOtherDocument> Update(EEmployeeOtherDocument model)
        {
            db.Entry<EEmployeeOtherDocument>(model).State= EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeOtherDocument>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<int>Remove(EEmployeeOtherDocument model)
        {
            db.EmployeeOtherDocument.Remove(model);
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
