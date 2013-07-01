namespace MvcTables.Samples
{
    #region

    using System.Web.Optimization;

    #endregion

    public class BundleConfig
    {
        public const string BundlesJquery = "~/bundles/jquery";
        public const string BundlesBootstrap = "~/bundles/bootstrap";
        public const string VirtualPath = "~/Content/css";
        public const string ContentBootstrap = "~/Content/bootstrap";
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(BundlesJquery).Include(
                                                                "~/Scripts/jquery-{version}.js"));


            bundles.Add(new ScriptBundle(BundlesBootstrap).Include(
                                                                   "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle(ContentBootstrap).Include(
                                                                  "~/Content/bootstrap.css",
                                                                  "~/Content/bootstrap-responsive.css"));
        }
    }
}