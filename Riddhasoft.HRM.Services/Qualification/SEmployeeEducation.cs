using Riddhasoft.DB;
using Riddhasoft.HRM.Entities.Qualification;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services.Qualification
{
    public class SEmployeeEducation : Riddhasoft.Services.Common.IBaseService<EEmployeeEducation>
    {
        RiddhaDBContext db = null;
        public SEmployeeEducation()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeEducation>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployeeEducation>>()
            {
                Data = db.EmployeeEducation,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeEducation> Add(EEmployeeEducation model)
        {
            db.EmployeeEducation.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeEducation>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployeeEducation> Update(EEmployeeEducation model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployeeEducation>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EEmployeeEducation model)
        {
            db.EmployeeEducation.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EmpQualificationReportModel> GetQualificationReport(int branchId)
        {
            EmpQualificationReportModel model = new EmpQualificationReportModel();
            model.Education = db.EmployeeEducation.Where(x => x.BranchId == branchId && x.IsApproved);
            model.Skill = db.EmployeeSkills.Where(x => x.BranchId == branchId && x.IsApproved);
            model.License = db.EmployeeLicense.Where(x => x.BranchId == branchId && x.IsApproved);
            model.Language = db.EmployeeLanguage.Where(x => x.BranchId == branchId && x.IsApproved);
            return new ServiceResult<EmpQualificationReportModel>()
            {
                Data = model,
                Status = ResultStatus.Ok
            };
        }
    }
    public class EmpQualificationReportModel
    {

        public IQueryable<EEmployeeEducation> Education { get; set; }
        public IQueryable<EEmployeeSkills> Skill { get; set; }
        public IQueryable<EEmployeeLicense> License { get; set; }
        public IQueryable<EEmployeeLanguage> Language { get; set; }

    }
}
