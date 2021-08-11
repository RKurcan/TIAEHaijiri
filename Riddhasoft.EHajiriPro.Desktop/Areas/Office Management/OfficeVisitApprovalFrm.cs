using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
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
    public partial class OfficeVisitApprovalFrm : Form
    {
        OfficeVisitModel vm = new OfficeVisitModel();
        public EEmployee employee { get; set; }
        private int employeeId;
        TimeSpan fromT;
        TimeSpan toT;
        public OfficeVisitApprovalFrm()
        {
            InitializeComponent();
            btnApprove.Enabled = false;
            btnReject.Enabled = false;
            populateOfficeVisitApprovalGrid();
            vm.OfficeVisit = new EOfficeVisit();
            tmpFromTime.Text = "00:00";
            tmpToTime.Text = "00:00";
        }
        private void populateOfficeVisitApprovalGrid()
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
                              Status = Enum.GetName(typeof(OfficeVisitStatus), c.OfficeVisit.OfficeVisitStatus)
                          }).ToList();
            officeVisitApprovalGridView.DataSource = result;
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (officeVisitApprovalGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to view.");
                return;
            }
            setSelectedData();
            if (vm.OfficeVisit.Id == 0)
            {
                MessageBox.Show("Please select row to view.");
                return;
            }
            setInputValue();
        }
        public void setSelectedData()
        {
            SOfficeVisit officeVisitServices = new SOfficeVisit();
            var selectedRow = officeVisitApprovalGridView.Rows[officeVisitApprovalGridView.CurrentRow.Index];
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

        private void txtEmployeeCode_Leave(object sender, EventArgs e)
        {
            SEmployee service = new SEmployee();
            if (employeeId != 0)
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
                btnApprove.Enabled = true;
                btnReject.Enabled = true;
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
        private void setInputValue()
        {
            mtbFromDate.Text = vm.OfficeVisit.From.ToString("yyyy/MM/dd");
            tmpFromTime.Text = fromT.ToString(@"hh\:mm");
            mtbToDate.Text = vm.OfficeVisit.To.ToString("yyyy/MM/dd");
            tmpToTime.Text = toT.ToString(@"hh\:mm");
            rtxtRemark.Text = vm.OfficeVisit.Remark;
        }

        private void OfficeVisitApprovalFrm_Load(object sender, EventArgs e)
        {
            tmpFromTime.Text = "00:00";
            tmpToTime.Text = "00:00";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (vm.OfficeVisit.Id != 0)
            {
                if (vm.OfficeVisit.IsApprove)
                {
                    MessageBox.Show("Already Approved", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                SOfficeVisit officeVisitServices = new SOfficeVisit();
                var officeVisit = officeVisitServices.List().Data.Where(x => x.Id == vm.OfficeVisit.Id).FirstOrDefault();
                if (officeVisit != null)
                {
                    officeVisit.IsApprove = true;
                    officeVisit.OfficeVisitStatus = OfficeVisitStatus.Approve;
                    officeVisit.ApprovedById = RiddhaSession.UserId;
                    officeVisit.ApprovedOn = DateTime.Now;
                    var result = officeVisitServices.Approve(officeVisit);
                    if (result.Status == ResultStatus.Ok)
                    {
                        populateOfficeVisitApprovalGrid();
                        resetInputs();
                        MessageBox.Show("Approve Sucessfully", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            resetInputs();
        }
        private void resetInputs()
        {
            vm.OfficeVisit = new EOfficeVisit();
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
            btnApprove.Enabled = false;
            btnReject.Enabled = false;
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
                              From = c.OfficeVisit.From.ToString("yyyy/MM/dd"),
                              Id = c.OfficeVisit.Id,
                              IsApprove = c.OfficeVisit.IsApprove,
                              Remark = c.OfficeVisit.Remark,
                              To = c.OfficeVisit.To.ToString("yyyy/MM/dd")
                          }).ToList();
            officeVisitApprovalGridView.DataSource = result;
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (vm.OfficeVisit.Id != 0)
            {
                if (vm.OfficeVisit.IsApprove)
                {
                    MessageBox.Show("Already Approved can't reject.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (vm.OfficeVisit.OfficeVisitStatus == OfficeVisitStatus.Reject)
                {
                    MessageBox.Show("Already Rejected.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                SOfficeVisit officeVisitServices = new SOfficeVisit();
                var officeVisit = officeVisitServices.List().Data.Where(x => x.Id == vm.OfficeVisit.Id).FirstOrDefault();
                if (officeVisit != null)
                {

                    officeVisit.OfficeVisitStatus = OfficeVisitStatus.Reject;
                    var result = officeVisitServices.Reject(officeVisit);
                    if (result.Status == ResultStatus.Ok)
                    {
                        populateOfficeVisitApprovalGrid();
                        resetInputs();
                        MessageBox.Show("Reject Sucessfully", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void OfficeVisitApprovalFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
