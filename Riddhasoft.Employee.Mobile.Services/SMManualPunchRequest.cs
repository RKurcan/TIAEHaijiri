using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;

namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMManualPunchRequest
    {
        DB.RiddhaDBContext db;
        public SMManualPunchRequest()
        {
            db = new DB.RiddhaDBContext();
        }

        public MobileResult<EManualPunchRequest> Add(EManualPunchRequest model)
        {
            db.ManualPunchRequest.Add(model);
            db.SaveChanges();
            return new MobileResult<EManualPunchRequest>()
            {
                Data = model,
                Message = "Manual Punch Requested Sucessfully.",
                Status = MobileResultStatus.Ok,
            };
        }
    }
}
