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

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    public partial class BranchFrm : Form
    {
        EBranch _branchData = null;
        int companyId = RiddhaSession.CompanyId;
        public BranchFrm()
        {
            InitializeComponent();
            _branchData = new EBranch();
            populateBranchGrid();
            txtBranchCode.Focus();
            btnSave.Text = "Create";
        }
        private void populateBranchGrid()
        {
            SBranch branchServices = new SBranch();
            var list = (from c in branchServices.List().Data.Where(x => x.CompanyId == companyId).ToList()
                        select new BranchGridVm()
                        {
                            Address = c.Address,
                            AddressNp = c.AddressNp,
                            Code = c.Code,
                            CompanyId = c.CompanyId,
                            ContactNo = c.ContactNo,
                            Email = c.Email,
                            Id = c.Id,
                            IsHeadOffice = c.IsHeadOffice,
                            Name = c.Name,
                            NameNp = c.NameNp,
                            CompanyName = c.Company.Name
                        }).ToList();

            branchGridView.DataSource = list;
        }

        private void setInputValue()
        {
            txtBranchCode.Text = _branchData.Code;
            txtBranchName.Text = _branchData.Name;
            txtBranchAddress.Text = _branchData.Address;
            txtContactNumber.Text = _branchData.ContactNo;
            txtEmail.Text = _branchData.Email;
        }
        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok || result.Status == ResultStatus.dataBaseError)
            {
                //MessageBox.Show(result.Message);
                populateBranchGrid();
                createToolStripMenuItem_Click(null, null);
                ResetInputs();
            }
        }
        public void setSelectedData()
        {
            var selectedRow = branchGridView.Rows[branchGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as BranchGridVm;
            _branchData = new EBranch()
            {
                Id = selectedData.Id,
                Address = selectedData.Address,
                AddressNp = selectedData.AddressNp,
                Code = selectedData.Code,
                CompanyId = selectedData.CompanyId,
                ContactNo = selectedData.ContactNo,
                Email = selectedData.Email,
                IsHeadOffice = selectedData.IsHeadOffice,
                Name = selectedData.Name,
                NameNp = selectedData.NameNp,
            };
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _branchData = new EBranch();
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (branchGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_branchData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SBranch branchServices = new SBranch();
            if (branchGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_branchData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var data = branchServices.List().Data.Where(x => x.Id == _branchData.Id).FirstOrDefault();
                var result = branchServices.Remove(data);
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
            SBranch branchServices = new SBranch();
            var list = (from c in branchServices.List().Data.Where(x => x.CompanyId == companyId && x.Name.StartsWith(txtSearch.Text)).ToList()
                        select new BranchGridVm()
                        {
                            Address = c.Address,
                            AddressNp = c.AddressNp,
                            Code = c.Code,
                            CompanyId = c.CompanyId,
                            ContactNo = c.ContactNo,
                            Email = c.Email,
                            Id = c.Id,
                            IsHeadOffice = c.IsHeadOffice,
                            Name = c.Name,
                            NameNp = c.NameNp,
                            CompanyName = c.Company.Name
                        }).ToList();

            branchGridView.DataSource = list;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SBranch branchServices = new SBranch();

            if (txtBranchCode.Text.Trim() == "")
            {
                MessageBox.Show("Branch code is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBranchCode.Focus();
                return;
            }
            else if (txtBranchName.Text.Trim() == "")
            {
                MessageBox.Show("Branch name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBranchName.Focus();
                return;
            }
            else if (txtBranchAddress.Text.Trim() == "")
            {
                MessageBox.Show("Branch address is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBranchAddress.Focus();
                return;
            }
            else if (txtContactNumber.Text.Trim() == "")
            {
                MessageBox.Show("Contact number is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactNumber.Focus();
                return;
            }
            else if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Email address is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            _branchData.Address = txtBranchAddress.Text;
            _branchData.Code = txtBranchCode.Text;
            _branchData.CompanyId = companyId;
            _branchData.ContactNo = txtContactNumber.Text;
            _branchData.Email = txtEmail.Text;
            _branchData.Name = txtBranchName.Text;
            if (_branchData.Id == 0)
            {
                var result = branchServices.Add(_branchData);
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
                btnSave.Enabled = true;
            }
            else
            {
                var result = branchServices.Update(_branchData);
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
                btnSave.Enabled = true;
                btnSave.Text = "Create";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetInputs();
        }
        public void ResetInputs()
        {
            txtBranchAddress.Text = "";
            txtBranchCode.Text = "";
            txtContactNumber.Text = "";
            txtEmail.Text = "";
            txtBranchName.Text = "";
        }

        private void BranchFrm_Load(object sender, EventArgs e)
        {
            btnSave.Text = "Create";
            txtBranchCode.Focus();
            populateBranchGrid();
        }

        private void BranchFrm_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
