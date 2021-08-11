using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;

namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMKaj
    {
        DB.RiddhaDBContext db;
        public SMKaj()
        {
            db = new DB.RiddhaDBContext();
        }
        public MobileResult<EKaj> AddKaj(EKaj model)
        {
            db.Kaj.Add(model);
            db.SaveChanges();
            return new MobileResult<EKaj>()
            {
                Data = model,
                Message = "Kaj Requested Sucessfully.",
                Status = MobileResultStatus.Ok,
            };
        }
        public MobileResult<EKajDetail> AddKajDetail(EKajDetail model)
        {
            db.KajDetail.Add(model);
            db.SaveChanges();
            return new MobileResult<EKajDetail>()
            {
                Data = model,
                Message = "Kaj Requested Sucessfully.",
                Status = MobileResultStatus.Ok,
            };
        }
    }
}
