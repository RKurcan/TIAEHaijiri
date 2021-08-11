
USE hrmplDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[SP_Get_Leave_Application]
 @branchId int =1,
 @empIds nvarchar(max) ='490',
 @lang nvarchar(10)='En'
AS
BEGIN
select el.Id Id, el.EmployeeId EmployeeId,ee.Code EmployeeCode, 
(CASE WHEN @lang='EN' OR (ee.NameNp IS NULL OR ee.NAMENP='')  THEN ee.Code+' - '+ ee.Name ELSE ee.Code+' - '+ee.NameNp END) EmployeeName,
(CASE WHEN @lang='EN' OR (elm.NameNp IS NULL OR elm.NAMENP='')  THEN elm.Name ELSE elm.NameNp END) LeaveMaster,
el.LeaveMasterId LeaveMasterId, CONVERT (varchar(10),el.[From],111) [From] , CONVERT (varchar(10),el.[To],111) [To],
(Case when el.LeaveDay = 0 then 'FullDay' when el.LeaveDay =1 then 'EarlyLeave' else 'LateLeave' end) LeaveDayName, el.LeaveDay  LeaveDay ,
el.[Description] [Description], el.ApprovedById ApprovedById , 
(Case when el.LeaveStatus = 0 then 'New' when el.LeaveStatus =1 then 'Approve' when el.LeaveStatus =2  then'Reject'  else 'Revert' end) LeaveStatusName,
el.LeaveStatus LeaveStatus, datediff(D,el.[From],el.[To]) +1 [Days] ,
(CASE WHEN @lang='EN' OR (eu.NameNp IS NULL OR eu.NAMENP='')  THEN eu.Name ELSE eu.NameNp END) ApprovedByUser
 from ELeaveApplications el
inner join EEmployees ee on  ee.Id = el.EmployeeId
inner join ELeaveMasters elm on el.LeaveMasterId = elm.Id
left join EEmployees eu on eu.Id = el.ApprovedById
Where el.BranchId =@branchId 
--and el.EmployeeId in (select items from dbo.split(@empIds,',')) 

END
go
exec [dbo].[SP_Get_Leave_Application]





