using Riddhasoft.Employee.Entities;
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
using Riddhasoft.Globals.Conversion;
using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    public partial class FiscalYearFrm : Form
    {
        EFiscalYear _fiscalYearData = null;
        public FiscalYearFrm(EventHandler Exit)
        {
            InitializeComponent();

            _fiscalYearData = new EFiscalYear();
            populateFiscalYearInGrid();
            btnSave.Text = "Create";
        }
        private void populateFiscalYearInGrid()
        {
            SFiscalYear fiscalYearServices = new SFiscalYear();
            fiscalYearGridView.DataSource = (from c in fiscalYearServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                                             select new FiscalYearGrdiVm()
                                             {
                                                 Id = c.Id,
                                                 BranchId = c.BranchId,
                                                 CurrentFiscalYear = c.CurrentFiscalYear,
                                                 EndDate = GlobalParam.OperationDate == OperationDate.English ? c.EndDate.ToString("yyyy/MM/dd") : c.EndDate.ToNepaliDate(),
                                                 FiscalYear = c.FiscalYear,
                                                 StartDate = GlobalParam.OperationDate == OperationDate.English ? c.StartDate.ToString("yyyy/MM/dd") : c.StartDate.ToNepaliDate(),
                                             }).ToList();
        }
        private void setInputValue()
        {
            txtFY.Text = _fiscalYearData.FiscalYear;
            chkCurrentFY.Checked = _fiscalYearData.CurrentFiscalYear;

            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    FromMtb.Text = _fiscalYearData.StartDate.ToString("yyyy/MM/dd");
                    ToMtb.Text = _fiscalYearData.EndDate.ToString("yyyy/MM/dd");
                    break;
                case OperationDate.Nepali:
                    FromMtb.Text = _fiscalYearData.StartDate.ToNepaliDate();
                    ToMtb.Text = _fiscalYearData.EndDate.ToNepaliDate();
                    break;
                default:
                    break;
            }

        }
        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateFiscalYearInGrid();
                createToolStripMenuItem_Click(null, null);
                ResetInupts();
            }
        }
        public void setSelectedData()
        {
            var selectedRow = fiscalYearGridView.Rows[fiscalYearGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as FiscalYearGrdiVm;
            _fiscalYearData = new EFiscalYear()
            {
                BranchId = selectedData.BranchId,
                CurrentFiscalYear = selectedData.CurrentFiscalYear,
                FiscalYear = selectedData.FiscalYear,
                Id = selectedData.Id,
            };
            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    _fiscalYearData.StartDate = selectedData.StartDate.ToDateTime();
                    _fiscalYearData.EndDate = selectedData.EndDate.ToDateTime();
                    break;
                case OperationDate.Nepali:
                    _fiscalYearData.StartDate = selectedData.StartDate.ToEnglishDate();
                    _fiscalYearData.EndDate = selectedData.EndDate.ToEnglishDate();
                    break;
                default:
                    break;
            }
        }
        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fiscalYearData = new EFiscalYear();
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fiscalYearGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_fiscalYearData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFiscalYear fiscalYearServices = new SFiscalYear();
            if (fiscalYearGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_fiscalYearData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var data = fiscalYearServices.List().Data.Where(x => x.Id == _fiscalYearData.Id).FirstOrDefault();
                if (data.CurrentFiscalYear)
                {
                    MessageBox.Show("Current fiscal year cannot be deleted.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var result = fiscalYearServices.Remove(data);
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

        private void Searchtxt_KeyUp(object sender, KeyEventArgs e)
        {
            SFiscalYear fiscalYearServices = new SFiscalYear();
            fiscalYearGridView.DataSource = fiscalYearServices.List().Data.Where(x => x.FiscalYear.StartsWith(Searchtxt.Text)).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SFiscalYear fiscalYearServices = new SFiscalYear();
            if (txtFY.Text == "")
            {
                MessageBox.Show("Fiscal Year is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFY.Focus();
                return;
            }
            else if (FromMtb.Text == "    /  /")
            {
                MessageBox.Show("From Date is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FromMtb.Focus();
                return;
            }
            else if (ToMtb.Text == "    /  /")
            {
                MessageBox.Show("To Date is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ToMtb.Focus();
                return;
            }

            _fiscalYearData = new EFiscalYear()
            {
                Id = _fiscalYearData.Id,
                FiscalYear = txtFY.Text,
                BranchId = RiddhaSession.BranchId,
                CurrentFiscalYear = chkCurrentFY.Checked,
            };
            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    _fiscalYearData.StartDate = (FromMtb.Text).ToDateTime();
                    _fiscalYearData.EndDate = (ToMtb.Text).ToDateTime();
                    break;
                case OperationDate.Nepali:
                    _fiscalYearData.StartDate = (FromMtb.Text).ToEnglishDate();
                    _fiscalYearData.EndDate = (ToMtb.Text).ToEnglishDate();
                    break;
                default:
                    break;
            }
            if (_fiscalYearData.Id == 0)
            {
                if (_fiscalYearData.CurrentFiscalYear)
                {
                    var existingCurrentFiscalYear = fiscalYearServices.List().Data.Where(x => x.CurrentFiscalYear).FirstOrDefault();
                    if (existingCurrentFiscalYear != null)
                    {
                        existingCurrentFiscalYear.CurrentFiscalYear = false;
                        fiscalYearServices.Update(existingCurrentFiscalYear);
                    }
                }
                var result = fiscalYearServices.Add(_fiscalYearData);
                //Update Session Fiscal Year Id
                processResult(result);
                if (result.Status == ResultStatus.Ok)
                {
                    if (result.Data.CurrentFiscalYear)
                    {
                        RiddhaSession.fiscalYearId = result.Data.Id;
                    }
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
                var result = fiscalYearServices.Update(_fiscalYearData);
                //Update Session Fiscal Year Id
                processResult(result);
                if (result.Status == ResultStatus.Ok)
                {
                    if (result.Data.CurrentFiscalYear)
                    {
                        RiddhaSession.fiscalYearId = result.Data.Id;
                    }
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

        private void FiscalYearFrm_Load(object sender, EventArgs e)
        {
            btnSave.Text = "Create";
            txtFY.Focus();
            populateFiscalYearInGrid();
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fiscalYearGridView.ToExcel();
        }

        private void ResetInupts()
        {
            txtFY.Text = "";
            FromMtb.Text = "";
            ToMtb.Text = "";
            chkCurrentFY.Checked = false;
        }

        private void FiscalYearFrm_KeyDown(object sender, KeyEventArgs e)
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
