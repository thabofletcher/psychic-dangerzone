using System.Web;
using System.Web.Optimization;

namespace psychic_dangerzone
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/shared").Include("~/Scripts/*.js"));

            bundles.Add(new StyleBundle("~/Content/shared").Include("~/Content/*.css"));
        }
    }
}