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
    public class NoticeApiController : ApiController
    {
        SNotification notificationServices = null;
        SNotice noticeServices = null;
        SBranch branchServices = null;
        SDepartment departmentServices = null;
        SSection sectionServices = null;
        LocalizedString loc = null;
        public NoticeApiController()
        {
            noticeServices = new SNotice();
            loc = new LocalizedString();
            notificationServices = new SNotification();
            branchServices = new SBranch();
            departmentServices = new SDepartment();
            sectionServices = new SSection();
        }
        [ActionFilter("2041")]
        public ServiceResult<List<NoticeGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SNotice service = new SNotice();
            var noticetLst = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                              select new NoticeGridVm()
                              {
                                  Id = c.Id,
                                  BranchId = c.BranchId,
                                  CreatedById = c.CreatedById,
                                  CreatedOn = c.CreatedOn,
                                  Description = c.Description,
                                  ExpiredOn = c.ExpiredOn,
                                  FiscalYearId = c.FiscalYearId,
                                  IsUrgent = c.IsUrgent,
                                  NoticeLevel = Enum.GetName(typeof(NotificationLevel), c.NoticeLevel),
                                  PublishedOn = c.PublishedOn,
                                  Title = c.Title
                              }).ToList();

            return new ServiceResult<List<NoticeGridVm>>()
            {
                Data = noticetLst,
                Status = ResultStatus.Ok
            };
        }


        public ServiceResult<NoticeViewModel> Get(int id)
        {
            NoticeViewModel vm = new NoticeViewModel();
            vm.Notice = noticeServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            List<ENoticeDetails> detailLst = noticeServices.ListDetails().Data.Where(x => x.NoticeId == id).ToList();
            vm.Targets = (from c in detailLst
                          select c.TargetId).ToArray();
            return new ServiceResult<NoticeViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("2027")]
        public ServiceResult<ENotice> Post(NoticeViewModel vm)
        {
            int fyId = RiddhaSession.FYId;
            var companyId = RiddhaSession.CompanyId;
            var branchId = RiddhaSession.BranchId;
            vm.Notice.CreatedById = 1;
            vm.Notice.BranchId = branchId;
            vm.Notice.FiscalYearId = RiddhaSession.FYId;
            var result = noticeServices.Add(vm.Notice, vm.Targets);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2006", "2027", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Notice.Id, loc.Localize(result.Message));
                ENotification notification = new ENotification()
                {
                    CompanyId = companyId,
                    EffectiveDate = vm.Notice.PublishedOn,
                    ExpiryDate=vm.Notice.ExpiredOn,
                    FiscalYearId = fyId,
                    Message = vm.Notice.Description,
                    NotificationLevel = vm.Notice.NoticeLevel,
                    NotificationType = NotificationType.Notice,
                    PublishDate = vm.Notice.PublishedOn.Date > DateTime.Now.Date ? vm.Notice.PublishedOn.AddDays(-1) : vm.Notice.PublishedOn,
                    Title = vm.Notice.Title,
                    TranDate = DateTime.Now,
                    TypeId = result.Data.Id
                };
                var notificationResult = notificationServices.Add(notification, vm.Targets);
            }
            return new ServiceResult<ENotice>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [ActionFilter("2028")]
        public ServiceResult<ENotice> Put([FromBody]NoticeViewModel vm)
        {
            int fyId = RiddhaSession.FYId;
            var userId = RiddhaSession.UserId;
            var companyId = RiddhaSession.CompanyId;
            vm.Notice.CreatedById = userId;
            vm.Notice.CreatedOn = DateTime.Now;
            var result = noticeServices.Update(vm.Notice, vm.Targets);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2006", "2028", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Notice.Id, loc.Localize(result.Message));
                ENotification notification = new ENotification()
                {
                    CompanyId = companyId,
                    EffectiveDate = vm.Notice.PublishedOn,
                    ExpiryDate=vm.Notice.ExpiredOn,
                    FiscalYearId = fyId,
                    Message = vm.Notice.Description,
                    NotificationLevel = vm.Notice.NoticeLevel,
                    NotificationType = NotificationType.Notice,
                    PublishDate = vm.Notice.PublishedOn.Date > DateTime.Now.Date ? vm.Notice.PublishedOn.AddDays(-1) : vm.Notice.PublishedOn,
                    Title = vm.Notice.Title,
                    TranDate = DateTime.Now,
                    TypeId = result.Data.Id
                };
                var notificationResult = notificationServices.Update(notification, vm.Targets);
            }
            return new ServiceResult<ENotice>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [ActionFilter("2029")]
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
            var result = noticeServices.Remove(id);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2006", "2029", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
                var notificationResult = notificationServices.Remove(id, NotificationType.Notice);
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet]
        public ServiceResult<ENotice> Publish(int id)
        {
            var notice = noticeServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            //notice.PublishedById = RiddhaSession.CurrentUser.Id;
            notice.PublishedOn = DateTime.Now;
            noticeServices.Publish(notice);
            return new ServiceResult<ENotice>()
            {
                Data = null,
                Message = "Published Successfully",
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
    public class NoticeViewModel
    {
        public ENotice Notice { get; set; }
        public int[] Targets { get; set; }
    }

    public class NoticeGridVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedById { get; set; }
        public int? BranchId { get; set; }
        public bool IsUrgent { get; set; }
        public string NoticeLevel { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        public int FiscalYearId { get; set; }
    }


}
