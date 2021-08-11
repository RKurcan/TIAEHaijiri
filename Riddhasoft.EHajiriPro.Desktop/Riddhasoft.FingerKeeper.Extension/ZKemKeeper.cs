using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.FingerKeeper.Extension
{
    public class ZKemKeeperConnection
    {
        zkemkeeper.CZKEMClass axCZKEM1;
        public ZKemKeeperConnection(zkemkeeper.CZKEMClass AxCZKEM1)
        {
            axCZKEM1 = AxCZKEM1;
        }
        public bool ConnectNet(string Ip, int Port)
        {
            return axCZKEM1.Connect_Net(Ip, (Port));
        }
        public void Disconnect(string Ip, int Port)
        {
            axCZKEM1.Disconnect();
        }
        public USBConnectResult ConnectUSB(int MachineNumber, int Port)
        {
            USBConnectResult result = new USBConnectResult() { Error = true, ErrorCode = 0 };

            SearchforUSBCom usbcom = new SearchforUSBCom();
            string sCom = "";
            bool bSearch = usbcom.SearchforCom(ref sCom);//modify by Darcy on Nov.26 2009
            if (bSearch == false)//modify by Darcy on Nov.26 2009
            {
                result.Message = ("Can not find the virtual serial port that can be used");
                return result;
            }

            int iPort;
            for (iPort = 1; iPort < 10; iPort++)
            {
                if (sCom.IndexOf(iPort.ToString()) > -1)
                {
                    break;
                }
            }


            if (MachineNumber == 0 || MachineNumber > 255)
            {
                result.Message = ("The Machine Number is invalid!");

                return result;
            }

            int iBaudRate = 115200;//115200 is one possible baudrate value(its value cannot be 0)
            result.Error = !axCZKEM1.Connect_Com(iPort, MachineNumber, iBaudRate);
            int idwErrorCode = 0;
            if (result.Error == false)
            {
                axCZKEM1.RegEvent(MachineNumber, 65535);//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                return result;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                result.ErrorCode = idwErrorCode;
                result.Message = ("Unable to connect the device,ErrorCode=" + idwErrorCode.ToString());
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MachineNumber"></param>
        /// <param name="Mode">true to enable and false to disable</param>
        public bool EnableDevice(int MachineNumber, bool Mode)
        {
            return axCZKEM1.EnableDevice(MachineNumber, Mode);
        }

    }
    public class ZKTDeviceMangement
    {
        zkemkeeper.CZKEMClass axCZKEM1;
        public ZKTDeviceMangement(zkemkeeper.CZKEMClass AxCZKEM1)
        {
            axCZKEM1 = AxCZKEM1;
        }
        public bool SetDeviceTime(int MachineNumber)
        {

            if (axCZKEM1.SetDeviceTime(MachineNumber))
            {
                axCZKEM1.RefreshData(MachineNumber);//the data in the device should be refreshed


                return true;
            }
            else
            {
                return false;
            }

        }
        public string GetDeviceTime(int MachineNumber)
        {
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            if (axCZKEM1.GetDeviceTime(MachineNumber, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond))//show the time
            {
                return idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString();
            }
            else
            {
                return "";
            }
        }
       

    }
     public class ZKTUser
     {
            zkemkeeper.CZKEMClass axCZKEM1;
            public ZKTUser(zkemkeeper.CZKEMClass AxCZKEM1)
            {
                axCZKEM1 = AxCZKEM1;
            }

            public DataTable GetTemplate(int MachineNumber) 
            {
                string sdwEnrollNumber = "";
                string sName = "";
                string sPassword = "";
                int iPrivilege = 0;
                bool bEnabled = false;

                int idwFingerIndex;
                string sTmpData = "";
                int iTmpLength = 0;
                int iFlag = 0;
                DataTable dt = new DataTable("FPTemplate");
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("UserId");
                dt.Columns.Add("Name");
                dt.Columns.Add("FingerPrintId");
                dt.Columns.Add("FingerTemplate");
                dt.Columns.Add("Previledge");
                axCZKEM1.EnableDevice(MachineNumber, false);

                axCZKEM1.ReadAllUserID(MachineNumber);//read all the user information to the memory
                axCZKEM1.ReadAllTemplate(MachineNumber);//read all the users' fingerprint templates to the memory
                DataRow drow = null;
                while (axCZKEM1.SSR_GetAllUserInfo(MachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
                {
                    int i = 0;
                    for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                    {
                        if (axCZKEM1.GetUserTmpExStr(MachineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                        {
                            drow = dt.NewRow();
                            drow["Id"] = (sdwEnrollNumber);
                            drow["UserId"] = (sdwEnrollNumber);//modify by Darcy on Nov.26 2009
                            drow["Name"] = sName;
                            drow["FingerTemplate"] = sTmpData;//(idwVerifyMode.ToString());
                            //lvLogs.Items[iIndex].SubItems.Add(idwInOutMode.ToString());
                            drow["FingerPrintId"] ="Fp-"+ (++i);
                            drow["Previledge"] = iPrivilege;
                            dt.Rows.Add(drow);
                        }
                        
                    }
                    if (axCZKEM1.GetUserFaceStr(MachineNumber, sdwEnrollNumber, 50, ref sTmpData, ref iTmpLength))//get the face templates from the memory
                    {
                        drow = dt.NewRow();
                        drow["Id"] = (sdwEnrollNumber);
                        drow["UserId"] = (sdwEnrollNumber);//modify by Darcy on Nov.26 2009
                        drow["Name"] = sName;
                        //list.SubItems.Add(sPassword);
                        drow["FingerPrintId"] ="Face";
                        drow["Previledge"] = iPrivilege;
                        dt.Rows.Add(drow);
                    }
                }

                axCZKEM1.EnableDevice(MachineNumber, true);
                return dt;
            }

            public bool SetTemplate(int MachineNumber,DataTable batchtatble) 
            {
                int idwErrorCode = 0;

                string sdwEnrollNumber = "";
                string sName = "";
                int idwFingerIndex = 0;
                string sTmpData = "";
                int iPrivilege = 0;
                string sPassword = "";
                string sEnabled = "";
                bool bEnabled = false;
                int iFlag = 1;

                int iUpdateFlag = 1;

               
                axCZKEM1.EnableDevice(MachineNumber, false);
                if (axCZKEM1.BeginBatchUpdate(MachineNumber, iUpdateFlag))//create memory space for batching data
                {
                    string sLastEnrollNumber = "";//the former enrollnumber you have upload(define original value as 0)
                    for (int i = 0; i < batchtatble.Rows.Count; i++)
                    {
                        sdwEnrollNumber = batchtatble.Rows[i][0].ToString();
                        sName = batchtatble.Rows[i][1].ToString();
                        idwFingerIndex = Convert.ToInt32(batchtatble.Rows[i][2].ToString());
                        sTmpData = batchtatble.Rows[i][3].ToString();
                       // iPrivilege = Convert.ToInt32(batchtatble.Rows[i][4].ToString());
                      //  sPassword = batchtatble.Rows[i][5].ToString();
                       // sEnabled = batchtatble.Rows[i][6].ToString();
                     //   iFlag = Convert.ToInt32(batchtatble.Rows[i][7].ToString());

                       
                        if (sdwEnrollNumber != sLastEnrollNumber)//identify whether the user information(except fingerprint templates) has been uploaded
                        {
                            if (axCZKEM1.SSR_SetUserInfo(MachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the memory
                            {
                                axCZKEM1.SetUserTmpExStr(MachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);//upload templates information to the memory
                            }
                            else
                            {
                                axCZKEM1.GetLastError(ref idwErrorCode);
                               // MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                               // Cursor = Cursors.Default;
                                axCZKEM1.EnableDevice(MachineNumber, true);
                                return false;
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
                return false;
            }
        }
    public class ZkemkeeperAttendanceLog
    {
        zkemkeeper.CZKEMClass axCZKEM1;
        public ZkemkeeperAttendanceLog(zkemkeeper.CZKEMClass AxCZKEM1)
        {
            this.axCZKEM1 = AxCZKEM1;
        }

        public DataTable GetAllAttendanceLogData(int MachineNumber)
        {
            #region return variable
            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;

            int idwErrorCode = 0;
            int iGLCount = 0;
            int iIndex = 0;
            #endregion

            #region datatable
            DataTable dt = new DataTable("AttendanceLog");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("UserId");
            dt.Columns.Add("Name");
            dt.Columns.Add("FingerPrintId");
            dt.Columns.Add("DateTime");
            #endregion
            axCZKEM1.EnableDevice(MachineNumber, false);//disable the device
            if (axCZKEM1.ReadGeneralLogData(MachineNumber))//read all the attendance records to the memory
            {
                DataRow drow = null;
                while (axCZKEM1.SSR_GetGeneralLogData(MachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                {
                    drow = dt.NewRow();
                    iGLCount++;
                    drow["Id"] = (iGLCount);
                    drow["UserId"] = (sdwEnrollNumber);//modify by Darcy on Nov.26 2009
                    drow["Name"] = "";
                    drow["DateTime"] = "";//(idwVerifyMode.ToString());
                    //lvLogs.Items[iIndex].SubItems.Add(idwInOutMode.ToString());
                    drow["FingerPrintId"] = (idwYear.ToString() + "/" + idwMonth.ToString() + "/" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString());
                    //drow["FingerPrintId"] = (idwDay.ToString() + "/" + idwMonth.ToString() + "/" + idwYear.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString());
                    //lvLogs.Items[iIndex].SubItems.Add(idwWorkcode.ToString());
                    iIndex++;
                    dt.Rows.Add(drow);
                }
            }
            axCZKEM1.EnableDevice(MachineNumber, true);
            return dt;
        }

    }
    public class USBConnectResult
    {

        public string Message { get; set; }

        public bool Error { get; set; }
        public int ErrorCode { get; set; }
    }
    class SearchforUSBCom
    {
        //Search for the virtual serial port created by usbclient.
        public bool SearchforCom(ref string sCom)//modify by Darcy on Nov.26 2009
        {
            string sComValue;
            string sTmpara;
            RegistryKey myReg = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM");
            string[] sComNames = myReg.GetValueNames();//strings array composed of the key name holded by the subkey "SERIALCOMM"
            for (int i = 0; i < sComNames.Length; i++)
            {
                sComValue = "";
                sComValue = myReg.GetValue(sComNames[i]).ToString();//obtain the key value of the corresponding key name
                if (sComValue == "")
                {
                    continue;
                }

                sCom = "";
                if (sComNames[i] == "\\Device\\USBSER000")//find the virtual serial port created by usbclient
                {
                    for (int j = 0; j <= 10; j++)
                    {
                        sTmpara = "";
                        RegistryKey myReg2 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USB\VID_1B55&PID_B400\" + j.ToString() + @"\Device Parameters");//find the plug and play USB device
                        if (myReg2 != null)//add by Darcy on Nov.26 2009
                        {
                            sTmpara = myReg2.GetValue("PortName").ToString();

                            if (sComValue == sTmpara)
                            {
                                sCom = sTmpara;
                                return true;//add by Darcy on Nov.26 2009
                            }
                        }
                    }
                }
            }
            return false;//add by Darcy on Nov.26 2009
        }
    }
}
