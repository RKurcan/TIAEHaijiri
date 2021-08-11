using Riddhasoft.Attendance.Services;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class EmployeeLateInAndEarlyOutRequestApiController : ApiController
    {
        private int branchId = (int)RiddhaSession.BranchId;
        private LocalizedString _loc = null;
        private int currentFiscalYearId = RiddhaSession.FYId;
        public EmployeeLateInAndEarlyOutRequestApiController()
        {
            _loc = new LocalizedString();
        }
        [HttpPost]
        public KendoGridResult<List<EmployeeLateInAndEarlyOutVm>> GetKendoGrid(KendoPageListArguments arg)
        {
            SEmployeeLateInAndEarlyOutRequest service = new SEmployeeLateInAndEarlyOutRequest();
            IQueryable<EEmployeeLateInAndEarlyOutRequest> employeeLateInAndEarlyOutRequestQuery;
            employeeLateInAndEarlyOutRequestQuery = service.List().Data.Where(x => x.Employee.BranchId == branchId);
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EEmployeeLateInAndEarlyOutRequest> paginatedQuery;
            var user = new SUser().List().Data.Where(x => x.BranchId == branchId);
            switch (searchField)
            {
                case "EmployeeCode":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = employeeLateInAndEarlyOutRequestQuery.Where(x => x.Employee.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    else
                    {
                        paginatedQuery = employeeLateInAndEarlyOutRequestQuery.Where(x => x.Employee.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    break;
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = employeeLateInAndEarlyOutRequestQuery.Where(x => x.Employee.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    else
                    {
                        paginatedQuery = employeeLateInAndEarlyOutRequestQuery.Where(x => x.Employee.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    }
                    break;
                default:
                    paginatedQuery = employeeLateInAndEarlyOutRequestQuery.OrderByDescending(x => x.Id).ThenBy(x => x.EmployeeId);
                    break;
            }
            var list = (from c in paginatedQuery.ToList()
                        join d in user on c.ApproveById equals d.Id into joinedT
                        from pd in joinedT.DefaultIfEmpty()
                        select new EmployeeLateInAndEarlyOutVm()
                        {
                            EmployeeId = c.EmployeeId,
                            Id = c.Id,
                            ApproveById = c.ApproveById,
                            ApproveByName = pd == null ? "" : pd.FullName,
                            IsApproved = c.IsApproved == true ? "YES" : "NO",
                            RequestDate = c.RequestedDate.ToString("yyyy/MM/dd"),
                            ApproveDate = c.ApproveDate.HasValue ? c.ApproveDate.Value.ToString("yyyy/MM/dd") : string.Empty,
                            DepartmentName = c.Employee.Section.Department.Name,
                            SystemDate = c.SystemDate,
                            EmployeeCode = c.Employee.Code,
                            EmployeeName = c.Employee.Name,
                            Remark = c.Remark,
                            LateInEarlyOutRequestType = c.LateInEarlyOutRequestType,
                            LateInEarlyOutRequestTypeName = Enum.GetName(typeof(LateInEarlyOutRequestType), c.LateInEarlyOutRequestType)
                        }).ToList();
            return new KendoGridResult<List<EmployeeLateInAndEarlyOutVm>>()
            {
                Data = list.Skip(arg.Skip).Take(arg.Take).ToList(),
                Message = "",
                Status = ResultStatus.Ok,
                TotalCount = list.Count(),
            };
        }

        [HttpGet]
        public ServiceResult<EmployeeLateInAndEarlyOutVm> Get(int id)
        {
            var data = new SEmployeeLateInAndEarlyOutRequest().List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (data != null)
            {
                var approvedUser = new SUser().List().Data.Where(x => x.Id == data.ApproveById).FirstOrDefault();
                EmployeeLateInAndEarlyOutVm vm = new EmployeeLateInAndEarlyOutVm();
                vm.ApproveById = data.ApproveById;
                vm.ApproveByName = approvedUser == null ? "" : approvedUser.FullName;
                vm.ApproveDate = data.ApproveDate.HasValue ? data.ApproveDate.Value.ToString("yyyy/MM/dd") : string.Empty;
                vm.DepartmentName = data.Employee.Section.Department.Name;
                vm.EmployeeCode = data.Employee.Code;
                vm.EmployeeName = data.Employee.Name;
                vm.EmployeePhoto = data.Employee.ImageUrl;
                vm.EmployeeDesignation = data.Employee.Designation.Name;
                vm.Id = data.Id;
                vm.IsApproved = data.IsApproved == true ? "YES" : "NO";
                vm.LateInEarlyOutRequestTypeName = Enum.GetName(typeof(LateInEarlyOutRequestType), data.LateInEarlyOutRequestType);
                vm.Remark = data.Remark;
                vm.RequestDate = data.RequestedDate.ToString("yyyy/MM/dd");
                vm.WorkTime = getWorkTime(data);
                return new ServiceResult<EmployeeLateInAndEarlyOutVm>()
                {
                    Data = vm,
                    Message = "",
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<EmployeeLateInAndEarlyOutVm>()
            {
                Data = null,
                Status = ResultStatus.processError,
                Message = "Process error."
            };
        }

        private string getWorkTime(EEmployeeLateInAndEarlyOutRequest data)
        {
            if (data.IsApproved)
            {
                var savedLog = new SEmployeeLateInAndEarlyOutRequest().List().Data.Where(x => x.Id == data.Id).FirstOrDefault();
                if (LateInEarlyOutRequestType.LateIn == data.LateInEarlyOutRequestType)
                {
                    return "Planned Time In : " + savedLog.PlannedInTime + " & Late In :" + savedLog.LateInTime;
                }
                else
                {
                    return "Planned Time Out : " + savedLog.PlannedOutTime + " & Early Out :" + savedLog.EarlyOutTime;
                }
            }
            SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(RiddhaSession.BranchId.ToInt(), RiddhaSession.FYId, RiddhaSession.Language);
            var attendance = reportService.GetAttendanceReportFromSp(data.RequestedDate).Data.Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault();
            if (LateInEarlyOutRequestType.LateIn == data.LateInEarlyOutRequestType)
            {
                return "Planned Time In : " + attendance.PlannedTimeIn + " & Late In :" + attendance.LateIn;
            }
            else
            {
                return "Planned Time Out : " + attendance.PlannedTimeOut + " & Early Out :" + attendance.EarlyOut;
            }
        }

        //private string getLateIn(int employeeId, DateTime requestedDate, LateInEarlyOutRequestType lateInEarlyOutRequestType, bool isApproved)
        //{
        //    if (isApproved)
        //    {

        //    }
        //    else
        //    {
        //        SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(RiddhaSession.BranchId.ToInt(), RiddhaSession.FYId, RiddhaSession.Language);
        //        var attendance = reportService.GetAttendanceReportFromSp(requestedDate).Data.Where(x => x.EmployeeId == employeeId).FirstOrDefault();
        //        if (LateInEarlyOutRequestType.LateIn == lateInEarlyOutRequestType)
        //        {
        //            return "Planned Time In : " + attendance.PlannedTimeIn + " & Late In :" + attendance.LateIn;
        //        }
        //        else
        //        {
        //            return "Planned Time Out : " + attendance.PlannedTimeOut + " & Early Out :" + attendance.EarlyOut;
        //        }
        //    }
        //    return "";

        //}

        [HttpGet]
        public ServiceResult<bool> Approve(int id)
        {
            SEmployeeLateInAndEarlyOutRequest services = new SEmployeeLateInAndEarlyOutRequest();
            var data = services.List().Data.Where(x => x.Id == id).FirstOrDefault();
            if (data.IsApproved)
            {
                return new ServiceResult<bool>()
                {
                    Data = false,
                    Message = "Already Approved.",
                    Status = ResultStatus.Ok
                };
            }
            if (data != null)
            {
                SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(RiddhaSession.BranchId.ToInt(), RiddhaSession.FYId, RiddhaSession.Language);
                var attendance = reportService.GetAttendanceReportFromSp(data.RequestedDate).Data.Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault();
                if (attendance != null)
                {
                    if (data.LateInEarlyOutRequestType == LateInEarlyOutRequestType.LateIn)
                    {
                        data.PlannedInTime = attendance.PlannedTimeIn.ToTimeSpan();
                        data.ActualInTime = attendance.ActualTimeIn.ToTimeSpan();
                        data.LateInTime = attendance.LateIn.ToTimeSpan();
                    }
                    else
                    {
                        data.PlannedOutTime = attendance.PlannedTimeOut.ToTimeSpan();
                        data.ActualOutTime = attendance.ActualTimeOut.ToTimeSpan();
                        data.EarlyOutTime = attendance.EarlyOut.ToTimeSpan();
                    }
                }
                data.ApproveById = RiddhaSession.UserId;
                data.ApproveDate = DateTime.Now;
                data.IsApproved = true;
                var result = services.Update(data);
                if (result.Status == ResultStatus.Ok)
                {
                    SAttendanceLog attendanceLogServices = new SAttendanceLog();
                    bool status = false;
                    if (result.Data.LateInEarlyOutRequestType == LateInEarlyOutRequestType.LateIn)
                    {
                        var lateInAttendanceLog = attendanceLogServices.List().Data.Where(x => x.EmployeeId == data.EmployeeId && DbFunctions.TruncateTime(x.DateTime) == DbFunctions.TruncateTime(data.RequestedDate)).ToList().OrderByDescending(x => x.DateTime).LastOrDefault();
                        if (lateInAttendanceLog != null)
                        {
                            string date = lateInAttendanceLog.DateTime.ToString("yyyy/MM/dd") + " " + attendance.PlannedTimeIn;
                            DateTime attendanceDate = DateTime.Parse(date);
                            var log = attendanceLogServices.List().Data.Where(x => x.Id == lateInAttendanceLog.Id).FirstOrDefault();
                            log.DateTime = attendanceDate;
                            var logResult = attendanceLogServices.Update(log);
                            if (logResult.Status == ResultStatus.Ok)
                            {
                                status = true;
                            }
                        }
                    }
                    else
                    {
                        var earlyOutAttendanceLog = attendanceLogServices.List().Data.Where(x => x.EmployeeId == data.EmployeeId && DbFunctions.TruncateTime(x.DateTime) == DbFunctions.TruncateTime(data.RequestedDate)).ToList().OrderByDescending(x => x.DateTime).FirstOrDefault();
                        if (earlyOutAttendanceLog != null)
                        {
                            string date = earlyOutAttendanceLog.DateTime.ToString("yyyy/MM/dd") + " " + attendance.PlannedTimeOut;
                            DateTime attendanceDate = DateTime.Parse(date);
                            var log = attendanceLogServices.List().Data.Where(x => x.Id == earlyOutAttendanceLog.Id).FirstOrDefault();
                            log.DateTime = attendanceDate;
                            var logResult = attendanceLogServices.Update(log);
                            if (logResult.Status == ResultStatus.Ok)
                            {
                                status = true;
                            }
                        }
                    }
                    if (status == true)
                    {

                        var emp = new SEmployee().List().Data.Where(x => x.Id == data.EmployeeId).FirstOrDefault();
                        //decimal manualPunchApplyDay = (manualPunchResult.Data.DateTime - manualPunchResult.Data.DateTime).Days + 1;
                        SNotification notificationServices = new SNotification();
                        int[] notificationTargets = Common.intToArray(emp.Id);
                        int notificationExpiryDays = WebConfigurationManager.AppSettings["NotificationDays"].ToInt();
                        DateTime expiryDate = result.Data.ApproveDate.Value.AddDays(notificationExpiryDays);
                        string requestType = data.LateInEarlyOutRequestType == LateInEarlyOutRequestType.LateIn ? "Late In" : "Early Out";
                        notificationServices.Add(new ENotification()
                        {
                            CompanyId = emp.Branch.CompanyId,
                            EffectiveDate = data.ApproveDate.ToDateTime(),
                            ExpiryDate = expiryDate,
                            FiscalYearId = currentFiscalYearId,
                            NotificationLevel = NotificationLevel.Employee,
                            NotificationType = NotificationType.LateInEarlyOut,
                            PublishDate = data.ApproveDate.ToDateTime(),
                            TranDate = DateTime.Now,
                            TypeId = result.Data.Id,
                            Title = requestType + " request has been approved.",
                            Message = "This is to inform you that you're " + requestType + " request on " + data.RequestedDate.ToString("yyyy/MM/dd") + " due to the reason of " + result.Data.Remark + " has been approved by " + RiddhaSession.CurrentUser.FullName,
                        }, notificationTargets);
                    }
                }
                return new ServiceResult<bool>()
                {
                    Data = true,
                    Message = "Approvd Sucessfully.",
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<bool>()
            {
                Data = false,
                Message = "Process error.",
                Status = ResultStatus.processError,
            };
        }


        [HttpGet]
        public ServiceResult<EmployeeLateInAndEarlyOutVm> GetPunchTimeforLateInAccordingtoDateTimeAndEmpId(string dateTime, int empId, int requestType)
        {
            DateTime dateToday = DateTime.Now;
            //if (date < dateToday)
            DateTime date = Convert.ToDateTime(dateTime);
            if (date != null)
            {
                var emp = new SEmployee().List().Data.Where(x => x.Id == empId).FirstOrDefault();
                var fiscalYear = new SFiscalYear().List().Data.Where(x => x.BranchId == emp.BranchId && x.CurrentFiscalYear).FirstOrDefault();
                if (fiscalYear == null)
                {
                    return new ServiceResult<EmployeeLateInAndEarlyOutVm>()
                    {
                        Data = null,
                        Message = "Current fiscal year is not set. Please request you're admin to set current fiscal year.",
                        Status = ResultStatus.processError,
                    };
                }
                SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(emp.BranchId.ToInt(), RiddhaSession.FYId, RiddhaSession.Language);

                var attendance = reportService.GetAttendanceReportFromSp(date).Data.Where(x => x.EmployeeId == emp.Id).FirstOrDefault();
                //Check that day his first punch was alright
                if (attendance.Holiday == "Yes")
                {
                    return new ServiceResult<EmployeeLateInAndEarlyOutVm>()
                    {
                        Data = null,
                        Message = "There is a holiday on this date.",
                        Status = ResultStatus.processError,
                    };
                }
                if (attendance.OnLeave == "Yes")
                {
                    return new ServiceResult<EmployeeLateInAndEarlyOutVm>()
                    {
                        Data = null,
                        Message = "You're on leave on this date.",
                        Status = ResultStatus.processError,
                    };
                }
                if (attendance.Weekend == "Yes")
                {
                    return new ServiceResult<EmployeeLateInAndEarlyOutVm>()
                    {
                        Data = null,
                        Message = "There is a weekend on this date.",
                        Status = ResultStatus.processError,
                    };
                }
                if (requestType == 0)
                {
                    bool validateLateIn = false;
                    if (!string.IsNullOrEmpty(attendance.LateIn))
                    {
                        validateLateIn = true;
                    }
                    if (validateLateIn) /*if not alright return punch Time of that day*/
                    {
                        EmployeeLateInAndEarlyOutVm model = new EmployeeLateInAndEarlyOutVm();
                        model.PunchTime = attendance.ActualTimeIn;
                        model.PlannedInTime = attendance.PlannedTimeIn;
                        return new ServiceResult<EmployeeLateInAndEarlyOutVm>
                        {
                            Data = model,
                            Message = "",
                            Status = ResultStatus.Ok
                        };
                    }
                    else
                    {
                        return new ServiceResult<EmployeeLateInAndEarlyOutVm>
                        {
                            Data = null,
                            Message = "You are not latein on selected date.",
                            Status = ResultStatus.processError
                        };
                    }

                }
                else
                {
                    bool validateLateOut = false;
                    if (!string.IsNullOrEmpty(attendance.EarlyOut))
                    {
                        validateLateOut = true;
                    }
                    if (validateLateOut) /*if not alright return punch Time of that day*/
                    {
                        EmployeeLateInAndEarlyOutVm model = new EmployeeLateInAndEarlyOutVm();
                        model.ActualOutTime = attendance.ActualTimeOut;
                        model.PlannedOutTime = attendance.PlannedTimeOut;
                        return new ServiceResult<EmployeeLateInAndEarlyOutVm>
                        {
                            Data = model,
                            Message = "",
                            Status = ResultStatus.Ok
                        };
                    }
                    else
                    {
                        return new ServiceResult<EmployeeLateInAndEarlyOutVm>
                        {
                            Data = null,
                            Message = "You are not early out on selected date.",
                            Status = ResultStatus.processError
                        };
                    }
                }


            }
            else
            {

                return new ServiceResult<EmployeeLateInAndEarlyOutVm>
                {
                    Data = null,
                    Message = "Choosen date cannot be greater than today.",
                    Status = ResultStatus.processError
                };
            }


        }
        [HttpPost]
        public ServiceResult<EEmployeeLateInAndEarlyOutRequest> Post(EmployeeLateInAndEarlyOutVm vm)
        {
            SEmployeeLateInAndEarlyOutRequest service = new SEmployeeLateInAndEarlyOutRequest();
            EEmployeeLateInAndEarlyOutRequest model = new EEmployeeLateInAndEarlyOutRequest()
            {
                ActualOutTime = vm.ActualOutTime.ToTimeSpan(),
                RequestedDate = Convert.ToDateTime(vm.RequestDate),
                SystemDate=DateTime.Now,
                ActualInTime=vm.ActualInTime.ToTimeSpan(),
                EmployeeId=vm.EmployeeId,
                EarlyOutTime=vm.ActualOutTime.ToTimeSpan(),
                LateInEarlyOutRequestType=vm.LateInEarlyOutRequestType,
              




            };
            var result = service.Add(model);
            return new ServiceResult<EEmployeeLateInAndEarlyOutRequest>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
    }

    public class EmployeeLateInAndEarlyOutVm
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhoto { get; set; }
        public string EmployeeDesignation { get; set; }
        public string DepartmentName { get; set; }
        public string RequestDate { get; set; }
        public string Remark { get; set; }
        public int? ApproveById { get; set; }
        public string ApproveByName { get; set; }
        public string ApproveDate { get; set; }
        public DateTime SystemDate { get; set; }
        public string IsApproved { get; set; }
        public string ActualInTime { get; set; }
        public string PunchTime { get; set; }
        public string ActualOutTime { get; set; }
        public string PlannedInTime { get; set; }
        public string PlannedOutTime { get; set; }
        public string WorkTime { get; set; }
        public LateInEarlyOutRequestType LateInEarlyOutRequestType { get; set; }
        public string LateInEarlyOutRequestTypeName { get; set; }
    }
}
