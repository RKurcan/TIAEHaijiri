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
    public class SKaj
    {
        RiddhaDBContext db = null;
        public SKaj()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EKaj>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EKaj>>()
            {
                Data = db.Kaj,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<KajModel> Add(KajModel vm)
        {
            db.Kaj.Add(vm.Kaj);
            db.SaveChanges();
            List<EKajDetail> detailLst = new List<EKajDetail>();
            for (int i = 0; i < vm.EmpIds.Length; i++)
            {
                detailLst.Add(new EKajDetail() { KajId = vm.Kaj.Id, EmployeeId = vm.EmpIds[i] });
            }
            db.KajDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<KajModel>()
            {
                Data = vm,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<KajModel> Update(KajModel vm)
        {
            db.Entry(vm.Kaj).State = EntityState.Modified;
            db.SaveChanges();
            var existingDetails = db.KajDetail.Where(x => x.KajId == vm.Kaj.Id).ToList();
            if (existingDetails.Count() > 0)
            {
                db.KajDetail.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            List<EKajDetail> detailLst = new List<EKajDetail>();
            for (int i = 0; i < vm.EmpIds.Length; i++)
            {
                detailLst.Add(new EKajDetail() { KajId = vm.Kaj.Id, EmployeeId = vm.EmpIds[i] });
            }
            db.KajDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<KajModel>()
            {
                Data = vm,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EKaj model)
        {
            db.Kaj.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<EKajDetail>> ListDetail()
        {
            return new ServiceResult<IQueryable<EKajDetail>>()
            {
                Data = db.KajDetail,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EKaj> Approve(EKaj model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EKaj>()
            {
                Data = model,
                Message = "ApproveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<bool> Reject(EKaj model)
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
    public class KajModel
    {
        public EKaj Kaj { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int[] EmpIds { get; set; }
        public List<KajEmpVm> EmpLst { get; set; }

    }
    public class KajEmpVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
