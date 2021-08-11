using Riddhasoft.Device.Services;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Entity.User;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
namespace RTech.Demo.Utilities
{
    public class Common
    {
        public static bool AjaxRequest
        {
            get
            {

                return true;
            }

        }
        public static bool ValidateToken(string token)
        {
            if (token != "")
            {
                SContext con = new SContext();
                return con.List().Data.Where(x => x.Token == token).FirstOrDefault() != null;
            }
            else
            {
                return false;
            }
        }

        public static string RequestToken { get { return HttpContext.Current.Request.Headers.GetValues("Token").FirstOrDefault() ?? ""; } }
        private static int GetCurrentFiscalYear()
        {
            var branch = RiddhaSession.CurrentUser.Branch;
            SFiscalYear fiscalYearServices = new SFiscalYear();
            var fiscalYear = fiscalYearServices.List().Data.Where(x => x.CurrentFiscalYear && x.BranchId == branch.Id).FirstOrDefault() ?? new EFiscalYear();
            return fiscalYear.Id;
        }
        public static int GetNewResellerCode()
        {
            SReseller resellerService = new SReseller();
            var maxId = resellerService.List().Data.ToList();
            if (maxId != null && maxId.Count == 0)
            {
                return 1;
            }
            else
            {
                return maxId.Max(x => x.Code).ToInt() + 1;
            }

        }
        public List<ReportItem> populateList()
        {
            LocalizedString loc = new LocalizedString();
            List<ReportItem> Reports = new List<ReportItem>() {
                new ReportItem(){
                SN=1,
                Report=loc.Localize("DailyEmployeeAttendanceReport"),
                ReportId=2,
                //Description="Daily Employee Performance that includes all performance factor."
                Description=loc.Localize("DailyEmployeeAttendanceReportDesc"),
                ActionCode="5011"

                },
                new ReportItem(){
                SN=2,
                ReportId=4,
                Report=loc.Localize("DailyEarlyInReport"),
                //Description="Report on Employees early Arrival"
                Description=loc.Localize("DailyEarlyInDesc"),
                ActionCode="5002"
                },
                new ReportItem(){
                SN=3,
                ReportId=5,
                Report=loc.Localize("DailyEarlyOutReport"),
                //Description="Report on Employees leaving early"
                Description=loc.Localize("DailyEarlyOutDesc"),
                ActionCode="5003"
                },
                new ReportItem(){
                SN=4,
                ReportId=6,
                Report=loc.Localize("DailyEmployeeAbsentReport"),
               // Description="Report on Absent Employees"
                Description=loc.Localize("DailyEmployeeAbsentReportDesc"),
                ActionCode="5004"
                },
                new ReportItem(){
                SN=5,
                ReportId=7,
                Report=loc.Localize("DailyLateInReport"),
                //Description="Report on Employees Late Arrival"
                Description=loc.Localize("DailyLateInDesc"),
                ActionCode="5005"
                },
                new ReportItem(){
                SN=6,
                Report=loc.Localize("DailyLateOutReport"),
                ReportId=8,
                //Description="Report on employees leaving Late"
                Description=loc.Localize("DailyLateOutDesc"),
                ActionCode="5006"
                },
                new ReportItem(){
                SN=7,
                ReportId=9,
                Report=loc.Localize("DailyMissingPunchesReport"),
                //Description="Report on missing punches on selected period"
                Description=loc.Localize("DailyMissingPunchesReportDesc"),
                ActionCode="5007"
                },
                new ReportItem(){
                SN=8,
                ReportId=10,
                Report=loc.Localize("MonthlyAttendancereport"),
                //Description="Monthly report on attendance for a selected period"
                Description=loc.Localize("MonthlyAttendancereportDesc"),
                ActionCode="5008"
                },
                  new ReportItem(){
                SN=11,
                ReportId=16,
                Report=loc.Localize("MonthlyEmployeeSummaryReport"),
                //Description="Employee Summary report for selected period."
                Description=loc.Localize("MonthlyEmployeeSummaryReportDesc"),
                ActionCode="5009"
                },
                     new ReportItem(){
                SN=12,
                ReportId=11,
                Report=loc.Localize("DailyEmployeeLeaveReport"),
                //Description="Employee Daily Leave report for selected period."
                Description=loc.Localize("DailyEmployeeLeaveReportDesc"),
                ActionCode="5010"
                },
                 new ReportItem(){
                SN=13,
                ReportId=13,
                Report=loc.Localize("MonthlyAttendanceStatistic"),
                Description=loc.Localize("MonthlyAttendanceStatistic"),
                ActionCode="5010"
                },
            };

            return Reports;
        }

        internal static T GetObjFromQueryString<T>() where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (Type.GetType(property.PropertyType.ToString()) == typeof(List<>))
                {
                    //var lo=GetObjFromQueryString<Type.GetType(property.PropertyType.ToString())>
                }
                else
                {
                    var valueAsString = HttpContext.Current.Request.QueryString[property.Name];

                    var value = Convert.ChangeType(valueAsString, Type.GetType(property.PropertyType.ToString()));

                    if (value == null)
                        continue;

                    property.SetValue(obj, value, null);
                }

            }
            return obj;
        }

        internal static string GetNepaliUnicodeNumber(string field)
        {
            /*१ २ ३ ४ ५ ६ ७ ८ ९ ० */
            string result = "";
            for (int i = 0; i < field.Length; i++)
            {
                switch (field[i])
                {
                    case '0':
                        result = result + "०";
                        break;
                    case '1':
                        result = result + "१";
                        break;
                    case '2':
                        result = result + "२";
                        break;
                    case '3':
                        result = result + "३";
                        break;
                    case '4':
                        result = result + "४";
                        break;
                    case '5':
                        result = result + "५";
                        break;
                    case '6':
                        result = result + "६";
                        break;
                    case '7':
                        result = result + "७";
                        break;
                    case '8':
                        result = result + "८";
                        break;
                    case '9':
                        result = result + "९";
                        break;

                    default:
                        result = result + field[i];
                        break;
                }
            }
            return result;
        }

        internal static void AddAuditTrail(string menuCode, string actionCode, DateTime systemTime, int userId, int targetId, string message)
        {
            //SAuditTrial services = new SAuditTrial(menuCode, actionCode, systemTime, userId, targetId, message);
            //services.Add();
            SAuditTrial services = new SAuditTrial();
            services.Add(menuCode, actionCode, systemTime, userId, targetId, message);
        }


        public static ServiceResult<int[]> GetEmpIdsForReportParam(string deps = null, string secs = null, string emps = null)
        {
            DataVisibilityLevel dataVisibilityLevel = (DataVisibilityLevel)RiddhaSession.DataVisibilityLevel;
            ServiceResult<int[]> result = new ServiceResult<int[]>();
            if (emps != null)
            {
                result.Data = Array.ConvertAll(emps.Split(','), s => int.Parse(s));
                return result;
            }
            SEmployee empService = new SEmployee();
            var empQuery = empService.List().Data;
            if (deps == null)
            {

                switch (dataVisibilityLevel)
                {
                    case DataVisibilityLevel.Self:
                        result.Data = (from c in empQuery where c.Id == RiddhaSession.EmployeeId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.Section:
                        result.Data = (from c in empQuery where c.SectionId == RiddhaSession.SectionId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.Department:
                        result.Data = (from c in empQuery where c.Section.DepartmentId == RiddhaSession.DepartmentId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.Branch:
                        result.Data = (from c in empQuery where c.BranchId == RiddhaSession.BranchId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.All:
                        result.Data = (from c in empQuery where c.Branch.CompanyId == RiddhaSession.CompanyId select c.Id).ToArray();
                        return result;
                    default:
                        break;
                }
                result.Data = new int[] { };
                return result;
            }
            else
            {
                if (secs != null)
                {
                    int[] secIds = Array.ConvertAll(secs.Split(','), s => int.Parse(s));

                    switch (dataVisibilityLevel)
                    {
                        case DataVisibilityLevel.Self:
                            result.Data = (from c in empQuery
                                           where c.Id == RiddhaSession.EmployeeId
                                           select c.Id).ToArray();
                            break;
                        case DataVisibilityLevel.Section:
                        case DataVisibilityLevel.Department:
                        case DataVisibilityLevel.Branch:
                        case DataVisibilityLevel.All:
                            result.Data = (from c in empQuery
                                           join d in secIds on c.SectionId equals d
                                           select c.Id).ToArray();
                            break;
                        default:
                            break;
                    }
                    return result;
                }
                else
                {
                    int[] depIds = Array.ConvertAll(deps.Split(','), s => int.Parse(s));
                    switch (dataVisibilityLevel)
                    {
                        case DataVisibilityLevel.Self:
                            result.Data = (from c in empQuery
                                           where c.Id == RiddhaSession.EmployeeId
                                           select c.Id).ToArray();
                            break;
                        case DataVisibilityLevel.Section:
                            result.Data = (from c in empQuery.Where(x => x.SectionId == RiddhaSession.SectionId)
                                           join d in depIds on c.Section.DepartmentId equals d
                                           select c.Id).ToArray();
                            break;
                        case DataVisibilityLevel.Department:
                        case DataVisibilityLevel.Branch:
                        case DataVisibilityLevel.All:
                            result.Data = (from c in empQuery
                                           join d in depIds on c.Section.DepartmentId equals d
                                           select c.Id).ToArray();
                            break;
                        default:
                            break;
                    }
                    return result;
                }
            }
        }
        public static ServiceResult<int[]> GetFilteredEmployeeIDs(string deps = null, string secs = null, string emps = null)
        {
            DataVisibilityLevel dataVisibilityLevel = (DataVisibilityLevel)RiddhaSession.DataVisibilityLevel;
            ServiceResult<int[]> result = new ServiceResult<int[]>();
            if (emps != null)
            {
                result.Data = Array.ConvertAll(emps.Split(','), s => int.Parse(s));
                return result;
            }
            SEmployee empService = new SEmployee();
            var empQuery = empService.List().Data;
            if (deps == null)
            {

                switch (dataVisibilityLevel)
                {
                    case DataVisibilityLevel.Self:
                        result.Data = (from c in empQuery where c.Id == RiddhaSession.EmployeeId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.Section:
                        result.Data = (from c in empQuery where c.SectionId == RiddhaSession.SectionId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.Department:
                        result.Data = (from c in empQuery where c.Section.DepartmentId == RiddhaSession.DepartmentId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.Branch:
                        result.Data = (from c in empQuery where c.BranchId == RiddhaSession.BranchId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.All:
                        result.Data = (from c in empQuery where c.Branch.CompanyId == RiddhaSession.CompanyId select c.Id).ToArray();
                        return result;
                    default:
                        break;
                }
                result.Data = new int[] { };
                return result;
            }
            else
            {
                if (secs != null)
                {
                    int[] secIds = Array.ConvertAll(secs.Split(','), s => int.Parse(s));

                    switch (dataVisibilityLevel)
                    {
                        case DataVisibilityLevel.Self:
                            result.Data = (from c in empQuery
                                           where c.Id == RiddhaSession.EmployeeId
                                           select c.Id).ToArray();
                            break;
                        case DataVisibilityLevel.Section:
                        case DataVisibilityLevel.Department:
                        case DataVisibilityLevel.Branch:
                        case DataVisibilityLevel.All:
                            result.Data = (from c in empQuery
                                           join d in secIds on c.SectionId equals d
                                           select c.Id).ToArray();
                            break;
                        default:
                            break;
                    }
                    return result;
                }
                else
                {
                    int[] depIds = Array.ConvertAll(deps.Split(','), s => int.Parse(s));
                    switch (dataVisibilityLevel)
                    {
                        case DataVisibilityLevel.Self:
                            result.Data = (from c in empQuery
                                           where c.Id == RiddhaSession.EmployeeId
                                           select c.Id).ToArray();
                            break;
                        case DataVisibilityLevel.Section:
                            result.Data = (from c in empQuery.Where(x => x.SectionId == RiddhaSession.SectionId)
                                           join d in depIds on c.Section.DepartmentId equals d
                                           select c.Id).ToArray();
                            break;
                        case DataVisibilityLevel.Department:
                        case DataVisibilityLevel.Branch:
                        case DataVisibilityLevel.All:
                            result.Data = (from c in empQuery
                                           join d in depIds on c.Section.DepartmentId equals d
                                           select c.Id).ToArray();
                            break;
                        default:
                            break;
                    }
                    return result;
                }
            }
        }
        public static ServiceResult<List<EEmployee>> GetEmployees()
        {
            DataVisibilityLevel dataVisibilityLevel = (DataVisibilityLevel)RiddhaSession.DataVisibilityLevel;
            ServiceResult<List<EEmployee>> result = new ServiceResult<List<EEmployee>>();
            SBranch branchServices = new SBranch();
            var branch = branchServices.List().Data.Where(x => x.Id == RiddhaSession.BranchId).FirstOrDefault();
            if (!branch.IsHeadOffice)
            {
                dataVisibilityLevel = DataVisibilityLevel.Branch;
            }
            SEmployee empService = new SEmployee();
            var empQuery = empService.List().Data;
            switch (dataVisibilityLevel)
            {
                case DataVisibilityLevel.Self:
                    result.Data = (from c in empQuery where c.Id == RiddhaSession.EmployeeId select c).ToList();
                    return result;
                case DataVisibilityLevel.Section:
                    result.Data = (from c in empQuery where c.SectionId == RiddhaSession.SectionId select c).ToList();
                    return result;
                case DataVisibilityLevel.Department:
                    result.Data = (from c in empQuery where c.Section.DepartmentId == RiddhaSession.DepartmentId select c).ToList();
                    return result;
                case DataVisibilityLevel.Branch:
                    result.Data = (from c in empQuery where c.BranchId == RiddhaSession.BranchId select c).ToList();
                    return result;
                case DataVisibilityLevel.All:
                    result.Data = (from c in empQuery where c.Branch.CompanyId == RiddhaSession.CompanyId select c).ToList();
                    return result;
                case DataVisibilityLevel.ReportingHierarchy:
                    var query = (from c in empQuery where c.Branch.CompanyId == RiddhaSession.CompanyId select c);
                    result.Data = new List<EEmployee>();
                    result.Data.Add(empQuery.Where(x => x.Id == RiddhaSession.EmployeeId).FirstOrDefault());
                    AddChildEmployee(query, RiddhaSession.EmployeeId, result.Data);
                    return result;
                default:
                    result.Data = new List<EEmployee>();
                    return result;
            }
        }

        private static void AddChildEmployee(IQueryable<EEmployee> query, int employeeId, List<EEmployee> employees)
        {
            var emps = query.Where(x => x.ReportingManagerId == employeeId).ToList();
            foreach (var emp in emps)
            {
                employees.Add(emp);
                AddChildEmployee(query, emp.Id, employees);
            }

        }



        public static int[] intToArray(int n)
        {
            if (n == 0) return new int[1] { 0 };
            var digits = new List<int>();
            digits.Add(n);
            var arr = digits.ToArray();
            Array.Reverse(arr);
            return arr;
        }
    }
    public class ReportItem
    {
        public int SN { get; set; }
        public int ReportId { get; set; }
        public string Report { get; set; }
        public string Description { get; set; }
        public string ActionCode { get; set; }
    }


    public static class WDMS
    {
        public static void CopyDeviceInfo()
        {
            try
            {
                WdmsData.WdmsEntities wdmsdb = new WdmsData.WdmsEntities();
                // var devices = wdmsdb.iclock.Where(x => x.company_id == null).ToList();
                var devices = (from c in wdmsdb.iclock
                               select new
                               {
                                   SN = c.SN,
                                   IPAddress = c.IPAddress
                               }
                               ).ToList();

                Riddhasoft.DB.RiddhaDBContext hrmpldb = new Riddhasoft.DB.RiddhaDBContext();
                SDevice deviceServices = new SDevice();
                var existingDevices = deviceServices.List().Data.ToList();
                int newDeviceCount = 0;
                if (devices.Count > 0)
                    foreach (var item in devices)
                    {
                        var isNewDevice = existingDevices.Where(x => x.SerialNumber == item.SN).Count() == 0;
                        if (isNewDevice)
                        {
                            newDeviceCount++;
                            hrmpldb.Device.Add(new Riddhasoft.Device.Entities.EDevice()
                            {
                                SerialNumber = item.SN,
                                ModelId = null,
                                Status = Riddhasoft.Device.Entities.Status.New,
                                DeviceType = Riddhasoft.Device.Entities.DeviceType.ADMS,
                                IpAddress = item.IPAddress
                            });
                        }
                    }
                if (newDeviceCount > 0)
                    hrmpldb.SaveChanges();
            }
            catch (Exception)
            {

                //should use log for net

            }

        }

        public static void CopyDeviceLog()
        {
            //WdmsData.WdmsEntities wdmsdb = new WdmsData.WdmsEntities();
            //SDevice deviceServices = new SDevice();
            //SEmployee employeeServices = new SEmployee();
            //var existingDevices = deviceServices.List().Data.Where(x => x.Status == Riddhasoft.Device.Entities.Status.Customer).ToList();
            //foreach (var item in existingDevices)
            //{
            //    // var lastActivity = DateTime.Parse((item.LastActivity).ToString());
            //    var glogFromWdms = (from c in wdmsdb.checkinout.Where(x => x.SN == item.SerialNumber && x.id > item.CheckInOutIndex)
            //                        select new
            //                        {
            //                            Id = c.id,
            //                            CheckTime = c.checktime,
            //                            UserId = c.userid,
            //                            Cnt = c.verifycode
            //                        }
            //                            ).ToList();
            //    item.LastActivity = System.DateTime.Now;


            //    SAttendanceLog logService = new SAttendanceLog();
            //    SCompanyDeviceAssignment comDevAssSer = new SCompanyDeviceAssignment();
            //    foreach (var gfwems in glogFromWdms)
            //    {

            //        //TODO: Check again Device Log Data 

            //        //tranDate.AddTicks(obj[1].ToTimeSpan().Ticks);
            //        var companyDevice = comDevAssSer.List().Data.Where(x => x.DeviceId == item.Id).FirstOrDefault();
            //        var wdmsUserInfo = (from c in wdmsdb.userinfo.Where(x => x.userid == gfwems.UserId)
            //                            select new
            //                            {
            //                                badgeNumber = c.badgenumber,
            //                                userId = c.userid
            //                            }
            //                              ).ToList();


            //        if (wdmsUserInfo != null && wdmsUserInfo.Count > 0)
            //        {
            //            int badgeNumber = wdmsUserInfo.First().badgeNumber.ToInt();
            //            var employee = employeeServices.List().Data.Where(x => x.DeviceCode == badgeNumber && x.Branch.CompanyId == companyDevice.CompanyId).FirstOrDefault();
            //            if (employee != null)
            //            {
            //                logService.Add(new EAttendanceLog()
            //                {
            //                    CompanyCode = "",
            //                    DateTime = gfwems.CheckTime,
            //                    DeviceId = item.Id,
            //                    EmployeeId = employee.Id,
            //                    VerifyMode = gfwems.Cnt,
            //                });
            //            }
            //        }

            //        #region last glog index
            //        item.CheckInOutIndex = gfwems.Id;
            //        deviceServices.Update(item);
            //        #endregion
            //    }

            //}
        }


        public class Log
        {
            public static void Write(string text)
            {
                string txt = string.Format("CompanyId={0}, UserId={1}, time={2},message={3}", RiddhaSession.CompanyId, RiddhaSession.UserId, DateTime.Now, text) + Environment.NewLine;

                File.AppendAllText("Log.txt", txt);
            }
            public static void SytemLog(string text)
            {
                string txt = string.Format(" time={0},message={1}", DateTime.Now, text) + Environment.NewLine;
                try
                {
                    File.AppendAllText(System.Web.Hosting.HostingEnvironment.MapPath("\\Log.txt"), txt);
                }
                catch (Exception ex)
                {


                }

            }
        }
    }
}