using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.EHajiriPro.Desktop.Common
{
    public class RiddhaSession
    {
        public static int BranchId { get; set; }
        public static int CompanyId { get; set; }
        public static int fiscalYearId { get; set; }

        public static int UserId { get; set; }

        public static string Language { get; set; }

        public static string CompanyName { get; set; }
        public static string OpreationDate { get; set; }

        public static string CompanyContact { get; set; }

        public static string CompanyAddress { get; set; }

        public static string UserName { get; set; }
    }
}
