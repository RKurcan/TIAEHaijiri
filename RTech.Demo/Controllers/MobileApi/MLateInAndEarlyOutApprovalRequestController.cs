using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTech.Demo.Controllers.MobileApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MLateInAndEarlyOutApprovalRequestController : MRootController
    {
        public MLateInAndEarlyOutApprovalRequestController()
        {

        }

        #region Early Out

        [HttpPost]
        public MobileResult<EMLateInAndEarlyOutApprovalRequest> RequestEarlyOutApproval(EMLateInAndEarlyOutApprovalRequest vm)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                EEmployeeLateInAndEarlyOutRequest model = new EEmployeeLateInAndEarlyOutRequest();
                model.ApproveById = null;
                model.ApproveDate = null;
                model.EmployeeId = vm.EmployeeId;
                model.IsApproved = false;
                model.LateInEarlyOutRequestType = LateInEarlyOutRequestType.EarlyOut;
                model.Remark = vm.Reason;
                model.RequestedDate = vm.Date;
                model.SystemDate = DateTime.Now;
                SEmployeeLateInAndEarlyOutRequest employeeLateInAndEarlyOutRequestServices = new SEmployeeLateInAndEarlyOutRequest();
                employeeLateInAndEarlyOutRequestServices.Add(model);
                return new MobileResult<EMLateInAndEarlyOutApprovalRequest>()
                {
                    Data = vm,
                    Status = MobileResultStatus.Ok,
                    Message = "Thank you. Early out request is successfully sent. We will notify you soon after your request is approved."
                };
            }
            return InvalidTokeResult<EMLateInAndEarlyOutApprovalRequest>();
        }


        [HttpGet]
        public MobileResult<EMPucnchTimeInfo> GetPunchTimeforEarlyOutAccordingtoDateTimeAndEmpId(DateTime date, int empId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                DateTime dateToday = DateTime.Now;
                //if (date < dateToday)
                if (date != null)
                {
                    var emp = new SEmployee().List().Data.Where(x => x.Id == empId).FirstOrDefault();
                    var fiscalYear = new SFiscalYear().List().Data.Where(x => x.CompanyId == emp.Branch.CompanyId && x.CurrentFiscalYear).FirstOrDefault();
                    if (fiscalYear == null)
                    {
                        return new MobileResult<EMPucnchTimeInfo>()
                        {
                            Data = null,
                            Message = "Current fiscal year is not set. Please request you're admin to set current fiscal year.",
                            Status = MobileResultStatus.ProcessError,
                        };
                    }
                    SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(emp.BranchId.ToInt(), RiddhaSession.FYId, RiddhaSession.Language);
                    var attendance = reportService.GetAttendanceReportFromSp(date).Data.Where(x => x.EmployeeId == emp.Id).FirstOrDefault();
                    //Check that day his first punch was alright
                    if (attendance.Holiday == "Yes")
                    {
                        return new MobileResult<EMPucnchTimeInfo>()
                        {
                            Data = null,
                            Message = "There is a holiday on this date.",
                            Status = MobileResultStatus.ProcessError,
                        };
                    }
                    if (attendance.OnLeave == "Yes")
                    {
                        return new MobileResult<EMPucnchTimeInfo>()
                        {
                            Data = null,
                            Message = "You're on leave on this date.",
                            Status = MobileResultStatus.ProcessError,
                        };
                    }
                    if (attendance.Weekend == "Yes")
                    {
                        return new MobileResult<EMPucnchTimeInfo>()
                        {
                            Data = null,
                            Message = "There is a weekend on this date.",
                            Status = MobileResultStatus.ProcessError,
                        };
                    }
                    bool validateEarlyOut = false;
                    if (!string.IsNullOrEmpty(attendance.EarlyOut))
                    {
                        validateEarlyOut = true;
                    }
                    if (validateEarlyOut) /*if not alright return punch Time of that day*/
                    {
                        EMPucnchTimeInfo model = new EMPucnchTimeInfo();
                        model.PunchTime = "TimeOut: " + attendance.ActualTimeOut + "  Planned: " + attendance.PlannedTimeOut; 
                        return new MobileResult<EMPucnchTimeInfo>
                        {
                            Data = model,
                            Message = "",
                            Status = MobileResultStatus.Ok
                        };
                    }
                    else
                    {
                        return new MobileResult<EMPucnchTimeInfo>
                        {
                            Data = null,
                            Message = "You are not early out on selected date.",
                            Status = MobileResultStatus.ProcessError
                        };
                    }

                }
                else
                {

                    return new MobileResult<EMPucnchTimeInfo>
                    {
                        Data = null,
                        Message = "Choosen date cannot be greater than today.",
                        Status = MobileResultStatus.ProcessError
                    };
                }
            }
            return InvalidTokeResult<EMPucnchTimeInfo>();
        }
        #endregion

        #region Late In

        [HttpPost]
        public MobileResult<EMLateInAndEarlyOutApprovalRequest> RequestLateInApproval(EMLateInAndEarlyOutApprovalRequest vm)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                EEmployeeLateInAndEarlyOutRequest model = new EEmployeeLateInAndEarlyOutRequest();
                model.ApproveById = null;
                model.ApproveDate = null;
                model.EmployeeId = vm.EmployeeId;
                model.IsApproved = false;
                model.LateInEarlyOutRequestType = LateInEarlyOutRequestType.LateIn;
                model.Remark = vm.Reason;
                model.RequestedDate = vm.Date;
                model.SystemDate = DateTime.Now;
                SEmployeeLateInAndEarlyOutRequest employeeLateInAndEarlyOutRequestServices = new SEmployeeLateInAndEarlyOutRequest();
                employeeLateInAndEarlyOutRequestServices.Add(model);
                return new MobileResult<EMLateInAndEarlyOutApprovalRequest>()
                {
                    Data = vm,
                    Status = MobileResultStatus.Ok,
                    Message = "Thank you. Late in request is successfully sent. We will notify you soon after your request is approved."
                };
            }
            return InvalidTokeResult<EMLateInAndEarlyOutApprovalRequest>();
        }
        [HttpGet]
        public MobileResult<EMPucnchTimeInfo> GetPunchTimeforLateInAccordingtoDateTimeAndEmpId(DateTime date, int empId)
        {
            string token = Common.RequestToken;
            if (Common.ValidateToken(token))
            {
                DateTime dateToday = DateTime.Now;
                //if (date < dateToday)
                if (date != null)
                {
                    var emp = new SEmployee().List().Data.Where(x => x.Id == empId).FirstOrDefault();
                    var fiscalYear = new SFiscalYear().List().Data.Where(x => x.BranchId == emp.BranchId && x.CurrentFiscalYear).FirstOrDefault();
                    if (fiscalYear == null)
                    {
                        return new MobileResult<EMPucnchTimeInfo>()
                        {
                            Data = null,
                            Message = "Current fiscal year is not set. Please request you're admin to set current fiscal year.",
                            Status = MobileResultStatus.ProcessError,
                        };
                    }
                    SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(emp.BranchId.ToInt(), RiddhaSession.FYId, RiddhaSession.Language);
                    var attendance = reportService.GetAttendanceReportFromSp(date).Data.Where(x => x.EmployeeId == emp.Id).FirstOrDefault();
                    //Check that day his first punch was alright
                    if (attendance.Holiday == "Yes")
                    {
                        return new MobileResult<EMPucnchTimeInfo>()
                        {
                            Data = null,
                            Message = "There is a holiday on this date.",
                            Status = MobileResultStatus.ProcessError,
                        };
                    }
                    if (attendance.OnLeave == "Yes")
                    {
                        return new MobileResult<EMPucnchTimeInfo>()
                        {
                            Data = null,
                            Message = "You're on leave on this date.",
                            Status = MobileResultStatus.ProcessError,
                        };
                    }
                    if (attendance.Weekend == "Yes")
                    {
                        return new MobileResult<EMPucnchTimeInfo>()
                        {
                            Data = null,
                            Message = "There is a weekend on this date.",
                            Status = MobileResultStatus.ProcessError,
                        };
                    }

                    bool validateLateIn = false;
                    if (!string.IsNullOrEmpty(attendance.LateIn))
                    {
                        validateLateIn = true;
                    }
                    if (validateLateIn) /*if not alright return punch Time of that day*/
                    {
                        EMPucnchTimeInfo model = new EMPucnchTimeInfo();
                        model.PunchTime = attendance.ActualTimeIn;
                        return new MobileResult<EMPucnchTimeInfo>
                        {
                            Data = model,
                            Message = "",
                            Status = MobileResultStatus.Ok
                        };
                    }
                    else
                    {
                        return new MobileResult<EMPucnchTimeInfo>
                        {
                            Data = null,
                            Message = "You are not latein on selected date.",
                            Status = MobileResultStatus.ProcessError
                        };
                    }

                }
                else
                {

                    return new MobileResult<EMPucnchTimeInfo>
                    {
                        Data = null,
                        Message = "Choosen date cannot be greater than today.",
                        Status = MobileResultStatus.ProcessError
                    };
                }
            }
            return InvalidTokeResult<EMPucnchTimeInfo>();
        }
        #endregion
    }
    public class EMLateInAndEarlyOutApprovalRequest
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }

    }
    public class EMPucnchTimeInfo
    {
        public string PunchTime { get; set; }

    }
}
