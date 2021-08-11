USE [hrmplDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LEAVEINFORMATION]    Script Date: 4/5/2019 3:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GET_LEAVEINFORMATION](


 @EMPLOYEE_ID INT =2
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
select EL.Id LeaveId, EL.Name LeaveName,EDLB.Balance,convert(decimal,ISNULL( DATEDIFF(D, ELA.[From],ELA.[To])+1,0)) TakenLeave,convert(decimal,ISNULL(EDLB.Balance- (DATEDIFF(D, ELA.[From],ELA.[To])+1),EDLB.Balance)) RemLeave,
--COUNT(CASE --WHEN ELA.LeaveStatus IS NULL THEN 0  
--			WHEN ELA.LeaveStatus=0 THEN 1
--			ELSE 0 END
--) OVER (PARTITION BY EL.ID) 
0.0 UnapprovedLeave from ELeaveMasters EL
	INNER JOIN EDesignationWiseLeavedBalances EDLB ON EDLB.LeaveId =EL.Id
	
	INNER JOIN EEmployees EE ON EE.DesignationId=EDLB.DesignationId AND EE.Id=@EMPLOYEE_ID
	LEFT JOIN ELeaveApplications ELA ON ELA.LeaveMasterId=EDLB.LeaveId AND ELA.EmployeeId=EE.Id
END
