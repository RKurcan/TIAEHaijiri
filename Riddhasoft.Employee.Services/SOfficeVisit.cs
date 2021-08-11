using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.Employee.Services
{
    public class SOfficeVisit
    {
        RiddhaDBContext db = null;
        public SOfficeVisit()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EOfficeVisit>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EOfficeVisit>>()
            {
                Data = db.OfficeVisit,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<OfficeVisitModel> Add(OfficeVisitModel vm)
        {
            db.OfficeVisit.Add(vm.OfficeVisit);
            db.SaveChanges();
            List<EOfficeVisitDetail> detailLst = new List<EOfficeVisitDetail>();
            for (int i = 0; i < vm.EmpIds.Length; i++)
            {
                detailLst.Add(new EOfficeVisitDetail() { OfficeVisitId = vm.OfficeVisit.Id, EmployeeId = vm.EmpIds[i] });
            }
            db.OfficeVisitDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<OfficeVisitModel>()
            {
                Data = vm,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EOfficeVisit> Add(EOfficeVisit model)
        {
            db.OfficeVisit.Add(model);
            db.SaveChanges();
            return new ServiceResult<EOfficeVisit>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<EOfficeVisitDetail> AddDetail(EOfficeVisitDetail model)
        {
            db.OfficeVisitDetail.Add(model);
            db.SaveChanges();
            return new ServiceResult<EOfficeVisitDetail>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<OfficeVisitModel> Update(OfficeVisitModel vm)
        {
            db.Entry(vm.OfficeVisit).State = EntityState.Modified;
            db.SaveChanges();
            var existingDetails = db.OfficeVisitDetail.Where(x => x.OfficeVisitId == vm.OfficeVisit.Id).ToList();
            if (existingDetails.Count() > 0)
            {
                db.OfficeVisitDetail.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            List<EOfficeVisitDetail> detailLst = new List<EOfficeVisitDetail>();
            for (int i = 0; i < vm.EmpIds.Length; i++)
            {
                detailLst.Add(new EOfficeVisitDetail() { OfficeVisitId = vm.OfficeVisit.Id, EmployeeId = vm.EmpIds[i] });
            }
            db.OfficeVisitDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<OfficeVisitModel>()
            {
                Data = vm,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EOfficeVisit model)
        {
            db.OfficeVisit.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EOfficeVisitDetail>> ListDetail()
        {
            return new ServiceResult<IQueryable<EOfficeVisitDetail>>()
            {
                Data = db.OfficeVisitDetail,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<bool> Approve(EOfficeVisit model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Message = "ApproveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<bool> Reject(EOfficeVisit model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Message = "RejectSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
    public class OfficeVisitModel
    {
        public EOfficeVisit OfficeVisit { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int[] EmpIds { get; set; }
        public List<OfficeVisitEmpVm> EmpLst { get; set; }
    }
    public class OfficeVisitEmpVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
