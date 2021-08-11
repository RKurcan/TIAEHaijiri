using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Views
{
    public partial class ProductKeyRequestForm : Form
    {
        public ProductKeyRequestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            #region validation
            if (CompanyNameTxt.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Company Name");
                return;
            }
            if (AddressTxt.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Company Address");
                return;
            }
            if (AuthorizedMailtxt.Text.Trim() == "")
            {

                MessageBox.Show("Please Enter Company Email");
                return;
            }
            if (contactNoTxt.Text.Trim() == "")
            {

                MessageBox.Show("Please Enter Company Contact No");
                return;
            }


            #endregion


            RTech.Lib.ProductKey.RiddhaKey key = new RTech.Lib.ProductKey.RiddhaKey();
            var macAddr = key.GetMacAddress();
            var tomail = "hrmplriddhasoft@gmail.com";
            MailCommon mail = new MailCommon();
            MailAddress from = new MailAddress("hrmplriddhasoft@gmail.com");
            NetworkCredential cred = new NetworkCredential("hrmplriddhasoft@gmail.com", "riddhasoft");
            MailAddress to = new MailAddress(tomail);
            var mailMsg = mail.CreateMessage(from, to, string.Format("Product key Request from {0}", CompanyNameTxt.Text),
            string.Format("Company Name :  {0} , Address : {1}, Email : {2}, Contact No : {3},Mac Address : {4},"
                           ,
                            CompanyNameTxt.Text,
                            AddressTxt.Text,
                            AuthorizedMailtxt.Text,
                            contactNoTxt.Text,

                            macAddr));
            mailMsg.Subject = string.Format("Product key Request From {0}", CompanyNameTxt.Text);
            mail.SendEmail(mailMsg, cred);
            mailMsg.Dispose();

            MessageBox.Show("Your Request Has Been Sent. We will get back to you soon...");
            this.Close();
        }

        private void ProductKeyRequestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ProductKeyRequestForm_Load(object sender, EventArgs e)
        {
            var list = GetMacAddresses();
            txtMac.Text = list.First();
        }
        public List<string> GetMacAddresses()
        {

            return (from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where string.IsNullOrEmpty(nic.GetPhysicalAddress().ToString()) == false
                    select nic.GetPhysicalAddress().ToString()
                   ).ToList();



        }

        private void ProductKeyRequestForm_Load_1(object sender, EventArgs e)
        {
            var list = GetMacAddresses();
            txtMac.Text = list.First();
        }
    }
}
