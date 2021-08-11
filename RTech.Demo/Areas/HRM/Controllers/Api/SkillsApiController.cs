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
    public class SkillsApiController : ApiController
    {
        SSkills skillsServices = null;
        SEmployeeSkills employeeSkillServices = null;
        SUser userServices = null;
        LocalizedString loc = null;
        SEmployee employeeServices = null;
        public SkillsApiController()
        {
            skillsServices = new SSkills();
            employeeSkillServices = new SEmployeeSkills();
            loc = new LocalizedString();
            userServices = new SUser();
            employeeServices = new SEmployee();
        }
        public ServiceResult<List<SkillsGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var skillsLst = (from c in skillsServices.List().Data.Where(x => x.BranchId == branchId)
                             select new SkillsGridVm()
                             {
                                 Id = c.Id,
                                 Code = c.Code,
                                 Name = c.Name,
                                 Description = c.Description,
                                 BranchId = c.BranchId,
                             }).ToList();
            return new ServiceResult<List<SkillsGridVm>>()
            {
                Data = skillsLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ESkills> Get(int id)
        {
            ESkills skills = skillsServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ESkills>()
            {
                Data = skills,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ESkills> Post(ESkills model)
        {
            model.BranchId = RiddhaSession.BranchId;
            var result = skillsServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7008", "7150", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ESkills>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ESkills> Put(ESkills model)
        {
            var result = skillsServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7008", "7151", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ESkills>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var skills = skillsServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = skillsServices.Remove(skills);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7008", "7152", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #region Employee Skill
        [HttpGet]
        public ServiceResult<SkillSearchVm> SearchSkills(string skillCode = "", int skillId = 0)
        {
            SkillSearchVm vm = new SkillSearchVm();
            var skillList = skillsServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            ESkills skill = new ESkills();
            if (!string.IsNullOrEmpty(skillCode))
            {
                skill = skillList.Where(x => x.Code.ToLower() == skillCode.ToLower()).FirstOrDefault();
            }
            else if (skillId != 0)
            {
                skill = skillList.Where(x => x.Id == skillId).FirstOrDefault();
            }
            if (skill != null)
            {

                vm.Id = skill.Id;
                vm.Code = skill.Code;
                vm.Name = skill.Name;
                vm.Description = skill.Description;
            }
            return new ServiceResult<SkillSearchVm>()
            {
                Data = vm,
                Message = "Return Successfully",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<List<SkillSearchVm>> GetSkillLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<SkillSearchVm> resultLst = new List<SkillSearchVm>();
            string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
            if (string.IsNullOrEmpty(searchText))
            {
                return new ServiceResult<List<SkillSearchVm>>()
                {
                    Data = resultLst,
                    Status = ResultStatus.Ok
                };
            }
            if (model != null)
            {

                var skillLst = skillsServices.List().Data.Where(x => x.BranchId == branchId).ToList();
                if (searchText == "___")
                {
                    skillLst = skillLst.OrderBy(x => x.Name).Take(20).ToList();
                }
                else
                {
                    skillLst = skillLst.Where(x => x.Name.ToLower().Contains(searchText.Trim().ToLower())).ToList();
                }
                resultLst = (from c in skillLst
                             select new SkillSearchVm()
                             {
                                 Id = c.Id,
                                 Code = c.Code,
                                 Name = c.Code + " - " + c.Name,
                                 Description = c.Description
                             }).OrderBy(x => x.Name).ToList();
            }
            return new ServiceResult<List<SkillSearchVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<EmployeeSkillGridVm>> GetSkillByEmpId(int empId)
        {
            var employeeSkillLst = (from c in employeeSkillServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                                    select new EmployeeSkillGridVm()
                                    {
                                        Id = c.Id,
                                        BranchId = c.BranchId,
                                        SkillCode = c.Skills.Code,
                                        SkillDescription = c.Skills.Description,
                                        SkillId = c.SkillsId,
                                        SkillName = c.Skills.Name,
                                        EmployeeId = c.EmployeeId,
                                        CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd")
                                    }).ToList();
            return new ServiceResult<List<EmployeeSkillGridVm>>()
            {
                Data = employeeSkillLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<EEmployeeSkills> CreateEmpSkill(EEmployeeSkills model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = System.DateTime.Now;
            var result = employeeSkillServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7008", "7150", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, result.Data.Id, "Create Employee Skill");
            }
            return new ServiceResult<EEmployeeSkills>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPut]
        public ServiceResult<EEmployeeSkills> UpdateEmpSkill(EEmployeeSkills model)
        {
            var result = employeeSkillServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7008", "7151", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, "Update Employee Skill");
            }
            return new ServiceResult<EEmployeeSkills>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<int> DeleteEmpSkill(int id)
        {
            var empSkill = employeeSkillServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = employeeSkillServices.Remove(empSkill);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7008", "7152", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Delete employee skill");
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #endregion

        #region Employee Skill Verification
        [HttpPost]
        public KendoGridResult<List<SkillVerificationGridVm>> GetEmpSkillKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            List<EEmployeeSkills> employeeSkillQuery = new List<EEmployeeSkills>();
            IQueryable<EEmployeeSkills> employeeSkills = employeeSkillServices.List().Data.Where(x => x.Employee.BranchId == branchId && x.IsApproved == false);

            try
            {
                employeeSkillQuery = (from c in employeeSkills.ToList()
                                      join d in Common.GetEmployees().Data.ToList() on c.Id equals d.Id
                                      select c
                                                             ).ToList();
            }
            catch (Exception ex)
            {


            }
            var userQuery = userServices.List().Data;
            int totalRowNum = employeeSkillQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var skilllist = (from c in employeeSkillQuery
                             join d in employeeServices.List().Data
                             on c.EmployeeId equals d.Id
                             select new SkillVerificationGridVm()
                             {
                                 Id = c.Id,
                                 EmployeeName = d.Name,
                                 EmployeeCode = d.Code,
                                 EmployeeId = c.EmployeeId,
                                 Code = c.Skills.Code,
                                 Name = c.Skills.Name,
                                 Description = c.Skills.Description,
                                 ApprovedById = c.ApprovedById,
                                 ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                 ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById),

                             }).OrderByDescending(x => x.Id).ToList();
            return new KendoGridResult<List<SkillVerificationGridVm>>()
            {
                Data = skilllist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = skilllist.Count()
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
        public ServiceResult<List<SkillVerificationGridVm>> GetVerificationSkillByEmpId(int empId)
        {
            var skill = (from c in employeeSkillServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                         join d in employeeServices.List().Data
                         on c.EmployeeId equals d.Id
                         select new SkillVerificationGridVm()
                         {
                             Id = c.Id,
                             EmployeeName = d.Name,
                             EmployeeCode = d.Code,
                             EmployeeId = c.EmployeeId,
                             Code = c.Skills.Code,
                             Name = c.Skills.Name,
                             Description = c.Skills.Description,
                             ApprovedById = c.ApprovedById,
                         }).ToList();
            return new ServiceResult<List<SkillVerificationGridVm>>()
            {
                Data = skill,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeSkills> Approve(int id, int empId)
        {
            string msg = "";
            var skill = employeeSkillServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (skill != null)
            {
                if (skill.IsApproved == false)
                {
                    skill.ApprovedById = RiddhaSession.CurrentUser.Id;
                    skill.ApprovedOn = System.DateTime.Now;
                    skill.IsApproved = true;
                    var result = employeeSkillServices.Update(skill);
                    Common.AddAuditTrail("7008", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Skill Approve");
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EEmployeeSkills>()
            {
                Data = skill,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }


        [HttpGet]
        public ServiceResult<EEmployeeSkills> Revert(int id, int empId)
        {
            string msg = "";
            ResultStatus status = new ResultStatus();
            var skill = employeeSkillServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (skill != null)
            {
                if (skill.IsApproved)
                {
                    skill.ApprovedById = null;
                    skill.ApprovedOn = null;
                    skill.IsApproved = false;
                    var result = employeeSkillServices.Update(skill);
                    Common.AddAuditTrail("7008", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee Skill Revert");
                    msg = "Reverted Successfully";
                    status = ResultStatus.Ok;
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<EEmployeeSkills>()
            {
                Data = skill,
                Message = msg,
                Status = status
            };
        }
        #endregion
    }
    public class SkillsGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BranchId { get; set; }
    }

    public class SkillSearchVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EmployeeSkillGridVm
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string SkillCode { get; set; }
        public string SkillDescription { get; set; }
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int EmployeeId { get; set; }
        public string CreatedOn { get; set; }
    }

    public class SkillVerificationGridVm
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
