using Riddhasoft.DB;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;

namespace Riddhasoft.Report
{
    public class SMonthWiseExpiredCompanyReport
    {
        Riddhasoft.DB.RiddhaDBContext db;
        public SMonthWiseExpiredCompanyReport()
        {
            db = new DB.RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<List<ExpiredCompanyVm>> GetReport(DateTime FromDate, DateTime ToDate)
        {
            var result = db.SP_GET_Month_Wise_Expired_Company(FromDate, ToDate);
            return new Services.Common.ServiceResult<List<ExpiredCompanyVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok,
            };

        }
    }

}
