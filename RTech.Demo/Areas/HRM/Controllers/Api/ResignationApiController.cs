using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class ResignationApiController : ApiController
    {
        SResignation resignationServices = null;
        LocalizedString loc = null;
        SEmployee employeeServices = null;
        SUser userServices = null;
        int branchId = (int)RiddhaSession.BranchId;
        public ResignationApiController()
        {
            resignationServices = new SResignation();
            loc = new LocalizedString();
            employeeServices = new SEmployee();
            userServices = new SUser();
        }
        public ServiceResult<List<ResignationGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            resignationServices = new SResignation();
            var resignationLst = (from c in resignationServices.List().Data.Where(x => x.Employee.BranchId == branchId).ToList()
                                  select new ResignationGridVm()
                                  {
                                      Id = c.Id,
                                      Code = c.Code,
                                      EmployeeId = c.EmployeeId,
                                      DesiredResignDate = c.DesiredResignDate,
                                      Details = c.Details,
                                      EmployeeName = c.Employee.Name,
                                      NoticeDate = c.NoticeDate,
                                      Reason = c.Reason,
                                      ApprovedById = c.ApprovedById,
                                      ApprovedOn = c.ApprovedOn
                                  }).ToList();
            return new ServiceResult<List<ResignationGridVm>>()
            {
                Data = resignationLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<ResignationGridVm>> GetResignationByEmpId(int empId)
        {
            var resignationLst = (from c in resignationServices.List().Data.Where(x => x.EmployeeId == empId)
                                  select new ResignationGridVm()
                                  {
                                      Id = c.Id,
                                      Code = c.Code,
                                      EmployeeId = c.EmployeeId,
                                      EmployeeName = c.Employee.Name,
                                      DesiredResignDate = c.DesiredResignDate,
                                      Details = c.Details,
                                      NoticeDate = c.NoticeDate,
                                      Reason = c.Reason,
                                      FordwardToName = c.ForwardTo.Name,
                                      ForwardToId = c.ForwardToId,
                                      CreatedById = c.CreatedById,
                                      CreatedOn = c.CreatedOn,
                                      ApprovedById = c.ApprovedById,
                                      ApprovedOn = c.ApprovedOn,
                                      FileUrl = c.FileUrl
                                  }).ToList();
            return new ServiceResult<List<ResignationGridVm>>()
            {
                Data = resignationLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EResignation> Get(int id)
        {
            EResignation resignation = resignationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EResignation>()
            {
                Data = resignation,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EResignation> Post(EResignation model)
        {
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.BranchId = branchId;
            var result = resignationServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7001", "7105", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EResignation>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EResignation> Put(EResignation model)
        {
            model.BranchId = branchId;
            var result = resignationServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7001", "7105", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EResignation>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var resignation = resignationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = resignationServices.Remove(resignation);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetEmployeesForDropdown()
        {
            string language = RiddhaSession.Language.ToString();
            int? branchId = RiddhaSession.BranchId;
            employeeServices = new SEmployee();
            List<DropdownViewModel> resultLst = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager == true).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = language == "ne" &&
                                                     c.NameNp != null ? c.NameNp : c.Name
                                                 }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<ResignationVerificationGridVm>> GetResignationKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            List<EResignation> resignationQuery = new List<EResignation>();
            IQueryable<EResignation> resignations = resignationServices.List().Data.Where(x => x.Employee.BranchId == branchId && x.IsApproved == false);
            try
            {
                resignationQuery = (from c in resignations.ToList()
                                    join d in Common.GetEmployees().Data.ToList() on c.EmployeeId equals d.Id
                                    select c
                                   ).ToList();
            }
            catch (Exception ex)
            {


            }

            var userQuery = userServices.List().Data;
            int totalRowNum = resignationQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var resignationlist = (from c in resignationQuery
                                   select new ResignationVerificationGridVm()
                                   {
                                       Id = c.Id,
                                       Code = c.Code,
                                       EmployeeName = !string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.Name + "  (" + c.Employee.NameNp + ")" : c.Employee.Name,
                                       EmployeeId = c.EmployeeId,
                                       DesiredResignDate = c.DesiredResignDate.ToString("yyyy/MM/dd"),
                                       Details = c.Details,
                                       ForwardToName = c.ForwardTo.Name,
                                       ForwardToId = c.ForwardToId,
                                       NoticeDate = c.NoticeDate.ToString("yyyy/MM/dd"),
                                       Reason = c.Reason,
                                       TotalCount = totalRowNum,
                                       ApprovedById = c.ApprovedById,
                                       ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                       ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById)
                                   }).OrderByDescending(x => x.Id).ToList();
            return new KendoGridResult<List<ResignationVerificationGridVm>>()
            {
                Data = resignationlist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = resignationlist.Count()
            };
        }
        private string getapprovedByUser(IQueryable<Riddhasoft.User.Entity.EUser> userQuery, int? approvedById)
        {
            if (approvedById == null)
            {
                return "";
            }
            else
            {
                return userQuery.Where(x => x.Id == (int)approvedById).FirstOrDefault().Name;
            }
        }
        [HttpGet]
        public ServiceResult<List<ResignationVerificationGridVm>> GetVerificationResignationsByEmpId(int empId)
        {

            var resignationlist = (from c in resignationServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                                   select new ResignationVerificationGridVm()
                                   {
                                       Id = c.Id,
                                       Code = c.Code,
                                       EmployeeName = !string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.Name + "  (" + c.Employee.NameNp + ")" : c.Employee.Name,
                                       EmployeeId = c.EmployeeId,
                                       DesiredResignDate = c.DesiredResignDate.ToString("yyyy/MM/dd"),
                                       Details = c.Details,
                                       ForwardToName = c.ForwardTo.Name,
                                       ForwardToId = c.ForwardToId,
                                       NoticeDate = c.NoticeDate.ToString("yyyy/MM/dd"),
                                       Reason = c.Reason,
                                   }).ToList();
            return new ServiceResult<List<ResignationVerificationGridVm>>()
            {
                Data = resignationlist,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EResignation> Approve(int id, int empId)
        {
            string msg = "";
            var resignation = resignationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (resignation != null)
            {
                if (resignation.IsApproved == false)
                {
                    resignation.ApprovedById = RiddhaSession.CurrentUser.Id;
                    resignation.ApprovedOn = System.DateTime.Now;
                    resignation.IsApproved = true;
                    var result = resignationServices.Update(resignation);
                    if (result.Status == ResultStatus.Ok)
                    {
                        var resignedEmp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                        resignedEmp.EmploymentStatus = EmploymentStatus.Resigned;
                        employeeServices.Update(resignedEmp);
                        msg = "Approved Successfully";
                        Common.AddAuditTrail("7001", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee resignation approve");
                    }
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EResignation>()
            {
                Data = resignation,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EResignation> Revert(int id, int empId)
        {
            string msg = "";
            var status = new ResultStatus();
            var resignation = resignationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (resignation != null)
            {
                if (resignation.IsApproved)
                {
                    resignation.ApprovedById = null;
                    resignation.ApprovedOn = null;
                    resignation.IsApproved = false;
                    var result = resignationServices.Update(resignation);
                    if (result.Status == ResultStatus.Ok)
                    {
                        var resignedEmp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                        resignedEmp.EmploymentStatus = EmploymentStatus.NormalEmployment;
                        employeeServices.Update(resignedEmp);
                        msg = "Reverted Successfully";
                        Common.AddAuditTrail("7001", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee resignation revert");
                        status = ResultStatus.Ok;
                    }
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<EResignation>()
            {
                Data = resignation,
                Message = msg,
                Status = status
            };
        }
    }
    public class ResignationGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime NoticeDate { get; set; }
        public DateTime DesiredResignDate { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FordwardToName { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int ForwardToId { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }
        public string FileUrl { get; set; }
    }
    public class ResignationVerificationGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public string NoticeDate { get; set; }
        public string DesiredResignDate { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
        public string ForwardToName { get; set; }
        public int ForwardToId { get; set; }
        public int TotalCount { get; set; }
        public string ApprovedBy { get; set; }
        public int? ApprovedById { get; set; }

        public string ApprovedOn { get; set; }
    }
}
