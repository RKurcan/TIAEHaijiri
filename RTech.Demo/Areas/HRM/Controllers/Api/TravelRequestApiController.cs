using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.HRM.Entities.Travel;
using Riddhasoft.HRM.Services.Travel;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class TravelRequestApiController : ApiController
    {
        STravelRequest _travelRequestServices = null;
        SEmployee _employeeServices = null;
        LocalizedString _loc = null;
        public TravelRequestApiController()
        {
            _travelRequestServices = new STravelRequest();
            _loc = new LocalizedString();
            _employeeServices = new SEmployee();
        }
        public ServiceResult<List<TravelRequestGridVm>> Get()
        {
            int branchId = (int)RiddhaSession.BranchId;
            var travelRequestLst = (from c in _travelRequestServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                    select new TravelRequestGridVm()
                                    {
                                        Id = c.Id,
                                        Currency = Enum.GetName(typeof(Currency), c.Currency),
                                        DepartmentName = c.Employee.Section.Department.Name,
                                        DesignationName = c.Employee.Designation.Name,
                                        EmployeeCode = c.Employee.Code,
                                        EmployeeId = c.EmployeeId,
                                        EmployeeName = c.Employee.Name,
                                        Section=c.Employee.Section.Name,
                                        Photo=c.Employee.ImageUrl,
                                        Purpose = c.Purpose,
                                        ApplyForCashAdvance = c.ApplyForCashAdvance,
                                        AdvanceAmount = c.AdvanceAmount,
                                        BranchId = c.BranchId
                                    }).ToList();
            return new ServiceResult<List<TravelRequestGridVm>>()
            {
                Data = travelRequestLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ETravelRequest> Get(int id)
        {
            ETravelRequest travelRequest = _travelRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ETravelRequest>()
            {
                Data = travelRequest,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ETravelRequest> Post(ETravelRequest model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            var result = _travelRequestServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8011", "7160", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ETravelRequest>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ETravelRequest> Put(ETravelRequest model)
        {
            var result = _travelRequestServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8011", "7161", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ETravelRequest>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var travelRequest = _travelRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _travelRequestServices.Remove(travelRequest);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8011", "7162", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPost]
        public ServiceResult<List<TravelRequestEmployeeAutoCompleteVm>> GetEmployeeLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<TravelRequestEmployeeAutoCompleteVm> resultLst = new List<TravelRequestEmployeeAutoCompleteVm>();
            if (model != null)
            {
                var employeeLst = _employeeServices.List().Data.Where(x => x.BranchId == branchId);
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                if (searchText == "___")
                {
                    employeeLst = employeeLst.OrderBy(x => x.Name).Take(20);
                }
                else
                {
                    employeeLst = employeeLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower()));
                }
                if (employeeLst != null)
                {
                    resultLst = (from c in employeeLst
                                 select new TravelRequestEmployeeAutoCompleteVm()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     Name = c.Code + " - " + c.Name + " - " + (c.Mobile ?? ""),
                                     Designation = c.Designation == null ? "" : c.Designation.Name,
                                     Department = c.Section.Department == null ? "" : c.Section.Department.Name,
                                     Section = c.Section == null ? "" : c.Section.Name,
                                     Photo = c.ImageUrl

                                 }).OrderBy(x => x.Name).ToList();
                }
            }
            return new ServiceResult<List<TravelRequestEmployeeAutoCompleteVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
    }
    public class TravelRequestGridVm
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Currency { get; set; }
        public string Section { get; set; }
        public string Photo { get; set; }
        public string Purpose { get; set; }
        public bool ApplyForCashAdvance { get; set; }
        public decimal AdvanceAmount { get; set; }
    }
    public class TravelRequestEmployeeAutoCompleteVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Photo { get; set; }
    }
}

