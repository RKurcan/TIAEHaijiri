using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class KajApiController : ApiController
    {
        SKaj _kajServices = null;
        LocalizedString _loc = null;
        SEmployee _employeeServices = null;
        private int currentFiscalYearId = RiddhaSession.FYId;
        public KajApiController()
        {
            _kajServices = new SKaj();
            _loc = new LocalizedString();
            _employeeServices = new SEmployee();
        }
        public ServiceResult<KajModel> Get(int id)
        {
            string lang = RiddhaSession.Language;
            KajModel vm = new KajModel();
            vm.Kaj = _kajServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            vm.FromTime = vm.Kaj.From.TimeOfDay.ToString(@"hh\:mm");
            vm.ToTime = vm.Kaj.To.TimeOfDay.ToString(@"hh\:mm");
            vm.Kaj.IsApprove = vm.Kaj.IsApprove;
            var kajDetailLst = _kajServices.ListDetail().Data.Where(x => x.KajId == vm.Kaj.Id);
            if (lang == "ne")
            {
                vm.EmpLst = (from c in kajDetailLst
                             select new KajEmpVm()
                             {
                                 Id = c.EmployeeId,
                                 Name = c.Employee.Code + " - " + (!string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.NameNp : c.Employee.Name) + " - " + (c.Employee.Mobile ?? "")
                             }).ToList();
            }
            else
            {
                vm.EmpLst = (from c in kajDetailLst
                             select new KajEmpVm()
                             {
                                 Id = c.EmployeeId,
                                 Name = c.Employee.Code + " - " + c.Employee.Name + " - " + (c.Employee.Mobile ?? "")
                             }).ToList();
            }
            return new ServiceResult<KajModel>
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EmpSearchViewModel> SearchEmployee(string empCode, string empName)
        {
            EmpSearchViewModel vm = new EmpSearchViewModel();
            var empList = _employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            EEmployee employee = new EEmployee();
            if (empCode != null)
            {
                employee = empList.Where(x => x.Code == empCode).FirstOrDefault();
            }
            else if (empName != null)
            {
                employee = empList.Where(x => x.Name.ToUpper().Contains(empName.ToUpper())).FirstOrDefault();
            }
            if (employee != null)
            {
                vm.Id = employee.Id;
                vm.Code = employee.Code;
                vm.Name = employee.Name;
                vm.Designation = employee.Designation == null ? "" : employee.Designation.Name;
                vm.Section = employee.Section == null ? "" : employee.Section.Name;
                vm.Department = employee.Section.Department == null ? "" : employee.Section.Department.Name;
                vm.Photo = employee.ImageUrl;
            }
            return new ServiceResult<EmpSearchViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<KajModel> Post(KajModel vm)
        {
            vm.Kaj.BranchId = (int)RiddhaSession.BranchId;
            vm.Kaj.From = vm.Kaj.From.AddTicks(vm.FromTime.ToTimeSpan().Ticks);
            vm.Kaj.To = vm.Kaj.To.AddTicks(vm.ToTime.ToTimeSpan().Ticks);
            vm.Kaj.IsApprove = false;
            vm.Kaj.KajStatus = KajStatus.New;
            var result = _kajServices.Add(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8020", "7228", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Kaj.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<KajModel>()
            {
                Data = vm,
                Status = ResultStatus.Ok,
                Message = _loc.Localize("AddedSuccess")
            };
        }
        public ServiceResult<KajModel> Put(KajModel vm)
        {
            vm.Kaj.From = vm.Kaj.From.AddTicks(vm.FromTime.ToTimeSpan().Ticks);
            vm.Kaj.To = vm.Kaj.To.AddTicks(vm.ToTime.ToTimeSpan().Ticks);
            var result = _kajServices.Update(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8020", "7229", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Kaj.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<KajModel>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var kaj = _kajServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _kajServices.Remove(kaj);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8020", "7230", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPost]
        public KendoGridResult<List<KajKendoGridVm>> GetKajKendoGrid(KendoPageListArguments arg)
        {
            var branchId = RiddhaSession.BranchId;
            IQueryable<EKaj> kajQuery;
            var empQuery = Common.GetEmployees().Data;
            if (RiddhaSession.IsHeadOffice || RiddhaSession.DataVisibilityLevel == 4)
            {
                kajQuery = _kajServices.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId);
            }
            else
            {
                kajQuery = _kajServices.List().Data.Where(x => x.BranchId == branchId);
            }
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EKaj> paginatedQuery;
            switch (searchField)
            {
                case "Remark":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = kajQuery.Where(x => x.Remark.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id);
                    }
                    else
                    {
                        paginatedQuery = kajQuery.Where(x => x.Remark == searchValue.Trim()).OrderByDescending(x => x.Id);
                    }
                    break;
                default:
                    paginatedQuery = kajQuery.OrderByDescending(x => x.Id);
                    break;
            }
            var kajlist = (from c in paginatedQuery.ToList()
                           join d in _kajServices.ListDetail().Data on c.Id equals d.KajId
                           join e in empQuery on d.EmployeeId equals e.Id
                           select new KajKendoGridVm()
                           {
                               Id = c.Id,
                               From = c.From.ToString("yyyy/MM/dd"),
                               FromTime = c.From.TimeOfDay.ToString(@"hh\:mm"),
                               To = c.To.ToString("yyyy/MM/dd"),
                               ToTime = c.To.TimeOfDay.ToString(@"hh\:mm"),
                               Remark = c.Remark,
                               KajStatusName = Enum.GetName(typeof(OfficeVisitStatus), c.KajStatus),
                               IsApprove = c.IsApprove,
                               KajStatus = c.KajStatus
                           }).ToList();
            return new KendoGridResult<List<KajKendoGridVm>>()
            {
                Data = kajlist.OrderByDescending(x => x.Id).Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = kajlist.Count()
            };
        }
        [HttpGet]
        public ServiceResult<KajApprovalVm> Approve(int id)
        {
            var kaj = _kajServices.List().Data.Where(x => x.Id == id && x.KajStatus == KajStatus.New).FirstOrDefault();
            if (kaj != null)
            {
                kaj.KajStatus = KajStatus.Approve;
                kaj.ApprovedOn = System.DateTime.Now;
                kaj.IsApprove = true;
                kaj.ApprovedById = RiddhaSession.UserId;
                var result = _kajServices.Approve(kaj);
                if (result.Status == ResultStatus.Ok)
                {
                    SKaj kajServices = new SKaj();
                    var kajDetails = kajServices.ListDetail().Data.Where(x => x.KajId == kaj.Id).FirstOrDefault();
                    var emp = new SEmployee().List().Data.Where(x => x.Id == kajDetails.EmployeeId).FirstOrDefault();
                    Common.AddAuditTrail("8021", "7229", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
                    decimal kajApplyDay = (kaj.To - kaj.From).Days + 1;
                    SNotification notificationServices = new SNotification();
                    var empQuery = new SEmployee().List().Data.Where(x => x.BranchId == emp.BranchId);
                    int[] notificationTargets = Common.intToArray(emp.Id);
                    int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                    DateTime expiryDate = result.Data.ApprovedOn.Value.AddDays(notificationExpiryDays);
                    notificationServices.Add(new ENotification()
                    {
                        CompanyId = emp.Branch.CompanyId,
                        ExpiryDate = expiryDate,
                        FiscalYearId = currentFiscalYearId,
                        EffectiveDate = result.Data.ApprovedOn.ToDateTime(),
                        NotificationLevel = NotificationLevel.Employee,
                        NotificationType = NotificationType.ManualPunch,
                        PublishDate = result.Data.ApprovedOn.ToDateTime(),
                        TranDate = DateTime.Now,
                        TypeId = result.Data.Id,
                        Title = "Kaj request has been approved.",
                        Message = "This is to inform you that you're Kaj request on " + result.Data.From.ToString("yyyy/MM/dd") + " - " + result.Data.From.ToString(@"hh\:mm") + " To " + result.Data.To.ToString("yyyy/MM/dd") + " - " + result.Data.To.ToString(@"hh\:mm") + " due to the reason of " + result.Data.Remark + " has been approved by " + RiddhaSession.CurrentUser.FullName,
                    }, notificationTargets);
                }
            }
            else
            {
                return new ServiceResult<KajApprovalVm>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = "Already Approved"
                };
            }
            return new ServiceResult<KajApprovalVm>()
            {
                Data = null,
                Status = ResultStatus.Ok,
                Message = "Approve Successfuly"
            };
        }

        [HttpGet]
        public ServiceResult<KajApprovalVm> Reject(int id)
        {
            var kaj = _kajServices.List().Data.Where(x => x.Id == id && x.KajStatus == KajStatus.New).FirstOrDefault();
            if (kaj != null)
            {
                kaj.KajStatus = KajStatus.Reject;
                kaj.IsApprove = false;
                var result = _kajServices.Reject(kaj);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("8021", "7229", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
                }
            }
            else
            {
                return new ServiceResult<KajApprovalVm>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = "Already Reject"
                };
            }
            return new ServiceResult<KajApprovalVm>()
            {
                Data = null,
                Status = ResultStatus.Ok,
                Message = "Reject Successfuly"
            };
        }
    }
    public class KajKendoGridVm
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Remark { get; set; }
        public string KajStatusName { get; set; }
        public KajStatus KajStatus { get; set; }
        public bool IsApprove { get; set; }
    }

    public class KajApprovalVm
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int? BranchId { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApprove { get; set; }
        public KajStatus KajStatus { get; set; }
    }
}
