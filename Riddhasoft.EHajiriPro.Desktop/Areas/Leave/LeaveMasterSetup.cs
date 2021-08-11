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

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    public partial class LeaveMasterSetup : Form
    {
        ELeaveMaster _leaveMasterData = null;
        SLeaveMaster _leaveMasterServices = null;
        public LeaveMasterSetup()
        {
            InitializeComponent();
            _leaveMasterData = new ELeaveMaster();
            _leaveMasterServices = new SLeaveMaster();
            cmbApplicableGender.SelectedIndex = 0;
            populateLeaveMasterGrid();
            btnSave.Text = "Create";
        }
        private void chkIsReplacementLeave_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsReplacementLeave.Checked != true)
            {
                DisablePanel.Enabled = true;
            }
            else
            {
                DisablePanel.Enabled = false;
            }
        }
        private void populateLeaveMasterGrid()
        {
            var list = _leaveMasterServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            var data = (from c in list
                        select new LeaveMasterGridVm()
                        {
                            ApplicableGender = c.ApplicableGender,
                            ApplicableGenderName = Enum.GetName(typeof(ApplicableGender), c.ApplicableGender),
                            Balance = c.Balance,
                            BranchId = c.BranchId,
                            Code = c.Code,
                            CreatedOn = c.CreatedOn,
                            Description = c.Description,
                            Id = c.Id,
                            IsLeaveCarryable = c.IsLeaveCarryable,
                            IsPaidLeave = c.IsPaidLeave,
                            IsReplacementLeave = c.IsReplacementLeave,
                            Name = c.Name,
                        }).ToList();
            leaveMasterGridView.DataSource = data;
        }
        private void setInputValue()
        {
            txtCode.Text = _leaveMasterData.Code;
            txtName.Text = _leaveMasterData.Name;
            chkIsReplacementLeave.Checked = _leaveMasterData.IsReplacementLeave;
            cmbApplicableGender.SelectedIndex = (int)_leaveMasterData.ApplicableGender;
            cmbNumberOfDays.Value = _leaveMasterData.Balance;
            chkIsLeaveCarryable.Checked = _leaveMasterData.IsLeaveCarryable;
            chkIsPaidLeave.Checked = _leaveMasterData.IsPaidLeave;
            txtDescription.Text = _leaveMasterData.Description;
        }


        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateLeaveMasterGrid();
                createToolStripMenuItem_Click(null, null);
                Reset();
            }
        }
        public void setSelectedData()
        {
            var selectedRow = leaveMasterGridView.Rows[leaveMasterGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as LeaveMasterGridVm;
            _leaveMasterData = new ELeaveMaster()
            {
                Id = selectedData.Id,
                BranchId = selectedData.BranchId,
                Code = selectedData.Code,
                Name = selectedData.Name,
                ApplicableGender = selectedData.ApplicableGender,
                Balance = selectedData.Balance,
                CreatedOn = selectedData.CreatedOn,
                Description = selectedData.Description,
                IsLeaveCarryable = selectedData.IsLeaveCarryable,
                IsPaidLeave = selectedData.IsPaidLeave,
                IsReplacementLeave = selectedData.IsReplacementLeave,
            };
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _leaveMasterData = new ELeaveMaster();
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (leaveMasterGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_leaveMasterData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (leaveMasterGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_leaveMasterData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var result = _leaveMasterServices.Remove(_leaveMasterData);
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

        private void searchTxt_KeyUp(object sender, KeyEventArgs e)
        {
            leaveMasterGridView.DataSource = _leaveMasterServices.List().Data.Where(x => x.Name.StartsWith(searchTxt.Text)).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _leaveMasterServices = new SLeaveMaster();
            if (txtCode.Text == "")
            {
                MessageBox.Show("Code is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("Name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            if (cmbNumberOfDays.Value == 0)
            {
                MessageBox.Show("Number of days is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbNumberOfDays.Focus();
                return;
            }
            _leaveMasterData.BranchId = RiddhaSession.BranchId;
            _leaveMasterData.Code = txtCode.Text;
            _leaveMasterData.Name = txtName.Text;
            _leaveMasterData.IsReplacementLeave = chkIsReplacementLeave.Checked;
            _leaveMasterData.ApplicableGender = (ApplicableGender)Enum.ToObject(typeof(ApplicableGender), cmbApplicableGender.SelectedIndex);
            _leaveMasterData.Balance = Convert.ToInt32(Math.Round(cmbNumberOfDays.Value, 0));
            _leaveMasterData.IsLeaveCarryable = chkIsLeaveCarryable.Checked;
            _leaveMasterData.IsPaidLeave = chkIsPaidLeave.Checked;
            _leaveMasterData.Description = txtDescription.Text;
            _leaveMasterData.CreatedOn = DateTime.Now;

            if (_leaveMasterData.Id == 0)
            {
                var result = _leaveMasterServices.Add(_leaveMasterData);
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
                var result = _leaveMasterServices.Update(_leaveMasterData);
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
                btnSave.Text = "Create";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            txtCode.Text = "";
            txtName.Text = "";
            chkIsReplacementLeave.Checked = false;
            cmbApplicableGender.SelectedIndex = 0;
            cmbNumberOfDays.Value = 0;
            chkIsLeaveCarryable.Checked = false;
            chkIsPaidLeave.Checked = false;
            txtDescription.Text = "";
        }

        private void LeaveMasterSetup_Load(object sender, EventArgs e)
        {
            txtCode.Focus();
            populateLeaveMasterGrid();
        }

        private void LeaveMasterSetup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
