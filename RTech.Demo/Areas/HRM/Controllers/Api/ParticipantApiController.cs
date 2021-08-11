using Riddhasoft.Employee.Services;
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

namespace RTech.Demo.Areas.HRM.Controllers.Api
{
    public class ParticipantApiController : ApiController
    {
        SParticipant _participantServices = null;
        SSession _sessionServices = null;
        SEmployee _employeeServices = null;
        LocalizedString _loc = null;
        public ParticipantApiController()
        {
            _participantServices = new SParticipant();
            _sessionServices = new SSession();
            _employeeServices = new SEmployee();
            _loc = new LocalizedString();
        }
        public ServiceResult<List<ParticipantGridVm>> Get()
        {
            int? branchId = RiddhaSession.BranchId;
            var participantLst = (from c in _participantServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                  select new ParticipantGridVm()
                                  {
                                      Id = c.Id,
                                      BranchId = c.BranchId,
                                      CourseName = c.Session.Course.Title,
                                      EndDate = c.EndDate.ToString("yyyy/MM/dd"),
                                      ParticipantStatus = Enum.GetName(typeof(ParticipantStatus), c.ParticipantStatus),
                                      SessionId = c.SessionId,
                                      SessionName = c.Session.Name,
                                      StartDate = c.StartDate.ToString("yyyy/MM/dd"),
                                  }).ToList();
            return new ServiceResult<List<ParticipantGridVm>>()
            {
                Data = participantLst,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ParticipantVmToSave> Get(int id)
        {
            string lang = RiddhaSession.Language;
            ParticipantVmToSave vm = new ParticipantVmToSave();
            vm.Participant = _participantServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var participantDetailLst = _participantServices.ListDetail().Data.Where(x => x.ParticipantId == vm.Participant.Id);
            if (lang == "ne")
            {
                vm.EmpLst = (from c in participantDetailLst
                             select new ParticipantEmpVm()
                             {
                                 Id = c.EmployeeId,
                                 Name = c.Employee.Code + " - " + (!string.IsNullOrEmpty(c.Employee.NameNp) ? c.Employee.NameNp : c.Employee.Name) + " - " + (c.Employee.Mobile ?? "")
                             }).ToList();
            }
            else
            {
                vm.EmpLst = (from c in participantDetailLst
                             select new ParticipantEmpVm()
                             {
                                 Id = c.EmployeeId,
                                 Name = c.Employee.Code + " - " + c.Employee.Name + " - " + (c.Employee.Mobile ?? "")
                             }).ToList();
            }
            return new ServiceResult<ParticipantVmToSave>
            {
                Data = vm,
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<ParticipantVmToSave> Post(ParticipantVmToSave vm)
        {
            vm.Participant.BranchId = (int)RiddhaSession.BranchId;
            vm.Participant.CreatedById = RiddhaSession.UserId;
            vm.Participant.CreatedOn = DateTime.Now;
            var result = _participantServices.Add(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8008", "7134", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Participant.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ParticipantVmToSave>()
            {
                Data = vm,
                Status = ResultStatus.Ok,
                Message = _loc.Localize("AddedSuccess")
            };
        }
        public ServiceResult<ParticipantVmToSave> Put(ParticipantVmToSave vm)
        {
            var result = _participantServices.Update(vm);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8008", "7135", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, vm.Participant.Id, _loc.Localize(result.Message));
            }
            return new ServiceResult<ParticipantVmToSave>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        public ServiceResult<int> Delete(int id)
        {
            var participant = _participantServices.List().Data.Where(x => x.Id == id).FirstOrDefault();
            var result = _participantServices.Remove(participant);
            if (result.Status == ResultStatus.Ok)
            {
                Common.AddAuditTrail("8008", "7136", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, id, _loc.Localize(result.Message));
            }
            return new ServiceResult<int>()
            {
                Data = result.Data,
                Message = _loc.Localize(result.Message),
                Status = result.Status
            };
        }
        [HttpPost]
        public ServiceResult<List<SessionAutoCompleteVm>> GetSessionLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<SessionAutoCompleteVm> resultLst = new List<SessionAutoCompleteVm>();
            if (model != null)
            {
                var sessionLst = _sessionServices.List().Data.Where(x => x.BranchId == branchId);
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                if (searchText == "___")
                {
                    sessionLst = sessionLst.OrderBy(x => x.Name).Take(20);
                }
                else
                {
                    sessionLst = sessionLst.Where(x => x.Name.ToLower().Contains(searchText.ToLower()));
                }
                if (sessionLst != null)
                {
                    resultLst = (from c in sessionLst
                                 select new SessionAutoCompleteVm()
                                 {
                                     Id = c.Id,
                                     Name = c.Name
                                 }).OrderBy(x => x.Name).ToList();
                }
            }
            return new ServiceResult<List<SessionAutoCompleteVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }

        [HttpPost]
        public ServiceResult<List<SessionAutoCompleteVm>> GetEmpLstForAutoComplete(EKendoAutoComplete model)
        {
            int? branchId = RiddhaSession.BranchId;
            List<SessionAutoCompleteVm> resultLst = new List<SessionAutoCompleteVm>();
            if (model != null)
            {
                string searchText = model.Filter.Filters.Count() > 0 ? model.Filter.Filters[0].Value : "";
                if (searchText == null)
                {
                    return new ServiceResult<List<SessionAutoCompleteVm>>()
                    {
                        Data = resultLst,
                        Status = ResultStatus.Ok
                    };
                }
                var employeeLst = (from c in _employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList()
                                   join d in _participantServices.List().Data.ToList() on c.Section.DepartmentId equals d.Session.Course.DepartmentId
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
                                 select new SessionAutoCompleteVm()
                                 {
                                     Id = c.Id,
                                     Name = c.Code + " - " + (c.Name) + " - " + (c.Mobile ?? ""),
                                 }).OrderBy(x => x.Name).ToList();
                }
            }
            return new ServiceResult<List<SessionAutoCompleteVm>>()
            {
                Data = resultLst,
                Status = ResultStatus.Ok
            };
        }


    }


    public class ParticipantGridVm
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ParticipantStatus { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
    }

    public class SessionAutoCompleteVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
