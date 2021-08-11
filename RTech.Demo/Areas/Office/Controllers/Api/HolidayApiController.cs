using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class HolidayApiController : ApiController
    {
        SDateTable dateService = null;
        SHoliday holidayServices = null;
        SHolidayEmployee empHolidayServices = null;
        SEmployee empServices = null;
        SDepartment depServices = null;
        LocalizedString loc = null;
        SNotification notificationServices = null;
        public HolidayApiController()
        {
            holidayServices = new SHoliday();
            empHolidayServices = new SHolidayEmployee();
            empServices = new SEmployee();
            depServices = new SDepartment();
            loc = new LocalizedString();
            notificationServices = new SNotification();
            dateService = new SDateTable();
        }
        [ActionFilter("2039")]
        public ServiceResult<List<HolidayGridViewModel>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SHoliday service = new SHoliday();
            List<HolidayGridViewModel> data = service.GetHolidayList(branchId, RiddhaSession.FYId);


            return new ServiceResult<List<HolidayGridViewModel>>()
            {
                Data = data,
                Status = ResultStatus.Ok
            };
        }
        [HttpGet]
        // GET api/holidayapi/5
        public ServiceResult<HolidayViewModel> GetHoliday(int id)
        {
            int? branchId = RiddhaSession.BranchId;
            HolidayViewModel vm = new HolidayViewModel();
            SDepartmentWiseHoliday departmentWiseHolidayServices = new SDepartmentWiseHoliday();
            vm.Holiday = holidayServices.List().Data.Where(x => x.Id == id && x.BranchId == branchId).FirstOrDefault();
            vm.HolidayDetails = holidayServices.ListDetails().Data.Where(x => x.HolidayId == vm.Holiday.Id).ToList();
            int[] sec = departmentWiseHolidayServices.List().Data.Where(x => x.HolidayId == id).Select(x => x.SectionId).ToArray();
            int[] dep = departmentWiseHolidayServices.List().Data.Where(x => x.HolidayId == id).Select(x => x.Section.DepartmentId).ToArray();
            vm.SectionIds = string.Join(",", sec);
            vm.DepartmentIds = string.Join(",", dep);
            return new ServiceResult<HolidayViewModel>()
            {
                Data = vm,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        // POST api/holidayapi
        [ActionFilter("2021")]
        public ServiceResult<HolidayViewModel> Post(HolidayViewModel vm)
        {
            int fyId = RiddhaSession.FYId;
            string lang = RiddhaSession.Language;
            //vm.Holiday.BranchId = RiddhaSession.BranchId;
            var result = holidayServices.Add(vm);
            int[] SectionIds = { };
            if (vm.SectionIds != null)
                SectionIds = vm.SectionIds.Split(',').Select(x => int.Parse(x)).ToArray();

            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("2004", "2021", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Holiday.Id, loc.Localize(result.Message));
                List<ENotification> lst = new List<ENotification>();
                foreach (var item in result.Data.HolidayDetails)
                {
                    ENotification notification = new ENotification();
                    notification.CompanyId = RiddhaSession.CompanyId;
                    notification.FiscalYearId = fyId;
                    notification.EffectiveDate = item.BeginDate;
                    notification.ExpiryDate = item.EndDate;
                    notification.Message = string.IsNullOrEmpty(result.Data.Holiday.Description) ? "There will be holiday of " + result.Data.Holiday.Name + " from " + item.BeginDate.ToString("yyyy/MM/dd") + " to " + item.EndDate.ToString("yyyy/MM/dd") : result.Data.Holiday.Description;
                    notification.NotificationType = NotificationType.Holiday;
                    notification.NotificationLevel = NotificationLevel.All;
                    notification.PublishDate = item.BeginDate.Date > DateTime.Now.Date ? item.BeginDate.AddDays(-1) : DateTime.Now;
                    notification.Title = vm.Holiday.Name;
                    notification.TranDate = DateTime.Now;
                    notification.TypeId = result.Data.Holiday.Id;
                    lst.Add(notification);
                }
                var notificationResult = notificationServices.AddRange(lst);

                List<EDepartmentWiseHoliday> list = new List<EDepartmentWiseHoliday>();
                SDepartmentWiseHoliday departmentWiseHolidayServices = new SDepartmentWiseHoliday();
                foreach (var item in SectionIds)
                {
                    EDepartmentWiseHoliday model = new EDepartmentWiseHoliday();
                    model.SectionId = item;
                    model.HolidayId = result.Data.Holiday.Id;
                    list.Add(model);
                }
                departmentWiseHolidayServices.Add(list);
            }
            return new ServiceResult<HolidayViewModel>()
            {
                Data = vm,
                Status = ResultStatus.Ok,
                Message = loc.Localize(result.Message)
            };
        }
        // PUT api/holidayapi/5
        [ActionFilter("2022")]
        public ServiceResult<HolidayViewModel> Put(HolidayViewModel vm)
        {
            int fyId = RiddhaSession.FYId;
            var result = holidayServices.Update(vm);
            var companyId = RiddhaSession.CompanyId;
            //vm.Holiday.BranchId = RiddhaSession.BranchId;
            int[] SectionIds = { };
            if (vm.SectionIds != null)
                SectionIds = vm.SectionIds.Split(',').Select(x => int.Parse(x)).ToArray();
            if (result.Status == ResultStatus.Ok)
            {
                SDepartmentWiseHoliday departmentWiseHolidayServices = new SDepartmentWiseHoliday();
                Common.AddAuditTrail("2004", "2022", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Holiday.Id, loc.Localize(result.Message));
                List<ENotification> lst = new List<ENotification>();
                foreach (var item in result.Data.HolidayDetails)
                {
                    ENotification notification = new ENotification()
                    {
                        CompanyId = companyId,
                        FiscalYearId = fyId,
                        EffectiveDate = item.BeginDate,
                        ExpiryDate = item.EndDate,
                        Message = vm.Holiday.Description == null ? "There will be holiday of " + vm.Holiday.Name + " from " + item.BeginDate.ToString("yyyy/MM/dd") + " to " + item.EndDate.ToString("yyyy/MM/dd") : vm.Holiday.Description,
                        NotificationType = NotificationType.Holiday,
                        NotificationLevel = NotificationLevel.All,
                        PublishDate = item.BeginDate.Date > DateTime.Now.Date ? item.BeginDate.AddDays(-1) : DateTime.Now,
                        Title = vm.Holiday.Name,
                        TranDate = DateTime.Now,
                        TypeId = result.Data.Holiday.Id
                    };
                    lst.Add(notification);
                }
                var notificationResult = notificationServices.AddRange(lst);

                //Remove Existing Department Wise Holiday
                var existingHolidayList = departmentWiseHolidayServices.List().Data.Where(x => x.HolidayId == vm.Holiday.Id).ToList();
                departmentWiseHolidayServices.Remove(existingHolidayList);

                //Add Department Wise Holiday

                List<EDepartmentWiseHoliday> list = new List<EDepartmentWiseHoliday>();
                foreach (var item in SectionIds)
                {
                    EDepartmentWiseHoliday model = new EDepartmentWiseHoliday();
                    model.SectionId = item;
                    model.HolidayId = result.Data.Holiday.Id;
                    list.Add(model);
                }
                departmentWiseHolidayServices.Add(list);
            }
            return new ServiceResult<HolidayViewModel>()
            {
                Data = vm,
                Message = loc.Localize(result.Message),
                Status = ResultStatus.Ok
            };
        }

        // DELETE api/holidayapi/5
        [ActionFilter("2023")]
        public ServiceResult<int> Delete(int id)
        {
            var existingEmpHoliday = empHolidayServices.List().Data.Where(x => x.HolidayId == id).ToList();
            empHolidayServices.RemoveRange(existingEmpHoliday);
            var holiday = holidayServices.List().Data.Where(x => x.Id == id).FirstOrDefault() ?? new EHoliday();
            var result = holidayServices.Remove(holiday);
            if (result.Status == ResultStatus.Ok)
            {
                SDepartmentWiseHoliday departmentWiseHolidayServices = new SDepartmentWiseHoliday();
                Common.AddAuditTrail("2004", "2023", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
                var notificationResult = notificationServices.Remove(id, NotificationType.Holiday);
                var departmentWiseHolidayList = departmentWiseHolidayServices.List().Data.Where(x => x.HolidayId == id).ToList();
                foreach (var item in departmentWiseHolidayList)
                {
                    departmentWiseHolidayServices.Remove(item);
                }
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet, ActionFilter("2020")]
        public ServiceResult<EHoliday> PullHolidays()
        {
            int fiscalYearId = RiddhaSession.FYId;
            if (fiscalYearId == 0)
            {
                return new ServiceResult<EHoliday>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = "FiscalYearNotSet"
                };
            }
            var branchId = RiddhaSession.BranchId;
            var rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~/files");
            string path = rootPath + "/HamrohajiriHoliday.xlsx";
            FileStream stream = new FileStream(path, FileMode.Open);
            using (var package = new OfficeOpenXml.ExcelPackage(stream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {

                    EHoliday holiday = new EHoliday();
                    holiday.Name = (workSheet.Cells[rowIterator, 1].Value ?? string.Empty).ToString();
                    if (checkValidString(holiday.Name) == false)
                    {
                        continue;
                    }
                    holiday.NameNp = (workSheet.Cells[rowIterator, 2].Value ?? string.Empty).ToString();
                    holiday.ApplicableReligion = (ApplicableReligion)(workSheet.Cells[rowIterator, 3].Value ?? string.Empty).ToInt();
                    holiday.ApplicableGender = (ApplicableGender)(workSheet.Cells[rowIterator, 4].Value ?? string.Empty).ToInt();
                    holiday.BranchId = branchId;
                    holiday.HolidayType = (HolidayType)(workSheet.Cells[rowIterator, 5].Value ?? string.Empty).ToInt();
                    holiday.IsOccuredInSameDate = (workSheet.Cells[rowIterator, 6].Value ?? string.Empty).ToString() == "1" ? true : false;
                    string fromDate = (workSheet.Cells[rowIterator, 7].Value) == null ? String.Empty : (ConversionExtension.ToDateTime(workSheet.Cells[rowIterator, 7].Value.ToString()).Date.ToString("yyyy/MM/dd"));
                    if (string.IsNullOrEmpty(fromDate))
                    {
                        continue;
                    }
                    EHolidayDetails detail = new EHolidayDetails();
                    detail.BeginDate = dateService.ConvertToEngDate(fromDate);
                    string toDate = (workSheet.Cells[rowIterator, 8].Value) == null ? String.Empty : (ConversionExtension.ToDateTime(workSheet.Cells[rowIterator, 8].Value.ToString()).Date.ToString("yyyy/MM/dd"));
                    if (string.IsNullOrEmpty(toDate))
                    {
                        continue;
                    }
                    detail.EndDate = dateService.ConvertToEngDate(toDate);
                    detail.NumberOfDays = (workSheet.Cells[rowIterator, 9].Value ?? string.Empty).ToInt();
                    detail.FiscalYearId = fiscalYearId;

                    string description = "";
                    string nepBeginDate = dateService.ConvertToNepDate(detail.BeginDate);
                    string nepEndDate = dateService.ConvertToNepDate(detail.EndDate);
                    if (detail.BeginDate.Date == detail.EndDate.Date)
                    {
                        description = string.IsNullOrEmpty(holiday.Description) ? "We would like to inform you that there will be holiday of " + holiday.Name + " on " + nepBeginDate : holiday.Description;
                    }
                    else
                    {
                        description = string.IsNullOrEmpty(holiday.Description) ? "We would like to inform you that there will be holiday of " + holiday.Name + " from " + nepBeginDate + " to " + nepEndDate : holiday.Description;
                    }
                    holiday.Description = description;
                    bool result = holidayServices.PullHolidays(holiday, detail);
                }
                stream.Close();
                stream.Dispose();
                Common.AddAuditTrail("2004", "2020", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 1, loc.Localize("ImportSuccess"));
                return new ServiceResult<EHoliday>()
                {
                    Data = null,
                    Status = ResultStatus.Ok,
                    Message = loc.Localize("ImportSuccess")
                };
            }
        }

        private bool checkValidString(string value)
        {
            return !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
        }

        [HttpPost, ActionFilter("2039")]
        public KendoGridResult<List<HolidayKendoGridVm>> GetHolidayKendoGrid(KendoPageListArguments arg)
        {
            //int fiscalyearId = new SFiscalYear().List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId && x.CurrentFiscalYear).FirstOrDefault().Id;
            int branchId = (int)RiddhaSession.BranchId;
            int fiscalyearId = RiddhaSession.FYId;
            SHoliday service = new SHoliday();
            IQueryable<EHoliday> holidayQuery = null;
            var empQuery = Common.GetEmployees().Data;
            holidayQuery = service.List().Data.Where(x => x.BranchId == branchId);
            var holidayDetail = service.ListDetails().Data;
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EHoliday> paginatedQuery;
            switch (searchField)
            {
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = holidayQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id);
                    }
                    else
                    {
                        paginatedQuery = holidayQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id);
                    }
                    break;
                default:
                    paginatedQuery = holidayQuery.OrderByDescending(x => x.Id);
                    break;
            }

            var holidaylist = (from c in paginatedQuery.ToList()
                               join d in holidayDetail on c.Id equals d.HolidayId
                               where d.FiscalYearId == fiscalyearId
                               select new HolidayKendoGridVm()
                               {
                                   Id = c.Id,
                                   Name = c.Name,
                                   NameNp = c.NameNp,
                                   ApplicableGender = c.ApplicableGender,
                                   ApplicableReligion = c.ApplicableReligion,
                                   BranchId = c.BranchId,
                                   Description = c.Description,
                                   HolidayType = c.HolidayType,
                                   IsOccuredInSameDate = c.IsOccuredInSameDate,
                                   Date = d ==null?"":d.BeginDate.ToString("yyyy/MM/dd")
                               }).ToList();
            return new KendoGridResult<List<HolidayKendoGridVm>>()
            {
                Data = holidaylist.OrderByDescending(x => x.Date).Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = holidaylist.Count()
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetDepartment()
        {
            var result = (from c in depServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId)
                          select new DropdownViewModel()
                          {
                              Id = c.Id,
                              Name = c.Name,
                          }).ToList();

            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetSections(string depIds)
        {
            string[] departments = depIds.Split(',');
            SSection sectionServices = new SSection();
            var result = (from c in sectionServices.List().Data.ToList()
                          join d in departments on c.DepartmentId equals int.Parse(d)
                          select new DropdownViewModel()
                          {
                              Id = c.Id,
                              Name = c.Name,
                          }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetFiscalYear()
        {
            SFiscalYear fiscalYearServices = new SFiscalYear();
            var result = (from c in fiscalYearServices.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId && x.CurrentFiscalYear)
                          select new DropdownViewModel()
                          {
                              Id = c.Id,
                              Name = c.FiscalYear,
                          }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

    public class HolidayKendoGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string Description { get; set; }
        public HolidayType HolidayType { get; set; }
        public bool IsOccuredInSameDate { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public ApplicableReligion ApplicableReligion { get; set; }
        public int? BranchId { get; set; }
        public string Date { get; set; }
    }
}
