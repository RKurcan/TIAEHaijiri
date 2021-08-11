using Riddhasoft.Device.Services;
using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.Employee.Services;
using Riddhasoft.HumanResource.Management.Report;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Report.ReportViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Views
{
    public partial class DashboardWidgetsMoreInfoFrm : Form
    {
        public DashboardWidgetsMoreInfoFrm(DashboardMoreInfoType type)
        {
            InitializeComponent();
            switch (type)
            {
                case DashboardMoreInfoType.ActiveDevice:
                    this.Text = "Device List";
                    break;
                case DashboardMoreInfoType.TotalEmployee:
                    this.Text = "Total Employee List";
                    break;
                case DashboardMoreInfoType.Present:
                    this.Text = "Present List";
                    break;
                case DashboardMoreInfoType.Absent:
                    this.Text = "Absent List";
                    break;
                case DashboardMoreInfoType.Department:
                    this.Text = "Department List";
                    break;
                case DashboardMoreInfoType.LateIn:
                    this.Text = "Late In List";
                    break;
                case DashboardMoreInfoType.OnLeave:
                    this.Text = "On Leave List";
                    break;
                case DashboardMoreInfoType.OfficeVisit:
                    this.Text = "Office Visit & Kaj List";
                    break;
                default:
                    break;
            }
            PopulateDashboardMoreInfo(type);
        }
        private void PopulateDashboardMoreInfo(DashboardMoreInfoType type)
        {
            SEmployee empServices = new SEmployee();
            SDepartment depServices = new SDepartment();
            SCompanyDeviceAssignment cmpDeviceServices = new SCompanyDeviceAssignment();
            SDevice deviceServices = new SDevice();
            SDailyEmployeePerformanceReport reportService = new SDailyEmployeePerformanceReport(RiddhaSession.BranchId, RiddhaSession.fiscalYearId);
            List<AttendanceReportDetailViewModel> reportList = new List<AttendanceReportDetailViewModel>();
            var result = reportService.GetAttendanceReportFromSp(System.DateTime.Now);
            reportList = result.Data;
            List<EmpInfoVm> vm = new List<EmpInfoVm>();
            switch (type)
            {
                case DashboardMoreInfoType.ActiveDevice:
                    var deviceList = deviceServices.List().Data.ToList();
                    var deviceData = (from c in deviceList
                                      select new DeviceInfo()
                                      {
                                          Name = c.Name,
                                          IPAddress = c.IpAddress,
                                          SerialNo = c.SerialNumber,
                                          Model = c.Model == null ? "" : c.Model.Name,
                                      }).ToList();
                    dashboardInfoGridView.DataSource = deviceData;
                    break;
                case DashboardMoreInfoType.TotalEmployee:
                    var empData = (from c in reportList
                                   select new EmpInfoVm()
                                   {
                                       Code = c.EmployeeCode,
                                       Name = c.EmployeeName,
                                       Department = c.DepartmentName,
                                       PlannedIn = c.PlannedTimeIn,
                                       PlannedOut = c.PlannedTimeOut,
                                       ActualIn = c.ActualTimeIn,
                                       ActualOut = c.ActualTimeOut,
                                   }).ToList();
                    dashboardInfoGridView.DataSource = empData;
                    break;
                case DashboardMoreInfoType.Present:
                    var presentData = (from c in reportList.Where(x => x.ActualTimeIn != "00:00")
                                       select new EmpInfoVm()
                                       {
                                           Code = c.EmployeeCode,
                                           Name = c.EmployeeName,
                                           Department = c.DepartmentName,
                                           PlannedIn = c.PlannedTimeIn,
                                           PlannedOut = c.PlannedTimeOut,
                                           ActualIn = c.ActualTimeIn,
                                           ActualOut = c.ActualTimeOut,
                                       }).ToList();
                    dashboardInfoGridView.DataSource = presentData;
                    break;
                case DashboardMoreInfoType.Absent:
                    var absentData = (from c in reportList.Where(x => x.ActualTimeIn == "00:00" && x.OnLeave == "No" && x.OfficeVisit == "NO" && x.Kaj == "NO")
                                      select new EmpInfoVm()
                                      {
                                          Code = c.EmployeeCode,
                                          Name = c.EmployeeName,
                                          Department = c.DepartmentName,
                                          PlannedIn = c.PlannedTimeIn,
                                          PlannedOut = c.PlannedTimeOut,
                                          ActualIn = c.ActualTimeIn,
                                          ActualOut = c.ActualTimeOut,
                                      }).ToList();
                    dashboardInfoGridView.DataSource = absentData;
                    break;
                case DashboardMoreInfoType.Department:
                    var depList = depServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
                    var depData = (from c in depList
                                   select new DepartmentInfo()
                                   {
                                       Code = c.Code,
                                       Name = c.Name,
                                       NumberOfStaff = c.NumberOfStaff,
                                   }).ToList();
                    dashboardInfoGridView.DataSource = depData;
                    break;
                case DashboardMoreInfoType.LateIn:
                    var lateInData = (from c in reportList.Where(x => x.LateIn != "00:00" && x.LateIn != "")
                                      select new LateInInfo()
                                      {
                                          Code = c.EmployeeCode,
                                          Name = c.EmployeeName,
                                          Department = c.DepartmentName,
                                          PlannedIn = c.PlannedTimeIn,
                                          ActualIn = c.ActualTimeIn,
                                          LateIn = GetLateTime(c.ActualTimeIn, c.PlannedTimeIn),
                                      }).ToList();
                    dashboardInfoGridView.DataSource = lateInData;
                    break;
                case DashboardMoreInfoType.OnLeave:
                    var onLeaveData = (from c in reportList.Where(x => x.LeaveName != null)
                                       select new LateInInfo()
                                       {
                                           Code = c.EmployeeCode,
                                           Name = c.EmployeeName,
                                           Department = c.DepartmentName,
                                           PlannedIn = c.PlannedTimeIn,
                                           ActualIn = c.ActualTimeIn,
                                           LateIn = GetLateTime(c.ActualTimeIn, c.PlannedTimeIn),
                                       }).ToList();
                    dashboardInfoGridView.DataSource = onLeaveData;
                    break;
                case DashboardMoreInfoType.OfficeVisit:
                    var officeVisitData = (from c in reportList.Where(x => x.OfficeVisit == "YES" || x.Kaj == "YES")
                                           select new OfficeVisitInfo()
                                           {
                                               Code = c.EmployeeCode,
                                               Name = c.EmployeeName,
                                               Department = c.DepartmentName,
                                               Remark = c.Remark
                                           }).ToList();
                    dashboardInfoGridView.DataSource = officeVisitData;
                    break;
                default:
                    break;

            }

        }
        private string GetLateTime(string ActualTimeIn, string PlannedTimeIn)
        {
            DateTime d1 = new DateTime();
            d1 = Convert.ToDateTime(ActualTimeIn); DateTime d2 = new DateTime();
            d2 = Convert.ToDateTime(PlannedTimeIn);
            TimeSpan ts = d1.Subtract(d2);
            return ts.ToString(@"hh\:mm");
        }
    }
    public class EmpInfoVm
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string PlannedIn { get; set; }
        public string PlannedOut { get; set; }
        public string ActualIn { get; set; }
        public string ActualOut { get; set; }

    }

    public class DepartmentInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int NumberOfStaff { get; set; }
    }

    public class DeviceInfo
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string SerialNo { get; set; }
        public string Model { get; set; }
    }

    public class LateInInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string PlannedIn { get; set; }
        public string ActualIn { get; set; }
        public string LateIn { get; set; }
    }

    public class OfficeVisitInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Remark { get; set; }
    }
}
