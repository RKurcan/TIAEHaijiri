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
    public class SEvent 
    {
        RiddhaDBContext db = null;
        public SEvent()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEvent>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEvent>>()
            {
                Data = db.Event,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEvent> Add(EEvent model, int[] targets)
        {
            db.Event.Add(model);
            db.SaveChanges();
            if (model.EventLevel != NotificationLevel.All)
            {
                List<EEventDetails> detailLst = new List<EEventDetails>();
                for (int i = 0; i < targets.Count(); i++)
                {
                    EEventDetails detail = new EEventDetails();
                    detail.EventId = model.Id;
                    detail.TargetId = targets[i];
                    detailLst.Add(detail);
                }
                db.EventDetail.AddRange(detailLst);
                db.SaveChanges();

            }
            return new ServiceResult<EEvent>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEvent> Update(EEvent model,int[] targets)
        {
            EEvent evnt= db.Event.Where(x=>x.Id==model.Id).FirstOrDefault();
            evnt.CreatedOn = model.CreatedOn;
            evnt.Description=model.Description;
            evnt.CreatedById = model.CreatedById;
            evnt.EventLevel = model.EventLevel;
            evnt.Title = model.Title;
            evnt.From = model.From;
            evnt.To = model.To;
            db.Entry(evnt).State = EntityState.Modified;
            db.SaveChanges();

            List<EEventDetails> exestingDetails = db.EventDetail.Where(x =>x.EventId == model.Id).ToList();
            if (exestingDetails.Count()>0)
            {
                db.EventDetail.RemoveRange(exestingDetails);
                db.SaveChanges();
            }
            if (model.EventLevel!=NotificationLevel.All)
            {
                List<EEventDetails> detailLst = new List<EEventDetails>();
                for (int i = 0; i < targets.Count(); i++)
                {
                    EEventDetails detail = new EEventDetails();
                    detail.EventId = model.Id;
                    detail.TargetId = targets[i];
                    detailLst.Add(detail);
                }
                db.EventDetail.AddRange(detailLst);
                db.SaveChanges();
                
            }
            return new ServiceResult<EEvent>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };

        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(int id)
        {
            List<EEventDetails> existingDetails = db.EventDetail.Where(x => x.EventId == id).ToList();
            if (existingDetails.Count() > 0)
            {
                db.EventDetail.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            EEvent evnt = db.Event.Where(x => x.Id == id).FirstOrDefault();
            db.Event.Remove(evnt);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }


        public ServiceResult<IQueryable<EEventDetails>> ListDetails()
        {
            return new ServiceResult<IQueryable<EEventDetails>>()
            {
                Data = db.EventDetail,
                Status = ResultStatus.Ok
            };
        }

    }
}
