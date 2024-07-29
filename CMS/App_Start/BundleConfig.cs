using System.Web;
using System.Web.Optimization;

namespace CMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/assets/plugins/jquery-ui/jquery-ui-1.10.2.custom.js"
                , "~/Scripts/jquery-ui.js"
                         
                         ));



            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
          "~/Scripts/bootstrap.js",
          "~/Scripts/respond.js"
          ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/jquery-ui.css",
                "~/Content/PagedList.css",
                "~/assets/css/rtl-version.css"
                //"~/Content/Menu/ContextMenu.css",
                //"~/Content/OrgChart/jquery.orgchart.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"
                        , "~/Scripts/jquery.unobtrusive*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/FrameWork").Include(
                "~/Scripts/FrameWorkScripts.js"
                ));

           /* bundles.Add(new ScriptBundle("~/bundles/OrgChart").Include(
              
               "~/Scripts/OrgChart/jquery.orgchart.js"
               ));*/

            //bundles.Add(new ScriptBundle("~/bundles/bootstraptreeview").Include(
            //    "~/Scripts/bootstrap-treeview.js"
            //    ));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include("~/assets/plugins/dropzone/downloads/dropzone.js"));
            bundles.Add(new ScriptBundle("~/content/dropzoneStyle").Include("~/assets/plugins/dropzone/downloads/css/dropzone.css"));

            bundles.Add(new ScriptBundle("~/bundles/Jcrop").Include("~/scripts/JCrop/scripts/jquery.Jcrop.js", "~/scripts/JCrop/scripts/jquery.SimpleCropper.js"));
            bundles.Add(new ScriptBundle("~/content/JcropStyle").Include("~/scripts/JCrop/css/style.css", "~/scripts/JCrop/css/jquery.Jcrop.css"));
        }
    }
}
