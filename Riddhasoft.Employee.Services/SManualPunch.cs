using Riddhasoft.Attendance.Entities;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Services
{
    public class SManualPunch : Riddhasoft.Services.Common.IBaseService<Riddhasoft.Employee.Entities.EManualPunch>
    {
        Riddhasoft.DB.RiddhaDBContext db = null;
        public SManualPunch()
        {
            db = new Riddhasoft.DB.RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<Entities.EManualPunch>> List()
        {
            return new ServiceResult<IQueryable<Entities.EManualPunch>>()
            {
                Data = db.ManualPunch,
                Message = "",
                Status = ResultStatus.Ok

            };
        }

        public Riddhasoft.Services.Common.ServiceResult<Entities.EManualPunch> Add(Entities.EManualPunch model)
        {
            model = db.ManualPunch.Add(model);
            //var employeeData = db.Employee.Find(model.EmployeeId);
            db.AttendanceLog.Add(new EAttendanceLog()
            {

                EmployeeId = model.EmployeeId,
                DeviceId = model.CompanyId ?? 0,
                DateTime = model.DateTime,
                VerifyMode = 0,
                Remark = "Manual"
            });
            db.SaveChanges();
            return new ServiceResult<EManualPunch>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<Entities.EManualPunch> Update(Entities.EManualPunch model)
        {
            model.Employee = null;
            model.Company = null;
            model.Branch = null;
            model.CompanyId = null;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            db.AttendanceLog.Add(new EAttendanceLog()
            {

                EmployeeId = model.EmployeeId,
                DeviceId = model.CompanyId ?? 0,
                DateTime = model.DateTime,
                VerifyMode = 0,
                Remark = "Manual"
            });
            db.SaveChanges();

            return new ServiceResult<EManualPunch>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(Entities.EManualPunch model)
        {
            model = db.ManualPunch.Remove(db.ManualPunch.Find(model.Id));
            db.SaveChanges();
            EAttendanceLog data = db.AttendanceLog.Where(x => x.EmployeeId == model.EmployeeId && x.DateTime == model.DateTime).FirstOrDefault();
            if (data!=null)
            {
                db.AttendanceLog.Remove(data);
                db.SaveChanges();
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(Entities.EManualPunch model, DateTime PreviousDateTime)
        {

            model = db.ManualPunch.Remove(db.ManualPunch.Find(model.Id));
            db.SaveChanges();

            var employeeData = db.Employee.Find(model.EmployeeId);
            EAttendanceLog data = db.AttendanceLog.Where(x => x.DeviceId == employeeData.DeviceCode && x.DateTime == PreviousDateTime).FirstOrDefault();
            if (data!=null)
            {
                db.AttendanceLog.Remove(data);
                db.SaveChanges();  
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public void Add(EAttendanceLog data)
        {
            db.AttendanceLog.Add(data);
            db.SaveChanges();
        }

    }
}
