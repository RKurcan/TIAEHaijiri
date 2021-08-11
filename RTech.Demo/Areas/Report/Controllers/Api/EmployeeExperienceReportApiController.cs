using Riddhasoft.HRM.Entities.Qualification;
using Riddhasoft.HRM.Services.Qualification;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class EmployeeExperienceReportApiController : ApiController
    {
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId = RiddhaSession.BranchId ?? 0;

            SExperience experienceService = new SExperience();
            List<EExperience> experience = new List<EExperience>();
            experience = experienceService.List().Data.Where(x => x.BranchId == branchId && x.IsApproved == true).ToList();

            List<EUser>  users = new List<EUser>();
            SUser userServices = new SUser();
            users = userServices.List().Data.Where(x => x.BranchId == branchId).ToList();

            int[] employeeIds = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;

            List<EmployeeExperienceGridViewModel> result = new List<EmployeeExperienceGridViewModel>();
            result = (from e in experience
                      join f in employeeIds
                          on e.EmployeeId equals f
                      join g in users
                           on e.ApprovedById equals g.Id
                      select new EmployeeExperienceGridViewModel() 
                      { 
                        EmployeeName = e.Employee.Code +" - "+ e.Employee.Name,
                        Code = e.Code.ToString(),
                        Title = e.Title,
                        Organization = e.OrganizationName,
                        BeganOn = e.BeganOn.ToString("yyyy/MM/dd"),
                        EndedOn = e.EndedOn.ToString("yyyy/MM/dd"),
                        Description = e.Description,
                        ApprovedBy = g.FullName ?? g.Name,
                       Department=e.Employee.Section.Department.Code+" - "+e.Employee.Section.Department.Name
                      }
                      ).ToList();
            return new KendoGridResult<object>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = result.Count()
            };
        }
    }

    public class EmployeeExperienceGridViewModel
    {
        public string EmployeeName { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string BeganOn { get; set; }
        public string EndedOn { get; set; }
        public string Description { get; set; }
        public string ApprovedBy { get; set; }
        public string Department { get; set; }
    }
}
