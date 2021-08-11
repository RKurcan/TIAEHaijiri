using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using System.Configuration;


namespace Riddhasoft.EHajiriPro.Desktop.Common
{
    public class GlobalParam
    {
        //old public static Color darkColor = Color.FromArgb(41, 57, 85);
        public static Color darkColor = Color.FromArgb(2, 84, 119);
        public static Color lightColor = Color.FromArgb(24, 137, 186);
        //old public static Color lightColor = Color.FromArgb(155, 167, 183);
        public static int TableWidth = 500;
        public static Riddhasoft.User.Entity.EUser User { get; set; }
        public static string HelpXmlPath = @"HelpResource\";

        public static string Lang { get { return "en"; } }
        // public static string ComputerId = System.Configuration.ConfigurationManager.AppSettings["ComputerId"];
        public static OperationDate OperationDate { get; set; }
    }
    public enum OperationDate
    {
        English,
        Nepali
    }
    public static class NepaliDateExtension
    {
        public static string ToNepaliDate(this DateTime EnglishDate)
        {
            SDateTable service = new SDateTable();
            return service.ConvertToNepDate(EnglishDate);
        }
        public static DateTime ToEnglishDate(this string NepaliDate)
        {

            SDateTable service = new SDateTable();
            return service.ConvertToEngDate(NepaliDate);
        }
    }
    public static class ControlExtension
    {
        public static void ClearControl(this Control control)
        {
            control.Controls.Clear();
        }
        public static void BindDataToCombobox(this ComboBox combobox, object Datasource, string TextField = "Name", string ValueField = "Id")
        {

            combobox.ValueMember = ValueField;
            combobox.DisplayMember = TextField;
            combobox.DataSource = Datasource;

        }
    }
    public static class CheckboxListExtension
    {
        /// <summary>
        /// use to bind checkedListBox
        /// </summary>
        /// <param name="CheckedListBox">CheckedListBox item </param>
        /// <param name="data">data to be bind on , must be list of checkboxlistmodel </param>
        public static void BindData(this CheckedListBox CheckedListBox, List<CheckBoxListModel> data)
        {
            ((ListBox)CheckedListBox).DataSource = data;
            ((ListBox)CheckedListBox).DisplayMember = "Name";
            ((ListBox)CheckedListBox).ValueMember = "IsChecked";

            foreach (CheckBoxListModel item in data.Where(x => x.IsChecked))
            {
                CheckedListBox.SetItemChecked(data.IndexOf(item), true);
            }

        }
        /// <summary>
        /// use to check all checkbox in the listbox . 
        /// </summary>
        /// <param name="checkedListBox"></param>
        /// <param name="value">true , false</param>
        public static void CheckAll(this CheckedListBox checkedListBox, bool value)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, value);
            }

        }
        public delegate void DoThisForItem(CheckBoxListModel checkedListBoxObject);
        /// <summary>
        /// this will call function for all checked item
        /// </summary>
        /// <param name="checkedListBox"></param>
        /// <param name="DoThisForItem"> this delegate is called for selected listbox only</param>
        public static void DoThisForCheckedItem(this CheckedListBox checkedListBox, DoThisForItem DoThisForItem)
        {
            foreach (object item in checkedListBox.CheckedItems)
            {
                DoThisForItem(item as CheckBoxListModel);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkedListBox"></param>
        /// <returns>List of Checkedboxlistmodel</returns>
        public static List<CheckBoxListModel> GetCheckedItem(this CheckedListBox checkedListBox)
        {
            List<CheckBoxListModel> data = new List<CheckBoxListModel>();
            foreach (object item in checkedListBox.CheckedItems)
            {

                data.Add(item as CheckBoxListModel);

            }
            return data;

        }
    }

    public class RegistryRiddha
    {
        private bool toDb;
        public void TrialInitializer()
        {
            var date = System.DateTime.Now.ToString("dd/MM/yyyy");


        }
        public void TrialInitializer(bool ToDb)
        {
            var date = System.DateTime.Now.ToString("dd/MM/yyyy");
            this.toDb = toDb;

        }
        public void SetKeyToRegistry(string ProductKey)
        {
            Microsoft.Win32.RegistryKey test9999 = Registry.CurrentUser.CreateSubKey("HRMPLAttendance");
            RegistryKey testName = test9999.CreateSubKey("Setting");
            testName.SetValue("ProductKey", ProductKey);
        }
        public void SetDateToRegistry()
        {
            string hashedUser = System.DateTime.Now.AddDays(7).ToShortDateString();
            Microsoft.Win32.RegistryKey test9999 = Registry.CurrentUser.CreateSubKey("HRMPLAttendance");
            RegistryKey testName = test9999.CreateSubKey("Setting");
            // Create data for the TestSettings subkey.
            testName.SetValue("Date", hashedUser);
        }
        public string GetDateFromRegistry()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "HRMPLAttendance\\Setting";
            const string keyName = userRoot + "\\" + subkey;
            try
            {
                string value = Registry.GetValue(keyName, "Date", "").ToString();
                return value;
            }
            catch
            {
                return "";
            }
        }
        public string GetKeyFromRegistry()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "HRMPLAttendance\\Setting";
            const string keyName = userRoot + "\\" + subkey;
            try
            {
                string value = Registry.GetValue(keyName, "ProductKey", "").ToString();
                return value;
            }
            catch
            {
                return "";
            }
        }
    }
    public class CheckBoxListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }

    }
}
