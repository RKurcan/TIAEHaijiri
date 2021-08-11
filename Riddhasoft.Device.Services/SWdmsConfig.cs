using Riddhasoft.DB;
using Riddhasoft.Device.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Device.Services
{
    public class SWdmsConfig
    {
        RiddhaDBContext db = null;
        public SWdmsConfig()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<EWdmsConfig> Get()
        {
            var data = db.WdmsConfig.FirstOrDefault();
            if (data == null)
            {
                data = new EWdmsConfig()
                {
                    Url = "http://202.51.74.187:8081",
                    UserName = "admin",
                    Password = "wdmsR!dB@r"
                };
                db.WdmsConfig.Add(data);
                db.SaveChanges();
            };
            return new Riddhasoft.Services.Common.ServiceResult<EWdmsConfig>()
            {
                Data = data,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EWdmsConfig> Update(EWdmsConfig model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EWdmsConfig>()
            {
                Data = model,
                Status = ResultStatus.Ok
            };
        }
    }
}
