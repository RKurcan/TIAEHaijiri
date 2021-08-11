using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;

namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMOfficeVisitRequest
    {
        DB.RiddhaDBContext db;

        public SMOfficeVisitRequest()
        {
            db = new DB.RiddhaDBContext();

        }

        public MobileResult<EOfficeVisitRequest> AddOfficeVisitRequest(EOfficeVisitRequest model)
        {
            db.OfficeVisitRequest.Add(model);
            db.SaveChanges();
            return new MobileResult<EOfficeVisitRequest>()
            {
                Data = model,
                Message = "Office Visit Requested Sucessfully.",
                Status = MobileResultStatus.Ok,
            };
        }
    }
}
