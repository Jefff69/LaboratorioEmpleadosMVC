
using System.Web;
using System.Web.Optimization;

namespace LaboratorioEmpleadosMVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Carga jQuery.
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-3.7.0.js"));

            // Carga jQuery UI para utilizar el diálogo.
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-1.14.1.js"));

            // Carga las validaciones de formularios.
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.js"));

            // Carga Modernizr.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            // Carga Bootstrap.
            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            // Estilos principales.
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            // Estilos de jQuery UI.
            bundles.Add(new StyleBundle("~/Content/jqueryui")
                .Include("~/Content/themes/base/jquery-ui.css"));
        }
    }
}
