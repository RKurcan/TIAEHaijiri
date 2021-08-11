using Riddhasoft.Entity.User;
using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.HRM.Services.Training;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class TrainingSessionReportApiController : ApiController
    {
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId = RiddhaSession.BranchId ?? 0;

            SSession sessionService = new SSession();
            List<ESession> sessions = new List<ESession>();
            sessions = sessionService.List().Data.Where(x => x.BranchId == branchId).ToList();

            int[] courseIds = GetEmpIdsForReportParam(vm.DeptIds,vm.CourseIds).Data;

            var result = new List<TrainingSessionGridViewModel>();

            result = (from c in sessions
                      join d in courseIds
                        on c.CourseId equals d
                      select new TrainingSessionGridViewModel()
                      {
                          SessionName = c.Name,
                          CourseName = c.Course.Title,
                          DepartmentName = c.Course.Department.Name,
                          Duration = c.Duration,
                          Method = c.Method.ToString(),
                          Location = c.Location,
                          Description = c.Description
                      }).ToList();

           
            return new KendoGridResult<object>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = result.Count()
            };
            
        }

        public static ServiceResult<int[]> GetEmpIdsForReportParam(string deps, string courses){
            DataVisibilityLevel dataVisibilityLevel = (DataVisibilityLevel)RiddhaSession.DataVisibilityLevel;
            ServiceResult<int[]> result = new ServiceResult<int[]>();
            if (courses != null)
            {
                result.Data = Array.ConvertAll(courses.Split(','), s => int.Parse(s));
                return result;
            }
            SCourse courseService = new SCourse();
            var courseQuery = courseService.List().Data;
            if (deps == null)
            {

                switch (dataVisibilityLevel)
                {
                    case DataVisibilityLevel.Self:
                        result.Data = (from c in courseQuery where c.Id == RiddhaSession.EmployeeId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.Department:
                        result.Data = (from c in courseQuery where c.DepartmentId == RiddhaSession.DepartmentId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.Branch:
                        result.Data = (from c in courseQuery where c.BranchId == RiddhaSession.BranchId select c.Id).ToArray();
                        return result;
                    case DataVisibilityLevel.All:
                        result.Data = (from c in courseQuery where c.Branch.CompanyId == RiddhaSession.CompanyId select c.Id).ToArray();
                        return result;
                    default:
                        break;
                }
                result.Data = new int[] { };
                return result;
            }        
                else
                {
                    int[] depIds = Array.ConvertAll(deps.Split(','), s => int.Parse(s));
                    switch (dataVisibilityLevel)
                    {
                        case DataVisibilityLevel.Self:
                            result.Data = (from c in courseQuery
                                           where c.Id == RiddhaSession.EmployeeId
                                           select c.Id).ToArray();
                            break;
                        case DataVisibilityLevel.Department:
                        case DataVisibilityLevel.Branch:
                        case DataVisibilityLevel.All:
                            result.Data = (from c in courseQuery
                                           join d in depIds on c.DepartmentId equals d
                                           select c.Id).ToArray();
                            break;
                        default:
                            break;
                    }
                    return result;
                }
            }

         public ServiceResult<List<CourseGridVm>> GetCoursesByDepartment(string id)
            {
            int branchId = RiddhaSession.BranchId ?? 0;
            SCourse courseService = new SCourse();
            List<ECourse> courseList = new List<ECourse>();
            courseList = courseService.List().Data.Where(x => x.BranchId == branchId).ToList();

            string[] departments = id.Split(',');
            List<CourseGridVm> courses = new List<CourseGridVm>();
            if (RiddhaSession.RoleId == 0)
            {
                courses = (from c in courseList
                           join d in departments
                               on c.DepartmentId equals int.Parse(d)
                           select new CourseGridVm()
                           {
                               Id = c.Id,
                               Name = c.Title
                           }).ToList();
            }

            return new ServiceResult<List<CourseGridVm>>()
            {
                Data = courses,
                Status = ResultStatus.Ok
            };
        }
    }

    public class TrainingSessionGridViewModel
    {
        public String SessionName { get; set; }
        public string CourseName { get; set; }
        public string DepartmentName { get; set; }
        public string Method { get; set; }
        public string Duration { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }

    public class CourseGridVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
