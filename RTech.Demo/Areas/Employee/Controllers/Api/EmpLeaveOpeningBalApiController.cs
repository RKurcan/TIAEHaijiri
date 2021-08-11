using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Services;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using RTech.Demo.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RTech.Demo.Areas.Employee.Controllers.Api
{
    public class EmpLeaveOpeningBalApiController : ApiController
    {
        SDepartment _departmentServices = null;
        SSection _sectionServices = null;
        SEmployee _employeeServices = null;
        public EmpLeaveOpeningBalApiController()
        {
            _departmentServices = new SDepartment();
            _sectionServices = new SSection();
            _employeeServices = new SEmployee();
        }
        [HttpPost]
        public ServiceResult<OpBalViewModel> Post(OpBalViewModel OpBalViewModel)
        {

            List<ELeaveBalance> leaveBalances = new List<ELeaveBalance>();
            foreach (var row in OpBalViewModel.EmpLeaveBalRows)
            {
                foreach (var col in row.EmpLeaveBalColumns)
                {

                    leaveBalances.Add(new ELeaveBalance()
                    {
                        EmployeeId = row.EmployeeId,
                        LeaveMasterId = col.LeaveId,
                        OpeningBalance = col.OpBal,
                        RemainingBalance = col.OpBal,
                    });
                }
            }

            SLeaveBalance balService = new SLeaveBalance();
            if (leaveBalances.Count > 0)
            {
                balService.UpdateOpeningBalance(leaveBalances);
                Common.AddAuditTrail("8023", "7234", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 0, "Saved Sucessfully");
            }

            return new ServiceResult<OpBalViewModel>()
            {
                Status = ResultStatus.Ok,
                Data = OpBalViewModel
            };


        }
        public ServiceResult<OpBalViewModel> RefreshGrid(Argument args)
        {
            int branchId = (int)RiddhaSession.BranchId;
            int[] DeptIds = { };
            int[] SectionIds = { };
            int[] EmpIds = { };

            var empids = Common.GetEmpIdsForReportParam(args.DeptIds, args.EmpIds, args.EmpIds);
            SLeaveMaster sLeaveMaster = new SLeaveMaster();
            var list = sLeaveMaster.GetELeaveBalances(RiddhaSession.CompanyId);
            OpBalViewModel vm = new OpBalViewModel();
            SContract contractServices = new SContract();
            //manipulate data
            var leaveMasterList = sLeaveMaster.List().Data.Where(x => x.CompanyId == RiddhaSession.CompanyId);
            var empQuery = new SEmployee(sLeaveMaster.GetDbContext()).List().Data;
            var designationwiseLeaveMapped = new SDesignation().ListLeaveQouta();
            var employmentStatusWiseMappedLeave = new SEmploymentStatusWiseLeavedBalance().List();
            foreach (var id in empids.Data)
            {
                var opBalItems = list.Where(x => x.EmployeeId == id).ToList();
                var emp = empQuery.Where(x => x.Id == id).FirstOrDefault();
                var rowModel = new EmpLeaveBalRowModel()
                {
                    EmployeeId = id,
                    EmployeeCode = emp.Code,
                    EmployeeName = emp.Code + "-" + emp.Name,
                };
                if (RiddhaSession.EmploymentStatusWiseLeave)
                {
                    foreach (var item in leaveMasterList)
                    {
                        var opBal = opBalItems.Where(x => x.LeaveMasterId == item.Id).FirstOrDefault();
                        var contract = contractServices.List().Data.Where(x => x.EmployeeId == emp.Id).ToList();
                        if (contract.Count() > 0)
                        {
                            int employmentStatusId = contract.LastOrDefault().EmploymentStatusId;
                            var leavMapped = employmentStatusWiseMappedLeave.Data.Where(x => x.EmploymentStatusId == employmentStatusId && x.LeaveId == item.Id && (x.ApplicableGender == Riddhasoft.OfficeSetup.Entities.ApplicableGender.All || (int)x.ApplicableGender == ((int)emp.Gender) + 1)).FirstOrDefault();
                            //var leavMapped = designationwiseLeaveMapped.Data.Where(x => x.DesignationId == emp.DesignationId && x.LeaveId == item.Id && (x.ApplicableGender == Riddhasoft.OfficeSetup.Entities.ApplicableGender.All || (int)x.ApplicableGender == ((int)emp.Gender) + 1)).FirstOrDefault();
                            rowModel.EmpLeaveBalColumns.Add(new EmpLeaveBalColumnModel()
                            {

                                IsMapped = (leavMapped ?? new Riddhasoft.HRM.Entities.EEmploymentStatusWiseLeavedBalance()).IsMapped,
                                LeaveCode = item.Code,
                                LeaveName = item.Name,
                                LeaveId = item.Id,
                                OpBal = (opBal ?? new Riddhasoft.Employee.Entities.ELeaveBalance()).OpeningBalance
                            });
                        }
                        else
                        {
                            return new ServiceResult<OpBalViewModel>()
                            {
                                Data = null,
                                Message = "This employee isn't in Contract.. Please add contract first.",
                                Status = ResultStatus.processError
                            };
                        }
                    }
                }
                else
                {
                    foreach (var item in leaveMasterList)
                    {
                        var opBal = opBalItems.Where(x => x.LeaveMasterId == item.Id).FirstOrDefault();
                        var leavMapped = designationwiseLeaveMapped.Data.Where(x => x.DesignationId == emp.DesignationId && x.LeaveId == item.Id && (x.ApplicableGender == Riddhasoft.OfficeSetup.Entities.ApplicableGender.All || (int)x.ApplicableGender == ((int)emp.Gender) + 1)).FirstOrDefault();
                        rowModel.EmpLeaveBalColumns.Add(new EmpLeaveBalColumnModel()
                        {

                            IsMapped = (leavMapped ?? new Riddhasoft.OfficeSetup.Entities.EDesignationWiseLeavedBalance()).IsMapped,
                            LeaveCode = item.Code,
                            LeaveName = item.Name,
                            LeaveId = item.Id,
                            OpBal = (opBal ?? new Riddhasoft.Employee.Entities.ELeaveBalance()).OpeningBalance
                        });
                    }
                }
                vm.EmpLeaveBalRows.Add(rowModel);
            }
            Common.AddAuditTrail("8023", "7233", RiddhaSession.CurDate.ToDateTime(), RiddhaSession.UserId, 0, "Refresh");
            return new ServiceResult<OpBalViewModel>()
            {
                Status = ResultStatus.Ok,
                Data = vm,
            };

        }
    }
    public class Argument
    {
        public string DeptIds { get; set; }
        public string SectionIds { get; set; }
        public string EmpIds { get; set; }

    }

    public class OpBalViewModel
    {
        public OpBalViewModel()
        {
            EmpLeaveBalRows = new List<EmpLeaveBalRowModel>();
        }
        public List<EmpLeaveBalRowModel> EmpLeaveBalRows { get; set; }
    }
    public class EmpLeaveBalRowModel
    {
        public EmpLeaveBalRowModel()
        {
            EmpLeaveBalColumns = new List<EmpLeaveBalColumnModel>();
        }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public List<EmpLeaveBalColumnModel> EmpLeaveBalColumns { get; set; }

    }

    public class EmpLeaveBalColumnModel
    {
        public int LeaveId { get; set; }
        public string LeaveCode { get; set; }
        public string LeaveName { get; set; }
        public decimal OpBal { get; set; }
        public bool IsMapped { get; set; }
    }

}
