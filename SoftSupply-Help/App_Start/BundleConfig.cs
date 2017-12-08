using System.Web;
using System.Web.Optimization;

namespace SoftSupply.Help
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Styles

            IItemTransform cssTransform = new CssRewriteUrlTransformWrapper();

            bundles.Add(new StyleBundle("~/styles/bootstrap")
                .Include("~/components/bootstrap/dist/css/bootstrap.css")
                .Include("~/components/jasny-bootstrap/dist/css/jasny-bootstrap.css")
                .Include("~/content/css/bootstrap-11292.css"));

            bundles.Add(new StyleBundle("~/styles/font-awesome").Include(
                "~/components/font-awesome/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/styles/lightbox").Include(
                "~/components/lightbox2/dist/css/lightbox.css"));

            bundles.Add(new StyleBundle("~/styles/animate").Include(
                "~/components/animate.css/animate.css"));

            bundles.Add(new StyleBundle("~/styles/default")
                .Include("~/content/css/default.css")
                .Include("~/content/css/default.cd-top.css"));

            #endregion

            #region Scripts 

            bundles.Add(new ScriptBundle("~/scripts/jquery")
                .Include("~/components/jquery/dist/jquery.js"));

            bundles.Add(new ScriptBundle("~/scripts/jqueryval")
                .Include("~/components/jquery.validation/dist/jquery.validate.js",
                 "~/components/jquery.validation/dist/additional-methods.js",
                 "~/components/Microsoft.jQuery.Unobtrusive.Validation/jquery.validate.unobtrusive.js"
                 ));

            bundles.Add(new ScriptBundle("~/scripts/html5shiv")
                .Include("~/components/html5shiv/dist/html5shiv.js",
                "~/components/respond/dist/respond.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/scripts/modernizr")
                .Include("~/components/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/scripts/bootstrap")
                .Include("~/components/bootstrap/dist/js/bootstrap.js")
                .Include("~/components/jasny-bootstrap/dist/js/jasny-bootstrap.js"));
            
            bundles.Add(new ScriptBundle("~/scripts/lightbox")
                .Include("~/components/lightbox2/dist/js/lightbox.js"));

            bundles.Add(new ScriptBundle("~/scripts/holder")
                .Include("~/components/holderjs/holder.js"));

            bundles.Add(new ScriptBundle("~/scripts/default")
                .Include("~/content/js/default.js", "~/content/js/default.cd-top.js"));

            #endregion

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

        }
    }
}

namespace System.Web.Optimization
{
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}
