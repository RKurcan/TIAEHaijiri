using OfficeOpenXml;
using OfficeOpenXml.Style;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.HRM.Entities;
using Riddhasoft.HRM.Services;
using Riddhasoft.OfficeSetup.Services;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Areas.HRM.Controllers
{
    public class ContractController : Controller
    {
        //
        // GET: /HRM/Contract/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public void ExportEmpExcel(string filter)
        {
            SEmployee employeeServices = new SEmployee();
            SEmployeeDocument documentServices = new SEmployeeDocument(); 
            var empLst = employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            var docLst = documentServices.List().Data.Where(x => x.Employee.BranchId == RiddhaSession.BranchId).ToList();
            if (filter == "active")
            {
                empLst = empLst.Where(x => x.EmploymentStatus != EmploymentStatus.Resigned && x.EmploymentStatus != EmploymentStatus.Terminated).ToList();
            }
            else if (filter == "inactive")
            {
                empLst = empLst.Where(x => x.EmploymentStatus == EmploymentStatus.Resigned || x.EmploymentStatus == EmploymentStatus.Terminated).ToList();
            }
            var empDocLst = (from c in empLst
                             join d in docLst
                                on c.Id equals d.EmployeeId into doc
                                from p in doc.DefaultIfEmpty(new EEmployeeDocument())
                             select new EmployeeDocumentVm()
                             {
                                 Code = c.Code,
                                 Name = c.Name,
                                 PfNo = p.PFNo,
                                 CitNo = p.CITNo,
                                 BankAcNo = p.BankACNo,
                                 PanNo = p.PanNo
                             }).ToList();

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            //style for day
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
            workSheet.Row(1).Style.Font.Color.SetColor(Color.Black);
            /*header*/
            workSheet.Cells[1, 1].Value = "Code";
            workSheet.Cells[1, 2].Value = "Name";
            workSheet.Cells[1, 3].Value = "PF No";
            workSheet.Cells[1, 4].Value = "CIT No";
            workSheet.Cells[1, 5].Value = "Bank AC No";
            workSheet.Cells[1, 6].Value = "PAN No";
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[workSheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            for (int i = 0; i < empDocLst.Count(); i++)
            {
                workSheet.Cells[i + 2, 1].Value = empDocLst[i].Code;
                workSheet.Cells[i + 2, 2].Value = empDocLst[i].Name;
                workSheet.Cells[i + 2, 3].Value = empDocLst[i].PfNo;
                workSheet.Cells[i + 2, 4].Value = empDocLst[i].CitNo;
                workSheet.Cells[i + 2, 5].Value = empDocLst[i].BankAcNo;
                workSheet.Cells[i + 2, 6].Value = empDocLst[i].PanNo;
            }
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=ZktekoHajiriEmpInfo.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

        public class EmployeeDocumentVm
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string PfNo { get; set; }
            public string CitNo { get; set; }
            public string BankAcNo { get; set; }
            public string PanNo { get; set; }
        }
    }
}