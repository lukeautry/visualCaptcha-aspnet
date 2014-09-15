using System.Web.Mvc;
using System.Web.Routing;

namespace VisualCaptchaSample
{
    public sealed class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Start", "Home/Start/{numberOfImages}", new { controller = "Home", action = "Start", numberOfImages = UrlParameter.Optional } );
            routes.MapRoute("Image", "Home/Image/{imageIndex}", new { controller = "Home", action = "Image", imageIndex = UrlParameter.Optional } );
            routes.MapRoute("Audio", "Home/Audio/{index}", new { controller = "Home", action = "Audio", index = UrlParameter.Optional } );
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional } );
        }
    }
}
