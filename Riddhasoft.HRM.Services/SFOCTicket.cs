using Riddhasoft.DB;
using Riddhasoft.HRM.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Services
{
    public class SFOCTicket
    {
        RiddhaDBContext db = null;
        public SFOCTicket()
        {
            db = new RiddhaDBContext();
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EFOCTicket>> List()
        {
            return new ServiceResult<IQueryable<EFOCTicket>>()
            {
                Data = db.FOCTicket,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<FocTicketViewModel> Add(FocTicketViewModel vm)
        {
            db.FOCTicket.Add(vm.FOCTicket);
            db.SaveChanges();
            List<EFOCTicketDetail> detailLst = new List<EFOCTicketDetail>();
            foreach (var item in vm.FOCTicketDetail)
            {
                EFOCTicketDetail model = new EFOCTicketDetail();
                model.FOCTicketId = vm.FOCTicket.Id;
                model.Name = item.Name;
                model.Relation = item.Relation;
                detailLst.Add(model);
            }
            db.FOCTicketDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<FocTicketViewModel>()
            {
                Data = vm,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<FocTicketViewModel> Update(FocTicketViewModel vm)
        {
            db.Entry(vm.FOCTicket).State = EntityState.Modified;
            db.SaveChanges();
            var existingDetails = db.FOCTicketDetail.Where(x => x.FOCTicketId == vm.FOCTicket.Id).ToList();
            if (existingDetails.Count() > 0)
            {
                db.FOCTicketDetail.RemoveRange(existingDetails);
                db.SaveChanges();
            }
            List<EFOCTicketDetail> detailLst = new List<EFOCTicketDetail>();
            foreach (var item in vm.FOCTicketDetail)
            {
                EFOCTicketDetail model = new EFOCTicketDetail();
                model.FOCTicketId = item.FOCTicketId;
                model.Name = item.Name;
                model.Relation = item.Relation;
                detailLst.Add(model);
            }
            db.FOCTicketDetail.AddRange(detailLst);
            db.SaveChanges();
            return new ServiceResult<FocTicketViewModel>()
            {
                Data = vm,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EFOCTicket model)
        {
            db.FOCTicket.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<EFOCTicketDetail>> ListDetails()
        {
            return new ServiceResult<IQueryable<EFOCTicketDetail>>()
            {
                Data = db.FOCTicketDetail,
                Status = ResultStatus.Ok
            };
        }
        public List<FocGridVm> GeFOCTicketList(int branchId, int FiscalYearId)
        {
            var list = (from c in db.FOCTicketDetail
                        where c.FOCTicket.BranchId == branchId
                        select new FocGridVm()
                        {
                            AppliedDate = c.FOCTicket.AppliedDate,
                            ApprovedById = c.FOCTicket.ApprovedById,
                            ApprovedOn = c.FOCTicket.ApprovedOn,
                            BranchId = c.FOCTicket.BranchId,
                            CreatedById = c.FOCTicket.CreatedById,
                            CreatedOn = c.FOCTicket.CreatedOn,
                            EmployeeId = c.FOCTicket.EmployeeId,
                            Id = c.FOCTicket.Id,
                            IsApproved = c.FOCTicket.IsApproved,
                            Rebate = c.FOCTicket.Rebate,
                            RecommendedBy = c.FOCTicket.RecommendedBy,
                            RequestType = c.FOCTicket.RequestType,
                            SectorADateOfFlight = c.FOCTicket.SectorADateOfFlight,
                            SectorAFlightNo = c.FOCTicket.SectorAFlightNo,
                            SectorAFrom = c.FOCTicket.SectorAFrom,
                            SectorATo = c.FOCTicket.SectorATo,
                            SectorBDateOfFlight = c.FOCTicket.SectorBDateOfFlight,
                            SectorBFlightNo = c.FOCTicket.SectorBFlightNo,
                            SectorBFrom = c.FOCTicket.SectorBFrom,
                            SectorBTo = c.FOCTicket.SectorBTo,
                        }).OrderBy(x => x.Id).ToList();
            return list;
        }
        public ServiceResult<EFOCTicket> Approve(EFOCTicket model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EFOCTicket>()
            {
                Data = model,
                Message = "ApprovedSuccessfully",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<EFOCTicket> Revert(EFOCTicket model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EFOCTicket>()
            {
                Data = model,
                Message = "RevertSuccessfully",
                Status = ResultStatus.Ok
            };
        }
    }
    public class FocTicketViewModel
    {
        public EFOCTicket FOCTicket { get; set; }
        public List<EFOCTicketDetail> FOCTicketDetail { get; set; }
    }
    public class FocGridVm : EFOCTicket
    {

    }
}
