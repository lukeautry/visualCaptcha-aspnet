using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VisualCaptcha.assets;

namespace VisualCaptcha
{
    public sealed class Captcha
    {
        #region Fields

        private readonly CryptoHelper _crypto = new CryptoHelper();
        private readonly CaptchaSession _session;
        private readonly string _baseDirectory = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, "assets");

        #endregion

        #region Constructor

        public Captcha(CaptchaSession session = null)
        {
            _session = session ?? new CaptchaSession();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generate available captcha options
        /// </summary>
        /// <returns>Session data for future gets/posts</returns>
        public CaptchaSession Generate(int numberOfOptions = 5)
        {
            if (numberOfOptions < 2) { numberOfOptions = 5; } // Reset to default value if out of range 

            _session.Images = GetRandomOptions(Assets.Images, numberOfOptions);
            var imageValues = HashValues(_session.Images);
            _session.ValidImageOption = GetRandomOption(_session.Images);

            _session.Audios = GetRandomOptions(Assets.Audios, numberOfOptions);
            _session.ValidAudioOption = GetRandomOption(_session.Audios);

            _session.FrontEndData = new FrontEndData
            {
                values = imageValues,
                imageName = _session.ValidImageOption.Key,
                imageFieldName = _crypto.GetRandomString(20),
                audioFieldName = _crypto.GetRandomString(20)
            };

            return _session;
        }

        /// <summary>
        /// Get file content for image Captcha option
        /// </summary>
        /// <param name="index">Image index</param>
        /// <param name="isRetina">Uses Retina display</param>
        public FileStream GetImage(int index, bool isRetina)
        {
            var key = _session.Images.ToList()[index].Key;
            var imagePath = Path.Combine(_baseDirectory, "images", Assets.Images[key]);

            return File.OpenRead(imagePath);
        }

        /// <summary>
        /// Get file content for audio Captcha option
        /// </summary>
        /// <param name="type">Either mp3 or ogg</param>
        public FileStream GetAudio(string type)
        {
            var audioName = _session.ValidAudioOption.Key;
            if (type == "ogg") { audioName = audioName.Replace(".mp3", ".ogg"); }

            var audioPath = Path.Combine(_baseDirectory, "audios", audioName);
            return File.OpenRead(audioPath);
        }

        #endregion

        #region Private Methods

        private Dictionary<string, string> GetRandomOptions(IDictionary<string, string> options, int numberOfOptions)
        {
            var randomOptions = new Dictionary<string, string>();
            var availableOptions = new Dictionary<string, string>(options);

            for (var i = 0; i < numberOfOptions; i++)
            {
                var randomItem = availableOptions.ToList()[_crypto.GetRandomIndex(availableOptions.Count)];

                randomOptions.Add(randomItem.Key, randomItem.Value);
                availableOptions.Remove(randomItem.Key); // We don't want duplicate entries
            }

            return randomOptions;
        }

        private KeyValuePair<string, string> GetRandomOption(ICollection<KeyValuePair<string, string>> options)
        {
            return options.ToList()[_crypto.GetRandomIndex(options.Count)];
        }

        private List<string> HashValues(IDictionary<string, string> images)
        {
            var imageValues = new List<string>();

            // Set a random value for each of the images (used in front end)
            for(var i = 0; i < images.Count; i++)
            {
                var randomValue = _crypto.GetRandomString(20);
                imageValues.Add(randomValue);

                var option = images.ToList()[i];
                images[option.Key] = randomValue;
            }

            return imageValues;
        }

        #endregion

        public bool ValidateImage(string hashedPath)
        {
            return _session.ValidImageOption.Value == hashedPath;
        }
    }
}
