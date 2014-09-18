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

            var clientSideDataObject = captcha.GetFrontEndData();

            // Client side library requires lowercase property names
            return Json(new {
                values = clientSideDataObject.Values,
                imageName = clientSideDataObject.ImageName,
                imageFieldName = clientSideDataObject.ImageFieldName,
                audioFieldName = clientSideDataObject.AudioFieldName
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
            var result = captcha.ValidateAnswer(value);

            return Json(new { success = result.Item1, message = result.Item2 });
        }
    }
}