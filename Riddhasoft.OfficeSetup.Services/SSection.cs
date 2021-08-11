using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SSection : Riddhasoft.Services.Common.IBaseService<ESection>
    {
        RiddhaDBContext db = null;
        public SSection()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ESection>> List()
        {
            return new ServiceResult<IQueryable<ESection>>()
            {
                Data = db.Section,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ESection> Add(ESection model)
        {
            db.Section.Add(model);
            db.SaveChanges();
            return new ServiceResult<ESection>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ESection> Update(ESection model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ESection>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ESection model)
        {
            //check existing employee
            if (model != null)
            {
                int existingUser = db.Employee.Where(x => x.SectionId == model.Id).Count();
                if (existingUser > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Status = ResultStatus.dataBaseError,
                        Message = string.Format("There are {0} numbers of Employee in this section. Section can't be deleted.", existingUser)
                    };
                }
                db.Section.Remove(model);
                db.SaveChanges();
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = "RemoveSuccess",
                    Status = ResultStatus.Ok
                };
            }
            return new ServiceResult<int>()
            {
                Data = 0,
                Message = "ProcessError",
                Status = ResultStatus.Ok
            };

        }

        public ServiceResult<List<ESection>> UploadExcel(List<ESection> SectionLst, int branchId)
        {
            List<ESection> SectionToSave = (from c in SectionLst
                                            where !(from o in db.Employee.Where(x => x.BranchId == branchId)
                                                    select o.Code.ToUpper()).Contains(c.Code.ToUpper())
                                            select c).ToList();
            if (SectionToSave.Count() > 0)
            {
                for (int i = 0; i < SectionToSave.Count(); i++)
                {
                    ESection section = new ESection();
                    section = SectionToSave[i];
                    db.Section.Add(section);
                    db.SaveChanges();
                }
                return new ServiceResult<List<ESection>>()
                {
                    Data = SectionToSave,
                    Message = "ImportSuccess",
                    Status = ResultStatus.Ok
                };
            }
            else
            {
                return new ServiceResult<List<ESection>>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = "DataAlreadyPulled"
                };
            }


        }
    }
}
