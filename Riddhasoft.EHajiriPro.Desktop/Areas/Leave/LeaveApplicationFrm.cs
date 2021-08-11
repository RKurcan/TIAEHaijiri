using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.EHajiriPro.Desktop.Views;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
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
using Riddhasoft.Services.Common;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    public partial class LeaveApplicationFrm : Form
    {
        private int branchId = RiddhaSession.BranchId;
        public EEmployee employee { get; set; }
        private ELeaveApplication _LeaveApplicationData = null;
        private int employeeId;
        public LeaveApplicationFrm()
        {
            InitializeComponent();
            _LeaveApplicationData = new ELeaveApplication();
            populateLeaveApplicationGrid();
            populateEmployeeForApproveByDropdown();
            cmbApproveBy.SelectedValue = 1;
            cmbLeaveDay.SelectedIndex = 0;
            cmbLeaveType.SelectedValue = 1;
        }

        public void populateEmployeeForApproveByDropdown()
        {
            SEmployee employeeServices = new SEmployee();
            var result = employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.IsManager).ToList();
            cmbApproveBy.DisplayMember = "Name";
            cmbApproveBy.ValueMember = "Id";
            cmbApproveBy.DataSource = result;
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

        private void displayDesignationWiseLeave(int? designationId)
        {
            if (designationId != null)
            {
                SDesignation designationServices = new SDesignation();
                var data = designationServices.ListLeaveQouta().Data.Where(x => x.DesignationId == designationId);
                var result = (from c in data
                              select new DropdownVm()
                              {
                                  Id = c.LeaveId,
                                  Name = c.Leave.Name
                              }).ToList();

                cmbLeaveType.DisplayMember = "Name";
                cmbLeaveType.ValueMember = "Id";
                cmbLeaveType.DataSource = result;
            }
            else
            {
                MessageBox.Show("Please mapped designation wise leave from Designation");
                return;
            }
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
                displayDesignationWiseLeave(employee.DesignationId);
            }
            else
            {
                MessageBox.Show("Invalid Employee Code..");
                displayEmployeeValue(new EEmployee());
            }
        }

        private void cmbLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SLeaveApplication leaveAppicationServices = new SLeaveApplication();

            int leaveMastId = cmbLeaveType.SelectedValue.ToInt();
            int empId = employee.Id;
            int fyId = RiddhaSession.fiscalYearId;
            if (fyId == 0)
            {
                MessageBox.Show("Current fiscal year is not set. Please set current fiscal year to continue.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal remLeave = leaveAppicationServices.GetRemBal(leaveMastId, empId, fyId).Data;
            if (remLeave != 0)
            {
                txtRemaningLeave.Text = remLeave.ToString();
            }
            else
            {
                MessageBox.Show("Please Asign Leave Balance And Then Continue.");
                return;
            }
        }

        private void LeaveApplicationFrm_Load(object sender, EventArgs e)
        {
            populateEmployeeForApproveByDropdown();
            populateLeaveApplicationGrid();
            cmbApproveBy.SelectedValue = 1;
            cmbLeaveDay.SelectedIndex = 0;
            cmbLeaveType.SelectedValue = 1;
        }

        private void populateLeaveApplicationGrid()
        {
            SEmployee employeeServices = new SEmployee();
            SLeaveApplication leaveApplicationServices = new SLeaveApplication();
            leaveApplicationGridView.DataSource = (from c in leaveApplicationServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                                                   join d in employeeServices.List().Data on c.ApprovedById equals d.Id
                                                   select new LeaveApplicationGridVm()
                                                   {
                                                       ApprovedById = c.ApprovedById,
                                                       ApprovedOn = c.ApprovedOn,
                                                       ApproveByUser = d.Name,
                                                       BranchId = c.BranchId,
                                                       CreatedById = c.CreatedById,
                                                       Description = c.Description,
                                                       EmployeeId = c.EmployeeId,
                                                       EmployeeName = c.Employee.Name,
                                                       From = GlobalParam.OperationDate == OperationDate.English ? c.From.ToString("yyyy/MM/dd") : c.From.ToNepaliDate(),
                                                       Id = c.Id,
                                                       LeaveDay = c.LeaveDay,
                                                       LeaveDayName = Enum.GetName(typeof(LeaveDay), c.LeaveDay),
                                                       LeaveMasterId = c.LeaveMasterId,
                                                       LeaveMasterName = c.LeaveMaster.Name,
                                                       LeaveStatus = c.LeaveStatus,
                                                       LeaveStatusName = Enum.GetName(typeof(LeaveStatus), c.LeaveStatus),
                                                       To = GlobalParam.OperationDate == OperationDate.English ? c.To.ToString("yyyy/MM/dd") : c.To.ToNepaliDate(),
                                                       TransactionDate = c.TransactionDate,
                                                       TotalLeaveDay = c.LeaveDay == LeaveDay.FullDay ? (c.To - c.From).Days + 1 : 0.5m
                                                   }).OrderBy(x => (int)(x.LeaveStatus)).ToList();
        }
        private void setInputValue()
        {
            rtxtDescription.Text = _LeaveApplicationData.Description;
            cmbLeaveDay.SelectedIndex = (int)_LeaveApplicationData.LeaveDay;
            MtbFrom.Text = _LeaveApplicationData.From.ToString("yyyy/MM/dd");
            mtbTo.Text = _LeaveApplicationData.To.ToString("yyyy/MM/dd");
            cmbLeaveType.SelectedValue = _LeaveApplicationData.LeaveMasterId;
            cmbApproveBy.SelectedValue = _LeaveApplicationData.ApprovedById;
        }

        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateLeaveApplicationGrid();
                createToolStripMenuItem_Click(null, null);
            }
        }
        public void setSelectedData()
        {
            var selectedRow = leaveApplicationGridView.Rows[leaveApplicationGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as LeaveApplicationGridVm;
            _LeaveApplicationData = new SLeaveApplication().List().Data.Where(x => x.Id == selectedData.Id).FirstOrDefault();
            _LeaveApplicationData.From = DateTime.Parse(selectedData.From);
            _LeaveApplicationData.To = DateTime.Parse(selectedData.To);
            employeeId = _LeaveApplicationData.EmployeeId;
            txtEmployeeCode_Leave(null, null);
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LeaveApplicationData = new ELeaveApplication();
            cmbApproveBy.SelectedValue = 1;
            cmbLeaveDay.SelectedIndex = 0;
            cmbLeaveType.SelectedValue = 1;
            ResetInupts();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (leaveApplicationGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_LeaveApplicationData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();

        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SLeaveApplication leaveApplicationServices = new SLeaveApplication();
            if (leaveApplicationGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_LeaveApplicationData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var leaveApplication = leaveApplicationServices.List().Data.Where(x => x.Id == _LeaveApplicationData.Id).FirstOrDefault();
                var result = leaveApplicationServices.Remove(leaveApplication);
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
            SEmployee employeeServices = new SEmployee();
            SLeaveApplication leaveApplicationServices = new SLeaveApplication();
            leaveApplicationGridView.DataSource = (from c in leaveApplicationServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.Employee.Name.StartsWith(txtSearch.Text)).ToList()
                                                   join d in employeeServices.List().Data on c.ApprovedById equals d.Id
                                                   select new LeaveApplicationGridVm()
                                                   {
                                                       ApprovedById = c.ApprovedById,
                                                       ApprovedOn = c.ApprovedOn,
                                                       ApproveByUser = d.Name,
                                                       BranchId = c.BranchId,
                                                       CreatedById = c.CreatedById,
                                                       Description = c.Description,
                                                       EmployeeId = c.EmployeeId,
                                                       EmployeeName = c.Employee.Name,
                                                       From = GlobalParam.OperationDate == OperationDate.English ? c.From.ToString("yyyy/MM/dd") : c.From.ToNepaliDate(),
                                                       Id = c.Id,
                                                       LeaveDay = c.LeaveDay,
                                                       LeaveDayName = Enum.GetName(typeof(LeaveDay), c.LeaveDay),
                                                       LeaveMasterId = c.LeaveMasterId,
                                                       LeaveMasterName = c.LeaveMaster.Name,
                                                       LeaveStatus = c.LeaveStatus,
                                                       LeaveStatusName = Enum.GetName(typeof(LeaveStatus), c.LeaveStatus),
                                                       To = GlobalParam.OperationDate == OperationDate.English ? c.To.ToString("yyyy/MM/dd") : c.To.ToNepaliDate(),
                                                       TransactionDate = c.TransactionDate,
                                                       //TotalLeaveDay = (c.To - c.From).Days + 1
                                                       TotalLeaveDay = c.LeaveDay == LeaveDay.FullDay ? (c.To - c.From).Days + 1 : 0.5m
                                                   }).OrderBy(x => (int)(x.LeaveStatus)).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SLeaveApplication leaveApplicationServices = new SLeaveApplication();
            if (_LeaveApplicationData.LeaveStatus != LeaveStatus.New)
            {
                MessageBox.Show("Approved , Rejected & Reverted Leave Application cannot be changed.");
                return;
            }
            else if (employeeId == 0)
            {
                MessageBox.Show("Please select employee.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnSearch.Focus();
                return;
            }
            else if (cmbLeaveType.SelectedValue == null)
            {
                MessageBox.Show("leave type is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbLeaveType.Focus();
                return;
            }
            else if (rtxtDescription.Text == "")
            {
                MessageBox.Show("Description is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtDescription.Focus();
                return;
            }
            else if (MtbFrom.Text == "    /  /")
            {
                MessageBox.Show("From date is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MtbFrom.Focus();
                return;
            }
            else if (cmbLeaveDay.SelectedIndex == 0)
            {
                if (mtbTo.Text == "    /  /")
                {
                    MessageBox.Show("To date is required.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mtbTo.Focus();
                    return;
                }

            }
            else if (cmbApproveBy.SelectedValue == null)
            {
                MessageBox.Show("ApproveBy user is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbApproveBy.Focus();
                return;
            }
            _LeaveApplicationData.BranchId = RiddhaSession.BranchId;
            _LeaveApplicationData.EmployeeId = employeeId;
            //_LeaveApplicationData.From = (MtbFrom.Text).ToDateTime();
            //_LeaveApplicationData.To = (mtbTo.Text).ToDateTime();
            _LeaveApplicationData.LeaveMasterId = cmbLeaveType.SelectedValue.ToInt();
            _LeaveApplicationData.CreatedById = RiddhaSession.UserId;
            _LeaveApplicationData.TransactionDate = DateTime.Now;
            _LeaveApplicationData.ApprovedById = cmbApproveBy.SelectedValue.ToInt();
            _LeaveApplicationData.ApprovedOn = null;
            _LeaveApplicationData.LeaveDay = (LeaveDay)Enum.ToObject(typeof(LeaveDay), cmbLeaveDay.SelectedIndex);
            _LeaveApplicationData.Description = rtxtDescription.Text;
            _LeaveApplicationData.LeaveStatus = 0;
            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    _LeaveApplicationData.From = (MtbFrom.Text).ToDateTime();
                    _LeaveApplicationData.To = (mtbTo.Text).ToDateTime();
                    break;
                case OperationDate.Nepali:
                    _LeaveApplicationData.From = (MtbFrom.Text).ToEnglishDate();
                    _LeaveApplicationData.To = (mtbTo.Text).ToEnglishDate();
                    break;
                default:
                    break;
            }
            if (_LeaveApplicationData.LeaveDay != LeaveDay.FullDay)
            {
                _LeaveApplicationData.To = _LeaveApplicationData.From;
            }
            else
            {
                _LeaveApplicationData.To = _LeaveApplicationData.To;
            }
            EleaveApplicationLog log = new EleaveApplicationLog();
            log.FiscalYearId = RiddhaSession.fiscalYearId;
            log.LeaveCount = (_LeaveApplicationData.To - _LeaveApplicationData.From).Days + 1;
            if (_LeaveApplicationData.Id == 0)
            {
                var result = leaveApplicationServices.Add(_LeaveApplicationData, log);
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
                var result = leaveApplicationServices.Update(_LeaveApplicationData);
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
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ResetInupts();
        }

        private void ResetInupts()
        {
            txtEmployeeCode.Text = "";
            txtEmployeeName.Text = "";
            txtDepartment.Text = "";
            txtDesignation.Text = "";
            txtRemaningLeave.Text = "";
            txtSection.Text = "";
            txtSearch.Text = "";
            rtxtDescription.Text = "";
            cmbLeaveType.SelectedValue = 1;
            cmbApproveBy.SelectedValue = 1;
            cmbLeaveDay.SelectedIndex = 0;
            MtbFrom.Text = "";
            mtbTo.Text = "";
            employeeId = 0;
        }

        private void LeaveApplicationFrm_KeyDown(object sender, KeyEventArgs e)
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

        private void cmbLeaveDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLeaveDay.SelectedIndex != 0)
            {
                mtbTo.ReadOnly = true;
            }
            else
            {
                mtbTo.ReadOnly = false;
            }
        }
    }
    public class DropdownVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
