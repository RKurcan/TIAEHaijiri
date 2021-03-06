USE [hrmplDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_EmployeeInfo_REPORT]    Script Date: 3/5/2020 2:37:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GET_EmployeeInfo_REPORT](

	 @EMP_IDS VARCHAR(MAX)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
select ISNULL(Employees.Code,'') Code ,ISNULL(Employees.Name,'') FullName ,ISNULL(Sections.Name,'') SectionName,ISNULL( Designations.Name,'') DesignationName
 , ISNULL(GradeGroups.Name,'') GroupGradeName,ISNULL(Branches.Name,'') BranchesName,ISNULL(Companies.Name,'') CompanyName, ISNULL( CONVERT (varchar(10) ,Employees.DateOfJoin,111 ),'') DateOfJoin 
 , ISNULL( Employees.DeviceCode,'') DeviceCode , (CASE WHEN Employees.IsManager = 1 THEN 'True' else 'False' end) IsManager , 
   ISNULL( CONVERT (varchar(10) ,Employees.DateOfBirth,111 ),'') DateOfBirth , (case when Employees.MaritialStatus = 0 then 'Yes' else	'False' end) MaritialStatus,
  (Case When Employees.BloodGroup = 0 then 'APositive'
   when Employees.BloodGroup = 1 then 'ANeagtive' 
      when Employees.BloodGroup = 3 then 'BPositive' 
	     when Employees.BloodGroup = 4 then 'BNeagtive' 
		    when Employees.BloodGroup = 5 then 'ABPositive' 
			   when Employees.BloodGroup = 6 then 'ABNeagtive' 
			   when Employees.BloodGroup = 7 then 'OPositive' 
			   else	'ONeagtive' end) BloodGroup,
			   (case when  Employees.Gender=0 then 'Male'
			   
			   when  Employees.Gender=1 then 'Female'
			   else 'Others' end) Gender,
			   Isnull(Employees.Mobile,'') Mobile,
			   Isnull(Employees.Email,'') Email,
			    Isnull(Employees.PermanentAddress ,'') PermanentAddress,
				Isnull(Employees.TemporaryAddress ,'') TemporaryAddress,
				Isnull(Employees.PassportNo ,'') PassportNo,
				Isnull(Employees.CitizenNo ,'') CitizenNo,
				Isnull( convert ( varchar (10),Employees.IssueDate ,111), '' )IssueDate,
				Isnull(Employees.IssueDistict , '' )IssueDistrict,
				Isnull(Employees.ImageUrl , '' )ImageUrl
				

			   

 
  from EEmployees Employees 
inner join  dbo.Split(@EMP_IDS,',') t1 on t1.items=Employees.Id
left join dbo.ESections Sections on Sections.Id = Employees.SectionId
left join  dbo.EDesignations Designations on Designations.Id = Employees.DesignationId
left join dbo.EGradeGroups GradeGroups on GradeGroups.Id  = Employees.GradeGroupId
left join dbo.EBranches Branches on Branches.Id = Employees.BranchId
inner join dbo.ECompanies Companies on Companies.Id = Branches.CompanyId


  
END
go
exec SP_GET_EmployeeInfo_REPORT '490'
