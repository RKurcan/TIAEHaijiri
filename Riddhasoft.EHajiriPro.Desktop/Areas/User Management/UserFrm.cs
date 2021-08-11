using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.User_Management
{
    public partial class UserFrm : Form
    {
        EUser _UserData = null;
        public UserFrm()
        {
            InitializeComponent();
            _UserData = new EUser();
            btnSave.Text = "Create";
            populateUsersInGrid();

        }
        private void populateUsersInGrid()
        {
            SUser userServices = new SUser();
            UserGridView.DataSource = (from c in userServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.IsDeleted ==false).ToList()
                                       select new UserGridVm()
                                             {
                                                 Id = c.Id,
                                                 BranchId = c.BranchId,
                                                 FullName = c.FullName,
                                                 IsDeleted = c.IsDeleted,
                                                 IsSuspended = c.IsSuspended,
                                                 Name = c.Name,
                                                 Password = c.Password,
                                                 PhotoURL = c.PhotoURL,
                                                 RoleId = c.RoleId,
                                                 UserType = c.UserType,
                                                 PasswordChar = "*******",
                                             }).ToList();
        }
        private void setInputValue()
        {
            txtFullName.Text = _UserData.FullName;
            txtUserName.Text = _UserData.Name;
            txtPassword.Text = _UserData.Password;

        }
        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateUsersInGrid();
                createToolStripMenuItem_Click(null, null);
                ResetInupts();
            }
        }
        public void setSelectedData()
        {
            var selectedRow = UserGridView.Rows[UserGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as UserGridVm;
            _UserData = new EUser()
            {
                BranchId = selectedData.BranchId,
                Id = selectedData.Id,
                UserType = selectedData.UserType,
                RoleId = selectedData.RoleId,
                Role = null,
                PhotoURL = selectedData.PhotoURL,
                Password = selectedData.Password,
                Name = selectedData.Name,
                IsSuspended = selectedData.IsSuspended,
                IsDeleted = selectedData.IsDeleted,
                FullName = selectedData.FullName,
            };
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _UserData = new EUser();
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_UserData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SUser userServices = new SUser();
            if (UserGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_UserData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var data = userServices.List().Data.Where(x => x.Id == _UserData.Id).FirstOrDefault();
                var result = userServices.Remove(data);
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

        private void Searchtxt_KeyUp(object sender, KeyEventArgs e)
        {
            SUser userServices = new SUser();
            UserGridView.DataSource = (from c in userServices.List().Data.Where(x => x.FullName.StartsWith(Searchtxt.Text) && x.IsDeleted == false).ToList()
                                       select new UserGridVm()
                                       {
                                           Id = c.Id,
                                           BranchId = c.BranchId,
                                           FullName = c.FullName,
                                           IsDeleted = c.IsDeleted,
                                           IsSuspended = c.IsSuspended,
                                           Name = c.Name,
                                           Password = c.Password,
                                           PhotoURL = c.PhotoURL,
                                           RoleId = c.RoleId,
                                           UserType = c.UserType
                                       }).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SUser UserServices = new SUser();
            if (txtFullName.Text == "")
            {
                MessageBox.Show("Full Name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }
            else if (txtUserName.Text == "")
            {
                MessageBox.Show("User Name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }
            else if (txtPassword.Text == "")
            {
                MessageBox.Show("Password is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }
            Regex regexObj = new Regex(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})$");
            bool match = regexObj.IsMatch(txtPassword.Text);
            if (!match)
            {
                MessageBox.Show("Password should contain atleast alphanumeric and special character", "Invalid Password",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            _UserData = new EUser()
            {
                Id = _UserData.Id,
                BranchId = RiddhaSession.BranchId,
                FullName = txtFullName.Text,
                IsDeleted = false,
                IsSuspended = false,
                Name = txtUserName.Text,
                Password = txtPassword.Text,
                PhotoURL = null,
                Role = null,
                RoleId = null,
                UserType = UserType.User,
            };

            if (_UserData.Id == 0)
            {
                var result = UserServices.Add(_UserData);
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
                var result = UserServices.Update(_UserData);
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
            ResetInupts();
        }

        private void UserFrm_Load(object sender, EventArgs e)
        {
            btnSave.Text = "Create";
            txtFullName.Focus();
            populateUsersInGrid();
        }

        private void ResetInupts()
        {
            txtFullName.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
        }

        private void UserFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
