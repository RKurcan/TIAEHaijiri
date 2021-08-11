using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SDesktopProductKey : Riddhasoft.Services.Common.IBaseService<EDesktopProductKey>
    {
        RiddhaDBContext db = null;
        public SDesktopProductKey()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<EDesktopProductKey> Add(EDesktopProductKey model)
        {
            db.DesktopProductKey.Add(model);
            db.SaveChanges();
            return new ServiceResult<EDesktopProductKey>()
            {
                Data = model,
                Message = "Added Successfully.",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EDesktopProductKey>> List()
        {
            return new ServiceResult<IQueryable<EDesktopProductKey>>()
            {
                Data = db.DesktopProductKey,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(EDesktopProductKey model)
        {
            db.DesktopProductKey.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Remove Sucessfully.",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EDesktopProductKey> Update(EDesktopProductKey model)
        {
            db.Entry<EDesktopProductKey>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDesktopProductKey>()
            {
                Data = model,
                Message = "Updated Sucessfully",
                Status = ResultStatus.Ok
            };
        }
    }
}
