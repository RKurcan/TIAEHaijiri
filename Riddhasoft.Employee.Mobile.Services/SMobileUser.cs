using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Entity.User;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using Riddhasoft.User.Entity;
using System;
using System.Data;
using System.Linq;

namespace Riddhasoft.Employee.Mobile.Services
{
    public class SMobileUser
    {
        Riddhasoft.DB.RiddhaDBContext db;
        public SMobileUser()
        {
            db = new DB.RiddhaDBContext();
            // db.Configuration.ProxyCreationEnabled = false;
        }
        public MobileResult<EMEmployeeProfile> Login(EMLogin model)
        {
            try
            {
                var userData = db.User.Where(x => x.Name == model.Username && x.Password == model.Password && x.UserType == UserType.User && x.IsDeleted == false && x.Branch.Code == model.CompanyCode).FirstOrDefault();
                if (userData != null)
                {
                    if (userData.Branch.Company.IsSuspended == true)
                    {
                        return new MobileResult<EMEmployeeProfile>()
                        {
                            Data = null,
                            Message = "Company Account is suspended please contact to Vendor",
                            Status = MobileResultStatus.ProcessError,
                            Token = null
                        };
                    }
                    else
                    {
                        var emp = db.Employee.Where(x => x.UserId == userData.Id).FirstOrDefault();
                        if (emp != null)
                        {
                            var context = UpdateSession(userData);
                            var empQuery = (from c in db.Employee.Where(x => x.UserId == userData.Id).ToList()
                                            select new EMEmployeeProfile()
                                            {
                                                Department = (c.Designation ?? new EDesignation() { Name = "" }).Name,
                                                EmployeeId = c.Id,
                                                FullName = c.Name,
                                                IdCardNo = c.Code,
                                                PhotoUrl = c.ImageUrl,
                                                Designation = c.Section == null ? "" : c.Section.Department.Name
                                            });
                            var emProfile = empQuery.Single();
                            return new MobileResult<EMEmployeeProfile>()
                            {
                                Data = emProfile,
                                Message = "Login Successfully",
                                Status = MobileResultStatus.Ok,
                                Token = context.Token
                            };
                        }
                        else
                        {
                            return new MobileResult<EMEmployeeProfile>()
                            {
                                Data = null,
                                Message = "Employee not updated. Please contact ur admin.",
                                Status = MobileResultStatus.ProcessError
                            };
                        }

                    }

                }
                else
                {
                    return new MobileResult<EMEmployeeProfile>()
                    {
                        Data = null,
                        Message = "Invalid Login. Please Try Again.",
                        Status = MobileResultStatus.ProcessError,
                        Token = null
                    };
                }
            }
            catch (Exception )
            {

                return new MobileResult<EMEmployeeProfile>()
                {
                    Data = null,
                    Message = "Error, Please contact to support center...",
                    Status = MobileResultStatus.ProcessError,
                    Token = null
                };
            }
          

        }

        public int GetApiVersion()
        {
            int ApiVersion = 0;
            try
            {
                ApiVersion = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("ApiVersion"));

            }
            catch (Exception ex)
            {
                ApiVersion = 0;
            }
            return ApiVersion;
        }

        private EContext UpdateSession(EUser User)
        {
            var newContext = new EContext() { Id = 0, LastLogin = DateTime.Now, TimeOut = TimeSpan.FromMinutes(20), UserId = User.Id, Token = getToken() };
            db.Context.Add(newContext);
            db.SaveChanges();

            db.SessionDetail.Add(new ESessionDetail() { Id = 0, Key = "User", Value = User.Id.ToString(), ContextId = newContext.Id });
            db.SaveChanges();
            return newContext;
        }
        private string getToken()
        {
            return Guid.NewGuid().ToString();
        }
        public MobileResult<EMEmployeeProfile> GetEmployeeInfoById(int EmployeeId)
        {
            if (EmployeeId != 0)
            {

                var empQuery = (from c in db.Employee.Where(x => x.Id == EmployeeId).ToList()
                                select new EMEmployeeProfile()
                                {
                                    Department = (c.Designation ?? new EDesignation() { Name = "" }).Name,
                                    EmployeeId = c.Id,
                                    FullName = c.Name,
                                    IdCardNo = c.Code,
                                    PhotoUrl = c.ImageUrl == null ? "/Images/UserImageIcon.png" : c.ImageUrl,
                                    Designation = c.Section == null ? "" : c.Section.Department.Name,


                                }).FirstOrDefault();
                var result = empQuery;



                getCurrentUserAttendanceInfo(result);





                return new MobileResult<EMEmployeeProfile>()
                {
                    Data = empQuery,
                    Message = "Data Returned Successfully",
                    Status = MobileResultStatus.Ok,
                };
            }
            else
            {
                return new MobileResult<EMEmployeeProfile>()
                {
                    Data = null,
                    Message = "Invalid Login. Please Try Again.",
                    Status = MobileResultStatus.ProcessError,
                    Token = null
                };
            }

        }

        public MobileResult<EMEmployeesPersonalInfo> GetEmployeePersonalInfoById(int EmployeeId)
        {
            if (EmployeeId != 0)
            {
                string EmpIds = EmployeeId.ToString();


                var empQuery = db.SP_GET_EmployeePersonalInfo_By_Id(EmpIds);
                if (empQuery.Count>0)
                {
                    var result = (from c in empQuery.ToList()
                                  select new EMEmployeesPersonalInfo()
                                  {
                                      BranchesName = c.BranchesName,
                                      CitizenNo = c.CitizenNo,
                                      Code = c.Code,
                                      DateOfBirth = c.DateOfBirth,
                                      DateOfJoin = c.DateOfJoin,
                                      DesignationName = c.DesignationName,
                                      DeviceCode = c.DeviceCode,
                                      CompanyName = c.CompanyName,
                                      FullName = c.FullName,
                                      Gender = c.Gender,
                                      GroupGradeName = c.GroupGradeName,
                                      ImageUrl = c.ImageUrl,
                                      IsManager = c.IsManager,
                                      IssueDate = c.IssueDate,
                                      IssueDistrict = c.IssueDistrict,
                                      MaritialStatus = c.MaritialStatus,
                                      Mobile = c.Mobile,
                                      PassportNo = c.PassportNo,
                                      PermanentAddress = c.PermanentAddress,
                                      SectionName = c.SectionName,
                                      TemporaryAddress = c.TemporaryAddress,
                                      DepartmentName = c.DepartmentName

                                  }).FirstOrDefault();
                    return new MobileResult<EMEmployeesPersonalInfo>()
                    {
                        Data = result,
                        Message = "Data Returned Successfully",
                        Status = MobileResultStatus.Ok,
                    };
                }
                else
                {
                    return new MobileResult<EMEmployeesPersonalInfo>()
                    {
                        Data = null,
                        Message = "No data found...",
                        Status = MobileResultStatus.ProcessError,
                    };
                }
            }
            else
            {
                return new MobileResult<EMEmployeesPersonalInfo>()
                {
                    Data = null,
                    Message = "Emp Id was not suppied",
                    Status = MobileResultStatus.ProcessError,
                    Token = null
                };
            }

        }
        private void getCurrentUserAttendanceInfo(EMEmployeeProfile result)
        {
            SMHome homeService = new SMHome();
            var homeAttendanceInfo = homeService.GetHomeAttendanceInfo(result.EmployeeId);
            if (homeAttendanceInfo != null)
            {
                result.PunchTime = homeAttendanceInfo.Attendance;
                result.Shift = homeAttendanceInfo.PlannedTime;


                if (string.IsNullOrEmpty(homeAttendanceInfo.PlannedTime))
                {
                    result.Remark = "Weekend";
                }
                else if (!string.IsNullOrEmpty(homeAttendanceInfo.Kaj))
                {
                    result.Remark = "Kaj";
                }
                else if (!string.IsNullOrEmpty(homeAttendanceInfo.Leave))
                {
                    result.Remark = "Leave";
                }
                else if (!string.IsNullOrEmpty(homeAttendanceInfo.OfficeVisit))
                {
                    result.Remark = "Office Visit";
                }

                else if (string.IsNullOrEmpty(result.Remark) && string.IsNullOrEmpty(result.PunchTime))
                {
                    result.Remark = "Absent";
                }
                else
                {
                    result.Remark = "Present";
                }
            }
        }
    }
}
