using Riddhasoft.EHajiriPro.Desktop.Migration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            //newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            newCulture.DateTimeFormat.TimeSeparator = ":";
            Thread.CurrentThread.CurrentCulture = newCulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DbMigration();
            Application.Run(new Views.Login());
            
            //Application.Run(new Views.Home());
        }
        private static void DbMigration()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}
