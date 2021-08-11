using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Riddhasoft.Employee.Services
{
    public class SEmployee
    {
        RiddhaDBContext db = null;
        public int _softwarePackageId = 0;
        public SEmployee()
        {
            db = new RiddhaDBContext();
        }
        public SEmployee(RiddhaDBContext db)
        {
            this.db = db;
        }
        public RiddhaDBContext GetDbContext()
        {
            return db;
        }
        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployee>> List()
        {
            return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EEmployee>>()
            {
                Data = db.Employee,
                Message = "",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<List<EmployeeFixedShiftModel>> GetFixedRoster(int[] sectionIds, string language)
        {

            var list = (from c in db.EmployeeShitList.Where(x => x.Employee.ShiftTypeId == 0 && x.Employee.EmploymentStatus != EmploymentStatus.Resigned && x.Employee.EmploymentStatus != EmploymentStatus.Terminated)
                        join d in sectionIds on c.Employee.SectionId equals d
                        select new EmployeeFixedShiftModel()
                        {
                            Id = c.EmployeeId,
                            Code = c.Employee.Code,
                            //Name = c.Employee.Name,
                            Name = language == "ne" && c.Employee.NameNp != null ? c.Employee.NameNp : c.Employee.Name,
                            ShiftId = c.ShiftId
                        }
                          ).ToList();

            return new Riddhasoft.Services.Common.ServiceResult<List<EmployeeFixedShiftModel>>()
            {
                Data = list,
                Message = "",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployee> Add(EEmployee model, int shiftId, int[] weeklyoffArray)
        {
            db.Employee.Add(model);
            db.SaveChanges();
            if (model.ShiftTypeId == 0)
            {
                EEmployeeShitList shiftLst = new EEmployeeShitList();
                shiftLst.EmployeeId = model.Id;
                shiftLst.ShiftId = shiftId;
                shiftLst.AfterDays = 7;
                db.EmployeeShitList.Add(shiftLst);
                db.SaveChanges();

                foreach (var item in weeklyoffArray)
                {
                    db.EmployeeWOList.Add(new EEmployeeWOList()
                    {
                        EmployeeId = model.Id,
                        OffDayId = item,
                    });
                    db.SaveChanges();
                }

            }
            if (_softwarePackageId > 0)
            {
                if (model.DesignationId != 0 || model.DesignationId != null)
                {
                    var existinhHist = db.EmployeeDesignationHistory.Where(x => x.EmployeeId == model.Id).FirstOrDefault();
                    if (existinhHist == null)
                    {
                        EEmployeeDesignationHistory hist = new EEmployeeDesignationHistory()
                        {
                            BranchId = (int)model.BranchId,
                            DesignationId = (int)model.DesignationId,
                            EmployeeId = model.Id,
                            DateTime = DateTime.Now,
                        };
                        db.EmployeeDesignationHistory.Add(hist);
                        db.SaveChanges();
                    }
                }
            }
            return new ServiceResult<EEmployee>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<EEmployee> Add(EEmployee model)
        {
            var shift = db.Shift.Where(x => x.BranchId == model.BranchId).FirstOrDefault();
            if (shift != null)
            {
                //var section=db.Section.Where(x => x.Department.BranchId == model.BranchId).FirstOrDefault();
                //model.SectionId = section.Id;
                db.Employee.Add(model);
                db.SaveChanges();
                //TODO: Save With Default Shift
                //get default shift

                EEmployeeShitList shiftLst = new EEmployeeShitList();
                shiftLst.EmployeeId = model.Id;
                shiftLst.ShiftId = shift.Id;
                shiftLst.AfterDays = 7;
                db.EmployeeShitList.Add(shiftLst);
                db.SaveChanges();

                //set weekly off to saturday
                db.EmployeeWOList.Add(new EEmployeeWOList()
                {
                    EmployeeId = model.Id,
                    OffDayId = 6,
                });
                db.SaveChanges();
            }
            return new ServiceResult<EEmployee>()
            {
                Data = model,
                Message = "AddedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public Riddhasoft.Services.Common.ServiceResult<EEmployee> AddEmployeeOnly(EEmployee model)
        {
            db.Employee.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployee>()
            {
                Data = model,
                Message = "Added Successfully",
                Status = ResultStatus.Ok
            };
        }
        public Riddhasoft.Services.Common.ServiceResult<EEmployee> Update(EEmployee model, int shiftId, int[] weeklyoffArray)
        {
            if (model.UserId == 0)
            {
                model.UserId = null;
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            UpdateShift(model.Id, model.ShiftTypeId, shiftId, weeklyoffArray);
            if (_softwarePackageId > 0)
            {
                if (model.DesignationId != 0 || model.DesignationId != null)
                {
                    var existinhHist = db.EmployeeDesignationHistory.Where(x => x.EmployeeId == model.Id).FirstOrDefault();
                    if (existinhHist == null)
                    {
                        EEmployeeDesignationHistory hist = new EEmployeeDesignationHistory()
                        {
                            BranchId = (int)model.BranchId,
                            DesignationId = (int)model.DesignationId,
                            EmployeeId = model.Id,
                            DateTime = DateTime.Now,
                        };
                        db.EmployeeDesignationHistory.Add(hist);
                        db.SaveChanges();
                    }
                }
            }
            return new ServiceResult<EEmployee>()
            {
                Data = model,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }

        public void UpdateShift(int id, int? ShiftTypeId, int shiftId, int[] weeklyoffArray)
        {
            //delete shift list if exist
            var existingShifts = db.EmployeeShitList.Where(x => x.EmployeeId == id).ToList();
            if (existingShifts.Count > 0)
            {
                db.EmployeeShitList.RemoveRange(existingShifts);
                db.SaveChanges();
            }

            //delete weekly list if exist
            var existingWeeklyOf = db.EmployeeWOList.Where(x => x.EmployeeId == id).ToList();
            if (existingWeeklyOf.Count > 0)
            {
                db.EmployeeWOList.RemoveRange(existingWeeklyOf);
                db.SaveChanges();
            }

            //add new shift and weekly off
            if (ShiftTypeId == 0)
            {
                EEmployeeShitList shiftLst = new EEmployeeShitList();
                shiftLst.EmployeeId = id;
                shiftLst.ShiftId = shiftId;
                shiftLst.AfterDays = 7;
                db.EmployeeShitList.Add(shiftLst);
                db.SaveChanges();

                if (weeklyoffArray != null)
                {
                    foreach (var item in weeklyoffArray)
                    {
                        db.EmployeeWOList.Add(new EEmployeeWOList()
                        {
                            EmployeeId = id,
                            OffDayId = item,
                        });
                        db.SaveChanges();
                    }
                }
            }
        }
        public void UpdateShift(int id, int? ShiftTypeId, int shiftId)
        {
            //delete shift list if exist
            var existingShifts = db.EmployeeShitList.Where(x => x.EmployeeId == id).ToList();
            if (existingShifts.Count > 0)
            {
                db.EmployeeShitList.RemoveRange(existingShifts);
                db.SaveChanges();
            }
            //add new shift and weekly off
            if (ShiftTypeId == 0)
            {
                EEmployeeShitList shiftLst = new EEmployeeShitList();
                shiftLst.EmployeeId = id;
                shiftLst.ShiftId = shiftId;
                shiftLst.AfterDays = 7;
                db.EmployeeShitList.Add(shiftLst);
                db.SaveChanges();
            }
        }
        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EEmployee model)
        {
            db.Employee.Remove(model);
            db.SaveChanges();
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "RemoveSuccess",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EmployeeVm> GetEmployeVm(int id)
        {

            var model = db.Employee.Where(x => x.Id == id).FirstOrDefault();
            var login = db.EmployeeLogin.Where(x => x.EmployeeId == model.Id).FirstOrDefault() ?? new EEmployeeLogin();
            if (model != null)
            {
                EmployeeVm vm = new EmployeeVm()
                {
                    AllowedEarlyOut = model.AllowedEarlyOut,
                    AllowedLateIn = model.AllowedLateIn,
                    BloodGroup = model.BloodGroup,
                    BranchId = model.BranchId,
                    Code = model.Code,
                    IsManager = model.IsManager,
                    ConsiderTimeLoss = model.ConsiderTimeLoss,
                    DateOfBirth = model.DateOfBirth,
                    DateOfJoin = model.DateOfJoin,
                    Designation = model.Designation,
                    DesignationId = model.DesignationId,
                    DeviceCode = model.DeviceCode,
                    Email = model.Email,
                    FourPunch = model.FourPunch,
                    Gender = model.Gender,
                    HalfDayMarking = model.HalfDayMarking,
                    HalfdayWorkingHour = model.HalfdayWorkingHour,
                    Id = model.Id,
                    ImageUrl = model.ImageUrl,
                    MaritialStatus = model.MaritialStatus,
                    MaxWorkingHour = model.MaxWorkingHour,
                    Mobile = model.Mobile,
                    MultiplePunch = model.MultiplePunch,
                    Name = model.Name,
                    NoPunch = model.NoPunch,
                    PermanentAddress = model.PermanentAddress,
                    PresentMarkingDuration = model.PresentMarkingDuration,
                    Section = model.Section,
                    SectionId = model.SectionId,
                    ShiftTypeId = model.ShiftTypeId,
                    ShortDayWorkingHour = model.ShortDayWorkingHour,
                    SinglePunch = model.SinglePunch,
                    TemporaryAddress = model.TemporaryAddress,
                    TwoPunch = model.TwoPunch,
                    WOTypeId = model.WOTypeId,
                    NameNp = model.NameNp,
                    PermanentAddressNp = model.PermanentAddressNp,
                    TemporaryAddressNp = model.TemporaryAddressNp,
                    PassportNo = model.PassportNo,
                    CitizenNo = model.CitizenNo,
                    IssueDate = model.IssueDate,
                    IssueDistict = model.IssueDistict,
                    Religion = model.Religion,
                    GradeGroup = model.GradeGroup,
                    GradeGroupId = model.GradeGroupId,
                    Password = login.Password,
                    UserName = login.UserName,
                    RoleId = login.RoleId,
                    IsOTAllowed = model.IsOTAllowed,
                    MinOTHour = model.MinOTHour,
                    MaxOTHour = model.MaxOTHour,
                    EmploymentStatus = model.EmploymentStatus,
                    ReportingManagerId = model.ReportingManagerId,
                    EnableSSN = model.EnableSSN,
                    PANNo = model.PANNo,
                    SSNNo = model.SSNNo,
                    BankAccountNo = model.BankAccountNo,
                    BankId = model.BankId,
                };


                #region getting shift
                if (model.ShiftTypeId == 0)
                {
                    EEmployeeShitList shift = db.EmployeeShitList.Where(x => x.EmployeeId == id).FirstOrDefault();
                    vm.ShiftId = (shift ?? new EEmployeeShitList() { ShiftId = 0 }).ShiftId;
                }
                #endregion
                #region Weekly off region
                vm.WeeklyOffIds = (from c in db.EmployeeWOList.Where(x => x.EmployeeId == id)
                                   select c.OffDayId
                                 ).ToArray();
                #endregion

                return new ServiceResult<EmployeeVm>()
                {
                    Data = vm,
                    Status = ResultStatus.Ok
                };
            }
            else
            {
                return new ServiceResult<EmployeeVm>()
                {
                    Message = "Invalid Employee Code...!!!",
                    Status = ResultStatus.processError
                };
            }
        }

        public IQueryable<EEmployeeShitList> ListEmpShift()
        {
            return db.EmployeeShitList;
        }

        public IQueryable<EEmployeeWOList> ListEmpWeeklyOff(int employeeId)
        {
            return db.EmployeeWOList.Where(x => x.EmployeeId == employeeId);
        }
        //operation of Employee Login
        public ServiceResult<EEmployeeLogin> AddEmployeeLogin(EEmployeeLogin model)
        {
            db.EmployeeLogin.Add(model);
            db.SaveChanges();
            return new ServiceResult<EEmployeeLogin>()
            {
                Data = model,
                Status = ResultStatus.Ok,
                Message = "AddedSuccess"
            };
        }

        public ServiceResult<EEmployee> UpdateEmployeeOnly(EEmployee employee)
        {
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return new ServiceResult<EEmployee>()
            {
                Data = employee,
                Status = ResultStatus.Ok,
                Message = "UpdatedSuccess"
            };
        }

        public ServiceResult<List<EEmployee>> UploadExcel(List<EEmployee> EmpLst, int branchId)
        {
            var shift = db.Shift.Where(x => x.BranchId == branchId).FirstOrDefault();
            if (shift != null)
            {
                var employees = db.Employee.Where(x => x.BranchId == branchId);
                List<EEmployee> EmpToSave = (from c in EmpLst
                                             where !(from o in employees
                                                     select o.DeviceCode).Contains(c.DeviceCode)
                                             select c).ToList();
                List<EEmployee> EmpToUpdate = (from c in EmpLst
                                               join d in employees on c.DeviceCode equals d.DeviceCode
                                               select new EEmployee()
                                               {
                                                   Id = d.Id,
                                                   Code = c.Code,
                                                   Name = c.Name,
                                                   NameNp = c.NameNp,
                                                   Gender = c.Gender,
                                                   PermanentAddress = c.PermanentAddress,
                                                   Mobile = c.Mobile,
                                                   DesignationId = c.DesignationId,
                                                   SectionId = c.SectionId
                                               }).ToList();
                if (EmpToSave.Count() > 0)
                {
                    for (int i = 0; i < EmpToSave.Count(); i++)
                    {
                        EEmployee emp = new EEmployee();
                        emp = EmpToSave[i];
                        db.Employee.Add(emp);
                        db.SaveChanges();
                        db.EmployeeShitList.Add(new EEmployeeShitList()
                        {
                            AfterDays = 7,
                            EmployeeId = emp.Id,
                            ShiftId = shift.Id
                        });
                        db.SaveChanges();
                        db.EmployeeWOList.Add(new EEmployeeWOList()
                        {
                            EmployeeId = emp.Id,
                            OffDayId = 6,
                        });
                        db.SaveChanges();
                    }

                }
                if (EmpToUpdate.Count() > 0)
                {
                    EEmployee emp = new EEmployee();
                    foreach (var item in EmpToUpdate)
                    {
                        emp = db.Employee.Where(x => x.Id == item.Id).FirstOrDefault();
                        emp.Code = item.Code;
                        emp.Name = item.Name;
                        emp.NameNp = item.NameNp;
                        emp.Gender = item.Gender;
                        emp.PermanentAddress = item.PermanentAddress;
                        emp.Mobile = item.Mobile;
                        emp.DesignationId = item.DesignationId;
                        emp.SectionId = item.SectionId;
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                return new ServiceResult<List<EEmployee>>()
                {
                    Data = null,
                    Status = ResultStatus.processError,
                    Message = "Please enter the shift to upload employee"
                };
            }
            return new ServiceResult<List<EEmployee>>()
            {
                Data = EmpLst,
                Message = "ImportSuccess",
                Status = ResultStatus.Ok
            };
        }

        public void Update(EEmployee employee)
        {
            employee.GradeGroup = null;
            employee.Section = null;

            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
        }

        public ServiceResult<bool> RemoveRange(List<EEmployee> empLst)
        {
            db.Employee.RemoveRange(empLst);
            db.SaveChanges();
            return new ServiceResult<bool>()
            {
                Data = true,
                Status = ResultStatus.Ok,
                Message = "RemoveSuccess"
            };
        }

        public void AddEmployeeShitList(List<EEmployeeShitList> dataToSave)
        {
            db.EmployeeShitList.AddRange(dataToSave);
            db.SaveChanges();

        }

        public ServiceResult<int> RemoveShiftListByEmployeeId(int EmployeeId)
        {
            var existingShiftList = db.EmployeeShitList.Where(x => x.EmployeeId == EmployeeId).ToList();
            if (existingShiftList.Count > 0)
            {
                existingShiftList.ForEach(x => clearRosterProxy(x));
                db.EmployeeShitList.RemoveRange(existingShiftList);
                db.SaveChanges();
            }
            return new ServiceResult<int>()
            {
                Data = 1,
                Message = "Removed Successfully",
                Status = ResultStatus.Ok
            };
        }
        private EEmployeeShitList clearRosterProxy(EEmployeeShitList x)
        {
            x.Employee = null;
            x.Shift = null;
            return x;
        }

        public ServiceResult<object> UpdateShiftType(ApplyShifttypeModel model)
        {
            var empLstToApplyShiftType = db.Employee.Where(f => model.EmpIds.Contains(f.Id)).ToList();
            empLstToApplyShiftType.ForEach(a => a.ShiftTypeId = model.ShiftTypeId);
            db.SaveChanges();
            return new ServiceResult<object>()
            {
                Message = "ShiftType Applied Successfully",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<object> UpdateDesignationPromotion(ApplyDesignationPromotiontypeModel model)
        {
            var empLstToApplyShiftType = db.Employee.Where(f => model.EmpIds.Contains(f.Id)).ToList();
            empLstToApplyShiftType.ForEach(a => a.DesignationId = model.PromotionDesignationId);
            db.SaveChanges();
            foreach (var item in empLstToApplyShiftType)
            {
                if (item.DesignationId != 0 || item.DesignationId != null)
                {
                    var existingHist = db.EmployeeDesignationHistory.Where(x => x.EmployeeId == item.Id && x.DesignationId == item.DesignationId).FirstOrDefault();
                    if (existingHist == null)
                    {
                        EEmployeeDesignationHistory hist = new EEmployeeDesignationHistory()
                        {
                            BranchId = (int)item.BranchId,
                            DesignationId = (int)item.DesignationId,
                            EmployeeId = item.Id,
                            DateTime = DateTime.Now,
                        };
                        db.EmployeeDesignationHistory.Add(hist);
                        db.SaveChanges();
                    }
                }
            }
            return new ServiceResult<object>()
            {
                Message = "Designation Applied Successfully",
                Status = ResultStatus.Ok
            };
        }

        public ServiceResult<EmployeeOTVm> ManageOt(EmployeeOTVm vm, int branchId)
        {
            if (string.IsNullOrEmpty(vm.EmpIds))
            {
                return new ServiceResult<EmployeeOTVm>()
                {
                    Data = null,
                    Message = "Process error",
                    Status = ResultStatus.processError,
                };
            }
            string[] employeeArray = vm.EmpIds.Split(',');
            var employees = (from c in db.Employee.Where(x => x.BranchId == branchId).ToList()
                             join d in employeeArray on c.Id equals int.Parse(d)
                             select c
                             ).ToList();
            employees.ForEach(x => x.IsOTAllowed = true);
            employees.ForEach(x => x.MaxOTHour = vm.OTModel.MaxOTHour);
            employees.ForEach(x => x.MinOTHour = vm.OTModel.MinOTHour);
            foreach (var item in employees)
            {
                db.Entry<EEmployee>(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            return new ServiceResult<EmployeeOTVm>()
            {
                Data = vm,
                Message = "UpdatedSuccess",
                Status = ResultStatus.Ok
            };
        }
    }
    public class EmployeeVm : EEmployee
    {
        public int ShiftId { get; set; }
        public int[] WeeklyOffIds { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? UserId { get; set; }
    }

    public class EmployeeFixedShiftModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ShiftId { get; set; }
    }
    public class ApplyShifttypeModel
    {
        public int ShiftTypeId { get; set; }
        public int[] EmpIds { get; set; }
    }

    public class ApplyDesignationPromotiontypeModel
    {
        public int PromotionDesignationId { get; set; }
        public int[] EmpIds { get; set; }
    }

    public class OTModel
    {
        public TimeSpan MaxOTHour { get; set; }
        public TimeSpan MinOTHour { get; set; }
    }
    public class EmployeeOTVm
    {
        public OTModel OTModel { get; set; }
        public string EmpIds { get; set; }
    }
}
