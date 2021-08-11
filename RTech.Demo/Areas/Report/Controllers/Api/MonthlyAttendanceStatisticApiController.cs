using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class MonthlyAttendanceStatisticApiController : ApiController
    {
        SDateTable dateTableServices = null;
        SEmployee empServices = null;
        SMonthlyWiseReport reportServices = null;
        public MonthlyAttendanceStatisticApiController()
        {
            dateTableServices = new SDateTable();
            empServices = new SEmployee();
            reportServices = new SMonthlyWiseReport(RiddhaSession.Language);
        }
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            string opDate = RiddhaSession.OperationDate;
            string language = RiddhaSession.Language;
            DateTime fromDate;
            DateTime toDate;
            if (opDate == "ne")
            {
                var dates = dateTableServices.GetDaysInNepaliMonth(vm.Year, vm.MonthId);
                fromDate = dates.First().EngDate;
                toDate = dates.Last().EngDate;
            }
            else
            {
                var firstDayInEnglishMonth = dateTableServices.GetFirstDayInEnglishMonth(vm.Year, vm.MonthId);
                var lastDayInEnglishMonth = dateTableServices.GetLastDayInEnglishMonth(vm.Year, vm.MonthId);
                fromDate = firstDayInEnglishMonth.EngDate;
                toDate = lastDayInEnglishMonth.EngDate;
            }
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            int totalRecords = employees.Count();
            employees = employees.Skip(vm.Skip).Take(vm.Take).ToArray();
            reportServices.FilteredEmployeeIDs = employees;
            var result = reportServices.GetAttendanceReportFromSp(fromDate, toDate, RiddhaSession.BranchId.ToInt()).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            dynamic[] data = new dynamic[] { };
            if (employees.Count() > 0)
            {
                var reportData = (from c in result
                                  join d in employees on c.EmployeeId equals d
                                  select new
                                  {
                                      EmployeeName = c.EmployeeCode + "-" + c.EmployeeName,
                                      WorkDate = getWorkDate(c),
                                      Remark = getRemark(c.Remark.Substring(0, 1), c.ActualTimeIn, c.ActualTimeOut, vm.IncludePunchTime, c)
                                  }).AsEnumerable();
                data = reportData.ToPivotArray(x => x.WorkDate, x => x.EmployeeName, x => x.First().Remark);
            }
            else
            {
                var reportData = (from c in result
                                  select new
                                  {
                                      EmployeeName = c.EmployeeCode + "-" + c.EmployeeName,
                                      WorkDate = getWorkDate(c),
                                      Remark = getRemark(c.Remark.Substring(0, 1), c.ActualTimeIn, c.ActualTimeOut, vm.IncludePunchTime, c)
                                  }).AsEnumerable();
                data = reportData.ToPivotArray(x => x.WorkDate, x => x.EmployeeName, x => x.First().Remark);
            }

            return new KendoGridResult<object>()
            {
                Data = data,
                Status = ResultStatus.Ok,
                TotalCount = totalRecords
            };
        }

        private object getRemark(string remark, string inTime, string outTime, bool includePuchTime, MonthlyWiseReport c)
        {
            string result = c.Remark;

            #region generate short remarks

            if (c.NoPunch)
            {
                //return OnLeave == "Yes" ? "Leave" : Holiday == "Yes" ? "Holiday" : Weekend == "Yes" ? "Weekend" : "Present";
                result = c.OnLeave == "Yes" ? c.LeaveName : c.Holiday == "Yes" ? c.HolidayName : c.Weekend == "Yes" ? "W" : "P";
            }
            if (c.SinglePunch)
            {
                if (c.ActualTimeIn != "00:00")
                    result = "P";
                else
                {
                    //return OnLeave == "Yes" ? "Leave" : Holiday == "Yes" ? "Holiday" : Weekend == "Yes" ? "Weekend" : "Absent";
                    result = c.OnLeave == "Yes" ? c.LeaveName : c.Holiday == "Yes" ? c.HolidayName : c.Weekend == "Yes" ? "W" : "A";
                }
            }
            if (c.TwoPunch || c.FourPunch || c.MultiplePunch)
            {
                result = c.Actual.ToTimeSpan().Hours == 0 ? c.OnLeave == "Yes" ? c.LeaveName : c.Holiday == "Yes" ? c.HolidayName : c.Weekend == "Yes" ? "W" : c.Remark == "Misc" ? "M" : "A" : "P";
            }
            //result = c.Actual.ToTimeSpan().Hours == 0 ? c.OnLeave == "Yes" ? c.LeaveName : c.Holiday == "Yes" ? c.HolidayName : c.Weekend == "Yes" ? "W" : "A" : "P";

            if ((c.ActualTimeOut == "00:00" && c.ActualTimeIn != "00:00") && (c.TwoPunch || c.FourPunch || c.MultiplePunch))
            {
                result = "M";
            }
            if (c.OfficeVisit == "YES")
            {
                result = "OV";
            }
            if (c.Kaj == "YES")
            {
                result = "Kaj";
            }
            if (includePuchTime && (result == "M" || result == "P"))
            {
                if (remark == "P" || remark == "M")
                {
                    result = inTime + "-" + outTime;
                }
            }
            //return Actual.ToTimeSpan().Hours == 0 ? OnLeave == "Yes" ? "Leave" : Holiday == "Yes" ? "Holiday" : Weekend == "Yes" ? "Weekend" : "Absent" : "Present";
            return result;
            #endregion
        }

        private object getWorkDate(MonthlyWiseReport c)
        {
            switch (RiddhaSession.OperationDate)
            {
                case "en":
                    return "Day" + c.WorkDate.Substring(8, 2);
                case "ne":
                    return "Day" + c.NepDate.Substring(8, 2);
                default:
                    return "Day" + c.WorkDate.Substring(8, 2);
            }
        }

    }
}
