GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LEAVE_SUMMARY]    Script Date: 14/03/2020 2:07:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[SP_GET_Month_Wise_Expired_Company](

 
 @FROMDATE DATETIME='2020-03-14',
 @TODATE DATETIME='2020-04-12'
)
AS
BEGIN	
SET ARITHABORT ON
SET NOCOUNT ON


select comp.Id CompanyId, comp.Code CompanyCode  ,comp.Name CompanyName ,comp.Address CompanyAddress,
comp.ContactPerson CompanyContactPerson,comp.ContactNo CompanyContactNo ,
cl.ExpiryDate CompanyExpiryDate,res.Name ResellerName,res.Address ResellerAddress,
res.ContactPerson ResellerContactPerson,
res.ContactNo ResellerContactNo  ,res.Id ResellerId from ECompanies  comp 
inner join ECompanyLicenses cl on cl.CompanyId = comp.Id
inner join EResellers res on comp.ResellerId  = res.Id
where  cl.ExpiryDate >=@FROMDATE and cl.ExpiryDate <=@TODATE
	
END
go 
exec SP_GET_Month_Wise_Expired_Company  '2020/03/14','2020/04/12'



