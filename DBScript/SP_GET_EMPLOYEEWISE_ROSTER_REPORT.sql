USE [hrmplDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_ATTENDACE_REPORT]    Script Date: 11/17/2017 4:38:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GET_EMPLOYEEWISE_ROSTER_REPORT](


 @BRANCH_ID INT =2,
 @FROMDATE DATETIME='2017-11-16',
 @TODATE DATETIME='2017-12-16',
 @LANGUAGE VARCHAR(2)='en',
 @EMP_IDS VARCHAR(MAX)='492,494',
 @SECTION_IDS VARCHAR(MAX)='',
 @DEPT_IDS VARCHAR(MAX)='4,6'
)
AS
BEGIN	
SET ARITHABORT ON
SET NOCOUNT ON





Select * into #shift from
(select *,null ShortDayStartDate,null ShortDayEndDate, (case when ShiftStartTime<ShiftEndTime then 0 else 1 end) RoundInClock from EShifts where ShortDayWorkingEnable =0 and BranchId=@BRANCH_ID
union all 
select es.*,dbo.sf_NEPTOENG( LEFT('2074',4)+'/'+dbo.sf_PadLeft(CONVERT(varchar(10),es.StartMonth),2)+'/'+dbo.sf_PadLeft(CONVERT(varchar(10),es.StartDays),2)) ShortDayStartDate,
			dbo.sf_NEPTOENG( LEFT('2074',4)+'/'+dbo.sf_PadLeft(CONVERT(varchar(10),es.EndMonth),2)+'/'+dbo.sf_PadLeft(CONVERT(varchar(10),es.EndDays),2)) ShortDayEndDate,
			 (case when ShiftStartTime<ShiftEndTime then 0 else 1 end) RoundInClock
 from EShifts es
 where ShortDayWorkingEnable=1 and BranchId=@BRANCH_ID)t


 SELECT a.EmployeeId, A.EmployeeCode,A.EmployeeName,A.EmployeeNameNp,
		A.SECTIONCODE,A.SECTIONNAME,A.SECTIONNAMENP,
		A.DEPARTMENTCODE,A.DEPARTMENTNAME,A.DEPARTMENTNAMENP,A.DESIGNATIONLEVEL,
		A.ENGDATE,A.NEPDATE,
		(CASE WHEN H.Name IS NULL AND L.LEAVENAME IS NULL AND A.WEEKEND='NO' THEN A.ShiftCode 
			  WHEN L.LEAVENAME is not NULL THEN 'L' 
			  WHEN H.Name IS NULL THEN 'OFF' ELSE  'PH' END) ShiftCode,
		(CASE WHEN H.Name IS NULL AND L.LEAVENAME IS NULL AND A.WEEKEND='NO' THEN A.ShiftName 
			  WHEN L.LEAVENAME is not NULL THEN L.LEAVENAME 
			  WHEN H.Name IS NULL THEN 'WEEKEND' ELSE  H.Name END) ShiftName
		
		 FROM (

	SELECT A.ID EmployeeId,
		   A.Code EmployeeCode,
		   A.DeviceCode EmployeeDeviceCode,
		   a.Code+'-'+ (CASE WHEN @LANGUAGE='EN' OR (A.NameNp IS NULL OR A.NAMENP='')  THEN A.Name ELSE A.NameNp END) EmployeeName,
		   A.NameNp EmployeeNameNp,
		   ES.Code SECTIONCODE,
		   (CASE WHEN @LANGUAGE='EN' OR (ES.NameNp IS NULL OR ES.NAMENP='')  THEN ES.Name ELSE ES.NameNp END) SECTIONNAME,
		   ES.NameNp SECTIONNAMENP,
		   EDEP.Code DEPARTMENTCODE,
		   (CASE WHEN @LANGUAGE='EN' OR (EDEP.NameNp IS NULL OR EDEP.NAMENP='')  THEN EDEP.Name ELSE EDEP.NameNp END) DEPARTMENTNAME,
		   EDEP.NameNp DEPARTMENTNAMENP,
		   EDES.DesignationLevel DESIGNATIONLEVEL,
		   ED.EngDate ENGDATE,
		   ED.NEPDATE NEPDATE,
		   D.SHIFTCODE,
		   D.SHIFTNAME,
		   (CASE WHEN EEWO.OffDayId>0 THEN 'Yes' ELSE 'No' END) WEEKEND
		   ,ShiftTypeId

		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		Inner Join EDesignations EDES ON EDES.Id=A.DesignationId
		INNER JOIN EEmployeeShitLists C ON C.EmployeeId =A.ID 
		INNER JOIN 
		#shift D ON D.Id=C.ShiftId
		
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN EEmployeeWOLists EEWO ON EEWO.EmployeeId=A.Id AND (EEWO.OffDayId +1) =DATEPART(DW,ED.EngDate)
	WHERE ShiftTypeId=0 AND A.BranchId=@BRANCH_ID 
	UNION ALL 
	
	 SELECT 
	   A.ID EmployeeId,
	   A.Code EmployeeCode,
	   A.DeviceCode EmployeeDeviceCode,
	  (CASE WHEN @LANGUAGE='EN' OR (A.NameNp IS NULL OR A.NAMENP='')  THEN A.Name ELSE A.NameNp END) EmployeeName,
	   A.NameNp EmployeeNameNp,
	   ES.Code SECTIONCODE,
	   (CASE WHEN @LANGUAGE='EN' OR (ES.NameNp IS NULL OR ES.NAMENP='')  THEN ES.Name ELSE ES.NameNp END) SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   (CASE WHEN @LANGUAGE='EN' OR (EDEP.NameNp IS NULL OR EDEP.NAMENP='')  THEN EDEP.Name ELSE EDEP.NameNp END) DEPARTMENTNAME,
	   EDEP.NameNp DEPARTMENTNAMENP,
	   EDES.DesignationLevel DESIGNATIONLEVEL,

	   ED.EngDate ENGDATE,
	   ED.NEPDATE NEPDATE,
	   D.SHIFTCODE,
		   D.SHIFTNAME,
	   (CASE WHEN ER.ShiftId >0 THEN 'No' ELSE 'Yes' END) WEEKEND
	   ,ShiftTypeId
		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		Inner Join EDesignations EDES ON EDES.Id=A.DesignationId
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN ERosters ER ON ER.EmployeeId =A.ID AND ER.Date =ED.EngDate

		LEFT JOIN #shift D ON D.Id=ER.ShiftId
		
		WHERE ShiftTypeId=2  AND A.BranchId=@BRANCH_ID 
		
		UNION ALL 
	
	 SELECT 
	   A.ID EmployeeId,
	   A.Code EmployeeCode,
	   A.DeviceCode EmployeeDeviceCode,
	   (CASE WHEN @LANGUAGE='EN' OR (A.NameNp IS NULL OR A.NAMENP='')  THEN A.Name ELSE A.NameNp END) EmployeeName,
	   A.NameNp EmployeeNameNp,
	   ES.Code SECTIONCODE,
	   (CASE WHEN @LANGUAGE='EN' OR (ES.NameNp IS NULL OR ES.NAMENP='')  THEN ES.Name ELSE ES.NameNp END) SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   (CASE WHEN @LANGUAGE='EN' OR (EDEP.NameNp IS NULL OR EDEP.NAMENP='')  THEN EDEP.Name ELSE EDEP.NameNp END) DEPARTMENTNAME,
	   EDEP.NameNp DEPARTMENTNAMENP,
	    EDES.DesignationLevel DESIGNATIONLEVEL,
	   ED.EngDate ENGDATE,
	   ED.NEPDATE NEPDATE,
	   D.SHIFTCODE,
		   D.SHIFTNAME,
	   (CASE WHEN ER.ShiftId >0 THEN 'No' ELSE 'Yes' END) WEEKEND
	   ,ShiftTypeId
		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		Inner Join EDesignations EDES ON EDES.Id=A.DesignationId
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN EWEEKLYROSTERS ER ON ER.EmployeeId =A.ID AND (ER.Day+1) =DATEPART(DW,ED.EngDate)

		LEFT JOIN  #shift D ON D.Id=ER.ShiftId
		
		WHERE ShiftTypeId=1  AND A.BranchId=@BRANCH_ID 
		
	) AS A
	
	


	LEFT JOIN (SELECT EH.Name,EH.NameNp,EHD.FiscalYearId,EHD.BeginDate,EHD.EndDate FROM EHolidays EH INNER JOIN 
							  EHolidayDetails EHD ON EHD.HolidayId =EH.Id AND cONVERT(DATE,EHD.BeginDate) >=@FROMDATE AND cONVERT(DATE,EHD.EndDate)<=@TODATE
						  WHERE EH.BranchId=@BRANCH_ID
						  ) AS H
		ON cONVERT(DATE,H.BeginDate)>=A.EngDate AND cONVERT(DATE,H.EndDate)<=A.EngDate 
	LEFT JOIN (SELECT ELM.NAME LEAVENAME,ELA.[FROM],ELA.[TO],ELA.EmployeeId FROM ELeaveMasters ELM 
							INNER JOIN ELeaveApplications ELA ON ELA.LeaveMasterId=ELM.Id and ela.LeaveStatus=1
									   AND ELA.BranchId=@BRANCH_ID
									   AND cONVERT(DATE,ELA.[FROM]) >=@FROMDATE AND cONVERT(DATE,ELA.[TO])<=@TODATE
									   
							
							) AS L
		ON L.EmployeeId=A.EmployeeId AND  cONVERT(DATE,L.[From])<=A.EngDate AND cONVERT(DATE,L.[To])>= A.EngDate
		
		Order by DEPARTMENTNAME,SECTIONNAME,DESIGNATIONLEVEL,EmployeeName
 drop table #shift

 --drop table #EMPLOYEES
END
go
exec SP_GET_EMPLOYEEWISE_ROSTER_REPORT 2
