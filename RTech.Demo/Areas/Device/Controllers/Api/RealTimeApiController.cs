using Riddhasoft.Attendance.Entities;
using Riddhasoft.Attendance.Services;
using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Areas.Device.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Device.Controllers.Api
{
    public class RealTimeApiController : ApiController
    {
        SAttendanceLog attendanceLogServices = null;
        public RealTimeApiController()
        {
            attendanceLogServices = new SAttendanceLog();
        }

       

        [HttpPost]
        public  ServiceResult<bool> Post(RealTimeModel model)
        {
            var user = loginValidated(model.UserName, model.Password, model.CompanyCode);
            //Log.SytemLog("company Code : " + model.CompanyCode);
            if (user!=null)
            {
                SAttendanceLog attendanceLogServices = new SAttendanceLog();
                var result = attendanceLogServices.AddUsingSP(model.CompanyCode, model.EnrollId, model.VerifyMode, model.Date, 0, model.DeviceSN);
                if (result.Data.hasError)
                {
                    return new ServiceResult<bool>()
                    {
                        Data = result.Status == ResultStatus.Ok ? true : false,
                        Status = ResultStatus.processError,
                        Message = result.Data.message
                    };
                }
                return new ServiceResult<bool>()
                {
                    Data = result.Status == ResultStatus.Ok ? true : false,
                    Status = ResultStatus.Ok,
                    Message = result.Data.message
                };
            }
            else
            {
                throw new Exception("Invalid Login");
            }
        }
        
        private EUser loginValidated(string UserName, string Password, string CompanyCode)
        {
            EUser userData = new SUser().List().Data.Where(x => x.Name.ToUpper() == UserName.ToUpper() && x.Password == Password && x.UserType == UserType.User && x.Branch.Company.Code == CompanyCode).FirstOrDefault();
            return userData;

        }

        private int getEmpId(int EnrollId,int? BranchId)
        {
            return (new SEmployee().List().Data.Where(x =>x.BranchId==BranchId && x.DeviceCode == EnrollId ).FirstOrDefault() ?? new EEmployee()).Id;
        }

        private int getDeviceId(string deviceSn)
        {
            EDevice device = new SDevice().List().Data.Where(x => x.SerialNumber == deviceSn).FirstOrDefault() ?? new EDevice();
            return device.Id;
        }

    }
}
