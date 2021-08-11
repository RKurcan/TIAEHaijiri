using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
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

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Device
{
    public partial class DeviceSetupFrm : Form
    {
        EDevice _deviceData = null;
        SDevice _deviceServices = null;
        public DeviceSetupFrm()
        {
            InitializeComponent();
            _deviceData = new EDevice();
            _deviceServices = new SDevice();
            populateDeviceGrid();
            btnSave.Text = "Create";
        }
        private void populateDeviceGrid()
        {
            SDevice _deviceServices = new SDevice();
            var devices = _deviceServices.List().Data.ToList();
            var result = (from c in devices
                          select new DeviceGridVm()
                          {
                              Id = c.Id,
                              Name = c.Name,
                              CheckInOutIndex = c.CheckInOutIndex,
                              Company_Id = c.Company_Id,
                              DeviceType = c.DeviceType,
                              IpAddress = c.IpAddress,
                              LastActivity = c.LastActivity,
                              ModelId = c.ModelId,
                              ModelName = c.Model == null ? "" : c.Model.Name,
                              SerialNumber = c.SerialNumber,
                              Status = c.Status
                          }).ToList();
            deviceGridView.DataSource = result;
        }
        private void DeviceSetupFrm_Load(object sender, EventArgs e)
        {
            populateDeviceGrid();
            btnSave.Text = "Create";
        }
        private void setInputValue()
        {
            txtDeviceName.Text = _deviceData.Name;
            txtIPAddress.Text = _deviceData.IpAddress;
            txtSerialNumber.Text = _deviceData.SerialNumber;
        }
        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                MessageBox.Show(result.Message);
                populateDeviceGrid();
                createToolStripMenuItem_Click(null, null);
            }
        }
        public void setSelectedData()
        {
            var selectedRow = deviceGridView.Rows[deviceGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as DeviceGridVm;
            _deviceData = new EDevice()
            {
                Id = selectedData.Id,
                Name = selectedData.Name,
                CheckInOutIndex = selectedData.CheckInOutIndex,
                Company_Id = selectedData.Company_Id,
                DeviceType = selectedData.DeviceType,
                IpAddress = selectedData.IpAddress,
                //LastActivity = selectedData.LastActivity.ToString(),
                ModelId = selectedData.ModelId,
                SerialNumber = selectedData.SerialNumber,
                Status = selectedData.Status
            };
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _deviceData = new EDevice();
            setInputValue();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (deviceGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_deviceData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SDevice deviceServices = new SDevice();
            if (deviceGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.");
                return;
            }
            setSelectedData();
            if (_deviceData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.");
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var device = deviceServices.List().Data.Where(x => x.Id == _deviceData.Id).FirstOrDefault();
                var result = deviceServices.Remove(device);
                processResult<int>(result);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SDevice _deviceServices = new SDevice();
            var devices = _deviceServices.List().Data.Where(x => x.Name.StartsWith(txtSearch.Text) || x.IpAddress.StartsWith(txtSearch.Text)).ToList();
            var result = (from c in devices
                          select new CheckBoxListModel()
                          {
                              Id = c.Id,
                              Name = c.Name + "-" + c.IpAddress,
                          }).ToList();
            deviceGridView.DataSource = result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SDevice deviceServices = new SDevice();

            if (txtDeviceName.Text == "")
            {
                MessageBox.Show("Name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDeviceName.Focus();
                return;
            }
            else if (txtIPAddress.Text == "")
            {
                MessageBox.Show("Ip address is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIPAddress.Focus();
                return;
            }
            else if (txtSerialNumber.Text == "")
            {
                MessageBox.Show("Serial number is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSerialNumber.Focus();
                return;
            }

            _deviceData.Name = txtDeviceName.Text;
            _deviceData.IpAddress = txtIPAddress.Text;
            _deviceData.SerialNumber = txtSerialNumber.Text;
            if (_deviceData.Id == 0)
            {
                var result = deviceServices.Add(_deviceData);
                processResult(result);
            }
            else
            {
                var device = _deviceServices.List().Data.Where(x => x.Id == _deviceData.Id).FirstOrDefault();
                device.IpAddress = _deviceData.IpAddress;
                device.Name = _deviceData.Name;
                device.SerialNumber = _deviceData.SerialNumber;
                var result = deviceServices.Update(device);
                processResult(result);
                btnSave.Text = "Create";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtDeviceName.Text = "";
            txtIPAddress.Text = "";
            txtSearch.Text = "";
            txtSerialNumber.Text = "";
        }

        private void DeviceSetupFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
