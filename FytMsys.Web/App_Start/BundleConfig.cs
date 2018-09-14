using System.Web;
using System.Web.Optimization;

namespace FytMsys.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //网页前端样式表
            bundles.Add(new StyleBundle("~/indexs/css").Include(
                "~/lib/css/bootstrap.min.css",
                        "~/lib/css/flexslider.css",
                        "~/assets/css/css3/animate.css",
                        "~/lib/css/animate.css",
                        "~/lib/css/index.css"));


            //manage -css
            bundles.Add(new StyleBundle("~/manage/css").Include(
                "~/assets/css/admin/select2.min.css",
                        "~/assets/css/admin/bootstrap.min.css",
                        "~/assets/css/css3/animate.css",
                        "~/assets/css/fonts.css",
                        "~/assets/css/icons.css",
                        "~/assets/css/admin/style.css",
                        "~/assets/css/admin/jquery.dataTables.css"));

            //manage -js
            bundles.Add(new ScriptBundle("~/manage/js").Include(
                        "~/assets/js/jquery.form.min.js",
                        "~/assets/js/jquery.unobtrusive-ajax.min.js",
                        "~/assets/js/admin/bootstrap.min.js",
                        "~/assets/js/admin/select2.js",
                        "~/assets/js/fyt-form.js",
                        "~/assets/js/data/laydate.dev.js"));

            //manage - table -js
            bundles.Add(new ScriptBundle("~/table/js").Include(
                        "~/assets/js/admin/jquery.tmpl.js",
                        "~/assets/js/admin/colResizable-1.5.min.js",
                        "~/assets/js/fyt-common.js"));

            //font -css
            bundles.Add(new StyleBundle("~/font/css").Include(
                        "~/assets/css/fonts.css",
                        "~/assets/css/icons.css"));

            //index -css
            bundles.Add(new StyleBundle("~/index/css").Include(
                        "~/assets/dialog/ui-dialog.css",
                        "~/assets/css/dpl-min.css",
                        "~/assets/css/bui-min.css",
                        "~/assets/css/jquery.gritter.css",
                        "~/assets/css/main-min.css"));
            //index -js
            bundles.Add(new ScriptBundle("~/index/js").Include(
                        "~/assets/dialog/dialog-plus-min.js",
                        "~/assets/js/admin/jquery.gritter.min.js",
                        "~/assets/js/jpreloader/jpreLoader.min.js",
                        "~/assets/js/fyt-common.js"));

            //login -js
            bundles.Add(new ScriptBundle("~/login/js").Include(
                        "~/assets/js/bootstrap/bootstrap.min.js",
                        "~/assets/js/bootstrap/bootstrap-progressbar.js",
                        "~/assets/js/jquery.unobtrusive-ajax.min.js",
                        "~/assets/js/fyt-form.js",
                        "~/assets/js/fyt.login.js"));
            //login -css
            bundles.Add(new StyleBundle("~/login/css").Include(
                        "~/assets/css/bootstrap.min.css",
                        "~/assets/css/fonts.css",
                        "~/assets/css/icons.css",
                        "~/assets/css/login.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
