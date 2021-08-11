using Riddhasoft.Globals.Conversion;
using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report.ReportViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddhasoft.Services.Common;


namespace Riddhasoft.EHajiriPro.Desktop.Areas.Report
{
    public partial class MonthlyAttendanceReportDailougeFrm : Form
    {
        private int branchId = RiddhaSession.BranchId;
        private int reportid = 0;
        string ReportPath;
        string reportTitle;
        public List<CheckBoxListModel> SelectedDepartMentCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedSectionCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedEmployeeCheckBoxList { get; set; }
        public MonthlyAttendanceReportDailougeFrm()
        {
            InitializeComponent();

            populateYear();
            this.Text = reportTitle;
            SelectedDepartMentCheckBoxList = new List<CheckBoxListModel>();
            SelectedSectionCheckBoxList = new List<CheckBoxListModel>();
            SelectedEmployeeCheckBoxList = new List<CheckBoxListModel>();
            populateDepartment();
        }

        private void MonthlyAttendanceReportDailougeFrm_Load(object sender, EventArgs e)
        {
        }
        #region Checkbox work
        private void populateDepartment()
        {
            SDepartment services = new SDepartment();
            var data = services.List().Data.Where(x => x.BranchId == branchId).ToList();
            var checkBoxListModels = (from d in data
                                      select new CheckBoxListModel()
                                      {
                                          Id = d.Id,
                                          Name = d.Name,
                                          IsChecked = false
                                      }).ToList();
            departmentChkboxlist.BindData(checkBoxListModels);
        }
        private void GetCheckedDepartment(CheckBoxListModel CheckBoxListModel)
        {
            SelectedDepartMentCheckBoxList.Add(CheckBoxListModel);

        }
        private void GetCheckedSection(CheckBoxListModel CheckBoxListModel)
        {

            SelectedSectionCheckBoxList.Add(CheckBoxListModel);

        }
        private void GetCheckedEmployee(CheckBoxListModel checkedListBoxObject)
        {
            SelectedEmployeeCheckBoxList.Add(checkedListBoxObject);
        }
        private void populateSection()
        {
            //now we have to get checked department
            #region Get All Selected Departments
            //
            SelectedDepartMentCheckBoxList.Clear();
            departmentChkboxlist.DoThisForCheckedItem(GetCheckedDepartment);
            #endregion

            SSection service = new SSection();
            var data = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                        join d in SelectedDepartMentCheckBoxList
                        on c.DepartmentId equals d.Id
                        select c
                            ).ToList();

            var checkBoxListModels = (from d in data
                                      select new CheckBoxListModel()
                                      {
                                          Id = d.Id,
                                          Name = d.Name,
                                          IsChecked = false

                                      }).ToList();
            SectionChkboxlist.BindData(checkBoxListModels);
        }
        private void populateEmployee()
        {
            #region Get All Selected Section
            SelectedSectionCheckBoxList.Clear();
            SectionChkboxlist.DoThisForCheckedItem(GetCheckedSection);
            #endregion
            SEmployee services = new SEmployee();
            var data = (from c in services.List().Data.Where(x => x.BranchId == branchId).ToList()
                        join d in SelectedSectionCheckBoxList
                        on c.SectionId equals d.Id
                        select c
                        );
            var checkBoxListModels = (from d in data
                                      select new CheckBoxListModel()
                                      {
                                          Id = d.Id,
                                          Name = d.Name,
                                          IsChecked = false
                                      }).ToList();
            EmployeeChkBoxList.BindData(checkBoxListModels);
        }
        private void DepartmentSelectAllChkBox_CheckedChanged(object sender, EventArgs e)
        {
            checkAllDepartment();
            populateSection();
        }
        private void sectionSelectAllChkBox_CheckedChanged(object sender, EventArgs e)
        {
            checkAllSection();
            populateEmployee();
        }
        private void checkAllSection()
        {
            SectionChkboxlist.CheckAll(sectionSelectAllChkBox.Checked);

        }
        private void checkAllEmployee()
        {
            EmployeeChkBoxList.CheckAll(EmployeeSelectAllChkBox.Checked);
            #region Get All Selected Section
            //
            SelectedEmployeeCheckBoxList.Clear();
            EmployeeChkBoxList.DoThisForCheckedItem(GetCheckedEmployee);
            #endregion

        }
        private void checkAllDepartment()
        {
            departmentChkboxlist.CheckAll(DepartmentSelectAllChkBox.Checked);
        }
        private void EmployeeSelectAllChkBox_CheckedChanged(object sender, EventArgs e)
        {
            checkAllEmployee();
        }
        private void departmentChkboxlist_SelectedValueChanged(object sender, EventArgs e)
        {
            populateSection();
        }
        private void SectionChkboxlist_SelectedValueChanged(object sender, EventArgs e)
        {
            populateEmployee();
        }
        private void EmployeeChkBoxList_SelectedValueChanged(object sender, EventArgs e)
        {

        }
        #endregion

        private void populateYear()
        {
            DateTime date = DateTime.Now;
            if (GlobalParam.OperationDate == OperationDate.English)
            {
                yearTxt.Text = date.Year.ToString();
                cmbEnglishMonth.Visible = true;
                cmbEnglishMonth.SelectedIndex = DateTime.Now.Month - 1;

                var startDate = new DateTime(date.Year, date.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                FromMtb.Text = startDate.ToString("yyyy/MM/dd");
                toDateMtb.Text = endDate.ToString("yyyy/MM/dd");
            }
            else
            {
                SDateTable dateTableServices = new SDateTable();
                string Date = NepaliDateExtension.ToNepaliDate(DateTime.Now);
                string[] dateArray = Date.Split('/');
                yearTxt.Text = dateArray[0];
                int month = int.Parse(dateArray[1]);

                cmbNepaliMonth.Visible = true;
                cmbNepaliMonth.SelectedIndex = month - 1;


                var startDate = dateTableServices.GetFirstDayInNepaliMonth(int.Parse(dateArray[0]), int.Parse(dateArray[1]));
                string endDate = dateTableServices.GetLastDayInNepaliMonth(int.Parse(dateArray[0]), int.Parse(dateArray[1]));

                FromMtb.Text = startDate.NepDate;
                toDateMtb.Text = endDate;
            }
        }


        private void btnGenerate_Click(object sender, EventArgs e)
        {
            SMonthlyWiseReport reportService = new SMonthlyWiseReport(RiddhaSession.Language);
            List<MonthlyWiseReport> reportData = new List<MonthlyWiseReport>();

            DateTime FromDate = new DateTime();
            DateTime ToDate = new DateTime();

            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    FromDate = (FromMtb.Text).ToDateTime();
                    ToDate = (toDateMtb.Text).ToDateTime();
                    break;
                case OperationDate.Nepali:
                    FromDate = (FromMtb.Text).ToEnglishDate();
                    ToDate = (toDateMtb.Text).ToEnglishDate();
                    break;
                default:
                    break;
            }
            SelectedEmployeeCheckBoxList.Clear();
            EmployeeChkBoxList.DoThisForCheckedItem(GetCheckedEmployee);
            int[] employees = (from c in SelectedEmployeeCheckBoxList
                               select c.Id
                               ).ToArray();
            if (employees.Count() > 0)
            {
                reportService.FilteredEmployeeIDs = employees;
            }
            else
            {
                SEmployee empServices = new SEmployee();
                int[] emp = empServices.List().Data.Select(x => x.Id).ToArray();
                reportService.FilteredEmployeeIDs = emp;
            }
            if (rbtnAttendance.Checked)
            {
                var result = reportService.GetAttendanceReportFromSp(FromDate, ToDate, RiddhaSession.BranchId).Data;
                if (employees.Count() > 0)
                {


                    reportData = orderedList((from c in result
                                              join d in employees
                                              on c.EmployeeId equals d
                                              select c
                                      ), chkOrderByCode.Checked, chkOrderByName.Checked);
                }
                else
                {
                    reportData = orderedList(result, chkOrderByCode.Checked, chkOrderByName.Checked);
                }
                if (RiddhaSession.OpreationDate == "ne")
                {
                    SDateTable config = new SDateTable();
                    foreach (var item in reportData)
                    {
                        item.WorkDate = config.ConverToNepDateForDesktop(item.WorkDate.ToDateTime());
                    }
                }
                ReportPath = @"Report\MonthlyWiseReport.rdlc";
                reportTitle = string.Format("Monthly Attendance Report As On {0}", FromMtb.Text);
                ReportViewerFrm rviewer = new ReportViewerFrm(ReportPath, "DataSet1", reportData.ToDataTable<MonthlyWiseReport>())
                {
                    CompanyName = RiddhaSession.CompanyName,
                    ReportTitle = reportTitle
                };
                rviewer.ShowDialog();
            }
            else if (rbtnSummary.Checked)
            {
                int totalDays = (ToDate - FromDate).Days + 1;

                var result = reportService.GetAttendanceReportFromSp(FromDate, ToDate, RiddhaSession.BranchId.ToInt()).Data.OrderBy(x => x.EmployeeName).ToList();
                int maxCount = 0;
                if (employees.Count() > 0)
                {
                    maxCount = employees.Count();
                    reportData = (from c in result
                                  join d in employees on c.EmployeeId equals d
                                  select c
                                     ).ToList();
                }
                else
                {
                    reportData = result;
                }
                int test = reportData.Where(x => x.Weekend.ToLower() == "yes").ToList().Count();
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
                    Present = i.Where(j => j.Remark.ToLower() == "present").Count().ToString(),
                    PresentInHoliday = i.Where(j => j.Remark.ToLower() == "present" && j.Holiday.ToLower() == "yes").Count().ToString(),
                    PresentInDayOff = i.Where(j => j.Remark.ToLower() == "present" && j.Weekend.ToLower() == "yes").Count().ToString(),
                    Misc = i.Where(j => j.Remark.ToLower() == "misc").Count().ToString(),
                    Weekend = i.Where(j => j.Weekend.ToLower() == "yes").Count().ToString(),
                    DutyDay = (totalDays - (i.Where(j => j.Weekend.ToLower() == "yes").Count() + i.Where(j => j.Holiday.ToLower() == "yes").Count())).ToString(),
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
                ReportPath = @"Report\MonthlyEmployeeSummaryReport.rdlc";
                reportTitle = string.Format("Monthly Summary Attendance Report As On {0}", FromMtb.Text);
                ReportViewerFrm rviewer = new ReportViewerFrm(ReportPath, "DataSet1", summaryLst.ToDataTable<SummaryDaywiseReport>())
                {
                    CompanyName = RiddhaSession.CompanyName,
                    ReportTitle = reportTitle
                };
                rviewer.ShowDialog();
            }
            else
            {
                bool IncludePunchTime = false;
                SDateTable dateTableServices = new SDateTable();
                string opDate = RiddhaSession.OpreationDate;
                DateTime fromDate;
                DateTime toDate;
                DateTime StartDate;
                DateTime EndDate;
                if (opDate == "ne")
                {
                    var dates = dateTableServices.GetDaysInNepaliMonth(yearTxt.Text.ToInt(), cmbNepaliMonth.SelectedIndex);
                    fromDate = dates.First().EngDate;
                    toDate = dates.Last().EngDate;
                    StartDate = FromMtb.Text.ToEnglishDate();
                    EndDate = toDateMtb.Text.ToEnglishDate();
                }
                else
                {
                    var firstDayInEnglishMonth = dateTableServices.GetFirstDayInEnglishMonth(yearTxt.Text.ToInt(), cmbEnglishMonth.SelectedIndex);
                    var lastDayInEnglishMonth = dateTableServices.GetLastDayInEnglishMonth(yearTxt.Text.ToInt(), cmbEnglishMonth.SelectedIndex);
                    fromDate = firstDayInEnglishMonth.EngDate;
                    toDate = lastDayInEnglishMonth.EngDate;
                    StartDate = FromMtb.Text.ToDateTime();
                    EndDate = toDateMtb.Text.ToDateTime();
                }


                var result = reportService.GetAttendanceReportFromSp(StartDate, EndDate, RiddhaSession.BranchId.ToInt()).Data.OrderBy(x => x.EmployeeName).ToList();
                ReportPath = @"Report\MonthlyEmployeeTimeSHeet.rdlc";
                reportTitle = string.Format("Monthly Attendance Statistic Report As On {0}", FromMtb.Text);
                switch (GlobalParam.OperationDate)
                {
                    case OperationDate.English:
                        foreach (var item in result)
                        {
                            string[] array = item.WorkDate.Split('/');
                            item.WorkDate = array[2];
                        }
                        break;
                    case OperationDate.Nepali:
                        foreach (var item in result)
                        {
                            DateTime date = DateTime.Parse(item.WorkDate);
                            string nepDate = date.ToNepaliDate();
                            string[] array = nepDate.Split('/');
                            item.WorkDate = array[2];
                        }
                        break;
                    default:
                        break;
                }
                ReportViewerFrm rviewer = new ReportViewerFrm(ReportPath, "DataSet1", result.ToDataTable<MonthlyWiseReport>())
                {
                    CompanyName = RiddhaSession.CompanyName,
                    ReportTitle = reportTitle
                };
                rviewer.ShowDialog();
            }
        }

        private List<MonthlyWiseReport> orderedList(IEnumerable<MonthlyWiseReport> enumerable, bool code, bool name)
        {
            if (code)
            {
                if (name)
                {
                    return enumerable.OrderBy(x => x.EmployeeCode).ThenBy(x => x.EmployeeName).ToList();
                }

            }
            if (name)
            {
                return enumerable.OrderBy(x => x.EmployeeName).ToList();
            }
            return enumerable.OrderBy(x => x.EmployeeCode).ToList();

        }

        private string getRemarks(List<MonthlyWiseReport> leaveLst, List<MonthlyWiseReport> absentLst)
        {
            return joinStringForRemarks(leaveLst);
        }
        private string joinStringForRemarks(List<MonthlyWiseReport> list)
        {
            if (list.Count() < 1)
            {
                return "";
            }
            string opDate = RiddhaSession.OpreationDate;
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
            //result = c.Actual.ToTimeSpan().Hours == 0 ? c.OnLeave == "Yes" ? c.LeaveName : c.Holiday == "Yes" ? c.HolidayName : c.Weekend == "Yes" ? "W" : "A" : "P";
            result = c.Actual.ToTimeSpan().Hours == 0 ? c.OnLeave == "Yes" ? c.LeaveName : c.Holiday == "Yes" ? c.HolidayName : c.Weekend == "Yes" ? "W" : c.Remark == "Misc" ? "M" : "A" : "P";
            if (c.ActualTimeOut == "00:00" && c.ActualTimeIn != "00:00")
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
            switch (RiddhaSession.OpreationDate)
            {
                case "en":
                    return "Day" + c.WorkDate.Substring(8, 2);
                case "ne":
                    return "Day" + c.NepDate.Substring(8, 2);
                default:
                    return "Day" + c.WorkDate.Substring(8, 2);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            EmployeeSelectAllChkBox.Checked = false;
            sectionSelectAllChkBox.Checked = false;
            DepartmentSelectAllChkBox.Checked = false;
        }

        private void cmbNepaliMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            SDateTable dateTableServices = new SDateTable();
            int year = yearTxt.Text.ToInt();
            int month = cmbNepaliMonth.SelectedIndex + 1;

            var startDate = dateTableServices.GetFirstDayInNepaliMonth(year, month);
            var endDate = dateTableServices.GetLastDayInNepaliMonth(year, month);

            FromMtb.Text = startDate.NepDate;
            toDateMtb.Text = endDate;
        }

        private void cmbEnglishMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            SDateTable dateTableServices = new SDateTable();
            int year = yearTxt.Text.ToInt();
            int month = cmbEnglishMonth.SelectedIndex + 1;

            var startDate = dateTableServices.GetFirstDayInEnglishMonth(year, month);
            var endDate = dateTableServices.GetLastDayInEnglishMonth(year, month);

            FromMtb.Text = startDate.NepDate;
            toDateMtb.Text = endDate.EngDate.ToString("yyyy/MM/dd");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    public class MonthlyStatic
    {
        public string EmployeeName { get; set; }
        public string WorkDate { get; set; }
        public string Remark { get; set; }
    }
    public class SummaryDaywiseReport
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNamee { get; set; }
        public string TotalDays { get; set; }
        public string DutyDay { get; set; }
        public string Weekend { get; set; }
        public string Holiday { get; set; }
        public string Present { get; set; }
        public string Absent { get; set; }
        public string Leave { get; set; }
        public string OfficeOut { get; set; }
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
