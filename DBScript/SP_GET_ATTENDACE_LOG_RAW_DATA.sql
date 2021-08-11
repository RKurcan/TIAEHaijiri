
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[SP_GET_ATTENDACE_LOG_RAW_DATA](
	-- Add the parameters for the stored procedure here
	@BRANCH_ID INT =129,
	@FROM_DATE DATETIME,
	@TO_DATE DATETIME
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select ea.Id,ea.DeviceId,ea.EmployeeId,ea.VerifyMode,ea.DateTime,ea.Remark,ea.IsDelete,ea.CompanyCode,ea.Temperature from EBranches eb 
	inner join EEmployees  ee on eb.Id = ee.BranchId
	inner join EAttendanceLogs  ea on ee.Id  = ea.EmployeeId
	where eb.Id =@BRANCH_ID and  ea.DateTime>=@FROM_DATE AND ea.DateTime<=@TO_DATE
    
END
GO


