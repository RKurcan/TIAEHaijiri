using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Mobile.Entities;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SHoliday
    {
        RiddhaDBContext db = null;
        public SHoliday()
        {
            db = new DB.RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EHoliday>> List()
        {
            return new ServiceResult<IQueryable<EHoliday>>()
            {
                Data = db.Holiday,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<HolidayViewModel> Add(HolidayViewModel vm)
        {
            db.Holiday.Add(vm.Holiday);
            db.SaveChanges();

            List<EHolidayDetails> detailLst = new List<EHolidayDetails>();
            foreach (var item in vm.HolidayDetails)
            {
                EHolidayDetails model = new EHolidayDetails();
                model.BeginDate = item.BeginDate;
                model.EndDate = item.EndDate;
                model.FiscalYearId = item.FiscalYearId;
                model.HolidayId = vm.Holiday.Id;
                model.NumberOfDays = (item.EndDate - item.BeginDate).Days + 1;
                detailLst.Add(model);
            }
            db.HolidayDetails.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<HolidayViewModel>()
            {
                Data = vm,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<List<EHoliday>> AddRange(List<EHoliday> list)
        {
            db.Holiday.AddRange(list);
            db.SaveChanges();
            return new ServiceResult<List<EHoliday>>()
            {
                Data = list,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<HolidayViewModel> Update(HolidayViewModel vm)
        {
            db.Entry(vm.Holiday).State = EntityState.Modified;
            db.SaveChanges();
            var existingDetails = db.HolidayDetails.Where(x => x.HolidayId == vm.Holiday.Id).ToList();
            if (existingDetails.Count() > 0)
            {
                db.HolidayDetails.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            List<EHolidayDetails> detailLst = new List<EHolidayDetails>();
            foreach (var item in vm.HolidayDetails)
            {
                EHolidayDetails model = new EHolidayDetails();
                model.BeginDate = item.BeginDate;
                model.EndDate = item.EndDate;
                model.FiscalYearId = item.FiscalYearId;
                model.HolidayId = vm.Holiday.Id;
                model.NumberOfDays = (item.EndDate - item.BeginDate).Days + 1;
                detailLst.Add(model);
            }
            db.HolidayDetails.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<HolidayViewModel>()
            {
                Data = vm,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EHoliday model)
        {
            db.Holiday.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> RemoveRange(List<EHoliday> list)
        {
            db.Holiday.RemoveRange(list);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EHolidayDetails>> ListDetails()
        {
            return new ServiceResult<IQueryable<EHolidayDetails>>()
            {
                Data = db.HolidayDetails,
                Status = ResultStatus.Ok
            };
        }

        public bool PullHolidays(EHoliday holiday, EHolidayDetails detail)
        {
            EHoliday existingHoliday = db.Holiday.Where(x => x.BranchId == holiday.BranchId && x.Name.ToUpper() == holiday.Name.ToUpper()).FirstOrDefault();
            if (existingHoliday == null)
            {
                db.Holiday.Add(holiday);
                db.SaveChanges();
                var existingDetail = db.HolidayDetails.Where(x => x.HolidayId == holiday.Id && x.FiscalYearId == detail.FiscalYearId).FirstOrDefault();
                if (existingDetail != null)
                {
                    db.HolidayDetails.Remove(existingDetail);
                    db.SaveChanges();
                }
                detail.HolidayId = holiday.Id;
                db.HolidayDetails.Add(detail);
                db.SaveChanges();
                AddNotification(holiday, detail);
            }
            else
            {
                var existingDetail = db.HolidayDetails.Where(x => x.HolidayId == existingHoliday.Id && x.FiscalYearId == detail.FiscalYearId).FirstOrDefault();
                if (existingDetail != null)
                {
                    db.HolidayDetails.Remove(existingDetail);
                    db.SaveChanges();
                }
                detail.HolidayId = existingHoliday.Id;
                db.HolidayDetails.Add(detail);
                db.SaveChanges();
                AddNotification(existingHoliday, detail);
            }
            return true;
        }

        private void AddNotification(EHoliday holiday, EHolidayDetails detail)
        {
            var existingNotification = db.Notification.Where(x => x.CompanyId == holiday.BranchId && x.FiscalYearId == detail.FiscalYearId && x.TypeId == holiday.Id).FirstOrDefault();
            if (existingNotification != null)
            {
                db.Notification.Remove(existingNotification);
                db.SaveChanges();
            }

            ENotification notification = new ENotification()
            {
                CompanyId = (int)holiday.BranchId,
                FiscalYearId = detail.FiscalYearId,
                EffectiveDate = detail.BeginDate,
                ExpiryDate = detail.EndDate,
                Message = holiday.Description,
                NotificationType = NotificationType.Holiday,
                NotificationLevel = NotificationLevel.All,
                PublishDate = detail.BeginDate.AddDays(-1),
                Title = holiday.Name,
                TranDate = DateTime.Now,
                TypeId = holiday.Id
            };
            db.Notification.Add(notification);
            db.SaveChanges();
        }

        public List<HolidayGridViewModel> GetHolidayList(int? branchId, int FiscalYearId)
        {
            var list = (from c in db.HolidayDetails
                        where c.Holiday.BranchId == branchId && c.FiscalYearId == FiscalYearId
                        select new HolidayGridViewModel()
                        {
                            Id = c.Holiday.Id,
                            ApplicableGender = c.Holiday.ApplicableGender,
                            ApplicableReligion = c.Holiday.ApplicableReligion,
                            BranchId = c.Holiday.BranchId,
                            Date = c.BeginDate,
                            Description = c.Holiday.Description,
                            HolidayType = c.Holiday.HolidayType,
                            IsOccuredInSameDate = c.Holiday.IsOccuredInSameDate,
                            Name = c.Holiday.Name,
                            NameNp = c.Holiday.NameNp

                        }).OrderBy(x => x.Date).ToList();
            return list;
        }
    }
    public class HolidayViewModel
    {
        public EHoliday Holiday { get; set; }
        public List<EHolidayDetails> HolidayDetails { get; set; }
        public string SectionIds { get; set; }
        public string DepartmentIds { get; set; }
    }
    public class HolidayGridViewModel : EHoliday
    {
        //public DateTime SetDate { set { Date=value.ToString("yyyy/MM/dd") } }
        public DateTime Date { get; set; }
    }
}
