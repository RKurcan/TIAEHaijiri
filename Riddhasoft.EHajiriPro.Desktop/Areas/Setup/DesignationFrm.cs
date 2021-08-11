using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.OfficeSetup.Entities;
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
using Riddhasoft.Services.Common;
using Riddhasoft.EHajiriPro.Desktop.Views;
using Riddhasoft.EHajiriPro.Desktop.Areas.Leave;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    public partial class DesignationFrm : Form
    {
        EDesignation _designationData = null;
        public DesignationFrm(EventHandler Exit)
        {
            InitializeComponent();
            _designationData = new EDesignation();
            populateDesignationGrid();
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            designationGridView.Columns.Add(btn);
            btn.HeaderText = "Action";
            btn.Text = "Leave Quota";
            btn.Name = "btnLeaveQuata";
            btn.UseColumnTextForButtonValue = true;
            btnSave.Text = "Create";
            txtMaxSalary.Text = "0";
            txtMinSalary.Text = "0";
        }
        private void populateDesignationGrid()
        {
            SDesignation designationServices = new SDesignation();
            designationGridView.DataSource = (from c in designationServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                                              select new DesignationGridVm()
                                          {
                                              BranchId = c.BranchId,
                                              Code = c.Code,
                                              Id = c.Id,
                                              Name = c.Name,
                                              DesignationLevel = c.DesignationLevel,
                                              MaxSalary = c.MaxSalary,
                                              MinSalary = c.MinSalary
                                          }).ToList();
        }
        private void setInputValue()
        {
            txtCode.Text = _designationData.Code;
            txtName.Text = _designationData.Name;
            txtMaxSalary.Text = _designationData.MaxSalary.ToString();
            txtMinSalary.Text = _designationData.MinSalary.ToString();
            mtbDesignationLvl.Value = _designationData.DesignationLevel;
        }
        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateDesignationGrid();
                createToolStripMenuItem_Click(null, null);
            }
        }
        public void setSelectedData()
        {
            var selectedRow = designationGridView.Rows[designationGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as DesignationGridVm;
            _designationData = new EDesignation()
            {
                Id = selectedData.Id,
                BranchId = selectedData.BranchId,
                Code = selectedData.Code,
                Name = selectedData.Name,
                MinSalary = selectedData.MinSalary,
                MaxSalary = selectedData.MaxSalary,
                DesignationLevel = selectedData.DesignationLevel,
            };
        }
        private void DesignationFrm_Load(object sender, EventArgs e)
        {
            btnSave.Text = "Create";
            populateDesignationGrid();
            txtCode.Focus();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _designationData = new EDesignation();
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (designationGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_designationData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SDesignation designationServices = new SDesignation();
            EDesignation designation = new EDesignation();
            if (designationGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_designationData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                designation = designationServices.List().Data.Where(x => x.Id == _designationData.Id).FirstOrDefault();
                var result = designationServices.Remove(designation);
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
            SDesignation designationServices = new SDesignation();
            designationGridView.DataSource = (from c in designationServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.Name.StartsWith(txtSearch.Text)).ToList()
                                              select new DesignationGridVm()
                                              {
                                                  BranchId = c.BranchId,
                                                  Code = c.Code,
                                                  Id = c.Id,
                                                  Name = c.Name,
                                                  DesignationLevel = c.DesignationLevel,
                                                  MaxSalary = c.MaxSalary,
                                                  MinSalary = c.MinSalary
                                              }).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SDesignation designationServices = new SDesignation();

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

            _designationData.BranchId = RiddhaSession.BranchId;
            _designationData.Code = txtCode.Text;
            _designationData.Name = txtName.Text;
            _designationData.DesignationLevel = Convert.ToInt32(Math.Round(mtbDesignationLvl.Value, 0));
            _designationData.MaxSalary = decimal.Parse(txtMaxSalary.Text);
            _designationData.MinSalary = decimal.Parse(txtMinSalary.Text);

            if (_designationData.Id == 0)
            {
                var result = designationServices.Add(_designationData);
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
                var result = designationServices.Update(_designationData);
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
            txtCode.Text = "";
            txtName.Text = "";
            txtSearch.Text = "";
            txtMinSalary.Text = "0";
            txtMaxSalary.Text = "0";
            mtbDesignationLvl.Value = 0;
        }

        private void designationGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                var selectedRow = designationGridView.Rows[designationGridView.CurrentRow.Index];
                var selectedData = selectedRow.DataBoundItem as DesignationGridVm;
                LeaveQuataFrm frm = new LeaveQuataFrm(selectedData);
                frm.ShowDialog();
            }
        }

        private void DesignationFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
