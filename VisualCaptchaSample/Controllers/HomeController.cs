using System.Web.Mvc;
using VisualCaptcha;

namespace VisualCaptchaSample.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionKey = "visualcaptcha";

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Start(int numberOfImages)
        {
            var captcha = new Captcha(numberOfImages);
            Session[SessionKey] = captcha;

            var frontEndData = captcha.GetFrontEndData();

            // Client side library requires lowercase property names
            return Json(new {
                values = frontEndData.Values,
                imageName = frontEndData.ImageName,
                imageFieldName = frontEndData.ImageFieldName,
                audioFieldName = frontEndData.AudioFieldName
            }, JsonRequestBehavior.AllowGet);
        }

        public FileResult Image(int imageIndex, int retina = 0)
        {
            var captcha = (Captcha)Session[SessionKey];
            var content = captcha.GetImage(imageIndex, retina == 1);

            return File(content, "image/png");
        }

        public FileResult Audio(string type = "mp3")
        {
            var captcha = (Captcha)Session[SessionKey];
            var content = captcha.GetAudio(type);

            var contentType = type == "mp3" ? "audio/mpeg" : "audio/ogg";
            return File(content, contentType);
        }

        public JsonResult Try(string value)
        {
            var captcha = (Captcha)Session[SessionKey];
            var success = captcha.ValidateAnswer(value);
            var message = "Your answer was " + (success ? "valid." : "invalid.");

            return Json(new { success, message });
        }
    }
}