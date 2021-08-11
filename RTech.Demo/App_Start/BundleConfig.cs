using System.Web;
using System.Web.Optimization;

namespace RTech.Demo
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/bundles/Device").Include(
                      "~/areas/device/scripts/riddha.script.*"));
            bundles.Add(new ScriptBundle("~/bundles/Employee").Include(
                      "~/Areas/Employee/Scripts/Riddha.Script.*"));
            bundles.Add(new ScriptBundle("~/bundles/Office").Include(
                      "~/Areas/Office/Scripts/Riddha.Script.*"));
            bundles.Add(new ScriptBundle("~/bundles/User").Include(
                      "~/Areas/User/Scripts/Riddha.Script.*"));
            bundles.Add(new ScriptBundle("~/bundles/Report").Include(
                      "~/Areas/Report/Scripts/Riddha.Script.*"));
            bundles.Add(new ScriptBundle("~/bundles/APP").Include(
                      "~/Scripts/App/Globals/Riddha.*",
                      "~/Scripts/App/Globals/koGrid.Paging.Ex.js"));

            bundles.Add(new ScriptBundle("~/bundles/Company").Include(
                     "~/Areas/Scripts/Riddha.Script.*"
                    ));



        }
    }
}
