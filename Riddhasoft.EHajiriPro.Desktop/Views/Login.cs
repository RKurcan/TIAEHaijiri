using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using RTech.Lib.ProductKey;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Views
{
    public partial class Login : Form
    {
        EUser userData = null;
        SUser userServices = null;
        SCompany _companyServices = null;
        SFiscalYear _fiscalYearServices = null;
        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            txtCompanyCode.Text = Riddhasoft.EHajiriPro.Desktop.Properties.Settings.Default.CompanyCode;
            txtUsername.Text = Riddhasoft.EHajiriPro.Desktop.Properties.Settings.Default.UserName;
            txtPassword.Text = Riddhasoft.EHajiriPro.Desktop.Properties.Settings.Default.Password;
            if (!string.IsNullOrEmpty(txtCompanyCode.Text) && !string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text))
            {
                chkRememberMe.Checked = true;
                button1.Focus();
            }
            txtCompanyCode.Focus();
            userData = new EUser();
            userServices = new SUser();
            _companyServices = new SCompany();
            _fiscalYearServices = new SFiscalYear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int fiscalYearId = 0;

            GlobalParam.OperationDate = NepaliDateRbtn.Checked ? OperationDate.Nepali : OperationDate.English;

            if (chkRememberMe.Checked)
            {
                Properties.Settings.Default.CompanyCode = txtCompanyCode.Text;
                Properties.Settings.Default.UserName = txtUsername.Text;
                Properties.Settings.Default.Password = txtPassword.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.CompanyCode = "";
                Properties.Settings.Default.UserName = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.Save();
            }
            string CompanyCode = txtCompanyCode.Text;
            userData = userServices.List().Data.Where(x => x.Name == txtUsername.Text && x.Password == txtPassword.Text && x.UserType == UserType.User && x.Branch.Company.Code == txtCompanyCode.Text && x.IsDeleted == false).FirstOrDefault();
            if (userData != null)
            {
                var fiscalYear = _fiscalYearServices.List().Data.Where(x => x.BranchId == userData.BranchId && x.CurrentFiscalYear).FirstOrDefault();
                if (fiscalYear != null)
                {
                    fiscalYearId = fiscalYear.Id;
                }

                if (userData.Branch.Company.Code.ToUpper() != CompanyCode.ToUpper())
                {
                    errorLabel.Text = "Invalid Company Code";
                    return;
                }
                if (userData.Branch.Company.IsSuspended == true)
                {
                    errorLabel.Text = "Company Account is suspended please contact to vendor.";
                    return;
                }
                else
                {
                    RiddhaSession.BranchId = (int)userData.BranchId;
                    RiddhaSession.CompanyId = userData.Branch.CompanyId;
                    RiddhaSession.fiscalYearId = fiscalYearId;
                    RiddhaSession.UserId = userData.Id;
                    RiddhaSession.Language = "en";
                    RiddhaSession.OpreationDate = NepaliDateRbtn.Checked ? "ne" : "en";
                    RiddhaSession.CompanyName = userData.Branch.Company.Name;
                    RiddhaSession.CompanyContact = userData.Branch.Company.ContactNo;
                    RiddhaSession.CompanyAddress = userData.Branch.Company.Address;
                    RiddhaSession.UserName = userData.FullName;
                    
                    showDashboard();
                }
            }
            else
            {
                errorLabel.Text = "Invalid Login. Please Try Again.";
                return;
            }
        }

        private void showDashboard()
        {
            Home frm = new Home();
            this.Hide();
            frm.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            RegistryRiddha reg = new RegistryRiddha();
            string ProductKey = reg.GetKeyFromRegistry();
            RTech.Lib.ProductKey.RiddhaKey key = new RTech.Lib.ProductKey.RiddhaKey();
            key.validateProduct(ProductKey,"HrmplDesk");
            //key.validateProduct(ProductKey);
            //if (!key.validMac)

            //for khotang only
            //RegisterAuto(365);
            

            if (string.IsNullOrEmpty(ProductKey))
            {
                MessageBox.Show("Welcome To Hamro-hajiri. Please Enter Your Product Key...!!!");
                ProductKeyEntryForm frm = new ProductKeyEntryForm();
                frm.ShowDialog();
                return;
            }
            if (key.SecretKey != "HrmplDesk")
            {
                MessageBox.Show("This Product Is Not Registered In This Workstation...!!!!");
                ProductKeyEntryForm frm = new ProductKeyEntryForm();
                frm.ShowDialog();
                return;
            }
           
            if (!validateMac(key.MacAddress))
            {
                MessageBox.Show("This Product Is Not Registered In This Workstation...!!!!");
                ProductKeyEntryForm frm = new ProductKeyEntryForm();
                frm.ShowDialog();
                return;
            }
            if (System.DateTime.Now > key.ExpiryDate)
            {
                MessageBox.Show("Your period has been Expired. Please Contact to the Vendor.");
                ProductKeyEntryForm frm = new ProductKeyEntryForm();
                frm.ShowDialog();
                Application.Exit();
            }
            int remainingDays = (key.ExpiryDate - System.DateTime.Now).Days;
            if (remainingDays <= 7)
            {
                if (DialogResult.OK == (MessageBox.Show(string.Format("Your license will expire in {0} days. Please register this application.\n Click Ok to register, Cancel to continue", remainingDays), "License", MessageBoxButtons.OKCancel)))
                {
                    ProductKeyEntryForm frm = new ProductKeyEntryForm();
                    frm.ShowDialog();
                    return;
                }
            }
            txtCompanyCode.Text = Riddhasoft.EHajiriPro.Desktop.Properties.Settings.Default.CompanyCode;
            txtUsername.Text = Riddhasoft.EHajiriPro.Desktop.Properties.Settings.Default.UserName;
            txtPassword.Text = Riddhasoft.EHajiriPro.Desktop.Properties.Settings.Default.Password;
        }

        private bool validateMac(string p)
        {
            var macAddress = GetMacAddresses();
            return macAddress.Contains(p);
        }
        public List<string> GetMacAddresses()
        {

            return (from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where string.IsNullOrEmpty(nic.GetPhysicalAddress().ToString()) == false
                    select nic.GetPhysicalAddress().ToString()
                   ).ToList();



        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            if (panel1.BorderStyle == BorderStyle.FixedSingle)
            {
                int thickness = 1;//it's up to you
                int halfThickness = thickness / 2;
                using (Pen p = new Pen(Color.Black, thickness))
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                              halfThickness,
                                                              panel1.ClientSize.Width - thickness,
                                                              panel1.ClientSize.Height - thickness));
                }
            }
        }
       
        public void RegisterAuto(int PeriodInDays)
        {
            string macAddr = GetMacAddresses().FirstOrDefault();
            DateTime today=new DateTime(2019,12,27);

            RiddhaKey key = new RiddhaKey("HrmplDesk",today.AddDays(365), macAddr);
            string productKey = key.getProductKey();
            RegistryRiddha reg = new RegistryRiddha();
            reg.SetKeyToRegistry(productKey);
            reg.SetDateToRegistry();


          

        }
    }
}
