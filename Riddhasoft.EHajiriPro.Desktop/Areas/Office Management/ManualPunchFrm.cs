using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.EHajiriPro.Desktop.Views;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddhasoft.Globals.Conversion;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    public partial class ManualPunchFrm : Form
    {
        public EEmployee employee { get; set; }
        int branchId = RiddhaSession.BranchId;
        private int employeeId;
        private EManualPunch _manualPunchData = null;
        public ManualPunchFrm()
        {
            InitializeComponent();
            _manualPunchData = new EManualPunch();
            populateManualPunchGrid();
            mtbTIme.Text = "00:00";
            btnCreate.Text = "Create";
        }
        public void populateManualPunchGrid()
        {
            SManualPunch manualPunchService = new SManualPunch();
            var result = (from c in manualPunchService.List().Data.Where(x => x.BranchId == branchId).ToList()
                          select new ManualPunchGridVm()
                          {
                              BranchId = c.BranchId,
                              CompanyId = c.CompanyId,
                              DateTime = RiddhaSession.OpreationDate == "ne" ? c.DateTime.ToNepaliDate() + "-" + c.DateTime.TimeOfDay.ToString(@"hh\:mm") : c.DateTime.ToString("yyyy/MM/dd") + "-" + c.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                              EmployeeId = c.EmployeeId,
                              Employee = c.Employee.Name,
                              Id = c.Id,
                              Remark = c.Remark,
                          }).ToList();
            manualPunchGridView.DataSource = result;

        }
        private void setInputValue()
        {
            mtbDate.Text = _manualPunchData.DateTime.ToString("yyyy/MM/dd");
            mtbTIme.Text = _manualPunchData.DateTime.TimeOfDay.ToString(@"hh\:mm");
            rtxtRemark.Text = _manualPunchData.Remark;
        }
        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateManualPunchGrid();
                createToolStripMenuItem_Click(null, null);
            }
        }

        public void setSelectedData()
        {
            var selectedRow = manualPunchGridView.Rows[manualPunchGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as ManualPunchGridVm;
            _manualPunchData = new EManualPunch()
            {
                BranchId = selectedData.BranchId,
                CompanyId = selectedData.CompanyId,
                DateTime = DateTime.Parse(selectedData.DateTime),
                EmployeeId = selectedData.EmployeeId,
                Id = selectedData.Id,
                Remark = selectedData.Remark
            };
            //_manualPunchData = new SManualPunch().List().Data.Where(x => x.Id == selectedData.Id).FirstOrDefault();
            employeeId = _manualPunchData.EmployeeId;
            txtEmployeeCode_Leave(null, null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            EmployeeSearchFrm frm = new EmployeeSearchFrm(SearchComplete);
            frm.ShowDialog();
        }
        private void SearchComplete(EmployeeSearchVm Data)
        {
            txtEmployeeCode.Text = Data.IDCardNo;
            employeeId = Data.Id;
            txtEmployeeCode_Leave(null, null);
        }
        private void displayEmployeeValue(EEmployee employee)
        {
            if (employee != null)
            {
                txtEmployeeCode.Text = employee.Code;
                txtEmployeeName.Text = employee.Name;
                if (employee.Designation != null)
                {
                    txtDesignation.Text = employee.Designation.Name;
                }
                else
                {
                    txtDesignation.Text = "";
                }
                if (File.Exists(employee.ImageUrl))
                {
                    employeePictureBox.Image = Image.FromFile(employee.ImageUrl);

                }
                else
                {
                    //employeePictureBox.Image = Riddhasoft.EHajiriPro.Desktop.Properties.Resources.ImagePlaceHolder;
                }
                employeePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                if (employee.Section != null)
                {
                    txtSection.Text = employee.Section.Name;
                    if (employee.Section.Department != null)
                    {
                        txtDepartment.Text = employee.Section.Department.Name;
                    }
                    else
                    {
                        txtDepartment.Text = "";
                    }
                }
                else
                {

                    txtSection.Text = "";
                }
            }
            else
            {
                //resetInput();
            }
        }

        private void txtEmployeeCode_Leave(object sender, EventArgs e)
        {
            SEmployee service = new SEmployee();
            if (employeeId == 0)
            {
                if (string.IsNullOrEmpty(txtEmployeeCode.Text.Trim()))
                {
                    return;
                }
                employee = service.List().Data.Where(x => x.Code.ToLower() == txtEmployeeCode.Text.ToLower()).FirstOrDefault();
            }
            else
            {
                employee = service.List().Data.Where(x => x.Id == employeeId).FirstOrDefault();
            }

            if (employee != null)
            {
                employeeId = employee.Id;
                displayEmployeeValue(employee);
            }
            else
            {
                MessageBox.Show("Invalid Employee Code..");
                displayEmployeeValue(new EEmployee());
            }
        }

        private void ManualPunchFrm_Load(object sender, EventArgs e)
        {
            btnCreate.Text = "Create";
            populateManualPunchGrid();
            mtbTIme.Text = "00:00";
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _manualPunchData = new EManualPunch();
            ResetInupts();
        }


        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (manualPunchGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_manualPunchData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnCreate.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SManualPunch manualPunchServices = new SManualPunch();
            if (manualPunchGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_manualPunchData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var manualPunch = manualPunchServices.List().Data.Where(x => x.Id == _manualPunchData.Id).FirstOrDefault();
                var result = manualPunchServices.Remove(manualPunch);
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
            SManualPunch manualPunchServices = new SManualPunch();
            manualPunchGridView.DataSource = (from c in manualPunchServices.List().Data.Where(x => x.BranchId == branchId && x.Employee.Name.StartsWith(txtSearch.Text)).ToList()
                                              select new ManualPunchGridVm()
                                              {
                                                  BranchId = c.BranchId,
                                                  CompanyId = c.CompanyId,
                                                  //DateTime = c.DateTime.ToString("dd/MM/yyyy") + "-" + c.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                                                  DateTime = RiddhaSession.OpreationDate == "ne" ? c.DateTime.ToNepaliDate() + "-" + c.DateTime.TimeOfDay.ToString(@"hh\:mm") : c.DateTime.ToString("yyyy/MM/dd") + "-" + c.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                                                  EmployeeId = c.EmployeeId,
                                                  Employee = c.Employee.Name,
                                                  Id = c.Id,
                                                  Remark = c.Remark
                                              }).ToList();

        }
        private void ResetInupts()
        {
            txtEmployeeCode.Text = "";
            txtEmployeeName.Text = "";
            txtDepartment.Text = "";
            txtDesignation.Text = "";
            txtSection.Text = "";
            txtSearch.Text = "";
            rtxtRemark.Text = "";
            employeeId = 0;
            mtbTIme.Text = "00:00";
            mtbDate.Text = "";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            SManualPunch manualPunchServices = new SManualPunch();
            if (employeeId == 0)
            {
                MessageBox.Show("Please select employee.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnSearch.Focus();
                return;
            }
            else if (mtbDate.Text == "    /  /")
            {
                MessageBox.Show("Date is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbDate.Focus();
                return;
            }
            else if (mtbTIme.Text == "")
            {
                MessageBox.Show("Time is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbTIme.Focus();
                return;
            }
            else if (rtxtRemark.Text.Trim() == "")
            {
                MessageBox.Show("Remark is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtRemark.Focus();
                return;
            }
            TimeSpan punchTime = mtbTIme.Text.ToTimeSpan();
            DateTime punchDate = new DateTime();
            _manualPunchData.BranchId = branchId;
            _manualPunchData.CompanyId = RiddhaSession.CompanyId;
            _manualPunchData.EmployeeId = employeeId;
            _manualPunchData.Remark = rtxtRemark.Text;
            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    punchDate = mtbDate.Text.ToDateTime();
                    break;
                case OperationDate.Nepali:
                    punchDate = (mtbDate.Text).ToEnglishDate();
                    break;
                default:
                    break;
            }
            _manualPunchData.DateTime = punchDate.Add(punchTime);
            if (_manualPunchData.Id == 0)
            {
                var result = manualPunchServices.Add(_manualPunchData);
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
                var result = manualPunchServices.Update(_manualPunchData);
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
            ResetInupts();
        }

        private void ManualPunchFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Home home = new Home();
            //home.getDashboarCount();
        }

        private void ManualPunchFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
