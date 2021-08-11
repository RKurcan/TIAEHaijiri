
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Raz Basnet>
-- Create date: <2020/11/13>
-- Description:	<Employee Salary History Report>
-- =============================================
alter PROCEDURE [dbo].[SP_Employee_Salary_History]
(
	@FISCALYEAR_ID INT=3241,
	@EMP_IDS VARCHAR(MAX)='525,490'
	)
AS
BEGIN
	SET NOCOUNT ON;
    select ee.Name Employee,edep.Name Department,es.Name Section,ed.Name Designation,ed.DesignationLevel DesignationLevel ,MSP.Grade GradeValue,MSP.GradeName GradeName,
	MSP.BasicSalary BasicSalary,MSP.GrossSalary GrossSalary , MSP.TaxableAmount TaxableAmount,MSP.SocialSecurityTax , MSP.RenumerationTax RenumerationTax,
	MSP.PFEE PFEmployee, MSP.PFER PFEmployeer, MSP.Gratituity, MSP.SSEE SSEmployee , MSP.SSER SSEmployeer, MSP.InsurancePremiumAmount InsurancePremiumAmount, MSP.CITAmount  CITAmount ,
	MSP.[Absent] [Absent],MSP.Leave Leave , MSP.LateIn LateIn ,MSP.EarlyOut EarlyOut , MSP.DeductionAmount DeductionAmount, MSP.RebateAmount RebateAmount , MSP.NetSalary NetSalary,
	MSP.PensionFundEE PensionFundEmployee, MSP.PensionFundER PensionFundEmployeer, MSP.InsurancePaidbyOffice InsurancePaidbyOffice, MSP.AdditionAmount AdditionAmount
    from EMonthlySalarySheetPostings  MSP
	inner join EEmployees ee on  ee.Id =MSP.EmployeeId  
	inner join EDesignations  ed on  ed.Id =ee.DesignationId 
	inner join ESections es on es.Id =ee.SectionId  
	inner join EDepartments edep on  edep.Id=es.DepartmentId 
	where MSP.FiscalYearId = @FISCALYEAR_ID and MSP.EmployeeId IN (select items from dbo.split(@EMP_IDS,','))
	order by edep.Name,es.Name,ed.DesignationLevel,ee.Name
END
GO
exec [dbo].[SP_Employee_Salary_History]


--select * from EMonthlySalarySheetPostings where EmployeeId =525 and FiscalYearId =3241