USE [hrmplDB]
GO
/****** Object:  UserDefinedFunction [dbo].[sf_NEPTOENG]    Script Date: 30/11/2017 4:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER FUNCTION [dbo].[sf_NEPTOENG]
(	
	-- Add the parameters for the function here
	@value varchar(10)
)
RETURNS dATE
AS
begin
	declare @result DATETIME;
	SELECT TOP 1 @result= EngDate FROM EDateTables WHERE NepDate=@value
	return @result
end
