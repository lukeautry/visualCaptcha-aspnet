using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisualCaptcha;

namespace VisualCaptchaTests
{
    [TestClass]
    public class VisualCaptchaTests
    {
        [TestMethod]
        public void ValidImageOption_Succeeds()
        {
            for (var i = 0; i < 100; i++)
            {
                var captcha = new Captcha(5);
                var data = captcha.GetFrontEndData();

                Assert.IsTrue(
                    captcha.ValidateAnswer(data.Values.FirstOrDefault(v => v == captcha.ValidImageOption.Value)).Item1);
            }
        }

        [TestMethod]
        public void InvalidImageOptions_Fail()
        {
            for (var i = 0; i < 100; i++)
            {
                var captcha = new Captcha(5);
                var data = captcha.GetFrontEndData();

                foreach (var option in data.Values.Where(option => option != captcha.ValidImageOption.Value))
                {
                    Assert.IsFalse(captcha.ValidateAnswer(option).Item1);
                }
            }
        }

        [TestMethod]
        public void GeneratedImageOptions_DuplicateFree()
        {
            for (var i = 0; i < 100; i++)
            {
                var captcha = new Captcha(10);
                var optionValues = new List<string>();
                foreach (var option in captcha.PossibleImageOptions)
                {
                    Assert.IsFalse(optionValues.Contains(option.Key), string.Format("Duplicate option found: {0}", option.Key));
                    optionValues.Add(option.Key);
                }
            }
        }

        [TestMethod]
        public void GetImage_StandardSize_NotNull()
        {
            GetImageSucceeds(false);
        }

        [TestMethod]
        public void GetImage_RetinaSize_NotNull()
        {
            GetImageSucceeds(true);
        }

        [TestMethod]
        public void GetAudioMp3_NotNull()
        {
            GetAudioSucceeds("mp3");
        }

        [TestMethod]
        public void GetAudioOgg_NotNull()
        {
            GetAudioSucceeds("ogg");
        }

        private static void GetAudioSucceeds(string audioType)
        {
            var captcha = new Captcha(5);
            var bytes = captcha.GetAudio(audioType);
            Assert.IsTrue(bytes != null && bytes.Length > 0);
        }

        private static void GetImageSucceeds(bool isRetina)
        {
            var captcha = new Captcha(10);
            var options = captcha.PossibleImageOptions.ToList();
            for (var i = 0; i < options.Count; i++)
            {
                var bytes = captcha.GetImage(i, isRetina);
                Assert.IsTrue(bytes != null && bytes.Length > 0);
            }
        }
    }
}
