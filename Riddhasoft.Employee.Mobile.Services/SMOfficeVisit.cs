using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;

namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMOfficeVisit
    {
        DB.RiddhaDBContext db;

        public SMOfficeVisit()
        {
            db = new DB.RiddhaDBContext();

        }

        public MobileResult<EOfficeVisit> AddOfficeVisit(EOfficeVisit model)
        {
            db.OfficeVisit.Add(model);
            db.SaveChanges();
            return new MobileResult<EOfficeVisit>()
            {
                Data = model,
                Message = "Office Visit Requested Sucessfully.",
                Status = MobileResultStatus.Ok,
            };
        }
        public MobileResult<EOfficeVisitDetail> AddOfficeVisitDetail(EOfficeVisitDetail model)
        {
            db.OfficeVisitDetail.Add(model);
            db.SaveChanges();
            return new MobileResult<EOfficeVisitDetail>()
            {
                Data = model,
                Message = "Office Visit Requested Sucessfully.",
                Status = MobileResultStatus.Ok,
            };
        }
    }
}
