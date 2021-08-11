using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace Riddhasoft.FingerKeeper.Extension
{
    public static class Fingerkeeper
    {
        /////////////////////////////////////////////////////////////////////
        // FKAttend dll APIs
        /////////////////////////////////////////////////////////////////////

        //{ Connection
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ConnectComm(
            int anMachineNo,
            int anComPort,
            int anBaudRate,
            string astrTelNumber,
            int anWaitDialTime,
            int anLicense,
            int anComTimeOut);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ConnectNet(
            int anMachineNo,
            string astrIpAddress,
            int anNetPort,
            int anTimeOut,
            int anProtocolType,
            int anNetPassword,
            int anLicense);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ConnectGPRS(
            string astrMachineNo,
            string astrIpAddress,
            int anNetPort,
            int anTimeOut,
            int anProtocolType,
            int anNetPassword,
            int anLicense);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ConnectUSB(
            int anMachineNo,
            int anLicense);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern void FK_DisConnect(int anHandleIndex);
        //} Connection

        //{ Error processing
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetLastError(int anHandleIndex);
        //} Error processing

        //{ Device Setting
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_EnableDevice(int anHandleIndex, byte anEnableFlag);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern void FK_PowerOnAllDevice(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_PowerOffDevice(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetDeviceStatus(
            int anHandleIndex,
            int anStatusIndex,
            ref int apnValue);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetDeviceTime(
            int anHandleIndex,
            ref DateTime apnDateTime);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetDeviceTime(
            int anHandleIndex,
            DateTime anDateTime);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetDeviceInfo(
            int anHandleIndex,
            int anInfoIndex,
            ref int apnValue);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetDeviceInfo(
            int anHandleIndex,
            int anInfoIndex,
            int anValue);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetProductData(
            int anHandleIndex,
            int anDataIndex,
            [MarshalAs(UnmanagedType.LPStr)] ref string apstrValue);
        //} Device Setting

        //{ Log Data
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_LoadSuperLogData(int anHandleIndex, int anReadMark);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBLoadSuperLogDataFromFile(
            int anHandleIndex,
            string astrFilePath);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetSuperLogData(
            int anHandleIndex,
            ref int apnSEnrollNumber,
            ref int apnGEnrollNumber,
            ref int apnMachinePrivilege,
            ref int apnBackupNumber,
            ref DateTime apnDateTime);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_EmptySuperLogData(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_LoadGeneralLogData(int anHandleIndex, int anReadMark);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBLoadGeneralLogDataFromFile(
            int anHandleIndex,
            string astrFilePath);


        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetGeneralLogData(
            int anHandleIndex,
            ref UInt32 apnEnrollNumber,
            ref int apnVerifyMode,
            ref int apnInOutMode,
            ref DateTime apnDateTime);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_EmptyGeneralLogData(int anHandleIndex);
        //} Log Data

        //{ Enroll Data, User Name, Message
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_EnableUser(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anBackupNumber,
            int anEnableFlag);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ModifyPrivilege(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anBackupNumber,
            int anMachinePrivilege);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_BenumbAllManager(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ReadAllUserID(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetAllUserID(
            int anHandleIndex,
            ref UInt32 apnEnrollNumber,
            ref int apnBackupNumber,
            ref int apnMachinePrivilege,
            ref int apnEnable);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetEnrollData(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anBackupNumber,
            ref int apnMachinePrivilege,
            byte[] apnEnrollData,
            ref int apnPassword);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_PutEnrollData(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anBackupNumber,
            int anMachinePrivilege,
            byte[] apnEnrollData,
            int anPassword);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SaveEnrollData(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_DeleteEnrollData(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anBackupNumber);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_EmptyEnrollData(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ClearKeeperData(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBReadAllEnrollDataFromFile(
            int anHandleIndex,
            string astrFilePath);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUSBModel(
            int anHandleIndex,
            int anModel);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUDiskFileFKModel(
            int anHandleIndex,
            string strFKModel);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBReadAllEnrollDataCount(
            int anHandleIndex, int apnValue);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBGetOneEnrollData(
            int anHandleIndex,
            ref UInt32 apnEnrollNumber,
            ref int apnBackupNumber,
            ref int apnMachinePrivilege,
            byte[] apnEnrollData,
            ref int apnPassword,
            ref int apnEnableFlag,
            [MarshalAs(UnmanagedType.LPStr)] ref string apstrEnrollName);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBSetOneEnrollData(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anBackupNumber,
            int anMachinePrivilege,
            byte[] apnEnrollData,
            int anPassword,
            int anEnableFlag,
            string astrEnrollName);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBWriteAllEnrollDataToFile(
            int anHandleIndex,
            string astrFilePath);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBReadAllEnrollDataFromFile_Color(
            int anHandleIndex,
            string astrFilePath);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBWriteAllEnrollDataToFile_Color(
            int anHandleIndex,
            string astrFilePath,
            int anNewsKind);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBGetOneEnrollData_Color(
            int anHandleIndex,
            ref UInt32 apnEnrollNumber,
            ref int apnBackupNumber,
            ref int apnMachinePrivilege,
            byte[] apnEnrollData,
            ref int apnPassWord,
            ref int apnEnableFlag,
            [MarshalAs(UnmanagedType.LPStr)] ref string apstrEnrollName,
            int anNewsKind);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBSetOneEnrollData_Color(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anBackupNumber,
            int anMachinePrivilege,
            byte[] apnEnrollData,
            int anPassWord,
            int anEnableFlag,
            string astrEnrollName,
            int anNewsKind);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetUserName(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            [MarshalAs(UnmanagedType.LPStr)] ref string apstrUserName);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUserName(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            string astrUserName);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetNewsMessage(
            int anHandleIndex,
            int anNewsId,
            [MarshalAs(UnmanagedType.LPStr)] ref string apstrNews);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetNewsMessage(
            int anHandleIndex,
            int anNewsId,
            string astrNews);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetUserNewsID(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            ref int apnNewsId);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUserNewsID(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anNewsId);
        //} Enroll Data, User Name, Message

        //{ BELL setting
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetBellTime(
            int anHandleIndex,
            ref int apnBellCount,
            byte[] apnBellInfo);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetBellTime(
            int anHandleIndex,
            int anBellCount,
            byte[] apnBellInfo);
        //} BELL setting

        //{ Access Control
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetDoorStatus(int anHandleIndex, ref int apnStatusVal);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetDoorStatus(int anHandleIndex, int anStatusVal);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetPassTime(
            int anHandleIndex,
            int anPassTimeID,
            byte[] apnPassTime,
            int anPassTimeSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetPassTime(
            int anHandleIndex,
            int anPassTimeID,
            byte[] apnPassTime,
            int anPassTimeSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetUserPassTime(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            ref int apnGroupID,
            byte[] apnUserPassTimeInfo,
            int anUserPassTimeInfoSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUserPassTime(
            int anHandleIndex,
            UInt32 anEnrollNumber,
            int anGroupID,
            byte[] apnUserPassTimeInfo,
            int anUserPassTimeInfoSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetGroupPassTime(
            int anHandleIndex,
            int anGroupID,
            byte[] apnGroupPassTimeInfo,
            int anGroupPassTimeInfoSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetGroupPassTime(
            int anHandleIndex,
            int anGroupID,
            byte[] apnGroupPassTimeInfo,
            int anGroupPassTimeInfoSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetGroupMatch(
            int nHandleIndex,
            byte[] apnGroupMatchInfo,
            int anGroupMatchInfoSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetGroupMatch(
            int nHandleIndex,
            byte[] apnGroupMatchInfo,
            int anGroupMatchInfoSize);
        //} Access Control

        //{ Etc Functions
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetAdjustInfo(
            int nHandleIndex,
            ref int apnAdjustedState,
            ref int apnAdjustedMonth,
            ref int apnAdjustedDay,
            ref int apnAdjustedHour,
            ref int apnAdjustedMinute,
            ref int apnRestoredState,
            ref int apnRestoredMonth,
            ref int apnRestoredDay,
            ref int apnRestoredHour,
            ref int apnRestoredMinte);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetAdjustInfo(
            int anHandleIndex,
            int anAdjustedState,
            int anAdjustedMonth,
            int anAdjustedDay,
            int anAdjustedHour,
            int anAdjustedMinute,
            int anRestoredState,
            int anRestoredMonth,
            int anRestoredDay,
            int anRestoredHour,
            int anRestoredMinte);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetAccessTime(
            int nHandleIndex,
            UInt32 anEnrollNumber,
            ref int apnAccessTime);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetAccessTime(
            int nHandleIndex,
            UInt32 anEnrollNumber,
            int anAccessTime);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetFontName(
            int nHandleIndex,
            string aStrFontName,
            int anFontType);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetRealTimeInfo(
            int anHandleIndex,
            byte[] apnRealTimeInfo);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetRealTimeInfo(
            int anHandleIndex,
            byte[] apnRealTimeInfo);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetServerNetInfo(
            int anHandleIndex,
            [MarshalAs(UnmanagedType.LPStr)] ref string apstrServerIPAddress,
            ref int apnServerPort,
            ref int apnServerRequest);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetServerNetInfo(
            int nHandleIndex,
            string astrServerIPAddress,
            int anServerPort,
            int anServerRequest);
        //} Etc Functions

        //{ Post & Shift Info (EXCEL version)
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetPostShiftInfo(
            int nHandleIndex,
            byte[] abytPostShiftInfo,
            ref int apPostShiftInfoLen);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetPostShiftInfo(
            int nHandleIndex,
            byte[] abytPostShiftInfo,
            int anPostShiftInfoLen);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetUserInfoEx(
            int nHandleIndex,
            UInt32 anEnrollNumber,
            byte[] abytUserInfo,
            ref int apUserInfoLen);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUserInfoEx(
            int nHandleIndex,
            UInt32 anEnrollNumber,
            byte[] abytUserInfo,
            int anUserInfoLen);
        //} Post & Shift Info (EXCEL version)

        //{ Photo transfer
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetEnrollPhoto(
            int nHandleIndex,
            UInt32 anEnrollNumber,
            byte[] abytPhotoImage,
            ref int anPhotoLength);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetEnrollPhoto(
            int nHandleIndex,
            UInt32 anEnrollNumber,
            byte[] abytPhotoImage,
            int anPhotoLength);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_DeleteEnrollPhoto(
            int nHandleIndex,
            UInt32 anEnrollNumber);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetLogPhoto(
            int nHandleIndex,
            UInt32 anEnrollNumber,
            int anYear,
            int anMonth,
            int anDay,
            int anHour,
            int anMinute,
            int anSec,
            byte[] abytPhotoImage,
            ref int anPhotoLength);
        //} Photo transfer

        // Get supported flag of specific enroll data type
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_IsSupportedEnrollData(
            int nHandleIndex,
            int anBackupNumber,
            ref int apnSupportFlag);

        //{ Access Control(Haoshun FK) function
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_HS_GetTimeZone(int anHandleIndex, byte[] abytOneTimeZone);
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_HS_SetTimeZone(int anHandleIndex, byte[] abytOneTimeZone);
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_HS_GetUserWeekPassTime(int anHandleIndex, byte[] abytUserWeekPassTime);
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_HS_SetUserWeekPassTime(int anHandleIndex, byte[] abytUserWeekPassTime);
        //}

        // All purpose function
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ExecCommand(
            int nHandleIndex,
            string astrCommand,
            [MarshalAs(UnmanagedType.LPStr)] ref string astrResult);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ExtCommand(
            int anHandleIndex,
            byte[] abytCmdStruct);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetIsSupportStringID(int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetLogDataIsSupportStringID(
            int anHandleIndex);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetUSBEnrollDataIsSupportStringID(int anHandleIndex);


        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]//[MarshalAs(UnmanagedType.LPStr)] ref string 
        public static extern int FK_GetSuperLogData_StringID(int anHandleIndex, [MarshalAs(UnmanagedType.LPStr)] ref string apnSEnrollNumber, [MarshalAs(UnmanagedType.LPStr)] ref string apnGEnrollNumber, ref int apnManipulation, ref int apnBackupNumber, ref DateTime apnDateTime);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetGeneralLogData_StringID(int anHandleIndex, [MarshalAs(UnmanagedType.LPStr)] ref string apnEnrollNumber, ref int apnVerifyMode, ref int apnInOutMode, ref DateTime apnDateTime);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_EnableUser_StringID(int anHandleIndex, string apEnrollNumber, int anBackupNumber, int anEnableFlag);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ModifyPrivilege_StringID(int anHandleIndex, string apEnrollNumber, int anBackupNumber, int anMachinePrivilege);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        //public static extern int FK_GetAllUserID_StringID(int anHandleIndex, [MarshalAs(UnmanagedType.LPStr)] ref string apEnrollNumber, ref int apnBackupNumber, ref int apnMachinePrivilege, ref int apnEnableFlag); 
        public static extern int FK_GetAllUserID_StringID(int anHandleIndex, ref string apEnrollNumber, ref int apnBackupNumber, ref int apnMachinePrivilege, ref int apnEnableFlag);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetEnrollData_StringID(int anHandleIndex, string apEnrollNumber, int anBackupNumber, ref int apnMachinePrivilege, byte[] apEnrollData, ref int apnPassWord);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_PutEnrollData_StringID(int anHandleIndex, string apEnrollNumber, int anBackupNumber, int anMachinePrivilege, byte[] apEnrollData, int anPassWord);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_DeleteEnrollData_StringID(int anHandleIndex, string apEnrollNumber, int anBackupNumber);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetUserName_StringID(int anHandleIndex, string apEnrollNumber, [MarshalAs(UnmanagedType.LPStr)] ref string apstrUserName);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUserName_StringID(int anHandleIndex, string apEnrollNumber, string astrUserName);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetUserPassTime_StringID(int anHandleIndex, string apEnrollNumber, ref int apnGroupID, byte[] apUserPassTimeInfo, int anUserPassTimeInfoSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUserPassTime_StringID(int anHandleIndex, string anEnrollNumber, int anGroupID, byte[] apUserPassTimeInfo, int anUserPassTimeInfoSize);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBGetOneEnrollData_StringID(int anHandleIndex, [MarshalAs(UnmanagedType.LPStr)] ref string apEnrollNumber, ref int apnBackupNumber, ref int apnMachinePrivilege, byte[] apEnrollData, ref int apnPassWord, ref int apnEnableFlag, [MarshalAs(UnmanagedType.LPStr)] ref string apstrEnrollName);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_USBSetOneEnrollData_StringID(int anHandleIndex, string apEnrollNumber, int anBackupNumber, int anMachinePrivilege, byte[] apEnrollData, int anPassWord, int anEnableFlag, string astrEnrollName);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetUserInfoEx_StringID(int anHandleIndex, string apEnrollNumber, byte[] apUserInfo, ref int apnUserInfoLen);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetUserInfoEx_StringID(int anHandleIndex, string apEnrollNumber, byte[] apUserInfo, int anUserInfoLen);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetEnrollPhoto_StringID(int anHandleIndex, string apEnrollNumber, byte[] apPhotoImage, ref int apnPhotoLength);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_SetEnrollPhoto_StringID(int anHandleIndex, string apEnrollNumber, byte[] apPhotoImage, int anPhotoLength);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_DeleteEnrollPhoto_StringID(int anHandleIndex, string apEnrollNumber);

        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetLogPhoto_StringID(int anHandleIndex, string apEnrollNumber, int anYear, int anMonth, int anDay, int anHour, int anMinute, int anSec, byte[] apPhotoImage, ref int apnPhotoLength);




        public static int nCommHandleIndex;

        public const short MAX_BELLCOUNT_DAY = 24;
        public const short MAX_PASSCTRLGROUP_COUNT = 50;
        public const short MAX_PASSCTRL_COUNT = 7;
        public const short MAX_USERPASSINFO_COUNT = 3;
        public const short MAX_GROUPPASSINFO_COUNT = 3;
        public const short MAX_GROUPMATCHINFO_COUNT = 10;
        public const short NEWS_EXTEND = 2;
        public const short NEWS_STANDARD = 1;

        public const short SIZE_BELLINFO = 72;
        public const short SIZE_PASSTIME = 4;
        public const short SIZE_PASSCRLTIME = 28;
        public const short SIZE_USERPASSINFO = 3;
        public const short SIZE_GROUPPASSINFO = 3;
        public const short SIZE_GROUPMATCHINFO = 20;

        public const short USER_ID_LENGTH = 16;
        public const short MAX_SHIFT_COUNT = 24;
        public const short MAX_POST_COUNT = 16;
        public const short NAME_BYTE_COUNT = 128;
        public const short MAX_DAY_IN_MONTH = 31;

        public const short SIZE_POST_SHIFT_INFO_V2 = 2476;
        public const short VER2_POST_SHIFT_INFO = 2;

        public const short SIZE_USER_INFO_V2 = 184;
        public const short SIZE_USER_INFO_V3 = 196;

        public const short VER2_USER_INFO = 2;
        public const short VER3_USER_INFO = 3;

        public const int SIZE_EXT_CMD_CODE = 56;

        public const int TIME_SLOT_COUNT = 6;
        public const int TIME_ZONE_COUNT = 255;
        public const int SIZE_TIME_ZONE_STRUCT = 32;
        public const int SIZE_USER_WEEK_PASS_TIME_STRUCT = 16;

        public const string msErrorNoDevice = "No Device";

        public static string ReturnResultPrint(long anResultCode)
        {
            switch (anResultCode)
            {
                case (int)enumErrorCode.RUN_SUCCESS:
                    return "Successful!";
                case (int)enumErrorCode.RUNERR_NOSUPPORT:
                    return "No support";
                case (int)enumErrorCode.RUNERR_UNKNOWNERROR:
                    return "Unknown error";
                case (int)enumErrorCode.RUNERR_NO_OPEN_COMM:
                    return "No Open Comm";
                case (int)enumErrorCode.RUNERR_WRITE_FAIL:
                    return "Write Error";
                case (int)enumErrorCode.RUNERR_READ_FAIL:
                    return "Read Error";
                case (int)enumErrorCode.RUNERR_INVALID_PARAM:
                    return "Parameter Error";
                case (int)enumErrorCode.RUNERR_NON_CARRYOUT:
                    return "execution of command failed";
                case (int)enumErrorCode.RUNERR_DATAARRAY_END:
                    return "End of data";
                case (int)enumErrorCode.RUNERR_DATAARRAY_NONE:
                    return "Nonexistence data";
                case (int)enumErrorCode.RUNERR_MEMORY:
                    return "Memory Allocating Error";
                case (int)enumErrorCode.RUNERR_MIS_PASSWORD:
                    return "License Error";
                case (int)enumErrorCode.RUNERR_MEMORYOVER:
                    return "full enrolldata & can`t put enrolldata";
                case (int)enumErrorCode.RUNERR_DATADOUBLE:
                    return "this ID is already  existed.";
                case (int)enumErrorCode.RUNERR_MANAGEROVER:
                    return "full manager & can`t put manager.";
                case (int)enumErrorCode.RUNERR_FPDATAVERSION:
                    return "mistake fp data version.";
                default:
                    return "Unknown error";
            }
        }

        public static object ConvertByteArrayToStructure(byte[] aByteArray, System.Type aType)
        {
            object vObject;
            IntPtr vptr;
            try
            {
                int vnSize = Marshal.SizeOf(aType);
                if (aByteArray.Length < vnSize)
                    return null;
                vptr = Marshal.AllocHGlobal(vnSize);
                Marshal.Copy(aByteArray, 0, vptr, vnSize);
                vObject = Marshal.PtrToStructure(vptr, aType);
                Marshal.FreeHGlobal(vptr);
                return vObject;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static void ConvertStructureToByteArray(object aStruct, byte[] aByteArray)
        {
            try
            {
                IntPtr vptr = IntPtr.Zero;
                int vnSize = Marshal.SizeOf(aStruct);
                if (aByteArray.Length < vnSize)
                    return;
                vptr = Marshal.AllocHGlobal(vnSize);
                Marshal.StructureToPtr(aStruct, vptr, false);
                Marshal.Copy(vptr, aByteArray, 0, vnSize);
                Marshal.FreeHGlobal(vptr);
                return;
            }
            catch (System.Exception)
            {
                throw new Exception("Fail to convert structure to byte array");
            }
        }

        public static void StringToByteArrayUtf16(String aString, byte[] aByteArray)
        {
            byte[] bytRet;
            int nBytesToCopy;

            Array.Clear(aByteArray, 0, aByteArray.Length);
            bytRet = System.Text.Encoding.GetEncoding("utf-16").GetBytes(aString);
            nBytesToCopy = bytRet.Length;
            if (nBytesToCopy > aByteArray.Length - 2)
                nBytesToCopy = aByteArray.Length - 2;
            Array.Copy(bytRet, aByteArray, nBytesToCopy);
        }

        public static String ByteArrayUtf16ToString(byte[] aByteArray)
        {
            string strRet = "";
            try
            {
                strRet = System.Text.Encoding.GetEncoding("utf-16").GetString(aByteArray, 0, aByteArray.Length);
                strRet = strRet.TrimEnd('\0');
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return strRet;
        }

        public static String ByteArrayUtf8ToString(byte[] aByteArray)
        {
            string strRet = "";
            try
            {
                strRet = System.Text.Encoding.GetEncoding("utf-8").GetString(aByteArray, 0, aByteArray.Length);
                strRet = strRet.TrimEnd('\0');
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return strRet;
        }

        public static bool IsNumeric(string str)
        {
            try
            {
                Convert.ToInt32(str);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static string StringToUTF16HexString(string astrVal)
        {
            string sRet = null;
            byte[] bytUtf16 = null;

            bytUtf16 = System.Text.Encoding.GetEncoding("utf-16").GetBytes(astrVal);
            sRet = HexEncoding.ToString(bytUtf16);
            return sRet;
        }

        public static string UTF16HexStringToString(string astrUtf16Hex)
        {
            string sRet = null;
            byte[] bytUtf16 = null;
            int discarded = 0;

            bytUtf16 = HexEncoding.GetBytes(astrUtf16Hex, out discarded);
            sRet = System.Text.Encoding.GetEncoding("utf-16").GetString(bytUtf16);
            return sRet;
        }

        public static UInt32 GetInt(string asVal)
        {
            try
            {
                return Convert.ToUInt32(asVal);
            }
            catch
            {
                return 0;
            }
        }
        public static String GetStringVerifyMode(Int32 nVerifyMode)
        {
            String vRet = "";
            int vByteCount = 4;
            byte[] vbyteKind = new byte[vByteCount];
            int vFirstKind, vSecondKind;
            vbyteKind = BitConverter.GetBytes(nVerifyMode);
            for (int nIndex = vByteCount - 1; nIndex >= 0; nIndex--)
            {
                vFirstKind = vSecondKind = vbyteKind[nIndex];
                vFirstKind = vFirstKind & 0xF0;
                vSecondKind = vSecondKind & 0x0F;
                vFirstKind = vFirstKind >> 4;
                if (vFirstKind == 0) break;
                if (nIndex < vByteCount - 1)
                    vRet += "+";
                switch (vFirstKind)
                {
                    case (int)enumVerifyKind.VK_FP: vRet += "FP"; break;
                    case (int)enumVerifyKind.VK_PASS: vRet += "PASS"; break;
                    case (int)enumVerifyKind.VK_CARD: vRet += "CARD"; break;
                    case (int)enumVerifyKind.VK_FACE: vRet += "FACE"; break;
                    case (int)enumVerifyKind.VK_VEIN: vRet += "VEIN"; break;
                    case (int)enumVerifyKind.VK_IRIS: vRet += "IRIS"; break;
                }
                if (vSecondKind == 0) break;
                vRet += "+";
                switch (vSecondKind)
                {
                    case (int)enumVerifyKind.VK_FP: vRet += "FP"; break;
                    case (int)enumVerifyKind.VK_PASS: vRet += "PASS"; break;
                    case (int)enumVerifyKind.VK_CARD: vRet += "CARD"; break;
                    case (int)enumVerifyKind.VK_FACE: vRet += "FACE"; break;
                    case (int)enumVerifyKind.VK_VEIN: vRet += "VEIN"; break;
                    case (int)enumVerifyKind.VK_IRIS: vRet += "IRIS"; break;
                }
            }
            //nVerifyMode.
            if (vRet == "") vRet = "--";
            return vRet;
        }

        public static void GetIoModeAndDoorMode(Int32 nIoMode, ref int vIoMode, ref int vDoorMode)
        {
            int vByteCount = 4;
            byte[] vbyteKind = new byte[vByteCount];
            byte[] vbyteDoorMode = new byte[vByteCount];
            vbyteKind = BitConverter.GetBytes(nIoMode);
            vIoMode = vbyteKind[0];
            for (int nIndex = 0; nIndex < 3; nIndex++)
            {
                vbyteDoorMode[nIndex] = vbyteKind[nIndex + 1];
            }
            vbyteDoorMode[3] = 0;
            vDoorMode = BitConverter.ToInt32(vbyteDoorMode, 0);


        }

    }
    //******************************************************************/
    //*                            Structure                           */
    //******************************************************************/

    struct BELLINFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_BELLCOUNT_DAY)]
        public byte[] mValid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_BELLCOUNT_DAY)]
        public byte[] mHour;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_BELLCOUNT_DAY)]
        public byte[] mMinute;
    };//72byte

    //--- Pass Control Time ---
    struct PASSTIME
    {
        public byte StartHour;
        public byte StartMinute;
        public byte EndHour;
        public byte EndMinute;
    };//4byte

    //--- Pass Control Time Infomation ---
    struct PASSCTRLTIME
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_PASSCTRL_COUNT)]
        public PASSTIME[] PassCtrlTime;
    };//28byte

    struct USERPASSINFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_USERPASSINFO_COUNT)]
        public byte[] UserPassID;
    };
    //3byte

    struct GROUPPASSINFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_GROUPPASSINFO_COUNT)]
        public byte[] GroupPassID;
    };
    //3byte


    struct GROUPMATCHINFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_GROUPMATCHINFO_COUNT)]
        public short[] GroupMatch;
    };
    //20byte

    //{-- Post & Shift Info
    struct SHIFT_TIME_SLOT
    {
        public byte AMStartH;   // AM time(hour)
        public byte AMStartM;   // AM min(minute)
        public byte AMEndH;   // AM time(hour)
        public byte AMEndM;   // AM min(minute)
        public byte PMStartH;   // PM time(hour)
        public byte PMStartM;   // PM min(minute)
        public byte PMEndH;   // PM time(hour)
        public byte PMEndM;   // PM min(minute)
        public byte OVStartH;   // OV time(hour)
        public byte OVStartM;   // OV min(minute)
        public byte OVEndH;   // OV time(hour)
        public byte OVEndM;   // OV min(minute)

        public void Init()
        {
            AMStartH = 0;
            AMStartM = 0;
            AMEndH = 0;
            AMEndM = 0;

            PMStartH = 0;
            PMStartM = 0;
            PMEndH = 0;
            PMEndM = 0;

            OVStartH = 0;
            OVStartM = 0;
            OVEndH = 0;
            OVEndM = 0;
        }
    };  // 12 bytes

    struct POST_NAME
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.NAME_BYTE_COUNT)]
        public byte[] PostName;

        public void Init()
        {
            PostName = new byte[Fingerkeeper.NAME_BYTE_COUNT];
        }
    };  // 14 bytes

    struct POST_SHIFT_INFO
    {
        public Int32 Size;                      // 0, 4
        public Int32 Ver;                       // 4, 4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_SHIFT_COUNT)]
        public SHIFT_TIME_SLOT[] ShiftTime;     // 8, 288 (=12*24)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_POST_COUNT)]
        public POST_NAME[] PostInfo;            // 296, 2048 (=128*16)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.NAME_BYTE_COUNT)]
        public byte[] CompanyName;              // 2344, 128
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;                 // 2472, 4

        public void Init()
        {
            Size = 2476;
            Ver = 2;
            ShiftTime = new SHIFT_TIME_SLOT[Fingerkeeper.MAX_SHIFT_COUNT];
            PostInfo = new POST_NAME[Fingerkeeper.MAX_POST_COUNT];
            CompanyName = new byte[Fingerkeeper.NAME_BYTE_COUNT];
            Reserved = new byte[4];
        }
    }; // size = 2476 bytes

    struct USER_INFO
    {
        public Int32 Size;                  // 0, 4
        public Int32 Ver;                   // 4, 4
        public UInt32 UserId;                // 8, 4
        public Int32 Reserved;	            // 12, 4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.NAME_BYTE_COUNT)]
        public byte[] UserName;             // 16, 128
        public Int32 PostId;                // 144, 4
        public Int16 YearAssigned;          // 148, 2
        public Int16 MonthAssigned;         // 150, 2
        public byte StartWeekdayOfMonth;    // 152, 1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_DAY_IN_MONTH)]
        public byte[] ShiftId;              // 152, 32

        public void Init()
        {
            Size = Fingerkeeper.SIZE_USER_INFO_V2;
            Ver = 2;

            YearAssigned = 0;
            MonthAssigned = 0;
            UserId = 0;
            UserName = new byte[Fingerkeeper.NAME_BYTE_COUNT];
            PostId = 0;
            ShiftId = new byte[Fingerkeeper.MAX_DAY_IN_MONTH];
        }
    };  // size = 184 bytes

    struct USER_INFO_STRING_ID
    {
        public Int32 Size;                  // 0, 4
        public Int32 Ver;                   // 4, 4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.USER_ID_LENGTH)]
        public byte[] UserId;                // 8, 4
        public Int32 Reserved;	            // 12, 4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.NAME_BYTE_COUNT)]
        public byte[] UserName;             // 16, 128
        public Int32 PostId;                // 144, 4
        public Int16 YearAssigned;          // 148, 2
        public Int16 MonthAssigned;         // 150, 2
        public byte StartWeekdayOfMonth;    // 152, 1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.MAX_DAY_IN_MONTH)]
        public byte[] ShiftId;              // 152, 32

        public void Init()
        {
            Size = Fingerkeeper.SIZE_USER_INFO_V3;
            Ver = 2;
            UserId = new byte[Fingerkeeper.USER_ID_LENGTH];
            YearAssigned = 0;
            MonthAssigned = 0;
            UserName = new byte[Fingerkeeper.NAME_BYTE_COUNT];
            PostId = 0;
            ShiftId = new byte[Fingerkeeper.MAX_DAY_IN_MONTH];
        }
    };  // size = 196 bytes
    //}

    //=============== USBModel Kind  ===============//
    public enum enumUSBModelKind
    {
        FK625_FP1000 = 2001,
        FK625_FP2000 = 2002,
        FK625_FP3000 = 2003,
        FK625_FP5000 = 2004,
        FK625_FP10000 = 2005,
        FK625_FP30000 = 2006,
        FK625_ID30000 = 2007,
        FK635_FP700 = 3001,
        FK635_FP3000 = 3002,
        FK635_FP10000 = 3003,
        FK635_ID30000 = 3004,
        FK723_FP1000 = 4001,
        FK725_FP1000 = 5001,
        FK725_FP1500 = 5002,
        FK725_ID5000 = 5003,
        FK725_ID30000 = 5004,
        FK735_FP500 = 6001,
        FK735_FP3000 = 6002,
        FK735_ID30000 = 6003,
        FK925_FP3000 = 7001,
        FK935_FP3000 = 8001,
    };
    //=============== Protocol Type ===============//
    public enum enumProtocolType
    {
        PROTOCOL_TCPIP = 0,               // TCP/IP
        PROTOCOL_UDP = 1,                  // UDP
        PROTOCOL_GPRS = 2,                  // UDP
    };

    //=============== Backup Number Constant ===============//
    public enum enumBackupNumberType
    {
        BACKUP_FP_0 = 0,        // Finger 0
        BACKUP_FP_1 = 1,        // Finger 1
        BACKUP_FP_2 = 2,        // Finger 2
        BACKUP_FP_3 = 3,        // Finger 3
        BACKUP_FP_4 = 4,        // Finger 4
        BACKUP_FP_5 = 5,        // Finger 5
        BACKUP_FP_6 = 6,        // Finger 6
        BACKUP_FP_7 = 7,        // Finger 7
        BACKUP_FP_8 = 8,        // Finger 8
        BACKUP_FP_9 = 9,        // Finger 9
        BACKUP_PSW = 10,        // Password
        BACKUP_CARD = 11,       // Card
        BACKUP_FACE = 12,       // Face
        BACKUP_VEIN_0 = 20,     // Vein 0
    };

    //=============== Manipulation of SuperLogData ===============//
    public enum enumSuperLogInfo
    {
        LOG_ENROLL_USER = 3,               // Enroll-User
        LOG_ENROLL_MANAGER = 4,            // Enroll-Manager
        LOG_ENROLL_DELFP = 5,              // FP Delete
        LOG_ENROLL_DELPASS = 6,            // Pass Delete
        LOG_ENROLL_DELCARD = 7,            // Card Delete
        LOG_LOG_ALLDEL = 8,                // LogAll Delete
        LOG_SETUP_SYS = 9,                 // Setup Sys
        LOG_SETUP_TIME = 10,               // Setup Time
        LOG_SETUP_LOG = 11,                // Setup Log
        LOG_SETUP_COMM = 12,               // Setup Comm
        LOG_PASSTIME = 13,                 // Pass Time Set
        LOG_SETUP_DOOR = 14,               // Door Set Log
    };

    //=============== VerifyMode of GeneralLogData ===============//
    public enum enumGLogVerifyMode
    {
        LOG_FPVERIFY = 1,                   // Fp Verify
        LOG_PASSVERIFY = 2,                 // Pass Verify
        LOG_CARDVERIFY = 3,                 // Card Verify
        LOG_FPPASS_VERIFY = 4,              // Pass+Fp Verify
        LOG_FPCARD_VERIFY = 5,              // Card+Fp Verify
        LOG_PASSFP_VERIFY = 6,              // Pass+Fp Verify
        LOG_CARDFP_VERIFY = 7,              // Card+Fp Verify
        LOG_JOB_NO_VERIFY = 8,              // Job number Verify
        LOG_CARDPASS_VERIFY = 9,            // Card+Pass Verify

        LOG_FACEVERIFY = 20,                // Face Verify
        LOG_FACECARDVERIFY = 21,            // Face+Card Verify
        LOG_FACEPASSVERIFY = 22,            // Face+Pass Verify
        LOG_CARDFACEVERIFY = 23,            // Card+Face Verify
        LOG_PASSFACEVERIFY = 24,            // Pass+Face Verify
    };

    //=============== IOMode of GeneralLogData ===============//
    public enum enumGLogIOMode
    {
        LOG_IOMODE_IO = 0,
        LOG_IOMODE_IN1 = 1,
        LOG_IOMODE_OUT1 = 2,
        LOG_IOMODE_IN2 = 3,
        LOG_IOMODE_OUT2 = 4,
        LOG_IOMODE_IN3 = 5,
        LOG_IOMODE_OUT3 = 6,
        LOG_IOMODE_IN4 = 7,
        LOG_IOMODE_OUT4 = 8,
    };
    public enum enumGLogDoorMode
    {

        LOG_CLOSE_DOOR = 1,                // Door Close
        LOG_OPEN_HAND = 2,                 // Hand Open
        LOG_PROG_OPEN = 3,                 // Open by PC
        LOG_PROG_CLOSE = 4,                // Close by PC
        LOG_OPEN_IREGAL = 5,               // Illegal Open
        LOG_CLOSE_IREGAL = 6,              // Illegal Close
        LOG_OPEN_COVER = 7,                // Cover Open
        LOG_CLOSE_COVER = 8,               // Cover Close
        LOG_OPEN_DOOR = 9,                 // Door Open
        LOG_OPEN_DOOR_THREAT = 10,         // Door Open
    }

    public enum enumVerifyKind
    {
        VK_NONE = 0,
        VK_FP = 1,
        VK_PASS = 2,
        VK_CARD = 3,
        VK_FACE = 4,
        VK_VEIN = 5,
        VK_IRIS = 6,
    }




    //=============== Machine Privilege ===============//
    public enum enumMachinePrivilege
    {
        MP_NONE = 0,                       // General user
        MP_ALL = 1,                        // Manager
    };

    //=============== Index of  GetDeviceStatus ===============//
    public enum enumGetDeviceStatus
    {
        GET_MANAGERS = 1,
        GET_USERS = 2,
        GET_FPS = 3,
        GET_PSWS = 4,
        GET_SLOGS = 5,
        GET_GLOGS = 6,
        GET_ASLOGS = 7,
        GET_AGLOGS = 8,
        GET_CARDS = 9,
    };
    //=============== Index of  GetDeviceInfo ===============//
    public enum enumGetDeviceInfo
    {
        DI_MANAGERS = 1,                   // Numbers of Manager
        DI_MACHINENUM = 2,                 // Device ID
        DI_LANGAUGE = 3,                   // Language
        DI_POWEROFF_TIME = 4,              // Auto-PowerOff Time
        DI_LOCK_CTRL = 5,                  // Lock Control
        DI_GLOG_WARNING = 6,               // General-Log Warning
        DI_SLOG_WARNING = 7,               // Super-Log Warning
        DI_VERIFY_INTERVALS = 8,           // Verify Interval Time
        DI_RSCOM_BPS = 9,                  // Comm Buadrate
        DI_DATE_SEPARATE = 10,             // Date Separate Symbol
        DI_VERIFY_KIND = 24,               // Verify Kind Symbol
    };
    //=============== Baudrate = value of DI_RSCOM_BPS ===============//
    public enum enumBaudrate
    {
        BPS_9600 = 3,
        BPS_19200 = 4,
        BPS_38400 = 5,
        BPS_57600 = 6,
        BPS_115200 = 7,
    };
    //=============== Product Data Index ===============//
    public enum enumProductInfo
    {
        PRODUCT_SERIALNUMBER = 1,     // Serial Number
        PRODUCT_BACKUPNUMBER = 2,     // Backup Number
        PRODUCT_CODE = 3,             // Product code
        PRODUCT_NAME = 4,             // Product name
        PRODUCT_WEB = 5,              // Product web
        PRODUCT_DATE = 6,             // Product date
        PRODUCT_SENDTO = 7,           // Product sendto
    };
    //=============== Door Status ===============//
    public enum enumDoorStatus
    {
        DOOR_CONROLRESET = 0,
        DOOR_OPEND = 1,
        DOOR_CLOSED = 2,
        DOOR_COMMNAD = 3,
    };
    //=============== Error code ===============//
    public enum enumErrorCode
    {
        RUN_SUCCESS = 1,
        RUNERR_NOSUPPORT = 0,
        RUNERR_UNKNOWNERROR = -1,
        RUNERR_NO_OPEN_COMM = -2,
        RUNERR_WRITE_FAIL = -3,
        RUNERR_READ_FAIL = -4,
        RUNERR_INVALID_PARAM = -5,
        RUNERR_NON_CARRYOUT = -6,
        RUNERR_DATAARRAY_END = -7,
        RUNERR_DATAARRAY_NONE = -8,
        RUNERR_MEMORY = -9,
        RUNERR_MIS_PASSWORD = -10,
        RUNERR_MEMORYOVER = -11,
        RUNERR_DATADOUBLE = -12,
        RUNERR_MANAGEROVER = -14,
        RUNERR_FPDATAVERSION = -15,
    };

    //{ Pass Time (HaoShun)
    struct TIME_SLOT
    {
        public byte StartHour;
        public byte StartMinute;
        public byte EndHour;
        public byte EndMinute;
    }; // size = 4 bytes

    struct TIME_ZONE
    {
        public Int32 Size;                  // 0, 4
        public Int32 TimeZoneId;            // 4, 4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.TIME_SLOT_COUNT)]
        public TIME_SLOT[] TimeSlots;       // 8, 24 (= 4*6)

        public void Init()
        {
            Size = Fingerkeeper.SIZE_TIME_ZONE_STRUCT;
            TimeZoneId = 0;
            TimeSlots = new TIME_SLOT[Fingerkeeper.TIME_SLOT_COUNT];
        }
    };  // size = 32 bytes

    struct USER_WEEK_PASS_TIME
    {
        public Int32 Size;          // 0, 4
        public UInt32 UserId;        // 4, 4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] WeekPassTime; // 8, 7
        public byte Reserved;       // 15, 1

        public void Init()
        {
            Size = Fingerkeeper.SIZE_USER_WEEK_PASS_TIME_STRUCT;
            UserId = 0;
            WeekPassTime = new byte[7];
        }
    }   //size = 16 bytes
    //}

    struct ExtCmdStructHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Fingerkeeper.SIZE_EXT_CMD_CODE)]
        public byte[] bytCmdCode;    // 0, 56
        public Int32 StructVer;     // 56, 4
        public Int32 StructSize;    // 60, 4

        public void Init(string asCmdCode, int aStructVer, int aStructSize)
        {
            bytCmdCode = new byte[Fingerkeeper.SIZE_EXT_CMD_CODE];
            Array.Clear(bytCmdCode, 0, bytCmdCode.Length);
            byte[] bytAnsi = System.Text.Encoding.GetEncoding("utf-8").GetBytes(asCmdCode);
            int nBytesToCopy = bytAnsi.Length;
            if (nBytesToCopy > bytCmdCode.Length - 1)
                nBytesToCopy = bytCmdCode.Length - 1;
            Array.Copy(bytAnsi, bytCmdCode, nBytesToCopy);

            StructVer = aStructVer;
            StructSize = aStructSize;
        }
    }
}