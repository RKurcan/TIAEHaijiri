using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class OfficeVisitApprovalApiController : ApiController
    {
        SOfficeVisit _officeVisitServices = null;
        LocalizedString _loc = null;
        SDateTable _dateTableServices = null;
        public OfficeVisitApprovalApiController()
        {
            _officeVisitServices = new SOfficeVisit();
            _loc = new LocalizedString();
            _dateTableServices = new SDateTable();
        }
        [HttpPost]
        public KendoGridResult<List<OfficeVisitApprovalGridVm>> GetOfficeVisitApprovalKendoGrid(KendoPageListArguments vm)
        {
            int branchId = (int)RiddhaSession.BranchId;
            string opDate = RiddhaSession.OperationDate;
            List<EOfficeVisit> officeVisitQuery = new List<EOfficeVisit>();
            List<EOfficeVisitDetail> officeVisitDetailList = new List<EOfficeVisitDetail>();
            officeVisitDetailList = _officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisit.BranchId == branchId).ToList();
            if (RiddhaSession.IsHeadOffice || RiddhaSession.DataVisibilityLevel == 4)
            {
                officeVisitQuery = _officeVisitServices.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).ToList();
            }
            else
            {
                officeVisitQuery = _officeVisitServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            }
            int totalRowNum = officeVisitQuery.Count();
            SEmployee empServices = new SEmployee();
            SOfficeVisit service = new SOfficeVisit();

            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            List<EOfficeVisit> paginatedQuery;

            switch (searchField)
            {
                case "Remark":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = officeVisitQuery.Where(x => x.Remark.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.From).ToList();
                    }
                    else
                    {
                        paginatedQuery = officeVisitQuery.Where(x => x.Remark == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.From).ToList();
                    }
                    break;
                case "From":
                    DateTime fromDate;
                    if (opDate == "ne")
                    {
                        fromDate = _dateTableServices.ConvertToEngDate(searchValue);
                    }
                    else
                    {
                        fromDate = searchValue.ToDateTime();
                    }

                    if (searchOp == "startswith")
                    {
                        paginatedQuery = officeVisitQuery.Where(x => DbFunctions.TruncateTime(x.From) == DbFunctions.TruncateTime(fromDate)).OrderByDescending(x => x.Id).ThenBy(x => x.From).ToList();
                    }
                    else
                    {
                        paginatedQuery = officeVisitQuery.Where(x => DbFunctions.TruncateTime(x.From) == DbFunctions.TruncateTime(fromDate)).OrderByDescending(x => x.Id).ThenBy(x => x.From).ToList();
                    }
                    break;
                case "To":
                    DateTime toDate;
                    if (opDate == "ne")
                    {
                        toDate = _dateTableServices.ConvertToEngDate(searchValue);
                    }
                    else
                    {
                        toDate = searchValue.ToDateTime();
                    }

                    if (searchOp == "startswith")
                    {
                        paginatedQuery = officeVisitQuery.Where(x => DbFunctions.TruncateTime(x.To) == DbFunctions.TruncateTime(toDate)).OrderByDescending(x => x.Id).ThenBy(x => x.From).ToList();
                    }
                    else
                    {
                        paginatedQuery = officeVisitQuery.Where(x => DbFunctions.TruncateTime(x.To) == DbFunctions.TruncateTime(toDate)).OrderByDescending(x => x.Id).ThenBy(x => x.From).ToList();
                    }
                    break;
                default:
                    paginatedQuery = officeVisitQuery.OrderByDescending(x => x.Id).ThenBy(x => x.From).Skip(vm.Skip).Take(vm.Take).ToList();
                    break;
            }
            if (!(RiddhaSession.PackageId != 3))
            {
                ///userid=== curent user or reporting manager is==curent user
                ///
                if (RiddhaSession.RoleId != 0)
                {
                    var reportingManager = empServices.List().Data.Where(x => x.ReportingManagerId == RiddhaSession.EmployeeId || x.Id == RiddhaSession.EmployeeId).ToList();
                    if (reportingManager.Count() != 0)
                    {
                        //var filterEmpUnderManager = empServices.List().Data.Where(x => x.ReportingManagerId == RiddhaSession.UserId || x.).ToList();

                        var officeVisitDetails = (from c in service.ListDetail().Data.ToList()
                                                  join d in reportingManager on c.EmployeeId equals d.Id
                                                  select c.OfficeVisitId
                                                  ).Distinct().ToList();

                        var officeVisitMaster = (from c in service.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                                                 join d in officeVisitDetails on c.Id equals d
                                                 select c
                                                 ).ToList();

                        paginatedQuery = officeVisitMaster;
                    }
                    else
                    {
                        paginatedQuery = service.List().Data.Where(x => x.BranchId == branchId).ToList();
                    }
                }
                else
                {
                    paginatedQuery = officeVisitQuery;
                }
            }
            var officeVisitlist = (from c in paginatedQuery.ToList()
                                   select new OfficeVisitApprovalGridVm()
                                   {
                                       Id = c.Id,
                                       From = c.From.ToString("yyyy/MM/dd"),
                                       FromTime = c.From.TimeOfDay.ToString(@"hh\:mm"),
                                       To = c.To.ToString("yyyy/MM/dd"),
                                       ToTime = c.To.TimeOfDay.ToString(@"hh\:mm"),
                                       Remark = c.Remark,
                                       OfficeVisitStatusName = Enum.GetName(typeof(OfficeVisitStatus), c.OfficeVisitStatus),
                                       OfficeVisitStatus = c.OfficeVisitStatus,
                                       IsApprove = c.IsApprove,
                                       EmployeeName = String.Join(" , ", officeVisitDetailList.Where(x => x.OfficeVisitId == c.Id).Select(x => x.Employee.Name))
                                   }).ToList();
            return new KendoGridResult<List<OfficeVisitApprovalGridVm>>()
            {
                Data = officeVisitlist.OrderByDescending(x => x.From).Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = officeVisitlist.Count(),
            };
        }

        [HttpGet]
        public ServiceResult<OfficeVisitApprovalVm> Approve(int id)
        {
            var officeVisit = _officeVisitServices.List().Data.Where(x => x.Id == id && x.OfficeVisitStatus == OfficeVisitStatus.New).FirstOrDefault();
            if (officeVisit != null)
            {
                officeVisit.OfficeVisitStatus = OfficeVisitStatus.Approve;
                officeVisit.ApprovedOn = System.DateTime.Now;
                officeVisit.IsApprove = true;
                officeVisit.ApprovedById = RiddhaSession.UserId;
                var result = _officeVisitServices.Approve(officeVisit);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("8019", "7226", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
                }
            }
            else
            {
                return new ServiceResult<OfficeVisitApprovalVm>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = "Already Approved"
                };
            }
            return new ServiceResult<OfficeVisitApprovalVm>()
            {
                Data = null,
                Status = ResultStatus.Ok,
                Message = "Approve Successfuly"
            };
        }

        [HttpGet]
        public ServiceResult<OfficeVisitApprovalVm> Reject(int id)
        {
            var officeVisit = _officeVisitServices.List().Data.Where(x => x.Id == id && x.OfficeVisitStatus == OfficeVisitStatus.New).FirstOrDefault();
            if (officeVisit != null)
            {
                officeVisit.OfficeVisitStatus = OfficeVisitStatus.Reject;
                officeVisit.IsApprove = false;
                var result = _officeVisitServices.Reject(officeVisit);
            }
            else
            {
                return new ServiceResult<OfficeVisitApprovalVm>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = "Already Reject"
                };
            }
            return new ServiceResult<OfficeVisitApprovalVm>()
            {
                Data = null,
                Status = ResultStatus.Ok,
                Message = "Reject Successfuly"
            };
        }
    }
    public class OfficeVisitApprovalGridVm
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string FromTime { get; set; }
        public string To { get; set; }
        public string ToTime { get; set; }
        public string Remark { get; set; }
        public string OfficeVisitStatusName { get; set; }
        public bool IsApprove { get; set; }
        public string EmployeeName { get; set; }
        public OfficeVisitStatus OfficeVisitStatus { get; set; }
    }

    public class OfficeVisitApprovalVm
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int? BranchId { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApprove { get; set; }
        public OfficeVisitStatus OfficeVisitStatus { get; set; }
    }
}
