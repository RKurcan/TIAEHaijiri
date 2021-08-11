using Riddhasoft.DB;
using Riddhasoft.Device.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Device.Services
{
    public class SModel : Riddhasoft.Services.Common.IBaseService<EModel>
    {
        RiddhaDBContext db = null;
        public SModel()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EModel>> List()
        {
            return new ServiceResult<IQueryable<EModel>>()
            {
                Data = db.Model,
                Message = "",
                Status = ResultStatus.Ok

            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EModel> Add(EModel model)
        {
            db.Model.Add(model);
            db.SaveChanges();
            return new ServiceResult<EModel>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EModel> Update(EModel model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EModel>()
            {
                Data = model,
                Message = "Updated Successfully",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EModel model)
        {
            try
            {
                int deviceCount = db.Device.Count(x => x.ModelId == model.Id);
                if (deviceCount > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("There {1} {0} Device on this Model. Model Can't be deleted.", deviceCount, deviceCount == 1 ? "is" : "are"),
                        Status = ResultStatus.dataBaseError
                    };
                }
                model = db.Model.Remove(db.Model.Find(model.Id));
                db.SaveChanges();
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = "Removed Successfully",
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
