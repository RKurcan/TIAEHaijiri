using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.FingerKeeper.Extension;
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
using Riddhasoft.Attendance.Entities;
using Riddhasoft.Attendance.Services;
using Riddhasoft.Employee.Services;
using Riddhasoft.Employee.Entities;
using System.Threading;
using System.Data.Entity;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Device
{
    public partial class DeviceDataManagementFrm : Form
    {
        public EDevice Device { get; set; }
        zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
        List<FingerPrint> fps = new List<FingerPrint>();
        List<EAttendanceLog> glogdatas = new List<EAttendanceLog>();
        List<FingerPrint> reservedFingerPrints = new List<FingerPrint>();
        public DeviceDataManagementFrm()
        {
            InitializeComponent();
            lblMessage.BringToFront();
            lblMessage.Text = "Show me ";
            populateMachineToCheckBox();
        }
        public void ClearSystemLabelTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10000; // 10 second
            timer.Tick += new EventHandler(ClearLabel);
            timer.Start();
        }
        public void ClearLabel(object sender, EventArgs e)
        {
            lblMessage.Text = "";
        }

        private void populateMachineToCheckBox()
        {
            SDevice _deviceServices = new SDevice();
            var devices = _deviceServices.List().Data.ToList();
            var result = (from c in devices
                          select new CheckBoxListModel()
                          {
                              Id = c.Id,
                              Name = c.Name + "-" + c.IpAddress,
                          }).ToList();
            deviceChkboxList.BindData(result);
        }

        private void DeviceDataManagementFrm_Load(object sender, EventArgs e)
        {
            populateMachineToCheckBox();
            lblMessage.BringToFront();
            lblMessage.Text = "Show me ";
        }

        private void chkSelectAllMachines_CheckedChanged(object sender, EventArgs e)
        {
            deviceChkboxList.CheckAll(chkSelectAllMachines.Checked);
            deviceChkboxList.DoThisForCheckedItem(DothisTest);
        }
        private void DothisTest(CheckBoxListModel model)
        {

        }

        private bool noDeviceSelected()
        {
            return Device == null;
        }
        void ConnectDevice()
        {
            var status = axCZKEM1.Connect_Net(Device.IpAddress, Convert.ToInt32(4370));
        }
        void DisconnectDevice()
        {
            //Loading.CloseLoading();
            ZKemKeeperConnection con = new ZKemKeeperConnection(axCZKEM1);
            con.Disconnect(Device.IpAddress, 4370);
        }
        private void SetSelectedDevice()
        {
            deviceChkboxList.DoThisForCheckedItem(GetCurrentDevices);
            if (Device == null)
            {
                lblMessage.Text = "Please choose a machine";
                ClearSystemLabelTimer();
                return;
            }
        }
        private void GetCurrentDevices(CheckBoxListModel model)
        {
            SDevice service = new SDevice();
            Device = service.List().Data.Where(x => x.Id == model.Id).FirstOrDefault();
        }
        private void btnGetTime_Click(object sender, EventArgs e)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {
                return;
            }
            ConnectDevice();
            ZKTDeviceMangement mgr = new ZKTDeviceMangement(axCZKEM1);
            lblMessage.Text = mgr.GetDeviceTime(Device.SerialNumber.ToInt());
            ClearSystemLabelTimer();
            DisconnectDevice();
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {

                return;
            }
            ConnectDevice();
            ZKTDeviceMangement mgr = new ZKTDeviceMangement(axCZKEM1);
            if (mgr.SetDeviceTime(Device.SerialNumber.ToInt()))
            {
                lblMessage.Text = "Set Time Successfully";
                ClearSystemLabelTimer();
            }
            DisconnectDevice();
        }

        private void btnDownloadNewLog_Click(object sender, EventArgs e)
        {
            dowloadLog(true);
        }

        private void dowloadLog(bool newlog)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {
                return;
            }
            ConnectDevice();
            SAttendanceLog glogServices = new SAttendanceLog();
            SDevice deviceServices = new SDevice();
            ZkemkeeperAttendanceLog AttendanceLog = new ZkemkeeperAttendanceLog(axCZKEM1);
            DataTable dt = AttendanceLog.GetAllAttendanceLogData(Device.Id.ToInt());
            fps.Clear();
            fps = (from DataRow drow in dt.Rows
                   select new FingerPrint()
                   {
                       Id = drow.Field<int>("Id"),
                       Name = drow.Field<string>("Name"),
                       UserId = drow.Field<string>("UserId"),
                       FingerPrintId = drow.Field<string>("FingerPrintId")
                   }
                     ).ToList();

            Device.LastActivity = Device.LastActivity ?? new DateTime(2019, 01, 01);
            SEmployee service = new SEmployee();
            var employees = service.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId);


            var attLogs = (from c in fps
                           join d in employees
                           on c.UserId.ToInt() equals d.DeviceCode
                           select new EAttendanceLog()
                           {
                               EmployeeId = d.Id,
                               DeviceId = Device.Id,
                               DateTime = c.FingerPrintId.ToDateTime(),
                               //CompanyCode='RS'
                               VerifyMode = 0
                           }).ToList();



            if (newlog == true)
            {
                fps = fps.Where(x => x.FingerPrintId.ToDateTime() > Device.LastActivity).ToList();
            }
            glogdatas.AddRange(attLogs);
            List<EAttendanceLog> logToSave = new List<EAttendanceLog>();
            if (chkValidateLastActivityDate.Checked)
            {
                logToSave = attLogs.Where(x => x.DateTime > Device.LastActivity).ToList();
            }
            else
            {
                logToSave = attLogs.ToList();
            }
            if (logToSave.Count > 0)
            {
                glogServices.AddRange(logToSave);

                Device.LastActivity = logToSave.OrderByDescending(x => x.DateTime).First().DateTime;
                //update device
                deviceServices.Update(Device);
            }
            templateGridView.DataSource = fps;
            DisconnectDevice();
        }

        private void btnDownloadAllLog_Click(object sender, EventArgs e)
        {
            dowloadLog(false);
        }

        private void btnSetAdmin_Click(object sender, EventArgs e)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {
                return;
            }
            ConnectDevice();
            setAdminZktEco();
        }
        private void setAdminZktEco()
        {
            int idwErrorCode = 0;
            axCZKEM1.EnableDevice(Device.SerialNumber.ToInt(), false);
            foreach (DataGridViewRow item in templateGridView.Rows)
            {
                var employeeData = item.DataBoundItem as FingerPrint;
                if (employeeData.Select)
                {
                    if (!(axCZKEM1.SSR_SetUserInfo(Device.SerialNumber.ToInt(), employeeData.UserId, employeeData.Name, employeeData.UserPassword.ToString(), 2, true)))
                    {
                        axCZKEM1.GetLastError(ref idwErrorCode);

                    }
                    lblMessage.Text = "Admin set successfull...!!!";
                    ClearSystemLabelTimer();
                }
            }
            axCZKEM1.EnableDevice(Device.SerialNumber.ToInt(), true);
        }

        private void btnClearAdmin_Click(object sender, EventArgs e)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {

                return;
            }
            ConnectDevice();
            ClearAdminZKTEco();
        }
        private void ClearAdminZKTEco()
        {
            int idwErrorCode = 0;

            Cursor = Cursors.WaitCursor;
            if (axCZKEM1.ClearAdministrators(Device.SerialNumber.ToInt()))
            {
                axCZKEM1.RefreshData(Device.SerialNumber.ToInt());//the data in the device should be refreshed
                lblMessage.Text = ("Successfully clear administrator privilege from teiminal!");
                ClearSystemLabelTimer();
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                lblMessage.Text = ("Operation failed,ErrorCode=" + idwErrorCode.ToString());
                ClearSystemLabelTimer();
            }
            Cursor = Cursors.Default;
        }

        private void btnDownloadEnrollment_Click(object sender, EventArgs e)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {
                return;
            }
            ConnectDevice();
            fps.Clear();
            ZKTUser user = new ZKTUser(axCZKEM1);
            DataTable dt = user.GetTemplate(Device.SerialNumber.ToInt());
            foreach (DataRow drow in dt.Rows)
            {
                fps.Add(
                         new FingerPrint()
                         {
                             UserId = drow[0].ToString(),
                             Name = "",
                             FingerPrintId = drow["FingerPrintId"].ToString(),
                             sTmpDate = drow["FingerTemplate"].ToString(),
                             Previledge = drow["Previledge"].ToInt()
                         });
            }

            SEmployee service = new SEmployee();
            var employees = service.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();

            fps = (from c in fps
                   join d in employees
                   on c.UserId.ToInt() equals d.DeviceCode into subpet
                   from j in subpet.DefaultIfEmpty(new EEmployee() { Name = "" })
                   select new FingerPrint()
                   {
                       UserId = c.UserId,
                       Name = string.IsNullOrEmpty((j.Name.Trim())) ? "Unfilled Employee" : j.Name,
                       FingerPrintId = c.FingerPrintId,
                       sTmpDate = c.sTmpDate,
                       Previledge = c.Previledge,
                       UserPassword = c.UserPassword
                   }).OrderByDescending(x => x.sTmpDate).ToList();

            templateGridView.DataSource = fps;
            reservedFingerPrints.AddRange(fps);
            DisconnectDevice();
        }

        private void btnUploadEnrollment_Click(object sender, EventArgs e)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {
                return;
            }
            ConnectDevice();
            SetEnrollData();
            DisconnectDevice();
        }

        private void SetEnrollData()
        {
            #region zkt setTemplate
            int idwErrorCode = 0;

            string sdwEnrollNumber = "";
            int idwFingerIndex = 0;
            string sTmpData = "";
            int iPrivilege = 0;
            string sPassword = "";
            bool bEnabled = true;
            int iFlag = 1;

            int iUpdateFlag = 1;

            var MachineNumber = Device.SerialNumber.ToInt();
            axCZKEM1.EnableDevice(MachineNumber, false);
            if (axCZKEM1.BeginBatchUpdate(MachineNumber, iUpdateFlag))//create memory space for batching data
            {
                string sLastEnrollNumber = "";//the former enrollnumber you have upload(define original value as 0)
                foreach (DataGridViewRow Drow in templateGridView.Rows)
                {
                    var selectedRow = Drow.DataBoundItem as FingerPrint;
                    if (selectedRow.Select)
                    {



                        var curList = (from c in fps
                                       where c.UserId == selectedRow.UserId && c.FingerPrintId == selectedRow.FingerPrintId
                                       select c
                                         ).ToList();
                        #region  loop Employee data
                        foreach (FingerPrint data in fps)
                        {
                            var employeeData = data as FingerPrint;
                            sdwEnrollNumber = employeeData.UserId;
                            sTmpData = employeeData.sTmpDate;
                            iPrivilege = employeeData.Previledge;
                            //  sPassword = batchtatble.Rows[i][5].ToString();
                            // sEnabled = batchtatble.Rows[i][6].ToString();
                            //   iFlag = Convert.ToInt32(batchtatble.Rows[i][7].ToString());
                            if (sdwEnrollNumber != sLastEnrollNumber)//identify whether the user information(except fingerprint templates) has been uploaded
                            {
                                if (axCZKEM1.SSR_SetUserInfo(MachineNumber, sdwEnrollNumber, (employeeData.Name), sPassword, iPrivilege, bEnabled))//upload user information to the memory
                                {
                                    axCZKEM1.SetUserTmpExStr(MachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);//upload templates information to the memory
                                }
                                else
                                {
                                    axCZKEM1.GetLastError(ref idwErrorCode);
                                    axCZKEM1.EnableDevice(MachineNumber, true);
                                    lblMessage.Text = "Operation failed";
                                    ClearSystemLabelTimer();
                                }
                            }
                            else//the current fingerprint and the former one belongs the same user,that is ,one user has more than one template
                            {
                                axCZKEM1.SetUserTmpExStr(MachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);

                            }
                            sLastEnrollNumber = sdwEnrollNumber;//change the value of iLastEnrollNumber dynamicly
                        }
                        #endregion}
                    }
                }
            }
            axCZKEM1.BatchUpdate(MachineNumber);//upload all the information in the memory
            axCZKEM1.RefreshData(MachineNumber);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(MachineNumber, true);
            lblMessage.Text = "SetUserName OK";
            ClearSystemLabelTimer();
            #endregion
        }

        private void btnDeleteEnrollment_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do You Want To Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                SetSelectedDevice();
                if (noDeviceSelected())
                {
                    return;
                }
                ConnectDevice();
                DeleteZKTEcoEnrollMent();
                DisconnectDevice();
            }
        }
        private void DeleteZKTEcoEnrollMent()
        {
            int iBackupNumber = 0;
            int idwErrorCode = 0;
            foreach (DataGridViewRow item in templateGridView.Rows)
            {
                var employeeData = item.DataBoundItem as FingerPrint;
                iBackupNumber = (employeeData.FingerPrintId.ToInt());
                if (employeeData.Select)
                {
                    if (axCZKEM1.SSR_DeleteEnrollData(Device.SerialNumber.ToInt(), employeeData.UserId, iBackupNumber))
                    {
                        axCZKEM1.RefreshData(Device.SerialNumber.ToInt());//the data in the device should be refreshed
                        lblMessage.Text = ("DeleteEnrollData,UserID=" + employeeData.UserId + " BackupNumber=" + iBackupNumber.ToString());
                        ClearSystemLabelTimer();
                    }
                    else
                    {
                        axCZKEM1.GetLastError(ref idwErrorCode);
                        lblMessage.Text = ("Operation failed,ErrorCode=" + idwErrorCode.ToString());
                        ClearSystemLabelTimer();
                    }
                }
            }
        }

        private System.Windows.Forms.Timer timer;

        private void btnSetNameToDevice_Click(object sender, EventArgs e)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {
                return;
            }
            ConnectDevice();
            SetUserNameTODevice();
            DisconnectDevice();
        }
        private void SetUserNameTODevice()
        {
            string vStrEnrollNumber = "";
            UInt32 vEnrollNumber;
            int vnResultCode;


            lblMessage.Text = "Working...";
            #region zkt setTemplate
            int idwErrorCode = 0;

            string sdwEnrollNumber = "";
            string sName = "";
            int idwFingerIndex = 0;
            string sTmpData = "";
            int iPrivilege = 0;
            string sPassword = "";
            string sEnabled = "";
            bool bEnabled = true;
            int iFlag = 1;

            int iUpdateFlag = 1;

            var MachineNumber = Device.SerialNumber.ToInt();
            axCZKEM1.EnableDevice(MachineNumber, false);
            if (axCZKEM1.BeginBatchUpdate(MachineNumber, iUpdateFlag))//create memory space for batching data
            {
                string sLastEnrollNumber = "";//the former enrollnumber you have upload(define original value as 0)
                foreach (DataGridViewRow item in templateGridView.Rows)
                {
                    var employeeData = item.DataBoundItem as FingerPrint;
                    sdwEnrollNumber = employeeData.UserId;

                    if (sdwEnrollNumber != sLastEnrollNumber)//identify whether the user information(except fingerprint templates) has been uploaded
                    {
                        if (axCZKEM1.SSR_SetUserInfo(MachineNumber, sdwEnrollNumber, (employeeData.Name), sPassword, iPrivilege, bEnabled))//upload user information to the memory
                        {
                        }
                        else
                        {
                            axCZKEM1.GetLastError(ref idwErrorCode);
                            // MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                            // Cursor = Cursors.Default;
                            axCZKEM1.EnableDevice(MachineNumber, true);
                            lblMessage.Text = "Operation failed";
                            ClearSystemLabelTimer();
                        }
                    }
                    else//the current fingerprint and the former one belongs the same user,that is ,one user has more than one template
                    {
                        axCZKEM1.SetUserTmpExStr(MachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);

                    }
                    sLastEnrollNumber = sdwEnrollNumber;//change the value of iLastEnrollNumber dynamicly
                }
            }
            axCZKEM1.BatchUpdate(MachineNumber);//upload all the information in the memory
            axCZKEM1.RefreshData(MachineNumber);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(MachineNumber, true);
            lblMessage.Text = "SetUserName OK";
            ClearSystemLabelTimer();
            #endregion
        }

        private void btnDeleteAllLog_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do You Want To Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                SetSelectedDevice();

                if (noDeviceSelected())
                {
                    return;
                }
                ConnectDevice();
                clearAllRecordZKteco();
                DisconnectDevice();

            }
        }
        private void clearAllRecordZKteco()
        {

            int idwErrorCode = 0;

            axCZKEM1.EnableDevice(Device.SerialNumber.ToInt(), false);//disable the device
            if (axCZKEM1.ClearGLog(Device.SerialNumber.ToInt()))
            {
                axCZKEM1.RefreshData(Device.SerialNumber.ToInt());//the data in the device should be refreshed
                MessageBox.Show("All att Logs have been cleared from teiminal!", "Success");
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
            axCZKEM1.EnableDevice(Device.SerialNumber.ToInt(), true);//enable the device
        }

        private void DeviceDataManagementFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnDownloadEmployee_Click(object sender, EventArgs e)
        {
            SetSelectedDevice();
            if (noDeviceSelected())
            {
                return;
            }
            ConnectDevice();
            lblMessage.Text = "Wait.. Downloading user from device.";
            string sdwEnrollNumber = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;

            int idwFingerIndex;
            string sTmpData = "";
            int iTmpLength = 0;
            int iFlag = 0;

            axCZKEM1.EnableDevice(Device.SerialNumber.ToInt(), false);
            Cursor = Cursors.WaitCursor;

            List<EEmployee> empList = new List<EEmployee>();
            axCZKEM1.ReadAllUserID(Device.SerialNumber.ToInt());//read all the user information to the memory
            //axCZKEM1.ReadAllTemplate(Device.SerialNumber.ToInt());//read all the users' fingerprint templates to the memory
            while (axCZKEM1.SSR_GetAllUserInfo(Device.SerialNumber.ToInt(), out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            {
                EEmployee temp = new EEmployee()
                {
                    DeviceCode = sdwEnrollNumber.ToInt(),
                    Name = sName == "" ? "Unfilled Employee" : sName,
                };
                empList.Add(temp);
            }
            // Enable device & disconnecting device after dowlading user from particular device
            axCZKEM1.EnableDevice(Device.SerialNumber.ToInt(), true);
            DisconnectDevice();
            int savedEmpCount = 0;
            if (empList.Count() > 0)
            {
                //Add Employe & Shift Wo from 
                SSection sectionServices = new SSection();
                SDesignation designationServices = new SDesignation();
                var designation = designationServices.List().Data.FirstOrDefault();
                var section = sectionServices.List().Data.FirstOrDefault();
                SEmployee employeeServices = new SEmployee();
                List<FingerPrint> templateList = new List<FingerPrint>();
                foreach (var item in empList)
                {
                    EEmployee emp = new EEmployee();
                    {
                        emp.Code = item.DeviceCode.ToString();
                        emp.Name = item.Name;
                        emp.SectionId = section == null ? 0 : section.Id;
                        emp.DesignationId = designation == null ? 0 : designation.Id;
                        emp.BranchId = section == null ? 0 : section.BranchId;
                        emp.DeviceCode = item.DeviceCode;
                        emp.TwoPunch = true;
                        emp.ShiftTypeId = 0;
                    }
                    var validateduplicateUser = employeeServices.List().Data.Where(x => x.DeviceCode == emp.DeviceCode).FirstOrDefault();
                    if (validateduplicateUser == null)
                    {
                        savedEmpCount = savedEmpCount + 1;
                        var result = employeeServices.Add(emp);

                        FingerPrint fingerTemplate = new FingerPrint();
                        fingerTemplate.Name = result.Data.Name;
                        fingerTemplate.UserId = result.Data.DeviceCode.ToString();
                        templateList.Add(fingerTemplate);
                    }
                }
                templateGridView.DataSource = templateList;
            }
            lblMessage.Text = "Download Complete.. Total " + savedEmpCount + " unique user download from device " + Device.IpAddress;
            Cursor = Cursors.Default;
        }
    }
}
