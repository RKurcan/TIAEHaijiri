using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class OfficeVisitRequestApiController : ApiController
    {
        private LocalizedString _loc = null;
        private int branchId = (int)RiddhaSession.BranchId;
        private int companyId = RiddhaSession.CompanyId;
        private SOfficeVisitRequest _officeVisitRequestServices = null;
        private SOfficeVisit _officeVisitServices = null;
        private int currentFiscalYearId = RiddhaSession.FYId;
        public OfficeVisitRequestApiController()
        {
            _loc = new LocalizedString();
            _officeVisitServices = new SOfficeVisit();
            _officeVisitRequestServices = new SOfficeVisitRequest();
        }

        [HttpGet]
        public ServiceResult<OffiveVisitRequestVm> Get(int id)
        {
            _officeVisitRequestServices = new SOfficeVisitRequest();
            var officeVisitRequest = _officeVisitRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (officeVisitRequest != null)
            {
                OffiveVisitRequestVm vm = new OffiveVisitRequestVm();
                vm.BranchId = officeVisitRequest.BranchId;
                vm.EmployeeId = officeVisitRequest.EmployeeId;
                vm.Id = officeVisitRequest.Id;
                vm.FromDate = officeVisitRequest.From.ToString("yyyy/MM/dd");
                vm.FromDateAndTime = officeVisitRequest.From.ToString("yyyy/MM/dd") + "-" + officeVisitRequest.From.TimeOfDay.ToString(@"hh\:mm");
                vm.ToDate = officeVisitRequest.To.ToString("yyyy/MM/dd");
                vm.ToDateAndTime = officeVisitRequest.To.ToString("yyyy/MM/dd") + "-" + officeVisitRequest.To.TimeOfDay.ToString(@"hh\:mm");
                vm.ApprovedById = officeVisitRequest.ApprovedById;
                vm.IsApprove = officeVisitRequest.IsApprove;
                vm.ApprovedOn = officeVisitRequest.ApprovedOn.HasValue ? officeVisitRequest.ApprovedOn.Value.ToString() : string.Empty;
                vm.Department = officeVisitRequest.Employee.Section.Department.Name;
                vm.Designation = officeVisitRequest.Employee.Designation.Name;
                vm.Section = officeVisitRequest.Employee.Section.Name;
                vm.SystemDate = officeVisitRequest.SystemDate;
                vm.FromTime = officeVisitRequest.From.TimeOfDay.ToString(@"hh\:mm");
                vm.ToTime = officeVisitRequest.To.TimeOfDay.ToString(@"hh\:mm");
                vm.EmployeeCode = officeVisitRequest.Employee.Code;
                vm.EmployeeName = officeVisitRequest.Employee.Name;
                vm.Remark = officeVisitRequest.Remark;
                vm.Altitude = officeVisitRequest.Altitude;
                vm.Image = officeVisitRequest.Image;
                vm.Latitude = officeVisitRequest.Latitude;
                vm.Longitude = officeVisitRequest.Longitude;
                vm.AdminRemark = officeVisitRequest.AdminRemark;
                return new ServiceResult<OffiveVisitRequestVm>()
                {
                    Status = ResultStatus.Ok,
                    Message = "",
                    Data = vm,
                };
            }
            return new ServiceResult<OffiveVisitRequestVm>()
            {
                Data = null,
                Message = "Process error.",
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<OffiveVisitRequestVm>> GetKendoGrid(KendoPageListArguments arg)
        {
            string language = RiddhaSession.Language.ToString();
            IQueryable<EOfficeVisitRequest> officeVisitRequestQuery;
            officeVisitRequestQuery = _officeVisitRequestServices.List().Data.Where(x => x.BranchId == branchId);
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EOfficeVisitRequest> paginatedQuery;
            var user = new SUser().List().Data.Where(x => x.BranchId == branchId);
            switch (searchField)
            {
                case "EmployeeCode":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = officeVisitRequestQuery.Where(x => x.Employee.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    else
                    {
                        paginatedQuery = officeVisitRequestQuery.Where(x => x.Employee.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    break;
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = officeVisitRequestQuery.Where(x => x.Employee.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    else
                    {
                        paginatedQuery = officeVisitRequestQuery.Where(x => x.Employee.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    break;
                default:
                    paginatedQuery = officeVisitRequestQuery.OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    break;
            }
            var emp = Common.GetEmployees().Data;
            var list = (from c in paginatedQuery.ToList()
                        join d in user on c.ApprovedById equals d.Id into joinedT
                        join e in emp on c.EmployeeId equals e.Id
                        from pd in joinedT.DefaultIfEmpty()
                        select new OffiveVisitRequestVm()
                        {
                            BranchId = c.BranchId,
                            EmployeeId = c.EmployeeId,
                            Id = c.Id,
                            FromDate = c.From.ToString("yyyy/MM/dd"),
                            FromDateAndTime = c.From.ToString("yyyy/MM/dd") + "-" + c.From.TimeOfDay.ToString(@"hh\:mm"),
                            ToDate = c.To.ToString("yyyy/MM/dd"),
                            ToDateAndTime = c.To.ToString("yyyy/MM/dd") + "-" + c.To.TimeOfDay.ToString(@"hh\:mm"),
                            ApprovedById = c.ApprovedById,
                            ApproveByName = pd == null ? "" : pd.FullName,
                            IsApprove = c.IsApprove,
                            ApprovedOn = c.ApprovedOn.HasValue ? c.ApprovedOn.Value.ToString() : string.Empty,
                            Department = c.Employee.Section.Department.Name,
                            Designation = c.Employee.Designation.Name,
                            Section = c.Employee.Section.Name,
                            SystemDate = c.SystemDate,
                            FromTime = c.From.TimeOfDay.ToString(@"hh\:mm"),
                            ToTime = c.To.TimeOfDay.ToString(@"hh\:mm"),
                            EmployeeCode = c.Employee.Code,
                            EmployeeName = c.Employee.Name,
                            Remark = c.Remark,
                            Altitude = c.Altitude,
                            //Image = c.Image,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                        }).ToList();
            return new KendoGridResult<List<OffiveVisitRequestVm>>()
            {
                Data = list.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = list.Count()
            };
        }
     
        [HttpPost]
        public ServiceResult<EOfficeVisitRequest> Post(OffiveVisitRequestVm vm)
        {
            EOfficeVisitRequest model = new EOfficeVisitRequest()
            {
                BranchId = branchId,
                From = DateTime.Parse(vm.FromDate).AddTicks(vm.FromTime.ToTimeSpan().Ticks),
                EmployeeId = vm.EmployeeId,
                Remark = vm.Remark,
                SystemDate = DateTime.Now,
                Altitude = vm.Altitude,
                Branch = null,
                Employee = null,
                Image = vm.Image,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                To = DateTime.Parse(vm.ToDate).AddTicks(vm.ToTime.ToTimeSpan().Ticks),
                AdminRemark = vm.AdminRemark,

            };
            var result = _officeVisitRequestServices.Add(model);
            return new ServiceResult<EOfficeVisitRequest>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var officeVisitRequest = _officeVisitRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (officeVisitRequest.IsApprove == true)
            {
                return new ServiceResult<int>()
                {
                    Data = 0,
                    Message = "Already Approved.",
                    Status = ResultStatus.processError,
                };
            }
            var result = _officeVisitRequestServices.Remove(officeVisitRequest);
            if (result.Status == ResultStatus.Ok)
            {
                //Common.AddAuditTrail("1005", "1021", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = _loc.Localize(result.Message)
            };
        }

        [HttpGet]
        public ServiceResult<int> Approve(int id, string adminRemark)
        {
            var officeVisitRequest = _officeVisitRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (officeVisitRequest != null)
            {
                officeVisitRequest.IsApprove = true;
                officeVisitRequest.ApprovedById = RiddhaSession.UserId;
                officeVisitRequest.ApprovedOn = DateTime.Now;
                officeVisitRequest.AdminRemark = adminRemark;
                var result = _officeVisitRequestServices.Update(officeVisitRequest);
                if (result.Status == ResultStatus.Ok)
                {
                    EOfficeVisit officeVisit = new EOfficeVisit()
                    {
                        ApprovedById = result.Data.ApprovedById,
                        ApprovedOn = result.Data.ApprovedOn,
                        BranchId = result.Data.BranchId,
                        From = result.Data.From,
                        IsApprove = result.Data.IsApprove,
                        OfficeVisitStatus = OfficeVisitStatus.Approve,
                        Remark = result.Data.Remark,
                        To = result.Data.To,
                    };
                    var officeVisitResult = _officeVisitServices.Add(officeVisit);
                    if (officeVisitResult.Status == ResultStatus.Ok)
                    {
                        EOfficeVisitDetail officeVisitDetail = new EOfficeVisitDetail()
                        {
                            EmployeeId = result.Data.EmployeeId,
                            OfficeVisitId = officeVisitResult.Data.Id,
                        };
                        var officeVisitDetailResult = _officeVisitServices.AddDetail(officeVisitDetail);
                        if (officeVisitDetailResult.Status == ResultStatus.Ok)
                        {
                            SNotification notificationServices = new SNotification();
                            var empQuery = new SEmployee().List().Data.Where(x => x.BranchId == officeVisitDetailResult.Data.Employee.BranchId);
                            int[] notificationTargets = Common.intToArray(officeVisitDetailResult.Data.EmployeeId);
                            int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                            DateTime expiryDate = result.Data.ApprovedOn.Value.AddDays(notificationExpiryDays);
                            notificationServices.Add(new ENotification()
                            {
                                CompanyId = RiddhaSession.CompanyId,
                                EffectiveDate = officeVisitResult.Data.ApprovedOn.ToDateTime(),
                                ExpiryDate = expiryDate,
                                FiscalYearId = currentFiscalYearId,
                                NotificationLevel = NotificationLevel.Employee,
                                NotificationType = NotificationType.OfficeVisit,
                                PublishDate = officeVisitResult.Data.ApprovedOn.ToDateTime(),
                                TranDate = DateTime.Now,
                                TypeId = result.Data.Id,
                                Title = "Office Visit request has been approved.",
                                Message = "This is to inform you that you're office visit request on " + result.Data.From.ToString("yyyy/MM/dd") + " - " + result.Data.From.ToString(@"hh\:mm") + " To " + result.Data.To.ToString("yyyy/MM/dd") + " - " + result.Data.To.ToString(@"hh\:mm") + " due to the reason of " + result.Data.Remark + " has been approved by " + RiddhaSession.CurrentUser.FullName,
                            }, notificationTargets);
                        }
                    }
                }
            }
            return new ServiceResult<int>()
            {
                Data = 0,
                Message = "Process error.",
                Status = ResultStatus.Ok
            };
        }
    }

    public class OffiveVisitRequestVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string FromDate { get; set; }
        public string FromDateAndTime { get; set; }
        public string FromTime { get; set; }
        public string ToDate { get; set; }

        public string ToDateAndTime { get; set; }
        public string ToTime { get; set; }

        public string Remark { get; set; }
        public int BranchId { get; set; }
        public int? ApprovedById { get; set; }
        public string ApproveByName { get; set; }
        public string ApprovedOn { get; set; }
        public bool IsApprove { get; set; }
        public string Image { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Altitude { get; set; }
        public DateTime SystemDate { get; set; }
        public string AdminRemark { get;  set; }
    }
}
