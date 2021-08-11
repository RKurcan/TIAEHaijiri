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
    public class LicenseApiController : ApiController
    {
        SLicense licenseServices = null;
        SEmployeeLicense employeeLicenseServices = null;
        LocalizedString loc = null;
        SUser userServices = null;
        SEmployee employeeServices = null;
        public LicenseApiController()
        {
            licenseServices = new SLicense();
            loc = new LocalizedString();
            employeeLicenseServices = new SEmployeeLicense();
            userServices = new SUser();
            employeeServices = new SEmployee();
        }
        public ServiceResult<List<LicenseGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var languageLst = (from c in licenseServices.List().Data.Where(x => x.BranchId == branchId)
                               select new LicenseGridVm()
                               {
                                   Id = c.Id,
                                   Code = c.Code,
                                   Name = c.Name,
                                   Description = c.Description,
                                   BranchId = c.BranchId,
                               }).ToList();
            return new ServiceResult<List<LicenseGridVm>>()
            {
                Data = languageLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ELicense> Get(int id)
        {
            ELicense license = licenseServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ELicense>()
            {
                Data = license,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ELicense> Post(ELicense model)
        {
            model.BranchId = RiddhaSession.BranchId;
            var result = licenseServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7007", "7146", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ELicense>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<ELicense> Put(ELicense model)
        {
            var result = licenseServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7007", "7147", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ELicense>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var license = licenseServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = licenseServices.Remove(license);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7007", "7148", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        #region Employee License
        [HttpGet]
        public ServiceResult<LicenseSearchVm> SearchLicense(string licenseCode = "", int licenseId = 0)
        {
            LicenseSearchVm vm = new LicenseSearchVm();
            var licenseList = licenseServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            ELicense license = new ELicense();
            if (!string.IsNullOrEmpty(licenseCode))
            {
                license = licenseList.Where(x => x.Code.ToLower() == licenseCode.ToLower()).FirstOrDefault();
            }
            else if (licenseId != 0)
            {
                license = licenseList.Where(x => x.Id == licenseId).FirstOrDefault();
            }
            if (license != null)
            {

                vm.Id = license.Id;
                vm.Code = license.Code;
                vm.Name = license.Name;
                vm.Description = license.Description;
            }
            return new ServiceResult<LicenseSearchVm>()
            {
                Data = vm,
                Message = "Return Successfully",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<List<LicenseSearchVm>> GetLicenseLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<LicenseSearchVm> resultLst = new List<LicenseSearchVm>();
            string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
            if (string.IsNullOrEmpty(searchText))
            {
                return new ServiceResult<List<LicenseSearchVm>>()
                {
                    Data = resultLst,
                    Status = ResultStatus.Ok
                };
            }
            if (model != null)
            {

                var licenseList = licenseServices.List().Data.Where(x => x.BranchId == branchId).ToList();
                if (searchText == "___")
                {
                    licenseList = licenseList.OrderBy(x => x.Name).Take(20).ToList();
                }
                else
                {
                    licenseList = licenseList.Where(x => x.Name.ToLower().Contains(searchText.Trim().ToLower())).ToList();
                }
                resultLst = (from c in licenseList
                             select new LicenseSearchVm()
                             {
                                 Id = c.Id,
                                 Code = c.Code,
                                 Name = c.Code + " - " + c.Name,
                                 Description = c.Description
                             }).OrderBy(x => x.Name).ToList();
            }
            return new ServiceResult<List<LicenseSearchVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<EmployeeLicenseGridVm>> GetLicenseByEmpId(int empId)
        {
            var employeeLicenseLst = (from c in employeeLicenseServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                                      select new EmployeeLicenseGridVm()
                                      {
                                          Id = c.Id,
                                          BranchId = c.BranchId,
                                          LicenseCode = c.License.Code,
                                          LicenseDescription = c.License.Description,
                                          LicenseId = c.LicenseId,
                                          LicenseName = c.License.Name,
                                          EmployeeId = c.EmployeeId,
                                          CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd")
                                      }).ToList();
            return new ServiceResult<List<EmployeeLicenseGridVm>>()
            {
                Data = employeeLicenseLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<EEmployeeLicense> CreateEmpLicense(EEmployeeLicense model)
        {
            model.BranchId = (int)RiddhaSession.BranchId;
            model.CreatedById = RiddhaSession.UserId;
            model.CreatedOn = System.DateTime.Now;
            var result = employeeLicenseServices.Add(model);

            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7007", "7146", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, "Add employee license");
            }
            return new ServiceResult<EEmployeeLicense>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPut]
        public ServiceResult<EEmployeeLicense> UpdateEmpLicense(EEmployeeLicense model)
        {
            var result = employeeLicenseServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7007", "7147", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, "Update employee license");
            }
            return new ServiceResult<EEmployeeLicense>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<int> DeleteEmpLicense(int id)
        {
            var empLicense = employeeLicenseServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = employeeLicenseServices.Remove(empLicense);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("7007", "7148", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Delete employee license");
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        #endregion

        #region Employee License Verification
        [HttpPost]
        public KendoGridResult<List<LicenseverificationVm>> GetEmpLicenseKendoGrid(KendoPageListArguments vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;

            IQueryable<EEmployeeLicense> employeeLicenseQuery = employeeLicenseServices.List().Data.Where(x => x.License.BranchId == branchId && x.IsApproved ==false);
            var userQuery = userServices.List().Data;
            int totalRowNum = employeeLicenseQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            var licenselist = (from c in employeeLicenseQuery.ToList()
                               join d in Common.GetEmployees().Data.ToList() on c.Id equals d.Id
                               select new LicenseverificationVm()
                               {
                                   Id = c.Id,
                                   EmployeeName = d.Name,
                                   EmployeeCode = d.Code,
                                   EmployeeId = c.EmployeeId,
                                   Code = c.License.Code,
                                   Name = c.License.Name,
                                   Description = c.License.Description,
                                   ApprovedById = c.ApprovedById,
                                   ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString("yyyy/MM/dd") : "Not Approved",
                                   ApprovedBy = getapprovedByUser(userQuery, c.ApprovedById),

                               }).OrderByDescending(x => x.Id).ToList();
            return new KendoGridResult<List<LicenseverificationVm>>()
            {
                Data = licenselist.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = licenselist.Count()
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
        public ServiceResult<List<LicenseverificationVm>> GetVerificationLicenseByEmpId(int empId)
        {
            var license = (from c in employeeLicenseServices.List().Data.Where(x => x.EmployeeId == empId).ToList()
                           join d in employeeServices.List().Data
                           on c.EmployeeId equals d.Id
                           select new LicenseverificationVm()
                           {
                               Id = c.Id,
                               EmployeeName = d.Name,
                               EmployeeCode = d.Code,
                               EmployeeId = c.EmployeeId,
                               Code = c.License.Code,
                               Name = c.License.Name,
                               Description = c.License.Description,
                               ApprovedById = c.ApprovedById,
                           }).ToList();
            return new ServiceResult<List<LicenseverificationVm>>()
            {
                Data = license,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EEmployeeLicense> Approve(int id, int empId)
        {
            string msg = "";
            var license = employeeLicenseServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (license != null)
            {
                if (license.IsApproved == false)
                {
                    license.ApprovedById = RiddhaSession.CurrentUser.Id;
                    license.ApprovedOn = System.DateTime.Now;
                    license.IsApproved = true;
                    var result = employeeLicenseServices.Update(license);
                    Common.AddAuditTrail("7007", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee license approve");
                    msg = "Approved Successfully";
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EEmployeeLicense>()
            {
                Data = license,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeLicense> Revert(int id, int empId)
        {
            string msg = "";
            ResultStatus status = new ResultStatus();
            var license = employeeLicenseServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (license != null)
            {
                if (license.IsApproved)
                {
                    license.ApprovedById = null;
                    license.ApprovedOn = null;
                    license.IsApproved = false;
                    var result = employeeLicenseServices.Update(license);
                    Common.AddAuditTrail("7007", "", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, "Employee license revert");
                    msg = "Reverted Successfully";
                    status = ResultStatus.Ok;
                }
                else
                {
                    msg = "Already Reverted";
                    status = ResultStatus.processError;
                }
            }
            return new ServiceResult<EEmployeeLicense>()
            {
                Data = license,
                Message = msg,
                Status = status
            };
        }
        #endregion
    }
    public class LicenseGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BranchId { get; set; }
    }
    public class LicenseSearchVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class EmployeeLicenseGridVm
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string LicenseCode { get; set; }
        public string LicenseDescription { get; set; }
        public int LicenseId { get; set; }
        public string LicenseName { get; set; }
        public int EmployeeId { get; set; }
        public string CreatedOn { get; set; }
    }
    public class LicenseverificationVm
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
