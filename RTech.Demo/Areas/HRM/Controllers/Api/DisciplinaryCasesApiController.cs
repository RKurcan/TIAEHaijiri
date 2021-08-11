using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Models;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class DisciplinaryCasesApiController : ApiController
    {
        SDisciplinaryCases _disciplinaryCasesServices = null;
        SEmployee _employeeServices = null;
        LocalizedString _loc = null;
        public DisciplinaryCasesApiController()
        {
            _disciplinaryCasesServices = new SDisciplinaryCases();
            _employeeServices = new SEmployee();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<DisciplinaryCasesGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var Lst = (from c in _disciplinaryCasesServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                       select new DisciplinaryCasesGridVm()
                       {
                           Id = c.Id,
                           CaseName = c.CaseName,
                           CreatedBy = c.CreatedBy,
                           CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd"),
                           Description = c.Description,
                           DisciplinaryActions = Enum.GetName(typeof(DisciplinaryActions), c.DisciplinaryActions),
                           DisciplinaryStatus = Enum.GetName(typeof(DisciplinaryStatus), c.DisciplinaryStatus),
                           ForwardToId = c.ForwardToId,
                       }).ToList();
            return new ServiceResult<List<DisciplinaryCasesGridVm>>()
            {
                Data = Lst,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<DisciplinaryCasesVmToSave> Get(int id)
        {
            string lang = RiddhaSession.Language;
            DisciplinaryCasesVmToSave vm = new DisciplinaryCasesVmToSave();
            vm.DisciplinaryCases = _disciplinaryCasesServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var disciplinaryCasesDetailLst = _disciplinaryCasesServices.ListDetail().Data.Where(x => x.DisciplinaryCasesId == vm.DisciplinaryCases.Id);
            if (lang == "ne")
            {
                vm.EmpLst = (from c in disciplinaryCasesDetailLst
                             select new DisciplinaryCasesEmpVm()
                             {
                                 Id = c.EmployeeId,
                                 Name = c.Employee.Code + " - " + (!string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.NameNp : c.Employee.Name) + " - " + (c.Employee.Mobile ?? "")
                             }).ToList();
            }
            else
            {
                vm.EmpLst = (from c in disciplinaryCasesDetailLst
                             select new DisciplinaryCasesEmpVm()
                             {
                                 Id = c.EmployeeId,
                                 Name = c.Employee.Code + " - " + c.Employee.Name + " - " + (c.Employee.Mobile ?? "")
                             }).ToList();
            }
            return new ServiceResult<DisciplinaryCasesVmToSave>
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<DisciplinaryCasesVmToSave> Post(DisciplinaryCasesVmToSave vm)
        {
            vm.DisciplinaryCases.BranchId = (int)RiddhaSession.BranchId;
            vm.DisciplinaryCases.CreatedBy = RiddhaSession.UserId;
            vm.DisciplinaryCases.CreatedOn = DateTime.Now;
            vm.DisciplinaryCases.CaseName = vm.DisciplinaryCases.CaseName.Trim();
            var result = _disciplinaryCasesServices.Add(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8012", "7164", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.DisciplinaryCases.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<DisciplinaryCasesVmToSave>()
            {
                Data = vm,
                Status = ResultStatus.Ok,
                Message = _loc.Localize("AddedSuccess")
            };
        }
        public ServiceResult<DisciplinaryCasesVmToSave> Put(DisciplinaryCasesVmToSave vm)
        {
            var result = _disciplinaryCasesServices.Update(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8012", "7165", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.DisciplinaryCases.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<DisciplinaryCasesVmToSave>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var disciplinaryCases = _disciplinaryCasesServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _disciplinaryCasesServices.Remove(disciplinaryCases);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8012", "7166", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }

        [HttpPost]
        public ServiceResult<List<EmployeeMultiSelectVm>> GetEmpLstForMultiSelect(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<EmployeeMultiSelectVm> resultLst = new List<EmployeeMultiSelectVm>();
            if (model != null)
            {
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                if (searchText == null)
                {
                    return new ServiceResult<List<EmployeeMultiSelectVm>>()
                    {
                        Data = resultLst,
                        Status = ResultStatus.Ok
                    };
                }
                var employeeLst = (from c in _employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                   select c
                    );

                if (searchText == "___")
                {
                    employeeLst = employeeLst.OrderBy(x => x.Name).Take(20);
                }
                else
                {
                    employeeLst = employeeLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower()));
                }
                if (employeeLst != null)
                {
                    resultLst = (from c in employeeLst
                                 select new EmployeeMultiSelectVm()
                                 {
                                     Id = c.Id,
                                     Name = c.Code + " - " + (c.Name) + " - " + (c.Mobile ?? ""),
                                 }).OrderBy(x => x.Name).ToList();
                }
            }
            return new ServiceResult<List<EmployeeMultiSelectVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpGet]
        public ServiceResult<List<DropdownViewModel>> GetFordwardToEmployee()
        {
            string language = RiddhaSession.Language.ToString();
            int? branchId = RiddhaSession.BranchId;
            List<DropdownViewModel> resultLst = (from c in _employeeServices.List().Data.Where(x => x.BranchId == branchId && x.IsManager == true && x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated).ToList()
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
    }
    public class DisciplinaryCasesGridVm
    {
        public int Id { get; set; }
        public string CaseName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string DisciplinaryStatus { get; set; }
        public string DisciplinaryActions { get; set; }
        public int ForwardToId { get; set; }
        public string ForwardToName { get; set; }
        public int BranchId { get; set; }
    }
    public class EmployeeMultiSelectVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
