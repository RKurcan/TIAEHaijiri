using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class SectionApiController : ApiController
    {
        SSection sectionServices = null;
        LocalizedString loc = null;
        public SectionApiController()
        {
            sectionServices = new SSection();
            loc = new LocalizedString();
        }
        [ActionFilter("1043")]
        public ServiceResult<List<SectionGridVm>> Get(int branchId)
        {
            SSection service = new SSection();
            List<ESection> list = new List<ESection>();
            if (branchId == 0)
            {
                var branch = new SBranch().List().Data.Where(x => x.Id == RiddhaSession.BranchId).FirstOrDefault();
                if (branch.IsHeadOffice)
                {
                    list = service.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).ToList();
                }
                else
                {
                    list = service.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
                }
            }
            else
            {
                list = service.List().Data.Where(x => x.BranchId == branchId).ToList();
            }
            var sectionLst = (from c in list
                              select new SectionGridVm()
                              {
                                  Id = c.Id,
                                  BranchId = c.BranchId,
                                  DepartmentId = c.DepartmentId,
                                  Code = c.Code,
                                  Name = c.Name,
                                  NameNp = c.NameNp
                              }).ToList();

            return new ServiceResult<List<SectionGridVm>>()
            {
                Data = sectionLst,
                Status = ResultStatus.Ok
            };
        }
        //public ServiceResult<ESection> Get(int id)
        //{
        //    ESection section = sectionServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
        //    return new ServiceResult<ESection>()
        //    {
        //        Data = section,
        //        Status = ResultStatus.Ok
        //    };
        //}
        public ServiceResult<List<SectionGridVm>> GetSectionsByDepartment(string id)
        {
            string[] departments = id.Split(',');
            List<SectionGridVm> sections = (from c in sectionServices.List().Data.ToList()
                                            where c.BranchId == RiddhaSession.BranchId
                                            join d in departments on c.DepartmentId equals int.Parse(d)
                                            select new SectionGridVm
                                            {
                                                Id = c.Id,
                                                Code = c.Code,
                                                Name = c.Name,
                                                NameNp = c.NameNp,
                                                BranchId = c.BranchId,
                                                DepartmentId = c.DepartmentId,

                                            }).ToList();
            return new ServiceResult<List<SectionGridVm>>()
            {
                Data = sections,
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("1022")]
        public ServiceResult<ESection> Post(ESection model)
        {
            if (model.BranchId == 0)
            {
                model.BranchId = RiddhaSession.BranchId;
            }
            //model.BranchId = RiddhaSession.BranchId;
            var result = sectionServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1006", "1022", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ESection>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [ActionFilter("1023")]
        public ServiceResult<ESection> Put(ESection model)
        {
            model.BranchId = RiddhaSession.BranchId;
            var result = sectionServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1006", "1023", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ESection>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }
        [HttpDelete, ActionFilter("1024")]
        public ServiceResult<int> Delete(int id)
        {
            var section = sectionServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = sectionServices.Remove(section);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("1006", "1024", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }
        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetDepartmentsForDropdown(int branchId)
        {
            string language = RiddhaSession.Language.ToString();
            SDepartment departmentServices = new SDepartment();
            List<EDepartment> list = new List<EDepartment>();
            var branch = new SBranch().List().Data.Where(x => x.Id == RiddhaSession.BranchId).FirstOrDefault();
            if (branchId == 0)
            {
                if (branch.IsHeadOffice)
                {
                    list = departmentServices.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId).ToList();
                }
                else
                {
                    list = departmentServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
                }
            }
            else
            {
                list = departmentServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            }
            List<DropdownViewModel> resultLst = (from c in list.ToList()
                                                 select new DropdownViewModel()
                                                 {
                                                     Id = c.Id,
                                                     Name = language == "ne" &&
                                                     c.NameNp != null ? c.NameNp : c.Name
                                                 }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost, ActionFilter("1043")]
        public KendoGridResult<List<SectionGridVm>> GetSectionKendoGrid(KendoPageListArguments arg)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            SSection sectionServices = new SSection();
            IQueryable<ESection> sectionQuery;
            if (arg.BranchId == 0)
            {
                var branch = new SBranch().List().Data.Where(x => x.Id == RiddhaSession.BranchId).FirstOrDefault();
                if (branch.IsHeadOffice)
                {
                    sectionQuery = sectionServices.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId);
                }
                else
                {
                    sectionQuery = sectionServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId);
                }
            }
            else
            {
                sectionQuery = sectionServices.List().Data.Where(x => x.BranchId == arg.BranchId);
            }
            int totalRowNum = sectionQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<ESection> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = sectionQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    else
                    {
                        paginatedQuery = sectionQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Name);
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = sectionQuery.Where(x => x.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = sectionQuery.Where(x => x.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                case "DepartmentName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = sectionQuery.Where(x => x.Department.Name.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    else
                    {
                        paginatedQuery = sectionQuery.Where(x => x.Department.Name == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Code);
                    }
                    break;
                default:
                    paginatedQuery = sectionQuery.OrderByDescending(x => x.Id).ThenBy(x => x.Name);

                    break;
            }
            var sectionlist = (from c in paginatedQuery
                               select new SectionGridVm()
                               {
                                   Id = c.Id,
                                   Code = c.Code,
                                   Name = c.Name,
                                   NameNp = c.NameNp,
                                   BranchId = c.BranchId,
                                   DepartmentId = c.DepartmentId,
                                   DepartmentName = c.Department == null ? "" : ((!string.IsNullOrEmpty(c.Department.NameNp) && language == "ne") ? c.Department.NameNp : c.Department.Name),
                                   BranchName = c.Branch.Name,
                               }).ToList();
            return new KendoGridResult<List<SectionGridVm>>()
            {
                Data = sectionlist.Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = sectionlist.Count()
            };
        }
        private bool checkValidString(string value)
        {
            return !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
        }
        private int getDepartmentId(IQueryable<EDepartment> departmentQuery, string code, int branchId)
        {
            if (string.IsNullOrEmpty(code))
            {
                return 0;
            }
            var department = departmentQuery.Where(x => x.BranchId == branchId && x.Code.ToUpper() == code.Trim().ToUpper()).FirstOrDefault();
            if (department == null)
            {
                return 0;
            }
            else
            {
                return department.Id;
            }
        }
        [HttpPost]
        public ServiceResult<List<ESection>> Upload()
        {
            var branch = RiddhaSession.CurrentUser.Branch;
            var request = HttpContext.Current.Request;
            List<ESection> SectionLst = new List<ESection>();
            using (var package = new OfficeOpenXml.ExcelPackage(request.InputStream))
            {
                IQueryable<EDepartment> departmentQuery = new SDepartment().List().Data;
                TimeSpan defaultTime = "00:00".ToTimeSpan();
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    ESection model = new ESection();
                    model.BranchId = branch.Id;
                    model.Code = (workSheet.Cells[rowIterator, 1].Value ?? string.Empty).ToString();
                    if (checkValidString(model.Code) == false)
                    {
                        continue;
                    }
                    model.Name = (workSheet.Cells[rowIterator, 2].Value ?? string.Empty).ToString();
                    if (checkValidString(model.Name) == false)
                    {
                        continue;
                    }
                    model.NameNp = (workSheet.Cells[rowIterator, 3].Value ?? string.Empty).ToString();
                    model.DepartmentId = getDepartmentId(departmentQuery, (workSheet.Cells[rowIterator, 4].Value ?? string.Empty).ToString(), branch.Id);


                    SectionLst.Add(model);
                }
                var uniqueLst = SectionLst.GroupBy(x => x.Code)
              .Where(g => g.Count() == 1)
              .Select(y => new { Element = y.Key, Counter = y.Count() })
              .ToList();
                var listToSave = (from c in SectionLst
                                  join d in uniqueLst on c.Code equals d.Element
                                  select c).ToList();
                var result = new ServiceResult<List<ESection>>();
                if (listToSave.Count() > 0)
                {
                    result = sectionServices.UploadExcel(listToSave, branch.Id);
                }
                if (uniqueLst.Count() != SectionLst.Count())
                {
                    return new ServiceResult<List<ESection>>()
                    {
                        Data = listToSave,
                        Message = listToSave.Count().ToString() + " out of " + SectionLst.Count().ToString() + " Saved Successfully",
                        Status = ResultStatus.Ok
                    };
                }
                return new ServiceResult<List<ESection>>()
                {
                    Data = result.Data,
                    Status = result.Status,
                    Message = loc.Localize(result.Message)
                };
            }
        }
    }
    public class SectionGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public int DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameNp { get; set; }
    }
}
