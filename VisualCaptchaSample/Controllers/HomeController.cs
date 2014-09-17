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
            var session = new Captcha(new CaptchaSession()).Generate(numberOfImages);
            Session[SessionKey] = session;

            // Client side library requires lowercase property names
            return Json(new {
                values = session.FrontEndData.Values,
                imageName = session.FrontEndData.ImageName,
                imageFieldName = session.FrontEndData.ImageFieldName,
                audioFieldName = session.FrontEndData.AudioFieldName
            }, JsonRequestBehavior.AllowGet);
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

        public JsonResult Try(string value)
        {
            var session = (CaptchaSession)Session[SessionKey];
            var result = new Captcha(session).ValidateAttempt(value);

            return Json(new { success = result.Item1, message = result.Item2 });
        }
    }
}