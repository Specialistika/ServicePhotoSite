using System.Web;
using System.Web.Optimization;

namespace BundleModule
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
                        //"~/Scripts/gridmvc.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        //"~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //"~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/progressbar").Include(
            //            "~/Scripts/progressbar.js"));
    
            //bundles.Add(new ScriptBundle("~/bundles/ImageDropdown").Include(
            //            "~/Scripts/ImageDropdown.js"));
    
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/grid-0.4.3.js",
                      "~/Scripts/DateFormat.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/UploadFile.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/grid-0.4.3.min.css",
                      //"~/Content/animate-bottom.css",
                      "~/Content/glyphicon-refresh-animate.css"
                      ));

            //bundles.Add(new ScriptBundle("~/bundles/angular").Include(
            //            "~/Scripts/angular.js",
            //            "~/Scripts/angular-*",
            //            //"~/Scripts/bower_components/*",
            //            "~/Scripts/bower_components/ng-flow/dist/ng-flow-standalone.js",
            //            "~/Scripts/bower_components/fusty-flow.js/src/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-*",
                        "~/Scripts/bower_components/ng-flow/dist/ng-flow-standalone.js",
                        "~/Scripts/bower_components/fusty-flow.js/src/*.js"));


            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/App/app.js",
                        "~/App/*.js",
                        "~/App/components/files.module.js",
                        "~/App/components/*.js"));
            BundleTable.EnableOptimizations = false;
        }
    }
}
