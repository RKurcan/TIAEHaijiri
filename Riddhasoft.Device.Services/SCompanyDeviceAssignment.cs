using Riddhasoft.DB;
using Riddhasoft.Device.Entities;
using Riddhasoft.Services.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.Device.Services
{
    public class SCompanyDeviceAssignment
    {
        RiddhaDBContext db = null;
        public SCompanyDeviceAssignment()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ECompanyDeviceAssignment>> List()
        {
            return new ServiceResult<IQueryable<ECompanyDeviceAssignment>>()
            {
                Data = db.CompanyDeviceAssignment,
                Message = "",
                Status = ResultStatus.Ok

            };
        }

        public Riddhasoft.Services.Common.ServiceResult<List<ECompanyDeviceAssignment>> Add(List<ECompanyDeviceAssignment> model)
        {
            db.CompanyDeviceAssignment.AddRange(model);
            db.SaveChanges();
            foreach (var item in model)
            {
                EDevice device = db.Device.Find(item.DeviceId);
                device.Status = Status.Customer;
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
            }

            return new ServiceResult<List<ECompanyDeviceAssignment>>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ECompanyDeviceAssignment> Update(ECompanyDeviceAssignment model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ECompanyDeviceAssignment>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ECompanyDeviceAssignment model)
        {
            db.CompanyDeviceAssignment.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Removed Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<List<ECompanyDeviceAssignment>> GetCompanyDevices(int companyId)
        {
            var data = db.CompanyDeviceAssignment.Where(x => x.CompanyId == companyId).ToList();
            return new ServiceResult<List<ECompanyDeviceAssignment>>()
            {
                Data = data,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }
}
