using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class PartnerReportApiController : ApiController
    {


        [HttpPost]
        public ServiceResult<List<ExpiredCompanyVm>> GenerateReport(string fromDate, string toDate)
        {
            SReseller resellerServices = new SReseller();
            var reseller = resellerServices.ListResellerLogin().Data.Where(x => x.UserId == RiddhaSession.UserId).FirstOrDefault();
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate);
            var result = new SMonthWiseExpiredCompanyReport().GetReport(startDate, endDate);
            if (reseller != null)
            {
                var filteredData = (from c in result.Data.Where(x => x.ResellerId == reseller.Id)
                                    select new ExpiredCompanyVm()
                                    {
                                        CompanyAddress = c.CompanyAddress,
                                        CompanyCode = c.CompanyCode,
                                        CompanyContactNo = c.CompanyContactNo,
                                        CompanyContactPerson = c.CompanyContactPerson,
                                        ExpiryDate = c.CompanyExpiryDate.ToString("yyyy/MM/dd"),
                                        CompanyName = c.CompanyName,
                                    }).ToList();
                return new ServiceResult<List<ExpiredCompanyVm>>()
                {
                    Data = filteredData,
                    Message = "",
                    Status = ResultStatus.Ok
                };
            }
            else
            {
                var data = (from c in result.Data
                            select new ExpiredCompanyVm()
                            {
                                CompanyAddress = c.CompanyAddress,
                                CompanyCode = c.CompanyCode,
                                CompanyContactNo = c.CompanyContactNo,
                                CompanyContactPerson = c.CompanyContactPerson,
                                CompanyExpiryDate = c.CompanyExpiryDate,
                                CompanyName = c.CompanyName,
                                ResellerAddress = c.ResellerAddress,
                                CompanyId = c.CompanyId,
                                ResellerContactNo = c.ResellerContactNo,
                                ResellerContactPerson = c.ResellerContactPerson,
                                ResellerId = c.ResellerId,
                                ResellerName = c.ResellerName,
                                ExpiryDate = c.CompanyExpiryDate.ToString("yyyy/MM/dd")
                            }).ToList();
                return new ServiceResult<List<ExpiredCompanyVm>>()
                {
                    Data = data,
                    Message = "",
                    Status = ResultStatus.Ok
                };
            }
        }

        [HttpPost]
        public ServiceResult<List<MonthWiseCustomerReportVm>> GetMonthWiseCustomerReport()
        {
            SReseller resellerServices = new SReseller();
            var reseller = resellerServices.ListResellerLogin().Data.Where(x => x.UserId == RiddhaSession.UserId).FirstOrDefault();
            SCompany companyServices = new SCompany();
            List<ECompany> companies = new List<ECompany>();
            if (reseller != null)
            {
                companies = companyServices.List().Data.Where(x => x.ResellerId == reseller.Id).ToList();
            }
            else
            {
                companies = companyServices.List().Data.ToList();
            }
            var companyLicense = companyServices.ListCompanyLicenseLog().Data;
            var filteredCompany = (from c in companies
                                   join d in companyLicense on c.Id equals d.CompanyId
                                   where d.IssueDate.Year == DateTime.Now.Year
                                   select new FilteredCustomerReportVm()
                                   {
                                       CompanyCode = c.Code,
                                       CompanyName = c.Name,
                                       CustomerCreationDate = d.IssueDate,
                                   }).GroupBy(l => l.CompanyCode).Select(g => g.OrderByDescending(c => c.CustomerCreationDate).FirstOrDefault()).ToList();

            List<MonthWiseCustomerReportVm> list = new List<MonthWiseCustomerReportVm>
                {
                    new MonthWiseCustomerReportVm{ Month = "January",MonthNo =1},
                    new MonthWiseCustomerReportVm{ Month = "February",MonthNo =2},
                    new MonthWiseCustomerReportVm{ Month = "March",MonthNo =3},
                    new MonthWiseCustomerReportVm{ Month = "April",MonthNo =4},
                    new MonthWiseCustomerReportVm{ Month = "May",MonthNo =5},
                    new MonthWiseCustomerReportVm{ Month = "June",MonthNo =6},
                    new MonthWiseCustomerReportVm{ Month = "July",MonthNo =7},
                    new MonthWiseCustomerReportVm{ Month = "August",MonthNo =8},
                    new MonthWiseCustomerReportVm{ Month = "September",MonthNo =9},
                    new MonthWiseCustomerReportVm{ Month = "October",MonthNo =10},
                    new MonthWiseCustomerReportVm{ Month = "November",MonthNo =11},
                    new MonthWiseCustomerReportVm{ Month = "December",MonthNo =12},
                };
            foreach (var item in filteredCompany)
            {
                string monthName = item.CustomerCreationDate.ToString("MMMM");
                var validateMonthBeforeAdd = list.Where(x => x.Month == monthName).FirstOrDefault();
                if (validateMonthBeforeAdd != null)
                {
                    validateMonthBeforeAdd.Count = validateMonthBeforeAdd.Count + 1;
                }
            }
            decimal preserveLastMonthCount = 0;
            foreach (var data in list)
            {
                if (data.Count == 0)
                {
                    data.Diffrence = 0;
                }
                else
                {
                    if (preserveLastMonthCount == 0)
                    {
                        preserveLastMonthCount = data.Count;
                    }

                    if (preserveLastMonthCount != 0)
                    {
                        data.Diffrence = data.Count - preserveLastMonthCount;
                    }
                    preserveLastMonthCount = data.Count;
                }
            }
            return new ServiceResult<List<MonthWiseCustomerReportVm>>()
            {
                Data = list,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<List<CustomerDesktopreportVm>> GenerateMonthWiseDesktopCustomerReport(string fromDate, string toDate)
        {
            SReseller resellerServices = new SReseller();
            var reseller = resellerServices.ListResellerLogin().Data.Where(x => x.UserId == RiddhaSession.UserId).FirstOrDefault();
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate);
            SDesktopProductKey desktopProductKeyServices = new SDesktopProductKey();
            List<EDesktopProductKey> desktopProductKeyList = new List<EDesktopProductKey>();
            if (reseller != null)
            {
                desktopProductKeyList = desktopProductKeyServices.List().Data.Where(x => x.Company.ResellerId == reseller.Id && (DbFunctions.TruncateTime(x.IssueDate)) >= (DbFunctions.TruncateTime(startDate)) && (DbFunctions.TruncateTime(x.IssueDate)) <= (DbFunctions.TruncateTime(endDate))).ToList();
            }
            else
            {
                desktopProductKeyList = desktopProductKeyServices.List().Data.Where(x => (DbFunctions.TruncateTime(x.IssueDate)) >= (DbFunctions.TruncateTime(startDate)) && (DbFunctions.TruncateTime(x.IssueDate)) <= (DbFunctions.TruncateTime(endDate))).ToList();
            }
            var result = (from c in desktopProductKeyList
                          select new CustomerDesktopreportVm()
                          {
                              CompanyAddress = c.Company.Address,
                              CompanyCode = c.Company.Code,
                              CompanyName = c.Company.Name,
                              IssueDate = c.IssueDate.ToString("yyyy/MM/dd"),
                              Key = c.Key,
                              MAC = c.MAC,
                              ResellerAddress = c.Company.Reseller.Address,
                              ResellerContactNo = c.Company.Reseller.ContactNo,
                              ResellerContactPerson = c.Company.Reseller.ContactPerson,
                              ResellerName = c.Company.Reseller.Name,
                              CompanyContactNo = c.Company.ContactNo,
                              CompanyContactPerson = c.Company.ContactPerson,
                          }).ToList();
            return new ServiceResult<List<CustomerDesktopreportVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

    public class FilteredCustomerReportVm
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public DateTime CustomerCreationDate { get; set; }
    }
    public class MonthWiseCustomerReportVm
    {
        public string Month { get; set; }
        public int MonthNo { get; set; }
        public int Count { get; set; }
        public decimal Diffrence { get; set; }
    }
    public class CustomerDesktopreportVm
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContactPerson { get; set; }
        public string CompanyContactNo { get; set; }
        public string IssueDate { get; set; }
        public string MAC { get; set; }
        public string Key { get; set; }
        public string ResellerName { get; set; }
        public string ResellerAddress { get; set; }
        public string ResellerContactPerson { get; set; }
        public string ResellerContactNo { get; set; }
    }
}
