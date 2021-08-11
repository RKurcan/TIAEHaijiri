using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    public partial class EmployeeViewDetailsFrm : Form
    {
        public EmployeeViewDetailsFrm(int employeeId = 0)
        {
            InitializeComponent();
            SEmployee employeeServices = new SEmployee();
            var emp = employeeServices.List().Data.Where(x => x.Id == employeeId).FirstOrDefault();
            setValuetoInputs(emp);
        }

        private void setValuetoInputs(EEmployee emp)
        {
            lblName.Text = emp.Name;
            lblDepartment.Text = emp.Section == null ? "" : emp.Section.Department.Name;
            lblSection.Text = emp.Section == null ? "" : emp.Section.Name;
            lblEmployeeCode.Text = emp.Code;
            lblDateOfJoin.Text = emp.DateOfJoin.HasValue ? emp.DateOfJoin.Value.ToString("yyyy/MM/dd") : "";
            lblDesignation.Text = emp.Designation.Name;
            lblDeviceCode.Text = emp.DeviceCode.ToString();
            lblDateOfBirth.Text = emp.DateOfBirth.HasValue ? emp.DateOfBirth.Value.ToString("yyyy/MM/dd") : "";
            lblMobile.Text = emp.Mobile;
            lblAddress.Text = emp.PermanentAddress;
            lblBloodGroup.Text = Enum.GetName(typeof(BloodGroup), emp.BloodGroup);
            lblPunchType.Text = getEmpPunchType(emp);
            lblMaxWorkingHour.Text = emp.MaxWorkingHour.ToString(@"hh\:mm");
            lblShiftType.Text = emp.ShiftTypeId == 0 ? "Fixed" : "Dynamic";
            lblShift.Text = getEmpShift(emp.Id);
            if (!string.IsNullOrEmpty(emp.ImageUrl))
            {
                var stream = new FileStream(emp.ImageUrl, FileMode.Open, FileAccess.Read);
                empPcb.BackgroundImage = Image.FromStream(stream);
                empPcb.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                empPcb.BackgroundImage = Riddhasoft.EHajiriPro.Desktop.Properties.Resources.men3;
                empPcb.BackgroundImageLayout = ImageLayout.Stretch;
            }

        }

        private string getEmpShift(int Id)
        {
            SEmployee empServices = new SEmployee();
            var employeeShift = empServices.ListEmpShift().Where(x => x.EmployeeId == Id).FirstOrDefault();
            return employeeShift == null ? "" : employeeShift.Shift.ShiftName;
        }

        private string getEmpPunchType(EEmployee emp)
        {
            if (emp.NoPunch)
            {
                return "No Punch";
            }
            else if (emp.SinglePunch)
            {
                return "Single Punch";
            }
            else if (emp.FourPunch)
            {
                return "Four Punch";
            }
            else if (emp.MultiplePunch)
            {
                return "Multiple Punch";
            }
            return "Two Punch";
        }
        class OvalPictureBox : PictureBox
        {
            public OvalPictureBox()
            {
                this.BackColor = Color.DarkGray;
            }
            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                using (var gp = new GraphicsPath())
                {
                    gp.AddEllipse(new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                    this.Region = new Region(gp);
                }
            }
        }

        private void EmployeeViewDetailsFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
