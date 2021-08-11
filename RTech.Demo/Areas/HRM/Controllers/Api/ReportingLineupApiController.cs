using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class ReportingLineupApiController : ApiController
    {
        SEmployee _employeeServices = null;
        int branchId = (int)RiddhaSession.BranchId;
        public ReportingLineupApiController()
        {
            _employeeServices = new SEmployee();
        }
        [ActionFilter("7255")]
        public void Save(ReportingLineupArgument args)
        {
            int[] EmpIds = { };
            if (args.EmpIds != null)
                EmpIds = args.EmpIds.Split(',').Select(x => int.Parse(x)).ToArray();
            else
                EmpIds = (from c in _employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                          select c.Id).ToArray();
            for (int i = 0; i < EmpIds.Length; i++)
            {
                int empId = EmpIds[i];
                if (empId != args.ManagerId)
                {
                    var data = _employeeServices.List().Data.Where(x => x.Id == empId).FirstOrDefault();
                    data.ReportingManagerId = args.ManagerId;
                    _employeeServices.Update(data);
                }
            }
        }
        [ActionFilter("7233"), HttpPost]
        public KendoGridResult<List<ReportingLineupGridVm>> GetEmpKendoGrid(HREmployeeGridModel vm)
        {
            string language = RiddhaSession.Language.ToString();
            var branchId = RiddhaSession.BranchId;
            SEmployee service = new SEmployee();
            IQueryable<EEmployee> empQuery;
            empQuery = Common.GetEmployees().Data.AsQueryable();
            int totalRowNum = empQuery.Count();
            string searchField = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Field;
            string searchOp = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Operator;
            string searchValue = vm.Filter.Filters.Count() <= 0 ? "" : vm.Filter.Filters[0].Value;
            IQueryable<EEmployee> paginatedQuery;
            switch (searchField)
            {
                case "IdCardNo":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Code.StartsWith(searchValue.Trim())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Code == searchValue.Trim()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "EmployeeName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "DepartmentName":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "Section":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "Mobile":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Mobile.StartsWith(searchValue)).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Mobile == searchValue).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                case "Department":
                    if (searchOp == "startswith")
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower().StartsWith(searchValue.Trim().ToLower())).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    else
                    {
                        paginatedQuery = empQuery.Where(x => x.Section.Department.Name.ToLower() == searchValue.Trim().ToLower()).OrderByDescending(x => x.Id).ThenBy(x => x.Designation.DesignationLevel).Skip(vm.Skip).Take(vm.Take);
                    }
                    break;
                default:
                    paginatedQuery = empQuery;
                    break;
            }
            var list = _employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            var employeelist = (from c in paginatedQuery.ToList()
                                join d in list on c.ReportingManagerId equals d.Id
                                into joined
                                from j in joined.DefaultIfEmpty(new EEmployee())
                                select new ReportingLineupGridVm()
                                {
                                    EmployeeId = c.Id,
                                    Department = c.Section == null ? "" : c.Section.Department.Code + " - " + (!string.IsNullOrEmpty(c.Section.Department.NameNp) && language == "ne" ? c.Section.Department.NameNp : c.Section.Department.Name),
                                    EmployeeName = c.Name,
                                    EmployeeNameNp = c.NameNp,
                                    IdCardNo = c.Code,
                                    Mobile = c.Mobile,
                                    Email = c.Email,
                                    Section = c.Section == null ? "" : c.Section.Code + " - " + (!string.IsNullOrEmpty(c.Section.NameNp) && language == "ne" ? c.Section.NameNp : c.Section.Name),
                                    Designation = c.Designation == null ? "" : c.Designation.Code + " - " + (language == "ne" ? c.Designation.NameNp : c.Designation.Name),
                                    ReportingManagerId = c.ReportingManagerId,
                                    ReportingManager = j.Name ?? ""
                                })
                                //.
                                //AsEnumerable().Select(c => new ReportingLineupGridVm
                                //{
                                //    EmployeeId = c.EmployeeId,
                                //    Department = c.Department,
                                //    EmployeeName = c.EmployeeName,
                                //    EmployeeNameNp = c.EmployeeNameNp,
                                //    IdCardNo = c.IdCardNo,
                                //    Mobile = c.Mobile,
                                //    Email = c.Email,
                                //    Section = c.Section,
                                //    Designation = c.Designation,
                                //    ReportingManagerId = c.ReportingManagerId,
                                //    ReportingManager = GetName(c.ReportingManagerId, list)
                                //})
                                .ToList();

            switch (searchField)
            {
                case "IdCardNo":
                case "EmployeeName":
                case "DepartmentName":
                case "Section":
                case "Mobile":
                case "Department":
                    addParent(employeelist.Select(x => x.ReportingManagerId).Distinct().ToList(), list, employeelist);
                    break;
                default:

                    break;
            }
            return new KendoGridResult<List<ReportingLineupGridVm>>()
            {
                Data = employeelist,
                Status = ResultStatus.Ok,
                TotalCount = totalRowNum
            };
        }

        private void addParent(List<int?> parentIds, List<EEmployee> alllist, List<ReportingLineupGridVm> employeeList)
        {
            string language = RiddhaSession.Language.ToString();
            var parents = (from c in alllist
                           join d in parentIds on c.Id equals d
                           select new ReportingLineupGridVm()
                           {
                               EmployeeId = c.Id,
                               Department = c.Section == null ? "" : c.Section.Department.Code + " - " + (!string.IsNullOrEmpty(c.Section.Department.NameNp) && language == "ne" ? c.Section.Department.NameNp : c.Section.Department.Name),
                               EmployeeName = c.Name,
                               EmployeeNameNp = c.NameNp,
                               IdCardNo = c.Code,
                               Mobile = c.Mobile,
                               Email = c.Email,
                               Section = c.Section == null ? "" : c.Section.Code + " - " + (!string.IsNullOrEmpty(c.Section.NameNp) && language == "ne" ? c.Section.NameNp : c.Section.Name),
                               Designation = c.Designation == null ? "" : c.Designation.Code + " - " + (language == "ne" ? c.Designation.NameNp : c.Designation.Name),
                               ReportingManagerId = null,
                               ReportingManager = ""
                           }
                         ).ToList();

            employeeList.AddRange(parents);
        }

        private string GetName(int? reportingManagerId, List<EEmployee> list)
        {
            string lang = RiddhaSession.Language;
            string name;
            if (reportingManagerId == null || reportingManagerId == 0)
            {
                name = "";
            }
            else
            {
                name = list.Where(x => x.Id == reportingManagerId).FirstOrDefault().Name;
            }
            return name;
        }


        [HttpGet]
        public ServiceResult<ReportingLineupEditVm> Get(int id)
        {
            var empInfo = _employeeServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            ReportingLineupEditVm vm = new ReportingLineupEditVm();
            vm.EmployeeId = empInfo.Id;
            vm.EmployeeName = empInfo.Name;
            vm.Designation = empInfo.Designation.Name;
            vm.Department = empInfo.Section.Department.Name;
            vm.Section = empInfo.Section.Name;
            vm.Photo = empInfo.ImageUrl;
            if (empInfo.ReportingManagerId != null)
            {
                var repManagerInfo = _employeeServices.List().Data.Where(x => x.Id == empInfo.ReportingManagerId).FirstOrDefault();
                vm.ReportingManagerId = repManagerInfo.Id;
                vm.ReportingManagerName = repManagerInfo.Name;
                vm.ReportingManagerDesignation = repManagerInfo.Designation.Name;
                vm.ReportingManagerDepartment = repManagerInfo.Section.Department.Name;
                vm.ReportingManagerSection = repManagerInfo.Section.Name;
                vm.ReportingManagerPhoto = repManagerInfo.ImageUrl;
            }
            return new ServiceResult<ReportingLineupEditVm>()
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }

    }
    public class ReportingLineupArgument
    {
        public int ManagerId { get; set; }
        public string EmpIds { get; set; }
    }

    public class ReportingLineupGridVm
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameNp { get; set; }
        public string Designation { get; set; }
        public int? ReportingManagerId { get; set; }
        public string ReportingManager { get; set; }
        public string IdCardNo { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }

    public class ReportingLineupEditVm
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Photo { get; set; }
        public int ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerDesignation { get; set; }
        public string ReportingManagerDepartment { get; set; }
        public string ReportingManagerSection { get; set; }
        public string ReportingManagerPhoto { get; set; }
    }
}
