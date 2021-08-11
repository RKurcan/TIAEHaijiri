using Riddhasoft.DB;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.Services.Common;
using Riddhasoft.User.Entity;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Riddhasoft.OfficeSetup.Services
{
    public class SCompany : Riddhasoft.Services.Common.IBaseService<ECompany>
    {
        RiddhaDBContext db = null;
        private bool _proxy = true;
        public SCompany()
        {
            db = new RiddhaDBContext();
        }
        public SCompany(bool proxy)
        {
            this._proxy = proxy;

            db = new RiddhaDBContext();
            db.Configuration.ProxyCreationEnabled = false;
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<ECompany>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<ECompany>>()
            {
                Data = db.Company,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<ECompany> Add(ECompany model)
        {
            db.Company.Add(model);
            db.SaveChanges();
            if (model.SoftwareType != SoftwareType.Desktop)
            {
                EBranch branch = new EBranch()
                {
                    CompanyId = model.Id,
                    Code = model.Code,
                    Name = model.Name,
                    NameNp = model.NameNp,
                    Address = model.Address,
                    AddressNp = model.AddressNp,
                    ContactNo = model.ContactNo,
                    Email = model.Email,
                    IsHeadOffice = true
                };
                db.Branch.Add(branch);
                db.SaveChanges();

                EDepartment department = new EDepartment()
                {
                    Code = "D01",
                    Name = "Default",
                    BranchId = branch.Id,
                    NumberOfStaff = 0
                };
                db.Department.Add(department);
                db.SaveChanges();

                ESection section = new ESection()
                {
                    Code = "S01",
                    Name = "Default",
                    DepartmentId = department.Id,
                    BranchId = department.BranchId
                };
                db.Section.Add(section);
                db.SaveChanges();
            }
            return new ServiceResult<ECompany>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<ECompany> Update(ECompany model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ECompany>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(ECompany model)
        {
            try
            {
                int branchCount = db.Branch.Count(x => x.CompanyId == model.Id);
                if (branchCount > 0)
                {
                    return new ServiceResult<int>()
                    {
                        Data = 1,
                        Message = string.Format("There {1} {0} branch on this company. Company Can't be deleted.", branchCount, branchCount == 1 ? "is" : "are"),
                        Status = ResultStatus.dataBaseError
                    };
                }
                model = db.Company.Remove(db.Company.Find(model.Id));
                db.SaveChanges();
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = "Removed Successfully",
                    Status = ResultStatus.Ok
                };
            }
            catch (SqlException ex)
            {

                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.dataBaseError
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>()
                {
                    Data = 1,
                    Message = ex.Message,
                    Status = ResultStatus.unHandeledError
                };
            }
        }
        public ServiceResult<IQueryable<ECompanyLogin>> ListCompanyLogin()
        {
            return new ServiceResult<IQueryable<ECompanyLogin>>()
            {
                Data = db.CompanyLogin,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<ECompanyLicense>> ListCompanyLicense()
        {
            return new ServiceResult<IQueryable<ECompanyLicense>>()
            {
                Data = db.CompanyLicense,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<IQueryable<ECompanyLicenseLog>> ListCompanyLicenseLog()
        {
            return new ServiceResult<IQueryable<ECompanyLicenseLog>>()
            {
                Data = db.CompanyLicenseLog,
                Status = ResultStatus.Ok
            };
        }
        public ServiceResult<ECompanyLicense> AddCompanyLicense(ECompanyLicense model)
        {
            string message = "";
            ECompanyLicense license = db.CompanyLicense.Where(x => x.CompanyId == model.CompanyId).FirstOrDefault();
            if (license != null)
            {
                db.CompanyLicense.Remove(license);
                db.SaveChanges();
                message = "Renewed Successfully!!!";
            }
            else
            {
                message = "License Provided Successfully!!!";
            }
            db.CompanyLicense.Add(model);
            db.SaveChanges();
            ECompanyLicenseLog log = new ECompanyLicenseLog();
            log.CompanyId = model.CompanyId;
            log.IssueDate = model.IssueDate;
            log.LicensePeriod = model.LicensePeriod;
            db.CompanyLicenseLog.Add(log);
            db.SaveChanges();
            return new ServiceResult<ECompanyLicense>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = message
            };
        }

        public ServiceResult<ECompanyLicenseLog> UpdateCompanyLicenseLog(ECompanyLicenseLog model)
        {
            db.Entry<ECompanyLicenseLog>(model).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<ECompanyLicenseLog>()
            {
                Data = model,
                Message = "UpdateSuccess",
                Status = ResultStatus.Ok
            };
        }
        public int GetCompanyDeviceCount(int id)
        {
            return db.CompanyDeviceAssignment.Where(x => x.CompanyId == id).Count();
        }

        public ServiceResult<ECompany> Find(int id)
        {
            return new ServiceResult<ECompany>()
            {
                Data = db.Company.Find(id),
                Message = "",
                Status = ResultStatus.Ok
            };
        }
    }
}
