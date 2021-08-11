using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Areas.Office.Controllers.Api;
using RTech.Demo.Utilities;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace RTech.Demo.Areas.User.Controllers
{
    public class UserController : Controller
    {
        EUser userData = new EUser();
        SUser userServices = new SUser();
        SEmployee employeeServices = new SEmployee();
        SUserRole roleServices = new SUserRole();
        SSessionDetail sessionServices = new SSessionDetail();
        SContext contextServices = new SContext();
        SCompany _companyServices = null;
        SReseller resellerServices = null;
        public UserController()
        {
            _companyServices = new SCompany();
        }
        //
        // GET: /User/User/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "";
            ViewBag.LoginAs = 2;
            ViewBag.IpAddress = GetIPAddress();
            ViewBag.CompanyCode = "";


            return PartialView("_PortalPage");
        }
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        [HttpPost]
        public ActionResult Login(UserViewModel user, string UserLogin, string OwnerLogin, string ResellerLogin)
        {
            //CaptchaHelper captchaHelper = new CaptchaHelper();
            if (RiddhaSession.OperationDate == "" || RiddhaSession.Language == "")
            {
                RiddhaSession.Language = "en";
                RiddhaSession.OperationDate = "en";
            }

            RiddhaSession.CurDate = DateTime.Now.ToString("yyyy/MM/dd");
            if (OwnerLogin != null)
            {
                userData = userServices.List().Data.Where(x => x.Name.ToUpper() == user.UserName.ToUpper() && x.Password == user.Password && x.IsDeleted == false && (x.UserType == UserType.Admin || x.UserType == UserType.Owner)).FirstOrDefault();
                if (IsFirstLogin())
                {
                    userServices.Add(new EUser() { Id = 0, Name = "Admin", Password = "Admin", RoleId = null, UserType = UserType.Admin });
                    return PartialView("_OwnerLogin");
                }
                if (userData != null)
                {
                    var context = UpdateSession(userData);
                    RiddhaSession.CurrentToken = context.Token;
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    ViewBag.IpAddress = GetIPAddress();
                    ViewBag.LoginAs = UserType.Admin;
                    ViewBag.Message = "Invalid User Name or Password";
                    return PartialView("_OwnerLogin");
                }
            }
            else if (ResellerLogin != null)
            {

                //if (captchaHelper.Verify(user.Captcha))
                //{
                    userData = userServices.List().Data.Where(x => x.Name.Equals(user.UserName, StringComparison.Ordinal) && x.Password.Equals(user.Password, StringComparison.Ordinal) && x.IsDeleted == false && x.UserType == UserType.Reseller && x.IsSuspended == false).FirstOrDefault();
                    if (userData != null && userData.Password == user.Password)
                    {
                        var context = UpdateSession(userData);
                        RiddhaSession.CurrentToken = context.Token;
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    else
                    {
                        ViewBag.IpAddress = GetIPAddress();
                        ViewBag.LoginAs = UserType.Reseller;
                        ViewBag.Message = "Invalid User Name or Password";
                        return PartialView("_PortalPage");
                    }
                //}
                //else
                //{
                //    ViewBag.IpAddress = GetIPAddress();
                //    ViewBag.LoginAs = UserType.Reseller;
                //    ViewBag.Message = "Invalid Captcha";
                //    return PartialView("_PortalPage");
                //}
            }
            else if (UserLogin != null)
            {
                string ipAddress = GetIPAddress();
                ViewBag.CompanyCode = user.CompanyCode;
                //if (captchaHelper.Verify(user.Captcha))
                //{
                    userData = userServices.List().Data.Where(x => x.Name == user.UserName && x.Password == user.Password && x.UserType == UserType.User && x.Branch.Code == user.CompanyCode && x.IsDeleted == false).FirstOrDefault();
                    if (userData != null)
                    {
                        var employee = employeeServices.List().Data.Where(x => x.UserId == userData.Id).FirstOrDefault();
                        if (RiddhaSession.PackageId == 3)
                        {
                            if (userData.RoleId != null)
                            {
                                if (employee != null)
                                {
                                    if (employee.EmploymentStatus == EmploymentStatus.ContractPeriodOver
                                        || employee.EmploymentStatus == EmploymentStatus.Deceased
                                        || employee.EmploymentStatus == EmploymentStatus.Defaulter
                                        || employee.EmploymentStatus == EmploymentStatus.EarlyRetirement
                                        || employee.EmploymentStatus == EmploymentStatus.NormalRetirement
                                        || employee.EmploymentStatus == EmploymentStatus.Resigned
                                        || employee.EmploymentStatus == EmploymentStatus.Retiring
                                        || employee.EmploymentStatus == EmploymentStatus.Terminated
                                        )
                                    {
                                        ViewBag.IpAddress = ipAddress;
                                        ViewBag.LoginAs = UserType.User;
                                        ViewBag.Message = "Invalid Login. Please Try Again.";
                                        return PartialView("_PortalPage");
                                    }
                                }
                            }
                        }
                        if (userData.Branch.Code.ToUpper() != user.CompanyCode.ToUpper())
                        {
                            ViewBag.IpAddress = ipAddress;
                            ViewBag.LoginAs = UserType.User;
                            ViewBag.Message = "Invalid Branch Code";
                            ViewBag.CompanyCode = "";
                            return PartialView("_PortalPage");
                        }
                        if (userData.Branch.Company.IsSuspended == true)
                        {
                            ViewBag.IpAddress = ipAddress;
                            ViewBag.LoginAs = UserType.User;
                            ViewBag.Message = "Company Account is suspended please contact to vendor.";
                            return PartialView("_PortalPage");
                        }
                        else
                        {
                            var companyLicense = _companyServices.ListCompanyLicense().Data.Where(x => x.CompanyId == userData.Branch.CompanyId).FirstOrDefault();

                            //var companyLicenseLog = _companyServices.ListCompanyLicenseLog().Data.Where(x => x.CompanyId == companyLicense.CompanyId && x.IsPaid == false).OrderByDescending(x => x.IssueDate).ToList();
                            //int year =ConfigurationManager.AppSettings["year"].ToInt();
                            //foreach (var item in companyLicenseLog)
                            //{
                            //    if (item.IssueDate.Year < year && item.IsPaid == false)
                            //    {
                            //        ViewBag.Message = "your license payment is due. Please contact your vendor.";
                            //        return PartialView("_PortalPage");
                            //    }
                            //}
                            if (companyLicense == null)
                            {
                                ViewBag.Message = "License is not issued. Please contact you'r admin.";
                                return PartialView("_PortalPage");
                            }
                            else if (companyLicense.ExpiryDate.Date < DateTime.Now.Date)
                            {
                                ViewBag.Message = "The Company license is expired.Please contact you'r vendor.";
                                return PartialView("_PortalPage");
                            }
                            else
                            {
                                var context = UpdateSession(userData);
                                RiddhaSession.CurrentToken = context.Token;
                                return RedirectToAction("Index", "Home", new { area = "" });
                            }
                        }
                    }
                    else
                    {
                        ViewBag.IpAddress = ipAddress;
                        ViewBag.LoginAs = UserType.User;
                        ViewBag.Message = "Invalid Login. Please Try Again.";
                    }
                //}
                //else
                //{
                //    ViewBag.IpAddress = ipAddress;
                //    ViewBag.LoginAs = UserType.User;
                //    ViewBag.Message = "Invalid Captcha";
                //    return PartialView("_PortalPage");
                //}
            }
            return PartialView("_PortalPage");
        }

        [HttpPost]
        public JsonResult ResellerLogin(UserViewModel user)
        {
            userData = userServices.List().Data.Where(x => x.Name.Equals(user.UserName, StringComparison.Ordinal) && x.Password.Equals(user.Password, StringComparison.Ordinal) && x.UserType == UserType.Reseller && x.IsSuspended == false).FirstOrDefault();
            EContext context = new EContext();
            if (userData != null && userData.Password == user.Password)
            {
                context = UpdateSession(userData);
                string result = "Login Sucessfully";
                return Json(result);
            }
            else
            {
                string result = "Invalid Login";
                return Json(result);
            }
        }

        [HttpPost]
        public JsonResult BecomeAPartner(EReseller model)
        {
            string message = "";
            resellerServices = new SReseller();
            userServices = new SUser();
            if (string.IsNullOrEmpty(model.Name))
            {
                message = "Name is requierd";
                return Json(message);
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                message = "Address is requierd";
                return Json(message);
            }
            if (string.IsNullOrEmpty(model.ContactNo))
            {
                message = "Contact Number is requierd";
                return Json(message);
            }
            if (string.IsNullOrEmpty(model.ContactPerson))
            {
                message = "Contact Person is requierd";
                return Json(message);
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                message = "Email is requierd";
                return Json(message);
            }
            bool isDuplicateEmailAddress = CheckDuplicateEmail(model.Email);
            if (isDuplicateEmailAddress)
            {
                message = "Email address is already exist.";
                return Json(message);
            }
            model.Code = Common.GetNewResellerCode().ToString();
            var result = resellerServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                if (model.Email != null && !isDuplicateEmailAddress)
                {
                    string username = model.Email;
                    string password = createRandomNumber();
                    EUser user = new EUser() { UserType = UserType.Reseller, Name = username, Password = password, RoleId = null, BranchId = null };
                    var userResult = userServices.Add(user);
                    EResellerLogin resellerLogin = new EResellerLogin() { ResellerId = result.Data.Id, UserId = userResult.Data.Id };
                    userServices.AddResellerLogin(resellerLogin);
                }
                message = "Added Successfully.";
                return Json(message);
            }
            string error = "Process Error.";
            return Json(error);
        }

        [HttpGet]
        public bool CheckDuplicateEmail(string email)
        {
            return resellerServices.List().Data.Where(x => x.Email == email).Any();
        }
        private string createRandomNumber()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }

        private EContext UpdateSession(EUser User)
        {
            var newContext = new EContext() { Id = 0, LastLogin = DateTime.Now, TimeOut = TimeSpan.FromMinutes(20), UserId = User.Id, Token = getToken() };
            var contextResult = contextServices.Add(newContext);
            sessionServices.Add(new ESessionDetail() { Id = 0, Key = "User", Value = User.Id.ToString(), ContextId = contextResult.Data.Id });
            return newContext;
        }
        private string getToken()
        {
            return Guid.NewGuid().ToString();
        }
        public bool IsFirstLogin()
        {
            return (userServices.List().Data.Count() == 0 && roleServices.List().Data.Count() == 0);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            var curuser = RiddhaSession.CurrentUser;

            if ((UserType)RiddhaSession.UserType == UserType.Admin || (UserType)RiddhaSession.UserType == UserType.Owner)
            {
                RiddhaSession.Logout();
                ViewBag.Message = "";
                return RedirectToAction("owner", "User", new { area = "User" });
            }
            RiddhaSession.Logout();
            ViewBag.Message = "";
            return RedirectToAction("login", "User", new { area = "User" });
        }
        //[HttpGet]
        //public ActionResult ChangePassword()
        //{
        //    ChangePasswordViewModel vm = new ChangePasswordViewModel();
        //    var user = RiddhaSession.CurrentUser;
        //    vm.UserName = user.Name;
        //    vm.Password = user.Password;
        //    var result = new ServiceResult<ChangePasswordViewModel>() { Data = vm,Status = ResultStatus.Ok};
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //    //return View(vm);
        //}

        //[HttpPost]
        //public ActionResult ChangePassword(ChangePasswordViewModel vm)
        //{
        //    ServiceResult<int> result = new ServiceResult<int>();
        //    EUser user = userServices.List().Data.Where(x => x.Id == RiddhaSession.CurrentUser.Id).FirstOrDefault();
        //    if (vm.NewPassword != vm.ConfirmPassword)
        //    {
        //        result = new ServiceResult<int>() { Data = 1, Message = "Password confirmation do not match", Status = ResultStatus.processError };
        //    }
        //    else if (user.Name == vm.UserName && user.Password == vm.CurrentPassword)
        //    {
        //        user.Password = vm.NewPassword;
        //        userServices.Update(user);
        //        result = new ServiceResult<int>() { Data = 1, Message = "Changed Successfully", Status = ResultStatus.Ok };
        //    }
        //    else
        //    {
        //        result = new ServiceResult<int>() { Data = 0, Message = "Invalid User or Password", Status = ResultStatus.processError };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    if (user.Password == vm.NewPassword && user.Password == vm.ConfirmPassword)
        //    {
        //        user.Password = vm.NewPassword;
        //        user.Password = vm.ConfirmPassword;
        //    }
        //    else
        //    {
        //        result = new ServiceResult<int>() { Data = 0, Message = "Password do not match!!!", Status = ResultStatus.processError };
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}
        private void SetCulture(string culture)
        {
            string language = CultureHelper.GetImplementedCulture(culture);
            CultureInfo ci = CultureInfo.GetCultureInfo(language);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
        public ActionResult Reseller()
        {
            ViewBag.Message = "";
            return PartialView("_ResellerLogin");
        }
        public ActionResult Owner()
        {
            ViewBag.Message = "";
            return PartialView("_OwnerLogin");
        }
        public ActionResult Captcha()
        {
            CaptchaHelper captchaHelper = new CaptchaHelper();
            return File(captchaHelper.DrawByte(), "image/jpeg");
        }
        [HttpGet]
        public ActionResult LockScreen()
        {
            RiddhaSession.CurrentToken = "";
            var user = new SUser().List().Data.Where(x => x.Id == RiddhaSession.UserId).FirstOrDefault() ?? new EUser();
            LockScreenViewModel vm = new LockScreenViewModel()
            {
                img = user.PhotoURL,
                UserName = user.Name,
                FullName = user.FullName
            };
            return View("_LockScreen", vm);
        }
        [HttpPost]
        public ActionResult LockScreen(int UserId, string Password)
        {
            var user = userServices.List().Data.Where(x => x.Id == UserId).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == Password)
                {
                    var requestedUrl = RiddhaSession.RequestUrl;

                    var context = UpdateSession(user);
                    RiddhaSession.CurrentToken = context.Token;
                    var url = requestedUrl.Split('/');
                    if (url.Length >= 3)
                    {
                        string area = url[1];
                        string controller = url[2];
                        string action = "Index";

                        if (url.Count() == 4)
                        {
                            action = url[3];
                        }
                        return RedirectToAction(action, controller, new { area = area });
                    }
                    else
                    {
                        return RedirectToAction("index", "home", new { area = "" });
                    }
                }
            }
            return RedirectToAction("LockScreen");
        }
        public class UserViewModel
        {
            public UserType UserType { get; set; }
            public string CompanyCode { get; set; }
            public string Language { get; set; }
            public string IpAddress { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Captcha { get; set; }
        }
        public class ChangePasswordViewModel
        {
            public string UserName { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
            public string Password { get; set; }
        }
    }
    public class LockScreenViewModel
    {
        /* preserve*/
        #region for preserve
        public int UserId { get { return RiddhaSession.UserId; } }
        public string RequestUrl { get { return RiddhaSession.RequestUrl; } }

        #endregion


        public string img { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}