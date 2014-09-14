using System.Web.Mvc;
using System.Web.Routing;

namespace VisualCaptchaSample
{
    public sealed class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Start", "start/{numberOfImages}", new { controller = "Home", action = "Start", numberOfImages = UrlParameter.Optional } );
            routes.MapRoute("Image", "image/{imageIndex}", new { controller = "Home", action = "Image", imageIndex = UrlParameter.Optional } );
            routes.MapRoute("Audio", "audio/{index}", new { controller = "Home", action = "Audio", index = UrlParameter.Optional } );
            routes.MapRoute("Try", "try", new { controller = "Home", action = "Try" } );
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional } );
        }
    }
}
