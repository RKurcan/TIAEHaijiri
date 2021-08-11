using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Office.Controllers.Api
{
    public class LeaveMasterApiController : ApiController
    {
        SLeaveMaster leaveMasterServices = null;
        LocalizedString loc = null;
        int branchId = (int)RiddhaSession.BranchId;
        public LeaveMasterApiController()
        {
            leaveMasterServices = new SLeaveMaster();
            loc = new LocalizedString();

        }
        [ActionFilter("3025")]
        public ServiceResult<List<LeaveMasterGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            SLeaveMaster service = new SLeaveMaster();
            var leaveMastertLst = (from c in service.List().Data.Where(x => x.BranchId == branchId).ToList()
                                   select new LeaveMasterGridVm()
                                   {
                                       Id = c.Id,
                                       BranchId = c.BranchId,
                                       Code = c.Code,
                                       Name = c.Name,
                                       NameNp = c.NameNp,
                                       Description = c.Description,
                                       Balance = c.Balance,
                                       ApplicableGender = c.ApplicableGender,
                                       IsPaidLeave = c.IsPaidLeave,
                                       IsLeaveCarryable = c.IsLeaveCarryable,
                                       CreatedOn = c.CreatedOn,
                                       IsReplacementLeave = c.IsReplacementLeave,
                                       MaximumLeaveBalance = c.MaximumLeaveBalance,
                                   }).ToList();

            return new ServiceResult<List<LeaveMasterGridVm>>()
            {
                Data = leaveMastertLst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ELeaveMaster> Get(int id)
        {
            ELeaveMaster leaveMaster = leaveMasterServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            return new ServiceResult<ELeaveMaster>()
            {
                Data = leaveMaster,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        [ActionFilter("3007")]
        public ServiceResult<ELeaveMaster> Post(ELeaveMaster model)
        {
            model.BranchId = RiddhaSession.BranchId;
            model.CompanyId = RiddhaSession.CompanyId;
            model.CreatedOn = System.DateTime.Now;
            bool isValid = validateLeaveMasterCode(model.Code);
            if (isValid == false)
            {
                return new ServiceResult<ELeaveMaster>()
                {
                    Data = null,
                    Message = "Leave master code is duplicate..",
                    Status = ResultStatus.processError
                };
            }
            if (model.IsReplacementLeave == true)
            {
                ELeaveMaster checkedLeaveMaster = leaveMasterServices.List().Data.Where(x => x.IsReplacementLeave == true && x.BranchId == model.BranchId).FirstOrDefault();
                if (checkedLeaveMaster != null)
                {
                    checkedLeaveMaster.IsReplacementLeave = false;
                    var rs = leaveMasterServices.Update(checkedLeaveMaster);
                }
            }
            var result = leaveMasterServices.Add(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("3001", "3007", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ELeaveMaster>()
            {
                Data = result.Data,
                Status = result.Status,
                Message = loc.Localize(result.Message)
            };
        }

        private bool validateLeaveMasterCode(string code)
        {
            leaveMasterServices = new SLeaveMaster();
            var leave = leaveMasterServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.Code.ToLower().Trim() == code.ToLower().Trim()).FirstOrDefault();
            return leave == null ? true : false;
        }

        [ActionFilter("3008")]
        public ServiceResult<ELeaveMaster> Put(ELeaveMaster model)
        {
            model.CompanyId = RiddhaSession.CompanyId;
            var result = leaveMasterServices.Update(model);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("3001", "3008", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, model.Id, loc.Localize(result.Message));
            }
            return new ServiceResult<ELeaveMaster>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status

            };
        }

        [HttpDelete, ActionFilter("3009")]
        public ServiceResult<int> Delete(int id)
        {
            var leaveMaster = leaveMasterServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = leaveMasterServices.Remove(leaveMaster);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("3001", "3009", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpGet, ActionFilter("3006")]
        public ServiceResult<List<ELeaveMaster>> PullLeaveMaster()
        {
            var branch = RiddhaSession.CurrentUser.Branch;
            var rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~/files");
            string path = rootPath + "/Hamrohajiri_Leave.xlsx";
            FileStream stream = new FileStream(path, FileMode.Open);


            List<ELeaveMaster> leavelst = new List<ELeaveMaster>();
            using (var package = new OfficeOpenXml.ExcelPackage(stream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    ELeaveMaster model = new ELeaveMaster();
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
                    model.NameNp = workSheet.Cells[rowIterator, 3].Value.ToString();
                    model.Description = workSheet.Cells[rowIterator, 4].Value.ToString();
                    model.Balance = workSheet.Cells[rowIterator, 5].Value.ToDecimal();
                    model.IsPaidLeave = workSheet.Cells[rowIterator, 6].Value.ToString() == "1" ? true : false;
                    model.IsLeaveCarryable = workSheet.Cells[rowIterator, 7].Value.ToString() == "1" ? true : false;
                    model.ApplicableGender = (ApplicableGender)workSheet.Cells[rowIterator, 8].Value.ToInt();
                    model.BranchId = branch.Id;
                    model.CreatedOn = DateTime.Now;
                    leavelst.Add(model);
                }

                var result = leaveMasterServices.PullLeaveMaster(leavelst, branch.Id);
                if (result.Status == ResultStatus.Ok)
                {
                    Common.AddAuditTrail("3001", "3006", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 1, loc.Localize(result.Message));
                }
                stream.Close();
                stream.Dispose();
                return new ServiceResult<List<ELeaveMaster>>()
                {
                    Data = result.Data,
                    Status = result.Status,
                    Message = loc.Localize(result.Message)
                };
            }
        }

        private bool checkValidString(string value)
        {
            return !(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
        }

        [HttpPost, ActionFilter("3025")]
        public KendoGridResult<List<LeaveMasterGridVm>> GetLeaveMasterKendoGrid(KendoPageListArguments arg)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            SLeaveMaster service = new SLeaveMaster();
            IQueryable<ELeaveMaster> leaveMasterQuery;
            leaveMasterQuery = service.List().Data.Where(x => x.Branch.CompanyId == RiddhaSession.CompanyId);
            int totalRowNum = leaveMasterQuery.Count();
            string searchField = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Field;
            string searchOp = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Operator;
            string searchValue = arg.Filter.Filters.Count() <= 0 ? "" : arg.Filter.Filters[0].Value;
            IQueryable<ELeaveMaster> paginatedQuery;
            switch (searchField)
            {
                case "Code":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = leaveMasterQuery.Where(x => x.Code.StartsWith(searchValue.Trim()));
                    }
                    else
                    {
                        paginatedQuery = leaveMasterQuery.Where(x => x.Code == searchValue.Trim());
                    }
                    break;
                case "Name":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = leaveMasterQuery.Where(x => x.Name.StartsWith(searchValue.Trim()));
                    }
                    else
                    {
                        paginatedQuery = leaveMasterQuery.Where(x => x.Name == searchValue.Trim());
                    }
                    break;
                default:
                    paginatedQuery = leaveMasterQuery;
                    break;
            }

            var leaveMasterlist = (from c in paginatedQuery
                                   select new LeaveMasterGridVm()
                                   {
                                       Id = c.Id,
                                       BranchId = c.BranchId,
                                       Code = c.Code,
                                       Name = c.Name,
                                       NameNp = c.NameNp,
                                       Description = c.Description,
                                       Balance = c.Balance,
                                       ApplicableGender = c.ApplicableGender,
                                       IsPaidLeave = c.IsPaidLeave,
                                       IsLeaveCarryable = c.IsLeaveCarryable,
                                       CreatedOn = c.CreatedOn,
                                       IsReplacementLeave = c.IsReplacementLeave,
                                       MaximumLeaveBalance = c.MaximumLeaveBalance,
                                   }).ToList();
            return new KendoGridResult<List<LeaveMasterGridVm>>()
            {
                Data = leaveMasterlist.OrderByDescending(x => x.Id).Skip(arg.Skip).Take(arg.Take).ToList(),
                Status = ResultStatus.Ok,
                TotalCount = leaveMasterlist.Count(),
            };
        }


        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetLeaveMaster()
        {
            var result = (from c in leaveMasterServices.List().Data.Where(x => x.BranchId == branchId)
                          select new DropdownViewModel()
                          {
                              Id = c.Id,
                              Name = c.Name,
                          }).ToList();
            return new ServiceResult<List<DropdownViewModel>>()
            {
                Data = result,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }

    public class LeaveMasterGridVm
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public int? BranchId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsReplacementLeave { get; set; }
        public decimal MaximumLeaveBalance { get; set; }
    }
}