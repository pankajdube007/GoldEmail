using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace Goldmedal.Emails
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));





            bundles.Add(new ScriptBundle("~/bundles/bootstrap-dialog").Include(
                            "~/Scripts/bootstrap-dialog.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.smartmenus").Include(
                           "~/Scripts/jquery.smartmenus.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.smartmenus.bootstrap").Include(
                           "~/Scripts/jquery.smartmenus.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/dialog_box").Include(
                           "~/Scripts/dialog_box.js"));

            bundles.Add(new ScriptBundle("~/bundles/ListBoxFilter").Include(
                           "~/Scripts/ListBoxFilter.js"));

            bundles.Add(new ScriptBundle("~/bundles/BlockSpecialChars").Include(
                           "~/Scripts/BlockSpecialChars.js"));






            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                           "~/Scripts/modernizr-*"));

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });
        }
    }
}