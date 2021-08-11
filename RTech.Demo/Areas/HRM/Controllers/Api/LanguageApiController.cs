using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Entities.Qualification;
using Riddhasoft.HRM.Services.Qualification;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class LanguageApiController : ApiController
    {
        SLanguage languageServices = null;
        SEmployeeLanguage employeeLanguageServices = null;
        LocalizedString loc = null;
        SUser userServices = null;
        SEmployee employeeServices = null;
        public LanguageApiController()
        {
            languageServices = new SLanguage();
            employeeLanguageServices = new SEmployeeLanguage();
            loc = new LocalizedString();
            userServices = new SUser();
            employeeServices = new SEmployee();
        }
        public ServiceResult<List<LanguageGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var languageLst = (from c in languageServices.List().Data.Where(x => x.BranchId == branchId)
                               select new LanguageGridVm()
                               {
                                   Id = c.Id,
                                   Code = c.Code,
                                   Name = c.Name,
                                   Description = c.Description,
                                   BranchId = c.BranchId,
                               }).ToList();
            return new ServiceResult<List<LanguageGridVm>>()
            {
                Data = languageLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ELanguage> Get(int id)
        {
            ELanguage language = languageServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ELanguage>()
            {
                Data = language,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ELanguage> Post(ELanguage model)
        {
            model.BranchId = RiddhaSession.BranchId;
            var result = languageServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7006", "7142", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ELanguage>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ELanguage> Put(ELanguage model)
        {
            var result = languageServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7006", "7143", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ELanguage>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        public ServiceResult<int> Delete(int id)
        {
            var language = languageServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = languageServices.Remove(language);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7006", "7144", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #region Employee Language
        [HttpGet]
        public ServiceResult<LanguageSearchVm> SearchLanguage(string languageCode = "", int languageId = 0)
        {
            LanguageSearchVm vm = new LanguageSearchVm();
            var languageList = languageServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            ELanguage language = new ELanguage();
            if (!string.IsNullOrEmpty(languageCode))
            {
                language = languageList.Where(x => x.Code.ToLower() == languageCode.ToLower()).FirstOrDefault();
            }
            else if (languageId != 0)
            {
                language = languageList.Where(x => x.Id == languageId).FirstOrDefault();
            }
            if (language != null)
            {

                vm.Id = language.Id;
                vm.Code = language.Code;
                vm.Name = language.Name;
                vm.Description = language.Description;
            }
            return new ServiceResult<LanguageSearchVm>()
            {
                Data = vm,
                Message = "Return Successfully",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<List<LanguageSearchVm>> GetLanguageLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<LanguageSearchVm> resultLst = new List<LanguageSearchVm>();
            string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
            if (string.IsNullOrEmpty(searchText))
            {
                return new ServiceResult<List<LanguageSearchVm>>()
                {
                    Data = resultLst,
                    Status = ResultStatus.Ok
                };
            }
            if (model != null)
            {

                var languageList = languageServices.List().Data.Where(x => x.BranchId == branchId).ToList();
                if (searchText == "___")
                {
                    languageList = languageList.OrderBy(x => x.Name).Take(20).ToList();
                }
                else
                {
                    languageList = languageList.Where(x => x.Name.ToLower().Contains(searchText.Trim().ToLower())).ToList();
                }
                resultLst = (from c in languageList
                             select new LanguageSearchVm()
                             {
                                 Id = c.Id,
                                 Code = c.Code,
                                 Name = c.Code + " - " + c.Name,
                                 Description = c.Description
                             }).OrderBy(x => x.Name).ToList();
            }
            return new ServiceResult<List<LanguageSearchVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<EmployeeLanguageGridVm>> GetLanguageByEmpId(int empId)
        {
            var employeeLanguageLst = (from c in employeeLanguageServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                                       select new EmployeeLanguageGridVm()
                                       {
                                           Id = c.Id,
                                           BranchId = c.BranchId,
                                           LanguageCode = c.Language.Code,
                                           LanguageDescription = c.Language.Description,
                                           LanguageId = c.LanguageId,
                                           LanguageName = c.Language.Name,
                                           EmployeeId = c.EmployeeId,
                                           CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd")
                                       }).ToList();
            return new ServiceResult<List<EmployeeLanguageGridVm>>()
            {
                Data = employeeLanguageLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<EEmployeeLanguage> CreateEmpLanguage(EEmployeeLanguage model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = System.DateTime.Now;
            var result = employeeLanguageServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7006", "7142", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, "Create employee language");
            }
            return new ServiceResult<EEmployeeLanguage>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPut]
        public ServiceResult<EEmployeeLanguage> UpdateEmpLanguage(EEmployeeLanguage model)
        {
            var result = employeeLanguageServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7006", "7143", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, "Update employee language");
            }
            return new ServiceResult<EEmployeeLanguage>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<int> DeleteEmpLanguage(int id)
        {
            var empLanguage = employeeLanguageServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = employeeLanguageServices.Remove(empLanguage);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7006", "7144", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Delete employee language");
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        #endregion

        #region Employeee Language Verification
        [HttpPost]
        public KendoGridResult<List<LanguageVerificationGridVm>> GetEmpLanguageKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;

            IQueryable<EEmployeeLanguage> employeeLanguageQuery = employeeLanguageServices.List().Data.Where(x => x.Language.BranchId == branchId && x.IsApproved == false);
            var userQuery = userServices.List().Data;
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var languageist = (from c in employeeLanguageQuery.ToList()
                               join d in Common.GetEmployees().Data.ToList()
                               on c.Id equals d.Id
                               select new LanguageVerificationGridVm()
                               {
                                   Id = c.Id,
                                   EmployeeName = d.Name,
                                   EmployeeCode = d.Code,
                                   EmployeeId = c.EmployeeId,
                                   Code = c.Language.Code,
                                   Name = c.Language.Name,
                                   Description = c.Language.Description,
                                   ApprovedById = c.ApprovedById,
                                   ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                   ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById),
                               }).OrderByDescending(x => x.Id).ToList();
            return new KendoGridResult<List<LanguageVerificationGridVm>>()
            {
                Data = languageist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = languageist.Count()
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
        public ServiceResult<List<LanguageVerificationGridVm>> GetVerificationLanguageByEmpId(int empId)
        {
            var language = (from c in employeeLanguageServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                            join d in employeeServices.List().Data
                            on c.EmployeeId equals d.Id
                            select new LanguageVerificationGridVm()
                            {
                                Id = c.Id,
                                EmployeeName = d.Name,
                                EmployeeCode = d.Code,
                                EmployeeId = c.EmployeeId,
                                Code = c.Language.Code,
                                Name = c.Language.Name,
                                Description = c.Language.Description,
                                ApprovedById = c.ApprovedById,
                            }).ToList();
            return new ServiceResult<List<LanguageVerificationGridVm>>()
            {
                Data = language,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EEmployeeLanguage> Approve(int id, int empId)
        {
            string msg = "";
            var language = employeeLanguageServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (language != null)
            {
                if (language.IsApproved == false)
                {
                    language.ApprovedById = RiddhaSession.CurrentUser.Id;
                    language.ApprovedOn = System.DateTime.Now;
                    language.IsApproved = true;
                    var result = employeeLanguageServices.Update(language);
                    Common.AddAuditTrail("7006", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Language Approve");
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EEmployeeLanguage>()
            {
                Data = language,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeLanguage> Revert(int id, int empId)
        {
            string msg = "";
            ResultStatus status = new ResultStatus();
            var language = employeeLanguageServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (language != null)
            {
                if (language.IsApproved)
                {
                    language.ApprovedById = null;
                    language.ApprovedOn = null;
                    language.IsApproved = false;
                    var result = employeeLanguageServices.Update(language);
                    Common.AddAuditTrail("7006", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Language Revert");
                    msg = "Reverted Successfully";
                    status = ResultStatus.Ok;
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<EEmployeeLanguage>()
            {
                Data = language,
                Message = msg,
                Status = status
            };
        }
        #endregion



    }
    public class LanguageGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BranchId { get; set; }
    }
    public class LanguageSearchVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class EmployeeLanguageGridVm
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageDescription { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public int EmployeeId { get; set; }
        public string CreatedOn { get; set; }
    }
    public class LanguageVerificationGridVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ApprovedById { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
    }
}
