USE [hrmplDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_MultiPunch_Report]    Script Date: 10/12/2020 10:12:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GET_MultiPunch_Report](
	-- Add the parameters for the stored procedure here
	@BRANCH_ID INT =1,
	@FROM_DATE DATETIME,
	@TO_DATE DATETIME,
	@Language varchar(max) = 'en'
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
select em.Id EmployeeId, em.Code, (case when @Language = 'en' then em.Name else (case when  em.NameNp is null or em.NameNp=''  then em.Name else em.NameNp end) end) Name,ed.EngDate [Date] 
,DATENAME(WEEKDAY,ed.EngDate) [Day],dbo.SF_GetMultiPunch(em.Id,ed.EngDate) PunchTime,ehd.HolidayId,ehd.Name HolidayName,ld.Name LeaveName from eemployees em
  left join EDateTables ed on CONVERT(nvarchar(10),ed.EngDate,111) >= CONVERT(nvarchar(10),@FROM_DATE,111)  and CONVERT(nvarchar(10),ed.EngDate,111)  <=CONVERT(nvarchar(10),@TO_DATE,111) 
  left join(select eh.Name,ehd.HolidayId,ehd.BeginDate,ehd.EndDate from EHolidayDetails ehd
			inner join EHolidays eh on eh.id=ehd.HolidayId
             and BranchId=@BRANCH_ID)ehd
			 on ehd.BeginDate<=ed.EngDate and ehd.EndDate>=ed.EngDate

 left join(select m.Name,d.[From],d.[To],d.EmployeeId from ELeaveApplications d
			inner join ELeaveMasters m on m.id=d.LeaveMasterId 
             and d.BranchId=@BRANCH_ID)ld
			 on ld.EmployeeId=em.Id and ld.[From]<=ed.EngDate and ld.[To]>=ed.EngDate
	--inner join EAttendanceLogs eal on eal.EmployeeId=em.Id 
 where em.BranchId=@BRANCH_ID and CONVERT(nvarchar(10),ed.EngDate,111) >=CONVERT(nvarchar(10),@FROM_DATE,111) and CONVERT(nvarchar(10),ed.EngDate,111)<= CONVERT(nvarchar(10),@TO_DATE,111)
END

go 

