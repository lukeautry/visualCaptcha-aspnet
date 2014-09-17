using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VisualCaptcha
{
    /// <summary>
    /// VisualCaptcha object
    /// </summary>
    public sealed class Captcha
    {
        #region Fields

        private readonly CryptoHelper _crypto = new CryptoHelper();
        private readonly CaptchaSession _session;

        #endregion

        #region Constructor

        /// <param name="session">New or existing Captcha session</param>
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
            var imageValues = HashImagePaths(_session.Images);
            _session.ValidImageOption = GetRandomOption(_session.Images);

            _session.Audios = GetRandomOptions(Assets.Audios, numberOfOptions);
            _session.ValidAudioOption = GetRandomOption(_session.Audios);

            _session.FrontEndData = new FrontEndData
            {
                Values = imageValues,
                ImageName = _session.ValidImageOption.Key,
                ImageFieldName = _crypto.GetRandomString(20),
                AudioFieldName = _crypto.GetRandomString(20)
            };

            return _session;
        }

        /// <summary>
        /// Get file content for image Captcha option
        /// </summary>
        /// <param name="index">Image index</param>
        /// <param name="isRetina">Uses Retina display</param>
        public byte[] GetImage(int index, bool isRetina)
        {
            var key = _session.Images.ToList()[index].Key;
            var imageName = Assets.Images[key];

            if (isRetina) { imageName = imageName.Replace(".png", "2x.png"); }

            return ReadResource("images", imageName);
        }

        /// <summary>
        /// Get file content for audio Captcha option
        /// </summary>
        /// <param name="type">Either mp3 or ogg</param>
        public byte[] GetAudio(string type)
        {
            var audioName = _session.ValidAudioOption.Key;
            if (type == "ogg") { audioName = audioName.Replace(".mp3", ".ogg"); }

            return ReadResource("audios", audioName);
        }

        private static byte[] ReadResource(string resourceFolder, string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var streamPath = string.Format("VisualCaptcha.Assets.{0}.{1}", resourceFolder, resourceName);
            using (var stream = assembly.GetManifestResourceStream(streamPath))
            {
                if (stream == null) { return null;  }
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        /// <summary>
        /// Returns success (boolean) and message response (string)
        /// </summary>
        /// <param name="answerValue">This could be the hashed value of the image path or the answer to an audio question</param>
        public Tuple<bool, string> ValidateAttempt(string answerValue)
        {
            bool success;
            string message;

            if (_session.Images.Any(i => i.Value == answerValue))
            {
                success = ValidateImage(answerValue);
                message = success ? "Image selected was valid." : "Image selection was invalid.";
            }
            else // Provided value doesn't exist in Images collection, check against Audios
            {
                success = ValidateAudio(answerValue);
                message = success ? "Accessibility answer was valid." : "Invalid answer, please try again.";
            }

            return new Tuple<bool, string>(success, message);
        }

        #endregion

        #region Private Methods

        private Dictionary<string, string> GetRandomOptions(IDictionary<string, string> options, int numberOfOptions)
        {
            var randomOptions = new Dictionary<string, string>();
            var availableOptions = new Dictionary<string, string>(options); // No deep copy needed here since we're using value types

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

        /// <summary>
        /// Get a list of hashed image paths
        /// </summary>
        /// <param name="images">Dictionary of image name/path pairs</param>
        /// <returns>List of hashed strings (used in FrontEndData)</returns>
        private List<string> HashImagePaths(IDictionary<string, string> images)
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

        private bool ValidateImage(string hashedPath)
        {
            return _session.ValidImageOption.Value == hashedPath;
        }

        private bool ValidateAudio(string value)
        {
            return _session.ValidAudioOption.Value.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
