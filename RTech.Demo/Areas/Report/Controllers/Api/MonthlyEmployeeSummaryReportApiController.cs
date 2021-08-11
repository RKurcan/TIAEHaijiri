using Riddhasoft.Employee.Entities;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class MonthlyEmployeeSummaryReportApiController : ApiController
    {
        [HttpPost]
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int totalDays = (vm.ToDate.ToDateTime() - vm.OnDate.ToDateTime()).Days + 1;
            SMonthlyWiseReport reportService = new SMonthlyWiseReport(RiddhaSession.Language);
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            reportService.FilteredEmployeeIDs = employees;
            var result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt(), vm.OTV2).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            int maxCount = 0;
            List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();
            if (employees.Count() > 0)
            {
                maxCount = employees.Count();
                // employees = employees.Skip(vm.Skip).Take(vm.Take).ToArray();
                reportData = (from c in result
                              join d in employees on c.EmployeeId equals d
                              select c
                                 ).ToList();
            }
            else
            {
                reportData = result;
            }
             var summaryLst = (reportData.GroupBy(i => i.EmployeeId)
            .Select(i => new SummaryDaywiseReport()
            {
                EmployeeCode = i.FirstOrDefault().EmployeeCode,
                EmployeeName = i.FirstOrDefault().EmployeeName,
                DepartmentName = i.FirstOrDefault().DepartmentName,
                DepartmentNamee = i.FirstOrDefault().DepartmentNamee,
                TotalDays = totalDays.ToString(),
                Absent = i.Where(j => j.Remark.ToLower() == "absent").Count().ToString(),
                OfficeOut = i.Where(j => j.OfficeVisit.ToLower() == "yes").Count().ToString(),
                KajOut = i.Where(j => j.Kaj.ToLower() == "yes").Count().ToString(),
                Present = i.Where(j => j.Remark.ToLower() == "present").Count().ToString(),
                PresentInHoliday = i.Where(j => j.Remark.ToLower() == "present" && j.Holiday.ToLower() == "yes").Count().ToString(),
                PresentInDayOff = i.Where(j => j.Remark.ToLower() == "present" && j.Weekend.ToLower() == "yes").Count().ToString(),
                Misc = i.Where(j => j.Remark.ToLower() == "misc").Count().ToString(),
                Weekend = i.Where(j => j.Weekend.ToLower() == "yes").Count().ToString(),
                Holiday = i.Where(j => j.Holiday.ToLower() == "yes").Count().ToString(),
                //DutyDay = (totalDays - (i.Where(j => j.Remark.ToLower() == "weekend").Count() + i.Where(j => j.Holiday.ToLower() == "yes").Count())).ToString(),
                EarlyIn = i.Where(j => j.EarlyIn != "").Count().ToString(),
                EarlyOut = i.Where(j => j.EarlyOut != "").Count().ToString(),
                LateIn = i.Where(j => j.LateIn != "").Count().ToString(),
                LateOut = i.Where(j => j.LateOut != "").Count().ToString(),
                Leave = i.Where(j => j.OnLeave.ToLower() == "yes").Count().ToString(),
                Worked = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Actual.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //Ot = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Ot.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //Ot = new TimeSpan(i.Sum(r => r.Ot.ToTimeSpan().Ticks)).ToString(@"hh\:mm"),
                Ot = GetOt(i),
                Remarks = getRemarks(i.Where(j => j.OnLeave.ToLower() == "yes").ToList(), i.Where(j => j.Remark.ToLower() == "absent").ToList())
            })).ToList();
            return new KendoGridResult<object>()
            {
                Data = summaryLst,
                Status = ResultStatus.Ok,
                TotalCount = maxCount == 0 ? summaryLst.Count : maxCount
            };
        }

        private string GetOt(IGrouping<int, MonthlyWiseReport> i)
        {
            TimeSpan sumTillNowTimeSpan = TimeSpan.Zero;
            foreach (var item in i)
            {
                sumTillNowTimeSpan += item.Ot.ToTimeSpan();
            }
            double hours = sumTillNowTimeSpan.Hours + (sumTillNowTimeSpan.Days * 24);
            string time = hours + ":" + sumTillNowTimeSpan.Minutes;
            return time;
        }

        private string getRemarks(List<MonthlyWiseReport> leaveLst, List<MonthlyWiseReport> absentLst)
        {
            return joinStringForRemarks(leaveLst);
            //string absent = joinStringForRemarks(absentLst);
            //return "Leave:" + leave + "& Absent:" + absent;
        }
        private string joinStringForRemarks(List<MonthlyWiseReport> list)
        {
            if (list.Count() < 1)
            {
                return "";
            }
            string opDate = RiddhaSession.OperationDate;
            var dates = (from c in list
                         select c.WorkDate.ToDateTime()).ToList();
            List<DateRange> dateRanges = GetDateRanges(dates);
            IList<string> result = new List<string> { };
            foreach (var item in dateRanges)
            {
                if (opDate == "ne")
                {
                    string nepStartDate = list.Where(x => x.WorkDate.ToDateTime() == item.Start).FirstOrDefault().NepDate;
                    var startSplit = nepStartDate.Split('/');
                    if (item.Start == item.End)
                    {

                        result.Add(startSplit[2]);
                    }
                    else
                    {
                        string nepEndDate = list.Where(x => x.WorkDate.ToDateTime() == item.End).FirstOrDefault().NepDate;
                        var endSplit = nepEndDate.Split('/');
                        result.Add(startSplit[2] + "-" + endSplit[2]);
                    }
                }
                else
                {
                    if (item.Start == item.End)
                    {
                        result.Add(item.Start.Day.ToString());
                    }
                    else
                    {
                        result.Add(item.Start.Day.ToString() + "-" + item.End.Day.ToString());
                    }
                }

            }
            return string.Join(",", result);
        }
        private List<DateRange> GetDateRanges(List<DateTime> dates)
        {
            if (dates == null || !dates.Any()) return null;
            dates = dates.OrderBy(x => x.Date).ToList();

            var dateRangeList = new List<DateRange>();

            DateRange dateRange = null;
            for (var i = 0; i < dates.Count; i++)
            {
                if (dateRange == null)
                {
                    dateRange = new DateRange { Start = dates[i] };
                }
                if (i == dates.Count - 1 || dates[i].Date.AddDays(1) != dates[i + 1].Date)
                {
                    dateRange.End = dates[i].Date;
                    dateRangeList.Add(dateRange);
                    dateRange = null;
                }
            }
            return dateRangeList;
        }
        [HttpPost]
        public KendoGridResult<object> GenerateReportHourWise(KendoReportViewModel vm)
        {
            int totalDays = (vm.ToDate.ToDateTime() - vm.OnDate.ToDateTime()).Days + 1;
            SMonthlyWiseReport reportService = new SMonthlyWiseReport(RiddhaSession.Language);
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            reportService.FilteredEmployeeIDs = employees;
            int totalRecords = employees.Count();
            //employees = employees.Skip(vm.Skip).Take(vm.Take).ToArray();
            var result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt(), vm.OTV2).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();
            if (employees.Count() > 0)
            {
                reportData = (from c in result
                              join d in employees
                              on c.EmployeeId equals d
                              select c
                                 ).ToList();
            }
            else
            {
                reportData = result;
            }
            var summaryLst = (reportData.GroupBy(i => i.EmployeeId)
            .Select(i => new SummaryDaywiseReport()
            {
                Weekend = i.Where(j => j.Remark.ToLower() == "weekend").Count().ToString(),
                EmployeeCode = i.FirstOrDefault().EmployeeCode,
                EmployeeName = i.FirstOrDefault().EmployeeName,
                DepartmentName = i.FirstOrDefault().DepartmentName,
                DepartmentNamee = i.FirstOrDefault().DepartmentNamee,
                //ShiftWorkedTime = i.Where(x => x.Weekend.ToLower() == "no").Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Standard.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //Worked = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Actual.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //Ot = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Ot.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //LateIn = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.LateIn.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //EarlyOut = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.EarlyOut.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //EarlyIn = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.EarlyIn.ToTimeSpan())).TotalHours.ToString("#00.00"),
                //LateOut = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.LateOut.ToTimeSpan())).TotalHours.ToString("#00.00"),
                ShiftWorkedTime = GetTimeSpanToString(i.Where(x => x.Weekend.ToLower() == "no").Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Standard.ToTimeSpan()))),
                Worked = GetTimeSpanToString(i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Actual.ToTimeSpan()))),
                Ot = GetTimeSpanToString(i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Ot.ToTimeSpan()))),
                LateIn = GetTimeSpanToString(i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.LateIn.ToTimeSpan()))),
                EarlyOut = GetTimeSpanToString(i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.EarlyOut.ToTimeSpan()))),
                EarlyIn = GetTimeSpanToString(i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.EarlyIn.ToTimeSpan()))),
                LateOut = GetTimeSpanToString(i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.LateOut.ToTimeSpan()))),

            })).ToList();
            return new KendoGridResult<object>()
            {
                Data = summaryLst,
                Status = ResultStatus.Ok,
                TotalCount = totalRecords
            };
        }

        public string GetTimeSpanToString(TimeSpan timeSpan)
        {

            try
            {

                return (int)timeSpan.TotalHours + "." + timeSpan.Minutes;
            }
            catch (Exception)
            {

                return "00.00";
            }
            return "00.00";
        }
        [HttpPost]
        public KendoGridResult<object> GenerateReportLeaveWise(KendoReportViewModel vm)
        {
            int totalDays = (vm.ToDate.ToDateTime() - vm.OnDate.ToDateTime()).Days + 1;
            SMonthlyWiseReport reportService = new SMonthlyWiseReport(RiddhaSession.Language);
            int[] employees = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;
            reportService.FilteredEmployeeIDs = employees;
            var result = reportService.GetAttendanceReportFromSp(vm.OnDate.ToDateTime(), vm.ToDate.ToDateTime(), RiddhaSession.BranchId.ToInt(), vm.OTV2).Data;
            if (RiddhaSession.PackageId > 0 && vm.ActiveInactiveMode == 0)
            {
                result = result.Where(x => x.EmploymentStatus == EmploymentStatus.NormalEmployment || x.EmploymentStatus == EmploymentStatus.OnContract || x.EmploymentStatus == EmploymentStatus.PermanentJob || x.EmploymentStatus == EmploymentStatus.Retiring).ToList();
            }
            int maxCount = 0;
            List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();
            if (employees.Count() > 0)
            {
                maxCount = employees.Count();
                // employees = employees.Skip(vm.Skip).Take(vm.Take).ToArray();
                reportData = (from c in result
                              join d in employees
                              on c.EmployeeId equals d
                              select c
                                 ).ToList();
            }
            else
            {
                reportData = result;
            }
            var summaryLst = (reportData.GroupBy(i => i.EmployeeId)
            .Select(i => new SummaryDaywiseReport()
            {
                EmployeeCode = i.FirstOrDefault().EmployeeCode,
                EmployeeName = i.FirstOrDefault().EmployeeName,
                DepartmentName = i.FirstOrDefault().DepartmentName,
                DepartmentNamee = i.FirstOrDefault().DepartmentNamee,
                TotalDays = totalDays.ToString(),
                Absent = i.Where(j => j.Remark.ToLower() == "absent").Count().ToString(),
                OfficeOut = i.Where(j => j.OfficeVisit.ToLower() == "yes").Count().ToString(),
                KajOut = i.Where(j => j.Kaj.ToLower() == "yes").Count().ToString(),
                Present = i.Where(j => j.Remark.ToLower() == "present").Count().ToString(),
                PresentInHoliday = i.Where(j => j.Remark.ToLower() == "present" && j.Holiday.ToLower() == "yes").Count().ToString(),
                PresentInDayOff = i.Where(j => j.Remark.ToLower() == "present" && j.Weekend.ToLower() == "yes").Count().ToString(),
                Misc = i.Where(j => j.Remark.ToLower() == "misc").Count().ToString(),
                Weekend = i.Where(j => j.Weekend.ToLower() == "yes").Count().ToString(),
                //DutyDay = (totalDays - (i.Where(j => j.Remark.ToLower() == "weekend").Count() + i.Where(j => j.Holiday.ToLower() == "yes").Count())).ToString(),
                //EarlyIn = i.Where(j => j.EarlyIn != "00:00").Count().ToString(),
                //EarlyOut = i.Where(j => j.EarlyOut != "00:00").Count().ToString(),
                //LateIn = i.Where(j => j.LateIn != "00:00").Count().ToString(),
                //LateOut = i.Where(j => j.LateOut != "00:00").Count().ToString(),
                EarlyIn = i.Where(j => j.EarlyIn != "").Count().ToString(),
                EarlyOut = i.Where(j => j.EarlyOut != "").Count().ToString(),
                LateIn = i.Where(j => j.LateIn != "").Count().ToString(),
                LateOut = i.Where(j => j.LateOut != "").Count().ToString(),
                Holiday = i.Where(j => j.Holiday.ToLower() == "yes").Count().ToString(),
                Leave = i.Where(j => j.OnLeave.ToLower() == "yes").Count().ToString(),
                Worked = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Actual.ToTimeSpan())).TotalHours.ToString("#00.00"),
                Ot = i.Aggregate(new TimeSpan(), (sum, nextData) => sum.Add(nextData.Ot.ToTimeSpan())).TotalHours.ToString("#00.00"),
                Remarks = getRemarks(i.Where(j => j.OnLeave.ToLower() == "yes").ToList(), i.Where(j => j.Remark.ToLower() == "absent").ToList())
            })).ToList();
            var dt = summaryLst.ToDataTable();
            SLeaveMaster leaveMasterServices = new SLeaveMaster();
            var leaveMastObj = leaveMasterServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            foreach (var leav in leaveMastObj)
            {
                dt.Columns.Add(leav.Code);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                foreach (var leav in leaveMastObj)
                {
                    var leaveCode = leav.Code;
                    var leaveCodeAndName = leav.Code + "-" + leav.Name;
                    string EmployeeCode = dt.Rows[i]["EmployeeCode"].ToString();
                    //var value = reportData.Count(x => x.EmployeeCode == EmployeeCode && x.LeaveName == leaveCode);
                    var value = reportData.Where(x => x.EmployeeCode == EmployeeCode && x.LeaveName == leaveCodeAndName).FirstOrDefault();
                    if (value != null)
                    {
                        var count = value.LeaveName.Split('-')[0].Count();
                        dt.Rows[i][leaveCode] = count;
                    }
                }
            }
            var finalList = dt.AsDynamicEnumerable();
            finalList = dt.ToDynamicList();
            return new KendoGridResult<object>()
            {
                Data = finalList,
                Status = ResultStatus.Ok,
                TotalCount = maxCount == 0 ? summaryLst.Count : maxCount
            };
        }
    }



    public class SummaryDaywiseReport
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNamee { get; set; }
        public string TotalDays { get; set; }
        public string Weekend { get; set; }
        public string Holiday { get; set; }
        //public string DutyDay { get; set; }
        public string DutyDay
        {
            get
            {

                int totalDays = TotalDays.ToInt();
                int weekendCount = Weekend.ToInt();
                int holidayCount = Holiday.ToInt();
                int result = totalDays - (weekendCount + holidayCount);
                return result.ToString();

            }
        }
        public string Present { get; set; }
        public string Absent { get; set; }
        public string Leave { get; set; }
        public string OfficeOut { get; set; }
        public string KajOut { get; set; }
        public string PresentInHoliday { get; set; }
        public string PresentInDayOff { get; set; }
        public string Worked { get; set; }
        public string Ot { get; set; }
        public string LateIn { get; set; }
        public string EarlyOut { get; set; }
        public string EarlyIn { get; set; }
        public string LateOut { get; set; }
        public string ShiftWorkedTime { get; set; }
        public string Remarks { get; set; }
        public string Misc { get; set; }
    }
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
