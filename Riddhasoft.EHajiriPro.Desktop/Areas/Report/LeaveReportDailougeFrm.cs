using Riddhasoft.DB;
using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddhasoft.Services.Common;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Report
{
    public partial class LeaveReportDailougeFrm : Form
    {
        private int branchId = RiddhaSession.BranchId;
        private int reportid = 0;
        private string ReportPath;
        private string reportTitle;
        private int fiscalYearId = 0;
        public List<CheckBoxListModel> SelectedDepartMentCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedSectionCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedEmployeeCheckBoxList { get; set; }
        public LeaveReportDailougeFrm()
        {
            InitializeComponent();
            SelectedDepartMentCheckBoxList = new List<CheckBoxListModel>();
            SelectedSectionCheckBoxList = new List<CheckBoxListModel>();
            SelectedEmployeeCheckBoxList = new List<CheckBoxListModel>();
            populateDepartment();
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

            SelectedEmployeeCheckBoxList.Clear();
            EmployeeChkBoxList.DoThisForCheckedItem(GetCheckedEmployee);
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

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            SFiscalYear fiscalYearServices = new SFiscalYear();
            fiscalYearId = fiscalYearServices.List().Data.Where(x => x.CurrentFiscalYear).FirstOrDefault().Id;
            List<EEmployeeLeaveSummary> reportData = new List<EEmployeeLeaveSummary>();
            SelectedEmployeeCheckBoxList.Clear();
            EmployeeChkBoxList.DoThisForCheckedItem(GetCheckedEmployee);
            int[] employees = (from c in SelectedEmployeeCheckBoxList
                               select c.Id
                               ).ToArray();

            if (rbtnEmpWise.Checked)
            {
                DateTime fromDate = DateTime.Parse("2019/01/01");
                DateTime toDate = DateTime.Now;
                SLeaveReport reportService = new SLeaveReport(RiddhaSession.Language);
                var result = reportService.GetLeaveReportFromSp(fromDate, toDate, RiddhaSession.BranchId, fiscalYearId).Data;

                if (employees.Count() > 0)
                {
                    reportData = (from c in result
                                  join d in employees
                                  on c.EmployeeId equals d
                                  select c).ToList();
                }
                else
                {
                    reportData = result;
                }
                ReportPath = @"Report\EmployeeWiseLeaveReport.rdlc";
                reportTitle = "Employee Wise Leave Report";
                ReportViewerFrm rviewer = new ReportViewerFrm(ReportPath, "DataSet1", reportData.ToDataTable<EEmployeeLeaveSummary>())
                {
                    CompanyName = RiddhaSession.CompanyName,
                    ReportTitle = reportTitle
                };
                rviewer.ShowDialog();
            }
           
        }


    }
}
