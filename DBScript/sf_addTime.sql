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
CREATE FUNCTION sf_AddTime
(
	-- Add the parameters for the function here
	 @RequestedTime TIME(0),
        @TimeIntervel TIME(0)
)
RETURNS time
AS
BEGIN
	-- Declare the return variable here
	declare @result time;
	SELECT @result= DATEADD(SECOND,DATEDIFF(SECOND,'00:00:00',@RequestedTime),@TimeIntervel);

	-- Return the result of the function
	RETURN @result

END
GO

