using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EHoliday
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100), Required]
        public string Name { get; set; }
        [StringLength(200)]
        public string NameNp { get; set; }
        public string Description { get; set; }
        public HolidayType HolidayType { get; set; }
        public bool IsOccuredInSameDate { get; set; }
        public ApplicableGender ApplicableGender { get; set; }
        public ApplicableReligion ApplicableReligion { get; set; }

        public int? BranchId { get; set; }
        public virtual EBranch Branch { get; set; }
    }

    public class EHolidayDetails
    {
        public int Id { get; set; }
        public int HolidayId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public int FiscalYearId { get; set; }
        public virtual EHoliday Holiday { get; set; }
        public virtual EFiscalYear FiscalYear { get; set; }
    }

    public class EDepartmentWiseHoliday
    {
        public int Id { get; set; }
        public int HolidayId { get; set; }
        public int SectionId { get; set; }

        public virtual ESection Section { get; set; }
        public virtual EHoliday Holiday { get; set; }
    }

    /// <summary>
    ///  All = 0,
    ///Male = 1,
    ///    Female = 2,
    ///    Others = 3
    /// </summary>
    public enum ApplicableGender
    {
        All = 0,
        Male = 1,
        Female = 2,
        Others = 3
    }
    public enum ApplicableReligion
    {
        All = 0,
        Hinduism = 1,
        Buddhism = 2,
        Islam = 3,
        Judaism = 4,
        Christianity = 5,
    }
    public enum HolidayType
    {
        Religious,
        NonReligious
    }
}
