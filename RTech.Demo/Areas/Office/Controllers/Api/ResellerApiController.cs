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
    public class ResellerApiController : ApiController
    {
        SReseller resellerServices = null;
        SUser userServices = null;
        public ResellerApiController()
        {
            resellerServices = new SReseller();
            userServices = new SUser();
        }
        public ServiceResult<List<ResellerGridVm>> Get(string searchText = "")
        {
            var resellerLogin = resellerServices.ListResellerLogin().Data;
            List<ResellerGridVm> resultLst = new List<ResellerGridVm>();
            List<EReseller> resellerList = new List<EReseller>();
            if (searchText == null || searchText == "")
            {
                resellerList = resellerServices.List().Data.OrderBy(x => x.Name).ToList();
            }
            else
            {
                resellerList = resellerServices.List().Data.Where(x => x.Code.ToLower().Contains(searchText.ToLower()) || x.Name.ToLower().Contains(searchText.ToLower())).OrderBy(x => x.Name).ToList();
            }

            foreach (var item in resellerList)
            {
                ResellerGridVm vm = new ResellerGridVm();
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
                vm.Status = getResellerStatus(resellerLogin, item.Id);
                resultLst.Add(vm);
            }
            return new ServiceResult<List<ResellerGridVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<ResellerGridVm>> GetKendoGrid(KendoPageListArguments arg)
        {
            SReseller resellerServices = new SReseller();
            var resellerLogin = resellerServices.ListResellerLogin().Data;
            IQueryable<EReseller> resellerQuery;
            resellerQuery = resellerServices.List().Data;
            int totalRowNum = resellerQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EReseller> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                case "Address":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Address.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Address == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                case "ContactNo":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = resellerQuery.Where(x => x.ContactNo.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = resellerQuery.Where(x => x.ContactNo == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                case "ContactPerson":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = resellerQuery.Where(x => x.ContactPerson.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = resellerQuery.Where(x => x.ContactPerson == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                case "Email":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Email.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Email == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                case "WebUrl":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = resellerQuery.Where(x => x.WebUrl.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = resellerQuery.Where(x => x.WebUrl == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                default:
                    paginatedQuery = resellerQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    break;
            }
            var list = (from c in paginatedQuery.ToList()
                           select new ResellerGridVm()
                           {
                               Id = c.Id,
                               Code = c.Code,
                               Name = c.Name,
                               Address = c.Address,
                               ContactNo = c.ContactNo,
                               ContactPerson = c.ContactPerson,
                               Email = c.Email,
                               PAN = c.PAN,
                               WebUrl = c.WebUrl,
                               LogoUrl = c.LogoUrl,
                               Status = getResellerStatus(resellerLogin, c.Id),
                           }).ToList();
            return new KendoGridResult<List<ResellerGridVm>>()
            {
                Data = list.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = list.Count()
            };
        }

        private string getResellerStatus(IQueryable<EResellerLogin> resellerLogin, int resellerId)
        {
            bool IsSupended = false;
            var user = resellerLogin.Where(x => x.ResellerId == resellerId).FirstOrDefault();
            if (user.ActivationCode == null)
            {
                return "New";
            }
            else if (user.IsActivated == false)
            {
                return "In Activation";
            }
            else
            {
                IsSupended = user.User.IsSuspended; //userServices.List().Data.Where(x => x.Id == user.UserId).FirstOrDefault().IsSuspended;
                return IsSupended == true ? "Suspended" : "Approved";
            }

        }
        public ServiceResult<ResellerViewModel> Get(int id)
        {
            ResellerViewModel vm = new ResellerViewModel();
            vm.Reseller = resellerServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            int userId = userServices.FindResellerUser(id);
            EUser user = userServices.List().Data.Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                vm.UserName = user.Name;
                vm.Password = user.Password;
            }
            return new ServiceResult<ResellerViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public bool CheckDuplicateSNo(string code)
        {
            return resellerServices.List().Data.Where(x => x.Code == code).Any();
        }

        [HttpGet]
        public bool CheckDuplicateEmail(string email)
        {
            return resellerServices.List().Data.Where(x => x.Email == email).Any();
        }
        public ServiceResult<EReseller> Post([FromBody]ResellerViewModel vm)
        {
            ServiceResult<EReseller> resellerResult = new ServiceResult<EReseller>();
            CaptchaHelper captchaHelper = new CaptchaHelper();
            bool isDuplicateEmailAddress = CheckDuplicateEmail(vm.Reseller.Email);
            vm.Reseller.Code = Common.GetNewResellerCode().ToString();
            resellerResult = resellerServices.Add(vm.Reseller);
            if (vm.Reseller.Email != null && !isDuplicateEmailAddress)
            {
                vm.UserName = vm.Reseller.Email;
                vm.Password = createRandomNumber();
                EUser user = new EUser() { UserType = UserType.Reseller, Name = vm.UserName, Password = vm.Password, RoleId = null, BranchId = null };
                var userResult = userServices.Add(user);
                EResellerLogin resellerLogin = new EResellerLogin() { ResellerId = resellerResult.Data.Id, UserId = userResult.Data.Id };
                userServices.AddResellerLogin(resellerLogin);
            }
            if (resellerResult.Status == ResultStatus.Ok)
            {
                MailCommon mail = new MailCommon();
                string[] receivers = new string[] { "binodsapkota96@gmail.com", "support@barcodenepal.com" };
                string subject = vm.Reseller.Name + " from " + vm.Reseller.Address + " wants to become a partner";
                string message = vm.Reseller.Name + " apply the submission form of Become a Partner on " + DateTime.Now.ToString("yyyy/MM/dd") + Environment.NewLine + "Contact No: " + vm.Reseller.ContactNo + Environment.NewLine + "Contact Person:" + vm.Reseller.ContactPerson + Environment.NewLine + "Email: " + vm.Reseller.Email + " Please respond it soon.";
                for (int i = 0; i < receivers.Count(); i++)
                {
                    //mail.SendMail(receivers[i], subject, message);
                }

            }
            return resellerResult;
        }
        private string createRandomNumber()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
        public ServiceResult<ResellerViewModel> Put([FromBody]ResellerViewModel vm)
        {
            var resellerResult = resellerServices.Update(vm.Reseller);
            vm.Reseller = null;
            int userId = userServices.FindResellerUser(resellerResult.Data.Id);
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
                    EUser newUser = new EUser() { UserType = UserType.Reseller, Name = vm.UserName, Password = vm.Password, RoleId = null, BranchId = RiddhaSession.CurrentUser.BranchId };
                    var userResult = userServices.Add(newUser);
                    EResellerLogin resellerLogin = new EResellerLogin() { ResellerId = resellerResult.Data.Id, UserId = userResult.Data.Id };
                    userServices.AddResellerLogin(resellerLogin);
                }
            }
            return new ServiceResult<ResellerViewModel>()
            {
                Data = vm,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            ResellerViewModel vm = new ResellerViewModel();
            int userId = userServices.FindResellerUser(id);
            var user = userServices.List().Data.Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                return new ServiceResult<int>()
                {
                    Data = 0,
                    Message = "The user account of reseller is existed so it can not be deleted",
                    Status = ResultStatus.processError
                };
            }
            var reseller = resellerServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return resellerServices.Remove(reseller);
        }

        [HttpGet]
        public ServiceResult<string> Suspend(int id)
        {
            string msg = "";
            //int userId = userServices.FindResellerUser(id);
            string status = "";
            var resellerLogin = resellerServices.ListResellerLogin().Data.Where(x => x.ResellerId == id).FirstOrDefault();
            if (resellerLogin.IsActivated)
            {
                if (resellerLogin != null)
                {
                    var user = userServices.List().Data.Where(x => x.Id == resellerLogin.UserId).FirstOrDefault();
                    if (user.IsSuspended == true)
                    {
                        user.IsSuspended = false;
                        var result = userServices.Update(user);
                        if (result.Status == ResultStatus.Ok)
                        {
                            msg = "Approved Successfully";
                            status = "Approved";
                        }
                    }
                    else if (user.IsSuspended == false)
                    {
                        user.IsSuspended = true;
                        var result = userServices.Update(user);
                        if (result.Status == ResultStatus.Ok)
                        {
                            msg = "Suspended Successfully";
                            status = "Suspended";
                        }
                    }
                }
            }
            else
            {
                //send activation link to the user

                MailCommon mail = new MailCommon();
                var guId = Guid.NewGuid().ToString();
                resellerLogin.ActivationCode = guId;
                resellerServices.UpdateResellerLogin(resellerLogin);
                var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                var link = string.Format("{0}/activation/partner/{1}", baseUrl, guId);
                mail.SendMail(resellerLogin.Reseller.Email, "EHajiri Partner Activation Link.", link);
                msg = "Partners account is activated. Activation link has been sent via email.";
                status = "In Activation";
                //update reseller 
                //IsActivated True
            }
            return new ServiceResult<string>
            {
                Data = status,
                Status = ResultStatus.Ok,
                Message = msg
            };
        }
        [HttpGet]
        public ServiceResult<bool> ResetResellerPassword(string email)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            var resellerLogin = resellerServices.ListResellerLogin().Data.Where(x => x.Reseller.Email == email && x.IsActivated).FirstOrDefault();
            if (resellerLogin != null)
            {
                var user = userServices.List().Data.Where(x => x.Id == resellerLogin.UserId && x.IsSuspended == false && x.IsDeleted == false && x.UserType == UserType.Reseller).FirstOrDefault();
                if (user != null)
                {
                    MailCommon mail = new MailCommon();
                    var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                    var link = string.Format("{0}/activation/ResetPassword/{1}", baseUrl, resellerLogin.ActivationCode);
                    mail.SendMail(user.Name, "Hamro-Hajiri Password reset Link.", link);
                    result.Data = true;
                    result.Status = ResultStatus.Ok;
                    result.Message = "Reset Password link has been sent via email.";
                }
                else
                {
                    result.Data = false;
                    result.Status = ResultStatus.processError;
                    result.Message = "No User registration is found for this email address";
                }
            }
            else
            {
                result.Data = false;
                result.Status = ResultStatus.processError;
                result.Message = "No partner registration is found for this email address";
            }
            return result;
        }

        [HttpGet]
        public ServiceResult<bool> ResetUserPassword(string email)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            var resellerLogin = resellerServices.ListResellerLogin().Data.Where(x => x.Reseller.Email == email && x.IsActivated).FirstOrDefault();
            if (resellerLogin != null)
            {
                var user = userServices.List().Data.Where(x => x.Id == resellerLogin.UserId && x.IsSuspended == false && x.IsDeleted == false && x.UserType == UserType.Reseller).FirstOrDefault();
                if (user != null)
                {
                    MailCommon mail = new MailCommon();
                    var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                    var link = string.Format("{0}/activation/ResetPassword/{1}", baseUrl, resellerLogin.ActivationCode);
                    mail.SendMail(user.Name, "Hamro-Hajiri Password reset Link.", link);
                    result.Data = true;
                    result.Status = ResultStatus.Ok;
                    result.Message = "Reset Password link has been sent via email.";
                }
                else
                {
                    result.Data = false;
                    result.Status = ResultStatus.processError;
                    result.Message = "No User registration is found for this email address";
                }
            }
            else
            {
                result.Data = false;
                result.Status = ResultStatus.processError;
                result.Message = "No partner registration is found for this email address";
            }
            return result;
        }
    }

    public class ResellerViewModel
    {
        public EReseller Reseller { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }
    }

    public class ResellerGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string WebUrl { get; set; }
        public string PAN { get; set; }
        public string LogoUrl { get; set; }
        public string Status { get; set; }
        public string ContactPerson { get; set; }
    }
}
