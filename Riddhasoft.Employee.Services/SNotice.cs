using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;

using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Services
{
    public class SNotice
    {
        RiddhaDBContext db = null;
        public SNotice()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ENotice>> List()
        {
            return new ServiceResult<IQueryable<ENotice>>()
            {
                Data = db.Notice,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ENotice> Add(ENotice model,int[] targets)
        {
            db.Notice.Add(model);
            db.SaveChanges();
            if (model.NoticeLevel!=NotificationLevel.All)
            {
                List<ENoticeDetails> detailLst = new List<ENoticeDetails>();
                for (int i = 0; i < targets.Count(); i++)
                {
                    ENoticeDetails detail = new ENoticeDetails();
                    detail.NoticeId = model.Id;
                    detail.TargetId = targets[i];
                    detailLst.Add(detail);
                }
                db.NoticeDetail.AddRange(detailLst);
                db.SaveChanges();
            }
            return new ServiceResult<ENotice>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ENotice> Update(ENotice model,int[] targets)
        {
            ENotice notice = db.Notice.Where(x => x.Id == model.Id).FirstOrDefault();
            notice.Description = model.Description;
            notice.ExpiredOn = model.ExpiredOn;
            notice.IsUrgent = model.IsUrgent;
            notice.NoticeLevel = model.NoticeLevel;
            notice.PublishedOn = model.PublishedOn;
            notice.Title = model.Title;
            notice.CreatedById = model.CreatedById; 
            notice.CreatedOn = model.CreatedOn;
            db.Entry(notice).State = EntityState.Modified;
            db.SaveChanges();

            List<ENoticeDetails> existingDetails = db.NoticeDetail.Where(x => x.NoticeId == model.Id).ToList();
            if (existingDetails.Count()>0)
            {
                db.NoticeDetail.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            if (model.NoticeLevel != NotificationLevel.All)
            {
                List<ENoticeDetails> detailLst = new List<ENoticeDetails>();
                for (int i = 0; i < targets.Count(); i++)
                {
                    ENoticeDetails detail = new ENoticeDetails();
                    detail.NoticeId = model.Id;
                    detail.TargetId = targets[i];
                    detailLst.Add(detail);
                }
                db.NoticeDetail.AddRange(detailLst);
                db.SaveChanges();
            }
            return new ServiceResult<ENotice>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(int id)
        {
            List<ENoticeDetails> existingDetails = db.NoticeDetail.Where(x => x.NoticeId == id).ToList();
            if (existingDetails.Count()>0)
            {
                db.NoticeDetail.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            ENotice notice = db.Notice.Where(x => x.Id == id).FirstOrDefault();
            db.Notice.Remove(notice);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ENotice> Publish(ENotice request)
        {
            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ENotice>()
            {
                Data = null,
                Status = ResultStatus.Ok,
                Message = "Approved Successfully"
            };
        }

        public ServiceResult<IQueryable<ENoticeDetails>> ListDetails()
        {
            return new ServiceResult<IQueryable<ENoticeDetails>>()
            {
                Data = db.NoticeDetail,
                Status = ResultStatus.Ok
            };
        }
    }
}
