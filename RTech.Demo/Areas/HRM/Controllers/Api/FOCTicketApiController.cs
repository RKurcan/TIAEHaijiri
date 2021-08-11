using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class FOCTicketApiController : ApiController
    {
        SFOCTicket _focTicketServices = null;
        SEmployee _employeeServices = null;
        LocalizedString _loc = null;
        public FOCTicketApiController()
        {
            _focTicketServices = new SFOCTicket();
            _employeeServices = new SEmployee();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<FocGridVm>> Get()
        {
            int branchId = (int)RiddhaSession.BranchId;
            List<FocGridVm> data = _focTicketServices.GeFOCTicketList(branchId, RiddhaSession.FYId);
            return new ServiceResult<List<FocGridVm>>()
            {
                Data = data,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<FocTicketViewModel> GetFOCTicket(int id)
        {
            int branchId = (int)RiddhaSession.BranchId;
            FocTicketViewModel vm = new FocTicketViewModel();
            vm.FOCTicket = _focTicketServices.List().Data.Where(x => x.Id == id && x.BranchId == branchId).FirstOrDefault();
            vm.FOCTicketDetail = _focTicketServices.ListDetails().Data.Where(x => x.FOCTicketId == vm.FOCTicket.Id).ToList();
            return new ServiceResult<FocTicketViewModel>()
            {
                Data = vm,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("7180")]
        public ServiceResult<FocTicketViewModel> Post(FocTicketViewModel vm)
        {
            vm.FOCTicket.BranchId = (int)RiddhaSession.BranchId;
            vm.FOCTicket.CreatedById = RiddhaSession.UserId;
            vm.FOCTicket.CreatedOn = System.DateTime.Now;
            vm.FOCTicket.FiscalYearId = RiddhaSession.FYId;
            var result = _focTicketServices.Add(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8014", "7180", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.FOCTicket.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<FocTicketViewModel>()
            {
                Data = vm,
                Status = result.Status,
                Message = _loc.Localize(result.Message)
            };
        }
        [ActionFilter("7181")]
        public ServiceResult<FocTicketViewModel> Put(FocTicketViewModel vm)
        {
            var result = _focTicketServices.Update(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8014", "7181", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.FOCTicket.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<FocTicketViewModel>()
            {
                Data = vm,
                Message = _loc.Localize(result.Message),
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("7182")]
        public ServiceResult<int> Delete(int id)
        {
            var foc = _focTicketServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _focTicketServices.Remove(foc);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8014", "7182", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPost]
        public ServiceResult<List<EmpSearchViewModel>> GetEmpLstForAutoComplete(EKendoAutoComplete model)
        {
            string lang = RiddhaSession.Language;
            int? branchId = RiddhaSession.BranchId;
            List<EmpSearchViewModel> resultLst = new List<EmpSearchViewModel>();
            if (model != null)
            {
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                var empLst = _employeeServices.List().Data.Where(x => x.BranchId == branchId && x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated);
                if (lang == "ne")
                {
                    resultLst = (from c in empLst
                                 where c.Name.ToLower().Contains(searchText.ToLower()) || c.NameNp.Contains(searchText)
                                 select new EmpSearchViewModel()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     Name = c.Code + " - " + (!string.IsNullOrEmpty(c.NameNp) ? c.NameNp : c.Name) + " - " + (c.Mobile ?? ""),
                                     EmployeeName = c.Name,
                                     Designation = c.Designation == null ? "" : c.Designation.Name,
                                     Section = c.Section == null ? "" : c.Section.Name,
                                     Department = c.Section.Department == null ? "" : c.Section.Department.Name,
                                     DesignationId = c.DesignationId,
                                     Photo = c.ImageUrl,
                                 }).OrderBy(x => x.Name).ToList();
                }
                else
                {
                    resultLst = (from c in empLst
                                 where c.Name.ToLower().Contains(searchText.ToLower()) || c.NameNp.Contains(searchText)
                                 select new EmpSearchViewModel()
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     Name = c.Code + " - " + c.Name + " - " + (c.Mobile ?? ""),
                                     EmployeeName = c.Name,
                                     Designation = c.Designation == null ? "" : c.Designation.Name,
                                     Section = c.Section == null ? "" : c.Section.Name,
                                     Department = c.Section.Department == null ? "" : c.Section.Department.Name,
                                     DesignationId = c.DesignationId,
                                     Photo = c.ImageUrl,
                                 }).OrderBy(x => x.Name).ToList();
                }
            }
            return new ServiceResult<List<EmpSearchViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetRecommendedByForDropdown()
        {
            string language = RiddhaSession.Language.ToString();
            int? branchId = RiddhaSession.BranchId;
            List<DropdownViewModel> resultLst = (from c in _employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager == true).ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = language == "ne" &&
                                                     c.NameNp != null ? c.NameNp : c.Name
                                                 }
                                                   ).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }
        [HttpPost, ActionFilter("7179")]
        public KendoGridResult<List<FocTicketGridVm>> GetFOCKendoGrid(KendoPageListArguments vm)
        {
            var branchId = RiddhaSession.BranchId;
            var empQuery = _employeeServices.List().Data;
            SFOCTicket service = new SFOCTicket();
            IQueryable<EFOCTicket> focTicketQuery = service.List().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = focTicketQuery.Count();
            var focTicketlist = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                                 select new FocTicketGridVm()
                                 {
                                     Id = c.Id,
                                     AppliedDate = c.AppliedDate.ToString("yyyy/MM/dd"),
                                     ApprovedById = c.ApprovedById,
                                     ApprovedByName = getapprovedByName(empQuery, c.ApprovedById),
                                     BranchId = c.BranchId,
                                     Department = c.Employee.Section.Department.Name,
                                     EmployeeId = c.EmployeeId,
                                     Name = c.Employee.Name,
                                     Rebate = c.Rebate,
                                     RecommendedBy = c.RecommendedBy,
                                     RecommendedByName = getrecommendedByName(empQuery, c.RecommendedBy),
                                     RequestType = Enum.GetName(typeof(RequestType), c.RequestType),
                                     Code = c.Employee.Code,
                                     Designation = c.Employee.Designation.Name,
                                     Section = c.Employee.Section.Name,
                                     Photo = c.Employee.ImageUrl,
                                     IsApproved = c.IsApproved,
                                     ApprovedOn = c.ApprovedOn == null ? "" : c.ApprovedOn.Value.ToString("yyyy/MM/dd")

                                 }).ToList();
            return new KendoGridResult<List<FocTicketGridVm>>()
            {
                Data = focTicketlist,
                Status = ResultStatus.Ok,
                TotalCount = totalRowNum
            };
        }
        private string getrecommendedByName(IQueryable<EEmployee> empQuery, int recommendedById)
        {
            if (recommendedById == 0)
            {
                return "";
            }
            else
            {
                return empQuery.Where(x => x.Id == recommendedById).FirstOrDefault().Name;
            }
        }
        private string getapprovedByName(IQueryable<EEmployee> empQuery, int? approvedById)
        {
            if (approvedById == null || approvedById == 0)
            {
                return "";
            }
            else
            {
                return empQuery.Where(x => x.Id == (int)approvedById).FirstOrDefault().Name;
            }
        }

        #region FOC Approval
        [HttpPost]
        public KendoGridResult<List<FocTicketGridVm>> GetFOCApprovalKendoGrid(KendoPageListArguments vm)
        {
            var branchId = RiddhaSession.BranchId;
            var empQuery = _employeeServices.List().Data;
            SFOCTicket service = new SFOCTicket();
            IQueryable<EFOCTicket> focTicketQuery = service.List().Data.Where(x => x.BranchId == branchId);
            int totalRowNum = focTicketQuery.Count();
            var focTicketlist = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                                 select new FocTicketGridVm()
                                 {
                                     Id = c.Id,
                                     AppliedDate = c.AppliedDate.ToString("yyyy/MM/dd"),
                                     ApprovedById = c.ApprovedById,
                                     ApprovedByName = getapprovedByName(empQuery, c.ApprovedById),
                                     BranchId = c.BranchId,
                                     Department = c.Employee.Section.Department.Name,
                                     EmployeeId = c.EmployeeId,
                                     Name = c.Employee.Name,
                                     Rebate = c.Rebate,
                                     RecommendedBy = c.RecommendedBy,
                                     RecommendedByName = getrecommendedByName(empQuery, c.RecommendedBy),
                                     RequestType = Enum.GetName(typeof(RequestType), c.RequestType),
                                     Code = c.Employee.Code,
                                     Designation = c.Employee.Designation.Name,
                                     Section = c.Employee.Section.Name,
                                     Photo = c.Employee.ImageUrl,
                                     IsApproved = c.IsApproved,
                                     ApprovedOn = c.ApprovedOn == null ? "" : c.ApprovedOn.Value.ToString("yyyy/MM/dd")

                                 }).ToList();
            return new KendoGridResult<List<FocTicketGridVm>>()
            {
                Data = focTicketlist,
                Status = ResultStatus.Ok,
                TotalCount = totalRowNum
            };
        }
        [HttpGet]
        public ServiceResult<EFOCTicket> Approve(int id)
        {
            string msg = "";
            var focTicket = _focTicketServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (focTicket != null)
            {
                if (focTicket.IsApproved == false)
                {
                    //focTicket.ApprovedById = RiddhaSession.CurrentUser.Id;
                    focTicket.ApprovedOn = System.DateTime.Now;
                    focTicket.IsApproved = true;
                    var result = _focTicketServices.Approve(focTicket);

                    if (result.Status == ResultStatus.Ok)
                    {
                        Common.AddAuditTrail("8015", "7191", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
                        msg = "Approved Successfully";
                    }
                }
                else
                {
                    msg = "Already Approved";
                }
            }
            return new ServiceResult<EFOCTicket>()
            {
                Data = focTicket,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<EFOCTicket> Revert(int id)
        {
            string msg = "";
            var focTicket = _focTicketServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (focTicket != null)
            {
                if (focTicket.IsApproved == true)
                {
                    //focTicket.ApprovedById = RiddhaSession.CurrentUser.Id;
                    focTicket.ApprovedOn = null;
                    focTicket.IsApproved = false;
                    var result = _focTicketServices.Revert(focTicket);
                    if (result.Status == ResultStatus.Ok)
                    {
                        Common.AddAuditTrail("8015", "7192", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
                        msg = "Revert Successfully";
                    }
                }
                else
                {
                    msg = "Already Revert";
                }
            }
            return new ServiceResult<EFOCTicket>()
            {
                Data = focTicket,
                Message = msg,
                Status = ResultStatus.Ok
            };
        }

        #endregion


    }

    public class FocTicketGridVm : EmpSearchViewModel
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int EmployeeId { get; set; }
        public string AppliedDate { get; set; }
        public int Rebate { get; set; }
        public string RequestType { get; set; }
        public int RecommendedBy { get; set; }
        public string RecommendedByName { get; set; }
        public int? ApprovedById { get; set; }
        public string ApprovedByName { get; set; }
        public string ApprovedOn { get; set; }
        public bool IsApproved { get; set; }
    }
}
