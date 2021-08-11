using Riddhasoft.Attendance.Entities;
using Riddhasoft.Attendance.Services;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Controllers.ADMS
{
    public class ADMSApiController : ApiController
    {
        Utilities.WebrequestService webrequest = new WebrequestService(RiddhaSession.ADMSUrl);
        string branchCode = RiddhaSession.BranchCode;
        /**
    * Remove the device and all data related to the device from the server.
    * @return
    * DeleteServerData
    */
        [HttpGet]
        public ServiceResult<string> deleteDevice(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            string[] sns = sn.Split(',');
            var result = webrequest.Get<string>("/Api/HomeApi/deleteDevice?sn=" + sn).Result;
            //return "deviceList";
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        /**
   * According to the conditions of interface,delete attendance records from the specified device.
   * Corresponding to the "CLEAR LOG" command.
   * @return
   * --DeleteAttendanceLog
   */
        [HttpGet]
        public ServiceResult<string> ClearAttLogFromDevice(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            //api call
            string[] sns = sn.Split(',');
            var result = webrequest.Get<string>("/Api/HomeApi/ClearAttLogFromDevice?sn=" + sn).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Status = ResultStatus.Ok,
                Message = "",
            };
        }
        /**
    * According to the conditions of interface,delete attendance photo from the specified device.
    * Corresponding to the "CLEAR PHOTO" command.
    * @return
    */
        [HttpGet]
        public ServiceResult<string> ClearPhotoFromDevice(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            //
            //api call
            string[] sns = sn.Split(',');
            var result = webrequest.Get<string>("/Api/HomeApi/ClearPhotoFromDevice?sn=" + sn).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Status = ResultStatus.Ok,
                Message = "",
            };

        }

        /*DeleteDeviceData*/
        [HttpGet]
        public ServiceResult<string> ClearAllData(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            string[] sns = sn.Split(',');
            var result = webrequest.Get<string>("/Api/HomeApi/ClearAllData?sn=" + sn).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Status = ResultStatus.Ok,
                Message = "",
            };
        }

        /**
    * According to the conditions of interface,reboot specified device.
    * Corresponding to the "REBOOT" command.
    * @return
    */
        [HttpGet]
        public ServiceResult<string> RebootDevice(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            //api call
            string[] sns = sn.Split(',');
            var result = webrequest.Get<string>("/Api/HomeApi/RebootDevice?sn=" + sn).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok,
            };
        }

        /**
  * According to the list of device serial numbers from pages,send the individual device data to they own device in the server.Corresponding to the "DATA UPDATE" command.
  * @return
  */
        [HttpGet]
        public ServiceResult<string> RestoreUserInfo(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            //logger.info("get restore request begin make cmd");
            var result = webrequest.Get<string>("/Api/HomeApi/RestoreUserInfo?sn=" + sn + "&userInfoOnly=Yes").Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        /**
   * Synchronize software data to device (user information/biometrics/photos/bioPothos) and advertisement
   * @return
   */
        [HttpGet]
        public ServiceResult<string> syncDevice(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            //
            //api call
            string[] sns = sn.Split(',');
            var result = webrequest.Get<ServiceResult<string>>("/Api/HomeApi/syncDevice?sn=" + sn).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        /*
         Add And Update Device Information
             
             */
        [HttpPost]
        public ServiceResult<string> UpdateDevice(DeviceUpdateVm model)
        {
            if (string.IsNullOrEmpty(model.SN))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }

            //api call
            //string[] sns = sn.Split(',');
            DeviceGridVm admsVm = new DeviceGridVm()
            {
                SN = model.SN,
                BranchCode = RiddhaSession.BranchCode,
                DepartmentCode = model.DeptNo,
                IsAccessDevice = model.IsAccess,
                IsFaceDevice = model.IsFace
            };
            var result = webrequest.Post<string>("/Api/HomeApi/UpdateDevice", webrequest.SerializeObject<DeviceGridVm>(admsVm)).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        /**
  * According to the list of device serial numbers from pages,Create clear all data of device commands "CHECK" to whole device,which different serial number.
  * If get checknew parameter from page,send CHECK command issued directly.Otherwise, Set 0 to the Stamp. eg.(Stamp = 0)
  * @return
  */
        [HttpGet]
        public ServiceResult<string> checkDeviceData(string sn, string checkNew = null)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            //
            //api call
            string[] sns = sn.Split(',');
            var result = webrequest.Get<ServiceResult<string>>("/Api/AdmsEx/checkDeviceData?sn=" + sn +"&checknew=NO").Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        /**
    * According to the list of device serial numbers from pages,transfer data of source device in the data server to specified new device.
    * Corresponding to the "DATA UPDATE" command.
    * @return
    */
        /*
         Destination Device sn should be passed 
         it will copy all data from same branch
                */

        [HttpGet]
        public ServiceResult<string> ToNewDevice(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            //
            //api call
            string[] sns = sn.Split(',');
            var result = webrequest.Get<ServiceResult<string>>("/Api/HomeApi/ToNewDevice?sn=" + sn).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        /**
   * According to the list of device serial numbers from pages,Create clear all data of device commands "INFO" to whole device,which different serial number.
   * @return
   * Refresh Device
   */
        [HttpGet]
        public ServiceResult<string> checkDeviceInfo(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select device.",
                    Status = ResultStatus.processError,
                };
            }
            string[] sns = sn.Split(',');
            var result = webrequest.Get<ServiceResult<string>>("/Api/AdmsEx/checkDeviceInfo?sn=" + sn).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };

        }


        #region device user delete
        [HttpGet]
        public ServiceResult<string> deleteUserDev(int empId)
        {
            SEmployee sEmployee = new SEmployee();
            var emp = sEmployee.List().Data.Where(x => x.Id == empId).FirstOrDefault();
            string userPin = emp.DeviceCode.ToString();
            if (string.IsNullOrEmpty(userPin))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select user.",
                    Status = ResultStatus.processError,
                };
            }
            var result = webrequest.Get<ServiceResult<string>>("/Api/AdmsEx/deleteUserDev?userpin=" + userPin + "&branchcode=" + branchCode).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<string> deleteUserFaceDev(int empId)
        {
            SEmployee sEmployee = new SEmployee();
            var emp = sEmployee.List().Data.Where(x => x.Id == empId).FirstOrDefault();
            string userPin = emp.DeviceCode.ToString();
            if (string.IsNullOrEmpty(userPin))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select user.",
                    Status = ResultStatus.processError,
                };
            }
            var result = webrequest.Get<ServiceResult<string>>("/Api/AdmsEx/deleteUserFaceDev?userpin=" + userPin + "&branchcode=" + branchCode).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<string> deleteUserFpDev(int empId)
        {
            SEmployee sEmployee = new SEmployee();
            var emp = sEmployee.List().Data.Where(x => x.Id == empId).FirstOrDefault();
            string userPin = emp.DeviceCode.ToString();
            if (string.IsNullOrEmpty(userPin))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select user.",
                    Status = ResultStatus.processError,
                };
            }
            var result = webrequest.Get<ServiceResult<string>>("/Api/AdmsEx/deleteUserFpDev?userpin=" + userPin + "&branchcode=" + branchCode).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<string> deleteUserPicDev(int empId)
        {
            SEmployee sEmployee = new SEmployee();
            var emp = sEmployee.List().Data.Where(x => x.Id == empId).FirstOrDefault();
            string userPin = emp.DeviceCode.ToString();
            if (string.IsNullOrEmpty(userPin))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select user.",
                    Status = ResultStatus.processError,
                };
            }
            var result = webrequest.Get<ServiceResult<string>>("/Api/AdmsEx/deleteUserPicDev?userpin=" + userPin + "&branchcode=" + branchCode).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<string> toNewDevice(int empId, string deviceSN)
        {
            SEmployee sEmployee = new SEmployee();
            var emp = sEmployee.List().Data.Where(x => x.Id == empId).FirstOrDefault();
            string userPin = emp.DeviceCode.ToString();
            if (string.IsNullOrEmpty(userPin))
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Message = "please select user.",
                    Status = ResultStatus.processError,
                };
            }
            var result = webrequest.Get<ServiceResult<string>>("/Api/AdmsEx/toNewDevice?userpin=" + userPin + "&destSn=" + deviceSN).Result;
            return new ServiceResult<string>()
            {
                Data = "deviceList",
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        #endregion

        [HttpPost]
        public ServiceResult<string> PostADMS(AdmsAttendanceModel model)
        {
            SAttendanceLog attendanceLogServices = new SAttendanceLog();
            var result = attendanceLogServices.AddUsingSP(model.BranchCode,model.UserPin.ToInt(),model.VerifyType,model.VerifyTime.ToDateTime(),model.Temperature,model.DeviceSn);
            if(result.Data.hasError)
            {
                return new ServiceResult<string>()
                {
                    Status = ResultStatus.processError,
                    Message = result.Data.message
                };
            }
            return new ServiceResult<string>()
            {
                Status = ResultStatus.Ok,
                Message = result.Data.message
            };
        }
        private int getEmpId(int EnrollId, int? BranchId)
        {
            return (new SEmployee().List().Data.Where(x => x.BranchId == BranchId && x.DeviceCode == EnrollId).FirstOrDefault() ?? new Riddhasoft.Employee.Entities.EEmployee()).Id;
        }

        private int getDeviceId(string deviceSn)
        {
            Riddhasoft.Device.Entities.EDevice device = new Riddhasoft.Device.Services.SDevice().List().Data.Where(x => x.SerialNumber == deviceSn).FirstOrDefault() ?? new Riddhasoft.Device.Entities.EDevice();
            return device.Id;
        }
    }
    public class AdmsAttendanceModel
    {
        public int Id { get; set; }
        public string UserPin { get; set; }
        public int VerifyType { get; set; }
        public string VerifyTypeStr { get; set; }
        //public DateTime VerifyTime { get; set; }
        public string VerifyTime { get; set; }
        public int Status { get; set; }
        public string StatusStr { get; set; }
        public int WorkCode { get; set; }
        public int SensorNo { get; set; }
        public int AttFlag { get; set; }
        public string DeviceSn { get; set; }
        public int Reserved1 { get; set; }
        public int Reserved2 { get; set; }
        public string UserName { get; set; }
        public string BranchCode { get; set; }
        public decimal Temperature { get; set; }



    }

}
