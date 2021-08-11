using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
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
using System.Web;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class EmployeeDocumentApiController : ApiController
    {
        private SEmployeeDocument _empDocServices = null;
        private LocalizedString _loc = null;
        public EmployeeDocumentApiController()
        {
            _empDocServices = new SEmployeeDocument();
            _loc = new LocalizedString();
        }
        [HttpGet]
        public ServiceResult<IQueryable<EEmployeeDocument>> Get()
        {
            var result = _empDocServices.List();
            return new ServiceResult<IQueryable<EEmployeeDocument>>()
            {
                Data = result.Data.Where(x => x.Employee.BranchId == RiddhaSession.BranchId),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<EEmployeeDocument> Get(int empId)
        {
            var result = _empDocServices.List().Data.Where(x => x.EmployeeId == empId).FirstOrDefault() ?? new EEmployeeDocument();
            return new ServiceResult<EEmployeeDocument>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<EEmployeeDocument> Post(EEmployeeDocument model)
        {
            var result = _empDocServices.Add(model);
            return new ServiceResult<EEmployeeDocument>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = _loc.Localize(result.Message)
            };
        }

     
        [HttpPut]
        public ServiceResult<EEmployeeDocument> Put(EEmployeeDocument model)
        {
            EEmployeeDocument doc = _empDocServices.Find(model.Id).Data;
            if (model.AppointmentFileUrl != doc.AppointmentFileUrl)
            {
                deleteFile(doc.AppointmentFileUrl);
            }
            if (model.CITFileUrl != doc.CITFileUrl)
            {
                deleteFile(doc.CITFileUrl);
            }
            if (model.ContractFileUrl != doc.ContractFileUrl)
            {
                deleteFile(doc.ContractFileUrl);
            }
            if (model.PFFileUrl != doc.PFFileUrl)
            {
                deleteFile(doc.PFFileUrl);
            }
            doc.AppointmentFileUrl = model.AppointmentFileUrl;
            doc.BankACNo = model.BankACNo;
            doc.CITFileUrl = model.CITFileUrl;
            doc.CITNo = model.CITNo;
            doc.ContractFileUrl = model.ContractFileUrl;
            doc.PanNo = model.PanNo;
            doc.PFFileUrl = model.PFFileUrl;
            doc.PFNo = model.PFNo;
            var result = _empDocServices.Update(doc);
            return new ServiceResult<EEmployeeDocument>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = _loc.Localize(result.Message)
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

      

        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var result = _empDocServices.Remove(id);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPost]
        public ServiceResult<object> Upload()
        {
            var branchId = RiddhaSession.BranchId ?? 0;
            var request = HttpContext.Current.Request;
            var empQuery = new SEmployee().List().Data.Where(x => x.BranchId == branchId);
            SEmployeeDocument documentService = new SEmployeeDocument();

            using (var package = new OfficeOpenXml.ExcelPackage(request.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                int empId = 0;
                List<EEmployeeDocument> docLst = new List<EEmployeeDocument>();
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    empId = getEmpIdFromCodeAndName(empQuery, (workSheet.Cells[rowIterator, 1].Value ?? string.Empty).ToString(), (workSheet.Cells[rowIterator, 2].Value ?? string.Empty).ToString());
                    if (empId == 0)
                    {
                        continue;
                    }

                    string pfNo = (workSheet.Cells[rowIterator, 3].Value ?? String.Empty).ToString();
                    string citNo = (workSheet.Cells[rowIterator, 4].Value ?? String.Empty).ToString();
                    string bankAcNo = (workSheet.Cells[rowIterator, 5].Value ?? String.Empty).ToString();
                    string panNo = (workSheet.Cells[rowIterator, 6].Value ?? String.Empty).ToString();

                    //if (pfNo != "" || citNo != "" || bankAcNo != "" || panNo == "")
                    //{
                    var employee = documentService.List().Data.Where(x => x.EmployeeId == empId).FirstOrDefault();
                    if (employee == null)
                    {
                        documentService.Add(new EEmployeeDocument()
                        {
                            EmployeeId = empId,
                            PFNo = pfNo,
                            CITNo = citNo,
                            BankACNo = bankAcNo,
                            PanNo = panNo,
                        });
                    }
                    else
                    {
                        employee.PFNo = pfNo;
                        employee.CITNo = citNo;
                        employee.BankACNo = bankAcNo;
                        employee.PanNo = panNo;
                        documentService.Update(employee);
                    }
                    //}
                }

            }
            return new ServiceResult<object>()
            {
                Status = ResultStatus.Ok,
                Message = "Uploaded successfully"
            };
        }

        private int getEmpIdFromCodeAndName(IQueryable<EEmployee> empQuery, string code, string name)
        {
            var emp = empQuery.Where(x => x.Code.ToLower() == code.ToLower() && x.Name.ToLower() == name.Trim().ToLower()).FirstOrDefault() ?? new EEmployee();
            return emp.Id;
        }
    }
}
