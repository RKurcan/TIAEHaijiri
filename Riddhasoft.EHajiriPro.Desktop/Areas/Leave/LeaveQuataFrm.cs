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
using Riddhasoft.Services.Common;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    public partial class LeaveQuataFrm : Form
    {
        private DesignationGridVm selectedData;

        public LeaveQuataFrm(string empName)
        {
            InitializeComponent();
            PopulateLeaveQuataGrid();
        }

        public LeaveQuataFrm(DesignationGridVm selectedData)
        {
            // TODO: Complete member initialization
            this.Text += " Leave Quota For " + selectedData.Name;
            InitializeComponent();
            this.selectedData = selectedData;
            PopulateLeaveQuataGrid();

        }

        private void PopulateLeaveQuataGrid()
        {
            SLeaveMaster lm = new SLeaveMaster();
            int branchId = RiddhaSession.BranchId;
            SDesignation designationServices = new SDesignation();
            List<ELeaveMaster> leaveMastLst = new SLeaveMaster().List().Data.Where(x => x.BranchId == branchId).ToList();
            List<EDesignationWiseLeavedBalance> leaveQuotaLst = designationServices.ListLeaveQouta().Data.Where(c => c.DesignationId == selectedData.Id).ToList();
            var result = (from c in leaveMastLst
                          join p in leaveQuotaLst on c.Id equals p.LeaveId into ps
                          from j in ps.DefaultIfEmpty((new EDesignationWiseLeavedBalance()))
                          select new LeaveQuataGridVm()
                          {
                              LeaveName = c.Name,
                              Balance = j.Id == 0 ? c.Balance : j.Balance,
                              Id = j.Id,
                              LeaveMasterId = c.Id,
                              IsPaidLeave = j.Id == 0 ? c.IsPaidLeave : j.IsPaidLeave,
                              IsLeaveCarryable = j.Id == 0 ? c.IsLeaveCarryable : j.IsLeaveCarryable,
                              ApplicableGender = (int)j.ApplicableGender,
                              IsMapped = j.IsMapped,
                          }).ToList();

            leaveQuataGridView.DataSource = result;
        }

        private void leaveQuataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SDesignation designationServices = new SDesignation();
            List<LeaveQuataGridVm> list = (from c in leaveQuataGridView.DataSource as List<LeaveQuataGridVm>
                                           where c.IsMapped == true
                                           select c
                                              ).ToList();

            var data = (from c in list
                        select new EDesignationWiseLeavedBalance()
                        {
                            ApplicableGender = (ApplicableGender)Enum.ToObject(typeof(ApplicableGender), c.ApplicableGender),
                            Balance = c.Balance,
                            CreatedOn = DateTime.Now,
                            DesignationId = selectedData.Id,
                            IsLeaveCarryable = c.IsLeaveCarryable,
                            IsMapped = c.IsMapped,
                            IsPaidLeave = c.IsPaidLeave,
                            LeaveId = c.LeaveMasterId,
                            Id = c.Id
                        }).ToList();
            var result = designationServices.ApplyLeaveQuota(data);
            if (result.Status == ResultStatus.Ok)
            {
                MessageBox.Show("Leave Applied Successfully.", "Scuess",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LeaveQuataFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
