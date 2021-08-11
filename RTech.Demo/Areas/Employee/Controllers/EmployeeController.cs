using OfficeOpenXml;
using OfficeOpenXml.Style;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.OfficeSetup.Services;
using RTech.Demo.Filters;
using RTech.Demo.Models;
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
    [MenuFilter("2001")]
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/Employee/
        public ActionResult Index()
        {
            //return RedirectToAction("Test", "Home", new { area=""});
            return View();
        }


        [HttpGet]
        public void ExportEmployee()
        {
            SEmployee employeeServices = new SEmployee();
            int currentFiscalYear = RiddhaSession.FYId;
            var branch = RiddhaSession.CurrentUser.Branch;
            var branchId = branch.Id;
            var employeelist = (from c in employeeServices.List().Data.Where(x => x.BranchId == branchId)
                                select new EmployeeExcelExportModel()
                                {

                                    BadgeNumber = c.Code,
                                    Address = c.PermanentAddress,
                                    Designation = c.Designation.Name,
                                    Section = c.Section.Name,
                                    DeviceId = c.DeviceCode,
                                    Gender = c.Gender == Gender.Female ? "Female" : "Male",
                                    Mobile = c.Mobile,
                                    Name = c.Name,
                                    NameNepali = c.NameNp,

                                }).ToList();


            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
            workSheet.Row(1).Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 1].LoadFromCollection(employeelist, true);
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=EmployeeExcelDownload.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
        [HttpGet]
        public void ExportEmployeeFormatToSave()
        {
            SEmployee employeeServices = new SEmployee();
            var branch = RiddhaSession.CurrentUser.Branch;
            var branchId = branch.Id;
            var employeelist = employeeServices.List().Data.Where(x => x.BranchId == branchId).ToList();
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
            workSheet.Row(1).Style.Font.Color.SetColor(Color.Black);
            workSheet.Cells[1, 1].Value = "Device Code";
            workSheet.Cells[1, 2].Value = "Employee Code";
            workSheet.Cells[1, 3].Value = "Employee Name";
            workSheet.Cells[1, 4].Value = "Employee Name(Nepali)";
            workSheet.Cells[1, 5].Value = "Gender";
            workSheet.Cells[1, 6].Value = "Address";
            workSheet.Cells[1, 7].Value = "Mobile";
            workSheet.Cells[1, 8].Value = "Designation Code";
            workSheet.Cells[1, 9].Value = "Section Code";
            int rowIndex = 2;
            for (int i = 0; i < employeelist.Count(); i++)
            {
                workSheet.Cells[rowIndex, 1].Value = employeelist[i].DeviceCode;
                workSheet.Cells[rowIndex, 2].Value = employeelist[i].Code;
                workSheet.Cells[rowIndex, 3].Value = employeelist[i].Name;
                workSheet.Cells[rowIndex, 4].Value = employeelist[i].NameNp??"";
                workSheet.Cells[rowIndex, 5].Value =(int)employeelist[i].Gender;
                workSheet.Cells[rowIndex, 6].Value = employeelist[i].PermanentAddress??"";
                workSheet.Cells[rowIndex, 7].Value = employeelist[i].Mobile??"";
                workSheet.Cells[rowIndex, 8].Value = employeelist[i].Designation == null ? "" : employeelist[i].Designation.Code;
                workSheet.Cells[rowIndex, 9].Value = employeelist[i].Section == null ? "" : employeelist[i].Section.Code;
                workSheet.Column(i + 3).AutoFit(30, 30);
                rowIndex++;
            }
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=EmployeeExcelDownload.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
}