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
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class ContractApiController : ApiController
    {
        SContract contractServices = null;
        LocalizedString loc = null;
        SEmployee employeeServices = null;
        SEmploymentStatus employmentStatusServices = null;
        SUser userServices = null;
        int branchId = (int)RiddhaSession.BranchId;
        public ContractApiController()
        {
            contractServices = new SContract();
            loc = new LocalizedString();
            employeeServices = new SEmployee();
            employmentStatusServices = new SEmploymentStatus();
            userServices = new SUser();
        }
        public ServiceResult<List<ContractGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            contractServices = new SContract();
            var contractLst = (from c in contractServices.List().Data.Where(x => x.Employee.BranchId == branchId).ToList()
                               select new ContractGridVm()
                               {
                                   Id = c.Id,
                                   BeganOn = c.BeganOn,
                                   Code = c.Code,
                                   EmployeeId = c.EmployeeId,
                                   EmployeeName = c.Employee.Name,
                                   EmploymentStatusName = c.EmploymentStatus.Name,
                                   EmploymentStatusId = c.EmploymentStatusId,
                                   EndedOn = c.EndedOn,
                                   CreatedById = c.CreatedById,
                                   CreatedOn = c.CreatedOn,
                                   ApprovedById = c.ApprovedById,
                                   ApprovedOn = c.ApprovedOn
                               }).ToList();
            return new ServiceResult<List<ContractGridVm>>()
            {
                Data = contractLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<ContractGridVm>> GetContractsByEmpId(int empId)
        {
            var contractLst = (from c in contractServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                               select new ContractGridVm()
                               {
                                   Id = c.Id,
                                   BeganOn = c.BeganOn,
                                   Code = c.Code,
                                   EmployeeId = c.EmployeeId,
                                   EmployeeName = c.Employee.Name,
                                   EmploymentStatusName = c.EmploymentStatus.Name,
                                   EmploymentStatusId = c.EmploymentStatusId,
                                   EndedOn = c.EndedOn,
                                   CreatedById = c.CreatedById,
                                   CreatedOn = c.CreatedOn,
                                   ApprovedById = c.ApprovedById,
                                   ApprovedOn = c.ApprovedOn,
                                   FileUrl = c.FileUrl
                               }).ToList();
            return new ServiceResult<List<ContractGridVm>>()
            {
                Data = contractLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<EmployeeGridVm>> GetEmpKendoGrid(HREmployeeGridModel vm)
        {
            string language = RiddhaSession.Language.ToString();
            int branchId = 0;
            if (vm.BranchId == 0)
            {
                branchId = (int)RiddhaSession.BranchId;
            }
            else
            {
                branchId = vm.BranchId;
            }

            SEmployee service = new SEmployee();
            //IQueryable<EEmployee> empQuery = service.List().Data.Where(x => x.BranchId == branchId);
            List<EEmployee> empQuery = Common.GetEmployees().Data;
            if (vm.Type == "active")
            {
                empQuery = empQuery.Where(x => x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated).ToList();
            }
            else if (vm.Type == "inactive")
            {
                empQuery = empQuery.Where(x => x.EmploymentStatus == EmploymentStatus.Resigned || x.EmploymentStatus == EmploymentStatus.Terminated).ToList();
            }
            int totalRowNum = empQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            List<EEmployee> paginatedQuery;
            switch (searchField)
            {
                case "IdCardNo":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "DepartmentName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "Section":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "Mobile":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Mobile.StartsWith(searchValue)).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Mobile == searchValue).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "DesignationName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Designation.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Designation.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                default:
                    paginatedQuery = empQuery.OrderByDescending(x => x.Id).ToList();
                    break;
            }
            var employeelist = (from c in paginatedQuery.ToList()
                                select new EmployeeGridVm()
                                {
                                    Id = c.Id,
                                    DepartmentName = getDeptName(c.Section, language),
                                    EmployeeName = !string.IsNullOrEmpty(c.NameNp) ? c.Name + "  (" + c.NameNp + ")" : c.Name,
                                    IdCardNo = c.Code,
                                    Mobile = c.Mobile,
                                    Email = c.Email,
                                    PhotoURL = c.ImageUrl,
                                    SectionId = c.SectionId,
                                    Section = c.Section == null ? "" : c.Section.Code + " - " + (!string.IsNullOrEmpty(c.Section.NameNp) && language == "ne" ? c.Section.NameNp : c.Section.Name),
                                    DepartmentId = c.Section == null ? 0 : c.Section.DepartmentId,
                                    DesignationName = c.Designation == null ? "" : c.Designation.Code + " - " + (language == "ne" ? c.Designation.NameNp : c.Designation.Name),
                                    GradeGroupName = c.GradeGroup == null ? "" : c.GradeGroup.Name,
                                    EmploymentStatus = c.EmploymentStatus,
                                    EmploymentStatusName = getEmployementStatusName(c.EmploymentStatus, language),
                                }).ToList();
            return new KendoGridResult<List<EmployeeGridVm>>()
            {
                Data = employeelist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = employeelist.Count()
            };
        }

        private string getEmployementStatusName(EmploymentStatus employmentStatus, string lang)
        {
            string result = "";
            switch (employmentStatus)
            {
                case EmploymentStatus.NormalEmployment:
                    result = lang == "ne" ? "सामान्य रोजगार" : "Normal Employment";
                    break;
                case EmploymentStatus.Deceased:
                    result = lang == "ne" ? "मृत" : "Deceased";
                    break;
                case EmploymentStatus.Defaulter:
                    result = lang == "ne" ? "पूर्वनिर्धारित" : "Defaulter";
                    break;
                case EmploymentStatus.Terminated:
                    result = lang == "ne" ? "समाप्त" : "Terminated";
                    break;
                case EmploymentStatus.Resigned:
                    result = lang == "ne" ? "त्याग गरिएको" : "Resigned";
                    break;
                case EmploymentStatus.EarlyRetirement:
                    result = lang == "ne" ? "प्रारम्भिक अवकाश" : "Early Retirement";
                    break;
                case EmploymentStatus.NormalRetirement:
                    result = lang == "ne" ? "सामान्य अवकाश" : "Normal Retirement";
                    break;
                case EmploymentStatus.ContractPeriodOver:
                    result = lang == "ne" ? "अनुबंध अवधि समाप्त" : "Contract Period Over";
                    break;
                case EmploymentStatus.OnContract:
                    result = lang == "ne" ? "अनुबंधमा" : "On Contract";
                    break;
                case EmploymentStatus.PermanentJob:
                    result = lang == "ne" ? "स्थायी रोजगार" : "Permanent Job";
                    break;
                case EmploymentStatus.Retiring:
                    result = lang == "ne" ? "अवकाश लिदै" : "Retiring";
                    break;
                default:
                    break;
            }
            return result;
        }

        private string getDeptName(Riddhasoft.OfficeSetup.Entities.ESection section, string language)
        {
            string deptName = "";
            if (section != null)
            {
                if (language == "ne")
                {
                    deptName = section.Department.Code + " - " + (string.IsNullOrEmpty(section.Department.NameNp) ? section.Department.Name : section.Department.NameNp);
                }
                else
                {
                    deptName = section.Department.Code + " - " + section.Department.Name;
                }
            }
            return deptName;
        }

        public ServiceResult<EContract> Get(int id)
        {
            EContract contract = contractServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EContract>()
            {
                Data = contract,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EContract> Post(EContract model)
        {
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = RiddhaSession.CurDate.ToDateTime();
            model.BranchId = branchId;
            var result = contractServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7001", "7101", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EContract>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EContract> Put(EContract model)
        {
            model.BranchId = branchId;
            var result = contractServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7001", "7102", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EContract>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var contract = contractServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = contractServices.Remove(contract);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7001", "7103", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetEmploymentStatusForDropdown()
        {
            string language = RiddhaSession.Language.ToString();
            int? branchId = RiddhaSession.BranchId;
            List<DropdownViewModel> resultLst = (from c in employmentStatusServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = c.Name
                                                 }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<ContractVerificationGridVm>> GetContractKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            List<EContract> contractQuery = new List<EContract>();
            IQueryable<EContract> contracts = contractServices.List().Data.Where(x => x.BranchId == branchId && x.IsApproved == false);
            try
            {
                contractQuery = (from c in contracts.ToList()
                                 join d in Common.GetEmployees().Data.ToList() on c.EmployeeId equals d.Id
                                 select c
                                             ).ToList();
            }
            catch (Exception ex)
            {


            }
            var userQuery = userServices.List().Data;
            int totalRowNum = contractQuery.Count();
            //int totalunApproveCount = contractServices.List().Data.Where(x => x.Employee.BranchId == branchId && x.ApprovedById == null && x.ApprovedOn == null).Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var contractlist = (from c in contractQuery
                                select new ContractVerificationGridVm()
                                {
                                    Id = c.Id,
                                    Code = c.Code,
                                    EmployeeName = !string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.Name + "  (" + c.Employee.NameNp + ")" : c.Employee.Name,
                                    EmployeeCode = c.Employee.Code,
                                    EmployeeId = c.EmployeeId,
                                    EmploymentStatusId = c.EmploymentStatusId,
                                    EmploymentStatusName = c.EmploymentStatus.Name,
                                    Period = c.BeganOn.ToString("yyyy/MM/dd") + " To " + c.EndedOn.ToString("yyyy/MM/dd"),
                                    TotalCount = totalRowNum,
                                    ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                    ApprovedById = c.ApprovedById,
                                    ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById),
                                    totalunApproveCount = contractQuery.Where(x => x.ApprovedById == null && x.ApprovedOn == null).Count()
                                }).OrderByDescending(x => x.Id).ToList();

            return new KendoGridResult<List<ContractVerificationGridVm>>()
            {
                Data = contractlist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = contractlist.Count()
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
        public ServiceResult<List<ContractVerificationGridVm>> GetVerificationContractsByEmpId(int empId)
        {
            var contractLst = (from c in contractServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                               select new ContractVerificationGridVm()
                               {
                                   Id = c.Id,
                                   Code = c.Code,
                                   EmployeeId = c.EmployeeId,
                                   EmployeeName = c.Employee.Name + " (" + c.Employee.Code + ")",
                                   EmploymentStatusName = c.EmploymentStatus.Name,
                                   EmploymentStatusId = c.EmploymentStatusId,
                                   Period = c.BeganOn.ToString("yyyy/MM/dd") + " To " + c.EndedOn.ToString("yyyy/MM/dd"),
                               }).ToList();
            return new ServiceResult<List<ContractVerificationGridVm>>()
            {
                Data = contractLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EContract> Approve(int id, int empId)
        {
            string msg = "";
            var contract = contractServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (contract != null)
            {
                if (contract.IsApproved == false)
                {
                    contract.ApprovedById = RiddhaSession.CurrentUser.Id;
                    contract.ApprovedOn = System.DateTime.Now;
                    contract.IsApproved = true;
                    var result = contractServices.Update(contract);
                    if (result.Status == ResultStatus.Ok)
                    {
                        var contractEmp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                        contractEmp.EmploymentStatus = EmploymentStatus.OnContract;
                        employeeServices.Update(contractEmp);
                        msg = "Approved Successfully";
                        Common.AddAuditTrail("7001", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Contract Approve");
                    }
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EContract>()
            {
                Data = contract,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EContract> Revert(int id, int empId)
        {
            string msg = "";
            var status = new ResultStatus();
            var contract = contractServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (contract != null)
            {
                if (contract.IsApproved)
                {
                    contract.ApprovedById = null;
                    contract.ApprovedOn = null;
                    contract.IsApproved = false;
                    var result = contractServices.Update(contract);
                    if (result.Status == ResultStatus.Ok)
                    {
                        var contractEmp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                        contractEmp.EmploymentStatus = EmploymentStatus.NormalEmployment;
                        employeeServices.Update(contractEmp);
                        msg = "Reverted Successfully";
                        Common.AddAuditTrail("7001", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Contract Revert");
                        status = ResultStatus.Ok;
                    }
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<EContract>()
            {
                Data = contract,
                Message = msg,
                Status = status
            };
        }

        [HttpGet]
        public ServiceResult<UnapprovedCountModel> GetUnapprovedCount()
        {
            return contractServices.GetUnapprovedCount((int)RiddhaSession.BranchId);
        }
    }

    public class ContractGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime BeganOn { get; set; }
        public DateTime EndedOn { get; set; }
        public string EmploymentStatusName { get; set; }
        public int EmploymentStatusId { get; set; }
        public string Period { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }

        public int? ApprovedById { get; set; }

        public DateTime? ApprovedOn { get; set; }
        public string FileUrl { get; set; }
    }
    public class ContractEmployeeGridVm
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string IdCardNo { get; set; }
        public int TotalCount { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public string SectionName { get; set; }
        public int? SectionId { get; set; }
        public string DesignationName { get; set; }
        public int DesignationId { get; set; }
        public string PhotoURL { get; set; }

    }
    public class ContractVerificationGridVm
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int EmployeeId { get; set; }
        public string EmploymentStatusName { get; set; }
        public int EmploymentStatusId { get; set; }
        public string Period { get; set; }
        public int TotalCount { get; set; }
        public string Code { get; set; }
        public string ApprovedOn { get; set; }
        public int? ApprovedById { get; set; }
        public string ApprovedBy { get; set; }
        public int totalunApproveCount { get; set; }
    }
    public class HREmployeeGridModel : KendoPageListArguments
    {
        public string Type { get; set; }
    }
}
