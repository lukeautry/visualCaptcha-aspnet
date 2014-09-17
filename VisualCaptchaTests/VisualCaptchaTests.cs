using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisualCaptcha;

namespace VisualCaptchaTests
{
    [TestClass]
    public class VisualCaptchaTests
    {
        [TestMethod]
        public void ValidOptions_Succeed()
        {
            for (var i = 0; i < 100; i++)
            {
                var captcha = new Captcha(new CaptchaSession());
                var session = captcha.Generate();

                var newCaptcha = new Captcha(session);
                Assert.IsTrue(newCaptcha.ValidateAttempt(session.ValidImageOption.Value).Item1);
            }
        }
    }
}
