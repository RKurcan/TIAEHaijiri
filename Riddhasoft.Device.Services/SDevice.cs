using Riddhasoft.DB;
using Riddhasoft.Device.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.Device.Services
{
    public class SDevice : Riddhasoft.Services.Common.IBaseService<EDevice>
    {
        RiddhaDBContext db = null;
        public SDevice()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EDevice>> List()
        {
            return new ServiceResult<IQueryable<EDevice>>()
            {
                Data = db.Device,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDevice> Add(EDevice model)
        {
            db.Device.Add(model);
            db.SaveChanges();
            return new ServiceResult<EDevice>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<List<EDevice>> AddRange(List<EDevice> list)
        {
            db.Device.AddRange(list);
            db.SaveChanges();
            return new ServiceResult<List<EDevice>>()
            {
                Data = list,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDevice> Update(EDevice model)
        {
            model.Model = null;
            var obj = db.Device.Find(model.Id);
            if (obj.IsFaceDevice)
            {
                model.IsFaceDevice = true;
            };
            if (obj.IsAccessDevice)
            {
                model.IsAccessDevice = true;
            }
            obj.ModelId = model.ModelId;
            obj.DeviceType = model.DeviceType;
            obj.LastActivity = model.LastActivity;
            obj.SerialNumber = model.SerialNumber;
            obj.IpAddress = model.IpAddress;
            db.Entry<EDevice>(obj).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDevice>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EDevice model)
        {
            db.Device.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Removed Successfully",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<bool> UpdateCompanyDevice(List<EDevice> list)
        {
            var devices = db.Device;
            foreach (var item in list)
            {
                EDevice device = new EDevice();
                device = devices.Where(x => x.Id == item.Id).FirstOrDefault();
                device.Name = item.Name;
                device.IpAddress = item.IpAddress;
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
            }
            return new ServiceResult<bool>()
            {
                Data = true,
                Status = ResultStatus.Ok,
                Message = "UpdatedSuccess"
            };
        }

        public ServiceResult<bool> UpdateDevice(EDevice model)
        {
            db.Entry<EDevice>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public EDevice Find(int deviceId)
        {
            return db.Device.Find(deviceId);
        }
    }
}
