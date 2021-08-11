using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.OfficeSetup.Entities;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    public partial class CompanyProfile : Form
    {
        ECompany _companyData = null;
        int companyId = RiddhaSession.CompanyId;
        public CompanyProfile()
        {
            InitializeComponent();
            _companyData = new ECompany();
            setInputValue();
        }

        private void CompanyProfile_Load(object sender, EventArgs e)
        {
            setInputValue();
        }

        public void setInputValue()
        {
            SCompany companyServices = new SCompany();
            var data = companyServices.List().Data.Where(x => x.Id == companyId).FirstOrDefault();
            if (data != null)
            {
                _companyData.Id = data.Id;
                txtCompanyCode.Text = data.Code;
                txtCompanyName.Text = data.Name;
                txtCompanyAddress.Text = data.Address;
                txtContactPerson.Text = data.ContactPerson;
                txtContactNumber.Text = data.ContactNo;
                txtEmail.Text = data.Email;
                txtWeb.Text = data.WebUrl;
                txtPan.Text = data.PAN;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCompanyCode.Text.Trim() == "")
            {
                MessageBox.Show("Company code is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCompanyCode.Focus();
                return;
            }
            else if (txtCompanyName.Text.Trim() == "")
            {
                MessageBox.Show("Company name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCompanyName.Focus();
                return;
            }
            else if (txtCompanyAddress.Text.Trim() == "")
            {
                MessageBox.Show("Company address is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCompanyAddress.Focus();
                return;
            }
            else if (txtContactPerson.Text.Trim() == "")
            {
                MessageBox.Show("Contact person is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactPerson.Focus();
                return;
            }
            else if (txtContactNumber.Text.Trim() == "")
            {
                MessageBox.Show("Contact number is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactNumber.Focus();
                return;
            }
            else if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Email address is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }
            SCompany companyServices = new SCompany();
            _companyData = companyServices.List().Data.Where(x => x.Id == _companyData.Id).FirstOrDefault();
            _companyData.Address = txtCompanyAddress.Text;
            _companyData.ContactNo = txtContactNumber.Text; ;
            _companyData.ContactPerson = txtContactPerson.Text;
            _companyData.Email = txtEmail.Text;
            _companyData.Name = txtCompanyName.Text;
            _companyData.PAN = txtPan.Text;
            _companyData.WebUrl = txtWeb.Text;
            var result = companyServices.Update(_companyData);
            if (result.Status == ResultStatus.Ok)
            {
                MessageBox.Show("Company profile update sucessfully.", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                CompanyProfile_Load(null, null);
            }

        }

        public void resetInputs()
        {
            txtCompanyName.Text = "";
            txtCompanyAddress.Text = "";
            txtContactPerson.Text = "";
            txtContactNumber.Text = "";
            txtEmail.Text = "";
            txtWeb.Text = "";
            txtPan.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetInputs();
        }

        private void CompanyProfile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
