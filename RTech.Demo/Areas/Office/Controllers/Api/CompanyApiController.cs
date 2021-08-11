using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class CompanyApiController : ApiController
    {
        SCompany companyServices = null;
        SUser userServices = null;
        SBranch branchServices = null;
        LocalizedString loc = new LocalizedString();
        public CompanyApiController()
        {
            companyServices = new SCompany();
            userServices = new SUser();
            branchServices = new SBranch();
        }
        public ServiceResult<List<CompanyGridVm>> Get()
        {
            var companyLogin = companyServices.ListCompanyLogin().Data;
            List<CompanyGridVm> resultLst = new List<CompanyGridVm>();
            List<ECompany> ResellerCompanyList = new List<ECompany>();
            EUser user = RiddhaSession.CurrentUser;
            if (user.UserType == UserType.Reseller)
            {
                int resellerId = userServices.GetResellerLoginLst().Where(x => x.UserId == user.Id).FirstOrDefault().ResellerId;
                ResellerCompanyList = companyServices.List().Data.Where(x => x.ResellerId == resellerId).ToList();
                foreach (var item in ResellerCompanyList)
                {
                    CompanyGridVm vm = new CompanyGridVm();
                    vm.Id = item.Id;
                    vm.Code = item.Code;
                    vm.Name = item.Name;
                    vm.NameNp = item.NameNp;
                    vm.Address = item.Address;
                    vm.AddressNp = item.AddressNp;
                    vm.ContactNo = item.ContactNo;
                    vm.ContactPerson = item.ContactPerson;
                    vm.ContactPersonNp = item.ContactPersonNp;
                    vm.Email = item.Email;
                    vm.PAN = item.PAN;
                    vm.WebUrl = item.WebUrl;
                    vm.LogoUrl = item.LogoUrl;
                    vm.AllowDepartmentwiseAttendance = item.AllowDepartmentwiseAttendance;
                    vm.Status = getCompanyStatus(companyLogin, item.Id, item.SoftwareType);
                    vm.SoftwarePackageType = Enum.GetName(typeof(SoftwarePackageType), item.SoftwarePackageType);
                    vm.SoftwareType = Enum.GetName(typeof(SoftwareType), item.SoftwareType);
                    vm.OrganizationType = Enum.GetName(typeof(OrganizationType), item.OrganizationType);
                    vm.AllowDepartmentwiseAttendance = item.AllowDepartmentwiseAttendance;
                    vm.EmploymentStatusWiseLeave = item.EmploymentStatusWiseLeave;
                    vm.AutoLeaveApproved = item.AutoLeaveApproved;
                    resultLst.Add(vm);
                }
            }
            else if (user.UserType == UserType.User)
            {
                int companyId = user.Branch.CompanyId;
                var userCompanyList = companyServices.List().Data.Where(x => x.Id == companyId).ToList();
                foreach (var item in userCompanyList)
                {
                    CompanyGridVm vm = new CompanyGridVm();
                    vm.Id = item.Id;
                    vm.Code = item.Code;
                    vm.Name = item.Name;
                    vm.Address = item.Address;
                    vm.ContactNo = item.ContactNo;
                    vm.ContactPerson = item.ContactPerson;
                    vm.Email = item.Email;
                    vm.PAN = item.PAN;
                    vm.WebUrl = item.WebUrl;
                    vm.LogoUrl = item.LogoUrl;
                    vm.AllowDepartmentwiseAttendance = item.AllowDepartmentwiseAttendance;
                    vm.Status = getCompanyStatus(companyLogin, item.Id, item.SoftwareType);
                    vm.AllowDepartmentwiseAttendance = item.AllowDepartmentwiseAttendance;
                    vm.EmploymentStatusWiseLeave = item.EmploymentStatusWiseLeave;
                    vm.AutoLeaveApproved = item.AutoLeaveApproved;
                    resultLst.Add(vm);
                }
            }

            return new ServiceResult<List<CompanyGridVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<CompanyGridVm>> GetKendoGrid(KendoPageListArguments arg)
        {
            var companyLogin = companyServices.ListCompanyLogin().Data;
            List<CompanyGridVm> resultLst = new List<CompanyGridVm>();
            List<ECompany> ResellerCompanyList = new List<ECompany>();
            EUser user = RiddhaSession.CurrentUser;
            IQueryable<ECompany> companyQuery;
            if (user.UserType == UserType.Reseller)
            {
                int resellerId = userServices.GetResellerLoginLst().Where(x => x.UserId == user.Id).FirstOrDefault().ResellerId;
                companyQuery = companyServices.List().Data.Where(x => x.ResellerId == resellerId);
            }
            else
            {
                companyQuery = companyServices.List().Data;
            }
            int totalRowNum = companyQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<ECompany> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = companyQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = companyQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = companyQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = companyQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                default:
                    paginatedQuery = companyQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    break;
            }
            var result = (from c in paginatedQuery.ToList()
                          select new CompanyGridVm()
                          {
                              Id = c.Id,
                              Code = c.Code,
                              Name = c.Name,
                              NameNp = c.NameNp,
                              Address = c.Address,
                              AddressNp = c.AddressNp,
                              ContactNo = c.ContactNo,
                              ContactPerson = c.ContactPerson,
                              ContactPersonNp = c.ContactPersonNp,
                              Email = c.Email,
                              PAN = c.PAN,
                              WebUrl = c.WebUrl,
                              LogoUrl = c.LogoUrl,
                              AllowDepartmentwiseAttendance = c.AllowDepartmentwiseAttendance,
                              Status = getCompanyStatus(companyLogin, c.Id, c.SoftwareType),
                              SoftwarePackageType = Enum.GetName(typeof(SoftwarePackageType), c.SoftwarePackageType),
                              SoftwareType = Enum.GetName(typeof(SoftwareType), c.SoftwareType),
                              OrganizationType = Enum.GetName(typeof(OrganizationType), c.OrganizationType),
                              EmploymentStatusWiseLeave = c.EmploymentStatusWiseLeave,
                              AutoLeaveApproved = c.AutoLeaveApproved,
                          }).ToList();
            return new KendoGridResult<List<CompanyGridVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = result.Count(),
            };
        }

        private string getCompanyStatus(IQueryable<ECompanyLogin> companyLogin, int companyId, SoftwareType softwareType)
        {
            if (softwareType == SoftwareType.Desktop)
            {
                return "Approved";
            }
            bool IsSupended = false;
            var user = companyLogin.Where(x => x.CompanyId == companyId).FirstOrDefault();
            if (user == null)
            {
                return "New";
            }
            else
            {
                IsSupended = companyServices.List().Data.Where(x => x.Id == companyId).FirstOrDefault().IsSuspended;
            }
            return IsSupended == true ? "Suspended" : "Approved";
        }
        public ServiceResult<CompanyViewModel> Get(int id)
        {
            CompanyViewModel vm = new CompanyViewModel();
            vm.Company = companyServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            int userId = userServices.FindCompanyUser(id);
            EUser user = userServices.List().Data.Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                vm.UserName = user.Name;
                vm.Password = user.Password;
            }
            return new ServiceResult<CompanyViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<DateTime> GetLicenseExpiryDate(int period, DateTime issueDate)
        {
            DateTime expiryDate = issueDate.Date.AddYears(period);
            return new ServiceResult<DateTime>()
            {
                Data = expiryDate,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<ECompanyLicense> GetCompanyLicense(int companyId)
        {
            ECompanyLicense resultData = companyServices.ListCompanyLicense().Data.Where(x => x.CompanyId == companyId).FirstOrDefault() ?? new ECompanyLicense();
            return new ServiceResult<ECompanyLicense>()
            {
                Data = resultData,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public bool CheckDuplicateSNo(string code)
        {
            return companyServices.List().Data.Where(x => x.Code == code).Any();
        }
        [HttpGet]
        public bool CheckDuplicateEmail(string email)
        {
            return companyServices.List().Data.Where(x => x.Email == email).Any();
        }
        public ServiceResult<ECompany> Post(CompanyViewModel vm)
        {
            EUser sessionUser = RiddhaSession.CurrentUser;
            vm.Company.ResellerId = userServices.GetResellerLoginLst().Where(x => x.UserId == sessionUser.Id).FirstOrDefault().ResellerId;
            var compResult = companyServices.Add(vm.Company);
            if (compResult.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1003", "1015", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Company.Id, "Added Successfully");
            }
            var branch = branchServices.List().Data.Where(x => x.Company.Id == compResult.Data.Id).FirstOrDefault();
            if (vm.Company.SoftwareType != SoftwareType.Desktop)
            {
                if (vm.UserName != null && vm.Password != null)
                {
                    EUser user = new EUser() { UserType = UserType.User, Name = vm.UserName, Password = vm.Password, RoleId = null, BranchId = branch.Id };
                    var userResult = userServices.Add(user);
                    ECompanyLogin compLogin = new ECompanyLogin() { CompanyId = compResult.Data.Id, UserId = userResult.Data.Id };
                    userServices.AddCompanyLogin(compLogin);

                    new SShift().Add(new EShift()
                    {
                        BranchId = branch.Id,
                        ShiftName = "Default Shift",
                        ShiftCode = "DS",
                        ShiftType = ShiftType.Day,
                        ShiftEndTime = new TimeSpan(17, 0, 0),
                        ShiftStartTime = new TimeSpan(10, 0, 0)
                    });
                    new SFiscalYear().Add(new EFiscalYear()
                    {
                        BranchId = branch.Id,
                        CurrentFiscalYear = true,
                        StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                        EndDate = new DateTime(DateTime.Now.Year, 12, 31),
                        FiscalYear = DateTime.Now.Year.ToString()
                    });
                }
                #region wdms region
                WdmsData.WdmsEntities wdmsDb = new WdmsData.WdmsEntities();
                var wdmsCompany = new WdmsData.company()
                {
                    companyid = compResult.Data.Id,
                    companyname = compResult.Data.Name
                };
                try
                {
                    wdmsDb.Database.ExecuteSqlCommand(string.Format("insert into company values({0},'{1}')", compResult.Data.Id, compResult.Data.Code));
                    wdmsDb.Database.ExecuteSqlCommand(string.Format("INSERT INTO [dbo].[departments] ([DeptName],[supdeptid],[company_id],[SyncTag],[middleUpk_id])     VALUES('{0}',null,'{1}',0,NULL)", "Default Department For " + vm.Company.Code, compResult.Data.Id));
                }
                catch (Exception)
                {

                    return compResult;
                }
                #endregion
            }
            return compResult;
        }
        public ServiceResult<CompanyViewModel> Put(CompanyViewModel vm)
        {
            EUser sessionUser = RiddhaSession.CurrentUser;
            vm.Company.ResellerId = userServices.GetResellerLoginLst().Where(x => x.UserId == sessionUser.Id).FirstOrDefault().ResellerId;
            var compResult = companyServices.Update(vm.Company);
            if (compResult.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1003", "1015", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Company.Id, "Update Successfully");
            }
            var branch = branchServices.List().Data.Where(x => x.Company.Id == compResult.Data.Id).FirstOrDefault();
            int userId = userServices.FindCompanyUser(compResult.Data.Id);
            var user = userServices.List().Data.Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                user.Name = vm.UserName;
                user.Password = vm.Password;
                userServices.Update(user);
            }
            else
            {
                if (vm.UserName != null && vm.Password != null)
                {
                    EUser newUser = new EUser() { UserType = UserType.User, Name = vm.UserName, Password = vm.Password, RoleId = null, BranchId = branch.Id };
                    var userResult = userServices.Add(newUser);
                    ECompanyLogin compLogin = new ECompanyLogin() { CompanyId = compResult.Data.Id, UserId = userResult.Data.Id };
                    userServices.AddCompanyLogin(compLogin);
                }
            }
            return new ServiceResult<CompanyViewModel>()
            {
                Data = vm,
                Message = loc.Localize("UpdatedSuccess"),
                Status = ResultStatus.Ok
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            CompanyViewModel vm = new CompanyViewModel();
            var company = companyServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            int userId = userServices.FindCompanyUser(id);
            var user = userServices.List().Data.Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                return new ServiceResult<int>()
                {
                    Data = 0,
                    Message = "The user account of company is existed so it can not be deleted",
                    Status = ResultStatus.processError
                };

            }
            else
            {
                SSection sectionServices = new SSection();
                SDepartment departmentServices = new SDepartment();
                SBranch branchServices = new SBranch();
                var section = sectionServices.List().Data.Where(x => x.Department.Branch.CompanyId == id).FirstOrDefault();
                var department = departmentServices.List().Data.Where(x => x.Branch.CompanyId == id).FirstOrDefault();
                var branch = branchServices.List().Data.Where(x => x.CompanyId == id).FirstOrDefault();
                if (user == null)
                {
                    sectionServices.Remove(section);
                    departmentServices.Remove(department);
                    branchServices.Remove(branch);
                }
            }
            return companyServices.Remove(company);
        }
        [HttpGet]
        public ServiceResult<CompanyGridVm> Suspend(int id)
        {
            string msg = "";
            var company = companyServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (company != null)
            {
                if (company.IsSuspended == true)
                {
                    company.IsSuspended = false;
                    var result = companyServices.Update(company);
                    if (result.Status == ResultStatus.Ok)
                    {
                        msg = "Approved Successfully";
                    }
                }
                else if (company.IsSuspended == false)
                {
                    company.IsSuspended = true;
                    var result = companyServices.Update(company);
                    if (result.Status == ResultStatus.Ok)
                    {
                        msg = "Suspended Successfully";
                    }
                }
            }
            return new ServiceResult<CompanyGridVm>
            {
                Data = null,
                Status = ResultStatus.Ok,
                Message = msg
            };
        }
        [HttpGet, ActionFilter("1040")]
        public ServiceResult<ECompany> GetComapnyProfile()
        {
            companyServices = new SCompany(false);
            var company = companyServices.List().Data.Where(x => x.Id == RiddhaSession.CompanyId).FirstOrDefault();

            company.Reseller = null;

            return new ServiceResult<ECompany>()
            {
                Data = company,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<ECompanyLicense> SaveCompanyLicense(ECompanyLicense model)
        {
            var result = companyServices.AddCompanyLicense(model);
            var company = companyServices.Find(model.CompanyId);
            SReseller resellerServices = new SReseller();
            var reseller = resellerServices.ListResellerLogin().Data.Where(x => x.UserId == RiddhaSession.UserId).FirstOrDefault();
            if (reseller != null)
            {
                MailCommon mailServices = new MailCommon();

                string baseUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                string fromDate = model.IssueDate.ToString("yyyy/MM/dd");
                string toDate = model.ExpiryDate.ToString("yyyy/MM/dd");
                string msg = reseller.Reseller.Name + " has created " + model.LicensePeriod + " year license from " + fromDate + " to " + toDate + " for " + company.Data.Name + ", " + company.Data.Address;
                string mailTemplateLink = baseUrl + "/Templates/EmailTemplate/Index?msg=" + msg;

                new Thread(() =>
                {
                    mailServices.SendCustomizedEmail(mailTemplateLink);

                }).Start();

            }
            return new ServiceResult<ECompanyLicense>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = result.Message
            };
        }

        [HttpPut, ActionFilter("1015")]
        public ServiceResult<bool> UpdateCompanyProfile(ECompany model)
        {
            ECompany company = companyServices.List().Data.Where(x => x.Id == model.Id).FirstOrDefault();
            company.Address = model.Address;
            company.AddressNp = model.AddressNp;
            company.ContactNo = model.ContactNo;
            company.ContactPerson = model.ContactPerson;
            company.ContactPersonNp = model.ContactPersonNp;
            company.Email = model.Email;
            company.LogoUrl = model.LogoUrl;
            company.Name = model.Name;
            company.NameNp = model.NameNp;
            company.PAN = model.PAN;
            company.WebUrl = model.WebUrl;
            company.AllowDepartmentwiseAttendance = model.AllowDepartmentwiseAttendance;
            company.EmploymentStatusWiseLeave = model.EmploymentStatusWiseLeave;
            company.OrganizationType = model.OrganizationType;
            company.AutoLeaveApproved = model.AutoLeaveApproved;
            company.MinimumOTHour = model.MinimumOTHour;
            var result = companyServices.Update(company);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1003", "1015", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, result.Data.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<bool>()
            {
                Data = true,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<LicenseInfoVm> GetLicenseInfo()
        {
            var companyId = RiddhaSession.CompanyId;
            var LicenseInfo = (from c in companyServices.ListCompanyLicense().Data.Where(x => x.CompanyId == companyId).ToList()
                               select new LicenseInfoVm()
                               {
                                   LicensePeriod = c.LicensePeriod,
                                   ExpiryDate = c.ExpiryDate,
                                   IssueDate = c.IssueDate,
                                   CompanyLogo = c.Company.LogoUrl,
                                   Price = c.Company.Price,
                                   SoftwarePackageType = Enum.GetName(typeof(SoftwarePackageType), c.Company.SoftwarePackageType),
                                   CompanyName = c.Company.Name
                               }).FirstOrDefault();
            return new ServiceResult<LicenseInfoVm>()
            {
                Data = LicenseInfo,
                Status = ResultStatus.Ok
            };
        }
    }
    public class CompanyViewModel
    {
        public ECompany Company { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class CompanyGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string Address { get; set; }
        public string AddressNp { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNp { get; set; }
        public string Email { get; set; }
        public string WebUrl { get; set; }
        public string PAN { get; set; }
        public string LogoUrl { get; set; }
        public string Status { get; set; }
        public string SoftwarePackageType { get; set; }
        public string SoftwareType { get; set; }
        public string OrganizationType { get; set; }
        public bool AllowDepartmentwiseAttendance { get; set; }
        public bool EmploymentStatusWiseLeave { get; set; }
        public bool AutoLeaveApproved { get; set; }
    }

    public class LicenseInfoVm
    {
        public int LicensePeriod { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SoftwarePackageType { get; set; }
        public decimal Price { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
    }

}
