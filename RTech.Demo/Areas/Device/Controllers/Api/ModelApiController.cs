using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using Riddhasoft.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Device.Controllers.Api
{
    public class ModelApiController : ApiController
    {
        SModel modelServices = null;
        public ModelApiController()
        {
            modelServices = new SModel();
        }
        public ServiceResult<IQueryable<EModel>> Get()
        {
            return modelServices.List();
        }
        public ServiceResult<EModel> Get(int id)
        {
            EModel model = modelServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<EModel>()
            {
                Data = model,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public KendoGridResult<List<EModel>> GetKendoGrid(KendoPageListArguments arg)
        {
            SModel modelServices = new SModel();
            IQueryable<EModel> resellerQuery;
            resellerQuery = modelServices.List().Data;
            int totalRowNum = resellerQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<EModel> paginatedQuery;
            switch (searchField)
            {

                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id);
                    }
                    else
                    {
                        paginatedQuery = resellerQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id);
                    }
                    break;
                default:
                    paginatedQuery = resellerQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    break;
            }
            var list = (from c in paginatedQuery.ToList()
                        select new EModel()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            ImageURL = c.ImageURL == null ? "/Images/NoImage.png" : c.ImageURL,
                            IsAccessDevice = c.IsAccessDevice,
                            IsFaceDevice = c.IsFaceDevice,
                        }).ToList();
            return new KendoGridResult<List<EModel>>()
            {
                Data = list.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = list.Count()
            };
        }
        public ServiceResult<EModel> Post(EModel model)
        {
            model.Manufacture = Manufacture.ZKT;
            model.Brand = Brand.ZKT;
            return modelServices.Add(model);
        }
        public ServiceResult<EModel> Put(EModel model)
        {
            model.Manufacture = Manufacture.ZKT;
            model.Brand = Brand.ZKT;
            return modelServices.Update(model);
        }
        [HttpDelete]
        public ServiceResult<int> Delete(int id)
        {
            var model = modelServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return modelServices.Remove(model);
        }
    }
}

