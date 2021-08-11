using Riddhasoft.HRM.Entities.Training;
using Riddhasoft.HRM.Services.Training;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTech.Demo.Areas.Report.Controllers.Api
{
    public class TrainingParticipationReportApiController : ApiController
    {
        public KendoGridResult<object> GenerateReport(KendoReportViewModel vm)
        {
            int branchId = RiddhaSession.BranchId ?? 0;

            SParticipant participantService = new SParticipant();
            List<EParticipant> participants = new List<EParticipant>();
            participants = participantService.List().Data.Where(x => x.BranchId == branchId).ToList();

            List<EParticipantDetail> participantDetails = new List<EParticipantDetail>();
            participantDetails = participantService.ListDetail().Data.ToList();

            //SUser userService = new SUser();
            //List<EUser> users = new List<EUser>();
            //users = userService.List().Data.Where(x => x.BranchId == branchId).ToList();

            int[] employeeIds = Common.GetEmpIdsForReportParam(vm.DeptIds, vm.SectionIds, vm.EmpIds).Data;

            var result = new List<ParticipatingSessionModel>();

            result = (from c in participantDetails
                      join e in employeeIds
                            on c.EmployeeId equals e
                      join d in participants
                            on c.ParticipantId equals d.Id
                      select new ParticipatingSessionModel()
                      {
                          EmployeeName = c.Employee.Code+" - "+c.Employee.Name,
                          SessionName = d.Session.Name,
                          CourseName = d.Session.Course.Title,
                          StartDate = d.StartDate.ToString("yyyy/MM/dd"),
                          EndDate = d.EndDate.ToString("yyyy/MM/dd"),
                          ParticipantStatus = d.ParticipantStatus.ToString()
                      }).ToList();

            return new KendoGridResult<object>()
            {
                Data = result.Skip(vm.Skip).Take(vm.Take),
                Status = ResultStatus.Ok,
                TotalCount = result.Count()
            };
        }

      
    }

    public class ParticipatingSessionModel
    {
        public string EmployeeName { get; set; }
        public string SessionName { get; set; }
        public string CourseName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ParticipantStatus { get; set; }
    }

    
}
