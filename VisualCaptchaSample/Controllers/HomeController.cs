using System;
using System.Linq;
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
            var session = new Captcha().Generate(numberOfImages);
            Session[SessionKey] = session;
            return Json(session.FrontEndData, JsonRequestBehavior.AllowGet);
        }

        public FileResult Image(int imageIndex)
        {
            var session = (CaptchaSession)Session[SessionKey];
            var stream = new Captcha(session).GetImage(imageIndex, false);

            return File(stream, "image/png");
        }

        public FileResult Audio(string type = "mp3")
        {
            var session = (CaptchaSession)Session[SessionKey];
            var stream = new Captcha(session).GetAudio(type);

            var contentType = type == "mp3" ? "audio/mpeg" : "audio/ogg";
            return File(stream, contentType);
        }

        public JsonResult Try(string key, string value)
        {
            var result = ValidateAttempt(value);

            // Clear out session data on attempt - user gets one try only
            Session[SessionKey] = null;

            return Json(new { success = result.Item1, message = result.Item2 });
        }

        private Tuple<bool, string> ValidateAttempt(string value)
        {
            var session = (CaptchaSession) Session[SessionKey];
            var captcha = new Captcha(session);

            bool success;
            string message;

            if (session.Images.Any(i => i.Value == value))
            {
                success = captcha.ValidateImage(value);
                message = success ? "Image was valid." : "Image was invalid.";
            }
            else // Provided value doesn't exist in Images collection, check against Audios
            {
                success = captcha.ValidateAudio(value);
                message = success ? "Accessibility answer was valid." : "Invalid answer, please try again.";
            }

            return new Tuple<bool, string>(success, message);
        }
    }
}