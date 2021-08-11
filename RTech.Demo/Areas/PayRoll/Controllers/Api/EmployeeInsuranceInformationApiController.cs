using Riddhasoft.Employee.Services;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.PayRoll.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.PayRoll.Controllers.Api
{
    public class EmployeeInsuranceInformationApiController : ApiController
    {
        SEmployeeInsuranceInformation _employeeInsuranceInformationServices = null;
        SEmployee _employeeServices = null;
        SInsuranceCompany _insuranceCompanyServices = null;
        LocalizedString _loc = null;
        int BranchId = (int)RiddhaSession.BranchId;
        public EmployeeInsuranceInformationApiController()
        {
            _employeeInsuranceInformationServices = new SEmployeeInsuranceInformation();
            _employeeServices = new SEmployee();
            _insuranceCompanyServices = new SInsuranceCompany();
            _loc = new LocalizedString();
        }

        [HttpPost]
        public KendoGridResult<List<EmployeeInsuranceCompanyGridVM>> GetEmpInsuranceInfoKendoGrid(KendoPageListArguments vm)
        {
            var branchId = RiddhaSession.BranchId;
            IQueryable<EEmployeeInsuranceInformation> insuranceInfoQuery = _employeeInsuranceInformationServices.List().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = insuranceInfoQuery.Count();

            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            IQueryable<EEmployeeInsuranceInformation> paginatedQuery;
            switch (searchField)
            {
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = insuranceInfoQuery.Where(x => x.Employee.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Employee.Name);
                    }
                    else
                    {
                        paginatedQuery = insuranceInfoQuery.Where(x => x.Employee.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Employee.Name);
                    }
                    break;
                case "InsuranceCompanyName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = insuranceInfoQuery.Where(x => x.InsuranceCompany.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Employee.Name);
                    }
                    else
                    {
                        paginatedQuery = insuranceInfoQuery.Where(x => x.InsuranceCompany.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Employee.Name);
                    }
                    break;
                case "PolicyNo":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = insuranceInfoQuery.Where(x => x.PolicyNo.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Employee.Name);
                    }
                    else
                    {
                        paginatedQuery = insuranceInfoQuery.Where(x => x.PolicyNo == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Employee.Name);
                    }
                    break;

                default:
                    paginatedQuery = insuranceInfoQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Employee.Name);
                    break;
            }
            var insuranceInfoLst = (from c in paginatedQuery.ToList()
                                    select new EmployeeInsuranceCompanyGridVM()
                                    {
                                        Id = c.Id,
                                        EmployeeId = c.EmployeeId,
                                        EmployeeName = c.Employee.Name,
                                        InsuranceCompanyId = c.InsuranceCompanyId,
                                        InsuranceCompanyName = c.InsuranceCompany.Name,
                                        InsuraneDocument = c.InsuraneDocument,
                                        PolicyNo = c.PolicyNo,
                                        PolicyAmount = c.PolicyAmount,
                                        PremiumAmount = c.PremiumAmount,
                                        ExpiryDate = c.ExpiryDate.ToString("yyyy/MM/dd"),
                                        IssueDate = c.IssueDate.ToString("yyyy/MM/dd")
                                    }).ToList();
            return new KendoGridResult<List<EmployeeInsuranceCompanyGridVM>>()
            {
                Data = insuranceInfoLst.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = insuranceInfoLst.Count()
            };
        }
        [HttpPost]
        public ServiceResult<EEmployeeInsuranceInformation> Post(EEmployeeInsuranceInformation model)
        {
            model.BranchId = BranchId;
            var result = _employeeInsuranceInformationServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8003", "7251", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<EEmployeeInsuranceInformation>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPut]
        public ServiceResult<EEmployeeInsuranceInformation> Put(EEmployeeInsuranceInformation model)
        {
            model.BranchId = BranchId;
            var result = _employeeInsuranceInformationServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8003", "7252", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<EEmployeeInsuranceInformation>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpDelete]
        public ServiceResult<int> Delete(int Id)
        {
            var data = _employeeInsuranceInformationServices.List().Data.Where(x => x.Id == Id).FirstOrDefault();
            var result = _employeeInsuranceInformationServices.Remove(data);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8003", "7253", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<List<EEmployeeInsuranceInformation>> Get()
        {

            var result = _employeeInsuranceInformationServices.List().Data.Where(x => x.BranchId == BranchId).ToList();

            return new ServiceResult<List<EEmployeeInsuranceInformation>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<List<EEmployeeInsuranceInformation>> GetEmpInsuranceInfo(int EmpId) {


            var result = _employeeInsuranceInformationServices.List().Data.Where(x => x.EmployeeId == EmpId).ToList();

            return new ServiceResult<List<EEmployeeInsuranceInformation>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EEmployeeInsuranceInformation> Get(int Id)
        {

            var result = _employeeInsuranceInformationServices.List().Data.Where(x => x.Id == Id).FirstOrDefault();

            return new ServiceResult<EEmployeeInsuranceInformation>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<DropDownVM>> GetInsuranceCompanies()
        {

            var result = (from c in _insuranceCompanyServices.List().Data.Where(x => x.BranchId == BranchId).ToList()
                          select new DropDownVM()
                          {
                              Id = c.Id,
                              Code = c.Code,
                              Name = c.Name
                          }).ToList();
            return new ServiceResult<List<DropDownVM>>()
            {
                Data = result,
                Status = ResultStatus.Ok

            };
        }

        [HttpGet]
        public ServiceResult<List<DropDownVM>> GetEmployees()
        {
            var result = (from c in _employeeServices.List().Data.Where(x => x.BranchId == BranchId).ToList()
                          select new DropDownVM()
                          {
                              Id = c.Id,
                              Code = c.Code,
                              Name = c.Name
                          }).ToList();
            return new ServiceResult<List<DropDownVM>>()
            {
                Data = result,
                Status = ResultStatus.Ok

            };
        }

    }

    public class EmployeeInsuranceCompanyGridVM
    {

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int InsuranceCompanyId { get; set; }
        public string EmployeeName { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string PolicyNo { get; set; }
        public string PolicyAmount { get; set; }
        public decimal PremiumAmount { get; set; }
        public string InsuraneDocument { get; set; }
        public string IssueDate { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class DropDownVM
    {

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }

    }
}
