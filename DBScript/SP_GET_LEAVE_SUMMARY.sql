USE [hrmplDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LEAVE_SUMMARY]    Script Date: 08/02/2020 3:37:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GET_LEAVE_SUMMARY](

 @BRANCH_ID INT =1,
 @FROMDATE DATETIME='2017-01-01',
 @TODATE DATETIME='2020-07-02',
 @LANGUAGE VARCHAR(2)='en',
 @FISCALYEAR int =4339
)
AS
BEGIN	
SET ARITHABORT ON
SET NOCOUNT ON
--DECLARE @CURDATE DATE=GETDATE();
--DECLARE @BRANCH_ID INT=0;
-- SELECT @BRANCH_ID=EM.BranchId FROM EEmployees EM
--	INNER JOIN	ESections ES ON ES.Id = EM.SectionId
--	INNER JOIN EDepartments ED ON ED.Id= ES.DepartmentId
--	INNER JOIN EBranches EB ON EB.Id=ES.BranchId 
--	WHERE EM.Id=@EMPLOYEE_ID
select ee.Code,ee.Id as EmployeeId,ee.Name,ee.EmploymentStatus, EL.Id LeaveId, EL.Name LeaveName,EDLB.Balance,
sum( ISNULL(CONVERT(decimal, DATEDIFF(D, ELA.[From],ELA.[To]))+(case when ELA.LeaveDay=0 then 1 else .5 end),0)) TakenLeave,
(EDLB.Balance- sum( ISNULL( CONVERT(decimal, DATEDIFF(D, ELA.[From],ELA.[To]))+(case when ELA.LeaveDay=0 then 1 else .5 end),0))) RemLeave,
--COUNT(CASE --WHEN ELA.LeaveStatus IS NULL THEN 0  
--			WHEN ELA.LeaveStatus=0 THEN 1
--			ELSE 0 END
--) OVER (PARTITION BY EL.ID) 
0 UnapprovedLeave,ISNUll(EDES.Name,'') DesignationName,ISnull(EDES.DesignationLevel,0) DesignationLevel,IsNUll(EDEP.Name,'') DepartmentName,ISNULL(ES.Name,'') SectionName
 from ELeaveMasters EL
	INNER JOIN EDesignationWiseLeavedBalances EDLB ON EDLB.LeaveId =EL.Id
	INNER JOIN EEmployees EE ON EE.DesignationId=EDLB.DesignationId and ee.BranchId=@BRANCH_ID
	Inner Join ESections ES on ES.Id=EE.SectionId 
	inner join EDepartments EDEP on EDEP.Id=ES.DepartmentId
	Inner Join EDesignations EDES on EDES.Id=EE.DesignationId
	LEFT JOIN ELeaveApplications ELA ON ELA.LeaveMasterId=EDLB.LeaveId AND ELA.EmployeeId=EE.Id and ELA.LeaveStatus=1
	inner join EleaveApplicationLogs lal on lal.LeaveApplicationId  = ELA.Id
	inner join EFiscalYears FY  on FY.Id = lal.FiscalYearId
    WHERE lal.FiscalYearId = @FISCALYEAR
	GROUP BY  FY.Id ,ee.Code,ee.Id,ee.Name,ee.EmploymentStatus, EL.Id, EL.Name,EDLB.Balance,EDES.DesignationLevel,EDES.Name,EDep.Name,Es.Name
	Order By DepartmentName,SectionName,DesignationLevel,ee.Name
	
END

go 
exec [SP_GET_LEAVE_SUMMARY]
