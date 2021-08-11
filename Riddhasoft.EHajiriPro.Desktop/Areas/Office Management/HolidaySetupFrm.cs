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
using Riddhasoft.Globals.Conversion;
using Riddhasoft.Services.Common;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    public partial class HolidaySetupFrm : Form
    {
        private EHoliday holidayData = null;
        public List<HolidayDetailsVm> holidayDetailsItems = new List<HolidayDetailsVm>();
        private HolidayDetailsVm SelectedHolidayDetailsVm { get; set; }

        public HolidaySetupFrm()
        {
            InitializeComponent();
            holidayData = new EHoliday();
            PopulateFiscalYear();
            PopulateHolidayGrid();
            cmbHolidayType.SelectedIndex = 0;
            cmbApplicableGender.SelectedIndex = 0;
            cmbApplicableReligion.SelectedIndex = 0;

            DataGridViewButtonColumn editBtn = new DataGridViewButtonColumn();
            holidayDetailsGridView.Columns.Add(editBtn);
            editBtn.HeaderText = "Edit";
            editBtn.Text = "Edit";
            editBtn.Name = "btnEdit";
            editBtn.UseColumnTextForButtonValue = true;
            editBtn.Width = 50;

            DataGridViewButtonColumn deleteBtn = new DataGridViewButtonColumn();
            holidayDetailsGridView.Columns.Add(deleteBtn);
            deleteBtn.HeaderText = "Delete";
            deleteBtn.Text = "Delete";
            deleteBtn.Name = "btnDelete";
            deleteBtn.UseColumnTextForButtonValue = true;
            deleteBtn.Width = 50;
            btnAdd.Text = "Add";
            btnSave.Enabled = false;
        }

        public void PopulateHolidayGrid()
        {
            SHoliday holidayService = new SHoliday();
            var data = (from c in holidayService.GetHolidayList(RiddhaSession.BranchId, RiddhaSession.fiscalYearId).ToList()
                        select new HolidayGridVm()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            NameNp = c.NameNp,
                            ApplicableGender = c.ApplicableGender,
                            ApplicableGenderName = Enum.GetName(typeof(ApplicableGender), c.ApplicableGender),
                            ApplicableReligion = c.ApplicableReligion,
                            ApplicableReligionName = Enum.GetName(typeof(ApplicableReligion), c.ApplicableReligion),
                            BranchId = c.BranchId,
                            Description = c.Description,
                            HolidayType = c.HolidayType,
                            IsOccuredInSameDate = c.IsOccuredInSameDate,
                            Date = GlobalParam.OperationDate == OperationDate.English ? c.Date.ToString("yyyy/MM/dd") : c.Date.ToNepaliDate(),
                        }).ToList();
            holidayGridView.DataSource = data;
        }

        public void PopulateFiscalYear()
        {
            var fiscalYearServices = new SFiscalYear();
            var result = fiscalYearServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.CurrentFiscalYear).ToList();
            cmbFiscalYear.DisplayMember = "FiscalYear";
            cmbFiscalYear.ValueMember = "Id";
            cmbFiscalYear.DataSource = result;
        }

        private void HolidaySetupFrm_Load(object sender, EventArgs e)
        {
            txtHolidayName.Focus();
            cmbHolidayType.SelectedIndex = 0;
            cmbApplicableGender.SelectedIndex = 0;
            cmbApplicableReligion.SelectedIndex = 0;
        }

        private EFiscalYear getselectedFiscalYearFromCmb()
        {
            return (cmbFiscalYear.SelectedItem as EFiscalYear);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbFiscalYear.SelectedValue == null)
            {
                MessageBox.Show("Fiscal Year is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbFiscalYear.Focus();
                return;
            }
            var fiscalYear = getselectedFiscalYearFromCmb();
            SelectedHolidayDetailsVm = SelectedHolidayDetailsVm ?? new HolidayDetailsVm();
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    fromDate = (mtbFrom.Text).ToDateTime();
                    toDate = (mtbTo.Text).ToDateTime();
                    break;
                case OperationDate.Nepali:
                    fromDate = (mtbFrom.Text).ToEnglishDate();
                    toDate = (mtbTo.Text).ToEnglishDate();
                    break;
                default:
                    break;
            }
            HolidayDetailsVm vm = new HolidayDetailsVm()
            {
                Id = SelectedHolidayDetailsVm.Id,
                FiscalYearId = fiscalYear.Id,
                FiscalYear = fiscalYear.FiscalYear,
                From = GlobalParam.OperationDate == OperationDate.English ? fromDate.ToString("yyyy/MM/dd") : toDate.ToNepaliDate(),
                To = GlobalParam.OperationDate == OperationDate.English ? toDate.ToString("yyyy/MM/dd") : toDate.ToNepaliDate(),
            };
            int rowIndex = holidayDetailsItems.IndexOf(SelectedHolidayDetailsVm);
            if (rowIndex > -1)
            {
                holidayDetailsItems.RemoveAt(rowIndex);
                holidayDetailsItems.Insert(rowIndex, vm);
                holidayDetailsReset();
            }
            else
            {
                holidayDetailsItems.Add(vm);
                btnSave.Enabled = true;
                holidayDetailsReset();
            }
            refreshHolidayDetails();
        }

        private void refreshHolidayDetails()
        {

            holidayDetailsItems = (from c in holidayDetailsItems select c).ToList();
            holidayDetailsGridView.DataSource = holidayDetailsItems;
            holidayDetailsGridView.Refresh();
        }

        private void holidayDetailsReset()
        {
            cmbFiscalYear.SelectedValue = 0;
            mtbFrom.Text = "";
            mtbTo.Text = "";
            btnAdd.Text = "Add";
        }

        private void holidayDetailsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                SelectedHolidayDetailsVm = SelectedHolidayDetailsVm ?? new HolidayDetailsVm();
                if (holidayDetailsGridView.Rows.Count == 0)
                {
                    MessageBox.Show("There is not any data to edit.");
                    return;
                }
                setSelectedHolidayDetailData();
                if (SelectedHolidayDetailsVm.FiscalYearId == 0)
                {
                    MessageBox.Show("Please select row to edit.");
                    return;
                }
                btnAdd.Text = "Update";
                setHolidayDetailInputValue();
            }
            else if (e.ColumnIndex == 6)
            {
                SelectedHolidayDetailsVm = SelectedHolidayDetailsVm ?? new HolidayDetailsVm();
                if (holidayDetailsGridView.Rows.Count == 0)
                {
                    MessageBox.Show("There is not any data to delete.");
                    return;
                }
                setSelectedHolidayDetailData();
                if (SelectedHolidayDetailsVm.FiscalYearId == 0)
                {
                    MessageBox.Show("Please select row to delete.");
                    return;
                }
                if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    holidayDetailsItems.Remove(SelectedHolidayDetailsVm);
                    refreshHolidayDetails();
                }
            }
        }
        private void setSelectedHolidayDetailData()
        {
            var selectedRow = holidayDetailsGridView.Rows[holidayDetailsGridView.CurrentRow.Index];
            SelectedHolidayDetailsVm = selectedRow.DataBoundItem as HolidayDetailsVm;
        }
        private void setHolidayDetailInputValue()
        {
            SelectedHolidayDetailsVm = SelectedHolidayDetailsVm ?? new HolidayDetailsVm();
            cmbFiscalYear.SelectedValue = SelectedHolidayDetailsVm.FiscalYearId;
            mtbFrom.Text = SelectedHolidayDetailsVm.From;
            mtbTo.Text = SelectedHolidayDetailsVm.To;
        }

        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                PopulateHolidayGrid();
                Reset();
                createToolStripMenuItem_Click(null, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SNotification notificationServices = new SNotification();
            SHoliday holidayServices = new SHoliday();
            HolidayViewModel vm = new HolidayViewModel();
            vm.Holiday = new EHoliday();
            vm.Holiday.Id = holidayData.Id;
            vm.Holiday.Name = txtHolidayName.Text;
            vm.Holiday.ApplicableReligion = (ApplicableReligion)Enum.ToObject(typeof(ApplicableReligion), cmbApplicableReligion.SelectedIndex);
            vm.Holiday.ApplicableGender = (ApplicableGender)Enum.ToObject(typeof(ApplicableGender), cmbApplicableGender.SelectedIndex);
            vm.Holiday.HolidayType = (HolidayType)Enum.ToObject(typeof(HolidayType), cmbHolidayType.SelectedIndex);
            vm.Holiday.IsOccuredInSameDate = chkSameDate.Checked;
            vm.Holiday.Description = rtxtDescription.Text;
            vm.Holiday.BranchId = RiddhaSession.BranchId;

            foreach (var detail in holidayDetailsItems)
            {
                switch (GlobalParam.OperationDate)
                {
                    case OperationDate.English:
                        detail.From = detail.From.ToDateTime().ToString("yyyy/MM/dd");
                        detail.To = detail.To.ToDateTime().ToString("yyyy/MM/dd");
                        break;
                    case OperationDate.Nepali:
                        detail.From = detail.From.ToEnglishDate().ToString("yyyy/MM/dd");
                        detail.To = detail.To.ToEnglishDate().ToString("yyyy/MM/dd");
                        break;
                    default:
                        break;
                }
            }

            vm.HolidayDetails = (from c in holidayDetailsItems
                                 select new EHolidayDetails()
                                 {
                                     FiscalYearId = c.FiscalYearId,
                                     BeginDate = DateTime.Parse(c.From),
                                     EndDate = DateTime.Parse(c.To),

                                 }).ToList();
            if (holidayData.Id == 0)
            {
                var result = holidayServices.Add(vm);
                if (result.Status == ResultStatus.Ok)
                {
                    List<ENotification> lst = new List<ENotification>();
                    foreach (var item in result.Data.HolidayDetails)
                    {
                        ENotification notification = new ENotification();
                        notification.CompanyId = RiddhaSession.CompanyId;
                        notification.FiscalYearId = RiddhaSession.fiscalYearId;
                        notification.EffectiveDate = item.BeginDate;
                        notification.ExpiryDate = item.EndDate;
                        notification.Message = string.IsNullOrEmpty(result.Data.Holiday.Description) ? "There will be holiday of " + result.Data.Holiday.Name + " from " + item.BeginDate.ToString("yyyy/MM/dd") + " to " + item.EndDate.ToString("yyyy/MM/dd") : result.Data.Holiday.Description;
                        notification.NotificationType = NotificationType.Holiday;
                        notification.NotificationLevel = NotificationLevel.All;
                        notification.PublishDate = item.BeginDate.Date > DateTime.Now.Date ? item.BeginDate.AddDays(-1) : DateTime.Now;
                        notification.Title = vm.Holiday.Name;
                        notification.TranDate = DateTime.Now;
                        notification.TypeId = result.Data.Holiday.Id;
                        lst.Add(notification);
                    }
                    var notificationResult = notificationServices.AddRange(lst);
                }
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
                var result = holidayServices.Update(vm);
                List<ENotification> lst = new List<ENotification>();
                foreach (var item in result.Data.HolidayDetails)
                {
                    ENotification notification = new ENotification()
                    {
                        CompanyId = RiddhaSession.CompanyId,
                        FiscalYearId = RiddhaSession.fiscalYearId,
                        EffectiveDate = item.BeginDate,
                        ExpiryDate = item.EndDate,
                        Message = vm.Holiday.Description == null ? "There will be holiday of " + vm.Holiday.Name + " from " + item.BeginDate.ToString("yyyy/MM/dd") + " to " + item.EndDate.ToString("yyyy/MM/dd") : vm.Holiday.Description,
                        NotificationType = NotificationType.Holiday,
                        NotificationLevel = NotificationLevel.All,
                        PublishDate = item.BeginDate.Date > DateTime.Now.Date ? item.BeginDate.AddDays(-1) : DateTime.Now,
                        Title = vm.Holiday.Name,
                        TranDate = DateTime.Now,
                        TypeId = result.Data.Holiday.Id
                    };
                    lst.Add(notification);
                }
                var notificationResult = notificationServices.AddRange(lst);
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

        public void Reset()
        {
            holidayDetailsItems = new List<HolidayDetailsVm>();
            holidayDetailsGridView.DataSource = new List<HolidayDetailsVm>();
            txtHolidayName.Text = "";
            cmbApplicableGender.SelectedIndex = 0;
            cmbApplicableReligion.SelectedIndex = 0;
            cmbHolidayType.SelectedIndex = 0;
            rtxtDescription.Text = "";
            chkSameDate.Checked = false;
            cmbFiscalYear.SelectedValue = 1;
            mtbFrom.Text = "";
            mtbTo.Text = "";
        }

        private EHoliday setSelectedData()
        {
            var selectedRow = holidayGridView.Rows[holidayGridView.CurrentRow.Index];
            HolidayGridVm vm = selectedRow.DataBoundItem as HolidayGridVm;
            holidayData = new EHoliday()
            {
                Id = vm.Id,
                ApplicableGender = vm.ApplicableGender,
                ApplicableReligion = vm.ApplicableReligion,
                BranchId = vm.BranchId,
                Description = vm.Description,
                HolidayType = vm.HolidayType,
                IsOccuredInSameDate = vm.IsOccuredInSameDate,
                Name = vm.Name,
            };
            return holidayData;
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            holidayData = setSelectedData();
            holidayDetailsItems.Clear();
            if (holidayData == null || holidayData.Id == 0)
            {
                MessageBox.Show("Please Select Row to Edit.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SHoliday holidayServices = new SHoliday();
            var detail = holidayServices.ListDetails().Data.Where(x => x.HolidayId == holidayData.Id);
            txtHolidayName.Text = holidayData.Name;
            cmbApplicableGender.SelectedIndex = (int)holidayData.ApplicableGender;
            cmbApplicableReligion.SelectedIndex = (int)holidayData.ApplicableReligion;
            cmbHolidayType.SelectedIndex = (int)holidayData.HolidayType;
            rtxtDescription.Text = holidayData.Description;
            chkSameDate.Checked = holidayData.IsOccuredInSameDate;

            holidayDetailsItems = (from c in detail.ToList()
                                   select new HolidayDetailsVm()
                                   {
                                       FiscalYearId = c.FiscalYearId,
                                       From = c.BeginDate.ToString("yyyy/MM/dd"),
                                       HolidayId = c.HolidayId,
                                       Id = c.Id,
                                       NumberOfDays = c.NumberOfDays,
                                       To = c.EndDate.ToString("yyyy/MM/dd"),
                                       FiscalYear = c.FiscalYear.FiscalYear,
                                   }).ToList();
            if (holidayDetailsItems.Count > 0)
            {
                btnSave.Enabled = true;
            }
            refreshHolidayDetails();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SHoliday holidayServices = new SHoliday();
            EHoliday holiday = new EHoliday();
            if (holidayGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (holidayData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                holiday = holidayServices.List().Data.Where(x => x.Id == holidayData.Id).FirstOrDefault();
                var result = holidayServices.Remove(holiday);
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

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            holidayData = new EHoliday();
            cmbHolidayType.SelectedIndex = 0;
            cmbApplicableGender.SelectedIndex = 0;
            cmbApplicableReligion.SelectedIndex = 0;
            Reset();
        }

        private void HolidaySetupFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }


}
