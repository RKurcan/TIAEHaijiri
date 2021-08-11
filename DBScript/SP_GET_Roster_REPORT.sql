ALTER PROCEDURE [dbo].[SP_GET_Roster_REPORT](


 @EMPLOYEE_ID INT =3,
 @FROMDATE Date='2017-08-01',
 @TODATE date='2017-09-31',
 @LANGUAGE VARCHAR(2)='en'
)
AS
BEGIN	
SET ARITHABORT ON
SET NOCOUNT ON
DECLARE @CURDATE DATE=GETDATE();
DECLARE @BRANCH_ID INT=0;
 SELECT @BRANCH_ID=EM.BranchId FROM EEmployees EM
	INNER JOIN	ESections ES ON ES.Id = EM.SectionId
	INNER JOIN EDepartments ED ON ED.Id= ES.DepartmentId
	INNER JOIN EBranches EB ON EB.Id=ES.BranchId 
	WHERE EM.Id=@EMPLOYEE_ID
SELECT t.EmployeeName,
case when ShiftTypeId=0 then 'Fixed Shift'
when ShiftTypeId=1 then 'Weekly Shift'
else 'Monthly'
 end RosterType,
Case when t.HOLIDAYNAME is null and LEAVENAME is null and WEEKEND ='No' then
--Convert(varchar(10),t.ENGDATE,111)+
--'('+
t.nepDate
--+')'   
when LEAVENAME is not null then 
'Leave' --LEAVENAME 
when HOLIDAYNAME is not null then
'Holiday'--HOLIDAYNAME 
else 'Weekend'
end
Title,

Case when t.HOLIDAYNAME is null and LEAVENAME is null and WEEKEND ='No' then
convert(varchar(5),convert(time, t.PlannedTimeIn))
when LEAVENAME is not null then 
LEAVENAME 
when HOLIDAYNAME is not null then
HOLIDAYNAME 
else ''
end 

 [From],
 Case when t.HOLIDAYNAME is null and LEAVENAME is null and WEEKEND='No' then
convert(varchar(5),convert(time, t.PlannedTimeOut))
else
'' 
end
  [To],
 case when convert(date,t.ENGDATE)=@CURDATE then  Convert(bit, 1) else Convert(bit, 0) end IsCurrentDay
   FROM(


SELECT A.*

   ,H.Name+'-'+h.NameNp HOLIDAYNAME
	,H.BeginDate HOLIDAYSTARTDATE
	 ,H.EndDate HOLIDAYENDDATE
	 ,L.LEAVENAME
	 ,L.[From] LeaveStartDate
	 ,L.[To] leaveEndDate,
	 ROW_NUMBER() OVER(PARTITION BY A.EMPLOYEEID,A.ENGDATE ORDER BY A.EMPLOYEEID,A.ENGDATE ASC) RN
FROM (
SELECT A.ID EmployeeId,
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
	   ED.NepDate nepDate,
	   (CASE WHEN EEWO.OffDayId>0 THEN 'Yes' ELSE 'No' END) WEEKEND
	   ,ShiftTypeId
	FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		INNER JOIN EEmployeeShitLists C ON C.EmployeeId =A.ID 
		INNER JOIN EShifts D ON D.Id=C.ShiftId
		
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN EEmployeeWOLists EEWO ON EEWO.EmployeeId=A.Id AND (EEWO.OffDayId +1) =DATEPART(DW,ED.EngDate)
	WHERE ShiftTypeId=0 AND A.BranchId=@BRANCH_ID AND A.Id=@EMPLOYEE_ID
	UNION ALL 
	SELECT B.* FROM(
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
	    ED.NepDate nepDate,
	   (CASE WHEN ER.ShiftId >0 THEN 'No' ELSE 'Yes' END) WEEKEND
	   ,ShiftTypeId
		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN ERosters ER ON ER.EmployeeId =A.ID AND ER.Date =ED.EngDate

		LEFT JOIN EShifts D ON D.Id=ER.ShiftId
		
		WHERE ShiftTypeId=2  AND A.BranchId=@BRANCH_ID AND A.Id=@EMPLOYEE_ID
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
	   (CASE WHEN @LANGUAGE='EN' OR (ES.NameNp IS NULL OR ES.NAMENP='')  THEN ES.Name ELSE ES.NameNp END) SECTIONNAME,
	   ES.NameNp SECTIONNAMENP,
	   EDEP.Code DEPARTMENTCODE,
	   (CASE WHEN @LANGUAGE='EN' OR (EDEP.NameNp IS NULL OR EDEP.NAMENP='')  THEN EDEP.Name ELSE EDEP.NameNp END) DEPARTMENTNAME,
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
	    ED.NepDate nepDate,
	   (CASE WHEN ER.ShiftId >0 THEN 'No' ELSE 'Yes' END) WEEKEND
	   ,ShiftTypeId
		FROM EEmployees A
		INNER JOIN ESections  ES ON ES.Id=A.SectionId
		INNER JOIN EDepartments EDEP ON EDEP.Id=ES.DepartmentId
		LEFT JOIN EDateTables ED ON ED.EngDate >=@FROMDATE AND ED.EngDate<=@TODATE
		LEFT JOIN EWEEKLYROSTERS ER ON ER.EmployeeId =A.ID AND (ER.Day+1) =DATEPART(DW,ED.EngDate)

		LEFT JOIN EShifts D ON D.Id=ER.ShiftId
		
		WHERE ShiftTypeId=1  AND A.BranchId=@BRANCH_ID AND A.Id=@EMPLOYEE_ID
		) W
	) AS A
	
	


	LEFT JOIN (SELECT EH.Name,EH.NameNp,EHD.FiscalYearId,EHD.BeginDate,EHD.EndDate FROM EHolidays EH INNER JOIN 
							  EHolidayDetails EHD ON EHD.HolidayId =EH.Id AND cONVERT(DATE,EHD.BeginDate) >=@FROMDATE AND cONVERT(DATE,EHD.EndDate)<=@TODATE
						  WHERE EH.BranchId=@BRANCH_ID
						  ) AS H
		ON cONVERT(DATE,H.BeginDate)<=A.EngDate AND cONVERT(DATE,H.EndDate)>=A.EngDate 
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
	   a.nepDate,
	   WEEKEND,
	   ShiftTypeId
	   ,H.Name 
	   ,h.NameNp
	,H.BeginDate
	 ,H.EndDate 
	 ,LEAVENAME
	 ,[From]
	 ,[To]
	 )T
	 WHERE T.RN=1
 ORDER BY EmployeeId
END
go
exec SP_GET_Roster_REPORT 3,'2017/9/17','2017/10/18'