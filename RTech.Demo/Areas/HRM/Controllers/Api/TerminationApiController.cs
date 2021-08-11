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
    public class TerminationApiController : ApiController
    {
        STermination terminationServices = null;
        LocalizedString loc = null;
        SEmployee employeeServices = null;
        SUser userServices = null;
        int branchId = (int)RiddhaSession.BranchId;
        public TerminationApiController()
        {
            terminationServices = new STermination();
            loc = new LocalizedString();
            employeeServices = new SEmployee();
            userServices = new SUser();
        }
        public ServiceResult<List<TerminationgridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var terminationLst = (from c in terminationServices.List().Data.Where(x => x.Employee.BranchId == branchId).ToList()
                                  select new TerminationgridVm()
                                  {
                                      Id = c.Id,
                                      ApprovedById = c.ApprovedById,
                                      ChangeStatus = c.ChangeStatus,
                                      ApprovedOn = c.ApprovedOn,
                                      Code = c.Code,
                                      Details = c.Details,
                                      EmployeeId = c.EmployeeId,
                                      FordwardToName = c.Employee.Name,
                                      ForwardToId = c.ForwardToId,
                                      NoticeDate = c.NoticeDate,
                                      Reason = c.Reason,
                                      ServiceEndDate = c.ServiceEndDate,
                                  }).ToList();
            return new ServiceResult<List<TerminationgridVm>>()
            {
                Data = terminationLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<TerminationgridVm>> GetTerminationByEmpId(int empId)
        {
            var terminationLst = (from c in terminationServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                                  select new TerminationgridVm()
                                  {
                                      Id = c.Id,
                                      ChangeStatus = c.ChangeStatus,
                                      ChangeStatusName = Enum.GetName(typeof(ChangeStatus), c.ChangeStatus),
                                      Code = c.Code,
                                      Details = c.Details,
                                      EmployeeId = c.EmployeeId,
                                      FordwardToName = c.ForwardTo.Name,
                                      ForwardToId = c.ForwardToId,
                                      NoticeDate = c.NoticeDate,
                                      Reason = c.Reason,
                                      ServiceEndDate = c.ServiceEndDate,
                                      CreatedById = c.CreatedById,
                                      CreatedOn = c.CreatedOn,
                                      ApprovedById = c.ApprovedById,
                                      ApprovedOn = c.ApprovedOn,
                                      FileUrl = c.FileUrl,
                                  }).ToList();
            return new ServiceResult<List<TerminationgridVm>>()
            {
                Data = terminationLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ETermination> Get(int id)
        {
            ETermination termination = terminationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ETermination>()
            {
                Data = termination,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ETermination> Post(ETermination model)
        {
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.BranchId = branchId;
            var result = terminationServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7001", "7107", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ETermination>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ETermination> Put(ETermination model)
        {
            model.BranchId = branchId;
            var result = terminationServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7001", "7107", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ETermination>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var termination = terminationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = terminationServices.Remove(termination);
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
        public KendoGridResult<List<TerminationVerificationGridVm>> GetTerminationKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            List<ETermination> terminationQuery = new List<ETermination>();
            IQueryable<ETermination> terminations = terminationServices.List().Data.Where(x => x.Employee.BranchId == branchId && x.IsApproved == false);
            try
            {
                terminationQuery = (from c in terminations.ToList()
                                    join d in Common.GetEmployees().Data.ToList() on c.Id equals d.Id
                                    select c
                                    ).ToList();
            }
            catch (Exception ex)
            {


            }
            var userQuery = userServices.List().Data;
            int totalRowNum = terminationQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var terminationlist = (from c in terminationQuery
                                   select new TerminationVerificationGridVm()
                                   {
                                       Id = c.Id,
                                       Code = c.Code,
                                       EmployeeName = !string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.Name + "  (" + c.Employee.NameNp + ")" : c.Employee.Name,
                                       EmployeeId = c.EmployeeId,
                                       ServiceEndDate = c.ServiceEndDate.ToString("yyyy/MM/dd"),
                                       Details = c.Details,
                                       ForwardToName = c.ForwardTo.Name,
                                       ForwardToId = c.ForwardToId,
                                       NoticeDate = c.NoticeDate.ToString("yyyy/MM/dd"),
                                       Reason = c.Reason,
                                       TotalCount = totalRowNum,
                                       ChangeStatus = c.ChangeStatus,
                                       ChangeStatusName = Enum.GetName(typeof(ChangeStatus), c.ChangeStatus),
                                       ApprovedById = c.ApprovedById,
                                       ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                       ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById)
                                   }).OrderByDescending(x => x.Id).ToList();
            return new KendoGridResult<List<TerminationVerificationGridVm>>()
            {
                Data = terminationlist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = terminationlist.Count(),
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
        public ServiceResult<List<TerminationVerificationGridVm>> GetVerificationTerminationsByEmpId(int empId)
        {
            var termination = (from c in terminationServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                               select new TerminationVerificationGridVm()
                               {
                                   Id = c.Id,
                                   Code = c.Code,
                                   EmployeeName = !string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.Name + "  (" + c.Employee.NameNp + ")" : c.Employee.Name,
                                   EmployeeId = c.EmployeeId,
                                   ServiceEndDate = c.ServiceEndDate.ToString("yyyy/MM/dd"),
                                   Details = c.Details,
                                   ForwardToName = c.ForwardTo.Name,
                                   ForwardToId = c.ForwardToId,
                                   NoticeDate = c.NoticeDate.ToString("yyyy/MM/dd"),
                                   Reason = c.Reason,
                                   ChangeStatus = c.ChangeStatus,
                                   ChangeStatusName = Enum.GetName(typeof(ChangeStatus), c.ChangeStatus),
                                   CreatedById = c.CreatedById,
                                   CreatedOn = c.CreatedOn
                               }).ToList();
            return new ServiceResult<List<TerminationVerificationGridVm>>()
            {
                Data = termination,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<ETermination> Approve(int id, int empId)
        {
            string msg = "";
            var termination = terminationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (termination != null)
            {
                if (termination.IsApproved == false)
                {
                    termination.ApprovedById = RiddhaSession.CurrentUser.Id;
                    termination.ApprovedOn = System.DateTime.Now;
                    termination.IsApproved = true;
                    var result = terminationServices.Update(termination);
                    if (result.Status == ResultStatus.Ok)
                    {
                        var terminationEmp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                        terminationEmp.EmploymentStatus = EmploymentStatus.Terminated;
                        employeeServices.Update(terminationEmp);
                        Common.AddAuditTrail("7001", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Termination Approve");
                        msg = "Approved Successfully";
                    }
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<ETermination>()
            {
                Data = termination,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<ETermination> Revert(int id, int empId)
        {
            string msg = "";
            ResultStatus status = new ResultStatus();
            var termination = terminationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (termination != null)
            {
                if (termination.IsApproved)
                {
                    termination.ApprovedById = null;
                    termination.ApprovedOn = null;
                    termination.IsApproved = false;
                    var result = terminationServices.Update(termination);
                    if (result.Status == ResultStatus.Ok)
                    {
                        var terminationEmp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                        terminationEmp.EmploymentStatus = EmploymentStatus.NormalEmployment;
                        employeeServices.Update(terminationEmp);
                        Common.AddAuditTrail("7001", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Termination Revert");
                        msg = "Reverted Successfully";
                        status = ResultStatus.Ok;
                    }
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<ETermination>()
            {
                Data = termination,
                Message = msg,
                Status = status
            };
        }
    }
    public class TerminationgridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime NoticeDate { get; set; }
        public DateTime ServiceEndDate { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
        public int ForwardToId { get; set; }
        public string FordwardToName { get; set; }
        public int EmployeeId { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public ChangeStatus ChangeStatus { get; set; }
        public string ChangeStatusName { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FileUrl { get; set; }
    }

    public class TerminationVerificationGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NoticeDate { get; set; }
        public string ServiceEndDate { get; set; }
        public string Reason { get; set; }
        public string Details { get; set; }
        public ChangeStatus ChangeStatus { get; set; }
        public string ChangeStatusName { get; set; }
        public string ForwardToName { get; set; }
        public int ForwardToId { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public int TotalCount { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public int? ApprovedById { get; set; }
        public string ApprovedOn { get; set; }
    }
}
