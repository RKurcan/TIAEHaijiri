using Riddhasoft.EHajiriPro.Desktop.Areas.Leave;
using Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management;
using Riddhasoft.EHajiriPro.Desktop.Areas.Payroll;
using Riddhasoft.EHajiriPro.Desktop.Areas.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using Riddhasoft.Employee.Services;
using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.Report.ReportViewModel;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Device.Services;
using System.Globalization;

namespace Riddhasoft.EHajiriPro.Desktop.Views
{
    public partial class Home : Form
    {
        Timer t = new Timer();
        private readonly int branchId = (int)RiddhaSession.BranchId;
        public Home()
        {
            InitializeComponent();
            SetCompanyInfo();
            getDashboarCount();
            btnAbsentMoreInfo.Focus();
           
        }

        private void SetCompanyInfo()
        {
            this.Text = RiddhaSession.CompanyName + " , " + RiddhaSession.CompanyAddress;
            if (RiddhaSession.OpreationDate == "ne")
            {
                lblDate.Text = "BS " + NepaliDateExtension.ToNepaliDate(DateTime.Now);
            }
            else
            {
                lblDate.Text = "AD " + System.DateTime.Now.ToString("dd/MM/yyyy");
            }
            lblLoginUser.Text = RiddhaSession.UserName;
        }
        public void getDashboarCount()
        {
            DashboardVm vm = new DashboardVm();
            SEmployee empServices = new SEmployee();
            SDepartment depServices = new SDepartment();
            SDevice deviceServices = new SDevice();
            SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(RiddhaSession.BranchId, RiddhaSession.fiscalYearId);
            List<AttendanceReportDetailViewModel> reportList = new List<AttendanceReportDetailViewModel>();

            var result = reportService.GetAttendanceReportFromSp(DateTime.Now);
            reportList = result.Data;
            if (reportList.Count() > 0)
            {
                int[] empIds = new SEmployee().List().Data.Where(x => x.BranchId == branchId).Select(x => x.Id).ToArray();
                reportList = (from c in reportList
                              join d in empIds
                              on c.EmployeeId equals d
                              select c
                              ).ToList();
            }

            vm.PresentCount = reportList.Count(x => x.ActualTimeIn != "00:00");
            vm.AbsentCount = reportList.Count(x => x.ActualTimeIn == "00:00" && x.OnLeave == "No" && x.OfficeVisit == "NO" && x.Kaj == "NO");
            vm.OnLeaveCount = reportList.Count(x => x.LeaveName != null);
            vm.LateInCount = reportList.Count(x => x.LateIn != "00:00" && x.LateIn != "");
            vm.OffiveVisitCount = reportList.Count(x => x.OfficeVisit == "YES" || x.Kaj == "YES");
            vm.DeviceCount = deviceServices.List().Data.Count();
            //vm.EmployeeCount = empServices.List().Data.Count();
            vm.EmployeeCount = reportList.Count();
            vm.DepartmentCount = depServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).Count();
            lblTotalEmployeeCount.Text = vm.EmployeeCount.ToString();
            lblPresentCount.Text = vm.PresentCount.ToString();
            lblAbsentCount.Text = vm.AbsentCount.ToString();
            lblDepartmentCount.Text = vm.DepartmentCount.ToString();
            lblLateInCount.Text = vm.LateInCount.ToString();
            lblOnLeaveCount.Text = vm.OnLeaveCount.ToString();
            lblOfficeVisitCount.Text = vm.OffiveVisitCount.ToString();
            lblAbsentGridCount.Text = vm.AbsentCount.ToString();
            lblOnLeaveGridCount.Text = vm.OnLeaveCount.ToString();
            lblOfficeVisitGridCount.Text = vm.OffiveVisitCount.ToString();
            lblTotalDeviceCount.Text = vm.DeviceCount.ToString();

            vm.EmployeeList = (from c in reportList.Where(x => x.Absent.ToLower() == "yes" && x.ActualTimeIn == "00:00" && x.OfficeVisit == "NO" && x.LeaveName == null)
                               select new DashboarCountGridVm()
                               {
                                   Department = c.DepartmentName,
                                   Employee = c.EmployeeName,
                               }).ToList();
            absentGridView.DataSource = vm.EmployeeList;

            vm.OnLeaveList = (from c in reportList.Where(x => !string.IsNullOrEmpty(x.LeaveName))
                              select new DashboarCountGridVm()
                              {
                                  Department = c.DepartmentName,
                                  Employee = c.EmployeeName,
                              }).ToList();
            onLeaveGridView.DataSource = vm.OnLeaveList;

            vm.OfficeVisitList = (from c in reportList.Where(x => x.OfficeVisit == "YES")
                                  select new DashboarCountGridVm()
                                  {
                                      Department = c.DepartmentName,
                                      Employee = c.EmployeeName,
                                  }).ToList();
            officeVisitGridView.DataSource = vm.OfficeVisitList;

            #region Progress Bar
            progressBarPresent.Value = vm.PresentCount * 100 / vm.EmployeeCount;
            lblPresentCountPB.Text = vm.PresentCount.ToString();
            lblTotalEmployeePresent.Text = vm.EmployeeCount.ToString();

            lblAbsentCountPB.Text = vm.AbsentCount.ToString();
            progressBarAbsent.Value = vm.AbsentCount * 100 / vm.EmployeeCount;
            lblTotalEmployeeAbsent.Text = vm.EmployeeCount.ToString();
            lblAbsentCountPB.Text = vm.AbsentCount.ToString();

            progressBarOfficeVisit.Value = vm.OffiveVisitCount * 100 / vm.EmployeeCount;
            lblOfficeVisitCountPB.Text = vm.OffiveVisitCount.ToString();
            lblTotalEmployeeOfficeVisit.Text = vm.EmployeeCount.ToString();

            progressBarOnLeave.Value = vm.OnLeaveCount * 100 / vm.EmployeeCount;
            lblOnLeaveCountPB.Text = vm.OnLeaveCount.ToString();
            lblTotalEmployeeOnLeave.Text = vm.EmployeeCount.ToString();

            progressBarLateIn.Value = vm.LateInCount * 100 / vm.EmployeeCount;
            lblLateInCountPB.Text = vm.LateInCount.ToString();
            lblTotalEmployeeLateIn.Text = vm.EmployeeCount.ToString();
            #endregion

            if (absentGridView.Rows.Count == 0)
            {
                lblAbsentNoRecord.Visible = true;
            }
            else
            {
                lblAbsentNoRecord.Visible = false;
            }
            if (onLeaveGridView.Rows.Count == 0)
            {
                lblOnLeaveRecord.Visible = true;
            }
            else
            {
                lblOnLeaveRecord.Visible = false;
            }
            if (officeVisitGridView.Rows.Count == 0)
            {
                lblOfficeVisitRecord.Visible = true;
            }
            else
            {
                lblOfficeVisitRecord.Visible = false;
            }

        }

        private void eventHandelr(object sender, EventArgs e)
        {
        }
        private void dataManaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Areas.Device.DeviceDataManagementFrm frm = new Areas.Device.DeviceDataManagementFrm();
            frm.ShowDialog();

        }

        private void fiscalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FiscalYearFrm frm = new FiscalYearFrm(eventHandelr);
            frm.ShowDialog();
        }

        private void companyProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompanyProfile frm = new CompanyProfile();
            frm.ShowDialog();
        }

        private void openform<T>(Form frm) where T : new()
        {
            frm.ShowDialog();
        }

        private void branchManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BranchFrm frm = new BranchFrm();
            frm.ShowDialog();
        }

        private void departmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DepartmentFrm frm = new DepartmentFrm(eventHandelr);
            frm.ShowDialog();
        }

        private void sectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SectionFrm frm = new SectionFrm(eventHandelr);
            frm.ShowDialog();
        }

        private void designationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DesignationFrm frm = new DesignationFrm(eventHandelr);
            frm.ShowDialog();
        }

        private void shiftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShiftFrm frm = new ShiftFrm();
            frm.ShowDialog();
        }

        private void deviceSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Areas.Device.DeviceSetupFrm frm = new Areas.Device.DeviceSetupFrm();
            frm.ShowDialog();
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeFrm frm = new EmployeeFrm();
            frm.ShowDialog();
        }

        private void dynamicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonthlyRosterSetupFrm frm = new MonthlyRosterSetupFrm();
            frm.ShowDialog();
        }

        private void weeklyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WeeklyRosterSetupFrm frm = new WeeklyRosterSetupFrm();
            frm.ShowDialog();
        }

        private void entryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OfficeVisitFrm frm = new OfficeVisitFrm();
            frm.ShowDialog();
        }

        private void approvalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OfficeVisitApprovalFrm frm = new OfficeVisitApprovalFrm();
            frm.ShowDialog();
        }

        private void applicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeaveApplicationFrm frm = new LeaveApplicationFrm();
            frm.ShowDialog();
        }

        private void fixedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeaveApprovalFrm frm = new LeaveApprovalFrm();
            frm.ShowDialog();
        }

        private void leaveQuotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DesignationFrm frm = new DesignationFrm(eventHandelr);
            frm.ShowDialog();
        }

        private void leaveOpeningBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeaveOpeningBalanceForm frm = new LeaveOpeningBalanceForm();
            frm.ShowDialog();
        }

        private void leaveSettlementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeaveSettlementForm frm = new LeaveSettlementForm();
            frm.ShowDialog();
        }

        private void leaveMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LeaveMasterSetup frm = new LeaveMasterSetup();
            frm.ShowDialog();
        }

        private void holidayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HolidaySetupFrm frm = new HolidaySetupFrm();
            frm.ShowDialog();
        }

        private void payrollSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PayrollSetupFrm frm = new PayrollSetupFrm();
            frm.ShowDialog();
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manualPunchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualPunchFrm frm = new ManualPunchFrm();
            frm.ShowDialog();
        }

        private void kajToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Home_Load(object sender, EventArgs e)
        {
            btnAbsentMoreInfo.Focus();
            getDashboarCount();
            SetCompanyInfo();
            Clock();
        }

        private void Clock()
        {
            //timer interval
            t.Interval = 1000;  //in milliseconds

            t.Tick += new EventHandler(this.t_Tick);

            //start timer when form loads
            t.Start();  //this will use t_Tick() method
        }
        private void t_Tick(object sender, EventArgs e)
        {
            //get current time
            int hh = DateTime.Now.Hour;
            int mm = DateTime.Now.Minute;
            int ss = DateTime.Now.Second;


            //time
            string time = "";

            //padding leading zero
            if (hh < 10)
            {
                time += "0" + hh;
            }
            else
            {
                time += hh;
            }
            time += ":";

            if (mm < 10)
            {
                time += "0" + mm;
            }
            else
            {
                time += mm;
            }
            time += ":";

            if (ss < 10)
            {
                time += "0" + ss;
            }
            else
            {
                time += ss;
            }

            //update label
            lblClock.Text = "- " + time + " " + DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getDashboarCount();
            SetCompanyInfo();
            if (absentGridView.Rows.Count == 0)
            {
                lblAbsentNoRecord.Visible = true;
            }
            if (onLeaveGridView.Rows.Count == 0)
            {
                lblOnLeaveRecord.Visible = true;
            }
            if (officeVisitGridView.Rows.Count == 0)
            {
                lblOfficeVisitRecord.Visible = true;
            }
        }

        private void lblTotalEmpInfo_Click(object sender, EventArgs e)
        {
            DashboardWidgetsMoreInfoFrm frm = new DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType.TotalEmployee);
            frm.ShowDialog();
        }

        private void lblDepartmentInfo_Click(object sender, EventArgs e)
        {
            DashboardWidgetsMoreInfoFrm frm = new DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType.Department);
            frm.ShowDialog();
        }

        private void lblTotaldeviceInfo_Click(object sender, EventArgs e)
        {
            DashboardWidgetsMoreInfoFrm frm = new DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType.ActiveDevice);
            frm.ShowDialog();
        }

        private void lblPresentInfo_Click(object sender, EventArgs e)
        {
            DashboardWidgetsMoreInfoFrm frm = new DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType.Present);
            frm.ShowDialog();
        }

        private void lblAbsetntInfo_Click(object sender, EventArgs e)
        {
            DashboardWidgetsMoreInfoFrm frm = new DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType.Absent);
            frm.ShowDialog();
        }

        private void lblLateInInfo_Click(object sender, EventArgs e)
        {
            DashboardWidgetsMoreInfoFrm frm = new DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType.LateIn);
            frm.ShowDialog();
        }

        private void lblOnLeaveInfo_Click(object sender, EventArgs e)
        {
            DashboardWidgetsMoreInfoFrm frm = new DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType.OnLeave);
            frm.ShowDialog();
        }

        private void lblOfficeVisitInfo_Click(object sender, EventArgs e)
        {
            DashboardWidgetsMoreInfoFrm frm = new DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType.OfficeVisit);
            frm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void atendanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Areas.Report.AttendanceReportFrm frm = new Areas.Report.AttendanceReportFrm();
            frm.ShowDialog();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Areas.User_Management.UserFrm frm = new Areas.User_Management.UserFrm();
            frm.ShowDialog();
        }

        private void payrollReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Areas.Report.LeaveReportDailougeFrm frm = new Areas.Report.LeaveReportDailougeFrm();
            frm.ShowDialog();
        }
    }

    public enum DashboardMoreInfoType
    {
        ActiveDevice,
        TotalEmployee,
        Present,
        Absent,
        Department,
        LateIn,
        OnLeave,
        OfficeVisit,
    }

}
