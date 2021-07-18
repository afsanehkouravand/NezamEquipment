using System.Web.Optimization;

namespace NezamEquipment.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            BundleTable.EnableOptimizations = true;

            #region Default

            bundles.Add(new ScriptBundle("~/bundles/jsDefault").IncludeDirectory(
                "~/Scripts/_Default", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/cssDefault").IncludeDirectory(
                "~/Content/_Default", "*.css"));

            #endregion

            #region Admin

            bundles.Add(new ScriptBundle("~/bundles/jsAdmin").IncludeDirectory(
                "~/Scripts/_Admin","*.js"));

            bundles.Add(new StyleBundle("~/bundles/cssAdmin").IncludeDirectory(
                "~/Content/_Admin","*.css"));

            #endregion

            #region Admin

            bundles.Add(new ScriptBundle("~/bundles/jsHome").IncludeDirectory(
                "~/Scripts/_Home", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/cssHome").IncludeDirectory(
                "~/Content/_Home", "*.css"));

            #endregion

        }
    }
}
