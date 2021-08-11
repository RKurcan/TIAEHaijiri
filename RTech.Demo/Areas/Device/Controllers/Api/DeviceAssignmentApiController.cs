using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.Services.Common;
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
    public class DeviceAssignmentApiController : ApiController
    {
        SDeviceAssignment deviceAssignmentServices = null;
        SDevice deviceServices = null;
        public DeviceAssignmentApiController()
        {
            deviceAssignmentServices = new SDeviceAssignment();
            deviceServices = new SDevice();
        }
        public ServiceResult<List<DeviceAssignmentViewModel>> Get(int pageSize = 10, int page = 1, string searchText = "")
        {
            int totalRowNum = deviceAssignmentServices.List().Data.Count();
            var devicesAssignment = deviceAssignmentServices.List().Data.Where(x => x.Device.SerialNumber.Contains(searchText)).OrderBy(x => x.Device.SerialNumber).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            List<DeviceAssignmentViewModel> resultData = (from c in devicesAssignment
                                                          select new DeviceAssignmentViewModel
                                                          {
                                                              AssignOn = c.AssignedOn,
                                                              DeviceSerialNo = c.Device.SerialNumber,
                                                              Id = c.Id,
                                                              DeviceId = c.DeviceId,
                                                              Model = c.Device.Model.Name,
                                                              Reseller = c.Reseller.Name,
                                                              Status = Enum.GetName(typeof(Status), c.Device.Status),
                                                              IsPrivate = c.IsPrivate
                                                          }).ToList();
            resultData.ForEach(x => x.TotalCount = totalRowNum);
            return new ServiceResult<List<DeviceAssignmentViewModel>>()
            {
                Data = resultData,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EDeviceAssignment> Get(int id)
        {
            EDeviceAssignment deviceAssignment = deviceAssignmentServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EDeviceAssignment>()
            {
                Data = deviceAssignment,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<EDeviceAssignment>> Post(DeviceAssignmentVm vm)
            {
            List<EDeviceAssignment> list = new List<EDeviceAssignment>();
            var currentUser = RiddhaSession.CurrentUser;
            foreach (var item in vm.DeviceIds)
            {
                list.Add(new EDeviceAssignment()
                {
                    ResellerId = vm.ResellerId,
                    DeviceId = item,
                    //AssignedBy=currentUser.Id,
                    AssignedById = currentUser.Id,
                    IsPrivate = vm.IsPrivate,
                    AssignedOn = System.DateTime.Now
                });
            }
            return deviceAssignmentServices.Add(list);
        }
        public ServiceResult<EDeviceAssignment> Put(EDeviceAssignment model)
        {
            return deviceAssignmentServices.Update(model);
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var deviceAssignment = deviceAssignmentServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return deviceAssignmentServices.Remove(deviceAssignment);
            
        }

        [HttpGet]
        public ServiceResult<int> Return(int id)
        {
            EDevice device = deviceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (device != null)
            {
                device.Status = Status.Damage;
                deviceServices.Update(device);
            }


            return new ServiceResult<int>
            {
                Data = 1,
                Status = ResultStatus.Ok,
                Message = "Device Returned Successfully"
            };
        }
    }
    public class DeviceAssignmentVm
    {
        public int ResellerId { get; set; }
        public bool IsPrivate { get; set; }
        public int[] DeviceIds { get; set; }
    }
}
