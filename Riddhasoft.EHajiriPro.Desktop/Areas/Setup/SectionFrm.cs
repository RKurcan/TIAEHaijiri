using Riddhasoft.EHajiriPro.Desktop.Common;
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
using Riddhasoft.EHajiriPro.Desktop.ViewModel;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    public partial class SectionFrm : Form
    {
        ESection _sectionData = null;
        public SectionFrm(EventHandler Exit)
        {
            InitializeComponent();
            _sectionData = new ESection();
            populateSectionGrid();
            populateDepartment();
            cmbDepartment.SelectedValue = 1;
            btnSave.Text = "Create";
        }
        public void populateDepartment()
        {
            var departmentServices = new SDepartment();
            var result = departmentServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            cmbDepartment.DisplayMember = "Name";
            cmbDepartment.ValueMember = "Id";
            cmbDepartment.DataSource = result;
        }
        private void populateSectionGrid()
        {
            SSection sectionServices = new SSection();
            sectionGridView.DataSource = (from c in sectionServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                                          select new SectionGridVm()
                                          {
                                              BranchId = c.BranchId,
                                              Code = c.Code,
                                              DepartmentId = c.DepartmentId,
                                              DepartmentName = c.Department.Name,
                                              Id = c.Id,
                                              Name = c.Name
                                          }).ToList();
        }
        private void setInputValue()
        {
            txtCode.Text = _sectionData.Code;
            txtName.Text = _sectionData.Name;
            cmbDepartment.SelectedValue = _sectionData.DepartmentId;
        }
        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateSectionGrid();
                createToolStripMenuItem_Click(null, null);
            }
        }

        public void setSelectedData()
        {
            var selectedRow = sectionGridView.Rows[sectionGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as SectionGridVm;
            _sectionData = new ESection()
            {
                Id = selectedData.Id,
                BranchId = selectedData.BranchId,
                Code = selectedData.Code,
                Name = selectedData.Name,
                DepartmentId = selectedData.DepartmentId
            };
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sectionData = new ESection();
            cmbDepartment.SelectedValue = 1;
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sectionGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_sectionData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SSection sectionServices = new SSection();
            ESection section = new ESection();
            if (sectionGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_sectionData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                section = sectionServices.List().Data.Where(x => x.Id == _sectionData.Id).FirstOrDefault();
                var result = sectionServices.Remove(section);
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

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SSection sectionServices = new SSection();
            sectionGridView.DataSource = (from c in sectionServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.Name.StartsWith(txtSearch.Text)).ToList()
                                          select new SectionGridVm()
                                          {
                                              BranchId = c.BranchId,
                                              Code = c.Code,
                                              DepartmentId = c.DepartmentId,
                                              DepartmentName = c.Department.Name,
                                              Id = c.Id,
                                              Name = c.Name
                                          }).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SSection sectionServices = new SSection();

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
            if (cmbDepartment.SelectedValue == null)
            {
                MessageBox.Show("Department is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDepartment.Focus();
                return;
            }

            _sectionData.BranchId = RiddhaSession.BranchId;
            _sectionData.Code = txtCode.Text;
            _sectionData.Name = txtName.Text;
            _sectionData.DepartmentId = cmbDepartment.SelectedValue.ToInt();
            if (_sectionData.Id == 0)
            {
                var result = sectionServices.Add(_sectionData);
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
                var result = sectionServices.Update(_sectionData);
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
            cmbDepartment.SelectedValue = 1;
        }

        private void SectionFrm_Load(object sender, EventArgs e)
        {
            populateSectionGrid();
            populateDepartment();
            txtCode.Focus();
            cmbDepartment.SelectedValue = 1;
            btnSave.Text = "Create";

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SectionFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
