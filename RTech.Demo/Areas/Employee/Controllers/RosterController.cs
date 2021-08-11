using OfficeOpenXml;
using OfficeOpenXml.Style;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using RTech.Demo.Filters;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.Employee.Controllers
{
    [MenuFilter("2002")]
    public class RosterController : Controller
    {
        SRoster rosterServices = new SRoster();
        //
        // GET: /Employee/Roster/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public void ExportRosterExcel(int year, int monthId, string SectionIds)
        {
            string lang = RiddhaSession.Language;
            RosterVM vm = new RosterVM();
            SDateTable service = new SDateTable();
            var totalDays = 0;
            var dateList = new List<EDateTable>();
            string monthName = "";
            int compId=RiddhaSession.CompanyId;
            string companyName = new SCompany().List().Data.Where(x => x.Id == compId).FirstOrDefault().Name;
            switch (RiddhaSession.OperationDate)
            {
                case "ne":
                    dateList = service.GetDaysInNepaliMonth(year, monthId);
                    totalDays = dateList.Count;
                    monthName = service.GetNepaliMonths(lang)[monthId - 1];
                    break;
                case "en":
                    dateList = service.GetDaysInEnglishMonth(year, monthId);
                    totalDays = dateList.Count;
                    monthName = service.GetEnglishMonths(lang)[monthId - 1];
                    break;
                default:
                    break;
            }

            List<DateTableViewModel> days = new List<DateTableViewModel>();
            for (int i = 1; i <= totalDays; i++)
            {
                DateTableViewModel dt = new DateTableViewModel();
                dt.Id = i;
                dt.NepDate = dateList[i - 1].NepDate;
                dt.EngDate = dateList[i - 1].EngDate;
                dt.DayName = Enum.GetName(typeof(DayOfWeek), dt.EngDate.DayOfWeek);
                days.Add(dt);
            }
            vm.Year = year;
            vm.MonthId = monthId;
            DateTime minDate = dateList.First().EngDate;
            DateTime maxDate = dateList.Last().EngDate;
            var RosterRawData = (from c in rosterServices.List().Data
                                 where c.Date >= minDate && c.Date <= maxDate
                                 select c
                                     ).ToList();
            //foreach (var employeeId in (from c in RosterRawData select c.EmployeeId).Distinct().ToList())
            SEmployee employeeServices = new SEmployee();
            //prepare section
            var sections = (from c in SectionIds.Split(',')
                            select c.ToInt()
                               ).Distinct().ToArray();
            //list employee
            var Employees = (from c in employeeServices.List().Data.Where(x => x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated)
                             join d in sections
                             on c.SectionId equals d
                             where c.ShiftTypeId == 2
                             select c
                               ).ToList();
            //prepare rosterSheet

            foreach (var employee in Employees)
            {
                vm.RosterRows.Add(new RosterRow()
                {
                    EmployeeId = employee.Id.ToInt(),
                    EmployeeName = employee.Name,
                    EmployeeCode=employee.Code,
                    Columns = (from c in days
                               join d in RosterRawData.Where(x => x.EmployeeId == employee.Id).ToList()
                               on c.EngDate equals d.Date into joined
                               from j in joined.DefaultIfEmpty()
                               select new ColumnsModel()
                               {
                                   Day = c.Id,
                                   DayName = c.DayName,
                                   ShiftId = j == null ? 0 : j.ShiftId,
                                   ShiftCode=j==null?"":j.Shift.ShiftCode
                               }
                                 ).ToList()
                });
            }
            ExcelPackage excel = new ExcelPackage();
            string text =companyName+"\n Monthly Duty Roster for  " + monthName + "  " + year.ToString();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1, 1, totalDays+2].Merge = true;
            workSheet.Cells[1, 1, 1, totalDays + 2].Style.WrapText = true;
            workSheet.Cells[1, 1, 1, 15].Style.Font.Bold = true;
            workSheet.Cells[1, 1, 1, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 1, 1, 15].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 1, 1, 15].Style.Font.Size = 16;
            workSheet.Row(1).CustomHeight = true;
            workSheet.Row(1).Height = 63;
            workSheet.Cells[1, 1, 1, 15].Value = text;
            

            //style for day
            workSheet.Row(2).Style.Font.Bold = true;
            workSheet.Row(2).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(2).Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
            workSheet.Row(2).Style.Font.Color.SetColor(Color.Black);

            //style for dayname
            workSheet.Row(3).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(3).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            /*header*/
            workSheet.Cells[2, 1].Value = "Code";
            workSheet.Cells[2, 2].Value = "Name";

            for (int i = 0; i < totalDays; i++)
            {
                workSheet.Cells[2, i + 3].Value = i + 1;
                workSheet.Cells[3, i + 3].Value = days[i].DayName.Substring(0, 3);
                workSheet.Column(i + 3).AutoFit(34, 40);
            }
            int k = 4, l = 1;
            foreach (var item in vm.RosterRows)
            {


                workSheet.Cells[k, 1].Value = item.EmployeeCode;
                workSheet.Cells[k, 2].Value = item.EmployeeName;
                l = 2;
                foreach (var col in item.Columns)
                {
                    workSheet.Cells[k, ++l].Value = col.ShiftCode;
                }
                k = k + 1;
            }
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Hamrohajiri_Duty_Roster_"+monthName+".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    public class ExcelRosterModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}