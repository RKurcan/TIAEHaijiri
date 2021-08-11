-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE SP_Post_AttendanceData 
	 @BRANCH_CODE VARCHAR(10),@USER_PIN INT,@VERIFY_MODE INT,@VERIFY_TIME DATETIME,@TEMPERATURE DECIMAL(18,2),@DEVICE_SN varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--branch validation

	DECLARE @BRANCH_ID AS INT,
			@DEVICE_ID AS INT,
			@EMP_ID AS INT,
			@ATT_LOG_ID INT,
			@has_error bit =1
	SELECT TOP 1 @BRANCH_ID= ID FROM EBranches WHERE Code=@BRANCH_CODE

	IF @BRANCH_ID>0
	BEGIN
	    --SELECT @DEVICE_SN= REPLACE(@DEVICE_SN,'?','')
		SET @DEVICE_ID=(SELECT top 1 ID FROM EDEVICES WHERE ID IN( SELECT  DeviceId FROM ECompanyDeviceAssignments WHERE CompanyId in(select TOP 1 CompanyId from EBranches where id=@BRANCH_ID))and 
		CAST( SerialNumber AS varchar(100))= @DEVICE_SN)
		
		
		IF @DEVICE_ID>0
		BEGIN
			SELECT TOP 1 @EMP_ID= ID FROM EEMPLOYEES WHERE DeviceCode=@USER_PIN AND BranchId=@BRANCH_ID

			IF @EMP_ID>0
			BEGIN
			    SELECT TOP 1 @ATT_LOG_ID= ID FROM EAttendanceLogs WHERE DeviceId=@DEVICE_ID AND EmployeeId=@EMP_ID AND DateTime=@VERIFY_TIME

				IF @ATT_LOG_ID>0
				BEGIN
			    --RETURN
				SELECT @has_error hasError,'Already Exists' [message]

				END
				ELSE
				BEGIN
				 INSERT INTO EAttendanceLogs (DeviceId,EmployeeId,VerifyMode,DateTime,Remark,IsDelete,CompanyCode,Temperature)
											VALUES(@DEVICE_ID,@EMP_ID,@VERIFY_MODE,@VERIFY_TIME,'',0,@BRANCH_CODE,@TEMPERATURE)

			     set @has_error=0
				 SELECT 0 hasError,'INSERTED' [message]
				END
			END
			ELSE
			BEGIN
			 SELECT @has_error hasError,'Employee Not found' [message]
			END
		END
		ELSE
		BEGIN
		 SELECT @has_error hasError,'Invalid Device Serial Number' [message]
		END
	END
	ELSE
	BEGIN
	SELECT @has_error hasError,'INVALID BRANCH CODE ' [message]
	END


END
GO
EXEC SP_Post_AttendanceData 'nphq','106',1,'1990-06-02 14:55:55',0.0,N'‎6209204400008'
