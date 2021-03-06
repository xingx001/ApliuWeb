using ApliuTools;
using System.Web;
using System.Web.Optimization;

namespace ApliuWeb
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            try
            {
                bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery-{version}.js"));

                bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                            "~/Scripts/jquery-ui-{version}.js"));

                bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                            "~/Scripts/jquery.unobtrusive*",
                            "~/Scripts/jquery.validate*"));

                bundles.Add(new ScriptBundle("~/bundles/jsscript").Include("~/JsScript/apliualert.js", "~/JsScript/CommonJs.js"));

                // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
                // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
                bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

                bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
                bundles.Add(new StyleBundle("~/Content/CssStyle").Include("~/CssStyle/CommonStyle.css"));

                bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                            "~/Content/themes/base/jquery.ui.core.css",
                            "~/Content/themes/base/jquery.ui.resizable.css",
                            "~/Content/themes/base/jquery.ui.selectable.css",
                            "~/Content/themes/base/jquery.ui.accordion.css",
                            "~/Content/themes/base/jquery.ui.autocomplete.css",
                            "~/Content/themes/base/jquery.ui.button.css",
                            "~/Content/themes/base/jquery.ui.dialog.css",
                            "~/Content/themes/base/jquery.ui.slider.css",
                            "~/Content/themes/base/jquery.ui.tabs.css",
                            "~/Content/themes/base/jquery.ui.datepicker.css",
                            "~/Content/themes/base/jquery.ui.progressbar.css",
                            "~/Content/themes/base/jquery.ui.theme.css"));
                Logger.WriteLogWeb("加载默认js、css完成");
            }
            catch (System.Exception ex)
            {
                Logger.WriteLogWeb("加载默认js、css失败，详情：" + ex.Message);
            }
        }
    }
}