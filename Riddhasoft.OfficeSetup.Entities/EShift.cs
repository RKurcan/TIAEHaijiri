using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class EShift
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string ShiftCode { get; set; }
        [StringLength(50), Required]
        public string ShiftName { get; set; }
        [StringLength(100)]
        public string NameNp { get; set; }
        public TimeSpan? EarlyGrace { get; set; }
        public TimeSpan? LateGrace { get; set; }
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public TimeSpan LunchStartTime { get; set; }
        public TimeSpan LunchEndTime { get; set; }
        public ShiftType ShiftType { get; set; }
        public int NumberOfStaff { get; set; }
        public int? BranchId { get; set; }
        public virtual EBranch Branch { get; set; }

        #region Short Day Working
        public bool ShortDayWorkingEnable { get; set; }
        public TimeSpan ShiftStartGrace { get; set; }
        public TimeSpan ShiftEndGrace { get; set; }
        public NepaliMonth StartMonth { get; set; }
        public int StartDays { get; set; }
        public NepaliMonth EndMonth { get; set; }
        public int EndDays { get; set; }
        #endregion

        #region Attendance rule
        public TimeSpan? HalfDayWorkingHour { get; set; }
        public bool DeclareAbsentForLateIn { get; set; }
        public bool DeclareAbsentForEarlyOut { get; set; }
        #endregion


    }
    public enum ShiftType
    {
        Morning = 0,
        Day = 1,
        Evening = 2,
        Night = 3,
        Dynamic = 4,
        DayOff = 5,
        NightOff = 6,
    }

    public enum NepaliMonth
    {
        Baishakh = 1,
        Jestha = 2,
        Asar = 3,
        Shrawan = 4,
        Bhadau = 5,
        Aswin = 6,
        Kartik = 7,
        Mansir = 8,
        Poush = 9,
        Magh = 10,
        Falgun = 11,
        Chaitra = 12
    }

}
