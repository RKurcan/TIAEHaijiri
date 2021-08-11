using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
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


namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    public partial class WeeklyRosterSetupFrm : Form
    {
        int branchId = RiddhaSession.BranchId;
        public List<CheckBoxListModel> SelectedDepartMentCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedSectionCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedEmployeeCheckBoxList { get; set; }
        public WeeklyRosterSetupFrm()
        {
            InitializeComponent();
            SelectedDepartMentCheckBoxList = new List<CheckBoxListModel>();
            SelectedSectionCheckBoxList = new List<CheckBoxListModel>();
            SelectedEmployeeCheckBoxList = new List<CheckBoxListModel>();
            populateDepartment();
            //SetUpDataGridView();
            createGrid();
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
            var data = (from c in services.List().Data.Where(x => x.BranchId == branchId && x.ShiftTypeId == 1).ToList()
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

        private void SetUpDataGridView()
        {
            weeklyRosterGridView.EnableHeadersVisualStyles = false;
            foreach (DataGridViewColumn item in weeklyRosterGridView.Columns)
            {
                item.HeaderCell.Style.BackColor = GlobalParam.darkColor;
            }
        }
        private void createGrid()
        {
            SShift shiftservice = new SShift();
            var shiftList = shiftservice.List().Data.Where(x => x.BranchId == branchId).ToList();
            var Days = new System.Windows.Forms.DataGridViewTextBoxColumn();
            weeklyRosterGridView.AllowUserToResizeColumns = false;
            Days.ReadOnly = true;
            Days.Width = 150;
            Days.HeaderText = "Days";
            Days.DataPropertyName = "Days";
            Days.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            weeklyRosterGridView.Columns.Add(Days);
            var xLocationforCheckBox = 150;

            foreach (var item in shiftList)
            {
                DataGridViewCheckBoxColumn Column1 = new DataGridViewCheckBoxColumn();
                Column1.HeaderText = item.ShiftName;
                Column1.DataPropertyName = item.Id.ToString();
                Column1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Column1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                weeklyRosterGridView.Columns.Add(Column1);
                var columnIndex = shiftList.IndexOf(item) + 1;
                CheckBox checkBox = new CheckBox();
                checkBox.Size = new Size(15, 15);
                checkBox.BackColor = Color.Transparent;

                // Reset properties
                checkBox.Padding = new Padding(0);
                checkBox.Margin = new Padding(0);
                checkBox.Text = item.ShiftName; ;
                checkBox.Name = columnIndex.ToString();
                checkBox.CheckedChanged += RosterColumnSelect;

                // Add checkbox to datagrid cell
                weeklyRosterGridView.Controls.Add(checkBox);
                var curCol = weeklyRosterGridView.Columns[columnIndex];

                DataGridViewHeaderCell header = weeklyRosterGridView.Columns[columnIndex].HeaderCell;
                checkBox.Location = new Point(
                   xLocationforCheckBox,
                   (weeklyRosterGridView.Location.Y));
                xLocationforCheckBox += 100;

            }
            weeklyRosterGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            weeklyRosterGridView.AutoGenerateColumns = false;
        }
        private void RosterColumnSelect(object sender, EventArgs e)
        {
            var chkSelect = sender as CheckBox;
            int columnIndex = chkSelect.Name.ToInt();
            foreach (DataGridViewRow drow in weeklyRosterGridView.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)drow.Cells[columnIndex];
                chk.Value = chkSelect.Checked;
            }
        }

        private void weeklyRosterGridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                foreach (Control item in weeklyRosterGridView.Controls)
                {
                    if (item is CheckBox)
                    {
                        item.Location = new Point(item.Location.X - (e.NewValue - e.OldValue), item.Location.Y);
                    }
                }
            }
        }

        private void btnRefreshRoster_Click(object sender, EventArgs e)
        {
            var year = 0;
            var CheckedEmployee = EmployeeChkBoxList.GetCheckedItem().FirstOrDefault();
            if (CheckedEmployee == null)
            {
                MessageBox.Show("Please Select Employee To Proceed");
                return;
            }
            SShift shiftservice = new SShift();
            var shiftList = shiftservice.List().Data.Where(x => x.BranchId == branchId).ToList();
            SDateTable service = new SDateTable();
            DataTable dt = new DataTable();
            dt.Columns.Add("Days");
            foreach (var item in shiftList)
            {
                dt.Columns.Add(item.Id.ToString());
            }
            DataRow drow = null;
            SWeeklyRoster rosterservice = new SWeeklyRoster();
            List<EWeeklyRoster> RosterList = new List<EWeeklyRoster>();
            RosterList = (rosterservice.List().Data.Where(x => x.EmployeeId == CheckedEmployee.Id).ToList());
            int i = 0;
            SDateTable date = new SDateTable();
            var days = date.GetDaysInWeek();
            foreach (var item in days)
            {
                drow = dt.NewRow();

                drow["Days"] = item;
                foreach (var shift in shiftList)
                {
                    if (RosterList.Where(x => x.Day == (Riddhasoft.Employee.Entities.Day)days.IndexOf(item) && x.ShiftId == shift.Id).Count() > 0)
                        drow[shift.Id.ToString()] = true;
                    else
                        drow[shift.Id.ToString()] = false;
                }
                dt.Rows.Add(drow);
            }
            weeklyRosterGridView.DataSource = dt;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SWeeklyRoster rosterService = new SWeeklyRoster();
            List<EWeeklyRoster> dataToSave = new List<EWeeklyRoster>();
            var CheckedEmployees = EmployeeChkBoxList.GetCheckedItem();
            if (CheckedEmployees.Count == 0)
            {
                MessageBox.Show("Please Select Employee To Proceed");
                return;
            }
            try
            {

                foreach (var CheckedEmployee in CheckedEmployees)
                {
                    int i = 0;
                    foreach (DataGridViewRow row in weeklyRosterGridView.Rows)
                    {
                        
                        for (int j = 0; j < weeklyRosterGridView.ColumnCount; j++)
                        {

                            if (j > 0)
                            {
                                var roster = new EWeeklyRoster() { RosterCreationDate = System.DateTime.Now };
                                roster.Day = (Riddhasoft.Employee.Entities.Day)(i);
                                roster.EmployeeId = CheckedEmployee.Id;
                                int shiftId = weeklyRosterGridView.Columns[j].DataPropertyName.ToInt();
                                roster.ShiftId = shiftId;
                                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[j];
                                var Ischecked = bool.Parse(chk.Value.ToString() == "" ? "False" : chk.Value.ToString());
                                if (Ischecked)
                                {
                                    dataToSave.Add(roster);
                                }
                                rosterService.RemoveWeekly(roster);
                            }
                            
                        }
                        i++;
                    }
                    
                }
                rosterService.AddWeeklyRosterRange(dataToSave);
                MessageBox.Show("Roster Updated Successfully.");
            }
            catch
            {
                MessageBox.Show("Some Error Occured.");

            }
        }

        private void WeeklyRosterSetupFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
