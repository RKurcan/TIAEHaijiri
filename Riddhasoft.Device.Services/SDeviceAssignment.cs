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
    public class SDeviceAssignment
    {
        RiddhaDBContext db = null;
        public SDeviceAssignment()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EDeviceAssignment>> List()
        {
            return new ServiceResult<IQueryable<EDeviceAssignment>>()
            {
                Data = db.DeviceAssignment,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<List<EDeviceAssignment>> Add(List<EDeviceAssignment> model)
        {
            db.DeviceAssignment.AddRange(model);
            db.SaveChanges();

            foreach (var item in model)
            {
                EDevice device = db.Device.Find(item.DeviceId);
                device.Status = Status.Reseller;
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
            }
            return new ServiceResult<List<EDeviceAssignment>>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDeviceAssignment> Update(EDeviceAssignment model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDeviceAssignment>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EDeviceAssignment model)
        {
            db.DeviceAssignment.Remove(model);
            db.SaveChanges();

            //change device status
            EDevice device = db.Device.Find(model.DeviceId);
            device.Status = Status.New;
            db.Entry(device).State = EntityState.Modified;
            db.SaveChanges();

            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Removed Successfully",
                Status = ResultStatus.Ok
            };
        }
    }
}
