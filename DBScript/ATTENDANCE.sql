USE [hrmplDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_ATTENDACE_REPORT]    Script Date: 2/23/2021 12:20:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SP_GET_ATTENDACE_REPORT](


 @BRANCH_ID INT ,
 @FROMDATE DATETIME,
 @TODATE DATETIME,
 @LANGUAGE VARCHAR(2)='en',
 @EMP_IDS VARCHAR(MAX)='',
 @SECTION_IDS VARCHAR(MAX)='',
 @DEPT_IDS VARCHAR(MAX)='',
 @BRANCH_IDs VARCHAR(MAX) =''
)
AS
BEGIN	
SET ARITHABORT ON
SET NOCOUNT ON

DECLARE  @FISCAL_YEAR_ID INT=0;

SELECT @FISCAL_YEAR_ID= ID FROM EFiscalYears WHERE BranchId=@BRANCH_ID AND cONVERT(DATE,StartDate) >=@FROMDATE 



Select * into #shift from
(select *,null ShortDayStartDate,null ShortDayEndDate, (case when ShiftStartTime<=ShiftEndTime then 0 else 1 end) RoundInClock from EShifts where ShortDayWorkingEnable =0 and BranchId=@BRANCH_ID
union all 
select es.*,dbo.sf_NEPTOENG( LEFT('2077',4)+'/'+dbo.sf_PadLeft(CONVERT(varchar(10),es.StartMonth),2)+'/'+dbo.sf_PadLeft(CONVERT(varchar(10),es.StartDays),2)) ShortDayStartDate,
			dbo.sf_NEPTOENG( LEFT('2077',4)+'/'+dbo.sf_PadLeft(CONVERT(varchar(10),es.EndMonth),2)+'/'+dbo.sf_PadLeft(CONVERT(varchar(10),es.EndDays),2)) ShortDayEndDate,
			 (case when ShiftStartTime<=ShiftEndTime then 0 else 1 end) RoundInClock
 from EShifts es
 where ShortDayWorkingEnable=1 and BranchId=@BRANCH_ID)t





  select EA.* into #attendanceLog
 from  EAttendanceLogs EA
	inner join EEmployees EE on EE.Id=EA.EmployeeId AND BranchId=@BRANCH_ID
	
 --WHERE EA.EmployeeId IN (SELECT PART FROM DBO.SPLIT(@EMP_IDS,''))
	

SELECT 
	  ROW_NUMBER() OVER(PARTITION BY T.EmployeeId ORDER BY T.EMPLOYEEID ASC) SN,
	   T.EmployeeId,
	   T.EmployeeCode,
	   T.EmployeeDeviceCode,
	   T.EmployeeName,
	   T.EmployeeNameNp,
	   T.SECTIONCODE,
	   T.SECTIONNAME,
	   T.SECTIONNAMENP,
	   T.DEPARTMENTCODE,
	   T.DEPARTMENTNAME,
	   T.DEPARTMENTNAMENP,
	   T.DESIGNATIONNAME,
	   T.DESIGNATIONNAMENP,
	   T.DESIGNATIONLEVEL,
	   T.MobileNo,
	   T.EmploymentStatus,
	   T.FourPunch,
	   T.MultiplePunch,
	   T.NoPunch,
	   T.SinglePunch,
	   T.TwoPunch,
	   (CASE WHEN HOLIDAYNAME IS NULL THEN 
			CASE WHEN LEAVENAME IS NULL THEN 
				CASE WHEN T.WEEKEND= 'NO' THEN (case when ShortDayWorkingEnable=1 then 
														(case when t.ENGDATE between t.ShortDayStartDate and t.ShortDayEndDate then 
															dbo.sf_AddTime( T.PlannedTimeIn,t.ShiftStartGrace) else t.PlannedTimeIn end) 
													else T.PlannedTimeIn end)
				 ELSE '00:00' END
			ELSE '00:00' END
		ELSE '00:00' END	 ) PlannedTimeIn,
	   (CASE WHEN HOLIDAYNAME IS NULL THEN 
			CASE WHEN LEAVENAME IS NULL THEN 
				CASE WHEN T.WEEKEND= 'NO' THEN (case when ShortDayWorkingEnable=1 then 
														(case when t.ENGDATE between t.ShortDayStartDate and t.ShortDayEndDate then 
															dbo.sf_TimeDiff( T.ShiftEndGrace,t.PlannedTimeOut) else t.PlannedTimeOut end) 
													else T.PlannedTimeOut end)
				 ELSE '00:00' END
			ELSE '00:00' END
		ELSE '00:00' END	 ) PlannedTimeOut,
	   EarlyGrace,
	   LateGrace,
	   T.PLANNEDLUNCHSTART,
	   T.PLANNEDLUNCHEND,
	   T.ShortDayWorkingEnable,
	    t.ShortDayStartDate,
	   t.ShortDayEndDate,
	   T.RoundInClock,
	   isnull(t.ShiftType,0) ShiftType,
	   --t.StartMonth,
	   T.ENGDATE,
	   T.NEPDATE,
	   T.WEEKEND,
	   T.ShiftTypeId,
	   T.PUNCHDATETIME,
	   T.HOLIDAYNAME,
	   T.HOLIDAYSTARTDATE,
	   T.HOLIDAYENDDATE,
	   T.LEAVENAME,
	   T.LeaveDescription,
	   T.OFFICEVISIT,
	   t.KAJ,
	   T.LeaveStartDate,
	   T.leaveEndDate,
	   T.PUNCHIN,
	   T.PUNCHOUT

		 FROM(


SELECT A.*
,EAL.DateTime PUNCHDATETIME
   ,(CASE WHEN H.ApplicableGender=0 OR H.ApplicableGender=(A.Gender +1)THEN H.Name ELSE NULL END) HOLIDAYNAME
	,H.BeginDate HOLIDAYSTARTDATE
	 ,H.EndDate HOLIDAYENDDATE
	 ,L.LEAVENAME
	 ,L.LeaveDescription
	 ,L.[From] LeaveStartDate
	 ,L.[To] leaveEndDate
	 ,(CASE WHEN  OV.VISITSTART IS NULL THEN 'NO' ELSE 'YES' END) OFFICEVISIT
	 ,(CASE WHEN  KAJ.KAJSTART IS NULL THEN 'NO' ELSE 'YES' END) KAJ,
	 (CASE WHEN isnull(A.RoundInClock,0)=0  THEN 
			FIRST_VALUE(EAL.DATETIME) OVER(PARTITION BY A.EMPLOYEEID,A.ENGDATE ORDER BY A.EMPLOYEEID,A.ENGDATE ASC) 
		   ELSE
			LAST_VALUE(EAL.DATETIME) OVER(PARTITION BY A.EMPLOYEEID,A.ENGDATE ORDER BY A.EMPLOYEEID,A.ENGDATE ASC)--LOGIC FOR ROUND IN CLOCK SHOULD BE HERE IF ANY PROBLEM OCCURE
		   END)
		PUNCHIN,
	 (CASE WHEN isnull(A.RoundInClock,0)=0  THEN
			LAST_VALUE(EAL.DATETIME) OVER(PARTITION BY A.EMPLOYEEID,A.ENGDATE ORDER BY A.EMPLOYEEID,A.ENGDATE ASC) 
		   ELSE
		    (SELECT TOP 1 DATETIME FROM #attendanceLog WHERE  cONVERT(DATE,[DateTime])=DATEADD(D,1, A.ENGDATE) AND EmployeeId=A.EmployeeId)--DATE TIME SHOULD BE FROM NEXT DAY BEFORE 12 NOON
		   END
			)
			PUNCHOUT,
	 ROW_NUMBER() OVER(PARTITION BY A.EMPLOYEEID,A.ENGDATE ORDER BY A.EMPLOYEEID,A.ENGDATE ASC) RN
FROM (
SELECT A.ID EmployeeId,
	   A.Code EmployeeCode,
	   A.DeviceCode EmployeeDeviceCode,
	   (CASE WHEN @LANGUAGE='EN' OR (A.NameNp IS NULL OR A.NAMENP='')  THEN A.Name ELSE A.NameNp END) EmployeeName,
	   A.NameNp EmployeeNameNp,
	   ES.Code SECTIONCODE,
	   ES.Id SectionId,
	   (CASE WHEN @LANGUAGE='EN' OR (ES.NameNp IS NULL OR ES.NAMENP='')  THEN ES.Name ELSE ES.NameNp END) SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   (CASE WHEN @LANGUAGE='EN' OR (EDEP.NameNp IS NULL OR EDEP.NAMENP='')  THEN EDEP.Name ELSE EDEP.NameNp END) DEPARTMENTNAME,
	   EDEP.NameNp DEPARTMENTNAMENP,
	    (CASE WHEN @LANGUAGE='EN' OR (EDES.NameNp IS NULL OR EDES.NAMENP='')  THEN EDES.Name ELSE EDES.NameNp END) DESIGNATIONNAME,
	   EDES.NameNp DESIGNATIONNAMENP,
	   EDES.DESIGNATIONLEVEL DESIGNATIONLEVEL,
	   A.Mobile MobileNo,
	   A.EmploymentStatus,
	   A.FourPunch FourPunch,
	   A.MultiplePunch MultiplePunch,
	   A.NoPunch NoPunch,
	   A.SinglePunch SinglePunch,
	   A.TwoPunch TwoPunch,
	   D.ShiftStartTime PlannedTimeIn,
	   D.EarlyGrace,
	   D.LateGrace,
	   D.ShiftEndTime PlannedTimeOut,
	   D.LunchStartTime PLANNEDLUNCHSTART,
	   D.LunchEndTime PLANNEDLUNCHEND,
	   D.ShortDayWorkingEnable,
	   D.ShortDayStartDate,
	   d.ShiftStartGrace,
	   d.ShiftEndGrace,
	   d.ShortDayEndDate,
	   
	   isnull(D.RoundInClock,0) RoundInClock,
	   d.ShiftType,
	   ED.EngDate ENGDATE,
	   ED.NEPDATE NEPDATE,
	   (case when d.ShiftType in(5,6) then 'YES'
	   else
	   (CASE WHEN EEWO.OffDayId>=0 THEN 'Yes' ELSE 'No' END) end) WEEKEND
	   ,ShiftTypeId,
	   A.Gender
	FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		Inner join EDesignations EDES on EDES.Id=A.DesignationId
		INNER JOIN EEmployeeShitLists C ON C.EmployeeId =A.ID 
		INNER JOIN 
		#shift D ON D.Id=C.ShiftId
		
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN EEmployeeWOLists EEWO ON EEWO.EmployeeId=A.Id AND (EEWO.OffDayId +1) =DATEPART(DW,ED.EngDate)
	WHERE ShiftTypeId=0 AND A.BranchId=@BRANCH_ID 
	UNION ALL 
	SELECT B.* FROM(
	 SELECT 
	   A.ID EmployeeId,
	   A.Code EmployeeCode,
	   A.DeviceCode EmployeeDeviceCode,
	  (CASE WHEN @LANGUAGE='EN' OR (A.NameNp IS NULL OR A.NAMENP='')  THEN A.Name ELSE A.NameNp END) EmployeeName,
	   A.NameNp EmployeeNameNp,
	   ES.Code SECTIONCODE,
	   ES.Id SectionId,
	   (CASE WHEN @LANGUAGE='EN' OR (ES.NameNp IS NULL OR ES.NAMENP='')  THEN ES.Name ELSE ES.NameNp END) SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   (CASE WHEN @LANGUAGE='EN' OR (EDEP.NameNp IS NULL OR EDEP.NAMENP='')  THEN EDEP.Name ELSE EDEP.NameNp END) DEPARTMENTNAME,
	   EDEP.NameNp DEPARTMENTNAMENP,
	    (CASE WHEN @LANGUAGE='EN' OR (EDES.NameNp IS NULL OR EDES.NAMENP='')  THEN EDES.Name ELSE EDES.NameNp END) DESIGNATIONNAME,
	   EDES.NameNp DESIGNATIONNAMENP,
	   EDES.DESIGNATIONLEVEL DESIGNATIONLEVEL,
	   A.Mobile MobileNo,
	   A.EmploymentStatus,
	   A.FourPunch FourPunch,
	   A.MultiplePunch MultiplePunch,
	   A.NoPunch NoPunch,
	   A.SinglePunch SinglePunch,
	   A.TwoPunch TwoPunch,
	   D.ShiftStartTime PlannedTimeIn,
	   D.EarlyGrace,
	   D.LateGrace,
	   D.ShiftEndTime PlannedTimeOut,
	   D.LunchStartTime PLANNEDLUNCHSTART,
	   D.LunchEndTime PLANNEDLUNCHEND,
	   D.ShortDayWorkingEnable,
	    D.ShortDayStartDate,
		d.ShiftStartGrace,
	   d.ShiftEndGrace,
	   d.ShortDayEndDate,
	   D.RoundInClock,
	   D.ShiftType,
	    --DBO.SF_NEPTOENG('2074/'+DBO.SF_PADLEFT(StartMonth,2)+'/'+DBO.SF_PADLEFT(StartDays,2)) StartMonth,
	   ED.EngDate ENGDATE,
	   ED.NEPDATE NEPDATE,
	   /**/
	   /**/

	   (CASE WHEN ER.ShiftId >0 THEN 
			(case when d.shiftType in(5,6) then 'YES' else 'NO' end)
	   
	    ELSE 'Yes' END) WEEKEND
	   ,ShiftTypeId,
	   A.Gender
		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		Inner join EDesignations EDES on EDES.Id=A.DesignationId
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN ERosters ER ON ER.EmployeeId =A.ID AND ER.Date =ED.EngDate

		LEFT JOIN #shift D ON D.Id=ER.ShiftId
		
		WHERE ShiftTypeId=2  AND A.BranchId=@BRANCH_ID 
		) B
		UNION ALL 
	SELECT W.* FROM(
	 SELECT 
	   A.ID EmployeeId,
	   A.Code EmployeeCode,
	   A.DeviceCode EmployeeDeviceCode,
	   (CASE WHEN @LANGUAGE='EN' OR (A.NameNp IS NULL OR A.NAMENP='')  THEN A.Name ELSE A.NameNp END) EmployeeName,
	   A.NameNp EmployeeNameNp,
	   ES.Code SECTIONCODE,
	   ES.Id SectionId,
	   (CASE WHEN @LANGUAGE='EN' OR (ES.NameNp IS NULL OR ES.NAMENP='')  THEN ES.Name ELSE ES.NameNp END) SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   (CASE WHEN @LANGUAGE='EN' OR (EDEP.NameNp IS NULL OR EDEP.NAMENP='')  THEN EDEP.Name ELSE EDEP.NameNp END) DEPARTMENTNAME,
	   EDEP.NameNp DEPARTMENTNAMENP,
	     (CASE WHEN @LANGUAGE='EN' OR (EDES.NameNp IS NULL OR EDES.NAMENP='')  THEN EDES.Name ELSE EDES.NameNp END) DESIGNATIONNAME,
	   EDES.NameNp DESIGNATIONNAMENP,
	   EDES.DESIGNATIONLEVEL DESIGNATIONLEVEL,
	   A.Mobile MobileNo,
	   A.EmploymentStatus,
	   A.FourPunch FourPunch,
	   A.MultiplePunch MultiplePunch,
	   A.NoPunch NoPunch,
	   A.SinglePunch SinglePunch,
	   A.TwoPunch TwoPunch,
	   D.ShiftStartTime PlannedTimeIn,
	    D.EarlyGrace,
	   D.LateGrace,
	   D.ShiftEndTime PlannedTimeOut,
	   D.LunchStartTime PLANNEDLUNCHSTART,
	   D.LunchEndTime PLANNEDLUNCHEND,
	   D.ShortDayWorkingEnable,
	    D.ShortDayStartDate,
		d.ShiftStartGrace,
	   d.ShiftEndGrace,
	   d.ShortDayEndDate,
	   D.RoundInClock,
	   D.ShiftType,
	   -- DBO.SF_NEPTOENG('2074/'+DBO.SF_PADLEFT(StartMonth,2)+'/'+DBO.SF_PADLEFT(StartDays,2)) StartMonth,
	  
	   ED.EngDate ENGDATE,
	   ED.NEPDATE NEPDATE,
	   (CASE WHEN ER.ShiftId >0 THEN 
			(case when d.shiftType in(5,6) then 'YES' else 'NO' end)
	    ELSE 'Yes' END) WEEKEND
	   ,ShiftTypeId,
	   A.Gender
		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		Inner join EDesignations EDES on EDES.Id=A.DesignationId
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN EWEEKLYROSTERS ER ON ER.EmployeeId =A.ID AND (ER.Day+1) =DATEPART(DW,ED.EngDate)

		LEFT JOIN  #shift D ON D.Id=ER.ShiftId
		
		WHERE ShiftTypeId=1  AND A.BranchId=@BRANCH_ID 
		) W
	) AS A
	
	LEFT JOIN #attendanceLog EAL ON EAL.EmployeeId=A.EmployeeId AND CONVERT(DATE, EAL.DateTime)=A.ENGDATE 


	LEFT JOIN (SELECT EH.Name,EH.NameNp,EHD.FiscalYearId,EHD.BeginDate,EHD.EndDate,EH.ApplicableGender,eh.IsOccuredInSameDate,SectionId FROM EHolidays EH INNER JOIN 
							  EHolidayDetails EHD ON EHD.HolidayId =EH.Id 
							  left join EDepartmentWiseHolidays edwh on edwh.HolidayId=eh.Id 
							  --AND (cONVERT(DATE,EHD.BeginDate) >=@FROMDATE OR cONVERT(DATE,EHD.EndDate)<=@TODATE)
						  WHERE EH.BranchId=@BRANCH_ID 
						  ) AS H
		ON cONVERT(DATE,H.BeginDate)<=A.EngDate AND cONVERT(DATE,H.EndDate)>=A.EngDate and (h.IsOccuredInSameDate=0 or h.SectionId=a.sectionid)
	LEFT JOIN (SELECT elm.Code+'-'+ ELM.NAME LEAVENAME ,ELA.[Description] LeaveDescription ,ELA.[FROM],ELA.[TO],ELA.EmployeeId FROM ELeaveMasters ELM 
							INNER JOIN ELeaveApplications ELA ON ELA.LeaveMasterId=ELM.Id and ELA.LeaveStatus=1
									   AND ELA.BranchId=@BRANCH_ID
									   --AND cONVERT(DATE,ELA.[FROM]) >=@FROMDATE AND cONVERT(DATE,ELA.[TO])<=@TODATE
									   
							
							) AS L
		ON L.EmployeeId=A.EmployeeId AND  cONVERT(DATE,L.[From])<=A.EngDate AND cONVERT(DATE,L.[To])>= A.EngDate
	LEFT JOIN (select EOVD.EmployeeId,EOV.[From] VISITSTART,EOV.[TO] VISITEND from EOfficeVisitDetails EOVD
					INNER JOIN EOfficeVisits EOV ON EOVD.OfficeVisitId =EOV.Id
					WHERE EOV.IsApprove=1 AND BranchId=@BRANCH_ID ) OV --AND  cONVERT(DATE,EOV.[From]) >=@FROMDATE AND cONVERT(DATE,EOV.[TO])<=@TODATE ) OV
		ON OV.EmployeeId=A.EmployeeId AND  cONVERT(DATE,OV.VISITSTART)<=A.EngDate AND cONVERT(DATE,OV.VISITEND)>= A.EngDate 
		
		LEFT JOIN (select EKAJD.EmployeeId,EKAJ.[From] KAJSTART,EKAJ.[TO] KAJEND from EKajDetails EKAJD
					INNER JOIN EKajs EKAJ ON EKAJ.Id =EKAJD.KajId
					WHERE EKAJ.IsApprove=1 AND BranchId=@BRANCH_ID ) KAJ--AND  cONVERT(DATE,EKAJ.[From]) >=@FROMDATE AND cONVERT(DATE,EKAJ.[TO])<=@TODATE ) KAJ
		ON KAJ.EmployeeId=A.EmployeeId AND  cONVERT(DATE,KAJ.KAJSTART)<=A.EngDate AND cONVERT(DATE,KAJ.KAJEND)>= A.EngDate
		GROUP BY 
	   A.SECTIONCODE,
	   a.SectionId,
	   A.SECTIONNAME,
	   A.SECTIONNAMENP,
	   A.DEPARTMENTCODE,
	   A.DEPARTMENTNAME,
	   A.DEPARTMENTNAMENP,
	   A.DESIGNATIONNAME,
	   A.DESIGNATIONNAMENP,
	    A.DESIGNATIONLEVEL,
	   A.EmployeeId ,
	   A.EmployeeCode ,
	   A.EmployeeDeviceCode ,
	   A.EmployeeName ,
	   A.EmployeeNameNp ,
	   A.MobileNo ,
	   A.EmploymentStatus,
	   A.FourPunch ,
	   A.MultiplePunch ,
	   A.NoPunch ,
	   A.SinglePunch ,
	   a.TwoPunch,
	    A.PlannedTimeIn ,
	   A.PlannedTimeOut ,
	    A.EarlyGrace,
	   A.LateGrace,
	      A.PLANNEDLUNCHSTART ,
	   A.PLANNEDLUNCHEND ,
	   A.ShortDayWorkingEnable,
	    a.ShortDayStartDate,
		a.ShiftStartGrace,
	   a.ShiftEndGrace,
	   a.ShortDayEndDate,
	   a.RoundInClock,
	   a.ShiftType,
	   --A.StartMonth,
	   A.EngDate ,
	   A.NEPDATE,
	   WEEKEND,
	   ShiftTypeId,
	   A.Gender
	   ,H.Name 
	,H.BeginDate
	 ,H.EndDate ,
	 H.ApplicableGender
	 ,LEAVENAME
	 ,LeaveDescription
	 ,OV.VISITSTART,
	 KAJ.KAJSTART
	 ,[From]
	 ,[To],
	 DateTime
	 )T
	 --inner join DBO.Split(@EMP_IDS,',') item on  T.EmployeeId = (case when @EMP_IDS = '' then T.EmployeeId else  item.items end)    
	 WHERE T.RN=1
 ORDER BY DEPARTMENTNAME,SECTIONNAME,DESIGNATIONLEVEL,EmployeeName
 
 drop table #shift
 drop table #attendanceLog
 --drop table #EMPLOYEES
END
go

