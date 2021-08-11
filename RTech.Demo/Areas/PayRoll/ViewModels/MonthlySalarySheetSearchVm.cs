using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Areas.PayRoll.ViewModels
{
    public class MonthlySalarySheetSearchVm 
    {

        public string OnDate { get; set; }
        public string ToDate { get; set; }

        public string DepartmentIds { get; set; }   
        public string SectionIds { get; set; }
        public string EmpIds { get; set; }
    }
}