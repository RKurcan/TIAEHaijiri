using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddhasoft.Services.Common;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    public partial class LeaveApprovalFrm : Form
    {
        private int branchId = RiddhaSession.BranchId;
        public EEmployee employee { get; set; }
        private ELeaveApplication _LeaveApplicationData = null;
        private int employeeId;
        public LeaveApprovalFrm()
        {
            InitializeComponent();
            _LeaveApplicationData = new ELeaveApplication();
            populateLeaveApplicationGrid();
            btnApprove.Enabled = false;
            btnReject.Enabled = false;
            btnRevert.Enabled = false;
        }

        private void populateLeaveApplicationGrid()
        {
            SEmployee employeeServices = new SEmployee();
            SLeaveApplication leaveApplicationServices = new SLeaveApplication();
            leaveApprovalGridView.DataSource = (from c in leaveApplicationServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
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


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRow = leaveApprovalGridView.Rows[leaveApprovalGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as LeaveApplicationGridVm;
            _LeaveApplicationData = new SLeaveApplication().List().Data.Where(x => x.Id == selectedData.Id).FirstOrDefault();
            employeeId = _LeaveApplicationData.EmployeeId;
            if (employeeId != 0)
            {
                btnApprove.Enabled = true;
                btnReject.Enabled = true;
                btnRevert.Enabled = true;
            }
            setInputValue();
        }

        private void setInputValue()
        {
            SEmployee employeeServices = new SEmployee();
            rtxtDescription.Text = _LeaveApplicationData.Description;
            txtLeaveDay.Text = Enum.GetName(typeof(LeaveDay), _LeaveApplicationData.LeaveDay);
            txtLeaveType.Text = _LeaveApplicationData.LeaveMaster.Name;
            txtApproveBy.Text = employeeServices.List().Data.Where(x => x.Id == _LeaveApplicationData.ApprovedById).FirstOrDefault().Name;
            txtEmployeeName.Text = _LeaveApplicationData.Employee.Name;
            txtEmployeeCode.Text = _LeaveApplicationData.Employee.Code;
            txtDepartment.Text = _LeaveApplicationData.Employee.Section.Department.Name;
            txtSection.Text = _LeaveApplicationData.Employee.Section.Name;
            txtDesignation.Text = _LeaveApplicationData.Employee.Designation.Name;
            rtxtDescription.Text = _LeaveApplicationData.Description;

            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    MtbFrom.Text = _LeaveApplicationData.From.ToString("yyyy/MM/dd");
                    mtbTo.Text = _LeaveApplicationData.To.ToString("yyyy/MM/dd");
                    break;
                case OperationDate.Nepali:
                    MtbFrom.Text = _LeaveApplicationData.From.ToNepaliDate();
                    mtbTo.Text = _LeaveApplicationData.To.ToNepaliDate();
                    break;
                default:
                    break;
            }
        }
        private void Reset()
        {
            _LeaveApplicationData = new ELeaveApplication();
            txtEmployeeName.Text = "";
            txtEmployeeCode.Text = "";
            txtDepartment.Text = "";
            txtDesignation.Text = "";
            txtSection.Text = "";
            txtLeaveDay.Text = "";
            txtLeaveType.Text = "";
            txtSearch.Text = "";
            MtbFrom.Text = "";
            mtbTo.Text = "";
            txtApproveBy.Text = "";
            employeeId = 0;
            rtxtDescription.Text = "";
            btnApprove.Enabled = false;
            btnReject.Enabled = false;
            btnRevert.Enabled = false;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Reset();
            this.Close();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SEmployee employeeServices = new SEmployee();
            SLeaveApplication leaveApplicationServices = new SLeaveApplication();
            leaveApprovalGridView.DataSource = (from c in leaveApplicationServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.Employee.Name.StartsWith(txtSearch.Text)).ToList()
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
        private void btnApprove_Click(object sender, EventArgs e)
        {
            SLeaveApplication leaveAppicationServices = new SLeaveApplication();
            SNotification notificationServices = new SNotification();
            if (_LeaveApplicationData.Id == 0)
            {
                MessageBox.Show("Please select leave application to approve.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Reset();
                return;
            }
            if (RiddhaSession.fiscalYearId == 0)
            {
                MessageBox.Show("Fiscal Year is not set in application.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Reset();
                return;
            }
            if (_LeaveApplicationData.LeaveStatus == LeaveStatus.Approve)
            {
                MessageBox.Show("This leave application is already approved.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Reset();
                return;
            }
            if (_LeaveApplicationData.LeaveStatus == LeaveStatus.Revert)
            {
                MessageBox.Show("This leave application is already reverted.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Reset();
                return;
            }
            if (_LeaveApplicationData.LeaveStatus == LeaveStatus.Reject)
            {
                MessageBox.Show("This leave application is already rejected.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Reset();
                return;
            }
            var leaveApproval = leaveAppicationServices.List().Data.Where(x => x.Id == _LeaveApplicationData.Id && x.LeaveStatus == LeaveStatus.New).FirstOrDefault();
            string empName = "";
            empName = leaveApproval.Employee.Name;
            if (leaveApproval != null)
            {

                leaveApproval.LeaveStatus = LeaveStatus.Approve;
                leaveApproval.ApprovedOn = System.DateTime.Now;
                var result = leaveAppicationServices.Approve(leaveApproval, RiddhaSession.fiscalYearId);
                if (result.Status == ResultStatus.Ok)
                {
                    ENotification notification = new ENotification()
                    {
                        CompanyId = RiddhaSession.CompanyId,
                        FiscalYearId = RiddhaSession.fiscalYearId,
                        EffectiveDate = leaveApproval.From,
                        ExpiryDate = leaveApproval.From,
                        Message = (leaveApproval.LeaveMaster.Name + " that has been requested by " + empName + " from " + leaveApproval.From.ToString("yyyy/MM/dd") + " to " + leaveApproval.To.ToString("yyyy/MM/dd") + " has been approved"),
                        NotificationLevel = NotificationLevel.Employee,
                        NotificationType = NotificationType.Leave,
                        PublishDate = leaveApproval.From.Date > DateTime.Now.Date ? leaveApproval.From.AddDays(-1) : DateTime.Now,
                        Title = " Leave Approved for " + empName,
                        TranDate = DateTime.Now,
                        TypeId = leaveApproval.Id
                    };
                    int[] targets = new int[1];
                    targets[0] = leaveApproval.EmployeeId;
                    notificationServices.Add(notification, targets);
                    MessageBox.Show("Approve Successfully", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    populateLeaveApplicationGrid();
                    Reset();
                }

            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            SLeaveApplication leaveAppicationServices = new SLeaveApplication();
            SNotification notificationServices = new SNotification();
            if (_LeaveApplicationData.Id == 0)
            {
                MessageBox.Show("Please select leave application to reject.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Reset();
                return;
            }
            if (_LeaveApplicationData.LeaveStatus != LeaveStatus.New)
            {
                MessageBox.Show("Approved,Rejected & Reverted Leave Application cannot be changed.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Reset();
                return;
            }
            var leaveApplication = leaveAppicationServices.List().Data.Where(x => x.Id == _LeaveApplicationData.Id).FirstOrDefault();
            string empName = "";
            empName = leaveApplication.Employee.Name;
            if (leaveApplication != null)
            {
                leaveApplication.LeaveStatus = LeaveStatus.Reject;
                leaveApplication.ApprovedOn = System.DateTime.Now;
                var result = leaveAppicationServices.Reject(leaveApplication);
                if (result.Status == ResultStatus.Ok)
                {
                    ENotification notification = new ENotification()
                   {
                       CompanyId = RiddhaSession.CompanyId,
                       FiscalYearId = RiddhaSession.fiscalYearId,
                       EffectiveDate = leaveApplication.From,
                       ExpiryDate = leaveApplication.From,
                       Message = (leaveApplication.LeaveMaster.Name + " that has been requested by " + empName + " from " + leaveApplication.From.ToString("yyyy/MM/dd") + " to " + leaveApplication.To.ToString("yyyy/MM/dd") + " has been approved"),
                       NotificationLevel = NotificationLevel.Employee,
                       NotificationType = NotificationType.Leave,
                       PublishDate = leaveApplication.From.Date < DateTime.Now.Date ? leaveApplication.From.AddDays(-1) : DateTime.Now,
                       Title = " Leave Rejected for " + empName,
                       TranDate = DateTime.Now,
                       TypeId = leaveApplication.Id
                   };
                    int[] targets = new int[1];
                    targets[0] = leaveApplication.EmployeeId;
                    notificationServices.Add(notification, targets);
                    MessageBox.Show("Reject Successfully", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    populateLeaveApplicationGrid();
                    Reset();
                }
            }
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            SLeaveApplication leaveAppicationServices = new SLeaveApplication();
            var leave = leaveAppicationServices.List().Data.Where(x => x.Id == _LeaveApplicationData.Id).FirstOrDefault();
            if (leave != null)
            {
                var leavelog = leaveAppicationServices.ListLeaveAppLog().Data.Where(x => x.LeaveApplicationId == leave.Id).FirstOrDefault();
                var result = leaveAppicationServices.RemoveLeaveApplicationLog(leavelog);
                if (result.Status == ResultStatus.Ok)
                {
                    leave.Branch = null;
                    leave.CreatedBy = null;
                    leave.Employee = null;
                    leave.LeaveMaster = null;
                    leave.LeaveStatus = LeaveStatus.Revert;
                    leaveAppicationServices.Update(leave);
                };
                MessageBox.Show("Revert Successfully", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                populateLeaveApplicationGrid();
                Reset();
            }
        }

        private void LeaveApprovalFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
