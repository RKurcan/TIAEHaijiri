using RTech.Demo.Migrations;
using RTech.Demo.Utilities;
using System;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using static RTech.Demo.Utilities.WDMS;

namespace RTech.Demo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }
        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/api");
        }
        protected void Application_Start()
        {
            // testMethod();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
             DbMigration();
            configureCulture();
        }

        private void testMethod()
        {
            //var model = new Controllers.ADMS.AdmsAttendanceModel()
            //{
            //    BranchCode = "nphq",
            //    UserPin = "1",
            //    DeviceSn = "6209204400008",
            //    VerifyTime = System.DateTime.Now.ToString(),
            //    VerifyType = 1,
            //    Temperature = 0,


            //};

            //Riddhasoft.Attendance.Services.SAttendanceLog attendanceLogServices = new Riddhasoft.Attendance.Services.SAttendanceLog();
            //var result = attendanceLogServices.AddUsingSP(model.BranchCode, model.UserPin.ToInt(), model.VerifyType, model.VerifyTime.ToDateTime(), model.Temperature, model.DeviceSn);
        }

        private void wdmsAutoFunction()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = new TimeSpan(0, 15, 0).TotalMilliseconds;
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Utilities.WDMS.CopyDeviceInfo();
            Utilities.WDMS.CopyDeviceLog();
        }
        private static void DbMigration()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
        public static void configureCulture()
        {
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            newCulture.DateTimeFormat.DateSeparator = "/";
            newCulture.DateTimeFormat.TimeSeparator = ":";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the error details
            var lastErrorWrapper = Server.GetLastError();
            if (lastErrorWrapper != null)
                Log.SytemLog("Message=" + lastErrorWrapper.Message + ", Stack Trace= " + lastErrorWrapper.StackTrace);
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            configureCulture();
        }
    }
}
