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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddhasoft.Globals.Conversion;
using Riddhasoft.Services.Common;
namespace Riddhasoft.EHajiriPro.Desktop.Areas.Report
{
    public partial class DailyAttendanceReportDailougeFrm : Form
    {
        int branchId = RiddhaSession.BranchId;
        public List<CheckBoxListModel> SelectedDepartMentCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedSectionCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedEmployeeCheckBoxList { get; set; }
        private int reportid = 0;
        string ReportPath;
        string reportTitle;
        public DailyAttendanceReportDailougeFrm()
        {
            InitializeComponent();
            this.Text = reportTitle;
            SelectedDepartMentCheckBoxList = new List<CheckBoxListModel>();
            SelectedSectionCheckBoxList = new List<CheckBoxListModel>();
            SelectedEmployeeCheckBoxList = new List<CheckBoxListModel>();
            populateDepartment();
        }

        private void DailyAttendanceReportDailougeFrm_Load(object sender, EventArgs e)
        {
            if (GlobalParam.OperationDate == OperationDate.English)
            {
                FromMtb.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
            else
            {
                FromMtb.Text = NepaliDateExtension.ToNepaliDate(DateTime.Now);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            #region Get All Selected Section
            SelectedEmployeeCheckBoxList.Clear();
            EmployeeChkBoxList.DoThisForCheckedItem(GetCheckedEmployee);
            #endregion
        }
        #endregion

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            List<AttendanceReportDetailViewModel> reportData = new List<AttendanceReportDetailViewModel>();
            SDailyEmployeePerformanceReport dailyEmployeePerformanceReportService = new SDailyEmployeePerformanceReport(RiddhaSession.BranchId.ToInt(), RiddhaSession.fiscalYearId, RiddhaSession.Language);
            SDailyLateInReport dailyLateInReportServices = new SDailyLateInReport();

            SelectedEmployeeCheckBoxList.Clear();
            EmployeeChkBoxList.DoThisForCheckedItem(GetCheckedEmployee);
            int[] employees = (from c in SelectedEmployeeCheckBoxList
                               select c.Id
                               ).ToArray();

            DateTime date = new DateTime();
            if (GlobalParam.OperationDate == OperationDate.English)
            {
                date = DateTime.Parse(FromMtb.Text);
            }
            else
            {
                date = NepaliDateExtension.ToEnglishDate(FromMtb.Text);
            }
           
            if (rbtnAttendance.Checked)
            {
                reportData = dailyEmployeePerformanceReportService.GetAttendanceReportFromSp(date).Data.OrderBy(x=>x.EmployeeName).ToList();
                if (employees.Count() > 0)
                {
                    reportData = (from c in reportData
                                  join d in employees
                                  on c.EmployeeId equals d
                                  select c).ToList();
                }
                ReportPath = @"Report\DailyEmployeePerformanceReport.rdlc";
                reportTitle = string.Format("Daily Attendance Report As On {0}", FromMtb.Text);
            }
            else if (rbtnLateIn.Checked)
            {
                reportData = dailyEmployeePerformanceReportService.GetAttendanceReportFromSp(date).Data.OrderBy(x => x.EmployeeName).ToList();
                reportData = reportData.Where(x => x.LateIn != "").ToList();
                if (employees.Count() > 0)
                {
                    reportData = (from c in reportData
                                  join d in employees
                                  on c.EmployeeId equals d
                                  select c).ToList();
                }
                ReportPath = @"Report\DailyLateInReport.rdlc";
                reportTitle = string.Format("Daily Late In Report As On {0}", FromMtb.Text);
            }
            else if (rbtnEarlyIn.Checked)
            {
                reportData = dailyEmployeePerformanceReportService.GetAttendanceReportFromSp(date).Data.OrderBy(x => x.EmployeeName).ToList();
                reportData = reportData.Where(x => x.EarlyIn != "").ToList();
                if (employees.Count() > 0)
                {
                    reportData = (from c in reportData
                                  join d in employees
                                  on c.EmployeeId equals d
                                  select c).ToList();
                }
                ReportPath = @"Report\DailyEarlyInReport.rdlc";
                reportTitle = string.Format("Daily Early In Report As On {0}", FromMtb.Text);
            }
           // List<ReportParameter> param = new List<ReportParameter>();
            ReportViewerFrm rviewer = new ReportViewerFrm(ReportPath, "DataSet1", reportData.ToDataTable<AttendanceReportDetailViewModel>())
            {
                CompanyName = RiddhaSession.CompanyName,
                ReportTitle = reportTitle
            };
            
            rviewer.ShowDialog();
        }
    }
}
