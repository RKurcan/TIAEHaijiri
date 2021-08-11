using Riddhasoft.DB;
using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services.Training
{
    public class SParticipant
    {
        RiddhaDBContext db = null;
        public SParticipant()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EParticipant>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EParticipant>>()
            {
                Data = db.Participant,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<ParticipantVmToSave> Add(ParticipantVmToSave vm)
        {
            db.Participant.Add(vm.Participant);
            db.SaveChanges();
            List<EParticipantDetail> detailLst = new List<EParticipantDetail>();
            for (int i = 0; i < vm.EmpIds.Length; i++)
            {
                detailLst.Add(new EParticipantDetail() { ParticipantId = vm.Participant.Id, EmployeeId = vm.EmpIds[i] });
            }
            db.ParticipantDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<ParticipantVmToSave>()
            {
                Data = vm,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<ParticipantVmToSave> Update(ParticipantVmToSave vm)
        {
            db.Entry(vm.Participant).State = EntityState.Modified;
            db.SaveChanges();
            var existingDetails = db.ParticipantDetail.Where(x => x.ParticipantId == vm.Participant.Id).ToList();
            if (existingDetails.Count() > 0)
            {
                db.ParticipantDetail.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            List<EParticipantDetail> detailLst = new List<EParticipantDetail>();
            for (int i = 0; i < vm.EmpIds.Length; i++)
            {
                detailLst.Add(new EParticipantDetail() { ParticipantId = vm.Participant.Id, EmployeeId = vm.EmpIds[i] });
            }
            db.ParticipantDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<ParticipantVmToSave>()
            {
                Data = vm,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EParticipant model)
        {
            db.Participant.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<EParticipantDetail>> ListDetail()
        {
            return new ServiceResult<IQueryable<EParticipantDetail>>()
            {
                Data = db.ParticipantDetail,
                Status = ResultStatus.Ok
            };

        }
    }
    public class ParticipantVmToSave
    {
        public EParticipant Participant { get; set; }
        public int[] EmpIds { get; set; }
        public List<ParticipantEmpVm> EmpLst { get; set; }
    }
    public class ParticipantEmpVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
