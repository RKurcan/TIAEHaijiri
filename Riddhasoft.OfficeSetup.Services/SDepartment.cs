using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SDepartment : Riddhasoft.Services.Common.IBaseService<EDepartment>
    {
        RiddhaDBContext db = null;
        public SDepartment()
        {
            db = new DB.RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EDepartment>> List()
        {
            return new ServiceResult<IQueryable<EDepartment>>()
            {
                Data = db.Department,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDepartment> Add(EDepartment model)
        {
            db.Department.Add(model);
            db.SaveChanges();
            db.Section.Add(new ESection()
            {
                BranchId = model.BranchId,
                Code = "S" + model.Code,
                Name = "Section For " + model.Name,
                DepartmentId = model.Id,
            });
            db.SaveChanges();
            return new ServiceResult<EDepartment>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EDepartment> Update(EDepartment model)
        {
            db.Entry<EDepartment>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EDepartment>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EDepartment model)
        {
            try
            {
                var section = db.Section.Where(x => x.DepartmentId == model.Id).ToList();
                var sectionCount = section.Count();
                if (sectionCount > 0)
                {
                    var defaultSection = section.Where(x => x.Name.Contains("Section For ")).FirstOrDefault();
                    if (defaultSection != null && section.Count == 1)
                    {
                        var EmployeeCount = db.Employee.Count(x => x.SectionId == defaultSection.Id);
                        if (EmployeeCount > 0)
                        {
                            return new ServiceResult<int>()
                            {
                                Data = 1,
                                Message = string.Format("There {1} {0} section on this department. Department Can't be deleted.", sectionCount, sectionCount == 1 ? "is" : "are"),
                                Status = ResultStatus.dataBaseError
                            };
                        }

                    }
                    else
                    {
                        return new ServiceResult<int>()
                        {
                            Data = 1,
                            Message = string.Format("There {1} {0} section on this department. Department Can't be deleted.", sectionCount, sectionCount == 1 ? "is" : "are"),
                            Status = ResultStatus.dataBaseError
                        };
                    }
                }
                model = db.Department.Remove(db.Department.Find(model.Id));
                db.SaveChanges();
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = "RemoveSuccess",
                    Status = ResultStatus.Ok
                };
            }
            catch (SqlException ex)
            {

                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.dataBaseError
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.unHandeledError
                };
            }
        }
        public ServiceResult<List<EDepartment>> UploadExcel(List<EDepartment> DepLst, int branchId)
        {
            List<EDepartment> DepToSave = (from c in DepLst
                                           where !(from o in db.Department.Where(x => x.BranchId == branchId)
                                                   select o.Code.ToUpper()).Contains(c.Code.ToUpper())
                                           select c).ToList();
            if (DepToSave.Count() > 0)
            {
                for (int i = 0; i < DepToSave.Count(); i++)
                {
                    EDepartment dep = new EDepartment();
                    dep = DepToSave[i];
                    db.Department.Add(dep);
                    db.SaveChanges();
                }
                return new ServiceResult<List<EDepartment>>()
                {
                    Data = DepToSave,
                    Message = "ImportSuccess",
                    Status = ResultStatus.Ok
                };
            }
            else
            {
                return new ServiceResult<List<EDepartment>>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = "DataAlreadyPulled"
                };
            }

        }
    }
}
