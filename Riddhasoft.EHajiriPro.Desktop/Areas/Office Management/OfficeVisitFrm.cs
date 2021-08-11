using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.EHajiriPro.Desktop.Views;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
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

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    public partial class OfficeVisitFrm : Form
    {
        private int branchId = RiddhaSession.BranchId;
        public EEmployee employee { get; set; }
        private int employeeId;
        OfficeVisitModel vm = new OfficeVisitModel();
        TimeSpan fromT;
        TimeSpan toT;
        public OfficeVisitFrm()
        {
            InitializeComponent();
            vm.OfficeVisit = new EOfficeVisit();
            populateOfficeVisitGrid();
            tmpFromTime.Text = "00:00";
            tmpToTime.Text = "00:00";
            btnSearch.Focus();
            btnCreate.Text = "Create";
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

        private void populateOfficeVisitGrid()
        {
            SOfficeVisit officeVisitServices = new SOfficeVisit();
            var result = (from c in officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisit.BranchId == RiddhaSession.BranchId).ToList()
                          select new OfficeVisitGridVm()
                          {
                              ApprovedById = c.OfficeVisit.ApprovedById,
                              ApprovedOn = c.OfficeVisit.ApprovedOn,
                              BranchId = c.OfficeVisit.BranchId,
                              EmployeeId = c.EmployeeId,
                              EmployeeName = c.Employee.Name,
                              From = RiddhaSession.OpreationDate == "ne" ? c.OfficeVisit.From.ToNepaliDate() : c.OfficeVisit.From.ToString("yyyy/MM/dd"),
                              Id = c.OfficeVisit.Id,
                              IsApprove = c.OfficeVisit.IsApprove,
                              Remark = c.OfficeVisit.Remark,
                              To = RiddhaSession.OpreationDate == "ne" ? c.OfficeVisit.To.ToNepaliDate() : c.OfficeVisit.To.ToString("yyyy/MM/dd"),
                          }).ToList();
            officeVisitGridView.DataSource = result;
        }

        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateOfficeVisitGrid();
                createToolStripMenuItem_Click(null, null);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (employeeId == 0)
            {
                MessageBox.Show("Please select employee.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnSearch.Focus();
                return;
            }

            else if (mtbFromDate.Text == "    /  /")
            {
                MessageBox.Show("From date is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbFromDate.Focus();
                return;
            }

            else if (tmpFromTime.Text == "00:00")
            {
                MessageBox.Show("From time is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbFromDate.Focus();
                return;
            }

            else if (mtbToDate.Text == "    /  /")
            {
                MessageBox.Show("To date is required.", "Warning",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbToDate.Focus();
                return;
            }

            else if (tmpToTime.Text == "00:00")
            {
                MessageBox.Show("To time is required.", "Warning",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tmpToTime.Focus();
                return;
            }

            else if (rtxtRemark.Text == "")
            {
                MessageBox.Show("Remark is required.", "Warning",
                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtRemark.Focus();
                return;
            }

            TimeSpan fromTime = tmpFromTime.Text.ToTimeSpan();
            DateTime fromDate = new DateTime();

            TimeSpan toTime = tmpToTime.Text.ToTimeSpan();
            DateTime toDate = new DateTime();
            List<int> array = new List<int>();
            array.Add(employeeId);

            int masterId = vm.OfficeVisit.Id;
            vm.OfficeVisit = new EOfficeVisit();
            vm.OfficeVisit.Id = masterId;
            vm.OfficeVisit.BranchId = RiddhaSession.BranchId;
            vm.OfficeVisit.OfficeVisitStatus = OfficeVisitStatus.New;
            vm.OfficeVisit.Remark = rtxtRemark.Text;
            vm.EmpIds = array.ToArray();
            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    fromDate = mtbFromDate.Text.ToDateTime();
                    toDate = mtbToDate.Text.ToDateTime();
                    break;
                case OperationDate.Nepali:
                    fromDate = mtbFromDate.Text.ToEnglishDate();
                    toDate = mtbToDate.Text.ToEnglishDate();
                    break;
                default:
                    break;
            }
            vm.OfficeVisit.From = fromDate.Add(fromTime);
            vm.OfficeVisit.To = toDate.Add(toTime);
            if (vm.OfficeVisit.Id == 0)
            {
                SOfficeVisit officeVisitServices = new SOfficeVisit();
                var result = officeVisitServices.Add(vm);
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
                SOfficeVisit officeVisitServices = new SOfficeVisit();
                vm.OfficeVisit.Branch = null;
                var result = officeVisitServices.Update(vm);
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

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vm.OfficeVisit = new EOfficeVisit();
            resetInputs();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRow = officeVisitGridView.Rows[officeVisitGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as OfficeVisitGridVm;
            if (selectedData.IsApprove)
            {
                MessageBox.Show("Approved data cannot be edited.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (officeVisitGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            setSelectedData();
            if (vm.OfficeVisit.Id == 0)
            {
                MessageBox.Show("Please select row to edit.", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            setInputValue();
            btnCreate.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SOfficeVisit officeServices = new SOfficeVisit();
            var selectedRow = officeVisitGridView.Rows[officeVisitGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as OfficeVisitGridVm;
            if (selectedData.IsApprove)
            {
                MessageBox.Show("Approved data cannot be deleted.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (officeVisitGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            setSelectedData();
            if (vm.OfficeVisit.Id == 0)
            {
                MessageBox.Show("Please select row to delete.", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var officeVisit = officeServices.List().Data.Where(x => x.Id == vm.OfficeVisit.Id).FirstOrDefault();
                var result = officeServices.Remove(officeVisit);
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
        public void setSelectedData()
        {
            SOfficeVisit officeVisitServices = new SOfficeVisit();
            var selectedRow = officeVisitGridView.Rows[officeVisitGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as OfficeVisitGridVm;
            var data = officeVisitServices.List().Data.Where(x => x.Id == selectedData.Id).FirstOrDefault();
            string fromTime = data.From.TimeOfDay.ToString(@"hh\:mm");
            string toTIme = data.To.TimeOfDay.ToString(@"hh\:mm");
            if (RiddhaSession.OpreationDate == "ne")
            {
                data.From = DateTime.Parse(selectedData.From);
                data.To = DateTime.Parse(selectedData.To);
            }
            fromT = fromTime.ToTimeSpan();
            toT = toTIme.ToTimeSpan();
            vm.OfficeVisit = data;
            int empId = officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisitId == data.Id).FirstOrDefault().EmployeeId;
            List<int> array = new List<int>();
            array.Add(empId);
            vm.EmpIds = array.ToArray();
            employeeId = empId;
            txtEmployeeCode_Leave(null, null);
        }

        private void setInputValue()
        {
            mtbFromDate.Text = vm.OfficeVisit.From.ToString("yyyy/MM/dd");
            tmpFromTime.Text = fromT.ToString(@"hh\:mm");
            mtbToDate.Text = vm.OfficeVisit.To.ToString("yyyy/MM/dd");
            tmpToTime.Text = toT.ToString(@"hh\:mm");
            rtxtRemark.Text = vm.OfficeVisit.Remark;
        }
        private void resetInputs()
        {
            txtEmployeeName.Text = "";
            txtEmployeeCode.Text = "";
            txtSection.Text = "";
            txtDepartment.Text = "";
            txtDesignation.Text = "";
            mtbFromDate.Text = "";
            tmpFromTime.Text = "00:00";
            mtbToDate.Text = "";
            tmpToTime.Text = "00:00";
            rtxtRemark.Text = "";
            employeeId = 0;
            btnSearch.Focus();
        }

        private void OfficeVisitFrm_Load(object sender, EventArgs e)
        {
            tmpFromTime.Text = "00:00";
            tmpToTime.Text = "00:00";
            populateOfficeVisitGrid();
            btnSearch.Focus();
            btnCreate.Text = "Create";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetInputs();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SOfficeVisit officeVisitServices = new SOfficeVisit();
            var result = (from c in officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisit.BranchId == RiddhaSession.BranchId && x.Employee.Name.StartsWith(txtSearch.Text)).ToList()
                          select new OfficeVisitGridVm()
                          {
                              ApprovedById = c.OfficeVisit.ApprovedById,
                              ApprovedOn = c.OfficeVisit.ApprovedOn,
                              BranchId = c.OfficeVisit.BranchId,
                              EmployeeId = c.EmployeeId,
                              EmployeeName = c.Employee.Name,
                              Id = c.OfficeVisit.Id,
                              IsApprove = c.OfficeVisit.IsApprove,
                              Remark = c.OfficeVisit.Remark,
                              From = RiddhaSession.OpreationDate == "ne" ? c.OfficeVisit.From.ToNepaliDate() : c.OfficeVisit.From.ToString("yyyy/MM/dd"),
                              To = RiddhaSession.OpreationDate == "ne" ? c.OfficeVisit.To.ToNepaliDate() : c.OfficeVisit.To.ToString("yyyy/MM/dd"),
                          }).ToList();
            officeVisitGridView.DataSource = result;
        }

        private void OfficeVisitFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
