using Riddhasoft.Device.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class ClientSearchApiController : ApiController
    {
        LocalizedString _loc = null;
        public ClientSearchApiController()
        {
            _loc = new LocalizedString();
        }
        [HttpPost]
        public KendoGridResult<List<ClientGridVm>> GetKendoGrid(KendoResellerReportArg arg)
        {
            SCompany companyServices = new SCompany();
            //IQueryable<ECompany> companyQuery;
            List<ECompany> companyQuery = new List<ECompany>();
            if (RiddhaSession.UserType == (int)UserType.Reseller)
            {
                SUser userServices = new SUser();
                var reseller = userServices.GetResellerLoginLst().Where(x => x.UserId == RiddhaSession.UserId).FirstOrDefault();
                if (reseller != null)
                {
                    companyQuery = companyServices.List().Data.Where(x => x.ResellerId == reseller.Id).ToList();
                }
            }
            else
            {
                companyQuery = companyServices.List().Data.ToList();
            }
            //companyQuery = companyServices.List().Data;
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            List<ECompany> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = companyQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name).ToList();
                    }
                    else
                    {
                        paginatedQuery = companyQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name).ToList();
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = companyQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code).ToList();
                    }
                    else
                    {
                        paginatedQuery = companyQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code).ToList();
                    }
                    break;
                default:
                    paginatedQuery = companyQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name).ToList();
                    break;
            }
            if (!string.IsNullOrEmpty(arg.SearchText))
            {
                var device = new SDevice().List().Data.Where(x => x.SerialNumber == arg.SearchText).FirstOrDefault();
                if (device != null)
                {
                    var compDevice = new SCompanyDeviceAssignment().List().Data.Where(x => x.DeviceId == device.Id).FirstOrDefault();
                    paginatedQuery = companyServices.List().Data.Where(x => x.Id == compDevice.CompanyId).ToList();
                };
            }
            var list = (from c in paginatedQuery.ToList()
                        select new ClientGridVm()
                        {
                            Id = c.Id,
                            Address = c.Address,
                            Code = c.Code,
                            ContactNo = c.ContactNo,
                            ContactPerson = c.ContactPerson,
                            Name = c.Name,
                            SoftwarePackage = Enum.GetName(typeof(SoftwarePackageType), c.SoftwarePackageType),
                            ResellerName = c.Reseller.Name,
                        }).ToList();
            return new KendoGridResult<List<ClientGridVm>>()
            {
                Data = list.OrderBy(x => x.Name).Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = list.Count()
            };
        }

        public ServiceResult<ClientInfoVm> GetClientInfo(int companyId)
        {
            SReseller resellerServices = new SReseller();
            SCompany companyServices = new SCompany();
            SUser userServices = new SUser();
            ClientInfoVm vm = new ClientInfoVm();
            vm.CompanyLicenseLog = new List<CompanyLicenseLog>();
            vm.CompanyLogin = new List<CompanyLogin>();
            vm.CompanyResellerInfo = new CompanyResellerInfo();
            int resellerId = companyServices.List().Data.Where(x => x.Id == companyId).FirstOrDefault().ResellerId;
            int resellerUserId = resellerServices.ListResellerLogin().Data.Where(x => x.ResellerId == resellerId).FirstOrDefault().UserId;
            vm.CompanyLicenseLog = (from c in companyServices.ListCompanyLicenseLog().Data.Where(x => x.CompanyId == companyId).ToList()
                                    select new CompanyLicenseLog()
                                    {
                                        Id = c.Id,
                                        IsseDate = c.IssueDate.ToString("yyyy/MM/dd"),
                                        LicensePeriod = c.LicensePeriod,
                                        DueAmount = c.DueAmount,
                                        FileAttachment = c.FileAttachment,
                                        IsPaid = c.IsPaid,
                                        PaidAmount = c.PaidAmount,
                                        PaymentMethod = c.PaymentMethod,
                                        PaymentMethodName = Enum.GetName(typeof(PaymentMethod), c.PaymentMethod),
                                        Rate = c.Rate,
                                        Remark = c.Remark
                                    }).ToList();

            vm.CompanyLogin = (from c in userServices.List().Data.Where(x => x.Branch.CompanyId == companyId && x.RoleId == null).ToList()
                               select new CompanyLogin()
                               {
                                   UserType = Enum.GetName(typeof(UserType), c.UserType),
                                   FullName = c.FullName,
                                   Name = c.Name,
                                   Password = c.Password
                               }).ToList();

            vm.CompanyResellerInfo = (from c in resellerServices.List().Data.Where(x => x.Id == resellerId)
                                      join d in resellerServices.ListResellerLogin().Data on c.Id equals d.ResellerId
                                      select new CompanyResellerInfo()
                                      {
                                          Address = c.Address,
                                          ContactNo = c.ContactNo,
                                          ContactPerson = c.ContactPerson,
                                          Email = c.Email,
                                          Name = c.Name,
                                          Pan = c.PAN,
                                          Web = c.WebUrl,
                                          Password = d.User.Password,
                                          UserName = d.User.Name
                                      }).FirstOrDefault();

            return new ServiceResult<ClientInfoVm>()
            {
                Data = vm,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPut]
        public ServiceResult<ECompanyLicenseLog> Put(CompanyLicenseLog vm)
        {
            SCompany sCompany = new SCompany();
            var licenseLog = sCompany.ListCompanyLicenseLog().Data.Where(x => x.Id == vm.Id).FirstOrDefault();
            licenseLog.PaidAmount = vm.PaidAmount;
            licenseLog.Rate = vm.Rate;
            licenseLog.DueAmount = vm.DueAmount;
            licenseLog.PaidAmount = vm.PaidAmount;
            licenseLog.FileAttachment = vm.FileAttachment;
            licenseLog.Remark = vm.Remark;
            var result = sCompany.UpdateCompanyLicenseLog(licenseLog);
            return new ServiceResult<ECompanyLicenseLog>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = ResultStatus.Ok
            };
        }

        [HttpPut]
        public ServiceResult<ECompanyLicenseLog> Verify(CompanyLicenseLog vm)
        {
            SCompany sCompany = new SCompany();
            var licenseLog = sCompany.ListCompanyLicenseLog().Data.Where(x => x.Id == vm.Id).FirstOrDefault();
            licenseLog.IsPaid = true;
            var result = sCompany.UpdateCompanyLicenseLog(licenseLog);
            return new ServiceResult<ECompanyLicenseLog>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = ResultStatus.Ok
            };
        }
    }

    public class ClientGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string ResellerName { get; set; }
        public string SoftwarePackage { get; set; }
    }

    public class ClientInfoVm
    {
        public List<CompanyLicenseLog> CompanyLicenseLog { get; set; }
        public List<CompanyLogin> CompanyLogin { get; set; }
        public CompanyResellerInfo CompanyResellerInfo { get; set; }
    }

    public class CompanyLicenseLog
    {
        public int Id { get; set; }
        public string IsseDate { get; set; }
        public int LicensePeriod { get; set; }
        public bool IsPaid { get; set; }
        public decimal Rate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public string FileAttachment { get; set; }
        public string Remark { get; set; }
    }

    public class CompanyLogin
    {
        public string UserType { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class CompanyResellerInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string Pan { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class KendoResellerReportArg : KendoPageListArguments
    {
        public string SearchText { get; set; }
    }
}
