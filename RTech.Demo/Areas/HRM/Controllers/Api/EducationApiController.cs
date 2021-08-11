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
    public class EducationApiController : ApiController
    {
        SEducation educationServices = null;
        SUser userServices = null;
        SEmployee employeeServices = null;
        SEmployeeEducation employeeEducationServices = null;
        LocalizedString loc = null;
        int branchId = (int)RiddhaSession.BranchId;
        public EducationApiController()
        {
            educationServices = new SEducation();
            employeeEducationServices = new SEmployeeEducation();
            userServices = new SUser();
            employeeServices = new SEmployee();
            loc = new LocalizedString();
        }
        public ServiceResult<List<EducationGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var educationLst = (from c in educationServices.List().Data.Where(x => x.BranchId == branchId)
                                select new EducationGridVm()
                                {
                                    Id = c.Id,
                                    Code = c.Code,
                                    Name = c.Name,
                                    Description = c.Description,
                                    BranchId = c.BranchId,
                                }).ToList();
            return new ServiceResult<List<EducationGridVm>>()
            {
                Data = educationLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EEducation> Get(int id)
        {
            EEducation education = educationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EEducation>()
            {
                Data = education,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EEducation> Post(EEducation model)
        {
            model.BranchId = RiddhaSession.BranchId;
            var result = educationServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7005", "7138", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EEducation>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        public ServiceResult<EEducation> Put(EEducation model)
        {
            var result = educationServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7005", "7139", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<EEducation>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        public ServiceResult<int> Delete(int id)
        {
            var education = educationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = educationServices.Remove(education);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7005", "7140", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #region Employee Education
        [HttpGet]
        public ServiceResult<EducationSearchVm> SearchEducation(string eduCode = "", int eduId = 0)
        {
            EducationSearchVm vm = new EducationSearchVm();
            var educationList = educationServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            EEducation education = new EEducation();
            if (!string.IsNullOrEmpty(eduCode))
            {
                education = educationList.Where(x => x.Code.ToLower() == eduCode.ToLower()).FirstOrDefault();
            }
            else if (eduId != 0)
            {
                education = educationList.Where(x => x.Id == eduId).FirstOrDefault();
            }
            if (education != null)
            {

                vm.Id = education.Id;
                vm.Code = education.Code;
                vm.Name = education.Name;
                vm.Description = education.Description;
            }
            return new ServiceResult<EducationSearchVm>()
            {
                Data = vm,
                Message = "Return Successfully",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<List<EducationSearchVm>> GetEduLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<EducationSearchVm> resultLst = new List<EducationSearchVm>();
            string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
            if (string.IsNullOrEmpty(searchText))
            {
                return new ServiceResult<List<EducationSearchVm>>()
                {
                    Data = resultLst,
                    Status = ResultStatus.Ok
                };
            }
            if (model != null)
            {

                var educationLst = educationServices.List().Data.Where(x => x.BranchId == branchId).ToList();
                if (searchText == "___")
                {
                    educationLst = educationLst.OrderBy(x => x.Name).Take(20).ToList();
                }
                else
                {
                    educationLst = educationLst.Where(x => x.Name.ToLower().Contains(searchText.Trim().ToLower())).ToList();
                }
                resultLst = (from c in educationLst
                             select new EducationSearchVm()
                             {
                                 Id = c.Id,
                                 Code = c.Code,
                                 Name = c.Code + " - " + c.Name,
                                 Description = c.Description
                             }).OrderBy(x => x.Name).ToList();
            }
            return new ServiceResult<List<EducationSearchVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<EmployeeEducationGridVm>> GetEducationByEmpId(int empId)
        {
            var employeeEducationLst = (from c in employeeEducationServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                                        select new EmployeeEducationGridVm()
                                        {
                                            Id = c.Id,
                                            BranchId = c.BranchId,
                                            EducationCode = c.Education.Code,
                                            EducationDescription = c.Education.Description,
                                            EducationId = c.EducationId,
                                            EducationName = c.Education.Name,
                                            EmployeeId = c.EmployeeId,
                                            CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd")
                                        }).ToList();
            return new ServiceResult<List<EmployeeEducationGridVm>>()
            {
                Data = employeeEducationLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<EEmployeeEducation> CreateEmpEducation(EEmployeeEducation model)
        {
            model.BranchId = branchId;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = System.DateTime.Now;
            var result = employeeEducationServices.Add(model);
            return new ServiceResult<EEmployeeEducation>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPut]
        public ServiceResult<EEmployeeEducation> UpdateEmpEducation(EEmployeeEducation model)
        {
            var result = employeeEducationServices.Update(model);
            return new ServiceResult<EEmployeeEducation>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<int> DeleteEmpEducation(int id)
        {
            var empEducation = employeeEducationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = employeeEducationServices.Remove(empEducation);
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #endregion

        #region Employee Education Verification
        [HttpPost]
        public KendoGridResult<List<EducationVerificationGridVm>> GetEmpEducationKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            List<EEmployeeEducation> employeeEducationQuery = new List<EEmployeeEducation>();
            IQueryable<EEmployeeEducation> employeeEducations = employeeEducationServices.List().Data.Where(x => x.Employee.BranchId == branchId && x.IsApproved == false);
            try
            {
                employeeEducationQuery = (from c in employeeEducations
                                          join d in Common.GetEmployees().Data.ToList() on c.Id equals d.Id
                                          select c
                                                             ).ToList();
            }
            catch (Exception ex)
            {

            }


            var userQuery = userServices.List().Data;
            int totalRowNum = employeeEducationQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var educationlist = (from c in employeeEducationQuery
                                 join d in employeeServices.List().Data
                                 on c.EmployeeId equals d.Id
                                 select new EducationVerificationGridVm()
                                 {
                                     Id = c.Id,
                                     EmployeeName = d.Name,
                                     EmployeeCode = d.Code,
                                     EmployeeId = c.EmployeeId,
                                     Code = c.Education.Code,
                                     Name = c.Education.Name,
                                     Description = c.Education.Description,
                                     ApprovedById = c.ApprovedById,
                                     ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                     ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById),
                                     totalRowNum = totalRowNum
                                 }).OrderByDescending(x => x.Id).ToList();
            return new KendoGridResult<List<EducationVerificationGridVm>>()
            {
                Data = educationlist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = educationlist.Count()
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
        public ServiceResult<List<EducationVerificationGridVm>> GetVerificationEducationByEmpId(int empId)
        {
            var education = (from c in employeeEducationServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                             join d in employeeServices.List().Data
                             on c.EmployeeId equals d.Id
                             select new EducationVerificationGridVm()
                             {
                                 Id = c.Id,
                                 EmployeeName = d.Name,
                                 EmployeeCode = d.Code,
                                 EmployeeId = c.EmployeeId,
                                 Code = c.Education.Code,
                                 Name = c.Education.Name,
                                 Description = c.Education.Description,
                                 ApprovedById = c.ApprovedById,
                             }).ToList();
            return new ServiceResult<List<EducationVerificationGridVm>>()
            {
                Data = education,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EEmployeeEducation> Approve(int id, int empId)
        {
            string msg = "";
            var education = employeeEducationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (education != null)
            {
                if (education.IsApproved == false)
                {
                    education.ApprovedById = RiddhaSession.CurrentUser.Id;
                    education.ApprovedOn = System.DateTime.Now;
                    education.IsApproved = true;
                    var result = employeeEducationServices.Update(education);
                    Common.AddAuditTrail("7005", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee education approve");
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EEmployeeEducation>()
            {
                Data = education,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeEducation> Revert(int id, int empId)
        {
            string msg = "";
            var status = new ResultStatus();
            var education = employeeEducationServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (education != null)
            {
                if (education.IsApproved)
                {
                    education.ApprovedById = null;
                    education.ApprovedOn = null;
                    education.IsApproved = false;
                    var result = employeeEducationServices.Update(education);
                    Common.AddAuditTrail("7005", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee education revert");
                    msg = "Reverted Successfully";
                    status = ResultStatus.Ok;
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<EEmployeeEducation>()
            {
                Data = education,
                Message = msg,
                Status = status
            };
        }
        #endregion
    }
    public class EducationGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BranchId { get; set; }
    }

    public class EducationSearchVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EmployeeEducationGridVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int EducationId { get; set; }
        public string EducationName { get; set; }
        public string EducationCode { get; set; }
        public string EducationDescription { get; set; }
        public int BranchId { get; set; }
        public string CreatedOn { get; set; }
    }

    public class EducationVerificationGridVm
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
        public int totalRowNum { get; set; }
    }
}
