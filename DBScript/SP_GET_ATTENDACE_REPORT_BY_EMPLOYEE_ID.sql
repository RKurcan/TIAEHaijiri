ALTER PROCEDURE SP_GET_ATTENDACE_REPORT_BY_EMPLOYEE_ID(


 @EMPLOYEE_ID INT=2,
 @FROMDATE DATETIME='2017-08-01',
 @TODATE DATETIME='2017-08-31'
)
AS
BEGIN	
SET ARITHABORT ON
SET NOCOUNT ON
DECLARE @BRANCH_ID INT=0;
 SELECT @BRANCH_ID=EM.BranchId FROM EEmployees EM
	INNER JOIN	ESections ES ON ES.Id = EM.SectionId
	INNER JOIN EDepartments ED ON ED.Id= ES.DepartmentId
	INNER JOIN EBranches EB ON EB.Id=ES.BranchId 
	WHERE EM.Id=@EMPLOYEE_ID
SELECT T.* FROM(


SELECT A.EmployeeId,
	   A.EmployeeCode,
	   A.EmployeeDeviceCode,
	   A.EmployeeName,
	   A.EmployeeNameNp,
	   A.SECTIONCODE,
	   A.SECTIONNAME,
	   A.SECTIONNAMENP,
	   A.DEPARTMENTCODE,
	   A.DEPARTMENTNAME,
	   A.DEPARTMENTNAMENP,
	   A.MobileNo,
	   A.FourPunch,
	   A.MultiplePunch,
	   A.NoPunch,
	   A.SinglePunch,
	   A.TwoPunch,
	   (CASE WHEN H.Name IS NULL THEN 
			CASE WHEN LEAVENAME IS NULL THEN 
				CASE WHEN A.WEEKEND= 'NO' THEN A.PlannedTimeIn
				 ELSE '00:00' END
			ELSE '00:00' END
		ELSE '00:00' END	 ) PlannedTimeIn,
	   (CASE WHEN H.Name IS NULL THEN 
			CASE WHEN LEAVENAME IS NULL THEN 
				CASE WHEN A.WEEKEND= 'NO' THEN A.PlannedTimeOut
				 ELSE '00:00' END
			ELSE '00:00' END
		ELSE '00:00' END	 ) PlannedTimeOut,
	   A.PLANNEDLUNCHSTART,
	   A.PLANNEDLUNCHEND,
	   A.ENGDATE,
	   A.WEEKEND,
	   A.ShiftTypeId
,EAL.DateTime PUNCHDATETIME
   ,H.Name HOLIDAYNAME
	,H.BeginDate HOLIDAYSTARTDATE
	 ,H.EndDate HOLIDAYENDDATE
	 ,L.LEAVENAME
	 ,L.[From] LeaveStartDate
	 ,L.[To] leaveEndDate,
	 FIRST_VALUE(EAL.DATETIME) OVER(PARTITION BY A.EMPLOYEEID,A.ENGDATE ORDER BY A.EMPLOYEEID,A.ENGDATE ASC) PUNCHIN,
	 LAST_VALUE(EAL.DATETIME) OVER(PARTITION BY A.EMPLOYEEID,A.ENGDATE ORDER BY A.EMPLOYEEID,A.ENGDATE ASC) PUNCHOUT,
	 ROW_NUMBER() OVER(PARTITION BY A.EMPLOYEEID,A.ENGDATE ORDER BY A.EMPLOYEEID,A.ENGDATE ASC) RN
FROM (
SELECT A.ID EmployeeId,
	   A.Code EmployeeCode,
	   A.DeviceCode EmployeeDeviceCode,
	   A.Name EmployeeName,
	   A.NameNp EmployeeNameNp,
	   ES.Code SECTIONCODE,
	   ES.Name SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   EDEP.Name DEPARTMENTNAME,
	   EDEP.NameNp DEPARTMENTNAMENP,
	   A.Mobile MobileNo,
	   A.FourPunch FourPunch,
	   A.MultiplePunch MultiplePunch,
	   A.NoPunch NoPunch,
	   A.SinglePunch SinglePunch,
	   A.TwoPunch TwoPunch,
	   D.ShiftStartTime PlannedTimeIn,
	   D.ShiftEndTime PlannedTimeOut,
	   D.LunchStartTime PLANNEDLUNCHSTART,
	   D.LunchEndTime PLANNEDLUNCHEND,
	   ED.EngDate ENGDATE,
	   (CASE WHEN EEWO.OffDayId>0 THEN 'Yes' ELSE 'No' END) WEEKEND
	   ,ShiftTypeId
	FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		INNER JOIN EEmployeeShitLists C ON C.EmployeeId =A.ID 
		INNER JOIN EShifts D ON D.Id=C.ShiftId
		
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN EEmployeeWOLists EEWO ON EEWO.EmployeeId=A.Id AND (EEWO.OffDayId+1) =DATEPART(DW,ED.EngDate)
	WHERE ShiftTypeId=0 AND A.BranchId=@BRANCH_ID AND A.ID=@EMPLOYEE_ID
	UNION ALL 
	SELECT B.* FROM(
	 SELECT 
	   A.ID EmployeeId,
	   A.Code EmployeeCode,
	   A.DeviceCode EmployeeDeviceCode,
	   A.Name EmployeeName,
	   A.NameNp EmployeeNameNp,
	   ES.Code SECTIONCODE,
	   ES.Name SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   EDEP.Name DEPARTMENTNAME,
	   EDEP.NameNp DEPARTMENTNAMENP,
	   A.Mobile MobileNo,
	   A.FourPunch FourPunch,
	   A.MultiplePunch MultiplePunch,
	   A.NoPunch NoPunch,
	   A.SinglePunch SinglePunch,
	   A.TwoPunch TwoPunch,
	   D.ShiftStartTime PlannedTimeIn,
	   D.ShiftEndTime PlannedTimeOut,
	   D.LunchStartTime PLANNEDLUNCHSTART,
	   D.LunchEndTime PLANNEDLUNCHEND,
	   ED.EngDate ENGDATE,
	  (CASE WHEN ER.ShiftId >0 THEN 'No' ELSE 'Yes' END) WEEKEND
	   ,ShiftTypeId
		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN ERosters ER ON ER.EmployeeId =A.ID AND ER.Date =ED.EngDate

		LEFT JOIN EShifts D ON D.Id=ER.ShiftId
		
		WHERE ShiftTypeId=2  AND A.BranchId=@BRANCH_ID AND A.ID=@EMPLOYEE_ID
		) B
		UNION ALL 
	SELECT W.* FROM(
	 SELECT 
	   A.ID EmployeeId,
	   A.Code EmployeeCode,
	   A.DeviceCode EmployeeDeviceCode,
	   A.Name EmployeeName,
	   A.NameNp EmployeeNameNp,
	   ES.Code SECTIONCODE,
	   ES.Name SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   EDEP.Name DEPARTMENTNAME,
	   EDEP.NameNp DEPARTMENTNAMENP,
	   A.Mobile MobileNo,
	   A.FourPunch FourPunch,
	   A.MultiplePunch MultiplePunch,
	   A.NoPunch NoPunch,
	   A.SinglePunch SinglePunch,
	   A.TwoPunch TwoPunch,
	   D.ShiftStartTime PlannedTimeIn,
	   D.ShiftEndTime PlannedTimeOut,
	   D.LunchStartTime PLANNEDLUNCHSTART,
	   D.LunchEndTime PLANNEDLUNCHEND,
	   ED.EngDate ENGDATE,
	   (CASE WHEN ER.ShiftId >0 THEN 'No' ELSE 'Yes' END) WEEKEND
	   ,ShiftTypeId
		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN EWEEKLYROSTERS ER ON ER.EmployeeId =A.ID AND ER.Day =DATEPART(DW,ED.EngDate)

		LEFT JOIN EShifts D ON D.Id=ER.ShiftId
		
		WHERE ShiftTypeId=1  AND A.BranchId=@BRANCH_ID AND A.ID=@EMPLOYEE_ID
		) W
	) AS A
	
	LEFT JOIN EAttendanceLogs EAL ON EAL.EmployeeId=A.EmployeeId AND CONVERT(DATE, EAL.DateTime)=A.ENGDATE 


	LEFT JOIN (SELECT EH.Name,EH.NameNp,EHD.FiscalYearId,EHD.BeginDate,EHD.EndDate FROM EHolidays EH INNER JOIN 
							  EHolidayDetails EHD ON EHD.HolidayId =EH.Id AND cONVERT(DATE,EHD.BeginDate) >=@FROMDATE AND cONVERT(DATE,EHD.EndDate)<=@TODATE
						  WHERE EH.BranchId=@BRANCH_ID
						  ) AS H
		ON cONVERT(DATE,H.BeginDate)>=A.EngDate AND cONVERT(DATE,H.EndDate)<=A.EngDate 
	LEFT JOIN (SELECT ELM.NAME LEAVENAME,ELA.[FROM],ELA.[TO],ELA.EmployeeId FROM ELeaveMasters ELM 
							INNER JOIN ELeaveApplications ELA ON ELA.LeaveMasterId=ELM.Id 
									   AND ELA.BranchId=@BRANCH_ID
									   AND cONVERT(DATE,ELA.[FROM]) >=@FROMDATE AND cONVERT(DATE,ELA.[TO])<=@TODATE
									   
							
							) AS L
		ON L.EmployeeId=A.EmployeeId AND  cONVERT(DATE,L.[From])<=A.EngDate AND cONVERT(DATE,L.[To])>= A.EngDate
		
		GROUP BY 
	   A.SECTIONCODE,
	   A.SECTIONNAME,
	   A.SECTIONNAMENP,
	   A.DEPARTMENTCODE,
	   A.DEPARTMENTNAME,
	   A.DEPARTMENTNAMENP,
	   A.EmployeeId ,
	   A.EmployeeCode ,
	   A.EmployeeDeviceCode ,
	   A.EmployeeName ,
	   A.EmployeeNameNp ,
	   A.MobileNo ,
	   A.FourPunch ,
	   A.MultiplePunch ,
	   A.NoPunch ,
	   A.SinglePunch ,
	   a.TwoPunch,
	    A.PlannedTimeIn ,
	   A.PlannedTimeOut ,
	      A.PLANNEDLUNCHSTART ,
	   A.PLANNEDLUNCHEND ,
	   A.EngDate ,
	   WEEKEND,
	   ShiftTypeId
	   ,H.Name 
	,H.BeginDate
	 ,H.EndDate 
	 ,LEAVENAME
	 ,[From]
	 ,[To],
	 DateTime
	 )T
	 WHERE T.RN=1
 ORDER BY EmployeeId
END
GO
EXEC SP_GET_ATTENDACE_REPORT_BY_EMPLOYEE_ID 3,'2017-09-01','2017-09-13'