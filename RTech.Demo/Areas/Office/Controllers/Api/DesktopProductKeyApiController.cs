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
    public class DesktopProductKeyApiController : ApiController
    {
        private int userId = RiddhaSession.UserId;
        private SCompany companyServices = null;
        private SUser userServices = null;
        private string secretKey = "HrmplDesk";
        public DesktopProductKeyApiController()
        {

        }
        public ServiceResult<List<CustomerDeviceVm>> GetCustomerDevice(int companyId)
        {
            var customerDevices = new SCompanyDeviceAssignment().GetCompanyDevices(companyId);
            var result = (from c in customerDevices.Data
                          select new CustomerDeviceVm()
                          {
                              DeviceSerialNo = c.Device.SerialNumber,
                              DeviceId = c.DeviceId,
                              Model = c.Device.Model.Name,
                          }).ToList();
            return new ServiceResult<List<CustomerDeviceVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<DesktopProductKeyCompanyGridVm>> GetKendoGrid(KendoPageListArguments arg)
        {
            companyServices = new SCompany();
            userServices = new SUser();
            var companyLogin = companyServices.ListCompanyLogin().Data;
            List<CompanyGridVm> resultLst = new List<CompanyGridVm>();
            List<ECompany> ResellerCompanyList = new List<ECompany>();
            EUser user = RiddhaSession.CurrentUser;
            IQueryable<ECompany> companyQuery;
            int resellerId = userServices.GetResellerLoginLst().Where(x => x.UserId == user.Id).FirstOrDefault().ResellerId;
            companyQuery = companyServices.List().Data.Where(x => x.ResellerId == resellerId && x.SoftwareType == SoftwareType.Desktop);
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
                          select new DesktopProductKeyCompanyGridVm()
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
                              DeviceCount = getCompanyDevices(c.Id),
                              Key = getKey(c.Id),
                          }).ToList();
            return new KendoGridResult<List<DesktopProductKeyCompanyGridVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = result.Count(),
            };

        }

        private string getKey(int id)
        {
            SDesktopProductKey desktopProductKeyServices = new SDesktopProductKey();
            var key = desktopProductKeyServices.List().Data.Where(x => x.CompanyId == id).FirstOrDefault();
            if (key != null)
            {
                return key.Key;
            }
            return "";
        }

        public ServiceResult<List<DesktopProductKeyVm>> GetProductKeyByCompanyId(int companyId)
        {
            SDesktopProductKey desktopProductKeyServices = new SDesktopProductKey();
            var data = desktopProductKeyServices.List().Data.Where(x => x.CompanyId == companyId).ToList();
            var result = (from c in data.ToList()
                          select new DesktopProductKeyVm()
                          {
                              DeviceCount = c.DeviceCount,
                              Id = c.Id,
                              IssueDate = c.IssueDate.ToString("yyyy/MM/dd"),
                              Key = c.Key,
                              MAC = c.MAC,
                              CompanyId = c.CompanyId,
                              SystemDate = c.SystemDate.ToString("yyyy/MM/dd"),
                          }).ToList();
            return new ServiceResult<List<DesktopProductKeyVm>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        private int getCompanyDevices(int id)
        {
            companyServices = new SCompany();
            return companyServices.GetCompanyDeviceCount(id);
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

        public ServiceResult<DesktopProductKeyPostVm> Post(DesktopProductKeyPostVm vm)
        {
            SDesktopProductKey desktopProductKeyServices = new SDesktopProductKey();
            DateTime expiryDate = DateTime.Now.AddYears(1);
            //string date = "2021/09/01";
            //DateTime a = date.ToDateTime();
            vm.IssueDate = DateTime.Now;
            if (vm.BackDate != DateTime.MinValue)
            {
                expiryDate = vm.BackDate;
                vm.IssueDate = vm.BackDate;
            }
            Lib.ProductKey.RiddhaKey key = new Lib.ProductKey.RiddhaKey(secretKey, expiryDate, vm.MAC);
            string productKey = key.getProductKey();

            vm.SystemDate = DateTime.Now;
            vm.Key = productKey;
            EDesktopProductKey model = new EDesktopProductKey()
            {
                CompanyId = vm.CompanyId,
                DeviceCount = vm.DeviceCount,
                IsPaid = vm.IsPaid,
                IssueDate = vm.IssueDate,
                Key = vm.Key,
                MAC = vm.MAC,
                SystemDate = vm.SystemDate
            };
            var result = desktopProductKeyServices.Add(model);
            return new ServiceResult<DesktopProductKeyPostVm>()
            {
                Data = vm,
                Message = result.Message,
                Status = result.Status
            };
        }

        [HttpGet]
        public ServiceResult<bool> SendKeyToMail(int productKeyId)
        {
            bool status = false;
            var key = new SDesktopProductKey().List().Data.Where(x => x.Id == productKeyId).FirstOrDefault();
            var company = new SCompany().List().Data.Where(x => x.Id == key.CompanyId).FirstOrDefault();
            if (company != null)
            {

                string subject = "Hamro-Hajiri Desktop Key for " + key.MAC;
                string message = key.Key;
                MailCommon mail = new MailCommon();
                mail.SendMail(company.Email, subject, message, ref status);
            }
            if (status)
            {
                return new ServiceResult<bool>()
                {
                    Data = true,
                    Message = "Key Sucessfully sent",
                    Status = ResultStatus.Ok,
                };
            }
            else
            {
                return new ServiceResult<bool>()
                {
                    Data = false,
                    Message = "Key Sent Failed. Please check ur email and try again.",
                    Status = ResultStatus.processError
                };
            }
        }
    }

    public class CustomerDeviceVm
    {
        public int DeviceId { get; set; }
        public string Model { get; set; }
        public string DeviceSerialNo { get; set; }
    }
    public class DesktopProductKeyVm
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string IssueDate { get; set; }
        public string SystemDate { get; set; }
        public int DeviceCount { get; set; }
        public string MAC { get; set; }
        public string Key { get; set; }


    }

    public class DesktopProductKeyPostVm
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime SystemDate { get; set; }
        public DateTime BackDate { get; set; }
        public int DeviceCount { get; set; }
        public string MAC { get; set; }
        public string Key { get; set; }
        public bool IsPaid { get; set; }
    }


    public class DesktopProductKeyCompanyGridVm
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
        public int DeviceCount { get; set; }
        public bool AllowDepartmentwiseAttendance { get; set; }
        public string Key { get; set; }

    }
}
