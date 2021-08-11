using Riddhasoft.DB;
using Riddhasoft.PayRoll.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.PayRoll.Services
{
    public class SSalarySheet 
    {
        RiddhaDBContext db = null;

        public SSalarySheet()
        {
            db = new RiddhaDBContext();
        }

        public ServiceResult<List<EMonthlySalarySheetPosting>> AddMonthlySalaryPostMasterInfo(List<EMonthlySalarySheetPosting> model)
        {

            db.MonthlySalarySheetPostingMaster.AddRange(model);
            db.SaveChanges();

            return new ServiceResult<List<EMonthlySalarySheetPosting>>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };

        }

     

        public ServiceResult<List<EMonthlySalarySheetAllowances>> AddMonthlySalarySheetAllowances(List<EMonthlySalarySheetAllowances> allowances) {

            db.MonthlySalarySheetAllowances.AddRange(allowances);
            db.SaveChanges();
            return new ServiceResult<List<EMonthlySalarySheetAllowances>>()
            {
                Data = allowances,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<List<EMonthlySalarySheetDeductions>> AddMonthlySalarySheetDeductions(List<EMonthlySalarySheetDeductions> deductions) {

            db.MonthlySalarySheetDeductions.AddRange(deductions);
            db.SaveChanges();
            return new ServiceResult<List<EMonthlySalarySheetDeductions>>()
            {
                Data = deductions,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<IQueryable<EMonthlySalarySheetPosting>> GetSalarySheetInfo()
        {
            return new ServiceResult<IQueryable<EMonthlySalarySheetPosting>>()
            {
                Data = db.MonthlySalarySheetPostingMaster,
                Status = ResultStatus.Ok
                };
        }

        public ServiceResult<EMonthlySalarySheetPosting> UpdateSalarySheet(EMonthlySalarySheetPosting model) {

            db.Entry<EMonthlySalarySheetPosting>(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EMonthlySalarySheetPosting>()
            {
                Data  = model,
                Message ="UpdateSuccess",
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<int> RemoveSalarySheetInfo(List<EMonthlySalarySheetPosting> MonthlySalarySheetPostings)
        {

            db.MonthlySalarySheetPostingMaster.RemoveRange(MonthlySalarySheetPostings);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message= "RemovedSuccess",
                Status = ResultStatus.Ok
            };

        }

        public ServiceResult<int> ApproveMonthlySalarySheet(List<EMonthlySalarySheetPosting> MonthlySalarySheetPostings) {

            foreach (var item in MonthlySalarySheetPostings)
            {

                db.Entry<EMonthlySalarySheetPosting>(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return new ServiceResult<int>()
            {

                Data = 1,
                Message = "ApprovedSuccess",
                Status = ResultStatus.Ok
            };

        }
        public ServiceResult<IQueryable<EMonthlySalarySheetAllowances>> GetEmpMonthlyAllowances()
        {
            return new ServiceResult<IQueryable<EMonthlySalarySheetAllowances>>()
            {
                Data = db.MonthlySalarySheetAllowances,
                Status = ResultStatus.Ok
            };
        }

    }
}
