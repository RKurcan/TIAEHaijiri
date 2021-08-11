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



using Riddhasoft.EHajiriPro.Desktop.Common;
namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    public partial class MonthlyRosterSetupFrm : Form
    {
        int branchId = RiddhaSession.BranchId;
        public List<CheckBoxListModel> SelectedDepartMentCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedSectionCheckBoxList { get; set; }
        public List<CheckBoxListModel> SelectedEmployeeCheckBoxList { get; set; }
        public MonthlyRosterSetupFrm()
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
            SEmployee empServices = new SEmployee();
            var employee = empServices.List().Data.Where(x => x.ShiftTypeId == 2).ToList();
            var data = (from c in employee
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
            ManualRosterGridView.EnableHeadersVisualStyles = false;
            foreach (DataGridViewColumn item in ManualRosterGridView.Columns)
            {
                item.HeaderCell.Style.BackColor = GlobalParam.darkColor;
            }
        }
        private void createGrid()
        {
            SShift shiftservice = new SShift();
            var shiftList = shiftservice.List().Data.Where(x => x.BranchId == branchId).ToList();
            var Days = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ManualRosterGridView.AllowUserToResizeColumns = false;
            Days.ReadOnly = true;
            Days.Width = 150;
            Days.HeaderText = "Days";
            Days.DataPropertyName = "Days";
            Days.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.ManualRosterGridView.Columns.Add(Days);
            var xLocationforCheckBox = 170;

            foreach (var item in shiftList)
            {

                var Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                Column1.HeaderText = item.ShiftName;
                Column1.DataPropertyName = item.Id.ToString();
                Column1.Width = 100;
                Column1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                Column1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                this.ManualRosterGridView.Columns.Add(Column1);
                var columnIndex = shiftList.IndexOf(item) + 1;
                CheckBox checkBox = new CheckBox();
                checkBox.Size = new Size(15, 15);
                checkBox.BackColor = Color.Transparent;

                // Reset properties
                checkBox.Padding = new Padding(0);
                checkBox.Margin = new Padding(0);
                checkBox.Text = item.ShiftName;
                checkBox.Name = columnIndex.ToString();
                checkBox.CheckedChanged += RosterColumnSelect;

                // Add checkbox to datagrid cell
                ManualRosterGridView.Controls.Add(checkBox);
                var curCol = ManualRosterGridView.Columns[columnIndex];

                DataGridViewHeaderCell header = ManualRosterGridView.Columns[columnIndex].HeaderCell;
                checkBox.Location = new Point(
                   xLocationforCheckBox,
                    (10));
                xLocationforCheckBox += 150;
            }
            this.ManualRosterGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            ManualRosterGridView.AutoGenerateColumns = false;
        }
        private void RosterColumnSelect(object sender, EventArgs e)
        {
            var chkSelect = sender as CheckBox;
            int columnIndex = chkSelect.Name.ToInt();
            foreach (DataGridViewRow drow in ManualRosterGridView.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)drow.Cells[columnIndex];
                chk.Value = chkSelect.Checked;
            }
        }

        private void weeklyRosterGridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                foreach (Control item in ManualRosterGridView.Controls)
                {
                    if (item is CheckBox)
                    {
                        item.Location = new Point(item.Location.X - (e.NewValue - e.OldValue), item.Location.Y);
                    }
                }
            }
        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    SWeeklyRoster rosterService = new SWeeklyRoster();
        //    List<EWeeklyRoster> dataToSave = new List<EWeeklyRoster>();
        //    var CheckedEmployees = EmployeeChkBoxList.GetCheckedItem();
        //    if (CheckedEmployees.Count == 0)
        //    {
        //        MessageBox.Show("Please Select Employee To Proceed");
        //        return;
        //    }
        //    try
        //    {

        //        foreach (var CheckedEmployee in CheckedEmployees)
        //        {
        //            int i = 0;
        //            foreach (DataGridViewRow row in ManualRosterGridView.Rows)
        //            {

        //                for (int j = 0; j < ManualRosterGridView.ColumnCount; j++)
        //                {

        //                    if (j > 0)
        //                    {
        //                        var roster = new EWeeklyRoster() { RosterCreationDate = System.DateTime.Now };
        //                        roster.Day = (Riddhasoft.Employee.Entities.Day)(i);
        //                        roster.EmployeeId = CheckedEmployee.Id;
        //                        int shiftId = ManualRosterGridView.Columns[j].DataPropertyName.ToInt();
        //                        roster.ShiftId = shiftId;
        //                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[j];
        //                        var Ischecked = bool.Parse(chk.Value.ToString() == "" ? "False" : chk.Value.ToString());
        //                        if (Ischecked)
        //                        {
        //                            dataToSave.Add(roster);
        //                        }
        //                        rosterService.RemoveWeekly(roster);
        //                    }

        //                }
        //                i++;
        //            }

        //        }
        //        rosterService.AddWeeklyRosterRange(dataToSave);
        //        MessageBox.Show("Roster Updated Successfully.");
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Some Error Occured.");

        //    }
        //}

        private void MonthlyRosterSetupFrm_Load(object sender, EventArgs e)
        {
            populateMonthInCombobox();
        }
        private void populateMonthInCombobox()
        {
            SDateTable service = new SDateTable();

            if (GlobalParam.OperationDate == OperationDate.English)
            {
                MonthCmb.DataSource = service.GetEnglishMonths();
                MonthCmb.DisplayMember = "EngDate";
            }
            else
            {
                MonthCmb.DataSource = service.GetNepaliMonths();
                MonthCmb.DisplayMember = "NepDate";
            }
        }
        private void YearTxt_Leave(object sender, EventArgs e)
        {
            var year = 0;
            if (string.IsNullOrEmpty(YearTxt.Text.Trim()))
            {
                return;
            }

            var CheckedEmployee = EmployeeChkBoxList.GetCheckedItem().FirstOrDefault();
            if (CheckedEmployee == null)
            {
                MessageBox.Show("Please Select Employee To Proceed");
                return;
            }

            SShift shiftservice = new SShift();
            var shiftList = shiftservice.List().Data.Where(x => x.BranchId == branchId).ToList();
            var dateTableList = new List<EDateTable>();

            if (int.TryParse(YearTxt.Text, out year))
            {
                SDateTable service = new SDateTable();
                switch (GlobalParam.OperationDate)
                {
                    case OperationDate.English:
                        dateTableList = service.GetDaysInEnglishMonth(year, MonthCmb.SelectedIndex + 1);


                        break;
                    case OperationDate.Nepali:
                        dateTableList = service.GetDaysInNepaliMonth(year, MonthCmb.SelectedIndex + 1);
                        break;
                    default:
                        break;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("Days");
                foreach (var item in shiftList)
                {
                    dt.Columns.Add(item.Id.ToString());

                }
                DataRow drow = null;
                SRoster rosterservice = new SRoster();

                List<ERoster> RosterList = new List<ERoster>();
                RosterList = (rosterservice.List().Data.Where(x => x.EmployeeId == CheckedEmployee.Id).ToList());
                int i = 0;

                foreach (var item in dateTableList)
                {
                    drow = dt.NewRow();
                    switch (GlobalParam.OperationDate)
                    {
                        case OperationDate.English:
                            drow["Days"] = item.EngDate.ToString("yyyy/MM/dd") + "," + item.EngDate.DayOfWeek;
                            break;
                        case OperationDate.Nepali:
                            string[] dates = item.NepDate.Split('/');
                            drow["Days"] = string.Format("{0}/{1}/{2},{3}", dates[0], dates[1], dates[2], item.EngDate.DayOfWeek);
                            break;
                        default:
                            drow["Days"] = item.EngDate + "," + item.EngDate.DayOfWeek;
                            break;
                    }

                    foreach (var shift in shiftList)
                    {
                        if (RosterList.Where(x => x.Date == item.EngDate && x.ShiftId == shift.Id).Count() > 0)
                            drow[shift.Id.ToString()] = true;
                        else
                            drow[shift.Id.ToString()] = false;
                    }

                    dt.Rows.Add(drow);
                }

                ManualRosterGridView.DataSource = dt;


            }
            else
            {
                MessageBox.Show("Invalid Year.");
            }
        }

        private void btnRefreshRoster_Click_1(object sender, EventArgs e)
        {
            YearTxt_Leave(null, null);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            SRoster rosterService = new SRoster();
            List<ERoster> dataToSave = new List<ERoster>();
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
                    foreach (DataGridViewRow row in ManualRosterGridView.Rows)
                    {
                        var days = new DateTime();

                        for (int j = 0; j < ManualRosterGridView.ColumnCount; j++)
                        {

                            if (j > 0)
                            {
                                var roster = new ERoster() { RosterCreationDate = System.DateTime.Now };
                                roster.Date = days;
                                roster.EmployeeId = CheckedEmployee.Id;
                                int shiftId = ManualRosterGridView.Columns[j].DataPropertyName.ToInt();
                                roster.ShiftId = shiftId;
                                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[j];
                                var Ischecked = bool.Parse(chk.Value.ToString() == "" ? "False" : chk.Value.ToString());
                                if (Ischecked)
                                {
                                    dataToSave.Add(roster);
                                }
                                var rosterToRemove = rosterService.List().Data.Where(x => x.EmployeeId == roster.EmployeeId).FirstOrDefault();
                                if (rosterToRemove != null)
                                {
                                    rosterService.Remove(rosterToRemove);
                                }
                            }
                            else
                            {
                                DataGridViewTextBoxCell txt = (DataGridViewTextBoxCell)row.Cells[j];
                                switch (GlobalParam.OperationDate)
                                {
                                    case OperationDate.English:
                                        days = txt.Value.ToString().Split(',')[0].ToDateTime();
                                        break;
                                    case OperationDate.Nepali:
                                        days = txt.Value.ToString().Split(',')[0].ToEnglishDate();
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }

                    }
                }
                rosterService.AddRange(dataToSave);
                MessageBox.Show("Roster Updated Successfully.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MonthlyRosterSetupFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
