using System.Web;
using System.Web.Optimization;

namespace Desktop
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                       "~/js/bootstrap.min.js",
                       "~/js/jquery-ui.min.js",
                       "~/js/plugins.js",
                       "~/js/functions.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/css/bootstrap.min.css",
                       "~/css/main.css",
                        "~/css/style.css",
                       "~/css/colors.css",
                        "~/css/responsive.css",
                       "~/css/jquery-ui.min.css",
                        "~/css/weather-icons.min.css",
                        "~/css/font-awesome.min.css"));
        }
    }
}
