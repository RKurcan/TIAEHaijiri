using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class OfficeVisitApiController : ApiController
    {

        #region Mailtemplate
        /// <summary>
        /// 0-managername
        /// 1-from time
        /// 2-to time
        /// 
        /// 3-employee list
        /// 4-approveBaseUrl
        /// 5-saved office visit primary key for approve
        /// 6-managerId
        /// 7-rejectBaseUrl
        /// 8-saved office visit primary key for reject
        /// </summary>
        string OfficeVisitManagerRequestTemplate = "<div class='panel panel-success'>" +
                                                              "<div class='panel-heading'>" +
                                                                  "<h2></h2>" +
                                                              "</div>" +
                                                               "<div class='panel-body'>" +
                                                                  "<p>" +
                                                                      "Dear {0},</br>" +
                                                                      "<p>You are requested to approve office Visit  from {1} to {2}</p>" +
                                                                      "<p></p>" +
                                                                      "<p><b>Employee Information</b></p>" +
                                                                      "{3}" +
                                                                      "<p></p>" +
                                                                      "<p>Please Click at link to approve or reject the request</p>" +
                                                                      "<p>" +
                                                                      "   <a style='display: block;width: 115px;height: 25px;background: #5CB85C;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='{4}/EmailActivities/OfficeVisitApprove?id={5}&managerId={6}'>Approve</a>" +
                                                                      "   <a style='display: block;width: 115px;height: 25px;background: #F0AD4E;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;' href='{7}/EmailActivities/OfficeVisitReject?id={8}&managerId={9}' >Reject</a>" +
                                                                      "</p>" +
                                                                  "</p>" +
                                                              "</div>" +
                                                          "</div>";
        /// <summary>
        /// 0-code
        /// 1-name
        /// 2-designation
        /// 3-department
        /// 
        /// </summary>
        string EmployeeInformationTemplate = "<p>{0}-{1}<br/>{2}-{3}</p>";
        #endregion

        SOfficeVisit officeVisitServices = null;
        LocalizedString loc = null;
        SEmployee employeeServices = null;
        private int branchId = (int)RiddhaSession.BranchId;
        public OfficeVisitApiController()
        {
            loc = new LocalizedString();
            officeVisitServices = new SOfficeVisit();
            employeeServices = new SEmployee();
        }
        [ActionFilter("2043")]
        public ServiceResult<List<OfficeVisitGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SOfficeVisit service = new SOfficeVisit();
            var officeVisitLst = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                                  select new OfficeVisitGridVm()
                                  {
                                      Id = c.Id,
                                      From = c.From.ToString("yyyy/MM/dd"),
                                      To = c.To.ToString("yyyy/MM/dd"),
                                      FromTime = c.From.TimeOfDay.ToString(@"hh\:mm"),
                                      ToTime = c.To.TimeOfDay.ToString(@"hh\:mm"),
                                      Remark = c.Remark
                                  }).ToList();

            return new ServiceResult<List<OfficeVisitGridVm>>()
            {
                Data = officeVisitLst,
                Status = ResultStatus.Ok
            };
        }
        // GET api/officevisiteapi/5
        public ServiceResult<OfficeVisitModel> Get(int id)
        {
            string lang = RiddhaSession.Language;
            OfficeVisitModel vm = new OfficeVisitModel();
            vm.OfficeVisit = officeVisitServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            vm.FromTime = vm.OfficeVisit.From.TimeOfDay.ToString(@"hh\:mm");
            vm.ToTime = vm.OfficeVisit.To.TimeOfDay.ToString(@"hh\:mm");
            vm.OfficeVisit.IsApprove = vm.OfficeVisit.IsApprove;
            var officeVisitDetailLst = officeVisitServices.ListDetail().Data.Where(x => x.OfficeVisitId == vm.OfficeVisit.Id);
            if (lang == "ne")
            {
                vm.EmpLst = (from c in officeVisitDetailLst
                             select new OfficeVisitEmpVm()
                             {
                                 Id = c.EmployeeId,
                                 Name = c.Employee.Code + " - " + (!string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.NameNp : c.Employee.Name) + " - " + (c.Employee.Mobile ?? "")
                             }).ToList();
            }
            else
            {
                vm.EmpLst = (from c in officeVisitDetailLst
                             select new OfficeVisitEmpVm()
                             {
                                 Id = c.EmployeeId,
                                 Name = c.Employee.Code + " - " + c.Employee.Name + " - " + (c.Employee.Mobile ?? "")
                             }).ToList();
            }
            return new ServiceResult<OfficeVisitModel>
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        public ServiceResult<EmpSearchViewModel> SearchEmployee(string empCode, string empName)
        {
            EmpSearchViewModel vm = new EmpSearchViewModel();
            var empList = employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
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

        // POST api/officevisiteapi
        [ActionFilter("2033")]
        public ServiceResult<OfficeVisitModel> Post(OfficeVisitModel vm)
        {
            vm.OfficeVisit.BranchId = RiddhaSession.BranchId;
            vm.OfficeVisit.From = vm.OfficeVisit.From.AddTicks(vm.FromTime.ToTimeSpan().Ticks);
            vm.OfficeVisit.To = vm.OfficeVisit.To.AddTicks(vm.ToTime.ToTimeSpan().Ticks);
            vm.OfficeVisit.IsApprove = false;
            vm.OfficeVisit.OfficeVisitStatus = OfficeVisitStatus.New;
            var result = officeVisitServices.Add(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2008", "2033", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.OfficeVisit.Id, loc.Localize(result.Message));
                if (RiddhaSession.PackageId == 3)
                {
                    var allemployee = employeeServices.List().Data.Where(x => x.BranchId == branchId);
                    var visitors = (from c in allemployee
                                    join d in vm.EmpIds on c.Id equals d
                                    select c
                                              ).ToList();
                    var requestingEmployee = visitors.GroupBy(x => x.ReportingManagerId).ToList();
                    string participants = "";
                    foreach (var item in visitors)
                    {
                        participants += string.Format(EmployeeInformationTemplate, item.Code, item.Name, item.Designation.Name, item.Section.Department.Name);
                    }
                    foreach (var item in requestingEmployee)
                    {
                        EEmployee manager = allemployee.Where(x => x.Id == item.Key).FirstOrDefault();
                        if (manager != null && manager.Email != null)
                        {
                            //manager email template
                            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                            var finalTemplate = string.Format(OfficeVisitManagerRequestTemplate, manager.Name, vm.OfficeVisit.From, vm.OfficeVisit.To, participants, baseUrl, result.Data.OfficeVisit.Id, manager.Id, baseUrl, result.Data.OfficeVisit.Id, manager.Id);
                            var mail = new MailCommon();
                            string subject = "Office Visit";
                            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(finalTemplate, null, "text/html");
                            mail.SendMail(manager.Email, subject, "", htmlView);
                        }
                    }
                }
            }
            return new ServiceResult<OfficeVisitModel>()
            {
                Data = vm,
                Status = ResultStatus.Ok,
                Message = loc.Localize("AddedSuccess")
            };
        }

        [ActionFilter("2034")]
        public ServiceResult<OfficeVisitModel> Put(OfficeVisitModel vm)
        {
            vm.OfficeVisit.From = vm.OfficeVisit.From.AddTicks(vm.FromTime.ToTimeSpan().Ticks);
            vm.OfficeVisit.To = vm.OfficeVisit.To.AddTicks(vm.ToTime.ToTimeSpan().Ticks);
            var result = officeVisitServices.Update(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2008", "2034", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.OfficeVisit.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<OfficeVisitModel>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status

            };
        }

        // DELETE api/officevisiteapi/5
        [ActionFilter("2035")]
        public ServiceResult<int> Delete(int id)
        {
            var officeVisit = officeVisitServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = officeVisitServices.Remove(officeVisit);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2008", "2035", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }


        [HttpPost, ActionFilter("2043")]
        public KendoGridResult<List<OfficeVisitGridVm>> GetOfficeVisitKendoGrid(KendoPageListArguments vm)
        {
            int branchId = (int)RiddhaSession.BranchId;
            SOfficeVisit service = new SOfficeVisit();
            SEmployee empServices = new SEmployee();
            List<EOfficeVisit> officeVisit = new List<EOfficeVisit>();
            List<EOfficeVisitDetail> officeVisitDetailList = new List<EOfficeVisitDetail>();
            officeVisitDetailList = service.ListDetail().Data.Where(x => x.OfficeVisit.BranchId == branchId).ToList();
            var empQuery = Common.GetEmployees().Data;
            if (RiddhaSession.IsHeadOffice || RiddhaSession.DataVisibilityLevel == 4)
            {
                officeVisit = service.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).ToList();
            }
            else
            {
                officeVisit = service.List().Data.Where(x => x.BranchId == branchId).ToList();
            }
            if (!(RiddhaSession.PackageId != 3))
            {
                if (RiddhaSession.RoleId != 0)
                {
                    var reportingManager = empServices.List().Data.Where(x => x.ReportingManagerId == RiddhaSession.EmployeeId || x.Id == RiddhaSession.EmployeeId).ToList();
                    if (reportingManager.Count() != 0)
                    {
                        var officeVisitDetails = (from c in service.ListDetail().Data.ToList()
                                                  join d in reportingManager on c.EmployeeId equals d.Id
                                                  select c.OfficeVisitId
                                                  ).Distinct().ToList();

                        var officeVisitMaster = (from c in service.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList()
                                                 join d in officeVisitDetails on c.Id equals d
                                                 select c
                                                 ).ToList();

                        officeVisit = officeVisitMaster;
                    }
                    else
                    {
                        officeVisit = service.List().Data.Where(x => x.BranchId == branchId).ToList();
                    }
                }
            }
            var officeVisitlist = (from c in officeVisit
                                   join d in service.ListDetail().Data on c.Id equals d.OfficeVisitId
                                   join e in empQuery on d.EmployeeId equals e.Id
                                   select new OfficeVisitGridVm()
                                   {
                                       Id = c.Id,
                                       From = c.From.ToString("yyyy/MM/dd"),
                                       FromTime = c.From.TimeOfDay.ToString(@"hh\:mm"),
                                       To = c.To.ToString("yyyy/MM/dd"),
                                       ToTime = c.To.TimeOfDay.ToString(@"hh\:mm"),
                                       Remark = c.Remark,
                                       OfficeVisitStatusName = Enum.GetName(typeof(OfficeVisitStatus), c.OfficeVisitStatus),
                                       IsApprove = c.IsApprove,
                                       OfficeVisitStatus = c.OfficeVisitStatus,
                                       EmployeeName = String.Join(" , ", officeVisitDetailList.Where(x => x.OfficeVisitId == c.Id).Select(x => x.Employee.Name))
                                   }).ToList();
            return new KendoGridResult<List<OfficeVisitGridVm>>()
            {
                Data = officeVisitlist.OrderByDescending(x => x.From).Skip(vm.Skip).Take(vm.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = officeVisitlist.Count(),
            };
        }

    }
    public class OfficeVisitGridVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Remark { get; set; }
        public string OfficeVisitStatusName { get; set; }
        public OfficeVisitStatus OfficeVisitStatus { get; set; }
        public bool IsApprove { get; set; }
    }
    public class officeVisitVm
    {
        public List<int> EmployeeId { get; set; }
        public int? ManagerId { get; set; }
    }
}
