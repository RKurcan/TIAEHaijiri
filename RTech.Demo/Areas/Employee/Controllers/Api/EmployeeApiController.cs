using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Areas.HRM.Controllers.Api;
using RTech.Demo.Areas.Office.Controllers.Api;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using static RTech.Demo.Utilities.WDMS;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class EmployeeApiController : ApiController
    {

        SEmployee employeeServices = null;
        SUser userServices = null;
        SEmployeeLogin empLoginServices = null;
        SUserRole userRoleServices = null;
        LocalizedString loc = null;
        public EmployeeApiController()
        {
            employeeServices = new SEmployee();
            loc = new LocalizedString();
            empLoginServices = new SEmployeeLogin();
            userServices = new SUser();
            userRoleServices = new SUserRole();
        }
        [ActionFilter("2036")]
        public ServiceResult<List<EmployeeGridVm>> Get(int pageSize = 10, int page = 1, string searchText = "")
        {
            return new ServiceResult<List<EmployeeGridVm>>()
            {
                Data = getEmployeList(pageSize, page, searchText),
                Status = ResultStatus.Ok
            };
        }
        private List<EmployeeGridVm> getEmployeList(int pageSize = 10, int page = 1, string searchText = "")
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            IQueryable<EUser> userQuery = userServices.List().Data;
            SEmployee service = new SEmployee();
            IQueryable<EEmployee> empQuery = service.List().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = empQuery.Count();
            IQueryable<EEmployee> paginatedQuery = empQuery.Where(x => x.Name.Contains(searchText)).OrderBy(x => x.Name).Skip(pageSize * (page - 1)).Take(pageSize);
            var employeelist = (from c in paginatedQuery
                                select new EmployeeGridVm()
                                {
                                    Id = c.Id,
                                    DateOfJoin = c.DateOfJoin,
                                    DepartmentName = c.Section == null ? "" : language == "ne" ? c.Section.Department.NameNp : c.Section.Department.Name,
                                    EmployeeName = c.Name,
                                    IdCardNo = c.Code,
                                    Mobile = c.Mobile,
                                    Email = c.Email,
                                    NameNp = c.NameNp,
                                    PhotoURL = c.ImageUrl,
                                    SectionId = c.SectionId,
                                    DepartmentId = c.Section.DepartmentId,
                                    DesignationName = c.Designation == null ? "" : language == "ne" ? c.Designation.NameNp : c.Designation.Name,
                                    GradeGroupId = c.GradeGroupId,
                                    GradeGroupName = c.GradeGroup.Name,
                                    UserId = c.UserId,
                                    UserName = c.User.Name,
                                    Password = c.User.Password,
                                    RoleId = c.User.RoleId,
                                    TotalCount = totalRowNum,
                                    ReportingManagerId = (int)c.ReportingManagerId,
                                    BankId = c.BankId,
                                    BankAccountNo = c.BankAccountNo,
                                }).ToList();
            employeelist.ForEach(x => x.Status = getLoginStatus(userQuery, x.UserId) == true ? "Off" : "On");
            return employeelist;
        }
        [HttpGet, ActionFilter("2013")]
        public ServiceResult<List<EmployeeGridVm>> PullDataFromWdms()
        {
            #region pull data from wdms

            string language = RiddhaSession.Language.ToString();
            var branch = RiddhaSession.CurrentUser.Branch;
            var branchId = branch.Id;
            WdmsData.WdmsEntities wdmsdb = new WdmsData.WdmsEntities();
            var wdmsUserInfo = (from c in wdmsdb.userinfo.Where(x => x.company_id == branch.CompanyId)
                                select new
                                {
                                    badgenumber = c.badgenumber,
                                    name = c.name
                                }
                                    ).ToList();
            SEmployee service = new SEmployee();
            SSection sectionService = new SSection();
            var sectionId = getSectionId(sectionService.List().Data.Where(x => x.BranchId == branchId).FirstOrDefault());
            foreach (var item in wdmsUserInfo)
            {
                //filter existing user
                var deviceCode = item.badgenumber.ToInt();//length is 9 in wdms badgenumber
                var employee = service.List().Data.Where(x => x.BranchId == branchId && x.DeviceCode == deviceCode).FirstOrDefault();
                var defaultTime = "00:00".ToTimeSpan();
                if (employee == null)
                {

                    service.Add(new EEmployee()
                    {
                        BranchId = branchId,
                        DeviceCode = deviceCode,
                        Code = deviceCode.ToString(),
                        Name = string.IsNullOrEmpty(item.name.Trim()) ? "Sync From Device" : item.name,

                        MaxWorkingHour = defaultTime,
                        AllowedLateIn = defaultTime,

                        AllowedEarlyOut = defaultTime,
                        HalfdayWorkingHour = defaultTime,
                        ShortDayWorkingHour = defaultTime,
                        PresentMarkingDuration = defaultTime,


                        TwoPunch = true,
                        ShiftTypeId = 0,
                        WOTypeId = 0,

                        IsManager = false,
                        CitizenNo = "",
                        ConsiderTimeLoss = false,
                        DateOfBirth = null,
                        DateOfJoin = null,
                        DesignationId = null,
                        Email = "",
                        FourPunch = false,
                        Gender = Gender.Others,
                        GradeGroupId = null,
                        HalfDayMarking = false,

                        ImageUrl = "",
                        IssueDate = null,
                        IssueDistict = "",
                        MaritialStatus = MaritialStatus.NotSpecified,
                        Mobile = "",
                        MultiplePunch = false,
                        NameNp = "",
                        NoPunch = false,
                        PassportNo = "",
                        PermanentAddress = "",
                        PermanentAddressNp = "",

                        Religion = Religious.Hinduism,
                        SectionId = sectionId,
                        SinglePunch = false,
                        TemporaryAddress = "",
                        TemporaryAddressNp = "",
                        UserId = null,


                    });
                }
                else if (employee.Name == "Sync From Device" && string.IsNullOrEmpty(item.name.Trim()) == false)
                {
                    employee.SectionId = sectionId;
                    Common.AddAuditTrail("2001", "2013", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 0, "Pull Device Employee");
                }
                //save new one
            }
            //
            //wdmsdb.Database.ExecuteSqlCommand();
            #endregion
            return new ServiceResult<List<EmployeeGridVm>>()
            {
                Data = getEmployeList(),
                Status = ResultStatus.Ok
            };
        }

        private int? getSectionId(ESection section)
        {
            if (section == null)
            {
                return null;
            }
            else
            {
                return section.Id;
            }
        }
        private bool getLoginStatus(IQueryable<EUser> userQuery, int? userId)
        {
            if (userId == null)
            {
                return true;
            }
            else
            {
                var user = userQuery.Where(x => x.Id == userId).Single();
                return user.IsSuspended;
            }
        }
        public ServiceResult<EmployeeVm> Get(int id)
        {
            return employeeServices.GetEmployeVm(id);
        }

        [HttpGet, ActionFilter("2012")]
        public ServiceResult<EmployeeInfoVm> GetEmpInfo(int id)
        {
            string lang = RiddhaSession.Language.ToString();
            EmployeeInfoVm vm = new EmployeeInfoVm();
            EEmployee employee = employeeServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            EEmployeeShitList empshift = employeeServices.ListEmpShift().Where(x => x.EmployeeId == id).FirstOrDefault();
            vm.Id = employee.Id;
            vm.Photo = employee.ImageUrl;
            vm.IdCardNo = employee.Code;
            vm.EmployeeName = lang == "ne" && employee.NameNp != null ? employee.NameNp : employee.Name;
            vm.BranchName = lang == "ne" && employee.Branch.NameNp != null ? employee.Branch.NameNp : employee.Branch.Name;
            vm.DepartmentName = lang == "ne" && employee.Section.Department.NameNp != null ? employee.Section.Department.NameNp : employee.Section == null ? "" : employee.Section.Department.Name;
            vm.SectionName = lang == "ne" && employee.Section.NameNp != null ? employee.Section.NameNp : employee.Section.Name;
            vm.DesignationName = lang == "ne" && employee.Designation.NameNp != null ? employee.Designation.NameNp : employee.Designation.Name;
            vm.DateOfBirth = employee.DateOfBirth;
            vm.DateOfJoin = employee.DateOfJoin;
            vm.Mobile = employee.Mobile;
            vm.Address = employee.PermanentAddress;
            vm.BloodGroup = (int)employee.BloodGroup;
            vm.MaxWorkingHours = employee.MaxWorkingHour.ToString();
            vm.ShiftType = employee.ShiftTypeId;
            vm.ShiftName = employee.ShiftTypeId == 0 ? ((lang == "ne" && empshift.Shift.NameNp != null) ? empshift.Shift.NameNp : empshift.Shift.ShiftName) : "Dynamic Shift";
            vm.PunchType = getPunchType(employee);
            vm.BankId = employee.BankId;
            vm.BankAccountNo = employee.BankAccountNo;

            return new ServiceResult<EmployeeInfoVm>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }

        private string getPunchType(EEmployee employee)
        {
            string result = "";
            if (employee.SinglePunch)
            {
                result = loc.Localize("SinglePunch");
            }
            else if (employee.TwoPunch)
            {
                result = loc.Localize("TwoPunch");
            }
            else if (employee.FourPunch)
            {
                result = loc.Localize("FourPunch");
            }
            else if (employee.MultiplePunch)
            {
                result = loc.Localize("MultiplePunch");
            }
            else
            {
                result = loc.Localize("NoPunch");
            }
            return result;
        }

        [HttpGet]
        public ServiceResult<EmpSearchViewModel> GetEmp(int id)
        {
            EmpSearchViewModel vm = new EmpSearchViewModel();
            EEmployee employee = employeeServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            vm.Id = employee.Id;
            vm.Code = employee.Code;
            vm.Name = employee.Name;
            vm.Designation = employee.Designation.Name;
            vm.Department = employee.Section.Department.Name;
            vm.Section = employee.Section.Name;
            vm.Photo = employee.ImageUrl;
            return new ServiceResult<EmpSearchViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        //public ServiceResult<List<EEmployee>> GetEmployeeBySection(string id)
        //{
        //    string[] sections = id.Split(',');
        //    List<EEmployee> employee = (from c in employeeServices.List().Data.ToList()
        //                                where c.BranchId == RiddhaSession.CurrentUser.BranchId
        //                                join d in sections on c.SectionId equals int.Parse(d)
        //                                select c).ToList();
        //    return new ServiceResult<List<EEmployee>>()
        //    {
        //        Data = employee,
        //        Status = ResultStatus.Ok
        //    };
        //}
        public ServiceResult<List<EmployeeGridVm>> GetEmployeeBySection(string id)
        {
            int? branchId = RiddhaSession.BranchId;
            //Changes For Report Search Parameter..
            string[] sections = id.Split(',');
            List<EmployeeGridVm> employee = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                             join d in sections on c.SectionId equals int.Parse(d)
                                             select new EmployeeGridVm
                                             {
                                                 Id = c.Id,
                                                 Name = c.Name
                                             }).ToList();
            return new ServiceResult<List<EmployeeGridVm>>()
            {
                Data = employee,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<EEmployee>> GetEmployeesByDepartment(string id)
        {
            string[] departments = id.Split(',');
            var employees = employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            var list = (from c in employees
                        join d in departments
                        on c.Section.DepartmentId equals int.Parse(d)
                        select c).ToList();
            return new ServiceResult<List<EEmployee>>()
            {
                Data = list,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost, ActionFilter("2009")]
        public ServiceResult<EEmployee> Post(EmployeeVm model)
        {
            //model.BranchId = RiddhaSession.BranchId;
            model.Section = null;
            EEmployee employee = castVmToEmployeeModel(model);
            employeeServices._softwarePackageId = RiddhaSession.PackageId;
            var result = employeeServices.Add(employee, model.ShiftId, model.WeeklyOffIds);

            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2001", "2009", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, employee.Id, result.Message);
                string EnableADMS = ConfigurationManager.AppSettings["EnableADMS"];
                if (EnableADMS != "0")
                {
                    WebrequestService webrequest = new WebrequestService(RiddhaSession.ADMSUrl);
                    var userPic = "";
                    if (model.ImageUrl != null)
                    {
                        userPic = getBase64ImageFromFile(model.ImageUrl);
                    }
                    ADMSUserInfoVm vm = new ADMSUserInfoVm()
                    {
                        Id = 0,
                        DeviceSn = "",
                        UserPin = model.DeviceCode + "",
                        Name = model.Name,
                        Card = "",
                        Password = "",
                        Privilege = model.IsManager ? 14 : 0,
                        Category = 0,
                        BranchCode = RiddhaSession.BranchCode,
                        PhotoURL = userPic,
                    };
                    try
                    {
                        new Thread(() =>
                        {
                            var webRequestResult = webrequest.Post<ADMSUserInfoVm>("api/UserInfoApi/updateuserfromhajiri", webrequest.SerializeObject<ADMSUserInfoVm>(vm)).Result;

                        }).Start();
                    }
                    catch (Exception ex)
                    {
                        Log.SytemLog(ex.Message);
                    }
                }
            }
            return new ServiceResult<EEmployee>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };

        }

        private EEmployee castVmToEmployeeModel(EmployeeVm model)
        {
            return new EEmployee()
            {
                AllowedEarlyOut = model.AllowedEarlyOut,
                AllowedLateIn = model.AllowedLateIn,
                BloodGroup = model.BloodGroup,
                BranchId = model.BranchId,
                Code = model.Code,
                ConsiderTimeLoss = model.ConsiderTimeLoss,
                DateOfBirth = model.DateOfBirth,
                DateOfJoin = model.DateOfJoin,
                Designation = model.Designation,
                DesignationId = model.DesignationId,
                DeviceCode = model.DeviceCode,
                Email = model.Email,
                FourPunch = model.FourPunch,
                Gender = model.Gender,
                HalfDayMarking = model.HalfDayMarking,
                HalfdayWorkingHour = model.HalfdayWorkingHour,
                Id = model.Id,
                ImageUrl = model.ImageUrl,
                MaritialStatus = model.MaritialStatus,
                MaxWorkingHour = model.MaxWorkingHour,
                Mobile = model.Mobile,
                MultiplePunch = model.MultiplePunch,
                Name = model.Name,
                NoPunch = model.NoPunch,
                PermanentAddress = model.PermanentAddress,
                PresentMarkingDuration = model.PresentMarkingDuration,
                Section = model.Section,
                SectionId = model.SectionId,
                ShiftTypeId = model.ShiftTypeId,
                ShortDayWorkingHour = model.ShortDayWorkingHour,
                SinglePunch = model.SinglePunch,
                TemporaryAddress = model.TemporaryAddress,
                TwoPunch = model.TwoPunch,
                WOTypeId = model.WOTypeId,
                NameNp = model.NameNp,
                PermanentAddressNp = model.PermanentAddressNp,
                TemporaryAddressNp = model.TemporaryAddressNp,
                PassportNo = model.PassportNo,
                CitizenNo = model.CitizenNo,
                IssueDate = model.IssueDate,
                IssueDistict = model.IssueDistict,
                Religion = model.Religion,
                GradeGroupId = model.GradeGroupId,
                GradeGroup = model.GradeGroup,
                IsManager = model.IsManager,
                UserId = model.UserId,
                IsOTAllowed = model.IsOTAllowed,
                MaxOTHour = model.MaxOTHour,
                MinOTHour = model.MinOTHour,
                ReportingManagerId = model.ReportingManagerId,
                EmploymentStatus = model.EmploymentStatus,
                EnableSSN = model.EnableSSN,
                PANNo = model.PANNo,
                SSNNo = model.SSNNo,
                BankId = model.BankId,
                BankAccountNo = model.BankAccountNo,
            };
        }

        [ActionFilter("2010")]
        public ServiceResult<EEmployee> Put(EmployeeVm model)
        {
            //var branch = RiddhaSession.CurrentUser.Branch;
            //var branchId = RiddhaSession.BranchId;
            //var companyId = RiddhaSession.CompanyId;
            //model.BranchId = branchId;
            model.Section = null;
            EEmployee employee = castVmToEmployeeModel(model);
            var result = employeeServices.Update(employee, model.ShiftId, model.WeeklyOffIds);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2001", "2010", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, result.Message);
                string EnableADMS = ConfigurationManager.AppSettings["EnableADMS"];
                if (EnableADMS != "0")
                {
                    #region sync employee to wdms devices

                    var userPic = "";
                    if (model.ImageUrl != null)
                    {
                        userPic = getBase64ImageFromFile(model.ImageUrl);
                    }
                    WebrequestService webrequest = new WebrequestService(RiddhaSession.ADMSUrl);
                    ADMSUserInfoVm vm = new ADMSUserInfoVm()
                    {
                        Id = model.Id,
                        DeviceSn = "",
                        UserPin = model.DeviceCode + "",
                        Name = model.Name,
                        Card = "",
                        Password = "",
                        Privilege = model.IsManager ? 14 : 0,
                        Category = 0,
                        BranchCode = RiddhaSession.BranchCode,
                        PhotoURL = userPic,
                    };
                    try
                    {
                        new Thread(() =>
                        {
                            var webRequestResult = webrequest.Post<ADMSUserInfoVm>("api/UserInfoApi/updateuserfromhajiri", webrequest.SerializeObject<ADMSUserInfoVm>(vm)).Result;

                        }).Start();
                    }
                    catch (Exception ex)
                    {
                        Log.SytemLog(ex.Message);
                    }
                    #endregion
                }
            }

            return new ServiceResult<EEmployee>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status

            };
        }

        private string getBase64ImageFromFile(string imageUrl)
        {
            var fullPath = Path.GetFullPath(HostingEnvironment.ApplicationPhysicalPath + imageUrl);
            if (File.Exists(fullPath))
            {
                using (Image image = Image.FromFile(fullPath))
                {
                    var img = (Image)ResizeImage(image, 300, 300);
                    using (MemoryStream m = new MemoryStream())
                    {
                        img.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            return "";
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        [HttpDelete, ActionFilter("2011")]
        public ServiceResult<int> Delete(int id)
        {
            var employee = employeeServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = employeeServices.Remove(employee);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2001", "2011", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, result.Message);
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status

            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetDesignationForDropdown()
        {
            int companyId = RiddhaSession.CompanyId;
            string language = RiddhaSession.Language.ToString();
            SDesignation designationServices = new SDesignation();
            List<DropdownViewModel> resultLst = (from c in designationServices.List().Data.Where(x => x.CompanyId == companyId).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                                 }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetGradeGroupForDropdown()
        {
            int? branchId = RiddhaSession.BranchId;
            string language = RiddhaSession.Language.ToString();
            SGradeGroup gradeGroupServices = new SGradeGroup();
            List<DropdownViewModel> resultLst = (from c in gradeGroupServices.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.Name
                                                 }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetShiftsForDropdown(int branchId)
        {
            string language = RiddhaSession.Language.ToString();
            SShift shiftServices = new SShift();
            List<DropdownViewModel> resultLst = (from c in shiftServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = language == "ne" && c.NameNp != null ? c.NameNp : c.ShiftName
                                                 }
                                                   ).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetRolesForDropdown()
        {
            int? branchId = RiddhaSession.BranchId;
            var userRoles = userRoleServices.List().Data.Where(x => x.BranchId == branchId);
            var resultLst = (from c in userRoles
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
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetDepartmentsForDropdown(int branchId)
        {
            string language = RiddhaSession.Language.ToString();
            SDepartment departmentServices = new SDepartment();
            List<DropdownViewModel> resultLst = (from c in departmentServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = language == "ne" &&
                                                     c.NameNp != null ? c.NameNp : c.Name
                                                 }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EUser> ActivateDeactivateEmp(int empId, string userName, string password, int roleId = 0)
        {
            bool isDuplicate = false;
            ServiceResult<EUser> result = new ServiceResult<EUser>();
            int branchId = (int)RiddhaSession.BranchId;
            EEmployee employee = employeeServices.List().Data.Where(x => x.Id == empId && x.BranchId == branchId).FirstOrDefault() ?? new EEmployee();
            EUser login = new EUser();
            if (employee.UserId == null)
            {
                isDuplicate = userServices.CheckDuplicate(userName, branchId, 0);
                if (isDuplicate)
                {
                    result.Data = null;
                    result.Status = ResultStatus.processError;
                    result.Message = loc.Localize("Duplicate");
                }
                else
                {
                    login.BranchId = branchId;
                    login.RoleId = roleId;
                    login.Name = userName.Trim();
                    login.Password = password.Trim();
                    login.IsSuspended = false;
                    login.FullName = employee.Name;
                    login.PhotoURL = employee.ImageUrl;
                    login.UserType = UserType.User;
                    result = userServices.Add(login);
                    if (result.Status == ResultStatus.Ok)
                    {
                        employee.UserId = result.Data.Id;
                        EEmployeeLogin employeeLogin = new EEmployeeLogin()
                        {
                            EmployeeId = employee.Id,
                            UserName = userName.Trim(),
                            Password = password.Trim(),
                            BranchId = branchId,
                            RoleId = roleId,
                            IsActivated = true
                        };
                        empLoginServices.Add(employeeLogin);
                        ServiceResult<EEmployee> empResult = employeeServices.UpdateEmployeeOnly(employee);
                        if (empResult.Status == ResultStatus.Ok)
                        {
                            result.Message = loc.Localize("ActivatedSuccess");
                        }
                        else
                        {
                            result.Status = ResultStatus.processError;
                            result.Message = loc.Localize("ActivatedFailed");
                            result.Data = null;
                        }
                    }
                }
            }
            else
            {
                login = userServices.List().Data.Where(x => x.Id == employee.UserId && x.IsDeleted == false).FirstOrDefault() ?? new EUser();

                if (login.IsSuspended == true)
                {
                    isDuplicate = userServices.CheckDuplicate(userName, branchId, login.Id);
                    if (isDuplicate)
                    {
                        result.Data = null;
                        result.Status = ResultStatus.processError;
                        result.Message = loc.Localize("Duplicate");
                    }
                    else
                    {
                        login.IsSuspended = false;
                        login.Name = userName.Trim();
                        login.Password = password.Trim();
                        login.RoleId = roleId;
                        result = userServices.Update(login);
                        if (result.Status == ResultStatus.Ok)
                        {
                            EEmployeeLogin empLogin = empLoginServices.List().Data.Where(x => x.EmployeeId == employee.Id).FirstOrDefault();
                            if (empLogin != null)
                            {
                                empLogin.UserName = userName.Trim();
                                empLogin.Password = password.Trim();
                                empLogin.RoleId = roleId;
                                empLoginServices.Update(empLogin);
                            }
                            result.Message = loc.Localize("ActivatedSuccess");
                        }
                    }
                }
                else if (login.IsSuspended == false)
                {
                    login.IsSuspended = true;
                    result = userServices.Update(login);
                    if (result.Status == ResultStatus.Ok)
                    {
                        result.Message = loc.Localize("DeactivatedSuccess");
                    }
                }
            }

            return result;
        }
        [HttpPost, ActionFilter("")]
        [ActionFilter("2017")]
        public ServiceResult<List<EEmployee>> Upload()
        {
            var branch = RiddhaSession.CurrentUser.Branch;
            var request = HttpContext.Current.Request;
            List<EEmployee> EmpLst = new List<EEmployee>();
            using (var package = new OfficeOpenXml.ExcelPackage(request.InputStream))
            {
                IQueryable<EDesignation> desigQuery = new SDesignation().List().Data;
                IQueryable<ESection> sectionQuery = new SSection().List().Data;
                TimeSpan defaultTime = "00:00".ToTimeSpan();
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    EEmployee model = new EEmployee();
                    model.BranchId = getBranchId(sectionQuery, (workSheet.Cells[rowIterator, 9].Value ?? string.Empty).ToString(), RiddhaSession.CompanyId);
                    model.DeviceCode = (workSheet.Cells[rowIterator, 1].Value ?? string.Empty).ToInt();
                    if (model.DeviceCode == 0)
                    {
                        continue;
                    }
                    model.Code = (workSheet.Cells[rowIterator, 2].Value ?? string.Empty).ToString();
                    if (checkValidString(model.Code) == false)
                    {
                        continue;
                    }
                    model.Name = (workSheet.Cells[rowIterator, 3].Value ?? string.Empty).ToString();
                    if (checkValidString(model.Name) == false)
                    {
                        continue;
                    }
                    model.NameNp = (workSheet.Cells[rowIterator, 4].Value ?? string.Empty).ToString();
                    model.Gender = (Gender)(workSheet.Cells[rowIterator, 5].Value ?? string.Empty).ToInt();
                    model.PermanentAddress = (workSheet.Cells[rowIterator, 6].Value ?? string.Empty).ToString();
                    model.Mobile = (workSheet.Cells[rowIterator, 7].Value ?? string.Empty).ToString();
                    model.DesignationId = getDesignationId(desigQuery, (workSheet.Cells[rowIterator, 8].Value ?? string.Empty).ToString(), RiddhaSession.CompanyId);
                    model.SectionId = getSectionId(sectionQuery, (workSheet.Cells[rowIterator, 9].Value ?? string.Empty).ToString(), RiddhaSession.CompanyId);

                    model.MaxWorkingHour = defaultTime;
                    model.AllowedLateIn = defaultTime;
                    model.AllowedEarlyOut = defaultTime;
                    model.HalfdayWorkingHour = defaultTime;
                    model.ShortDayWorkingHour = defaultTime;
                    model.PresentMarkingDuration = defaultTime;
                    model.TwoPunch = true;
                    model.ShiftTypeId = 0;
                    model.WOTypeId = 0;
                    model.IsManager = false;
                    EmpLst.Add(model);
                }
                var result = new ServiceResult<List<EEmployee>>();
                result = result = employeeServices.UploadExcel(EmpLst, branch.Id);
                return new ServiceResult<List<EEmployee>>()
                {
                    Data = result.Data,
                    Status = result.Status,
                    Message = loc.Localize(result.Message)
                };
            }
        }

        //private int? getSectionId(IQueryable<ESection> sectionQuery, string code, int branchId)
        //{
        //    if (string.IsNullOrEmpty(code))
        //    {
        //        return null;
        //    }
        //    var section = sectionQuery.Where(x => x.BranchId == branchId && x.Code.ToUpper() == code.Trim().ToUpper()).FirstOrDefault();
        //    if (section == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return section.Id;
        //    }
        //}
        private int? getSectionId(IQueryable<ESection> sectionQuery, string code, int companyId)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            var section = sectionQuery.Where(x => x.Branch.CompanyId == companyId && x.Code.ToUpper() == code.Trim().ToUpper()).FirstOrDefault();
            if (section == null)
            {
                return null;
            }
            else
            {
                return section.Id;
            }
        }

        private int? getBranchId(IQueryable<ESection> sectionQuery, string code, int companyId)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            var section = sectionQuery.Where(x => x.Branch.CompanyId == companyId && x.Code.ToUpper() == code.Trim().ToUpper()).FirstOrDefault();
            if (section == null)
            {
                return null;
            }
            else
            {
                return section.BranchId;
            }
        }

        private int? getDesignationId(IQueryable<EDesignation> desigQuery, string code, int companyId)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            var designation = desigQuery.Where(x => x.CompanyId == companyId && x.Code.ToUpper() == code.Trim().ToUpper()).FirstOrDefault();
            if (designation == null)
            {
                return null;
            }
            else
            {
                return designation.Id;
            }
        }

        [HttpGet, ActionFilter("2014")]
        public ServiceResult<EUser> GetEmpLogin(int empId)
        {
            EUser user = new EUser();
            EEmployee emp = employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
            if (emp.UserId == null)
            {
                return new ServiceResult<EUser>()
                {
                    Data = user,
                    Status = ResultStatus.Ok
                };
            }
            else
            {
                user = userServices.List().Data.Where(x => x.Id == emp.UserId).FirstOrDefault() ?? new EUser();
                Common.AddAuditTrail("2001", "2014", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, empId, "Create Login");
                return new ServiceResult<EUser>()
                {
                    Data = user,
                    Status = ResultStatus.Ok
                };
            }
        }
        private bool checkValidString(string value)
        {
            return !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
        }
        public ServiceResult<List<SectionGridVm>> GetAllSections(int branchId)
        {
            //int? branchId = RiddhaSession.BranchId;
            SSection service = new SSection();
            var sectionLst = (from c in service.List().Data.Where(x => x.BranchId == branchId)
                              select new SectionGridVm()
                              {
                                  Id = c.Id,
                                  DepartmentId = c.DepartmentId,
                                  Name = c.Name,
                                  NameNp = c.NameNp
                              }).ToList();

            return new ServiceResult<List<SectionGridVm>>()
            {
                Data = sectionLst,
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("2036"), HttpPost]
        public KendoGridResult<List<EmployeeGridVm>> GetEmpKendoGrid(HREmployeeGridModel vm)
        {
            string language = RiddhaSession.Language.ToString();
            IQueryable<EUser> userQuery = userServices.List().Data;
            SEmployee service = new SEmployee();
            //IQueryable<EEmployee> empQuery = service.List().Data.Where(x => x.BranchId == branchId && x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated);
            List<EEmployee> empQuery;
            var emp = Common.GetEmployees().Data;
            if (vm.Type == "active")
            {
                empQuery = emp.Where(x => x.BranchId == vm.BranchId && x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated).ToList();
            }
            else if (vm.Type == "inactive")
            {
                empQuery = emp.Where(x => x.BranchId == vm.BranchId && x.EmploymentStatus == EmploymentStatus.Resigned || x.EmploymentStatus == EmploymentStatus.Terminated).ToList();
            }
            else
            {
                empQuery = emp.Where(x => x.BranchId == vm.BranchId).ToList();
            }
            int totalRowNum = empQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            List<EEmployee> paginatedQuery;
            switch (searchField)
            {
                case "DeviceCode":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.DeviceCode.ToString().StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.DeviceCode.ToString() == searchValue.Trim()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "IdCardNo":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "DepartmentName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "Section":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                case "Mobile":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Mobile.StartsWith(searchValue)).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Mobile == searchValue).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).ToList();
                    }
                    break;
                case "DesignationName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Designation.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ToList();
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Designation.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ToList();
                    }
                    break;
                default:
                    paginatedQuery = empQuery.OrderByDescending(x => x.Id).ToList();
                    break;
            }
            var employeelist = (from c in paginatedQuery
                                select new EmployeeGridVm()
                                {
                                    Id = c.Id,
                                    DateOfJoin = c.DateOfJoin,
                                    DepartmentName = c.Section == null ? "" : c.Section.Department.Code + " - " + (!string.IsNullOrEmpty(c.Section.Department.NameNp) && language == "ne" ? c.Section.Department.NameNp : c.Section.Department.Name),
                                    //EmployeeName = !string.IsNullOrEmpty(c.NameNp) ? c.Name + "  (" + c.NameNp + ")" : c.Name,
                                    EmployeeName = c.Name,
                                    EmployeeNameNp = c.NameNp,
                                    IdCardNo = c.Code,
                                    DeviceCode = c.DeviceCode,
                                    Mobile = c.Mobile,
                                    Email = c.Email,
                                    PhotoURL = c.ImageUrl,
                                    SectionId = c.SectionId,
                                    Section = c.Section == null ? "" : c.Section.Code + " - " + (!string.IsNullOrEmpty(c.Section.NameNp) && language == "ne" ? c.Section.NameNp : c.Section.Name),
                                    DepartmentId = c.Section == null ? 0 : c.Section.DepartmentId,
                                    DesignationName = c.Designation == null ? "" : c.Designation.Code + " - " + (language == "ne" ? c.Designation.NameNp : c.Designation.Name),
                                    GradeGroupId = c.GradeGroupId,
                                    GradeGroupName = c.GradeGroup == null ? "" : c.GradeGroup.Name,
                                    UserId = c.UserId,
                                    UserName = c.User == null ? "" : c.User.Name,
                                    Password = c.User == null ? "" : c.User.Password,
                                    RoleId = c.User == null ? 0 : c.User.RoleId,
                                    ShiftType = c.ShiftTypeId == 0 ? "Fixed" : c.ShiftTypeId == 1 ? "Weekly" : "Monthly",
                                    EmploymentStatus = c.EmploymentStatus,
                                    IsOTAllowed = c.IsOTAllowed
                                }).ToList();
            employeelist.ForEach(x => x.Status = getLoginStatus(userQuery, x.UserId) == true ? loc.Localize("Off") : loc.Localize("On"));
            return new KendoGridResult<List<EmployeeGridVm>>()
            {
                Status = ResultStatus.Ok,
                Data = employeelist.Skip(vm.Skip).Take(vm.Take).ToList(),
                TotalCount = employeelist.Count()
            };
        }
        [HttpPost, ActionFilter("2011")]
        public ServiceResult<bool> DeleteKendoGridEmployee(EmpDeleteVm vm)
        {
            var empLst = (from c in employeeServices.List().Data
                          join d in vm.Ids on c.Id equals d
                          select c).ToList();
            var result = employeeServices.RemoveRange(empLst);
            return result;
        }
        //api for employee list in autocomplete
        [HttpPost]
        public ServiceResult<List<EmpSearchViewModel>> GetEmpLstForAutoComplete(EKendoAutoComplete model)
        {
            string lang = RiddhaSession.Language;
            int? branchId = RiddhaSession.BranchId;
            List<EmpSearchViewModel> resultLst = new List<EmpSearchViewModel>();
            if (model != null)
            {
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                var empLst = Common.GetEmployees().Data.Where(x => x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated);
                IQueryable<EEmployee> emp = employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager);
                if (searchText == null || searchText == "___")
                {
                    empLst = empLst.OrderBy(x => x.Name).Take(50);
                }
                else
                {
                    empLst = empLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).OrderBy(x => x.Name).Take(50);
                }
                if (lang == "ne")
                {
                    resultLst = (from c in empLst
                                 select new EmpSearchViewModel()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     //Name = c.Branch.Name + " - " + c.Code + " - " + (!string.IsNullOrEmpty(c.NameNp) ? c.NameNp : c.Name) + " - " + (c.Mobile ?? ""),
                                     Name = c.Code + " - " + (!string.IsNullOrEmpty(c.NameNp) ? c.NameNp : c.Name) + " - " + (c.Mobile ?? ""),
                                     EmployeeName = c.NameNp == null ? c.Name : c.NameNp,
                                     Designation = c.Designation == null ? "" : (c.Designation.NameNp == null ? c.Designation.Name : c.Designation.NameNp),
                                     Section = c.Section == null ? "" : (c.Section.NameNp == null ? c.Section.Name : c.Section.NameNp),
                                     Department = c.Section == null ? "" : (c.Section.Department.NameNp == null ? c.Section.Department.Name : c.Section.Department.NameNp),
                                     DesignationId = c.DesignationId,
                                     Photo = c.ImageUrl,
                                     Email = c.Email,
                                     ReportingManagerId = c.ReportingManagerId,
                                     ReportingManagerName = getReportingMangerName(emp, c.ReportingManagerId)
                                 }).ToList();
                }
                else
                {
                    resultLst = (from c in empLst
                                 select new EmpSearchViewModel()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     //Name = c.Branch.Name + " - " + c.Code + " - " + c.Name + " - " + (c.Mobile ?? ""),
                                     Name = c.Code + " - " + c.Name + " - " + (c.Mobile ?? ""),
                                     EmployeeName = c.Name,
                                     Designation = c.Designation == null ? "" : c.Designation.Name,
                                     Section = c.Section == null ? "" : c.Section.Name,
                                     Department = c.Section == null ? "" : c.Section.Department.Name,
                                     DesignationId = c.DesignationId,
                                     Photo = c.ImageUrl,
                                     Email = c.Email,
                                     ReportingManagerId = c.ReportingManagerId,
                                     ReportingManagerName = getReportingMangerName(emp, c.ReportingManagerId)
                                 }).ToList();
                }
            }
            return new ServiceResult<List<EmpSearchViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        private string getReportingMangerName(IQueryable<EEmployee> emp, int? reportingManagerId)
        {
            var data = emp.Where(x => x.Id == reportingManagerId).FirstOrDefault();
            return data == null ? "" : data.Name;
        }

        [HttpPost, ActionFilter("2044")]
        public ServiceResult<object> ApplyShifttype(ApplyShifttypeModel model)
        {
            var result = employeeServices.UpdateShiftType(model);
            return new ServiceResult<object>()
            {
                Status = result.Status,
                Message = result.Message
            };
        }

        [HttpPost]
        public ServiceResult<object> ApplyDesignationPromotion(ApplyDesignationPromotiontypeModel model)
        {
            var result = employeeServices.UpdateDesignationPromotion(model);
            return new ServiceResult<object>()
            {
                Status = result.Status,
                Message = result.Message
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetBanks()
        {
            SBank bankServices = new SBank();
            var result = (from c in bankServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId)
                          select new DropdownViewModel()
                          {
                              Id = c.Id,
                              Name = c.Name,
                          }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetCompanyDevices()
        {
            SCompanyDeviceAssignment companyDeviceAssignmentServices = new SCompanyDeviceAssignment();
            var result = (from c in companyDeviceAssignmentServices.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId && x.Device.DeviceType == DeviceType.ADMS).ToList()
                          select new DropdownViewModel()
                          {
                              Id = c.Id,
                              Name = c.Device.SerialNumber,
                          }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<EmployeeOTVm> ManageOT(EmployeeOTVm vm)
        {
            var result = employeeServices.ManageOt(vm, (int)RiddhaSession.BranchId);
            return new ServiceResult<EmployeeOTVm>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }
}

public class EmployeeGridVm
{
    public int Id { get; set; }
    public string IdCardNo { get; set; }
    public int DeviceCode { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeeNameNp { get; set; }
    public DateTime? DateOfJoin { get; set; }
    public int? DepartmentId { get; set; }
    public int? SectionId { get; set; }
    public string Section { get; set; }
    public string DepartmentName { get; set; }
    public int DesignationId { get; set; }
    public int? GradeGroupId { get; set; }
    public string GradeGroupName { get; set; }
    public string DesignationName { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string NameNp { get; set; }
    public string PhotoURL { get; set; }
    public string Status { get; set; }
    public int? UserId { get; set; }
    public string Name { get; set; }
    public int TotalCount { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int? RoleId { get; set; }
    public string ShiftType { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; }
    public string EmploymentStatusName { get; set; }
    public bool IsOTAllowed { get; set; }
    public int ReportingManagerId { get; set; }
    public int BankId { get; set; }
    public string BankAccountNo { get; set; }
}
public class EmployeeInfoVm
{
    public int Id { get; set; }
    public string Photo { get; set; }
    public string IdCardNo { get; set; }
    public string EmployeeName { get; set; }
    public string BranchName { get; set; }
    public string DepartmentName { get; set; }
    public string SectionName { get; set; }
    public string DesignationName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfJoin { get; set; }
    public string Mobile { get; set; }
    public string Address { get; set; }
    public int BloodGroup { get; set; }
    public string PunchType { get; set; }
    public string MaxWorkingHours { get; set; }
    public int? ShiftType { get; set; }
    public string ShiftName { get; set; }
    public int BankId { get; set; }
    public string BankAccountNo { get; set; }
}
public class EmpDeleteVm
{
    public int[] Ids { get; set; }
}
