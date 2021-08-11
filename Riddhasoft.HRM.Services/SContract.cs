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
    public class SContract
    {
        RiddhaDBContext db = null;
        public SContract()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EContract>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EContract>>()
            {
                Data = db.Contract,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EContract> Add(EContract model)
        {
            db.Contract.Add(model);
            db.SaveChanges();

            return new ServiceResult<EContract>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EContract> Update(EContract model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EContract>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EContract model)
        {
            db.Contract.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<UnapprovedCountModel> GetUnapprovedCount(int branchId)
        {
            UnapprovedCountModel model = new UnapprovedCountModel();
            model.Contract = db.Contract.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.Resignation = db.Resignation.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.Termination = db.Termination.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.EmployeeEducation = db.EmployeeEducation.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.EmployeeExperience = db.Experience.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.EmployeeLanguage = db.EmployeeLanguage.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.EmployeeLicense = db.EmployeeLicense.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.EmployeeMembership = db.Membership.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();
            model.EmployeeSkill = db.EmployeeSkills.Where(x => x.BranchId == branchId && x.IsApproved == false).Count();

            return new ServiceResult<UnapprovedCountModel>()
            {
                Data = model,
                Status = ResultStatus.Ok
            };
        }
    }
    public class UnapprovedCountModel
    {
        public int Contract { get; set; }
        public int Resignation { get; set; }
        public int Termination { get; set; }
        public int EmployeeEducation { get; set; }
        public int EmployeeSkill { get; set; }
        public int EmployeeExperience { get; set; }
        public int EmployeeLicense { get; set; }
        public int EmployeeMembership { get; set; }
        public int EmployeeLanguage { get; set; }
    }
}
