using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.Employee.Services;
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
    public partial class EmployeeSearchFrm : Form
    {
        public delegate void SearchComplete(EmployeeSearchVm Data);
        private SearchComplete delegateEmployee;
        public EmployeeSearchFrm()
        {
            InitializeComponent();
        }
        public EmployeeSearchFrm(SearchComplete EmployeeSearchComplete)
        {
            InitializeComponent();
            delegateEmployee = EmployeeSearchComplete;
            SEmployee employeeServices = new SEmployee();
            employeeSearchGridView.DataSource = (from c in employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId)
                                                 select new EmployeeSearchVm
                                                 {
                                                     Id = c.Id,
                                                     Department = c.Section == null ? "" : c.Section.Department.Name,
                                                     Designation = c.Designation == null ? "" : c.Designation.Name,
                                                     IDCardNo = c.Code,
                                                     Mobile = c.Mobile,
                                                     Name = c.DeviceCode + " - " + c.Name,
                                                 }).ToList();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SEmployee employeeServices = new SEmployee();
            employeeSearchGridView.DataSource = (from c in employeeServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId && x.Name.StartsWith(txtSearch.Text)).ToList()
                                                 select new EmployeeSearchVm
                                                 {
                                                     Id = c.Id,
                                                     Department = c.Section == null ? "" : c.Section.Department.Name,
                                                     Designation = c.Designation == null ? "" : c.Designation.Name,
                                                     IDCardNo = c.Code,
                                                     Mobile = c.Mobile,
                                                     Name = c.DeviceCode + " - " + c.Name,
                                                 }).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var employeeData = employeeSearchGridView.CurrentRow.DataBoundItem as EmployeeSearchVm;
            delegateEmployee(employeeData);
            this.Close();
        }

        private void EmployeeSearchFrm_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void employeeSearchGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
