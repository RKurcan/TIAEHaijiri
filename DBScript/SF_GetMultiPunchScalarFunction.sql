-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
alter FUNCTION SF_GetMultiPunch
(
	-- Add the parameters for the function here
	@EmpId int,
	@OnDate Datetime
)
RETURNS varchar(Max)
AS
BEGIN
	-- Declare the return variable here
	Declare @val Varchar(MAX); 
Select @val = COALESCE(@val + ', ' + Convert(varchar(10),[DateTime],108), Convert(varchar(10),[DateTime],108)) 
        From EAttendanceLogs where EmployeeId=@EmpId and convert(varchar(10), DateTime,111)=@OnDate

	-- Add the T-SQL statements to compute the return value here
	
	-- Return the result of the function
	RETURN @val;

END
GO


