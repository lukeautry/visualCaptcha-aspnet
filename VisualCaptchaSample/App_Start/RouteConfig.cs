using System.Web.Mvc;
using System.Web.Routing;

namespace VisualCaptchaSample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Start",
                url: "start/{numberOfImages}",
                defaults: new { controller = "Home", action = "Start", numberOfImages = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Image",
                url: "image/{imageIndex}",
                defaults: new { controller = "Home", action = "Image", imageIndex = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Audio",
                url: "audio/{index}",
                defaults: new { controller = "Home", action = "Audio", index = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Try",
                url: "try",
                defaults: new { controller = "Home", action = "Try" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
