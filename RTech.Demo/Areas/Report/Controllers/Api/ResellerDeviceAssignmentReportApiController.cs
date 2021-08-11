using Riddhasoft.Device.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class ResellerDeviceAssignmentReportApiController : ApiController
    {
        SDeviceAssignment _deviceAssignmentServices = null;
        SReseller _resellerServices = null;
        SBranch _branchServices = null;
        SUser _userServices = null;
        public ResellerDeviceAssignmentReportApiController()
        {
            _deviceAssignmentServices = new SDeviceAssignment();
            _resellerServices = new SReseller();
            _branchServices = new SBranch();
            _userServices = new SUser();
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetResellersForDropdown()
        {

            List<DropdownViewModel> resultLst = (from c in _resellerServices.List().Data.ToList()
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
        public KendoGridResult<List<ResellerDeviceAssignmentReportGridVm>> GenerateReport(KendoResellerDeviceAssignmentArguments args)
        {
            List<ResellerDeviceAssignmentReportGridVm> result = new List<ResellerDeviceAssignmentReportGridVm>();
            var deviceAssigneList = _deviceAssignmentServices.List().Data.ToList();
            int[] resellerIdsIds = { };
            if (args.ResellerIds != null)
            {
                resellerIdsIds = args.ResellerIds.Split(',').Select(x => int.Parse(x)).ToArray();
                var filtereddeviceAssigne = (from c in deviceAssigneList.ToList()
                                             join d in resellerIdsIds on c.ResellerId equals d
                                             select new ResellerDeviceAssignmentReportGridVm
                                             {
                                                 AssignedDate = c.AssignedOn.ToString("yyyy/MM/dd"),
                                                 DeviceModel = c.Device.Model.Name,
                                                 DeviceSerialNo = c.Device.SerialNumber,
                                                 ResellerAddress = c.Reseller.Address,
                                                 ResellerContact = c.Reseller.ContactNo,
                                                 ResellerName = c.Reseller.Name,
                                                 ContactPerson = c.Reseller.ContactPerson
                                             }).ToList();
                return new KendoGridResult<List<ResellerDeviceAssignmentReportGridVm>>()
                {
                    Data = filtereddeviceAssigne.Skip(args.Skip).Take(args.Take).ToList(),
                    Message = "",
                    Status = ResultStatus.Ok,
                    TotalCount = filtereddeviceAssigne.Count(),
                };
            }
            var allDeviceAssigne = (from c in deviceAssigneList.ToList()
                                    select new ResellerDeviceAssignmentReportGridVm()
                                    {
                                        AssignedDate = c.AssignedOn.ToString("yyyy/MM/dd"),
                                        DeviceModel = c.Device.Model.Name,
                                        DeviceSerialNo = c.Device.SerialNumber,
                                        ResellerAddress = c.Reseller.Address,
                                        ResellerContact = c.Reseller.ContactNo,
                                        ResellerName = c.Reseller.Name,
                                        ContactPerson = c.Reseller.ContactPerson
                                    }).ToList();
            return new KendoGridResult<List<ResellerDeviceAssignmentReportGridVm>>()
            {
                Data = allDeviceAssigne.Skip(args.Skip).Take(args.Take).ToList(),
                Message = "",
                Status = ResultStatus.Ok,

            };
        }

        [HttpPost]
        public KendoGridResult<List<CompanyStatusVm>> GenerateCompanyStatsuReport(KendoCompanyStatusArgument arg)
        {
            List<ECompany> companies = new List<ECompany>();
            List<ECompany> filteredCompanies = new List<ECompany>();
            List<CompanyStatusVm> list = new List<CompanyStatusVm>();
            SReseller resellerServices = new SReseller();
            SCompany companyServices = new SCompany();
            SContext contextServices = new SContext();
            int userId = RiddhaSession.UserId;
            var reseller = resellerServices.ListResellerLogin().Data.Where(x => x.UserId == userId).FirstOrDefault();
            var contextList = contextServices.List().Data;
            DateTime today = DateTime.Now.AddDays(-60);
            if (RiddhaSession.UserType == UserType.Reseller.ToInt())
            {
                if (reseller != null)
                {
                    companies = companyServices.List().Data.Where(x => x.ResellerId == reseller.ResellerId).ToList();
                    list = (from c in companies.ToList()
                            select new CompanyStatusVm()
                            {
                                Code = c.Code,
                                Address = c.Address,
                                ContactNo = c.ContactNo,
                                ContactPerson = c.ContactPerson,
                                LastLogin = getLastLogin(contextList, c.Id),
                                Name = c.Name,
                                ResellerContact = c.Reseller.ContactNo,
                                ResellerName = c.Reseller.Name,
                            }).ToList();
                    return new KendoGridResult<List<CompanyStatusVm>>()
                    {
                        Data = list,
                        Message = "",
                        Status = ResultStatus.Ok,
                        TotalCount = list.Count(),
                    };
                }
            }
            if (arg.Active)
            {
                var allInactiveCompanies = companyServices.List().Data.ToList();
                list = (from c in allInactiveCompanies
                        select new CompanyStatusVm()
                        {
                            Name = c.Name,
                            Code = c.Code,
                            Address = c.Address,
                            ContactNo = c.ContactNo,
                            ContactPerson = c.ContactPerson,
                            LastLogin = getLastLogin(contextList, c.Id),
                            ResellerContact = c.Reseller.ContactNo,
                            ResellerName = c.Reseller.Name,
                        }).ToList();
                return new KendoGridResult<List<CompanyStatusVm>>()
                {
                    Data = list,
                    Message = "",
                    Status = ResultStatus.Ok,
                    TotalCount = list.Count(),
                };
            }
            var allCompanies = companyServices.List().Data.ToList();
            foreach (var item in allCompanies)
            {
                int branchId = _branchServices.List().Data.Where(x => x.CompanyId == item.Id).FirstOrDefault().Id;
                var companyUser = _userServices.List().Data.Where(x => x.BranchId == branchId && x.RoleId == null).FirstOrDefault();
                if (companyUser != null)
                {
                    var context = contextServices.List().Data.Where(x => x.UserId == companyUser.Id).ToList().OrderByDescending(x => x.Id).FirstOrDefault();
                    if (context != null)
                    {
                        if (context.LastLogin < today)
                        {
                            filteredCompanies.Add(item);
                        }
                    }
                }
            }
            list = (from c in allCompanies
                    select new CompanyStatusVm()
                    {
                        Name = c.Name,
                        Code = c.Code,
                        Address = c.Address,
                        ContactNo = c.ContactNo,
                        ContactPerson = c.ContactPerson,
                        LastLogin = getLastLogin(contextList, c.Id),
                        ResellerContact = c.Reseller.ContactNo,
                        ResellerName = c.Reseller.Name,
                    }).ToList();
            return new KendoGridResult<List<CompanyStatusVm>>()
            {
                Data = list,
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = list.Count(),
            };

        }

        private string getLastLogin(IQueryable<EContext> contextList, int id)
        {
            var branch = _branchServices.List().Data.Where(x => x.CompanyId == id).FirstOrDefault();
            int userId = _userServices.List().Data.Where(x => x.BranchId == branch.Id && x.RoleId != null).FirstOrDefault().Id;
            var context = contextList.Where(x => x.UserId == userId).FirstOrDefault();
            if (context != null)
            {
                return context.LastLogin.ToString("yyyy/MM/dd");
            }
            return "";
        }

        [HttpPost]
        public KendoGridResult<List<CompanyLicenseRenewReportVm>> GetMonthWiseCustomerRenewReport(MonthWiseCustomerReportArg arg)
        {
            var data = new SCompany().ListCompanyLicenseLog().Data.Where(x => DbFunctions.TruncateTime(x.IssueDate) >= arg.OnDate && DbFunctions.TruncateTime(x.IssueDate) <= arg.EndDate);
            var result = (from c in data.ToList()
                          select new CompanyLicenseRenewReportVm
                          {
                              CompanyCode = c.Company.Code,
                              CompanyName = c.Company.Name,
                              CompanyAddress = c.Company.Address,
                              CompanyContact = c.Company.ContactNo,
                              CompanyContactPerson = c.Company.ContactPerson,
                              ResellerName = c.Company.Reseller.Name,
                              ResellerContact = c.Company.Reseller.ContactNo,
                              ResellerContactPerson = c.Company.Reseller.ContactPerson,
                              RenewDate = c.IssueDate.ToShortDateString()
                          }).ToList();
            return new KendoGridResult<List<CompanyLicenseRenewReportVm>>()
            {
                Data = result.Skip(arg.Skip).Take(arg.Take).ToList(),
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = result.Count(),
            };
        }
    }

    public class ResellerDeviceAssignmentReportGridVm
    {
        public string ResellerName { get; set; }
        public string ResellerAddress { get; set; }
        public string ResellerContact { get; set; }
        public string ContactPerson { get; set; }
        public string DeviceSerialNo { get; set; }
        public string DeviceModel { get; set; }
        public string AssignedDate { get; set; }
    }
    public class KendoResellerDeviceAssignmentArguments : KendoPageListArguments
    {
        public string ResellerIds { get; set; }
        public string OnDate { get; set; }
    }
    public class CompanyStatusVm
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string LastLogin { get; set; }
        public string ResellerName { get; set; }
        public string ResellerContact { get; set; }
    }
    public class KendoCompanyStatusArgument : KendoPageListArguments
    {
        public bool Active { get; set; }
    }

    public class CompanyLicenseRenewReportVm
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyContactPerson { get; set; }
        public string ResellerName { get; set; }
        public string ResellerContact { get; set; }
        public string ResellerContactPerson { get; set; }
        public string RenewDate { get; set; }
    }

    public class MonthWiseCustomerReportArg : KendoPageListArguments
    {
        public DateTime OnDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
