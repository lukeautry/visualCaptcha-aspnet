using System.Web.Mvc;
using VisualCaptcha;

namespace VisualCaptchaSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Start(int numberOfImages)
        {
            var session = new Captcha().Generate(numberOfImages);
            Session["visualcaptcha"] = session;
            return Json(session.FrontEndData, JsonRequestBehavior.AllowGet);
        }

        public FileResult Image(int imageIndex)
        {
            var session = (CaptchaSession)Session["visualcaptcha"];
            var stream = new Captcha(session).GetImage(imageIndex, false);

            return File(stream, "image/png");
        }

        public FileResult Audio(string type = "mp3")
        {
            var session = (CaptchaSession)Session["visualcaptcha"];
            var stream = new Captcha(session).GetAudio(type);

            var contentType = type == "mp3" ? "audio/mpeg" : "audio/ogg";
            return File(stream, contentType);
        }

        public JsonResult Try()
        {
            var session = (CaptchaSession)Session["visualcaptcha"];
            var captcha = new Captcha(session);

            var key = Request.Params.AllKeys[0];
            var value = Request.Params[key];

            return Json(captcha.ValidateImage(value));
        }
    }
}