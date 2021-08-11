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
    public class SShift : Riddhasoft.Services.Common.IBaseService<EShift>
    {
        RiddhaDBContext db = null;
        public SShift()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EShift>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EShift>>()
            {
                Data = db.Shift,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EShift> Add(EShift model)
        {
            db.Shift.Add(model);
            db.SaveChanges();
            return new ServiceResult<EShift>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EShift> Update(EShift model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EShift>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EShift model)
        {
            try
            {
                int employeeCount = db.EmployeeShitList.Count(x => x.ShiftId == model.Id);
                if (employeeCount > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("There {1} {0} employee on this Shift. Shift Can't be deleted.", employeeCount, employeeCount == 1 ? "is" : "are"),
                        Status = ResultStatus.dataBaseError
                    };
                }
                model = db.Shift.Remove(db.Shift.Find(model.Id));
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
    }
}
