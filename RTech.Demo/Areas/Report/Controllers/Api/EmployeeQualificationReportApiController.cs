using Riddhasoft.Employee.Entities;
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
    public class EmployeeQualificationReportApiController : ApiController
    {
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId = RiddhaSession.BranchId??0;
            int[] employeeIds = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;

            SEmployeeEducation educationService = new SEmployeeEducation();
            List<EEmployeeEducation> education = new List<EEmployeeEducation>();
            education = educationService.List().Data.Where(x => x.BranchId == branchId).ToList();

            //SUser userService = new SUser();
            //List<EUser> users = new List<EUser>();
            //users = userService.List().Data.Where(x => x.BranchId == branchId).ToList();

            var rpt = educationService.GetQualificationReport(branchId);
            List<QualificationViewModel> qualifications = (from s in rpt.Data.Skill
                                  select new QualificationViewModel()
                                  {
                                      EmployeeId = s.EmployeeId,
                                      EmployeeName = s.Employee.Code+" - "+s.Employee.Name,
                                      Type = "Skill",
                                      Name = s.Skills.Code + " - " + s.Skills.Name,
                                      Description = s.Skills.Description,
                                      //ApprovedById = s.ApprovedById??0
                                  }
                                  ).Union(
                                      from e in rpt.Data.Education
                                      select new QualificationViewModel()
                                      {
                                          EmployeeId = e.EmployeeId,
                                          EmployeeName = e.Employee.Code+" - "+e.Employee.Name,
                                          Type = "Education",
                                          Name = e.Education.Code + " - " + e.Education.Name,
                                          Description = e.Education.Description,
                                          //ApprovedById = e.ApprovedById ?? 0
                                      }
                                 ).Union(
                                      from f in rpt.Data.License
                                      select new QualificationViewModel()
                                      {
                                          EmployeeId = f.EmployeeId,
                                          EmployeeName = f.Employee.Code + " - " + f.Employee.Name,
                                          Type = "License",
                                          Name = f.License.Code + " - " + f.License.Name,
                                          Description = f.License.Description,
                                          //ApprovedById = f.ApprovedById ?? 0
                                      }
                                 ).Union(
                                      from g in rpt.Data.Language
                                      select new QualificationViewModel()
                                      {
                                          EmployeeId = g.EmployeeId,
                                          EmployeeName = g.Employee.Code + " - " + g.Employee.Name,
                                          Type = "Language",
                                          Name = g.Language.Code + " - " + g.Language.Name,
                                          Description = g.Language.Description,
                                          //ApprovedById = g.ApprovedById ?? 0
                                      }
                                 ).ToList();

            var result = new List<EmployeeQualificationGridViewModel>();

            result = (from c in qualifications
                      join d in employeeIds
                         on c.EmployeeId equals d
                      //join e in users
                      //   on c.ApprovedById equals e.Id
                      select new EmployeeQualificationGridViewModel()
                      {
                            EmployeeId = c.EmployeeId,
                            Type = c.Type,
                            EmployeeName = c.EmployeeName,
                            Name = c.Name,
                            Description = c.Description,
                            //ApprovedBy = e.Name
                      }).ToList();
            return new KendoGridResult<object>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = result.Count()
            };
        }
    }
    
    public class EmployeeQualificationGridViewModel
    {
        public int EmployeeId { get; set; }
        public string Type { get; set; }
        public string EmployeeName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ApprovedBy { get; set; }
    }

    public class QualificationViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public int ApprovedById { get; set; }
    }
}
