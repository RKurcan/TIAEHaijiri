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
    public class ExperienceApiController : ApiController
    {
        SExperience experienceServices = null;
        LocalizedString loc = null;
        SUser userServices = null;
        SEmployee employeeServices = null;
        public ExperienceApiController()
        {
            experienceServices = new SExperience();
            loc = new LocalizedString();
            userServices = new SUser();
            employeeServices = new SEmployee();
        }
        public ServiceResult<List<ExperienceGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var experienceLst = (from c in experienceServices.List().Data.Where(x => x.BranchId == branchId)
                                 select new ExperienceGridVm()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     Title = c.Title,
                                     Description = c.Description,
                                     BranchId = c.BranchId,
                                     OrganizationName = c.OrganizationName,
                                     BeganOn = c.BeganOn,
                                     EndedOn = c.EndedOn
                                 }).ToList();
            return new ServiceResult<List<ExperienceGridVm>>()
            {
                Data = experienceLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<ExperienceGridVm>> GetExperienceByEmpId(int empId)
        {
            var employeeExperienceLst = (from c in experienceServices.List().Data.Where(x => x.EmployeeId == empId)
                                         select new ExperienceGridVm()
                                         {
                                             Id = c.Id,
                                             Code = c.Code,
                                             Title = c.Title,
                                             Description = c.Description,
                                             BranchId = c.BranchId,
                                             OrganizationName = c.OrganizationName,
                                             BeganOn = c.BeganOn,
                                             EndedOn = c.EndedOn,
                                             EmployeeId = c.EmployeeId
                                         }).ToList();
            return new ServiceResult<List<ExperienceGridVm>>()
            {
                Data = employeeExperienceLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EExperience> Get(int id)
        {
            EExperience experience = experienceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EExperience>()
            {
                Data = experience,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EExperience> Post(EExperience model)
        {
            model.BranchId = RiddhaSession.BranchId;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = System.DateTime.Now;
            var result = experienceServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1010", "1035", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EExperience>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<EExperience> Put(EExperience model)
        {
            var result = experienceServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1010", "1036", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EExperience>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var experience = experienceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = experienceServices.Remove(experience);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1010", "1037", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #region Employeee Experience verification
        [HttpPost]
        public KendoGridResult<List<ExperienceVerificationVm>> GetEmpExperienceKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            List<EExperience> employeeExperienceQuery = new List<EExperience>();
            IQueryable<EExperience> experiences = experienceServices.List().Data.Where(x => x.BranchId == branchId && x.IsApproved == false);
            try
            {
                employeeExperienceQuery = (from c in experiences.ToList()
                                           join d in Common.GetEmployees().Data.ToList() on c.Id equals d.Id
                                           select c
                                                            ).ToList();
            }
            catch (Exception ex)
            {

            }


            var userQuery = userServices.List().Data;
            int totalRowNum = employeeExperienceQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var experiencelist = (from c in employeeExperienceQuery
                                  join d in employeeServices.List().Data
                                  on c.EmployeeId equals d.Id
                                  select new ExperienceVerificationVm()
                                  {
                                      Id = c.Id,
                                      EmployeeName = d.Name,
                                      EmployeeCode = d.Code,
                                      EmployeeId = c.EmployeeId,
                                      Code = c.Code,
                                      Title = c.Title,
                                      Description = c.Description,
                                      OrganizationName = c.OrganizationName,
                                      BeganOn = c.BeganOn.ToString("yyyy/MM/dd"),
                                      EndedOn = c.EndedOn.ToString("yyyy/MM/dd"),
                                      ApprovedById = c.ApprovedById,
                                      ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                      ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById),

                                  }).OrderByDescending(x => x.Id).ToList();
            return new KendoGridResult<List<ExperienceVerificationVm>>()
            {
                Data = experiencelist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = experiencelist.Count()
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
        public ServiceResult<List<ExperienceVerificationVm>> GetVerificationExperienceByEmpId(int empId)
        {
            var experience = (from c in experienceServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                              join d in employeeServices.List().Data
                              on c.EmployeeId equals d.Id
                              select new ExperienceVerificationVm()
                              {
                                  Id = c.Id,
                                  EmployeeName = d.Name,
                                  EmployeeCode = d.Code,
                                  EmployeeId = c.EmployeeId,
                                  Code = c.Code,
                                  Title = c.Title,
                                  Description = c.Description,
                                  OrganizationName = c.OrganizationName,
                                  BeganOn = c.BeganOn.ToString("yyyy/MM/dd"),
                                  EndedOn = c.EndedOn.ToString("yyyy/MM/dd"),
                                  ApprovedById = c.ApprovedById,
                              }).ToList();
            return new ServiceResult<List<ExperienceVerificationVm>>()
            {
                Data = experience,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EExperience> Approve(int id, int empId)
        {
            string msg = "";
            var experience = experienceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (experience != null)
            {
                if (experience.IsApproved == false)
                {
                    experience.ApprovedById = RiddhaSession.CurrentUser.Id;
                    experience.ApprovedOn = System.DateTime.Now;
                    experience.IsApproved = true;
                    var result = experienceServices.Update(experience);
                    Common.AddAuditTrail("1010", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Experience Approve");
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EExperience>()
            {
                Data = experience,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }
        #endregion

        [HttpGet]
        public ServiceResult<EExperience> Revert(int id, int empId)
        {
            string msg = "";
            var status = new ResultStatus();
            var experience = experienceServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (experience != null)
            {
                if (experience.IsApproved)
                {
                    experience.ApprovedById = null;
                    experience.ApprovedOn = null;
                    experience.IsApproved = false;
                    var result = experienceServices.Update(experience);
                    Common.AddAuditTrail("1010", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Experience Revert");
                    msg = "Reverted Successfully";
                    status = ResultStatus.Ok;
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<EExperience>()
            {
                Data = experience,
                Message = msg,
                Status = status
            };
        }
    }
    public class ExperienceGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OrganizationName { get; set; }
        public DateTime BeganOn { get; set; }
        public DateTime EndedOn { get; set; }
        public int EmployeeId { get; set; }
        public int? BranchId { get; set; }
    }
    public class ExperienceVerificationVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OrganizationName { get; set; }
        public string BeganOn { get; set; }
        public string EndedOn { get; set; }
        public int? ApprovedById { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public int BranchId { get; set; }
    }
}
