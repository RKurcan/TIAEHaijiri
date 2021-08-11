using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
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

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    public partial class ShiftFrm : Form
    {
        EShift _shiftData = null;
        public ShiftFrm()
        {
            InitializeComponent();
            _shiftData = new EShift();
            cmbStartMonth.SelectedIndex = 0;
            cmbEndMonth.SelectedIndex = 0;
            cmbShiftType.SelectedIndex = 0;
            Reset();
            populateShiftGrid();
            btnCreate.Text = "Create";
        }
        private void populateShiftGrid()
        {
            SShift shiftServices = new SShift();
            var data = (from c in shiftServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                        select new ShiftGridVm()
                        {
                            BranchId = c.BranchId,
                            ShiftCode = c.ShiftCode,
                            Id = c.Id,
                            LunchEndTime = c.LunchEndTime,
                            LunchStartTime = c.LunchStartTime,
                            ShiftName = c.ShiftName,
                            NumberOfStaff = c.NumberOfStaff,
                            ShiftEndTime = c.ShiftEndTime,
                            ShiftStartTime = c.ShiftStartTime,
                            ShiftType = c.ShiftType,
                            ShiftTypeName = Enum.GetName(typeof(ShiftType), c.ShiftType),
                            DeclareAbsentForEarlyOut = c.DeclareAbsentForEarlyOut,
                            DeclareAbsentForLateIn = c.DeclareAbsentForLateIn,
                            EarlyGrace = c.EarlyGrace,
                            EndDays = c.EndDays,
                            EndMonth = c.EndMonth,
                            HalfDayWorkingHour = c.HalfDayWorkingHour,
                            LateGrace = c.LateGrace,
                            NameNp = c.NameNp,
                            ShiftEndGrace = c.ShiftEndGrace,
                            ShiftStartGrace = c.ShiftStartGrace,
                            ShortDayWorkingEnable = c.ShortDayWorkingEnable,
                            StartDays = c.StartDays,
                            StartMonth = c.StartMonth
                        }).ToList();
            shiftGridView.DataSource = data;
        }


        private void setInputValue()
        {
            txtCode.Text = _shiftData.ShiftCode;
            txtName.Text = _shiftData.ShiftName;
            cmbShiftType.SelectedIndex = (int)_shiftData.ShiftType;
            mtbShiftStart.Text = (_shiftData.ShiftStartTime.ToString(@"hh\:mm"));
            mtbShiftEnd.Text = (_shiftData.ShiftEndTime.ToString(@"hh\:mm"));
            mtbLunchStart.Text = (_shiftData.LunchStartTime.ToString(@"hh\:mm"));
            mtbLunchEnd.Text = (_shiftData.LunchEndTime.ToString(@"hh\:mm"));
            cmbNoOfStaff.Value = _shiftData.NumberOfStaff;
            mtbEarlyGrace.Text = (_shiftData.EarlyGrace.HasValue ? _shiftData.EarlyGrace.Value.ToString(@"hh\:mm") : "");
            mtbLateGrace.Text = (_shiftData.LateGrace.HasValue ? _shiftData.LateGrace.Value.ToString(@"hh\:mm") : "");
            chkShortDayWorking.Checked = _shiftData.ShortDayWorkingEnable;
            mtbHalfDayWorkingHour.Text = (_shiftData.HalfDayWorkingHour.HasValue ? _shiftData.HalfDayWorkingHour.Value.ToString(@"hh\:mm") : "");
            chkLateIn.Checked = _shiftData.DeclareAbsentForLateIn;
            chkEarlyOut.Checked = _shiftData.DeclareAbsentForEarlyOut;
            mtbShiftStartGrace.Text = (_shiftData.ShiftStartGrace.ToString(@"hh\:mm"));
            mtbShiftEndGrace.Text = (_shiftData.ShiftEndGrace.ToString(@"hh\:mm"));
            mtbShiftEnd.Text = (_shiftData.ShiftEndTime.ToString(@"hh\:mm"));
            cmbStartMonth.SelectedIndex = (int)_shiftData.StartMonth;
            cmbEndMonth.SelectedIndex = (int)_shiftData.EndMonth;
            cmbStartDays.Value = _shiftData.StartDays;
            cmbEndDays.Value = _shiftData.EndDays;
        }

        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateShiftGrid();
                createToolStripMenuItem_Click(null, null);
                Reset();
            }
        }

        public void setSelectedData()
        {
            var selectedRow = shiftGridView.Rows[shiftGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as ShiftGridVm;
            _shiftData = new EShift()
            {
                Id = selectedData.Id,
                BranchId = selectedData.BranchId,
                ShiftCode = selectedData.ShiftCode,
                ShiftName = selectedData.ShiftName,
                ShiftStartTime = selectedData.ShiftStartTime,
                ShiftEndTime = selectedData.ShiftEndTime,
                LunchStartTime = selectedData.LunchStartTime,
                LunchEndTime = selectedData.LunchEndTime,
                ShiftType = selectedData.ShiftType,
                NumberOfStaff = selectedData.NumberOfStaff,
                DeclareAbsentForEarlyOut = selectedData.DeclareAbsentForEarlyOut,
                DeclareAbsentForLateIn = selectedData.DeclareAbsentForLateIn,
                EarlyGrace = selectedData.EarlyGrace,
                EndDays = selectedData.EndDays,
                EndMonth = selectedData.EndMonth,
                HalfDayWorkingHour = selectedData.HalfDayWorkingHour,
                LateGrace = selectedData.LateGrace,
                ShiftEndGrace = selectedData.ShiftEndGrace,
                ShiftStartGrace = selectedData.ShiftStartGrace,
                ShortDayWorkingEnable = selectedData.ShortDayWorkingEnable,
                StartDays = selectedData.StartDays,
                StartMonth = selectedData.StartMonth,
            };
        }
        private void ShiftFrm_Load(object sender, EventArgs e)
        {
            cmbStartMonth.SelectedIndex = 0;
            cmbEndMonth.SelectedIndex = 0;
            cmbShiftType.SelectedIndex = 0;
            btnCreate.Text = "Create";
        }

        private void chkShortDayWorking_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShortDayWorking.Checked == true)
            {
                ShortDayWorkingPanel.Enabled = true;
            }
            else
            {
                ShortDayWorkingPanel.Enabled = false;
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shiftData = new EShift();
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shiftGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_shiftData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            mtbShiftEnd_Leave(null, null);
            mtbShiftEndGrace_Leave(null, null);
            btnCreate.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SShift shiftServices = new SShift();
            EShift shift = new EShift();
            if (shiftGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_shiftData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                shift = shiftServices.List().Data.Where(x => x.Id == _shiftData.Id).FirstOrDefault();
                var result = shiftServices.Remove(shift);
                processResult<int>(result);
                if (result.Status == ResultStatus.Ok)
                {
                    MessageBox.Show("Remove Sucessfully.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SShift shiftServices = new SShift();
            shiftGridView.DataSource = (from c in shiftServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.ShiftName.StartsWith(txtSearch.Text)).ToList()
                                        select new ShiftGridVm()
                                        {
                                            BranchId = c.BranchId,
                                            ShiftCode = c.ShiftCode,
                                            Id = c.Id,
                                            LunchEndTime = c.LunchEndTime,
                                            LunchStartTime = c.LunchStartTime,
                                            ShiftName = c.ShiftName,
                                            NumberOfStaff = c.NumberOfStaff,
                                            ShiftEndTime = c.ShiftEndTime,
                                            ShiftStartTime = c.ShiftStartTime,
                                            ShiftType = c.ShiftType,
                                            ShiftTypeName = Enum.GetName(typeof(ShiftType), c.ShiftType),
                                        }).ToList();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            SShift shiftServices = new SShift();

            if (txtCode.Text == "")
            {
                MessageBox.Show("Shift Code is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return;
            }
            else if (txtName.Text == "")
            {
                MessageBox.Show("Shift Name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            else if (mtbShiftStart.Text == "")
            {
                MessageBox.Show("Shift Start time is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            else if (mtbShiftEnd.Text == "")
            {
                MessageBox.Show("Shift End time is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            _shiftData.BranchId = RiddhaSession.BranchId;
            _shiftData.DeclareAbsentForEarlyOut = chkEarlyOut.Checked;
            _shiftData.DeclareAbsentForLateIn = chkLateIn.Checked;
            _shiftData.EarlyGrace = TimeSpan.Parse(mtbEarlyGrace.Text);
            _shiftData.EndDays = Convert.ToInt32(Math.Round(cmbEndDays.Value, 0));
            _shiftData.EndMonth = (NepaliMonth)Enum.ToObject(typeof(NepaliMonth), cmbEndMonth.SelectedIndex + 1);
            _shiftData.HalfDayWorkingHour = TimeSpan.Parse(mtbHalfDayWorkingHour.Text);
            _shiftData.LateGrace = TimeSpan.Parse(mtbLateGrace.Text);
            _shiftData.LunchEndTime = TimeSpan.Parse(mtbLunchEnd.Text);
            _shiftData.LunchStartTime = TimeSpan.Parse(mtbLunchStart.Text);
            _shiftData.NumberOfStaff = Convert.ToInt32(Math.Round(cmbNoOfStaff.Value, 0));
            _shiftData.ShiftCode = txtCode.Text;
            _shiftData.ShiftEndGrace = TimeSpan.Parse(mtbShiftEndGrace.Text);
            _shiftData.ShiftEndTime = TimeSpan.Parse(mtbShiftEnd.Text);
            _shiftData.ShiftName = txtName.Text;
            _shiftData.ShiftStartGrace = TimeSpan.Parse(mtbShiftStartGrace.Text);
            _shiftData.ShiftStartTime = TimeSpan.Parse(mtbShiftStart.Text);
            _shiftData.ShiftType = (ShiftType)Enum.ToObject(typeof(ShiftType), cmbShiftType.SelectedIndex);
            _shiftData.ShortDayWorkingEnable = chkShortDayWorking.Checked;
            _shiftData.StartDays = Convert.ToInt32(Math.Round(cmbStartDays.Value, 0));
            _shiftData.StartMonth = (NepaliMonth)Enum.ToObject(typeof(NepaliMonth), cmbStartMonth.SelectedIndex + 1);

            if (_shiftData.Id == 0)
            {
                var result = shiftServices.Add(_shiftData);
                processResult(result);
                if (result.Status == ResultStatus.Ok)
                {

                    MessageBox.Show("Added Sucessfully.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var result = shiftServices.Update(_shiftData);
                processResult(result);
                if (result.Status == ResultStatus.Ok)
                {
                    MessageBox.Show("Updated Sucessfully.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnCreate.Text = "Create";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            mtbEarlyGrace.Text = "00:00";
            mtbHalfDayWorkingHour.Text = "00:00";
            mtbLateGrace.Text = "00:00";
            mtbLunchEnd.Text = "00:00";
            mtbLunchStart.Text = "00:00";
            mtbShiftEnd.Text = "00:00";
            mtbShiftEndGrace.Text = "00:00";
            mtbShiftStart.Text = "00:00";
            mtbShiftStartGrace.Text = "00:00";
            txtCode.Text = "";
            txtName.Text = "";
            cmbShiftType.SelectedIndex = 0;
            cmbNoOfStaff.Value = 0;
            txtShiftHours.Text = "";
            chkLateIn.Checked = false;
            chkEarlyOut.Checked = false;
            cmbStartMonth.SelectedIndex = 0;
            cmbEndMonth.SelectedIndex = 0;
            cmbStartDays.Value = 0;
            cmbEndDays.Value = 0;
            txtGraceShiftHours.Text = "";
            chkShortDayWorking.Checked = false;
        }
        private TimeSpan getShiftHours(TimeSpan shiftStartHour, TimeSpan shiftEndHour)
        {

            if (shiftEndHour > shiftStartHour)
            {
                TimeSpan shiftHour = new TimeSpan(shiftEndHour.Ticks - shiftStartHour.Ticks);
                return shiftHour;
            }

            else if (shiftEndHour == shiftStartHour)
            {
                TimeSpan idleTime = new TimeSpan(0, 0, 0);
                TimeSpan shiftHour = new TimeSpan();
                shiftHour = idleTime;
                return shiftHour;
            }

            else
            {
                TimeSpan idleTime = new TimeSpan(24, 0, 0);
                TimeSpan shiftHour = new TimeSpan();
                shiftHour = idleTime - shiftStartHour + shiftEndHour;
                return shiftHour;
            }
        }

        private void mtbShiftEnd_Leave(object sender, EventArgs e)
        {
            TimeSpan shiftHour = getShiftHours(TimeSpan.Parse(mtbShiftStart.Text), TimeSpan.Parse(mtbShiftEnd.Text));
            if (shiftHour.TotalHours != 24)
            {
                txtShiftHours.Text = shiftHour.ToString();
            }
            else
            {
                txtShiftHours.Text = "24:00";
            }
        }

        private void mtbShiftEndGrace_Leave(object sender, EventArgs e)
        {
            TimeSpan shiftHour = getShiftHours(TimeSpan.Parse(mtbShiftStartGrace.Text), TimeSpan.Parse(mtbShiftEndGrace.Text));
            if (shiftHour.TotalHours != 24)
            {
                txtGraceShiftHours.Text = shiftHour.ToString();
            }
            else
            {
                txtGraceShiftHours.Text = "24:00";
            }
        }

        private void ShiftFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
