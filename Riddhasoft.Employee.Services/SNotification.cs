using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riddhasoft.DB;
using Riddhasoft.Services.Common;
using Riddhasoft.Employee.Entities;

namespace Riddhasoft.Employee.Services
{
    public class SNotification
    {
        RiddhaDBContext db = new RiddhaDBContext();

        public ServiceResult<IQueryable<ENotification>> List()
        {
            return new ServiceResult<IQueryable<ENotification>>()
            {
                Data = db.Notification,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<ENotificationDetail>> ListDetails()
        {
            return new ServiceResult<IQueryable<ENotificationDetail>>()
            {
                Data = db.NotificationDetail,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ENotification> Find(int id)
        {
            ENotification model = db.Notification.Find(id);
            return new ServiceResult<ENotification>()
            {
                Data = model,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ENotification> Add(ENotification model, int[] targets)
        {
            db.Notification.Add(model);
            db.SaveChanges();
            if (model.NotificationLevel != NotificationLevel.All && targets.Count() > 0)
            {
                List<ENotificationDetail> detailLst = new List<ENotificationDetail>();
                for (int i = 0; i < targets.Count(); i++)
                {
                    ENotificationDetail detail = new ENotificationDetail();
                    detail.NotificationId = model.Id;
                    detail.TargetId = targets[i];
                    detailLst.Add(detail);
                }
                db.NotificationDetail.AddRange(detailLst);
                db.SaveChanges();
            }
            return new ServiceResult<ENotification>()
            {
                Data = model,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ENotification> Update(ENotification model, int[] targets)
        {
            ENotification existingNotification = db.Notification.Where(x => x.TypeId == model.TypeId).FirstOrDefault();
            if (existingNotification != null)
            {
                List<ENotificationDetail> existingDetails = db.NotificationDetail.Where(x => x.NotificationId == existingNotification.Id).ToList();
                if (existingDetails.Count() > 0)
                {
                    db.NotificationDetail.RemoveRange(existingDetails);
                    db.SaveChanges();
                }
                db.Notification.Remove(existingNotification);
                db.SaveChanges();
            }
            db.Notification.Add(model);
            db.SaveChanges();
            if (model.NotificationLevel != NotificationLevel.All && targets.Count() > 0)
            {
                List<ENotificationDetail> detailLst = new List<ENotificationDetail>();
                for (int i = 0; i < targets.Count(); i++)
                {
                    ENotificationDetail detail = new ENotificationDetail();
                    detail.NotificationId = model.Id;
                    detail.TargetId = targets[i];
                    detailLst.Add(detail);
                }
                db.NotificationDetail.AddRange(detailLst);
                db.SaveChanges();
            }
            return new ServiceResult<ENotification>()
            {
                Data = model,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<int> Remove(int typeId,NotificationType type)
        {
            ENotification notification = db.Notification.Where(x => x.TypeId == typeId && x.NotificationType==type).FirstOrDefault();
            if (notification != null)
            {
                List<ENotificationDetail> detailLst = db.NotificationDetail.Where(x => x.Id == notification.Id).ToList();
                if (detailLst.Count() > 0)
                {
                    db.NotificationDetail.RemoveRange(detailLst);
                    db.SaveChanges();
                }
                db.Notification.Remove(notification);
                db.SaveChanges();
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<List<ENotification>> AddRange(List<ENotification> lst)
        {
            int typeId = lst[0].TypeId;
            var existingNotifications = db.Notification.Where(x => x.TypeId ==typeId).ToList();
            if (existingNotifications.Count()>0)
            {
                db.Notification.RemoveRange(existingNotifications);
                db.SaveChanges();
            }
            db.Notification.AddRange(lst);
            db.SaveChanges();
            return new ServiceResult<List<ENotification>>()
            {
                Data = lst,
                Status = ResultStatus.Ok
            };
        }

        

    }
}
