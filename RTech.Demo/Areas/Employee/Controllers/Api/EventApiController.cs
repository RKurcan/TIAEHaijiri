using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.Employee.Mobile.Services;
using Riddhasoft.Employee.Services;
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

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class EventApiController : ApiController
    {
        SEvent eventServices = null;
        LocalizedString loc = null;
        SNotification notificationServices = null;
        SBranch branchServices = null;
        SDepartment departmentServices = null;
        SSection sectionServices = null;
        public EventApiController()
        {
            notificationServices = new SNotification();
            eventServices = new SEvent();
            loc = new LocalizedString();
            branchServices = new SBranch();
            departmentServices = new SDepartment();
            sectionServices = new SSection();
        }
        [ActionFilter("2042")]
        public ServiceResult<List<EventGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SEvent service = new SEvent();
            var eventLst = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                            select new EventGridVm()
                            {
                                Id = c.Id,
                                BranchId = c.BranchId,
                                CreatedById = c.CreatedById,
                                CreatedOn = c.CreatedOn,
                                Description = c.Description,
                                EventLevel = Enum.GetName(typeof(NotificationLevel), c.EventLevel),
                                FiscalYearId = c.FiscalYearId,
                                From = c.From,
                                Title = c.Title,
                                To = c.To
                            }).ToList();

            return new ServiceResult<List<EventGridVm>>()
            {
                Data = eventLst,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EventViewModel> Get(int id)
        {
            EventViewModel vm = new EventViewModel();
            vm.Event = eventServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            List<EEventDetails> detailLst = eventServices.ListDetails().Data.Where(x => x.EventId == id).ToList();
            vm.Targets = (from c in detailLst
                          select c.TargetId).ToArray();
            return new ServiceResult<EventViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        [ActionFilter("2030")]
        public ServiceResult<EEvent> Post(EventViewModel vm)
        {
            int fyId = RiddhaSession.FYId;
            var branchId = RiddhaSession.BranchId;
            var companyId = RiddhaSession.CompanyId;
            vm.Event.CreatedById = 1;
            vm.Event.BranchId = branchId;
            vm.Event.FiscalYearId = RiddhaSession.FYId;

            var result = eventServices.Add(vm.Event, vm.Targets);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2007", "2030", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Event.Id, loc.Localize(result.Message));
                ENotification notification = new ENotification()
                {
                    CompanyId = companyId,
                    EffectiveDate = vm.Event.From,
                    ExpiryDate=vm.Event.To,
                    Message = vm.Event.Description,
                    NotificationType = NotificationType.Event,
                    FiscalYearId = fyId,
                    NotificationLevel = vm.Event.EventLevel,
                    PublishDate = vm.Event.From.Date > DateTime.Now.Date ? vm.Event.From.AddDays(-1) : vm.Event.From,
                    Title = vm.Event.Title,
                    TranDate = DateTime.Now,
                    TypeId = result.Data.Id

                };
                var notificationResult = notificationServices.Add(notification, vm.Targets);
            }

            return new ServiceResult<EEvent>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = ResultStatus.Ok
            };

        }
        [ActionFilter("2031")]
        public ServiceResult<EEvent> Put( EventViewModel vm)
        {
            int fyId = RiddhaSession.FYId;
            var user = RiddhaSession.CurrentUser;
            vm.Event.CreatedById = user.Id;
            vm.Event.CreatedOn = System.DateTime.Now;
            var result = eventServices.Update(vm.Event, vm.Targets);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2007", "2031", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Event.Id, loc.Localize(result.Message));
                ENotification notification = new ENotification()
                {
                    CompanyId = user.Branch.CompanyId,
                    EffectiveDate = vm.Event.From,
                    ExpiryDate=vm.Event.To,
                    FiscalYearId = fyId,
                    Message = vm.Event.Description,
                    NotificationType = NotificationType.Event,
                    NotificationLevel = vm.Event.EventLevel,
                    PublishDate = vm.Event.From.Date > DateTime.Now.Date ? vm.Event.From.AddDays(-1) : vm.Event.From,
                    Title = vm.Event.Title,

                    TranDate = DateTime.Now,
                    TypeId = result.Data.Id


                };
                var notificationresult = notificationServices.Update(notification, vm.Targets);

            }
            return new ServiceResult<EEvent>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [ActionFilter("2032")]
        public ServiceResult<int> Delete(int id)
        {
            if (id < 1)
            {
                return new ServiceResult<int>()
                {
                    Data = 0,
                    Status = ResultStatus.processError
                };
            }
            var result = eventServices.Remove(id);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2007", "2032", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
                var notificationResult = notificationServices.Remove(id, NotificationType.Event);

            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = ResultStatus.Ok

            };


        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetBranches()
        {
            int companyId = RiddhaSession.CompanyId;
            var branches = (from c in branchServices.List().Data.Where(x => x.CompanyId == companyId)
                            select new DropdownViewModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                                NameNp = c.NameNp
                            }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = branches,
                Status = ResultStatus.Ok,
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetDepartments()
        {
            int? branchId = RiddhaSession.BranchId;
            var depaetment = (from c in departmentServices.List().Data.Where(x => x.BranchId == branchId)
                              select new DropdownViewModel
                              {
                                  Id = c.Id,
                                  Name = c.Name,
                                  NameNp = c.NameNp
                              }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = depaetment,
                Status = ResultStatus.Ok,
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetSections()
        {
            int? branchId = RiddhaSession.BranchId;
            var section = (from c in sectionServices.List().Data.Where(x => x.BranchId == branchId)
                           select new DropdownViewModel
                           {
                               Id = c.Id,
                               Name = c.Name,
                               NameNp = c.NameNp
                           }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = section,
                Status = ResultStatus.Ok,
            };
        }


    }
    public class EventViewModel
    {
        public EEvent Event { get; set; }
        public int[] Targets { get; set; }
    }
    public class EventGridVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EventLevel { get; set; }
        public int? BranchId { get; set; }
        public int FiscalYearId { get; set; }
        public int CreatedById { get; set; }
    }

}
