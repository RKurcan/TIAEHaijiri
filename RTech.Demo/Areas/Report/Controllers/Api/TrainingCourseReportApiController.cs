using Riddhasoft.Employee.Entities;
using Riddhasoft.HRM.Entities.Qualification;
using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.HRM.Services.Qualification;
using Riddhasoft.HRM.Services.Training;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class TrainingCourseReportApiController : ApiController
    {
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId = RiddhaSession.BranchId ?? 0;

            SCourse courseService = new SCourse();
            List<ECourse> courses = new List<ECourse>();
            int[] deptIds = vm.DeptIds != null ? Array.ConvertAll(vm.DeptIds.Split(','), s => int.Parse(s)) : null;
            courses = courseService.List().Data.Where(x => x.BranchId == branchId).ToList();

            var result = new List<TrainingCourseGridViewModel>();

            if(deptIds != null)
            {
                result = (from c in courses
                          join d in deptIds
                        on c.DepartmentId equals d
                          select new TrainingCourseGridViewModel
                          {
                              CourseTitle = c.Title,
                              DepartmentName = c.Department.Name,
                              Coordinator = c.Coordinator.Name,
                              Version = c.Version.ToString(),
                              Cost = c.Currency + " " + c.Cost,
                              Description = c.Description
                          }).ToList();
            }
            else
            {
                result = (from c in courses
                          select new TrainingCourseGridViewModel
                          {
                              CourseTitle = c.Title,
                              DepartmentName = c.Department.Name,
                              Coordinator = c.Coordinator.Name,
                              Version = c.Version.ToString(),
                              Cost = c.Currency + " " + c.Cost,
                              Description = c.Description
                          }).ToList();
            }
           
            return new KendoGridResult<object>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = result.Count()
            };
        }
    }

    public class TrainingCourseGridViewModel
    {
        public string CourseTitle { get; set; }
        public string DepartmentName { get; set; }
        public string Coordinator { get; set; }
        public string Version { get; set; }
        public string Cost { get; set; }
        public string Description { get; set; }
    }
}
