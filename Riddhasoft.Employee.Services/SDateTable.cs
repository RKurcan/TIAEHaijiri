using Riddhasoft.DB;
using Riddhasoft.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Services
{
    public class SDateTable : Riddhasoft.Services.Common.IBaseService<EDateTable>
    {
        RiddhaDBContext db = null;
        public SDateTable()
        {
            db = new RiddhaDBContext();
        }

        public Riddhasoft.Services.Common.ServiceResult<IQueryable<EDateTable>> List()
        {
            try
            {
                return new Riddhasoft.Services.Common.ServiceResult<IQueryable<EDateTable>>()
                {
                    Data = db.DateTable,
                    Status = Riddhasoft.Services.Common.ResultStatus.Ok
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Riddhasoft.Services.Common.ServiceResult<EDateTable> Add(EDateTable model)
        {
            try
            {
                db.DateTable.Add(model);
                db.SaveChanges();
                return new Riddhasoft.Services.Common.ServiceResult<EDateTable>()
                {
                    Data = model,
                    Status = Riddhasoft.Services.Common.ResultStatus.Ok

                };
            }
            catch (Exception)
            {
                return new Riddhasoft.Services.Common.ServiceResult<EDateTable>()
                {
                    Data = model,
                    Status = Riddhasoft.Services.Common.ResultStatus.dataBaseError
                };
            }
        }

        public string ConvertToNepDate(DateTime EngDate)
        {
            var NepDate = db.DateTable.Where(x => DbFunctions.TruncateTime(x.EngDate) == DbFunctions.TruncateTime(EngDate)).Select(x => x.NepDate).FirstOrDefault();
            if (NepDate != null)
            {
                var dates = NepDate.Split('/');
                //var FormatedDate = string.Format("{0}/{1}/{2}", dates[2], dates[1], dates[0]);
                return NepDate;
            }
            else
                return "";
        }

        public string ConverToNepDateForDesktop(DateTime EngDate)
        {
            var NepDate = db.DateTable.Where(x => DbFunctions.TruncateTime(x.EngDate) == DbFunctions.TruncateTime(EngDate)).Select(x => x.NepDate).FirstOrDefault();
            if (NepDate != null)
            {
                var dates = NepDate.Split('/');
                //var FormatedDate = string.Format("{0}/{1}/{2}", dates[2], dates[1], dates[0]);
                var FormatedDate = string.Format("{0}/{1}/{2}", dates[0], dates[1], dates[2]);
                return FormatedDate;
            }
            else
                return "";
        }

        public DateTime ConvertToEngDateForDesktop(string NepDate)
        {
            var dates = NepDate.Split('/');
            string FormatedDate = string.Format("{0}/{1}/{2}", dates[2], dates[1], dates[0]);
            return db.DateTable.Where(x => x.NepDate == FormatedDate).Select(x => x.EngDate).FirstOrDefault();
        }

        public DateTime ConvertToEngDate(string NepDate)
        {
            var dates = NepDate.Split('/');
            //string FormatedDate = string.Format("{0}/{1}/{2}", dates[2], dates[1], dates[0]);
            return db.DateTable.Where(x => x.NepDate == NepDate).Select(x => x.EngDate).FirstOrDefault();
        }

        public Riddhasoft.Services.Common.ServiceResult<int> AddRange(List<EDateTable> models)
        {

            try
            {
                db.DateTable.AddRange(models);
                db.SaveChanges();
                return new Riddhasoft.Services.Common.ServiceResult<int>()
                {
                    Data = 1,
                    Status = Riddhasoft.Services.Common.ResultStatus.Ok
                };
            }
            catch (Exception)
            {
                return new Riddhasoft.Services.Common.ServiceResult<int>()
                {
                    Data = 0,
                    Status = Riddhasoft.Services.Common.ResultStatus.dataBaseError
                };
            }
        }

        public Riddhasoft.Services.Common.ServiceResult<EDateTable> Update(EDateTable model)
        {
            throw new NotImplementedException();
        }

        public Riddhasoft.Services.Common.ServiceResult<int> Remove(EDateTable model)
        {
            throw new NotImplementedException();
        }

        public List<string> GetNepaliMonths(string lang = "en")
        {
            List<string> result = new List<string>();
            if (lang == "ne")
            {
                result = new List<string>() { "बैशाख", "जेठ ", "असार", "श्रावण", "भदौ", "असोज", "कार्तिक", "मंसिर", "पुष", "माघ", "फाल्गुन", "चैत्र" };
            }
            else
            {
                result = new List<string>() { "Baisakh", "Jestha", "Ashad", "Shrawan", "Bhadra", "Asoj", "Kartik", "Mangshir", "Poush", "Magh", "Falgun", "Chaitra" };
            }

            return result;
            //return new List<string>() { "Baisakh", "Jestha", "Ashad", "Shrawan", "Bhadra", "Asoj", "Kartik", "Mangshir", "Poush", "Magh", "Falgun", "Chaitra" };
        }

        public List<string> GetEnglishMonths(string lang = "en")
        {
            List<string> result = new List<string>();
            if (lang == "ne")
            {
                result = new List<string>() { "जनवरी", "फेब्रुअरी", "मार्च", "अप्रिल", "मे", "जून", "जुलाई", "अगस्ट", "सेप्टेम्बर", "अक्टोबर", "नोभेम्बर", "डिसेम्बर" };
            }
            else
            {
                result = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            }

            return result;
            //return new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        }

        public List<EDateTable> GetDaysInNepaliMonth(int year, int month)
        {
            var MonthdateString = string.Format("{0}/{1}", year, month.ToString().PadLeft(2, '0'));
            var dates = db.DateTable.Where(x => x.NepDate.StartsWith(MonthdateString)).ToList();
            return dates;
        }

        public List<EDateTable> GetDaysInEnglishMonth(int year, int month)
        {
            DateTime FromDate = new DateTime(year, month, 1);
            DateTime ToDate = FromDate.AddMonths(1).AddDays(-1);
            var MonthdateString = string.Format("{0}/{1}", year, month.ToString().PadLeft(2, '0'));
            var dates = db.DateTable.Where(x => x.EngDate >= FromDate && x.EngDate <= ToDate).ToList();
            return dates;
        }

        public int GetMaxDateEnglishMonth(int year, int month)
        {
            DateTime FromDate = new DateTime(year, month, 1);
            DateTime ToDate = FromDate.AddMonths(1).AddDays(-1);
            var MonthdateString = string.Format("{0}/{1}", year, month.ToString().PadLeft(2, '0'));
            var dates = db.DateTable.Where(x => x.EngDate >= FromDate && x.EngDate <= ToDate).ToList();
            return dates.Count;
        }

        public int GetMaxDateNepaliMonth(int year, int month)
        {
            var MonthdateString = string.Format("{0}/{1}", year, month.ToString().PadLeft(2, '0'));
            var dates = db.DateTable.Where(x => x.NepDate.StartsWith(MonthdateString)).ToList();
            return dates.Count();
        }

        public EDateTable GetFirstDayInEnglishMonth(int year, int month)
        {
            DateTime FromDate = new DateTime(year, month, 1);
            var dates = db.DateTable.Where(x => x.EngDate == FromDate).ToList();
            return dates.First();
        }

        public EDateTable GetLastDayInEnglishMonth(int year, int month)
        {
            DateTime FromDate = new DateTime(year, month, GetMaxDateEnglishMonth(year, month));
            var dates = db.DateTable.Where(x => x.EngDate == FromDate).ToList();
            return dates.Last();
        }

        public EDateTable GetFirstDayInNepaliMonth(int year, int month)
        {
            var MonthdateString = string.Format("{0}/{1}", year, month.ToString().PadLeft(2, '0'));
            var dates = db.DateTable.Where(x => x.NepDate.StartsWith(MonthdateString)).ToList();
            return dates.FirstOrDefault();
        }
        public string GetLastDayInNepaliMonth(int year, int month)
        {
            var date = string.Format("{0}/{1}/{2}", year, month.ToString().PadLeft(2, '0'), GetMaxDateNepaliMonth(year, month).ToString().PadLeft(2, '0'));
            return date;
        }
        public List<string> GetDaysInWeek()
        {
            return new List<string>() { 
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"
            };
        }

    }
}
