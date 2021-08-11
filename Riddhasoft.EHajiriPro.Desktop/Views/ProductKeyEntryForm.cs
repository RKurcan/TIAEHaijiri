using Riddhasoft.EHajiriPro.Desktop.Common;
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
    public partial class ProductKeyEntryForm : Form
    {
        public ProductKeyEntryForm()
        {
            InitializeComponent();
        }

        private void btnValidateProductKey_Click(object sender, EventArgs e)
        {
            RTech.Lib.ProductKey.RiddhaKey key = new RTech.Lib.ProductKey.RiddhaKey();
            if (string.IsNullOrEmpty(txtProductKey.Text))
            {
                MessageBox.Show("Please Enter Your Product Key");
                return;
            }
            key.validateProduct(txtProductKey.Text, "HrmplDesk");

            if (!validateMac(key.MacAddress))
            {
                MessageBox.Show("This Product Is Not Registered In This Workstation...!!!!");
                return;
            }
            if (!(key.validSecrete))
            {
                MessageBox.Show("Invalid Product Key...!!!");
                return;
            }
            if (System.DateTime.Now > key.ExpiryDate)
            {
                MessageBox.Show("Invalid Product Key...!!!");
                ProductKeyEntryForm frm = new ProductKeyEntryForm();
                frm.ShowDialog();
                Application.Exit();
            }
            RegistryRiddha reg = new RegistryRiddha();
            reg.SetKeyToRegistry(txtProductKey.Text);

            MessageBox.Show(this.Owner, "Thank You For Choosing Hamro-Hajiri Attendance System.", "Hamro-Hajiri Attendance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();

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
        private void RequestProductKeyBtn_Click(object sender, EventArgs e)
        {
            ProductKeyRequestForm form = new ProductKeyRequestForm();
            form.ShowDialog();
            Application.Exit();
        }

        private void ProductKeyEntryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
