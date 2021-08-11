using Riddhasoft.EHajiriPro.Desktop.Common;
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

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    public partial class DepartmentFrm : Form
    {
        EDepartment _departmentData = null;
        SDepartment _departmentServices = null;
        public DepartmentFrm(EventHandler Exit)
        {
            InitializeComponent();
            _departmentData = new EDepartment();
            _departmentServices = new SDepartment();
            populateDepartmentGrid();
            btnSave.Text = "Create";
        }
        private void populateDepartmentGrid()
        {
            var list = _departmentServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            departmentGridView.DataSource = list;
        }
        private void setInputValue()
        {
            txtCode.Text = _departmentData.Code;
            txtName.Text = _departmentData.Name;
        }
        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateDepartmentGrid();
                createToolStripMenuItem_Click(null, null);
            }
        }
        public void setSelectedData()
        {
            var selectedRow = departmentGridView.Rows[departmentGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as EDepartment;
            _departmentData = new EDepartment()
            {
                Id = selectedData.Id,
                BranchId = selectedData.BranchId,
                Code = selectedData.Code,
                Name = selectedData.Name
            };
        }



        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _departmentData = new EDepartment();
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (departmentGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_departmentData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (departmentGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_departmentData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var result = _departmentServices.Remove(_departmentData);
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
            departmentGridView.DataSource = _departmentServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.Name.StartsWith(txtSearch.Text)).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            _departmentServices = new SDepartment();


            if (txtCode.Text == "")
            {
                MessageBox.Show("Code is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return;
            }
            else if (txtName.Text == "")
            {
                MessageBox.Show("Name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            _departmentData.BranchId = RiddhaSession.BranchId;
            _departmentData.Code = txtCode.Text;
            _departmentData.Name = txtName.Text;
            if (_departmentData.Id == 0)
            {
                var result = _departmentServices.Add(_departmentData);
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
                var result = _departmentServices.Update(_departmentData);
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
        }

        private void DepartmentFrm_Load(object sender, EventArgs e)
        {
            txtCode.Focus();
            populateDepartmentGrid();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DepartmentFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
