using Microsoft.Reporting.WebForms;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace RTech.Demo.Areas.Report.Controllers
{
    public class ReportViewerController : Controller
    {
        //
        // GET: /Report/ReportViewer/
        public ActionResult Index()
        {
            return getPdf();
            //return View();
        }

        public FileResult getPdf()
        {
            ReportViewer reportViewer = new ReportViewer() { };
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportEmbeddedResource = TempData["ReportPath"] as string;


            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = TempData["ReportPath"] as string;
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", TempData["ReportData"] as DataTable));
            List<ReportParameter> param = new List<ReportParameter>();

            param.Add(new ReportParameter("CompanyName", RiddhaSession.CurrentUser.Branch.Company.Name));
            param.Add(new ReportParameter("ReportTitle", TempData["ReportTitle"].ToString()));

            reportViewer.LocalReport.SetParameters(param);

            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);

            reportViewer.LocalReport.Refresh();




            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string encoding;
            string extension;

            byte[] bytes = reportViewer.LocalReport.Render(TempData["type"].ToString(), null, out contentType, out encoding, out extension, out streamIds, out warnings);

            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = contentType;
            //Response.AppendHeader("Content-Disposition", "attachment; filename=RDLC." + extension);
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();

            return File(bytes, contentType, TempData["ReportTitle"].ToString() + '.' + extension);

        }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            //  Log.Error(filterContext.Exception);

            //Redirect or return a view, but not both.

            TempData["Stacktrace"] = filterContext.Exception.StackTrace;
            var title = "An Error Occured";
            filterContext.Result = Redirect("/error/index?Title=" + filterContext.Exception.Message + "&Message=" + filterContext.Exception.InnerException.Message);
        }

    }
}