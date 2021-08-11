using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Report
{
    public partial class ReportViewerFrm : Form
    {
        private string reportPath;
        private string dataSetName;
        private DataTable reportData;
        List<ReportParameter> param = new List<ReportParameter>();
        public string ReportTitle { get; set; }
        public string CompanyName { get; set; }

        public ReportViewerFrm(string ReportPath, string DataSetName, DataTable ReportData)
        {
            InitializeComponent();
            this.reportPath = ReportPath;
            this.dataSetName = DataSetName;
            this.reportData = ReportData;
        }

        private void ReportViewerFrm_Load(object sender, EventArgs e)
        {
            showReportFromList();
            this.reportViewer.RefreshReport();
            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer.ZoomMode = ZoomMode.Percent;
            reportViewer.ZoomPercent = 100;
        }
        public void showReportFromList()
        {

            param.Add(new ReportParameter("CompanyName", CompanyName));
            param.Add(new ReportParameter("ReportTitle", ReportTitle));
            this.reportViewer.Reset();
            this.reportViewer.ProcessingMode = ProcessingMode.Local;
            this.reportViewer.LocalReport.ReportPath = AppDomain.CurrentDomain.BaseDirectory+@"\areas\" + reportPath;
            ReportDataSource reportDataSource = new ReportDataSource();

            reportViewer.LocalReport.SetParameters(param);
            reportDataSource.Name = dataSetName;
            reportDataSource.Value = reportData;
            this.reportViewer.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer.RefreshReport();
        }
    }
}
