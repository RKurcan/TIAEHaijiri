using Riddhasoft.Attendance.Entities;
using Riddhasoft.DB;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Attendance.Services
{
    public class SAttendanceLog : Riddhasoft.Services.Common.IBaseService<EAttendanceLog>
    {
        RiddhaDBContext db = null;
        public SAttendanceLog()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EAttendanceLog>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EAttendanceLog>>()
            {
                Data = db.AttendanceLog,
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EAttendanceLog> Add(EAttendanceLog model)
        {
            //var exist = db.AttendanceLog.Where(x => x.EmployeeId == model.EmployeeId && x.DateTime == model.DateTime && x.VerifyMode == model.VerifyMode).ToList();
            //foreach (var item in exist)
            //{
            //    db.AttendanceLog.Remove(item);
            //}
            model = db.AttendanceLog.Add(model);
            db.SaveChanges();
            return new ServiceResult<EAttendanceLog>()
            {
                Data = model,
                Message = "Added Successfully.",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<PostAttendanceLogResult> AddUsingSP(string branchCode, int userPin, int verifyMode, DateTime verifyTime, decimal temperature, string deviceSN)
        {
            //var exist = db.AttendanceLog.Where(x => x.EmployeeId == model.EmployeeId && x.DateTime == model.DateTime && x.VerifyMode == model.VerifyMode).ToList();
            //foreach (var item in exist)
            //{
            //    db.AttendanceLog.Remove(item);
            //}
            var result = db.SP_Post_AttendanceData(branchCode, userPin, verifyMode, verifyTime, temperature, deviceSN).FirstOrDefault();
            db.SaveChanges();
            return new ServiceResult<PostAttendanceLogResult>()
            {
                Data = result,
                Message = "Added Successfully.",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<EAttendanceLog> Update(EAttendanceLog model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EAttendanceLog>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EAttendanceLog model)
        {
            //model = db.GLogData.Remove(db.GLogData.Find(model.Id));
            //no hard delete for glog data.
            var data = db.AttendanceLog.Find(model.Id);
            data.IsDelete = true;
            var result = Update(data);
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Removed Successfully",
                Status = result.Status
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Recover(EAttendanceLog model)
        {
            var data = db.AttendanceLog.Find(model.Id);
            data.IsDelete = false;
            var result = Update(data);
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Recovered Successfully",
                Status = result.Status
            };
        }

        public void AddRange(List<EAttendanceLog> attLogs)
        {


            db.AttendanceLog.AddRange(attLogs);
            db.SaveChanges();



        }
    }
}
