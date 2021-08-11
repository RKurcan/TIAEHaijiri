using Riddhasoft.DB;
using Riddhasoft.HRM.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services
{
    public class SDisciplinaryCases
    {
        RiddhaDBContext db = null;
        public SDisciplinaryCases()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EDisciplinaryCases>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EDisciplinaryCases>>()
            {
                Data = db.DisciplinaryCases,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<DisciplinaryCasesVmToSave> Add(DisciplinaryCasesVmToSave vm)
        {
            db.DisciplinaryCases.Add(vm.DisciplinaryCases);
            db.SaveChanges();
            List<EDisciplinaryCasesDetail> detailLst = new List<EDisciplinaryCasesDetail>();
            for (int i = 0; i < vm.EmpIds.Length; i++)
            {
                detailLst.Add(new EDisciplinaryCasesDetail() { DisciplinaryCasesId = vm.DisciplinaryCases.Id, EmployeeId = vm.EmpIds[i] });
            }
            db.DisciplinaryCasesDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<DisciplinaryCasesVmToSave>()
            {
                Data = vm,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<DisciplinaryCasesVmToSave> Update(DisciplinaryCasesVmToSave vm)
        {
            db.Entry(vm.DisciplinaryCases).State = EntityState.Modified;
            db.SaveChanges();
            var existingDetails = db.DisciplinaryCasesDetail.Where(x => x.DisciplinaryCasesId == vm.DisciplinaryCases.Id).ToList();
            if (existingDetails.Count() > 0)
            {
                db.DisciplinaryCasesDetail.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            List<EDisciplinaryCasesDetail> detailLst = new List<EDisciplinaryCasesDetail>();
            for (int i = 0; i < vm.EmpIds.Length; i++)
            {
                detailLst.Add(new EDisciplinaryCasesDetail() { DisciplinaryCasesId = vm.DisciplinaryCases.Id, EmployeeId = vm.EmpIds[i] });
            }
            db.DisciplinaryCasesDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<DisciplinaryCasesVmToSave>()
            {
                Data = vm,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EDisciplinaryCases model)
        {
            db.DisciplinaryCases.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<EDisciplinaryCasesDetail>> ListDetail()
        {
            return new ServiceResult<IQueryable<EDisciplinaryCasesDetail>>()
            {
                Data = db.DisciplinaryCasesDetail,
                Status = ResultStatus.Ok
            };
        }
    }

    public class DisciplinaryCasesVmToSave
    {
        public EDisciplinaryCases DisciplinaryCases { get; set; }
        public int[] EmpIds { get; set; }
        public List<DisciplinaryCasesEmpVm> EmpLst { get; set; }
    }
    public class DisciplinaryCasesEmpVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
