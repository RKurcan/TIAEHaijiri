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
    public class ManualPunchRequestApiController : ApiController
    {
        private LocalizedString _loc = null;
        private int branchId = (int)RiddhaSession.BranchId;
        private SManualPunchRequest _manualPunchRequestServices = null;
        private int currentFiscalYearId = RiddhaSession.FYId;
        public ManualPunchRequestApiController()
        {
            _loc = new LocalizedString();
        }

        [HttpGet]
        public ServiceResult<ManualPunchRequestVm> Get(int id)
        {
            _manualPunchRequestServices = new SManualPunchRequest();
            var manualPunchRequest = _manualPunchRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();

            if (manualPunchRequest != null)
            {
                ManualPunchRequestVm vm = new ManualPunchRequestVm();
                vm.BranchId = manualPunchRequest.BranchId;
                vm.EmployeeId = manualPunchRequest.EmployeeId;
                vm.Id = manualPunchRequest.Id;
                vm.Date = manualPunchRequest.DateTime.ToString("yyyy/MM/dd");
                vm.DateTime = manualPunchRequest.DateTime;
                vm.ApproveBy = manualPunchRequest.ApproveBy;
                vm.IsApproved = manualPunchRequest.IsApproved;
                vm.ApproveDate = manualPunchRequest.ApproveDate.HasValue ? manualPunchRequest.ApproveDate.Value.ToString() : string.Empty;
                vm.Department = manualPunchRequest.Employee.Section.Department.Name;
                vm.Designation = manualPunchRequest.Employee.Designation.Name;
                vm.Section = manualPunchRequest.Employee.Section.Name;
                vm.SystemDate = manualPunchRequest.SystemDate;
                vm.Time = manualPunchRequest.DateTime.TimeOfDay.ToString(@"hh\:mm");
                vm.EmployeeCode = manualPunchRequest.Employee.Code;
                vm.EmployeeName = manualPunchRequest.Employee.Name;
                vm.Remark = manualPunchRequest.Remark;
                vm.Altitude = manualPunchRequest.Altitude;
                vm.Image = manualPunchRequest.Image;
                vm.Latitude = manualPunchRequest.Latitude;
                vm.Longitude = manualPunchRequest.Latitude;
                vm.AdminRemark = manualPunchRequest.AdminRemark;
                return new ServiceResult<ManualPunchRequestVm>()
                {
                    Data = vm,
                    Message = "",
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<ManualPunchRequestVm>()
            {
                Data = null,
                Message = "Process error.",
                Status = ResultStatus.processError,
            };
        }

        [HttpPost]
        public KendoGridResult<List<ManualPunchRequestVm>> GetKendoGrid(KendoPageListArguments arg)
        {
            string language = RiddhaSession.Language.ToString();
            SManualPunchRequest service = new SManualPunchRequest();
            IQueryable<EManualPunchRequest> manualPunchRequestQuery;
            manualPunchRequestQuery = service.List().Data.Where(x => x.BranchId == branchId);
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EManualPunchRequest> paginatedQuery;
            var user = new SUser().List().Data.Where(x => x.BranchId == branchId);
            switch (searchField)
            {
                case "EmployeeCode":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = manualPunchRequestQuery.Where(x => x.Employee.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    else
                    {
                        paginatedQuery = manualPunchRequestQuery.Where(x => x.Employee.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    break;
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = manualPunchRequestQuery.Where(x => x.Employee.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    else
                    {
                        paginatedQuery = manualPunchRequestQuery.Where(x => x.Employee.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    break;
                default:
                    paginatedQuery = manualPunchRequestQuery.OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    break;
            }
            var emp = Common.GetEmployees().Data;
            var list = (from c in paginatedQuery.ToList()
                        join d in user on c.ApproveBy equals d.Id into joinedT
                        join e in emp on c.EmployeeId equals e.Id
                        from pd in joinedT.DefaultIfEmpty()
                        select new ManualPunchRequestVm()
                        {
                            BranchId = c.BranchId,
                            EmployeeId = c.EmployeeId,
                            Id = c.Id,
                            Date = c.DateTime.ToString("yyyy/MM/dd"),
                            DateTime = c.DateTime,
                            ApproveBy = c.ApproveBy,
                            ApproveByName = pd == null ? "" : pd.FullName,
                            IsApproved = c.IsApproved,
                            ApproveDate = c.ApproveDate.HasValue ? c.ApproveDate.Value.ToString() : string.Empty,
                            Department = c.Employee.Section.Department.Name,
                            Designation = c.Employee.Designation.Name,
                            Section = c.Employee.Section.Name,
                            SystemDate = c.SystemDate,
                            Time = c.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                            EmployeeCode = c.Employee.Code,
                            EmployeeName = c.Employee.Name,
                            Remark = c.Remark,
                            Altitude = c.Altitude,
                            // Image = c.Image,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                        }).ToList();
            return new KendoGridResult<List<ManualPunchRequestVm>>()
            {
                Data = list.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = list.Count()
            };
        }

        [HttpPost]
        public ServiceResult<EManualPunchRequest> Post(ManualPunchRequestVm vm)
        {
            EManualPunchRequest model = new EManualPunchRequest()
            {
                BranchId = branchId,
                DateTime = DateTime.Parse(vm.Date).AddTicks(vm.Time.ToTimeSpan().Ticks),
                EmployeeId = vm.EmployeeId,
                Remark = vm.Remark,
                SystemDate = DateTime.Now,
                AdminRemark = vm.AdminRemark,
                Image = vm.Image,
                Altitude = vm.Altitude,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
            };
            var result = new SManualPunchRequest().Add(model);
            return new ServiceResult<EManualPunchRequest>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            _manualPunchRequestServices = new SManualPunchRequest();
            var manualPunchRequest = _manualPunchRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _manualPunchRequestServices.Remove(manualPunchRequest);
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
            SManualPunch manualPunchServices = new SManualPunch();
            _manualPunchRequestServices = new SManualPunchRequest();
            var manualPunchRequest = _manualPunchRequestServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var emp = new SEmployee().List().Data.Where(x => x.Id == manualPunchRequest.EmployeeId).FirstOrDefault();
            manualPunchRequest.IsApproved = true;
            manualPunchRequest.ApproveBy = RiddhaSession.UserId;
            manualPunchRequest.ApproveDate = DateTime.Now;
            manualPunchRequest.AdminRemark = adminRemark;
            var result = _manualPunchRequestServices.Update(manualPunchRequest);
            if (result.Status == ResultStatus.Ok)
            {
                //Common.AddAuditTrail("1005", "1021", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
                EManualPunch punchModel = new EManualPunch();
                punchModel.EmployeeId = manualPunchRequest.EmployeeId;
                punchModel.BranchId = manualPunchRequest.BranchId;
                punchModel.DateTime = manualPunchRequest.DateTime;
                punchModel.Remark = manualPunchRequest.Remark;
                var manualPunchResult = manualPunchServices.Add(punchModel);
                if (manualPunchResult.Status == ResultStatus.Ok)
                {
                    decimal manualPunchApplyDay = (manualPunchResult.Data.DateTime - manualPunchResult.Data.DateTime).Days + 1;
                    SNotification notificationServices = new SNotification();
                    int[] notificationTargets = Common.intToArray(emp.Id);
                    int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                    DateTime expiryDate = result.Data.ApproveDate.Value.AddDays(notificationExpiryDays);
                    notificationServices.Add(new ENotification()
                    {
                        CompanyId = emp.Branch.CompanyId,
                        EffectiveDate = manualPunchRequest.ApproveDate.ToDateTime(),
                        ExpiryDate = expiryDate,
                        FiscalYearId = currentFiscalYearId,
                        NotificationLevel = NotificationLevel.Employee,
                        NotificationType = NotificationType.ManualPunch,
                        PublishDate = manualPunchRequest.ApproveDate.ToDateTime(),
                        TranDate = DateTime.Now,
                        TypeId = result.Data.Id,
                        Title = "Manual punch request has been approved.",
                        Message = "This is to inform you that you're Manual punch on " + result.Data.DateTime.ToString("yyyy/MM/dd") + " - " + result.Data.DateTime.ToString(@"hh\:mm") + " due to the reason of " + result.Data.Remark + " has been approved by " + RiddhaSession.CurrentUser.FullName,
                    }, notificationTargets);
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Status = result.Status,
                        Message = "Approved Successfully."
                    };
                }
            }
            return new ServiceResult<int>()
            {
                Data = 0,
                Status = ResultStatus.processError,
                Message = "Process error."
            };
        }

    }
    public class ManualPunchRequestVm
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public string Date { get; set; }
        public DateTime DateTime { get; set; }
        public string Time { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public int BranchId { get; set; }
        public DateTime SystemDate { get; set; }
        public int? ApproveBy { get; set; }
        public string ApproveByName { get; set; }
        public string ApproveDate { get; set; }
        public string Image { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Altitude { get; set; }
        public bool IsApproved { get; set; }
        public string AdminRemark { get; set; }
    }
}
