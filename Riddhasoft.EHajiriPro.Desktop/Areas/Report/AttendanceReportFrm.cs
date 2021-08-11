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
    public partial class AttendanceReportFrm : Form
    {
        public AttendanceReportFrm()
        {
            InitializeComponent();
        }

        private void btnDailyAttendance_Click(object sender, EventArgs e)
        {
            DailyAttendanceReportDailougeFrm frm = new DailyAttendanceReportDailougeFrm();
            frm.ShowDialog();
        }

        private void btnMonthlyAttendance_Click(object sender, EventArgs e)
        {
            MonthlyAttendanceReportDailougeFrm frm = new MonthlyAttendanceReportDailougeFrm();
            frm.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DailyAttendanceReportDailougeFrm frm = new DailyAttendanceReportDailougeFrm();
            frm.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MonthlyAttendanceReportDailougeFrm frm = new MonthlyAttendanceReportDailougeFrm();
            frm.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            DailyAttendanceReportDailougeFrm frm = new DailyAttendanceReportDailougeFrm();
            frm.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MonthlyAttendanceReportDailougeFrm frm = new MonthlyAttendanceReportDailougeFrm();
            frm.ShowDialog();
        }
    }
}
