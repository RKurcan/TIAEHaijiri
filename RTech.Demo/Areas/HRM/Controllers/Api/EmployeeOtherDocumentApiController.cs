using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class EmployeeOtherDocumentApiController : ApiController
    {
        private SEmployeeOtherDocument _empOtherDocServices = null;
        private LocalizedString _loc = null;
        public EmployeeOtherDocumentApiController()
        {
            _empOtherDocServices = new SEmployeeOtherDocument();
            _loc = new LocalizedString();
        }
        [HttpGet]
        public ServiceResult<List<EEmployeeOtherDocument>> GetEmployeeOtherDocument(int empId)
        {

            var result = _empOtherDocServices.List().Data.Where(x => x.EmployeeId == empId).ToList();
            return new ServiceResult<List<EEmployeeOtherDocument>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpPost]
        public ServiceResult<EEmployeeOtherDocument> Post(EEmployeeOtherDocument model)
        {
            var result = _empOtherDocServices.Add(model);
            return new ServiceResult<EEmployeeOtherDocument>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = ResultStatus.Ok
            };
        }
        [HttpPut]
        public ServiceResult<EEmployeeOtherDocument> Put(EEmployeeOtherDocument model)
        {
            EEmployeeOtherDocument doc = _empOtherDocServices.List().Data.Where(x => x.Id == model.Id).FirstOrDefault();
            if (model.FileUrl != doc.FileUrl)
            {
                deleteFile(doc.FileUrl);
            }
            doc.FileName = model.FileName;
            doc.FileUrl = model.FileUrl;

            var result = _empOtherDocServices.Update(doc);
            return new ServiceResult<EEmployeeOtherDocument>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = _loc.Localize(result.Message)
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var data = _empOtherDocServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (!string.IsNullOrEmpty(data.FileUrl))
            {
                deleteFile(data.FileUrl);
            }
            var result = _empOtherDocServices.Remove(data);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        private void deleteFile(string fileUrl)
        {
            if (!string.IsNullOrEmpty(fileUrl))
            {
                try
                {
                    var rootPath = System.Web.Hosting.HostingEnvironment.MapPath(fileUrl);
                    File.Delete(rootPath);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
