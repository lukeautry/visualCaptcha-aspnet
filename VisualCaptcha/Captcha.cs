using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VisualCaptcha
{
    /// <summary>
    /// VisualCaptcha object
    /// </summary>
    [Serializable]
    public sealed class Captcha
    {
        public readonly Dictionary<string, string> PossibleImageOptions;
        public readonly KeyValuePair<string, string> ValidImageOption;
        public readonly KeyValuePair<string, string> ValidAudioOption;

        private readonly CryptoHelper _crypto = new CryptoHelper();

        public Captcha(int numberOfImageOptions)
        {
            PossibleImageOptions = GetRandomImageOptions(numberOfImageOptions);
            ValidImageOption = GetRandomOption(PossibleImageOptions);
            ValidAudioOption = GetRandomOption(Assets.Audios);
        }

        private Dictionary<string, string> GetRandomImageOptions(int numberOfOptions)
        {
            var randomOptions = new Dictionary<string, string>();
            var availableOptions = new Dictionary<string, string>(Assets.Images);

            for (var i = 0; i < numberOfOptions; i++)
            {
                var randomItem = availableOptions.ToList()[_crypto.GetRandomIndex(availableOptions.Count)];
                randomOptions.Add(randomItem.Key, _crypto.GetRandomString(20));

                availableOptions.Remove(randomItem.Key); // We don't want duplicate entries
            }

            return randomOptions;
        }

        private KeyValuePair<string, string> GetRandomOption(ICollection<KeyValuePair<string, string>> options)
        {
            return options.ToList()[_crypto.GetRandomIndex(options.Count)];
        }

        /// <summary>
        /// Retrieve data transfer object containing information needed by client-side library
        /// </summary>
        public FrontEndData GetFrontEndData()
        {
            return new FrontEndData
            {
                Values = PossibleImageOptions.Select(option => option.Value).ToList(),
                ImageName = ValidImageOption.Key,
                ImageFieldName = _crypto.GetRandomString(20),
                AudioFieldName = _crypto.GetRandomString(20)
            };
        }

        /// <summary>
        /// Get file content for image Captcha option
        /// </summary>
        /// <param name="index">Image index</param>
        /// <param name="isRetina">Uses Retina display</param>
        public byte[] GetImage(int index, bool isRetina)
        {
            var key = PossibleImageOptions.ToList()[index].Key;
            var imageName = Assets.Images[key];

            if (isRetina) { imageName = imageName.Replace(".png", "2x.png"); }

            return ReadResource("images." + imageName);
        }

        /// <summary>
        /// Get file content for audio Captcha option
        /// </summary>
        /// <param name="type">Either mp3 or ogg</param>
        public byte[] GetAudio(string type)
        {
            var audioName = ValidAudioOption.Key;
            if (type == "ogg") { audioName = audioName.Replace(".mp3", ".ogg"); }

            return ReadResource("audios." + audioName);
        }

        /// <summary>
        /// Returns success (boolean) and message response (string)
        /// </summary>
        /// <param name="answerValue">This could be the hashed value of the image path or the answer to an audio question</param>
        public Tuple<bool, string> ValidateAnswer(string answerValue)
        {
            bool success;
            string message;

            if (PossibleImageOptions.Any(i => i.Value == answerValue))
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

        private bool ValidateImage(string hashedPath)
        {
            return ValidImageOption.Value == hashedPath;
        }

        private bool ValidateAudio(string value)
        {
            return ValidAudioOption.Value.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <param name="optionPath"> e.g. image.mypicture.png </param>
        private static byte[] ReadResource(string optionPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceLocation = string.Format("VisualCaptcha.Assets.{0}", optionPath);
            using (var stream = assembly.GetManifestResourceStream(resourceLocation))
            {
                if (stream == null) { return null; }
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }
    }
}
