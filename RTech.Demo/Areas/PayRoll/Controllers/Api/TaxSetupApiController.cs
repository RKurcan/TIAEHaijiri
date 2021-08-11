using Riddhasoft.OfficeSetup.Services;
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
    public class TaxSetupApiController : ApiController
    {
        STaxSetup _taxSetupServices = null;
        int BranchId = (int)RiddhaSession.BranchId;
        LocalizedString _loc = null;
        SFiscalYear _fiscalYearServices = null;
        public TaxSetupApiController()
        {
            _taxSetupServices = new STaxSetup();
            _loc = new LocalizedString();
            _fiscalYearServices = new SFiscalYear();
        }

        [HttpPost]
        public KendoGridResult<List<TaxSetupGridVM>> GetTaxSetupKendoGrid(KendoPageListArguments vm)
        {


            var branchId = RiddhaSession.BranchId;
            IQueryable<ETaxSetup> taxSetupInfoQuery = _taxSetupServices.List().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = taxSetupInfoQuery.Count();

            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            IQueryable<ETaxSetup> paginatedQuery;

            switch (searchField)
            {
                case "FiscalYear":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = taxSetupInfoQuery.Where(x => x.FiscalYear.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.FiscalYear);
                    }
                    else
                    {
                        paginatedQuery = taxSetupInfoQuery.Where(x => x.FiscalYear == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.FiscalYear);
                    }
                    break;

                default:
                    paginatedQuery = taxSetupInfoQuery.OrderByDescending(x => x.Id).ThenBy(x => x.FiscalYear);
                    break;
            }
            var result = (from c in paginatedQuery
                          select new TaxSetupGridVM()
                          {
                              Id = c.Id,
                              FiscalYear = c.FiscalYear,
                              FiscalYearId = c.FiscalYearId
                          }).ToList();
            return new KendoGridResult<List<TaxSetupGridVM>>()
            {

                Data = result.Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = result.Count
            };
        }

        [HttpPost]
        public ServiceResult<TaxSetupVM> Post(TaxSetupVM vm)
        {
            vm.TaxSetupMaster.BranchId = BranchId;
            vm.TaxSetupMaster.CreationDate = DateTime.Now;
            if (TaxSetupAlreadyExist(vm.TaxSetupMaster.FiscalYearId))
            {
                return new ServiceResult<TaxSetupVM>()
                {
                    Data = vm,
                    Message = "Task setup already exist for this fiscal year",
                    Status = ResultStatus.processError
                };

            }
            var taxSetupMaster = BindTaxSetupMaster(vm.TaxSetupMaster);
            var masterResult = _taxSetupServices.Add(taxSetupMaster);

            if (masterResult.Status == ResultStatus.Ok)
            {
                var taxSlabDetails = BindTaxSlabDetails(vm.TaxSlabDetails, masterResult.Data.Id);
                var taxSlabDetailResult = _taxSetupServices.AddTaxSlabDetails(taxSlabDetails);
                Common.AddAuditTrail("8022", "7234", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.TaxSetupMaster.Id, _loc.Localize(masterResult.Message));
            }

            return new ServiceResult<TaxSetupVM>()
            {
                Data = vm,
                Message = _loc.Localize(masterResult.Message),
                Status = masterResult.Status

            };
        }

        [HttpPut]
        public ServiceResult<TaxSetupVM> Put(TaxSetupVM vm)
        {
            vm.TaxSetupMaster.BranchId = BranchId;
            var taxSetupMaster = BindTaxSetupMaster(vm.TaxSetupMaster);
            var masterResult = _taxSetupServices.Update(taxSetupMaster);
            if (masterResult.Status == ResultStatus.Ok)
            {
                DeleteExistingTaxSlabDetails(masterResult.Data.Id);
                var taxSlabDetails = BindTaxSlabDetails(vm.TaxSlabDetails, masterResult.Data.Id);
                var taxSlabDetailResult = _taxSetupServices.AddTaxSlabDetails(taxSlabDetails);
                Common.AddAuditTrail("8022", "7235", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.TaxSetupMaster.Id, _loc.Localize(masterResult.Message));
            }

            return new ServiceResult<TaxSetupVM>()
            {
                Data = vm,
                Message = _loc.Localize(masterResult.Message),
                Status = masterResult.Status
            };
        }


        [HttpDelete]
        public ServiceResult<int> Delete(int Id)
        {
            var data = _taxSetupServices.List().Data.Where(x => x.Id == Id).FirstOrDefault();
            var result = _taxSetupServices.Remove(data);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8022", "7236", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {

                Data = 1,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }


        [HttpGet]
        public ServiceResult<TaxSetupVM> Get(int Id)
        {
            TaxSetupVM vm = new TaxSetupVM();
            vm = GetTaxSetupDetails(Id);
            return new ServiceResult<TaxSetupVM>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };

        }


        public ServiceResult<List<FiscalYearDropDown>> GetFiscalYears()
        {
            var result = (from c in _fiscalYearServices.List().Data.Where(x => x.BranchId == BranchId)
                          select new FiscalYearDropDown()
                          {
                              Id = c.Id,
                              Name = c.FiscalYear,
                              ISCurrent = c.CurrentFiscalYear
                          }).ToList();

            return new ServiceResult<List<FiscalYearDropDown>>()
            {
                Data = result,
                Status = ResultStatus.Ok
            };

        }

        public ServiceResult<TaxSetupVM> GetLastFiscalYearDetails()
        {

            var vm = new TaxSetupVM();
            var fiscalYears = _fiscalYearServices.List().Data.Where(x => x.BranchId == BranchId).ToList();

            var currentFiscalYear = fiscalYears.Where(x => x.CurrentFiscalYear == true).FirstOrDefault();

            var lastFiscalYear = (from c in fiscalYears
                                  where currentFiscalYear.StartDate.AddYears(-1).Year == c.StartDate.Year
                                  select c).LastOrDefault();

            if (lastFiscalYear != null)
            {
                var taxDetailId = _taxSetupServices.List().Data.Where(x => x.FiscalYearId == lastFiscalYear.Id).Select(x => x.Id).FirstOrDefault();

                if (taxDetailId == 0)
                {
                    return new ServiceResult<TaxSetupVM>()
                    {
                        Data = vm,
                        Message = "There is no tax setup in last fiscal year",
                        Status = ResultStatus.processError
                    };
                }
                vm = GetTaxSetupDetails(taxDetailId);
            }
            return new ServiceResult<TaxSetupVM>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };

        }

        private ETaxSetup BindTaxSetupMaster(TaxSetupMasterVM vm)
        {

            ETaxSetup taxSetupMaster = new ETaxSetup()
            {
                Id = vm.Id,
                BranchId = BranchId,
                CreationDate = vm.CreationDate,
                FiscalYear = vm.FiscalYear,
                FiscalYearId = vm.FiscalYearId,
                TaxPercAboveFinalValue = vm.TaxPercAboveFinalValue,
                DeductionLimitAmount = vm.DeductionLimitAmount,
                DeductionLimitRatio = vm.DeductionLimitRatio,
                RebatePercForFemaleUnmarried = vm.RebatePercForFemaleUnmarried,
                PFPercByEmployer = vm.PFPercByEmployer,
                PFPercByEmployee = vm.PFPercByEmployee,
                GratituityPercByEmployer = vm.GratituityPercByEmployer,
                SSPercByEmployer = vm.SSPercByEmployer,
                SSPercByEmployee = vm.SSPercByEmployee,
                PensionFundPercByEmployee = vm.PensionFundByEmployee,
                PensionFundPercByEmployer = vm.PensionFundByEmployer
            };
            return taxSetupMaster;
        }

        private List<ETaxSlabDetails> BindTaxSlabDetails(List<TaxSlabDetailsVM> TaxSlabDetails, int MasterId)
        {

            var taxSlabDetails = (from c in TaxSlabDetails
                                  select new ETaxSlabDetails()
                                  {
                                      SN = c.SN,
                                      TaxPerc = c.TaxPerc,
                                      IndividualAmount = c.IndividualAmount,
                                      CoupleAmount = c.CoupleAmount,
                                      TaxSetupId = MasterId
                                  }).ToList();
            return taxSlabDetails;
        }



        private bool DeleteExistingTaxSlabDetails(int Id)
        {

            var existingTaxSlabDetails = _taxSetupServices.GetTaxSlabDetails().Data.Where(x => x.TaxSetupId == Id).ToList();

            _taxSetupServices.DeleteTaxSlabDetails(existingTaxSlabDetails);

            return true;

        }


        private TaxSetupVM GetTaxSetupDetails(int Id)
        {

            // Get tax setup master detail
            TaxSetupVM vm = new TaxSetupVM();
            vm.TaxSetupMaster = (from c in _taxSetupServices.List().Data.Where(x => x.Id == Id)
                                 select new TaxSetupMasterVM()
                                 {
                                     Id = c.Id,
                                     CreationDate = c.CreationDate,
                                     FiscalYear = c.FiscalYear,
                                     FiscalYearId = c.FiscalYearId,
                                     TaxPercAboveFinalValue = c.TaxPercAboveFinalValue,
                                     DeductionLimitAmount = c.DeductionLimitAmount,
                                     DeductionLimitRatio = c.DeductionLimitRatio,
                                     RebatePercForFemaleUnmarried = c.RebatePercForFemaleUnmarried,
                                     PFPercByEmployer = c.PFPercByEmployer,
                                     PFPercByEmployee = c.PFPercByEmployee,
                                     GratituityPercByEmployer = c.GratituityPercByEmployer,
                                     SSPercByEmployee = c.SSPercByEmployee,
                                     SSPercByEmployer = c.SSPercByEmployer,
                                     PensionFundByEmployee  = c.PensionFundPercByEmployee,
                                     PensionFundByEmployer  =c.PensionFundPercByEmployer,
                                     BranchId = c.BranchId
                                 }).FirstOrDefault();

            // Get tax slab detail

            vm.TaxSlabDetails = (from c in _taxSetupServices.GetTaxSlabDetails().Data.Where(x => x.TaxSetupId == Id)
                                 select new TaxSlabDetailsVM()
                                 {
                                     Id = c.Id,
                                     SN = c.SN,
                                     TaxPerc = c.TaxPerc,
                                     IndividualAmount = c.IndividualAmount,
                                     CoupleAmount = c.CoupleAmount
                                 }).ToList();

            return vm;
        }

        [HttpGet]
        public ServiceResult<TaxSetupGridVM> CheckIfFiscalYearSetupAlreadyExist(int FiscalYearId)
        {

            TaxSetupGridVM vm = new TaxSetupGridVM();
            var data = _taxSetupServices.List().Data.Where(x => x.FiscalYearId == FiscalYearId).FirstOrDefault();
            if (data != null)
            {

                vm.Id = data.Id;
                vm.FiscalYearId = data.FiscalYearId;
                vm.FiscalYear = data.FiscalYear;
            }
            return new ServiceResult<TaxSetupGridVM>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }

        private bool TaxSetupAlreadyExist(int FiscalYearId)
        {

            var data = _taxSetupServices.List().Data.Where(x => x.FiscalYearId == FiscalYearId).FirstOrDefault();
            return data == null ? false : true;
        }
    }

    public class FiscalYearDropDown
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public bool ISCurrent { get; set; }


    }
    public class TaxSetupGridVM
    {

        public int Id { get; set; }
        public string FiscalYear { get; set; }

        public int FiscalYearId { get; set; }



    }
    public class TaxSetupVM
    {

        public TaxSetupMasterVM TaxSetupMaster { get; set; }
        public List<TaxSlabDetailsVM> TaxSlabDetails { get; set; }

    }

    public class TaxSetupMasterVM
    {


        public int Id { get; set; }
        public int FiscalYearId { get; set; }

        public string FiscalYear { get; set; }
        public DateTime CreationDate { get; set; }
        public int BranchId { get; set; }

        #region Tax Slab for tax value above final value 

        public decimal TaxPercAboveFinalValue { get; set; }

        #endregion
        #region Maximum Deduction Limit 

        /// <summary>
        /// Example :  IN + PF + CIT = Rs 300000 
        /// </summary>
        public decimal DeductionLimitAmount { get; set; }
        /// <summary>
        /// Example : 1 / 3 of gross salary
        /// </summary>
        public string DeductionLimitRatio { get; set; }

        /// <summary>
        /// Rebate perc of tax calculated 
        /// </summary>
        public decimal RebatePercForFemaleUnmarried { get; set; }

        #endregion


        #region SSF Information 

        /// <summary>
        /// Provident Fund  By Employer
        /// </summary>
        public decimal PFPercByEmployer { get; set; }



        /// <summary>
        /// Provident Fund  By Employee
        /// </summary>
        public decimal PFPercByEmployee { get; set; }


        /// <summary>
        /// Gratituity  By Employee
        /// </summary>
        public decimal GratituityPercByEmployer { get; set; }


        /// <summary>
        /// Social Security  By Employer
        /// </summary>
        public decimal SSPercByEmployer { get; set; }

        /// <summary>
        /// Social Security  By Employee

        /// </summary>
        public decimal SSPercByEmployee { get; set; }


        public decimal PensionFundByEmployer { get; set; }
        public decimal PensionFundByEmployee { get; set; }

        #endregion
    }
    public class TaxSlabDetailsVM
    {

        public int Id { get; set; }
        public int SN { get; set; }
        public decimal TaxPerc { get; set; }
        public decimal IndividualAmount { get; set; }
        public decimal CoupleAmount { get; set; }

    }

}
