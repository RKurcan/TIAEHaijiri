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
    public class SEmployeeDocument
    {
        RiddhaDBContext db = null;
        public SEmployeeDocument()
        {
            db = new RiddhaDBContext();
        }
        public ServiceResult<EEmployeeDocument> Add(EEmployeeDocument model)
        {
            db.EmployeeDocument.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeDocument>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EEmployeeDocument> Update(EEmployeeDocument model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeDocument>()
            {
                Data = model,
                Message = "UpdateSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<int> Remove(int id)
        {
            EEmployeeDocument doc = db.EmployeeDocument.Find(id);
            db.EmployeeDocument.Remove(doc);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<EEmployeeDocument>> List()
        {
            return new ServiceResult<IQueryable<EEmployeeDocument>>()
            {
                Data = db.EmployeeDocument,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EEmployeeDocument> Find(int id)
        {
            EEmployeeDocument doc = db.EmployeeDocument.Find(id);
            return new ServiceResult<EEmployeeDocument>()
            {
                Data=doc,
                Status=ResultStatus.Ok
            };
        }

    }
}
