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
    public class SLateNote : Riddhasoft.Services.Common.IBaseService<ELateNote>
    {
        RiddhaDBContext db = null;
        public SLateNote()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<ELateNote> Add(ELateNote model)
        {
            db.LateNote.Add(model);
            db.SaveChanges();
            return new ServiceResult<ELateNote>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<ELateNote>> List()
        {
            return new ServiceResult<IQueryable<ELateNote>>()
            {
                Data = db.LateNote,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(ELateNote model)
        {
            db.LateNote.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<ELateNote> Update(ELateNote model)
        {
            db.Entry<ELateNote>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ELateNote>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
}
